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
    '判斷是否為行動裝置
    Public Function adjust_mobile() As String
        Dim Version As String = ""
        If Request.Headers("user-agent") IsNot Nothing And Request.Headers("user-agent").ToLower.ToString.IndexOf("windows") <> -1 Then
            Version = "PC"
        Else
            Version = "mobile"
        End If
        Return Version
    End Function
    '抓取伺服器時間
    Protected Sub SERVER_TIME()
        DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        Dim d1 As DateTime = New DateTime(1970, 1, 1)
        Dim d2 As DateTime = DateTime.Now.ToUniversalTime
        Dim ts As TimeSpan = New TimeSpan(d2.Ticks - d1.Ticks)
    End Sub
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
    '撈取資料IP _PHONE
    Public Function IP_PHONE(ByVal Hospcode As String, ByVal Wardcode As String) As DataTable
        '開啟EXCEL檔案
        Dim SavePath As String = Request.PhysicalApplicationPath & "Upload\"
        Dim FileName As String = String.Format("{0}IP_PHONE.xlsx", SavePath)
        Dim ExcelFile As FileStream = File.Open(FileName, FileMode.Open)

        Dim Workbook As XSSFWorkbook = New XSSFWorkbook(ExcelFile)
        Dim U_sheet As XSSFSheet = Workbook.GetSheetAt(0)
        Dim HeaderRow As XSSFRow = U_sheet.GetRow(0)

        Dim D_table As DataTable = New DataTable()
        Dim Table_column As DataColumn
        Dim Table_Row As DataRow

        '建置TABLE 架構
        Table_column = New DataColumn
        Table_column.DataType = Type.GetType("System.String")
        Table_column.ColumnName = "Hospcode"
        D_table.Columns.Add(Table_column)

        Table_column = New DataColumn
        Table_column.DataType = Type.GetType("System.String")
        Table_column.ColumnName = "Wardcode"
        D_table.Columns.Add(Table_column)

        Table_column = New DataColumn
        Table_column.DataType = Type.GetType("System.String")
        Table_column.ColumnName = "Cat"
        D_table.Columns.Add(Table_column)

        Table_column = New DataColumn
        Table_column.DataType = Type.GetType("System.String")
        Table_column.ColumnName = "Cat_detail"
        D_table.Columns.Add(Table_column)

        Table_column = New DataColumn
        Table_column.DataType = Type.GetType("System.String")
        Table_column.ColumnName = "phone"
        D_table.Columns.Add(Table_column)


        '逐列處理資料
        For i As Integer = (U_sheet.FirstRowNum + 1) To (U_sheet.LastRowNum)
            Dim Row As XSSFRow = U_sheet.GetRow(i)
            Table_Row = D_table.NewRow
            '逐欄處理資料
            If Row.GetCell(0).ToString = Hospcode And Row.GetCell(1).ToString = Wardcode Then   '判斷第一欄是否為空值
                Table_Row("Hospcode") = Row.GetCell(0).ToString
                Table_Row("Wardcode") = Row.GetCell(1).ToString
                Table_Row("Cat") = Row.GetCell(2).ToString
                Table_Row("Cat_detail") = Row.GetCell(3).ToString
                Table_Row("phone") = Row.GetCell(4).ToString
                D_table.Rows.Add(Table_Row)

            End If
        Next


        Return D_table
    End Function
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
    '依目前時間取得護理班別(可調整時間差)(共用)
    Public Function Get_Dancag(ByVal Hospcode As String, ByVal Wardcode As String, ByVal adjust_m As Integer, now_Time As DateTime) As String
        'adjust_m    切換時點的分鐘調整數
        '取得護理站班表切換時間
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM WHTBOARD  WHERE NS='{0}' AND EMPNO='DEN' ", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As New DataTable
        DA.Fill(DT)
        '取得ORACLE目前系統時間



        'select to_char(sysdate,'YYYY/MM/DD HH:MI:SS AM') FROM DUAL; 
        Dim D_Time As Integer = 700 '預設日班換班時間
        Dim E_Time As Integer = 1500 ' 預設中班換班時間
        Dim N_Time As Integer = 2300 ' 預設大夜換班時間
        If DT.Rows.Count > 0 Then
            Dim Den_LIST() As String = DT.Rows(0)("NAME").ToString.Split(",")
            D_Time = Convert.ToInt16(Den_LIST(0))
            E_Time = Convert.ToInt16(Den_LIST(1))
            N_Time = Convert.ToInt16(Den_LIST(2))
        End If
        Dim DanCag As String = ""
        Dim NowHour As Integer = now_time.AddMinutes(adjust_m).Hour
        Dim NowTime As Integer = now_time.Hour * 100 + Now.Minute
        'Dim NowHour As Integer = Now.AddMinutes(adjust_m).Hour
        'Dim NowTime As Integer = Now.Hour * 100 + Now.Minute

        If NowTime >= N_Time Then   '夜班
            DanCag = "N"
        ElseIf NowTime < D_Time Then '夜班
            DanCag = "N"
        ElseIf NowTime >= D_Time And NowTime < E_Time Then '日班
            DanCag = "D"
        ElseIf NowTime >= E_Time And NowTime < N_Time Then
            DanCag = "E"      '中班
        End If
        Return DanCag
    End Function
    '早班換班時間(共用)
    Public Function Get_Time(ByVal Hospcode As String, ByVal Wardcode As String) As Integer
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
    '早班換班時間(共用)
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
    '晚班換班時間(共用)
    Public Function Get_TimeN(ByVal Hospcode As String, ByVal Wardcode As String) As Integer
        '取得護理站班表切換時間
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM WHTBOARD  WHERE NS='{0}' AND EMPNO='DEN' ", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As New DataTable

        DA.Fill(DT)

        Dim N_Time As Integer = 2300 '預設大夜班換班時間
        If DT.Rows.Count > 0 Then
            Dim Den_LIST() As String = DT.Rows(0)("NAME").ToString.Split(",")
            N_Time = Convert.ToInt16(Den_LIST(2))
        End If
        Return N_Time
    End Function

    '將LIST 轉換成SQL In 的格式
    Public Function List_to_SQLin(ByVal dept() As String, ByVal Column As String) As String
        Dim DeptStr As String = String.Format("AND  {0} in ('{1}')", Column, "沒有值")
        If dept.Count > 0 Then
            DeptStr = String.Format(" AND  {0}  in ('{1}'", Column, dept(0).ToString)  '頭
            For i = 1 To dept.Count - 1                                                                                        '身
                DeptStr &= String.Format(",'{0}'", dept(i))
            Next
            DeptStr &= ") "                                                                                                               '尾

        End If
        Return DeptStr


    End Function
    '將LIST 轉換成SQL In 的格式,不加AND
    Public Function List_to_SQLin2(ByVal dept() As String, ByVal Column As String) As String
        Dim DeptStr As String = String.Format("  {0} in ('{1}')", Column, "沒有值")
        If dept.Count > 0 Then
            DeptStr = String.Format("   {0}  in ('{1}'", Column, dept(0).ToString)  '頭
            For i = 1 To dept.Count - 1                                                                                        '身
                DeptStr &= String.Format(",'{0}'", dept(i))
            Next
            DeptStr &= ") "                                                                                                               '尾

        End If
        Return DeptStr


    End Function
    '護理站名稱
    Public Function Show_Ward_Name(ByVal Hospcode As String, ByVal Wardcode As String, ByVal DanCag As String) As String
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM NSTBL  WHERE NS='{0}' ", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataTable
        Dim ContactInfo As String = Nothing
        DA.Fill(DS)
        If DS.Rows.Count > 0 Then
            If adjust_mobile() = "mobile" Then
                Show_Ward_Name_Label.Text = String.Format("<input type =""button"" onclick=""history.go(-1)"" value=""回患者名單"" style=""font-size:20px;color:#000000;""></input><span class = Numeric-font><a href=""javascript: history.go(-1)""  style =""color:rgb(255,236,122); font-size:48px;line-height:60px;"">{0}-{1}</a></span>", DS.Rows(0)("nseng"), DanCag)
            Else
                Show_Ward_Name_Label.Text = String.Format("<span class = Numeric-font><a href=""javascript: history.go(-1)""  style =""color:rgb(255,236,122); font-size:60px;line-height:60px;"">{0}-{1}</a></span>", DS.Rows(0)("nseng"), DanCag)
            End If

            Return DS.Rows(0)("NSENG").ToString
        End If
    End Function
    '判斷字數變更字體大小
    Public Function Adjust_Font_size(ByVal Str As String, ByVal StrCount As Integer, ByVal FontSize As Integer)
        'Str==要處理的字串
        'StrCount==字數限制   4    大於等於4時變更字型大小
        'FontSize== 字型大小  
        Dim Str_List() As Char = Str.ToCharArray
        If Str_List.Count >= StrCount Then
            Str = String.Format("<span style ="" font-size:{0}px;"">{1}</span>", FontSize, Str)
        End If
        Return Str
    End Function

    '#####################################統計資料##########################################
    '==輕中重以Bettbl紀錄顯示
    Sub Show_Statistics(ByVal Hospcode As String, ByVal Wardcode As String)
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr

        Dim Wardcode_temp As String

        If Hospcode = "2" And Wardcode = "70" Then
            Wardcode_temp = "'" & Wardcode & "'" & "OR Bedns = '75'"
        ElseIf Hospcode = "2" And Wardcode = "8A" Then
            Wardcode_temp = "'" & Wardcode & "'" & "OR Bedns = '8B'"
        Else
            Wardcode_temp = "'" & Wardcode & "'"
        End If
        '取得床位數資料
        Dim cmd As String = String.Format("SELECT  *  FROM Bedtbl  WHERE (Bedns={0})  AND Bedroom!='99' AND Pubbed = 'Y' ", Wardcode_temp)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As DataTable = New DataTable
        DA.Fill(DT)
        Dim Empty_rows() As DataRow = DT.Select("Bedsts = '0' ")

        '取得輕中重狀態資料
        Dim cmd_state As String = Nothing
        cmd_state &= " select  A.pno,A.name,A.ptstate,A.nsneg,B.ns "
        cmd_state &= " from Ptstatus A, Nstbl B "
        cmd_state &= "  where A.nsneg = B.nseng "
        cmd_state &= String.Format(" and B.NS= '{0}' ", Wardcode)
        cmd_state &= " and A.ptstate is not NULL " '排除未標示病患
        cmd_state &= " and A.pno not like '88%' " '排除測試號
        Dim DA_state As OleDbDataAdapter = New OleDbDataAdapter(cmd_state, Conn)
        Dim DT_state As DataTable = New DataTable
        DA_state.Fill(DT_state)
        'GridView2.DataSource = DT_state
        'GridView2.DataBind()

        '輕度人數
        Dim Light As Integer = DT_state.Select(" PTSTATE = 'R1' OR PTSTATE = 'T1'").Count
        '中度人數
        Dim Medium As Integer = DT_state.Select(" PTSTATE = 'R2' OR PTSTATE = 'T2'").Count
        '重度人數
        Dim Heavey As Integer = DT_state.Select(" PTSTATE = 'R3' OR PTSTATE = 'T3'").Count
        '空床數
        Dim Empty_bed As Integer = Empty_rows.Count
        '全部床數
        Dim All_bed As Integer = DT.Rows.Count
        '佔床率
        Dim Take_up_rate As Single = (All_bed - Empty_bed) / All_bed




        LightLabel.Text = Light
        MediumLabel.Text = Medium
        HeaveyLabel.Text = Heavey
        Take_up_rate_Label.Text = Take_up_rate.ToString("P0")
        Empty_Bed_Label.Text = Empty_bed
        All_bed_Label.Text = All_bed

        Conn.Close()

    End Sub
    '==輕中重以未交班顯示isbarrec
    Sub Show_Statistics_2(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Now_Time As  DateTime)
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr

        Dim Wardcode_temp As String


        If Now_Time.Hour >= 10 Then
            Now_Time = Now_Time.AddDays(1)
        End If
        Dim Nowdate As String = Now_Time.ToString("yyyy/MM/dd")
        'Response.Write(Now_Time)

        If Hospcode = "2" And Wardcode = "70" Then
            Wardcode_temp = "'" & Wardcode & "'" & "OR Bedns = '75'"
        ElseIf Hospcode = "2" And Wardcode = "8A" Then
            Wardcode_temp = "'" & Wardcode & "'" & "OR Bedns = '8B'"
        Else
            Wardcode_temp = "'" & Wardcode & "'"
        End If
        '取得床位數資料
        Dim cmd As String = String.Format("SELECT  *  FROM Bedtbl  WHERE (Bedns={0})  AND Bedroom!='99' AND Pubbed = 'Y' ", Wardcode_temp)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As DataTable = New DataTable
        DA.Fill(DT)
        Dim Empty_rows() As DataRow = DT.Select("Bedsts = '0' ")

        '取得輕中重狀態資料
        Dim cmd_state As String = Nothing
        cmd_state &= " Select I.PNO,I.CASENO,I.DTYPE,B.bedns,B.bedno as btbedno,I.recdate,I.dline,B.BEDNO,"
        cmd_state &= " Decode(i.SEVERITY,1,'輕',2,'中',3,'重') DSEVERITY,"
        cmd_state &= " i.recvdate "
        cmd_state &= " From ISBARREC I,BedTbl B"
        cmd_state &= " Where (i.DTYPE like '內%' or i.DTYPE like '外%' or i.DTYPE like '兒%' or i.DTYPE like '婦%' or i.DTYPE like '%急%')"
        cmd_state &= " And B.bedpno = i.pno   "
        cmd_state &= " And B.BedPno not like '88______' "
        cmd_state &= String.Format(" and B.BEDNS= '{0}' ", Wardcode)
        cmd_state &= " AND i.recvdate is null"
        cmd_state &= String.Format(" And i.DLINE >= To_Date('{0}','yyyy/MM/DD')", Nowdate)
        cmd_state &= " Order By DSEVERITY"
        Dim DA_state As OleDbDataAdapter = New OleDbDataAdapter(cmd_state, Conn)
        Dim DT_state As DataTable = New DataTable
        DA_state.Fill(DT_state)
        'GridView2.DataSource = DT_state
        'GridView2.DataBind()
        'Response.Write(DT_state.Rows.Count)
        '輕度人數
        Dim Light As Integer = DT_state.Select(" DSEVERITY = '輕'").Count
        '中度人數
        Dim Medium As Integer = DT_state.Select(" DSEVERITY = '中'").Count
        '重度人數
        Dim Heavey As Integer = DT_state.Select(" DSEVERITY = '重'").Count
        '空床數
        Dim Empty_bed As Integer = Empty_rows.Count
        '全部床數
        Dim All_bed As Integer = DT.Rows.Count
        ' If Hospcode = "1" And Wardcode = "02" Then
        'All_bed = All_bed - 6
        ' Empty_bed = Empty_bed - 6
        'End If

        '佔床率
        Dim Take_up_rate As Single = (All_bed - Empty_bed) / All_bed

        LightLabel.Text = Light
        MediumLabel.Text = Medium
        HeaveyLabel.Text = Heavey
        Take_up_rate_Label.Text = Take_up_rate.ToString("P0")
        Empty_Bed_Label.Text = Empty_bed
        All_bed_Label.Text = All_bed
        Conn.Close()

    End Sub

    '#############################取得大白板資料######################
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
    '取得護理站的值班醫師科別(轉為LIST_Str)
    Public Function GET_DUTY_LIST(ByVal Hospcode As String, ByVal Wardcode As String) As String
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim TpConnStr As String = SELECT_ORACLE("1") '目前淡水值班醫師資料存在台北資料庫
        Dim Conn As OleDbConnection = New OleDbConnection
        Dim TpConn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        TpConn.ConnectionString = TpConnStr
        Dim DUTY_LIST_Str As String = ""
        Dim DT As DataTable = New DataTable
        Dim AddHD_cmd As String = String.Format("SELECT * FROM WHTBOARD WHERE NS='{0}' and GRP='值班醫師科別' and empno='duty'", Wardcode)
        Dim AddHD_DA As OleDbDataAdapter = New OleDbDataAdapter(AddHD_cmd, Conn)
        AddHD_DA.Fill(DT)

        If DT.Rows.Count > 0 Then
            DUTY_LIST_Str = DT.Rows(0)("NAME").ToString
        End If
        ' Response.Write(DUTY_LIST_Str)
        Return DUTY_LIST_Str
    End Function
    '取得護理站的照會醫師科別
    Public Function GET_ONCALL(ByVal Hospcode As String, ByVal Wardcode As String) As String
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim DT As DataTable = New DataTable
        Dim HD_cmd As String = String.Format("SELECT A.NAME  FROM WHTBOARD A  WHERE A.NS='{0}'  and A.empno='cons' ", Wardcode)
        Dim HD_DA As OleDbDataAdapter = New OleDbDataAdapter(HD_cmd, Conn)
        HD_DA.Fill(DT)

        Dim Master As String = ""
        If DT.Rows.Count > 0 Then
            Master = DT.Rows(0)("NAME").ToString
        End If
        If (Conn.State = ConnectionState.Open) Then
            Conn.Close()
            Conn.Dispose()
        End If
        Return Master
    End Function
    '取得護理站的白天呈現科別選項
    Public Function Get_Day(ByVal Hospcode As String, ByVal Wardcode As String) As String
        Dim Conn As OleDbConnection = New OleDbConnection
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Conn.ConnectionString = ConnStr
        Conn.Open()

        Dim DS As New DataSet
        Dim HD_cmd As String = String.Format("SELECT * FROM WHTBOARD WHERE NS='{0}' and GRP='白天呈現科別' and empno='day'", Wardcode)
        Dim HD_DA As OleDbDataAdapter = New OleDbDataAdapter(HD_cmd, Conn)
        HD_DA.Fill(DS, "DAY")
        Dim Day As String = ""
        If DS.Tables("DAY").Rows.Count > 0 Then
            Day = DS.Tables("DAY").Rows(0)("NAME").ToString
        End If

        If (Conn.State = ConnectionState.Open) Then
            Conn.Close()
            Conn.Dispose()
        End If

        Return Day
    End Function

    '取得護理站未交班醫班醫師LIST
    Public Function ISBARREC_RECVING(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Btime As Integer, ByVal Etime As Integer, Now_Time As DateTime) As ArrayList
        'Btime  未交班燈號開始時間
        'Etime  未交班燈號結束時間
        Dim NowTime As Integer = Now_Time.Hour * 100 + Now_Time.Minute
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr

        Dim Nowdate As String = Now_Time.ToString("yyyy/MM/dd")
        If Now_Time.Hour > 10 Then
            Nowdate = Now_Time.AddDays(1).ToString("yyyy/MM/dd")
        End If


        Dim cmd_state As String = Nothing
        cmd_state &= " Select i.PNO,i.CASENO,i.DTYPE,i.BEDNO,i.SEVERITY,i.TRANSFER,i.RECVING,i.CR,i.CRFOLLOWUP,"
        cmd_state &= " Decode(i.SEVERITY,1,'輕',2,'中',3,'重') DSEVERITY,d.CNAME1 as DeptName,n.NSENG,"
        cmd_state &= " i.Dline,i.recvdate,Dr1.code as TDRcode,Dr1.NAME as TDRName,dr2.code as RDRcode,  Dr2.NAME as RDRName,  Dr3.NAME as CRName,  "
        cmd_state &= " IDP.NAME as Name,IDP.SEX,IDP.BIRTH  "
        cmd_state &= " From ISBARREC i,Dept d,Nstbl n,Dr Dr1,Dr Dr2,Dr Dr3,IDP  "
        cmd_state &= " Where (i.DTYPE like '內%' or i.DTYPE like '外%' or i.DTYPE like '兒%' or i.DTYPE like '婦%')"
        cmd_state &= " And i.DEPT = d.CODE  And i.BEDNS = n.NS  And i.TRANSFER = Dr1.CODE  "
        cmd_state &= " And i.RECVING = Dr2.CODE(+)  And i.CR = Dr3.CODE(+)  "
        cmd_state &= " And i.Pno=IDP.Pno  And i.Pno not like '88______' "
        cmd_state &= " AND i.recvdate is null"
        cmd_state &= String.Format(" and i.BEDNS= '{0}' ", Wardcode)
        cmd_state &= String.Format(" And i.DLINE >= To_Date('{0}','yyyy/MM/DD')", Nowdate)
        cmd_state &= " Order By Recdate,Bedno" '
        Dim DA_state As OleDbDataAdapter = New OleDbDataAdapter(cmd_state, Conn)
        Dim DT_state As DataTable = New DataTable
        DA_state.Fill(DT_state)
        Dim TDR_LIST As ArrayList = New ArrayList
        'TDR_LIST.Add("4388") '輸入測試的帳號
        'TDR_LIST.Add("5898") '輸入測試的帳號
        For i = 0 To DT_state.Rows.Count - 1
            Dim temp As String = DT_state.Rows(i)("RECVING").ToString
            If Not TDR_LIST.Contains(temp) And Not String.IsNullOrEmpty(temp) Then   '欄位的名稱 DT_state.Rows(i)("RECVDATE").ToString
                TDR_LIST.Add(temp)
            End If
        Next
        'GridView1.Caption = "未交班醫總表"
        'GridView1.DataSource = DT_state
        'GridView1.DataBind()
        If NowTime < Btime And NowTime > Etime Then   '不顯示的區間內,將ARRAY清空
            TDR_LIST.Clear()
        End If
        Return TDR_LIST
    End Function
    '重度病患醫師陣列
    Public Function ISBARREC_RECVING_3(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Now_Time As DateTime) As ArrayList
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr

        Dim Nowdate As String = Now_Time.ToString("yyyy/MM/dd")
        If Now_Time.Hour > 10 Then
            Nowdate = Now_Time.AddDays(1).ToString("yyyy/MM/dd")
        End If


        Dim cmd_state As String = Nothing
        cmd_state &= " Select i.PNO,i.CASENO,i.DTYPE,i.BEDNO,i.SEVERITY,i.TRANSFER,i.RECVING,i.CR,i.CRFOLLOWUP,"
        cmd_state &= " Decode(i.SEVERITY,1,'輕',2,'中',3,'重') DSEVERITY,d.CNAME1 as DeptName,n.NSENG,"
        cmd_state &= " i.Dline,i.recvdate,Dr1.code as TDRcode,Dr1.NAME as TDRName,dr2.code as RDRcode,  Dr2.NAME as RDRName,  Dr3.NAME as CRName,  "
        cmd_state &= " IDP.NAME as Name,IDP.SEX,IDP.BIRTH  "
        cmd_state &= " From ISBARREC i,Dept d,Nstbl n,Dr Dr1,Dr Dr2,Dr Dr3,IDP  "
        cmd_state &= " Where (i.DTYPE like '內%' or i.DTYPE like '外%' or i.DTYPE like '兒%' or i.DTYPE like '婦%')"
        cmd_state &= " And i.DEPT = d.CODE  And i.BEDNS = n.NS  And i.TRANSFER = Dr1.CODE  "
        cmd_state &= " And i.RECVING = Dr2.CODE(+)  And i.CR = Dr3.CODE(+)  "
        cmd_state &= " And i.Pno=IDP.Pno  And i.Pno not like '88______' "
        cmd_state &= " AND i.recvdate is null"
        cmd_state &= " AND i.SEVERITY = '3'"
        cmd_state &= String.Format(" and i.BEDNS= '{0}' ", Wardcode)
        cmd_state &= String.Format(" And i.DLINE >= To_Date('{0}','yyyy/MM/DD')", Nowdate)
        cmd_state &= " Order By Recdate,Bedno" '
        Dim DA_state As OleDbDataAdapter = New OleDbDataAdapter(cmd_state, Conn)
        Dim DT_state As DataTable = New DataTable
        DA_state.Fill(DT_state)
        Dim TDR_LIST As ArrayList = New ArrayList
        'TDR_LIST.Add("4388") '輸入測試的帳號
        For i = 0 To DT_state.Rows.Count - 1
            Dim temp As String = DT_state.Rows(i)("RECVING").ToString
            If Not TDR_LIST.Contains(temp) And Not String.IsNullOrEmpty(temp) Then   '欄位的名稱 DT_state.Rows(i)("RECVDATE").ToString
                TDR_LIST.Add(temp)
            End If
        Next
        ' Response.Write(TDR_LIST.Count)
        'GridView1.DataSource = DT_state
        'GridView1.DataBind()

        Return TDR_LIST
    End Function
    '中度病患人數
    Public Function ISBARREC_RECVING_2(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Now_Time As DateTime) As ArrayList
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr

        Dim Nowdate As String = Now_Time.ToString("yyyy/MM/dd")
        If Now_Time.Hour > 10 Then
            Nowdate = Now_Time.AddDays(1).ToString("yyyy/MM/dd")
        End If


        Dim cmd_state As String = Nothing
        cmd_state &= " Select i.PNO,i.CASENO,i.DTYPE,i.BEDNO,i.SEVERITY,i.TRANSFER,i.RECVING,i.CR,i.CRFOLLOWUP,"
        cmd_state &= " Decode(i.SEVERITY,1,'輕',2,'中',3,'重') DSEVERITY,d.CNAME1 as DeptName,n.NSENG,"
        cmd_state &= " i.Dline,i.recvdate,Dr1.code as TDRcode,Dr1.NAME as TDRName,dr2.code as RDRcode,  Dr2.NAME as RDRName,  Dr3.NAME as CRName,  "
        cmd_state &= " IDP.NAME as Name,IDP.SEX,IDP.BIRTH  "
        cmd_state &= " From ISBARREC i,Dept d,Nstbl n,Dr Dr1,Dr Dr2,Dr Dr3,IDP  "
        cmd_state &= " Where (i.DTYPE like '內%' or i.DTYPE like '外%' or i.DTYPE like '兒%' or i.DTYPE like '婦%')"
        cmd_state &= " And i.DEPT = d.CODE  And i.BEDNS = n.NS  And i.TRANSFER = Dr1.CODE  "
        cmd_state &= " And i.RECVING = Dr2.CODE(+)  And i.CR = Dr3.CODE(+)  "
        cmd_state &= " And i.Pno=IDP.Pno  And i.Pno not like '88______' "
        cmd_state &= " AND i.recvdate is null"
        cmd_state &= " AND i.SEVERITY = '2'"
        cmd_state &= String.Format(" and i.BEDNS= '{0}' ", Wardcode)
        cmd_state &= String.Format(" And i.DLINE >= To_Date('{0}','yyyy/MM/DD')", Nowdate)
        cmd_state &= " Order By Recdate,Bedno" '
        Dim DA_state As OleDbDataAdapter = New OleDbDataAdapter(cmd_state, Conn)
        Dim DT_state As DataTable = New DataTable
        DA_state.Fill(DT_state)
        Dim TDR_LIST As ArrayList = New ArrayList
        'TDR_LIST.Add("4388") '輸入測試的帳號
        For i = 0 To DT_state.Rows.Count - 1
            Dim temp As String = DT_state.Rows(i)("RECVING").ToString
            If Not TDR_LIST.Contains(temp) And Not String.IsNullOrEmpty(temp) Then   '欄位的名稱 DT_state.Rows(i)("RECVDATE").ToString
                TDR_LIST.Add(temp)
            End If
        Next
        ' Response.Write(TDR_LIST.Count)
        'GridView2.DataSource = DT_state
        'GridView2.DataBind()

        Return TDR_LIST
    End Function
    '輕度病患人數
    Public Function ISBARREC_RECVING_1(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Now_Time As DateTime) As ArrayList
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr

        Dim Nowdate As String = Now_Time.ToString("yyyy/MM/dd")
        If Now_Time.Hour > 10 Then
            Nowdate = Now_Time.AddDays(1).ToString("yyyy/MM/dd")
        End If


        Dim cmd_state As String = Nothing
        cmd_state &= " Select i.PNO,i.CASENO,i.DTYPE,i.BEDNO,i.SEVERITY,i.TRANSFER,i.RECVING,i.CR,i.CRFOLLOWUP,"
        cmd_state &= " Decode(i.SEVERITY,1,'輕',2,'中',3,'重') DSEVERITY,d.CNAME1 as DeptName,n.NSENG,"
        cmd_state &= " i.Dline,i.recvdate,Dr1.code as TDRcode,Dr1.NAME as TDRName,dr2.code as RDRcode,  Dr2.NAME as RDRName,  Dr3.NAME as CRName,  "
        cmd_state &= " IDP.NAME as Name,IDP.SEX,IDP.BIRTH  "
        cmd_state &= " From ISBARREC i,Dept d,Nstbl n,Dr Dr1,Dr Dr2,Dr Dr3,IDP  "
        cmd_state &= " Where (i.DTYPE like '內%' or i.DTYPE like '外%' or i.DTYPE like '兒%' or i.DTYPE like '婦%')"
        cmd_state &= " And i.DEPT = d.CODE  And i.BEDNS = n.NS  And i.TRANSFER = Dr1.CODE  "
        cmd_state &= " And i.RECVING = Dr2.CODE(+)  And i.CR = Dr3.CODE(+)  "
        cmd_state &= " And i.Pno=IDP.Pno  And i.Pno not like '88______' "
        cmd_state &= " AND i.recvdate is null"
        cmd_state &= " AND i.SEVERITY = '1'"
        cmd_state &= String.Format(" and i.BEDNS= '{0}' ", Wardcode)
        cmd_state &= String.Format(" And i.DLINE >= To_Date('{0}','yyyy/MM/DD')", Nowdate)
        cmd_state &= " Order By Recdate,Bedno" '
        Dim DA_state As OleDbDataAdapter = New OleDbDataAdapter(cmd_state, Conn)
        Dim DT_state As DataTable = New DataTable
        DA_state.Fill(DT_state)
        Dim TDR_LIST As ArrayList = New ArrayList
        ' TDR_LIST.Add("5898") '輸入測試的帳號
        For i = 0 To DT_state.Rows.Count - 1
            Dim temp As String = DT_state.Rows(i)("RECVING").ToString
            If Not TDR_LIST.Contains(temp) And Not String.IsNullOrEmpty(temp) Then   '欄位的名稱 DT_state.Rows(i)("RECVDATE").ToString
                TDR_LIST.Add(temp)
            End If
        Next
        ' Response.Write(TDR_LIST.Count)
        'GridView1.DataSource = DT_state
        'GridView1.DataBind()

        Return TDR_LIST
    End Function

    '###############################################值班護士資料########################################################(共用)

    Public Function Nurse_List(ByVal Hospcode As String, ByVal Wardcode As String, ByVal NSCount As Integer, ByVal Bed_Count As Integer, ByVal NsSort As String, ByVal Dancag As String, ByVal Now_Time As DateTime) As Integer
        'NsSort 控制護理人員排序的參數,其值為bedno時以床病排序
        'NSCount 控制護理人員的顯示筆數
        'Bed_Count 控制病床的顯示筆數

        '護士排班表病床代碼數為5的護理站
        Dim BedCodeCountH() As String = {"1", "2"}
        Dim BedCodeCountW() As String = {"31", "8A", "8B", "90", "95"}

        '台北新生兒病房護士班表病房CODE數為5
        Dim Bedwordcount As Integer = 3
        Dim bedfontsize As Integer = 35
        Dim WardStytle As String = "Nurse_ward"
        If BedCodeCountH.Contains(Hospcode) And BedCodeCountW.Contains(Wardcode) Then

            Bedwordcount = 5
            bedfontsize = 26
        End If



        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim ConnORA As String = SELECT_ORACLE(0)
        Dim Nowdate As String = Now_Time.ToString("yyyy/MM/dd 00:00:00")
        Dim Nowtime As String = Now_Time.ToString("yyyy/MM/dd HH:00:00")
        'Dim DanCag As String = Nothing '判斷目前時間的班別
        Dim NowHour As Integer = Now_Time.Hour
        Dim NOWT As Integer = Now_Time.Hour * 100 + Now_Time.Minute
        Dim addDay As Integer = 5
        '開始時間
        Dim FromDate As String = Now_Time.ToString("yyyy/MM/dd 00:00:00")
        '結束時間
        Dim ToDate As String = Now_Time.AddDays(1).ToString("yyyy/MM/dd 00:00:00")

        'If NowHour <= 6 And Dancag = "N" Then
        '台北護理站切換時間為2300,淡水切換時間為2400

        Select Case Hospcode
            Case "1", "4"  '大夜交班時間為2300
                If NOWT <= Get_TimeD(Hospcode, Wardcode) And Dancag = "N" Then
                    FromDate = Now_Time.AddDays(-1).ToString("yyyy/MM/dd 00:00:00")
                    ToDate = Now_Time.ToString("yyyy/MM/dd 00:00:00")
                End If

            Case "2" '大夜交班時間為2400
                If NOWT >= Get_TimeN(Hospcode, Wardcode) And Dancag = "N" Then
                    FromDate = Now_Time.AddDays(1).ToString("yyyy/MM/dd 00:00:00")
                    ToDate = Now_Time.AddDays(2).ToString("yyyy/MM/dd 00:00:00")
                End If
        End Select

        '值班護士名單資料
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  DISTINCT (AL1.EMPNO), AL1.DEN, AL1.DUTY, AL1.XFIRE, AL1.NAME, AL1.BACKUP1 FROM MMH.NSASSIST AL1 WHERE AL1.NS='{0}' AND AL1.DEN='{1}' AND AL1.FDATE >= to_date('{2}','YYYY/MM/DD HH24:MI:SS') AND AL1.FDATE < to_date('{3}','YYYY/MM/DD HH24:MI:SS') AND AL1.EFFECT='Y' ORDER BY AL1.DUTY", Wardcode, Dancag, FromDate, ToDate)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataSet
        DA.Fill(DS, "NurseList") '護理人員資料



        'H班值班護士名單資料
        Dim H_cmd As String = String.Format("SELECT  DISTINCT (AL1.EMPNO), AL1.DEN, AL1.DUTY, AL1.XFIRE, AL1.NAME, AL1.BACKUP1 FROM MMH.NSASSIST AL1 WHERE AL1.NS='{0}' AND AL1.DEN='H' AND AL1.FDATE <=to_date('{2}','YYYY/MM/DD HH24:MI:SS') AND  AL1.TDATE > to_date('{3}','YYYY/MM/DD HH24:MI:SS')  AND AL1.EFFECT='Y'  ORDER BY AL1.DUTY", Wardcode, Dancag, Nowtime, Nowtime)
        Dim H_DA As OleDbDataAdapter = New OleDbDataAdapter(H_cmd, Conn)
        H_DA.Fill(DS, "HNurseList")
        DS.Tables("NurseList").Merge(DS.Tables("HNurseList")) '合併正常班護理人員和H班護理人員的資料



        '病床資料
        Dim cmd2 As String = String.Format("SELECT  AL1.EMPNO, AL1.Bedno, AL1.Pno FROM MMH.NSASSIST AL1 WHERE AL1.NS='{0}' AND AL1.DEN='{1}' AND AL1.FDATE >= to_date('{2}','YYYY/MM/DD HH24:MI:SS') AND AL1.FDATE < to_date('{3}','YYYY/MM/DD HH24:MI:SS') AND AL1.EFFECT='Y'", Wardcode, Dancag, FromDate, ToDate)
        Dim DA2 As OleDbDataAdapter = New OleDbDataAdapter(cmd2, Conn)
        DA2.Fill(DS, "BedList") '病床資料

        'H班病床資料
        Dim H_cmd2 As String = String.Format("SELECT  AL1.EMPNO, AL1.Bedno, AL1.Pno FROM MMH.NSASSIST AL1 WHERE AL1.NS='{0}' AND AL1.DEN='H' AND AL1.FDATE <= to_date('{2}','YYYY/MM/DD HH24:MI:SS') AND AL1.TDATE > to_date('{3}','YYYY/MM/DD HH24:MI:SS') AND AL1.EFFECT='Y'", Wardcode, Dancag, Nowtime, Nowtime)
        Dim H_DA2 As OleDbDataAdapter = New OleDbDataAdapter(H_cmd2, Conn)
        H_DA2.Fill(DS, "H_BedList")

        DS.Tables("BedList").Merge(DS.Tables("H_BedList")) '合併病床和H班病床資料


        '護理人員病床排序,NSSort的參數為'bedno'時將護士排序規則變更為以病床排序

        Dim NurseList As DataTable = New DataTable
        'If NsSort = "bedno" Then  
        NurseList = NsBedSort(DS.Tables("NurseList"), DS.Tables("BedList"), NsSort)
        'Else
        'NurseList = NsDutySort(DS.Tables("NurseList"), DS.Tables("BedList"))
        ' End If

        Dim NurseInfo As String = Nothing '最後顯示資料
        'NEW PATIENT 病床ARRAY
        Dim cmd3 As String = String.Format("SELECT  Bedno, Beddate  FROM Bedtbl  WHERE Bedns='{0}' And Bedsts='1' AND Bedroom!='99'", Wardcode)
        Dim DA3 As OleDbDataAdapter = New OleDbDataAdapter(cmd3, Conn)
        Dim DT3 As New DataTable
        Dim New_Patient_List As ArrayList = New ArrayList 'NEW PATIENT 的床位清單
        DA3.Fill(DT3)
        For p = 0 To DT3.Rows.Count - 1
            If DT3.Rows(p)("Beddate") = Nowdate Then
                New_Patient_List.Add(DT3.Rows(p)(0).ToString)
            End If
        Next


        '目前有佔床的床位號
        '淡水70,8A要放入75,8B的資料
        Dim Wardcode_temp As String
        If Hospcode = "2" And Wardcode = "70" Then
            Wardcode_temp = "'" & Wardcode & "'" & "OR Bedns = '75'"
        ElseIf Hospcode = "2" And Wardcode = "8A" Then
            Wardcode_temp = "'" & Wardcode & "'" & "OR Bedns = '8B'"
        Else
            Wardcode_temp = "'" & Wardcode & "'"
        End If


        'Dim cmd4 As String = String.Format("SELECT  Bedno  FROM Bedtbl  WHERE (Bedns={0} )  And Bedsts='1' AND Bedroom!='99'", Wardcode_temp)
        Dim cmd4 As String = String.Format("SELECT A.bedno,B.pnofrom, B.admdate,b.caseno , b.firstcaseno  From BEDTBL A, CASETBL B WHERE A.bedpno = B.pno AND A.bedcaseno = B.caseno And (A.bedns = {0}) AND Bedsts = '1' AND Bedroom != '99'", Wardcode_temp)
        Dim DA4 As OleDbDataAdapter = New OleDbDataAdapter(cmd4, Conn)
        Dim InBed_List As ArrayList = New ArrayList '目前佔床的床位清單
        Dim NewBed_List As ArrayList = New ArrayList '新住院病人清單 
        Dim ERBed_List As ArrayList = New ArrayList ' 從急診轉來的病患床位清單
        DA4.Fill(DS, "InBed")
        ' GridView1.DataSource = DS.Tables("InBed")
        'GridView1.DataBind()
        For l = 0 To DS.Tables("InBed").Rows.Count - 1
            Dim bedno As String = Right(DS.Tables("InBed").Rows(l)("Bedno"), Bedwordcount) '顯示於白板後三碼病房號
            Dim bedfrom As String = DS.Tables("Inbed").Rows(l)("pnofrom").ToString '
            Dim admdate As String = String.Format("{0:yyyy/MM/dd 00:00:00}", DS.Tables("Inbed").Rows(l)("admdate"))
            Dim caseno As String = DS.Tables("Inbed").Rows(l)("Caseno").ToString
            Dim firstcaseno As String = DS.Tables("Inbed").Rows(l)("firstCaseno").ToString
            'Response.Write(FormatDateTime(Nowdate, DateFormat.ShortDate))
            '佔床病人列表
            If Not InBed_List.Contains(bedno) Then
                InBed_List.Add(bedno)
            End If
            '新住院病人列表(不含急診新進病人)
            If Not NewBed_List.Contains(bedno) And Not (bedfrom = "2" Or bedfrom = "4" Or bedfrom = "6" Or bedfrom = "8") And FormatDateTime(admdate, DateFormat.ShortDate) = FormatDateTime(Nowdate, DateFormat.ShortDate) And caseno = firstcaseno Then
                NewBed_List.Add(bedno)
            End If
            '急診新進病人列表
            If (Not ERBed_List.Contains(bedno)) And (bedfrom = "2" Or bedfrom = "4" Or bedfrom = "6" Or bedfrom = "8") And FormatDateTime(admdate, DateFormat.ShortDate) = FormatDateTime(Nowdate, DateFormat.ShortDate) And caseno = firstcaseno Then
                ERBed_List.Add(bedno)
            End If
        Next

        Select Case Hospcode
            Case "1", "2"
                For index = 0 To (NurseList.Rows.Count - 1)
                    If index < NSCount Then
                        '處理護士所負責的病床資料==========
                        Dim DV As DataView = DS.Tables("BedList").DefaultView
                        'GridView1.DataSource = DV
                        'GridView1.DataBind()
                        Dim DVTable As DataTable = Nothing
                        Dim BedList As String = Nothing '床位顯示字串
                        Dim BedCount As Integer = 0 '非空床床位數

                        DV.RowFilter = ""
                        DV.RowFilter = String.Format("EMPNO = '{0}'", NurseList.Rows(index)("EMPNO"))
                        DV.Sort = "Bedno"
                        DVTable = DV.ToTable 'DataView無法直接抓取己處理的資料,須轉為Table,DataView.table內是DataView的原始資料


                        '處理病床資料
                        '判斷是床位數小於等於15時顯示
                        If DV.Count < 16 Then
                            For i = 0 To (DV.Count - 1)
                                Dim BedNo As String = DVTable.Rows(i)("Bedno").ToString
                                If Not String.IsNullOrEmpty(DVTable.Rows(i)("Bedno").ToString) Then
                                    If Not (BedNo = "書記" Or BedNo = "服務" Or BedNo = "助理員") Then
                                        BedCount += 1  '計算非書記、服務的床位數
                                    End If

                                    If i < Bed_Count Then '控制所要顯示的病床數
                                        If ERBed_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then '判斷是否為急診病患,如為變更框架為綠底黑字
                                            'BedList &= String.Format("<div class="" New_nurse_ward"">{0}</div>", Right(BedNo, 3))'藍底黑字
                                            BedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:32px;background-color:rgb(0,221,0); "">{1}</div>", bedfontsize, Right(BedNo, Bedwordcount)) '綠底黑字
                                        ElseIf InBed_List.Contains(Right(BedNo, Bedwordcount)) Or BedNo = "書記" Or BedNo = "服務" Or BedNo = "組長" Or BedNo = "行政" Then '判斷是否佔床,"書記,服務,組長,行政"
                                            BedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:32px; "">{1}</div>", bedfontsize, Right(BedNo, Bedwordcount))
                                        ElseIf BedNo = "助理員" Then '助理員時縮小字型
                                            BedList &= String.Format("<div class="" Nurse_ward_3 "">{0}</div>", BedNo)
                                        Else '空床
                                            BedList &= String.Format("<div class="" Nurse_ward_2"" style ="" font-size:{0}px; line-height:32px; "">{1}</div>", bedfontsize, Right(BedNo, Bedwordcount))
                                        End If
                                    End If
                                End If
                            Next
                            '病床數大於16時縮小顯示
                        Else
                            For i = 0 To (DV.Count - 1)
                                Dim BedNo As String = DVTable.Rows(i)("Bedno").ToString
                                If Not String.IsNullOrEmpty(DVTable.Rows(i)("Bedno").ToString) Then
                                    If Not (BedNo = "書記" Or BedNo = "服務" Or BedNo = "助理員") Then
                                        BedCount += 1  '計算非書記、服務的床位數
                                    End If

                                    If i < Bed_Count Then '控制所要顯示的病床數
                                        bedfontsize = 30
                                        If ERBed_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then '判斷是否為急診病患,如為變更框架為綠底黑字
                                            'BedList &= String.Format("<div class="" New_nurse_ward"">{0}</div>", Right(BedNo, 3))'藍底黑字
                                            BedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:25px;height:24.5px;background-color:rgb(0,221,0); "">{1}</div>", bedfontsize, Right(BedNo, Bedwordcount)) '綠底黑字
                                        ElseIf InBed_List.Contains(Right(BedNo, Bedwordcount)) Or BedNo = "書記" Or BedNo = "服務" Or BedNo = "組長" Or BedNo = "行政" Then '判斷是否佔床
                                            BedList &= String.Format("<div class=""Nurse_ward4"" style ="" font-size:{0}px; line-height:25px; left:0px;top:0px;border-color:#1C42D8;color:#333;font-weight:bold;height:24.5px;padding-top:-3px;text-align:center;border-style:groove;border-width:1px;border-radius:10px;float:left;width:20%;background-color:#eece11;font-family:Arial; "">{1}</div>", bedfontsize, Right(BedNo, Bedwordcount))
                                        ElseIf BedNo = "助理員" Then '助理員時縮小字型
                                            BedList &= String.Format("<div class=""Nurse_ward4"" style ="" font-size:23px; line-height:25px;  left:0px;top:0px;border-color:#1C42D8;color:#333;font-weight:bold;height:24.5px;padding-top:-3px;text-align:center;border-style:groove;border-width:1px;border-radius:10px;float:left;width:20%;background-color:#eece11;font-family:Arial;"">{0}</div>", BedNo)
                                        Else   '顯示空床,黑底白字
                                            BedList &= String.Format("<div class="" Nurse_ward4 "" style ="" font-size:{0}px; line-height:25px; background-color: #FFF;  left:0px;top:0px;border-color:#1C42D8;font-weight:bold;height:24.5px;padding-top:-3px;text-align:center;border-style:groove;border-width:1px;border-radius:10px;float:left;width:20%;font-family:Arial;"">{1}</div>", bedfontsize, Right(BedNo, Bedwordcount))
                                        End If
                                    End If
                                End If
                            Next

                        End If
                        '==================================
                        '處理備援資料====
                        Dim DVNurseList As DataView = NurseList.DefaultView
                        Dim DVNurseTable As DataTable = Nothing
                        Dim BackupCode As String = Nothing
                        DVNurseList.RowFilter = String.Format("EMPNO = '{0}'", NurseList.Rows(index)("BACKUP1"))
                        DVNurseTable = DVNurseList.ToTable
                        If (DVNurseTable.Rows.Count > 0) Then
                            BackupCode = Left(DVNurseTable.Rows(0)("DUTY").ToString, 1)
                        End If
                        '================
                        '處理護理人員資料
                        Dim EMPNO As String = Nothing
                        Dim Name As String = Adjust_Font_size(NurseList.Rows(index)("Name").ToString, 4, 36)
                        Dim Duty As String = NurseList.Rows(index)("Duty").ToString
                        Dim Xfire As String = NurseList.Rows(index)("Xfire").ToString
                        Dim Den As String = NurseList.Rows(index)("DEN").ToString

                        Dim Foundrows() As DataRow

                        NurseInfo &= String.Format("<div class=""Nurse"">")
                        NurseInfo &= String.Format("     <div  style=""width:50px;height:95px; float:left;"">")
                        NurseInfo &= String.Format("           <div style=""width:50px; height:20px;""></div>")
                        NurseInfo &= String.Format("          <div style=""width:50px; height:75px; font-size:20px;  line-height: 30px;vertical-align:middle;""  class=Numeric-font >{0}</div>", Xfire)

                        NurseInfo &= String.Format("     </div>")
                        NurseInfo &= String.Format("     <div  style=""width:150px;height:95px;margin-left:0px; float:left;"">")
                        'DUTY 欄位中有L時,將NAME顏色改黃色
                        If InStr(1, Duty, "L") > 0 Then
                            NurseInfo &= String.Format("          <div style=""width:150px; height:65px; font-size:50px;line-height:45px; color:rgb(239, 255, 0);"">{0}</div>", Name)
                        ElseIf Den = "H" Then   '判斷H班的護理人員名字以藍色顯示
                            NurseInfo &= String.Format("          <div style=""width:150px; height:65px; font-size:50px;line-height:45px;color:rgb(56, 238, 245);"">{0}</div>", Name)
                        Else
                            NurseInfo &= String.Format("          <div style=""width:150px; height:65px; font-size:50px;line-height:45px;"">{0}</div>", Name)
                        End If
                        NurseInfo &= String.Format("          <div   style=""width:150px; height:30px; font-size:40px;line-height:20px;  TEXT-ALIGN: center;color:#ffd800;"" class=Numeric-font >{0}  </div>", Duty)
                        NurseInfo &= String.Format("     </div>")
                        NurseInfo &= String.Format("     <div class = ""btn-group"" style=""width:400px;height:100px;border:3px;"">")
                        NurseInfo &= String.Format("          <div class =""container-fluid"" style=""width:400px;height:100px;padding:0px;text-align:left; "">")
                        NurseInfo &= String.Format("               <div class =""row row_adjust"" style=""width:400px;height:33px;text-align:left;margin-right:0px;margin-left:0px;"">{0}", BedList)
                        NurseInfo &= String.Format("               </div>")
                        NurseInfo &= String.Format("          </div>")
                        NurseInfo &= String.Format("     </div>")
                        NurseInfo &= String.Format("</div>")
                    End If
                Next

            Case "4"    '新竹護理人員版本
                Select Case NurseList.Rows.Count
                    Case Is <= 6 '新竹護理人員小於6的版本
                        For index = 0 To (NurseList.Rows.Count - 1)
                            If index < NSCount Then
                                '處理護士所負責的病床資料==========
                                Dim DV As DataView = DS.Tables("BedList").DefaultView
                                'GridView1.DataSource = DV
                                'GridView1.DataBind()
                                Dim DVTable As DataTable = Nothing
                                Dim BedList As String = Nothing '床位顯示字串
                                Dim BedCount As Integer = 0 '非空床床位數

                                DV.RowFilter = ""
                                DV.RowFilter = String.Format("EMPNO = '{0}'", NurseList.Rows(index)("EMPNO"))
                                DV.Sort = "Bedno"
                                DVTable = DV.ToTable 'DataView無法直接抓取己處理的資料,須轉為Table,DataView.table內是DataView的原始資料


                                '處理病床資料
                                '判斷是床位數小於等於15時顯示
                                If DV.Count < 16 Then
                                    For i = 0 To (DV.Count - 1)
                                        Dim BedNo As String = DVTable.Rows(i)("Bedno").ToString
                                        If Not String.IsNullOrEmpty(DVTable.Rows(i)("Bedno").ToString) Then
                                            If Not (BedNo = "書記" Or BedNo = "服務" Or BedNo = "助理員") Then
                                                BedCount += 1  '計算非書記、服務的床位數
                                            End If

                                            If i < Bed_Count Then '控制所要顯示的病床數
                                                If ERBed_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then '判斷是否為急診病患,如為變更框架為綠底黑字
                                                    BedList &= String.Format("<div class="" Nurse_ward_5 "" style =""background-color:rgb(0,221,0); "">{0}</div>", Right(BedNo, Bedwordcount)) '急診新進病床綠底黑字
                                                ElseIf NewBed_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then
                                                    BedList &= String.Format("<div class="" Nurse_ward_5 "" style =""background-color:rgb(17,238,238); "">{0}</div>", Right(BedNo, Bedwordcount)) '新進病床-藍底黑字
                                                ElseIf InBed_List.Contains(Right(BedNo, Bedwordcount)) Or BedNo = "書記" Or BedNo = "服務" Or BedNo = "組長" Or BedNo = "行政" Then '判斷是否佔床,"書記,服務,組長,行政"
                                                    BedList &= String.Format("<div class="" Nurse_ward_5"" >{0}</div>", Right(BedNo, Bedwordcount))
                                                ElseIf BedNo = "助理員" Then '助理員時縮小字型
                                                    BedList &= String.Format("<div class="" Nurse_ward_5 "" style ="" font-size:26px; "">{0}</div>", BedNo)
                                                Else '空床
                                                    BedList &= String.Format("<div class="" Nurse_ward_5"" style =""background-color:#fff;  "">{1}</div>", bedfontsize, Right(BedNo, Bedwordcount))
                                                End If
                                            End If
                                        End If
                                    Next
                                    '病床數大於16時縮小顯示
                                Else
                                    For i = 0 To (DV.Count - 1)
                                        Dim BedNo As String = DVTable.Rows(i)("Bedno").ToString
                                        If Not String.IsNullOrEmpty(DVTable.Rows(i)("Bedno").ToString) Then
                                            If Not (BedNo = "書記" Or BedNo = "服務" Or BedNo = "助理員") Then
                                                BedCount += 1  '計算非書記、服務的床位數
                                            End If

                                            If i < Bed_Count Then '控制所要顯示的病床數
                                                bedfontsize = 30
                                                If ERBed_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then '判斷是否為急診病患,如為變更框架為綠底黑字
                                                    BedList &= String.Format("<div class="" Nurse_ward_6 "" style =""background-color:rgb(0,221,0); "">{0}</div>", Right(BedNo, Bedwordcount)) '急診新進病床綠底黑字
                                                ElseIf NewBed_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then
                                                    BedList &= String.Format("<div class="" Nurse_ward_6 "" style =""background-color:rgb(17,238,238); "">{0}</div>", Right(BedNo, Bedwordcount)) '新進病床-藍底黑字
                                                ElseIf InBed_List.Contains(Right(BedNo, Bedwordcount)) Or BedNo = "書記" Or BedNo = "服務" Or BedNo = "組長" Or BedNo = "行政" Then '判斷是否佔床
                                                    BedList &= String.Format("<div class=""Nurse_ward_6"" >{0}</div>", Right(BedNo, Bedwordcount))
                                                ElseIf BedNo = "助理員" Then '助理員時縮小字型
                                                    BedList &= String.Format("<div class=""Nurse_ward_6"" style ="" font-size:26px; "">{0}</div>", BedNo)
                                                Else   '顯示空床,黑底白字
                                                    BedList &= String.Format("<div class="" Nurse_ward_6 "" style ="" background-color: #FFF; "">{0}</div>", Right(BedNo, Bedwordcount))
                                                End If
                                            End If
                                        End If
                                    Next

                                End If
                                '==================================
                                '處理備援資料====
                                Dim DVNurseList As DataView = NurseList.DefaultView
                                Dim DVNurseTable As DataTable = Nothing
                                Dim BackupCode As String = Nothing
                                DVNurseList.RowFilter = String.Format("EMPNO = '{0}'", NurseList.Rows(index)("BACKUP1"))
                                DVNurseTable = DVNurseList.ToTable
                                If (DVNurseTable.Rows.Count > 0) Then
                                    BackupCode = Left(DVNurseTable.Rows(0)("DUTY").ToString, 1)
                                End If
                                '================
                                '處理護理人員資料
                                Dim EMPNO As String = Nothing
                                Dim Name As String = Adjust_Font_size(NurseList.Rows(index)("Name").ToString, 4, 36)
                                Dim Duty As String = NurseList.Rows(index)("Duty").ToString
                                Dim Xfire As String = NurseList.Rows(index)("Xfire").ToString
                                Dim Den As String = NurseList.Rows(index)("DEN").ToString

                                Dim Foundrows() As DataRow

                                NurseInfo &= String.Format("<div class=""Nurse_Max"">")
                                NurseInfo &= String.Format("     <div  style=""width:50px;height:95px; float:left;"">")
                                NurseInfo &= String.Format("           <div style=""width:50px; height:20px;""></div>")
                                NurseInfo &= String.Format("          <div style=""width:50px; height:75px; font-size:20px;  line-height: 30px;vertical-align:middle;""  class=Numeric-font >{0}</div>", Xfire)

                                NurseInfo &= String.Format("     </div>")
                                NurseInfo &= String.Format("     <div  style=""width:150px;height:95px;margin-left:0px; float:left;"">")
                                'DUTY 欄位中有L時,將NAME顏色改黃色
                                If InStr(1, Duty, "L") > 0 Then
                                    NurseInfo &= String.Format("          <div style=""width:150px; height:65px; font-size:50px;line-height:45px; color:rgb(239, 255, 0);"">{0}</div>", Name)
                                ElseIf Den = "H" Then   '判斷H班的護理人員名字以藍色顯示
                                    NurseInfo &= String.Format("          <div style=""width:150px; height:65px; font-size:50px;line-height:45px;color:rgb(56, 238, 245);"">{0}</div>", Name)
                                Else
                                    NurseInfo &= String.Format("          <div style=""width:150px; height:65px; font-size:50px;line-height:45px;"">{0}</div>", Name)
                                End If


                                NurseInfo &= String.Format("          <div   style=""width:150px; height:80px; font-size:40px;line-height:40px;  TEXT-ALIGN: center;color:#ffd800;"" class=Numeric-font >{0}  </div>", Duty)
                                NurseInfo &= String.Format("     </div>")
                                NurseInfo &= String.Format("     <div class = ""btn-group"" style=""width:400px;height:100px;border:3px;"">")
                                NurseInfo &= String.Format("          <div class =""container-fluid"" style=""width:400px;height:100px;padding:0px;text-align:left; "">")
                                NurseInfo &= String.Format("               <div class =""row row_adjust"" style=""width:400px;height:33px;text-align:left;margin-right:0px;margin-left:0px;"">{0}", BedList)
                                NurseInfo &= String.Format("               </div>")
                                NurseInfo &= String.Format("          </div>")
                                NurseInfo &= String.Format("     </div>")
                                NurseInfo &= String.Format("</div>")
                            End If
                        Next
                    Case Is > 6  '新竹護理人員大於6的版本
                        For index = 0 To (NurseList.Rows.Count - 1)
                            If index < NSCount Then
                                '處理護士所負責的病床資料==========
                                Dim DV As DataView = DS.Tables("BedList").DefaultView
                                'GridView1.DataSource = DV
                                'GridView1.DataBind()
                                Dim DVTable As DataTable = Nothing
                                Dim BedList As String = Nothing '床位顯示字串
                                Dim BedCount As Integer = 0 '非空床床位數

                                DV.RowFilter = ""
                                DV.RowFilter = String.Format("EMPNO = '{0}'", NurseList.Rows(index)("EMPNO"))
                                DV.Sort = "Bedno"
                                DVTable = DV.ToTable 'DataView無法直接抓取己處理的資料,須轉為Table,DataView.table內是DataView的原始資料


                                '處理病床資料
                                '判斷是床位數小於等於15時顯示
                                If DV.Count < 16 Then
                                    For i = 0 To (DV.Count - 1)
                                        Dim BedNo As String = DVTable.Rows(i)("Bedno").ToString
                                        If Not String.IsNullOrEmpty(DVTable.Rows(i)("Bedno").ToString) Then
                                            If Not (BedNo = "書記" Or BedNo = "服務" Or BedNo = "助理員") Then
                                                BedCount += 1  '計算非書記、服務的床位數
                                            End If

                                            If i < Bed_Count Then '控制所要顯示的病床數
                                                If ERBed_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then '判斷是否為急診病患,如為變更框架為綠底黑字
                                                    BedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:32px;background-color:rgb(0,221,0); "">{1}</div>", bedfontsize, Right(BedNo, Bedwordcount)) '急診新進病床綠底黑字
                                                ElseIf NewBed_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then
                                                    BedList &= String.Format("<div class="" Nurse_ward "" style =""background-color:rgb(17,238,238); "">{0}</div>", Right(BedNo, Bedwordcount)) '新進病床-藍底黑字
                                                ElseIf InBed_List.Contains(Right(BedNo, Bedwordcount)) Or BedNo = "書記" Or BedNo = "服務" Or BedNo = "組長" Or BedNo = "行政" Then '判斷是否佔床,"書記,服務,組長,行政"
                                                    BedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:32px; "">{1}</div>", bedfontsize, Right(BedNo, Bedwordcount))
                                                ElseIf BedNo = "助理員" Then '助理員時縮小字型
                                                    BedList &= String.Format("<div class="" Nurse_ward_3 "">{0}</div>", BedNo)
                                                Else '空床
                                                    BedList &= String.Format("<div class="" Nurse_ward_2"" style ="" font-size:{0}px; line-height:32px; "">{1}</div>", bedfontsize, Right(BedNo, Bedwordcount))
                                                End If
                                            End If
                                        End If
                                    Next
                                    '病床數大於16時縮小顯示
                                Else
                                    For i = 0 To (DV.Count - 1)
                                        Dim BedNo As String = DVTable.Rows(i)("Bedno").ToString
                                        If Not String.IsNullOrEmpty(DVTable.Rows(i)("Bedno").ToString) Then
                                            If Not (BedNo = "書記" Or BedNo = "服務" Or BedNo = "助理員") Then
                                                BedCount += 1  '計算非書記、服務的床位數
                                            End If

                                            If i < Bed_Count Then '控制所要顯示的病床數
                                                bedfontsize = 30
                                                If ERBed_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then '判斷是否為急診病患,如為變更框架為綠底黑字
                                                    BedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:25px;height:24.5px;background-color:rgb(0,221,0); "">{1}</div>", bedfontsize, Right(BedNo, Bedwordcount)) '急診新進病床綠底黑字
                                                ElseIf NewBed_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then
                                                    BedList &= String.Format("<div class="" Nurse_ward_4 "" style =""background-color:rgb(17,238,238); "">{0}</div>", Right(BedNo, Bedwordcount)) '新進病床-藍底黑字
                                                ElseIf InBed_List.Contains(Right(BedNo, Bedwordcount)) Or BedNo = "書記" Or BedNo = "服務" Or BedNo = "組長" Or BedNo = "行政" Then '判斷是否佔床
                                                    BedList &= String.Format("<div class=""Nurse_ward4"" style ="" font-size:{0}px; line-height:25px; left:0px;top:0px;border-color:#1C42D8;color:#333;font-weight:bold;height:24.5px;padding-top:-3px;text-align:center;border-style:groove;border-width:1px;border-radius:10px;float:left;width:20%;background-color:#eece11;font-family:Arial; "">{1}</div>", bedfontsize, Right(BedNo, Bedwordcount))
                                                ElseIf BedNo = "助理員" Then '助理員時縮小字型
                                                    BedList &= String.Format("<div class=""Nurse_ward4"" style ="" font-size:23px; line-height:25px;  left:0px;top:0px;border-color:#1C42D8;color:#333;font-weight:bold;height:24.5px;padding-top:-3px;text-align:center;border-style:groove;border-width:1px;border-radius:10px;float:left;width:20%;background-color:#eece11;font-family:Arial;"">{0}</div>", BedNo)
                                                Else   '顯示空床,黑底白字
                                                    BedList &= String.Format("<div class="" Nurse_ward4 "" style ="" font-size:{0}px; line-height:25px; background-color: #FFF;  left:0px;top:0px;border-color:#1C42D8;font-weight:bold;height:24.5px;padding-top:-3px;text-align:center;border-style:groove;border-width:1px;border-radius:10px;float:left;width:20%;font-family:Arial;"">{1}</div>", bedfontsize, Right(BedNo, Bedwordcount))
                                                End If
                                            End If
                                        End If
                                    Next

                                End If
                                '==================================
                                '處理備援資料====
                                Dim DVNurseList As DataView = NurseList.DefaultView
                                Dim DVNurseTable As DataTable = Nothing
                                Dim BackupCode As String = Nothing
                                DVNurseList.RowFilter = String.Format("EMPNO = '{0}'", NurseList.Rows(index)("BACKUP1"))
                                DVNurseTable = DVNurseList.ToTable
                                If (DVNurseTable.Rows.Count > 0) Then
                                    BackupCode = Left(DVNurseTable.Rows(0)("DUTY").ToString, 1)
                                End If
                                '================
                                '處理護理人員資料
                                Dim EMPNO As String = Nothing
                                Dim Name As String = Adjust_Font_size(NurseList.Rows(index)("Name").ToString, 4, 36)
                                Dim Duty As String = NurseList.Rows(index)("Duty").ToString
                                Dim Xfire As String = NurseList.Rows(index)("Xfire").ToString
                                Dim Den As String = NurseList.Rows(index)("DEN").ToString

                                Dim Foundrows() As DataRow

                                NurseInfo &= String.Format("<div class=""Nurse"">")
                                NurseInfo &= String.Format("     <div  style=""width:50px;height:95px; float:left;"">")
                                NurseInfo &= String.Format("           <div style=""width:50px; height:20px;""></div>")
                                NurseInfo &= String.Format("          <div style=""width:50px; height:75px; font-size:20px;  line-height: 30px;vertical-align:middle;""  class=Numeric-font >{0}</div>", Xfire)

                                NurseInfo &= String.Format("     </div>")
                                NurseInfo &= String.Format("     <div  style=""width:150px;height:95px;margin-left:0px; float:left;"">")
                                'DUTY 欄位中有L時,將NAME顏色改黃色
                                If InStr(1, Duty, "L") > 0 Then
                                    NurseInfo &= String.Format("          <div style=""width:150px; height:65px; font-size:50px;line-height:45px; color:rgb(239, 255, 0);"">{0}</div>", Name)
                                ElseIf Den = "H" Then   '判斷H班的護理人員名字以藍色顯示
                                    NurseInfo &= String.Format("          <div style=""width:150px; height:65px; font-size:50px;line-height:45px;color:rgb(56, 238, 245);"">{0}</div>", Name)
                                Else
                                    NurseInfo &= String.Format("          <div style=""width:150px; height:65px; font-size:50px;line-height:45px;"">{0}</div>", Name)
                                End If
                                NurseInfo &= String.Format("          <div   style=""width:150px; height:30px; font-size:40px;line-height:20px;  TEXT-ALIGN: center;color:#ffd800;"" class=Numeric-font >{0}  </div>", Duty)
                                NurseInfo &= String.Format("     </div>")
                                NurseInfo &= String.Format("     <div class = ""btn-group"" style=""width:400px;height:100px;border:3px;"">")
                                NurseInfo &= String.Format("          <div class =""container-fluid"" style=""width:400px;height:100px;padding:0px;text-align:left; "">")
                                NurseInfo &= String.Format("               <div class =""row row_adjust"" style=""width:400px;height:33px;text-align:left;margin-right:0px;margin-left:0px;"">{0}", BedList)
                                NurseInfo &= String.Format("               </div>")
                                NurseInfo &= String.Format("          </div>")
                                NurseInfo &= String.Format("     </div>")
                                NurseInfo &= String.Format("</div>")
                            End If
                        Next
                End Select

        End Select

        NurseListLabel.Text = NurseInfo
        DS.Dispose()
        Conn.Close()



        Return DS.Tables("NurseList").Rows.Count
    End Function
    '====護士排序方式--以病床排序
    Public Function NsBedSort(ByVal NurseList As DataTable, ByVal BedList As DataTable, ByVal Nssort As String) As DataTable
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
    '====護士排序方式--以職務排序,已不使用
    Public Function NsDutySort(ByVal NurseList As DataTable, ByVal BedList As DataTable)
        '新增NurseList 的欄位Sort
        NurseList.Columns.Add("Sort")
        For i = 0 To NurseList.Rows.Count - 1
            Dim temprows As DataRow = NurseList.Rows(i)
            Dim temprows2() As DataRow
            temprows2 = BedList.Select(String.Format("Empno = '{0}'", NurseList.Rows(i)("empno")), "BEDNO")

            temprows("sort") = NurseList.Rows(i)("DUTY")
            If NurseList.Rows(i)("Den") = "H" Then
                temprows("sort") = "Z"
            End If
            If temprows2.Count > 0 Then
                If temprows2(0)("Bedno").ToString = "書記" Or temprows2(0)("Bedno").ToString = "服務" Then
                    temprows("sort") = temprows2(0)("Bedno")
                End If
            End If
        Next
        Dim DV As DataView = New DataView(NurseList)
        DV.Sort = "Sort "
        Dim final As DataTable = DV.ToTable
        Return final
    End Function



    '############################################主治及住院醫師資料#####################################################
    Protected Sub BanList(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Dr_Upper_limite As Integer, ByVal Bed_Upper_limite As Integer, ByVal Now_time As DateTime)
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim DR_List As ArrayList = New ArrayList '主治醫師列表
        Dim New_Patient_List As ArrayList = New ArrayList  '今日新今日新進病人名單,全五碼
        Dim Nowdate As String = Now_time.ToString("yyyy/MM/dd 00:00:00")


        '所有病房內病床和主治醫師資料==Bedtbl
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Conn.Open()
        Dim cmd As String = String.Format("SELECT  Bedno, Beddate, beddr, beddr2, beddr3, beddr4, beddr5, beddr6, beddr7, beddr8, beddr9   FROM Bedtbl  WHERE Bedns='{0}' And Bedsts='1' AND Bedroom!='99'", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataSet
        DA.Fill(DS, "BedInfo")


        '醫生排序用列表
        Dim Doctor_info As DataTable = New DataTable
        Doctor_info.Columns.Add("AD_EMPNO")
        Doctor_info.Columns.Add("AD_NAME")
        Doctor_info.Columns.Add("HD_EMPNO")
        Doctor_info.Columns.Add("HD_NAME")
        Doctor_info.Columns.Add("Bed_Qt", Type.GetType("System.Int16")) '將欄位宣告為Int,排序時才不會出錯

        '取出所有主治醫師名單=============================
        For i = 0 To (DS.Tables("BedInfo").Rows.Count - 1)
            For j = 2 To 10
                If Not (DS.Tables("BedInfo").Rows(i)(j).ToString = String.Empty) Then
                    If Not (DR_List.Contains(DS.Tables("BedInfo").Rows(i)(j).ToString)) Then
                        DR_List.Add(DS.Tables("BedInfo").Rows(i)(j))
                    End If
                End If
            Next
            '取出NEW PATIENT的病房名單
            If DS.Tables("BedInfo").Rows(i)(1) = Nowdate Then
                New_Patient_List.Add(DS.Tables("BedInfo").Rows(i)(0))
            End If
        Next
        DR_List.Sort()

        '*****************New Patient , 急診New Patient,佔床名單

        Dim cmd4 As String = String.Format("SELECT A.bedno,B.pnofrom, B.admdate,b.caseno , b.firstcaseno  From BEDTBL A, CASETBL B WHERE A.bedpno = B.pno AND A.bedcaseno = B.caseno And (A.bedns = '{0}') AND Bedsts = '1' AND Bedroom != '99'", Wardcode)
        Dim DA4 As OleDbDataAdapter = New OleDbDataAdapter(cmd4, Conn)
        Dim InBed_List As ArrayList = New ArrayList '目前佔床的床位清單
        Dim NewBed_List As ArrayList = New ArrayList '新住院病人清單 
        Dim ERBed_List As ArrayList = New ArrayList ' 從急診轉來的病患床位清單
        Dim Bedwordcount As Integer = 3
        DA4.Fill(DS, "InBed")
        ' GridView1.DataSource = DS.Tables("InBed")
        'GridView1.DataBind()
        For l = 0 To DS.Tables("InBed").Rows.Count - 1
            Dim bedno As String = DS.Tables("InBed").Rows(l)("Bedno").ToString '顯示於白板後三碼病房號
            Dim bedfrom As String = DS.Tables("Inbed").Rows(l)("pnofrom").ToString '
            Dim admdate As String = String.Format("{0:yyyy/MM/dd 00:00:00}", DS.Tables("Inbed").Rows(l)("admdate"))
            Dim caseno As String = DS.Tables("Inbed").Rows(l)("Caseno").ToString
            Dim firstcaseno As String = DS.Tables("Inbed").Rows(l)("firstCaseno").ToString
            '佔床病人列表
            If Not InBed_List.Contains(bedno) Then
                InBed_List.Add(bedno)
            End If
            '新住院病人列表(不含急診新進病人)
            If Not NewBed_List.Contains(bedno) And Not (bedfrom = "2" Or bedfrom = "4" Or bedfrom = "6" Or bedfrom = "8") And FormatDateTime(admdate, DateFormat.ShortDate) = FormatDateTime(Nowdate, DateFormat.ShortDate) And caseno = firstcaseno Then
                NewBed_List.Add(bedno)
            End If
            '急診新進病人列表
            If (Not ERBed_List.Contains(bedno)) And (bedfrom = "2" Or bedfrom = "4" Or bedfrom = "6" Or bedfrom = "8") And FormatDateTime(admdate, DateFormat.ShortDate) = FormatDateTime(Nowdate, DateFormat.ShortDate) And caseno = firstcaseno Then
                ERBed_List.Add(bedno)
            End If
        Next
        GridView2.Caption = "住院病床狀態"
        ' GridView2.DataSource = InBed_List
        'GridView2.DataBind()


        '*****************
        '取出醫師姓名名單@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        Dim Doctor_name_list_cmd As String
        Dim Temp_name As String
        If DR_List.Count > 0 Then


            For u = 0 To (DR_List.Count - 1)
                If (u = DR_List.Count - 1) Then
                    Temp_name &= String.Format("CODE ='{0}'", DR_List(u))
                Else
                    Temp_name &= String.Format("CODE ='{0}' OR ", DR_List(u))
                End If
            Next
            Dim Conn2 As OleDbConnection = New OleDbConnection
            Conn2.ConnectionString = ConnStr
            Conn2.Open()
            Doctor_name_list_cmd = String.Format("SELECT * FROM DR WHERE {0}", Temp_name)
            Dim DA2 As OleDbDataAdapter = New OleDbDataAdapter(Doctor_name_list_cmd, Conn2)
            DA2.Fill(DS, "Doctor_name_list")

            Conn2.Close()
        End If


        '取主治醫師所負責的病床資料#################
        Dim DV As DataView = New DataView(DS.Tables("BedInfo").DefaultView.ToTable)
        Dim BedCount = DV.ToTable.Rows.Count

        For i = 0 To (DR_List.Count - 1)
            DV.RowFilter = String.Format("Beddr2 = '{0}' OR Beddr3 = '{0}' OR Beddr4 = '{0}' OR Beddr5 = '{0}' OR Beddr6 = '{0}' OR Beddr7 = '{0}' OR Beddr8 = '{0}' OR Beddr9 = '{0}' ", DR_List(i))
            Dim Temp_Table As DataTable = DV.ToTable

            If Temp_Table.Rows.Count > 0 Then
                For j = 0 To (Temp_Table.Rows.Count - 1)
                    Dim New_Row As DataRow = DS.Tables("BedInfo").NewRow()
                    New_Row("Bedno") = Temp_Table.Rows(j)("Bedno").ToString
                    New_Row("Beddate") = Temp_Table.Rows(j)("Beddate").ToString
                    New_Row("Beddr") = DR_List(i).ToString
                    DS.Tables("BedInfo").Rows.Add(New_Row)
                Next
            End If
            '建立總表
            '醫師資料總表****************
            Dim New_row1 As DataRow = Doctor_info.NewRow()
            '取出醫生所負責的病房個數
            Dim SCHED_COUNT As DataView = New DataView(DS.Tables("BedInfo").DefaultView.ToTable)
            SCHED_COUNT.RowFilter = String.Format("Beddr = '{0}'", DR_List(i))
            Dim Bed_Qt As Integer = SCHED_COUNT.Count
            Dim AD_NAME() As DataRow = DS.Tables("Doctor_name_list").Select(String.Format("CODE = '{0}'", DR_List(i)))
            If AD_NAME.Count > 0 Then
                New_row1("AD_EMPNO") = DR_List(i).ToString
                New_row1("AD_NAME") = AD_NAME(0)("NAME").ToString
                New_row1("Bed_Qt") = Bed_Qt
            End If
            Doctor_info.Rows.Add(New_row1)
            '*****************************
        Next


        Dim Doctor_info_sortV As DataView = New DataView(Doctor_info.DefaultView.ToTable)
        Doctor_info_sortV.Sort = "Bed_Qt DESC"
        Dim Doctor_info_sort As DataTable = Doctor_info_sortV.ToTable

        Dim Bed_info_sortV As DataView = New DataView(DS.Tables("BedInfo").DefaultView.ToTable)
        Bed_info_sortV.Sort = "Bedno"
        Dim Bed_info_sort As DataTable = Bed_info_sortV.ToTable


        '資料輸出$$$$$$$$$$$$$$$$$$$$$$$$$$
        Dim Doctor_Info_show As String = Nothing 'Doctor1醫生資料輸出
        Dim Doctor_Row_count As Integer = 0


        For w = 0 To (Mymin(Doctor_info_sort.Rows.Count - 1, Dr_Upper_limite - 1))
            Dim Bed_Row_count As Integer = 0
            '&&&&&&病床資料輸出
            Dim Bedrow() As DataRow = Bed_info_sort.Select(String.Format("BEDDR = '{0}'", Doctor_info_sort.Rows(w)("AD_EMPNO")))
            Dim BedRow_Show As String = Nothing

            For Each Data_Row As DataRow In Bedrow
                Bed_Row_count = Bed_Row_count + 1
                If Bed_Row_count <= Bed_Upper_limite Then '只顯示前10筆資料
                    If NewBed_List.Contains(Data_Row(0)) Then '判斷是否NEW PATIENT
                        BedRow_Show &= String.Format("<div class=""Nurse_ward_7"" style= ""background-color: rgb(17,238,238);"">{0}</div>", Right(Data_Row(0), 3))
                    ElseIf ERBed_List.Contains(Data_Row(0)) Then '判斷是否急診NEW PATIENT
                        BedRow_Show &= String.Format("<div class=""Nurse_ward_7"" style= ""background-color: rgb(0,221,0);"">{0}</div>", Right(Data_Row(0), 3))
                    Else  '佔床病人
                        BedRow_Show &= String.Format("<div class=""Nurse_ward_7"">{0}</div>", Right(Data_Row(0), 3))
                    End If
                End If
            Next

            '&&&&&&醫生資料輸出
            Doctor_Info_show += String.Format("<div style=""width:750px;height:63.9px;border-bottom-style:groove;border-width:1px;margin:0px;"" > ")
            Doctor_Info_show += String.Format("         <div style=""height:62px;width:125px;float:left;"">")
            Doctor_Info_show += String.Format("                 <div style=""height:42px; width:125px; float:left; font-size:40px; line-height:34px; text-align:center; background-color:#F0A75F;"">{0}</div>", Doctor_info_sort.Rows(w)("AD_NAME"))
            Doctor_Info_show += String.Format("                 <div style=""height:20px; width:125px; float:left; font-size:30px; line-height:16px; text-align:center; background-color:#F0A75F; font-family:Arial;"">{0}</div>", Doctor_info_sort.Rows(w)("AD_EMPNO"))
            Doctor_Info_show += String.Format("         </div>")
            Doctor_Info_show += String.Format("         <div style=""height:62px;width:625px;float:left"">{0}", BedRow_Show)
            Doctor_Info_show += String.Format("         </div>")
            Doctor_Info_show += String.Format("</div>")
        Next

        Doctor_Info_Label.Text = Doctor_Info_show
        'GridView1.DataSource = Doctor_info
        'GridView1.DataBind()

    End Sub
    '回傳較小值
    Public Function Mymin(ByVal int1 As Integer, ByVal int2 As Integer) As Integer
        Dim min As Integer = 0
        Dim temp As Integer = 0
        min = int1
        If int2 <= int1 Then
            min = int2
        End If
        Return min
    End Function
    '####################################################團隊連絡資料###################################################(共用)
    Public Function Contact_List(ByVal Hospcode As String, ByVal Wardcode As String, ByVal ContactCount As Integer)
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM MMH.Whtboard AL1 WHERE AL1.NS='{0}' AND seqno != 'xx'  and (empno not in ('mstr','day','duty','DEN') OR empno is  NULL) and grp is not null ORDER BY AL1.Seqno", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataSet
        Dim ContactInfo As String = Nothing
        DA.Fill(DS, "ContactList") '連絡人資料
        For index = 0 To (DS.Tables("ContactList").Rows.Count - 1)
            Dim Grp As String = DS.Tables("ContactList").Rows(index)("Grp").ToString
            Dim Name As String = DS.Tables("ContactList").Rows(index)("Name").ToString
            Dim Tel As String = DS.Tables("ContactList").Rows(index)("Tel").ToString
            Dim Strtemp As String = Grp + Name
            Grp = Adjust_Font_size(Grp, 6, 28)

            If index < (ContactCount) Then '只顯示12筆資料
                ContactInfo &= String.Format("<div  style=""height:8.2%; border-radius:15px; border-style:solid; border-width:1px; margin-bottom:1px;"">")
                ContactInfo &= String.Format("        <div style=""width:250px;height:73px;font-size:48px; text-align:left; line-height:70px; float:left; word-break:break-all;"">{0}</div>", Grp)
                ContactInfo &= String.Format("        <div style=""width:250px;height:73px;float:left"">")
                ContactInfo &= String.Format("                <div style=""width:250px;height:36px; font-size:40px; text-align:center; line-height:38px; font-weight:500;"">{0}</div>", Name)
                ContactInfo &= String.Format("                <div style=""width:250px;height:36px; font-size:36px; text-align:center; line-height:38px; font-weight:bold;"">{0}</div>", Tel)
                ContactInfo &= String.Format("        </div>")
                ContactInfo &= String.Format("</div>")

            End If
        Next
        ContactListLabel.Text = ContactInfo
    End Function
    '####################################################主程式###########################################################
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim Now_Time As DateTime = GET_Now_Time(HospCode)

        Dim key_time As String = Request.QueryString("time")
        If Not String.IsNullOrEmpty(key_time) Then
            Now_Time = DateTime.Parse(key_time)
            Response.Write("測試時間:" & key_time)
        End If


        Dim NsSort As String = Request.QueryString("NsSort")
        Dim MSTR As String = Get_Master(HospCode, WardCode)
        'ORACLE系統時間

        '####測試時間專用
        'Now_Time = Now_Time.AddDays(-1)
        'Response.Write(String.Format("目前系統時間為：{0}", Now_Time))
        '####
        'Dim Temp_Time As String = "2018/04/11 00:00:00"
        'Dim Now_Time As DateTime = Temp_Time.t
        'Dim Now_Time As DateTime = Now.AddHours(-12)
        'Response.Write(Now_Time)
        '主治醫師依所選白天呈現科別呈現的大科別
        Dim DAY_Banlist() As String = {"IM"}
        Dim Dancag As String = Get_Dancag(HospCode, WardCode, 0, Now_Time) '控制護理站名稱和護士班表切換時點
        Dim NSENG = Show_Ward_Name(HospCode, WardCode, Dancag) '顯示護理站簡稱
        Show_Statistics_2(HospCode, WardCode, Now_Time) '畫面上方統計資料
        Nurse_List(HospCode, WardCode, 9, 20, NsSort, Dancag, Now_Time) '列出護士班表資料
        BanList(HospCode, WardCode, 15, 16, Now_Time)
        ' BanList_NS_D(HospCode, WardCode, 14, 10, Now_Time) '目前護理站主治醫師班表
        Contact_List(HospCode, WardCode, 12) '列出連絡人資料
        URL_Change_Label.Text = String.Format("<a href=""station-N.aspx?Hospcode={0}&Wardcode={1}"" target=""_self"" style =""color:white;"">輕</a><br>", HospCode, WardCode)
        URL_Reset_Label.Text = String.Format("<a href=""station.aspx?Hospcode={0}&Wardcode={1}"" target=""_self"" style =""color:white;"">中</a><br>", HospCode, WardCode)
        '測試
    End Sub


End Class
