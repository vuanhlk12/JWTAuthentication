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

                    string queryJoin = $"SELECT c.ID as CartID, c.AddedTime,  p.* FROM Cart c INNER JOIN Product p on c.ProductID = p.ID where c.BuyerID = N'{buyerID}' ";

                    var query = conn.QueryAsync<CartModel, ProductModel, CartModel>(queryJoin, (cart, product) =>
                    {
                        cart.Product = product;
                        return cart;
                    }, splitOn: "ID").Result.ToList();


                    if (query.Count > 0) return Ok(new
                    {
                        code = 200,
                        Cart = query
                    });
                    else return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không có mặt hàng trong giỏ" });
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

                    string queryJoin = $"SELECT c.ID as CartID, c.AddedTime,  p.* FROM Cart c INNER JOIN Product p on c.ProductID = p.ID where c.BuyerID = N'{buyerID}' and c.ProductID = N'{productID}' ";

                    var query = conn.QueryAsync<CartModel, ProductModel, CartModel>(queryJoin, (cart, product) =>
                    {
                        cart.Product = product;
                        return cart;
                    }, splitOn: "ID").Result.ToList();

                    if (query.Count > 0) return Ok(new
                    {
                        code = 200,
                        Cart = query
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

                    string checkExist = $"SELECT * FROM Cart where BuyerID ='{cart.BuyerID}' and ProductID = '{cart.ProductID}' AND Status ='Added'";
                    List<CartModel> cartQuerry = conn.Query<CartModel>(checkExist).AsList();

                    if (cartQuerry.Count == 0)
                    {
                        string insertNewItem = $"INSERT INTO Cart(ID, BuyerID,ProductID,AddedTime, Status, Quantity) VALUES(N'{Guid.NewGuid()}', N'{cart.BuyerID}', N'{cart.ProductID}',N'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',N'Added',N'{cart.Quantity}');";
                        conn.Execute(insertNewItem);
                        return Ok(new { code = 200, message = $"Them gio hang thành công" });
                    }
                    else
                    {
                        string updateQuanity = $"UPDATE Cart SET Quantity = {cart.Quantity + cartQuerry.FirstOrDefault().Quantity}, AddedTime = '{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE BuyerID = '{cart.BuyerID}' AND ProductID = '{cart.ProductID}' AND Status ='Added'";
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
                    string checkExist = $"SELECT * FROM Cart where BuyerID = N'{userID}' and ProductID = N'{productID}' and Status = 'Added'";
                    string deleteItem = $"DELETE FROM Cart where BuyerID = N'{userID}' and ProductID = N'{productID}' and Status = 'Added'";
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
                    string consumePending = $"SELECT c.buyerID, c.quantity,  c.status, c.ProductID, p.ID, p.Name, p.Price, p.Discount FROM Cart c INNER JOIN Product p ON c.ProductID = p.ID where c.BuyerID = N'{userID}' and c.Status = 'Added' ";
                    string changeStatus = $"UPDATE Cart SET Status = 'Paid' where BuyerID =  N'{userID}' AND Status = 'Added' ";
                    var query = conn.QueryAsync<CartModel, ProductModel, CartModel>(consumePending, (cart, product) =>
                    {
                        cart.Product = product;
                        return cart;
                    }, splitOn: "ID").Result.ToList();
                    
                     conn.Execute(changeStatus);
                    if (query.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không có mặt hàng trong giỏ" });
                    else
                    {
                        string listItem = JsonConvert.SerializeObject(query);
                        int total = 0;
                        foreach (CartModel c in query)
                        {
                            total += c.Quantity * c.Product.Price * c.Product.Discount;
                        }
                        string createBill = $"INSERT INTO Bill(ID,BuyerID,ListItem,Total,OrderTime,ShipTime) VALUES(N'{Guid.NewGuid()}', N'{userID}', N'{listItem}', {total},N'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}', N'{DateTime.Now.AddDays(7).ToString("yyyy-MM-dd HH:mm:ss")}' )";
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

