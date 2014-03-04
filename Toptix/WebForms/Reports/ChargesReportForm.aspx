<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChargesReportForm.aspx.cs" Inherits="Toptix.WebForms.Reports.ChargesReportForm" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Charges Report</title>
    <link href="../../Content/themes/base/jquery-ui.css" rel="stylesheet" />
    <link href="/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <meta name="viewport" content="width=device-width" />
    <link href="/Content/site1.css" rel="stylesheet"/>

    <script src="/Scripts/modernizr-2.5.3.js"></script>
</head>
 <body>
        <header>
            <div class="content-wrapper">
                <div class="float-left">
                      <p class="site-title">  <a href="/DataManagement">ToptixCRM</a></p>
                </div>

                <div class="float-right">
                    
                    <nav>
                        <ul id="menu">
                            <li><a href="/DataManagement">Data Management</a></li>
                            <li><a href="/Order">Orders</a></li>
                            <li><a href="/Report">Reports</a></li>
                            <li><a href="/Admin">Admin</a></li>   
                        </ul>
                    </nav>
                </div>
            </div>
        </header>
        
        <div id="body" >
            <div id="subheader">
                <div id="submenu">
                    
                </div>
            </div>
            <div class="content-wrapper">
               

     <div class="rightnav">
        <h2>Reports</h2>
         <h3>Main</h3>
        <ul class="sidenav">
            <li><a href="/WebForms/Reports/ChargesReportForm.aspx" <%--target="_blank"--%>>Charges</a></li>
            <li><a href="/WebForms/Reports/ClientDataForm.aspx" <%--target="_blank"--%>>Client Sales</a></li>
        </ul>
     </div>
                <section class="main-content clear-fix">
    <h2>Charges Report</h2>
    <form id="form1" runat="server" aria-expanded="undefined" >
    <div>
    
        <asp:SqlDataSource ID="Distributor" runat="server" ConnectionString="<%$ ConnectionStrings:TCRMDBContext %>" SelectCommand="SELECT * FROM [Distributor]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="Clients" runat="server" ConnectionString="<%$ ConnectionStrings:TCRMDBContext %>" SelectCommand="SELECT * FROM [Client]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="ChargeFrequencies" runat="server" ConnectionString="<%$ ConnectionStrings:TCRMDBContext %>" SelectCommand="SELECT * FROM [EnumChargeFrequency]"></asp:SqlDataSource>
    <div class="form-options">    
        <asp:Panel ID="Panel1" runat="server" >
            <div class="section1">
                <div class="row">
                    <div class="item" style="margin-right: 80px;">
                        <asp:Label ID="Label3" runat="server" Text="Client:"></asp:Label>
                        <asp:DropDownList ID="ClientDDL" runat="server" AppendDataBoundItems="true" DataSourceID="Clients" DataTextField="Name" DataValueField="Name">
                            <asp:ListItem Text="ALL" />
                        </asp:DropDownList>
                    </div>
                    <div class="item">
                        <asp:Label ID="Label4" runat="server" Text="Distributor:"></asp:Label>
                        <asp:DropDownList ID="distributorDDL" runat="server" AppendDataBoundItems="true" DataSourceID="Distributor" DataTextField="Name" DataValueField="Name">
                            <asp:ListItem Text="ALL" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="item">
                        <asp:Label ID="Label2" runat="server" Text="Client Country : "></asp:Label>
                        <asp:DropDownList ID="clientCountryDDL" runat="server" AppendDataBoundItems="True" DataSourceID="Countries" DataTextField="Description" DataValueField="Description">
                            <asp:ListItem Text="ALL" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <asp:CheckBox ID="IsPaidCB" runat="server" /><span class="padd">Is paid?</span>
                    
                    <asp:CheckBox ID="IsAlertCB" runat="server" /><span class="padd">Is Alert?</span>
                    
                    <asp:CheckBox ID="IsInvoiceCB" runat="server" /><span class="padd">Is Invoice?</span>
                    Is Valid 
                    <asp:DropDownList ID="IsValidDDL" runat="server" AppendDataBoundItems="True" style="margin-right: 5px;">
                    <asp:ListItem Text="ALL" />
                    <asp:ListItem Text="Yes" />
                    <asp:ListItem Text="No" />
                    </asp:DropDownList>
                </div>
                <div class="row" style="padding-top:10px;border-top:1px solid #ccc">
                    <div class="item" style="margin-right: 80px;">
                        <asp:Label ID="Label5" runat="server" Text="Charge Frequency:"></asp:Label>
                        <asp:DropDownList ID="chargeFrequencyDDL" runat="server" AppendDataBoundItems="true" DataSourceID="ChargeFrequencies" DataTextField="Description" DataValueField="Description">
                            <asp:ListItem Text="ALL" />
                        </asp:DropDownList>
                    </div>
                    <div class="item">
                        <asp:Label ID="Label7" runat="server" Text="Currency :"></asp:Label>
                        <asp:DropDownList ID="CurrencyDDL" runat="server" DataSourceID="AvailableCurrencies" DataTextField="CurrencyFullDescription" DataValueField="ID" AppendDataBoundItems="True" Width="130px">
                            <asp:ListItem Text="System Currency" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row" style="padding-top:10px;border-top:1px solid #ccc">
                    <asp:Label ID="Label1" runat="server" Text="Charges From:"></asp:Label>
                    <%--<asp:DropDownList ID="chargeDateFromDDL" runat="server" AppendDataBoundItems="True" DataSourceID="ChargeReportDataSource" DataTextField="ChargeDate" DataTextFormatString="{0:d}" DataValueField="ChargeDate" Height="16px">
                        <asp:ListItem Text="ALL" />
                    </asp:DropDownList>--%>
                    <asp:TextBox ID="chargeDateFromDatePicker" runat="server"></asp:TextBox>
                    <asp:Label ID="Label6" runat="server" Text="To:"></asp:Label>
                    <asp:TextBox id="chargeDateToDatePicker" runat="server"></asp:TextBox>
                    <asp:SqlDataSource ID="AvailableCurrencies" runat="server" ConnectionString="<%$ ConnectionStrings:TCRMDBContext %>" SelectCommand="SELECT DISTINCT Currency.Description + ' ' + Currency.Country AS CurrencyFullDescription, Currency.ID FROM Currency INNER JOIN ConversionRate ON ConversionRate.CurrencyID = Currency.ID WHERE (ConversionRate.Date &lt; GETDATE()) UNION SELECT Currency_1.Description + ' ' + Currency_1.Country AS CurrencyFullDescription, Currency_1.ID FROM Currency AS Currency_1 INNER JOIN GlobalParameters ON Currency_1.ID = GlobalParameters.CurrencyID"></asp:SqlDataSource>
                    <asp:Button ID="RunReport" runat="server" Font-Bold="True" OnClick="RunReport_Click" style="height: 26px" Text="Run Report" />
                </div>
        </asp:Panel>
    </div>
        <asp:SqlDataSource ID="Countries" runat="server" ConnectionString="<%$ ConnectionStrings:TCRMDBContext %>" SelectCommand="SELECT * FROM [Country]"></asp:SqlDataSource>
    
    </div>
        <rsweb:ReportViewer ID="ChargesReportViewer" runat="server" Font-Names="Verdana" Font-Size="8pt" Visible="False" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="8.5in" Height="841px" BorderStyle="Double" BackColor="#CCCCCC" SplitterBackColor="White">
            <LocalReport ReportEmbeddedResource="Toptix.Reports.ChargeReport.rdlc">
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="GetData" TypeName="Toptix.App_Code.ChargeReportTableAdapters.ChargesReportTableAdapter" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:Parameter Name="startDate" Type="String" />
                <asp:Parameter Name="endDate" Type="String" />
                <asp:Parameter Name="isAlert" Type="Boolean" />
                <asp:Parameter Name="isValid" Type="Boolean" />
                <asp:Parameter Name="isPaid" Type="Boolean" />
                <asp:Parameter Name="isInvoice" Type="Boolean" />
                <asp:Parameter Name="chargeFrequency" Type="String" />
                <asp:Parameter Name="clientName" Type="String" />
                <asp:Parameter Name="distributorName" Type="String" />
                <asp:Parameter Name="clientCountry" Type="String" />
                <asp:Parameter Name="conversionRate" Type="Single" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ObjectDataSource ID="ChargeReportDataSource" runat="server" OldValuesParameterFormatString="original_{0}" SelectMethod="GetData" TypeName="Toptix.App_Code.ChargeReportTableAdapters.ChargesReportTableAdapter">
            <SelectParameters>
                <asp:Parameter Name="startDate" Type="String" />
                <asp:Parameter Name="endDate" Type="String" />
                <asp:Parameter Name="isAlert" Type="Boolean" />
                <asp:Parameter Name="isValid" Type="Boolean" />
                <asp:Parameter Name="isPaid" Type="Boolean" />
                <asp:Parameter Name="isInvoice" Type="Boolean" />
                <asp:Parameter Name="chargeFrequency" Type="String" />
                <asp:Parameter Name="clientName" Type="String" />
                <asp:Parameter Name="distributorName" Type="String" />
                <asp:Parameter Name="clientCountry" Type="String" />
                <asp:Parameter Name="conversionRate" Type="Single" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </form>
</body>
<script src="../../Scripts/jquery-1.7.2.min.js"></script>
<script src="../../Scripts/jquery-ui-1.9.2.min.js"></script>
<script>
    $("#chargeDateFromDatePicker").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: '-100:+100'
    });
    $("#chargeDateToDatePicker").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: '-100:+100'
    });
    $("#chargeDateFromDatePicker").attr('readOnly', 'true');
    $("#chargeDateToDatePicker").attr('readOnly', 'true');
</script> 
</html>
