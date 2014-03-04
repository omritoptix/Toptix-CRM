using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Toptix.Models;
using Toptix.Utilities;

namespace Toptix.WebForms.Reports
{
    public partial class ChargesReportForm : System.Web.UI.Page
    {
        public string thisConnectionString =
        ConfigurationManager.ConnectionStrings[
        "TCRMDBContext"].ConnectionString;

        public SqlParameter[] SearchValue = new SqlParameter[11];

        protected void Page_Load(object sender, EventArgs e)
        {
            ChargesReportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/ChargeReport.rdlc");

        }

        protected void RunReport_Click(object sender, EventArgs e)
        {

            //ReportViewer1.Visible is set to false in design mode
            TCRMDBContext db = new TCRMDBContext();
            ChargesReportViewer.Visible = true;
            //define connection string
            SqlConnection thisConnection = new SqlConnection(thisConnectionString);
            System.Data.DataSet thisDataSet = new System.Data.DataSet();
            //Get user Parameters
            SearchValue[0] = new SqlParameter("@startDate",
               chargeDateFromDatePicker.Text);

            SearchValue[1] = new SqlParameter("@endtDate",
                chargeDateToDatePicker.Text);

            SearchValue[2] = new SqlParameter("@clientName",
                ClientDDL.SelectedValue);

            SearchValue[3] = new SqlParameter("@distributorName",
                distributorDDL.SelectedValue);

            SearchValue[4] = new SqlParameter("@clientCountry",
                clientCountryDDL.SelectedValue);

            SearchValue[5] = new SqlParameter("@chargeFrequency",
                chargeFrequencyDDL.SelectedValue);

            SearchValue[6] = new SqlParameter("@isPaid",
               IsPaidCB.Checked);

            SearchValue[7] = new SqlParameter("@isValid",
                IsValidDDL.SelectedValue);

            SearchValue[8] = new SqlParameter("@isAlert",
               IsAlertCB.Checked);

            SearchValue[9] = new SqlParameter("@isInvoice",
               IsInvoiceCB.Checked);

            SearchValue[10] = new SqlParameter("@conversionRate",
            CurrencyDDL.SelectedValue);


            //check if the user chose "ALL"
            if (SearchValue[0].Value == "")
                SearchValue[0] = null;

            if (SearchValue[1].Value == "")
                SearchValue[1] = null;

            if (SearchValue[2].Value.ToString() == "ALL")
                SearchValue[2] = null;

            if (SearchValue[3].Value.ToString() == "ALL")
                SearchValue[3] = null;

            if (SearchValue[4].Value.ToString() == "ALL")
                SearchValue[4] = null;

            if (SearchValue[5].Value.ToString() == "ALL")
                SearchValue[5] = null;

            if (SearchValue[7].Value.ToString() == "ALL")
                SearchValue[7] = null;



            


            /*Add parameters to the SQL Stored Procedure*/
            SqlCommand mySqlCommand = thisConnection.CreateCommand();
            mySqlCommand.CommandText = "ChargesReport";
            mySqlCommand.CommandType = CommandType.StoredProcedure;

            //Add DropDownList Parameters to the SP
            if (SearchValue[0] != null)
                mySqlCommand.Parameters.Add("@startDate", SqlDbType.VarChar).Value = SearchValue[0].Value;
            if (SearchValue[1] != null)
                mySqlCommand.Parameters.Add("@endDate", SqlDbType.VarChar).Value = SearchValue[1].Value;
            if (SearchValue[2] != null)
                mySqlCommand.Parameters.Add("@clientName", SqlDbType.VarChar).Value = SearchValue[2].Value.ToString();
            if (SearchValue[3] != null)
                mySqlCommand.Parameters.Add("@distributorName", SqlDbType.VarChar).Value = SearchValue[3].Value.ToString();
            if (SearchValue[4] != null)
                mySqlCommand.Parameters.Add("@clientCountry", SqlDbType.VarChar).Value = SearchValue[4].Value.ToString();
            if (SearchValue[5] != null)
                mySqlCommand.Parameters.Add("@chargeFrequency", SqlDbType.VarChar).Value = SearchValue[5].Value.ToString();
            
            //calculate conversion rate           
            if (SearchValue[10].Value.ToString() == "System Currency")
                mySqlCommand.Parameters.Add("@conversionRate", SqlDbType.Real).Value = 1;
            else
            {
                int currencyID = Convert.ToInt32(SearchValue[10].Value);
                //if the system currency was chosen from the list (not as "System Currency") but as "Dollars" 
                //for example which is the current system Currency
                if (currencyID == db.GlobalParameteres.First().CurrencyID)
                    mySqlCommand.Parameters.Add("@conversionRate", SqlDbType.Real).Value = 1;
                else
                {
                    double conversionRate = OrderUtilities.caculateCurrentConversion(currencyID);
                    mySqlCommand.Parameters.Add("@conversionRate", SqlDbType.Real).Value = conversionRate;
                }
            }

            //define values to is valid based on DDL options
            if (SearchValue[7] != null)
            {
                if (SearchValue[7].Value.ToString() == "Yes")
                    mySqlCommand.Parameters.Add("@isValid", SqlDbType.Bit).Value = true;
                if (SearchValue[7].Value.ToString() == "No")
                    mySqlCommand.Parameters.Add("@isValid", SqlDbType.Bit).Value = false;
            }

            //Add checkbox parametes to the SP
            mySqlCommand.Parameters.Add("@isPaid", SqlDbType.Bit).Value = SearchValue[6].Value;
            mySqlCommand.Parameters.Add("@isAlert", SqlDbType.Bit).Value = SearchValue[8].Value;
            mySqlCommand.Parameters.Add("@isInvoice", SqlDbType.Bit).Value = SearchValue[9].Value;


            SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
            mySqlDataAdapter.SelectCommand = mySqlCommand;  
            thisConnection.Open();
            //load dataset with stored procedure result
            mySqlDataAdapter.Fill(thisDataSet);

            /********************************************************/


            /* Associate thisDataSet  (now loaded with the stored 
               procedure result) with the  ReportViewer datasource */
            ReportDataSource datasource = new
              ReportDataSource("ChargeReportDataSet",
              thisDataSet.Tables[0]);

            ChargesReportViewer.LocalReport.DataSources.Clear();
            ChargesReportViewer.LocalReport.DataSources.Add(datasource);
            ChargesReportViewer.LocalReport.Refresh();
        }










    }
}
