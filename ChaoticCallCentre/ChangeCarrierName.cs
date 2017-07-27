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
    public partial class ChangeCarrierName : Form
    {

/// <summary>
/// Declare required Variables
/// </summary>
#region VARIABLES

        public string CarrierName { get; set; }

#endregion

/// <summary>
/// Handles all initial actions when the program is initialized
/// </summary>
#region INITIALIZE

        public ChangeCarrierName()
        {
            InitializeComponent();
        }
        
        private void ChangeCarrierName_Load(object sender, EventArgs e)
        {
            lblHeading.Text = "Please enter the new name for the carrier: "+CarrierName+".";
        }
        
#endregion

/// <summary>
/// All database updating methods
/// </summary>
#region UPDATE

        // update the currently selected enabled carrier with the name given in its text field
        private void UpdateCarrierName()
        {
            try
            {
                var connection = new SqlConnection();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;

                connection.Open();
                SqlCommand cmd = new SqlCommand("UPDATE tblCarriers SET Carrier = '"+txtCarrierName.Text+"' WHERE Carrier = '"+CarrierName+"'", connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                MessageBox.Show("Success, Please press the load button to show any updated carriers.", "Success!");  
            }
            catch
            {
                MessageBox.Show("An error has occured please contact your systems administrator.", "Error!"); 
            } 
        }

#endregion

/// <summary>
/// Click events
/// </summary>
#region EVENTS

        private void btnUpdateCarrier_Click(object sender, EventArgs e)
        {
            if (txtCarrierName.Text != "")
            {
                UpdateCarrierName();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Null or whitespace detected, please ensure all required fields are filled!", "Error!");
            }
            
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

#endregion

    }
}
