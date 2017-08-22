using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

using Genesis.idlib.Models;

using idsvr4.Models;
using idsvr4.Infrastructure;
using idsvr4.Services;

using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;

namespace idsvr4.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _interaction;

        private readonly IEventService _eventsSvc;
        

        private readonly AccountService _account;

        public AccountController(IIdentityServerInteractionService  interaction,
                                IHttpContextAccessor                httpContextAccessor,
                                IClientStore                        clientStore,
                                IEventService                       eventSvc,
                                UserManager<ApplicationUser>        userManager)
        {
             _interaction = interaction;
             _account = new AccountService(interaction, httpContextAccessor, clientStore);
             _eventsSvc = eventSvc;
             _userManager = userManager;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model)
        {
            if(ModelState.IsValid)
            {
                var identityUser = await _userManager.FindByEmailAsync(model.Username);
                if(identityUser!=null && await _userManager.CheckPasswordAsync(identityUser, model.Password))
                {
                    AuthenticationProperties props = null;
                    if(AccountOptions.AllowRememberLogin && model.RememberMe)
                    {
                        props = new AuthenticationProperties{
                        IsPersistent = true,
                        ExpiresUtc  = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration)
                        };
                    }

                    await _eventsSvc.RaiseAsync(new UserLoginSuccessEvent(identityUser.UserName,identityUser.Id.ToString(),identityUser.UserName));

                    await HttpContext.Authentication.SignInAsync(identityUser.Id.ToString(), identityUser.UserName, props);

                    if(_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return Redirect(model.ReturnUrl);
                    }

                    return Redirect("~/");

                }

                await _eventsSvc.RaiseAsync(new UserLoginFailureEvent(model.Username,"invalid credentials"));
            
                ModelState.AddModelError("",AccountOptions.InvalidCredentialsErrorMessage);
            }

            
            var vm = await _account.BuildLoginViewModelAsync(model);

            var returnUrl = model.ReturnUrl == null ? null  : model.ReturnUrl;
            
            if(returnUrl == null)
                return View(vm);

             return RedirectToAction("Login", new { returnUrl });   
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            var vm = await _account.BuildLogoutViewModelAsync(logoutId);

            if (vm.ShowLogoutPrompt == false)
            {
                // no need to show prompt
                return await Logout(vm);
            }

            return View(vm);
        }

        /// <summary>
        /// Handle logout page postback
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            var vm = await _account.BuildLoggedOutViewModelAsync(model.LogoutId);
            if (vm.TriggerExternalSignout)
            {
                string url = Url.Action("Logout", new { logoutId = vm.LogoutId });
                try
                {
                    // hack: try/catch to handle social providers that throw
                    await HttpContext.Authentication.SignOutAsync(vm.ExternalAuthenticationScheme,
                        new AuthenticationProperties { RedirectUri = url });
                }
                catch (NotSupportedException) // this is for the external providers that don't have signout
                {
                }
                catch (InvalidOperationException) // this is for Windows/Negotiate
                {
                }
            }

            // delete local authentication cookie
            await HttpContext.Authentication.SignOutAsync();

            var user = await HttpContext.GetIdentityServerUserAsync();
            if (user != null)
            {
                await _eventsSvc.RaiseAsync(new UserLogoutSuccessEvent(user.GetSubjectId(), user.GetName()));
            }

            return View("LoggedOut", vm);
        }

    }
}