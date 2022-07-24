using System;
using System.Collections.Generic;

#nullable disable

namespace FootballNews.Models
{
    public partial class Image
    {
        public Image()
        {
            Contents = new HashSet<Content>();
        }

        public int ImageId { get; set; }
        public string ImageUrl { get; set; }
        public int? NewsId { get; set; }

        public virtual News News { get; set; }
        public virtual ICollection<Content> Contents { get; set; }
    }
}
