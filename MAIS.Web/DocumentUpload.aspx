<%@ Page Title="Document Upload" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="DocumentUpload.aspx.vb" Inherits="MAIS.Web.DocumentUpload" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
 <%--<script type = "text/javascript">
     $(document).ready(function () {
         formVisible($('#fileUpload'), false)
         $('input:checkbox').live('click', function (event) {
             if ($(this).is(':checked')) {
                 formVisible($('#fileUpload'), false)
             }
         });
          });
 </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <asp:ValidationSummary ID="valsum1" runat="server" CssClass="ErrorSummary" ValidationGroup="a" />
    <div id="pError" runat="server" class="ErrorSummary" visible="false"></div>
    <div style="text-align:left;" >
    <asp:Panel ID="pDocumentType" runat="server" GroupingText="Select Document Type" 
        BorderColor="Maroon"    
        HorizontalAlign="Left"><div style="text-align:left;" >
    <asp:RadioButtonList id="rdbDocTypes" runat="server" BorderStyle="None" 
         RepeatDirection = "Vertical" CssClass = "rdbInitial">
            <asp:ListItem Value="1" Text="Application" Selected="True">Application<span style="color: #FF0000">*</span></asp:ListItem>
<%--            <asp:ListItem Value="2" Text="Security Disclosure Form">Security Disclosure Form<span style="color: #FF0000">*</span></asp:ListItem>
            <asp:ListItem Value="3" Text="Other"></asp:ListItem>--%>
</asp:RadioButtonList></div>
        <asp:RequiredFieldValidator ID="ReqdVal1" runat="server" ControlToValidate="rdbDocTypes" Display="None" validationgroup="a"
            ErrorMessage="Please select a Document Type"></asp:RequiredFieldValidator>
    </asp:Panel>
<%--<table cellspacing="0" style="width:100%">
                                <tr>
    <td>
    <asp:Panel ID="pnlProgressBar" runat="server" GroupingText="Required Documents" 
        BorderColor="Maroon" BorderStyle="Solid"  Width = "450px"
        HorizontalAlign="Left">
        <input type = "checkbox" id = "chkApplication" runat = "server"/>APPLICATIONS
        <br />
        <input type = "checkbox" id = "chkNotationSupporting" runat = "server"/>NOTATION SUPPORTING DOCUMENTS<br />
        <input type = "checkbox" id = "chkSyllabus" runat = "server"/>SYLLABUS
    </asp:Panel>
    </td>
    <td>
    <asp:Panel ID="Panel2" runat="server" GroupingText="Optional Documents" 
        BorderColor="Maroon" BorderStyle="Solid"   Width = "450px"
        HorizontalAlign="Left">
        <input type = "checkbox" id = "chkSecurity" runat = "server"/>Security Affidavit<br />
        <input type = "checkbox" id = "chkOther" runat = "server"/>Other<br />
    </asp:Panel>
    </td>
    </tr>
    </table>--%>
    
        <asp:Panel ID="pnlUpload" runat="server" GroupingText="Upload" ><center>
                        <table>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label1" runat="server" Text="Choose File to Upload:" />&nbsp;
                                        <asp:FileUpload ID="uplFile" runat="server" Width="400px" BackColor="White" />
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align:center">
                                       
                                        <input type="submit" id="btnUpload" runat="server" value="Upload"  validationgroup="a" causesvalidation="true" /> 
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" colspan="2" width="60%">
                                        <asp:Label ID="lblErrorLabel" runat="server" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                                <%--<tr><td>&nbsp;</td></tr>--%>
                                <tr>
                                    <td colspan="2">
                                       
                                    </td>
                                </tr>
                            </table></center></asp:Panel>
                          
         <center>
                                            <asp:Panel ID="Panel96" runat="server" GroupingText="Documents Uploaded" BorderStyle="Solid"
                                                BorderColor="Black" BorderWidth="1" >
                                                <br />
                                                <asp:Panel ID="Panel1" runat="server" Width="95%" Height="175px" ScrollBars="Auto"
                                                    >
                                                    <asp:GridView ID="gvFiles" runat="server" AutoGenerateColumns="False"
                                                        AutoGenerateDeleteButton="True" DataKeyNames="ImageSID"  BorderColor="Black"
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
                                        </center>
  </div><br />
<asp:Panel ID="pnlFooterNav" runat="server" HorizontalAlign="Right" CssClass="NavigationMenu">
    <input type="button" runat="server" id ="btnPrevious" value="Previous" />&nbsp;&nbsp;&nbsp;
   <%-- <input type="button" id="btnSave" runat="server" value="Save" style="width: 87px" />--%>&nbsp;&nbsp;&nbsp;
    <input type="button" id="btnSaveContinue" runat="server" value="Save and Continue" />
</asp:Panel> 

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
