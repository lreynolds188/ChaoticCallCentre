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
    public partial class CallCentre : Form
    {

/// <summary>
/// Declare required Variables
/// </summary>
#region VARIABLES

            int maxIndex = 0;
            int currentIndex = 0;
            string strAscOrDesc = "ASC";
            int intWhere;

            bool blnSort = false;
            bool blnNew = false;
            bool blnToggled = false;

            DataSet dataSet = new DataSet();
            DataRow dataRow;

            DataSet enabledCarrierDataSet = new DataSet();

            DataSet searchedDataSet = new DataSet();
            DataRow searchedDataRow;

            DataSet carrierDataSet = new DataSet();
            DataRow carrierDataRow;

#endregion

/// <summary>
/// Handles all initial actions when the program is initialized
/// </summary>
#region INITIALIZE

            public CallCentre()
            {
                InitializeComponent();
            }

            public string UserName { get; set; }

            private void CallCentre_Load(object sender, EventArgs e)
            {
                // add required tables to the dataset
                dataSet.Tables.Add("firstName");
                dataSet.Tables.Add("lastName");
                dataSet.Tables.Add("dOB");
                dataSet.Tables.Add("description");
                dataSet.Tables.Add("carrier");
                dataSet.Tables.Add("enabled");
                enabledCarrierDataSet.Tables.Add("enabledCarriers");
                carrierDataSet.Tables.Add("carrier");
                
                FillCarrierBox();
            
                txtUser.Text = UserName;
            
                // if the user is an admin enable admin options
                if (UserName.ToLower() == "admin")
                {
                    btnEdit.Show();
                    btnEdit.Enabled = true;
                    boxEnabled.Show();
                    boxEnabled.Enabled = true;
                } 
                else
                {
                    btnEdit.Hide();
                    boxEnabled.Hide();
                }
            }

#endregion

/// <summary>
/// All database updating methods
/// </summary>
#region UPDATE

        private void SubmitEntry()
            {
                // if required fields are not empty
                if (txtFirstName.Text != "" && txtLastName.Text != "" && txtDescription.Text != "")
                {
                    try
                    {
                        string strSqlCommand;
                        var connection = new SqlConnection();
                        connection.ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;

                        connection.Open();
                        // if this is a new record in the system
                        if (blnNew == true)
                        {
                            strSqlCommand = "INSERT INTO [tblCalls] (FirstName, LastName, DoB, Description, CarrierID, Enabled) VALUES ('" + txtFirstName.Text + "', '" + txtLastName.Text + "', '" + dtpDOB.Value.ToShortDateString() + "', '" + txtDescription.Text + "', '" + ConvertToCarrierID() + "', 'true')";
                            currentIndex++;
                        }
                        // else this is a current record
                        else
                        {
                            // if records are not sorted
                            if (!blnSort)
                            {
                                intWhere = currentIndex;
                                SqlCommand updateCarrier = new SqlCommand("UPDATE tblCalls SET CarrierID = '" + ConvertToCarrierID() + "' WHERE CallID = '" + intWhere + "' + 1", connection);
                                updateCarrier.ExecuteNonQuery();
                                strSqlCommand = "UPDATE tblCalls SET FirstName = '" + txtFirstName.Text + "', LastName = '" + txtLastName.Text + "', DoB = '" + dtpDOB.Text + "', Description = '" + txtDescription.Text + "', Enabled = '" + !boxEnabled.Checked + "' WHERE CallID = '" + intWhere + "' + 1";
                            }
                            else
                            {
                                if (UserName.ToLower() == "admin")
                                {
                                    intWhere = maxIndex - (currentIndex + 1);
                                    SqlCommand updateCarrier = new SqlCommand("UPDATE tblCalls SET CarrierID = '" + ConvertToCarrierID() + "' WHERE CallID = '" + intWhere + "' + 1", connection);
                                    updateCarrier.ExecuteNonQuery();
                                    strSqlCommand = "UPDATE tblCalls SET FirstName = '" + txtFirstName.Text + "', LastName = '" + txtLastName.Text + "', DoB = '" + dtpDOB.Text + "', Description = '" + txtDescription.Text + "', Enabled = '" + !boxEnabled.Checked + "' WHERE CallID = '" + intWhere + "' + 1";
                                }
                                else
                                {
                                    intWhere = maxIndex - currentIndex;
                                    SqlCommand updateCarrier = new SqlCommand("UPDATE tblCalls SET CarrierID = '" + ConvertToCarrierID() + "' WHERE CallID = '" + intWhere + "' + 1", connection);
                                    updateCarrier.ExecuteNonQuery();
                                    strSqlCommand = "UPDATE tblCalls SET FirstName = '" + txtFirstName.Text + "', LastName = '" + txtLastName.Text + "', DoB = '" + dtpDOB.Text + "', Description = '" + txtDescription.Text + "', Enabled = '" + !boxEnabled.Checked + "' WHERE CallID = '" + intWhere + "'";
                                }
                            }
                        }
                        SqlCommand cmd = new SqlCommand(strSqlCommand, connection);
                        cmd.ExecuteNonQuery();
                        blnNew = false;
                        btnSubmit.Text = "UPDATE";
                        connection.Close();

                        LoadTables();

                        LoadCurrentIndex(currentIndex);
                    }
                    catch (Exception errorTxt)
                    {
                        MessageBox.Show("An error has occured, please contact your systems administrator.\n\nERROR: " + errorTxt.Message, "Error");
                    }
                }
                else
                {
                    MessageBox.Show("A null field has been detected, please fill all required fields!", "Error");
                }
            }

        // Inverts the enabled status of the current record 
        private void InvertCarrierStatus()
        {
            try
            {
                if (!blnNew)
                {
                    blnToggled = true;
                    var connection = new SqlConnection();
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;

                    connection.Open();
                    SqlCommand cmd = new SqlCommand("UPDATE tblCalls SET Enabled = ~Enabled WHERE FirstName = '" + txtFirstName.Text + "'", connection);
                    cmd.ExecuteNonQuery();
                    connection.Close();
                }
            }
            catch
            {

            }

        }

#endregion

/// <summary>
/// All database loading methods
/// </summary>
#region LOAD

        // Load database tables into DataSets
        private void LoadTables()
            {
                dataSet.Clear();

                var connection = new SqlConnection();
                var firstNameAdapter = new SqlDataAdapter();
                var lastNameAdapter = new SqlDataAdapter();
                var dateOfBirthAdapter = new SqlDataAdapter();
                var descriptionAdapter = new SqlDataAdapter();
                var carrierAdapter = new SqlDataAdapter();
                var enabledAdapter = new SqlDataAdapter();
                var callIDAdapter = new SqlDataAdapter();

                connection.ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;
                connection.Open();

                // if the user is an admin, load admin fields from DB
                if (UserName.ToLower() == "admin")
                {
                    firstNameAdapter.SelectCommand = new SqlCommand("SELECT FirstName FROM tblCalls ORDER BY CallID "+ strAscOrDesc +"", connection);
                    lastNameAdapter.SelectCommand = new SqlCommand("SELECT LastName FROM tblCalls ORDER BY CallID "+ strAscOrDesc +"", connection);
                    dateOfBirthAdapter.SelectCommand = new SqlCommand("SELECT DoB FROM tblCalls ORDER BY CallID "+ strAscOrDesc +"", connection);
                    descriptionAdapter.SelectCommand = new SqlCommand("SELECT Description FROM tblCalls ORDER BY CallID "+ strAscOrDesc +"", connection);
                    carrierAdapter.SelectCommand = new SqlCommand("SELECT dbo.tblCarriers.Carrier FROM dbo.tblCalls INNER JOIN dbo.tblCarriers ON dbo.tblCalls.CarrierID = dbo.tblCarriers.CarrierID ORDER BY CallID " + strAscOrDesc + "", connection);
                    enabledAdapter.SelectCommand = new SqlCommand("SELECT Enabled FROM tblCalls ORDER BY CallID " + strAscOrDesc + "", connection);
                    callIDAdapter.SelectCommand = new SqlCommand("SELECT CallID FROM tblCalls ORDER BY CallID " +strAscOrDesc+ "", connection);
                } 
                // else load regular user fields
                else
                {
                    firstNameAdapter.SelectCommand = new SqlCommand("SELECT FirstName FROM tblCalls WHERE Enabled = 'true' ORDER BY CallID "+strAscOrDesc+"", connection);
                    lastNameAdapter.SelectCommand = new SqlCommand("SELECT LastName FROM tblCalls WHERE Enabled = 'true' ORDER BY CallID  " + strAscOrDesc + "", connection);
                    dateOfBirthAdapter.SelectCommand = new SqlCommand("SELECT DoB FROM tblCalls WHERE Enabled = 'true' ORDER BY CallID  " + strAscOrDesc + "", connection);
                    descriptionAdapter.SelectCommand = new SqlCommand("SELECT Description FROM tblCalls WHERE Enabled = 'true' ORDER BY CallID  " + strAscOrDesc + "", connection);
                    carrierAdapter.SelectCommand = new SqlCommand("SELECT dbo.tblCarriers.Carrier FROM dbo.tblCalls INNER JOIN dbo.tblCarriers ON dbo.tblCalls.CarrierID = dbo.tblCarriers.CarrierID ORDER BY CallID " + strAscOrDesc + "", connection);
                    callIDAdapter.SelectCommand = new SqlCommand("SELECT CallID FROM tblCalls ORDER BY CallID " + strAscOrDesc + "", connection);
                }

                // fill corresponding dataSets
                firstNameAdapter.Fill(dataSet, "firstName");
                lastNameAdapter.Fill(dataSet, "lastName");
                dateOfBirthAdapter.Fill(dataSet, "dOB");
                descriptionAdapter.Fill(dataSet, "description");
                carrierAdapter.Fill(dataSet, "carrier");
                callIDAdapter.Fill(dataSet, "callID");
                if (UserName.ToLower() == "admin")
                {
                    enabledAdapter.Fill(dataSet, "enabled");
                }

                // set the maxIndex
                maxIndex = dataSet.Tables[0].Rows.Count;

                LoadCurrentIndex(currentIndex);
                btnSubmit.Text = "UPDATE";
                connection.Close();
            }

        // Display current record on screen
        private void LoadCurrentIndex(int x)
            {
                try
                {
                    // load each variable of the current record one by one into the dataRow and set the field on screen to match
                    dataRow = dataSet.Tables[0].Rows[x];
                    txtFirstName.Text = dataRow.ItemArray.GetValue(0).ToString();

                    dataRow = dataSet.Tables[1].Rows[x];
                    txtLastName.Text = dataRow.ItemArray.GetValue(0).ToString();

                    dataRow = dataSet.Tables[2].Rows[x];
                    dtpDOB.Value = DateTime.Parse(dataRow.ItemArray.GetValue(0).ToString());

                    dataRow = dataSet.Tables[3].Rows[x];
                    txtDescription.Text = dataRow.ItemArray.GetValue(0).ToString();

                    dataRow = dataSet.Tables[4].Rows[x];
                    txtCarrier.Text = dataRow.ItemArray.GetValue(0).ToString();

                    if (UserName.ToLower() == "admin")
                    {
                        if (!blnToggled)
                        {
                            dataRow = dataSet.Tables[5].Rows[x];
                            if (dataRow.ItemArray.GetValue(0).ToString().ToLower() == "false")
                            {
                                boxEnabled.Checked = true;
                            }
                            else
                            {
                                boxEnabled.Checked = false;
                            }
                        }
                        blnToggled = false;

                    }
                }
                catch
                {

                }
            }

        // Fills the carrier drop box with the carrier list from the database
        private void FillCarrierBox()
            {
                try
                {
                    enabledCarrierDataSet.Clear();
                    var connection = new SqlConnection();
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;

                    var enabledCarrierAdapter = new SqlDataAdapter();

                    connection.Open();
                    enabledCarrierAdapter.SelectCommand = new SqlCommand("SELECT Carrier FROM tblCarriers WHERE Enabled = 'true'", connection);
                    enabledCarrierAdapter.Fill(enabledCarrierDataSet, "enabledCarriers");
                    txtCarrier.DataSource = enabledCarrierDataSet.Tables[0];
                    txtCarrier.DisplayMember = "Carrier";
                    connection.Close();
                }
                catch
                {

                }
            }

        // Converts the text in the carrier field to the corresponding ID in the database
        private int ConvertToCarrierID()
            {
                try
                {
                    carrierDataSet.Clear();
                    var connection = new SqlConnection();
                    connection.ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;

                    var carrierAdapter = new SqlDataAdapter();

                    connection.Open();
                    carrierAdapter.SelectCommand = new SqlCommand("SELECT CarrierID FROM tblCarriers WHERE Carrier = '" + txtCarrier.Text + "'", connection);
                    carrierAdapter.Fill(carrierDataSet, "carrier");
                    
                    carrierDataRow = carrierDataSet.Tables[0].Rows[0];
                    return Int32.Parse(carrierDataRow.ItemArray.GetValue(0).ToString());
                }
                catch
                {
                    MessageBox.Show("Invalid Carrier Name", "Error!");
                    return 0;
                }
            }

#endregion

/// <summary>
/// All index navigation methods
/// </summary>
#region NAVIGATE

        // Move to the last index + 1 and clear all fields
        private void NewIndex()
        {
                btnSubmit.Text = "SUBMIT FEEDBACK";
                currentIndex = maxIndex;
                blnNew = true;
                txtFirstName.Text = "";
                txtLastName.Text = "";
                dtpDOB.Value = DateTime.Parse("1/01/1990");
                txtDescription.Text = "";
                txtCarrier.Text = "";
                boxEnabled.Checked = false;
        }

        // Load the first record in the dataSet onto the screen
        private void FirstIndex()
        {
                currentIndex = 0;
                blnNew = false;
                btnSubmit.Text = "UPDATE";
                LoadCurrentIndex(currentIndex);
        }

        // Load the last record in the dataSet onto the screen
        private void LastIndex()
        {
                currentIndex = maxIndex - 1;
                blnNew = false;
                btnSubmit.Text = "UPDATE";
                LoadCurrentIndex(currentIndex);
        }

        // Load the next record in the dataSet onto the screen
        private void NextIndex()
        {
            // if not at the last record
            if (currentIndex < maxIndex - 1)
            {
                currentIndex++;
                LoadCurrentIndex(currentIndex);
            } 
            else 
            {
                MessageBox.Show("There are no more records!", "Error");
            }
        }

        // Load the previous record in the dataSet onto the screen
        private void PreviousIndex()
        {
            // if not at the first record
            if (currentIndex != 0)
            {
                currentIndex--;
                blnNew = false;
                btnSubmit.Text = "UPDATE";
                LoadCurrentIndex(currentIndex);
            }
            else
            {
                MessageBox.Show("You are at the first index", "Error");
            }
        }

        // Search the database for the search term and display on screen and set current index
        private void SearchIndex()
        {
            try
            {
                // incase tables are not loaded, load them
                LoadTables();

                searchedDataSet.Clear();

                var connection = new SqlConnection();
                connection.ConnectionString = ConfigurationManager.ConnectionStrings[1].ConnectionString;
                
                connection.Open();
                var firstName = new SqlDataAdapter();
                firstName.SelectCommand = new SqlCommand("SELECT * FROM tblCalls WHERE FirstName = '"+ txtSearch.Text +"' AND Enabled = 'true'", connection);
                firstName.Fill(searchedDataSet, "IndexFound");
                searchedDataRow = searchedDataSet.Tables[0].Rows[0];
                
                currentIndex = Int32.Parse(searchedDataRow.ItemArray.GetValue(0).ToString()) - 1;
                txtFirstName.Text = searchedDataRow.ItemArray.GetValue(1).ToString();
                txtLastName.Text = searchedDataRow.ItemArray.GetValue(2).ToString();
                dtpDOB.Value = DateTime.Parse(searchedDataRow.ItemArray.GetValue(3).ToString());
                txtDescription.Text = searchedDataRow.ItemArray.GetValue(4).ToString();
                txtCarrier.Text = searchedDataRow.ItemArray.GetValue(5).ToString();
                connection.Close();
            }
            catch
            {
                MessageBox.Show("No matching index found, Ensure you are searching the correct first name.", "Error!");
            }
        }

        // Sets a boolean sort to true which changes the way the load and update functions work
        private void Sort()
        {
            if (!blnNew)
            {
                blnSort = !blnSort;

                if (blnSort == false)
                {
                    btnSort.Text = "Sort Dsc";
                    strAscOrDesc = "ASC";
                }
                else
                {
                    btnSort.Text = "Sort Asc";
                    strAscOrDesc = "DESC";
                }
                LoadTables();
            }
            else
            {
                MessageBox.Show("Cannot sort data while creating new entry.", "Error!");
            }
        } 

    #endregion

/// <summary>
/// Click events
/// </summary>
#region EVENTS

            private void btnLoad_Click(object sender, EventArgs e)
            {
                if (blnNew == false)
                {
                    LoadTables();
                }     
            }
        
            private void btnSubmit_Click(object sender, EventArgs e)
            {
                try
                {
                    if (dataSet.Tables[0].Rows[0] != null)
                    {
                        SubmitEntry();
                    }
                }
                catch
                {
                    MessageBox.Show("Please press the new button to make a new entry", "Error!");
                }
            }

            private void btnNext_Click(object sender, EventArgs e)
            {
                NextIndex();
            }

            private void btnPrev_Click(object sender, EventArgs e)
            {
                PreviousIndex();
            }

            private void btnFirst_Click(object sender, EventArgs e)
            {
                FirstIndex();
            }

            private void btnLast_Click(object sender, EventArgs e)
            {
                LastIndex();
            }

            private void btnNew_Click(object sender, EventArgs e)
            {
                LoadTables();
                NewIndex();
            }

            private void btnSearch_Click(object sender, EventArgs e)
            {
                SearchIndex();
            }

            private void btnSort_Click(object sender, EventArgs e)
            {
                Sort();
            }
        
            private void btnLogout_Click(object sender, EventArgs e)
            {
                this.Hide();
                LoginForm myLoginForm = new LoginForm();
                myLoginForm.Show();
            }

            private void btnEdit_Click(object sender, EventArgs e)
            {
                CarrierEditer myCarrierEditer = new CarrierEditer();
                myCarrierEditer.Show();
            }
        
            private void txtCarrier_Enter(object sender, EventArgs e)
            {
                FillCarrierBox();
            }

#endregion

    }
}
