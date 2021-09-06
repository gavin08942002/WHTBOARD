Imports System.Data
Imports System.Data.SqlClient


Partial Class Test_CKEditor
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim HospCode As Integer = Request.QueryString("HospCode")
        Dim WardCode As Integer = Request.QueryString("WardCode")
        Dim conn As SqlConnection = New SqlConnection
        conn.ConnectionString = "Data Source=.\sqlexpress;Initial Catalog=E-board;User ID=test;Password=test"
        conn.Open()
        Dim StrSelect As String = String.Format("SELECT [ID],[HospCode],[WardCode],[BoardText] FROM [BoardInfo]  ")
        Dim StrInsert As String = String.Format("INSERT INTO [BoardInfo] ([HospCode],[WardCode],[BoardText])VALUES({0},{1},'{2}')", HospCode, WardCode, CKEditorControl1.Text)
        Dim StrUpdate As String = String.Format("UPDATE [BoardInfo] SET [BoardText] = '{2}'  WHERE [HospCode] ={0} AND [WardCode]={1}", HospCode, WardCode, CKEditorControl1.Text)
        Dim cmd As SqlCommand = New SqlCommand(StrUpdate, conn)
        Dim cmdInsert As SqlCommand = New SqlCommand(StrInsert, conn)
        'Dim dr As SqlDataReader = cmd.EndExecuteReader()
        If cmd.ExecuteNonQuery = 0 Then
            cmdInsert.ExecuteNonQuery()
            cmdInsert.Cancel()
        End If
        cmd.Cancel()
        conn.Close()
        'Response.Write(TextBox1.Text)
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim HospCode As String = Request.QueryString("HospCode")
        Dim WardCode As String = Request.QueryString("WardCode")
        If (0) Then
            Label1.Text = Me.CKEditorControl1.Text
            SqlDataSource1.InsertParameters.Item(0).DefaultValue = HospCode
            SqlDataSource1.InsertParameters.Item(1).DefaultValue = WardCode
            SqlDataSource1.InsertParameters.Item(2).DefaultValue = CKEditorControl1.Text
            SqlDataSource1.Insert()
        Else
            'SqlDataSource1.UpdateParameters.Item(2).DefaultValue = CKEditorControl1.Text
            'SqlDataSource1.Update()
            'Response.Write("test")
        End If


    End Sub




    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


    End Sub
End Class
