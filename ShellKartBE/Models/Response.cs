using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShellKartBE.Models
{
    public class Response
    {
        public int StatusCode { get; set; }
        public string StatusMessage { get; set; }
        public List<Users> listUsers { get; set; }
        public Users user { get; set; }
        public List<Products> listProducts { get; set; }
        public Products product { get; set; }
        public List<Cart> listCart { get; set; }
        
        public List<Orders> listOrders { get; set; }
        public Orders order { get; set; }
        public List<OrderItems> listOrderItems { get; set; }
        public OrderItems orderItem { get; set; }
    }
}
