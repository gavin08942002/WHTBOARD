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
        HyperLink2.NavigateUrl = String.Format("Patients.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink3.NavigateUrl = String.Format("Operation.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink4.NavigateUrl = String.Format("Inout.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink5.NavigateUrl = String.Format("Nurses.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink6.NavigateUrl = String.Format("Doctors.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink7.NavigateUrl = String.Format("Contacts.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink8.NavigateUrl = String.Format("Info.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink1.NavigateUrl = String.Format("Index.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
    End Sub
    ' 性名轉中文(F 女 M 男)
    Public Function Adjust_Gender(ByVal PATGENDER As String) As String
        Dim ChGender As String = Nothing
        Select Case PATGENDER
            Case "F"
                ChGender = "女"
            Case "M"
                ChGender = "男"
        End Select
        Return ChGender
    End Function
    '入院資料處理
    Public Function IN_DEAL(ByVal InTable As DataTable) As String
        'InTable =入院資料DataTable
        Dim InStrList As String = Nothing '入院資料
        For i = 0 To InTable.Rows.Count - 1
            Dim BEDNO As String = Right(InTable(i)("BEDNO").ToString, 3)
            Dim STATUS As String = InTable(i)("STATUS").ToString
            Dim PATPNO As String = InTable(i)("PATPNO").ToString
            Dim PATNAME As String = InTable(i)("PATNAME").ToString
            Dim PATGENDER As String = Adjust_Gender(InTable(i)("PATGENDER").ToString)
            Dim PATAGE As String = InTable(i)("PATAGE").ToString
            Dim VSEMPNO As String = InTable(i)("VSEMPNO").ToString
            Dim VSNAME As String = InTable(i)("VSNAME").ToString
            Dim MAINDIAGNOSIS As String = InTable(i)("MAINDIAGNOSIS").ToString


            InStrList &= String.Format("<tr class =""Table_tr_style2"">")
            InStrList &= String.Format("<td class=""Table_td_style"" style=""width:5%;"">{0}</td>", BEDNO)
            InStrList &= String.Format("<td class=""Table_td_style"" style=""width:5%;"">{0}</td>", STATUS)
            InStrList &= String.Format("<td class=""Table_td_style"" style=""width:10%;"">{0}</td>", PATPNO)
            InStrList &= String.Format("<td class=""Table_td_style"" style=""width:10%;"">{0}</td>", PATNAME)
            InStrList &= String.Format("<td class=""Table_td_style"" style=""width:5%;"">{0}</td>", PATGENDER)
            InStrList &= String.Format("<td class=""Table_td_style"" style=""width:5%;"">{0}</td>", PATAGE)
            InStrList &= String.Format("<td class=""Table_td_style"" style=""width:12%;"">{0}/{1}</td>", VSNAME, VSEMPNO)
            InStrList &= String.Format("<td class=""Table_td_style"" style=""width:35%;"">{0}</td>", MAINDIAGNOSIS)
            InStrList &= String.Format("<td class=""Table_td_style"" style=""width:13%;"">{0}</", "TEST")
            InStrList &= String.Format("</tr>")
        Next
        Return InStrList
    End Function
    '出院資料處理
    Public Function OUT_DEAL(ByVal OutTable As DataTable) As String
        Dim OutStrList As String = Nothing
        For i = 0 To OutTable.Rows.Count - 1
            Dim PATPNO As String = OutTable.Rows(i)("PATPNO").ToString
            Dim PATNAME As String = OutTable.Rows(i)("PATNAME").ToString
            Dim BEDNO As String = Right(OutTable.Rows(i)("BEDNO"), 3).ToString
            Dim REMARK As String = OutTable.Rows(i)("REMARK").ToString

            OutStrList &= String.Format("<tr class =""Table_tr_style2"">")
            OutStrList &= String.Format("<td class=""Table_td_style"" style=""width:5%;"">{0}</td>", BEDNO)
            OutStrList &= String.Format("<td class=""Table_td_style"" style=""width:5%;"">{0}</td>", "註記")
            OutStrList &= String.Format("<td class=""Table_td_style"" style=""width:10%;"">{0}</td>", PATPNO)
            OutStrList &= String.Format("<td class=""Table_td_style"" style=""width:10%;"">{0}</td>", PATNAME)
            OutStrList &= String.Format("<td class=""Table_td_style"" style=""width:70%;"">{0}</td", "備註")
            OutStrList &= String.Format("</tr>")



        Next
        Return OutStrList
    End Function
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim Empno As String = Request.QueryString("Empno")
        Dim ConnStr As String = SELECT_ORACLE(HospCode)
        '超連結設定
        Hyperlink_Default(HospCode, WardCode)
        '顯示病房名稱, 查詢時間, 操作者
        Ward_Name_Label.Text = Title_default(HospCode, WardCode, ConnStr)
        Update_Time_Label.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        User_Label.Text = Show_User()
        Dim ws2 As ServiceReference2.ServiceSoapClient = New ServiceReference2.ServiceSoapClient()
        Dim InTable As DataTable = ws2.GetTrnsList(HospCode, WardCode)   '入院資料
        Dim OutTable As DataTable = ws2.GetDisList(HospCode, WardCode) '出院資料
        Dim INCount As Integer = InTable.Rows.Count '入院人數
        Dim OUTCount As Integer = OutTable.Rows.Count '出院人數
        Dim Total As Integer '總計

        
        IN_Label.Text = IN_DEAL(InTable) '入院資料處理
        OUTLabel.Text = OUT_DEAL(OutTable) '出院資料處理

        GridView1.DataSource = InTable
        GridView1.DataBind()
        GridView2.DataSource = OutTable
        GridView2.DataBind()



    End Sub
End Class
