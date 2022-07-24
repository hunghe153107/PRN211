using FootballNews.Logics;
using FootballNews.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballNews.Controllers
{
    public class AuthorController : Controller
    {
        public IActionResult Error()
        {
            return View("Views/Error.cshtml");
        }

        public IActionResult MyNews(int CategoryId, int Page)
        {
            User CurrentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("CurrentUser"));
            if (CurrentUser.RoleId != 2 || HttpContext.Session.GetString("CurrentUser") == null)
            {
                return Error();
            }
            else
            {
                NewsManager newsManager = new NewsManager();
                UserManager userManager = new UserManager();
                CategoryManager categoryManager = new CategoryManager();
                int PageSize = 10;
                ViewBag.AllNews = newsManager.GetAllNewsByAuthorId(CurrentUser.UserId, CategoryId, (Page - 1) * PageSize + 1, PageSize);
                ViewBag.AllCategories = categoryManager.GetAllCategories();
                int TotalNews = newsManager.GetNumberOfNewsByAuthorId(CurrentUser.UserId);
                int TotalPage = TotalNews / PageSize;

                if (TotalNews % PageSize != 0)
                {
                    TotalPage++;
                }

                ViewData["TotalPage"] = TotalPage;
                ViewData["CurrentPage"] = Page;
                ViewData["CurrentCategory"] = 0;

                return View("Views/Journalist/MyNews.cshtml");

            }
        }

        [HttpGet]
        public IActionResult AddNews()
        {
            CategoryManager categoryManager = new CategoryManager();
            ViewBag.AllCategories = categoryManager.GetAllCategories();
            return View("Views/Admin/AddNews.cshtml");
        }


        [HttpPost]
        public IActionResult AddNews(string Title, string ShortDescription, string Thumbnail, int Category, string[] Image, string[] Content)
        {
            User CurrentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("CurrentUser"));

            NewsManager newsManager = new NewsManager();
            ImageManager imageManager = new ImageManager();
            ContentManager contentManager = new ContentManager();

            newsManager.AddNews(CurrentUser.UserId, Title, ShortDescription, Thumbnail, Category);
            News news = newsManager.GetLatestNews();
            for (int i = 0; i < Image.Length; i++)
            {
                imageManager.AddImages(news.NewsId, Image[i]);
                Image image = imageManager.GetLastImage();
                contentManager.AddContents(image.ImageId, Content[i]);
            }

            return RedirectToAction("MyNews", "Author");
        }

        public IActionResult DeleteNews(int NewsId)
        {
            NewsManager newsManager = new NewsManager();
            ImageManager imageManager = new ImageManager();
            ContentManager contentManager = new ContentManager();
            CommentManager commentManager = new CommentManager();

            commentManager.DeleteCommentsById(NewsId);
            contentManager.DeleteContentsById(NewsId);
            imageManager.DeleteImagesById(NewsId);
            newsManager.DeleteNewsById(NewsId);

            return RedirectToAction("MyNews", "Author");
        }

        [HttpGet]
        public IActionResult UpdateNews(int NewsId)
        {
            CategoryManager categoryManager = new CategoryManager();
            NewsManager newsManager = new NewsManager();
            ImageManager imageManager = new ImageManager();
            ContentManager contentManager = new ContentManager();

            List<Category> categories = categoryManager.GetAllCategories();
            ViewBag.AllCategories = categories;

            News news = newsManager.GetNewsById(NewsId);
            ViewData["NewsId"] = news.NewsId;
            ViewData["Title"] = news.Title;
            ViewData["ShortDescription"] = news.ShortDescription;
            ViewData["Thumbnail"] = news.Thumbnail;
            ViewData["CategoryId"] = news.CategoryId;
            ViewData["DatePublished"] = news.DatePublished;

            List<Image> images = imageManager.GetAllImagesByNewsId(NewsId);
            ViewBag.GetImages = images;

            List<Content> contents = contentManager.GetAllContents();
            ViewBag.GetContents = contents;

            return View("Views/Admin/UpdateNews.cshtml");
        }

        [HttpPost]
        public IActionResult UpdateNews(int NewsId, string Title, string ShortDescription, string Thumbnail, string ThumbnailU,
            int Category, string[] ImageUrl, string[] ImageUrlU, int[] ImageId, string[] Content)
        {
            NewsManager newsManager = new NewsManager();
            ImageManager imageManager = new ImageManager();
            ContentManager contentManager = new ContentManager();

            if (ThumbnailU != null)
            {
                newsManager.UpdateNews(NewsId, Title, ShortDescription, ThumbnailU, Category);
            }
            else
            {
                newsManager.UpdateNews(NewsId, Title, ShortDescription, Thumbnail, Category);
            }


            for (int i = 0; i < ImageId.Length; i++)
            {
                if (ImageUrlU[i] != null)
                {
                    imageManager.UpdateImages(NewsId, ImageUrlU[i]);
                }
                else
                {
                    imageManager.UpdateImages(NewsId, ImageUrl[i]);
                }

                using (var context = new FootballNewsContext())
                {
                    List<Content> contents = context.Contents.Where(x => x.ImageId == ImageId[i]).ToList();
                    for (int j = 0; j < contents.Count; j++)
                    {
                        contents[j].Content1 = Content[j];
                        context.SaveChanges();
                    }
                }
            }

            return RedirectToAction("MyNews", "Author");
        }
    }
}
