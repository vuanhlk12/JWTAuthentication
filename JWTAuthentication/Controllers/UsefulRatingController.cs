using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using JWTAuthentication.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.Data.SqlClient;
using Dapper;
using Microsoft.AspNetCore.Http;

namespace JWTAuthentication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class UsefulRatingController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public UsefulRatingController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [Authorize]
        [HttpPost("LikeOrDislike")]
        public async Task<IActionResult> LikeOrDislike([FromBody] UsefulRatingModel usefulRating)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkRating = $"SELECT * FROM Rating WHERE ID ='{usefulRating.RatingID}'";
                    var rating = conn.Query<RatingModel>(checkRating).FirstOrDefault();
                    if (rating == null) return StatusCode(StatusCodes.Status406NotAcceptable, new { code = 406, message = "Không tìm thấy rating" });

                    string checkUseful = $"SELECT * FROM UsefulRating WHERE UserID ='{user.Id}' AND RatingID='{rating.ID}'";
                    var togleLike = conn.Query<RatingModel>(checkUseful).FirstOrDefault();

                    string LikeQuery = "";
                    if (togleLike == null)
                    {
                        LikeQuery = $"INSERT INTO UsefulRating (ID, UserID, RatingID) VALUES('{Guid.NewGuid()}', '{user.Id}', '{rating.ID}')";
                        conn.Execute(LikeQuery);
                        return Ok(new { code = 200, message = "Like thành công" });
                    }
                    else
                    {
                        LikeQuery = $"DELETE FROM UsefulRating WHERE UserID='{user.Id}' AND RatingID='{rating.ID}'";
                        conn.Execute(LikeQuery);
                        return Ok(new { code = 200, message = "Disike thành công" });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }

    }

}
