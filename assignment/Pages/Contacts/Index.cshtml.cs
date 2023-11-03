using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace assignment.Pages.Clients
{
    public class IndexModel : PageModel
    {
        public List<ContactInfo> ListContacts = new List<ContactInfo>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=assignmentDB;Integrated Security=True";
            
                using (SqlConnection connection = new SqlConnection(connectionString))  
                {
                    connection.Open();
                    String sql = "SELECT * FROM contacts";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ContactInfo contactInfo = new ContactInfo();
                                contactInfo.id = "" + reader.GetInt32(0);
                                contactInfo.name = reader.GetString(1);
                                contactInfo.email = reader.GetString(2);
                                contactInfo.phone = reader.GetString(3);
                                contactInfo.address = reader.GetString(4);
                                contactInfo.created_at = reader.GetDateTime(5).ToString();

                                ListContacts.Add(contactInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception : " + ex.ToString());
            }
        }
    }

    public class ContactInfo
    {
        public string id;
        public string name;
        public string email;
        public string phone;
        public string address;
        public string created_at;
    }

}
