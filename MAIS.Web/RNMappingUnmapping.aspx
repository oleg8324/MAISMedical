<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="RNMappingUnmapping.aspx.vb" Inherits="MAIS.Web.SecurityMapping" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript"  src="Scripts/RNMapping.js"></script>
     <script type="text/javascript" src="Scripts/spin.min.js"></script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
<center>
     <table class="CountySelection" style="width: 100%;">
                        <tr>
                            <td >                              
                                <asp:Label  ID="Label13" runat="server" Font-Bold="True" Text="RN Unmapped/Mapped"></asp:Label> 
                            </td>                                              
                        </tr>
                    </table><br />
<label style="font-weight: bold" class="CountySelection">Please enter one of the selections below:</label> <br />
<br />
<asp:ValidationSummary runat="server" ID="RNInputValidation" ValidationGroup="rnMap"  CssClass="ErrorSummary errMsg" />
<asp:Panel ID="PPersonal"  GroupingText="RN Search Options" runat="server" Width="80%" Font-Size="9pt" Font-Italic="false">
<table>
<tr><td style="text-align:right">
        <label>RN License No.:</label></td>
    <td style="text-align:left">
        <input type="text" class="txtRNLNO" id="txtRNLNO" runat="server" />        
    </td></tr>
<tr><td style="text-align:right">
        <label>First Name :</label></td>
    <td style="text-align:left">
    <input type="text" class="txtFName" id="txtFName" runat="server" />
</td></tr>
<tr><td style="text-align:right">
        <label>Last Name :</label></td>
    <td style="text-align:left">
        <input type="text" class="txtLName" id="txtLName" runat="server" />        
    </td></tr>
<tr><td style="text-align:right">
        <label>Access Status :</label></td>
    <td style="text-align:left">
        <asp:DropDownList ID="ddlStatus" CssClass="ddlStatus"  runat="server">
                <asp:ListItem Value="0" >Select Status</asp:ListItem>
                <asp:ListItem Value="1">All</asp:ListItem>
                <asp:ListItem Value="2" >Mapped</asp:ListItem>
                <asp:ListItem Value="3">Unmapped</asp:ListItem>
            </asp:DropDownList><br /></td>
</tr><tr><td></td></tr>
<tr>
    <td></td>
    <td style="text-align:left">
        <asp:Button ID="btnSearch" CssClass="btnSearch" Text="Search" runat="server" />
    </td>
</tr>
</table><br />
</asp:Panel>


<asp:Panel ID = "pnlResults" runat = "server">
     <div id="divSpinner" runat="server" class="divSpinner" style="font-family:Tahoma, Arial, Verdana; font-size:24pt; font-weight:bold; text-align: center;">
          <br /><br />
        </div>
   
<label runat="server" id = "lblRNErr" style="color:red;"></label> <br /> 
      <label runat="server" id = "lblCount" style="color:blue;"></label>
     <asp:GridView runat="server" ID="grdResults" BackColor="White" BorderColor="#3366CC" BorderStyle="Inset" BorderWidth="1px" CellPadding="4" 
                        DataKeyNames="RN_Sid" HorizontalAlign="Center" AutoGenerateColumns="false" AutoGenerateEditButton="true" Width="100%">
                        <Columns>                           
                           <asp:TemplateField HeaderText="UnMapped">
                                <ItemTemplate>
                                  <%--  <asp:Label ID="lblMapFlg" runat="server" Text='<%#IIf(Boolean.Parse(Eval("Un_Map_Flg").ToString()), "UnMapped", "Mapped")%>'></asp:Label>--%>
                                    <asp:CheckBox ID="chMap" runat="server" Checked='<%#Boolean.Parse(Eval("Un_Map_Flg"))%>' Enabled="false" />
                                   <%-- <asp:LinkButton ID="lnkBtnUnmap" runat="server" CommandName='<%#IIf(Boolean.Parse(Eval("Un_Map_Flg").ToString()), "UnMapped", "Mapped")%>'>
                                                  <%#IIf(Boolean.Parse(Eval("Un_Map_Flg").ToString()), "UnMapped", "Mapped")%>
                                    </asp:LinkButton>--%>
                                </ItemTemplate>
                                <EditItemTemplate>
                                     <asp:CheckBox ID="chMap" runat="server" Checked='<%#Boolean.Parse(Eval("Un_Map_Flg"))%>' Enabled="true" />
                                 </EditItemTemplate>
                            </asp:TemplateField>                                
                            <asp:BoundField DataField="RNLicenseNumber" HeaderText="RN License No." ReadOnly="true" />                          
                            <asp:BoundField DataField="First_Name" HeaderText="FirstName" ReadOnly="true" />
                            <asp:BoundField DataField="Last_Name" HeaderText="LastName" ReadOnly="true" />                                                                              
                             <asp:TemplateField HeaderText="Comments" SortExpression="Active">
                                <ItemTemplate>
                                    <asp:Label ID="lblCommentsReadOnly" runat="server" ReadOnly="true" Text='<%#Eval("Comments")%>' ></asp:Label>
                                    <%--<asp:TextBox ID="txtCommentsReadOnly" runat="server" ReadOnly="true" Text='<%#Eval("Comments")%>' TextMode="MultiLine" ></asp:TextBox>--%>
                                </ItemTemplate>
                                <EditItemTemplate >
                                      <asp:TextBox ID="txtComments" runat="server" Text='<%#Eval("Comments")%>' TextMode="MultiLine" ></asp:TextBox>
                                </EditItemTemplate>
                            </asp:TemplateField>                                       
                            <asp:BoundField DataField="EditedBy" HeaderText="Updated By" ReadOnly="true" />                                                                                                                                   
                        </Columns>
                        <RowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" VerticalAlign="Top" />
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                        <HeaderStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" BorderColor="Black" />
                    </asp:GridView>
</asp:Panel>
</center>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
