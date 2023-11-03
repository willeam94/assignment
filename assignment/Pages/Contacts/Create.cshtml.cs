using assignment.Pages.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace assignment.Pages.Contacts
{
    public class CreateModel : PageModel
    {
        public ContactInfo contactInfo = new ContactInfo();
        public String errorMessage = "";
        public String successMessage = "";
        public void OnGet()
        {
        }

        public void OnPost() { 
            contactInfo.name = Request.Form["name"];
            contactInfo.email = Request.Form["email"];
            contactInfo.phone = Request.Form["phone"];
            contactInfo.address = Request.Form["address"];

            if (contactInfo.name.Length == 0 || contactInfo.email.Length == 0 ||
                contactInfo.phone.Length == 0 || contactInfo.address.Length == 0)
            {
                errorMessage = "All the fields are required";
                return;
            }

            // save the new contact to database
            try
            {
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=assignmentDB;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString)) 
                {
                    connection.Open();
                    String sql = "INSERT INTO contacts " +
                                 "(name, email, phone, address) VALUES " +
                                 "(@name, @email, @phone, @address);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", contactInfo.name);
						command.Parameters.AddWithValue("@email", contactInfo.email);
						command.Parameters.AddWithValue("@phone", contactInfo.phone);
						command.Parameters.AddWithValue("@address", contactInfo.address);

                        command.ExecuteNonQuery();
					}
                }
			}
			catch (Exception ex) 
            {
                errorMessage = ex.Message;
                return;
            }


            contactInfo.name = "";
            contactInfo.email = "";
            contactInfo.phone = "";
            contactInfo.address = "";
            successMessage = "New contact added";

            //if success added, redirect to main page
            Response.Redirect("/Contacts/Index");
        }
    }
}
