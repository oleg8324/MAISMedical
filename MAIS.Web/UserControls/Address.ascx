<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Address.ascx.vb" Inherits="MAIS.Web.Address" %>
<asp:Panel ID = "pnlMainAddress" runat = "server" CssClass = "pnlMainAddress">
<table class="leftAlign">   
    <tr>
        <td colspan="8">

        </td>
    </tr> 
    <tr>
        <td colspan="1"  style="text-align:right;">Address 1:<span style="color: Red;">*</span></td>
        <td > 
            <input  type = "text" id = "txtAdressLine1" runat = "server"  maxlength = "100" class = "txtAdressLine1"/>           
        </td>
        <td colspan="1"  style="text-align:right;">Address 2:</td>
        <td >
            <input type = "text" id = "txtAddressLine2" runat = "server"  maxlength = "100" class = "txtAddressLine2"/>
        </td>
        <td colspan="1"  style="text-align:right;" >City:<span style="color: Red;">*</span></td>
        <td >
            <input type = "text" id = "txtCity" runat = "server" size="15"  maxlength = "100" class = "txtCity"/>            
        </td>
    </tr>
    <tr>
        <td colspan="1"  style="text-align:right;">State:<span style="color: Red;">*</span></td>
        <td  >
            <asp:DropDownList ID="ddlState" CssClass="ddlState" runat="server" AppendDataBoundItems="True" BackColor="WhiteSmoke"></asp:DropDownList>
            <%--<input type = "text" id = "txtState" runat = "server" size="15" maxlength = "50" class = "txtState"/>--%>           
        </td>
        <td colspan="1"  style="text-align:right;">Zip:<span style="color: Red;">*</span></td>
        <td  >
            <input type = "text" id = "txtZip" runat = "server" size = "5" maxlength = "5" class = "txtZip"/>-            
            <input type = "text" id = "txtZipPlus" runat = "server" size = "4" maxlength = "4" class = "txtZipPlus"/>            
        </td>
        <td colspan="1"  style="text-align:right;" >County:<span id="spanCounty" runat="server" style="color: Red;">*</span></td>
        <td>
            <asp:DropDownList ID="ddlCounty"  CssClass="ddlCounty" runat="server" AppendDataBoundItems="True" BackColor="WhiteSmoke"></asp:DropDownList>
            <%--<input type = "text" id = "txtCounty" runat = "server" size="15" maxlength = "50" class = "txtCounty"/>--%>            
        </td>
    </tr>        
    <tr>
        <td colspan="1"><label id="lblPhone" runat="server"  style="text-align:right;" >Phone Number:<span style="color: Red;" id ="fntPhone" runat ="server">*</span></label></td>
        <td >
            <input type = "text" id = "txtPhoneNumber" runat = "server" size="10" maxlength = "10" class = "txtPhoneNumber"/>                         
        </td>
        <td colspan="1" ><label id="lblEmail" runat="server"  style="text-align:right;">Email Address:<span style="color: Red;" id ="fntEmail" runat ="server">*</span></label></td>
        <td>
            <input type = "text" id = "txtEmailAddress" runat = "server" size="20" maxlength = "50" class = "txtEmailAddress"/>            
        </td>
    </tr>   
</table>
</asp:Panel>
