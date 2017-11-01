<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="maisDODD_Message.aspx.vb" Inherits="MAIS.Web.maisDODD_Message" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/jscript" src="Scripts/DODDMessages.js"></script>

     <script type="text/javascript">
         function IsPublishToValid(sender, args) {
             var test = false;

             if ($(".lstRolesTo OPTION").length > 0) {
                 test = true;
             }
             if ($('.lstPersonTo OPTION').length > 0) {
                 test = true;
             }

             args.IsValid = test;
             return
         };
        

     </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
     <table class="CountySelection" style="width: 100%;">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="DODD Messages"></asp:Label>
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
                   <tr>
                       <td style="width:150px">Start Date  
                           <asp:RequiredFieldValidator ID="rfvMessageStartDate" runat="server" ControlToValidate="txtStartDate" ErrorMessage="Must enter a start date for the message." ForeColor="Red" ValidationGroup="MessageAdding">*</asp:RequiredFieldValidator>
                       </td>
                       <td style="width:150px">End Date<asp:RequiredFieldValidator ID="rfvMessageEndDate" runat="server" ControlToValidate="txtEndDate" ErrorMessage="Must enter an end date for message. " ForeColor="Red" ValidationGroup="MessageAdding">*</asp:RequiredFieldValidator>
                       </td>
                   </tr>
               </thead>
               <tr>
                   <td>
                       <asp:TextBox ID="txtStartDate" runat="server" CssClass="date-pick txtStartDate"></asp:TextBox></td>
                   <td>
                       <asp:TextBox ID="txtEndDate" runat="server" CssClass="date-pick txtEndDate"></asp:TextBox></td>

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
           </table>
            <br />
            <table align="center">
                <thead>
                    <tr class="gridviewHeader">
                        <td colspan="2">Post To<asp:CustomValidator ID="cvPublicistTo" runat="server" EnableTheming="True" ErrorMessage="Must select a role and/or Person to publicist the message too.  " ForeColor="Red" ClientValidationFunction="IsPublishToValid" ValidationGroup="MessageAdding">*</asp:CustomValidator>
                        </td>
                    </tr>
                    <tr>
                        <td style="width:150px"> <asp:CheckBox ID="ckGroup" runat="server" CssClass="ckGroup" Text="Group" /></td>
                        <td style="width:150px"><asp:CheckBox ID="ckPerson" runat="server" CssClass="ckPerson" text="Person"/></td>
                    </tr>
                </thead>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
                
                <tr id="trGroup" runat="server" class="trGroup" style="column-width:auto; display:none">
                    <td colspan="2">
                        <asp:Panel ID="pnlAddGrop" runat="server" Height="100px" GroupingText="Group">
                            <div style="float:left;width:34%;height:95%">
                                <asp:ListBox ID="lstRolesFrom" runat="server" Width="100%" Height="100%" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div style="float:left;Width:10%; padding-left:5%; padding-right:5%; padding-top:5%">
                                <asp:Button ID="bntAdd" runat="server" Text=">" ToolTip="Add Group" /> <br />
                                
                                 <asp:Button ID="bntRemove" runat="server" Text="<" ToolTip="Remove group" />
                            </div>
                            <div style="float:left;width:33%; height:95%" >
                                <asp:ListBox ID="lstRolesTo" runat="server" Width="100%" Height="100%" SelectionMode="Multiple" CssClass="lstRolesTo"></asp:ListBox>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr id="trGroupSendEmail" class="trGroup" style="column-width:auto; display:none">
                    <td colspan="2"><asp:CheckBox ID="ckbGroupSendEmail" runat="server" Text="Send Email to group." /></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                </tr>
                <tr id="trPerson" runat="server" class="trPerson" style="column-width:auto; display:none">
                    <td colspan="3">
                        <asp:Panel ID="pnlAddPerson" runat="server" Height="100px" GroupingText="Person">
                            <div style="float:left;width:40%;height:95%">
                                <asp:ListBox ID="lstPersonFrom" runat="server" Width="100%" Height="100%" SelectionMode="Multiple"></asp:ListBox>
                            </div>
                            <div style="float:left;Width:10%; padding-left:5%; padding-right:5%; padding-top:5%">
                                <asp:Button ID="bntAddPerson" runat="server" Text=">" ToolTip="Add Person" /> <br />
                                
                                 <asp:Button ID="bntRemovePerson" runat="server" Text="<" ToolTip="Remove Person" />
                            </div>
                            <div style="float:left;width:40%; height:95%" >
                                <asp:ListBox ID="lstPersonTo" runat="server" Width="100%" Height="100%" SelectionMode="Multiple" CssClass="lstPersonTo"></asp:ListBox>
                            </div>
                        </asp:Panel>
                    </td>
                </tr>
                <tr id="trPersonSendEmail" class="trPerson" style="column-width:auto; display:none">
                    <td colspan="2">
                        <asp:CheckBox ID="ckbPersonSendEmail" runat="server" Text="Send Email to Persons" />
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
        </div>
        <div id="dvListOfMessage" runat="server">
            <div style="text-align:center">
                <asp:Button ID="bntNewMessage" runat="server" Text="New Message" /> <br /><br />
                <asp:CheckBox ID="ckbSearchMessage" runat="server" CssClass="ckbSearchMessage" Text="Search Messages" AutoPostBack="True" />
            </div>
            <div  style="text-align:center; display:none" class="dvMessageSearchDates" >
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
            </div>
            <br />
            <asp:GridView ID="gvListMessage" runat="server" AutoGenerateColumns="False" CssClass="gridview" DataKeyNames="DODD_Message_Sid">
                
                <Columns>
                    <asp:CommandField ShowSelectButton="True" >
                    </asp:CommandField>
                    <asp:BoundField DataField="Subject" HeaderText="Subject" />
                    <asp:BoundField DataField="Description" HeaderText="Message" />
                    <asp:BoundField DataField="Message_Start_Date" DataFormatString="{0:d}" HeaderText="Start Date" />
                    <asp:BoundField DataField="Message_End_Date" DataFormatString="{0:d}" HeaderText="End Date" />
                    <asp:TemplateField HeaderText="Group of Roles">
                        <ItemTemplate>
                            <asp:BulletedList ID="BulletedList1" runat="server"   DataSource='<%# DataBinder.Eval(Container.DataItem, "RolesList")%>' DataTextField="MAISRoleName">
                            </asp:BulletedList>
                        </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="LIst of RN's">
                        <ItemTemplate>
                            <asp:BulletedList ID="BulletedList2" runat="server" DataSource='<%# DataBinder.Eval(Container.DataItem, "PersonList")%>' DataTextField="RN_Name">
                            </asp:BulletedList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField ShowHeader="False">
                        <EditItemTemplate>
                            <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="True" CommandName="Update" Text="Update"></asp:LinkButton>
                        </EditItemTemplate>
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEditMessage" runat="server" CausesValidation="False" CommandName="Edit" Text="Edit" CommandArgument='<%# DataBinder.Eval(Container.DataItem, "DODD_Message_Sid")%>' OnClick="lnkEditMessage_Click"></asp:LinkButton>
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
                   <tr>
                       <td style="width:150px">Start Date  </td>
                       <td style="width:150px">End Date</td>
                   </tr>
               </thead>
               <tr>
                   <td>
                       <asp:TextBox ID="txtViewStartDate" runat="server" CssClass="txtStartDate"></asp:TextBox></td>
                   <td>
                       <asp:TextBox ID="txtViewEndDate" runat="server" CssClass="txtEndDate"></asp:TextBox></td>

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
