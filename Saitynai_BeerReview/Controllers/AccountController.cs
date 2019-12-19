using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Saitynai_BeerReview.Models;
using Saitynai_BeerReview.ViewModels;

namespace Saitynai_BeerReview.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private readonly UserContext db;

        public AccountController(UserContext context)
        {
            db = context;
        }

        [HttpPost("login")]
        //[HttpPost]
        // [ValidateAntiForgeryToken]
        //[Route("api/login")]
        public async Task<IActionResult> Login(LoginModel model)//UserModel model
        {
            var asd = 0;


            if (ModelState.IsValid)
            {
                User user = db.Users.Where(u => u.Email == model.Email && u.Password == model.Password).FirstOrDefault();

                if (user != null)
                {
                    await Authenticate(user);

                    //Response.StatusCode = 200;
                    //await Response.WriteAsync("Loged In");
                }
                Response.StatusCode = 400;
                await Response.WriteAsync("Invalid username or password.");
            }
            return BadRequest("");

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User _user =  db.Users.FirstOrDefault(u => u.Email == model.Email);
                if (_user == null)
                {
                    User user = new User { Email = model.Email, Password = model.Passwrod, Role = "User" };
                    db.Users.Add(user);
                    db.SaveChanges();
                    await Authenticate(user);
                }
                else
                {
                    ModelState.AddModelError("", "Incorect Email or password");
                }
            }
            return BadRequest("");
        }


        private async Task Authenticate(User user)
        {

            var Claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role),

                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(Claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            var now = DateTime.UtcNow;

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: Claims,
                    expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                username = user.Email

            };
            Response.StatusCode = 200;
            Response.ContentType = "application/json";
            await Response.WriteAsync(JsonConvert.SerializeObject(response, new JsonSerializerSettings { Formatting = Formatting.Indented }));
        }




    }
}