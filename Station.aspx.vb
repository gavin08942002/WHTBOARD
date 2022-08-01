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

Imports NPOI
Imports NPOI.XSSF.UserModel
Imports NPOI.POIFS
Imports NPOI.POIFS.FileSystem
Imports NPOI.Util

Partial Class Station
    Inherits System.Web.UI.Page
    '取目前ORACLE系統時間
    Public Function GET_Now_Time(Hospcode As String) As DateTime
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd_Time As String = "select sysdate FROM DUAL "
        Dim DA_Time As OleDbDataAdapter = New OleDbDataAdapter(cmd_Time, Conn)
        Dim DT_Time As New DataTable
        DA_Time.Fill(DT_Time)
        Return DT_Time.Rows(0)("sysdate")
    End Function
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
            Case "5"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=cc_opd_ord; persist security info=true;password=his1160"
            Case "0"
                OraStr = "Provider=OraOLEDB.Oracle;user id=dr;data source=hcora2; persist security info=true;password=mmhedp"
        End Select
        Return OraStr
    End Function
    '##########判斷護理站代號,決定開啟兒科版本或一般護理站版本
    Public Function distinguish_URL(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Now_time As DateTime, ByVal ifkey As String, MSTR As String) As String
        Dim P_LIST() As String = {"16", "17"} '顯示小兒科版本的護理站代號

        Dim uri As New Uri(Request.Url.AbsoluteUri)
        Dim collection As NameValueCollection = HttpUtility.ParseQueryString(uri.Query, System.Text.Encoding.UTF8)
        '預設指向台北護理站版本
        'Dim URL As String = String.Format("station-D.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection.Get("Wardcode"))
        Dim URL As String = Get_week(Hospcode, Wardcode, Now_time, ifkey)

        Select Case Hospcode
            '台北
            Case "1"
                '台北小兒科版本 station-p.aspx
                If P_LIST.Contains(Wardcode) And ifkey = "Y" Then '時間測試
                    URL = String.Format("station-P.aspx?Hospcode={0}&Wardcode={1}&time= {2}", collection.Get("Hospcode"), collection.Get("Wardcode"), Now_time.ToString("yyyy/MM/dd hh:mm:ss"))
                ElseIf P_LIST.Contains(Wardcode) Then
                    URL = String.Format("station-P.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection.Get("Wardcode"))
                End If



                '淡水
            Case "2"


                '台東
            Case "3"
                '台東版本station-TT.aspx
                URL = String.Format("station-TT.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection.Get("Wardcode"))


                '新竹
            Case "4"
                '新竹版本station-CH.aspx
                If ifkey = "Y" Then '時間測試
                    URL = String.Format("station-CH.aspx?Hospcode={0}&Wardcode={1}&time= {2}", collection.Get("Hospcode"), collection.Get("Wardcode"), Now_time.ToString("yyyy/MM/dd hh:mm:ss"))
                Else
                    URL = String.Format("station-CH.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection.Get("Wardcode"))
                End If
            Case "5"
                URL = String.Format("station-CH.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection.Get("Wardcode"))

        End Select

        Return URL

    End Function
    '依星期及時間指向station-D.aspx或station-N.aspx
    Public Function Get_week(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Now_Time As DateTime, ByVal ifkey As String)
        Response.Write(Now_Time)
        Dim test As String = Request.QueryString("test")
        Dim Btime As Integer = Nothing
        Dim Etime As Integer = Nothing
        Dim URL = Nothing
        Dim Todaytime As Integer = Now_Time.Hour * 100 + Now_Time.Minute
        Dim URL_FILE_NAME As String = Nothing '依時間判斷應指向檔名
        Dim NOW_URL_FILE_NAME As String = System.IO.Path.GetFileName(Request.PhysicalPath).ToString.ToLower '目網頁顯示檔案名
        If Holiday(Now_Time) Then
            URL_FILE_NAME = "station-N.aspx"
        Else
            Select Case Weekday(Now)
                Case 2, 3, 4, 5, 6 '星期一至五
                    Btime = 800
                    Etime = 1700
                    If Todaytime >= Btime And Todaytime < Etime Then
                        URL_FILE_NAME = "station-D.aspx"
                    Else
                        URL_FILE_NAME = "station-N.aspx"
                    End If
                Case 7  '星期六
                    Btime = 800
                    Etime = 1200
                    If Todaytime >= Btime And Todaytime < Etime Then
                        URL_FILE_NAME = "station-D.aspx"
                    Else
                        URL_FILE_NAME = "station-N.aspx"
                    End If
                Case 1 '星期日
                    URL_FILE_NAME = "station-N.aspx"
            End Select
        End If


        If String.Compare(NOW_URL_FILE_NAME.ToUpper, URL_FILE_NAME.ToUpper) And ifkey = "Y" Then
            URL = String.Format("{0}?Hospcode={1}&Wardcode={2}&time= {3}", URL_FILE_NAME, Hospcode, Wardcode, Now_Time.ToString("yyyy/MM/dd hh:mm:ss"))
        Else
            URL = String.Format("{0}?Hospcode={1}&Wardcode={2}", URL_FILE_NAME, Hospcode, Wardcode)
        End If

        If Not String.IsNullOrEmpty(URL) Then
            Return URL
        End If
    End Function
    '判斷今日是否為馬偕假日 true , false
    Public Function Holiday(ByVal Now_Time As DateTime) As Boolean
        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim Nowdate As String = Now_Time.ToString("yyyy/MM/dd 00:00:00") ' 2015/07/13 00:00:00
        Dim ConnStr As String = SELECT_ORACLE(HospCode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT * FROM HOLIDAY WHERE Offdate = '{0}' ", Nowdate)
        Dim DA_state As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT_state As DataTable = New DataTable
        DA_state.Fill(DT_state)

        If DT_state.Rows.Count > 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    '取得護理站的主責科別
    Public Function Get_Master(ByVal Hospcode As String, ByVal Wardcode As String) As String
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim DT As DataTable = New DataTable
        Dim HD_cmd As String = String.Format("SELECT A.NAME  FROM WHTBOARD A  WHERE A.NS='{0}' and A.GRP='主責科別' and A.empno='mstr' ", Wardcode)
        Dim HD_DA As OleDbDataAdapter = New OleDbDataAdapter(HD_cmd, Conn)
        HD_DA.Fill(DT)

        Dim Master As String = Nothing
        If DT.Rows.Count > 0 Then
            Master = DT.Rows(0)("NAME").ToString
        End If
        If (Conn.State = ConnectionState.Open) Then
            Conn.Close()
            Conn.Dispose()
        End If
        Return Master




    End Function

    '主程式
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim Now_Time As DateTime = GET_Now_Time(HospCode)
        Dim key_time As String = Request.QueryString("time")
        Dim ifkey As String = "N"
        Dim MSTR As String = Get_Master(HospCode, WardCode)
        If Not String.IsNullOrEmpty(key_time) Then
            Now_Time = DateTime.Parse(key_time)
            ifkey = "Y"
            '  Now_Time = Convert.ToDateTime(key_time, )
            Response.Write("測試時間:" & Now_Time.ToString("yyyy/MM/dd hh:mm:ss"))
        End If
        Dim URL As String = distinguish_URL(HospCode, WardCode, Now_Time, ifkey, MSTR)
        Response.Redirect(URL)
    End Sub
End Class
