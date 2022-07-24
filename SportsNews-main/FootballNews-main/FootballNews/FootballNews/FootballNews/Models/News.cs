using System;
using System.Collections.Generic;

#nullable disable

namespace FootballNews.Models
{
    public partial class News
    {
        public News()
        {
            Comments = new HashSet<Comment>();
            Images = new HashSet<Image>();
        }

        public int NewsId { get; set; }
        public int? AuthorId { get; set; }
        public string Title { get; set; }
        public string ShortDescription { get; set; }
        public string Thumbnail { get; set; }
        public int? CategoryId { get; set; }
        public DateTime? DatePublished { get; set; }
        public bool? Status { get; set; }

        public virtual User Author { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Image> Images { get; set; }
    }
}
