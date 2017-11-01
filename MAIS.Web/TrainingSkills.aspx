<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="TrainingSkills.aspx.vb" Inherits="MAIS.Web.TrainingSkills" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/jscript" src="Scripts/TrainingSkillsPage.js"></script>
<script type="text/javascript">
    $("[src*=plus]").live("click", function () {
        $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
        $(this).attr("src", "images/minus.png");
    });
    $("[src*=minus]").live("click", function () {
        $(this).attr("src", "images/plus.png");
        $(this).closest("tr").next().remove();
    });
   

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
    <style type="text/css">
        .auto-style1
        {
            height: 20px;
        }
        .auto-style2
        {
            height: 40px;
        }
        .auto-style3
        {
            height: 26px;
        }
        .auto-style4
        {
            font-family: Arial;
            font-size: medium;
            font-style: normal;
            width: 92%;
        }
    </style>
    </asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <div id="divBack" runat ="server" style="text-align:right">
        <asp:LinkButton ID="hlbBack" runat="server" Visible="false"  >Back to Profile</asp:LinkButton>
     </div>
          <table class="CountySelection" style="width: 100%;">
            <tr>
                <td>
                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Text="Training And CEU's "></asp:Label>
                </td>
            </tr>
        
        </table>
     <div id="div_MessagesContent" runat="server" class="error">
                    </div>
    <asp:Panel ID="pnlNewCourse" runat="server" Visible="false">
        <fieldset id="flsNewCourse" title="Create New Course" >
            <table style="width: 100%;">
                <tr class="gridviewHeader">
                    <td>
                    <asp:Label ID="Label1" runat="server" Text="RN Instuctor Name"></asp:Label></td>
                    <td><asp:Label ID="Label2" runat="server" Text="Effective Start Date"></asp:Label></td>
                    <td><asp:Label ID="Label3" runat="server" Text="Effective End Date"></asp:Label></td>
                    <td><asp:Label ID="Label4" runat="server" Text="Course Number RN"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:DropDownList ID="ddlRnNames" runat="server" AutoPostBack="True"></asp:DropDownList></td>
                    <td>
                        <asp:TextBox ID="txtEffectiveStartDateNew" runat="server" CssClass="date-pick txtEffectiveStartDate"></asp:TextBox></td>
                    <td>
                        <asp:TextBox ID="txtEffectiveEndDateNew" runat="server" CssClass="date-pick txtEffectiveEndDate" ></asp:TextBox></td>
                     <td>
                         <asp:TextBox ID="txtCourseNumberRnNew" runat="server" CssClass="txtCourseNumberRnNew"></asp:TextBox></td>
                </tr>
                <tr class="gridviewHeader">
                    <td>
                    <asp:Label ID="Label5" runat="server" Text="Categaory A CEs"></asp:Label></td>
                    <td><asp:Label ID="Label6" runat="server" Text="Total CEs"></asp:Label></td>
                    <td><asp:Label ID="Label7" runat="server" Text="Level"></asp:Label></td>
                    <td><asp:Label ID="Label8" runat="server" Text="Category"></asp:Label></td>
                </tr>
                <tr>
                    <td>
                        <asp:TextBox ID="txtCategoryACWSNew" runat="server"></asp:TextBox></td>
                    <td>
                        <asp:TextBox ID="txtTotalCEsNew" runat="server"></asp:TextBox>
                    </td>
                    <td>
                        <asp:TextBox ID="txtLevelNew" runat="server"></asp:TextBox> 
                    </td>
                     <td>
                         <asp:TextBox ID="txtCategoryNew" runat="server"></asp:TextBox>
                     </td>
                </tr>
                <tr>
                    <td colspan="5" class="gridviewHeader">
                        <asp:Label ID="Label9" runat="server" Text="Course Description"></asp:Label></td>
                  
                </tr>
                 <tr>
                    <td colspan="5" >
                        <asp:TextBox ID="txtCourseDescriptionNew" runat="server" TextMode="MultiLine" MaxLength="1000" Width="100%"></asp:TextBox>
                    </td>
                    
                </tr>
                <tr>
                     <td>
                         <br />
                         &nbsp;</td>
                     <td>&nbsp;</td>
                     <td>&nbsp;</td>
                     <td>&nbsp;</td>
                </tr>
                <tr>
                    
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>
                        <asp:Button ID="bntCancelCours" runat="server" CommandName="Cancel" Text="Cancel" Style="border: 1px solid #507CD1;
                                                        width: 100px; background-color: White; font-family: Verdana; font-size: 0.8em;
                                                        color: #284E98; margin-bottom: 0px;" Enabled="true" Width="100px"
                                                        Height="18px"/>
                    </td>
                     <td>
                         <asp:Button ID="bntSaveCourse" runat="server" CommandName="Submit" Text="Save Course" Style="border: 1px solid #507CD1;
                                                        width: 100px; background-color: White; font-family: Verdana; font-size: 0.8em;
                                                        color: #284E98; margin-bottom: 0px;"  Enabled="true" Width="100px"
                                                        Height="18px" />
                    </td>
                
                </tr>
            </table>
        </fieldset> 
    </asp:Panel>
    <asp:Panel ID="pnlAddCourse" runat="server" Visible="False">
        <div id="divSearchRNmain" runat="server" style="margin-bottom: 0px;border:solid;height:100px" >
           <div id="divSearchRN" runat ="server" style="float:left;width:49%">
               <asp:Panel ID="Panel1" runat="server">
                   <table style="width: 100%;">
                       <tr class="gridviewHeader">
                           <td colspan="2">Search on RN License or RN Name</td>
                           
                       </tr>
                       <tr>
                           <td>RN License #</td>
                           <td>RN Name</td>
                          
                       </tr>
                       <tr>
                           <td class="auto-style3">
                               <asp:TextBox ID="txtRNNumber" CssClass="txtRNNumber" runat="server"></asp:TextBox></td>
                           <td class="auto-style3">
                               <asp:TextBox ID="txtRNName" runat="server" CssClass="txtRNName"></asp:TextBox></td>
                          
                       </tr>
                       <tr>
                           <td colspan="2">
                               <asp:Button ID="bntRNSearch" runat="server" Text="Search" />
                           </td>
                           
                       </tr>
                   </table>
               </asp:Panel>
           </div>
            <div id="divSearchDate" runat ="server" style="float:left;width:49%">
                <table style="width: 100%;">
                    <tr class="gridviewHeader">
                        <td>Filter by date</td>
                       
                        
                    </tr>
                    <tr>
                        <td>Session Start Date</td>
                        
                        
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            <asp:TextBox ID="txtFilterStartDate" runat="server" CssClass="txtFilterStartDate date-pick" Enabled="False"></asp:TextBox>
                        </td>
                        
                        
                    </tr>
                    <tr>
                        <td class="auto-style1">
                            <asp:Button ID="bntRNFilter" runat="server" Enabled="False" Text="Filter" UseSubmitBehavior="False" />
                        </td>
                    </tr>
                </table>
           </div>
            <div id="divHiddenSearch" runat="server" style="display:none">
                <asp:Button ID="bntSearchRN" runat="server" Text="Search RN" CssClass="bntSearchRN" />
                 <asp:Button ID="bntSearchRNName" runat="server" Text="Search RN Name" CssClass="bntSearchRNName" />
            </div>
         </div> 
        <div id="divSecretaryView" runat="server" visible ="false">
            <table style="width: 100%;">
                <tr class="gridviewHeader">
                    <td>&nbsp;</td>
                    <td>Please Select a RN</td>
                    <td>&nbsp;</td>
                </tr>
                <tr >
                    <td>&nbsp;</td>
                    <td>
                        <asp:DropDownList ID="ddlRNforSecretory" runat="server" Width="200px" AutoPostBack="True"></asp:DropDownList></td>
                    <td>&nbsp;</td>
                </tr>
                <tr>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                    <td>&nbsp;</td>
                </tr>
            </table>
        </div>
        <asp:Panel ID="pnlAddDetails" runat="server">
            <asp:Panel ID="pnlMessage" runat="server" GroupingText=""></asp:Panel>
            <asp:GridView ID="gvSearchData" runat="server" CssClass="gridview" AutoGenerateColumns="False" DataKeyNames="Session_Sid">
                <Columns>
                    <asp:CommandField ShowSelectButton="True" SelectText="Add Session" />
                    <asp:BoundField DataField="Session_Sid" HeaderText="Session ID"   />
                    <asp:BoundField DataField="RN_Name" HeaderText="RN Name" />
                    <asp:BoundField DataField="CourseNumber" HeaderText="Course ID Number" />
                    <asp:BoundField DataField="CourseDescription" HeaderText="Course Description" />
                    <asp:BoundField DataField="StartDate" HeaderText="Session Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="TotalCEs" HeaderText="Total CEs" />
                    <asp:BoundField DataField="SessionLocation" HeaderText="Location" />
                </Columns>
                <EmptyDataTemplate>
                    <strong>No courses found. </strong>
                </EmptyDataTemplate>
                <HeaderStyle CssClass="gridviewHeader" />
            </asp:GridView>
        </asp:Panel>
        <asp:Panel ID="pnlFooterNav" runat="server" HorizontalAlign="Right" CssClass="NavigationMenu">
        <asp:Button ID="bntAddCancel" runat="server" CommandName="Cancel" Text="Cancel" CssClass="ButtonStyle" UseSubmitBehavior="False" /> &nbsp;
        <%--<asp:Button ID="bntAddCourse" runat="server" CommandName="Submit" Text="Add Course" CssClass="ButtonStyle" />--%>
        </asp:Panel>
    </asp:Panel>
    <div id="divCourse" runat="server">
 <asp:GridView ID="grdCourse" runat="server" AutoGenerateColumns="False" CssClass="Grid"
    DataKeyNames="OBNApprovalNumber" OnRowDataBound = "OnRowDataBound">
    <Columns>
        <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
            <ItemTemplate>
                <img alt = "" style="cursor: pointer" src="images/plus.png" />
                <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                    <asp:GridView ID="grdSession" runat="server" AutoGenerateColumns="false" CssClass = "ChildGrid">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                 <img id="imgSession" alt = "" style="cursor: pointer" src="Images/plus.png" />
                                     <asp:Panel ID="pnlSessionDates" runat="server" Style="display: none">
                                         <asp:GridView ID="grdSessionDates" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid">
                                             <Columns>
                                                 <asp:BoundField DataField="Session_Date" HeaderText="Session Date" DataFormatString="{0:MM/dd/yyyy}" />
                                                 <asp:BoundField DataField="Total_CEs" HeaderText="Total CEs" />
                                             </Columns>
                                             <HeaderStyle CssClass="gridviewHeader" />
                                             <EmptyDataTemplate>
                                                 <asp:Label ID="lblSessionDateEmpt" runat="server" Text="No dates to view"></asp:Label>
                                             </EmptyDataTemplate>
                                         </asp:GridView>

                                     </asp:Panel>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Session_Start_Date" HeaderText="Session Start Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                            <asp:BoundField DataField="Session_End_Date" HeaderText="Session End Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                            <asp:BoundField DataField="SessionAddressInfo.Street_Address" HeaderText="Street Address" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="Location_Name" HeaderText="Location Name" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="SessionAddressInfo.City" HeaderText="City" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="SessionAddressInfo.State" HeaderText="State" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="SessionAddressInfo.ZipWithPlus4" HeaderText="Zip" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <%--<asp:LinkButton ID="lnkBtnSelect" runat="server" CommandName="Select" Text = "Create New" CssClass="lnkBtnSelect" Visible = '<%# Eval("Session_Sid")%>' />--%>
                                </ItemTemplate>
                            </asp:TemplateField> 
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </ItemTemplate>

<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></ItemStyle>
        </asp:TemplateField>        
        <asp:BoundField DataField="InstructorName" HeaderText="RN Instructor Name" HeaderStyle-BackColor="#BFE4FF" >
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="StartDate" HeaderText="Effective Start Date" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="EndDate" HeaderText="Effective End Date" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="OBNApprovalNumber" HeaderText="Course ID Number" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="CategoryACEs" HeaderText="Category A CEs" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TotalCEs" HeaderText="Total CEs" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Level" HeaderText="Level" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="Category" HeaderText="Category" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="CourseDescription" HeaderText="Course Description" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
            <ItemTemplate>
                <asp:LinkButton ID="lnkBtnRemove" runat="server" CommandName="Delete" Text = "Remove" CssClass="lnkBtnSelect" Visible = "true" OnClick="lnkBtnSelect_Click" />
            </ItemTemplate>

<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></ItemStyle>
        </asp:TemplateField> 
    </Columns>
     <EmptyDataTemplate>
         <asp:Label ID="Label10" runat="server" Text="This application does not have any courses assigned."></asp:Label>
         <br />
         <asp:LinkButton ID="lkbAddCourse" runat="server" OnClick="lkbAddCourse_Click">Add Session to Application</asp:LinkButton>
     </EmptyDataTemplate>
</asp:GridView>
    </div>
    <br />
<div id="divCEUs" runat="server">
    <h3>CEU Page FOR RENEWAL APPLICATIONS ONLY</h3>
    <asp:ValidationSummary ID="VSCEUMessageSummary" runat="server" ForeColor="Red" ValidationGroup="CEU" />
    <asp:Label ID="lblCEURenewalFor" runat="server" Text="" ></asp:Label>
<div id="divAddCES" runat="server" style="margin-right:5%; margin-left:5%;">
    <table style="width: 100%;">
        <tr class="gridviewHeader"  style="padding-right:5px">
            <td>Date<asp:RequiredFieldValidator ID="RFVCEUDate" runat="server" ControlToValidate="txtCEUDate" ErrorMessage="Must have CEU date." ForeColor="Red" ValidationGroup="CEU">*</asp:RequiredFieldValidator>
            </td>
            <td>CEU's<asp:RequiredFieldValidator ID="RFVCEU" runat="server" ControlToValidate="txtCEUs" ErrorMessage="Must have CEU's." ForeColor="Red" ValidationGroup="CEU">*</asp:RequiredFieldValidator>
            </td>
            <td>RN Name<asp:RequiredFieldValidator ID="RFVRnName" runat="server" ControlToValidate="ddlRNs" ErrorMessage="Must have the RN selected. " ForeColor="Red" InitialValue="-1" ValidationGroup="CEU">*</asp:RequiredFieldValidator>
            </td>
            <td>Instructor Name<asp:RequiredFieldValidator ID="RFVInstructorName" runat="server" ControlToValidate="txtCEUInstructorName" ErrorMessage="Must have Instructor Name." ForeColor="Red" ValidationGroup="CEU">*</asp:RequiredFieldValidator>
            </td>
            <td>Title<asp:RequiredFieldValidator ID="RFVTitle" runat="server" ControlToValidate="txtCEUTitle" ErrorMessage="Must have Title." ForeColor="Red" ValidationGroup="CEU">*</asp:RequiredFieldValidator>
            </td>
            
        </tr>
        <tr>
            <td>
                <asp:TextBox ID="txtCEUDate" runat="server"  CssClass="date-pick txtCEUDate"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txtCEUs" runat="server"  CssClass="MaskTwo" Width="50px"></asp:TextBox></td>
            <td>
                <asp:DropDownList ID="ddlRNs" runat="server"></asp:DropDownList></td>
            <td>
                <asp:TextBox ID="txtCEUInstructorName" runat="server"></asp:TextBox></td>
            <td>
                <asp:TextBox ID="txtCEUTitle" runat="server" Height="22px"></asp:TextBox></td>
           
        </tr>
        <tr class="gridviewHeader">
            <td colspan="5" align="left">Course Description<asp:RequiredFieldValidator ID="RFVCourseDescription" runat="server" ControlToValidate="txtCEUCourseDescription" ErrorMessage="Must have course description." ForeColor="Red" ValidationGroup="CEU">*</asp:RequiredFieldValidator>
            </td>
           
        </tr>
        <tr>
             <td colspan="5" class="auto-style2">
                <asp:TextBox ID="txtCEUCourseDescription" runat="server" Width="95%" TextMode="MultiLine"></asp:TextBox></td>
        </tr>
         <tr>
            <td></td>
            <td></td>
             <td></td>
            <td colspan="2" align="right">
                <asp:Button ID="bntCancel" runat="server" Text="Cancel" OnClientClick="if (!confirm('Are you sure you want to cancel?')) return;" />&nbsp; <asp:Button ID="bntAddCEU" runat="server" Text="Add CEU"  OnClientClick="if (!confirm('Are you sure you want to add the CEU?')) return false;" ValidationGroup="CEU"/> </td>  
        </tr>
    </table>
</div>
<center>
    <br />
    <br />
    <asp:Label ID="lblCEUListMessage" runat="server" Text="Current CEUs that will be used for the renewal"></asp:Label>
<asp:GridView ID = "grdRenewal" runat = "server" AutoGenerateColumns="False" DataKeyNames="CEUs_Renewal_Sid"  >
    <HeaderStyle CssClass="gridviewHeader" />
    <Columns>
        <asp:BoundField DataField="CEUs_Renewal_Sid" HeaderText="CEU ID" Visible="False" />
        <asp:BoundField DataField="Start_Date" DataFormatString="{0:d}" HeaderText="Date" />
        <asp:BoundField DataField="Category_Type_Code" HeaderText="Category" />
        <asp:BoundField DataField="Total_CEUs" HeaderText="CEU " />
        <asp:BoundField DataField="RN_Name" HeaderText="RN" />
        <asp:BoundField DataField="Instructor_Name" HeaderText="Instructor" />
        <asp:BoundField DataField="Title" HeaderText="Title" />
        <asp:BoundField DataField="Course_Description" HeaderText="Description" />
        <asp:TemplateField ShowHeader="False">
            <ItemTemplate>
                <asp:LinkButton ID="LinkButton1" runat="server" CausesValidation="False" CommandName="Delete" CommandArgument='<%# Eval("Ceus_Renewal_Sid")%>' Text="Remove" Visible ='<%# Eval("AllowToRemove") %>'></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    </asp:GridView>
    </center><br />
</div>


<div class="NavigationMenu" >   
    <asp:Button ID="bntPrevious" runat="server"  class="" text="Previous" />&nbsp;&nbsp;
    <%--<asp:LinkButton ID="lnkBtnSelect" runat="server" CommandName="Select" Text = "Create New" CssClass="lnkBtnSelect" Visible = '<%# Eval("Session_Sid")%>' />--%>&nbsp;&nbsp;&nbsp;
    <asp:Button id="bntSaveAndContiue" class="bntSaveAndCountiue" text="Continue" runat = "server" />
</div>
    <asp:HiddenField ID="HFCertStartDate" runat="server"  ClientIDMode="Static"/>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
