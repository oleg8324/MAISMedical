<%@ Page Title="" Language="vb" AutoEventWireup="false" MasterPageFile="~/MasterPage.master" CodeBehind="WorkExperience.aspx.vb" Inherits="MAIS.Web.WorkExperience" EnableEventValidation ="false"  %>
<%@ MasterType VirtualPath="~/MasterPage.master" %>
<%@ Register Src="UserControls/Address.ascx" TagName="Address" TagPrefix="addr" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript" src="Scripts/WorkExperience.js"></script>
    <script type="text/javascript" src="Scripts/Address.js"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="PortalOverrideContent" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="mainContent" runat="server">
    <center>
        <table class="CountySelection" style="width: 100%;">
                        <tr>
                            <td >                              
                                <asp:Label  ID="Label13" runat="server" Font-Bold="True" Text="Work Experience Page"></asp:Label> 
                            </td>                                              
                        </tr>
                    </table></center>
       <br />
      <asp:Panel ID= "pnlapplication" runat="server">
          <div style="text-align:center">
              <label id="lblerr" class="Error1" runat="server" ></label>
              <asp:ValidationSummary runat="server" ID="workExpValidationSummary" ValidationGroup="workExpPage"  CssClass="ErrorSummary errMsg" />
              <asp:Panel ID="pnlMessage" runat="server" CssClass="CountySelection" > <center>
                   <asp:BulletedList ID="BulletedList1" runat="server">
                       <asp:ListItem>FOR RN TRAINER and 17BED RNs: Need 18 months experience as an RN with some DD experience</asp:ListItem>
                      <asp:ListItem>FOR RN INSTRUCTOR: 60 months experience as an RN with 24 months nursing experience in DD field</asp:ListItem>
                   </asp:BulletedList>      </center>             
              </asp:Panel>
              <br />              
              <label id="lblval" class="lblval" runat="server" ></label>
                <label style="color: #000000; font-weight: bold" class="CountySelection">Work Experience Information</label>
              <br />
              <div style="text-align:left;"><label id="lblTotalExperience" style="color:blue;"  runat="server" ></label></div> 
               <asp:Panel ID = "pnlEmployerInfo" runat = "server" GroupingText="Experience Information"  CssClass = "pnlEmployerInfo">  
                   <table class="leftAlign">
                       <tr>
                            <td colspan="8">
                             </td>
                       </tr>
                       <tr>
                            <td colspan="1"  style="text-align:right;">Agency/Employer Name:<span style="color: Red;">*</span></td>
                            <td >
                                 <input  type = "text" id = "txtEmpName" runat = "server"  maxlength = "100" class = "txtEmpName"/>                                                               
                            </td>
                           <td colspan="1"  style="text-align:right;" >Experience :<span style="color: Red;">*</span> </td>
                            <td align="left" colspan="1">     
                                <input type="checkbox" class="chkRNExp" id="chkRNExp" runat="server" />RN &nbsp;
                                 <input type="checkbox" class="chkDDExp" id="chkDDExp" runat="server" />DD
                                 
                            </td>                          
                       </tr>
                       <tr>
                            <td colspan="1"  style="text-align:right;">Start Date:<span style="color: Red;">*</span></td>
                            <td > 
                                <input  type = "text" id = "txtStartDate" class="date-pick txtStartDate" runat = "server"  maxlength = "10" />
                                <input type="hidden" id="hdRNDate" class="hdRNDate" runat="server" />                              
                               
                            </td>
                            <td colspan="1"  style="text-align:right;" >End Date:<span style="color: Red;">*</span></td>
                            <td >
                                <input type = "text" id = "txtEndDate" runat = "server" class="date-pick txtEndDate"  maxlength = "10" />                                                              
                            </td>                                                      
                       </tr>
                       <tr>
                           <td colspan="1"  style="text-align:right;" >Designation/Title:<span style="color: Red;">*</span> </td>
                            <td >  
                                <input type = "text" id = "txtDesignation" runat = "server"  maxlength = "100" class = "txtDesignation"/>                                                             
                            </td>
                            <td colspan="1"  style="text-align:right;" >Role/Duties:<span style="color: Red;">*</span></td>
                            <td colspan="1"> 
                                <textarea id="txtRoles" class="txtRoles" rows="5" runat="server" cols="40" > </textarea>                                                                                               
                            </td>
                       </tr>
                   </table>
               </asp:Panel>
               <asp:Panel ID = "pnlAddr" runat = "server" GroupingText="Address"  CssClass = "pnlAddr">                            
                             <addr:Address ID="workExpAddr" runat="server" />  
               </asp:Panel>  <br /> 
   
              <div style="text-align:right">           
                  <asp:Button ID="btnSaveExp" CssClass="btnSaveExp" Text="Save" runat="server" />   &nbsp;&nbsp;              
                  <asp:Button runat="server" ID="btnAddWe"  Text="Add Additional Work Experience" /> &nbsp;&nbsp;
                 
               </div> 
             <br />
              <div style="text-align:left">
                  
                  <asp:Panel ID="pnlWorkGrdCurrent" CssClass="pnlWorkGrdCurrent" runat="server" HorizontalAlign="Left"  GroupingText="Added Work Experience">
                    <asp:GridView runat="server" ID="grdCurrent" BackColor="White" BorderColor="#3366CC" BorderStyle="Inset" BorderWidth="1px" CellPadding="4" 
                        DataKeyNames="workID" HorizontalAlign="Center" AutoGenerateColumns="false" Width="100%">
                        <Columns>                           
                            <asp:TemplateField HeaderText="Agency/Employer Name">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkBtnSelect" runat="server" CommandName="Select">
                                                    <%# Eval("EmpName")%>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>                          
                            <asp:BoundField DataField="Title" HeaderText="Designation/Title" />
                            <asp:BoundField DataField="EmpStartDate" HeaderText="StartDate" />
                            <asp:BoundField DataField="EmpEndDate" HeaderText="EndDate" />                                                      
                            <asp:TemplateField HeaderText="RN Experience" SortExpression="Active">
                                <ItemTemplate><%#IIf(Boolean.Parse(Eval("ChkRNFlg").ToString()), "Yes", "No")%></ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="DD Experience" SortExpression="Active">
                                <ItemTemplate><%#IIf(Boolean.Parse(Eval("ChkDDFlg").ToString()), "Yes", "No")%></ItemTemplate>
                            </asp:TemplateField>                                                                                     
                             <asp:CommandField ShowDeleteButton="true" /> 
                        </Columns>
                        <RowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" VerticalAlign="Top" />
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                        <SelectedRowStyle BackColor="PowderBlue" Font-Bold="True" />
                        <HeaderStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" BorderColor="Black" />
                    </asp:GridView>
                </asp:Panel>
              </div><br />
                <div style="text-align:left">
                    <asp:Button id="btnWorkHistoryWe" class="btnWorkHistory" runat="server" Text="Work Experience History" />
                    
                <br />
                   <asp:Panel ID="pnlWorkGrd" CssClass="pnlWorkGrd" runat="server" HorizontalAlign="Left"  GroupingText="Work Experience">
                    <asp:GridView runat="server" ID="grdWorkHistory" BackColor="White" BorderColor="#3366CC" DataKeyNames="workID"
                         BorderStyle="Inset" BorderWidth="1px" CellPadding="4" HorizontalAlign="Center" AutoGenerateColumns="false" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="EmpName" HeaderText="Agency/Employer Name" />
                            <asp:BoundField DataField="Title" HeaderText="Designation/Title" />
                            <asp:BoundField DataField="EmpStartDate" HeaderText="StartDate" />
                            <asp:BoundField DataField="EmpEndDate" HeaderText="EndDate" />
                            <asp:BoundField DataField="ChkRNFlg" HeaderText="RN Experience" />
                             <asp:BoundField DataField="ChkDDFlg" HeaderText="DD Experience" />                            
                        </Columns>
                        <RowStyle BackColor="White" ForeColor="Black" HorizontalAlign="Left" VerticalAlign="Top" />
                        <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                        <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                        <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                        <HeaderStyle BackColor="#BFE4FF" Font-Bold="True" ForeColor="Black" HorizontalAlign="Center" BorderColor="Black" />
                    </asp:GridView>
                </asp:Panel>
               </div>
          </div>
          
      </asp:Panel>     
    <br />
     <input type ="hidden" id ="hdWorkID" class ="hdWorkID" runat ="server" />
<div class="NavigationMenu">
    <asp:Button runat="server" ID="btnPrevious" Text="Previous" />&nbsp;&nbsp;&nbsp;
    <%--<input type="button" class="ButtonStyle" value="Save" style="width: 87px" />&nbsp;&nbsp;&nbsp;--%>
    <asp:Button runat="server" ID="btnSaveContinue" Text="Continue" />    
</div>
     
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="WideContentPlaceHolder" runat="server">
</asp:Content>
