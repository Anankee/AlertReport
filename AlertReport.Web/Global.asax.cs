using AlertReport.Db.DAL;
using AlertReport.Db.Interfaces;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Web.App_Start;

namespace AlertReport.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            CreateDefaultUser();
        }

        private void CreateDefaultUser()
        {
            IUnitOfWork unitOfWork = DependencyResolver.Current.GetService<IUnitOfWork>();

            var role = unitOfWork.RoleRepository.Get(e => e.Name == "Admin").SingleOrDefault();
            var user = unitOfWork.UserRepository.Get(e => e.Login == "Test").SingleOrDefault();

            if (role == null)            
                unitOfWork.RoleRepository.Add(new Db.Models.Role() { Name = "Admin" });            
            if (user == null)
            {
                PasswordHasher passwordHasher = new PasswordHasher();
                unitOfWork.UserRepository.Add(new Db.Models.User()
                {
                    Login = "Test",
                    Password = passwordHasher.HashPassword("123456"),
                    Email = "test@test.pl",
                    Roles = new Db.Models.Role[] {unitOfWork.RoleRepository.Get(e=>e.Name == "Admin").SingleOrDefault()}
                });
            }
                
                unitOfWork.Dispose();
        }
    }
}
