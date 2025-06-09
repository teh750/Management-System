using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Pharmacy_Management_System
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void login_password_TextChanged(object sender, EventArgs e)
        {

        }

        private void login_showPass_CheckedChanged(object sender, EventArgs e)
        {
            login_password.PasswordChar = login_showPass.Checked ? '\0' : '*';
        }
        private void loginButton_Click(object sender, EventArgs e)
        {
            string username = login_username.Text;
            string password = login_password.Text;

            if (username == "admin" && password == "admin")
            {
                MessageBox.Show("Login successful!");

                // Create an instance of MainForm
                MainForm mainForm = new MainForm();

                // Show MainForm
                mainForm.Show();

                // Optional: Hide this login form instead of closing it
                this.Hide();

                // Or, if you want to close the login form, you can use:
                // this.Close();
            }
            else
            {
                MessageBox.Show("Login unsuccessful. Please check your username and password.");
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
    }