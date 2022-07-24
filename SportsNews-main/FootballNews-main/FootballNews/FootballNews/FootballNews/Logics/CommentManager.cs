using FootballNews.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballNews.Logics
{
    public class CommentManager
    {
        public List<Comment> GetAllComments(int NewsId)
        {
            using (var context = new FootballNewsContext())
            {
                return context.Comments.Where(x => x.NewsId == NewsId)
                    .OrderByDescending(x => x.Time).ToList();

            }
        }

        public void AddComment(int newsId, int userId, string comment)
        {
            using (var context = new FootballNewsContext())
            {
                Comment c = new Comment
                {
                    UserId = userId,
                    NewsId = newsId,
                    Content = comment
                };
                context.Add(c);
                context.SaveChanges();
            }
        }

        public void DeleteComment(int CommentId)
        {
            using (var context = new FootballNewsContext())
            {
                Comment c = context.Comments.Where(x => x.CommentId == CommentId).FirstOrDefault();
                context.Remove(c);
                context.SaveChanges();
            }
        }

        public void DeleteCommentsById(int NewsId)
        {
            using (var context = new FootballNewsContext())
            {
                List<Comment> cm = context.Comments.Where(x => x.NewsId == NewsId).ToList();
                if (cm.Count >= 1)
                {
                    foreach (Comment c in cm)
                    {
                        context.Remove(c);
                        context.SaveChanges();
                    }
                }
            }
        }
    }
}
