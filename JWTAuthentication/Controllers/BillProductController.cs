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
    public class BillProductController : ControllerBase
    {
        [HttpGet("GetProductByStore")]
        public IActionResult GetProductByStore(string storeID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * FROM Store where Id = N'{storeID}'";
                    string queryJoin = $"select bp.BillID, bp.ProductQuantity, p.* from BillProduct bp inner join product p on bp.ProductID = p.Id where bp.StoreID = N'{storeID}'";
                    List<StoreModel> store = conn.QueryAsync<StoreModel>(checkExist).Result.AsList();
                    if (store.Count == 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "User không tồn tại" });
                    }
                    else
                    {
                        var query = conn.QueryAsync<BillProductModel, ProductModel, BillProductModel>(queryJoin, (billProduct, product) =>
                        {
                            billProduct.Product = product;
                            return billProduct;
                        }, splitOn: "ID").Result.ToList();

                        if (query.Count > 0) return Ok(new
                        {
                            code = 200,
                            detail = query
                        });
                        else return StatusCode(StatusCodes.Status404NotFound, new { code = 4041, message = "Mặt hàng không có hóa đơn" });
                    }
                }
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = e.Message });
            }
        }

        [HttpGet("CancelOrder")]
        public IActionResult CancelOrder(string billID)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string checkExist = $"SELECT * FROM BillProduct where BillID = N'{billID}'";
                    string getProductlist = $"select * from BillProduct where BillID = N'{billID}'";
                    List<BillModel> bill = conn.QueryAsync<BillModel>(checkExist).Result.AsList();
                    if (bill.Count == 0)
                    {
                        return StatusCode(StatusCodes.Status404NotFound, new { code = 404, message = "Bill không tồn tại" });
                    }
                    else
                    {
                        //string delete = $"delete from BillProduct where BillID = N'{billID}'";
                        List<BillProductModel> billProduct = conn.QueryAsync<BillProductModel>(getProductlist).Result.AsList();
                        foreach(BillProductModel bp in billProduct)
                        {
                            string modify = $"Update Product set Quanlity = Quanlity + {bp.ProductQuantity} where id = N'{bp.ProductID}'";
                            conn.Execute(modify);
                        }
                        //conn.Execute(delete);
                        return Ok(new { code = 200, message = "Hủy đơn thành công" });
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
