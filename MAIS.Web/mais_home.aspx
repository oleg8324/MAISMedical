<%@ Page Title="HomePage" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="mais_home.aspx.vb" Inherits="MAIS.Web._Default" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
    <style type="text/css">
        .auto-style1
        {
            width: 111px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <br />
  <asp:Panel ID="pnlCertAlerts" runat="server"   >
 <center>
 <label style="font-weight: bold" class="CountySelection">Certification Alerts</label>
 <asp:Panel ID = "pnlCertification" runat = "server" GroupingText = " ">
     <asp:Repeater ID="rpRNCert" runat="server">
         <HeaderTemplate>
             <table style="border:thin solid #000000">
                   <tr class="gridviewHeader">
                       <th>Certificate</th>
                       <th>Status</th>
                       <th>Experation Date</th>
                       <th>Expires In</th>
                   </tr>
         </HeaderTemplate>
         <ItemTemplate>
             <tr style="padding: inherit; margin: inherit; border: thick solid #000000;">
                 <td>
                     <%# DataBinder.Eval(Container.DataItem, "Role")%>
                 </td>
                 <td>
                     <%# DataBinder.Eval(Container.DataItem, "Status")%>
                 </td>
                 <td>
                    <%# DataBinder.Eval(Container.DataItem, "EndDate", "{0:MM/dd/yyyy}")%>
                 </td>
                 <td>
                     <%# DataBinder.Eval(Container.DataItem, "ExpiersIn")%>
                 </td>
             </tr>
         </ItemTemplate>
         <AlternatingItemTemplate>
              <tr style="padding: inherit; margin: inherit; border: thick solid #000000;" >
                 <td class="gridviewAlternateRowStyle">
                     <%# DataBinder.Eval(Container.DataItem, "Role")%>
                 </td>
                 <td class="gridviewAlternateRowStyle">
                     <%# DataBinder.Eval(Container.DataItem, "Status")%>
                 </td>
                 <td class="gridviewAlternateRowStyle">
                    <%# DataBinder.Eval(Container.DataItem, "EndDate", "{0:MM/dd/yyyy}")%>
                 </td>
                 <td class="gridviewAlternateRowStyle">
                     <%# DataBinder.Eval(Container.DataItem, "ExpiersIn")%>
                 </td>
             </tr>
         </AlternatingItemTemplate>
         <FooterTemplate>
            </table>
         </FooterTemplate>
     </asp:Repeater>
     <br />
     <asp:Repeater ID="rptCertExpCount" runat="server">
         <HeaderTemplate>
             <table style="border:thin solid #000000">
                 <tr class="gridviewHeader">
                     <th>Certificate Type</th>
                     <th>Expires in 30</th>
                     <th>Expires in 60</th>
                     <th>Expires in 90</th>
                     <th>Expires in 180</th>
                 </tr>
         </HeaderTemplate>
         <ItemTemplate>
             <tr style="border:thick solid #000000">
                 <td> <%# DataBinder.Eval(Container.DataItem, "Role")%></td>
                 <td> <%# DataBinder.Eval(Container.DataItem, "Exp30Days")%></td>
                 <td> <%# DataBinder.Eval(Container.DataItem, "Exp60Days")%></td>
                 <td> <%# DataBinder.Eval(Container.DataItem, "Exp90Days")%></td>
                 <td> <%# DataBinder.Eval(Container.DataItem, "Exp180Days")%></td>
             </tr>
         </ItemTemplate>
         <AlternatingItemTemplate>
              <tr style="border:thick solid #000000">
                 <td class="gridviewAlternateRowStyle"> <%# DataBinder.Eval(Container.DataItem, "Role")%></td>
                 <td class="gridviewAlternateRowStyle"> <%# DataBinder.Eval(Container.DataItem, "Exp30Days")%></td>
                 <td class="gridviewAlternateRowStyle"> <%# DataBinder.Eval(Container.DataItem, "Exp60Days")%></td>
                 <td class="gridviewAlternateRowStyle"> <%# DataBinder.Eval(Container.DataItem, "Exp90Days")%></td>
                 <td class="gridviewAlternateRowStyle"> <%# DataBinder.Eval(Container.DataItem, "Exp180Days")%></td>
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
        <th class="auto-style1">
            Classification
        </th>
      </tr>
      <tr style="border: thin solid #000000">
        <td><asp:LinkButton ID="lbtnRN" runat="server" CssClass="floatright">RN Instructor Certification Alerts</asp:LinkButton></td>
        <td>01/25/2013</td>
        <td class="auto-style1">No Record(s) found</td>
      </tr>
    </table>--%>

 </asp:Panel>
     <br />
    <label  style="font-weight: bold" class="CountySelection">DODD Messages</label>
    <asp:Panel ID = "pnlDODDMessages" runat = "server" GroupingText = " ">
        <asp:Repeater ID="rptDODDMessage" runat="server">
            <HeaderTemplate>
                <table style="border:thin solid #000000">
                    <tr class="gridviewHeader">
                        <th>Priority</th>
                        <th>Date</th>
                        <th>Subject</th>
                    </tr>          

                
            </HeaderTemplate>
          <ItemTemplate>
                <tr>
                    <td><div id="dvPriority" runat="server" style="background-color: #FF0000 ; width : 25px ; height : 25px" visible='<%# DataBinder.Eval(Container.DataItem, "Priority")%>'></div></td>
                    <td>
                        <asp:Label ID="lblMessageStartDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Message_Start_Date", "{0:d}")%>'></asp:Label></td>
                    <td>
                        <asp:LinkButton ID="lkbSubjectMessage" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "DODD_Message_Sid")%>' Text='<%# DataBinder.Eval(Container.DataItem, "Subject")%>' CommandName="Select" OnClick="lkbSubjectMessage_Click"></asp:LinkButton></td>
                </tr>
          </ItemTemplate>
            <AlternatingItemTemplate>
                 <tr class="gridviewAlternateRowStyle">
                    <td><div id="dvPriority" runat="server" style="background-color: #FF0000 ; width : 25px ; height : 25px" visible='<%# DataBinder.Eval(Container.DataItem, "Priority")%>'></div></td>
                    <td>
                        <asp:Label ID="lblMessageStartDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Message_Start_Date", "{0:d}")%>'></asp:Label></td>
                    <td>
                        <asp:LinkButton ID="lkbSubjectMessage" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "DODD_Message_Sid")%>' Text='<%# DataBinder.Eval(Container.DataItem, "Subject")%>' CommandName="Select" OnClick="lkbSubjectMessage_Click"></asp:LinkButton></td>
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
    <br />
    <label >
        <asp:LinkButton ID="lkbArchivedMessage" runat="server">Archived Alerts Messages</asp:LinkButton></label>
    <br />
     <asp:Panel ID="pnlArchivedMessages" runat="server" GroupingText=" " Visible="false">
         <label style="font-weight: bold" class="CountySelection"> DODD Archived Messages</label>
           <asp:Repeater ID="rptArchivedMessage" runat="server">
            <HeaderTemplate>
                <table style="border:thin solid #000000">
                    <tr class="gridviewHeader">
                        <th>Priority</th>
                        <th>Date</th>
                        <th>Subject</th>
                    </tr>          

                
            </HeaderTemplate>
          <ItemTemplate>
                <tr>
                    <td><div id="dvPriority" runat="server" style="background-color: #FF0000 ; width : 25px ; height : 25px" visible='<%# DataBinder.Eval(Container.DataItem, "Priority")%>'></div></td>
                    <td>
                        <asp:Label ID="lblMessageStartDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Message_Start_Date", "{0:d}")%>'></asp:Label></td>
                    <td>
                        <asp:LinkButton ID="lkbSubjectMessage" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "DODD_Message_Sid")%>' Text='<%# DataBinder.Eval(Container.DataItem, "Subject")%>' CommandName="Select" OnClick="lkbSubjectMessage_Click"></asp:LinkButton></td>
                </tr>
          </ItemTemplate>
            <AlternatingItemTemplate>
                 <tr class="gridviewAlternateRowStyle">
                    <td><div id="dvPriority" runat="server" style="background-color: #FF0000 ; width : 25px ; height : 25px" visible='<%# DataBinder.Eval(Container.DataItem, "Priority")%>'></div></td>
                    <td>
                        <asp:Label ID="lblMessageStartDate" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Message_Start_Date", "{0:d}")%>'></asp:Label></td>
                    <td>
                        <asp:LinkButton ID="lkbSubjectMessage" runat="server" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "DODD_Message_Sid")%>' Text='<%# DataBinder.Eval(Container.DataItem, "Subject")%>' CommandName="Select" OnClick="lkbSubjectMessage_Click"></asp:LinkButton></td>
                </tr>
            </AlternatingItemTemplate>
            <FooterTemplate>
                </table>
            </FooterTemplate>
        </asp:Repeater>
     </asp:Panel>
    <br />
    <%--<label style=" font-weight: bold" class="CountySelection">Training/ Resource Forum</label>--%>
    <%--<asp:Panel ID = "pnlTraining" runat = "server" GroupingText = "Training/ Resource Forum">
    <table style="border: thin solid #000000">
        <tr style="border: thin solid #000000">
        <td><a href="http://doddprodsp05/SitePages/Home.aspx">TANF Summer Youth(0)</a></td>
        <td>01/25/2013</td>
      </tr>
    </table></asp:Panel>--%>
    <%--<label><a href="#">Go to Posts</a></label>--%>
    </center>
  </asp:Panel>
 
    
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
