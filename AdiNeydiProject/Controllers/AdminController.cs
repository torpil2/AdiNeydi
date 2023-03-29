using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AdiNeydiProject.DAL;
using Npgsql;
using System.Dynamic;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdiNeydiProject.Controllers
{
    public class AdminController : Controller
    {
        public PostgresContext _context;

        public AdminController(PostgresContext context)
        {
            _context = context;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            dynamic model = new ExpandoObject();
            model.LastPosts = _context.Posts.Reverse().Take(5).Reverse().ToList();
            model.LastUsers = _context.Users.Reverse().Take(5).Reverse().ToList();
            return View();
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
            return View();

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
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory()
        {
            return View();
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

            return View();
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


      

      



        

    }
}

