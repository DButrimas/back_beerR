using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saitynai_BeerReview.Models;

namespace Saitynai_BeerReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BeersController : ControllerBase
    {
        private readonly UserContext _context;

        public BeersController(UserContext context)
        {
            _context = context;
        }

        // GET: api/Beers
        [HttpGet]
     //   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<Beer>>> GetBeers()
        {
            return await _context.Beers.ToListAsync();
        }

        // GET: api/Beers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Beer>> GetBeer(long id)
        {
            var beer = await _context.Beers.FindAsync(id);

            if (beer == null)
            {
                return NotFound("");
            }

            return beer;
        }

        // PUT: api/Beers/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBeer(long id, Beer beer)
        {
            if (id != beer.BeerId)
            {
                return BadRequest("");
            }

            _context.Entry(beer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BeerExists(id))
                {
                    return NotFound("");
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Beers
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
   [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        public async Task<ActionResult<Beer>> PostBeer(Beer beer)
        {
            string e = User.Identity.Name;
            User u = _context.Users.Where(x => x.Email == e).FirstOrDefault();
            beer.UserId = u.UserId;
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();

            return StatusCode(201,"");
        }

        // DELETE: api/Beers/5

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost("delete/{id}")]
        public async Task<ActionResult<Beer>> DeleteBeer(long id)
        {
            var beer = await _context.Beers.FindAsync(id);
            if (beer == null)
            {
                return NotFound("");
            }

            _context.Beers.Remove(beer);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool BeerExists(long id)
        {
            return _context.Beers.Any(e => e.BeerId == id);
        }




        //////
        ///
        ///                            USER/BEER
        ///                            
        // GET: api/Users
        [HttpGet("{beerId}/comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetUserBeers(long beerId)
        {
            return await _context.Comments.Where(x => x.BeerId == beerId).ToListAsync();
        }


        // GET: api/Users/5
        [HttpGet("{beerId}/Comments/{commentId}")]
        public async Task<ActionResult<Comment>> GetUserBeer(long commentId, long beerId)
        {
            var comment = await _context.Comments.Where(x => x.BeerId == beerId && x.CommentId == commentId).FirstAsync();

            if (comment == null)
            {
                return NotFound("");
            }

            return comment;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{beerId}/Comments/{commentId}")]
        public async Task<IActionResult> PutUserBeer(long commentId, long beerId, Comment comment)
        {
            if (commentId != comment.CommentId)
            {
                return BadRequest("");
            }

            _context.Entry(comment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(commentId))
                {
                    return NotFound("");
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost("{beerId}/comments/")]
        public async Task<ActionResult<Comment>> PostUserBeer(long beerId, Comment comment)
        {
            comment.BeerId = beerId;
            comment.Date = DateTime.Now.ToString();
       
            if (comment == null)
            {
                return BadRequest("");
            }
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return StatusCode(201,"");
        }


        // DELETE: api/Users/5
        [HttpDelete("{beerId}/comments/{commentId}")]
        public async Task<ActionResult<Comment>> DeleteUserBeer(long beerId, long commentId)
        {
            var comment = await _context.Comments.Where(x => x.CommentId == commentId && x.BeerId == beerId).FirstOrDefaultAsync();
            if (comment == null)
            {
                return NotFound("");
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool CommentExists(long id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}