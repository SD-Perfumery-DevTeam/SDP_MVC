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

        public IActionResult Login()
        {
            return View();
        }


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


        public IActionResult Signup()
        {
            return View(new SignupView());
        }

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

                    return RedirectToAction("Index", "Home");
                }
                foreach (var e in result.Errors)
                {
                    ModelState.AddModelError("", e.Description);
                }
            }
            return View(user);
        }


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



        public async Task<IActionResult> LogoutAsync()
        {
            try
            {
                await _signInManager.SignOutAsync();
                HttpContext.Session.SetString("LoggedIN", "false");
                GuestCustomer gc = new GuestCustomer();
                Global.customerList.Add(gc);//register new guest customer
                //ViewService.DeleteCustomerFromList(HttpContext.Session.GetString("Id"));//delete guest customer
                HttpContext.Session.SetString("Id", gc.userId.ToString());
            }
            catch (Exception ex)
            {
                return RedirectToAction("Error", "Home");
            }
            return RedirectToAction("Login");
        }

        //cms that manages roles 
        [Authorize(Roles = "SuperAdmin")]
        public IActionResult RoleManage()
        {

            return View(new ManageRoleModel());
        }
        [HttpPost]
        [Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> RoleManage(string Email, string Id, ManageRoleModel MRM)
        {
            IdentityUser user = new IdentityUser();
            ManageRoleModel manageRoleModel = new ManageRoleModel();

            //update Role of user
            if (MRM.roleId != null)
            {
                var role = await _roleManager.FindByIdAsync(MRM.roleId);
                var _user = await _userManager.FindByIdAsync(Id);
                if (role == null || user == null)
                {
                    return View("Not found");
                }

                if (!(await _userManager.IsInRoleAsync(_user, role.Name)))
                {
                    var result = await _userManager.AddToRoleAsync(_user, role.Name);
                }
                return View(new ManageRoleModel());
            }

            //get user by email
            if (Email != null)
            {
                user = await _userManager.FindByEmailAsync(Email);
            }
            var roles = _roleManager.Roles;


            manageRoleModel.roleList = new List<SelectListItem>();
            //populate role list
            foreach (var role in roles)
            {
                if (role.Name.ToLower() != "superadmin") //cannot assign super admin 
                {
                    manageRoleModel.roleList.Add(new SelectListItem { Text = role.Name, Value = role.Id });
                }
            }

            manageRoleModel.user = user;

            return View(manageRoleModel);
        }
        //user to manage their account
        [Authorize]
        public async Task<IActionResult> MyAccount() 
        {
            var _user = await _userManager.FindByIdAsync(HttpContext.Session.GetString("Id"));
            return View(_user);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> DeleteMyAccount( string Id)
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
    }
}
