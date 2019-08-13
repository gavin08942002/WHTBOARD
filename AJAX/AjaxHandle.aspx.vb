Imports System
Imports System.IO
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.Web.Services
Imports System.Web.DynamicData
Imports System.Data.OleDb
Imports System.Windows.Forms
Imports System.Collections
Partial Class AJAX_AjaxHandle
    Inherits System.Web.UI.Page
    'ORACLE字串判斷
    Public Function SELECT_ORACLE(ByVal str As String) As String
        Dim OraStr As String = Nothing
        Select Case str
            Case "1"
                OraStr = "Provider=OraOLEDB.Oracle;user id=ord;data source=tp_opd_ord; persist security info=true;password=mmhedp"
            Case "2"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=ts_opd_ord; persist security info=true;password=his1160"
            Case "3"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=tt_opd_ord; persist security info=true;password=his1160"
            Case "4"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=hc_opd_ord; persist security info=true;password=his1160"
            Case "0"
                OraStr = "Provider=OraOLEDB.Oracle;user id=dr;data source=hcora2; persist security info=true;password=mmhedp"
        End Select
        Return OraStr
    End Function
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim ConnStr As String = SELECT_ORACLE("4")
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  BEDNS,BEDNO,BEDROOM,BEDPNO FROM BedTBL  WHERE bedNS='{0}' ", "38")
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As New DataTable
        DA.Fill(DT)
        GridView1.Caption = "40病房的病房表"
        GridView1.DataSource = DT
        GridView1.DataBind()

        Dim ID As String = Request.Form("ID")
        Dim name As String = Request.Form("name")
        Dim password As String = Request.Form("password")

        Response.Write(name & "<p>")
        Response.Write(password)
    End Sub
End Class
