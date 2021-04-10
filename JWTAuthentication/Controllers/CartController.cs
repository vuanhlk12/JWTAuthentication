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

namespace JWTAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CartController : ControllerBase
    {
        [HttpGet("CartList")]
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


        public IActionResult AddToCart(CartModel cart)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {

                    string checkExist = $"SELECT * FROM Cart where BuyerID ='${cart.BuyerID}' and ProductID = '${cart.ProductID}' ";
                    string insertNewItem = $"INSERT INTO Cart(ID, BuyerID,ProductID,AddedTime, Status, Quantity) VALUES(N'{Guid.NewGuid()}', N'{cart.BuyerID}', N'{cart.ProductID}',N'{DateTime.Now.ToString("yyyy-MM-dd h:mm")}',N'{cart.Status}',N'{cart.Quantity}');";
                    string updateQuanity = $"UPDATE Cart SET Quanity = '${cart.Quantity}, AddedTime = '${DateTime.Now.ToString("yyyy-MM-dd h:mm")}' WHERE BuyerID = '${cart.BuyerID}' AND ProductID = '${cart.ProductID}' ";
                    List<CartModel> cartQuerry = conn.Query<CartModel>(checkExist).AsList();


                    if (cartQuerry.Count == 0)

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


        [HttpGet("ConfirmPayment")]
        public IActionResult UserConfirmPayment(string userID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string consumePending = $"SELECT c.buyerID, c.quantity,  c.status, c.ProductID, p.ID, p.Name, p.Price, p.Discount FROM Cart c INNER JOIN Product p ON c.ProductID = p.ID where c.BuyerID = N'{userID}' and c.Status = 'pending' ";
                    string changeStatus = $"UPDATE Cart SET Status = 'delivering' where BuyerID =  N'{userID}' AND Status = 'pending' ";
                    var query = conn.QueryAsync<CartModel, ProductModel, CartModel>(consumePending, (cart, product) =>
                    {
                        cart.Product = product;
                        return cart;
                    }, splitOn: "ID").Result.ToList();
                    //update tất cả các order từ pending->delivering
                    // conn.Execute(changeStatus);
                    if (query.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không có mặt hàng trong giỏ" });
                    else
                    {
                        string listItem = JsonConvert.SerializeObject(query);
                        int total = 0;
                        foreach (CartModel c in query)
                        {
                            total += c.Quantity * c.Product.Price * c.Product.Discount;
                        }
                        string createBill = $"INSERT INTO Bill(ID,BuyerID,ListItem,Total,OrderTime,ShipTime) VALUES(N'{Guid.NewGuid()}', N'{userID}', N'{listItem}', {total},N'{DateTime.Now.ToString("yyyy-MM-dd h:mm")}', N'{DateTime.Now.AddDays(7).ToString("yyyy-MM-dd h:mm")}' )";
                        conn.Execute(createBill);
                        return Ok(new { code = 200, message = "Thanh toán giỏ hàng thành công", detail = query });
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

