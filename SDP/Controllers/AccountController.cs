using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SDP.Models.AccountModel;
using SDP.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using SDP.Models;
using SDP.Interfaces;
using SDP.Services;
using SDP.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SDP.Controllers
{
    //everything to do with Accounts, action names kinda speaks for itself
    public class AccountController : Controller
    {
        private UserManager<IdentityUser> _userManager { get; }
        private SignInManager<IdentityUser> _signInManager { get; }
        private readonly RoleManager<IdentityRole> _roleManager;
        private IConfiguration _configuration { get; }
        private string _APIkey; //this is the sendgrid api key grabbed from appsetting.json file, see below constructor

        public AccountController(UserManager<IdentityUser> um, SignInManager<IdentityUser> sm,
            RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this._userManager = um;
            this._signInManager = sm;
            this._roleManager = roleManager;
            this._configuration = configuration;
            this._APIkey = _configuration.GetValue<string>("APIKeys:SDP-SENDGRID-API");
        }


        //====================user to Login==========================
        public IActionResult Login()
        {
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
                        var user = _userManager.FindByNameAsync(LM.Email);
                        RegisteredCustomer rc = new RegisteredCustomer { userId = Guid.Parse(user.Result.Id), UserName = user.Result.UserName, Email = user.Result.Email, };
                        if (HttpContext.Session.GetString("Id") != null) //transfers the GuestCustomers Cart to registered customer
                        {
                            rc.cart = ViewService.getCustomerFromList(HttpContext.Session.GetString("Id")).cart;
                        }
                        else rc.cart = new Cart();


                        HttpContext.Session.SetString("Id", user.Result.Id.ToString());

                        Global.customerList.Add(rc);

                        HttpContext.Session.SetString("LoggedIN", "true");
                        return RedirectToAction("Index", "Home");
                    }
                    catch (Exception ex)
                    {
                        return RedirectToAction("Error", "Home");
                    }
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
                var newUser = new IdentityUser { UserName = user.Email, Email = user.Email };
                var result = await _userManager.CreateAsync(newUser, user.Password);
                if (result.Succeeded)//this part sends the confrimation email
                {
                    var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
                    var confirmationLink = Url.Action("ConfrimEmail",
                        "Account", new { userId = newUser.Id, token = token }, Request.Scheme);
                    var client = new SendGridClient(_APIkey);

                    var from = new EmailAddress("sdp.utils@gmail.com", "SDPAdmin");
                    var subject = "Confirm your SDP email";
                    var to = new EmailAddress(user.Email, "Dear Customer");
                    var plainTextContent = "please confirm email: ";
                    var htmlContent = "<a href=" + confirmationLink + "> click here to confirm email </a> <br>" + " <strong>Regards from the SDP team</strong>";
                    var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
                    var response = await client.SendEmailAsync(msg);

                    if (response.IsSuccessStatusCode)
                    {
                        var role = await _roleManager.FindByNameAsync("Customer");
                        if (role == null || newUser== null)
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
            return View(user);
        }

        //====================user to Confrim Email==========================
        public async Task<IActionResult> ConfrimEmail(string userId, string token)
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

            ViewBag.ErrorMessage = userId + "Email not confrimed ";
            return RedirectToAction("Error", "Home");
        }


        //==============User Logout================
        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                HttpContext.Session.SetString("LoggedIN", "false");
                GuestCustomer gc = new GuestCustomer();
                Global.customerList.Add(gc);//register new guest customer
              
                HttpContext.Session.SetString("Id", gc.userId.ToString());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Login");
        }

        //===================cms that manages roles=======================
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult RoleManage()
        {

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
                manageRoleModel.roleList.Add(new SelectListItem { Text = role.Name, Value = role.Id });
                if (await _userManager.IsInRoleAsync(user, role.Name))
                {
                    manageRoleModel.currentRoleList.Add(role);
                }
            }

            manageRoleModel.user = user;
           
            return View(manageRoleModel);
        }

        //==============user to manage their account================
        //[Authorize]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyAccount(string msg) 
        {
            var _user = await _userManager.FindByIdAsync(HttpContext.Session.GetString("Id"));

            ViewData["Error MSG"] = msg;
            return View(_user);
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


        //==============User delete self account================
        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> DeleteMyAccount(string Id)
        {
            var _user = await _userManager.FindByIdAsync(Id);

            if (!(await _userManager.IsInRoleAsync(_user, "SuperAdmin")))
            {
                try
                {
                    await _signInManager.SignOutAsync();
                    HttpContext.Session.SetString("LoggedIN", "false");
                    GuestCustomer gc = new GuestCustomer();
                    Global.customerList.Add(gc);//register new guest customer

                    HttpContext.Session.SetString("Id", gc.userId.ToString());
                    var result = await _userManager.DeleteAsync(await _userManager.FindByIdAsync(Id));
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
