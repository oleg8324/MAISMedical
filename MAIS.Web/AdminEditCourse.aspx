<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="AdminEditCourse.aspx.vb" Inherits="MAIS.Web.AdminEditCourse" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/AdminEditCourse.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <div>     
    <center>
<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ErrorSummary" ValidationGroup="a" />

<asp:Panel ID="pSearch"  GroupingText="Please choose one" runat="server" Width="80%" Font-Size="9pt" Font-Italic="false">
<asp:RadioButtonList ID="RadioButtonList1" class="rblSelect" runat="server" 
        RepeatDirection="Horizontal" BorderStyle="Dotted" BorderWidth="2px">
   <asp:ListItem Value="0" Text="Search for a course"></asp:ListItem>
   <asp:ListItem Value="1" Text="Search for a session"></asp:ListItem>
</asp:RadioButtonList>
</asp:Panel>

<input type="button" value="MAIS Report" onclick ="location.href='StartPage.aspx'" />
<br />
        
<asp:Panel ID="PPersonal"  GroupingText="Search Options" runat="server" Width="80%" Font-Size="9pt" Font-Italic="false">

<asp:CustomValidator ID="CustomValidator1" runat="server" 
        OnServerValidate="ServerValidate" ClientValidationFunction="OneReq" 
        ErrorMessage="Either name or RN Liscence number should be entered" Display="None" 
        ValidationGroup="a"></asp:CustomValidator>

   <table>

<tr><td>
<label>RN License No.</label></td><td>
<input type="text" id="txtRNNO" runat="server" />
<asp:RegularExpressionValidator ID="RegRNDDLicSSN" runat="server" ControlToValidate="txtRNNO" Display="None" 
                                    ErrorMessage="Please enter a valid Liscence Number" ValidationExpression="^[0-9]*$" ValidationGroup="a">
                                </asp:RegularExpressionValidator>
</td></tr>

<tr><td>
<label>First Name :</label></td><td>
<input type="text" id="txtFirstName" runat="server" />
<asp:RegularExpressionValidator ID="RegFirstName" runat="server" ControlToValidate="txtFirstName" Display="None" 
                                    ErrorMessage="Please enter only alphabets" ValidationExpression="^[a-zA-Z]+$" ValidationGroup="a">
                                </asp:RegularExpressionValidator>
</td></tr>

<tr><td>
<label>Last Name :</label></td><td>
<input type="text" id="txtLName" runat="server" />
<asp:RegularExpressionValidator ID="RegLastName" runat="server" ControlToValidate="txtLName" Display="None" 
                                    ErrorMessage="Please enter only alphabets" ValidationExpression="^[a-zA-Z]+$" ValidationGroup="a">
                                </asp:RegularExpressionValidator>
</td></tr>
<tr><td>
<label>Start Date :</label></td><td>
<input type="text" id="txtStartDate" runat="server"  />
<asp:CompareValidator Type="Date" Operator="DataTypeCheck" runat="server" ID="CompDate" controltovalidate="txtStartDate" validationgroup="a"/>


<br /></td></tr></table>

    

</asp:Panel>
<br />
<input id="btnSearch" runat="server" type="submit" causesvalidation="true" ValidationGroup="a" value="Search" /> &nbsp;&nbsp;
    <input type="button" id="btnExport" runat="server" value="Export to Excel" onclick ="location.href='StartPage.aspx'" />&nbsp;&nbsp;
    <input type="button" id="btnPrint" runat="server" value="Print" onclick ="location.href='StartPage.aspx'" /><br /><br />

    <span style="font-size: small">Please note if you edit a course or session all associated sessions will be affected and certificates will need to be reissued.</span><br />
    </center>

    <table width="100%" style="border-style: solid; font-size: small">
    <tr>
    <td rowspan="2" class="BlueColumnHead">Course<br />Information</td>
    <td class="BlueColumnHead">RN Instructor Name</td> <td class="BlueColumnHead">Effective Start Date</td> <td class="BlueColumnHead">Effective End Date</td> <td class="BlueColumnHead">OBN Approval Number</td> <td class="BlueColumnHead">Category A CEs</td> <td class="BlueColumnHead">Total CEs</td> <td class="BlueColumnHead">Level</td>
    <td class="BlueColumnHead">Category</td> <td class="BlueColumnHead">Course Description</td>
    </tr>
    <tr>
    <td>Dfsdf, dsf</td><td>12/12/13</td><td> 12/31/13</td><td>233123</td><td>33</td><td>4</td><td>3</td><td>4</td><td>Cool course</td>
    </tr>
    <tr>
    <td rowspan="2">Sponsor</td><td rowspan="2">Session<br />Information</td><td>Start Date</td><td>End Date</td><td>Street Address</td><td>Location Name</td><td>City</td><td>State</td><td>Zip</td>
    </tr>
    <tr>
    <td>10/10/2012</td> <td>12/12/2012</td><td>12 high st</td><td>Cool location</td><td>Columbus</td><td>OH</td><td>43221</td>
    </tr>
    
    </table>
</div><br />
<center>
    <input type="button" value="Save and Update and Email Certificates" onclick ="location.href='EmployerInformation.aspx'" />
</center>
<br />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">

</asp:Content>
