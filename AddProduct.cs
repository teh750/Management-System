using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data.SqlTypes;

namespace Pharmacy_Management_System
{
    public partial class AddProduct : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataTable dt;


        public AddProduct()
        {
            InitializeComponent();
        }

        private int GetNextProductId()
        {
            int nextId = 1; // Start numbering from 1
            string query = "SELECT MAX(Id) FROM Product";
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\teh75\source\repos\Pharmacy Management System\Pharmacy Management System\Pharmacy.accdb"))
            {
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        nextId = Convert.ToInt32(result) + 1;
                    }
                    conn.Close();
                }
            }
            return nextId;
        }

        void GetCustomer()
        {
            conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\teh75\source\repos\Pharmacy Management System\Pharmacy Management System\Pharmacy.accdb");
            dt = new DataTable(); // Initialize dt here
            adapter = new OleDbDataAdapter("SELECT * FROM [Product]", conn);
            conn.Open();
            adapter.Fill(dt);
            dgwProduct.DataSource = dt;
            conn.Close();
        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void txtId_TextChanged(object sender, EventArgs e)
        {

        }

        private void AddProduct_Load(object sender, EventArgs e)
        {
            txtId.Text = GetNextProductId().ToString();
            GetCustomer();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            // Define the SQL query with parameter placeholders
            string query = "INSERT INTO Product (Name, Type, Purpose,[Date]) VALUES (@name, @type, @purpose, @date)";

            // Ensure the connection is properly initialized and passed to the command
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\teh75\source\repos\Pharmacy Management System\Pharmacy Management System\Pharmacy.accdb"))
            {
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    // Add parameters to avoid SQL Injection
                    cmd.Parameters.AddWithValue("@name", txtName.Text); // Ensure these control names match your actual UI
                    cmd.Parameters.AddWithValue("@type", txtType.Text);
                    cmd.Parameters.AddWithValue("@purpose", txtPurpose.Text);
                    cmd.Parameters.Add("@date", OleDbType.Date).Value = dtpProduct.Value.Date; // Assuming dtpProduct is a DateTimePicker control

                    // Open the connection
                    conn.Open();

                    // Execute the command
                    cmd.ExecuteNonQuery();

                    // Close the connection
                    conn.Close();
                }
            }

            MessageBox.Show("Product added successfully.");
            GetCustomer(); // Refresh your data grid or list to show the new entry
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            string query = "UPDATE Product SET Name=@name, Type=@type, Purpose=@purpose, [Date]=@date WHERE Id=@id";
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\teh75\source\repos\Pharmacy Management System\Pharmacy Management System\Pharmacy.accdb"))
            {
                using (OleDbCommand cmd = new OleDbCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", txtName.Text);
                    cmd.Parameters.AddWithValue("@type", txtType.Text);
                    cmd.Parameters.AddWithValue("@purpose", txtPurpose.Text);
                    cmd.Parameters.AddWithValue("@date", dtpProduct.Value);

                    int Id;
                    if (int.TryParse(txtId.Text, out Id))
                    {
                        cmd.Parameters.AddWithValue("@id", Id);
                        conn.Open();
                        int rowsAffected = cmd.ExecuteNonQuery();
                        conn.Close();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Product Edited");
                            GetCustomer();
                        }
                        else
                        {
                            MessageBox.Show("No rows updated. Id not found.");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Id format.");
                    }
                }
            }
        }

        private void dgwProduct_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtId.Text = dgwProduct.CurrentRow.Cells[0].Value.ToString();
            txtName.Text = dgwProduct.CurrentRow.Cells[1].Value.ToString();
            txtType.Text = dgwProduct.CurrentRow.Cells[2].Value.ToString();
            txtPurpose.Text = dgwProduct.CurrentRow.Cells[3].Value.ToString();
            dtpProduct.Text = dgwProduct.CurrentRow.Cells[4].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string query = "DELETE FROM Product WHERE Id=@id";
            cmd = new OleDbCommand(query, conn);
            cmd.Parameters.AddWithValue("@id",Convert.ToInt32(txtId.Text));
            conn.Open();
            cmd.ExecuteNonQuery();
            conn.Close();
            MessageBox.Show("Product Deleted");
            GetCustomer();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (dt != null)
            {
                DataView dv = dt.DefaultView;
                dv.RowFilter = "Name LIKE '%" + txtSearch.Text + "%'";
                dgwProduct.DataSource = dv;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Clearing the text fields in the form
            txtName.Text = "";     // Clears the Name text box
            txtType.Text = "";     // Clears the Type text box
            txtPurpose.Text = "";  // Clears the Purpose text box
            dtpProduct.Value = DateTime.Now;  // Resets the date picker to current date

            // If you also want to clear the search text box:
            txtSearch.Text = "";

        
        }

        private void label8_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            this.Hide();

            // Show the MainForm
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        private void dgwProduct_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
