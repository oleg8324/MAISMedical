<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="MAISLogin.aspx.vb" Inherits="MAIS.Web.MAISLogin"%>
<%@ OutputCache location="none" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('.txtRNLicense').mask("RN999999");
        });
    </script>
    <style type="text/css">
table.tbllogin{
	 margin: auto;
}
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <div><asp:Label ID ="lblError" runat ="server" Font-Bold="True" ForeColor="Red"></asp:Label></div> 
<asp:Panel ID ="pnLogin" runat ="server" GroupingText="Login">
            <br />        
        <span style="font-weight:600" id="lblWelcome">Login for Registered Nurses</span>   <br /> <br />
    <table id ="tbllogin" class ="tbllogin" runat ="server">
        <tr align ="center">
        <td colspan="1" align ="right">RN License Number:</td>
        <td>
            <input  type = "text" id = "txtRNLicense" runat = "server"  maxlength = "8" class = "txtRNLicense" size ="25"/>
            <font style="color: Red;">*</font>
        </td>            
    </tr>
    <tr align ="center">
        <td colspan="1" align ="right">First Name:</td>
        <td>
            <input  type = "text" id = "txtFirstname" runat = "server"  maxlength = "100" class = "txtFirstname" size ="27"/>
        </td>
    </tr>      
    <tr align ="center">
        <td colspan="1" align ="right">Last Name:</td>
        <td>
            <input type = "text" id = "txtLastname" runat = "server" maxlength = "100" class = "txtLastname" size ="27"/>
        </td>
    </tr>
    </table> 
            <br />
            <asp:Button style="width: 87px" CssClass = "btnSignIn" ID = "btnSignIn" runat = "server" Text="Sign In"/>
    </asp:Panel>   
    
        <br />
    <div style="float:left;">
        <a href="<%=ConfigurationManager.AppSettings("PORTALURL") %>" id="returnLink" target="_parent">Back to Portal</a>
    </div>
       
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
