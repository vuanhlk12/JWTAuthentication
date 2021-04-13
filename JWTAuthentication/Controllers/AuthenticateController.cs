using Dapper;
using JWTAuthentication.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace JWTAuthentication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            var user = await userManager.FindByNameAsync(model.Account);
            if (user == null)
            {
                return Ok(new
                {
                    code = 401,
                    message = "User không tồn tại"
                });
            }
            if (await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );

                StoreModel store = null;
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"SELECT * FROM Store where OwnerID ='{user.Id}'";
                    store = conn.Query<StoreModel>(query).FirstOrDefault();
                }

                return Ok(new
                {
                    code = 200,
                    message = "Thành công",
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo,
                    userInfo = new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        dateOfBirth = user.DateOfBirth,
                        email = user.Email,
                        gender = user.Gender,
                        firstName = user.FirstName,
                        lastName = user.LastName,
                        phone = user.PhoneNumber,
                        profilePhoto = user.ProfilePhoto,
                        role = userRoles,
                        store = store
                    }
                }); ;
            }
            return Ok(new
            {
                code = 1200,
                message = "Thông tin đăng nhập sai"
            });
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Account);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Tài khoản đã tồn tại" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Account,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth

            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, Message = "Thông tin tạo tài khoản không hợp lệ, vui lòng kiểm tra lại" });

            await CreateRoleIfNotExist();

            if (await roleManager.RoleExistsAsync(UserRoles.User))
            {
                await userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new { code = 200, message = "Tạo user thành công" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Account);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Tài khoản đã tồn tại" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Account,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, Message = "Thông tin tạo tài khoản không hợp lệ, vui lòng kiểm tra lại" });

            await CreateRoleIfNotExist();

            if (await roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Admin);
            }

            return Ok(new { code = 200, message = "Tạo admin thành công" });
        }

        [HttpPost]
        [Route("register-manager")]
        public async Task<IActionResult> RegisterManager([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.Account);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Account,
                Gender = model.Gender,
                PhoneNumber = model.PhoneNumber,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            await CreateRoleIfNotExist();

            if (await roleManager.RoleExistsAsync(UserRoles.Manager))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Manager);
            }

            return Ok(new Response { Status = "Success", Message = "Manager created successfully!" });
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("admin/GrantSellerPermisson")]
        public async Task<IActionResult> GrantSellerPermisson([FromBody] string UserName)
        {
            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not exists!" });

            await CreateRoleIfNotExist();

            if (await roleManager.RoleExistsAsync(UserRoles.Seller))
            {
                await userManager.AddToRoleAsync(user, UserRoles.Seller);
            }
            return Ok(new Response { Status = "Success", Message = $"User '{UserName}': Seller permission granted!" });
        }

        [Authorize(Roles = UserRoles.Admin)]
        [HttpPost("admin/RemoveSellerPermisson")]
        public async Task<IActionResult> RemoveSellerPermisson([FromBody] string UserName)
        {
            var user = await userManager.FindByNameAsync(UserName);
            if (user == null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User not exists!" });

            await CreateRoleIfNotExist();

            if (await roleManager.RoleExistsAsync(UserRoles.Seller))
            {
                await userManager.RemoveFromRoleAsync(user, UserRoles.Seller);
            }
            return Ok(new Response { Status = "Success", Message = $"User '{UserName}': Seller permission removed!" });
        }

        public async Task CreateRoleIfNotExist()
        {
            if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.Manager))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.User));
            if (!await roleManager.RoleExistsAsync(UserRoles.Seller))
                await roleManager.CreateAsync(new IdentityRole(UserRoles.Seller));
        }
    }
}
