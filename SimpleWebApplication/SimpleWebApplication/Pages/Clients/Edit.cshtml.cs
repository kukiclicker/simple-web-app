using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SimpleWebApplication.Pages.Clients
{
    public class EditModel : PageModel
    {
        public Client c = new Client();
        public string errorMessage = "";
        public string successMessage = "";
        public void OnGet()
        {
            try
            {
                string id = Request.Query["id"];

                string connectionString = "Data Source=clients-db.cdrwukyirog3.us-east-1.rds.amazonaws.com,1433;Initial Catalog=Clients;Persist Security Info=True;User ID=admin;Password=vegaspro2";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Clients WHERE id=@id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Client c = new Client();
                                c.id = "" + reader.GetInt32(0);
                                c.name = reader.GetString(1);
                                c.email = reader.GetString(2);
                                c.phone = reader.GetString(3);
                                c.address = reader.GetString(4);

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception " + ex);
            }
        }
        public void onPost()
        {
            c.id = Request.Form["id"];
            c.name = Request.Form["name"];
            c.email = Request.Form["email"];
            c.phone = Request.Form["phone"];
            c.address = Request.Form["address"];

            if (c.id.Length == 0 || c.name.Length == 0 || c.email.Length == 0 ||
                c.phone.Length == 0 || c.address.Length == 0)
            {
                errorMessage = "All fields are required!";
                return;
            }
            
            try
            {
                string connectionString = "Data Source=clients-db.cdrwukyirog3.us-east-1.rds.amazonaws.com,1433;Initial Catalog=Clients;Persist Security Info=True;User ID=admin;Password=vegaspro2";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "UPDATE Clients" +
                                 "SET name=@name,email=@email, phone=@phone, address=@address " +
                                 "WHERE id=@id";
                    using (SqlCommand cmd = new SqlCommand(sql, connection))
                    {
                        cmd.Parameters.AddWithValue("@name", c.name);
                        cmd.Parameters.AddWithValue("@email", c.email);
                        cmd.Parameters.AddWithValue("@phone", c.phone);
                        cmd.Parameters.AddWithValue("@address", c.address);
                        cmd.Parameters.AddWithValue("@id", c.id);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            Response.Redirect("/Clients/Index");

        }
    }
}
