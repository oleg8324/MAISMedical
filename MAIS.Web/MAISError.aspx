<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="MAISError.aspx.vb" Inherits="MAIS.Web.MAISError" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
    <asp:Panel ID="Panel1" runat="server">
    <br />
    <br />
      
    <asp:Label runat="server"  style="font-family: Verdana, Arial; font-size: x-large">An Error Has Occurred</asp:Label>
    <br />
    <br />
    <%--
                    <asp:Label runat="server" ID="lblSpecificError" />
                    <br /><br />
                    <br />--%><asp:Label runat="server"  style="font-family: Verdana, Arial; font-size: large; font-weight: bold">What should I do?</asp:Label> <%--<a href="javascript:history.go(-1)">Go Back</a>--%>
    <ul>
        <li>If you&#39;ve been away from the site for more than 20 minutes, try logging out, then back in again.</li>
        <li>Click here to <a href="javascript:history.go(-1)">go back</a> and try the operation again.</li>
        <li>Validate the data you entered on previous form.</li>
    </ul>
    <%-- <br /><br /> 
                    <font style="font-family: Verdana, Arial; font-size: large; font-weight: bold">Support Information</font>
                    <br /><br />
                    If the error persists, please contact the Provider Certification Group for more information at 1-800-617-6733.
                    <br /><br />--%>
</asp:Panel>
</asp:Content>
