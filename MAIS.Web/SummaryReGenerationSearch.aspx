<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="SummaryReGenerationSearch.aspx.vb" Inherits="MAIS.Web.SummaryReGenerationSearch" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
     <center>
        <table class="CountySelection" style="width: 100%;">
                        <tr>
                            <td >                              
                                <asp:Label  ID="Label23" runat="server" Font-Bold="True" Text="Summary Regeneration Search Page"></asp:Label> 
                            </td>                                              
                        </tr>
                    </table></center> 
    <br />
     <label style="color: #000000; font-weight: bold" class="CountySelection">Personal Information</label>
    <asp:Panel ID="pPI" runat="server" GroupingText="Personal Information" Width="100%" HorizontalAlign="Left">
        <br />
        <table cellpadding="2" cellspacing="0" width="100%">
            <tr>
                <td align="left">
                    <asp:Label runat="server" ID="Label6" Font-Bold="true">First Name:</asp:Label>&nbsp;</td>
                <td>
                    <asp:Label runat="server" ID="lblFirstName" /></td>
                <td align="left">
                    <asp:Label runat="server" ID="Label1" Font-Bold="true">Last Name:</asp:Label>&nbsp;</td>
                <td>
                    <asp:Label runat="server" ID="lblLastName" /></td>
                <td colspan="1">
                    <asp:Label runat="server" ID="Label12" Font-Bold="true">Middle Name:</asp:Label></td>
                <td>
                    <asp:Label runat="server" ID="lblMiddleName" /></td>
                <td>
                    <asp:Label runat="server" ID="Label13" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:Label runat="server" ID="lblRNLNoOrSSN" Font-Bold="true"></asp:Label>&nbsp;</td>
                <td>
                    <asp:Label runat="server" ID="lblRNLNoOrSSNtxt" /></td>
                <td align="left">
                    <asp:Label runat="server" ID="lblDtIssuedOrDOB" Font-Bold="true"></asp:Label>&nbsp;</td>
                <td>
                    <asp:Label runat="server" ID="lblDtIssuedOrDOBtxt" /></td>
                <td colspan="1">
                    <asp:Label runat="server" ID="Label14" Font-Bold="true">Gender:</asp:Label></td>
                <td align="left">
                    <asp:RadioButtonList runat="server" ID="rdbGender" Enabled="false" CssClass="rdbGender"
                        RepeatDirection="Horizontal">
                        <asp:ListItem Value="0" Text="M"></asp:ListItem>
                        <asp:ListItem Value="1" Text="F"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <asp:Label runat="server" ID="Label2" Font-Bold="true">Address Line 1:</asp:Label></td>
                <td>
                    <asp:Label runat="server" ID="lblAddr1" />
                </td>
                <td colspan="1">
                    <asp:Label runat="server" ID="Label3" Font-Bold="true">Address Line 2:</asp:Label></td>
                <td>
                    <asp:Label runat="server" ID="lblAddr2" />
                </td>
                <td colspan="1">
                    <asp:Label runat="server" ID="Label4" Font-Bold="true">City:</asp:Label></td>
                <td>
                    <asp:Label runat="server" ID="lblCity" />
                </td>
            </tr>
            <tr>
                <td colspan="1">
                    <asp:Label runat="server" ID="Label5" Font-Bold="true">State:</asp:Label></td>
                <td>
                    <asp:Label runat="server" ID="lblState" />
                </td>
                <td colspan="1">
                    <asp:Label runat="server" ID="Label7" Font-Bold="true">Zip:</asp:Label></td>
                <td>
                    <asp:Label runat="server" ID="lblZip" />
                </td>
                <td colspan="1">
                    <asp:Label runat="server" ID="Label8" Font-Bold="true">County:</asp:Label></td>
                <td>
                    <asp:Label runat="server" ID="lblCounty" />
                </td>
            </tr>
        </table>
    </asp:Panel>
    <br /><br />
    <div id="divCertificationMessage" runat="server">
        <asp:Panel id="pnlCertMessage" runat="server">
            <asp:Label ID="lblPanelMessage" runat="server" Text="Label" ForeColor="Red"></asp:Label>
        </asp:Panel> 
    </div>

    <div id="divAppData" runat="server">
         <asp:Panel ID="pnlCertHistory" runat="server" GroupingText="Certification History" CssClass="pCH"  Width="100%" HorizontalAlign="Left">
                   <asp:Label runat="server" style="color:red; font-weight: bold"  ID="lblCerterr"/>  
                <asp:GridView ID="gvCertHistory" runat="server" DataKeyNames="Role_RN_DD_Personnel_Xref_Sid" Width="100%" AutoGenerateColumns="False" CssClass="Grid">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <HeaderStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" BorderColor="Black" />
                            <Columns>                               
                                <asp:BoundField DataField="Role" HeaderText="Role" HeaderStyle-BackColor="#BFE4FF">
                                <HeaderStyle BackColor="#BFE4FF" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Application_Sid" HeaderText="Appliction ID"/>
                                <asp:BoundField DataField="Category" HeaderText="Category" HeaderStyle-BackColor="#BFE4FF">
                                <HeaderStyle BackColor="#BFE4FF" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Level" HeaderText="Level" HeaderStyle-BackColor="#BFE4FF">
                                <HeaderStyle BackColor="#BFE4FF" />
                                </asp:BoundField>
                                <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-BackColor="#BFE4FF">
                                <HeaderStyle BackColor="#BFE4FF" />
                                </asp:BoundField>
                                <asp:BoundField DataField="StartDate" HeaderText="Start Date" HeaderStyle-BackColor="#BFE4FF">     
                                  <HeaderStyle BackColor="#BFE4FF" />
                                </asp:BoundField>
                                <asp:BoundField DataField="EndDate" HeaderStyle-BackColor="#BFE4FF" HeaderText="End Date">
                                <HeaderStyle BackColor="#BFE4FF" />
                                </asp:BoundField>
                                <asp:TemplateField FooterStyle-HorizontalAlign="Left" HeaderText="" InsertVisible="False" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdRoleLevelCategory" runat="server" Value='<%# Eval("Role_Category_Level_Sid")%>' />
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField FooterStyle-HorizontalAlign="Left" HeaderText="" InsertVisible="False" ShowHeader="False">
                                    <ItemTemplate>
                                        <asp:Button ID="BntRecSummary" runat="server" CommandArgument='<%# Eval("Application_Sid")%>' CommandName="RecreateSummary" Text="Recreate Summary" OnClientClick='if (!confirm("Are you sure you want to recreate summary?")) return false;'> 
                                       </asp:Button>
                                    </ItemTemplate>
                                    <FooterStyle HorizontalAlign="Left" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:Button ID="bntRecCertificate" runat="server" Text="Recreate Certificate"   
                                        CommandName="RecreateCertificate" CommandArgument='<%# Eval("Certification_Sid")%>' Enabled='<%# Eval("CertAllowed")%>' OnClientClick='if (!confirm("Are you sure you want to recreate certificate?")) return false;'> 
                                       </asp:Button>
                                    </ItemTemplate>
                                </asp:TemplateField>                                                               
                            </Columns>
                        <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />                
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
               </asp:GridView>
    </asp:Panel>
    </div>

     <rsweb:ReportViewer ID="rvCertificate" runat="server" Width="100%" Font-Names="Verdana"
        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
        WaitMessageFont-Size="14pt" ShowBackButton="False" ShowExportControls="False"
        ShowFindControls="False" ShowPageNavigationControls="False" ShowPrintButton="False"
        ShowRefreshButton="False" ShowZoomControl="False" Visible="False">
    </rsweb:ReportViewer>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
