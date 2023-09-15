using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using ShellKartBE.Models;

namespace ShellKartBE.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IConfiguration _configuration;
        public ProductsController(IConfiguration configuration)
        {

            configuration = _configuration;
        }

        [HttpPost]
        [Route("addToCart")]
        public Response addToCart(Cart cart)
        {
            DAL dal = new DAL();
            NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            Response response = dal.addToCart(cart, connection);
            return response;
        }

        [HttpPost]
        [Route("placeOrder")]
        public Response placeOrder(Users users)
        {
            DAL dal = new DAL();
            NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("EComCS").ToString());
            Response response = dal.placeOrder(users, connection);
            return response;
        }


        [HttpPost]
        [Route("orderList")]
        public Response orderList(Users users)
        {
            DAL dal = new DAL();
            NpgsqlConnection connection = new NpgsqlConnection(_configuration.GetConnectionString("EComCS").ToString()); 
            Response response = dal.orderList(users, connection);
            return response;
        }
    }
}
