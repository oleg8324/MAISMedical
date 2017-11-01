<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="StartPage.aspx.vb" Inherits="MAIS.Web.StartPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="Scripts/StartPage.js"></script>
<style type="text/css">
table.grdStart {
	font-family: verdana,arial,sans-serif;
	font-size:13px;
	color:#333333;
	border-width: 1px;
	border-color: #666666;
	border-collapse: collapse;
}
table.grdStart th {
	border-width: 1px;
	border-style: solid;
	border-color: #666666;
	background-color: #BFE4FF;
}
table.grdStart td {
	border-width: 1px;
	border-style: solid;
	border-color: #666666;
	background-color: #ffffff;
}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <center> <table class="CountySelection" style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Start Page"></asp:Label>
                            </td>
                        </tr>
                    </table></center>
    <center><label id ="lblError" runat ="server"></label></center>
<asp:ValidationSummary ID="ValidationSummary1" runat="server" 
        CssClass="ErrorSummary errMsg" ValidationGroup="landingPage"/><br />
 <asp:Panel ID = "pnlEligiblity" runat = "server" 
        GroupingText = "Certification Eligibility Information" 
        HorizontalAlign="Left" BorderColor="Black" Visible = "true">
     <center>
        <table id="startPage" class="grdStart" runat ="server">   
            <tr>
                <th align = "center">Certification Type</th>
                <th align="center">Application Type</th>
                <th align = "center">Requirements</th></tr>   
        </table></center>
</asp:Panel> <br />
<center>
    <asp:Panel ID ="pnlStart" runat ="server" GroupingText="Choose any one of the Following" BackColor="#EFF3FB">
<asp:Label id = "lblInitial" 
        style="color: #000000; text-decoration: underline; font-weight: bold" 
        class="CountySelection" runat = "server" Visible="False">Initial Certification or Registration</asp:Label>
<asp:Panel ID = "pnlInitial" runat = "server" 
        GroupingText = "Initial Certification or Registration" 
        HorizontalAlign="Left" Width = "450px" BorderColor="Black" BackColor="#EFF3FB"
        CssClass = "pnlInitial" Visible = "false">
<asp:RadioButtonList id="rdbInitial" runat="server" BorderStyle="Solid" BackColor="#EFF3FB"
        RepeatDirection = "Vertical" CssClass = "rdbInitial">
            <asp:ListItem Value="4" Text="RN TRAINER" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="7" Text="QA RN (no existing certification)" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="8" Text="17 + BED" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="15" Text="DD PERSONNEL - CAT1 ONLY" Enabled="False"></asp:ListItem>
</asp:RadioButtonList></asp:Panel><br />
<asp:Label id = "lblAddOn" 
        style="color: #000000; text-decoration: underline; font-weight: bold;" 
        class="CountySelection" runat = "server" Visible="False" >Add-On</asp:Label>
<asp:Panel ID = "pnlAddOn" runat = "server" 
        GroupingText = "Add-On" 
        HorizontalAlign="Left" Width = "450px" BorderColor="Black" BackColor="#EFF3FB"
        CssClass = "pnlAddOn" Visible = "false">
<asp:RadioButtonList id="rdbAddOn" runat="server" BorderStyle="Solid" BackColor="#EFF3FB"
        RepeatDirection = "Vertical" CssClass = "rdbAddOn">
            <asp:ListItem Value="4" Text="RN TRAINER" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="5" Text="RN INSTRUCTOR" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="6" Text="RN MASTER INSTRUCTOR" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="7" Text="QA RN" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="8" Text="17 + BED" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="16" Text="DD PERSONNEL CATEGORY - 2" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="17" Text="DD PERSONNEL CATEGORY - 3" Enabled="False"></asp:ListItem>
</asp:RadioButtonList></asp:Panel><br />
<asp:Label id = "lblRenewal" 
        style="color: #000000; text-decoration: underline; font-weight: bold;" 
        class="CountySelection" runat = "server" Visible="False" >Renewal</asp:Label>
<asp:Panel ID = "pnlRenewal" runat = "server" 
        GroupingText = "Renewal" 
        HorizontalAlign="Left" Width = "450px" BackColor="#EFF3FB"
        BorderColor="Black" CssClass = "pnlRenewal" Visible = "false">
<asp:RadioButtonList id="rdbRenewal" runat="server" BorderStyle="Solid" 
        RepeatDirection = "Vertical" TextAlign="Right" CssClass = "rdbRenewal" BackColor="#EFF3FB">
            <asp:ListItem Value="4" Text="RN TRAINER" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="5" Text="RN INSTRUCTOR" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="7" Text="QA RN" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="8" Text="17 + BED" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="15" Text="DD PERSONNEL CATEGORY - 1" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="16" Text="DD PERSONNEL CATEGORY - 2" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="17" Text="DD PERSONNEL CATEGORY - 3" Enabled="False"></asp:ListItem>
            <asp:ListItem Value="6" Text="RN MASTER INSTRUCTOR" Enabled="False"></asp:ListItem>
</asp:RadioButtonList></asp:Panel>
    <asp:Label id = "lblUpdate" 
        style="color: #000000; text-decoration: underline; font-weight: bold;" 
        class="CountySelection" runat = "server" Visible="False" >Update</asp:Label>
<asp:Panel ID = "pnlUpdate" runat = "server" 
        GroupingText = "Update" BackColor="#EFF3FB"
        HorizontalAlign="Left" Width = "450px" 
        BorderColor="Black" CssClass = "pnlUpdate" Visible = "false">
<asp:RadioButtonList id="rdbUpdate" runat="server" BorderStyle="Solid" BackColor="#EFF3FB"
        RepeatDirection = "Vertical" TextAlign="Right" CssClass = "rdbUpdate">
            <asp:ListItem Value="4" Text="UPDATE PROFILE" Enabled="False"></asp:ListItem>
</asp:RadioButtonList></asp:Panel>
        </asp:Panel>
<br />
</center>
<center>
<input type = "hidden" id = "hdUserRoleinMais" runat = "server" class = "hdUserRoleinMais"/>
<input type = "hidden" id = "hdRNorDDRole" runat = "server" class = "hdRNorDDRole"/>
<input type = "hidden" id = "hdApplicationID" runat = "server" class = "hdApplicationID"/>
<asp:Panel ID="pnlFooterNav" runat="server" HorizontalAlign="Right" CssClass="NavigationMenu">
<input type="button" value="Save and Continue" class = "btnSaveAndContinue" id =  "btnSaveAndContinue" runat ="server"/>
</asp:Panel> 

</center>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
