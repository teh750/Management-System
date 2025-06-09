using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Pharmacy_Management_System
{
    public partial class Addworker : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataTable dt;

        private byte[] ImageToByteArray(string imagePath)
        {
            using (Image image = Image.FromFile(imagePath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg); // Adjust the format as needed
                    return ms.ToArray();
                }
            }
        }

        public Addworker()
        {
            InitializeComponent();  // Initializes the form's components
            dgvWorker.AutoGenerateColumns = true;  // Ensures columns are created automatically
            GetCustomer();  // Load data
        }

        void GetCustomer()
        {
            string connectionString = @"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\teh75\source\repos\Pharmacy Management System\Pharmacy Management System\Pharmacy.accdb";
            string query = "SELECT * FROM [Worker]";
            dt = new DataTable(); // Initialize dt here

            try
            {
                using (conn = new OleDbConnection(connectionString))
                {
                    conn.Open();  // Open connection
                    adapter = new OleDbDataAdapter(query, conn);
                    adapter.Fill(dt);  // Fill DataTable with data

                    if (dt.Rows.Count > 0)
                    {
                        dgvWorker.DataSource = dt;  // Bind DataTable to DataGridView
                    }
                    else
                    {
                        MessageBox.Show("No data found!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load data: " + ex.Message);
            }
            finally
            {
                if (conn != null && conn.State == ConnectionState.Open)
                {
                    conn.Close();  // Ensure connection is closed
                }
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label6_Click(object sender, EventArgs e)
        {
            this.Hide();

            // Show the MainForm
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string query = "INSERT INTO Worker (Name, Gender, [Phone Number], [DOB], [Image]) VALUES (?, ?, ?, ?, ?)";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\teh75\source\repos\Pharmacy Management System\Pharmacy Management System\Pharmacy.accdb"))
                {
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.Add("?", OleDbType.VarChar).Value = txtName.Text;  // Name
                        cmd.Parameters.Add("?", OleDbType.VarChar).Value = cbGender.Text;  // Gender

                        int phoneNum;
                        if (int.TryParse(txtPhone.Text, out phoneNum))  // Use int instead of long
                        {
                            cmd.Parameters.Add("?", OleDbType.Integer).Value = phoneNum;
                        }
                        else
                        {
                            MessageBox.Show("Invalid phone number format.");
                            return;
                        }

                        cmd.Parameters.Add("?", OleDbType.Date).Value = dtpWorker.Value;  // DOB

                        byte[] imageData = null;
                        if (!string.IsNullOrWhiteSpace(txtPicture.Tag?.ToString()))
                        {
                            imageData = ImageToByteArray(txtPicture.Tag.ToString());
                        }
                        cmd.Parameters.Add("?", OleDbType.Binary).Value = imageData ?? (object)DBNull.Value;  // Image

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Worker added successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to add worker: " + ex.Message);
            }
        }

        private void txtChooseImage_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png";
                ofd.Title = "Select an Image";

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    // Get the path of specified file
                    string filePath = ofd.FileName;

                    // Assign file path to the Tag property of txtPicture for later use
                    txtPicture.Tag = filePath;  // Store the file path in the Tag property

                    // Display the image in txtPicture PictureBox
                    txtPicture.Image = Image.FromFile(filePath);
                    txtPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                }
            }
        }

        private void txtPicture_Click(object sender, EventArgs e)
        {
            // Check if the Tag property is not null and if the text in txtPicture.Tag contains a valid path
            if (txtPicture.Tag != null && !string.IsNullOrWhiteSpace(txtPicture.Tag.ToString()) && File.Exists(txtPicture.Tag.ToString()))
            {
                try
                {
                    // Load and display the image in the PictureBox
                    txtPicture.Image = Image.FromFile(txtPicture.Tag.ToString());

                    // Optional: Adjust PictureBox properties as needed, e.g., size mode
                    txtPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load image: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("No image has been selected or the file path is invalid.");
            }
        }

        private void dgvWorker_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Check if the clicked row index is valid
            {
                DataGridViewRow row = dgvWorker.Rows[e.RowIndex]; // Get the clicked row

                // Populate the form fields with values from the selected row
                txtId.Text = row.Cells["ID"].Value.ToString();
                txtName.Text = row.Cells["Name"].Value.ToString();
                cbGender.Text = row.Cells["Gender"].Value.ToString();
                txtPhone.Text = row.Cells["Phone Number"].Value.ToString();

                // Handle DBNull for DOB
                if (row.Cells["DOB"].Value != DBNull.Value)
                {
                    dtpWorker.Value = Convert.ToDateTime(row.Cells["DOB"].Value);
                }
                else
                {
                    // Set to current date or a placeholder if DOB is DBNull
                    dtpWorker.Value = DateTime.Now; // or some other default value
                }

                // Handle DBNull for Image
                if (row.Cells["Image"].Value != DBNull.Value)
                {
                    byte[] imageData = (byte[])row.Cells["Image"].Value;
                    using (MemoryStream ms = new MemoryStream(imageData))
                    {
                        txtPicture.Image = Image.FromStream(ms);
                        txtPicture.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                }
                else
                {
                    txtPicture.Image = null;
                }
            }
        }

        private void Addworker_Load(object sender, EventArgs e)
        {
            GetCustomer();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Please select a worker to edit.");
                return;
            }

            string query = "UPDATE Worker SET Name = ?, Gender = ?, [Phone Number] = ?, [DOB] = ?, [Image] = ? WHERE ID = ?";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\teh75\source\repos\Pharmacy Management System\Pharmacy Management System\Pharmacy.accdb"))
                {
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // Set parameters explicitly
                        cmd.Parameters.Add("?", OleDbType.VarChar).Value = txtName.Text;
                        cmd.Parameters.Add("?", OleDbType.VarChar).Value = cbGender.Text;

                        int phoneNum;
                        if (int.TryParse(txtPhone.Text, out phoneNum))
                        {
                            cmd.Parameters.Add("?", OleDbType.Integer).Value = phoneNum;
                        }
                        else
                        {
                            MessageBox.Show("Invalid phone number format.");
                            return;
                        }

                        cmd.Parameters.Add("?", OleDbType.Date).Value = dtpWorker.Value;

                        byte[] imageData = null;
                        if (!string.IsNullOrWhiteSpace(txtPicture.Tag?.ToString()) && File.Exists(txtPicture.Tag.ToString()))
                        {
                            imageData = ImageToByteArray(txtPicture.Tag.ToString());
                        }
                        cmd.Parameters.Add("?", OleDbType.Binary).Value = imageData ?? (object)DBNull.Value;

                        // Ensure the ID is an integer
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = int.Parse(txtId.Text);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Worker updated successfully.");
                            GetCustomer(); // Refresh DataGridView
                        }
                        else
                        {
                            MessageBox.Show("No worker was updated. Please check the details.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update worker: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // Check if an ID is selected
            if (string.IsNullOrEmpty(txtId.Text))
            {
                MessageBox.Show("Please select a worker to delete.");
                return;
            }

            string query = "DELETE FROM Worker WHERE ID = ?";

            try
            {
                using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\teh75\source\repos\Pharmacy Management System\Pharmacy Management System\Pharmacy.accdb"))
                {
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // Add the ID parameter to the command
                        cmd.Parameters.Add("?", OleDbType.Integer).Value = int.Parse(txtId.Text);

                        conn.Open();
                        int result = cmd.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Worker deleted successfully.");
                            GetCustomer(); // Refresh the DataGridView to show updated data
                        }
                        else
                        {
                            MessageBox.Show("No worker was deleted. Please check the details.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to delete worker: " + ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            // Clear text fields
            txtId.Text = "";
            txtName.Text = "";
            txtPhone.Text = "";

            // Reset the Gender ComboBox to its default state (assuming the first item is the default)
            if (cbGender.Items.Count > 0)
                cbGender.SelectedIndex = 0; // or cbGender.Text = "Select Gender"; if you have a placeholder like this

            // Reset the Date of Birth picker to the current date or a specific default date
            dtpWorker.Value = DateTime.Now; // or use a default date like DateTime.Parse("01/01/2000");

            // Clear the PictureBox and reset the image tag
            txtPicture.Image = null;
            txtPicture.Tag = null;

            // Optionally clear any error messages or status labels if you have them
            // lblStatus.Text = "";

            // Inform the user that the fields have been cleared
            MessageBox.Show("Form cleared successfully.");
        }
    }
}
