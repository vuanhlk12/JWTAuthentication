using System;
using System.Collections.Generic;
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

namespace JWTAuthentication.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class StoreController : ControllerBase
    {
        [HttpGet("GetStoreByUserID")]
        public StoreModel GetStoreByUserID(string UserID = null)
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                string query = $"SELECT * FROM Store WHERE OwnerID = '{UserID}'";
                StoreModel store = conn.Query<StoreModel>(query).AsList().FirstOrDefault();
                return store;
            }
        }

        [HttpGet("GetStoreByRange")]
        public List<StoreModel> GetStoreByRange(int size, int page)
        {
            using (SqlConnection conn = new SqlConnection(GlobalSettings.ConnectionStr))
            {
                string query = $"SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY id) RowNr, * FROM Store ) t WHERE RowNr BETWEEN {page * size} AND {(page + 1) * size}";
                List<StoreModel> store = conn.Query<StoreModel>(query).AsList();
                return store;
            }
        }
    }

}
