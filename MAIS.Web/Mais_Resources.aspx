<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="Mais_Resources.aspx.vb" Inherits="MAIS.Web.Mais_Resources" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
     <br />
    <label  style="font-weight: bold" class="CountySelection">MAIS Resources</label>
    <br />
    <asp:Panel ID = "pnlResources" runat = "server" GroupingText = " ">
        <asp:Repeater ID="rptResources" runat="server">
            <HeaderTemplate>
                <table style="border:thin solid #000000">
                    <tr class="gridviewHeader">
                        <th>Priority</th>
                        <th>Subject</th>
                    </tr>          

                
            </HeaderTemplate>
          <ItemTemplate>
                <tr>
                    <td><div id="dvPriority" runat="server" style="background-color: #FF0000 ; width : 25px ; height : 25px" visible='<%# DataBinder.Eval(Container.DataItem, "Priority")%>'></div></td>
                    
                    <td>
                        <asp:LinkButton ID="lkbSubjectMessage" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Resource_Sid")%>' Text='<%# DataBinder.Eval(Container.DataItem, "Subject")%>' CommandName="Select" OnClick="lkbSubjectMessage_Click"></asp:LinkButton></td>
                </tr>
          </ItemTemplate>
            <AlternatingItemTemplate>
                 <tr class="gridviewAlternateRowStyle">
                    <td><div id="dvPriority" runat="server" style="background-color: #FF0000 ; width : 25px ; height : 25px" visible='<%# DataBinder.Eval(Container.DataItem, "Priority")%>'></div></td>

                    <td>
                        <asp:LinkButton ID="lkbSubjectMessage" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Resource_Sid")%>' Text='<%# DataBinder.Eval(Container.DataItem, "Subject")%>' CommandName="Select" OnClick="lkbSubjectMessage_Click"></asp:LinkButton></td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
   <%--<table style="border: thin solid #000000">
    <tr style="border: thin solid #000000">
        <th>
            Priority
        </th>
        <th>
            Date     

        </th>
        <th>
            Subjects
        </th>
      </tr>
      <tr style="border: thin solid #000000">
        <td><div style="background-color: #FF0000 ; width : 25px ; height : 25px"></div></td>
        <td>01/25/2013</td>
        <td><a href="http://doddprodsp05/SitePages/Home.aspx">SM-2013-25 ~ CFIS Web Reverse Button</a></td>
      </tr>
    </table>--%>

    </asp:Panel>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
