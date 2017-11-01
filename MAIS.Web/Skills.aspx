<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="Skills.aspx.vb" Inherits="MAIS.Web.Skills" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="Scripts/SkillPage.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
    <style type="text/css">
        .auto-style1
        {
            height: 25px;
        }
        </style>
    <script type="text/javascript" >
        function ValidatecklSkillCheckList(sender, args) {
            args.IsValid = false;
            $(".cklSkillCheckList").find(":checkbox").each(function () {
                if ($(this).attr("checked")) {
                    args.IsValid = true;
                    return;
                }
            });
        }
    </script>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <div id="divSkillMain" runat="server">
        <table class="CountySelection" style="width: 100%;">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Skill Verification (DD Personnel Only)"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                   <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="Initial & Renewal Application"></asp:Label>

                </td>
            </tr>
        </table>

        <div id="dvSkillFields" runat="server" style="margin-right: 5%; margin-left: 5%;">
            <center>
                <asp:ValidationSummary ID="ValidationSummary1" style="text-align:left;" runat="server" CssClass="ErrorSummary" ValidationGroup="ac" />
            </center>
            <table style="width: 100%;">
                <tr class="gridviewHeader">
                    <td>Date
                        <asp:RequiredFieldValidator ID="rfvSkillsDate" runat="server" ErrorMessage="Must enter date." Text="*" ControlToValidate="txtSkillsDate" SetFocusOnError="True" ValidationGroup="ac" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <td>Name of person Verifying Skills
                        <asp:RequiredFieldValidator ID="rfvPersonVerifying" runat="server" ErrorMessage="Must enter Name of Person Verifying Skill." Text="*" ControlToValidate="txtSkillNamePersonVerifying" SetFocusOnError="True" ValidationGroup="ac" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <td>Title
                        <asp:RequiredFieldValidator ID="rfvTitle" runat="server" ErrorMessage="Must enter Title. " ControlToValidate="txtSkillTitle" SetFocusOnError="True" ValidationGroup="ac" ForeColor="Red">*</asp:RequiredFieldValidator>
                    </td>
                    <td>Certification Category
                        <asp:RequiredFieldValidator ID="rfvCategory" runat="server" ErrorMessage="Must select a Cerification Category" ControlToValidate="ddlSkillCategory" InitialValue="-1" SetFocusOnError="True" ValidationGroup="ac" ForeColor="Red" Enabled="False">*</asp:RequiredFieldValidator>
                    </td>
                    <td id="tdSkillsSelectAll" runat="server" class="ckbSkillsSelectAll">
                       Add All Skills
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                      
                        <asp:TextBox ID="txtSkillsDate" runat="server" CssClass="date-pick txtSkillsDate" Width="100px"></asp:TextBox>
                        
                    </td>
                                        <td class="auto-style1">
                        <asp:TextBox ID="txtSkillNamePersonVerifying" runat="server" Width="90%"></asp:TextBox></td>
                    <td>
                        <asp:TextBox ID="txtSkillTitle" runat="server"></asp:TextBox></td>

                    <td class="auto-style1">
                        <asp:TextBox ID="txtCatetory" runat="server" ReadOnly="True"></asp:TextBox>
                        <asp:DropDownList ID="ddlSkillCategory" runat="server" AutoPostBack="True" Visible="False"></asp:DropDownList></td>
                    <td id="tdSkillsSelectAll2" runat="server" class="tdSillsSelectAll">
                         <asp:CheckBox ID="ckbSkillsSelectAll" runat="server" CssClass="ckbSkillsSelectAll" AutoPostBack="True" />
                    </td>
                </tr>
                </table>
            <table id="tblSkills" runat="server"  width="100%" class="tblSkills">
                <tr class="gridviewHeader">
                    <td>Skills Verified
                        <asp:RequiredFieldValidator ID="rfvSkillsVerified" runat="server" ErrorMessage="Must select a Skills Verified. " Text="*" SetFocusOnError="True" ValidationGroup="ac" ControlToValidate="ddlSkillVerified" InitialValue="-1" ForeColor="Red"></asp:RequiredFieldValidator>
                    </td>
                    <td colspan ="3" align="left" style="padding-left:20px;">Skill Check list 
                        <asp:CustomValidator ID="cvSkillcheckList" runat="server" ErrorMessage="Must select at least one from the Skills Check List." ClientValidationFunction="ValidatecklSkillCheckList" ValidationGroup="ac" ForeColor="Red">*</asp:CustomValidator>
                    </td>
                   
                </tr>
                 <tr>
                    <td valign="top" style="width:40%">
                        <asp:DropDownList ID="ddlSkillVerified" runat="server" AutoPostBack="True" Width="90%"></asp:DropDownList></td>
                    <td colspan="3" align="left">
                        <asp:CheckBoxList ID="cklSkillCheckList" runat="server" CssClass="cklSkillCheckList" DataTextField="Skill_CheckList_Desc" DataValueField="Skill_CheckList_Sid"></asp:CheckBoxList></td>
                </tr>
                <tr>
                    <td></td>
                    <td></td>
                    <td></td>
                    <td>
                        
                    </td>
                </tr>
            </table>
            <table width="100%">
                <tr align="right">
                    <td>&nbsp</td>
                    <td>&nbsp</td>
                    <td>
                        <asp:Button ID="bntCancel" runat="server" Text="Cancel" />
                        &nbsp;&nbsp;
                        <asp:Button ID="bntSkillAdd" runat="server" Text="Add Skill" ValidationGroup="ac" />
                    </td>
                </tr>
            </table>
        </div>
       <br />
       <br />
        <div id="divSkillGrid" runat="server" style="margin-right: 5%; margin-left: 5%;">
            <asp:GridView ID="grvSkillsData" runat="server" AutoGenerateColumns="False" CssClass="gridview" DataKeyNames="Skill_Verification_Type_CheckList_Xref_Sid">
                <HeaderStyle CssClass="gridviewHeader" />
                <Columns>
                    <asp:BoundField DataField="Skill_Verification_Skill_Type_Xref_Sid" HeaderText="ID" Visible="False" />
                    <asp:BoundField DataField="Verification_Date" HeaderText="Date" DataFormatString="{0:MM/dd/yyyy}" />
                    <asp:BoundField DataField="Verified_Person_Title" HeaderText="Title" />
                    <asp:BoundField DataField="Verified_Person_Name" HeaderText="Person Verifying Skill" />
                    <asp:BoundField DataField="CategoryName" HeaderText="Category" />
                    <asp:BoundField DataField="Skill_Verification_Skill_Type" HeaderText="Skill Verified" />
                    <asp:BoundField DataField="Skill_CheckList_Name" HeaderText="Skill CheckList" />
                    <asp:CommandField DeleteText="Remove" ShowDeleteButton="True"  />
                </Columns>
            </asp:GridView>
        </div>
    </div>
    <br />
    <br />
<div class="NavigationMenu"><asp:Panel ID="pnlFooterNav" runat="server">
   <asp:Button  Text="Previous" class ="btnPrevious" id ="btnPrevious" runat ="server"/>&nbsp;&nbsp; 
    <asp:Button id="btnSaveAndContinue" text="Continue" value="Save and Continue" class = "btnSaveAndContinue" runat ="server"/></asp:Panel>
</div>
     <asp:HiddenField ID="hdfDDPeronalID" runat="server"  ClientIDMode="Static" />
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
