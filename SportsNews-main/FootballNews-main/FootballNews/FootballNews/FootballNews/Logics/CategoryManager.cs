using FootballNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballNews.Logics
{
    public class CategoryManager
    {
        public List<Category> GetTopCategories()
        {
            using (var context = new FootballNewsContext())
            {
                return context.Categories.
                    Where(x => x.CategoryId == 1 || x.CategoryId == 2)
                    .OrderBy(x => x.CategoryId).ToList();
            }
        }

        public List<Category> GetAllOtherCategories()
        {
            using (var context = new FootballNewsContext())
            {
                return context.Categories.
                    Where(x => x.CategoryId != 1 && x.CategoryId != 2 && x.CategoryId != 3 && x.CategoryId != 8)
                    .OrderBy(x => x.CategoryId).ToList();
            }
        }

        public Category GetCategoryById(int CategoryId)
        {
            using (var context = new FootballNewsContext())
            {
                return context.Categories.Where(x => x.CategoryId == CategoryId).FirstOrDefault();
            }
        }

        public dynamic GetAllCategories()
        {
            using (var context = new FootballNewsContext())
            {
                return context.Categories.ToList();
            }
        }
    }
}
