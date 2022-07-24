using System;
using System.Collections.Generic;

#nullable disable

namespace FootballNews.Models
{
    public partial class Content
    {
        public int ContentId { get; set; }
        public string Content1 { get; set; }
        public int? ImageId { get; set; }

        public virtual Image Image { get; set; }
    }
}
