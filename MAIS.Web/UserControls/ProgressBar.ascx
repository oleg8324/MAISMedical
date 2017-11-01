<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="ProgressBar.ascx.vb" Inherits="MAIS.Web.ProgressBar" %>
<style type="text/css">
    .cell
    {
        border-style: solid;
        border-width: 1px;
        border-color: Black;
        height: 7px;
        width: 45px;
        background-color: White;
    }
</style>
<asp:Panel ID="pnlProgressBar" runat="server">
    <table align="center">
        <tr>
            <td>
                <asp:Table ID="tblProgBar" CellPadding="3" CellSpacing="1" runat="server" Font-Size="Small">
                    <asp:TableRow ID="tblRow1">
                    </asp:TableRow>
                    <asp:TableRow ID="tblRow2">
                    </asp:TableRow>
                </asp:Table>
            </td>
        </tr>
    </table>
</asp:Panel>