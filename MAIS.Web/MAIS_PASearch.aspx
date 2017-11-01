<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MAIS_PASearch.aspx.vb" Inherits="MAIS.Web.MAIS_PASearch" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Medication Administration Certification Verification</title>
    <link rel="Stylesheet" type="text/css" href="App_Themes/CW_Style/Main.css" />
    <link rel="Stylesheet" type="text/css" href="Scripts/jquery/jquery.calendars.picker.css" />    
    <script type="text/javascript" src="Scripts/jquery/jquery.1.6.2.min.js"></script> 
    <script type="text/javascript" src="Scripts/jquery/jquery.calendars.pack.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery.calendars.plus.pack.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery.calendars.picker.pack.js"></script>    
      <script type="text/javascript" src="Scripts/jquery/jquery.maskedinput-1.3.min.js"></script>
    <script type="text/javascript" src="Scripts/spin.min.js"></script> 
    <script type="text/javascript" src="Scripts/MAISCommon.js"></script>  
    <script type="text/javascript" src="Scripts/MAIS_PASearch.js"></script>
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
</head>
<body class="FormFormat">
    <form id="form1" runat="server">
        <center>
    <div>
        
        <table width="850" class="FormFormat" cellpadding="0" cellspacing="0">
            <tr id="trLogo" class="HeaderRow" runat="server">
                <td class="TableHeaderStyle">
                    <div style="text-align: left; font-size: 10pt; font-family: Arial, Verdana">
                        <br />
                        <img src="Images/CertWizardHeader.jpg" alt="Ohio DODD - MAIS" />
                    </div>
                </td>
            </tr>
                </table>
        <br />
   
<p style="font-weight: bold; text-align: center">Hello! <br />Welcome to DODD<br />Medical Administration Certification Verification</p>
<p style="font-weight: bold; text-align: center">SELECT ONE TO BEGIN YOUR SEARCH</p>
        <br />
        <div id="pError" runat="server" class="ErrorSummary" visible="false" style="width:850px;"></div>

        <br />
<asp:Panel ID="pnlSearch" runat="server" Width="100%" Font-Size="9pt" Font-Italic="false">
                        <asp:RadioButtonList ID="rblSelect" CssClass="rblSelect" runat="server" 
                                    RepeatDirection="Horizontal" ValidationGroup = "searchpage" CausesValidation="false" BorderStyle="Dotted" BorderWidth="2px" AutoPostBack="true">
                                    <asp:ListItem Value="1" Text="Search for RN"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Search for DD Personnel"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Search RN Training Sessions"></asp:ListItem>
                                    <asp:ListItem Value="4" Text="Search DD Personnel Training Sessions"></asp:ListItem>
                               </asp:RadioButtonList>
</asp:Panel><br />
<input type="hidden" id="hRnSession" runat="server" value="1" />
<asp:Panel ID="pnlRNDDSearchOptions" CssClass="pnlRNDDSearchOptions"   runat="server" Width="100%" Font-Size="9pt" Font-Italic="false">    
              
    <asp:Panel ID="pnlRnSearch" CssClass="pnlFileds" runat="server" >
    <table>        
        <tr>
            <td colspan="1" style="text-align:right;">
                 <asp:label CssClass="lblRNLicenseNo" runat="server"  id="lblRNLicenseNo">RN License #:</asp:label>
            </td>
            <td>
                 <input type = "text" id="txtRNLicenseNo" class="txtRNLicenseNo" runat = "server" align ="left"/>
            </td>
        </tr>         
        <tr>
            <td colspan="1" style="text-align:right;">
                 <asp:label CssClass="lblLname" runat="server"  id="lblLname">Last Name:</asp:label>
            </td>
            <td>
                 <input type = "text" id="txtLname" class="txtLname" runat = "server" align ="left"/>
            </td>
        </tr>
        <tr>           
              <td colspan="1" style="text-align:right;">
                 <asp:label CssClass="lblFname" runat="server"  id="lblFname">First Name:</asp:label>
            </td>
            <td>
                 <input type = "text" id="txtFname" class="txtFname" runat = "server" align ="left"/>
            </td>
        </tr>       
        </table>        
     </asp:Panel>  

 <asp:Panel ID="pnlDDSearch" CssClass="pnlFileds" runat="server" >
    <table>        
        <tr>
            <td colspan="1" style="text-align:right;">
                 <asp:label CssClass="lblDODDIdNo" runat="server"  id="lblDODDIdNo">DODD ID #:</asp:label>
            </td>
            <td>
                 <input type = "text" id="txtDODDIdNo" class="txtDODDIdNo" runat = "server" align ="left"/>
            </td>
        </tr> 
        <tr>
            <td colspan="1" style="text-align:right;">
                 <asp:label CssClass="lblLast4ssn" runat="server"  id="lblLast4ssn">Last 4 SSN:</asp:label>
            </td>
            <td>
                <asp:TextBox ID="txLast4ssn" CssClass="txLast4ssn" runat="server" TextMode="Password" MaxLength="4"></asp:TextBox>
                 
            </td>
        </tr>        
        <tr>
            <td colspan="1" style="text-align:right;">
                 <asp:label CssClass="lblDDLname" runat="server"  id="lblDDLname">Last Name:</asp:label>
            </td>
            <td>
                 <input type = "text" id="txtDDLname" class="txtDDLname" runat = "server" align ="left"/>
            </td>
        </tr>
        <tr>           
              <td colspan="1" style="text-align:right;">
                 <asp:label CssClass="lblDDFname" runat="server"  id="lblDDFname">First Name:</asp:label>
            </td>
            <td>
                 <input type = "text" id="txtDDFname" class="txtDDFname" runat = "server" align ="left"/>
            </td>
        </tr>    
         <tr>           
              <td colspan="1" style="text-align:right;">
                 <asp:label CssClass="lblEmployer" runat="server"  id="lblEmployer">Employer Name:</asp:label>
            </td>
            <td>
                 <input type = "text" id="txtEmployer" class="txtEmployer" runat = "server" align ="left"/>
            </td>
        </tr>     
        </table>        
     </asp:Panel>  

     <asp:Panel ID="pnlSessions" CssClass="pnlFileds" runat="server" >
    <table>        
         <tr>
            <td colspan="1" style="text-align:right;">
                 <asp:label CssClass="lblRNLname" runat="server"  id="lblRNLname">RN Last Name:</asp:label>
            </td>
            <td>
                 <input type = "text" id="txtRNLname" class="txtRNLname" runat = "server" align ="left"/>
            </td>
        </tr>
        <tr>           
              <td colspan="1" style="text-align:right;">
                 <asp:label CssClass="lblRNFname" runat="server"  id="lblRNFname">RN First Name:</asp:label>
            </td>
            <td>
                 <input type = "text" id="txtRNFname" class="txtRNFname" runat = "server" align ="left"/>
            </td>
        </tr>    
        <tr>
            <td colspan="1" style="text-align:right;">
                 <asp:label CssClass="lblCounty" runat="server"  id="lblCounty">County </asp:label>
            </td>
            <td>
                 &nbsp;<asp:DropDownList ID="ddlCounty"  CssClass="ddlCounty" runat="server" AppendDataBoundItems="True" BackColor="WhiteSmoke"></asp:DropDownList>
                 <%--<input type = "text" id="txtCounty" class="txtCounty" runat = "server" align ="left"/>--%>
            </td>
        </tr>        
       
       <tr>
            <td colspan="1" style="text-align:right;">
                 <asp:label CssClass="lblDates" runat="server"  id="lblDates">Sessions between dates</asp:label>
            </td>
            <td>
                 <input type = "text" id="txtStDate" class="date-pick txtStDate" runat = "server" align ="left"/>
            </td>
            <td>
                 and &nbsp;&nbsp;<input type = "text" id="txtEndDate" class="date-pick txtEndDate" runat = "server" align ="left"/>
            </td>
        </tr> 
        </table>        
         <br />
     </asp:Panel> 
    
    </asp:Panel>
        <div id="divButtons" runat="server" class="divButtons">
    <input type="submit" id="btnRun" class="btnRun"  runat="server" value="Start Search" />   
</div>
        <div id="divSpinner" runat="server" class="divSpinner" style="font-family:Tahoma, Arial, Verdana; font-size:24pt; font-weight:bold; text-align: center;">
            <br /><br />
        </div> 
    </div>
            <br />
        <div style="width: 850px">
             <asp:GridView ID="grdRNList" DataKeyNames = "RNLicenseNumber" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True"
        ageSize="20" CssClass ="grdRNList" AllowPaging="false" 
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
                             <asp:BoundField DataField="RoleDescription" HeaderText="Certification Type" /> 
                             
                             <asp:BoundField DataField="EffectiveDate" HeaderText="Effective" DataFormatString="{0:MM/dd/yyyy}" /> 
                             <asp:BoundField DataField="ExpirationDate" HeaderText="Expiration" DataFormatString="{0:MM/dd/yyyy}" /> 
                             <asp:BoundField DataField="ConsectiveRenewals" HeaderText="RenewalCount" />
                            <asp:BoundField DataField="CurrentStatus" HeaderText="Certification Status" /> 
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
        
         <asp:BoundField DataField="RNLicenseNumber" HeaderText="RN License Number" />  
        <asp:BoundField DataField="LastName" HeaderText="Last Name" />
        <asp:BoundField DataField="FirstName" HeaderText="First Name" />      
    </Columns>
    <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
    <%--<PagerStyle BackColor="#BFE4FF" ForeColor="black" HorizontalAlign="Center" />--%>
    <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#E9E7E2" />
    <SortedAscendingHeaderStyle BackColor="#506C8C" />
    <SortedDescendingCellStyle BackColor="#FFFDF8" />
    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
     <EmptyDataTemplate>
        <asp:Label ID="lblEmpt1" runat="server" Text="No Results."></asp:Label>
   </EmptyDataTemplate>
    </asp:GridView>

        <asp:GridView ID="grdDDList" DataKeyNames = "DDPersonnelCode" runat="server" Width="100%" AutoGenerateColumns="False" AllowSorting="True"
        ageSize="20" CssClass ="grdDDList" AllowPaging="false" 
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
                             <asp:BoundField DataField="RoleDescription" HeaderText="Certification Type" /> 
                             
                             <asp:BoundField DataField="EffectiveDate" HeaderText="Effective" DataFormatString="{0:MM/dd/yyyy}" /> 
                             <asp:BoundField DataField="ExpirationDate" HeaderText="Expiration" DataFormatString="{0:MM/dd/yyyy}" /> 
                             <asp:BoundField DataField="ConsectiveRenewals" HeaderText="RenewalCount" />
                            <asp:BoundField DataField="CurrentStatus" HeaderText="Certification Status" /> 
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

        <asp:BoundField DataField="LastName" HeaderText="Last Name" />
        <asp:BoundField DataField="FirstName" HeaderText="First Name" />
        <asp:BoundField DataField="DDPersonnelCode" HeaderText="DD ID #" />  
        <asp:BoundField DataField="DOB" HeaderText="Date Of Birth" />    
    </Columns>
    <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
    <%--<PagerStyle BackColor="#BFE4FF" ForeColor="black" HorizontalAlign="Center" />--%>
    <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
    <SelectedRowStyle BackColor="#E2DED6" Font-Bold="True" ForeColor="#333333" />
    <SortedAscendingCellStyle BackColor="#E9E7E2" />
    <SortedAscendingHeaderStyle BackColor="#506C8C" />
    <SortedDescendingCellStyle BackColor="#FFFDF8" />
    <SortedDescendingHeaderStyle BackColor="#6F8DAE" />
     <EmptyDataTemplate>
        <asp:Label ID="lblEmpt2" runat="server" Text="No Results."></asp:Label>
   </EmptyDataTemplate>
    </asp:GridView>


<asp:GridView runat = "server" ID = "grdSessionSearch" Visible="false"  Width="100%" AutoGenerateColumns="False" AllowSorting="True"
        ageSize="20" CssClass ="grdDDList" AllowPaging="false" 
                CellPadding="4" ForeColor="#333333" BorderStyle="Solid" BorderWidth="1px" BorderColor="#222222" >
                <EditRowStyle BackColor="#999999" />
                 <FooterStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
            <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
                <HeaderStyle CssClass="gridviewHeader" BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" />
    <Columns>      
        <asp:BoundField DataField="StartDate" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}" />  
        <asp:BoundField DataField="EndDate" HeaderText="End Date" DataFormatString="{0:MM/dd/yyyy}"/>      
        <asp:BoundField DataField="RNTrainerName" HeaderText="RN Trainer Name" />
        <asp:BoundField DataField="CourseCategory" HeaderText="Course Category" />
        <asp:BoundField DataField="County" HeaderText="County" />
        <asp:BoundField DataField="RNTrainerEmail" HeaderText="RN Trainer Email" DataFormatString="<a href=mailto:{0}>{0}</a>" HtmlEncodeFormatString="false" />
        <asp:BoundField DataField="OBNNumber" HeaderText="Course ID Number" />
                   
    </Columns>
     <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" BackColor="White" ForeColor="#333333" />
    <%--<PagerStyle BackColor="#BFE4FF" ForeColor="black" HorizontalAlign="Center" />--%>
    <RowStyle VerticalAlign="Middle" HorizontalAlign="Center"  BackColor="#F7F6F3" ForeColor="#333333" />
   <EmptyDataTemplate>
        <asp:Label ID="lblEmpt3" runat="server" Text="No Results."></asp:Label>
   </EmptyDataTemplate>
</asp:GridView>


            <br />
            <br />
        </div>
            </center>
    </form>
</body>
</html>
