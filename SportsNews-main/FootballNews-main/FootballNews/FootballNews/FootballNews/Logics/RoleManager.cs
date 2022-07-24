using FootballNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballNews.Logics
{
    public class RoleManager
    {
        public List<Role> GetAllRoles()
        {
            using (var context = new FootballNewsContext())
            {
                return context.Roles.ToList();
            }
        }
    }
}
