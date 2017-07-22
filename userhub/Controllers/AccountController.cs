using System.Collections.Generic;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using Genesis.idlib.Models;
using Genesis.idlib.Services;

using userhub.Infrastructure.Extensions;
using userhub.Infrastructure.Services;
using userhub.Infrastructure.Mappers;
using userhub.Models;
using Genesis.idlib.Data;
using System;
using Genesis.idlib.RequestObjects;

namespace userhub.Controllers
{
    [Authorize]
    public class AccountController: Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IModelDataService _modelDataSvc;
        private readonly ILogger _logger;

        private readonly IUserDataService _usrDataSvc;

        public AccountController(
                                UserManager<ApplicationUser> userManager,
                                SignInManager<ApplicationUser> signInManager,
                                IModelDataService modelDataSvc,
                                ILoggerFactory loggerFactory,
                                IUserDataService usrDataSvc )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _modelDataSvc = modelDataSvc;
            _usrDataSvc = usrDataSvc;
            _logger = loggerFactory.CreateLogger<AccountController>();
        }

        [HttpGet]
        [Authorize(Policy="AdminOnly")]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            GeExternalVMData();

            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Policy="AdminOnly")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Email, Email = model.Email, FirstName = model.FirstName,
                                                LastName = model.LastName, CompanyId = model.CompanyId };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                    // Send an email with this link
                    //var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    //var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                    //await _emailSender.SendEmailAsync(model.Email, "Confirm your account",
                    //    $"Please confirm your account by clicking this link: <a href='{callbackUrl}'>link</a>");
                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    _logger.LogInformation(3, "User created a new account with password.");
                    return RedirectToLocal(returnUrl);
                }
                AddErrors(result);
            }

            GeExternalVMData();
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        // GET: /Account/ForgotPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        //
        // POST: /Account/ForgotPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return View("ForgotPasswordConfirmation");
                }

                // For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=532713
                // Send an email with this link
                //var code = await _userManager.GeneratePasswordResetTokenAsync(user);
                //var callbackUrl = Url.Action("ResetPassword", "Account", new { userId = user.Id, code = code }, protocol: HttpContext.Request.Scheme);
                //await _emailSender.SendEmailAsync(model.Email, "Reset Password",
                //   $"Please reset your password by clicking here: <a href='{callbackUrl}'>link</a>");
                //return View("ForgotPasswordConfirmation");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ForgotPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            return code == null ? View("Error") : View();
        }

        //
        // POST: /Account/ResetPassword
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                // Don't reveal that the user does not exist
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return RedirectToAction(nameof(AccountController.ResetPasswordConfirmation), "Account");
            }
            AddErrors(result);
            return View();
        }

        //
        // GET: /Account/ResetPasswordConfirmation
        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult Logout()
        {
            return new SignOutResult(new string[]{ "oidc", "Cookies" });
        }

        [HttpGet]        
        [AllowAnonymous]
        public IActionResult AccessDenied(string returnUrl)
        {
            return View();
        }

        [HttpGet]
        [Authorize(Policy="AdminOnly")]
        public async Task<IActionResult> EditUser(long id)
        {
            var usr =  await _userManager.FindByIdAsync(id.ToString());
            if(usr!=null)
            {
                var editUsrVM = usr.MapToEditUserViewModel();

                GeExternalVMData();
                
                return View(editUsrVM);
            }
           
            ModelState.AddModelError("","User was not found");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy="AdminOnly")]
        public async Task<IActionResult> EditUser(EditUserViewModel editUserVM)
        {
            var usr = await _userManager.FindByIdAsync(editUserVM.Id.ToString());

            if (ModelState.IsValid && usr!=null)
            {                
                usr.FirstName = editUserVM.FirstName;
                usr.LastName = editUserVM.LastName;
                usr.CompanyId = editUserVM.CompanyId;
                usr.PhoneNumber = editUserVM.PhoneNumber;
                var result = await _userManager.UpdateAsync(usr);

                if(result.Succeeded)
                    return RedirectToAction(nameof(UserList));
                else
                    AddErrors(result);
            }

            return View(editUserVM);
        }

        [HttpGet]
        [Authorize(Policy="AdminOnly")]
        public IActionResult UserList(int pageNum,int sortBy)
        {
            var usrPageReq = BuildUserPageRequest(pageNum,DecodeSort(sortBy));
            var usrPagedList = _usrDataSvc.GetUsersPage(usrPageReq);
            var usrListVM = usrPagedList.ItemList.ConverToUserRowVMList();
            return View(usrListVM);
        }

        private DataItemPageRequest BuildUserPageRequest(int pageNum, int sortBy)
        {
            var itemPageReq = new DataItemPageRequest();
            
            itemPageReq.PageNumber = (pageNum <= 0) ? 1 : pageNum;
            itemPageReq.PageSize = 10;
            itemPageReq.SortBy = (sortBy ==0) ? 1: sortBy;

            return itemPageReq;
        }

        private void InitSort(int sortBy)
        {
            if(sortBy == 0)
                InitViewData();
        }

        private void InitViewData()
        {
            ViewData["FirstNameSort"] = "3";
            ViewData["LastNameSort"] = "4";
            ViewData["CompanySort"] = "6";
            ViewData["ActiveSort"] = "7";
        }

        private int DecodeSort(int sortBy)
        {   
            InitSort(sortBy);
            if(sortBy == 0) return 3;

            int newSort;

            if(sortBy == 0 || Math.Abs(sortBy) > 4) return 3;            

            newSort = sortBy*(-1);
            InitViewData();

            switch(Math.Abs(sortBy))
            {
                case 3:
                        ViewData["FirstNameSort"] = newSort;
                        break;
                case 4: 
                        ViewData["LastNameSort"] = newSort;
                        break;
                case 6:
                        ViewData["CompanySort"] = newSort;
                        break;
                case 7:
                        ViewData["Activesort"] = newSort;
                        break;                        
            }

            return newSort;
        }
        

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        private void GeExternalVMData()
        {
            var CompanyList = _modelDataSvc.GetCompanies();
            ViewBag.CompanyList = CompanyList.ConvertList("CompanyName","CompanyId");
        }
    }

}