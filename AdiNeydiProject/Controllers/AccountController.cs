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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace AdiNeydiProject.Controllers
{
    public class AccountController : Controller
    {
        public PostgresContext _database;

        public AccountController(PostgresContext database)
        {
            _database = database;
        }
        public IActionResult Index()
        {
            return View();
        }

        
        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> Login(string txtUserName, string txtPassword)
        {

            // PostgresContext db = new PostgresContext();

            string passwordform = txtPassword;
            byte[] salt = Encoding.ASCII.GetBytes("440355d96220b9aa3829a5816257140b"); // Tuzlama için rastgele bir değer belirleyin
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(txtPassword, salt, 10000); // 10000 iterasyonla hashleme yap
            byte[] LoginPasshash = rfc2898DeriveBytes.GetBytes(32);

            var userget = _database.Users.Where(x => x.UserName == txtUserName && x.PasswordHash == LoginPasshash).FirstOrDefault();
            var usertype = _database.UserTypes.Where(x => x.Id == userget.UserTypeId).FirstOrDefault();
              string UsertypeName;
            if(usertype==null)
            {
             UsertypeName = "Rolsüz";
            }
            else
            {
             UsertypeName = usertype.Name;

            }
            if (userget != null)
            {
                var newclaims = new Claim[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName,txtUserName),
                    new Claim(JwtRegisteredClaimNames.NameId,Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.Name, userget.UserName),
                    new Claim ("UserMail",userget.Email),
                    new Claim("UserId",userget.Id.ToString()),                   
                    new Claim("Role", usertype.Name ),
                    
                    
                    //new Claim("UserType", usertype.Name)

            
       

            };


                SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("evlerkiralik1234"));
                var token = new JwtSecurityToken(
                    issuer: "evlerkiralik.com",
                    audience: "evlerkiralik.com",
                    claims: newclaims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256
                    ));

                var identity = new ClaimsIdentity(
                    newclaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);
                var props = new AuthenticationProperties();
                HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();

                HttpContext.Response.Cookies.Append("token", token.ToString(), new Microsoft.AspNetCore.Http.CookieOptions { Expires = DateTime.Now.AddHours(1) });
                //HttpContext.SignInAsync(
                //  CookieAuthenticationDefaults.AuthenticationScheme, principal, props).Wait();
                var newtoken = new JwtSecurityTokenHandler().WriteToken(token);
                ViewBag.UserId = userget.Email;
                return RedirectToAction("Index", "Home");
            }
            else
            {

                return RedirectToAction("Index", "Home");
            }
        }



        public IActionResult Logout()
        {
            return View();
        }
         [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> LogoutAct()
        {

            await HttpContext.SignOutAsync();
            Response.Cookies.Delete("adineydi.Auth");

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        public async Task<IActionResult> Register(string FirstName , string LastName,string username, string email, string password)
        {
            User newuser = new User();
            UserType UserTypeId = _database.UserTypes.Where(x=>x.Name == "User").FirstOrDefault();
           
            newuser.UserName = username;
            newuser.Email = email;
            newuser.FirstName = FirstName;
            newuser.LastName = LastName;
            newuser.UserTypeId = UserTypeId.Id;
            
            string passwordform = password;
            byte[] salt = Encoding.ASCII.GetBytes("440355d96220b9aa3829a5816257140b"); // Tuzlama için rastgele bir değer belirleyin
            var rfc2898DeriveBytes = new Rfc2898DeriveBytes(password, salt, 10000); // 10000 iterasyonla hashleme yap
            byte[] hash = rfc2898DeriveBytes.GetBytes(32);

            newuser.PasswordHash = hash;
          
            newuser.IsPhoneVerificated = "Unverified";
            _database.Add(newuser);
            await _database.SaveChangesAsync();
            return RedirectToAction("Index", "Home");

        }


    }
}

