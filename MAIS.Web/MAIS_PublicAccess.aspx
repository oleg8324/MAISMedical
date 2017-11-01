<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MAIS_PublicAccess.aspx.vb" Inherits="MAIS.Web.MAIS_PublicAccess" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Medication Administration Certification Verification</title>
    <link rel="Stylesheet" type="text/css" href="App_Themes/CW_Style/Main.css" />
    <link rel="Stylesheet" type="text/css" href="Scripts/jquery/jquery.calendars.picker.css" />    
    <script type="text/javascript" src="Scripts/jquery/jquery.1.6.2.min.js"></script> 
    <script type="text/javascript" src="Scripts/jquery/jquery.calendars.pack.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery.calendars.plus.pack.js"></script>
    <script type="text/javascript" src="Scripts/jquery/jquery.calendars.picker.pack.js"></script>    
      <script type="text/javascript" src="Scripts/jquery/jquery.maskedinput-1.3.min.js"></script>
    <script type="text/javascript" src="Scripts/MAISCommon.js"></script>  
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


        <br /><br />
<asp:Panel ID="pnlSearch" runat="server" Width="100%" Font-Size="9pt" Font-Italic="false">
                        <asp:RadioButtonList ID="rblSelect" CssClass="rblSelect" runat="server" 
                                    RepeatDirection="Horizontal" ValidationGroup = "searchpage" CausesValidation="false" BorderStyle="Dotted" BorderWidth="2px" AutoPostBack="true">
                                    <asp:ListItem Value="1" Text="For Current Certification Verification"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="For Certification History Prior to September 2013"></asp:ListItem>
                               </asp:RadioButtonList> 
</asp:Panel><br />

    </div>
            </center>
    </form>
</body>
</html>
