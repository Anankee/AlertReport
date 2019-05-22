﻿using AlertReport.Web.Infrastructure;
using AlertReport.Web.Interfaces;
using AlertReport.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AlertReport.Web.Controllers
{
    [AlertAuthorization]
    public class FormAlertController : Controller
    {
        private IAlertManager alertManager;

        public FormAlertController(IAlertManager alertManager)
        {
            this.alertManager = alertManager;
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View(new FormAlertViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormAlertViewModel model)
        {
            alertManager.Add(model);
            return View();
        }
    }
}