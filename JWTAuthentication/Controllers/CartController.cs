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

                    string query = $"SELECT * FROM Cart where BuyerID = '{buyerID}'";

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
        public IActionResult GetBuyerProductInCart(string buyerID, string productID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {

                    string query = $"SELECT * FROM Cart where BuyerID = N'{buyerID}' and ProductID = N'{productID}' ";

                    List<CartModel> cartQuerry = conn.Query<CartModel>(query).AsList();

                    if (cartQuerry.Count > 0) return Ok(new
                    {
                        code = 200,
                        Cart = cartQuerry
                    });
                    else return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không tìm thấy mặt hàng trong giỏ" });
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = e.Message });
            }
        }

        [HttpPost("AddProductToCart")]
<<<<<<< Updated upstream

        public IActionResult AddToCart(string userID, CartModel cart)
=======
        public IActionResult AddToCart(string userID ,CartModel cart)
>>>>>>> Stashed changes
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
<<<<<<< Updated upstream
                    string checkExist = $"SELECT * FROM Cart where BuyerID ='${userID}' and ProductID = '${cart.ProductID}' ";
                    string insertNewItem = $"INSERT INTO Cart(ID, BuyerID,ProductID,AddedTime, Status, ShippedTime, Quantity, OrderTime) VALUES(N'{Guid.NewGuid()}', N'{userID}', N'{cart.ProductID}',N'{DateTime.Now.ToString("yyyy-MM-dd h:mm")}',N'{cart.Status}',NULL,N'{cart.Quantity}',NULL);";
                    string updateQuanity = $"UPDATE Cart SET Quanity = '${cart.Quantity}, AddedTime = '${DateTime.Now.ToString("yyyy-MM-dd h:mm")}' WHERE BuyerID = '${userID}' AND ProductID = '${cart.ProductID}' ";
                    List<CartModel> cartQuerry = conn.Query<CartModel>(checkExist).AsList();
                    if (cartQuerry.Count == 0)
=======
                    string checkExist = $"SELECT * FROM Cart where BuyerID = N'{userID}' and ProductID = N'{cart.ProductID}' ";
                    string insertNewItem = $"INSERT INTO Cart(ID, BuyerID,ProductID,AddedTime, Status, ShippedTime, Quantity, OrderTime) VALUES(N'{Guid.NewGuid()}', N'{userID}', N'{cart.ProductID}',N'{DateTime.Now.ToString("yyyy-MM-dd h:mm")}',N'{cart.Status}',NULL,N'{cart.Quantity}',NULL);";
                    string updateQuanity = $"UPDATE Cart SET Quantity = N'{cart.Quantity}', AddedTime = N'{DateTime.Now.ToString("yyyy-MM-dd h:mm")}' WHERE BuyerID = N'{userID}' AND ProductID = N'{cart.ProductID}' ";
                    List<CartModel> cartQuerry = conn.Query<CartModel>(checkExist).AsList();
                
                    if(cartQuerry.Count == 0)
>>>>>>> Stashed changes
                    {
                        conn.Execute(insertNewItem);
                        return Ok(new { code = 200, message = $"Them gio hang thành công" });
                    }
                    else
                    {
                        conn.Execute(updateQuanity);
                        return Ok(new { code = 200, message = $"Cap nhat gio hang thành công" });
                    }


                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = e.Message });
            }
        }

        [HttpDelete("DeleteFromCart")]
        public IActionResult UserDeleteProduct(string userID, string productID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * FROM Cart where BuyerID = N'{userID}' and ProductID = N'{productID}' and Status = 'pending'";
                    string deleteItem = $"DELETE FROM Cart where BuyerID = N'{userID}' and ProductID = N'{productID}' and Status = 'pending'";
                    List<CartModel> cartQuerry = conn.Query<CartModel>(checkExist).AsList();

                    if (cartQuerry.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không tìm thấy mặt hàng trong giỏ" });
                    else
                    {
                        conn.Execute(deleteItem);
                        return Ok(new { code = 200, message = $"Xoa san pham thanh cong" });
                    }

                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = e.Message });
            }
        }

        public IActionResult UserConfirmPayment(string userID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string consumePending = $"SELECT * FROM Cart where BuyerID = N'{userID}' and Status = 'pending' ";
                   
                    List<CartModel> cartQuerry = conn.Query<CartModel>(consumePending).AsList();
                    
                    if(cartQuerry.Count > 0)
                    {
                        string listItem = JsonSerializer.Serialize(cartQuerry);
                    }

                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = e.Message });
            }
        }
    }
}

