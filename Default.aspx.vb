Imports System
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.DynamicData
Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim strArray() As String = {"Visual", "Basic", "VB", "Net", "Framework", "ASP.net"}
        Dim message As String
        For index1 = 0 To strArray.GetUpperBound(0)
            message += CStr(strArray(index1)) + ","

        Next

        Label2.Text = "陣列數為" & CInt(strArray.Count())
        Label1.Text = message
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim ws As ServiceReference1.RB_WSSoapClient = New ServiceReference1.RB_WSSoapClient()
        Dim ws2 As ServiceReference2.ServiceSoapClient = New ServiceReference2.ServiceSoapClient()

        Dim kDataSet As New DataSet
        Dim kDatable As DataTable = New DataTable("GetDisList")
        Dim Hospcode As String = "1"
        Dim Wardcode As String = "17"
        Dim Pno As String = "*" '八碼
        Dim test As String

        
        kDataSet = ws.Get_NSInfo(Hospcode, Wardcode, Pno)
        Label4.Text = "資料筆數：" & kDataSet.Tables(0).Rows.Count
        Label3.Text = "資料欄位數" & kDataSet.Tables(0).Columns.Count

        '列出所有欄位名稱()
        For index = 0 To ((kDataSet.Tables(0).Columns.Count) - 1)
            test &= kDataSet.Tables(0).Columns(index).ColumnName & "-" & kDataSet.Tables(0).Rows(0)(index) & ","
        Next
        Response.Write(test)


        'kDataSet = ws.Get_EmpInfo(1057)
        'Label4.Text = kDataSet.Tables(0).Rows(0)(1)


        'Get_NSInfo 測試成功

        'kDataSet = ws.Get_NSInfo(1, 40, "*")
        'Label4.Text = kDataSet.Tables(0).Rows(15)("PATIENT_NAME")
        ' Label3.Text = "資料列數" & kDataSet.Tables(0).Columns.Count & kDataSet.Tables(0).Rows(20)(30)

        'Label3.Text = "資料列數:" & kDatable.Columns.Count
        'Label4.Text = "資料行數：" & ws2.GetDisList("01", "012").Rows.Count
        ' Label4.Text = "資料行數：" & kDatable.Rows(1)(1)
        'Dim workrow As DataRow
        'Dim mycolumn As New DataColumn
        'mycolumn.DataType = System.Type.GetType("System.Int32")
        'kDatable.Columns.Add(mycolumn)

        'kDatable.Rows.Add(kDatable.NewRow())
        'kDatable.Rows.Add(kDatable.NewRow())
        'kDatable.Rows(0)(0) = 1
        'kDatable.Rows(1)(0) = 2


        'Label4.Text = "資料行數：" & kDatable.Rows.Count
        'Label3.Text = "資料列數:" & kDatable.Columns.Count




        ws2.Close()



    End Sub
End Class
