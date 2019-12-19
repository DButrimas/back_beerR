using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Saitynai_BeerReview.Models
{
    public class Beer
    {
        public long BeerId { get; set; }

        [Required]
        [StringLength(60)]
        public string Brand { get; set; }
        [Required]
        public double Alcohol { get; set; }
        [Required]
        public double Size { get; set; }
        [Required]
        public double Price { get; set; }
        public string LogoUrl { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        

        public long UserId { get; set; }
        public User User { get; set; }

    }
}
