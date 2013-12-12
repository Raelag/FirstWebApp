using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MvcApplication2.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        private Collection<Post> collPosts; 
        private Collection<Reply> collReplies;
        private MessagesEntities ME;

        public HomeController()
        {
            collPosts = new Collection<Post>();
            collReplies = new Collection<Reply>();
            ME = new MessagesEntities();
        }

        public ActionResult Index()
        {
            var posts = from Post in ME.Posts select Post;
            foreach (Post P in posts)
            {
                collPosts.Add(P);
            }
            ViewBag.Coll = collPosts;
            return View();
        }

        [HttpPost]
        public ActionResult Index(string title, string message)
        {
            Post Pt = new Post();
            Pt.Title = title;
            Pt.Message = message;
            Pt.Date = DateTime.Now;
            ME.Posts.Add(Pt);
            ME.SaveChanges();


            var posts = from Post in ME.Posts select Post;
            foreach (Post P in posts)
            {
                collPosts.Add(P);
            }
            ViewBag.Coll = collPosts;
            return View(collPosts);
        }

        [HttpPost]
        public ActionResult Post(FormCollection form)
        {
            int ID;
            Reply reply = new Reply();
            Post p;
            int rep = Convert.ToInt32(form["rep"]);
            if (rep == 0)
            {
                ID = Convert.ToInt32(form["ID"]);
                p = (from Post in ME.Posts where Post.PostID == ID select Post).First();
            }
            else
            {
                ID = Convert.ToInt32(form["postid"]);
                reply.PostID = ID;
                reply.Name = form["name"].ToString();
                reply.Email = form["email"].ToString();
                reply.Comment = form["comment"].ToString();
                reply.Date = DateTime.Now;
                ME.Replies.Add(reply);
                ME.SaveChanges();
                p = (from Post in ME.Posts where Post.PostID == reply.PostID select Post).First();
            }
            ViewBag.Post = p;
            var replies = from Reply in ME.Replies where Reply.PostID == ID select Reply;
            foreach (var r in replies)
            {
                collReplies.Add(r);
            }
            return View(collReplies);
        }
    }
}
