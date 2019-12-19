using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Saitynai_BeerReview.Models
{
    public class Comment
    {
        public long CommentId { get; set; }
      
       // public long BeerId { get; set; }
     //   public Beer Beer { get; set; }
     public long BeerId { get; set; }
        public long? UserId { get; set; }
        public User? User { get; set; }
        public string Text { get; set; }
        public string Date { get; set; }
    }
}
