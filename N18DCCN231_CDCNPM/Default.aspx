<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeBehind="Default.aspx.cs" Inherits="N18DCCN231_CDCNPM.Default" %>

<asp:Content ID="Content" ContentPlaceHolderID="MainContent" runat="server">
    <div id="header">
        <asp:Label ID="LabelTitle" runat="server" Text="Nhập tiêu đề báo cáo: " Font-Size="Large"></asp:Label>
        <asp:TextBox ID="TextBoxNhapTieuDe" runat="server" Width="916px" Font-Size="Large"></asp:TextBox>    
    </div>
    <div id="main">
        <div style="display: flex;">
            <div id="table-content">
                <asp:Panel ID="PanelChonBang" runat="server">
                    <br />
                    <asp:Label ID="LabelChonBang" runat="server" Text=" Chọn bảng " BorderStyle="Double" Font-Size="Large"></asp:Label>
                    <br />
                    <asp:CheckBoxList ID="CheckBoxListTable" runat="server" Height="20px" OnSelectedIndexChanged="CheckBoxListTable_SelectedIndexChanged" Width="500px" AutoPostBack="True">
                    </asp:CheckBoxList>
                    <br />
                </asp:Panel>
            </div>       
            <div id="column-content">
                <asp:Panel ID="PanelChonCot" runat="server">
                    <br />
                    <asp:Label ID="LabelChonCot" runat="server" Text=" Chọn cột " BorderStyle="Double" Font-Size="Large"></asp:Label>
                    <asp:Button ID="ButtonClearColumn" runat="server" OnClick="ButtonClearColumn_Click" Text="Bỏ chọn tất cả" />
                    <br />                 
                    <asp:CheckBoxList ID="CheckBoxListColumn" runat="server" Height="20px" OnSelectedIndexChanged="CheckBoxListColumn_SelectedIndexChanged" Width="500px" AutoPostBack="True" RepeatColumns="5" RepeatDirection="Horizontal">              
                    </asp:CheckBoxList>   
                    <br />   
                </asp:Panel>
            </div>
        </div>
        <div id="query-content">
            <asp:Panel ID="PanelGridViewColumn" runat="server">  
                <br />
                <asp:GridView ID="GridView1" runat="server" BackColor="White"  BorderColor="#CCCCCC"  BorderWidth="1px" CellPadding="3" Height="16px" Width="1300px" >
                    <Columns>
                        <asp:TemplateField HeaderText="Sắp xếp" >
                            <ItemTemplate>
                                <asp:DropDownList ID="DropDownList_Sort" runat="server" >
                                    <asp:ListItem Text="Không" Value="None"></asp:ListItem>
                                    <asp:ListItem Text="Tăng dần" Value="ASC"></asp:ListItem>
                                    <asp:ListItem Text="Giảm dần" Value="DESC"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderText="Hàm" >
                            <ItemTemplate>
                                <asp:DropDownList ID="DropDownList_Function" runat="server" >
                                    <asp:ListItem Text="Select" Value="Select"></asp:ListItem>
                                    <asp:ListItem Text="Count" Value="Count"></asp:ListItem>
                                    <asp:ListItem Text="Sum" Value="Sum"></asp:ListItem>
                                    <asp:ListItem Text="Min" Value="Min"></asp:ListItem>
                                    <asp:ListItem Text="Max" Value="Max"></asp:ListItem>                                     
                                    <asp:ListItem Text="Avg" Value="Avg"></asp:ListItem>
                                    <asp:ListItem Text="Group by" Value="Group by"></asp:ListItem>
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>          
                        <asp:TemplateField HeaderText="Điều Kiện">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDieuKien" runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
                    <HeaderStyle BackColor="#006699" Font-Bold="true" ForeColor="White"/>
                    <PagerStyle BackColor="White" ForeColor="#000066" HorizontalAlign="Left"/>
                    <RowStyle ForeColor="#000066"/>
                    <SelectedRowStyle BackColor="#006699" ForeColor="White" Font-Bold="true"/>
                    <SortedAscendingCellStyle BackColor="#000066"/>
                    <SortedAscendingHeaderStyle BackColor="#000066"/>
                    <SortedAscendingCellStyle BackColor="#007DBB"/>
                    <SortedDescendingCellStyle BackColor="#CAC9C9"/>
                    <SortedDescendingHeaderStyle BackColor="#00547E"/>
                </asp:GridView>
                <br />
                <asp:Button ID="ButtonQuery" runat="server" OnClick="ButtonQuery_Click" Text="Tạo QUERY" />
                <br />
                <br />
                <asp:TextBox ID="txtQuery" runat="server" Rows="5" TextMode="MultiLine" Width="1300px"></asp:TextBox>
                <br />
                   
            </asp:Panel>
        </div>

        <div id="report-content">        
            <br />   
            <br />
                <asp:Button ID="btnReport" runat="server" OnClick="ButtonReport_Click" Text="Tạo REPORT" />
            <br />
            <br />
        </div>
    </div>
</asp:Content>