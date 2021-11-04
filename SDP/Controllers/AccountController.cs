using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SDP.ViewModels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.SDP.SDPCore.Models;
using Microsoft.SDP.SDPInfrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.SDP.SDPCore.Interface;
using Microsoft.SDP.SDPCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.SDP.SDPCore.Models.DbContexts;
using SDPCore.Models.AccountModel;
using System.Linq;

namespace SDP.Controllers
{
    //everything to do with Accounts, action names kinda speaks for itself
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> _userManager { get; }
        private SignInManager<IdentityUser> _signInManager { get; }
        private readonly RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration { get; }
        private IEmailSender _emailSender { get; }
        private string _APIkey; //this is the sendgrid api key grabbed from appsetting.json file, see below constructor
        private IDbRepo _dbRepo;
        private readonly IDbContextFactory<Microsoft.SDP.SDPCore.Models.DbContexts.SDPDbContext> _contextFactory;
        private SDPDbContext _db;


        public AccountController(UserManager<IdentityUser> um, SignInManager<IdentityUser> sm,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration, IEmailSender emailSender, IDbRepo dbRepo, IDbContextFactory<Microsoft.SDP.SDPCore.Models.DbContexts.SDPDbContext> contextFactory, SDPDbContext db)
        {
            this._userManager = um;
            this._signInManager = sm;
            this._roleManager = roleManager;
            this._configuration = configuration;
            this._emailSender = emailSender;
            this._APIkey = _configuration.GetValue<string>("APIKeys:SDP-SENDGRID-API");
            this._dbRepo = dbRepo;
            this._contextFactory = contextFactory;
            this._db = db;
        }


        //====================user to Login==========================
        public IActionResult Login(string errorMsg)
        {

            ViewData["Error"] = errorMsg;
            return View();
        }

        //====================user to Login==========================
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView LM)
        {

            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(LM.Email, LM.Password, LM.RememberMe, false);
                if (result.Succeeded)
                {
                    try
                    {

                        var user =await _userManager.FindByNameAsync(LM.Email);
                        var userTask =  _userManager.FindByNameAsync(LM.Email);
                        using (var context = _contextFactory.CreateDbContext())  //check if the use is Active
                        {
                            var setting = context.userSetting.Include(m => m.user).ToList().Where(m => m.user.Id.ToString() == user.Id).FirstOrDefault();
                            if (!setting.isActive)
                            {
                                return RedirectToAction("Login", new { errorMsg = "Invalid Login infomation" });
                            }
                        }
                       
                        RegisteredCustomer rc = new RegisteredCustomer(_dbRepo) { Id = userTask.Result.Id, UserName = userTask.Result.UserName, Email = userTask.Result.Email };

                        rc.cart = HttpContext.Session.GetString("Id") != null ? ViewService.getCustomerFromList(HttpContext.Session.GetString("Id")).cart : new Cart();//transfers the GuestCustomers Cart to registered customer

                        HttpContext.Session.SetString("Id", userTask.Result.Id.ToString());

                        GlobalVar.customerList.Add(rc);

                        HttpContext.Session.SetString("LoggedIN", "true");
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Error", "Home");
                    }
                }
                else
                {
                    return RedirectToAction("Login", new { errorMsg = "Invalid Login infomation" });
                }
            }
            return View();
        }

        //====================user to Signup==========================
        public IActionResult Signup()
        {
            return View(new SignupView());
        }
        //====================user to Signup==========================
        //using send grid API please read "https://app.sendgrid.com/guide/integrate/langs/csharp" to under stand the code
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(SignupView user)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var newUser = new IdentityUser { UserName = user.Email, Email = user.Email };
                    var userSetting = new UserSetting(null, user.opIn, true);
                    var result = await _userManager.CreateAsync(newUser, user.Password);

                    userSetting.user = await _userManager.FindByIdAsync(newUser.Id);
                    _db.userSetting.Add(userSetting);
                    await _db.SaveChangesAsync();

                    if (result.Succeeded)//this part sends the confrimation email
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                        var confirmationLink = Url.Action("ConfirmEmail",
                            "Account", new { userId = newUser.Id, token = token }, Request.Scheme);

                        if (await _emailSender.sendConfirmationEmailAsyncAsync(user.Email, newUser, confirmationLink))//below code add role to new users
                        {
                            var role = await _roleManager.FindByNameAsync("Customer");
                            if (role == null || newUser == null)
                            {
                                return RedirectToAction("Error", "Home");
                            }

                            if (!(await _userManager.IsInRoleAsync(newUser, role.Name)))
                            {
                                var roleResult = await _userManager.AddToRoleAsync(newUser, role.Name);
                            }
                            return View(new SignupView()); // place holder for check email page
                        }

                        return RedirectToAction("Index", "Home");
                    }
                    foreach (var e in result.Errors)
                    {
                        ModelState.AddModelError("", e.Description);
                    }
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", "Home");
                }
            }
            return View(user);
        }

        //====================user to Confrim Email==========================
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                if (userId == null || token == null) return View("Signup");
                var user = await _userManager.FindByIdAsync(userId);
                if (user == null)
                {
                    ViewBag.ErrorMessage = userId + "invalid Id";
                    return RedirectToAction("Error", "Home");
                }
                var result = await _userManager.ConfirmEmailAsync(user, token);

                if (result.Succeeded)
                {
                    return View();
                }

            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

            ViewBag.ErrorMessage = userId + "Email not confrimed ";
            return RedirectToAction("Error", "Home");
        }


        //==============User Logout================
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                ViewService.DeleteCustomerFromList(HttpContext.Session.GetString("Id")); //deletes the user from temp access
                await _signInManager.SignOutAsync();
                HttpContext.Session.SetString("LoggedIN", "false");
                GuestCustomer gc = new GuestCustomer(_dbRepo);
                GlobalVar.customerList.Add(gc);//register new guest customer
                HttpContext.Session.SetString("Id", gc.Id.ToString());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Login");
        }

        //===================cms that manages roles=======================
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult RoleManage(string msg)
        {
            ViewData["Error MSG"] = msg;
            return View(new ManageRoleModel());
        }


        //===================cms that manages roles=======================
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RoleManage(string Email, string Id, ManageRoleModel MRM, string searchToken = "false")
        {
            IdentityUser user = new IdentityUser();
            ManageRoleModel manageRoleModel = new ManageRoleModel();
            var _user = await _userManager.FindByIdAsync(Id);


            //update Role of user
            if (searchToken == "true")
            {
                var role = await _roleManager.FindByIdAsync(MRM.roleId);
                var userForUpdate = await _userManager.FindByIdAsync(Id);

                if (role == null || userForUpdate == null)
                {
                    return RedirectToAction("Error", "Home");
                }

                if (!(await _userManager.IsInRoleAsync(userForUpdate, role.Name)))
                {
                    var result = await _userManager.AddToRoleAsync(userForUpdate, role.Name);
                }
                return View(new ManageRoleModel());

            }


            //get user by email
            if (Email != null)
            {
                user = await _userManager.FindByEmailAsync(Email);

                if (user == null) return View(new ManageRoleModel());//return empty view if user not found
            }
            var roles = _roleManager.Roles;

            //populate role list
            manageRoleModel.roleList = new List<SelectListItem>();
            manageRoleModel.currentRoleList = new List<IdentityRole>();
            foreach (var role in roles)
            {
                //excluding super admin
                if (role.Name != "SuperAdmin")
                {
                    manageRoleModel.roleList.Add(new SelectListItem { Text = role.Name, Value = role.Id });
                }

                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    manageRoleModel.currentRoleList.Add(role);
                }
            }

            manageRoleModel.user = user;

            return View(manageRoleModel);
        }

        //==============user to manage their account================

        [Authorize]
        public async Task<IActionResult> MyAccount(string msg)
        {
            var _user = await _userManager.FindByIdAsync(HttpContext.Session.GetString("Id"));

            ViewData["Error MSG"] = msg;
            return View(_user);
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> MyAccount(string userId, string password)
        {
            IdentityUser _user;
            try
            {
                _user = await _userManager.FindByIdAsync(HttpContext.Session.GetString("Id"));
                if (password != "" && await _userManager.CheckPasswordAsync(_user, password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(_user);
                    return RedirectToAction("ActualChangePassword", new { Email = _user.Email, token = token });
                }
                ViewData["Error MSG"] = "password incorrect"; //error msg 
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(_user);
        }

        //==============change password================
        public IActionResult PasswordChange()
        {
            return View();
        }

        //==============change password================
        [HttpPost]
        public async Task<IActionResult> PasswordChange(string Email)
        {

            var result = await _userManager.FindByEmailAsync(Email);
            if (result != null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(result);
                var changepasswordLink = Url.Action("ActualChangePassword",
                    "Account", new { Email = Email, token = token }, Request.Scheme);
                var response = await _emailSender.sendPasswordChangeEmail(Email, result, changepasswordLink);
            }
            else
            {
                ViewData["Msg"] = "user does not exist";
                return View();
            }
            ViewData["Msg"] = "Please check your email inbox and spam folder";
            return View();
        }


        //==============Page for actual password change================
        public IActionResult ActualChangePassword(string Email, string token)
        {
            return View(new PasswordChange { Email = Email, token = token });
        }
        [HttpPost]
        public async Task<IActionResult> ActualChangePassword(string Email, string Password, string token, PasswordChange pc)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(Email);
                var result = await _userManager.ResetPasswordAsync(user, token, Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Login");
                }
                ViewData["Msg"] = "Password not changed try again please";

            }
            return View(pc);
        }

        //==============Admin Delete  User  Role================
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteUserRole(string userId, string roleName)
        {
            try
            {
                var userForRoleDelete = await _userManager.FindByIdAsync(userId);

                var result = await _userManager.RemoveFromRoleAsync(userForRoleDelete, roleName.Trim());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }

            return RedirectToAction("RoleManage");
        }

        //============== delete User account================
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            try
            {
                var userForDelete = await _userManager.FindByIdAsync(userId);
                if (!(await _userManager.IsInRoleAsync(userForDelete, "SuperAdmin")))
                {
                    var result = await _userManager.DeleteAsync(userForDelete);
                }
                else return RedirectToAction("RoleManage", new { msg = "Cannot delete a Super Admin" });
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("RoleManage");
        }

        //==============User delete self account================
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteMyAccount(string Id)
        {
            var _user = await _userManager.FindByIdAsync(Id);

            if (!(await _userManager.IsInRoleAsync(_user, "SuperAdmin")))
            {
                try
                {
                    await _signInManager.SignOutAsync();
                    HttpContext.Session.SetString("LoggedIN", "false");
                    GuestCustomer gc = new GuestCustomer(_dbRepo);
                    GlobalVar.customerList.Add(gc);//register new guest customer

                    HttpContext.Session.SetString("Id", gc.Id.ToString());
                    //var user = await _userManager.FindByIdAsync(Id);
                    using (var context = _contextFactory.CreateDbContext()) 
                    {
                        var setting = context.userSetting.Include(m => m.user).ToList().Where(m => m.user.Id.ToString() == Id).FirstOrDefault();
                       
                        if (setting != null)
                        {
                            setting.isActive = false;
                            context.userSetting.Update(setting);
                            await context.SaveChangesAsync();
                        }
                      
                    }

                    return RedirectToAction("Login");
                }
                catch (Exception ex)
                {
                    return RedirectToAction("Error", "Home");
                }
            }

            else return RedirectToAction("MyAccount", new { msg = "Cannot delete a Super Admin" });
        }
    }
}
