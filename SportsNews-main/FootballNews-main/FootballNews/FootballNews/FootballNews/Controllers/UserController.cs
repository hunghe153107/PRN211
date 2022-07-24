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
    public class UserController : Controller
    {

        //Login Screen
        [HttpGet]
        public IActionResult Login()
        {
            return View("Views/User/Login.cshtml");
        }

        //Login Action
        [HttpPost]
        public IActionResult Login(string Username, string Password)
        {
            UserManager userManager = new UserManager();

            if (userManager.GetUserByName(Username) == null)
            {
                ViewBag.Error = "Tên người dùng không tồn tại !";
                return Login();
            }
            else
            {
                if (userManager.CheckLogin(Username, Password) == null)
                {
                    ViewBag.Error = "Mật khẩu không chính xác !";
                    return Login();
                }
                else
                {
                    if (userManager.GetUserByName(Username).Status == false)
                    {
                        HttpContext.Session.SetString("CurrentEmail", userManager.GetUserByName(Username).Email);
                        return RedirectToAction("Verify", "User");
                    }
                    else
                    {
                        HttpContext.Session.SetString("CurrentUser", JsonConvert.SerializeObject(userManager.GetUserByName(Username)));
                        return RedirectToAction("Index", "Home");
                    }
                }
            }

        }

        //Logout Action
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("CurrentUser");
            return RedirectToAction("Index", "Home");
        }

        //Register Screen
        [HttpGet]
        public IActionResult Register()
        {
            return View("Views/User/Register.cshtml");
        }

        //Register Action
        [HttpPost]
        public IActionResult Register(string Username, string Email, string Password, string ConfirmPassword)
        {
            UserManager userManager = new UserManager();

            User CheckUserName = userManager.GetUserByName(Username);
            if (CheckUserName != null)
            {
                ViewBag.Error1 = "Tên người dùng đã được sử dụng !";
            }

            User CheckEmail = userManager.GetUserByEmail(Email);
            if (CheckEmail != null)
            {
                ViewBag.Error2 = "Địa chỉ email đã được sử dụng !";
            }

            if (!Password.Equals(ConfirmPassword))
            {
                ViewBag.Error3 = "Mật khẩu và xác nhận mật khẩu không khớp !";
            }

            if (CheckUserName != null || CheckEmail != null || !Password.Equals(ConfirmPassword))
            {
                return Register();
            }
            else
            {
                EmailSender emailSender = new EmailSender();
                String Code = emailSender.GenerateRandomNumber();

                userManager.InsertUser(Username, Email, Password, Code);

                String HtmlContent = "<h2>Xin chào " + Email + " ,</h2>" +
                    "<p>Chúng tôi đã gửi 1 đoạn mã đến Email của bạn.<br><br>" +
                    "Hãy sử dụng mã này Để xác nhận tài khoản.<br><br>" +
                    "Mã xác nhận : <span style='font - weight: bold; '>" + Code + "</span><br><br>" +
                    "Vui lòng không chia sẻ mã này với bất kì ai.</p>";

                string FromEmail = "quizpracticeg6@gmail.com";
                string GetPassword = "mrxexghqvwyekhqk";

                HttpContext.Session.SetString("CurrentEmail", Email);

                emailSender.SendEmail(FromEmail, GetPassword, Email, "Xác Nhận Tài Khoản", HtmlContent);

                return RedirectToAction("Verify", "User");
            }

        }

        //Verify Screen
        [HttpGet]
        public IActionResult Verify()
        {
            return View("Views/User/Verify.cshtml");
        }

        //Verify Action
        [HttpPost]
        public IActionResult Verify(string Code)
        {
            UserManager userManager = new UserManager();

            string CurrentEmail = HttpContext.Session.GetString("CurrentEmail");
            string EmailFG = HttpContext.Session.GetString("EmailFG");

            if (CurrentEmail != null)
            {
                User CheckCode = userManager.CheckCode(CurrentEmail, Code);
                if (CheckCode == null)
                {
                    ViewBag.Error = "Mã xác nhận không chính xác !";
                    return Verify();
                }
                else
                {
                    userManager.UpdateStatus(CurrentEmail, true);
                    userManager.UpdateCode(CurrentEmail, null);
                    HttpContext.Session.Remove("CurrentEmail");
                    return RedirectToAction("Login", "User");
                }
            }

            if (EmailFG != null)
            {
                User CheckCode = userManager.CheckCode(EmailFG, Code);
                if (CheckCode == null)
                {
                    ViewBag.Error = "Mã xác nhận không chính xác !";
                    return Verify();
                }
                else
                {
                    return RedirectToAction("ChangePassword", "User");
                }
            }

            return Verify();

        }

        //Resend Code Action
        public IActionResult Resend()
        {
            UserManager userManager = new UserManager();

            EmailSender emailSender = new EmailSender();
            string CurrentEmail = HttpContext.Session.GetString("CurrentEmail");
            string NewCode = emailSender.GenerateRandomNumber();
            String HtmlContent = "<h2>Xin chào " + CurrentEmail + " ,</h2>" +
                    "<p>Chúng tôi đã gửi 1 đoạn mã đến Email của bạn.<br><br>" +
                    "Hãy sử dụng mã này Để xác nhận tài khoản.<br><br>" +
                    "Mã xác nhận : <span style='font - weight: bold; '>" + NewCode + "</span><br><br>" +
                    "Vui lòng không chia sẻ mã này với bất kì ai.</p>";

            string FromEmail = "quizpracticeg6@gmail.com";
            string GetPassword = "mrxexghqvwyekhqk";

            emailSender.SendEmail(FromEmail, GetPassword, CurrentEmail, "Xác Nhận Tài Khoản", HtmlContent);
            userManager.UpdateCode(CurrentEmail, NewCode);

            return RedirectToAction("Verify", "User");

        }

        //Forgot Password Screen
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View("Views/User/ForgotPassword.cshtml");
        }

        //Forgot Password Action
        [HttpPost]
        public IActionResult ForgotPassword(string Email)
        {
            UserManager userManager = new UserManager();

            User CheckEmail = userManager.GetUserByEmail(Email);
            if (CheckEmail == null)
            {
                ViewBag.Error = "Email không tồn tại trong hệ thống !";
                return ForgotPassword();
            }
            else
            {
                EmailSender emailSender = new EmailSender();
                string NewCode = emailSender.GenerateRandomNumber();
                String HtmlContent = "<h2>Xin chào " + Email + " ,</h2>" +
                        "<p>Chúng tôi đã gửi 1 đoạn mã đến Email của bạn.<br><br>" +
                        "Hãy sử dụng mã này Để xác nhận và đổi mật khẩu của bạn.<br><br>" +
                        "Mã xác nhận : <span style='font - weight: bold; '>" + NewCode + "</span><br><br>" +
                        "Vui lòng không chia sẻ mã này với bất kì ai.</p>";

                string FromEmail = "quizpracticeg6@gmail.com";
                string GetPassword = "mrxexghqvwyekhqk";

                emailSender.SendEmail(FromEmail, GetPassword, Email, "Đổi Mật Khẩu", HtmlContent);
                userManager.UpdateCode(Email, NewCode);
                HttpContext.Session.SetString("EmailFG", Email);

                return RedirectToAction("Verify", "User");
            }

        }

        //Change Password Screen
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View("Views/User/ChangePassword.cshtml");
        }

        //Change Password Action
        [HttpPost]
        public IActionResult ChangePassword(string Email, string Password, string ConfirmPassword)
        {
            UserManager userManager = new UserManager();

            if (!Password.Equals(ConfirmPassword))
            {
                ViewBag.Error = "Mật khẩu không khớp với nhau !";
                return ChangePassword();
            }
            else
            {
                userManager.UpdatePassword(Email, Password);
                userManager.UpdateCode(Email, "");
                HttpContext.Session.Remove("EmailFG");
                return RedirectToAction("Login", "User");
            }

        }

        //User Profile Screen
        [HttpGet]
        public IActionResult UserProfile()
        {
            return View("Views/User/UserProfile.cshtml");
        }

        //User Profile Action
        [HttpPost]
        public IActionResult UserProfile(string Username, string Avatar, string AvatarU)
        {
            UserManager userManager = new UserManager();

            User CurrentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("CurrentUser"));

            if (userManager.GetUserByName(Username) == null || Username.Equals(CurrentUser.UserName))
            {
                if (AvatarU != null)
                {
                    userManager.UpdateUserProfile(AvatarU, Username, CurrentUser.Email);

                }
                else
                {
                    userManager.UpdateUserProfile(Avatar, Username, CurrentUser.Email);
                }
                User ChangeUser = userManager.CheckLogin(Username, CurrentUser.Password);
                HttpContext.Session.SetString("CurrentUser", JsonConvert.SerializeObject(ChangeUser));
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Tên người dùng đã tồn tại !";
                return UserProfile();
            }

        }

        //Change New Password Screen
        [HttpGet]
        public IActionResult ChangeNewPassword()
        {
            return View("Views/User/ChangeNewPassword.cshtml");
        }

        //Change New Password Action
        [HttpPost]
        public IActionResult ChangeNewPassword(string OldPassword, string NewPassword, string ConfirmPassword)
        {
            UserManager userManager = new UserManager();

            User CurrentUser = JsonConvert.DeserializeObject<User>(HttpContext.Session.GetString("CurrentUser"));

            if (!OldPassword.Equals(CurrentUser.Password))
            {
                ViewBag.Error1 = "Mật khẩu cũ không chính xác !";
            }

            if (!NewPassword.Equals(ConfirmPassword))
            {
                ViewBag.Error2 = "Mật khẩu mới và xác nhận không khớp với nhau !";
            }

            if (!OldPassword.Equals(CurrentUser.Password) || !NewPassword.Equals(ConfirmPassword))
            {
                return ChangeNewPassword();
            }
            else
            {
                userManager.UpdatePassword(CurrentUser.Email, NewPassword);
                User ChangeUser = userManager.CheckLogin(CurrentUser.UserName, NewPassword);
                HttpContext.Session.SetString("CurrentUser", JsonConvert.SerializeObject(ChangeUser));
                return RedirectToAction("Index", "Home");
            }
        }

    }
}
