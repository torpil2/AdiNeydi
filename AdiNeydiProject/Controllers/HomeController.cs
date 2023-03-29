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
using AdiNeydiProject.CustomClasses;


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
        model.PostList = _database.Posts.OrderByDescending(p => p.Id).ToList();
        model.CategoryList = _database.Categories.ToList();
       

        var accesskey = "AKIAROTU5G7VPCYJ3U5T";

        var SecretKey = "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";

        RegionEndpoint bucketregion = RegionEndpoint.EUNorth1;

        var s3Client = new AmazonS3Client(accesskey, SecretKey, bucketregion);

        Dictionary<int, string> Resimler = new Dictionary<int, string>();
        foreach (Post item in model.PostList)
        {

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
        }


        model.ResimList = Resimler;
        //UploadFile();
        return View(model);
    }


    [HttpPost]
    public async Task<IActionResult> PostYukle(string description,int categoryid,IFormFile file, IFormFile audiofile)
    {
      
            Post newPost = new Post();
            newPost.Title = "Boş Title";
            newPost.Description = description;
            newPost.CategoryId = categoryid;

            _database.Posts.Add(newPost);

             _database.SaveChanges();
            int id = newPost.Id;
        
  
  
        var accesskey = "AKIAROTU5G7VPCYJ3U5T";

        var SecretKey = "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";

        RegionEndpoint bucketregion = RegionEndpoint.EUNorth1;

        var bucketName = "adineydibucket";

        var s3Client = new AmazonS3Client(accesskey, SecretKey, bucketregion);

        if (file.Length > 0)
        {
            var fileTransferUtility = new TransferUtility(s3Client);
            var key = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Path.GetTempPath(), key);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var fileTransferUtilityRequest = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                FilePath = filePath,
                Key = key,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.NoACL
            };
            await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);

            //string filepath = $"https://{bucketName}.s3.eu-north-1.amazonaws.com/{key}";

            string filepath = key;
            int PostID = id;
            Picture newpicture = new Picture();
            newpicture.Path = filepath;
            newpicture.PostId = PostID;
            _database.Pictures.Add(newpicture);
            await _database.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentException("File is empty");
        }




        var s3Client2 = new AmazonS3Client(accesskey, SecretKey, bucketregion);
        if (audiofile.Length > 0)
        {
            var fileTransferUtility2 = new TransferUtility(s3Client2);
            var key2 = Guid.NewGuid().ToString() + Path.GetExtension(audiofile.FileName);
            var filePath2 = Path.Combine(Path.GetTempPath(), key2);
            using (var stream2 = new FileStream(filePath2, FileMode.Create))
            {
                await audiofile.CopyToAsync(stream2);
            }
            var fileTransferUtilityRequest2 = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                FilePath = filePath2,
                Key = key2,
                ContentType = audiofile.ContentType,
                CannedACL = S3CannedACL.NoACL
            };
            await fileTransferUtility2.UploadAsync(fileTransferUtilityRequest2);
       


            //string filepath = $"https://{bucketName}.s3.eu-north-1.amazonaws.com/{key}";

            string filepath2 = key2;
            int PostID = id;
            Audio newaudio = new Audio();
            newaudio.Path = filepath2;
            newaudio.PostId = PostID;
            _database.Audios.Add(newaudio);
            await _database.SaveChangesAsync();
        }
        else
        {
            throw new ArgumentException("File is empty");
        }




        return RedirectToAction("Index");




    }



    public async void UploadFileAsync(IFormFile file,int postid)
    {

        var accesskey = "AKIAROTU5G7VPCYJ3U5T";

        var SecretKey = "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";

        RegionEndpoint bucketregion = RegionEndpoint.EUNorth1;

        var bucketName = "adineydibucket";

        var s3Client = new AmazonS3Client(accesskey, SecretKey, bucketregion);

        if (file.Length > 0)
        {
            var fileTransferUtility = new TransferUtility(s3Client);
            var key = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(Path.GetTempPath(), key);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var fileTransferUtilityRequest = new TransferUtilityUploadRequest
            {
                BucketName = bucketName,
                FilePath = filePath,
                Key = key,
                ContentType = file.ContentType,
                CannedACL = S3CannedACL.NoACL
            };
            await fileTransferUtility.UploadAsync(fileTransferUtilityRequest);
            //return $"https://{bucketName}.s3.eu-north-1.amazonaws.com/{key}";
            string filepath = $"https://{bucketName}.s3.eu-north-1.amazonaws.com/{key}";
            int PostID = postid;
            await UploadFile(filepath, postid);
        }
        else
        {
            throw new ArgumentException("File is empty");
        }
    }





    [HttpPost]
    public async Task<IActionResult> UploadFile(string filePath,int PostID)
    {
 

        Picture newpicture = new Picture();
        newpicture.Path = filePath;
        newpicture.PostId = PostID;
        _database.Pictures.Add(newpicture);
        await _database.SaveChangesAsync();
     
        return RedirectToAction("Index");
    }

}

