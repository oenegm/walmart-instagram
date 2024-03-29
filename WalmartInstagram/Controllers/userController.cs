﻿using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WalmartInstagram.Models;

namespace WalmartInstagram.Controllers
{
    public class userController : Controller
    {
        instagramContext db = new instagramContext();

        public ActionResult signUp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult signUp(user s, HttpPostedFileBase img)
        {
            user d = db.users.Where(n => n.username == s.username).FirstOrDefault();
            if (d != null)
            {
                ViewBag.status = "Username aleardy exists!!";
                return View();
            }

            string imgName = s.username;
            string extention = img.ContentType.Contains("image/jpeg") ? ".jpg" : ".png";
            img.SaveAs(Server.MapPath("~/attach/pfp/" + imgName + extention));
            s.profilePic = imgName + extention;

            db.users.Add(s);
            db.SaveChanges();

            return RedirectToAction("signIn");
        }

        public ActionResult signIn()
        {
            return View();
        }
        [HttpPost]
        public ActionResult signIn(user s)
        {
            user d = db.users.Where(n => n.username == s.username && n.password == s.password).FirstOrDefault();
            if (d != null)
            {
                Session.Add("username", d.username);
                return RedirectToAction("profile");
            }
            else
            {
                ViewBag.status = "incorrect username or password";
                return View();
            }
        }

        public ActionResult profile()
        {
            if (Session["username"] == null) return RedirectToAction("signIn");

            string id = (string)Session["username"];
            user s = db.users.Where(n => n.username == id).FirstOrDefault();

            DateTime birthday = db.users.Where(n => n.username == id).FirstOrDefault().dateOfBirth;
            int age = DateTime.Now.Year - birthday.Year;
            if (DateTime.Now.DayOfYear < birthday.DayOfYear) { age--; }
            ViewBag.age = age;

            return View(s);
        }

        public ActionResult signOut()
        {
            Session["username"] = null;
            return RedirectToAction("signIn");
        }

        public ActionResult edit(string username)
        {
            if (username != (string)Session["username"]) return RedirectToAction("signIn");
            user tempUser = db.users.Where(n => n.username == username).FirstOrDefault();
            return View(tempUser);
        }
        [HttpPost]
        public ActionResult edit(user newGuy, HttpPostedFileBase img)
        {
            string imgName = newGuy.username;
            string extention = img.ContentType.Contains("image/jpeg") ? ".jpg" : ".png";
            img.SaveAs(Server.MapPath("~/attach/pfp/" + imgName + extention));
            newGuy.profilePic = imgName + extention;

            db.Entry(newGuy).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("profile");
        }

        public ActionResult otherProfile(string otherusername)
        {
            if (otherusername == (string)Session["username"]) return RedirectToAction("profile");
            user otheruser = db.users.Where(n => n.username == otherusername).FirstOrDefault();

            ViewBag.username = otheruser.username;
            ViewBag.email = otheruser.email;
            ViewBag.name = otheruser.firstName;
            ViewBag.profilepic = otheruser.profilePic;
            int age = DateTime.Now.Year - otheruser.dateOfBirth.Year;
            if (DateTime.Now.DayOfYear < otheruser.dateOfBirth.DayOfYear) { age--; }
            ViewBag.age = age;

            return View();
        }
    }
}
