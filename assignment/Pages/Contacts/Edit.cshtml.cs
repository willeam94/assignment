using assignment.Pages.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace assignment.Pages.Contacts
{
    public class EditModel : PageModel
    {

		public ContactInfo contactInfo = new ContactInfo();
		public String errorMessage = "";
		public String successMessage = "";

		public void OnGet()
        {
            String id = Request.Query["id"];

            try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=assignmentDB;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "SELECT * FROM contacts WHERE id=@id";
					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@id", id);
						using (SqlDataReader reader = command.ExecuteReader())
						{
							while (reader.Read())
							{
								contactInfo.id = "" + reader.GetInt32(0);
								contactInfo.name = reader.GetString(1);
								contactInfo.email = reader.GetString(2);
								contactInfo.phone = reader.GetString(3);
								contactInfo.address = reader.GetString(4);
							}
						}
					}
				}

			}
            catch (Exception ex)
            {
                errorMessage = ex.Message;
            }
        }

		public void OnPost()
		{
			contactInfo.id = Request.Form["id"];
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

			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=assignmentDB;Integrated Security=True";
				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE contacts " +
								 "SET name=@name, email=@email, phone=@phone, address=@address " +
								 "WHERE id=@id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						command.Parameters.AddWithValue("@name", contactInfo.name);
						command.Parameters.AddWithValue("@email", contactInfo.email);
						command.Parameters.AddWithValue("@phone", contactInfo.phone);
						command.Parameters.AddWithValue("@address", contactInfo.address);
						command.Parameters.AddWithValue("@id", contactInfo.id);

						command.ExecuteNonQuery();
					}
				}

			}
			catch(Exception ex)
			{
				errorMessage = ex.Message;
			}

			//if success edited, redirect to main page
			Response.Redirect("/Contacts/Index");
		}
    }
}
