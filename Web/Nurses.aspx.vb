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
            Case "5"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=cc_opd_ord; persist security info=true;password=his1160"
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


    '!!!!!護士排班列表
    Public Function Nurse_List(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Dancag As String) As String
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim ConnORA As String = SELECT_ORACLE(0)
        Dim date1 As Date = Now
        If Not String.IsNullOrEmpty(Request.QueryString("Todate")) Then
            date1 = Request.QueryString("Todate")
        End If




        'Dim TestDate As Date = 

        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", date1.Year(), date1.Month(), date1.Day()) ' 2015/07/13 00:00:00
        Dim Nowtime As String = String.Format("{0}/{1}/{2} {3}:00:00", date1.Year(), date1.Month(), date1.Day(), date1.AddHours(0).Hour) '2015/07/13 11:00:00
        Dim NowHour As Integer = Now.Hour
        Dim NOWT As Integer = Now.Hour * 100 + Now.Minute
        Dim addDay As Integer = 5
        '開始時間
        Dim FromDate As String = String.Format("{0}/{1}/{2} 00:00:00", date1.Year(), date1.Month(), date1.Day())
        '結束時間
        Dim ToDate As String = String.Format("{0}/{1}/{2} 00:00:00", date1.AddDays(1).Year(), date1.AddDays(1).Month(), date1.AddDays(1).Day)


        '值班護士名單資料
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  DISTINCT (AL1.EMPNO), AL1.DEN, AL1.fdate, AL1.DUTY, AL1.XFIRE, AL1.NAME, AL1.BACKUP1 FROM MMH.NSASSIST AL1 WHERE AL1.NS='{0}' AND AL1.FDATE >= to_date('{1}','YYYY/MM/DD HH24:MI:SS') AND AL1.FDATE < to_date('{2}','YYYY/MM/DD HH24:MI:SS') AND AL1.EFFECT='Y' ORDER BY AL1.DUTY", Wardcode, FromDate, ToDate)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataSet
        DA.Fill(DS, "NurseList") '護理人員資料


        'GridView1.Caption = "全部護士列表"
        'GridView1.DataSource = DS.Tables("NurseList")
        'GridView1.DataBind()

        '病床資料
        Dim cmd2 As String = String.Format("SELECT  AL1.EMPNO,AL1.den, AL1.fdate , AL1.Bedno, AL1.Pno FROM MMH.NSASSIST AL1 WHERE AL1.NS='{0}'  AND AL1.FDATE >= to_date('{2}','YYYY/MM/DD HH24:MI:SS') AND AL1.FDATE < to_date('{3}','YYYY/MM/DD HH24:MI:SS') AND AL1.EFFECT='Y'", Wardcode, Dancag, FromDate, ToDate)
        Dim DA2 As OleDbDataAdapter = New OleDbDataAdapter(cmd2, Conn)
        DA2.Fill(DS, "BedList") '病床資料



        'GridView2.Caption = "全部病床列表"
        'GridView2.DataSource = DS.Tables("BedList")
        'GridView2.DataBind()
        '####護士班表初始化
        '護士D班
        DS.Tables("NurseList").DefaultView.RowFilter = "Den = 'D' OR Den = 'H'"
        DS.Tables("NurseList").DefaultView.Sort = "DEN"
        Dim NurstList_D As DataTable = DS.Tables("NurseList").DefaultView.ToTable
        '護士E班
        DS.Tables("NurseList").DefaultView.RowFilter = "Den = 'E' "
        DS.Tables("NurseList").DefaultView.Sort = "DEN"
        Dim NurstList_E As DataTable = DS.Tables("NurseList").DefaultView.ToTable
        '護士N班
        DS.Tables("NurseList").DefaultView.RowFilter = "Den = 'N'"
        DS.Tables("NurseList").DefaultView.Sort = "DEN"
        Dim NurstList_N As DataTable = DS.Tables("NurseList").DefaultView.ToTable
        'GridView3.Caption = "護士班表D"
        'GridView3.DataSource = NurstList_D
        'GridView3.DataBind()
        '####
        '****病床初始化
        'D班病床資料
        DS.Tables("BedList").DefaultView.RowFilter = "Den = 'D' "
        DS.Tables("BedList").DefaultView.Sort = "Bedno"
        Dim BedList_D As DataTable = DS.Tables("BedList").DefaultView.ToTable
        'E班病床資料
        DS.Tables("BedList").DefaultView.RowFilter = "Den = 'E'"
        DS.Tables("BedList").DefaultView.Sort = "Bedno"
        Dim BedList_E As DataTable = DS.Tables("BedList").DefaultView.ToTable
        'N班病床資料
        DS.Tables("BedList").DefaultView.RowFilter = "Den = 'N'"
        DS.Tables("BedList").DefaultView.Sort = "Bedno"
        Dim BedList_N As DataTable = DS.Tables("BedList").DefaultView.ToTable

        'GridView4.Caption = "病床表N"
        'GridView4.DataSource = BedList_N
        'GridView4.DataBind()
        '****
        Dim NurseSortList_D As DataTable = NsBedSort(NurstList_D, BedList_D)
        NurseSortList_D.Columns.Add("NurseBedList") '新增病床列表欄位
        NurseSortList_D.Columns.Add("BedCount")
        Dim NurseSortList_E As DataTable = NsBedSort(NurstList_E, BedList_E)
        NurseSortList_E.Columns.Add("NurseBedList") '新增病床列表欄位
        NurseSortList_E.Columns.Add("BedCount")
        Dim NurseSortList_N As DataTable = NsBedSort(NurstList_N, BedList_N)
        NurseSortList_N.Columns.Add("NurseBedList") '新增病床列表欄位
        NurseSortList_N.Columns.Add("BedCount")

        'GridView5.Caption = "護士依病床排序列表"
        'GridView5.DataSource = NurseSortList_E
        'GridView5.DataBind()



        '@@@@D護士班表病床資料結合
        For i = 0 To NurseSortList_D.Rows.Count - 1
            Dim SQL_D As String = String.Format("EMPNO = '{0}'", NurseSortList_D.Rows(i)("EMPNO").ToString)
            BedList_D.DefaultView.RowFilter = SQL_D
            Dim Table_Temp As DataTable = BedList_D.DefaultView.ToTable
            NurseSortList_D.Rows(i)("NurseBedList") = Table_To_String(Table_Temp, "Bedno")
            NurseSortList_D.Rows(i)("BedCount") = Bed_Count(Table_Temp)
        Next
        '@@@@
        '####E護士班表病床資料結合
        For i = 0 To NurseSortList_E.Rows.Count - 1
            Dim SQL_D As String = String.Format("EMPNO = '{0}'", NurseSortList_E.Rows(i)("EMPNO").ToString)
            BedList_E.DefaultView.RowFilter = SQL_D
            Dim Table_Temp As DataTable = BedList_E.DefaultView.ToTable
            NurseSortList_E.Rows(i)("NurseBedList") = Table_To_String(Table_Temp, "Bedno")
            NurseSortList_E.Rows(i)("BedCount") = Bed_Count(Table_Temp)
        Next

        '####
        '####N護士班表病床資料結合
        For i = 0 To NurseSortList_N.Rows.Count - 1
            Dim SQL_D As String = String.Format("EMPNO = '{0}'", NurseSortList_N.Rows(i)("EMPNO").ToString)
            BedList_N.DefaultView.RowFilter = SQL_D
            Dim Table_Temp As DataTable = BedList_N.DefaultView.ToTable
            NurseSortList_N.Rows(i)("NurseBedList") = Table_To_String(Table_Temp, "Bedno")
            NurseSortList_N.Rows(i)("BedCount") = Bed_Count(Table_Temp)
        Next
        '####

        '%%%%最後班表顯示合併
        NurseSortList_D.Merge(NurseSortList_E)
        NurseSortList_D.Merge(NurseSortList_N)
        ' GridView6.Caption = "D護士班表完成"
        ' GridView6.DataSource = NurseSortList_D
        ' GridView6.DataBind()
        Dim FinalDisplaytext As String = ""
        Dim DEN As String = ""
        Dim DUTY As String = ""
        Dim XFIRE As String = ""
        Dim NAME As String = ""
        Dim NurseBedList As String = ""
        Dim BedCount As String = ""
        For k = 0 To NurseSortList_D.Rows.Count - 1
            DEN = NurseSortList_D.Rows(k)("DEN").ToString
            DUTY = NurseSortList_D.Rows(k)("DUTY").ToString
            XFIRE = NurseSortList_D.Rows(k)("XFIRE").ToString
            NAME = NurseSortList_D.Rows(k)("NAME").ToString
            NurseBedList = NurseSortList_D.Rows(k)("NurseBedList").ToString
            BedCount = NurseSortList_D.Rows(k)("BedCount").ToString

            FinalDisplaytext &= String.Format("<tr class =""Table_tr_style1"">")
            FinalDisplaytext &= String.Format("               <td  class=""Table_td_style"">{0}</td>", DEN)
            FinalDisplaytext &= String.Format("               <td class=""Table_td_style"">{0}</td>", NAME)
            FinalDisplaytext &= String.Format("               <td align =""left""  class=""Table_td_style"" style=""word-wrap:break-word;"">{0}</td>", NurseBedList)
            FinalDisplaytext &= String.Format("               <td class=""Table_td_style"">{0}</td>", BedCount)
            FinalDisplaytext &= String.Format("               <td class=""Table_td_style"">{0}</td>", DUTY)
            FinalDisplaytext &= String.Format("               <td class=""Table_td_style"">{0}</td>", XFIRE)
            FinalDisplaytext &= String.Format("</tr>")
        Next
        '%%%%
        NurseBedListShow.Text = FinalDisplaytext
    End Function
    '====護士排序方式--以病床排序
    Public Function NsBedSort(ByVal NurseList As DataTable, ByVal BedList As DataTable) As DataTable
        '新增NurseList 的欄位Sort
        'Nssort 為QueryString 的參數
        NurseList.Columns.Add("Sort")
        For i = 0 To NurseList.Rows.Count - 1
            Dim temprows As DataRow = NurseList.Rows(i)
            Dim temprows2() As DataRow
            temprows2 = BedList.Select(String.Format("Empno = '{0}'", NurseList.Rows(i)("empno")), "BEDNO")

            If temprows2.Count > 0 Then '判斷是否為空值,非空值填入第一個病床號
                temprows("Sort") = temprows2(0)("BEDNO")
            End If
            If temprows("DEN") = "H" Then '判斷是否為H班人員
                temprows("Sort") = "Z"
            End If
        Next
        Dim DV As DataView = New DataView(NurseList)
        DV.Sort = "Sort "
        Dim final As DataTable = DV.ToTable
        Return final

    End Function
    '將Table內的欄位轉為字串 
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
        nobed.Add("")
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



    '主程式
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim Empno As String = Request.QueryString("Empno")
        Dim ConnStr As String = SELECT_ORACLE(HospCode)
        Dim Checkdate As String = Now.Date.ToString("yyyy/MM/dd")
        If Not String.IsNullOrEmpty(Request.QueryString("Todate")) Then
            Checkdate = Request.QueryString("Todate").ToString
        End If
        LabelCheckdate.Text = Checkdate
        'MMH LOGO顯示控制
        Show_LOGO(HospCode)
        '超連結設定
        Hyperlink_Default(HospCode, WardCode)
        '顯示病房名稱, 查詢時間, 操作者
        Ward_Name_Label.Text = Title_default(HospCode, WardCode, ConnStr)
        User_Label.Text = Show_User()
        Dim Dancag As String = Get_Dancag(HospCode, WardCode, 0) '控制護理站名稱和護士班表切換時點
        Nurse_List(HospCode, WardCode, Dancag)

    End Sub
End Class
