<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Test_CKEditor.aspx.vb" Inherits="Test_CKEditor" %>

<%@ Register assembly="CKEditor.NET" namespace="CKEditor.NET" tagprefix="CKEditor" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="ckeditor/ckeditor.js"></script> 

    <title></title>

    <style type="text/css">
        #TextArea1 {
            height: 487px;
            width: 1014px;
        }
    </style>

</head>
<body>
    <form id="form1" runat="server">
    <div style="height: 430px">
        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
        <asp:Button ID="Button1" runat="server" Text="Button" />
        <CKEditor:CKEditorControl ID="CKEditorControl1" runat="server" Width="828px" Toolbar="simple"></CKEditor:CKEditorControl>
        <asp:Button ID="Button2" runat="server" Text="Button" />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:E-boardConnectionString %>" DeleteCommand="DELETE FROM [BoardInfo] WHERE [ID] = @ID" InsertCommand="INSERT INTO [BoardInfo] ([HospCode], [WardCode], [BoardText]) VALUES (@HospCode, @WardCode, @BoardText)" SelectCommand="SELECT * FROM [BoardInfo] WHERE (([HospCode] = @HospCode) AND ([WardCode] = @WardCode))" UpdateCommand="UPDATE [BoardInfo] SET [HospCode] = @HospCode, [WardCode] = @WardCode, [BoardText] = @BoardText WHERE [ID] = @ID">
            <DeleteParameters>
                <asp:Parameter Name="ID" Type="Int32" />
            </DeleteParameters>
            <InsertParameters>
                <asp:Parameter Name="HospCode" Type="String" />
                <asp:Parameter Name="WardCode" Type="String" />
                <asp:Parameter Name="BoardText" Type="String" />
            </InsertParameters>
            <SelectParameters>
                <asp:QueryStringParameter Name="HospCode" QueryStringField="hospcode" Type="String" />
                <asp:QueryStringParameter Name="WardCode" QueryStringField="wardcode" Type="String" />
            </SelectParameters>
            <UpdateParameters>
                <asp:Parameter Name="HospCode" Type="String" />
                <asp:Parameter Name="WardCode" Type="String" />
                <asp:Parameter Name="BoardText" Type="String" />
                <asp:Parameter Name="ID" Type="Int32" />
            </UpdateParameters>
        </asp:SqlDataSource>
        </p>
        <p>
            <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
        </p>
    </div>
        <script>
            // Replace the <textarea id="editor1"> with a CKEditor
            // instance, using default configuration.
            
            CKEDITOR.replace('TextBox1');
            </script>
    </form>
    <p>
        
</body>
</html>
