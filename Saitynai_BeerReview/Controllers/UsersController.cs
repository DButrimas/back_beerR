using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Saitynai_BeerReview.Models;

namespace Saitynai_BeerReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound("");
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(long id, User user)
        {
            if (id != user.UserId)
            {
                return BadRequest("");
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
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
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            if (user == null)
            {
                return BadRequest("");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            

            return StatusCode(201,"");
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(long id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound("");
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool UserExists(long id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }









        ///                            USER/BEER
        ///                            
                // GET: api/Users
        [HttpGet("{userId}/beers")]
        public async Task<ActionResult<IEnumerable<Beer>>> GetUserBeers(long userId)
        {
            return await _context.Beers.Where(x => x.User.UserId == userId).ToListAsync();
        }


        // GET: api/Users/5
        [HttpGet("{userId}/beers/{beerId}")]
        public async Task<ActionResult<Beer>> GetUserBeer(long userId, long beerId)
        {
            var beer = await _context.Beers.Where(x => x.UserId == userId && x.BeerId == beerId).FirstAsync();

            if (beer == null)
            {
                return NotFound("");
            }

            return beer;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{userId}/beers/{beerId}")]
        public async Task<IActionResult> PutUserBeer(long userId,long beerId ,Beer beer)
        {
            if (beerId != beer.BeerId)
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
                if (!BeerExists(beerId))
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
        [HttpPost("{userId}/beers/")]
        public async Task<ActionResult<User>> PostUserBeer(long userId,Beer beer)
        {
            if (beer == null)
            {
                return BadRequest("");
            }
            _context.Beers.Add(beer);
            await _context.SaveChangesAsync();

            return StatusCode(201,"");
        }

        // DELETE: api/Users/5
        [HttpDelete("{userid}/beers/{beerid}")]
        public async Task<ActionResult<Beer>> DeleteUserBeer(long userId, long beerId)
        {
            var beer = await _context.Beers.Where(x => x.BeerId == beerId && x.UserId == userId).FirstOrDefaultAsync();
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




        //// USER/BEER/COMMENTS
        ///

        [HttpGet("{userId}/beers/{beerId}/comments")]
        public async Task<ActionResult<IEnumerable<Comment>>> GetUserBeersComment(long userId, long beerId)
        {////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            List<Comment> asd = _context.Comments.Where(x => x.UserId == userId && x.BeerId == beerId).Include(x => x.User).Where(x => x.UserId == userId).ToList();
            foreach (var item in asd)
            {
                item.User = null;
              
            }
            return asd;

        }


        // GET: api/Users/5
        [HttpGet("{userId}/beers/{beerId}/comments/{commentId}")]
        public async Task<ActionResult<Comment>> GetUserBeerComment(long userId, long beerId, long commentId)
        {
            Comment asd = (Comment)_context.Comments.Where(x => x.UserId == userId && x.BeerId == beerId && x.CommentId == commentId).Include(x => x.User).Where(x => x.UserId == userId).FirstOrDefault();

            
            if (asd == null)
            {
                
                return NotFound("");
            }
            asd.User = null;

            return asd;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{userId}/beers/{beerId}/comments/{commentId}")]
        public async Task<IActionResult> PutUserBeerComment(long userId, long beerId, Comment comment, long commentId)
        {
            if (commentId != comment.CommentId || comment.BeerId != beerId || comment.UserId != userId)
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
                if (!BeerExists(beerId))
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
        [HttpPost("{userId}/beers/{beerId}/comments")]
        public async Task<ActionResult<User>> PostUserBeerComment(long userId, long beerId, Comment comment)
        {
            if (comment == null)
            {
                return BadRequest("");
            }
            comment.UserId = userId;
            comment.BeerId = beerId;
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return StatusCode(201,"");
        }

        // DELETE: api/Users/5
        [HttpDelete("{userid}/beers/{beerid}/comments/{commentId}")]
        public async Task<ActionResult<Comment>> DeleteUserBeerComment(long userId, long beerId, long commentId)
        {
            Comment asd = (Comment)_context.Comments.Where(x => x.UserId == userId && x.BeerId == beerId && x.CommentId == commentId).Include(x => x.User).Where(x => x.UserId == userId).FirstOrDefault();
            if (asd == null)
            {
                return NotFound("");
            }

            _context.Comments.Remove(asd);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool CommentExists(long id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }

    }
}
