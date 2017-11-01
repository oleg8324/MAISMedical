<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="RNAttestation.aspx.vb" Inherits="MAIS.Web.RNAttestion" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/jscript" src="Scripts/Attestation.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <%--<asp:Panel ID="Panel2" runat="server" GroupingText="Question 1" BorderStyle="Solid">
    <asp:Label ID = "lbl1" runat = "server">
        Applicant meets the requirements of rule 5123:2-6 (Medication Administration Certification) of the Ohio Administrative Code
        and other standards and assurances established under Chapter 5123 of the Ohio Revised Code.
    </asp:Label><center>
    <asp:RadioButtonList ID="RadioButtonList1" CssClass="rblAnswer" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
    </asp:RadioButtonList></center>
</asp:Panel>--%>
<%--<asp:Panel ID="Panel1" runat="server" GroupingText="Question 1"  CssClass="leftAlign">
    <asp:Label ID = "Label1" runat = "server">
        Applicant mailing address, contact and employer information has been updated as needed.
    </asp:Label>
    <asp:RadioButtonList ID="RadioButtonList2" CssClass="rblAnswer" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true" >
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
    </asp:RadioButtonList>
</asp:Panel>
<asp:Panel ID="Panel3" runat="server" GroupingText="Question 2"  CssClass="leftAlign">
    <asp:Label ID = "Label22" runat = "server">
       I understand, as an RN Trainer, I must notify the department whenever there is a change in contact information and change or termination of my employment.
    </asp:Label>
    <asp:RadioButtonList ID="RadioButtonList3" CssClass="rblAnswer" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
    </asp:RadioButtonList>
</asp:Panel>
<asp:Panel ID="Panel4" runat="server" GroupingText="Question 3"  CssClass="leftAlign">
    <asp:Label ID = "Label3" runat = "server">
       RN Trainer has validated DD personnel current employment with an active certified DODD provider or self employment as an active current certified DODD Independent Provider.
    </asp:Label>
    <asp:RadioButtonList ID="RadioButtonList4" CssClass="rblAnswer" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
    </asp:RadioButtonList>
</asp:Panel>
<asp:Panel ID="Panel5" runat="server" GroupingText="Question 4" CssClass="leftAlign">
    <asp:Label ID = "Label4" runat = "server">
        Applicant is at least 18 years of age.
    </asp:Label>
    <asp:RadioButtonList ID="RadioButtonList5" CssClass="rblAnswer" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
    </asp:RadioButtonList>
</asp:Panel>
<asp:Panel ID="Panel6" runat="server" GroupingText="Question 5"  CssClass="leftAlign">
    <asp:Label ID = "Label5" runat = "server">
       Applicant has a high school diploma or GED.
    </asp:Label>
    <asp:RadioButtonList ID="RadioButtonList6" CssClass="rblAnswer" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
    </asp:RadioButtonList>
</asp:Panel>
<asp:Panel ID="Panel8" runat="server" GroupingText="Question 6"  CssClass="leftAlign">
    <asp:Label ID = "Label7" runat = "server">
       I will maintain a current RN License in the state of Ohio during my two year certification period and will notify DODD of any change in status.
        </asp:Label>
    <asp:RadioButtonList ID="RadioButtonList8" CssClass="rblAnswer" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
    </asp:RadioButtonList>
</asp:Panel>
<asp:Panel ID="Panel9" runat="server" GroupingText="Question 7"  CssClass="leftAlign">
    <asp:Label ID = "Label8" runat = "server">
        Applicant has completed a program evaluation.
    </asp:Label>
    
    <asp:RadioButtonList ID="RadioButtonList9" CssClass="rblAnswer" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
    </asp:RadioButtonList>
</asp:Panel>
<asp:Panel ID="Panel10" runat="server" GroupingText="Question 8" CssClass="leftAlign">
    <asp:Label ID = "Label9" runat = "server">
        The signed paper application has been uploaded and stored in UDS.
    </asp:Label>
    <asp:RadioButtonList ID="RadioButtonList10" CssClass="rblAnswer" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
    </asp:RadioButtonList>
</asp:Panel>
<asp:Panel ID="Panel11" runat="server" GroupingText="Question 9" CssClass="leftAlign">
    <asp:Label ID = "Label11" runat = "server">
        I have met the requirements for renewal as specified in Ohio Law and Rule and will, upon request, provide proof of CEs recorded in this system and any other requested documents to DODD.
    </asp:Label>
    <asp:RadioButtonList ID="RadioButtonList11" CssClass="rblAnswer" runat="server" RepeatDirection="Horizontal"
                    AutoPostBack="true">
                    <asp:ListItem Value="1">Yes</asp:ListItem>
                    <asp:ListItem Value="0">No</asp:ListItem>
    </asp:RadioButtonList>
</asp:Panel>--%>
    <div >
        <table class="CountySelection" style="width: 100%;">
            <tr>
                <td>
                    <asp:Label ID="Label1" runat="server" Font-Bold="True" Text="Attestations"></asp:Label>
                </td>
            </tr>
            
        </table>
    <asp:Panel ID="pnlAttestationMain" runat="server"  Width="95%" >

    </asp:Panel>

    </div>
    <div>
           <asp:Label ID="Label10" runat="server" Font-Bold="True" Font-Size="Medium" Style="text-align: center"
                                    Text="All RNs must read the statements below, and sign their initials">
                                </asp:Label>
        <br /><br />
        <div style="width:95%" class="leftAlign">
            <asp:Label ID="Label2" runat="server" Font-Bold="True" Text="In accordance with Executive Order 2011-03K, Vendor or Grantee, by signature on this document, certifies: (1) it has reviewed and understands Executive Order 2011-03K, (2) has reviewed and understands the Ohio ethics and conflict of interest laws, and (3) will take no action inconsistent with those laws and this order. The Vendor or Grantee understands that failure to comply with Executive Order 2011-03K is, in itself, grounds for termination of this contract or grant and may result in the loss of other contracts or grants with the State of Ohio."></asp:Label><br />
                                    <br />
                                    <asp:Label ID="Label185" runat="server" Font-Bold="True" Text="A copy of Executive Order 2011-03K can be found at:"></asp:Label>
                                    <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="http://www.governor.ohio.gov/Portals/0/pdf/executiveOrders/EO2011-03.pdf" Target="_blank">http://www.governor.ohio.gov/Portals/0/pdf/executiveOrders/EO2011-03.pdf  </asp:HyperLink><br />
                                    <br />
                                    <asp:Label ID="Label186" runat="server" Font-Bold="True" Text="Whoever knowingly and willfully makes or causes to be made a false statement or representation on this statement, may be prosecuted under applicable federal or state laws. In addition, knowingly and willfully failing to fully and accurately disclose the information requested may result in denial of a request to participate or where the entity already participates, a termination of its agreement or contract with the State agency or the Secretary, as appropriate."></asp:Label><br />
                                    <br />
        </div>
<asp:Panel ID="Panel7" runat="server" BackColor="White" GroupingText="Agreement"
                                        HorizontalAlign="Left" Width="95%">
                                        <table cellspacing="0" style="width: 100%;">
                                            <tr>
                                                <td align="left">
                                                    If you have questions about any of the items that you need to attest to, please email provider services at <a href="mailto:ma.database@dodd.ohio.gov">ma.database@dodd.ohio.gov</a> or contact 1&nbsp;(800)&nbsp;617-6733.
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:Label ID="lblAttestationError" CssClass="lblAttestationError" runat="server" ForeColor="Red"></asp:Label>
                                                    <br />
                                                    <asp:TextBox ID="txtInitials"  runat="server" MaxLength="3" Width="30px" CssClass="txtInitials"></asp:TextBox>&nbsp;<asp:Label
                                                        ID="Label6" runat="server" Font-Size="Small" Text="RN's Initials"></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                                    <asp:Button ID="btnAgreeInitials" class="btnSaveRequirement" runat="server" Style="border: 1px solid #507CD1;
                                                        width: 100px; background-color: White; font-family: Verdana; font-size: 0.8em;
                                                        color: #284E98; margin-bottom: 0px;" Text="Agree" Enabled="true" Width="100px"
                                                        Height="18px" ValidationGroup="1" />
                                                    <br />
                                                    <asp:Label ID="lblAgreed" runat="server" CssClass="lblAgreed" ForeColor="Blue"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    <asp:RegularExpressionValidator ID="revInitials" runat="server" ValidationGroup="1" ControlToValidate="txtInitials" 
                                                        Display="Dynamic" ErrorMessage="Invalid Initials: It can contain only alphabetic characters." ValidationExpression="^[a-zA-Z'.\s]*$" ForeColor="Red"></asp:RegularExpressionValidator>
                                                </td>
                                            </tr>
                                        </table>
                                        <!--   -->
                                    </asp:Panel><br />
</div>
<div class="NavigationMenu">                                    
    <asp:Button ID="bntPrevious" runat="server"  value="Previous" Text="Previous" />&nbsp;&nbsp;&nbsp;
    <asp:Button ID="btnSave" runat="server"  value="Save" Text="Save" Enabled="false" />
   <%-- <input type="button" value="Save" style="width: 87px" />--%>&nbsp;&nbsp;&nbsp;
    <%--<input type="button" value="Save and Continue" onclick ="location.href='Summary.aspx'" />--%>
                                        <asp:Button ID="bntSaveAndContinue1" runat="server" Text="Save and Continue" CssClass="bntSaveAndContinue"  />
</div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
