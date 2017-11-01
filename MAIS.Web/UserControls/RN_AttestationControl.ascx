<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="RN_AttestationControl.ascx.vb" Inherits="MAIS.Web.RN_AttestationControl" %>
<%--<style type="text/css">
    .style1
    {
        width: 144px;
        font-family: Verdana, Arial;
        font-size: 8pt;
    }
    .style2
    {
        width: 25px;
        font-family: Verdana, Arial;
        font-size: 8pt;
    }
    .style3
    {
        width: 263px;
        font-family: Verdana, Arial;
        font-size: 8pt;
    }
    .style4
    {
        width: 500px;
        font-family: Verdana, Arial;
        font-size: 8pt;
    }
    .style5
    {
        width: 720px;
      
    }
    .style6
    {
        width: 300px;
        font-family: Verdana, Arial;
        font-size: 8pt;
    }
    .style7
    {
        width: 110px;
        font-family: Verdana, Arial;
        font-size: 8pt;
    }
</style>--%>

<asp:Panel ID="pnlYesNo" runat="server">
    <table class="leftAlign"   id="tblYesNo" >
        <tr>
            <td>
                <asp:Label ID="lblQuestion" runat="server" Text=""></asp:Label>
            </td>
        </tr>
        <tr>
            <td >
                <asp:RadioButtonList ID="RadioButtonList5" CssClass="rblAnswerOnly" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
                </asp:RadioButtonList>
            </td>

        </tr>

    </table>


</asp:Panel>
<asp:HiddenField ID="hiddenQuestionID" runat="server" />
<asp:Label ID="lblErrMsg" CssClass="lblErrMsg" runat="server" Text="" ForeColor="Red"
    Font-Bold="True"></asp:Label>