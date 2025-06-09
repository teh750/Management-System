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

namespace Pharmacy_Management_System
{
    public partial class Database : Form
    {
        OleDbConnection conn;
        OleDbCommand cmd;
        OleDbDataAdapter adapter;
        DataTable dt;

        // Define variables to store product and worker data tables
        DataTable dtProduct;
        DataTable dtWorker;

        // Variable to store the currently displayed DataTable
        DataTable currentDataTable;


        public Database()
        {
            InitializeComponent();
        }

        void GetCustomer()
        {
            conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\teh75\source\repos\Pharmacy Management System\Pharmacy Management System\Pharmacy.accdb");
            dt = new DataTable(); // Initialize dt here
            adapter = new OleDbDataAdapter("SELECT * FROM [Product]", conn);
            conn.Open();
            adapter.Fill(dt);
            dataGridView1.DataSource = dt;
            conn.Close();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();

            // Show the MainForm
            MainForm mainForm = new MainForm();
            mainForm.Show();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void Database_Load(object sender, EventArgs e)
        {
            GetCustomer();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (currentDataTable != null)
            {
                // Apply search filter to the currently displayed DataTable
                DataView dv = currentDataTable.DefaultView;
                dv.RowFilter = "Name LIKE '%" + txtSearch.Text + "%'";
                dataGridView1.DataSource = dv;
            }
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            // Load product data if not loaded yet
            if (dtProduct == null)
            {
                dtProduct = LoadDataTable("SELECT * FROM [Product]");
            }

            // Set currentDataTable to dtProduct
            currentDataTable = dtProduct;

            // Display product data in the DataGridView
            dataGridView1.DataSource = dtProduct;
        }

        private void btnWorker_Click(object sender, EventArgs e)
        {
            // Load worker data if not loaded yet
            if (dtWorker == null)
            {
                dtWorker = LoadDataTable("SELECT * FROM [Worker]");
            }

            // Set currentDataTable to dtWorker
            currentDataTable = dtWorker;

            // Display worker data in the DataGridView
            dataGridView1.DataSource = dtWorker;
        }

        private DataTable LoadDataTable(string query)
        {
            DataTable dataTable = new DataTable();
            using (OleDbConnection conn = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.16.0;Data Source=C:\Users\teh75\source\repos\Pharmacy Management System\Pharmacy Management System\Pharmacy.accdb"))
            {
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(query, conn))
                {
                    conn.Open();
                    adapter.Fill(dataTable);
                }
            }
            return dataTable;
        }
    }
}
