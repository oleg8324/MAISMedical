<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="Mais_Resource_Detail.aspx.vb" Inherits="MAIS.Web.Mais_Resource_Detail" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
     <table class="CountySelection" style="width: 100%;">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Resource Messages"></asp:Label>
                </td>
            </tr>
        </table>
    <br />
    <br />

    <div id="divMainDODDMessage" runat="server">
        <div id="divDODAdmin" runat="server" style="margin-right: auto; margin-left: auto; width: 800px">
            <asp:ValidationSummary ID="vdsAddingMessage" runat="server" ValidationGroup="MessageAdding" ForeColor="Red" />
           <table align="center" >
               <thead>
                   <tr class="gridviewHeader">
                       <td colspan="2"> Message Data </td>
                   </tr>

               </thead>
               <tr>
                   <td>
                       <asp:TextBox ID="txtStartDate" runat="server" Visible="false" CssClass="date-pick txtStartDate"></asp:TextBox></td>
                   <td>
                       <asp:TextBox ID="txtEndDate" runat="server" Visible="false" CssClass="date-pick txtEndDate"></asp:TextBox></td>

               </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="ckPriority" runat="server" Text ="Is Priority" /></td>
                    
                </tr>
               <tr class="gridviewHeader">
                   <td colspan="2">Subject<asp:RequiredFieldValidator ID="rfvMessageSubject" runat="server" ControlToValidate="txtSubject" ErrorMessage="Must enter a subject for the message. " ForeColor="Red" ValidationGroup="MessageAdding">*</asp:RequiredFieldValidator>
                   </td>
               </tr>
               <tr>
                   <td colspan="2">
                       <asp:TextBox ID="txtSubject" runat="server" Width="95%"></asp:TextBox></td>
               </tr>
               <tr>
                   <td></td>
                   <td></td>
               </tr>
               <tr class="gridviewHeader">
                   <td colspan="2">Message<asp:RequiredFieldValidator ID="rfvMessage_Message" runat="server" ControlToValidate="txtMessage" ErrorMessage="Must enter the message. " ForeColor="Red" ValidationGroup="MessageAdding">*</asp:RequiredFieldValidator>
                   </td>
               </tr>
               <tr>
                   <td colspan="2" style="height:150px">
                       <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Height="100%" Width="100%"></asp:TextBox></td>
               </tr>
               <tr>
                   <td></td>
                   <td></td>
               </tr>
               <tr>
                   <td colspan="2" class="gridviewHeader" >Upload Documents</td>
               </tr>
               <tr>
                   <td colspan="2">
                       <asp:Label ID="lblFileUploadText" runat="server" Text="Upload Document"></asp:Label><asp:FileUpload ID="fulMessageDoc" runat="server" />
                   </td>
               </tr>
               <tr>
                   <td colspan="2">
                       <asp:Button ID="bntUploadDoc" runat="server" Text="Upload" /></td>
               </tr>
               <tr>
                   <td colspan="2">
                        <asp:GridView ID="grvMessageDocuments" runat="server" AutoGenerateColumns="False" CssClass="grvMessageDocuments" Width="80%" style="margin:0 2% auto 2%" DataKeyNames="ImageSID">
                        <RowStyle  HorizontalAlign="Left" VerticalAlign="Top" Font-Size="Small" />
                        <Columns>
                            <asp:CommandField ShowDeleteButton="True" />
                            <asp:CommandField SelectText="View" ShowSelectButton="True" />
                            <asp:BoundField DataField="DocumentName" HeaderText="Document Name" />
                        </Columns>
                        <HeaderStyle CssClass="gridviewHeader" />
                    </asp:GridView>
                   </td>
               </tr>
               <tr>
                   <td></td>
                   <td></td>
               </tr>
                <tr>
                   <td colspan="2">
                        <asp:GridView ID="grAdminDocumentsInUDS" runat="server" AutoGenerateColumns="False" CssClass="grvMessageDocuments" Width="100%" style="margin:0 2% auto 2%" DataKeyNames="FileName" Visible="False">
                        <RowStyle  HorizontalAlign="Left" VerticalAlign="Top" Font-Size="Small" />
                        <Columns>
                            
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <%--<asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select" Text="View"></asp:LinkButton>--%>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "DownloadURL")%>' >View</asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FileName" HeaderText="Document Name In UDS" />
                        </Columns>
                            <EmptyDataTemplate>
                                No documents to view.
                            </EmptyDataTemplate>
                        <HeaderStyle CssClass="gridviewHeader" />
                    </asp:GridView>
                   </td>
               </tr>
<tr>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td>
                        <asp:Button ID="bntSaveMessage" runat="server" Text="Save Message" ValidationGroup="MessageAdding" /></td>
                    <td>
                        <asp:Button ID="bntCancelMessage" runat="server" Text="Cancel" /></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:HiddenField ID="hfMessageID" runat="server" />
                    </td>
                </tr>
           </table>
            <br />
<%--            <table align="center">
                
            </table>--%>
        </div>
        <div id="dvListOfMessage" runat="server">
            <div style="text-align:center">
                <asp:Button ID="bntNewMessage" runat="server" Text="New Message" /> <br /><br />
                
            </div>
            <%--<div  style="text-align:center; display:none" class="dvMessageSearchDates" >
                <table align="center">
                    <thead>
                        <tr class="gridviewHeader">
                            <td>Start Date</td>
                            <td>End Date</td>
                        </tr>
                    </thead>
                    <tr>
                        <td>
                            <asp:TextBox ID="txtSearchStartDate" runat="server" CssClass="date-pick txtSearchStartDate"></asp:TextBox></td>
                        <td> <asp:TextBox ID="txtSearchEndDate" runat="server" CssClass="date-pick txtSearchEndDate"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan ="2">
                            <asp:Button ID="bntSearch" runat="server" Text="Search Messages" />
                        </td>
                    </tr>
                </table>
            </div>--%>
            <br />
            <asp:GridView ID="gvListMessage" runat="server" AutoGenerateColumns="False" CssClass="gridview" DataKeyNames="Resource_Sid">
                
                <Columns>
                    <asp:CommandField ShowSelectButton="True" >
                    </asp:CommandField>
                    <asp:BoundField DataField="Subject" HeaderText="Subject" />
                    <asp:BoundField DataField="Description" HeaderText="Message" />
                    
                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEditMessage" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "Resource_Sid")%>' OnClick="lnkEditMessage_Click"></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <HeaderStyle CssClass="gridviewHeader" />
                <AlternatingRowStyle CssClass="gridviewAlternateRowStyle" />
            </asp:GridView>
        </div>
        <br />

        <div id="dvViewMessage" runat="server" visible="false">
            <table align="center" >
               <thead>
                   <tr class="gridviewHeader">
                       <td colspan="2"> Message Data </td>
                   </tr>
                   
               </thead>
               <tr>
                   <td>
                       <asp:TextBox ID="txtViewStartDate" runat="server" Visible="false" CssClass="txtStartDate"></asp:TextBox></td>
                   <td>
                       <asp:TextBox ID="txtViewEndDate" runat="server" Visible="false" CssClass="txtEndDate"></asp:TextBox></td>

               </tr>
                <tr>
                    <td colspan="2">
                        <asp:CheckBox ID="ckViewPriority" runat="server" Text ="Is Priority" /></td>
                    
                </tr>
               <tr class="gridviewHeader">
                   <td colspan="2">Subject</td>
               </tr>
               <tr>
                   <td colspan="2">
                       <asp:TextBox ID="txtViewSubject" runat="server" Width="95%"></asp:TextBox></td>
               </tr>
               <tr>
                   <td></td>
                   <td></td>
               </tr>
               <tr class="gridviewHeader">
                   <td colspan="2">Message</td>
               </tr>
               <tr>
                   <td colspan="2" style="height:150px">
                       <asp:TextBox ID="txtViewMessage" runat="server" TextMode="MultiLine" Height="100%" Width="100%"></asp:TextBox></td>
               </tr>
               <tr>
                   <td></td>
                   <td></td>
               </tr>
               <tr>
                   <td colspan="2" class="gridviewHeader" >Documents</td>
               </tr>
               <tr>
                   <td colspan="2">
                       <%--<asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select" Text="View"></asp:LinkButton>--%><%--<asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select" Text="View"></asp:LinkButton>--%>
                   </td>
               </tr>
               <tr>
                   <td colspan="2">
                       <%--<asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select" Text="View"></asp:LinkButton>--%></td>
               </tr>
               <tr>
                   <td colspan="2">
                        <asp:GridView ID="grViewUDSDoc" runat="server" AutoGenerateColumns="False" CssClass="grvMessageDocuments" Width="100%" style="margin:0 2% auto 2%" DataKeyNames="FileName">
                        <RowStyle  HorizontalAlign="Left" VerticalAlign="Top" Font-Size="Small" />
                        <Columns>
                            
                            <asp:TemplateField ShowHeader="False">
                                <ItemTemplate>
                                    <%--<asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Select" Text="View"></asp:LinkButton>--%>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "DownloadURL")%>' >View</asp:HyperLink>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="FileName" HeaderText="Document Name" />
                        </Columns>
                            <EmptyDataTemplate>
                                No documents to view.
                            </EmptyDataTemplate>
                        <HeaderStyle CssClass="gridviewHeader" />
                    </asp:GridView>
                   </td>
               </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:Button ID="bntBackToHomePage" runat="server" Text="Back To Home Page" UseSubmitBehavior="False" />
                    </td>
                </tr>
           </table>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
