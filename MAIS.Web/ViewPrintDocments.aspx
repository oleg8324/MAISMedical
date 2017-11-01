<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="ViewPrintDocments.aspx.vb" Inherits="MAIS.Web.ViewPrintDocments" %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="Scripts/spin.min.js"></script> 
    <script type="text/jscript" src="Scripts/ViewPrintDocuments.js"></script>
    <script type ="text/jscript">
        function doCustomValidate(source, args) {
            args.IsValid = false;
            if ($('.txtDODDSSN').val().length > 0) {
                args.IsValid = true;
            }
            if ($('.txtDODDID').val().length > 0) {
                args.IsValid = true;
            }

        }
        function doCustomValidateRN(source, args) {
            args.IsValid = false;

            if ($('.txtRNNO').val().length > 0) {
                args.IsValid = true;
            }
            if ( $.trim($(".txtLName").val()).length > 0) {
                args.IsValid = true;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <div id="divBack" runat ="server" style="text-align:right">
        <asp:LinkButton ID="hlbBack" runat="server" Visible="false"  >Go Back</asp:LinkButton>
     </div>
    <div>
        <table class="CountySelection" style="width: 100%;">
            <tr>
                <td>
                    <asp:Label ID="Label12" runat="server" Font-Bold="True" Text="View/Print Documents"></asp:Label>
                </td>
            </tr>

        </table>
    </div>
    <br />
    <div id="dvRnType" runat="server" visible="true" class="dvRnType" style="margin-right: auto; margin-left: auto; width: 800px">
        <asp:Panel ID="Panel1" runat="server" GroupingText="Select a Type" >
            <center>
                <asp:RadioButtonList ID="rblRnTypeSelect" class="rblRnTypeSelect" runat="server"
                    RepeatDirection="Horizontal" BorderStyle="Dotted" BorderWidth="2px" AutoPostBack="True">
                    <asp:ListItem Value="0" Text="RN"></asp:ListItem>
                    <asp:ListItem Value="1" Text="DD Personnel"></asp:ListItem>

                </asp:RadioButtonList>
            </center>
        </asp:Panel>
    </div>
    <div id="dvRnSelectValues" class="dvRnSelectValues"  style="margin-right: auto; margin-left: auto; width: 800px">
        <asp:Panel ID="pnlRNSelectionValue" CssClass="pnlRnSelectionValue"  GroupingText="Search Options" runat="server" Font-Size="9pt" Font-Italic="false" >
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="a" ForeColor="Red" CssClass="ValidationSummaryRN" />
            <center>
                <table>

                    <tr>
                        <td align="right">
                            <label>RN License No. :</label></td>
                        <td>
                            <asp:TextBox ID="txtRNNO" runat="server" CssClass="txtRNNO" Style="float: left" Width="128px"></asp:TextBox>
                            <%--<asp:RegularExpressionValidator ID="RegRNDDLicSSN" runat="server" ControlToValidate="txtRNNO" Display="None"
                                ErrorMessage="Please enter a valid Liscence Number" ValidationExpression="^[0-9]*$" ValidationGroup="a" ForeColor="Red">*</asp:RegularExpressionValidator>--%>
                        </td>
                        <td>
                            <asp:CustomValidator ID="CustomValidator2" runat="server" ErrorMessage="Must have at least 1 field." Text="*" ValidationGroup="a" ForeColor="Red" ClientValidationFunction="doCustomValidateRN"></asp:CustomValidator>
                        </td>
                    </tr>

                   <%-- <tr>
                        <td align="right">
                            <label>First Name :</label></td>
                        <td>
                            <asp:TextBox ID="txtFirstName" runat="server" Style="float: left"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegFirstName" runat="server" ControlToValidate="txtFirstName" Display="None"
                                ErrorMessage="Please enter only alphabets" ValidationExpression="^[a-zA-Z]+$" ValidationGroup="a">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>--%>

                    <tr>
                        <td align="right">
                            <label>Last Name :</label></td>
                        <td>
                            <asp:TextBox ID="txtLName" runat="server" CssClass="txtLName" Style="float: left"></asp:TextBox>
                            
                        </td>
                        <td>
                            <asp:RegularExpressionValidator ID="RegLastName" runat="server" ControlToValidate="txtLName" Display="None"
                                ErrorMessage="Please enter only alphabets" ValidationExpression="^[a-zA-Z]+$" ValidationGroup="a" ForeColor="Red">*</asp:RegularExpressionValidator>
                        </td>
                    </tr>
                   <%-- <tr>
                        <td align="right">
                            <label>Session Start Date :</label></td>
                        <td>
                            <asp:TextBox ID="txtSearchSessionStartDate" runat="server" CssClass="date-pick txtSearchSessionStartDate" Style="float: left"></asp:TextBox>
                            <asp:CompareValidator Type="Date" Operator="DataTypeCheck" runat="server" ID="CompDate" ControlToValidate="txtSearchSessionStartDate" ValidationGroup="a" />


                            <br />
                        </td>
                    </tr>--%>
                </table>
                <asp:Button ID="bntSearchRN" runat="server" Text="Search" CssClass="SearchRN" ValidationGroup="a" />
            </center>

        </asp:Panel>
          <asp:Panel ID="pnlDDSelectionValue" CssClass="pnlDDSelectionValue" GroupingText="Search Options" runat="server"  Font-Size="9pt" Font-Italic="false" >
              <asp:ValidationSummary ID="ValidationSummary2" runat="server" ValidationGroup="b" ForeColor="Red" CssClass="ValidationSummaryDD" />
            <center>
                <table>

                    <tr>
                        <td align="right">
                            <label>DD Personnel Last 4 Digits of SSN :</label></td>
                        <td>

                            <asp:TextBox ID="txtDODDSSN" runat="server" CssClass="txtDODDSSN numericOnly" Style="float: left" MaxLength="4"  Width="128px" ValidationGroup="b"></asp:TextBox>
                            
                        </td>
                        <td>
<asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtDODDSSN" Display="None"
                                ErrorMessage="Please enter only numeric digits for the last 4 SS Number." ValidationExpression="^[0-9]*$" ValidationGroup="b" ForeColor="Red">*</asp:RegularExpressionValidator>

                        </td>
                    </tr>

                   <%-- <tr>
                        <td align="right">
                            <label>First Name :</label></td>
                        <td>
                            <asp:TextBox ID="txtFirstName" runat="server" Style="float: left"></asp:TextBox>
                            <asp:RegularExpressionValidator ID="RegFirstName" runat="server" ControlToValidate="txtFirstName" Display="None"
                                ErrorMessage="Please enter only alphabets" ValidationExpression="^[a-zA-Z]+$" ValidationGroup="a">
                            </asp:RegularExpressionValidator>
                        </td>
                    </tr>--%>

                    <tr>
                        <td align="right">
                            <label>DD Personnel Code:</label></td>
                        <td >
                            <asp:TextBox ID="txtDODDID" runat="server" Style="float: left" CssClass="txtDODDID" MaxLength="10"  Width="128px"></asp:TextBox>
                            
                        </td>
                        <td>
                            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Must have at least 1 field." ClientValidationFunction="doCustomValidate" ValidationGroup="b" Text="*" ForeColor="Red" ></asp:CustomValidator>
<%--<asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtDODDID" Display="None"
                                ErrorMessage="Please enter only numeric digits." ValidationExpression="^[0-9]*$" ValidationGroup="b" ForeColor="Red">*</asp:RegularExpressionValidator>--%>
                        </td>
                    </tr>
                   <%-- <tr>
                        <td align="right">
                            <label>Session Start Date :</label></td>
                        <td>
                            <asp:TextBox ID="txtSearchSessionStartDate" runat="server" CssClass="date-pick txtSearchSessionStartDate" Style="float: left"></asp:TextBox>
                            <asp:CompareValidator Type="Date" Operator="DataTypeCheck" runat="server" ID="CompDate" ControlToValidate="txtSearchSessionStartDate" ValidationGroup="a" />


                            <br />
                        </td>
                    </tr>--%>
                </table>
                <asp:Button ID="BntSearchDD" runat="server" Text="Search" CssClass="Search" ValidationGroup="b" />
            </center>

        </asp:Panel>
    </div>
    <div id="divSpinner" runat="server" class="divSpinner" style="font-family:Tahoma, Arial, Verdana; font-size:24pt; font-weight:bold; text-align: center;">
            <br /><br />
        </div>

    <div id="divUDSData" runat="server" style="margin-right: auto; margin-left: auto; width: 800px; ">
        <asp:GridView ID="gvUDSData" runat="server" AutoGenerateColumns="False" CssClass="gridview" AllowSorting="True">
            <HeaderStyle CssClass="gridviewHeader" />
            <Columns>

                <asp:TemplateField HeaderText="Name">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem,"Generics(1).value")%>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label1" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Generics(1).value")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="RN Lic or 4 SSN">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Generics(2).value")%>' ></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Generics(2).value")%>' ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="DD Personnnel Code">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox5" runat="server"  Text='<%# DataBinder.Eval(Container.DataItem, "Generics(7).value")%>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label4" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Generics(7).value")%>' ></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Application Type">
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox3" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Generics(0).value")%>'></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:Label ID="Label2" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "Generics(0).value")%>'></asp:Label>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="DocumentDescription" HeaderText="Doc. Description" />
                <asp:BoundField DataField="Received" HeaderText="Date " DataFormatString="{0:d}" />
                <asp:TemplateField>
                    <EditItemTemplate>
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl='<%# DataBinder.Eval(Container.DataItem, "DownloadURL")%>'>View Doc.</asp:HyperLink>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <EmptyDataTemplate>
                No data found for search.
            </EmptyDataTemplate>
        </asp:GridView>
    </div>

</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
