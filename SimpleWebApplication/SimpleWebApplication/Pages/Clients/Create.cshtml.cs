using Amazon.S3.Transfer;
using Amazon.S3;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Globalization;
using Amazon;
using System.Text.RegularExpressions;

namespace SimpleWebApplication.Pages.Clients
{
    public class Create : PageModel
    {
        public Client c = new Client();
        public string errorMessage = "";
        public string successMessage = "";
        public Regex validateEmailRegex = new Regex("^\\S+@\\S+\\.\\S+$");
        public void OnGet()
        {
        }
        
        public void OnPost()
        {
            
            c.name = Request.Form["name"];
            c.email = Request.Form["email"];
            c.phone = Request.Form["phone"];
            c.address = Request.Form["address"];
            
            if(c.name.Length==0 || c.email.Length == 0 || 
                c.phone.Length == 0 || c.address.Length == 0)
            {
                errorMessage = "All fields are required!";
                return;
            }
            if (!validateEmailRegex.IsMatch(c.email))
            {
                errorMessage = "Incorrect mail address!";
                return;
            }
            
            try
            {
                string connectionString = "Data Source=clients-db.cdrwukyirog3.us-east-1.rds.amazonaws.com,1433;Initial Catalog=Clients;User ID=admin;Password=vegaspro2";
                using(SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO CLIENTS(name,email,phone,address)" +
                        "VALUES (@name,@email,@phone,@address)";

                    using (SqlCommand cmd = new SqlCommand(sql,connection))
                    {
                        cmd.Parameters.AddWithValue("@name", c.name);
                        cmd.Parameters.AddWithValue("@email", c.email);
                        cmd.Parameters.AddWithValue("@phone", c.phone);
                        cmd.Parameters.AddWithValue("@address", c.address);

                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch(Exception ex)
            {
                errorMessage = ex.Message;
                return;
            }
            c.name = ""; c.email = ""; c.phone = "";c.address = "";
            successMessage = "New Client Added Correclty";

            Response.Redirect("/Clients/Index");
        }
    }
}
