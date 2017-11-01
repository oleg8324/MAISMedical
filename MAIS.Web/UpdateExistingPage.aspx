<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="UpdateExistingPage.aspx.vb" Inherits="MAIS.Web.UpdateExistingPage" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="Scripts/UpdateExistingPage.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <div>
        <center>
        <table class="CountySelection" style="width: 100%;">
                        <tr>
                            <td style="text-align:center;width:85%;">
                               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label  ID="Label13" runat="server" Font-Bold="True" Text="Update Existing Page"></asp:Label>     </td>                   
                           <td style="text-align:right;width:15%;">

                                <asp:LinkButton  ID="lnkBack" Text="Go To Search" runat="server" ></asp:LinkButton> 
                             </td>
                        </tr>
                    </table></center>
       <br />
      <label style="color: #000000; font-weight: bold" class="CountySelection">Personal Information</label>
        <asp:Panel ID="pPI" runat="server" GroupingText="Personal Information" Width="100%" HorizontalAlign="Left">
                            <br />
                            <table cellpadding="2" cellspacing="0" width="100%">
                                <tr>
                                    <td align="left"><asp:Label runat="server" ID="Label6" Font-Bold="true">First Name:</asp:Label>&nbsp;</td><td><asp:Label runat="server" ID="lblFirstName" /></td>
                                    <td align="left"><asp:Label runat="server" ID="Label2" Font-Bold="true">Last Name:</asp:Label>&nbsp;</td><td><asp:Label runat="server" ID="lblLastName" /></td>
                                     <td colspan="1" ><asp:Label runat="server" ID="Label12" Font-Bold="true">Middle Name:</asp:Label></td>
                                    <td >
                                       <asp:Label runat="server" ID="lblMI"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="left"><asp:Label runat="server" ID="lblRNLNoOrSSN" Font-Bold="true"></asp:Label>&nbsp;</td><td><asp:Label runat="server" ID="lblRNLNoOrSSNtxt" /></td>
                                    <td align="left"><asp:Label runat="server" ID="lblDtIssuedOrDOB" Font-Bold="true"></asp:Label>&nbsp;</td><td><asp:Label runat="server" ID="lblDtIssuedOrDOBtxt" /></td>
                                     <td colspan="1" ><asp:Label runat="server" ID="Label14" Font-Bold="true">Gender:</asp:Label></td>
                                    <td align="left">
                                        <asp:RadioButtonList runat = "server" ID = "rdbGender"  CssClass = "rdbGender" Enabled ="false"
                                            RepeatDirection="Horizontal">
                                            <asp:ListItem Value="1" Text="M"></asp:ListItem>
                                            <asp:ListItem Value="2" Text="F"></asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                 <tr>
                                    <td colspan="1"><asp:Label runat="server" ID="Label3" Font-Bold="true">Address Line 1:</asp:Label></td>
                                    <td >
                                        <asp:Label runat="server" ID="lblAddr1"/>
                                    </td>
                                    <td colspan="1" ><asp:Label runat="server" ID="Label7" Font-Bold="true">Address Line 2:</asp:Label></td>
                                    <td >
                                        <asp:Label runat="server" ID="lblAddr2"/>
                                    </td>
                                    <td colspan="1" ><asp:Label runat="server" ID="Label8" Font-Bold="true">City:</asp:Label></td>
                                    <td >
                                       <asp:Label runat="server" ID="lblCity"/>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="1"><asp:Label runat="server" ID="Label9" Font-Bold="true">State:</asp:Label></td>
                                    <td >
                                       <asp:Label runat="server" ID="lblState"/>
                                    </td>
                                    <td colspan="1" ><asp:Label runat="server" ID="Label10" Font-Bold="true">Zip:</asp:Label></td>
                                    <td  >
                                       <asp:Label runat="server" ID="lblZip"/>
                                    </td>
                                    <td colspan="1" ><asp:Label runat="server" ID="Label11" Font-Bold="true">County:</asp:Label></td>
                                    <td>
                                       <asp:Label runat="server" ID="lblCounty"/>
                                    </td>
                                </tr>                                  
                            </table>
                         </asp:Panel>
    <br />
     <label style="color: #000000; font-weight: bold" class="CountySelection">Personal Contact Information</label>
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
                        <asp:Label runat="server" ID="lblHomePhoneNumber" CssClass="PhoneFormat"/>                   
                    </td>
                    <td align = "left" colspan="1"  >
                            <asp:Label runat="server" ID="lblWorkPhoneNumber" CssClass="PhoneFormat"/>                     
                    </td>
                    <td align = "left" colspan="1" >
                        <asp:Label runat="server" ID="lblCellPhoneNumber" CssClass="PhoneFormat"/>                     
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
       <br /> <label style="color: #000000; font-weight: bold" class="CountySelection">Employer Information</label>
        <div style="text-align:left;" > <%--<asp:Button runat="server" ID="btnEmpHistory" Text="View Employer Information"/>   --%>         
        <asp:Panel ID="pnlEmpHistory" runat="server" GroupingText="Employer Information" Width="100%" HorizontalAlign="Left">       
                 <asp:Label runat="server" style="color:red; font-weight: bold"  ID="lblEmperror"/>  
                    <asp:GridView ID="gvEmpInformation" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="Grid">
                                <Columns>
                                <asp:BoundField DataField="EmployerType" HeaderText="Employer Type" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="EmployerName" HeaderText="Employer Name" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="CEOFirstName" HeaderText="CEO Name" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="SupervisorFirstName" HeaderText="Supervisor Name" HeaderStyle-BackColor="#BFE4FF"/>                            
                                </Columns>
                   </asp:GridView>
        </asp:Panel></div>  <br />
         <label style="color: #000000; font-weight: bold" class="CountySelection">Current Certification Information</label>    <br />     
            <%-- <asp:Panel ID="pnlCurrentCertification" runat="server" CssClass="pCH" GroupingText="Current Certification" Width="100%" HorizontalAlign="Left">
                <asp:GridView ID="gvCurrentCertification" runat="server" Width="100%" AutoGenerateColumns="false" CssClass="Grid">
                            <Columns>
                                <asp:BoundField DataField="Role" HeaderText="Role" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="Category" HeaderText="Category" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="Level" HeaderText="Level" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="StartDate" HeaderText="Start Date" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="EndDate" HeaderText="End Date" HeaderStyle-BackColor="#BFE4FF"/>                            
                            </Columns>
               </asp:GridView>
    </asp:Panel><br />--%>
        <div style="text-align:left;" >
      <%-- <asp:Button  runat="server" ID="btnViewCertHistory" Visible="false" Text="View Certification History"/><br />--%>
             <asp:Panel ID="pnlCertHistory" runat="server" GroupingText="Certification History" CssClass="pCH"  Width="100%" HorizontalAlign="Left">
                   <asp:Label runat="server" style="color:red; font-weight: bold"  ID="lblCerterr"/>  
                <asp:GridView ID="gvCertHistory" runat="server" DataKeyNames="Role_RN_DD_Personnel_Xref_Sid" Width="100%" AutoGenerateColumns="false" CssClass="Grid">
                    <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <HeaderStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" BorderColor="Black" />
                            <Columns>                               
                                <asp:BoundField DataField="Role" HeaderText="Role" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="Category" HeaderText="Category" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="Level" HeaderText="Level" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="Status" HeaderText="Status" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="StartDate" HeaderText="Start Date" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="EndDate" HeaderText="End Date" HeaderStyle-BackColor="#BFE4FF"/>     
                                  <asp:TemplateField HeaderText="" InsertVisible="False" 
                                    ShowHeader="False" FooterStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                        <asp:HiddenField ID="hdRoleLevelCategory"  Value ='<%# Eval("Role_Category_Level_Sid")%>'  runat="server" />                                      
                                    </ItemTemplate>
                                </asp:TemplateField>                                                                                               
                                <asp:TemplateField HeaderText="" InsertVisible="False" 
                                    ShowHeader="False" FooterStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                       <asp:LinkButton ID="lnkAction" runat="server" Text="Desired Action"   
                                        CommandName="DesiredOptions" CommandArgument='<%# Eval("EndDate")%>'> 
                                       </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>    
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkQAUnregister" runat="server" Text="UnRegister QA"   
                                        CommandName="QAUnRegister" CommandArgument='<%# Eval("Role_RN_DD_Personnel_Xref_Sid")%>' OnClientClick="if (!confirm('Are you sure you want to Unregister?')) return false;"> 
                                       </asp:LinkButton>
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
    </asp:Panel><br />        
        <label runat="server" id="lblAppstatus" style="font-weight: bold"></label> <asp:LinkButton runat="server" ID="lnkApplication" Text="Click Here"></asp:LinkButton><br /><br />
            </div>
       
        <asp:Panel ID="pnlAction" BorderWidth="1" runat="server">
             <label style="color: #000000; font-weight: bold" class="CountySelection">Select Desired Action Below</label> 
            <table>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="lnkInitial">Initial Certification or Registration</asp:LinkButton><br />
                         <label id="lblInitial">Choose this if current certification status above is denied or expired.</label><br />
                         <label id="Label4">(To inquire about Revocation,Suspention,Voluntary Withdrawal,Revocation contcat DODD medication administration).</label>
                    </td>
                </tr>                             
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="lnkAddon">Add-On</asp:LinkButton><br />
                        <asp:label id="lblAddOn" runat="server" >Choose this to add additional certifications, registrations or categories of training to an existing current certification.</asp:label>
                    </td>
                </tr>               
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="left">    <asp:Label ForeColor="Red" ID="lblMsgRenewal" runat="server" ></asp:Label><br />                    
                        <asp:LinkButton runat="server" ID="lnkRenewal">Renewal</asp:LinkButton><br />
                        <label id="Label5">Choose this to renew current certification.</label>
                    </td>
                </tr>               
                <tr>
                    <td>&nbsp;</td>
                </tr>
                 <tr>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="lnkUpdate" CssClass="lnkUpdate" Text="Update Profile"></asp:LinkButton><br />
                        <label id="Label15">Choose this to Add new employer,Name change,Change/Add supervisor,New work location.</label>                       
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                 <tr>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="lnkNotation">Add/Update/View Notations</asp:LinkButton><br />
                        <label id="Label26">Choose this to add,update or view notations for certified person.</label>
                    </td>
                </tr>
                 <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                     <td align="left">
                        <asp:LinkButton runat="server" ID="lnkContactInfo">Update Personal Contact Information</asp:LinkButton><br />
                        <label id="Label22">Choose this to update personnal contact information for certified person.</label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="left">
                        <asp:LinkButton runat="server" ID="lnkAddUpdateCeus">Add CEUs </asp:LinkButton><br />
                        <label id="Label23">Choose this to add CEUs for certified person.</label>
                    </td>
                </tr>
                 
                 <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="left">
                         <asp:LinkButton runat="server" ID="lnkAddSkills">Add Skills </asp:LinkButton><br />
                         <label id="Label25">Choose this to add skills for certified DD Personnal.</label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                 <tr>
                    <td align="left">
                         <asp:LinkButton runat="server" ID="lnkViewPrintDocument">View/Print Documents </asp:LinkButton><br />
                         <label id="Label1">Choose this to view or print documnets.</label>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                 <tr>
                    <td align="left">
                         <asp:Panel ID="pnlCertAdminPage" ForeColor="Blue" GroupingText ="Administration(DODD Admin Only)" runat ="server" >                            
                            <ul>
                                <li>  <asp:LinkButton runat="server" ID="lnkCertRegerenation">Certification Document Regeneration (DODD Admin Only)</asp:LinkButton><br />
                                     <label id="lblCertRegerenation" runat="server" style="color:black">Choose this to regenerate certification documents </label>
                                </li>
                                <li><asp:LinkButton runat="server" ID="lnkCertAdmin">Certification Administration(DODD Admin Only)</asp:LinkButton>
                                    <br /> <label id="Label16" style="color:black">Choose this to Suspend, Intent To Revoke,Revoke,Voluntary Withdrawal and Edit certification dates.</label>
                                </li>
                                <li><asp:LinkButton runat="server" ID="lnkRnlicensechange">Change RN License Number</asp:LinkButton></li>
                            </ul>                                                                             
                        </asp:Panel>
                        <asp:Panel runat ="server" GroupingText="Change RN License Number" ID="pnlRNChange" Visible="false"  >
                         <center>  Existing RN Number : <label id="lblrnExistingNumber" runat="server"  style="color:black"></label> <br />
                           New RN Number: <asp:TextBox runat ="server" CssClass="txtRnLicenseNumber" ID="txtRnLicenseNumber" ></asp:TextBox>
                          <br />
                          <asp:Button ID ="btnSaveRN" Text="Save" runat="server"  />&nbsp;&nbsp;  <asp:Button ID ="btnCancel" Text="Cancel" runat="server"  /><br />
                              <label id="lblRNChangeErrorMsg" runat="server"  style="color:red"></label>
                         </center>
                        </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td align="left">
                      
                    </td>
                </tr>
                
            </table>
        </asp:Panel>
        </div>
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
