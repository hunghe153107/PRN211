using System;
using System.Collections.Generic;

#nullable disable

namespace FootballNews.Models
{
    public partial class Comment
    {
        public int CommentId { get; set; }
        public int? UserId { get; set; }
        public int? NewsId { get; set; }
        public string Content { get; set; }

        public DateTime? Time { get; set; }

        public virtual News News { get; set; }
        public virtual User User { get; set; }
    }
}
