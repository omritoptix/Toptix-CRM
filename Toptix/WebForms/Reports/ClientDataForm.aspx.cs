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

namespace Toptix.WebForms.Reports
{
    public partial class ClientDataForm : System.Web.UI.Page
    {
        public string thisConnectionString =
        ConfigurationManager.ConnectionStrings[
        "TCRMDBContext"].ConnectionString;

        public SqlParameter[] SearchValue = new SqlParameter[6];

        protected void Page_Load(object sender, EventArgs e)
        {
            ClientDataReportViewer.LocalReport.ReportPath = Server.MapPath("~/Reports/ClientDataReport.rdlc");
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //ReportViewer1.Visible is set to false in design mode
            ClientDataReportViewer.Visible = true;
            //define connection string
            SqlConnection thisConnection = new SqlConnection(thisConnectionString);
            System.Data.DataSet thisDataSet = new System.Data.DataSet();
            //Get user Parameters
            SearchValue[0] = new SqlParameter("@startDate",
                           orderDateFromTB.Text);

            SearchValue[1] = new SqlParameter("@endtDate",
                 orderDateToTB.Text);

            SearchValue[2] = new SqlParameter("@clientName",
                ClientDDL.SelectedValue);

            SearchValue[3] = new SqlParameter("@distributorName",
                DistributorDDL.SelectedValue);

            SearchValue[4] = new SqlParameter("@clientCountry",
                ClientCountryDDL.SelectedValue);

            SearchValue[5] = new SqlParameter("@product",
                ProductDDL.SelectedValue);

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



            /*Add parameters to the SQL Stored Procedure*/
            SqlCommand mySqlCommand = thisConnection.CreateCommand();
            mySqlCommand.CommandText = "ClientDataReport";
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
                mySqlCommand.Parameters.Add("@product", SqlDbType.VarChar).Value = SearchValue[5].Value.ToString();


            SqlDataAdapter mySqlDataAdapter = new SqlDataAdapter();
            mySqlDataAdapter.SelectCommand = mySqlCommand;
            //DataSet myDataSet = new DataSet();
            thisConnection.Open();
            //load dataset with stored procedure result
            mySqlDataAdapter.Fill(thisDataSet);

            /********************************************************/


            /* Associate thisDataSet  (now loaded with the stored 
               procedure result) with the  ReportViewer datasource */
            ReportDataSource datasource = new
              ReportDataSource("ClientDataReportDataSet",
              thisDataSet.Tables[0]);

            ClientDataReportViewer.LocalReport.DataSources.Clear();
            ClientDataReportViewer.LocalReport.DataSources.Add(datasource);
            //if (thisDataSet.Tables[0].Rows.Count == 0)
            //{

            //    lblMessage.Text = "Sorry, no products under this category!";
            //}

            ClientDataReportViewer.LocalReport.Refresh();
        }
    }
}