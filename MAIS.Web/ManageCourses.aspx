<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="ManageCourses.aspx.vb" Inherits="MAIS.Web.ManageCourses" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="Scripts/spin.min.js"></script>
    <script type="text/jscript" src="Scripts/ManageCourses.js"></script>
    
    <script type="text/javascript">
        $("[src*=plus]").live("click", function () {
            $(this).closest("tr").after("<tr><td></td><td colspan = '999'>" + $(this).next().html() + "</td></tr>")
            $(this).attr("src", "images/minus.png");
        });
        $("[src*=minus]").live("click", function () {
            $(this).attr("src", "images/plus.png");
            $(this).closest("tr").next().remove();
        });

        function IsSyllabusValid(sender, args) {
            
            if ($(".grvSyllabus tr").length > 0) {
                args.IsValid = true;
            } else {
                args.IsValid = false;
            }
            return 
        };

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
    <style type="text/css">
        .auto-style1
        {
            height: 20px;
        }
        .ddlLevelNew
        {}
        .auto-style2
        {
            background-color: #BFE4FF;
            font-weight: bold;
            height: 20px;
        }
        .auto-style3
        {
            height: 29px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <div >
         <table class="CountySelection" style="width: 100%;">
             <tr>
                <td>
                    <asp:Label ID="Label5" runat="server" Font-Bold="True" Text="Manage Courses"></asp:Label>
                </td>
            </tr>
         </table>

   <%--<h3>Manage Courses</h3>--%> <div style="float:right">
       <asp:LinkButton ID="lkbBack" runat="server" Visible="False">Back to Manage Course Search</asp:LinkButton></div></div><br /> 


   <center>
<asp:ValidationSummary ID="ValidationSummary1" runat="server" CssClass="ErrorSummary" ValidationGroup="ac" />
        </center>
    <div id="div1" runat="server" class="divSpinner" style="font-family:Tahoma, Arial, Verdana; font-size:24pt; font-weight:bold; text-align: center;">
            <br /><br />
        </div>
    <div id="dvNewRnType" runat="server" visible="false" class="dvNewRnType" style="margin-right: auto; margin-left: auto; width: 800px" >
        <asp:Panel ID="Panel1" runat="server" GroupingText="Select a Course Type">
            <center>
                <asp:RadioButtonList ID="rblNewRnTypeSelect" class="rblNewRnTypeSelect" runat="server"
                    RepeatDirection="Horizontal" BorderStyle="Dotted" BorderWidth="2px" AutoPostBack="True">
                    <asp:ListItem Value="0" Text="RN"></asp:ListItem>
                    <asp:ListItem Value="1" Text="DD Personnel"></asp:ListItem>

                </asp:RadioButtonList>
            </center>
        </asp:Panel>
    </div>
     
    <div id="dvNewCourseTable" class="dvNewCourseTable"  style="margin-right: auto; margin-left:auto; width:800px" >
        <table style="width: 100%;">
            <thead>
                <tr style="width:100%">
                    <td colspan="4"><h3>Add Course</h3></td>
                </tr>
            </thead>
            <tr class="gridviewHeader">
                <td class="auto-style1">
                    <asp:Label ID="Label1" runat="server" Text="RN Instructor Name"></asp:Label>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddlRnNames" ErrorMessage="Must select a RN name." ForeColor="Red" InitialValue="-1" ValidationGroup="ac">*</asp:RequiredFieldValidator>
                </td>
                <td class="auto-style1">
                    <asp:Label ID="Label2" runat="server" Text="Effective Start Date"></asp:Label>
                    <asp:RequiredFieldValidator ID="rfvEffectiveStartDate" runat="server" ControlToValidate="txtEffectiveStartDateNew" ErrorMessage="Must enter an effective start date." ForeColor="Red" ValidationGroup="ac">*</asp:RequiredFieldValidator>
                </td>
                <td class="auto-style1">
                    <asp:Label ID="Label3" runat="server" Text="Effective End Date"></asp:Label>
                    <asp:RequiredFieldValidator ID="rfvEffectEndDate" runat="server" ControlToValidate="txtEffectiveEndDateNew" ErrorMessage="Must enter an effective end date" ForeColor="Red" ValidationGroup="ac">*</asp:RequiredFieldValidator>
                </td>
                <td class="auto-style1">
                    <asp:Label ID="Label4" runat="server" Text="Course ID Number"></asp:Label>
                    <asp:RequiredFieldValidator ID="rfvCourseNumberRnNew" runat="server" CssClass="rfvCourseNumberRnNew" ControlToValidate="txtCourseNumberRnNew" ErrorMessage="Must enter the course number. " ForeColor="Red" ValidationGroup="ac">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlRnNames" runat="server" CssClass="ddlRnNames" AutoPostBack="false" CausesValidation="True" Width="90%"></asp:DropDownList>
                </td>
                <td>
                    <asp:TextBox ID="txtEffectiveStartDateNew" runat="server" CssClass="date-pick txtEffectiveStartDate" Width="50%"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtEffectiveEndDateNew" runat="server" CssClass="txtEffectiveEndDate" Width="50%" ReadOnly="True"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtCourseNumberRnNew" runat="server" CssClass="txtCourseNumberRnNew"></asp:TextBox>
                </td>
            </tr>
            <tr class="gridviewHeader">
                <td>
                    <asp:Label ID="lblCategoryACES" runat="server" Text="Category A CEs"></asp:Label>
                    <asp:RequiredFieldValidator ID="rfvCategoryACES" runat="server" ControlToValidate="txtCategoryACWSNew" ErrorMessage="Must enter in a Category A CEs." ForeColor="Red" ValidationGroup="ac">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Label ID="Label6" runat="server" Text="Total CEs"></asp:Label>
                    <asp:RequiredFieldValidator ID="rfvTotalCesNew" runat="server" ControlToValidate="txtTotalCEsNew" ErrorMessage="Must enter in a Total CEs." ForeColor="Red" ValidationGroup="ac">*</asp:RequiredFieldValidator>
                    <asp:RangeValidator ID="rvTotalCES" runat="server" ErrorMessage="RangeValidator" ValidationGroup="ac" ControlToValidate="txtTotalCEsNew" ForeColor="Red" MaximumValue="99.0" MinimumValue="14.0" Type="Double">*</asp:RangeValidator>
                </td>
                <td>
                    <asp:Label ID="Label7" runat="server" Text="Level"></asp:Label>
                    <asp:RequiredFieldValidator ID="rvfTxtLevelnew" runat="server" ControlToValidate="ddlLevelNew" ErrorMessage="Must enter a Level." InitialValue="-1" ForeColor="Red" ValidationGroup="ac">*</asp:RequiredFieldValidator>
                </td>
                <td>
                    <asp:Label ID="Label8" runat="server" Text="Category"></asp:Label>
                    <asp:RequiredFieldValidator ID="rfvTxtCategoryNew" runat="server" ControlToValidate="ddlCategoryNew" ErrorMessage="Must enter a Category." InitialValue="-1" ForeColor="Red" ValidationGroup="ac">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtCategoryACWSNew" CssClass="txtCategoryACWSNew MaskOne" runat="server" Width="30px" MaxLength="1"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtTotalCEsNew" runat="server" CssClass="MaskTwo" Width="30px" MaxLength="5"></asp:TextBox>
                </td>
                <td >
                    <asp:DropDownList ID="ddlLevelNew" runat="server" CssClass="ddlLevelNew" AutoPostBack="True" Width="90%"  ></asp:DropDownList>
                </td>
                <td >
                    <asp:DropDownList ID="ddlCategoryNew" CssClass="ddlCategoryNew" runat="server" Width="90%" AutoPostBack="True" ></asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td colspan="4" class="auto-style2">
                    <asp:Label ID="Label9" runat="server" Text="Course Description"></asp:Label>
                    <asp:RequiredFieldValidator ID="rfvTxtCourseDescriptionNew" runat="server" ControlToValidate="txtCourseDescriptionNew" ErrorMessage="Must enter a course description." ForeColor="Red" ValidationGroup="ac">*</asp:RequiredFieldValidator>
                </td>

            </tr>
            <tr>
                <td colspan="4">
                    <asp:TextBox ID="txtCourseDescriptionNew" runat="server" TextMode="MultiLine" MaxLength="1000" Width="95%"></asp:TextBox>
                </td>

            </tr>
            <tr>
                <td colspan="4" class="auto-style2">
                    <asp:Label ID="Label12" runat="server" Text="Syllabus"></asp:Label>
                    <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="The course must have a syllabus." ForeColor="Red" ValidationGroup="ac" ClientValidationFunction="IsSyllabusValid">*</asp:CustomValidator>
                </td>

            </tr>
            <tr>
                <td colspan="4">
                    <asp:Label ID="Label11" runat="server" Text="Choose syllabus to upload:"></asp:Label><asp:FileUpload ID="uplSyllabusUpload" runat="server" />
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <asp:Button ID="bntSyllabusUpload" runat="server" Text="Upload" />


                    <br />
                    <asp:GridView ID="grvSyllabus" runat="server" AutoGenerateColumns="False" CssClass="grvSyllabus" Width="80%" style="margin:0 5% auto 5%" DataKeyNames="ImageSID">
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

              
                <td colspan ="4" align="right">   
                    <asp:Button ID="bntCancelCours" CssClass="bntCanceCourse" runat="server" CommandName="Cancel" Text="Cancel" UseSubmitBehavior="False"  />
                    &nbsp;&nbsp;
                    <asp:Button ID="BntSaveCourseData" CssClass="bntSaveCourseData" runat="server" Text="Save Course" UseSubmitBehavior="False" ValidationGroup="ac" OnClientClick="if (!confirm('Are you sure you want to save?')) return;" />
                      &nbsp;&nbsp;
                    <asp:Button ID="bntSaveCourse" CssClass="bntSaveCourse" runat="server" CommandName="Submit" Text="Move to Session" UseSubmitBehavior="False" ValidationGroup="ac"  />
                </td>

            </tr>
        </table>

    </div>

    <div id="dvAddSessions" runat="server"  class="dvAddSessions" style="margin-right: auto; margin-left:auto; width:800px" visible="False" >
        <table style="width: 100%;">
            <thead>
                <tr style="width:100%">
                    <td colspan ="4"> <h3> Add Session</h3></td>
                </tr>
                <tr>
                    <td colspan="4">
                        <center>
                            <asp:ValidationSummary ID="ValidationSummary2" runat="server" CssClass="ErrorSummary" ValidationGroup="ab" />
                        </center>
                    </td>
                </tr>
               
            </thead>
            <tr class="gridviewHeader">
                <td>Session Start Date<asp:RequiredFieldValidator ID="rfvSessionStartDate" runat="server" ErrorMessage="Must enter in a Session Start Date." ControlToValidate="txtSessionStartDateNew" ForeColor="Red" ValidationGroup="ab">*</asp:RequiredFieldValidator>
                </td>
                <td>Session End Date<asp:RequiredFieldValidator ID="rfvtxtSessionEndDateNew" runat="server" ControlToValidate="txtSessionEndDateNew" ErrorMessage="Must enter in a Session End Date. " ForeColor="Red" ValidationGroup="ab">*</asp:RequiredFieldValidator>
                </td>
                <td colspan="2">Location Name<asp:RequiredFieldValidator ID="rfvtxtSessionLocationNew" runat="server" ErrorMessage="Must enter in a Location. " ControlToValidate="txtSessionLocationNew" ForeColor="Red" ValidationGroup="ab">*</asp:RequiredFieldValidator>
                </td>
            </tr>
            <tr>
                
                <td>
                    <asp:TextBox ID="txtSessionStartDateNew" runat="server" CssClass="date-pick txtSessionStartDateNew"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtSessionEndDateNew" runat="server" CssClass="date-pick txtSessionEndDateNew"></asp:TextBox>
                </td>
                <td colspan="2">
                    <asp:TextBox ID="txtSessionLocationNew" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr class="gridviewHeader">
                <td colspan="2" align="left">Sponsor</td>
                
                <td><font color="#000000" face="Calibri" size="3">Open to the Public</font></td>
                <td></td>

            </tr>
            <tr>
                <td colspan="2" align="left" class="auto-style3">
                    <asp:TextBox ID="txtSessionSponserNew" runat="server" Width="90%" CssClass="txtSessionSponserNew" ></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvtxtSessionSponserNew" runat="server" ErrorMessage="Must enter a Sponser for the Session" Text ="*" ControlToValidate="txtSessionSponserNew" ForeColor="Red" ValidationGroup="ab"></asp:RequiredFieldValidator>
                </td>
                
                <td class="auto-style3">
                    <asp:CheckBox ID="ckbPublicView" runat="server" />
                </td>
                <td class="auto-style3"></td>
            </tr>
            <tr class="gridviewHeader" align="left">
                <td colspan="4" class="auto-style1">Street Address<asp:RequiredFieldValidator ID="rfvtxtSessionAddressNew" runat="server" ErrorMessage="Must enter in a Session Address. " ControlToValidate="txtSessionAddressNew" ForeColor="Red" ValidationGroup="ab">*</asp:RequiredFieldValidator>
                 </td>
                
            </tr>
            <tr align="left">
                 <td colspan="4">
                     <asp:TextBox ID="txtSessionAddressNew" runat="server" Width="90%"></asp:TextBox>
                 </td>
                
            </tr>
            <tr class="gridviewHeader">
                <td>City<asp:RequiredFieldValidator ID="rfvtxtSessionCityNew" runat="server" ControlToValidate="txtSessionCityNew" ErrorMessage="Must enter in a Session Address." ForeColor="Red" ValidationGroup="ab">*</asp:RequiredFieldValidator>
                 </td>
                <td>State<asp:RequiredFieldValidator ID="rfvtxtSessionStateNew" runat="server" ControlToValidate="ddlSessionStateNew" ErrorMessage="Must enter in a State. " ForeColor="Red" ValidationGroup="ab">*</asp:RequiredFieldValidator>
                 </td>
                <td>Zip<asp:RequiredFieldValidator ID="rfvtxtSessionZipNew" runat="server" ControlToValidate="txtSessionZipNew" ErrorMessage="Must enter a Zip Code." ForeColor="Red" ValidationGroup="ab">*</asp:RequiredFieldValidator>
                 </td>
                <td>County<asp:RequiredFieldValidator ID="rfvddlSessionCounty" runat="server" ControlToValidate="ddlSessionCounty" ErrorMessage="Must select a County. " ForeColor="Red" InitialValue="--- County Selection ---" ValidationGroup="ab">*</asp:RequiredFieldValidator>
                </td>

            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtSessionCityNew" runat="server" CssClass="CityMask"></asp:TextBox>
                 </td>
                <td>
                    <asp:DropDownList ID="ddlSessionStateNew" runat="server" with="20%"></asp:DropDownList>
                 </td>
                <td>
                    <asp:TextBox ID="txtSessionZipNew" runat="server" CssClass="ZipMask" MaxLength="5" Width="70px"></asp:TextBox>
                    -<asp:TextBox ID="txtSessionZipPlusFour" runat="server" CssClass="ZipMask" MaxLength="4" Width="50px"></asp:TextBox>
                 </td>
                <td>
                    <asp:DropDownList ID="ddlSessionCounty" runat="server">
                    </asp:DropDownList>
                </td>

            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>

  
                </td>
                 <td align="right">
                     <asp:Button ID="bntPriviousSession" runat="server" Text="Edit Course Details" UseSubmitBehavior="False" />
&nbsp;
                     <asp:Button ID="bntMoveToSessionDetails" CssClass="bntMoveToSessionDetails" runat="server" Text="Move to Session Detail" UseSubmitBehavior="False" ValidationGroup="ab" />
                </td>
            </tr>
           
        </table>
    </div>
  <div id="dvSessionDetailAdd" runat="server" class ="dvSessionDetailAdd"  style="margin-right: auto; margin-left:auto; width:800px" visible="False" >
        <table style="width: 100%;">
            <thead>
                <tr style="width:100%">
                    <td colspan="3"><h3>Add Session Details</h3></td>
                </tr>
                <tr>
                    <td colspan="3">
                        <asp:ValidationSummary ID="vsAddSessionDetails" runat="server" CssClass="ErrorSummary" ValidationGroup="ae" />
                    </td>
                </tr>
            </thead>
            <tr class="gridviewHeader">
                <td class="auto-style1">Class Date<asp:RequiredFieldValidator ID="rfvtxtAddClassDate" runat="server" ControlToValidate="txtAddClassDate" ErrorMessage="Must enter a the Class Date. " ForeColor="Red" ValidationGroup="ae">*</asp:RequiredFieldValidator>
                </td>
                <td class="auto-style1">Total CEs<asp:RequiredFieldValidator ID="rfvtxtAddTotalCEs" runat="server" ControlToValidate="txtAddTotalCEs" ErrorMessage="Must enter in Total CEs. " ForeColor="Red" ValidationGroup="ae">*</asp:RequiredFieldValidator>
                </td>
                <td class="auto-style1"></td>
            </tr>
            <tr>
                <td>
                    <asp:TextBox ID="txtAddClassDate" CssClass="date-pick txtAddClassDate" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:TextBox ID="txtAddTotalCEs" runat="server" CssClass="MaskTwo"></asp:TextBox>
                </td>
                <td>
                    <asp:LinkButton ID="lkAddSessionDetail" runat="server" ValidationGroup="ae">Add Session Detail </asp:LinkButton></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td colspan="3">
                    <asp:Label ID="lblSaveMessage" runat="server" ForeColor="Blue" Text="The Session and CE’s with the session are not final until the 'Save All Information' button is pressed."></asp:Label></td>
                
            </tr>
        </table>
      <center>
          <asp:GridView ID="gvAddSessionDetails" runat="server" CssClass="gvAddSessionDetails" AutoGenerateColumns="False" DataKeyNames="Session_Sid">
              <Columns>
                  <asp:CommandField ButtonType="Button" ShowDeleteButton="True" DeleteText="Remove" />
                  <asp:BoundField DataField="Session_Date" HeaderText="Start Date" DataFormatString="{0:MM/dd/yyyy}"  />
                  <asp:BoundField DataField="Total_CEs" HeaderText="Total CEs" />
              </Columns>
          </asp:GridView>
      </center>
      <table style="width: 100%;">
          <tr style="width:100%">
              <td align="right"> <asp:Button ID="bntSessionDetailCancel" runat="server" Text="Edit Session" />&nbsp; <asp:Button ID="BntSubitAll" runat="server" Text="Save All Information" OnClientClick="if (!confirm('Are you sure you want to commit your current changes?')) return;" Enabled="False" /></td>

          </tr>
         
          <tr>
              <td>
                  <br />
                  <asp:Label ID="lblAddSessionError" runat="server" ForeColor="Red"></asp:Label>
              </td>
              <td>&nbsp;</td>
              <td>&nbsp;</td>
          </tr>
      </table>
    </div>
    <%--<div id="dvAddCourse" runat="server" visible="false">
      <asp:GridView ID="gvAddCourse" runat="server" AutoGenerateColumns="False" CssClass="gridview" DataKeyNames="Course_Sid">
          <HeaderStyle CssClass="gridviewHeader" />
          <Columns>
            <asp:TemplateField>    
                <ItemTemplate>
                    <img alt="" style="cursor:pointer" src="Images/plus.png" />
                        <asp:Panel ID="pnlSession" runat="server" Style="display:none">
                            

                           <asp:gridview Id="gvAddSession" runat="server" AutoGenerateColumns="false" CssClass="ChildGrid" DataKeyNames="Session_Sid" >
                                <Columns>
                                    <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                        <ItemTemplate>
                                            <img alt="" style="cursor: pointer" src="Images/plus.png" />
                                        </ItemTemplate>

                                        
                                        <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%" />

                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblStartDate" runat="server" Text='<%# Bind("Session_Start_Date")%>'></asp:Label>
                                        </ItemTemplate>
                                        
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblEndDate" runat="server" Text='<%# Bind("Session_End_Date")%>' ></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSponsor" runat="server" Text='<%# Bind("Sponsor")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ListBox ID="lblLocationName" runat="server" text='<%# Bind("Location_Name") %>'></asp:ListBox>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSessionCEs" runat="server" Text='<%# Bind("Total_CEs")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSSessionStreetAddress" runat="server" Text='<%# Bind("SessionAddressInfo.Street_Address")%>>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSessionCity" runat="server" Text='<%# Bind("SessionAddressInfo.City")%>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSessionState" runat="server" Text='<%# Bind("SessionAddressInfo.State") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Label ID="lblSessionZipCode" runat="server" Text='<%# Bind("SessionAddressInfo.ZipWithPlus4") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                               <EmptyDataTemplate>
                                   <asp:Panel ID="Panel2" runat="server">
                                       <table>
                                           <thead class="gridviewHeader">
                                               <tr>
                                                   <td></td>
                                                   <td>Session Start Date</td>
                                                   <td>Session End Date</td>
                                               </tr>
                                           </thead>
                                           <tr>
                                               <td>
                                                   <asp:LinkButton ID="lkbInsertNewSession" runat="server" CommandName="Insert_New_Session" CommandArgument="StartDate:<%# txtNewSessionStartDate %>>" OnClick="lkbInsertNewSession_Click">Insert Session</asp:LinkButton></td>
                                               <td>
                                                   <asp:TextBox ID="txtNewSessionStartDate" runat="server"  CssClass="date-pick"></asp:TextBox>
                                               </td>
                                               <td> <asp:TextBox ID="txtNewSessionEndDate" runat="server" CssClass="date-pick"></asp:TextBox>
                                               </td>
                                           </tr>
                                       </table>
                                   </asp:Panel>
                               </EmptyDataTemplate>
                            </asp:gridview>
                            

                        </asp:Panel>
                </ItemTemplate>
            </asp:TemplateField>
              <asp:TemplateField HeaderText="Instructor Name">
                  <EditItemTemplate>
                      <asp:TextBox ID="txtInstructorName" runat="server" Text='<%# Bind("InstructorName") %>'></asp:TextBox>
                  </EditItemTemplate>
                  <ItemTemplate>
                      <asp:Label ID="Label1" runat="server" Text='<%# Bind("InstructorName") %>'></asp:Label>
                  </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="Start Date">
                  <EditItemTemplate>
                      <asp:TextBox ID="txtStartDate" runat="server" Text='<%# Bind("StartDate") %>'></asp:TextBox>
                  </EditItemTemplate>
                  <ItemTemplate>
                      <asp:Label ID="Label2" runat="server" Text='<%# Bind("StartDate") %>'></asp:Label>
                  </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="End Date">
                  <EditItemTemplate>
                      <asp:TextBox ID="txtEndDate" runat="server" Text='<%# Bind("EndDate") %>'></asp:TextBox>
                  </EditItemTemplate>
                  <ItemTemplate>
                      <asp:Label ID="Label3" runat="server" Text='<%# Bind("EndDate") %>'></asp:Label>
                  </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="Coures Number">
                  <EditItemTemplate>
                      <asp:TextBox ID="txtOBNApprovalNumber" runat="server" Text='<%# Bind("OBNApprovalNumber") %>'></asp:TextBox>
                  </EditItemTemplate>
                  <ItemTemplate>
                      <asp:Label ID="Label4" runat="server" Text='<%# Bind("OBNApprovalNumber") %>'></asp:Label>
                  </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="Category A CEs">
                  <EditItemTemplate>
                      <asp:TextBox ID="txtCategoryACEs" runat="server" Text='<%# Bind("CategoryACEs") %>'></asp:TextBox>
                  </EditItemTemplate>
                  <ItemTemplate>
                      <asp:Label ID="Label5" runat="server" Text='<%# Bind("CategoryACEs") %>'></asp:Label>
                  </ItemTemplate>
              </asp:TemplateField>
              <asp:TemplateField HeaderText="Total CEs">
                  <EditItemTemplate>
                      <asp:TextBox ID="txtTotalCEs" runat="server" Text='<%# Bind("TotalCEs") %>'></asp:TextBox>
                  </EditItemTemplate>
                  <ItemTemplate>
                      <asp:Label ID="Label6" runat="server" Text='<%# Bind("TotalCEs") %>'></asp:Label>
                  </ItemTemplate>
              </asp:TemplateField>
      
          </Columns>

          <EmptyDataTemplate>
              
              <br />
              
          </EmptyDataTemplate>

      </asp:GridView>
 

  </div>--%>
<div id="divHidenFields" runat="server" style="display:none" >
    <asp:HiddenField ID="hdfLoginRole"  runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdfMAISRole"  runat="server"  ClientIDMode ="Static"/>
    <asp:HiddenField ID="hdfUserID" runat="server" ClientIDMode="Static" />
    <asp:HiddenField ID="hdfRNID" runat="server" ClientIDMode="Static" />
 </div>
<div id="dvMainSearch" runat="server">
    <div   >
        <asp:Panel ID="pSearch"  GroupingText="Please choose one" runat="server" Width="80%" Font-Size="9pt" Font-Italic="false" style="display:none">
            <center>
                <asp:RadioButtonList ID="RadioButtonList1" class="rblSelect" runat="server" 
                     RepeatDirection="Horizontal" BorderStyle="Dotted" BorderWidth="2px">
                     <asp:ListItem Value="0" Text="Search for a course"></asp:ListItem>
                     <asp:ListItem Value="1" Text="Search for a session"></asp:ListItem>
                </asp:RadioButtonList>
            </center>
        </asp:Panel>
    </div>

    <div>
        <asp:Panel ID="PPersonal"  GroupingText="Search Options" runat="server" Width="80%" Font-Size="9pt" Font-Italic="false">

<asp:CustomValidator ID="CustomValidator1" runat="server" 
        OnServerValidate="ServerValidate" ClientValidationFunction="OneReq" 
        ErrorMessage="Either name or RN Liscence number should be entered" Display="None" 
        ValidationGroup="a"></asp:CustomValidator>
<center>
   <table>

<tr><td align="right">
<label>RN License No. :</label></td><td>
<asp:TextBox id="txtRNNO" runat="server" CssClass="txtRNNO" style="float:left" ></asp:TextBox>
<asp:RegularExpressionValidator ID="RegRNDDLicSSN" runat="server" ControlToValidate="txtRNNO" Display="None" 
                                    ErrorMessage="Please enter a valid Liscence Number" ValidationExpression="^[0-9]*$" ValidationGroup="a">
                                </asp:RegularExpressionValidator>
</td></tr>

<tr><td align="right">
<label>First Name :</label></td><td>
<asp:TextBox id="txtFirstName" runat="server" style="float:left" ></asp:TextBox>
<asp:RegularExpressionValidator ID="RegFirstName" runat="server" ControlToValidate="txtFirstName" Display="None" 
                                    ErrorMessage="Please enter only alphabets" ValidationExpression="^[a-zA-Z]+$" ValidationGroup="a">
                                </asp:RegularExpressionValidator>
</td></tr>

<tr><td align="right">
<label>Last Name :</label></td><td>
<asp:TextBox id="txtLName" runat="server" style="float:left" ></asp:TextBox>
<asp:RegularExpressionValidator ID="RegLastName" runat="server" ControlToValidate="txtLName" Display="None" 
                                    ErrorMessage="Please enter only alphabets" ValidationExpression="^[a-zA-Z]+$" ValidationGroup="a">
                                </asp:RegularExpressionValidator>
</td></tr>
<tr><td align="right">
<label>Session Start Date :</label></td><td>
<asp:TextBox id="txtSearchSessionStartDate" runat="server" CssClass="date-pick txtSearchSessionStartDate" style="float:left" ></asp:TextBox>
<asp:CompareValidator Type="Date" Operator="DataTypeCheck" runat="server" ID="CompDate" controltovalidate="txtSearchSessionStartDate" validationgroup="a"/>


<br /></td></tr></table>

</center>  

</asp:Panel>

    </div>
    <br />
    <asp:Button ID="bntSearch" CssClass="bntSearch" runat="server" Text="Search" /> 
    &nbsp; 
    <asp:Button id="bntAddNewCourse" runat="server" Text="Add Course" />
    <div id="divSpinner" runat="server" class="divSpinner" style="font-family:Tahoma, Arial, Verdana; font-size:24pt; font-weight:bold; text-align: center;">
            <br /><br />
        </div>

    <div id="dvSearchGrid" runat="server">
        <asp:GridView ID="grdCourse" runat="server" AutoGenerateColumns="False" CssClass="Grid"
    DataKeyNames="OBNApprovalNumber" OnRowDataBound = "OnRowDataBound">
    <Columns>
        <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle" ItemStyle-BorderStyle="NotSet">
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
                            <asp:BoundField DataField="SessionAddressInfo.County" HeaderText="County" HeaderStyle-BackColor="#BFE4FF" /> 
                            <asp:BoundField DataField="PublicAccessYesNo" HeaderText="Open to the Public" HeaderStyle-BackColor="#BFE4FF" />
                                 <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnDeleteSession" runat="server"  CommandName="DeleteSession" CommandArgument='<%# Eval("Session_Sid")%>' Text = "Delete" CssClass="lnkBtnDeleteSession" Visible = '<%# Eval("AllowSessionDelete")%>' OnClientClick="if (!confirm('You are about to delete this session. Are you sure you want to delete?')) return false;" />
                                </ItemTemplate>

<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></ItemStyle>
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
        <asp:TemplateField HeaderText="Syllabus" HeaderStyle-BackColor="#BFE4FF">
            <ItemTemplate>
                
                <asp:Repeater ID="rpSyllabus" runat="server">
                    <HeaderTemplate>
                        <table>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <tr>
                            <td>
                                <asp:HyperLink ID="HyperLink1" runat="server" Target="_new"  NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "DownloadURL")%>'  Text='<%# DataBinder.Eval(Container.DataItem, "fileName")%>' ImageUrl="~/Images/syllabus.png"></asp:HyperLink>
                            </td>
                        </tr>


                    </ItemTemplate>
                    <FooterTemplate>
                        </table>
                    </FooterTemplate>
                </asp:Repeater>
                
            </ItemTemplate>
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:TemplateField>
        <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnAddSession" runat="server"  CommandName="AddSession" CommandArgument='<%# Eval("Course_Sid")%>' Text = "Add Session" CssClass="lnkBtnAddSession" Visible = '<%# Eval("Course_Sid")%>' />
                                </ItemTemplate>

<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></ItemStyle>
                            </asp:TemplateField>
        <%--<asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
            <ItemTemplate>
                <asp:LinkButton ID="lnkBtnRemove" runat="server" CommandName="Delete" Text = "Remove" CssClass="lnkBtnSelect" Visible = "true" OnClick="lnkBtnSelect_Click" />
            </ItemTemplate>

<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></ItemStyle>
        </asp:TemplateField>--%> 
    </Columns>
     <EmptyDataTemplate>
         <asp:Label ID="Label10" runat="server" Text="No course or session were found."></asp:Label>
         <br />
         <%--<asp:LinkButton ID="lkbAddCourse" runat="server" OnClick="lkbAddCourse_Click">Add Session to Application</asp:LinkButton>--%>
     </EmptyDataTemplate>
</asp:GridView>

    </div>
 </div>
    <asp:HiddenField ID="htMode" ClientIDMode="Static" runat="server" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
   
     
</asp:Content>
