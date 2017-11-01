<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="PersonalInformation.aspx.vb" Inherits="MAIS.Web.PersonalInformation" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="UserControls/Address.ascx" TagName="Address" TagPrefix="uc2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="Scripts/PersonalInformation.js"></script>
<script type="text/javascript" src="Scripts/Address.js"></script>
<script type="text/javascript" src="Scripts/MAISCommon.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <div>
    <asp:ValidationSummary ID="ValidationSummary1" runat="server" 
        CssClass="ErrorSummary errMsg" ValidationGroup="landingPage"/>
        <input type="hidden" id ="hdRNFlag" class ="hdRNFlag" runat ="server" />
    <input type="hidden" id ="hdRole" class ="hdRole" runat ="server" />
        <input type="hidden" id ="hdStartpage" class ="hdStartpage" runat ="server" />
        <table class="CountySelection" style="width: 100%;">
                        <tr>
                            <td>
    <asp:Label ID="Label8" runat="server" Font-Bold="True" Text="Personal Information"></asp:Label>
                                </td>
                            <td style="text-align:right;width:30%" id ="trUpdateExisting" runat ="server">
                                <asp:LinkButton  ID="lnkBack" Text="Go To Update Existing Page" runat="server" ></asp:LinkButton> 
                             </td>
                        </tr>
                    </table>
    <asp:Panel ID = "pnlPersonalInformation" runat = "server" 
        GroupingText="Personal Information">
        <table width="100%">
            <tr>
                <td align="right">
                    <label id="lblLicenseNoLast4SSN" runat = "server" class = "lblLicenseNoLast4SSN"></label>                    
                    <label id = "Label1" style="color: #FF0000">*</label>                                    
                </td>
                <td align="left">
                    <input type = "text" id="txtLicenseNoLast4SSN" class="txtLicenseNoLast4SSN" runat = "server" causesvalidation="True" validationgroup="landingPage" maxlength="8" size ="15"/>
                </td>
                <td align="right">
                    <label id="lblDOBRNLicIssuance" runat = "server" class = "lblDOBRNLicIssuance"></label>
                    <label id = "Label3" style="color: #FF0000">*</label>                                                                   
                </td>
                <td align="left">
                    <input type = "text" id ="txtDOBRNLicIssuance" class="date-pick txtDOBRNLicIssuance" size = "15" runat = "server"/>
                </td>
                 <td align = "right">Gender:
                    <label id = "Label6" style="color: #FF0000">*</label></td>
                    <td align = "left">
                        <asp:RadioButtonList runat = "server" ID = "rdbGender"  CssClass = "rdbGender"
                            RepeatDirection="Horizontal">
                            <asp:ListItem Value="0" Text="M"></asp:ListItem>
                            <asp:ListItem Value="1" Text="F"></asp:ListItem>
                        </asp:RadioButtonList>
                    </td>
            </tr>
           <tr>
                <td align = "right">Last Name:
                    <label id = "Label4" style="color: #FF0000">*</label></td>
                <td align = "left">
                    <input type = "text" id = "txtLastname" runat = "server" size = "15" maxlength = "50" class = "txtLastname"/>
                </td>
                <td align = "right">First Name:
                    <label id = "Label5" style="color: #FF0000">*</label></td>
                <td align = "left">
                    <input type = "text" id = "txtFirstName" runat = "server" size = "15" maxlength = "50" class = "txtFirstName"/>
                </td>
                <td align = "right">MI.:</td>
                <td align = "left">
                    <input type = "text" id = "txtMiddleName" runat = "server" size = "5" maxlength = "2" class = "txtMiddleName"/>
                </td>
            </tr>
        </table> 
    </asp:Panel><br />
     <label style="color: #000000; font-weight: bold" class="CountySelection">Personal Contact Information</label>
     <asp:Panel ID = "pnlPersonalContact" runat = "server" 
        GroupingText="Personal Contact Information">
     <table class = "tableborder">
         <tr>
            <td>
            </td>
            <td>Home</td>
            <td>Work</td>
            <td>Cell/Other</td>
         </tr>
         <tr>
                <td align = "right">Telephone Number:<label id = "Label7" style="color: #FF0000">*</label></td>
                <td align = "left">
                    <input type = "text" id = "txtHomePhoneNumber" runat = "server" size = "30" maxlength = "12" class = "txtHomePhoneNumber"/>
                </td>
                <td align = "left">
                    <input type = "text" id = "txtWorkPhoneNumber" runat = "server" size = "30" maxlength = "12" class = "txtWorkPhoneNumber"/>
                </td>
                <td align = "left">
                    <input type = "text" id = "txtCellPhoneNumber" runat = "server" size = "30" maxlength = "12" class = "txtCellPhoneNumber"/>
                </td>
         </tr>
         <tr>
                <td align = "right">Email Address:
                    <label id = "Label2" style="color: #FF0000">*</label></td>
                <td align = "left">
                    <input type = "text" id = "txtHomeAddress" runat = "server" size = "30" maxlength = "100" class = "txtHomeAddress"/>
                </td>
                <td align = "left">
                    <input type = "text" id = "txtWorkAddress" runat = "server" size = "30" maxlength = "100" class = "txtWorkAddress"/>
                </td>
                <td align = "left">
                    <input type = "text" id = "txtCellAddress" runat = "server" size = "30" maxlength = "100" class = "txtCellAddress"/>
                </td>
         </tr>
         </table>
     </asp:Panel> 
     <asp:Panel ID = "pnlPersonalMailing" runat = "server" GroupingText="Personal Mailing Address" CssClass = "pnlPersonalMailing">
        <uc2:Address ID="addrBar1" runat="server"/>
     </asp:Panel>
</div><br />
    <input type ="hidden" id ="hdNew" class ="hdNew" runat ="server" />
<asp:Panel ID="pnlFooterNav" runat="server" HorizontalAlign="Right" CssClass="NavigationMenu">
    <input type="button" value="Previous" class ="btnPrevious" id ="btnPrevious" runat ="server"/>&nbsp;&nbsp;&nbsp;
    <input type="button" value="Save" style="width: 87px" id = "btnSave" class = "btnSave" runat ="server"/>&nbsp;&nbsp;&nbsp;
    <input type="button" value="Save and Continue" class = "btnSaveAndContinue" runat ="server" id ="btnSaveAndContinue"/>
</asp:Panel>
    <div style="border: medium solid #FF0000;text-align:center" runat ="server" id ="divNote">
        <p style="color: #FF0000">If you leave this page without Saving you will lose all unsaved information: Please Save or Save and continue.</p>
        <p style="color: #FF0000">All communications, certificates and expiration notices will be emailed to the certified person identified on this page. Certified persons must provide at least one valid email address. A home email address is preferred.</p>
        <p id="pNote18" runat="server" style="color: #FF0000">Alert : Date of Original Lic. Issuance date should be 18 months prior to the Certification date.</p>
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
