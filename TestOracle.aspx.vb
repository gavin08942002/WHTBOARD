Imports System
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.DynamicData
Imports System.Data.OleDb
Imports System.Collections.Generic
Imports System.Windows.Forms


Partial Class TestOracle
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load










    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim HospCode As String = "1"
        Dim WardCode As String = "17"
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = "Provider=OraOLEDB.Oracle;user id=dr;data source=hcora2; persist security info=true;password=mmhedp"
        Conn.Open()
        Dim cmd As String = String.Format("INSERT INTO WHTBOARD_SCHED(SCHED_ID, Bdate, Edate, NS, AD_EMPNO, AD_NAME, HD_EMPNO, HD_NAME, UDATE) VALUES ('{0}',to_date('2015/2/1 00:00:00','YYYY/MM/DD HH24:MI:SS'),to_date('2015/2/16 23:59:59','YYYY/MM/DD HH24:MI:SS'),'17','2222','王天祥','1111','朱八戒',to_date(sysdate,'YYYY/MM/DD HH24:MI:SS'))", TextBox1.Text)
        Dim DC As OleDbCommand = New OleDbCommand(cmd, Conn)
        DC.ExecuteNonQuery()
        Conn.Close()
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim test() As String = {"san", "t", "FF", "san"}
        Dim test2() As String = Nothing

        'MessageBox.Show("test")
        For Each j As String In test.Distinct.ToArray()
            test2 = {j}
        Next

        For i = 0 To (test2.Count - 1)
            Response.Write(test2(i) & ",")
        Next
        Response.Write(test2.Count)
    End Sub
End Class
