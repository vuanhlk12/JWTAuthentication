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

                    string query = $"SELECT * FROM Cart where c.BuyerID = '{buyerID}'";

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
        public IActionResult GetBuyerProductInCart(string buyerID,string productID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {

                    string query = $"SELECT * FROM  where c.BuyerID = '{buyerID} ' and p.ID = '{productID}'";

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

        [HttpPost("AddProductToCart")]
        
        public IActionResult AddToCart(CartModel cart)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"INSERT INTO Cart(ID, BuyerID,ProductID,AddedTime, Status, ShippedTime, Quantity, OrderTime) VALUES(N'{Guid.NewGuid()}', N'{cart.BuyerID}', N'{cart.ProductID}',N'{cart.AddedTime}',N'{cart.Status}',N'{cart.ShippedTime}',N'{cart.Quanlity}',N'{cart.OrderTime}');";
                    conn.Execute(query);
                    return Ok(new { code = 200, message = $"Thêm vao gio hang thành công" });

                }
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = e.Message });
            }
        }
    }
}

