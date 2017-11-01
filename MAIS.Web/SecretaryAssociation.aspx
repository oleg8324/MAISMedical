<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="SecretaryAssociation.aspx.vb" Inherits="MAIS.Web.SecretaryAssociation" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
       <script type="text/javascript"  src="Scripts/SecretaryAssociation.js"></script>
   <script type="text/javascript" src="Scripts/spin.min.js"></script> 
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    
         <table class="CountySelection" style="width: 100%;">
            <tr>
                <td>
                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Text="Secretary Association"></asp:Label>
                </td>
            </tr>        
        </table>

<%--<label style="font-weight: bold" class="CountySelection">Please Choose One of the selections below:</label> <br />--%>
<br />
<asp:ValidationSummary runat="server" ID="SecretaryInputValidation" ValidationGroup="SecMap"  CssClass="ErrorSummary errMsg" />
        <asp:Label ID="lblErr" ForeColor="red" runat="server" ></asp:Label>
<asp:Panel ID="pnlSearch"  GroupingText="Secretary Search Options" runat="server" Width="100%" Font-Size="9pt" Font-Italic="false">
    <center>
<table>
<tr><td style="text-align:right">
        <label>Email:</label></td>
    <td style="text-align:left">
        <input type="text" class="txtEmail" id="txtEmail" runat="server" />        
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
    <tr><td style="text-align:right"><label>Choose RN:</label></td>
    <td style="text-align:left">
         <asp:DropDownList ID="ddlRNSearch" CssClass="ddlRNSearch"  runat="server">                     
                    </asp:DropDownList>
    </td></tr>
<tr><td style="text-align:right">
        <label>Access Status :</label></td>
    <td style="text-align:left">
        <asp:DropDownList ID="ddlStatus" CssClass="ddlStatus"  runat="server">
                <asp:ListItem Value="0">Select Status</asp:ListItem>
                <asp:ListItem Value="1">All</asp:ListItem>
                <asp:ListItem Value="2">Associated</asp:ListItem>
                <asp:ListItem Value="3">Dis_Associated</asp:ListItem>
            </asp:DropDownList><br /></td>
</tr>

<tr><td></td></tr>
<tr>
    <td></td>
    <td style="text-align:left">
        <asp:Button ID="btnSearch" CssClass="btnSearch" Text="Search" runat="server" />         
    </td>
</tr>
    <tr><td></td></tr>
<tr>
    <td></td>
    <td><asp:LinkButton ID="lnkAddSecretary" runat="server" Text="Associate New Secretary" ></asp:LinkButton></td>
</tr>    
</table>
        </center><br />
</asp:Panel>
<asp:Panel ID = "pnlResults" Width="100%" Font-Size="9pt" Font-Italic="false"  runat = "server">
    <div id="divSpinner" runat="server" class="divSpinner" style="font-family:Tahoma, Arial, Verdana; font-size:24pt; font-weight:bold; text-align: center;">
            <br /><br />
        </div>
<label id = "lblSecErr" runat="server" style="color:red;">No secretary found</label>    
     <asp:GridView runat="server" ID="grdResults" BackColor="White" BorderColor="#3366CC" BorderStyle="Inset" BorderWidth="1px" CellPadding="4" 
                        DataKeyNames="User_Mapping_Sid" HorizontalAlign="Center" AutoGenerateColumns="false" Width="80%">
                        <Columns>                                     
                            <asp:TemplateField HeaderText="" InsertVisible="False" 
                                    ShowHeader="False" FooterStyle-HorizontalAlign="Left">
                                    <ItemTemplate>
                                       <asp:LinkButton ID="lnkSecAssociation" runat="server" Text="Associate/Dis_Associate"   
                                        CommandName="Associate" CommandArgument='<%# Eval("User_Mapping_Sid")%>'> 
                                       </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>                                                                                                                                            
                            <asp:BoundField DataField="First_Name" HeaderText="FirstName" />
                            <asp:BoundField DataField="Last_Name" HeaderText="LastName" />  
                            <asp:BoundField DataField="Middle_Name" HeaderText="MiddleName" />
                            <asp:BoundField DataField="SecretaryUserName" HeaderText="SecretaryUserName" /> 
                            <asp:BoundField DataField="Email" HeaderText="Email" />    
                            <asp:BoundField DataField="EditedBy" HeaderText="Last Updated By" />                                                                                                                                                                                                                                                                                 
                        </Columns>
                        <RowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" VerticalAlign="Top" />
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                        <SelectedRowStyle BackColor="Orange" />
                        <HeaderStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" BorderColor="Black" />
                    </asp:GridView>
</asp:Panel>      

<br /><br />
 <asp:Panel ID = "pnlAssocoate" runat = "server" Width="100%" Font-Size="9pt" Font-Italic="false"  GroupingText="Secretary Information">
     <asp:Panel ID="pnlSecretaryList" runat="server" >
          <asp:Label ID="lblNamepnl" style="font-weight:700"  runat="server" > Choose Secreatry: </asp:Label>
          <asp:DropDownList ID="ddlSecretaryList" Width="50%" CssClass="ddlSecretaryList" AutoPostBack="true"  runat="server">                     
                                      </asp:DropDownList><br />
     </asp:Panel><br />
     <table cellpadding="2" cellspacing="0" width="80%">                            
                                <tr>
                                    <td align="right" style="font-weight:700">First Name:</td><td align="left" >  <asp:Label ID="lblFirstName" runat="server" ></asp:Label></td>
                                    <td align="right" style="font-weight:700">Last Name:</td><td align="left">  <asp:Label ID="lblLastName" runat="server" ></asp:Label></td>
                                     <td  align="right" style="font-weight:700">Middle Name:</td><td align="left"> <asp:Label ID="lblMiddleName" runat="server" ></asp:Label></td>
                                   
                                 </tr>
                                <tr>
                                    <td align="right" style="font-weight:700">User Code:</td><td align="left">  <asp:Label ID="lblUserCode" runat="server" ></asp:Label></td>
                                    <td align="right" style="font-weight:700">Email:<span id = "spnE" style="color: #FF0000">*</span>      &nbsp;</td><td colspan="2" align="left">  <input type = "text" id ="txt_Email" class="txt_Email"  size = "40" runat = "server"/></td>
                                    
                                </tr>
                                 <tr>
        <td  align="right" style="font-weight:700" >AssociateRN:</td>
        <td align="left">
            <asp:DropDownList ID="ddlRNsList" CssClass="ddlRNsList"  runat="server">                     
                    </asp:DropDownList><br />
        </td>
        <td  align="right" style="font-weight:700" >Comments: <span id = "Span1" style="color: #FF0000">*</span>     </td>
        <td colspan="2"  align="left">
          <asp:TextBox ID="txtComments" CssClass="txtComments" TextMode="MultiLine" Width="90%" Height="50%" runat="server"></asp:TextBox>
        </td>
        <td  ></td>
        <td >
         
        </td>
    </tr>   
         <tr>
             <td></td>
         </tr>
    
</table>     
    <div class="NavigationMenu" >
        <asp:Button ID="btnSave" CssClass="btnSave" Text="Save" runat="server" />   &nbsp;&nbsp;                                            
        <asp:Button runat="server" ID="btnAddWe"  Text="Add Additional RN Association" /> 
    </div>
        
    <asp:GridView ID="grdRNAssociated" runat="server" BackColor="White" BorderColor="#3366CC" BorderStyle="Inset" BorderWidth="1px" CellPadding="4" 
        DataKeyNames="RN_Secretary_Association_Sid" HorizontalAlign="Center" AutoGenerateColumns="false" Width="100%">
        <Columns>     
               <asp:TemplateField HeaderText="" InsertVisible="False" 
                    ShowHeader="False" FooterStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:HiddenField ID="hdRN_Sid"  Value ='<%# Eval("RN_Sid")%>'  runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>                                                            
            <asp:TemplateField HeaderText="" InsertVisible="False" 
                    ShowHeader="False" FooterStyle-HorizontalAlign="Left">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkSecAssociation" runat="server" Text="Dis_Associate"   
                        CommandName="DisAssociate" CommandArgument='<%# Eval("RN_Secretary_Association_Sid")%>'> 
                        </asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>                                                                                                                                             
            <asp:BoundField DataField="RN_Name" HeaderText="RN Name" />
            <asp:BoundField DataField="RNLicense" HeaderText="RN License" />  
            <asp:BoundField DataField="Comments" HeaderText="Comments" />         
            <asp:BoundField DataField="EditedBy" HeaderText="Last Updated By" />                                                                                                        
        </Columns>
        <RowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" VerticalAlign="Top" />
        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
        <HeaderStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" BorderColor="Black" />
    </asp:GridView> 
     <div class="NavigationMenu" >
         <asp:LinkButton ID="lnkGoBack" Text="Go to Search" runat="server" ></asp:LinkButton>
     </div>              
    </asp:Panel><br />

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
