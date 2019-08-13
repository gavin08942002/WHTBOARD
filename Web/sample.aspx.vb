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
    Protected Sub Hyperlink_Default(ByVal HospCode As String, ByVal WardCode As String)
        ' HyperLink2.NavigateUrl = String.Format("Patients.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink3.NavigateUrl = String.Format("Operation.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        '  HyperLink4.NavigateUrl = String.Format("Inout.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink5.NavigateUrl = String.Format("Nurses.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        ' HyperLink6.NavigateUrl = String.Format("Doctors.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink7.NavigateUrl = String.Format("Contacts.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        ' HyperLink8.NavigateUrl = String.Format("Info.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink1.NavigateUrl = String.Format("Index.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim Empno As String = Request.QueryString("Empno")
        Dim ConnStr As String = SELECT_ORACLE(HospCode)
        '超連結設定
        Hyperlink_Default(HospCode, WardCode)
        '顯示病房名稱, 查詢時間, 操作者
        Ward_Name_Label.Text = Title_default(HospCode, WardCode, ConnStr)
        User_Label.Text = Show_User()

        
    End Sub
End Class
