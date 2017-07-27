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
    public partial class NewCarrierName : Form
    {

/// <summary>
/// Declare required Variables
/// </summary>
#region VARIABLES

        DataSet carrierDataSet = new DataSet();

#endregion

/// <summary>
/// Handles all initial actions when the program is initialized
/// </summary>
#region INITIALIZE

        public NewCarrierName()
        {
            InitializeComponent();
            carrierDataSet.Tables.Add("Carrier");
        }

#endregion

/// <summary>
/// All database updating methods
/// </summary>
#region INSERT

        // insert a new carrier into the database with the name given by its required field
        private void CreateNewCarrier()
        {
            try
            {
                var connection = new SqlConnection();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;

                connection.Open();
                SqlCommand cmd = new SqlCommand("INSERT INTO tblCarriers (Carrier, Enabled) VALUES ('" + txtCarrierName.Text + "', 'true')", connection);
                cmd.ExecuteNonQuery();
                connection.Close();
                this.Hide();
            }
            catch (Exception errorTxt)
            {

            }
        }

        #endregion

/// <summary>
/// All database loading methods
/// </summary>
#region SELECT

        // checks to see if the new carrier already exists in the database
        private bool CheckNoDuplicates()
        {
            carrierDataSet.Clear();
            var connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;
            var carrierDataAdapter = new SqlDataAdapter();
            
            connection.Open();
            carrierDataAdapter.SelectCommand = new SqlCommand("SELECT * FROM tblCarriers WHERE Carrier = '"+ txtCarrierName.Text +"'", connection);
            carrierDataAdapter.Fill(carrierDataSet, "Carrier");
            connection.Close();

            try
            {
                // if carrier already exists
                if (carrierDataSet.Tables[0].Rows[0] != null)
                {
                    MessageBox.Show("That carrier name already exists.", "Error!");
                    return false; 
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }
        }

#endregion

/// <summary>
/// Click events
/// </summary>
#region EVENTS

        private void btnCreateCarrier_Click(object sender, EventArgs e)
        {
            if (txtCarrierName.Text != "" && CheckNoDuplicates())
            {
                MessageBox.Show("Success, Please press the load button to see any newly added carriers.", "Success!");
                CreateNewCarrier();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        #endregion

    }
}