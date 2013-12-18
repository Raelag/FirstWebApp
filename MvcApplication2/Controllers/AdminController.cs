using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using WebMatrix.WebData;
using MvcApplication2.Models;

namespace MvcApplication2.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private MessagesEntities db = new MessagesEntities();

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string username, string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            MD5CryptoServiceProvider csp = new MD5CryptoServiceProvider();
            byte[] bHash = csp.ComputeHash(bytes);

            string hash = string.Empty;
            foreach (byte b in bHash)
                hash += string.Format("{0:x2}", b);
            if (username == "admin" && hash == "fab8d31c22e2bbbdc3d799699c0037e1")
            {
                FormsAuthentication.SetAuthCookie(username,false);
                return RedirectToAction("Index", "Admin");
            }
            else return View();
        }


        //
        // GET: /Admin/
        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection form)
        {
            Post Pt = new Post();
            Pt.Title = form["title"];
            Pt.Message = form["message"];
            Pt.Date = DateTime.Now;
            db.Posts.Add(Pt);
            db.SaveChanges();
            return View(db.Posts.ToList());
        }

        //
        // GET: /Admin/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /Admin/Edit/5

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Post post)
        {
            if (ModelState.IsValid)
            {
                db.Entry(post).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        //
        // GET: /Admin/Delete/5

        public ActionResult Delete(int id = 0)
        {
            Post post = db.Posts.Find(id);
            if (post == null)
            {
                return HttpNotFound();
            }
            return View(post);
        }

        //
        // POST: /Admin/Delete/5

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Post post = db.Posts.Find(id);
            var reply = from Reply in db.Replies where Reply.PostID == post.PostID select Reply;
            db.Posts.Remove(post);
            foreach (var r in reply)
            {
                db.Replies.Remove(r);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}