<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="MAIS_Reports.aspx.vb" Inherits="MAIS.Web.MAIS_Reports" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="UserControls/Address.ascx" TagName="Address" TagPrefix="addr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
     <script type="text/javascript" src="Scripts/spin.min.js"></script> 
<script src="Scripts/Reports.js" type="text/javascript"></script>  
     <script type="text/javascript">
         $("[src*=plus]").live("click", function () {
             $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
             $(this).attr("src", "images/minus.png");
         });
         $("[src*=minus]").live("click", function () {
             $(this).attr("src", "images/plus.png");
             $(this).closest("tr").next().remove();
         });
        </script>
<script type="text/javascript">
    function PrintGridData(sctVar) {
        if (sctVar == '1') {
            var prtGrid = document.getElementById('<%=grdRNSearch.ClientID%>');
        }
        if (sctVar == '2') {
            var prtGrid = document.getElementById('<%=grdDDSearch.ClientID%>');
        }
        if (sctVar == '3') {
            var prtGrid = document.getElementById('<%=grdDDList.ClientID%>');
         }
        if (sctVar == '4') {
            var prtGrid = document.getElementById('<%=grdEmployerList.ClientID%>');
        }
        if (sctVar == '5') {
            var prtGrid = document.getElementById('<%=grdSupervisorList.ClientID%>');
        }
        if (sctVar == '6') {
            var prtGrid = document.getElementById('<%=grdRNList.ClientID%>');
         }
        prtGrid.border = 0;
        var prtwin = window.open('', 'PrintGridViewData', 'left=100,top=100,width=1000,height=1000,tollbar=0,scrollbars=1,status=0,resizable=1');
        prtwin.document.write(prtGrid.outerHTML);
        prtwin.document.close();
        prtwin.focus();
        prtwin.print();
        prtwin.close();
    }
</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
    <style type="text/css">
        .auto-style1
        {
            width: 134px;
        }
        #txtDateTo {
            width: 142px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
   
    <center>
        <table class="CountySelection" style="width: 100%;">
                        <tr>
                            <td style="text-align:center;width:85%;">                              
                                <asp:Label  ID="Label13" runat="server" Font-Bold="True" Text="MAIS Report Page"></asp:Label>     </td>                                            
                        </tr>
                    </table></center>
       <br />
    <asp:ValidationSummary runat="server" ID="RNInputValidation" ValidationGroup="rnMap"  CssClass="ErrorSummary errMsg" />
<asp:Panel ID="pnlSearch"  GroupingText="Please choose one" runat="server" Width="100%" Font-Size="9pt" Font-Italic="false">
                    <center>     <asp:RadioButtonList ID="rblSelect" CssClass="rblSelect" runat="server" 
                                    RepeatDirection="Horizontal" ValidationGroup = "searchpage" CausesValidation="True" 
                                        BorderStyle="Dotted" BorderWidth="2px" AutoPostBack="true">
                                    <asp:ListItem Value="1" Text="RN"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="DD Personnel"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Notation"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Employers/CEO's"></asp:ListItem>                                  
                                    <asp:ListItem Value="5" Text="Supervisor"></asp:ListItem>
                               </asp:RadioButtonList> </center>
</asp:Panel><br />
<asp:Panel ID="pnlRNDDSearchOptions" CssClass="pnlRNDDSearchOptions"   runat="server" Width="100%" Font-Size="9pt" Font-Italic="false">    
              
    <asp:Panel ID="pnlFileds" CssClass="pnlFileds" GroupingText="Report Search Options" runat="server" >
    <table>        
        <tr>
            <td colspan="1" style="text-align:right;">
                 <asp:label CssClass="lblRNDDLicenseDDCode" runat="server"  id="lblRNDDLicenseDDCode"></asp:label>
            </td>
            <td>
                 <input type = "text" id="txtRNDDLicDDCode" class="txtRNDDLicDDCode" runat = "server" align ="left"/>
            </td>
            <td colspan="1" style="text-align:right;">First Name:</td>
            <td>
                <input type = "text" class="txtFName" id="txtFName" runat = "server" align ="left"/>
            </td>
            <td colspan="1" style="text-align:right;">Last Name:</td>
            <td>
                <input type = "text" class="txtLName" id="txtLName" runat = "server" align ="left"/>
            </td>
        </tr>         
        <tr>
            <td colspan="1" style="text-align:right;" >Employer/AgencyName:
            </td>
            <td>
               <input type = "text" class="txtEmpName" id="txtEmpName" runat = "server" align ="left"/>
            </td>
            <td  colspan="1" style="text-align:right;">CEO First Name:
            </td>
            <td>
                <input type = "text" id="txtCEOFirst" class="txtCEOFirst" runat = "server" align ="left"/>
            </td>
            <td colspan="1"  style="text-align:right;">CEOLastName:
            </td>
            <td>
                <input type = "text" id="txtCEOLast" class="txtCEOLast" runat = "server" align ="left"/>
            </td>
        </tr>
        <tr>           
              <td  colspan="1" style="text-align:right;">SupervisorFirstName:
            </td>
            <td>
                <input type = "text" id="txtSupFirst" class="txtSupFirst" runat = "server" align ="left"/>
            </td>
            <td colspan="1"  style="text-align:right;">SupervisorLastName:
            </td>
            <td>
                <input type = "text" id="txtSupLast" class="txtSupLast"  runat = "server" align ="left"/>
            </td>
            <td colspan="1" style="text-align:right;">
                 <asp:label CssClass="lbl4SSN" runat="server"  id="lbl4SSN">Last4SSN:</asp:label>
            </td>
            <td>
                 <input type = "text" id="txt4SSN" class="txt4SSN" runat = "server" align ="left"/>
            </td>
        </tr>       
        </table>        
     </asp:Panel>  
    <asp:Panel runat ="server" ID="pnlNewFeilds" CssClass="pnlNewFeilds" GroupingText="Additional Report Search Options">
        <table>
        <tr>
            <td  style="text-align:right;">Certification Type:
            </td>
            <td style="text-align:left;">
                 <asp:DropDownList ID="ddlCertTypes" CssClass="ddlCertTypes" AppendDataBoundItems="True" runat="server" style="Width:155px;"></asp:DropDownList>
            </td>
            <td  style="text-align:right;">Certification Status:
            </td>
            <td style="text-align:left;">
               <asp:DropDownList ID="ddlCertStatus" CssClass="ddlCertStatus" AppendDataBoundItems="True" runat="server" style="Width:155px;"></asp:DropDownList>
            </td>  
             
        </tr>
        <tr>
             <td style="text-align:right;" >Expiration Date From:</td>  
              <td style="text-align:left;">
                <input type = "text" class="date-pick txtDateFrom" id="txtDateFrom" runat = "server" style="Width:150px;"/>                  
            </td>   
             <td style="text-align:right;">Expiration Date To:</td>  
              <td style="text-align:left;">
                <input type = "text" class="date-pick txtDateTo" id="txtDateTo" runat = "server" style="Width:150px;"/>                  
            </td>   
            
        </tr>    
            <tr>
                <td style="text-align:right;">Course:</td>
            <td style="text-align:left;"><asp:DropDownList ID="ddlCourses" CssClass="ddlCourses" AutoPostBack="true" AppendDataBoundItems="True" runat="server" style="Width:155px;"></asp:DropDownList></td>
            <td style="text-align:right;">Session:</td>
            <td style="text-align:left;"><asp:DropDownList ID="ddlSessions" CssClass="ddlSessions" AppendDataBoundItems="True" runat="server" style="Width:155px;"></asp:DropDownList></td>
        </tr>        
        <tr>
            <td style="text-align:right;">Certification Expiration Dates With in(days):</td>
            <td style="text-align:left;">
                 <asp:RadioButtonList ID="rblExpWithIn" CssClass="rblExpWithIn" runat="server" 
                                    RepeatDirection="Horizontal" ValidationGroup = "searchpage" CausesValidation="True" 
                                        BorderStyle="Dotted" BorderWidth="2px" AutoPostBack="true">
                                    <asp:ListItem Value="1" Text="30"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="60"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="90"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="180"></asp:ListItem>                                                                      
                               </asp:RadioButtonList>
            </td>
         <td style="text-align:right;">RN Trainer:</td>
            <td style="text-align:left;"><asp:DropDownList ID="ddlRNTrainer" CssClass="ddlRNTrainer" AutoPostBack="true" AppendDataBoundItems="True" runat="server" style="Width:155px;"></asp:DropDownList></td>
        </tr>
               
    </table>
     </asp:Panel>       
    <asp:CheckBox ID="chkAll"  AutoPostBack="true"  runat="server" />     
</asp:Panel>
<asp:Panel ID="pnlNotationOptions" CssClass="pnlNotationOptions"  GroupingText="Notation Search Options" runat="server" Width="100%" Font-Size="9pt" Font-Italic="false">
    <asp:Panel ID="pnlNotaionSelect"   runat="server" Width="100%" Font-Size="9pt" Font-Italic="false">
                    <center>     <asp:RadioButtonList ID="rblNotation" CssClass="rblSelect" runat="server" 
                                    RepeatDirection="Horizontal" ValidationGroup = "searchpage" CausesValidation="True" 
                                        BorderStyle="Dotted" BorderWidth="2px" AutoPostBack="true">
                                    <asp:ListItem Value="1" Text="RN"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="DD Personnel"></asp:ListItem>                                   
                               </asp:RadioButtonList> </center>
        </asp:Panel><br />
     <asp:Panel ID="pnlNotationSearch"   runat="server" Width="100%" Font-Size="9pt" Font-Italic="false">
           <table style="Width:100%;">       
        <tr>
            <td colspan="1"  style="text-align:right;">NotationType:</td>
            <td  style="text-align:left;">
                 <asp:DropDownList ID="ddlNotationType" CssClass="ddlNotationType" AppendDataBoundItems="True" runat="server" style="Width:155px;"></asp:DropDownList>
            </td>       
            <td   style="text-align:right;">Notation Reason:</td>
            <td style="text-align:left;">
                    <asp:DropDownList ID="ddlNotationReason" CssClass="ddlNotationReason" AppendDataBoundItems="True" runat="server" style="Width:155px;"></asp:DropDownList>
            </td>               
        </tr>    
        <tr>
             <td  style="text-align:right;">
                <label>Date Of Occurance From:</label>
            </td>
            <td style="text-align:left;">
             <input type = "text" class="date-pick txtStartDateOccurance" id="txtStartDateOccurance" runat = "server" style="Width:150px;" /></td>
             <td  style="text-align:right;">
                <label>Date Of Occurance To:</label>
            </td>
            <td style="text-align:left;"><input type = "text" class="date-pick txtEndDateOccurance" id="txtEndDateOccurance" runat = "server" style="Width:150px;"/>
            </td>
            </tr>      
    </table>
 </asp:Panel>   
     
</asp:Panel>
<asp:Panel ID="pnlEmployerSupervisor" CssClass="pnlEmployerSupervisor"  GroupingText="Search Options" runat="server" Width="100%" Font-Size="9pt" Font-Italic="false">
    <center>     <asp:RadioButtonList ID="rblEmpSup" CssClass="rblEmpSup" runat="server" 
                                    RepeatDirection="Horizontal" ValidationGroup = "searchpage" CausesValidation="True" 
                                        BorderStyle="Dotted" BorderWidth="2px" AutoPostBack="true">
                                    <asp:ListItem Value="1" Text="RN"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="DD Personnel"></asp:ListItem>
                                    <asp:ListItem Value ="3" Text="All"></asp:ListItem>                                   
                               </asp:RadioButtonList> </center>
    <asp:Panel ID="pnlEmpSup" runat="server" Width="100%"><center>
        <table style="text-align:center;">        
            <tr>
                <td style="text-align:right;"><asp:Label ID="lblRNDDIndividual" runat="server" ></asp:Label></td>
                <td style="text-align:left;" class="auto-style1"><asp:DropDownList ID="ddlRDDDList" CssClass="ddlRDDDList"  AppendDataBoundItems="True" runat="server" ></asp:DropDownList></td>
            </tr>
            <tr>
                <td style="text-align:right;"><asp:Label ID="lblEmpSup" runat="server" ></asp:Label></td>
                <td style="text-align:left;" class="auto-style1"><asp:TextBox ID="txtEmployerName" CssClass="txtEmployerName"   runat="server" ></asp:TextBox></td>
            </tr>
            <tr>
                <td style="text-align:right;"><asp:Label ID="lblSupLast" Text="Supervisor Last Name:" runat="server" ></asp:Label></td>
                <td style="text-align:left;" class="auto-style1"><asp:TextBox ID="txtSupLastName" CssClass="txtSupLastName"   runat="server" ></asp:TextBox></td>
            </tr>
        </table> </center>
    </asp:Panel>
</asp:Panel>

    
<br />    
<div id="divButtons" runat="server" class="divButtons">
    <input type="submit" id="btnRun" class="btnRun"  runat="server" value="Run Report" /> &nbsp;&nbsp;
    <input type="submit" id="btnExport" class="btnExport" runat="server" value="Export to Excel"  />&nbsp;&nbsp;
    <input type="submit" id="btnPrint" class="btnPrint" runat="server" value="Print"   />      
</div>
     <div id="divSpinner" runat="server" class="divSpinner" style="font-family:Tahoma, Arial, Verdana; font-size:24pt; font-weight:bold; text-align: center;">
            <br /><br />
        </div> 
      <asp:Label ID="lblDDCount" ForeColor="Blue" runat="server" ></asp:Label>
      <asp:Label ID="lblRNCount" ForeColor="Blue" runat="server" ></asp:Label>
    <asp:Label ID="lblCount" ForeColor="Blue" runat="server" ></asp:Label><br />
    <asp:Label ID="lblErrorMsg" ForeColor="Red" runat="server" ></asp:Label>
  <asp:GridView ID="grdRNList" DataKeyNames = "RNLicence_DDPersonnel" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True"
        ageSize="20" CssClass ="grdRNList" AllowPaging="true" 
                CellPadding="4" ForeColor="#333333" BorderStyle="Solid" BorderWidth="1px" BorderColor="#222222" >
                <EditRowStyle BackColor="#999999" />
                 <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                <HeaderStyle CssClass="gridviewHeader" BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
    <Columns>   
       
        <asp:TemplateField HeaderText="Certification" >
            <ItemTemplate>
                <img alt = "" style="cursor: pointer" title="Certification" src="images/plus.png" />
                <asp:Panel ID="pnlCertification" GroupingText="Certification" runat="server" Width="100%" Style="display: none">
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
                </asp:Panel>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Employer" >
            <ItemTemplate>
                <img alt = "" style="cursor: pointer" title="Employer" src="images/plus.png" />
                <asp:Panel ID="pnlEmployer" GroupingText="Employer" runat="server" Style="display: none">
                    <asp:GridView ID="gvEmployer" runat="server" AutoGenerateColumns="false" Width="100%" CssClass = "ChildGrid">
                        <EditRowStyle BackColor="#999999" />
                         <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                        <HeaderStyle CssClass="gridviewHeader" BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                        <Columns>
                             <asp:BoundField DataField="Employer_Name" HeaderText="Employer Name" /> 
                             <asp:BoundField DataField="CEO_First_Name" HeaderText="CEO First Name " /> 
                             <asp:BoundField DataField="CEO_Last_Name" HeaderText="CEO Last Name " /> 
                             <asp:BoundField DataField="Supervisor_First_Name" HeaderText="Supervisor First Name " /> 
                             <asp:BoundField DataField="Supervisor_Last_Name" HeaderText="Supervisor Last Name" /> 
                             <asp:BoundField DataField="WorkAddress" HeaderText="Work Address" /> 
                             <asp:BoundField DataField="WorkCounty" HeaderText="County" /> 
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
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="Course" >
            <ItemTemplate>
                <img alt = "" style="cursor: pointer" title="Course" src="images/plus.png" />
                <asp:Panel ID="pnlCourse" GroupingText="Course" runat="server" Style="display: none">
                    <asp:GridView ID="gvCourse" runat="server" AutoGenerateColumns="False" Width="100%" CssClass = "ChildGrid">
                         <EditRowStyle BackColor="#999999" />
                         <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                         <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
                        <HeaderStyle CssClass="gridviewHeader" BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                         <Columns>
                             <asp:BoundField DataField="Course_Number" HeaderText="Course ID Number " /> 
                             <asp:BoundField DataField="Trainer_Name" HeaderText="Trainer Name " /> 
                             <asp:BoundField DataField="Session_Start_Date" HeaderText="Session Start Date " DataFormatString="{0:MM/dd/yyyy}" /> 
                             <asp:BoundField DataField="Session_End_Date" HeaderText="Session End Date " DataFormatString="{0:MM/dd/yyyy}" /> 
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
                </asp:Panel>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:BoundField DataField="RN_DD_Sid " HeaderText="RN_DD_Sid" Visible="false"  />  
         <asp:TemplateField HeaderText="RNLicense">
            <ItemTemplate>
                <asp:LinkButton ID="lnkRNDDPerson" runat="server" Text='<%# Eval("RNLicence_DDPersonnel")%>'   
                  CommandName="Select"> 
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>          
         <asp:BoundField DataField="FirstName" HeaderText="FirstName" /> 
         <asp:BoundField DataField="LastName" HeaderText="LastName" />     
         <asp:BoundField DataField="MiddleName" HeaderText="MiddleName" />           
         <asp:BoundField DataField="DOB_LicenseIssue" HeaderText="LicenseIssueDate" DataFormatString="{0:MM/dd/yyyy}" /> 
         <asp:BoundField DataField="HomeAddress" HeaderText="HomeAddress" /> 
         <asp:BoundField DataField="HCounty" HeaderText="HCounty" />         
         <asp:BoundField DataField="Addr_Sid" HeaderText="Addr_Sid" Visible="false"  />          
    </Columns>
    <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
    <PagerStyle BackColor="#BFE4FF" ForeColor="black" HorizontalAlign="Center" />
    <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#E9E7E2" />
    <SortedAscendingHeaderStyle BackColor="#506C8C" />
    <SortedDescendingCellStyle BackColor="#FFFDF8" />
    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />

    </asp:GridView>
    <asp:GridView ID="grdDDList" DataKeyNames = "RNLicence_DDPersonnel" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True"
        ageSize="20" CssClass ="grdDDList" AllowPaging="true" 
                CellPadding="4" ForeColor="#333333" BorderStyle="Solid" BorderWidth="1px" BorderColor="#222222" >
                <EditRowStyle BackColor="#999999" />
                 <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
            <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
                <HeaderStyle CssClass="gridviewHeader" BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
    <Columns>          
        <asp:TemplateField HeaderText="Certification" >
            <ItemTemplate>
                <img alt = "" style="cursor: pointer" title="Certification" src="images/plus.png" />
                <asp:Panel ID="pnlDDCertification" GroupingText="Certification" runat="server" Width="100%" Style="display: none">
                    <asp:GridView ID="gvDDCertification" runat="server" AutoGenerateColumns="false" CssClass = "ChildGrid">
                        <EditRowStyle BackColor="#999999" />
                         <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                        <HeaderStyle CssClass="gridviewHeader" BackColor="#BFE4FF" Font-Bold="True" ForeColor="black" />
                        <Columns>
                             <asp:BoundField DataField="Certification_Type" HeaderText="Certification Type" /> 
                            <asp:BoundField DataField="Category_Code" HeaderText="Category" /> 
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
                </asp:Panel>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Employer" >
            <ItemTemplate>
                <img alt = "" style="cursor: pointer" title="Employer" src="images/plus.png" />
                <asp:Panel ID="pnlDDEmployer" GroupingText="Employer" runat="server" Style="display: none">
                    <asp:GridView ID="gvDDEmployer" runat="server" AutoGenerateColumns="false" Width="100%" CssClass = "ChildGrid">
                        <EditRowStyle BackColor="#999999" />
                         <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                        <HeaderStyle CssClass="gridviewHeader" BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                        <Columns>
                             <asp:BoundField DataField="Employer_Name" HeaderText="Employer Name" /> 
                             <asp:BoundField DataField="CEO_First_Name" HeaderText="CEO First Name " /> 
                             <asp:BoundField DataField="CEO_Last_Name" HeaderText="CEO Last Name " /> 
                             <asp:BoundField DataField="Supervisor_First_Name" HeaderText="Supervisor First Name " /> 
                             <asp:BoundField DataField="Supervisor_Last_Name" HeaderText="Supervisor Last Name" /> 
                             <asp:BoundField DataField="WorkAddress" HeaderText="Work Address" /> 
                             <asp:BoundField DataField="WorkCounty" HeaderText="County" /> 
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
            </ItemTemplate>
        </asp:TemplateField>
         <asp:TemplateField HeaderText="Course" >
            <ItemTemplate>
                <img alt = "" style="cursor: pointer" title="Course" src="images/plus.png" />
                <asp:Panel ID="pnlDDCourse" GroupingText="Course" runat="server" Style="display: none">
                    <asp:GridView ID="gvDDCourse" runat="server" AutoGenerateColumns="False" Width="100%" CssClass = "ChildGrid">
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
                </asp:Panel>
            </ItemTemplate>
        </asp:TemplateField>
         <asp:BoundField DataField="RN_DD_Sid " HeaderText="RN_DD_Sid" Visible="false"  /> 
         <asp:TemplateField HeaderText="DDPersonnelCode">
            <ItemTemplate>
                <asp:LinkButton ID="lnkRNDDPerson" runat="server" Text='<%# Eval("RNLicence_DDPersonnel")%>'   
                  CommandName="Select"> 
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>              
         <asp:BoundField DataField="FirstName" HeaderText="FirstName" /> 
         <asp:BoundField DataField="LastName" HeaderText="LastName" />   
         <asp:BoundField DataField="MiddleName" HeaderText="MiddleName" />         
         <asp:BoundField DataField="DOB_LicenseIssue" HeaderText="DOB" DataFormatString="{0:MM/dd/yyyy}" /> 
         <asp:BoundField DataField="Last4SSN" HeaderText="Last4SSN" DataFormatString="{0:0000}" /> 
         <asp:BoundField DataField="HomeAddress" HeaderText="HomeAddress" /> 
         <asp:BoundField DataField="HCounty" HeaderText="HCounty" />         
         <asp:BoundField DataField="Addr_Sid" HeaderText="Addr_Sid" Visible="false"  />          
    </Columns>
    <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
    <PagerStyle BackColor="#BFE4FF" ForeColor="black" HorizontalAlign="Center" />
    <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#E9E7E2" />
    <SortedAscendingHeaderStyle BackColor="#506C8C" />
    <SortedDescendingCellStyle BackColor="#FFFDF8" />
    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />

    </asp:GridView>
<asp:GridView runat = "server" ID = "grdDDSearch" DataKeyNames = "DDPersonnelCode"  AllowSorting="True"
                AutoGenerateColumns="False" PageSize="20" CssClass ="grdDDSearch" AllowPaging="true" 
                CellPadding="4" ForeColor="#333333" BorderStyle="Solid" BorderWidth="1px" BorderColor="#222222" >
                <EditRowStyle BackColor="#999999" />
    <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
                <HeaderStyle CssClass="gridviewHeader" BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
    <Columns>      
        <asp:TemplateField HeaderText="DDPersonnal Code" >
            <ItemTemplate>
                <asp:LinkButton ID="lnkDDPerson" runat="server" Text='<%# Eval("DDPersonnelCode")%>'   
                CommandName="Select" > 
                </asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>   
        <asp:BoundField DataField="DDPersonnelCode" HeaderText="DDPersonnal Code" />  
        <asp:BoundField DataField="Last4SSN" HeaderText="Last 4 SSN" DataFormatString="{0:0000}"/>      
        <asp:BoundField DataField="LastName" HeaderText="Last Name" />
        <asp:BoundField DataField="FirstName" HeaderText="First Name" />
        <asp:BoundField DataField="MiddleName" HeaderText="Middle Name" />
        <asp:BoundField DataField="HomeAddress" HeaderText="Home Address" />
        <asp:BoundField DataField="County" HeaderText="County" Visible ="True" />
        <asp:BoundField DataField="DateOfBirth" HeaderText="Date Of Birth" DataFormatString="{0:MM/dd/yyyy}"  />
        <asp:BoundField DataField="CAT1" HeaderText="CAT 1" />
        <asp:BoundField DataField="CAT2" HeaderText="CAT 2" />
        <asp:BoundField DataField="CAT3" HeaderText="CAT 3" />                           
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
                AutoGenerateColumns="False" PageSize="20" CssClass="grdRNSearch" AllowPaging="true" 
                CellPadding="4" ForeColor="#333333" BorderStyle="Solid" BorderWidth="1px" BorderColor="#222222" >
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
                <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
                <HeaderStyle CssClass="gridviewHeader" BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
            <Columns>
                 <asp:TemplateField HeaderText="RN License">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkDDPerson" runat="server" Text='<%# Eval("RNLicenseNumber")%>'   
                          CommandName="Select"> 
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>          
                 <asp:BoundField DataField="DateRNIssuance" HeaderText="License Issue Date" DataFormatString="{0:MM/dd/yyyy}" />
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
 <asp:GridView runat="server" ID="grdEmployerList" DataKeyNames="EmployerName" AllowSorting="True"
                AutoGenerateColumns="False" PageSize="20" CssClass="grdEmployerList" AllowPaging="True" 
                CellPadding="4" ForeColor="#333333" BorderStyle="Solid" BorderWidth="1px" BorderColor="#222222" >
                <EditRowStyle BackColor="#999999" />
                <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
                <HeaderStyle CssClass="gridviewHeader" BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                <Columns>
                    <asp:BoundField DataField="EmployerName" HeaderText="Employer Name" />
                    <asp:BoundField DataField="IdentificationValue" HeaderText="RNLicense/ProviderNumber" />
                    <asp:BoundField DataField="CEOFirstName" HeaderText="CEO First Name" />
                    <asp:BoundField DataField="CEOLastName" HeaderText="CEO Last Name" />        
                    <asp:BoundField DataField="EmailAddress" HeaderText="Email Address" />
                    <asp:BoundField DataField="PhoneNumber" HeaderText="Phone Number" />
                    <asp:BoundField DataField="EmpStartDate" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="EmpEndDate" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}" />      
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
    <asp:GridView runat="server" ID="grdSupervisorList" DataKeyNames="supFirstName" AllowSorting="True"
                AutoGenerateColumns="False" PageSize="20" CssClass="grdSupervisorList" AllowPaging ="true" 
                CellPadding="4" ForeColor="#333333" BorderStyle="Solid" BorderWidth="1px" BorderColor="#222222" >
                <EditRowStyle BackColor="#999999" />
                 <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
                 <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
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
