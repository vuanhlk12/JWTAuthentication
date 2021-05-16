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

                    string queryJoin = $"SELECT c.ID as CartID, c.AddedTime, c.Quantity , p.* FROM Cart c INNER JOIN Product p on c.ProductID = p.ID where c.BuyerID = N'{buyerID}'  ";

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

        [HttpDelete("DeleteAllCart")]
        public IActionResult DeleteAllCart([FromBody] string buyerID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string deleteQuery = $"DELETE FROM Cart WHERE BuyerID ='{buyerID}'";
                    conn.Execute(deleteQuery);
                    return Ok(new { code = 200, message = "Đã xóa toàn bộ giỏ hàng" });
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

                    string queryJoin = $"SELECT c.ID as CartID, c.AddedTime, c.Quantity, p.* FROM Cart c INNER JOIN Product p on c.ProductID = p.ID where c.BuyerID = N'{buyerID}' and c.ProductID = N'{productID}'  ";

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

                    string checkExist = $"SELECT * FROM Cart where BuyerID ='{cart.BuyerID}' and ProductID = '{cart.ProductID}'";
                    List<CartModel> cartQuerry = conn.Query<CartModel>(checkExist).AsList();

                    if (cartQuerry.Count == 0)
                    {
                        string insertNewItem = $"INSERT INTO Cart(ID, BuyerID,ProductID,AddedTime,  Quantity) VALUES(N'{Guid.NewGuid()}', N'{cart.BuyerID}', N'{cart.ProductID}',N'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',N'{cart.Quantity}');";
                        conn.Execute(insertNewItem);
                        return Ok(new { code = 200, message = $"Them gio hang thành công" });
                    }
                    else
                    {
                        string updateQuanity = $"UPDATE Cart SET Quantity = {cart.Quantity}, AddedTime = N'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE BuyerID = '{cart.BuyerID}' AND ProductID = '{cart.ProductID}'";
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
                    string checkExist = $"SELECT * FROM Cart where BuyerID = N'{userID}' and ProductID = N'{productID}' ";
                    string deleteItem = $"DELETE FROM Cart where BuyerID = N'{userID}' and ProductID = N'{productID}' ";
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
        public IActionResult UserConfirmPayment(string userID, string AddressID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    //Vũ Anh sửa lại thành lấy hết
                    string consumePending = $"SELECT c.*, p.* FROM Cart c INNER JOIN Product p ON c.ProductID = p.ID where c.BuyerID = N'{userID}'  ";
                    string changeStatus = $"DELETE FROM Cart where BuyerID =  N'{userID}' ";
                    var query = conn.QueryAsync<CartModel, ProductModel, CartModel>(consumePending, (cart, product) =>
                    {
                        cart.Product = product;
                        return cart;
                    }, splitOn: "ID").Result.ToList();

                    conn.Execute(changeStatus);
                    if (query.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không có mặt hàng trong giỏ" });
                    else
                    {
                        Guid BillID = Guid.NewGuid();
                        string transactionTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                        string listItem = JsonConvert.SerializeObject(query);
                        int total = 0;

                        foreach (CartModel c in query)//Vũ Anh sửa, thêm khóa ngoại nên cần insert Bill trước
                        {
                            total += (int)(c.Quantity * c.Product.Price * (1 - (float)c.Product.Discount / 100));
                        }

                        string createBill = $"INSERT INTO Bill(ID,BuyerID,ListItem,Total,OrderTime,ShipTime,Status,AddressID) VALUES(N'{BillID}', N'{userID}', N'{listItem}', {total},N'{transactionTime}', null, 0 , N'{AddressID}')";
                        conn.Execute(createBill);

                        foreach (CartModel c in query)
                        {
                            string addToBillProduct = $"INSERT INTO BillProduct (ID, BillID, ProductID, ProductQuantity,StoreID, TransactionTime) VALUES('{Guid.NewGuid()}', '{BillID}', '{c.ProductID}', {c.Quantity}, N'{c.Product.StoreID}',N'{transactionTime}')";
                            string updateProduct = $"update Product set Quanlity = Quanlity - {c.Quantity} where id = N'{c.ProductID}'";

                            conn.Execute(addToBillProduct);//them truong storeID de tien tra cuu
                            conn.Execute(updateProduct);//sua lai so hang ton du
                        }

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

