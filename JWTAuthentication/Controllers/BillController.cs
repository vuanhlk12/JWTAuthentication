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
    public class BillController : ControllerBase
    {
        [HttpGet("GetTransactions")]
        public IActionResult GetAllTransaction(string userID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * FROM AspNetUsers where Id = N'{userID}'";
                    string query = $"SELECT * FROM Bill where BuyerID = '{userID}'";
                    List<UserModel> users = conn.QueryAsync<UserModel>(checkExist).Result.AsList();
                    if (users.Count == 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "User không tồn tại" });
                    }
                    else
                    {
                        List<BillModel> methods = conn.QueryAsync<BillModel>(query).Result.AsList();
                        if (methods.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không có giao dich" });
                        else return Ok(new { code = 200, detail = methods });
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }
        }

        //Vũ Anh thêm get trans cho store
        [HttpGet("GetTransactionsStore")]
        public IActionResult GetTransactionsStore(string StoreID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * FROM Store where Id = N'{StoreID}'";
                    List<StoreModel> stores = conn.QueryAsync<StoreModel>(checkExist).Result.AsList();
                    if (stores.Count == 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Store không tồn tại" });
                    }
                    else
                    {
                        string query = $@"SELECT b.ID AS BillID,
                                               b.BuyerID AS BuyerID,
                                               anu.UserName AS BuyerAccount,
                                               b.OrderTime,
                                               p.*
                                        FROM Bill b
                                        INNER JOIN BillProduct bp ON b.ID =bp.BillID
                                        INNER JOIN Product p ON bp.ProductID =p.ID
                                        INNER JOIN Store s ON p.StoreID =s.ID
                                        INNER JOIN AspNetUsers anu ON anu.Id = b.BuyerID
                                        WHERE s.ID = '{StoreID}'";
                        List<dynamic> methods = conn.QueryAsync<dynamic>(query).Result.AsList();
                        dynamic result = from method in methods
                                         select new
                                         {
                                             BillID = method.BillID,
                                             BuyerID = method.BuyerID,
                                             BuyerAccount = method.BuyerAccount,
                                             OrderTime = method.OrderTime,
                                             Product = new ProductModel
                                             {
                                                 ID = method.ID,
                                                 Name = method.Name,
                                                 Price = method.Price,
                                                 Color = method.Color,
                                                 Size = method.Size,
                                                 Detail = method.Detail,
                                                 Description = method.Description,
                                                 CategoryID = method.CategoryID,
                                                 Discount = method.Discount,
                                                 Quanlity = method.Quanlity,
                                                 Image = method.Image,
                                                 AddedTime = method.AddedTime,
                                                 LastModify = method.LastModify,
                                                 StoreID = method.StoreID,
                                                 SoldQuanlity = method.SoldQuanlity,
                                                 Star = method.Star,
                                                 RatingsCount = method.RatingsCount
                                             }
                                         };
                        if (methods.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không có giao dich" });
                        else return Ok(new { code = 200, test = "test", detail = result });
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }
        }

        [HttpPost("SetStatusCancel")]
        public IActionResult SetCancel(string transID)//method nay chi thang seller hay admin co the thuc hien
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {

                    string checkExist = $"SELECT * FROM Bill where Id = N'{transID}'";
                    string setStatus = $"update bill set status = 2 where id = N'{transID}'";
                    List<BillModel> trans = conn.QueryAsync<BillModel>(checkExist).Result.AsList();
                    if (trans.Count == 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Giao dịch không tồn tại" });
                    }
                    else
                    {
                        if (trans.FirstOrDefault().Status == 1 || trans.FirstOrDefault().Status == 2) return StatusCode(StatusCodes.Status403Forbidden, new { code = 403, message = "Giao dịch đã kết thúc" });
                        else
                        {
                            conn.Execute(setStatus);
                            return Ok(new { code = 200, message = "Hủy đơn hàng thành công" });
                        }
                    }

                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }
        }

        [HttpPost("SetStatusDelivered")]
        public IActionResult SetDeliver(string transID)//method nay chi thang seller hay admin co the thuc hien
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {

                    string checkExist = $"SELECT * FROM Bill where ID = N'{transID}'";
                    string setStatus = $"update bill set status = 1, shiptime = N'{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}' where id = N'{transID}'";
                    List<BillModel> trans = conn.QueryAsync<BillModel>(checkExist).Result.AsList();
                    if (trans.Count == 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Giao dịch không tồn tại" });
                    }
                    else
                    {
                        if (trans.FirstOrDefault().Status == 1 || trans.FirstOrDefault().Status == 2) return StatusCode(StatusCodes.Status403Forbidden, new { code = 403, message = "Giao dịch đã kết thúc" });
                        else
                        {
                            conn.Execute(setStatus);
                            return Ok(new { code = 200, message = "Mua đơn hàng thành công" });
                        }
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }
        }
    }
}
