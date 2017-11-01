<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="Search.aspx.vb" Inherits="MAIS.Web.About" EnableEventValidation="False"%>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/Search.js" type="text/javascript"></script>
     <script type="text/javascript" src="Scripts/spin.min.js"></script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">

    <asp:ScriptManager id="ScriptManager1" runat="server">
        </asp:ScriptManager>
       <script type="text/javascript" language="javascript">

           Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

           function EndRequestHandler(sender, args) {

               if (args.get_error() != undefined) {

                   args.set_errorHandled(true);

               }

           }

</script>
    <center><table class="CountySelection" style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Search Page"></asp:Label>
                            </td>
                        </tr>
                    </table></center>
    <asp:Panel ID= "pnlapplication" runat="server">
<div style="text-align:center">
<asp:ValidationSummary ID="searchValidationSummary" ValidationGroup="searchpage" runat="server" CssClass="ErrorSummary errMsg" />
<br /><br />
    <%--<script type ="text/javascript" language ="javascript">
        Sys.Application.add_load(radioButtonClick);
            </script>--%>
    <input type="hidden" id ="hdApplicationStatus" runat ="server" class ="hdApplicationStatus" />
            <table width="100%">
                <tr align="center">
                 <td>                     
                 <asp:Panel ID="Panel2"  GroupingText="Please choose one" runat="server" Width="80%" Font-Size="9pt" Font-Italic="false">                 
                    <table width="100%">
                        <tr align="center">
                            <td >
                                <asp:RadioButtonList ID="rblSelect" CssClass="rblSelect" runat="server" 
                                    RepeatDirection="Horizontal" ValidationGroup = "searchpage" CausesValidation="True" 
                                        BorderStyle="Dotted" BorderWidth="2px">
                                    <asp:ListItem Value="0" Text="RN"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="DD Personnel"></asp:ListItem>
                               </asp:RadioButtonList>                              
                            </td>
                        </tr>
                    </table> 
                </asp:Panel>
                </td>
                </tr>
                <tr align="center">               
                <td>
                        <asp:Panel ID="pnlSearch" GroupingText="Search Options" CssClass="pnlSearch"  runat="server" Width="80%" Font-Size="9pt" Font-Italic="false">
                    <table width="100%">
                        <tr>
                            <td align="left"  class ="tdMandatory">                            
                                    <label id="lblRNDDLicenseSSN" class="lblRNDDLicenseSSN" style="font-size:9pt; font-style:normal;"></label>    
                            </td>
                            <td align="left">
                                    <input type = "text" id="txtRNDDLicSSN" class="txtRNDDLicSSN" style="Width:300px;" runat = "server" maxlength="8" /> 
                            </td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                                <td align="left">
                                <asp:RegularExpressionValidator ID="RegRNDDLicSSN" runat="server" ControlToValidate="txtRNDDLicSSN" Display="None" 
                                    ErrorMessage="Please enter a valid Data" ValidationExpression="^[0-9]*$" ValidationGroup="searchpage">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>

                        <tr>
                            <td align="left">
                                    <label id="lblRNDateDDDOB" class="lblRNDateDDDOB" style="font-size:9pt; font-style:normal;"></label>
                            </td>
                            <td align="left">
                                    <input type = "text" id="txtRNDateDDDOB" class="date-pick txtRNDateDDDOB" style="Width:300px;" runat = "server"/>                                    
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                    <label id="lblFirstName" style="font-size:9pt; font-style:normal;">First Name:</label>                                   
                            </td>
                            <td align="left">
                                    <input type = "text" id="txtFirstName" class="txtFirstName" style="Width:300px;" runat = "server"/>                                    
                            </td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                            <td align="left">
                                <asp:RegularExpressionValidator ID="RegFirstName" runat="server" ControlToValidate="txtFirstName" Display="None" 
                                    ErrorMessage="Please enter a only alphabets" ValidationExpression="^[a-zA-Z]+$" ValidationGroup="searchpage">
                                </asp:RegularExpressionValidator>                      
                            </td>
                        </tr>
                            <tr>
                            <td align="left">
                                    <label id="lblLastName" style="font-size:9pt; font-style:normal;">Last Name:</label>                                   
                            </td>
                            <td align="left">
                                    <input type = "text" id="txtLastName" class="txtLastName" style="Width:300px;" runat = "server"/>                                    
                            </td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                            <td align="left">
                                <asp:RegularExpressionValidator ID="RegLastName" runat="server" ControlToValidate="txtLastName" Display="None" 
                                    ErrorMessage="Please enter a only alphabets" ValidationExpression="^[a-zA-Z]+$" ValidationGroup="searchpage">
                                </asp:RegularExpressionValidator>                      
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                    <label id="lblEmployer" style="font-size:9pt; font-style:normal;">Employer Name:</label>                                   
                            </td>
                            <td align="left">
                                    <input type = "text" id="txtEmployer" class="txtEmployer" style="Width:300px;" runat = "server"/>                                    
                            </td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                            <td align="left">
                                <asp:RegularExpressionValidator ID="RegEmployer" runat="server" ControlToValidate="txtEmployer" Display="None" 
                                    ErrorMessage="Please enter a only alphabets" ValidationExpression="^[a-zA-Z]+$"  ValidationGroup="searchpage">
                                </asp:RegularExpressionValidator>                      
                            </td>
                        </tr>                        
                        <tr>
                            <td align="left">
                                <label id="lblAppID" style="font-size:9pt; font-style:normal;">Application ID:</label>                                    
                            </td>
                            <td align="left">
                                    <input type = "text" id="txtAppID" class="txtAppID" style="Width:300px;" runat = "server"/>                                    
                            </td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                            <td align="left">
                                <asp:RegularExpressionValidator ID="revAppID" runat="server" ControlToValidate="txtAppID" Display="None" 
                                    ErrorMessage="Please enter a valid Application number." ValidationExpression="^\d{9}$" ValidationGroup="searchpage">
                                </asp:RegularExpressionValidator>
                            </td>
                        </tr>
                        <tr style="table-layout: inherit; border-spacing: 0px; margin: auto; padding: 0px">
                            <td align="left">
                                    <label id="lblStatus" style="font-size:9pt; font-style:normal;">Application Status:</label>                                   
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="ddlStatus" style="Width:300px;">
                                </asp:DropDownList>                 
                            </td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                            <td align="left">                                                      
                            </td>
                        </tr>
                        <tr runat ="server" id ="trDDCode" class ="trDDCode">
                            <td align="left">
                                    <label id="lblDDCode" style="font-size:9pt; font-style:normal;">DDPersonnel Code:</label>                                   
                            </td>
                            <td align="left">
                                    <input type = "text" id="txtDDPersoonelCode" class="txtDDPersoonelCode" style="Width:300px;" runat = "server"/>                                    
                            </td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                            <td align="left">                                                      
                            </td>
                        </tr>
                    </table>
         </asp:Panel>
                </td></tr>
            </table> 
        
 
            
<br />
    <%--<asp:UpdateProgress ID="updProgress"
        AssociatedUpdatePanelID="UpdatePanel1"
        runat="server">
            <ProgressTemplate>            
            <img alt="progress" src="images/progress.jpg"/>
               Processing...            
            </ProgressTemplate>
        </asp:UpdateProgress>--%>
    <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <ContentTemplate>
             <script type ="text/javascript" language ="javascript">
                 Sys.Application.add_load(buttonClick);
            </script>
<asp:Button ID="btnSearch" runat="server" cssClass="btnSearch" Text="Search" />

 <div id="divSpinner" runat="server" class="divSpinner" style="font-family:Tahoma, Arial, Verdana; font-size:24pt; font-weight:bold; text-align: center;">
          <br /><br />
        </div>
<asp:GridView runat = "server" ID = "grdDDSearch" DataKeyNames = "Last4SSN"  AllowSorting="True"
                AutoGenerateColumns="False" PageSize="100" CssClass ="grdDDSearch"
                CellPadding="4" ForeColor="#333333" BorderStyle="Solid" BorderWidth="1px" BorderColor="#222222" >
                <EditRowStyle BackColor="#999999" />
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle CssClass="gridviewHeader" BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <Columns>        
        <asp:BoundField DataField="Last4SSN" HeaderText="Last 4 SSN" />      
        <asp:BoundField DataField="LastName" HeaderText="Last Name" />
        <asp:BoundField DataField="FirstName" HeaderText="First Name" />
        <asp:BoundField DataField="MiddleName" HeaderText="Middle Name" />
        <asp:BoundField DataField="HomeAddress" HeaderText="Home Address" />
        <asp:BoundField DataField="County" HeaderText="County" Visible ="True" />
        <asp:BoundField DataField="DateOfBirth" HeaderText="Date Of Birth" />
        <asp:BoundField DataField="CAT1" HeaderText="CAT 1" />
        <asp:BoundField DataField="CAT2" HeaderText="CAT 2" />
        <asp:BoundField DataField="CAT3" HeaderText="CAT 3" />
        <asp:BoundField DataField="RoleID" HeaderText="Role ID" Visible ="False" />
        <asp:BoundField DataField="ApplicationID" HeaderText="APPL ID" />
        <asp:BoundField DataField="ApplicationStatus" HeaderText="Application Status" Visible ="True" />
        <asp:BoundField DataField="ApplicationType" HeaderText="Application Type" Visible ="True" />
        <asp:BoundField DataField="DDPersonnelCode" HeaderText="DDPersonnal Code" />
    </Columns>
    <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
</asp:GridView>
<asp:GridView runat = "server" ID = "grdRNSearch" DataKeyNames = "RNLicenseNumber" AllowSorting="True"
                AutoGenerateColumns="False" PageSize="100" CssClass="grdRNSearch"
                CellPadding="4" ForeColor="#333333" BorderStyle="Solid" BorderWidth="1px" BorderColor="#222222" >
                <EditRowStyle BackColor="#999999" />
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <HeaderStyle CssClass="gridviewHeader" BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <Columns>
        <asp:BoundField DataField="RNLicenseNumber" HeaderText="RN License" />
        <asp:BoundField DataField="LastName" HeaderText="Last Name" />
        <asp:BoundField DataField="FirstName" HeaderText="First Name" />
        <asp:BoundField DataField="MiddleName" HeaderText="Middle Name" />
        <asp:BoundField DataField="HomeAddress" HeaderText="Home Address" />
        <asp:BoundField DataField="County" HeaderText="County" Visible ="True" />
        <asp:BoundField DataField="RNTrainer" HeaderText="RN Trainer" />
        <asp:BoundField DataField="RNInstructor" HeaderText="RN Instructor" />
        <asp:BoundField DataField="RNMaster" HeaderText="RN Master" />
        <asp:BoundField DataField="ICFRN" HeaderText="17 + Bed" />
        <asp:BoundField DataField="QARN" HeaderText="QA RN" />
        <asp:BoundField DataField="RoleID" HeaderText="Role ID" Visible ="False" />
        <asp:BoundField DataField="ApplicationID" HeaderText="APPL ID" />
        <asp:BoundField DataField="ApplicationStatus" HeaderText="Application Status" Visible ="True" />
        <asp:BoundField DataField="ApplicationType" HeaderText="Application Type" Visible ="True" />
    </Columns>
    <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
</asp:GridView>
<asp:Panel ID = "pnlLabel" runat = "server" Visible="false" CssClass ="pnlLabel">
<label id = "lblNew"><a href = "StartPage.aspx?Create=New">Create New Application</a></label></asp:Panel> 
<asp:Label ID ="lblPermission"  runat ="server" />
<asp:Label ID="lblText" runat="server" Text=""></asp:Label>
             </ContentTemplate>      
 </asp:UpdatePanel>
</div>
</asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
