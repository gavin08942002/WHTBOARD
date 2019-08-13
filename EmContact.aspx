<%@ Page Language="VB" AutoEventWireup="false" CodeFile="EmContact.aspx.vb" Inherits="EmContact" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
    <link href="Content/bootstrap.min.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/bootstrap.min.js"></script>
    <script src="Scripts/html5shiv.min.js"></script>
    <script src="Scripts/respond.js"></script>
    
    <style type="text/css">
        .body {
            width:1280px;
            height:1024px;
            vertical-align:bottom
            
        }
     </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="body">
        <div style="width:1280px;height:224px;">
            <div class="btn-group margin:0px auto;">
                <button type="button" class="btn btn-default dropdown-toggle btn-primary" data-toggle="dropdown">
                      外白板<span class="caret"></span>
                </button>
                <ul class="dropdown-menu" role="menu">
                    <li><asp:HyperLink ID="HyperLink1" runat="server" >值班醫師</asp:HyperLink></li>
                   <li><asp:HyperLink ID="HyperLink2" runat="server">連絡人</asp:HyperLink></li>
                </ul>
            </div>
            <div class="btn-group margin:0px auto;">
                <button type="button" class="btn btn-default dropdown-toggle btn-success" data-toggle="dropdown">
                      醫生班表<span class="caret"></span>
                </button>
                <ul class="dropdown-menu" role="menu">
                    <li><asp:HyperLink ID="HyperLink3" runat="server" >班表</asp:HyperLink></li>
                </ul>
            </div>
            <div class="btn-group margin:0px auto;">
                <button type="button" class="btn btn-default dropdown-toggle btn-primary" data-toggle="dropdown">
                      佈告欄<span class="caret"></span>
                </button>
                <ul class="dropdown-menu" role="menu">
                    <li><asp:HyperLink ID="HyperLink4" runat="server" >內容編輯</asp:HyperLink></li>
                </ul>
            </div>
        </div>

        <div style="width:1280px;height:800px;">  
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataKeyNames="ID" DataSourceID="SqlDataSource1" Font-Size="18px" Width="1185px">
            <Columns>
                <asp:CommandField ShowEditButton="True" />
                <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Justify">
                <HeaderStyle Width="30px" />

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="是否顯示" SortExpression="ViewCheck" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center">
                    <EditItemTemplate>
                        <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal" SelectedValue='<%# Bind("ViewCheck") %>'>
                            <asp:ListItem Value="0">否</asp:ListItem>
                            <asp:ListItem Value="1">是</asp:ListItem>
                        </asp:RadioButtonList>
                        <br />
                    </EditItemTemplate>
                    <ItemTemplate>
                        <asp:RadioButtonList ID="RadioButtonList2" runat="server" RepeatDirection="Horizontal" SelectedValue='<%# Bind("ViewCheck") %>' Enabled="False">
                            <asp:ListItem Value="0">否</asp:ListItem>
                            <asp:ListItem Value="1">是</asp:ListItem>
                        </asp:RadioButtonList>
                    </ItemTemplate>
                    <HeaderStyle HorizontalAlign="Center" Wrap="True" />

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="EmPno" HeaderText="員工編號" SortExpression="EmPno" >
                <HeaderStyle HorizontalAlign="Center" />
                <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="CName" HeaderText="姓名" SortExpression="CName" ItemStyle-HorizontalAlign="Center">
                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />

<ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Position" HeaderText="職稱" SortExpression="Position" ItemStyle-HorizontalAlign="Center">
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="Phone" HeaderText="連絡電話" SortExpression="Phone" ItemStyle-HorizontalAlign="Center">
<ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:BoundField>
            </Columns>
            <HeaderStyle HorizontalAlign="Center" />
            <RowStyle HorizontalAlign="Center" />
        </asp:GridView>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:E-boardConnectionString %>" DeleteCommand="DELETE FROM [EmInfo] WHERE [ID] = @ID" InsertCommand="INSERT INTO [EmInfo] ([ViewCheck], [EmPno], [CName], [Position], [Phone]) VALUES (@ViewCheck, @EmPno, @CName, @Position, @Phone)" SelectCommand="SELECT [ID], [ViewCheck], [EmPno], [CName], [Position], [Phone] FROM [EmInfo] WHERE (([HospCode] = @HospCode) AND ([WardCode] = @WardCode)) ORDER BY [ID]" UpdateCommand="UPDATE [EmInfo] SET [ViewCheck] = @ViewCheck, [EmPno] = @EmPno, [CName] = @CName, [Position] = @Position, [Phone] = @Phone WHERE [ID] = @ID">
            <DeleteParameters>
                <asp:Parameter Name="ID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="ViewCheck" Type="String" />
                <asp:Parameter Name="EmPno" Type="String" />
                <asp:Parameter Name="CName" Type="String" />
                <asp:Parameter Name="Position" Type="String" />
                <asp:Parameter Name="Phone" Type="String" />
            </InsertParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="HospCode" QueryStringField="HospCode" Type="String" />
                <asp:QueryStringParameter Name="WardCode" QueryStringField="WardCode" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="ViewCheck" Type="String" />
                <asp:Parameter Name="EmPno" Type="String" />
                <asp:Parameter Name="CName" Type="String" />
                <asp:Parameter Name="Position" Type="String" />
                <asp:Parameter Name="Phone" Type="String" />
                <asp:Parameter Name="ID" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
        </div>
    </div>
    </form>
</body>
</html>
