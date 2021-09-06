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
Partial Class Web_Default
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
    '護理站名稱
    Protected Function Title_default(ByVal HospCode As String, ByVal WardCode As String, ByVal ConnStr As String) As String
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM NSTBL  WHERE NS='{0}' ", WardCode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataTable
        Dim ContactInfo As String = Nothing
        DA.Fill(DS) '連絡人資料
        If DS.Rows.Count > 0 Then
            Return DS.Rows(0)("nseng")
        End If

    End Function
    Protected Function Show_User() As String
        Dim User_Name As String = "Test"
        Return User_Name
    End Function
    '權限控管

    '主程式
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim Empno As String = Request.QueryString("Empno")
        Dim ConnStr As String = SELECT_ORACLE(HospCode)

        '顯示病房名稱, 查詢時間, 操作者
        'Ward_Name_Label.Text = Title_default(HospCode, WardCode, ConnStr)
        'User_Label.Text = Show_User()



    End Sub

    Protected Sub Page_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad

        Dim empno As String = Request.QueryString("empno")
        empno = empno.ToUpper()
        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim URL_Name As String = "TRM-DL.aspx?Hospcode=" & HospCode
        '程式檔名
        Dim Ap_name As String = "CALL_MAR"
        Dim UserCookie As HttpCookie = New HttpCookie("UserCookie")

        Dim ConnStr As String = SELECT_ORACLE(HospCode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = ""
        cmd += String.Format("SELECT * FROM V_Usrseculist WHERE Empno = '{0}' AND apname = '{1}'", empno, Ap_name)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As New DataTable
        DA.Fill(DT)
        'GridView1.DataSource = DT
        ' GridView1.DataBind()

        If DT.Rows.Count > 0 Then
            UserCookie.Values.Add("Enter", "Y")
            UserCookie.Values.Add("Empno", DT.Rows(0)("Empno").ToString)
            UserCookie.Values.Add("Name", DT.Rows(0)("Name").ToString)
            UserCookie.Values.Add("secu", DT.Rows(0)("secu").ToString)
        Else
            UserCookie.Values.Add("Enter", "N")
            Response.Write("沒有使用權限!!")
            Response.End()

        End If
        Response.Cookies.Add(UserCookie)
        Response.Redirect(URL_Name)
    End Sub
End Class
