﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Saitynai_BeerReview.Models
{
    public class User
    {
        public long UserId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Beer> Beers { get; set; }
    }
}
