using Npgsql;
using System.Data;

namespace ShellKartBE.Models
{
    public class DAL
    {
        public Response register(Users users, NpgsqlConnection connection)
        {
            Response response = new Response();
            NpgsqlCommand cmd = new NpgsqlCommand("public.sp_register", connection);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("ID", 0);
            cmd.Parameters.AddWithValue("FirstName", users.FirstName);
            cmd.Parameters.AddWithValue("LastName", users.LastName);
            cmd.Parameters.AddWithValue("Password", users.Password);
            cmd.Parameters.AddWithValue("Email", users.Email);
            cmd.Parameters.AddWithValue("Fund", 0);
            cmd.Parameters.AddWithValue("Type", "Users");
            cmd.Parameters.AddWithValue("Status", 1);
            cmd.Parameters.AddWithValue("ActionType", "Add");
            try
            {
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                
                if (i > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "User registered successfully";
                }
                else
                {
                    response.StatusCode = 400;
                    response.StatusMessage = "User registeration failed";
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions here
                response.StatusCode = 500; // Internal Server Error
                response.StatusMessage = "An error occurred: " + ex.Message;
            }
            finally
            {
                connection.Close();
            }
            return response;
        }

        public Response login(Users users, NpgsqlConnection connection)
        {
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("public.sp_login2", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure; 
            da.SelectCommand.Parameters.AddWithValue("@Email", users.Email); 
            da.SelectCommand.Parameters.AddWithValue("@Password", users.Password);
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            Users user = new Users();
            if (dt.Rows.Count > 0)
            {
                user.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                user.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                user.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Type = Convert.ToString(dt.Rows[0]["Type"]);
                response.StatusCode = 200;
                response.StatusMessage = "User is valid";
                response.user = user;
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "User is invalid";
                response.user = null;
            }
            return response;
        }

        public Response viewUser(Users users, NpgsqlConnection connection)
        {
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("p_viewUser", connection);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@ID", users.ID); 
            DataTable dt = new DataTable();
            da.Fill(dt);
            Response response = new Response();
            Users user = new Users();
            if (dt.Rows.Count > 0)
            {
                user.ID = Convert.ToInt32(dt.Rows[0]["ID"]);
                user.FirstName = Convert.ToString(dt.Rows[0]["FirstName"]);
                user.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                user.Email = Convert.ToString(dt.Rows[0]["Email"]);
                user.Type = Convert.ToString(dt.Rows[0]["Type"]);
                user.Fund = Convert.ToDecimal(dt.Rows[0]["Fund"]);
                user.Createdon = Convert.ToDateTime(dt.Rows[0]["CreatedOn"]);
                user.Password = Convert.ToString(dt.Rows[0]["Password"]);
                response.StatusCode = 200;
                response.StatusMessage = "User exists";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "User does not exist.";
                response.user = user;
            }
            return response;
        }

        public Response updateProfile(Users users, NpgsqlConnection connection)
        {
            Response response = new Response();
            NpgsqlCommand cmd = new NpgsqlCommand("sp_updateProfile", connection); cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FirstName", users.FirstName);
            cmd.Parameters.AddWithValue("@LastName", users.LastName);
            cmd.Parameters.AddWithValue("@Password", users.Password);
            cmd.Parameters.AddWithValue("@Email", users.Email); connection.Open(); connection.Close();
            int i = cmd.ExecuteNonQuery();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Record updated successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Some error occured ! Try after some time";
            }

            return response;

        }


        public Response addToCart(Cart cart, NpgsqlConnection connection)
        {
            Response response = new Response();
            NpgsqlCommand cmd = new NpgsqlCommand("sp_AddToCart", connection);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", cart.UserId);
            cmd.Parameters.AddWithValue("@UnitPrice", cart.UnitPrice);
            cmd.Parameters.AddWithValue("@Discount", cart.Discount);
            cmd.Parameters.AddWithValue("@TotalPrice", cart.TotalPrice);
            cmd.Parameters.AddWithValue("@ProductID", cart.ProductID);
            cmd.Parameters.AddWithValue("@Quantity", cart.Quantity);
            connection.Open();
            int i = cmd.ExecuteNonQuery();
            connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Item addedd successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Item could not be added";
            }
            return response;
        }


        public Response placeOrder(Users users, NpgsqlConnection connection)
        {
            Response response = new Response();
            NpgsqlCommand cmd = new NpgsqlCommand("sp_PlaceOrder", connection);
            cmd.CommandType = CommandType.StoredProcedure; cmd.Parameters.AddWithValue("@ID", users.ID);
            connection.Open();
            int i = cmd.ExecuteNonQuery(); connection.Close();
            if (i > 0)
            {
                response.StatusCode = 200;
                response.StatusMessage = "Order has been placed successfully";
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Order could not be placed";
            }
            return response;
        }


        public Response orderList(Users users, NpgsqlConnection connection)
        {
            Response response = new Response();
            List<Orders> listOrder = new List<Orders>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("sp_OrderList", connection);
            da.SelectCommand.Parameters.AddWithValue("@Type", users.Type);
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.SelectCommand.Parameters.AddWithValue("@ID", users.ID); 
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Orders order = new Orders();
                    order.Id = Convert.ToInt32(dt.Rows[i]["ID"]);
                    order.OrderNo = Convert.ToString(dt.Rows[i]["OrderNo"]);
                    order.OrderTotal = Convert.ToDecimal(dt.Rows[i]["OrderTotal"]);
                    order.OrderStatus = Convert.ToString(dt.Rows[i]["OrderStatus"]);
                    listOrder.Add(order);
                }
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++) if (listOrder.Count > 0)
                        {
                            response.StatusCode = 200;
                            response.StatusMessage = "Order details fetched";
                            response.listOrders = listOrder;
                        }
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "Order details are not available";
                    response.listOrders = null;
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "Order details are not available";
                response.listOrders = null;
            }
            return response;
        }


        public Response addUpdateProduct(Products products, NpgsqlConnection connection)
            {
                Response response = new Response();
                NpgsqlCommand cmd = new NpgsqlCommand("sp_AddUpdateProduct", connection);
                NpgsqlDataAdapter da = new NpgsqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                List<Products> listProduct = new List<Products>();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Name", products.Name);
                cmd.Parameters.AddWithValue("@Manufacturer", products.Manufacturer);
                cmd.Parameters.AddWithValue("@UnitPrice", products.UnitPrice);
                cmd.Parameters.AddWithValue("@Discount", products.Discount);
                cmd.Parameters.AddWithValue("@Quantity", products.Quantity);
                cmd.Parameters.AddWithValue("@ExpDate", products.ExpDate);
                cmd.Parameters.AddWithValue("@ImageUrl", products.ImageUrl);
                cmd.Parameters.AddWithValue("@Status", products.Status);
                cmd.Parameters.AddWithValue("@Type", products.Type);
            if (products.Type != "Get" && products.Type != "GetByID")
            {
                connection.Open();
                int i = cmd.ExecuteNonQuery();
                connection.Close();
                if (i > 0)
                {
                    response.StatusCode = 200;
                    if (products.Type == "Add")
                        response.StatusMessage = "Product inserted successfully";
                    if (products.Type == "Update")
                        response.StatusMessage = "Product updated successfully";
                    if (products.Type == "Delete")
                        response.StatusMessage = "Product deleted successfully";
                }
                else
                {
                    response.StatusCode = 100;
                    if (products.Type == "Add")
                        response.StatusMessage = "Product did not save. try again.";
                    if (products.Type == "Update")
                        response.StatusMessage = "Product did not update. try again.";
                    if (products.Type == "Delete")
                        response.StatusMessage = "Product did not delete. try again.";
                }
            }
            else
            {
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Products Product = new Products();
                        Product.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                        Product.Name = Convert.ToString(dt.Rows[i]["Name"]);
                        Product.Manufacturer = Convert.ToString(dt.Rows[i]["Manufacturer"]);
                        Product.UnitPrice = Convert.ToString(dt.Rows[i]["UnitPrice"]);
                        Product.Discount = Convert.ToString(dt.Rows[i]["Discount"]);
                        Product.Quantity = Convert.ToString(dt.Rows[i]["Quantity"]);
                        Product.ExpDate = Convert.ToString(dt.Rows[i]["ExpDate"]);
                        Product.ImageUrl = Convert.ToString(dt.Rows[i]["ImageUrl"]);
                        Product.Status = Convert.ToInt32(dt.Rows[i]["Status"]);
                        listProduct.Add(Product);
                    }
                    if (listProduct.Count > 0)
                    {
                        response.StatusCode = 200;
                        response.listProducts = listProduct;
                    }
                    else
                    {
                        response.StatusCode = 100;
                        response.listProducts = null;
                    }
                }
            }

            return response;
        }

        public Response userList(NpgsqlConnection connection)
        {
            Response response = new Response();
            List<Users> listUsers = new List<Users>();
            NpgsqlDataAdapter da = new NpgsqlDataAdapter("sp_UserList", connection);
            DataTable dt = new DataTable();
            da.SelectCommand.CommandType = CommandType.StoredProcedure;
            da.Fill(dt);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Users user = new Users();
                    user.ID = Convert.ToInt32(dt.Rows[i]["ID"]);
                    user.FirstName = Convert.ToString(dt.Rows[i]["FirstName"]);
                    user.LastName = Convert.ToString(dt.Rows[i]["LastName"]); 
                    user.Password = Convert.ToString(dt.Rows[i]["Password"]);
                    user.Email = Convert.ToString(dt.Rows[i]["Email"]); 
                    user.Fund = Convert.ToDecimal(dt.Rows[i]["Fund"]);
                    user.Status = Convert.ToInt32(dt.Rows[i]["Status"]);
                    user.Createdon = Convert.ToDateTime(dt.Rows[i]["CreatedOn"]);
                    listUsers.Add(user);
                }
                if (listUsers.Count > 0)
                {
                    response.StatusCode = 200;
                    response.StatusMessage = "User details fetched";
                    response.listUsers = listUsers;
                }
                else
                {
                    response.StatusCode = 100;
                    response.StatusMessage = "User details are not available";
                    response.listUsers = null;
                }
            }
            else
            {
                response.StatusCode = 100;
                response.StatusMessage = "User details are not available";
                response.listOrders = null;
            }
            return response;
        }



    }
}
