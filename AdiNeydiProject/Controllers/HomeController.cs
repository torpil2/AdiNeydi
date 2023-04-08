using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AdiNeydiProject.DAL;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using Npgsql;
using System.Web;
using Microsoft.AspNetCore.Http;
using System.Dynamic;
using Microsoft.EntityFrameworkCore;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon;
using Amazon.S3.Model;
using System.Configuration;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using AdiNeydiProject.CustomClasses;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Rewrite;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AdiNeydiProject.Controllers;

public class HomeController : Controller
{

    public PostgresContext _database;
    private readonly IWebHostEnvironment _webHostEnvironment;



    public HomeController(PostgresContext context, IWebHostEnvironment webHostEnvironment)
    {
        _database = context;
        _webHostEnvironment = webHostEnvironment;

    }



    public IActionResult Index()
    {
        
        dynamic model = new ExpandoObject();
        model.PostList = _database.Posts.OrderByDescending(p => p.Id).Take(10).ToList();
        model.CategoryList = _database.Categories.ToList();
        model.TypeList = _database.Types.ToList();
        List<UserIndex> PassUsers = new List<UserIndex>();

        var accesskey = "AKIAROTU5G7VPCYJ3U5T";

        var SecretKey = "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";

        RegionEndpoint bucketregion = RegionEndpoint.EUNorth1;

        var s3Client = new AmazonS3Client(accesskey, SecretKey, bucketregion);
        var s3Client2 = new AmazonS3Client(accesskey, SecretKey, bucketregion);


        Dictionary<int, string> Resimler = new Dictionary<int, string>();
        Dictionary<int, string> Audios = new Dictionary<int, string>();

        foreach (Post item in model.PostList)
        {
            if(item.UserId!=null)
            {

           
         User foundUser = _database.Users.Where(x => x.Id == item.UserId).FirstOrDefault();
        if(foundUser !=null)
        {

    
            UserIndex CustomUser = new UserIndex();
            CustomUser.UserID = foundUser.Id;
            CustomUser.FirstName = foundUser.FirstName;
            CustomUser.LastName = foundUser.LastName;
            CustomUser.UserName = foundUser.UserName;
            CustomUser.Email = foundUser.Email;
                bool UserExist = false;
                foreach (UserIndex checkuser in PassUsers)
                {
                    if(checkuser.UserID == CustomUser.UserID)
                    {
                        UserExist = true;

                    }
                }
                if(!UserExist)
                {
                    PassUsers.Add(CustomUser);
                }
            }

            }
            Picture postpicture = _database.Pictures.Where(x => x.PostId == item.Id).FirstOrDefault();
       if(postpicture!= null)
            {

         
            if(postpicture.Path!=null)
            {

         
        var request = new GetPreSignedUrlRequest
        {
            BucketName = "adineydibucket",
            Key = postpicture.Path,
            Expires = DateTime.UtcNow.AddHours(1)
        };

        var url = s3Client.GetPreSignedURL(request);
            Resimler.Add(item.Id, url);
        }
            }



            Audio postaudio = _database.Audios.Where(x => x.PostId == item.Id).FirstOrDefault();
           
            if(postaudio != null)
            {
             
                if (postaudio.Path !=null)
                {
                    var path = postaudio.Path.Trim();
                    var request2 = new GetPreSignedUrlRequest
                    {
                        BucketName = "adineydibucket",
                        Key = path,
                        Expires = DateTime.UtcNow.AddHours(1)
                    };

                    var audiourl = s3Client.GetPreSignedURL(request2);
                    Audios.Add(item.Id, audiourl);
                }
            }


        }


        model.AudioList = Audios;
        model.UserList = PassUsers;
        model.ResimList = Resimler;
        //UploadFile();
        return View(model);
    }



  
    [HttpPost]
    public IActionResult GetMoreData(int pageIndex, string[] categories, string[] postTypes)
    {
        try
        {

      
        dynamic model = new ExpandoObject();

        model.PostList = new List<Post>();

        int SkipIndexForCategory = pageIndex;
        int SkipIndexForType = pageIndex;


        for (int x= 0; x < 10; x++)
        {
      
            for (int i = 0; i < categories.Length; i++)
            {

              Post catPost =  _database.Posts.OrderByDescending(p => p.Id).Skip(SkipIndexForCategory).FirstOrDefault();

                if(catPost != null)
                {
                    if(catPost.CategoryId == Convert.ToInt32(categories[i]) && !model.PostList.Contains(catPost) )
                    {
                        model.PostList.Add(catPost);
                    }
                }
            }

            SkipIndexForCategory++;
        }


        for (int i = 0; i < 10; i++)
        {
            for (int x = 0; x < postTypes.Length; x++)
            {
                Post typePost = _database.Posts.OrderByDescending(p => p.Id).Skip(SkipIndexForType).FirstOrDefault();

                if (typePost != null)
                {
                    if (typePost.TypeId == Convert.ToInt32(postTypes[x]) && !model.PostList.Contains(typePost))
                    {
                        model.PostList.Add(typePost);
                    }
                }

            }
            SkipIndexForType++;
        }

        model.CategoryList = _database.Categories.ToList();

        List<UserIndex> PassUsers = new List<UserIndex>();

        var accesskey = "AKIAROTU5G7VPCYJ3U5T";

        var SecretKey = "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";

        RegionEndpoint bucketregion = RegionEndpoint.EUNorth1;

        var s3Client = new AmazonS3Client(accesskey, SecretKey, bucketregion);
        var s3Client2 = new AmazonS3Client(accesskey, SecretKey, bucketregion);


        Dictionary<int, string> Resimler = new Dictionary<int, string>();
        Dictionary<int, string> Audios = new Dictionary<int, string>();

        foreach (Post item in model.PostList)
        {
            if (item.UserId != null)
            {
                User foundUser = _database.Users.Where(x => x.Id == item.UserId).FirstOrDefault();
            if(foundUser != null)
            {

 
                UserIndex CustomUser = new UserIndex();
                CustomUser.UserID = foundUser.Id;
                CustomUser.FirstName = foundUser.FirstName;
                CustomUser.LastName = foundUser.LastName;
                CustomUser.UserName = foundUser.UserName;
                CustomUser.Email = foundUser.Email;
                bool UserExist = false;
                foreach (UserIndex checkuser in PassUsers)
                {
                    if (checkuser.UserID == CustomUser.UserID)
                    {
                        UserExist = true;
                    }
                }
                if (!UserExist)
                {
                    PassUsers.Add(CustomUser);
                }
            }
           }

            Picture postpicture = _database.Pictures.Where(x => x.PostId == item.Id).FirstOrDefault();
            if (postpicture != null)
            {
                if (postpicture.Path != null)
                {
                    var request = new GetPreSignedUrlRequest
                    {
                        BucketName = "adineydibucket",
                        Key = postpicture.Path,
                        Expires = DateTime.UtcNow.AddHours(1)
                    };

                    var url = s3Client.GetPreSignedURL(request);
                    if (!Resimler.ContainsKey(item.Id) || !Resimler.ContainsValue(url))
                    Resimler.Add(item.Id, url);
                }
            }

            Audio postaudio = _database.Audios.Where(x => x.PostId == item.Id).FirstOrDefault();

            if (postaudio != null)
            {

                if (postaudio.Path != null)
                {
                    var path = postaudio.Path.Trim();
                    var request2 = new GetPreSignedUrlRequest
                    {
                        BucketName = "adineydibucket",
                        Key = path,
                        Expires = DateTime.UtcNow.AddHours(1)
                    };

                    var audiourl = s3Client.GetPreSignedURL(request2);
                    if (!Audios.ContainsKey(item.Id) || !Audios.ContainsValue(audiourl))
                    {
                        Audios.Add(item.Id, audiourl);
                    }
              
                }
            }

        }


        model.AudioList = Audios;
        model.UserList = PassUsers;
        model.ResimList = Resimler;
        //UploadFile();
        
        return PartialView(model);
          }
          catch
          {
            return PartialView();
          }
    }

    [Authorize]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public async Task<IActionResult> PostYukle(string description,int categoryid,IFormFile file, IFormFile audiofile)
        {

            try
            {
         
      
            Post newPost = new Post();
            newPost.Title = "Boş Title";
            newPost.Description = description;
           if(categoryid != 0)
           {
            newPost.CategoryId = categoryid;
           }
           else
           {
            newPost.CategoryId = 3;

           }

        var currentuser = User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            newPost.UserId = Convert.ToInt32(currentuser);


            if(file != null && audiofile != null)
            {
              newPost.TypeId = 1;
            }
            else if(file != null && audiofile == null)
            {
              newPost.TypeId = 2;
            }
            else if(file == null && audiofile != null)
            {
              newPost.TypeId = 3;
            }
            else
            {
             newPost.TypeId =  4;
            }


               newPost.CreatedTime = DateTime.Now;
             _database.Posts.Add(newPost);

             _database.SaveChanges();
            int id = newPost.Id;
        
  
  
        var accesskey = "AKIAROTU5G7VPCYJ3U5T";

        var SecretKey = "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";

        RegionEndpoint bucketregion = RegionEndpoint.EUNorth1;

        var bucketName = "adineydibucket";


        if (file != null)
        {
            var s3Client = new AmazonS3Client(accesskey, SecretKey, bucketregion);

            var fileTransferUtility = new TransferUtility(s3Client);
            var key = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Path.GetTempPath(), key);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                 file.CopyTo(stream);
            }
            var fileTransferUtilityRequest = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                FilePath = filePath,
                Key = key,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.NoACL
            };
             fileTransferUtility.Upload(fileTransferUtilityRequest);

            //string filepath = $"https://{bucketName}.s3.eu-north-1.amazonaws.com/{key}";

            string filepath = key;
            int PostID = id;
            Picture newpicture = new Picture();
            newpicture.Path = filepath;
            newpicture.PostId = PostID;
            _database.Pictures.Add(newpicture);
            await _database.SaveChangesAsync();
        }
      




        if (audiofile != null)
        {
            var s3Client2 = new AmazonS3Client(accesskey, SecretKey, bucketregion);

            var fileTransferUtility2 = new TransferUtility(s3Client2);
            var key2 = Guid.NewGuid().ToString() + Path.GetExtension(audiofile.FileName);
            var filePath2 = Path.Combine(Path.GetTempPath(), key2);
            using (var stream2 = new FileStream(filePath2, FileMode.Create))
            {
                 audiofile.CopyTo(stream2);
            }
            var fileTransferUtilityRequest2 = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                FilePath = filePath2,
                Key = key2,
                ContentType = audiofile.ContentType,
                CannedACL = S3CannedACL.NoACL
            };
             fileTransferUtility2.Upload(fileTransferUtilityRequest2);
       



            //string filepath = $"https://{bucketName}.s3.eu-north-1.amazonaws.com/{key}";

            string filepath2 = key2;
            int PostID = id;
            Audio newaudio = new Audio();
            newaudio.Path = filepath2;
            newaudio.PostId = PostID;
            _database.Audios.Add(newaudio);
             _database.SaveChanges();
        }
      



        return RedirectToAction("Index");

               
            }
            catch
            {
                return RedirectToAction("Index");
            }


    }

    [HttpGet]
    public IActionResult Userdetails(int ID)
    {
        try
        {

      
        dynamic model = new ExpandoObject();


        User Usser = _database.Users.Where(x=>x.Id == ID).SingleOrDefault();
        if(Usser!=null)
        {

    
        model.UserList = _database.Users.Where(x => x.Id == ID).SingleOrDefault(); 
         
        model.UserPostList = _database.Posts.Where(x => x.UserId == ID).OrderBy(x => x.CreatedTime).ToList();
        model.CategoryList = _database.Categories.ToList();
        model.TypeList = _database.Types.ToList();
        int TrueCommentCount = _database.Comments.Where(x => x.UserId == ID && x.TrueComment == true).ToList().Count();
        model.TrueComment = TrueCommentCount;
        model.CommentList = _database.Comments.Where(x => x.UserId == ID).ToList();
      
        
        return View(model);
            }
            else
            {
            return RedirectToAction("Index","Home");

            }
          }
          catch
          {
            return RedirectToAction("Index","Home");
          }
    }


    [HttpGet]
    public IActionResult Postdetails(int ID)
    {
        try
        {

   
        dynamic model = new ExpandoObject();


        var accesskey = "AKIAROTU5G7VPCYJ3U5T";

        var SecretKey = "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";

        RegionEndpoint bucketregion = RegionEndpoint.EUNorth1;

        var s3Client = new AmazonS3Client(accesskey, SecretKey, bucketregion);


          Picture postpicture = _database.Pictures.Where(x => x.PostId == ID).FirstOrDefault();
            if (postpicture != null)
            {
                if (postpicture.Path != null)
                {
                    var request = new GetPreSignedUrlRequest
                    {
                        BucketName = "adineydibucket",
                        Key = postpicture.Path,
                        Expires = DateTime.UtcNow.AddHours(1)
                    };

                    var url = s3Client.GetPreSignedURL(request);
                    model.ResimURL = url;
                 
                }
            }
            else
            {
                model.ResimURL = null;
            }






        Post relatedPost = _database.Posts.Where(x => x.Id == ID).SingleOrDefault();
        model.Post = relatedPost;
        List<Comment> CommentList = _database.Comments.Where(x => x.PostId == ID).OrderBy(x => x.CommentOrder).ToList();
        model.CommentList = CommentList;
        model.PostOwner = _database.Users.Where(x=>x.Id == relatedPost.UserId).SingleOrDefault();
    
        List<User> CommentUsers = new List<User>();
        foreach (Comment item in CommentList)
        {
            User commentuser = _database.Users.Where(x => x.Id == item.UserId).SingleOrDefault();
            if(commentuser !=null)
            {
                if (!CommentUsers.Contains(commentuser))
                {
                    CommentUsers.Add(commentuser);
                }
            }
          
        }

        model.UserModelList = CommentUsers;
        model.UserList = _database.Users.ToList();
        if(relatedPost !=null)
        {
            model.Category = _database.Categories.Where(x => x.Id == relatedPost.CategoryId).SingleOrDefault();
            model.Type = _database.Types.Where(x => x.Id == relatedPost.TypeId).SingleOrDefault();
        }
     
        
        return View(model);
          }
          catch
          {
            return RedirectToAction("Index","Home");
          }
    }

    [Authorize]
    [ValidateAntiForgeryToken]
    [HttpPost]
    public IActionResult Leavecomment([FromRoute] int ID, string CommentText)
    {
        try
        {

       
        Post Post = _database.Posts.Where(x=>x.Id == ID).FirstOrDefault();
        
    var httpContext = HttpContext;

    // Get the user's IP address
    var ipAddress = httpContext.Connection.RemoteIpAddress;

    // If IP address is IPv4 mapped to IPv6, convert it to IPv4
    if(ipAddress != null)
    {
    if (ipAddress.IsIPv4MappedToIPv6)
    {
        ipAddress = ipAddress.MapToIPv4();
    }
    }
    
        Comment lastComment = _database.Comments.Where(x=>x.PostId == ID ).OrderByDescending(x=>x.CommentOrder).FirstOrDefault();

        Comment newComment = new Comment();
        int LastCommentOrder= 0 ;
        if(lastComment != null)
        {
         LastCommentOrder =  (Convert.ToInt32(lastComment.CommentOrder)+1);

        }
        else{
            LastCommentOrder = 1;
        }

        ClaimsPrincipal userClaims = HttpContext.User;

        // Find the claim that contains the user ID
        Claim userIdClaim = userClaims.Claims.FirstOrDefault(c => c.Type == "UserId");

        // Get the user ID value from the claim
        string userId = userIdClaim?.Value;

        newComment.IpAddress =  ipAddress.ToString();
        newComment.CommentOrder = LastCommentOrder;
        newComment.Text = CommentText;
        newComment.PostId = ID;
        if(userIdClaim != null)
        {
        newComment.UserId = Convert.ToInt32(userId);
        }
        newComment.CreatedTime = DateTime.Now;

        _database.Comments.Add(newComment);
        _database.SaveChanges();
        

        return RedirectToAction("Postdetails", "Home", new { ID = ID });
         }
         catch
         {
            return RedirectToAction("Postdetails", "Home", new { ID = ID });

         }
    }




}

