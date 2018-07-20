// <copyright file="ErrorController.cs" company="SchoolSpeak Technologies Private Limited">
// Copyright (c) SchoolSpeak Technologies Private Limited. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MarvelAdmin
{
    /// <summary>
    /// Error Controller
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    [RoutePrefix("Error")]
    public class ErrorController : Controller
    {
        /*/// <summary>
        /// Index
        /// </summary>
        /// <param name="code">code</param>
        /// <returns>view </returns>
        [AllowAnonymous]
        [Route("Error")]
        [HttpGet]
        public ViewResult Index(int? code)
        {
            var statusCode = code.HasValue ? code.Value : 500;
            var error = new HandleErrorInfo(new Exception("An exception with error " + statusCode + " occurred!"), "Error", "Index");

            int errorCode = statusCode;
            ViewBag.error = errorCode;
            return View();
        }*/

        /// <summary>
        /// Index
        /// </summary>
        /// <returns>view </returns>
        [AllowAnonymous]
        [Route("")]
        [HttpGet]
        public ViewResult Index()
        {
            return View();
        }

        /// <summary>
        /// Files the not found.
        /// </summary>
        /// <returns>Error for code</returns>
        [AllowAnonymous]
        [Route("NotFound")]
        [HttpGet]
        public ActionResult NotFound()
        {
            return View();
        }
    }
}