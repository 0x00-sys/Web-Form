using System;
using System.Data.SqlClient;
using System.Web.UI.WebControls;

namespace Web_Application
{
    public partial class Shop : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Učitaj podatke u GridView prilikom prvog učitavanja stranice
                BindGrid();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string description = txtDescription.Text;

            // Spremi proizvod u bazu podataka
            SaveProduct(name, description);

            // Osvježi GridView nakon spremanja
            BindGrid();

            // Očisti polja za unos nakon spremanja
            ClearInputFields();
        }

        protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Briši proizvod iz baze podataka
            int productId = Convert.ToInt32(GridView1.DataKeys[e.RowIndex].Values["Id"]);
            DeleteProduct(productId);

            // Osvježi GridView nakon brisanja
            BindGrid();
        }

        private void SaveProduct(string name, string description)
        {
            // Database connection string
            string connectionString = "Data Source=DESKTOP-A084HG4\\SQLEXPRESS;Initial Catalog=WebFormsLabos;Integrated Security=True;";

            // Query to insert a new product into the database
            string query = "INSERT INTO Products (Name, Description) VALUES (@Name, @Description)";

            // Using statement ensures proper disposal of resources
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Use parameterized query to prevent SQL injection
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Description", description);

                    // ExecuteNonQuery returns the number of rows affected
                    command.ExecuteNonQuery();
                }
            }
        }

        private void DeleteProduct(int productId)
        {
            // Database connection string
            string connectionString = "Data Source=DESKTOP-A084HG4\\SQLEXPRESS;Initial Catalog=WebFormsLabos;Integrated Security=True;";

            // Query to delete a product from the database
            string query = "DELETE FROM Products WHERE Id = @Id";

            // Using statement ensures proper disposal of resources
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Use parameterized query to prevent SQL injection
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Id", productId);

                    // ExecuteNonQuery returns the number of rows affected
                    command.ExecuteNonQuery();
                }
            }
        }

        private void BindGrid()
        {
            // Database connection string
            string connectionString = "Data Source=DESKTOP-A084HG4\\SQLEXPRESS;Initial Catalog=WebFormsLabos;Integrated Security=True;";

            // Query to select all products from the database
            string query = "SELECT Id, Name, Description FROM Products";

            // Using statement ensures proper disposal of resources
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Use SqlDataAdapter to fill a DataTable with the results of the query
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, connection))
                {
                    System.Data.DataTable dataTable = new System.Data.DataTable();
                    adapter.Fill(dataTable);

                    // Bind the DataTable to the GridView
                    GridView1.DataSource = dataTable;
                    GridView1.DataBind();
                }
            }
        }

        private void ClearInputFields()
        {
            txtName.Text = string.Empty;
            txtDescription.Text = string.Empty;
        }
    }
}
