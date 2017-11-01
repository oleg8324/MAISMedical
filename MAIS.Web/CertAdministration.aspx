<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="CertAdministration.aspx.vb" Inherits="MAIS.Web.CertAdministration" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91"
    Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/Notation.js" type="text/javascript"></script>
    <script src="Scripts/CertAdministration.js" type="text/javascript"></script>
    <script type="text/javascript" src="Scripts/spin.min.js"></script> 
    <link href="Scripts/jquery/redmond.datepick.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">

</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
                <center>
        <table class="CountySelection" style="width: 100%;">
                        <tr>
                            <td style="text-align:center;width:70%;">
                               &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                <asp:Label  ID="Label13" runat="server" Font-Bold="True" Text="Certification Administration Page"></asp:Label>     </td>                   
                           <td style="text-align:right;width:30%;">
                                <asp:HyperLink ID="LBackExisting" runat="server" NavigateUrl="UpdateExistingPage.aspx" Text="Go Back To Update Existing"></asp:HyperLink>
                             </td>
                        </tr>
                    </table></center><br />
     <asp:ScriptManager id="ScriptManager1" runat="server" EnablePartialRendering="true">
        </asp:ScriptManager>
       <script type="text/javascript" language="javascript">

           Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

           function EndRequestHandler(sender, args) {

               if (args.get_error() != undefined) {

                   args.set_errorHandled(true);

               }

           }

</script>
    <div>
        <center>
    <asp:ValidationSummary ID="valsum1" runat="server" CssClass="ErrorSummary" ValidationGroup="a" />
        <br />
       </center>
        <center>     
        <div id="pError" runat="server" class="ErrorSummary" visible="false"></div>
        </center>
        <center>
        <div id="pnote" runat="server" class="Note"></div>
        <br /><br />
        </center>
        <asp:Panel ID="pnlSearch"  GroupingText="Please choose one of the actions" runat="server" Width="100%" Font-Size="9pt" Font-Italic="false">
                    <center>     <asp:RadioButtonList ID="rblSelect" CssClass="rblSelect" runat="server" ValidationGroup = "searchpage" BorderStyle="Dotted" BorderWidth="2px"   AutoPostBack="true" RepeatLayout="Table" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="1" Text="Change Certificate Status"></asp:ListItem>
                                    <asp:ListItem Value="2" Text="Change Certificate Start Date"></asp:ListItem>
                                    <asp:ListItem Value="3" Text="Change Training Session"> </asp:ListItem>
                               </asp:RadioButtonList> </center>
    </asp:Panel>
            <br />
       <asp:Panel ID="PCert" CssClass="PCert" runat="server" HorizontalAlign="Left" Width="100%" Font-Size="9pt" Font-Italic="false">
                <table width="100%">
    <tr>
         <td class="BlueColumnHead">Role</td>
         <td class="BlueColumnHead">Category</td>
         <td class="BlueColumnHead">Level</td>
         <td class="BlueColumnHead">Select Status</td>
         <td class="BlueColumnHead" >Start Date</td>
         <td class="BlueColumnHead" >End Date</td>
         
    </tr>
            <tr>
               
        <td valign="top"> <asp:Label ID="lblRole" runat="server"></asp:Label></td>
        <td valign="top"> <asp:Label ID="lblCategory" runat="server"></asp:Label></td>
        <td valign="top"><asp:Label ID="lblLevel" runat="server"></asp:Label></td> 
        <td valign="top"><asp:DropDownList ID="ddCertStatus" AppendDataBoundItems="false" runat="server" AutoPostBack="true" >
        <asp:ListItem Value="0">Select Status</asp:ListItem>
    </asp:DropDownList></td> 
        <td valign="top">
        <input type = "text" id ="txtStDate" class="date-pick txtStDate" runat = "server" style="width:70px"/> 
        </td>
        <td>
            <input type = "text" id ="txtEDate" class="date-pick txtEDate" disabled="disabled" runat = "server" style="width:70px"/>
        </td>
                            </tr> </table>

            </asp:Panel>
        <br />
        <asp:Panel ID="pnlTrainingSession" runat="server" Visible="false" >      
            <div>
                <h4>Current Training Session</h4>
                <asp:GridView ID="gvSearchData" runat="server" CssClass="gridview" AutoGenerateColumns="False" DataKeyNames="Session_Sid">
                    <Columns>
                        <asp:BoundField DataField="Session_Sid" HeaderText="Session ID" />
                        <asp:BoundField DataField="RN_Name" HeaderText="RN Name" />
                        <asp:BoundField DataField="CourseNumber" HeaderText="Course #" />
                        <asp:BoundField DataField="CourseDescription" HeaderText="Course Description" />
                        <asp:BoundField DataField="StartDate" HeaderText="Session Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="TotalCEs" HeaderText="Total CEs" />
                        <asp:BoundField DataField="SessionLocation" HeaderText="Location" />
                    </Columns>
                    <EmptyDataTemplate>
                        <strong>No Training Session found. </strong>
                    </EmptyDataTemplate>
                    <HeaderStyle CssClass="gridviewHeader" />
                </asp:GridView>
            </div>  
            <div>
                <h4>Replace Session with One below</h4>
                <asp:GridView ID="gvSessionReplace" runat="server" CssClass="gridview" AutoGenerateColumns="False" DataKeyNames="Session_Sid">
                    <Columns>
                        <asp:CommandField HeaderText="Select One To Replace With" ShowSelectButton="True" />
                        <asp:BoundField DataField="Session_Sid" HeaderText="Session ID" />
                        <asp:BoundField DataField="RN_Name" HeaderText="RN Name" />
                        <asp:BoundField DataField="CourseNumber" HeaderText="Course #" />
                        <asp:BoundField DataField="CourseDescription" HeaderText="Course Description" />
                        <asp:BoundField DataField="StartDate" HeaderText="Session Start Date" DataFormatString="{0:MM/dd/yyyy}" />
                        <asp:BoundField DataField="TotalCEs" HeaderText="Total CEs" />
                        <asp:BoundField DataField="SessionLocation" HeaderText="Location" />
                    </Columns>
                    <EmptyDataTemplate>
                        <strong>No Training Session found. </strong>
                    </EmptyDataTemplate>
                     <SelectedRowStyle BackColor="PowderBlue" Font-Bold="True" />
                    <HeaderStyle CssClass="gridviewHeader" />
                </asp:GridView>
            </div>
            <br />
            <div style="text-align: right">
                <asp:Button ID="bntSaveSession" runat="server" Text="Save" Enabled="False" />
                &nbsp
                <%--<asp:Button ID="bntCancelSession" runat="server" Text="Cancel" />--%>
            </div>
        </asp:Panel>
            <br />
        <div style="text-align:right; display:none;">
            &nbsp;<input type="submit" id="btnSave" runat="server" value="Save" causesvalidation="false" />   
        </div>
        <div ID="dhLb" runat="server" style="display:none">
        <asp:ListBox ID= "lbHReasons" runat="server" SelectionMode="Multiple" Rows="8">
    
        </asp:ListBox>

        </div>

            <br />   

    <asp:Panel ID="PEnterNotation" class="PEnterNotation"  GroupingText="Enter Notation Information" runat="server" Width="100%" Font-Size="9pt" Font-Italic="false" HorizontalAlign="Left">
         <asp:UpdatePanel id="UpdatePanel1" runat="server" OnPreRender="upPreRender">

                    <ContentTemplate>
                        <script type="text/javascript" src="Scripts/jquery/jquery.datepick.min.js"></script>
<%--                        <script type="text/javascript">
                            function pageLoad() {
                                $('[id$=txtDtOccurance]').datepick();
                                $('[id$=txtStDate]').datepick();
                                $("[id$=btnSave]").click(function (e) {
                                    if (!confirm('Are you sure you want to submit the application with the selected status?')) { e.preventDefault(); return; }
                                });
                                $("[id$=txtStDate]").change(function (e) {
                                    var tmpdate = IsDate($.trim($("[id$=txtStDate]").val()));
                                    if (tmpdate != "") {
                                    } else {
                                        var startDate = new Date($("[id$=txtStDate]").val());
                                        var EndDate = new Date("12/31/9999");
                                        if ($("[id$=hIsPersonDD]").val()=="1") {
                                            var dd1 = new Date(startDate.getFullYear() + 1, startDate.getMonth(), startDate.getDate());
                                            EndDate = new Date(dd1.getFullYear(), dd1.getMonth(), dd1.getDate() - 1);
                                            $("[id$=txtEDate]").val((EndDate.getMonth() + 1) + '/' + EndDate.getDate() + '/' + EndDate.getFullYear());
                                        }
                                    }
                                });
                            }
                        </script> --%>
        <input type="hidden" id="hNotId" runat="server" value="0" />
        <input type="hidden" id="hStatus" runat="server" value="" />
        <input type="hidden" id="hIsPersonDD" runat="server" value="1" />
    <table width="100%">
    <tr>
         <td class="BlueColumnHead" style="width: 14%">Notation Date</td>
         <td class="BlueColumnHead" style="width: 14%">Notation Type</td>
         <td class="BlueColumnHead" style="width: 14%">Person <br /> entering notation</td>
         <td class="BlueColumnHead" style="width: 14%">Title</td>
         <td class="BlueColumnHead" style="width: 14%">Date Of <br /> Occurence</td>
         <td class="BlueColumnHead" style="width: 14%">Notation Reason</td>
         <td class="BlueColumnHead" style="width: 20%">Unflagged <br />date</td>
    </tr>

            <tr>
               
        <td valign="top"> <asp:Label ID="lblDt" runat="server"></asp:Label></td>
        <td valign="top">
        <asp:DropDownList ID="ddNotationType" AppendDataBoundItems="true" runat="server" OnSelectedIndexChanged="NotTypeChanged" AutoPostBack="true">
        <asp:ListItem Value="0">Select Reason</asp:ListItem>
    </asp:DropDownList>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="ddNotationType" InitialValue="0" Display="None" validationgroup="a"
            ErrorMessage="Please select Notation Type"></asp:RequiredFieldValidator>
        </td>
        <td valign="top"><input type = "text" id ="txtPerson" disabled="disabled" value="PersonA" style="Width:100px;" runat = "server"/></td> 
        <td valign="top"><input type = "text" id ="txtPersonTitle" disabled="disabled" value="RN" style="Width:100px;" runat = "server"/></td> 
        <td valign="top">
        <input type = "text" id ="txtDtOccurance" class="txtDtOccurance" runat = "server" style="width:70px"/> 
            <asp:CompareValidator Type="Date" Operator="DataTypeCheck" runat="server" ID="CompOcDate" Display="None" controltovalidate="txtDtOccurance" validationgroup="a"/>
            <asp:RequiredFieldValidator ID="reqval1" runat="server" ErrorMessage="Occurence Date is Required" Display="None" ControlToValidate="txtDtOccurance" validationgroup="a"></asp:RequiredFieldValidator>
        </td>
        <td>

    <%--<asp:ListBox ID= "lbReason" runat="server" SelectionMode="Multiple" Rows="8" Width="160px"></asp:ListBox>--%>
            <asp:CheckBoxList ID="cklReason" runat="server" CssClass="cklReason"></asp:CheckBoxList>
            <asp:CustomValidator runat="server" ID="cvReason" Display="None" ErrorMessage="Please select at least one Notation Reason" ValidationGroup="a" ClientValidationFunction="CheckSelected" OnServerValidate="ServerValidate"></asp:CustomValidator>
            <%--<asp:RequiredFieldValidator ID ="rvCBReasons" runat="server" ControlToValidate="cklReason" Display="None" validationgroup="a" ErrorMessage="Please select Notation Reason"></asp:RequiredFieldValidator>--%>
        </td>
        <td valign="top">
            <asp:CheckBox ID="chbUnflag" runat="server"/> <br /> <input type = "text" id ="txtUFDate" class="txtUFDate" disabled="disabled" style="Width:60px;" runat = "server"/> 
            <input type="hidden" id="hdUFDate" runat="server" value="" />
        </td>
                            </tr> </table>
                   

                         </ContentTemplate>
                 <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="ddNotationType" EventName="SelectedIndexChanged" />
                    <asp:AsyncPostBackTrigger ControlID="btnShowAddNotation" EventName="ServerClick" />
                    <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="ServerClick" />

                </Triggers>
                    </asp:UpdatePanel>
       
             <asp:Panel ID="pnlUpload" runat="server" GroupingText="Upload" >
                        <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Choose File to Upload:" />&nbsp;
                                        <asp:FileUpload ID="uplNotationFile" runat="server" Width="400px" BackColor="White" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center">
                                       
                                       <input type="button" id="btnUpload" runat="server" value="Upload"  causesvalidation="false" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center"  width="100%">
                                      <%--  <asp:Label ID="lblErrorLabel" runat="server" ForeColor="Red"></asp:Label>--%>
                                    </td>
                                </tr>
                               
                            </table></asp:Panel>
            
                                        
                                            <asp:Panel ID="Panel96" runat="server" GroupingText="Documents To Upload" 
                                                BorderColor="Black"  >
                                                <br />
                                                <asp:Panel ID="pUploadDocs" runat="server" Width="100%" ScrollBars="Auto" >
                                                    <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="False"
                                                        AutoGenerateDeleteButton="True" DataKeyNames="ImageSID" BackColor="#EFF3FB" BorderColor="Black"
                                                        BorderStyle="Solid" BorderWidth="1px" Font-Names="Arial" Font-Size="Small" Width="100%">
                                                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                        <Columns>
                                                            <asp:CommandField SelectText="View" ShowCancelButton="False" ShowSelectButton="True" />
                                                            <asp:BoundField DataField="DocumentName" HeaderText="FileName" />
                                                            <asp:BoundField DataField="DocumentType" HeaderText="Requirement" />

                                                        </Columns>
                                                        <AlternatingRowStyle BackColor="White" />
                                                    </asp:GridView>
                                                </asp:Panel>
                                            </asp:Panel>
                                        <asp:Panel ID="Panel1" runat="server" GroupingText="Documents Uploaded To UDS" 
                                                                                BorderColor="Black"  >
                                             <asp:GridView ID="gvUdsFiles" runat="server" AutoGenerateColumns="False"
                                                        AutoGenerateDeleteButton="false" BackColor="#EFF3FB" BorderColor="Black"
                                                        BorderStyle="Solid" BorderWidth="1px" Font-Names="Arial" Font-Size="Small" Width="100%">
                                                        <RowStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                        <Columns>
                                                            <%--<asp:CommandField SelectText="View" ShowCancelButton="False" ShowSelectButton="True" />
                                                            <asp:BoundField DataField="DocumentName" HeaderText="FileName" />
                                                            <asp:BoundField DataField="DocumentType" HeaderText="Requirement" />--%>
                                                            <asp:TemplateField HeaderText="File Name"> 
                                                                <ItemTemplate>
                                                                    <asp:HyperLink ID="HyperLink1" runat="server" Target="_new"  NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "DownloadURL")%>'  Text='<%# DataBinder.Eval(Container.DataItem, "fileName")%>'></asp:HyperLink>
                                                                </ItemTemplate>

                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <AlternatingRowStyle BackColor="White" />
                                                    </asp:GridView>
                                            
                                           

                                            </asp:Panel>


    </asp:Panel>
                    <div id="dvSaveNotationButtons" runat="server" style="text-align:right;">
        <input type="submit" id="btnSaveNotation" class="btnSaveNotation" causesvalidation="true" runat="server" value="Save" validationgroup="a" />
            &nbsp;<input type="button" id="btnCancel" causesvalidation="false" runat="server" value="Cancel" onclick ="$('.PEnterNotation').hide(); $('.ErrorSummary').hide(); $('[id$=txtDtOccurance]').val('');"  />
            </div>

        <div id="divSpinner" runat="server" class="divSpinner" style="font-family:Tahoma, Arial, Verdana; font-size:24pt; font-weight:bold; text-align: center;">
            <br /><br />
        </div> 
        <div id="dNotButtons" runat="server"> &nbsp;
        <div style="text-align:right; display:none">
            &nbsp;<input type="button" id="btnShowAddNotation" runat="server" value="Add new Notation" onserverclick="btnAddClicked" causesvalidation="false" />   
        </div>
            <br />
        </div>


<br />
    <asp:GridView ID="grdNotations" runat="server" DataKeyNames="AppNotId" OnRowCommand="EditCmd" Width="100%" AutoGenerateColumns="false" CssClass="Grid">
                            <Columns>
                                <asp:BoundField DataField="AppNotId" Visible="false" HeaderText="Notation Type" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:TemplateField  HeaderText="Notation Type" ItemStyle-HorizontalAlign="Left" HeaderStyle-BackColor="#BFE4FF">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkEdit" runat="server" CommandName ="mycmd" Text='<%# Eval("NotationType.NTypeDesc") %>' CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>'></asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>

                            <asp:BoundField DataField="AllReasons" HeaderText="Notation Reason" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="PersonEnteringNotation" HeaderText="Person Entering Notation" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="PersonTitle" HeaderText="Person Title" HeaderStyle-BackColor="#BFE4FF"/>
                                <asp:BoundField DataField="NotationDate" HeaderText="Notation Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                                <asp:BoundField  DataField="OccurenceDate" HeaderText="Occurence Date" ItemStyle-HorizontalAlign="Left" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                                   
                                <asp:BoundField DataField="UnflaggedDate" HeaderText="Unflagged Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                            </Columns>
        <EmptyDataTemplate>
         <asp:Label ID="Label10" runat="server" Text="There are no Notations to display."></asp:Label>
     </EmptyDataTemplate>
</asp:GridView>
</center>
</div>
<br />

   
    <%--<asp:Panel ID="PDates" class="PDates"  GroupingText="Certification Dates" runat="server" HorizontalAlign="Left" Width="100%" Font-Size="9pt" Font-Italic="false">
    &nbsp;&nbsp;Start Date <input type = "text" id ="txtStartDate" class="date-pick txtDtOccurance" style="Width:100px;" runat = "server"/>&nbsp;&nbsp;&nbsp;&nbsp; End Date
    <input type = "text" disabled="disabled" id ="txtEndDate" class="date-pick txtDtOccurance" style="Width:100px;" runat = "server"/>
         </asp:Panel>
    <br />
    <asp:Panel ID="PStatus" HorizontalAlign="Left" class="PStatus"  GroupingText="" runat="server" Width="100%" Font-Size="9pt" Font-Italic="false">
    <strong>Select Certification Status:</strong>&nbsp;<asp:DropDownList ID="ddlStatus" runat="server" AppendDataBoundItems="true">
        <asp:ListItem Value="0">Select Status</asp:ListItem>
    </asp:DropDownList>
       </asp:Panel>--%>

    <br />
<%--<div class="NavigationMenu">
   <input type="button" id="btnPrevious" runat="server" class="ButtonStyle" value="Previous" onclick ="location.href = 'Summary.aspx'" />&nbsp;&nbsp;&nbsp;
    <input type="button" id="btnSave" runat="server" class="ButtonStyle" value="Save" style="width: 87px"/>&nbsp;&nbsp;&nbsp;
    <input type="submit" id="btnNext" runat="server" class="btnContinue" value="Save and Continue" onclick ="location.href = 'ViewCertificate.aspx';" />
</div>--%>
  <rsweb:ReportViewer ID="rvCertificate" runat="server" Width="100%" Font-Names="Verdana"
        Font-Size="8pt" InteractiveDeviceInfos="(Collection)" WaitMessageFont-Names="Verdana"
        WaitMessageFont-Size="14pt" ShowBackButton="False" ShowExportControls="False"
        ShowFindControls="False" ShowPageNavigationControls="False" ShowPrintButton="False"
        ShowRefreshButton="False" ShowZoomControl="False" Visible="False">
    </rsweb:ReportViewer>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
