﻿using Microsoft.AspNetCore.Http;
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
using SDPWeb.ViewModels;

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
            // Display Error page
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
                // get sign in details from the user input
                var result = await _signInManager.PasswordSignInAsync(LM.Email, LM.Password, false, false);

                // if the SignInResult fails, return invalid login details
                if (result.Succeeded)
                {
                    try
                    {
                        var user =await _userManager.FindByNameAsync(LM.Email);
                        var userTask =  _userManager.FindByNameAsync(LM.Email);

                        using (var context = _contextFactory.CreateDbContext())  
                        {
                            var setting = context.userSettings.Include(m => m.user).ToList().Where(m => m.user.Id.ToString() == user.Id).FirstOrDefault();

                            // check if the user is active. Return invalid login credentials if user is inactive
                            if (!setting.isActive)
                            {
                                return RedirectToAction("Login", new { errorMsg = "Invalid Login infomation" });
                            }
                        }
                       
                        // Once login details have passed, instantiate the Registered Customer instance
                        RegisteredCustomer rc = new RegisteredCustomer(_dbRepo) { Id = userTask.Result.Id, UserName = userTask.Result.UserName, Email = userTask.Result.Email };

                        // Transfer the Guest Customer's existing cart to the Registered Customer
                        rc.cart = HttpContext.Session.GetString("Id") != null ? ViewService.getCustomerFromList(HttpContext.Session.GetString("Id")).cart : new Cart();

                        // Pass the User ID into Session Data
                        HttpContext.Session.SetString("Id", userTask.Result.Id.ToString());

                        // Add customer to Global List of customers
                        GlobalVar.customerList.Add(rc);

                        // Set session variables
                        HttpContext.Session.SetString("LoggedIN", "true");
                        HttpContext.Session.SetString("Email", LM.Email);

                        // Return to Home Page, Logged In
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
                    // Create a new User
                    var newUser = new IdentityUser { UserName = user.Email, Email = user.Email };
                    var userSetting = new UserSettings(null, user.opIn, true);
                    var result = await _userManager.CreateAsync(newUser, user.Password);

                    // Add the new User to the database
                    userSetting.user = await _userManager.FindByIdAsync(newUser.Id);
                    _db.userSettings.Add(userSetting);
                    await _db.SaveChangesAsync();

                    // If the user is created successfully, begin sending the confirmation email
                    if (result.Succeeded)
                    {
                        // Generate a confirmation token to the placed in the email
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);

                        // Generate a confirmation link and pass the user and token in
                        var confirmationLink = Url.Action("ConfirmEmailNotif",
                            "Account", new { userId = newUser.Id, token = token }, Request.Scheme);

                        // Below code add role to new users
                        if (await _emailSender.sendConfirmationEmailAsyncAsync(user.Email, newUser, confirmationLink))
                        {
                            var role = await _roleManager.FindByNameAsync("Customer");

                            if (role == null || newUser == null)
                            {
                                return RedirectToAction("Error", "Account");
                            }

                            if (!(await _userManager.IsInRoleAsync(newUser, role.Name)))
                            {
                                var roleResult = await _userManager.AddToRoleAsync(newUser, role.Name);
                            }

                            return RedirectToAction("ConfirmEmailMsg", "Account"); 
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
                    return RedirectToAction("Error", "Account");
                }
            }
            return View(user);
        }

        //====================user to Confrim Email==========================
        public async Task<IActionResult> ConfirmEmail(string userId, string token)
        {
            try
            {
                // If no user or token is passed in for confirm email, redirect to sign up page
                if (userId == null || token == null) return View("Signup");

                var user = await _userManager.FindByIdAsync(userId);

                // If the user doesnt exist, display invalid ID error
                if (user == null)
                {
                    ViewBag.ErrorMessage = userId + "invalid Id";
                    return RedirectToAction("Error", "Home");
                }

                // If token matches the one sent to the user, complete user confirmation
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
                // Deletes the user from session data
                ViewService.DeleteCustomerFromList(HttpContext.Session.GetString("Id")); 
                await _signInManager.SignOutAsync();
                HttpContext.Session.SetString("LoggedIN", "false");

                // Register a new guest customer
                GuestCustomer gc = new GuestCustomer(_dbRepo);
                GlobalVar.customerList.Add(gc);
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


            // Update Role of user
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


            // Get user by email
            if (Email != null)
            {
                user = await _userManager.FindByEmailAsync(Email);

                // Return empty view if user not found
                if (user == null) return View(new ManageRoleModel()); 
            }
            var roles = _roleManager.Roles;

            // Populate role list
            manageRoleModel.roleList = new List<SelectListItem>();
            manageRoleModel.currentRoleList = new List<IdentityRole>();
            foreach (var role in roles)
            {
                // Excluding super admin
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
          
            try
            {
                var _user = await _userManager.FindByIdAsync(HttpContext.Session.GetString("Id"));
                OrderView orderView = new OrderView();
                orderView.orders = new List<Order>();
                foreach (var order in _db.order.Include(m => m.user).Include(m => m.delivery).ToList())
                {
                    if (order.user != null && order.user.Id == _user.Id)
                    {
                        orderView.orders.Add(order);
                    }
                }
                orderView.user = _user;
                ViewData["Error MSG"] = msg;

                return View(orderView);
            try
            {
               
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
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
            ViewData["Msg"] = "Password or Email format incorrect";
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
                        var setting = context.userSettings.Include(m => m.user).ToList().Where(m => m.user.Id.ToString() == Id).FirstOrDefault();
                       
                        if (setting != null)
                        {
                            setting.isActive = false;
                            context.userSettings.Update(setting);
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
        public async Task<ActionResult> ConfirmEmailNotifAsync(string userId, string token)
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

        public ActionResult ConfirmEmailMsg() 
        {
            return View();
        }

        
        public IActionResult Error()
        {
            return View();
        }
    }
}
