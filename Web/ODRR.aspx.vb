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
Partial Class Web_ODRR
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
        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        '#####################生理排程資料撈取
        Dim ConnStr As String = SELECT_ORACLE("4")
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim date1 As Date = Now
        'Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", date1.Year(), date1.Month(), date1.Day()) ' 2015/07/13 00:00:00
        'Dim Nowdateadd As String = String.Format("{0}/{1}/{2} 00:00:00", date1.Year(), date1.Month(), date1.AddDays(1).Day)
        Dim cmd As String = String.Format(" SELECT pno,rdate,type,stype,replace(replace(ODRR.text,chr(32),'&nbsp&nbsp'),chr(10),'<br>' )as text ")
        cmd += String.Format(" FROM ODRR")
        cmd += String.Format(" WHERE ")
        cmd += String.Format(" effect= 'Y' AND pno ='60272250' ")
        cmd += String.Format(" ORDER BY rdate ")


        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT_PHY_SCH As New DataTable
        DA.Fill(DT_PHY_SCH)
        Dim test As String = DT_PHY_SCH.Rows(16)("text").ToString
        Response.Write(test)
        GridView2.Caption = "檢查排程列表"
        GridView2.DataSource = DT_PHY_SCH
        GridView2.DataBind()
    End Sub
End Class
