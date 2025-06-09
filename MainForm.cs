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
using System.Xml.Linq;
using static System.Net.Mime.MediaTypeNames;

namespace Pharmacy_Management_System
{
    public partial class MainForm : Form
    {
      

        public MainForm()
        {
            InitializeComponent();

        }

       

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
        
        }

        private void DashBoa_Click(object sender, EventArgs e)
        {
            // Create an instance of AddUser form
            Addworker AddworkerForm = new Addworker();

            // Show the AddUser form
            AddworkerForm.Show();

            // Optionally, you can hide the MainForm
            this.Hide();
        }

        private void AddProd_Click(object sender, EventArgs e)
        {
            // Create an instance of AddUser form
            AddProduct addProductForm = new AddProduct();

            // Show the AddUser form
            addProductForm.Show();

            // Optionally, you can hide the MainForm
            this.Hide();
        }

        private void close_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // Create an instance of Form1 form
            Form1 form1Form = new Form1();

            // Show the Form1 form
            form1Form.Show();

            // Optionally, you can hide the MainForm
            this.Hide();

            // Display a message indicating successful logout
            MessageBox.Show("Logout successful.");
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
           
        }

        private void Databasebtn_Click(object sender, EventArgs e)
        {
            // Create an instance of AddUser form
            Database DatabaseForm = new Database();

            // Show the AddUser form
            DatabaseForm.Show();

            // Optionally, you can hide the MainForm
            this.Hide();
        }

        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }
    }
}
