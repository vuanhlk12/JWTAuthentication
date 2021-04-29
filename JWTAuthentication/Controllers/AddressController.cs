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
using System.Data;

namespace JWTAuthentication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AddressController : ControllerBase
    {
        public List<AddressModel> _GetAddressByUser(string UserID)
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                string query = $"SELECT * FROM Address a2 inner join District d2 on a2.DistrictID =d2.ID inner join City c2 on d2.CityID =c2.ID WHERE a2.UserID ='{UserID}'";
                var Addresses = conn.QueryAsync<AddressModel, DistrictModel, CityModel, AddressModel>(query, (address, district, city) =>
                 {
                     address.District = district;
                     address.City = city;
                     return address;
                 }, splitOn: "ID").Result.ToList();
                return Addresses;
            }
        }

        [HttpGet("GetAddressByUser")]
        public IActionResult GetAddressByUser(string UserID)
        {
            try
            {
                List<AddressModel> result = _GetAddressByUser(UserID);
                return Ok(new { code = 200, message = result });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }
        }

        public dynamic _GetCityList()
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                string query = $"SELECT * FROM City c2 inner join District d2 on c2.ID =d2.CityID ";
                var Addresses = conn.QueryAsync<CityModel, DistrictModel, CityModel>(query, (city, district) =>
                {
                    city.District = district;
                    return city;
                }, splitOn: "ID").Result.ToList();

                var results = Addresses.GroupBy(
                                p => (new { p.ID, p.CityName }),
                                p => p.District,
                                (key, g) => new { City = key, Districts = g.ToList() });

                return results;
            }
        }

        [HttpGet("GetCityList")]
        public IActionResult GetCityList()
        {
            try
            {
                dynamic result = _GetCityList();
                return Ok(new { code = 200, message = result });
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra", detail = e.Message });
            }
        }

        [HttpPost("AddAddressForUser")]
        public IActionResult AddAddressForUser(AddressModel address)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    string query = $"INSERT INTO Address (ID, Address, Phone, UserID, DistrictID) VALUES('{Guid.NewGuid()}', '{address.Address}', '{address.Phone}', '{address.UserID}', '{address.DistrictID}')";
                    conn.Execute(query);
                    return Ok(new { code = 200, message = "Thêm địa chỉ thành công" });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }

        [HttpGet("GetCity")]
        public IActionResult GetCity(string CityID = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    if (CityID == null)
                    {
                        string query = $"SELECT * FROM City";
                        var ListCity = conn.Query<CityModel>(query).AsList();
                        return Ok(new { code = 200, message = ListCity });
                    }
                    else
                    {
                        string query = $"SELECT * FROM City where ID = '{CityID}'";
                        var ListCity = conn.Query<CityModel>(query).AsList().FirstOrDefault();
                        return Ok(new { code = 200, message = ListCity });
                    }

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }

        [HttpGet("GetDistrict")]
        public IActionResult GetDistrict(string DistrictID = null)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
                {
                    if (DistrictID == null)
                    {
                        string query = $"SELECT * FROM District";
                        var ListDistrict = conn.Query<DistrictModel>(query).AsList();
                        return Ok(new { code = 200, message = ListDistrict });
                    }
                    else
                    {
                        string query = $"SELECT * FROM District where ID = '{DistrictID}'";
                        var ListDistrict = conn.Query<DistrictModel>(query).AsList().FirstOrDefault();
                        return Ok(new { code = 200, message = ListDistrict });
                    }

                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new { code = 500, message = "Có lỗi đã xẩy ra: " + ex.Message });
            }
        }

    }
}
