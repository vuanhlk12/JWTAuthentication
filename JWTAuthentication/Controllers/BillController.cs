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
        public IActionResult GetTransactionsStore(int size, int page, string StoreID)
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
                                               b.AddressID,
                                               b.Status AS BillStatus,
                                               bp.ProductQuantity AS ProductQuantity,
                                               p.*
                                        FROM Bill b
                                        INNER JOIN BillProduct bp ON b.ID =bp.BillID
                                        INNER JOIN Product p ON bp.ProductID =p.ID
                                        INNER JOIN Store s ON p.StoreID =s.ID
                                        INNER JOIN AspNetUsers anu ON anu.Id = b.BuyerID
                                        WHERE s.ID = '{StoreID}'";
                        List<dynamic> methods = conn.QueryAsync<dynamic>(query).Result.AsList();
                        if (methods.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Không có giao dich" });

                        List<BillProductModel> modyfiedList = (from method in methods
                                                               select new BillProductModel
                                                               {
                                                                   BillID = method.BillID,
                                                                   BuyerID = method.BuyerID,
                                                                   BuyerAccount = method.BuyerAccount,
                                                                   OrderTime = method.OrderTime,
                                                                   AddressID = method.AddressID,
                                                                   BillStatus = method.BillStatus,
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
                                                                       Quanlity = method.ProductQuantity,
                                                                       Image = method.Image,
                                                                       AddedTime = method.AddedTime,
                                                                       LastModify = method.LastModify,
                                                                       StoreID = method.StoreID,
                                                                       SoldQuanlity = method.SoldQuanlity,
                                                                       Star = method.Star,
                                                                       RatingsCount = method.RatingsCount
                                                                   }
                                                               }).ToList();

                        var results = (modyfiedList.GroupBy(
                                p => new { p.BillID, p.BuyerID, p.BuyerAccount, p.OrderTime, p.AddressID, p.BillStatus },
                                p => p.Product,
                                (key, g) => new HistoryBillStoreModel
                                {
                                    BillID = key.BillID,
                                    BuyerID = key.BuyerID,
                                    BuyerAccount = key.BuyerAccount,
                                    OrderTime = key.OrderTime,
                                    AddressID = key.AddressID,
                                    BillStatus = key.BillStatus,
                                    Products = g.ToList()
                                })).OrderByDescending(t => t.OrderTime).Skip(size * page).Take(size).AsList();

                        if (results.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Page này không có kết quả" });
                        else return Ok(new { code = 200, detail = results });
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

        [HttpGet("GetBillByTime")]
        public IActionResult GetBillByTime(string mode, int previous)//method nay chi thang seller hay admin co the thuc hien
        {
            /*  mode chi co the la week hay month
             *  previous là so tuan (thang) truoc do. Neu de previous la 0 thi search theo tuan(thang) hien tai
             * */
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {

                    string selectByWeek = $"SELECT * FROM Bill where DATEPART(wk, OrderTime) = DATEPART(wk, GETDATE()) - {previous} and DATEPART(yy, OrderTime) = DATEPART(yy, GETDATE())";
                    string selectByMonth = $"SELECT * FROM Bill where DATEPART(mm, OrderTime) = DATEPART(mm, GETDATE()) - {previous} and DATEPART(yy, OrderTime) = DATEPART(yy, GETDATE())";
                    if (mode == "week")
                    {
                        List<BillModel> trans = conn.QueryAsync<BillModel>(selectByWeek).Result.AsList();
                        if (trans.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Giao dịch không tồn tại" });
                        else return Ok(new { code = 200, detail = trans });
                    }
                    else if (mode == "month")
                    {
                        List<BillModel> trans2 = conn.QueryAsync<BillModel>(selectByWeek).Result.AsList();
                        if (trans2.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Giao dịch không tồn tại" });
                        else return Ok(new { code = 200, detail = trans2 });
                    }
                    else return StatusCode(StatusCodes.Status403Forbidden, new { code = 403, message = "mode only 'week' or 'month' " });
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xay ra", detail = e.Message });
            }
        }

        [HttpGet("GetBillByDate")]
        public IActionResult GetBillByDate(int date, int month, int year)//method nay chi thang seller hay admin co the thuc hien
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {

                    string selectByDate = $"SELECT * FROM Bill where DATEPART(dd, OrderTime) = {date} and DATEPART(mm,OrderTime) = {month}  and DATEPART(yy, OrderTime) = {year}";
                    List<BillModel> trans = conn.QueryAsync<BillModel>(selectByDate).Result.AsList();
                    if (trans.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Giao dịch không tồn tại" });
                    else return Ok(new { code = 200, detail = trans });
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xay ra", detail = e.Message });
            }
        }

    }
}
