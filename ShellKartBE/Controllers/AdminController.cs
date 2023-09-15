using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ShellKartBE.Models;
using Npgsql;

namespace ShellKartBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IConfiguration _configuration;
        public AdminController(IConfiguration configuration)
        {

            _configuration = configuration;
        }

        [HttpPost]
        [Route("addUpdateProduct")]
        public Response addUpdateMedicine(Products products)
        {
            DAL dal = new DAL();
            NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            Response response = dal.addUpdateProduct(products, connection);
            return response;
        }


        [HttpGet]
        [Route("userList")]
        public Response userList()
        {
            DAL dal = new DAL();
            NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            Response response = dal.userList(connection);
            return response;
        }
    }
}
