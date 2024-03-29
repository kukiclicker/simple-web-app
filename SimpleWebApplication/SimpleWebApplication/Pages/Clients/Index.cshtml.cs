using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace SimpleWebApplication.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<Client> listClients = new List<Client>();
        public void OnGet()
        {
            try
            {
                string connString = "Data Source=clients-db.cdrwukyirog3.us-east-1.rds.amazonaws.com,1433;Initial Catalog=Clients;User ID=admin;Password=vegaspro2";
                using(SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM Clients";
                    using(SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Client c = new Client();
                                c.id = "" + reader.GetInt32(0);
                                c.name = reader.GetString(1);
                                c.email = reader.GetString(2);
                                c.phone = reader.GetString(3);
                                c.address = reader.GetString(4);
                                c.created_at = reader.GetDateTime(5).ToString();

                                listClients.Add(c);
                            }
                        }
                    }
                }
            } 
            catch(Exception ex)
            {
                Console.WriteLine("Exception " + ex);
            }
        }

       
    }
    public class Client
    {
        public string id;
        public string name;
        public string email;
        public string phone;
        public string address;
        public string created_at;
    }
}
