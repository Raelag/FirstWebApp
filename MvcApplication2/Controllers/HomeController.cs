using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MvcApplication2.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private MessagesEntities db = new MessagesEntities();

        public ActionResult Index()
        {
            return View(db.Posts.ToList());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Index(string title, string message)
        {
            Post Pt = new Post();
            Pt.Title = title;
            Pt.Message = message;
            Pt.Date = DateTime.Now;
            db.Posts.Add(Pt);
            db.SaveChanges();
            return View(db.Posts.ToList());
        }

        [HttpGet]
        public ActionResult Post(string id)
        {
            int ID;
            ID = Convert.ToInt32(id);
            var p = (from Post in db.Posts where Post.PostID == ID select Post).First();
            ViewBag.Post = p;
            var replies = from Reply in db.Replies where Reply.PostID == ID select Reply;
            return View(replies.ToList());
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Post(FormCollection form)
        {
            int ID;
            Reply reply = new Reply();
            ID = Convert.ToInt32(form["postid"]);
            reply.PostID = ID;
            reply.Name = form["name"].ToString();
            reply.Email = form["email"].ToString();
            reply.Comment = form["comment"].ToString();
            reply.Date = DateTime.Now;
            db.Replies.Add(reply);
            db.SaveChanges();
            var p = (from Post in db.Posts where Post.PostID == ID select Post).First();
            ViewBag.Post = p;
            var replies = from Reply in db.Replies where Reply.PostID == ID select Reply;
            return View(replies.ToList());
        }
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
