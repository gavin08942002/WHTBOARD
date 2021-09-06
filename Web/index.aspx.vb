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
Partial Class Web_index
    Inherits System.Web.UI.Page
    Public Function SELECT_ORACLE(ByVal str As String) As String
        Dim OraStr As String = Nothing

        Select Case str
            Case "1"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=tp_opd_ord; persist security info=true;password=his1160"
            Case "2"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=ts_opd_ord; persist security info=true;password=his1160"
            Case "3"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=tt_opd_ord; persist security info=true;password=his1160"
            Case "4"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=hc_opd_ord; persist security info=true;password=his1160"
        End Select
        Return OraStr
    End Function
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim Hospcode = Request.QueryString("Hospcode").ToString
        Dim Wardcode = Request.QueryString("Wardcode").ToString
        Dim UserID = Request.QueryString("UserID").ToString.ToUpper
        Dim UserCookie As HttpCookie = New HttpCookie("UserCookie")
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)

        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM PSN WHERE Empno  = '{0}'", UserID)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As New DataTable
        DA.Fill(DT) '護理人員資料

        If DT.Rows.Count > 0 Then
            UserCookie.Values.Add("Empno", DT.Rows(0)("Empno").ToString)
            UserCookie.Values.Add("Name", DT.Rows(0)("Namec").ToString)
            UserCookie.Values.Add("DEPT", DT.Rows(0)("DEPT").ToString)
            Response.Cookies.Add(UserCookie)
        End If
        'GridView1.Caption = "員工資料"
        'GridView1.DataSource = DT
        'GridView1.DataBind()

        ' Response.Write(UserCookie.Values("Empno"))
        'Response.Write(UserCookie.Values("Name"))
        Dim page As String = String.Format("<script>location.href =('Operation.aspx?HospCode={0}&WardCode={1}'); </script>", Hospcode, Wardcode)
        Response.Write(page)
    End Sub
End Class
