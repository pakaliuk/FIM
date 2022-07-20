using FIM.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace FIM.Controllers
{
    [Authorize(Roles = "simpleUser")]

    public class TopicController : Controller
    {
        ApplicationDbContext _context;
     
       


        public TopicController(ApplicationDbContext db, UserManager<IdentityUser> userManager)
        {
            _context = db;
            

        }
        public IActionResult Index()
        {
            var topics = _context.Topics.ToList();
            return View(topics);
        }


        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                await  _context.Topics.AddAsync(new Models.Topic { Name = name });
                                
                await _context.SaveChangesAsync();
                
                
                
            }
            return RedirectToAction("Index");

        }

        public IActionResult CreatePost()
        {
            ViewBag.topicId = Request.Query.FirstOrDefault(p => p.Key == "topid").Value;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost(string postHeader, string postContent, int topicId)
        {
           var id= this.User.Identity.Name.ToString();
            IdentityUser postOwner = _context.Users.Where(p => p.Email == id).FirstOrDefault();

       

           
            Models.Post post = new() { IdentityUser = postOwner, PostHeader = postHeader, PostContent = postContent, TopicId=topicId };

            
            if (!string.IsNullOrEmpty(postHeader) && !string.IsNullOrEmpty(postContent))
            {
                await _context.Posts.AddAsync(post);
                               
                await _context.SaveChangesAsync();

            }


            return RedirectToAction("Index");
        }
        


        public IActionResult GetPostsByTopic(int id)
        {
            ViewBag.TopId = id;
          

            var posts = _context.Posts.ToList().Where(p=>p.TopicId==id);
            
            return View(posts);
        }


        public IActionResult GetPostsByUser()
        {
            var id = this.User.Identity.Name.ToString();
            ViewBag.userid = id;  //email
            IdentityUser postOwner = _context.Users.Where(p => p.Email == id).FirstOrDefault();
            var posts = _context.Posts.ToList().Where(p => p.IdentityUser == postOwner);
            return View(posts);
        }

        public IActionResult GetPostById(string postid)
        {
            FIM.Models.Post post = _context.Posts.Where(p => p.Id == postid).FirstOrDefault();
           //post.PostContent= post.PostContent.Replace(Environment.NewLine, "</p><p>");
            return View(post);
        }

        public IActionResult EditPostById(string postid)
        {
            ViewBag.postid = Request.Query.FirstOrDefault(p => p.Key == "postid").Value;
            FIM.Models.Post post = _context.Posts.Find(postid);
            return View(post);
        }

        [HttpPost]

        public async Task<IActionResult> EditPostById(string postid, string postHeader, string postContent)
        {


            ViewBag.postid = postid;

            //FIM.Models.Post post = _context.Posts.Where(p => p.Id == postid).FirstOrDefault();
            FIM.Models.Post post = await _context.Posts.FindAsync(postid);
            if (post!=null)
            {
                post.PostHeader = "Edited";
                post.PostContent = postContent;
                await _context.SaveChangesAsync();
            }

            return RedirectToAction("GetPostsByUser");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
