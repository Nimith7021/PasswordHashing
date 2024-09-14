using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PasswordHashingFluentMVC.Data;
using PasswordHashingFluentMVC.Models;

namespace PasswordHashingFluentMVC.Controllers
{
    public class UserController : Controller
    {
        // GET: User
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Login(User user)
        {
            using (var session = NHibernateHelper.CreateSession())
            {
                if (!ModelState.IsValid)
                {
                    return View(user);
                }
                var userList = session.Query<User>().ToList();

                foreach (var targetUser in userList) {                             //one or more than one people with same
                                                                                                            //name
                    if (targetUser.UserName.Equals(user.UserName) &&
                        BCrypt.Net.BCrypt.Verify(user.Password, targetUser.Password))
                    {
                        TempData["userName"] = user.UserName;
                        return RedirectToAction("Page");
                    }
                }

                    ModelState.AddModelError("Error","Invalid");
                    return View(user);
                
                

                
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]

        public ActionResult Create(User user)
        {
            using (var session = NHibernateHelper.CreateSession()){
                using (var txn = session.BeginTransaction())
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                    session.Save(user);
                    txn.Commit();
                    return RedirectToAction("Login"); 
                }
            }
        }

        public ActionResult Page()
        {
            ViewBag.UserName = TempData["userName"];
            return View();
        }
    }
}