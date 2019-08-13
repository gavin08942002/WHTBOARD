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
    'MMH LOGO顯示控制
    Protected Sub Show_LOGO(ByVal Hospcode As String)
        Dim path As String = Nothing
        Select Case Hospcode
            Case 1, 2, 4
                path = "image/CHmmhlogo.jpg"
                LOGO_Image.Width = 240
                LOGO_Image.Height = 50
                LOGO_Image.ImageUrl = path
            Case 3
                path = "image/TTmmhlogo.jpg"
                LOGO_Image.Width = 210
                LOGO_Image.Height = 60
                LOGO_Image.ImageUrl = path

        End Select

    End Sub
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
    '顯示操作者姓名
    Protected Function Show_User() As String
        Dim ShowText As String = ""
        If Not (Request.Cookies("UserCookie") Is Nothing) Then
            If Not IsPostBack Then
                Dim show As HttpCookie = Request.Cookies("UserCookie")
                ShowText = show.Values("Name")
            End If
        End If
        Return ShowText
    End Function
    '超連結
    Protected Sub Hyperlink_Default(ByVal HospCode As String, ByVal WardCode As String)
        'HyperLink2.NavigateUrl = String.Format("Patients.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink3.NavigateUrl = String.Format("Operation.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink4.NavigateUrl = String.Format("Inout.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink5.NavigateUrl = String.Format("Nurses.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink6.NavigateUrl = String.Format("Doctors.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink7.NavigateUrl = String.Format("Contacts.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink8.NavigateUrl = String.Format("Info.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        'HyperLink1.NavigateUrl = String.Format("Index.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
    End Sub
    '依目前時間取得護理班別0700,1500,2300(可調整時間差)
    Public Function Get_Dancag(ByVal Hospcode As String, ByVal Wardcode As String, ByVal adjust_m As Integer) As String
        'adjust_m    切換時點的分鐘調整數
        '取得護理站班表切換時間
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM WHTBOARD  WHERE NS='{0}' AND EMPNO='DEN' ", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As New DataTable

        DA.Fill(DT)

        Dim D_Time As Integer = 700 '預設日班換班時間
        Dim E_Time As Integer = 1500 ' 預設中班換班時間
        Dim N_Time As Integer = 2300 ' 預設大夜換班時間
        If DT.Rows.Count > 0 Then
            Dim Den_LIST() As String = DT.Rows(0)("NAME").ToString.Split(",")
            D_Time = Convert.ToInt16(Den_LIST(0))
            E_Time = Convert.ToInt16(Den_LIST(1))
            N_Time = Convert.ToInt16(Den_LIST(2))
        End If
        Dim DanCag As String
        Dim NowHour As Integer = Now.AddMinutes(adjust_m).Hour
        Dim NowTime As Integer = Now.Hour * 100 + Now.Minute
        If NowTime >= N_Time Then   '夜班
            DanCag = "N"
        ElseIf NowTime < D_Time Then '夜班
            DanCag = "N"
        ElseIf NowTime >= D_Time And NowTime < E_Time Then '日班
            DanCag = "D"
        Else
            DanCag = "E"      '中班
        End If

        Return DanCag
    End Function
    '早班換班時間
    Public Function Get_TimeD(ByVal Hospcode As String, ByVal Wardcode As String) As Integer
        '取得護理站班表切換時間
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM WHTBOARD  WHERE NS='{0}' AND EMPNO='DEN' ", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As New DataTable

        DA.Fill(DT)

        Dim D_Time As Integer = 700 '預設日班換班時間
        If DT.Rows.Count > 0 Then
            Dim Den_LIST() As String = DT.Rows(0)("NAME").ToString.Split(",")
            D_Time = Convert.ToInt16(Den_LIST(0))
        End If
        Return D_Time
    End Function
    '晚班換班時間
    Public Function Get_TimeN(ByVal Hospcode As String, ByVal Wardcode As String) As Integer
        '取得護理站班表切換時間
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM WHTBOARD  WHERE NS='{0}' AND EMPNO='DEN' ", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As New DataTable

        DA.Fill(DT)

        Dim N_Time As Integer = 2300 '預設日班換班時間
        If DT.Rows.Count > 0 Then
            Dim Den_LIST() As String = DT.Rows(0)("NAME").ToString.Split(",")
            N_Time = Convert.ToInt16(Den_LIST(2))
        End If
        Return N_Time
    End Function


    Public Function Table_To_String(ByVal Temp_table As DataTable, ByVal columns As String) As String
        Dim Temp_String As String = ""
        For i = 0 To Temp_table.Rows.Count - 1
            If i <> 0 Then
                Temp_String += "/ "
            End If
            Temp_String += Right(Temp_table.Rows(i)(columns).ToString, 3)
        Next
        Return Temp_String
    End Function
    '計算病床數
    Public Function Bed_Count(ByVal Temp_Table As DataTable) As Integer
        Dim nobed As ArrayList = New ArrayList
        nobed.Add("組長")
        nobed.Add("行政")
        nobed.Add("書記")
        nobed.Add("助理員")


        Dim count As Integer = 0
        For i = 0 To Temp_Table.Rows.Count - 1
            If Not nobed.Contains(Temp_Table.Rows(i)("bedno").ToString) Then
                count += 1
            End If
        Next
        Return count
    End Function


    '!!!!!行動能力列表
    Protected Sub Activity_List(ByVal Hospcode As String, ByVal Wardcode As String)
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        '護理站行動能力全部資料
        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.Day()) ' 2015/07/13 00:00:00
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT B.bedsts, B.Bedns, B.Bedno,TS.activity,B.bedcaseno ,T.caseno, T.pno,  T.MAXUdate   ")
        cmd += String.Format(" FROM BedTBL B, TPRSheet TS")
        cmd += String.Format("      ,(SELECT  T.pno,T.caseno,MAX(T.udate)as MAXUdate  FROM TPRSheet T  WHERE T.Activity is not null   GROUP BY  T.pno,T.caseno) T ")
        cmd += String.Format(" WHERE        ")
        cmd += String.Format(" TS.pno = T. pno AND ")
        cmd += String.Format(" TS.caseno = T.caseno AND ")
        cmd += String.Format(" TS.udate = T.maxudate AND ")
        cmd += String.Format(" B.Bedpno = T.pno AND ")
        cmd += String.Format(" B.bedcaseno = T.caseno  AND ")
        cmd += String.Format(" B.bedns = '{0}' AND ", Wardcode)
        cmd += String.Format(" (B.bedsts = '1' OR (B.bedsts = '2' AND B.udendtime > to_date('{0}','YYYY/MM/DD HH24:MI:SS') )) AND ", Nowdate)
        cmd += String.Format(" Bedroom != '99' ")
        cmd += String.Format(" AND TS.Activity is not null ")
        cmd += String.Format(" AND TS.deleted = 'N' ") '修正以修改方式更改TPRSeet的資料時會有兩筆資料的問題
        cmd += String.Format(" order by B.bedno")

        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As New DataTable
        DA.Fill(DT)
        Dim st As String() = {"bedns", "bedno", "pno", "activity", "maxudate"}
        Dim DT_activity As DataTable = DT.DefaultView.ToTable(True, st)
        'GridView7.Caption = "行動能力SQL列表"
        'GridView7.DataSource = DT_activity
        'GridView7.DataBind()

        '行動的所有方式
        Dim Activity_Stytle As String() = {"步行", "輪椅", "推車", "抱", "娃娃車", "輸送保溫箱", "救助袋"}
        Dim Activity_Table As DataTable = New DataTable
        Activity_Table.Columns.Add("Activity")
        Activity_Table.Columns.Add("Bed")
        Activity_Table.Columns.Add("BedCount")
        '行動能力顯示排版
        For i = 0 To Activity_Stytle.Length - 1

            Dim SQL_s = String.Format("ACTIVITY = '{0}'", Activity_Stytle(i).ToString)
            DT_activity.DefaultView.RowFilter = SQL_s
            Dim Temp_table As DataTable = DT_activity.DefaultView.ToTable
            Dim Activity_row As DataRow = Activity_Table.NewRow
            If Bed_Count(Temp_table) > 0 Then
                Activity_row("Activity") = Activity_Stytle(i).ToString
                Activity_row("Bed") = Table_To_String(Temp_table, "Bedno")
                Activity_row("BedCount") = Bed_Count(Temp_table)
                Activity_Table.Rows.Add(Activity_row)
            End If
        Next
        'GridView8.Caption = "行動能力顯示列表"
        'GridView8.DataSource = Activity_Table
        'GridView8.DataBind()
        '行動能力最後顯示()
        Dim Activity_Show As String = ""
        For j = 0 To Activity_Table.Rows.Count - 1
            Dim Activity As String = Activity_Table.Rows(j)("Activity").ToString
            Dim Bedno As String = Activity_Table.Rows(j)("Bed").ToString
            Dim BedCount As Integer = Activity_Table.Rows(j)("bedcount")

            Activity_Show += String.Format("<tr class =""Table_tr_style1"">")
            Activity_Show += String.Format("    <td class=""Table_td_style"" >{0}</td>", Activity)
            Activity_Show += String.Format("    <td align=""left"" class=""Table_td_style"" >{0}</td>", Bedno)
            Activity_Show += String.Format("    <td class=""Table_td_style"" >{0}</td>", BedCount)
            Activity_Show += String.Format("</tr>")

        Next
        Activity_Show_All.Text = Activity_Show
    End Sub

    '主程式
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim Empno As String = Request.QueryString("Empno")
        Dim ConnStr As String = SELECT_ORACLE(HospCode)
        'MMH LOGO顯示控制
        Show_LOGO(HospCode)
        '超連結設定
        Hyperlink_Default(HospCode, WardCode)
        '顯示病房名稱, 查詢時間, 操作者
        Ward_Name_Label.Text = Title_default(HospCode, WardCode, ConnStr)
        User_Label.Text = Show_User()

        Dim Dancag As String = Get_Dancag(HospCode, WardCode, 0) '控制護理站名稱和護士班表切換時點
        User_Label.Text = Show_User()

        'Nurse_List(HospCode, WardCode, Dancag)
        Activity_List(HospCode, WardCode)
    End Sub
End Class
