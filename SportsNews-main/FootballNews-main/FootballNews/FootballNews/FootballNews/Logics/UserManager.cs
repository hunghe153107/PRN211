using FootballNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballNews.Logics
{
    public class UserManager
    {
        public User GetUserByName(string Username)
        {
            using (var context = new FootballNewsContext())
            {
                return context.Users.Where(x => x.UserName == Username).FirstOrDefault();
            }
        }

        public int GetNumberUserByRole(int RoleId)
        {
            using (var context = new FootballNewsContext())
            {
                return context.Users.Where(x => x.RoleId == RoleId).Count();
            }
        }

        public User GetUserByEmail(string Email)
        {
            using (var context = new FootballNewsContext())
            {
                return context.Users.Where(x => x.Email == Email).FirstOrDefault();
            }
        }

        public User CheckLogin(string Username, string Password)
        {
            using (var context = new FootballNewsContext())
            {
                return context.Users.Where(x => x.UserName == Username && x.Password == Password).FirstOrDefault();
            }
        }

        public void InsertUser(string Username, string Email, string Password, string Code)
        {
            using (var context = new FootballNewsContext())
            {
                User user = new User
                {
                    UserName = Username,
                    Email = Email,
                    Password = Password,
                    Avatar = ""
                    ,
                    RoleId = 3,
                    Code = Code,
                    Status = false
                };
                context.Add(user);
                context.SaveChanges();
            }
        }

        public User GetUserByRoleId(int RoleId)
        {
            using (var context = new FootballNewsContext())
            {
                return context.Users.Where(x => x.RoleId == RoleId).FirstOrDefault();
            }
        }

        public void AddUser(string Avatar, string Username, string Email, string Password, int RoleId)
        {
            using (var context = new FootballNewsContext())
            {
                User u = new User
                {
                    UserName = Username,
                    Email = Email,
                    Password = Password,
                    Avatar = Avatar,
                    RoleId = RoleId,
                    Code = null,
                    Status = true
                };
                context.Add(u);
                context.SaveChanges();
            }
        }

        public void DeleteUser(int UserId)
        {
            using (var context = new FootballNewsContext())
            {
                User u = context.Users.Where(x => x.UserId == UserId).FirstOrDefault();
                context.Remove(u);
                context.SaveChanges();
            }
        }

        public User GetUserById(int UserId)
        {
            using (var context = new FootballNewsContext())
            {
                return context.Users.Where(x => x.UserId == UserId).FirstOrDefault();
            }
        }

        public void SetRole(int SetRole, int UserId)
        {
            using (var context = new FootballNewsContext())
            {
                User CurrentUser = context.Users.Where(x => x.UserId == UserId).FirstOrDefault();
                CurrentUser.RoleId = SetRole;
                context.SaveChanges();
            }
        }

        public User GetUserByAuthorId(int AuthorId)
        {
            using (var context = new FootballNewsContext())
            {
                return context.Users.Where(x => x.UserId == AuthorId).FirstOrDefault();
            }
        }

        public User CheckCode(string Email,string Code)
        {
            using (var context = new FootballNewsContext())
            {
                return context.Users.Where(x => x.Email == Email && x.Code == Code).FirstOrDefault();
            }
        }

        public void UpdateStatus(string Email, bool Status)
        {

            using (var context = new FootballNewsContext())
            {
                User CurrentUser = context.Users.Where(x => x.Email == Email).FirstOrDefault();
                CurrentUser.Status = Status;
                context.SaveChanges();
            }
        }

        public void UpdateCode(string Email, string Code)
        {
            using (var context = new FootballNewsContext())
            {
                User CurrentUser = context.Users.Where(x => x.Email == Email).FirstOrDefault();
                CurrentUser.Code = Code;
                context.SaveChanges();
            }
        }

        public void UpdatePassword(string Email, string Password)
        {
            using (var context = new FootballNewsContext())
            {
                User CurrentUser = context.Users.Where(x => x.Email == Email).FirstOrDefault();
                CurrentUser.Password = Password;
                context.SaveChanges();
            }
        }

        public List<User> GetAllUsers()
        {
            using (var context = new FootballNewsContext())
            {
                return context.Users.ToList();
            }
        }

        public void UpdateUserProfile(string Avatar, string Username, string Email)
        {
            using (var context = new FootballNewsContext())
            {
                User CurrentUser = context.Users.Where(x => x.Email == Email).FirstOrDefault();
                CurrentUser.UserName = Username;
                CurrentUser.Avatar = Avatar;
                context.SaveChanges();
            }
        }

        public List<User> GetAllUsersByName(string Value)
        {
            using (var context = new FootballNewsContext())
            {
                return context.Users.Where(x=> x.UserName.Equals(Value)).ToList();
            }
        }
    }
}
