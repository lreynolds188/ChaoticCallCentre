using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ChaoticCallCentre
{
    public partial class LoginForm : Form
    {
   
/// <summary>
/// Handles all initial actions when the program is initialized
/// </summary>
#region INITIALIZE
     
        public LoginForm()
        {
            InitializeComponent();
        }

#endregion

/// <summary>
/// Validate the entered username and password against the database
/// </summary>
#region VALIDATE
        
        // Validate username and password against the database
        private void Login()
        {
            if (txtUsername.Text != "" && txtPassword.Text != "")
            {
                try
                {
                    // declare a new SqlConnection, DataTable and DataRow
                    var connection = new SqlConnection();
                    var dataTable = new DataTable();
                    DataRow row;

                    // create a new row in the DataRow
                    row = dataTable.NewRow();

                    // load connection string and open the connection
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;
                    connection.Open();

                    // declare a new SqlDataAdapter
                    SqlDataAdapter dataAdapter = new SqlDataAdapter();

                    // new SqlCommand that gets the password where the entered username and the username in the database match
                    dataAdapter.SelectCommand = new SqlCommand("SELECT Password FROM tblLogin WHERE Username = '" + txtUsername.Text + "'", connection);
                    dataAdapter.Fill(dataTable);
                    dataTable.Rows.Add(row);

                    // if the password matches the username's password in the system
                    if (dataTable.Rows[0][0].ToString().ToLower() == txtPassword.Text)
                    {
                        // load the main menu and send the username
                        CallCentre frmCallCentre = new CallCentre();
                        frmCallCentre.UserName = txtUsername.Text;
                        frmCallCentre.Show();
                        this.Hide();
                    }
                    else
                    {
                        MessageBox.Show("Incorrect Username/Password!", "Error");
                        dataTable.Rows.Remove(row);
                    }

                    // close the connection
                    connection.Close();
                }
                catch (Exception errorTxt)
                {
                    MessageBox.Show(errorTxt.ToString());
                }
            }
            else
            {
                MessageBox.Show("Null field detected, please fill all required fields!", "Error");
            }
        }

#endregion

/// <summary>
/// Click events
/// </summary>
#region EVENTS

        private void btnLogin_Click(object sender, EventArgs e)
        {
            Login();
        }

#endregion

    }
}
