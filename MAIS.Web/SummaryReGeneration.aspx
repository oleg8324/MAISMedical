<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="SummaryReGeneration.aspx.vb" Inherits="MAIS.Web.SummaryReGeneration" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/jscript" src="Scripts/SummaryReGeneration.js"></script>
     <script type="text/javascript" src="Scripts/spin.min.js"></script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
        <table>
             <tr id="MiddleRow">
                <td>
                    <center style="height: 50px">
                        <asp:Panel ID="pnlNew" runat="server" Height="20px">
                            <asp:Label ID="lblLoginLabel2" runat="server" Visible="True">LoginUser: </asp:Label>
                             <asp:Label ID="lblLoginUser2" runat="server" ForeColor="#006600" Visible="True"></asp:Label>&nbsp;
                            <asp:Label ID="lblNameLabel2" runat="server" Visible="True">Name: </asp:Label>
                            <asp:Label ID="lblName2" runat="server" ForeColor="#006600" Visible="True">Thomas,Greg</asp:Label>&nbsp;
                            <asp:Label ID="lblRNLicenseLabel2" runat="server" Visible="True">RNLicense# or DDPersonnelCode: </asp:Label>
                            <asp:Label ID="lblRNDDCode2" runat="server" ForeColor="#006600" Visible="True"></asp:Label>&nbsp;
                            <asp:Label ID="lblApplicationIDLabel2" runat="server" Visible="True">Application ID: </asp:Label>
                            <asp:Label ID="lblApplicationID2" runat="server" ForeColor="#006600" Visible="True"></asp:Label>
                            <br />
                             <asp:Label ID="lblApptypeLabel2" runat="server" Visible="True">Application Type: </asp:Label>
                            <asp:Label ID="lblApptype2" runat="server" ForeColor="#006600" Visible="True"></asp:Label>
                        </asp:Panel>
                    </center>
                </td>
            </tr>
        </table>
            <center>
        <table class="CountySelection" style="width: 100%;">
                        <tr>
                            <td >                              
                                <asp:Label  ID="Label23" runat="server" Font-Bold="True" Text="Summary Regeneration Page"></asp:Label> 
                            </td>                                              
                        </tr>
                    </table></center> <br />

    <asp:ScriptManager ID="smCert" runat="server">
    </asp:ScriptManager>
       <script type="text/javascript" language="javascript">

           Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

           function EndRequestHandler(sender, args) {

               if (args.get_error() != undefined) {

                   args.set_errorHandled(true);

               }

           }

</script>
    <center>
        <asp:Label ID="lblErrMsgSecurity" ForeColor="Red" runat="server"></asp:Label>
     <asp:ValidationSummary ID="valsum1" runat="server" CssClass="ErrorSummary" ValidationGroup="a" />
        </center>
    <center>
    <div id="pError" runat="server" class="ErrorSummary" visible="false"></div>
        <br />
    </center>
    <center>
        <div id="pnote" runat="server" class="Note"><asp:Label ID="lblNote" ForeColor="Green" runat="server"></asp:Label></div>
        </center>
<%--    <div>
<center>--%>
    <br />
    <div id="dPrint" class="dPrint" runat="server">
    <input type="button" id="bPrintWindow" runat="server" value="Print" onclick ="window.print();" />
</div>
    <br /><br />
    <label style="color: #000000; font-weight: bold" class="CountySelection">Personal Information</label>
                        <asp:Panel ID="pPI" runat="server" GroupingText="Personal Information" Width="100%" HorizontalAlign="Left">
                            <br />
                            <table cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left"><asp:Label runat="server" ID="Label6" Font-Bold="true">First Name:</asp:Label>&nbsp;</td><td><asp:Label runat="server" ID="lblFirstName" /></td>
                                    <td align="left"><asp:Label runat="server" ID="Label1" Font-Bold="true">Last Name:</asp:Label>&nbsp;</td><td><asp:Label runat="server" ID="lblLastName" /></td>
                                     <td colspan="1" ><asp:Label runat="server" ID="Label12" Font-Bold="true">Middle Name:</asp:Label></td><td><asp:Label runat="server" ID="lblMiddleName" /></td>
                                    <td >
                                       <asp:Label runat="server" ID="Label13"/>
                                    </td>
                                 </tr>
                                <tr>
                                    <td align="left"><asp:Label runat="server" ID="lblRNLNoOrSSN" Font-Bold="true"></asp:Label>&nbsp;</td><td><asp:Label runat="server" ID="lblRNLNoOrSSNtxt" /></td>
                                    <td align="left"><asp:Label runat="server" ID="lblDtIssuedOrDOB" Font-Bold="true"></asp:Label>&nbsp;</td><td><asp:Label runat="server" ID="lblDtIssuedOrDOBtxt" /></td>
                                     <td colspan="1" ><asp:Label runat="server" ID="Label14" Font-Bold="true">Gender:</asp:Label></td>
                                        <td align="left">
                                            <asp:RadioButtonList runat = "server" ID = "rdbGender"  Enabled ="false"  CssClass = "rdbGender"
                                                RepeatDirection="Horizontal">
                                                <asp:ListItem Value="0" Text="M"></asp:ListItem>
                                                <asp:ListItem Value="1" Text="F"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                </tr>
                                 <tr>
        <td colspan="1"><asp:Label runat="server" ID="Label2" Font-Bold="true">Address Line 1:</asp:Label></td>
        <td >
            <asp:Label runat="server" ID="lblAddr1"/>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label3" Font-Bold="true">Address Line 2:</asp:Label></td>
        <td >
            <asp:Label runat="server" ID="lblAddr2"/>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label4" Font-Bold="true">City:</asp:Label></td>
        <td >
           <asp:Label runat="server" ID="lblCity"/>
        </td>
    </tr>
    <tr>
        <td colspan="1"><asp:Label runat="server" ID="Label5" Font-Bold="true">State:</asp:Label></td>
        <td >
           <asp:Label runat="server" ID="lblState"/>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label7" Font-Bold="true">Zip:</asp:Label></td>
        <td  >
           <asp:Label runat="server" ID="lblZip"/>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label8" Font-Bold="true">County:</asp:Label></td>
        <td>
           <asp:Label runat="server" ID="lblCounty"/>
        </td>
    </tr>       
                            </table>
                         </asp:Panel>

    <br />
    <label style="color: #000000; font-weight: bold" class="CountySelection">Contact Information</label>
    <br />
    <asp:Panel ID = "pnlPersonalContact" runat = "server" 
        GroupingText="Personal Contact Information">
         <table cellpadding="2" cellspacing="0" width="100%">               
            <tr style="font-weight: bold;">
                <td  > </td>
                <td  >Home</td>
                <td  >Work</td>
                <td >Cell/Other</td>
                <td >&nbsp;</td>
                <td>&nbsp;</td>
                </tr>  
                <tr>
                    <td colspan="1" align = "left" style="font-weight: bold;">Telephone Number:</td>
                    <td align = "left" colspan="1" >
                        <asp:Label runat="server" ID="lblHomePhoneNumber"/>                   
                    </td>
                    <td align = "left" colspan="1"  >
                            <asp:Label runat="server" ID="lblWorkPhoneNumber"/>                     
                    </td>
                    <td align = "left" colspan="1" >
                        <asp:Label runat="server" ID="lblCellPhoneNumber"/>                     
                    </td>
                    <td colspan="1">&nbsp;</td>
                    <td colspan="1">&nbsp;</td>
                </tr>
                <tr>
                    <td colspan="1" align = "left" style="font-weight: bold;">Email Address:</td>
                    <td align = "left">
                            <asp:Label runat="server" ID="lblHomeAddress"/>                                                    
                    </td>
                    <td align = "left">
                            <asp:Label runat="server" ID="lblWorkAddress"/>                      
                    </td>
                    <td align = "left">
                            <asp:Label runat="server" ID="lblCellAddress"/>                      
                    </td>
                    <td colspan="1">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                    <td colspan="1">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;</td>
                </tr>
               
         </table>
     </asp:Panel>
    <br />
   <%-- <asp:Panel ID="pContacts" runat="server" GroupingText="Contact Information" Width="100%" HorizontalAlign="Left">
                <asp:GridView ID="gvContacts" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="Grid">
                            <Columns>
                            <asp:BoundField DataField="CTypeDesc" HeaderText="Contact Type" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="CPhone" HeaderText="Phone Number" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="CEmail" HeaderText="Email Address" HeaderStyle-BackColor="#BFE4FF"/>
                            
                            </Columns>
               </asp:GridView>


    </asp:Panel>--%>
    <div style="text-align: left">
        <input type="button" id="btnToggleEmp" runat="server" value="View Employers" />
        <hr style="line-height: 1px" />
        
    </div> 
 <div id="dEmployers" class="dEmployers" runat="server">
    <label style="font-weight: bold;" class="CountySelection">Employers Information</label>
<asp:Repeater id="rptEmp" runat="server" >

<ItemTemplate>
    <asp:Panel ID="pEmployers" runat="server" GroupingText="Employer" Width="100%" HorizontalAlign="Left">
     <table>
    <tr>
         <td colspan="1"><strong>Employer Name:</strong></td>
            <td>
                <%# DataBinder.Eval(Container.DataItem, "EmployerName")%>
            </td>
            <td ><strong><asp:Label ID="lblEmpIdentification" runat="server" Text="RNLicense/ProviderNumber:" ></asp:Label></strong></td>
            <td>
                <%# DataBinder.Eval(Container.DataItem, "EmployerTaxID")%>
         </td>
        <td ><strong>DODD Provider#:</strong></td>
            <td>
                <%# DataBinder.Eval(Container.DataItem, "DODDProviderContractNumber")%>
         </td>
    </tr>
    <%--<tr>
         <td colspan="1"><strong>Certification StartDate:</strong></td>
            <td>
                <%# DataBinder.Eval(Container.DataItem, "EmployerName")%>
            </td>
            <td colspan="1"><strong>Certification EndDate:</strong></td>
            <td>
                <input type = "text" id = "txtCertEndDate" runat = "server" size = "10" maxlength = "15" class="date-pick txtCertEndDate" tabindex="7" />
         </td>
         <td><strong>Certification Status:</strong></td>
            <td>
                <input type = "text" id = "txtCertStatus" runat = "server" size = "5" maxlength = "15" class ="txtCertStatus" tabindex="8" />
         </td>
    </tr>--%>
    <tr>
         <td colspan="1"><strong>Employment StartDate:</strong></td>
            <td>
                <%# DataBinder.Eval(Container.DataItem, "EmployerStartDate", "{0:MM/dd/yyyy}")%>
            </td>
            <td colspan="1"><strong>Employment EndDate:</strong></td>
            <td >
                <%# DataBinder.Eval(Container.DataItem, "EmployerEndDate", "{0:MM/dd/yyyy}")%>
         </td>
    </tr>
    <tr>
         <td colspan="1"><strong>CEO LastName:</strong></td>
            <td>
               <%# DataBinder.Eval(Container.DataItem, "CEOLastName")%>
            </td>
            <td colspan="1"><strong>CEO FirstName:</strong></td>
            <td>
               <%# DataBinder.Eval(Container.DataItem, "CEOFirstName")%>
            </td>
            <td class="style1" colspan="1"><strong>CEO MI:</strong></td>
            <td>
                <%# DataBinder.Eval(Container.DataItem, "CEOMiddleName")%>
            </td>
    </tr>
    <tr>
        <td colspan="1"><strong>Supervisor LastName:</strong></td>
            <td>
                <%# DataBinder.Eval(Container.DataItem, "SupervisorLastName")%>
            </td>
            <td colspan="1"><strong>Supervisor FirstName:</strong></td>
            <td>
                 <%# DataBinder.Eval(Container.DataItem, "SupervisorFirstName")%>
            </td>
    </tr>
    <tr>
        <td colspan="1"><strong>Supervisor Phone Number:</strong></td>
            <td>
                <%# DataBinder.Eval(Container.DataItem, "SupervisorPhoneEmail.Phone")%>
            </td>
            <td colspan="1"><strong>Supervisor Email Address:</strong></td>
            <td>
                <%# DataBinder.Eval(Container.DataItem, "SupervisorPhoneEmail.Email")%>
            </td>
    </tr>
         </table>
    <strong>Agency Address:</strong>
        <table cellpadding="2" cellspacing="0">
        <tr>
        <td colspan="1"><asp:Label runat="server" ID="Label9" Font-Bold="true">Address Line 1:</asp:Label></td>
        <td >
           <%# DataBinder.Eval(Container.DataItem, "AgencyLocationAddress.AddressLine1")%>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label11" Font-Bold="true">Address Line 2:</asp:Label></td>
        <td >
            <%# DataBinder.Eval(Container.DataItem, "AgencyLocationAddress.AddressLine2")%>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label13" Font-Bold="true">City:</asp:Label></td>
        <td >
           <%# DataBinder.Eval(Container.DataItem, "AgencyLocationAddress.City")%>
        </td>
    </tr>
    <tr>
        <td colspan="1"><asp:Label runat="server" ID="Label15" Font-Bold="true">State:</asp:Label></td>
        <td >
           <%# DataBinder.Eval(Container.DataItem, "AgencyLocationAddress.State")%>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label17" Font-Bold="true">Zip:</asp:Label></td>
        <td  >
           <%# DataBinder.Eval(Container.DataItem, "AgencyLocationAddress.Zip")%>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label19" Font-Bold="true">County:</asp:Label></td>
        <td>
           <%# DataBinder.Eval(Container.DataItem, "AgencyLocationAddress.County")%>
        </td>
    </tr>     
            </table>  

        <strong>Work Location Address:</strong>
        <table cellpadding="2" cellspacing="0">
        <tr>
        <td colspan="1"><asp:Label runat="server" ID="Label10" Font-Bold="true">Address Line 1:</asp:Label></td>
        <td >
           <%# DataBinder.Eval(Container.DataItem, "WorkAgencyLocationAddress.AddressLine1")%>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label12" Font-Bold="true">Address Line 2:</asp:Label></td>
        <td >
            <%# DataBinder.Eval(Container.DataItem, "WorkAgencyLocationAddress.AddressLine2")%>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label14" Font-Bold="true">City:</asp:Label></td>
        <td >
           <%# DataBinder.Eval(Container.DataItem, "WorkAgencyLocationAddress.City")%>
        </td>
    </tr>
    <tr>
        <td colspan="1"><asp:Label runat="server" ID="Label16" Font-Bold="true">State:</asp:Label></td>
        <td >
           <%# DataBinder.Eval(Container.DataItem, "WorkAgencyLocationAddress.State")%>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label18" Font-Bold="true">Zip:</asp:Label></td>
        <td  >
           <%# DataBinder.Eval(Container.DataItem, "WorkAgencyLocationAddress.Zip")%>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label20" Font-Bold="true">County:</asp:Label></td>
        <td>
           <%# DataBinder.Eval(Container.DataItem, "WorkAgencyLocationAddress.County")%>
        </td>
    </tr>     
            </table>  
     </asp:Panel>
</ItemTemplate>

</asp:Repeater>
    </div>
    <div style="text-align: left" id="dBtnWE" runat="server">
        <div style="text-align:left">
    <input type="button" style="text-align: left" id="btnToggleWE" runat="server" value="View Work Experience" />
        </div>
          <hr style="line-height: 1px" />
    </div>
  
 <div id="dWE" class="dWE" runat="server">
    <label style="font-weight: bold;" class="CountySelection">Work Experience Information</label>
   <asp:Repeater id="rptWE" runat="server" >

<ItemTemplate>

    <asp:Panel ID="pWorkExp" runat="server" GroupingText="Work Experience" Width="100%" HorizontalAlign="Left">
                   <table class="leftAlign">
                       <tr>
                            <td colspan="1"><strong>Agency/Employer Name:</strong></td>
                            <td >
                                 <%# DataBinder.Eval(Container.DataItem, "EmpName")%>                               
                            </td>
                           <td colspan="1" ><strong>Experience :</strong></td>
                            <td align="left" colspan="1">      
                                <input type="checkbox" disabled="disabled" class="chkRNExp" id="chkRNExp" runat="server" checked='<%# DataBinder.Eval(Container.DataItem, "ChkRNFlg")%>' />RN &nbsp;
                                 <input type="checkbox" disabled="disabled" class="chkDDExp" id="chkDDExp" runat="server" checked='<%# DataBinder.Eval(Container.DataItem, "ChkDDFlg")%>' />DD
                            </td>                          
                       </tr>
                       <tr>
                            <td colspan="1"><strong>Start Date:</strong></td>
                            <td >
                                <%# DataBinder.Eval(Container.DataItem, "EmpStartDate", "{0:MM/dd/yyyy}")%>
                            </td>
                            <td colspan="1" ><strong>End Date:</strong></td>
                            <td >
                                <%# DataBinder.Eval(Container.DataItem, "EmpEndDate", "{0:MM/dd/yyyy}")%>                                 
                            </td>                                                      
                       </tr>
                       <tr>
                           <td colspan="1" ><strong>Designation/Title:</strong></td>
                            <td >
                                <%# DataBinder.Eval(Container.DataItem, "Title")%>                             
                            </td>
                            <td colspan="1" ><strong>Role/Duties:</strong></td>
                            <td colspan="1">
                                <asp:Label runat="server" ID="txtRoles"><%# DataBinder.Eval(Container.DataItem, "JobDuties")%></asp:Label>                                 
                            </td>
                       </tr>
                   </table>
        <table cellpadding="2" cellspacing="0">
        <tr>
        <td colspan="1"><asp:Label runat="server" ID="Label9" Font-Bold="true">Address Line 1:</asp:Label></td>
        <td >
           <%# DataBinder.Eval(Container.DataItem, "Address.AddressLine1")%>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label11" Font-Bold="true">Address Line 2:</asp:Label></td>
        <td >
            <%# DataBinder.Eval(Container.DataItem, "Address.AddressLine2")%>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label13" Font-Bold="true">City:</asp:Label></td>
        <td >
           <%# DataBinder.Eval(Container.DataItem, "Address.City")%>
        </td>
    </tr>
    <tr>
        <td colspan="1"><asp:Label runat="server" ID="Label15" Font-Bold="true">State:</asp:Label></td>
        <td >
           <%# DataBinder.Eval(Container.DataItem, "Address.State")%>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label17" Font-Bold="true">Zip:</asp:Label></td>
        <td  >
           <%# DataBinder.Eval(Container.DataItem, "Address.Zip")%>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label19" Font-Bold="true">County:</asp:Label></td>
        <td>
           <%# DataBinder.Eval(Container.DataItem, "Address.County")%>
        </td>
    </tr>   
    <tr>
        <td colspan="1"><asp:Label runat="server" ID="Label21" Font-Bold="true">Phone Number:</asp:Label></td>
        <td >
           <%# DataBinder.Eval(Container.DataItem, "Address.Phone")%>
        </td>
        <td colspan="1" ><asp:Label runat="server" ID="Label22" Font-Bold="true">Email Address:</asp:Label></td>
        <td  >
           <%# DataBinder.Eval(Container.DataItem, "Address.Email")%>
        </td>
    </tr>   
            </table>  
    </asp:Panel>
    </ItemTemplate>
       </asp:Repeater>

</div>
    
    <br />

        <div id="divCourse" runat="server">
                <center><label style="font-weight: bold;" class="CountySelection">Training Skills and CEUs</label></center>
    <br /><br />

 <asp:GridView ID="grdCourse" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="Grid"
    DataKeyNames="OBNApprovalNumber">
    <Columns>
        <asp:BoundField DataField="InstructorName" HeaderText="RN Instructor Name" HeaderStyle-BackColor="#BFE4FF" >
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="EndDate" HeaderText="Effective End Date" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="StartDate" HeaderText="Effective Start Date" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="OBNApprovalNumber" HeaderText="Course ID Number" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="CategoryACEs" HeaderText="Category A CEs" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TotalCEs" HeaderText="Total CEs" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Level" HeaderText="Level" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Category" HeaderText="Category" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="CourseDescription" HeaderText="Course Description" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        
<%--                <asp:TemplateField ItemStyle-Width="100%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
            <ItemTemplate>
                <tr><td>
                <asp:Panel ID="pnlOrders" runat="server">
                    <asp:GridView ID="grdSession" runat="server" Width="100%" AutoGenerateColumns="false" CssClass = "ChildGrid">
                        <Columns>
                            <%--<asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>--%>
                                <%-- <img id="imgSession" alt = "" style="cursor: pointer" src="Images/plus.png" />--%>
                                    <%-- <asp:Panel ID="pnlSessionDates" runat="server" Style="display: none">--%>
<%--                                         <asp:GridView ID="grdSessionDates" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                             <Columns>
                                                 <asp:BoundField DataField="Session_Date" HeaderText="Session Date" DataFormatString="{0:MM/dd/yyyy}" />
                                                 <asp:BoundField DataField="Total_CEs" HeaderText="Total CEs" />
                                             </Columns>
                                             <HeaderStyle CssClass="gridviewHeader" />
                                             <EmptyDataTemplate>
                                                 <asp:Label ID="lblSessionDateEmpt" runat="server" Text="No dates to view"></asp:Label>
                                             </EmptyDataTemplate>
                                         </asp:GridView>--%>

                                    <%-- </asp:Panel>
                                </ItemTemplate>
                            </asp:TemplateField>--%>
<%--                            <asp:BoundField DataField="Session_Start_Date" HeaderText="Session Start Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                            <asp:BoundField DataField="Session_End_Date" HeaderText="Session End Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                            <asp:BoundField DataField="SessionAddressInfo.Street_Address" HeaderText="Street Address" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="Location_Name" HeaderText="Location Name" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="SessionAddressInfo.City" HeaderText="City" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="SessionAddressInfo.State" HeaderText="State" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="SessionAddressInfo.ZipWithPlus4" HeaderText="Zip" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                
                            </asp:TemplateField> 
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
                </td></tr>
            </ItemTemplate>


        </asp:TemplateField>--%>
    </Columns>
     <EmptyDataTemplate>
         <asp:Label ID="Label10" runat="server" Text="This application does not have any courses assigned."></asp:Label>
     </EmptyDataTemplate>
</asp:GridView>
    
                        <asp:GridView ID="grdSession" runat="server" Width="100%" AutoGenerateColumns="false" CssClass = "ChildGrid">
                        <Columns>
                            <asp:BoundField DataField="Session_Start_Date" HeaderText="Session Start Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                            <asp:BoundField DataField="Session_End_Date" HeaderText="Session End Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                            <asp:BoundField DataField="SessionAddressInfo.Street_Address" HeaderText="Street Address" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="Location_Name" HeaderText="Location Name" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="SessionAddressInfo.City" HeaderText="City" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="SessionAddressInfo.State" HeaderText="State" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="SessionAddressInfo.ZipWithPlus4" HeaderText="Zip" HeaderStyle-BackColor="#BFE4FF"/>
                        </Columns>
                    </asp:GridView>
    </div>
    <br />

    <div id="divSkillGrid" runat="server" style="width:100%;">
        <label style="font-weight: bold;" class="CountySelection">Skills</label>
        <br /><br />
            <asp:GridView ID="grvSkillsData" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="Grid" DataKeyNames="Skill_Verification_Type_CheckList_Xref_Sid">
                <HeaderStyle CssClass="gridviewHeader" />
                <Columns>
                    <asp:BoundField DataField="Skill_Verification_Skill_Type_Xref_Sid" HeaderText="ID" Visible="False" />
                    <asp:BoundField DataField="Verification_Date" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="Verified_Person_Title" HeaderText="Title" />
                    <asp:BoundField DataField="Verified_Person_Name" HeaderText="Person Verifying Skill" />
                    <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                    <asp:BoundField DataField="Skill_Verification_Skill_Type" HeaderText="Skill Verified" />
                    <asp:BoundField DataField="Skill_CheckList_Name" HeaderText="Skill CheckList" />
                    <%--<asp:CommandField DeleteText="Remove" ShowDeleteButton="True" />--%>
                </Columns>
            </asp:GridView>
        </div>
    <br />
        <asp:Panel ID="PCeus" runat="server">
        <label style="font-weight: bold;" class="CountySelection">CEUs</label>
        <br /><br />
            <asp:GridView ID="grvCeus" runat="server" Width="100%" AutoGenerateColumns="False" CssClass="Grid" DataKeyNames="CEUs_Renewal_Sid">
                <HeaderStyle CssClass="gridviewHeader" />
                <Columns>
                    <asp:BoundField DataField="CEUs_Renewal_Sid" HeaderText="ID" Visible="False" />
                    <asp:BoundField DataField="Category_Type_Sid" HeaderText="Category" />
                    <asp:BoundField DataField="Attended_Date" HeaderText="Attended Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="Total_CEUs" HeaderText="Total Ceus" />
                    <asp:BoundField DataField="Instructor_Name" HeaderText="Instructor Name" />
                    <asp:BoundField DataField="Title" HeaderText="Title" />
                    <asp:BoundField DataField="Course_Description" HeaderText="Course Description" />
                </Columns>
            </asp:GridView>
        </asp:Panel>
    <br />
    <asp:Panel ID="pDocUpload" runat="server">
    <label style="font-weight: bold;" class="CountySelection">Document Download/Upload</label>
       <asp:Panel ID="Panel96" runat="server" GroupingText="Documents Uploaded" BorderStyle="Solid"
                                                BorderColor="Black" BorderWidth="1" BackColor="#EFF3FB">
                                                <br />
                                                <asp:Panel ID="Panel1" runat="server" Width="100%" ScrollBars="Auto"
                                                    BackColor="#EFF3FB">
                                                    <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="False"
                                                        AutoGenerateDeleteButton="false" DataKeyNames="ImageSID" BackColor="#EFF3FB" BorderColor="Black"
                                                        BorderStyle="Solid" BorderWidth="1px" Font-Names="Arial" Font-Size="Small" Width="100%">
                                                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                        <Columns>
                                                            <asp:CommandField SelectText="View" ShowCancelButton="False" ShowSelectButton="True" />
                                                            <asp:BoundField DataField="DocumentName" HeaderText="FileName" />
                                                            <asp:BoundField DataField="DocumentType" HeaderText="Requirement" />
                                                        </Columns>
                                                        <AlternatingRowStyle BackColor="White" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </asp:Panel>
    <br />
        </asp:Panel>
    <div id="dAttestations" runat="server" style="width:100%">
    <label style="font-weight: bold;" class="CountySelection">RN Attestation</label> <br />
    <asp:Panel ID="pAttestationQuestions" runat="server" GroupingText="Attestation Questions" Width="100%" HorizontalAlign="Left">

        </asp:Panel>
    <br />
           <asp:Label ID="lblAttestation" runat="server" Text=""></asp:Label><br />
    </div>
    <br />
    <asp:Panel ID="pNotations" runat="server" BorderWidth="0">
        <label style="font-weight: bold;" class="CountySelection">Notations for Application</label> <br />
        <asp:Panel ID="pNotInternal" runat="server" GroupingText="Notations" BorderStyle="Solid" BorderColor="Black" BorderWidth="1">

            <br />
    <asp:GridView ID="grdNotationsSum" runat="server" DataKeyNames="AppNotId" Width="100%" AutoGenerateColumns="false" CssClass="Grid">
                            <Columns>
                                <asp:BoundField DataField="AppNotId" Visible="false" HeaderText="Notation Type" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="NotationType.NTypeDesc" HeaderText="Notation Type" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="AllReasons" HeaderText="Notation Reason" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="PersonEnteringNotation" HeaderText="Person Entering Notation" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="PersonTitle" HeaderText="Person Title" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="NotationDate" HeaderText="Notation Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                                <asp:BoundField  DataField="OccurenceDate" HeaderText="Occurence Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                                <asp:BoundField DataField="UnflaggedDate" HeaderText="Unflagged Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                            </Columns>
    </asp:GridView>

            </asp:Panel>
    </asp:Panel>
<center>
    <br />
   <div class="NavigationMenu" id="dViewPrint" runat="server" style="text-align: center">
<%--<input type="button" id="btnCertHist" runat="server" class="bCertHist" value="View Certification History" onclick ="location.href = 'StartPage.aspx';" onserverclick="btnCertHist_Click" />&nbsp;&nbsp;&nbsp;--%>
<input type="button" id="btnPrinterFriendly" runat="server" value="Print Application" onclick ="window.open('Summary.aspx?newwin=yes', 'PrintFriendly', 'menubar=1,toolbar=0,width=1080,height=600,location=0,scrollbars=1,resizable=1');" />
   </div>

</center>
    <br />
<%--     <asp:Panel ID="pCertHist" runat="server" CssClass="pCH" GroupingText="Certificate History" Width="100%" HorizontalAlign="Left">
                <asp:GridView ID="gvCertHist" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="Grid">
                            <Columns>
                            <asp:BoundField DataField="Role" HeaderText="Role" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="Category" HeaderText="Category" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="Level" HeaderText="Level" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="StartDate" HeaderText="Start Date" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="EndDate" HeaderText="End Date" HeaderStyle-BackColor="#BFE4FF"/>
                            </Columns>
               </asp:GridView>
    </asp:Panel>
<br />--%>
    <div class="dActions" id="dActions" runat="server">
        <asp:Panel ID="PDates" class="PDates"  GroupingText="Certification Dates" runat="server" HorizontalAlign="Left" Width="100%" Font-Size="9pt" Font-Italic="false">
    &nbsp;&nbsp;Start Date <input type = "text" id ="txtStartDate" class="date-pick txtStartDate" style="Width:100px;" runat = "server"/>&nbsp;&nbsp;&nbsp;&nbsp; End Date
    <input type = "text" disabled="disabled" id ="txtEndDate" class="date-pick txtEndDate" style="Width:100px;" runat = "server"/>

         </asp:Panel>
        <asp:RequiredFieldValidator ID="ReqdVal1" runat="server" ControlToValidate="txtStartDate" Display="None" validationgroup="a"
            ErrorMessage="Please select a Start Date"></asp:RequiredFieldValidator>

    <br />
        <asp:CompareValidator ID="CompareValidator1" runat="server" ErrorMessage="Please enter valid date" ControlToValidate="txtStartDate" ValidationGroup="a" Type="Date" Display="None" Operator="DataTypeCheck"></asp:CompareValidator>
        <input type="hidden" id="hCertTime" runat="server" value="0" />
        <input type="hidden" id="hRNDateOfIssuance" runat="server" value="12/12/1999" />
        <input type="hidden" id="hCourseStartDate" runat="server" value="12/12/1999" />
        <input type="hidden" id="hCertEndDate" class="hCertEndDate" runat="server" value="12/12/1999" />
        <input type="hidden" id="hBiggerDate" class="hBiggerDate" runat="server" value="12/12/1999" />
        <input type="hidden" id="hBiggerName" class="hBiggerName" runat="server" value="" />
     <asp:Panel ID="PStatus" HorizontalAlign="Left" class="PStatus"  GroupingText="" runat="server" Width="100%" Font-Size="9pt" Font-Italic="false">
    <strong class="Error1">Select Application Status/Decision:</strong>&nbsp;<asp:DropDownList ID="ddAppStatus" runat="server" AppendDataBoundItems="false" AutoPostBack="true" OnSelectedIndexChanged="AppStatChanged">
    </asp:DropDownList>
       </asp:Panel>
        <br />
             <asp:Panel ID="PAdminStatus" Visible="false" HorizontalAlign="Left" class="PStatus"  GroupingText="" runat="server" Width="100%" Font-Size="9pt" Font-Italic="false">
    <strong class="Error1">Select Administrative Status/Decision:</strong>&nbsp;<asp:DropDownList ID="ddAdminStatus" runat="server" AppendDataBoundItems="false" AutoPostBack="true" OnSelectedIndexChanged="AppStatChanged">
       <%-- <asp:ListItem Value="0">Select Status</asp:ListItem>--%>
    </asp:DropDownList>
       </asp:Panel>

    <br />

    <%--<p style="color: #FF0000"> ALERT! &nbsp; Once you Select and Save below, only DODD Admin will be able to change certification information. <br />
Are you sure?<br /> </p>--%>
    </div>
    <center>
    <div class="NavigationMenu" id="dNavButtons" runat="server">
    <input type="button" id="btnPrevious" runat="server" value="Previous" />&nbsp;&nbsp;&nbsp;
    <%--<input type="button" id="btnSave" runat="server" class="ButtonStyle" value="Save" style="width: 87px"/>--%>&nbsp;&nbsp;&nbsp;
    <input type="submit" id="btnSaveContinue" runat="server" class="btnSaveContinue" value="Save and Continue" validationgroup="a" causesvalidation="true" />
        </div>
       <div id="divSpinner" runat="server" class="divSpinner" style="font-family:Tahoma, Arial, Verdana; font-size:24pt; font-weight:bold; text-align: center;">
            <br /><br />
        </div> 
    </center>
     <rsweb:ReportViewer ID="rvCertificate" runat="server" Width="100%" Font-Names="Verdana"
        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
        WaitMessageFont-Size="14pt" ShowBackButton="False" ShowExportControls="False"
        ShowFindControls="False" ShowPageNavigationControls="False" ShowPrintButton="False"
        ShowRefreshButton="False" ShowZoomControl="False" Visible="False">
    </rsweb:ReportViewer>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
