﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Orhedge.Helpers;
using Orhedge.ViewModels;
using ServiceLayer.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using IAuthenticationService = ServiceLayer.Services.IAuthenticationService;

namespace Orhedge.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IAuthenticationService _authService;
        private readonly IMapper _mapper;

        public AuthenticationController(IAuthenticationService authService, IMapper mapper)
            => (_authService, _mapper) = (authService, mapper);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginViewModel login, [FromQuery] string returnUrl = "")
        {
            if (ModelState.IsValid)
            {
                LoginResponse loginResponse = await _authService.Login(_mapper.Map<LoginRequest>(login));
                if (loginResponse == null)
                {
                    ViewData["invalidCred"] = true;
                    ViewData["returnUrl"] = returnUrl;
                    return View("Views/Home/Login.cshtml", login);
                }
                else
                {
                    ClaimsPrincipal claimsPrincipal = GetClaimsPrincipal(loginResponse);
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, claimsPrincipal,
                        new AuthenticationProperties
                        {
                            IsPersistent = login.RememberMe
                        });

                    return Redirect(Url.IsLocalUrl(returnUrl) ? returnUrl : Url.Action("Index", "StudyMaterial"));
                }
            }
            else
            {
                ViewData["returnUrl"] = returnUrl;
                return View("Views/Home/Login.cshtml", login);
            }
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            // Simply delete authentication cookie
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        private ClaimsPrincipal GetClaimsPrincipal(LoginResponse loginResponse)
            => AuthenticationHelpers.GetClaimsPrincipal(loginResponse.Id, loginResponse.Privilege);

    }
}