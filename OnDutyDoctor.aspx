<%@ Page Language="VB" AutoEventWireup="false" CodeFile="OnDutyDoctor.aspx.vb" Inherits="UserControl" %>

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
            <asp:DetailsView ID="DetailsView1" runat="server" AutoGenerateRows="False" DataKeyNames="ID" DataSourceID="SqlDataSource1" Height="50px" Width="439px" DefaultMode="Edit" Font-Size="18px">
                <Fields>
                    <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True" SortExpression="ID" Visible="False" />
                    <asp:TemplateField HeaderText="員工編號" SortExpression="EmPno1">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("EmPno1") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox1" runat="server" Text='<%# Bind("EmPno1") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("EmPno1") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="總值班醫師" SortExpression="DrName1">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("DrName1") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox2" runat="server" Text='<%# Bind("DrName1") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("DrName1") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="員工編號" SortExpression="EmPno2">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("EmPno2") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox3" runat="server" Text='<%# Bind("EmPno2") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("EmPno2") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="值班醫師" SortExpression="DrName2">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("DrName2") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox4" runat="server" Text='<%# Bind("DrName2") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("DrName2") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="員工編號" SortExpression="Empno3">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Empno3") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox5" runat="server" Text='<%# Bind("Empno3") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label5" runat="server" Text='<%# Bind("Empno3") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="專科護理師" SortExpression="DrName3">
                        <EditItemTemplate>
                            <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("DrName3") %>'></asp:TextBox>
                        </EditItemTemplate>
                        <InsertItemTemplate>
                            <asp:TextBox ID="TextBox6" runat="server" Text='<%# Bind("DrName3") %>'></asp:TextBox>
                        </InsertItemTemplate>
                        <ItemTemplate>
                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("DrName3") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:CommandField ShowEditButton="True" />
                </Fields>
                <RowStyle HorizontalAlign="Center" />
            </asp:DetailsView>
            <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:E-boardConnectionString %>" DeleteCommand="DELETE FROM [DrInfo] WHERE [ID] = @ID" InsertCommand="INSERT INTO [DrInfo] ([EmPno1], [DrName1], [EmPno2], [DrName2], [Empno3], [DrName3]) VALUES (@EmPno1, @DrName1, @EmPno2, @DrName2, @Empno3, @DrName3)" SelectCommand="SELECT [ID], [EmPno1], [DrName1], [EmPno2], [DrName2], [Empno3], [DrName3] FROM [DrInfo] WHERE (([HospCode] = @HospCode) AND ([WardCode] = @WardCode))" UpdateCommand="UPDATE [DrInfo] SET [EmPno1] = @EmPno1, [DrName1] = @DrName1, [EmPno2] = @EmPno2, [DrName2] = @DrName2, [Empno3] = @Empno3, [DrName3] = @DrName3 WHERE [ID] = @ID">
                <DeleteParameters>
                    <asp:Parameter Name="ID" Type="Int32" />
                </DeleteParameters>
                <InsertParameters>
                    <asp:Parameter Name="EmPno1" Type="String" />
                    <asp:Parameter Name="DrName1" Type="String" />
                    <asp:Parameter Name="EmPno2" Type="String" />
                    <asp:Parameter Name="DrName2" Type="String" />
                    <asp:Parameter Name="Empno3" Type="String" />
                    <asp:Parameter Name="DrName3" Type="String" />
                </InsertParameters>
                <SelectParameters>
                    <asp:QueryStringParameter Name="HospCode" QueryStringField="HospCode" Type="String" />
                    <asp:QueryStringParameter Name="WardCode" QueryStringField="WardCode" Type="String" />
                </SelectParameters>
                <UpdateParameters>
                    <asp:Parameter Name="EmPno1" Type="String" />
                    <asp:Parameter Name="DrName1" Type="String" />
                    <asp:Parameter Name="EmPno2" Type="String" />
                    <asp:Parameter Name="DrName2" Type="String" />
                    <asp:Parameter Name="Empno3" Type="String" />
                    <asp:Parameter Name="DrName3" Type="String" />
                    <asp:Parameter Name="ID" Type="Int32" />
                </UpdateParameters>
            </asp:SqlDataSource>
        </div>  
        
    </div>
    </form>
</body>
</html>
