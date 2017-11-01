<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="ApplicationHistoryReport.aspx.vb" Inherits="MAIS.Web.ApplicationHistoryReport" EnableEventValidation="False" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="Scripts/ApplicationHistoryReport.js" type="text/javascript"></script>
     <script type="text/javascript" src="Scripts/spin.min.js"></script> 
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
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <asp:ScriptManager id="ScriptManager1" runat="server">
        </asp:ScriptManager>
       <script type="text/javascript" language="javascript">

           Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);

           function EndRequestHandler(sender, args) {

               if (args.get_error() != undefined) {

                   args.set_errorHandled(true);

               }

           }

</script>
     <center><table class="CountySelection" style="width: 100%;">
                        <tr>
                            <td>
                                <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Application History Report Page"></asp:Label>
                            </td>
                        </tr>
                    </table></center>
    <asp:Panel ID= "pnlapplication" runat="server">
<asp:ValidationSummary ID="searchValidationSummary" ValidationGroup="searchpage" runat="server" CssClass="ErrorSummary errMsg" />
<br /><br />
         <input type="hidden" id ="hdApplicationStatus" runat ="server" class ="hdApplicationStatus" />
         <input type="hidden" id ="hdRNName" runat ="server" class ="hdRNName" />
         <input type="hidden" id ="hdAppType" runat ="server" class ="hdAppType" />
     <table width="100%">
                <tr align="center">
                 <td>                     
                 <asp:Panel ID="Panel2"  GroupingText="Please choose one" runat="server" Width="80%" Font-Size="9pt" Font-Italic="false">                 
                    <table width="100%">
                        <tr align="center">
                            <td >
                                <asp:RadioButtonList ID="rblSelect" CssClass="rblSelect" runat="server" 
                                    RepeatDirection="Horizontal" ValidationGroup = "searchpage" CausesValidation="True" 
                                        BorderStyle="Dotted" BorderWidth="2px">
                                    <asp:ListItem Value="0" Text="RN" Selected="True"></asp:ListItem>
                                    <asp:ListItem Value="1" Text="DD Personnel"></asp:ListItem>
                               </asp:RadioButtonList>                              
                            </td>
                        </tr>
                    </table> 
                </asp:Panel>
                </td>
                </tr>
                <tr align="center">               
                <td>
                        <asp:Panel ID="pnlSearch" GroupingText="Search Options" CssClass="pnlSearch"  runat="server" Width="80%" Font-Size="9pt" Font-Italic="false">
                    <table width="100%">
                        <tr>
                            <td align="left"  class ="tdMandatory">                            
                                    <label id="lblRNDDLicenseSSN" class="lblRNDDLicenseSSN" style="font-size:9pt; font-style:normal;"></label>    
                            </td>
                            <td align="left">
                                    <input type = "text" id="txtRNDDLicSSN" class="txtRNDDLicSSN" style="Width:300px;" runat = "server" maxlength="8" /> 
                            </td>
                        </tr>
                         <tr>
                            <td align="left"></td>
                            <td align="left">                                                      
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                    <label id="lblStartDate" class="lblStartDate" style="font-size:9pt; font-style:normal;">Start Date</label>
                            </td>
                            <td align="left">
                                    <input type = "text" id="txtStartDate" class="date-pick txtStartDate" style="Width:300px;" runat = "server"/>                                    
                            </td>
                        </tr>
                         <tr>
                            <td align="left"></td>
                            <td align="left">                                                      
                            </td>
                        </tr>
                        <tr>
                            <td align="left">
                                    <label id="lblEndDate" class="lblEndDate" style="font-size:9pt; font-style:normal;">End Date</label>
                            </td>
                            <td align="left">
                                    <input type = "text" id="txtEndDate" class="date-pick txtEndDate" style="Width:300px;" runat = "server"/>                                    
                            </td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                            <td align="left">                                                      
                            </td>
                        </tr>
                        <tr style="table-layout: inherit; border-spacing: 0px; margin: auto; padding: 0px">
                            <td align="left">
                                    <label id="lblApplicationType" style="font-size:9pt; font-style:normal;">Application Type:</label>                                   
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlAppType" runat="server" CssClass="ddlAppType" style="Width:300px;">
                                </asp:DropDownList>                 
                            </td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                            <td align="left">                                                      
                            </td>
                        </tr>
                        <tr style="table-layout: inherit; border-spacing: 0px; margin: auto; padding: 0px">
                            <td align="left">
                                    <label id="lblStatus" style="font-size:9pt; font-style:normal;">Application Status:</label>                                   
                            </td>
                            <td align="left">
                                <asp:DropDownList ID="ddlStatus" runat="server" CssClass="ddlStatus" style="Width:300px;">
                                </asp:DropDownList>                 
                            </td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                            <td align="left">                                                      
                            </td>
                        </tr>
                        <tr runat ="server" id ="tr1">
                            <td align="left">
                                    <label id="lblApplicationDone" style="font-size:9pt; font-style:normal;">Application Done By RN:</label>                                   
                            </td>
                            <td align="left">
                                    <asp:DropDownList ID="ddlRNName" runat="server" CssClass="ddlRNName" style="Width:300px;">
                                </asp:DropDownList>                                     
                            </td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                            <td align="left">                                                      
                            </td>
                        </tr>
                        <tr runat ="server" id ="trDDCode" class ="trDDCode">
                            <td align="left">
                                    <label id="lblDDCode" style="font-size:9pt; font-style:normal;">DDPersonnel Code:</label>                                   
                            </td>
                            <td align="left">
                                    <input type = "text" id="txtDDPersoonelCode" class="txtDDPersoonelCode" style="Width:300px;" runat = "server"/>                                    
                            </td>
                        </tr>
                        <tr>
                            <td align="left"></td>
                            <td align="left">                                                      
                            </td>
                        </tr>
                    </table>
         </asp:Panel>
                </td></tr>
            </table> <br />
        <asp:UpdatePanel id="UpdatePanel1" runat="server">
        <ContentTemplate>
             <script type ="text/javascript" language ="javascript">
                 Sys.Application.add_load(buttonClick);
            </script>
    <asp:Button ID="btnSearch" runat="server" cssClass="btnSearch" Text="Search" /> &nbsp;&nbsp;
            <input type="submit" id="btnExport" runat="server" value="Export to Excel"  /><br /><br />
        <div id="divSpinner" runat="server" class="divSpinner" style="font-family:Tahoma, Arial, Verdana; font-size:24pt; font-weight:bold; text-align: center;"></div>
        <div style="float:left">
        <asp:Label ID ="lblmessage" runat ="server" Font-Bold="True" style="text-align:left; " CssClass="alertmessage lblmessage"></asp:Label></div>
    <div id="divCourse" runat="server">
 <asp:GridView ID="grdApplicationDetail" runat="server" AutoGenerateColumns="False" CssClass="grdApplicationDetail"
    DataKeyNames="ApplicationID">
    <Columns>
        <asp:TemplateField ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" ItemStyle-VerticalAlign="Middle">
           <ItemTemplate>
                <img alt = "" style="cursor: pointer" src="images/plus.png" />
                <asp:Panel ID="pnlOrders" runat="server" Style="display: none">
                    <asp:GridView ID="grdApplicationStatusDetail" runat="server" AutoGenerateColumns="false" CssClass = "grdApplicationStatusDetail">
                        <Columns>
                            <asp:BoundField DataField="ApplicationStatus" HeaderText="Application Status" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="ApplicationLatestUpdatedDate" HeaderText="Application Status EndDate" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}"/>
                            <asp:BoundField DataField="RNName" HeaderText="Application Done By" HeaderStyle-BackColor="#BFE4FF"/>
                            <asp:BoundField DataField="UserRole" HeaderText="Role" HeaderStyle-BackColor="#BFE4FF" />
                        </Columns>
                    </asp:GridView>
                </asp:Panel>
            </ItemTemplate>
<ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="10%"></ItemStyle>
        </asp:TemplateField>        
        <asp:BoundField DataField="ApplicationID" HeaderText="Application ID" HeaderStyle-BackColor="#BFE4FF" >
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
          <asp:BoundField DataField="UniqueCodeOrLicense" HeaderText="RNLicense Or DDPersonnelCode" HeaderStyle-BackColor="#BFE4FF" >
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
         <asp:BoundField DataField="ApplicationType" HeaderText="Application Type" HeaderStyle-BackColor="#BFE4FF" >
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="FinalApplicationStatus" HeaderText="Final Application Status" HeaderStyle-BackColor="#BFE4FF" >
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="TrainingEndDate" HeaderText="Training End Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="SkillsEndDate" HeaderText="Skills Latest Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="AttestationDate" HeaderText="Attestation Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="DecisionMadeRNName" HeaderText="Descision Made By RN" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="FinalDecisionName" HeaderText="Final Decision Made By" HeaderStyle-BackColor="#BFE4FF">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="EmailEndDate" HeaderText="Email Sent Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
        <asp:BoundField DataField="CertificatePrintDate" HeaderText="Certificate Print Date" HeaderStyle-BackColor="#BFE4FF" DataFormatString="{0:MM/dd/yyyy}">
<HeaderStyle BackColor="#BFE4FF"></HeaderStyle>
        </asp:BoundField>
    </Columns>
</asp:GridView>
    </div>
             </ContentTemplate>      
 </asp:UpdatePanel>
        </asp:Panel>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
