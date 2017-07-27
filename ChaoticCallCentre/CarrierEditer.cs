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
    public partial class CarrierEditer : Form
    {

/// <summary>
/// Declare required Variables
/// </summary>
#region VARIABLES

        DataSet carrierDataSet = new DataSet();
        bool blnLoaded = false;

#endregion

/// <summary>
/// Handles all initial actions when the program is initialized
/// </summary>
#region INITIALIZE

        public CarrierEditer()
        {
            InitializeComponent();
        }

        private void CarrierEditer_Load(object sender, EventArgs e)
        {
            carrierDataSet.Tables.Add("Enabled");
            carrierDataSet.Tables.Add("Disabled");
        }

#endregion

/// <summary>
/// All database loading methods
/// </summary>
#region LOAD

        // Load database tables into dataSet
        private void LoadTables()
        {
            try
            {
                carrierDataSet.Clear();
                var connection = new SqlConnection();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;

                connection.Open();
                var enabledCarrierAdapter = new SqlDataAdapter();
                // load enabled carriers
                enabledCarrierAdapter.SelectCommand = new SqlCommand("SELECT Carrier FROM tblCarriers WHERE Enabled = 'true'", connection);
                enabledCarrierAdapter.Fill(carrierDataSet, "Enabled");
                txtEnabledCarriers.DataSource = carrierDataSet.Tables[0];
                txtEnabledCarriers.DisplayMember = "Carrier";

                var disabledCarrierAdapter = new SqlDataAdapter();
                // load disabled carriers
                disabledCarrierAdapter.SelectCommand = new SqlCommand("SELECT Carrier FROM tblCarriers WHERE Enabled = 'false'", connection);
                disabledCarrierAdapter.Fill(carrierDataSet, "Disabled");
                txtDisabledCarriers.DataSource = carrierDataSet.Tables[1];
                txtDisabledCarriers.DisplayMember = "Carrier";
                connection.Close();
                blnLoaded = true;
            }
            catch (Exception errorTxt)
            {

            }
        }

#endregion

/// <summary>
/// All database updating methods
/// </summary>
#region UPDATE

        // update currently selected enabled carrier and set enabled to false
        private void DisableCurrentCarrier()
        {
            var connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;

            connection.Open();
            SqlCommand cmd = new SqlCommand("UPDATE tblCarriers SET Enabled = 'false' WHERE Carrier = '" + txtEnabledCarriers.Text + "'", connection);
            cmd.ExecuteNonQuery();
            LoadTables();
        }

        // update currently selected disabled carrier and set enabled to true
        private void EnableCurrentCarrier()
        {
            var connection = new SqlConnection();
            connection.ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;

            connection.Open();
            SqlCommand cmd = new SqlCommand("UPDATE tblCarriers SET Enabled = 'true' WHERE Carrier = '" + txtDisabledCarriers.Text + "'", connection);
            cmd.ExecuteNonQuery();
            LoadTables();
        }

#endregion

/// <summary>
/// Click events
/// </summary>
#region EVENTS

        private void btnLoad_Click(object sender, EventArgs e)
        {
            LoadTables();
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            DisableCurrentCarrier();
        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            EnableCurrentCarrier();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (blnLoaded)
            {
                ChangeCarrierName frmChangeCarrierName = new ChangeCarrierName();
                frmChangeCarrierName.CarrierName = txtEnabledCarriers.Text;
                frmChangeCarrierName.Show();
            }
            else
            {
                MessageBox.Show("Please press the load button before attempting to edit a carrier.", "Error!");
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            NewCarrierName newCarrierName = new NewCarrierName();
            newCarrierName.Show();
        }

#endregion

    }
}