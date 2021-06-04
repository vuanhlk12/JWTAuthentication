using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
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
using Newtonsoft.Json;
using System.Net.Mail;
using System.Net;

namespace JWTAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public MailController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }


        [HttpGet("ResetPasswordMail")]
        public async Task<IActionResult> ResetPasswordMail(string userName)
        {
            try
            {
                var user = await userManager.FindByNameAsync(userName);
                if (user == null) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "User not found" });
                if (string.IsNullOrEmpty(user.Email)) return StatusCode(StatusCodes.Status405MethodNotAllowed, new { code = 405, message = "Người dùng chưa có email" });

                var token = await userManager.GeneratePasswordResetTokenAsync(user);

                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress("tikitokodut@gmail.com");
                mail.To.Add(user.Email);
                mail.Subject = "Reset Password";
                mail.Body = $"Đường link reset password:\nhttp://localhost:3001/resest-password?userName={userName}&token={token}";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("tikitokodut", "vuanh123");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
                return Ok(new { code = 200, message = $"Đã gửi email reset tới {user.Email}" });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Đã có lỗi xẩy ra: " + ex.Message });
            }
        }



    }
}

