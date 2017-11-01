<%@ Page Title="Notations" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="Notation.aspx.vb" Inherits="MAIS.Web.Notation" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/Notation.js" type="text/javascript"></script>
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
                                <asp:Label  ID="Label13" runat="server" Font-Bold="True" Text="Notation Page"></asp:Label>     </td>                   
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
        
        <div ID="dhLb" runat="server" style="padding: 0px; margin: 0px; display:none">
        <asp:ListBox ID= "lbHReasons" runat="server" SelectionMode="Multiple" Rows="8">
        </asp:ListBox>
        </div>

    <asp:Panel ID="PEnterNotation" class="PEnterNotation"  GroupingText="Enter Notation Information" runat="server" Width="100%" Font-Size="9pt" Font-Italic="false" HorizontalAlign="Left">
         <asp:UpdatePanel id="UpdatePanel1" runat="server" OnPreRender="upPreRender">

                    <ContentTemplate>
                        <script type="text/javascript" src="Scripts/jquery/jquery.datepick.min.js"></script>

        <input type="hidden" id="hNotId" runat="server" value="0" />
        <input type="hidden" id="hStatus" runat="server" value="" />
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
               
                        <%-- <table width="100%">
                             <tr>--%>
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
                                <%--<tr><td>&nbsp;</td></tr>--%>
                               
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

        <div style="text-align:right;">
        <input type="submit" id="btnSaveNotation" class="btnSaveNotation" causesvalidation="true" runat="server" value="Save" validationgroup="a" />
            &nbsp;<input type="button" id="btnCancel" causesvalidation="false" runat="server" value="Cancel" onclick ="$('.PEnterNotation').hide(); $('.ErrorSummary').hide();"  />
            </div>
    </asp:Panel>
        <div id="divSpinner" runat="server" class="divSpinner" style="font-family:Tahoma, Arial, Verdana; font-size:24pt; font-weight:bold; text-align: center;">
            <br /><br />
        </div> 
        <div id="dNotButtons" runat="server"> &nbsp;
        <div style="text-align:right;">
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
  
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">

</asp:Content>
