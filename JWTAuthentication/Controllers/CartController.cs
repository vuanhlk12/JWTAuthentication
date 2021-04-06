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
    public class CartController : ControllerBase
    {
        [HttpGet("Cart")]
        public IActionResult GetBuyerCart(string buyerID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {

                    string query = $"SELECT * FROM Cart c LEFT JOIN Product p ON c.ProductID= p.ID where c.BuyerID = '{buyerID}'";

                    List<CartModel> cartQuerry = conn.Query<CartModel>(query).AsList();

                    if (cartQuerry.Count > 0) return Ok(new
                    {
                        code = 200,
                        Cart = cartQuerry
                    });
                    else return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không tìm thấy mặt hàng trong giỏ" });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra" });
            }
        }

        [HttpGet("ProductInCart")]
        public IActionResult GetBuyerProductInCart(string buyerID,string productName)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {

                    string query = $"SELECT * FROM Cart c LEFT JOIN Product p ON c.ProductID= p.ID where c.BuyerID = '{buyerID} ' and p.Name = '{productName}'";

                    List<CartModel> cartQuerry = conn.Query<CartModel>(query).AsList();

                    if (cartQuerry.Count > 0) return Ok(new
                    {
                        code = 200,
                        Cart = cartQuerry
                    });
                    else return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không tìm thấy mặt hàng trong giỏ" });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra" });
            }
        }
    }
}

