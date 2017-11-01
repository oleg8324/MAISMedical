<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="EmployerInformation.aspx.vb" Inherits="MAIS.Web.EmployerInformation" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="UserControls/Address.ascx" TagName="Address" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="Scripts/jquery/jquery.datepick.min.js"></script>
    <link href="Scripts/jquery/redmond.datepick.css" rel="stylesheet" />
    <script type="text/javascript" src="Scripts/EmployerInformation.js"></script>
<script type="text/javascript" src="Scripts/Address.js"></script>
    <style type="text/css">
table.grdEmp {
	font-family: verdana,arial,sans-serif;
	font-size:11px;
	color:#333333;
	border-width: 1px;
	border-color: #666666;
	border-collapse: collapse;
}
table.grdEmp th {
	border-width: 1px;
	border-style: solid;
	border-color: #666666;
	background-color: #BFE4FF;
}
table.grdEmp td {
	border-width: 1px;
	border-style: solid;
	border-color: #666666;
	background-color: #ffffff;
}
        #txtEmployerName
        {
            width: 334px;
        }
    </style>
    <script language="javascript" type="text/jscript">
        function  window.confirm(str) {
            execScript('n = msgbox("'+str+'","4132")', "vbscript");
            return(n == 6);
        }
    </script>
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
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Current Employer Information"></asp:Label>
                            </td>
                        </tr>
                    </table></center>
     <div style="border: medium solid #FF0000;text-align:center" runat ="server" id ="divError">
        <label id ="lblAppError" runat ="server"/>
    </div>      
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
        CssClass="ErrorSummary errMsg" ValidationGroup="landingPage"/><br /> 
     <asp:UpdatePanel id="UpdatePanel2" runat="server"> 
            <ContentTemplate>  
                                                          
<asp:Panel ID="pnlDD" runat="server" GroupingText = "Employer Selection"
        BorderColor="Black"
        HorizontalAlign="Left" BackColor="#EFF3FB" CssClass ="pnlDD">
        <table><tr><td> 
           <script type ="text/javascript" language ="javascript">
               Sys.Application.add_load(radiobuttonClick);
            </script>
            
<asp:RadioButtonList ID="rblSelect" CssClass="rblSelect" runat="server" RepeatColumns="2"
        RepeatDirection="Horizontal" ValidationGroup = "1" CausesValidation="True" BackColor="#EFF3FB"
            BorderStyle="Solid">
   <asp:ListItem Value="3" Text="DD PERSONNEL IS A DODD INDEPENDENT PROVIDER"></asp:ListItem>
   <asp:ListItem Value="4" Text="EMPLOYEE OF DODD AGENCY PROVIDER"></asp:ListItem>
    <asp:ListItem Value="5" Text="EMPLOYEE OF ICF PROVIDER"></asp:ListItem>
   <asp:ListItem Value="1" Text="RN IS SELF EMPLOYED"></asp:ListItem>
   <asp:ListItem Value="2" Text="OTHER EMPLOYER"></asp:ListItem></asp:RadioButtonList> 
</td></tr></table></asp:Panel>
<br /><br /> 
                <script type ="text/javascript" language ="javascript">
                    Sys.Application.add_load(searchClick);
            </script>
    <asp:Panel ID = "pnlEmployer" runat = "server" CssClass = "pnlEmployer" GroupingText = "DODD Provider Information">
        <table>
            <tr>
                <%--<td colspan="1">Employer/Agency Information:</td>
                    <td>
                        <input type = "text" id = "txtEmployerAgencyInformation" runat = "server" size = "15" maxlength = "15" class = "txtEmployerAgencyInformation" tabindex="1"/>
                    </td>--%>
                    <td colspan="1">DODD Provider#:</td>
                    <td>
                        <input type = "text" id = "txtDODDProvider" runat = "server" size = "15" maxlength = "15" class = "txtDODDProvider" tabindex="2"/>
                 </td>
                 <td><input type="button" value="Search" style="width: 87px" id = "btnSearch" class = "btnSearch" tabindex="3" runat ="server"/></td>
        </tr>            
        </table>
        <table id="grdEmp" class="grdEmp" runat ="server" visible="True">
        </table>
        <label id ="lblFound" runat ="server" class ="lblFound" style="color: #FF0000; float: left;">No records are found</label>
    </asp:Panel> <br />
    <asp:Panel ID  ="pnlEmp" runat = "server" GroupingText = "Agency Information">
    <table>
    
    <tr>
         <td colspan="1" align="right">Name:<span style="color: Red;">*</span></td>                        
            <td colspan="3" align="left"> 
                <input type = "text" id = "txtEmployerName" runat = "server"  maxlength = "99" class="txtEmployerName isTextChange" align="left" style="width: 99%"/>
            </td>
        <td class = "tdTaxID" colspan="1" id ="tdTaxID" runat ="server" align="right">Provider#:
                <font style="color: Red;">*</font></td>
            <td>
                <input type = "text" id = "txtEmployerTaxID" runat = "server" size = "15" maxlength = "12" class="txtEmployerTaxID isTextChange" tabindex="5" align="left" />
         </td>
    </tr>
    <tr>
         <td colspan="1" align="right">Certification StartDate:</td>
            <td>
                <input type = "text" id = "txtCertStartDate" runat = "server" size = "15" maxlength = "15" class ="txtCertStartDate" tabindex="6" align="left"/>
            </td>
            <td colspan="1" align="right">Certification EndDate:</td>
            <td>
                <input type = "text" id = "txtCertEndDate" runat = "server" size = "15" maxlength = "15" class="txtCertEndDate" tabindex="7" align="left"/>
         </td>
         <td align="right">Provider Status:</td>
            <td align="left">
                <input type = "text" id = "txtCertStatus" runat = "server" size = "10" maxlength = "15" class ="txtCertStatus" tabindex="8" />
         </td>
    </tr>
    <tr>
         <td colspan="1" align="right">Employment StartDate:
                <font style="color: Red;">*</font></td>
            <td>
                <input type = "text" id = "txtEmploymentStartDate" runat = "server" size = "15" maxlength = "15" class="txtEmploymentStartDate isDateChange" tabindex="9" align="left"/>
            </td>
            <td colspan="1" align="right">Employment EndDate:</td>
            <td >
                <input type = "text" id = "txtEmploymentEndDate" runat = "server" size = "15" maxlength = "15" class="txtEmploymentEndDate isDateChange" tabindex="11" align="left"/>
         </td>
    </tr>
    <tr>
         <td colspan="1" align="right">CEO LastName:
                <font style="color: Red;">*</font></td>
            <td>
                <input type = "text" id = "txtCEOLastName" runat = "server" size = "15" maxlength = "15" class = "txtCEOLastName isTextChange" align="left"/>
            </td>
            <td colspan="1" align="right">CEO FirstName:<font style="color: Red;">*</font></td>
            <td>
                <input type = "text" id = "txtCEOFristName" runat = "server" size = "15" maxlength = "15" class = "txtCEOFristName isTextChange" tabindex="2"  align="left"/>
            </td>
            <td class="style1" colspan="1" align="right">CEO MI:</td>
            <td>
                <input type = "text" id = "txtCEOMiddleName" runat = "server" size = "5" maxlength = "2" class = "txtCEOMiddleName isTextChange" tabindex="13" align="left"/>
            </td>
    </tr>
    <tr>
        <td><label style="color: #800000; text-align: center" class = "CountySelection">(Not Delegating Nurse)</label></td>
    </tr>    
    </table>
    </asp:Panel>
                    <asp:Panel ID="pnlProgressBar" runat="server" 
         GroupingText="Agency Address" BorderColor="Black" TabIndex="18" CssClass ="pnlProgressBar">
                        <script type ="text/javascript" language ="javascript">
                            Sys.Application.add_load(checkboxClick1);
                        </script>
                    <input type = "checkbox" id = "chkAgencyAddress" runat = "server" class = "chkAgencyAddress"/>&nbsp;Check if agency address is different from personal mailing address
                        <uc2:Address ID="addrBar" runat="server" />
                    </asp:Panel>
                <asp:Panel ID ="pnlSupervisor" runat ="server" GroupingText ="Supervisor" BorderColor="Black" TabIndex="19" CssClass ="pnlSupervisor">
                    <table>
                        <tr>
        <td colspan="1" align="right">Supervisor LastName:
                <font style="color: Red;">*</font></td>
            <td>
                <input type = "text" id = "txtSupervisorLastName" runat = "server" size = "15" maxlength = "15" class = "txtSupervisorLastName isTextChange" align="left"/>
            </td>
            <td colspan="1" align="right">Supervisor FirstName:
                <font style="color: Red;">*</font></td>
            <td>
                <input type = "text" id = "txtSupervisorFirstName" runat = "server" size = "15" maxlength = "15" class = "txtSupervisorFirstName isTextChange" align="left"/>
            </td>
    </tr>
    <tr>
        <td colspan="1" align="right">Supervisor Phone Number:
        <font style="color: Red;">*</font></td>
            <td>
                <input type = "text" id = "txtSuperVisorPhoneNumber" runat = "server" size = "15" maxlength = "15" class = "txtSuperVisorPhoneNumber isTextChange" align="left"/>
            </td>
            <td colspan="1" align="right">Supervisor Email Address:
        <font style="color: Red;">*</font></td>
            <td>
                <input type = "text" id = "txtSuperVisorEmail" runat = "server" size = "15" class ="txtSuperVisorEmail isTextChange"  align="left"/>
            </td>
    </tr>
    <tr>
        <td colspan="1" align="right">Supervisor Start Date:
        <font style="color: Red;">*</font></td>
            <td>
                <input type = "text" id = "txtSuperVisorStartDate" runat = "server" size = "15" maxlength = "15" class = "txtSuperVisorStartDate isDateChange" align="left"/>
            </td>
            <td colspan="1" align="right">Supervisor End Date:</td>
            <td>
                <input type = "text" id = "txtSuperVisorEndDate" runat = "server" size = "15" class ="txtSuperVisorEndDate isDateChange" align="left"/>
            </td>
    </tr>
                    </table>
                </asp:Panel>
                    <asp:Panel ID="Panel1" runat="server" GroupingText="Work Location Address" BorderColor="Black" CssClass ="pnlWork" TabIndex="20">  
                        <script type ="text/javascript" language ="javascript" >
                            Sys.Application.add_load(checkboxClick);
                        </script>                  
                        <input type ="checkbox" id ="chkAgency" runat ="server" class ="chkAgency" />&nbsp;Check if same as agency address
                      <div id ="dv" class="dv">    <uc2:Address ID="addrBar1" runat="server" AddressLine1 = "" AddressLine2 = "" City = "" State = "" ClientIDMode ="Static" /></div> 
                        <table>
                             <tr valign="bottom">
                                <td colspan="1" align="right">Work Location Start Date:
                                <font style="color: Red;">*</font></td>
                                    <td>
                                        <input type = "text" id = "txtWorkLocationStartDate" runat = "server" size = "15" maxlength = "15" class = "txtWorkLocationStartDate isDateChange" align="left"/>
                                    </td><td>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                                     <td align="right" colspan="1">Work Location End Date:</td>
                                     <td>
                                         <input type = "text" id = "txtWorkLocationEndDate" runat = "server" size = "15" class ="txtWorkLocationEndDate isDateChange" align="left"/>
                                     </td>
                            </tr>
                        </table>
                    </asp:Panel> 
                <script type ="text/javascript" language ="javascript">
                    Sys.Application.add_load(gridClick);
                </script>
            <asp:Panel ID ="pnlRecent" runat ="server" GroupingText ="Recently Added Employer Information" TabIndex ="21">
                    <asp:GridView ID = "grdRecent" runat = "server" AllowSorting="True"
                AutoGenerateColumns="False" DataKeyNames="EmployerID" PageSize="100" CssClass="grdRecent"
                CellPadding="4" ForeColor="#333333" BorderStyle="Solid" BorderWidth="1px" BorderColor="#222222">
                <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                         <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <HeaderStyle CssClass="gridviewHeader" BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <Columns>
                    <asp:TemplateField HeaderText="Employer Name">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnSelect" runat="server" CommandName="Select">
                                            <%# Eval("EmployerName")%>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="CEOFirstName" HeaderText="CEO First Name" />
                    <asp:BoundField DataField="CEOLastName" HeaderText="CEO Last Name" />
                    <asp:BoundField DataField="SupervisorFirstName" HeaderText="Supervisor First Name" />
                    <asp:BoundField DataField="SupervisorLastName" HeaderText="Supervisor Last Name" />
                    <asp:BoundField DataField="DODDProviderContractNumber" HeaderText="Provider Contract Number" />                                       
                    <asp:BoundField DataField="SuperVisorEndDate" HeaderText="Supervisor End Date"/>
                    <asp:BoundField DataField="WorkLocationEndDate" HeaderText="Work Location End Date"/>                                    
                    <asp:TemplateField HeaderText="" InsertVisible="False" 
                                    ShowHeader="False" FooterStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                       <asp:LinkButton ID="lnkAction" runat="server" Text="Add Supervisor"   
                                        CommandName="DesiredSuperVisorOptions" CommandArgument='<%# Eval("SuperVisorEndDate")%>'> 
                                       </asp:LinkButton>
                                    </ItemTemplate>
                    </asp:TemplateField>
                             <asp:TemplateField HeaderText="" InsertVisible="False" 
                                    ShowHeader="False" FooterStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                       <asp:LinkButton ID="lnkWorkAction" runat="server" Text="Add WorkLocation"   
                                        CommandName="DesiredWorkLocationOptions" CommandArgument='<%# Eval("WorkLocationEndDate")%>'> 
                                       </asp:LinkButton>
                                    </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="IdentitficationValue" HeaderText="Identity Of Employer"/>   
                   
                   <%--<asp:TemplateField ItemStyle-VerticalAlign="Middle" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="10%">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnRemove" runat="server" CommandName="Delete" Font-Size="10pt" Text="Delete" CssClass="lnkBtnRemove" OnClick ="lnkBtnRemove_Click" />
                        </ItemTemplate>
                   </asp:TemplateField>--%>
                            <asp:CommandField ShowDeleteButton="true" /> 
                             <asp:TemplateField HeaderText="" InsertVisible="False" 
                                    ShowHeader="False" FooterStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                       <asp:LinkButton ID="lnkUpdateInfo" runat="server" Text="Complete Employer Information"   
                                        CommandName="PendingEmployer" CommandArgument='<%# Eval("Pending_Information_Flg")%>'> 
                                       </asp:LinkButton>
                                    </ItemTemplate>
                    </asp:TemplateField>
                     </Columns> 
                 <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                 <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
                <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Center" />               
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
        </asp:GridView>
     </asp:Panel>
        <asp:Panel ID ="pnlHistroy" runat ="server" GroupingText ="View Employer Information" TabIndex ="22">
                    <asp:GridView ID = "grdViewHistory" runat = "server" AllowSorting="True"
                AutoGenerateColumns="False" DataKeyNames="EmployerID" PageSize="100" CssClass="grdViewHistory"
                CellPadding="4" ForeColor="#333333" BorderStyle="Solid" BorderWidth="1px" BorderColor="#222222">
                <EditRowStyle BackColor="#999999" />
                        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                         <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <HeaderStyle CssClass="gridviewHeader" BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                        <Columns>
                    <asp:TemplateField HeaderText="Employer Name">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnSelect" runat="server" CommandName="Select">
                                            <%# Eval("EmployerName")%>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                     <asp:BoundField DataField="CEOFirstName" HeaderText="CEO First Name" />
                    <asp:BoundField DataField="CEOLastName" HeaderText="CEO Last Name" />
                    <asp:BoundField DataField="SupervisorFirstName" HeaderText="Supervisor First Name" />
                    <asp:BoundField DataField="SupervisorLastName" HeaderText="Supervisor Last Name" />
                    <asp:BoundField DataField="DODDProviderContractNumber" HeaderText="Provider Contract Number" />                            
                    <asp:BoundField DataField="IdentitficationValue" HeaderText="Identity Of Employer"/> 
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
            </asp:Panel>
        <input type ="hidden" id ="hdEmployerID" class ="hdEmployerID" runat ="server" />        
<br /> 
<asp:Panel ID="pnlFooterNav" runat="server" HorizontalAlign="Right">
    <script type ="text/javascript" language ="javascript">
        Sys.Application.add_load(buttonClick);
    </script>
    <asp:Button Text="Add Additional Employers" style="width: 250px; float: right;" ID = "btnSaveAdditional" class = "btnSaveAdditional" tabindex="23" runat ="server" OnClientClick="return TestIfDataSaved()"   />
    <asp:Button style="width: 87px" CssClass = "btnSave" ID = "btnSave" 
        runat = "server" Text="Save"/>&nbsp;&nbsp;&nbsp;
</asp:Panel><br />
<asp:Panel ID="Panel2" runat="server" HorizontalAlign="Right" CssClass="NavigationMenu" TabIndex ="24">
    <input id="Button1" type="button" value="Previous" onclick ="location.href = 'PersonalInformation.aspx'" class ="btnPrevious" runat ="server"/>&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnSaveAndContinue" Text="Continue" runat = "server" cssClass ="btnSaveAndContinue" OnClientClick="return TestIfDataSaved()" />
</asp:Panel>   
     </ContentTemplate>
         <asp:Triggers>
             <asp:AsyncPostBackTrigger ControlID="grdRecent" EventName="RowDataBound" />
             <asp:AsyncPostBackTrigger ControlID="grdRecent" EventName="RowCommand" />
             <asp:AsyncPostBackTrigger ControlID="grdRecent" EventName="SelectedIndexChanged" />
             <asp:AsyncPostBackTrigger ControlID="grdRecent" EventName="RowDeleting" />
             <asp:AsyncPostBackTrigger ControlID="grdViewHistory" EventName="SelectedIndexChanged" />
         </asp:Triggers>
    </asp:UpdatePanel>
    <%--<div><p style="color: #FF0000">Certificates will be mailed in your work location email address. So please enter your work location email address</p></div>--%>
     
    
     
     </label>
     
    
     
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
