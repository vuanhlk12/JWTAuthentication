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
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;

        public BillController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        [Authorize]
        [HttpGet("GetTransactions")]
        public async Task<IActionResult> GetAllTransactionAsync(string userID = null)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * FROM AspNetUsers where Id = N'{user.Id}'";
                    string query = $"SELECT * FROM Bill where BuyerID = '{user.Id}'";
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

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Seller)]
        [HttpGet("GetTransactionsStoreColumnGraph")]
        public async Task<IActionResult> GetTransactionsStoreColumnGraph(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                var userRoles = await userManager.GetRolesAsync(user);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string storeQuery = $"SELECT * FROM Store where OwnerID ='{user.Id}'";
                    StoreModel store = conn.Query<StoreModel>(storeQuery).FirstOrDefault();

                    string query = $@"SELECT
	                                    CAST(b.OrderTime AS DATE) as date,
	                                    SUM(b.Total) as value
                                    FROM
	                                    Bill b
                                    inner join BillProduct bp on
	                                    b.ID = bp.BillID
                                    {(userRoles.Contains(UserRoles.Admin) ? "" : @$"WHERE bp.StoreID = '{store.ID}'")}
                                    GROUP BY
	                                    CAST(b.OrderTime AS DATE)";
                    List<ColumnGraph> graph = conn.Query<ColumnGraph>(query).AsList();
                    if (fromDate != null) graph = graph.Where(p => p.date >= fromDate).AsList();
                    if (toDate != null) graph = graph.Where(p => p.date <= toDate).AsList();
                    return Ok(new { code = 200, data = graph });
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }

        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Seller)]
        [HttpGet("GetTransactionsStoreDonutGraph")]
        public async Task<IActionResult> GetTransactionsStoreDonutGraph(DateTime? fromDate = null, DateTime? toDate = null)
        {
            try
            {
                var user = await userManager.FindByNameAsync(User.Identity.Name);
                var userRoles = await userManager.GetRolesAsync(user);
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string storeQuery = $"SELECT * FROM Store where OwnerID ='{user.Id}'";
                    StoreModel store = conn.Query<StoreModel>(storeQuery).FirstOrDefault();

                    string query = $@"SELECT
	                                    p.CategoryID ,
	                                    CAST(b.OrderTime AS DATE) as date,
	                                    SUM(b.Total) as value
                                    FROM
	                                    Bill b
                                    inner join BillProduct bp on
	                                    b.ID = bp.BillID
                                    INNER join Product p on
	                                    bp.ProductID = p.ID
                                    {(userRoles.Contains(UserRoles.Admin) ? "" : @$"WHERE bp.StoreID = '{store.ID}'")}
                                    GROUP BY
	                                p.CategoryID,
	                                CAST(b.OrderTime AS DATE)";
                    List<DonutGraph> graph = conn.Query<DonutGraph>(query).AsList();
                    if (fromDate != null) graph = graph.Where(p => p.date >= fromDate).AsList();
                    if (toDate != null) graph = graph.Where(p => p.date <= toDate).AsList();

                    double sum = graph.Sum(p => p.value);

                    List<DonutResult> results = (from grap in graph
                                                 select new DonutResult
                                                 {
                                                     Name = grap.Category.Name,
                                                     Value = (int)(grap.value / sum * 100)
                                                 }).ToList();

                    int sumPercent = 0;
                    for (int i = 0; i < results.Count - 1; i++)
                    {
                        sumPercent += results[i].Value;
                    }
                    results[results.Count - 1].Value = 100 - sumPercent;

                    return Ok(new { code = 200, data = new { series = results.Select(p => p.Name), labels = results.Select(p => p.Value) } });
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }

        }

        //Vũ Anh thêm get trans cho store
        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Seller)]
        [HttpGet("GetTransactionsStore")]
        public IActionResult GetTransactionsStore(int size, int page, string StoreID, DateTime? fromDate = null, DateTime? toDate = null, int? status = null)
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
                                               b.ShipTime,
                                               b.AddressID,
                                               b.PaymentID,
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
                                                                   ShipTime = method.ShipTime,
                                                                   AddressID = method.AddressID,
                                                                   BillStatus = method.BillStatus,
                                                                   PaymentID = method.PaymentID,
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
                                p => new { p.BillID, p.BuyerID, p.BuyerAccount, p.OrderTime, p.AddressID, p.BillStatus, p.ShipTime, p.PaymentID },
                                p => p.Product,
                                (key, g) => new HistoryBillStoreModel
                                {
                                    BillID = key.BillID,
                                    BuyerID = key.BuyerID,
                                    BuyerAccount = key.BuyerAccount,
                                    OrderTime = key.OrderTime,
                                    ShipTime = key.ShipTime,
                                    AddressID = key.AddressID,
                                    Status = key.BillStatus,
                                    PaymentID = key.PaymentID,
                                    Products = g.ToList()
                                })).OrderByDescending(t => t.OrderTime).AsList();

                        if (fromDate != null) results = results.Where(p => p.OrderTime >= fromDate).ToList();
                        if (toDate != null) results = results.Where(p => p.OrderTime <= ((DateTime)toDate).AddDays(1)).ToList();
                        if (status != null) results = results.Where(p => p.Status == status).ToList();

                        int total = results.Count;

                        results = results.Skip(size * page).Take(size).AsList();

                        if (results.Count == 0) return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Page này không có kết quả" });
                        else return Ok(new { code = 200, total = total, detail = results });
                    }
                }
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }
        }

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Seller)]
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

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Seller)]
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

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Seller)]
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

        [Authorize(Roles = UserRoles.Admin + "," + UserRoles.Seller)]
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
