<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="ViewCertificate.aspx.vb" Inherits="MAIS.Web.ViewCertificate" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <center><table class="CountySelection" style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="View/Print Certificate"></asp:Label>
                            </td>
                        </tr>
                    </table></center>
    <label id ="lblCertificate" runat ="server" visible ="false" style="color: #FF0000">No certificate is generated</label>
    <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="False"
                                                         DataKeyNames="ImageSID"  BorderColor="Black"
                                                        BorderStyle="Solid" BorderWidth="1px" Font-Names="Arial" Font-Size="Small" Width="100%">
                                                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                        <Columns>
                                                            <asp:CommandField SelectText="View" ShowCancelButton="False" ShowSelectButton="True" />
                                                            <asp:BoundField DataField="DocumentName" HeaderText="FileName" />
                                                            <asp:BoundField DataField="DocumentType" HeaderText="Requirement" />
                                                        </Columns>
                                                        <AlternatingRowStyle BackColor="White" />
                                                    </asp:GridView>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
