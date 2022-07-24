using System;
using System.Collections.Generic;

#nullable disable

namespace FootballNews.Models
{
    public partial class Category
    {
        public Category()
        {
            News = new HashSet<News>();
        }

        public int CategoryId { get; set; }
        public string CategoryName { get; set; }

        public virtual ICollection<News> News { get; set; }
    }
}
