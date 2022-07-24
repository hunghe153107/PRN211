using FootballNews.Logics;
using FootballNews.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace FootballNews.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {

            CategoryManager categoryManager = new CategoryManager();
            ViewBag.Top4Categories = categoryManager.GetTopCategories();
            ViewBag.AllOtherCategories = categoryManager.GetAllOtherCategories();

            NewsManager newsManager = new NewsManager();
            ViewBag.Top5LatestNews = newsManager.GetTop5LatestNews();
            ViewBag.Top5LatestTransferNews = newsManager.GetTop5LatestTransferNews();
            return View();
        }

        public IActionResult About()
        {
            CategoryManager categoryManager = new CategoryManager();
            ViewBag.Top4Categories = categoryManager.GetTopCategories();
            ViewBag.AllOtherCategories = categoryManager.GetAllOtherCategories();

            NewsManager newsManager = new NewsManager();
            ViewBag.Top5LatestNews = newsManager.GetTop5LatestNews();
            ViewBag.Top5LatestTransferNews = newsManager.GetTop5LatestTransferNews();

            return View("Views/Home/About.cshtml");
        }

        //Contact Screen
        [HttpGet]
        public IActionResult Contact()
        {
            CategoryManager categoryManager = new CategoryManager();
            ViewBag.Top4Categories = categoryManager.GetTopCategories();
            ViewBag.AllOtherCategories = categoryManager.GetAllOtherCategories();

            NewsManager newsManager = new NewsManager();
            ViewBag.Top5LatestNews = newsManager.GetTop5LatestNews();
            ViewBag.Top5LatestTransferNews = newsManager.GetTop5LatestTransferNews();

            return View("Views/Home/Contact.cshtml");
        }

      
    }
}
