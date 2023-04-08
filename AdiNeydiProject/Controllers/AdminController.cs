using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using AdiNeydiProject.DAL;
using AdiNeydiProject.CustomClasses;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Policy;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Build.Framework;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Cryptography;
using System.Dynamic;
using Microsoft.AspNetCore.Authorization;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdiNeydiProject.Controllers
{
   
    [Authorize(Policy = "Admin")]
    public class AdminController : Controller
    {
    
        public PostgresContext _database;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public AdminController(PostgresContext context, IWebHostEnvironment webHostEnvironment)
        {
        _database = context;
        _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Login()
        {
            
            return View();
        }

       
        // GET: /<controller>/


            
        public IActionResult Index()
        {
            dynamic model = new ExpandoObject();
            model.LastPosts = _database.Posts.OrderByDescending(p => p.Id).Take(10).ToList();
            List<User> LastPostsUser = new List<User>();
            List<Comment> LastPostsComments = new List<Comment>();
            List<Comment> LastUserComments = new List<Comment>();
            List<Post> LastUserPosts = new List<Post>();
            foreach(Post lastpost in model.LastPosts)
            {
                User user = _database.Users.Where(x=>x.Id == lastpost.UserId).FirstOrDefault();
              
                if(!LastPostsUser.Contains(user) && user != null)
                {
                LastPostsUser.Add(user);
                }
               
               
                    LastPostsComments.AddRange(  _database.Comments.Where(x=>x.PostId == lastpost.Id).ToList());
                 
            }

           
            model.LastPostsCommentModel = LastPostsComments;
            model.LastPostsUserModel = LastPostsUser;
            model.Kategoriler = _database.Categories.ToList();
            model.Types = _database.Types.ToList();
        
            model.LastUsers = _database.Users.OrderByDescending(x=>x.Id).Take(10).ToList();
            foreach(User lastusercom in model.LastUsers)
            {
                LastUserComments.AddRange(_database.Comments.Where(x=>x.UserId == lastusercom.Id).ToList());
                LastUserPosts.AddRange(_database.Posts.Where(x=>x.UserId== lastusercom.Id).ToList());
            }
            model.LastUserCommentsModel  = LastUserComments;
            model.LastUserPostModel = LastUserPosts;
            return View(model);
        }

        public IActionResult UserTypes()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddUserType()
        {
            return View();
        }

        
        public IActionResult Users()
        {
            dynamic model = new ExpandoObject();

            model.UserList = _database.Users.ToList();

            model.CommentList = _database.Comments.ToList();

            model.PostList = _database.Posts.ToList();

            

            return View(model);

        }

        public IActionResult UserDetail(int UserId)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> EditUser()
        {
            return View();

        }
        //dotnet ef dbcontext scaffold "Host=localhost;Database=adineydidb;Username=postgres;Password=0000" Npgsql.EntityFrameworkCore.PostgreSQL -c DbContext -o DAL
        public IActionResult Categories()
        {
            dynamic model = new ExpandoObject();

            model.CategoryList = _database.Categories.ToList();
            List<Post> CategoryPostList = _database.Posts.ToList();

            foreach (Category catpost in model.CategoryList)
            {
                CategoryPostList.AddRange(_database.Posts.Where(x=>x.CategoryId == catpost.Id).ToList());

            }
            model.CategoryPostModel = CategoryPostList;
            return View(model);
        }

        [HttpPost]
        public IActionResult AddCategory(string categoryname)
        {
            
            Category newCategory = new Category();
       
            newCategory.Name = categoryname;

            _database.Categories.Add(newCategory);
            _database.SaveChanges();
            return RedirectToAction("Categories","Admin");
        }

        [HttpGet]
        public IActionResult Categorydelete(int ID)
        {
            Category category = _database.Categories.Where(x=>x.Id ==ID).FirstOrDefault();

            _database.Categories.Remove(category);
            _database.SaveChanges();
            return RedirectToAction("Categories","Admin");
        }

        public IActionResult Types()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddType()
        {
            return View();
        }


        public IActionResult Posts()
        {
            dynamic model = new ExpandoObject();
            model.LastPosts = _database.Posts.OrderByDescending(p => p.Id).ToList();
            List<User> LastPostsUser = new List<User>();
            List<Comment> LastPostsComments = new List<Comment>();
       
            model.Kategoriler = _database.Categories.ToList();
            model.Types = _database.Types.ToList();
            foreach(Post lastpost in model.LastPosts)
            {
                User user = _database.Users.Where(x=>x.Id == lastpost.UserId).FirstOrDefault();
              
                if(!LastPostsUser.Contains(user) && user != null)
                {
                LastPostsUser.Add(user);
                }
               
               
                    LastPostsComments.AddRange(  _database.Comments.Where(x=>x.PostId == lastpost.Id).ToList());
                 
            }
          model.LastPostsCommentModel = LastPostsComments;
            model.LastPostsUserModel = LastPostsUser;
            return View(model);
        }

        public IActionResult PostDetails(int PostId)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> PostEdit()
        {

            return View();
        }


        [HttpGet]
        public IActionResult PostDelete(int ID)
        {
            Post post = _database.Posts.Where(x=>x.Id == ID).FirstOrDefault();
            if(post!=null)
            {
            _database.Remove(post);
            _database.SaveChanges();
            
          _database.RemoveRange(_database.Comments.Where(x=>x.PostId == post.Id).ToList());


            }
         

            return RedirectToAction("Index","Admin");
        }

        [HttpGet]
        public IActionResult UserDelete(int ID)
        {
            User user = _database.Users.Where(x=>x.Id==ID).FirstOrDefault();
            
            _database.Remove(user);

            _database.RemoveRange(_database.Comments.Where(x=>x.UserId == ID).ToList());
            _database.SaveChanges();

            return RedirectToAction("Index","Admim");
        }


        public IActionResult Comments()
        {
            dynamic model = new ExpandoObject();

            model.Comments = _database.Comments.ToList();
            List<User> CommentUserList = new List<User>();
            List<Post> CommentPostList = new List<Post>();

            foreach(Comment comms in model.Comments)
            {
                 User comuser =  _database.Users.Where(x=>x.Id==comms.UserId).FirstOrDefault();
                 Post compost = _database.Posts.Where(x=>x.Id == comms.PostId).FirstOrDefault();

                if(!CommentUserList.Contains(comuser))
                {
                      CommentUserList.Add(comuser);
                }
                if(!CommentPostList.Contains(compost))
                {
                    CommentPostList.Add(compost);
                }

            }
            model.ComUserList = CommentUserList;
            model.ComPostList = CommentPostList;
            return View(model);
        }

        [HttpGet]
        public IActionResult Deletecomment(int ID)
        {
            Comment comment = _database.Comments.Where(x=>x.Id==ID).FirstOrDefault();
            _database.Comments.Remove(comment);
            _database.SaveChanges();

            return RedirectToAction("Comments","Admin");
        }

      
        public IActionResult AccessDenied()
        {
            return RedirectToAction("Index","Home");
        }
      



        

    }
}

