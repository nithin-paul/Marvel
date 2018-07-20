// <copyright file="LoginController.cs" company="SchoolSpeak Technologies Private Limited">
// Copyright (c) SchoolSpeak Technologies Private Limited. All rights reserved.
// </copyright>

using Marvel.BLL;
using Marvel.Model;
using Marvel.Utils;
using MarvelAdmin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace MarvelAdmin
{
    /// <summary>
    /// Login Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    public class LoginController : Controller
    {

        public AdminManageBLL adminManagerBll = null;
        public LoginController()
        {
            adminManagerBll = new AdminManageBLL();
        }
        /// <summary>
        /// Logins this instance.
        /// </summary>
        /// <returns>View result for login</returns>
        [HttpGet]
        public ActionResult Index()
        {
            ViewBag.ReturnUrl = (!string.IsNullOrEmpty(Request.QueryString["ReturnUrl"])) ? Request.QueryString["ReturnUrl"] : string.Empty;
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        /// <summary>
        /// Logins the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <param name="returnUrl">The return URL.</param>
        /// <returns>View result</returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(LoginVM model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
               List<ModelErrorCollection> errors =  ModelState.Select(x => x.Value.Errors)
                           .Where(y => y.Count > 0)
                           .ToList();
                foreach (ModelErrorCollection error in errors)
                {
                    TempData["Error"] += error[0].ErrorMessage + "<br/>";
                }
                return View("Index", model);
            }

            // fecth active product from the products table
            var user = adminManagerBll.AuthenticateUser(model.UserName, model.Password);
            if (user == null)
            {
                model.Password = string.Empty;
                TempData["Error"] = "Invalid credentials. Please try again.";
                return View("Index", model);
            }
            var userVM =  MapperManager.GetMapperInstance().Map<UserDTO>(user);
            if (userVM != null)
            {
                model.Password = string.Empty;
                FormsAuthentication.SetAuthCookie(model.UserName, false);
                model.Roles = Constants.ROLE_ADMIN;
                var authTicket = new FormsAuthenticationTicket(1, userVM.UserName, DateTime.Now, DateTime.Now.AddMinutes(40), false, model.Roles);
                string encryptedTicket = FormsAuthentication.Encrypt(authTicket);
                var authCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
                HttpContext.Response.Cookies.Add(authCookie);
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                model.Password = string.Empty;
                TempData["Error"] = "Invalid credentials. Please try again.";
                return View("Index", model);
            }
        }

        /// <summary>
        /// Logs the off.
        /// </summary>
        /// <returns>Login Screen</returns>
        [Route("SignOut")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login");
        }
    }
}
