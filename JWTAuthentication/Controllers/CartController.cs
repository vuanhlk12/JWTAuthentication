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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public CartController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("CartList")]
        public async Task<IActionResult> GetBuyerCartAsync(string buyerID = null)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {

                    string queryJoin = $"SELECT c.ID as CartID, c.AddedTime, c.Quantity , p.* FROM Cart c INNER JOIN Product p on c.ProductID = p.ID where c.BuyerID = N'{user.Id}'  ";

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

        [Authorize]
        [HttpDelete("DeleteAllCart")]
        public async Task<IActionResult> DeleteAllCartAsync([FromBody] string buyerID = null)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string deleteQuery = $"DELETE FROM Cart WHERE BuyerID ='{user.Id}'";
                    conn.Execute(deleteQuery);
                    return Ok(new { code = 200, message = "Đã xóa toàn bộ giỏ hàng" });
                }
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra" });
            }
        }

        [Authorize]
        [HttpGet("ProductInCart")]
        public async Task<IActionResult> GetBuyerProductInCartAsync(string productID, string buyerID = null)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {

                    string queryJoin = $"SELECT c.ID as CartID, c.AddedTime, c.Quantity, p.* FROM Cart c INNER JOIN Product p on c.ProductID = p.ID where c.BuyerID = N'{user.Id}' and c.ProductID = N'{productID}'  ";

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

        [Authorize]
        [HttpPost("AddProductToCart")]
        public async Task<IActionResult> AddToCartAsync(CartModel cart)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {

                    string checkExist = $"SELECT * FROM Cart where BuyerID ='{user.Id}' and ProductID = '{cart.ProductID}'";
                    List<CartModel> cartQuerry = conn.Query<CartModel>(checkExist).AsList();

                    if (cartQuerry.Count == 0)
                    {
                        string insertNewItem = $"INSERT INTO Cart(ID, BuyerID,ProductID,AddedTime,  Quantity) VALUES(N'{Guid.NewGuid()}', N'{user.Id}', N'{cart.ProductID}',N'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}',N'{cart.Quantity}');";
                        conn.Execute(insertNewItem);
                        return Ok(new { code = 200, message = $"Them gio hang thành công" });
                    }
                    else
                    {
                        string updateQuanity = $"UPDATE Cart SET Quantity = {cart.Quantity}, AddedTime = N'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' WHERE BuyerID = '{user.Id}' AND ProductID = '{cart.ProductID}'";
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

        [Authorize]
        [HttpDelete("DeleteFromCart")]
        public async Task<IActionResult> UserDeleteProductAsync(string productID, string userID = null)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * FROM Cart where BuyerID = N'{user.Id}' and ProductID = N'{productID}' ";
                    string deleteItem = $"DELETE FROM Cart where BuyerID = N'{user.Id}' and ProductID = N'{productID}' ";
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

        [Authorize]
        [HttpGet("ConfirmPayment")]
        public async Task<IActionResult> UserConfirmPaymentAsync(string AddressID, string PaymentID, string userID = null)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    //Vũ Anh sửa lại thành lấy hết
                    string consumePending = $"SELECT c.*, p.* FROM Cart c INNER JOIN Product p ON c.ProductID = p.ID where c.BuyerID = N'{user.Id}'  ";
                    string changeStatus = $"DELETE FROM Cart where BuyerID =  N'{user.Id}' ";
                    var query = conn.QueryAsync<CartModel, ProductModel, CartModel>(consumePending, (cart, product) =>
                    {
                        cart.Product = product;
                        return cart;
                    }, splitOn: "ID").Result.ToList();

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

                        if (PaymentID != "88888888-8888-8888-8888-888888888888")
                        {
                            string checkPayment = $"SELECT * FROM Payment WHERE UserID ='{user.Id}'";
                            List<PaymentModel> payments = conn.Query<PaymentModel>(checkPayment).Where(p => p.ID == PaymentID).AsList();
                            if (payments.Count() == 0) return StatusCode(StatusCodes.Status402PaymentRequired, new { code = 402, message = "Người dùng không có phương thức thanh toán này" });
                        }

                        string createBill = $"INSERT INTO Bill(ID,BuyerID,ListItem,Total,OrderTime,ShipTime,Status,AddressID,PaymentID) VALUES(N'{BillID}', N'{user.Id}', N'{listItem}', {total},N'{transactionTime}', null, 0 , N'{AddressID}', N'{PaymentID}')";
                        conn.Execute(createBill);

                        foreach (CartModel c in query)
                        {
                            string addToBillProduct = $"INSERT INTO BillProduct (ID, BillID, ProductID, ProductQuantity,StoreID, TransactionTime) VALUES('{Guid.NewGuid()}', '{BillID}', '{c.ProductID}', {c.Quantity}, N'{c.Product.StoreID}',N'{transactionTime}')";
                            string updateProduct = $"update Product set Quanlity = Quanlity - {c.Quantity} where id = N'{c.ProductID}'";

                            conn.Execute(addToBillProduct);//them truong storeID de tien tra cuu
                            conn.Execute(updateProduct);//sua lai so hang ton du
                        }

                        conn.Execute(changeStatus);

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

