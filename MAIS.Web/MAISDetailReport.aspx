<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="MAISDetailReport.aspx.vb" Inherits="MAIS.Web.MAISDetailReport" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
     <center>
        <table class="CountySelection" style="width: 100%;">
                        <tr>
                            <td style="text-align:center;width:85%;">
                               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label  ID="Label13" runat="server" Font-Bold="True" Text="MAIS Detail Report Page"></asp:Label>     </td>                   
                           <td style="text-align:right;width:15%;">
                                <asp:LinkButton  ID="lnkBack" Text="Go To Search" runat="server" ></asp:LinkButton>                                 
                             </td>
                            <td>  <input type="button" id="bPrintWindow" runat="server" value="Print Page" onclick ="window.print();" /></td>
                        </tr>
                    </table></center>
       <br />
     <asp:Label ID="lblErrorMsg" ForeColor="Red" runat="server" ></asp:Label><br />  
     <label style="color: #000000; font-weight: bold" class="CountySelection">Personal Information</label>
        <asp:Panel ID="pPI" runat="server" GroupingText="" Width="100%" HorizontalAlign="Left">
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
        GroupingText="">
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
    <br />
     <label style="color: #000000; font-weight: bold" class="CountySelection">Certification Information</label>
     <asp:GridView ID="gvCertification" runat="server" AutoGenerateColumns="false" CssClass = "ChildGrid">
                        <EditRowStyle BackColor="#999999" />
                         <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                        <HeaderStyle CssClass="gridviewHeader" BackColor="#BFE4FF" Font-Bold="True" ForeColor="black" />
                        <Columns>
                             <asp:BoundField DataField="Certification_Type" HeaderText="Certification Type" /> 
                             <asp:BoundField DataField="Certification_Status" HeaderText="Certification Status" /> 
                             <asp:BoundField DataField="Certification_Start_Date" HeaderText="Certification Start Date" DataFormatString="{0:MM/dd/yyyy}" /> 
                             <asp:BoundField DataField="Certification_End_Date" HeaderText="Certification End Date" DataFormatString="{0:MM/dd/yyyy}" /> 
                             <asp:BoundField DataField="Attestant_Name" HeaderText="Attestant Name " /> 
                             <asp:BoundField DataField="RenewalCount" HeaderText="RenewalCount" />
                        </Columns>
                        <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
                        <PagerStyle BackColor="#BFE4FF" ForeColor="Black" HorizontalAlign="Center" />
                        <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />                       
                    </asp:GridView>
    <br />
     <label style="color: #000000; font-weight: bold" class="CountySelection">Course Information</label>
      <asp:GridView ID="gvCourse" runat="server" AutoGenerateColumns="False" Width="100%" CssClass = "ChildGrid">
                         <EditRowStyle BackColor="#999999" />
                         <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                        <HeaderStyle CssClass="gridviewHeader" BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                         <Columns>
                             <asp:BoundField DataField="Course_Number" HeaderText="Course ID Number " /> 
                             <asp:BoundField DataField="Trainer_Name" HeaderText="Trainer Name " /> 
                             <asp:BoundField DataField="Session_Start_Date" HeaderText="Session Start Date " DataFormatString="{0:MM/dd/yyyy}" /> 
                             <asp:BoundField DataField="Session_End_Date" HeaderText="Session End Date " DataFormatString="{0:MM/dd/yyyy}"/> 
                             <asp:BoundField DataField="Session_CEUs" HeaderText="Total Ceus" /> 
                        </Columns>
                        <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
                        <PagerStyle BackColor="#BFE4FF" ForeColor="Black" HorizontalAlign="Center" />
                        <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
          </asp:GridView>
    <br />
    <asp:Panel ID="pnlRenwalCEUS" runat="server" >
    <label style="color: #000000; font-weight: bold" class="CountySelection">Renewal CEUS Information</label>
      <asp:GridView ID="grdCEUSRenewal" runat="server" AutoGenerateColumns="False" Width="100%" CssClass = "ChildGrid">
                         <EditRowStyle BackColor="#999999" />
                         <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                        <HeaderStyle CssClass="gridviewHeader" BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                         <Columns>
                            <asp:BoundField DataField="CEUs_Renewal_Sid" HeaderText="CEU ID" Visible="False" />
                            <asp:BoundField DataField="Category_Type_Code" HeaderText="Category" />
                            <asp:BoundField DataField="Total_CEUs" HeaderText="CEU " />
                            <asp:BoundField DataField="RN_Name" HeaderText="RN" />
                            <asp:BoundField DataField="Instructor_Name" HeaderText="Instructor" />
                            <asp:BoundField DataField="Title" HeaderText="Title" />
                            <asp:BoundField DataField="Course_Description" HeaderText="Description" />
                        </Columns>
                        <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
                        <PagerStyle BackColor="#BFE4FF" ForeColor="Black" HorizontalAlign="Center" />
                        <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
          </asp:GridView>
        </asp:Panel>
    <br />
    <asp:Panel ID="pnlSkills" runat="server" >
    <label style="color: #000000; font-weight: bold" class="CountySelection">Skills Information</label>
      <asp:GridView ID="grdSkills" runat="server" AutoGenerateColumns="False" Width="100%" CssClass = "ChildGrid">
                         <EditRowStyle BackColor="#999999" />
                         <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                        <HeaderStyle CssClass="gridviewHeader" BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                         <Columns>
                             <asp:BoundField DataField="Category_Desc" HeaderText="Category" /> 
                             <asp:BoundField DataField="Skill_Desc" HeaderText="Skill Description " /> 
                             <asp:BoundField DataField="Skill_CheckList_Desc" HeaderText="Skill Check List" /> 
                             <asp:BoundField DataField="Verification_Date" HeaderText="Verification Date" DataFormatString="{0:MM/dd/yyyy}"/> 
                             <asp:BoundField DataField="Verified_Person_Name" HeaderText="Verified Person Name" /> 
                             <asp:BoundField DataField="Verified_Person_Title" HeaderText="Verified Person Title" /> 
                        </Columns>
                        <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
                        <PagerStyle BackColor="#BFE4FF" ForeColor="Black" HorizontalAlign="Center" />
                        <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
                        <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                        <SortedAscendingCellStyle BackColor="#E9E7E2" />
                        <SortedAscendingHeaderStyle BackColor="#506C8C" />
                        <SortedDescendingCellStyle BackColor="#FFFDF8" />
                        <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
          </asp:GridView>
        </asp:Panel>
    <br />
     <label style="color: #000000; font-weight: bold" class="CountySelection">Notation Information</label>
    <asp:GridView ID="grdNotations" runat="server"   Width="100%" AutoGenerateColumns="false" CssClass="Grid">
                            <Columns>                                                                                        
                            <asp:BoundField DataField="AllReasons" HeaderText="Notation Reason" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="PersonEnteringNotation" HeaderText="Person Entering Notation" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="PersonTitle" HeaderText="Person Title" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="NotationDate" HeaderText="Notation Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                                <asp:BoundField  DataField="OccurenceDate" HeaderText="Occurence Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                                   
                                <asp:BoundField DataField="UnflaggedDate" HeaderText="Unflagged Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                            </Columns>
        <EmptyDataTemplate>
         <asp:Label ID="Label10" runat="server" Text="There are no Notations to display."></asp:Label>
     </EmptyDataTemplate>
</asp:GridView>
    <br />
     <label style="color: #000000; font-weight: bold" class="CountySelection">Employer Information</label>
    <asp:GridView runat="server" ID="grdEmployerList" DataKeyNames="EmployerName" AllowSorting="True"
                AutoGenerateColumns="False" PageSize="20" CssClass="grdEmployerList" AllowPaging="True" 
                CellPadding="4" ForeColor="#333333" BorderStyle="Solid" BorderWidth="1px" BorderColor="#222222" >
                <EditRowStyle BackColor="#999999" />
    <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                <HeaderStyle CssClass="gridviewHeader" BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
    <Columns>
        <asp:BoundField DataField="EmployerName" HeaderText="Employer Name" />
        <asp:BoundField DataField="IdentificationValue" HeaderText="RNLicense/ProviderNumber" />
        <asp:BoundField DataField="CEOFirstName" HeaderText="CEO First Name" />
        <asp:BoundField DataField="CEOLastName" HeaderText="CEO Last Name" />        
        <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" />
        <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
        <asp:BoundField DataField="EmpStartDate" HeaderText="Start Date" />
        <asp:BoundField DataField="EmpEndDate" HeaderText="End Date" />      
    </Columns>
    <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
                <PagerStyle BackColor="#BFE4FF" ForeColor="Black" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
</asp:GridView>
    <br />
     <label style="color: #000000; font-weight: bold" class="CountySelection">Supervisors Information</label>
     <asp:GridView runat="server" ID="grdSupervisorList" DataKeyNames="supFirstName" AllowSorting="True"
                AutoGenerateColumns="False" PageSize="20" CssClass="grdSupervisorList" AllowPaging ="true" 
                CellPadding="4" ForeColor="#333333" BorderStyle="Solid" BorderWidth="1px" BorderColor="#222222" >
                <EditRowStyle BackColor="#999999" />
    <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                <HeaderStyle CssClass="gridviewHeader" BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
    <Columns>             
        <asp:BoundField DataField="supFirstName" HeaderText="Supervisor First Name" />
        <asp:BoundField DataField="supLastName" HeaderText="Supervisor Last Name" />        
        <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" />
        <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
        <asp:BoundField DataField="supStartDate" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
        <asp:BoundField DataField="supEndDate" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />      
    </Columns>
    <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
                <PagerStyle BackColor="#BFE4FF" ForeColor="Black" HorizontalAlign="Center" />
                <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
                <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
                <SortedAscendingCellStyle BackColor="#E9E7E2" />
                <SortedAscendingHeaderStyle BackColor="#506C8C" />
                <SortedDescendingCellStyle BackColor="#FFFDF8" />
                <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
</asp:GridView>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
