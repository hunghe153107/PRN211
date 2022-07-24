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
    public class AdminController : Controller
    {
        public IActionResult Error()
        {
            return View("Views/Error.cshtml");
        }

        public IActionResult ManageUser()
        {
            User CurrentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("CurrentUser"));
            if (CurrentUser.RoleId != 1 || HttpContext.Session.GetString("CurrentUser") == null)
            {
                return Error();
            }
            else
            {
                UserManager userManager = new UserManager();
                RoleManager roleManager = new RoleManager();

                List<User> users = userManager.GetAllUsers();
                ViewBag.AllUsers = users;

                List<Role> roles = roleManager.GetAllRoles();
                ViewBag.AllRoles = roles;

                ViewData["NumberAdmin"] = userManager.GetNumberUserByRole(1);
                ViewData["NumberJournalist"] = userManager.GetNumberUserByRole(2);
                ViewData["NumberReader"] = userManager.GetNumberUserByRole(3);
                ViewData["TotalUser"] = users.Count;

                return View("Views/Admin/ManageUser.cshtml");
            }

        }

        public IActionResult AddUser(string Avatar, string Username, string Email, string Password, int Role)
        {
            UserManager userManager = new UserManager();

            if (userManager.GetUserByName(Username) != null)
            {
                ViewBag.Error1 = "Tên người dùng đã được sử dụng !";
            }
            if (userManager.GetUserByEmail(Email) != null)
            {
                ViewBag.Error2 = "Địa chỉ email đã được sử dụng !";
            }
            if (userManager.GetUserByName(Username) != null || userManager.GetUserByEmail(Email) != null)
            {
                return ManageUser();
            }
            else
            {
                userManager.AddUser(Avatar, Username, Email, Password, Role);
                return RedirectToAction("ManageUser", "Admin");
            }

        }

        public IActionResult DeleteUser(int UserId)
        {
            UserManager userManager = new UserManager();
            NewsManager newsManager = new NewsManager();
            ImageManager imageManager = new ImageManager();
            ContentManager contentManager = new ContentManager();
            CommentManager commentManager = new CommentManager();

            User CurrentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("CurrentUser"));
            if (CurrentUser.RoleId != 1)
            {
                return Error();
            }
            else
            {
                User user = userManager.GetUserById(UserId);
                if (user.RoleId == 2)
                {
                    using (var context = new FootballNewsContext())
                    {
                        List<News> news = newsManager.GetAllNewsByUserId(UserId);
                        for (int i = 0; i < news.Count; i++)
                        {
                            commentManager.DeleteCommentsById(news[i].NewsId);
                            contentManager.DeleteContentsById(news[i].NewsId);
                            imageManager.DeleteImagesById(news[i].NewsId);
                            newsManager.DeleteNewsById(news[i].NewsId);
                        }

                    }
                    userManager.DeleteUser(UserId);
                    return RedirectToAction("ManageUser", "Admin");
                }
                else
                {
                    userManager.DeleteUser(UserId);
                    return RedirectToAction("ManageUser", "Admin");
                }
            }

        }

        public IActionResult SetRoleUser(int SetRole, int UserId)
        {
            UserManager userManager = new UserManager();

            User CurrentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("CurrentUser"));
            if (CurrentUser.RoleId != 1)
            {
                return Error();
            }
            else
            {
                userManager.SetRole(SetRole, UserId);
                return RedirectToAction("ManageUser", "Admin");
            }

        }

        public IActionResult ManageNews(int CategoryId, int Page)
        {
            User CurrentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("CurrentUser"));
            if (CurrentUser.RoleId != 1)
            {
                return Error();
            }
            else
            {
                NewsManager newsManager = new NewsManager();
                UserManager userManager = new UserManager();
                CategoryManager categoryManager = new CategoryManager();
                int PageSize = 10;
                ViewBag.AllNews = newsManager.GetAllNewsByCategoryId(CategoryId, (Page - 1) * PageSize + 1, PageSize);
                ViewBag.AllUsers = userManager.GetAllUsers();
                ViewBag.AllCategories = categoryManager.GetAllCategories();
                int TotalNews = newsManager.GetNumberOfNews(CategoryId);
                int TotalPage = TotalNews / PageSize;

                if (TotalNews % PageSize != 0)
                {
                    TotalPage++;
                }

                ViewData["TotalPage"] = TotalPage;
                ViewData["CurrentPage"] = Page;
                ViewData["CurrentCategory"] = 0;

                return View("Views/Admin/ManageNews.cshtml");

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

            return RedirectToAction("ManageNews", "Admin");
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

            return RedirectToAction("ManageNews", "Admin");
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

            return RedirectToAction("ManageNews", "Admin");
        }




    }
}
