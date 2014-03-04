<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ClientDataForm.aspx.cs" Inherits="Toptix.WebForms.Reports.ClientDataForm" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Client Date Report</title>
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

    <h2>Orders Report</h2>
    <form id="form1" runat="server" aria-orientation="horizontal">
    <div>
    
        <asp:SqlDataSource ID="Client" runat="server" ConnectionString="<%$ ConnectionStrings:TCRMDBContext %>" SelectCommand="SELECT * FROM [Client]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="Distributor" runat="server" ConnectionString="<%$ ConnectionStrings:TCRMDBContext %>" SelectCommand="SELECT * FROM [Distributor]"></asp:SqlDataSource>
        <asp:SqlDataSource ID="Country" runat="server" ConnectionString="<%$ ConnectionStrings:TCRMDBContext %>" SelectCommand="SELECT * FROM [Country]"></asp:SqlDataSource>
    
    </div>
        <asp:SqlDataSource ID="Product" runat="server" ConnectionString="<%$ ConnectionStrings:TCRMDBContext %>" SelectCommand="SELECT * FROM [Product]"></asp:SqlDataSource>
    <div class="form-options">
        <asp:Panel ID="Panel1" runat="server">
            <div class="section1">
                <div class="row">
                    <div class="item" style="margin-right: 80px;">
                        <asp:Label ID="Label3" runat="server" Text="Client:"></asp:Label>
                        <asp:DropDownList ID="ClientDDL" runat="server" AppendDataBoundItems="true" DataSourceID="Client" DataTextField="Name" DataValueField="Name">
                            <asp:ListItem Text="ALL" />
                        </asp:DropDownList>
                    </div>
                    <div class="item">
                        <asp:Label ID="Label4" runat="server" Text="Distributor:"></asp:Label>
                        <asp:DropDownList ID="DistributorDDL" runat="server" AppendDataBoundItems="true" DataSourceID="Distributor" DataTextField="Name" DataValueField="Name">
                            <asp:ListItem Text="ALL" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row">
                    <div class="item" style="margin-right: 80px;">
                        <asp:Label ID="Label6" runat="server" Text="Client Country:"></asp:Label>
                        <asp:DropDownList ID="ClientCountryDDL" runat="server" AppendDataBoundItems="true" DataSourceID="Country" DataTextField="Description" DataValueField="Description">
                            <asp:ListItem Text="ALL" />
                        </asp:DropDownList>
                    </div>
                    <div class="item">
                        <asp:Label ID="Label5" runat="server" Text="Product:"></asp:Label>
                        <asp:DropDownList ID="ProductDDL" runat="server" AppendDataBoundItems="true" DataSourceID="Product" DataTextField="Name" DataValueField="Name">
                            <asp:ListItem Text="ALL" />
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="row" style="padding-top:10px;border-top:1px solid #ccc">
                    <asp:Label ID="Label7" runat="server" Text="Orders From:"></asp:Label>
                    <asp:TextBox ID="orderDateFromTB" runat="server" Height="17px" style="margin-bottom: 7px"></asp:TextBox>
                    <asp:Label ID="Label2" runat="server" Text="To:"></asp:Label>
                    <asp:TextBox ID="orderDateToTB" runat="server"></asp:TextBox>
                    <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="Run Report" />
                </div>
            </div>
            
        </asp:Panel>
    </div>
        <asp:SqlDataSource ID="Order" runat="server" ConnectionString="<%$ ConnectionStrings:TCRMDBContext %>" SelectCommand="SELECT * FROM [Order]"></asp:SqlDataSource>
        <rsweb:ReportViewer ID="ClientDataReportViewer" runat="server" Font-Names="Verdana" Font-Size="8pt" Visible="False" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Width="8.5in" BorderStyle="Double" Height="11.6in">
            <LocalReport ReportEmbeddedResource="Toptix.Reports.ClientDataReport.rdlc">
                <DataSources>
                    <rsweb:ReportDataSource DataSourceId="ClientDataReportDataSource" Name="ClientDataReportDataSet" />
                </DataSources>
            </LocalReport>
        </rsweb:ReportViewer>
        <asp:ObjectDataSource ID="ClientDataReportDataSource" runat="server" SelectMethod="GetData" TypeName="ClientDataTableAdapters.ClientDataReportTableAdapter"></asp:ObjectDataSource>
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
    </form>
</body>
<script src="../../Scripts/jquery-1.7.2.min.js"></script>
<script src="../../Scripts/jquery-ui-1.9.2.min.js"></script>
<script>
    $("#orderDateToTB").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: '-100:+100'
    });
    $("#orderDateFromTB").datepicker({
        changeMonth: true,
        changeYear: true,
        yearRange: '-100:+100'
    });
    $("#orderDateToTB").attr('readOnly', 'true');
    $("#orderDateFromTB").attr('readOnly', 'true');
</script> 
</html>
