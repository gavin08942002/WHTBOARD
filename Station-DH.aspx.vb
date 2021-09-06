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
        cmd_state &= " Order By Recdate,Bedno"
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


        '護士區急診交班(30,60,90,已交班)要顯示的單位
        Dim ORHCList() As String = {"1"}
        Dim ORWCList() As String = {"40", "43", "44"}
        Dim ORSHOWList As String = "N"
        If ORHCList.Contains(Hospcode) And ORWCList.Contains(Wardcode) Then
            ORSHOWList = "Y"
        End If



        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim ConnORA As String = SELECT_ORACLE(0)
        Dim Nowdate As String = Now_Time.ToString("yyyy/MM/dd 00:00:00")
        Dim Nowtime As String = Now_Time.ToString("yyyy/MM/dd HH:00:00")
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
                '    Response.Write(FromDate)
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

        '尿管移除提示資料名單(以字體閃鑠表示)
        'Dim H_cmd3 As String = String.Format("select bedns,bedno,bedpno,bedcaseno from bedtbl A, cauti B where a.bedpno = b.pno And a.bedcaseno = b.caseno And a.bedsts > 0 and b.show_msg='Y' and trunc(msg_bdate)=trunc(sysdate)  and a.bedns = '{0}'  order by bedns,bedno,bedpno", Wardcode)
        Dim H_cmd3 As String = String.Format("select bedns,bedno,bedpno,bedcaseno,msg_bdate,rdate from bedtbl A, cauti B left join cautidly C on b.pno=c.pno and b.ordlinkno=c.ordlinkno where a.bedpno = b.pno And a.bedcaseno = b.caseno And a.bedsts > 0 and b.show_msg='Y' and a.bedns='{0}' and trunc(rdate)=trunc(sysdate) order by bedns,bedno,bedpno,rdate ", Wardcode)
        Dim H_DA3 As OleDbDataAdapter = New OleDbDataAdapter(H_cmd3, Conn)
        H_DA3.Fill(DS, "H_BedCauti")
        Dim cauti_List As ArrayList = New ArrayList
        For c = 0 To DS.Tables("H_BedCauti").Rows.Count - 1
            Dim cauti_bedno = Right(DS.Tables("H_BedCauti").Rows(c)("bedno"), Bedwordcount)
            If Not cauti_List.Contains(cauti_bedno) Then
                cauti_List.Add(cauti_bedno)
            End If
            'Response.Write(cauti_bedno & ",")
        Next
        'Response.Write(cauti_List(0).ToString)
        'GridView1.Caption = "住院病人列表"
        'GridView1.DataSource = DS.Tables("H_BedCauti")
        'GridView1.DataBind()


        '護理人員病床排序,NSSort的參數為'bedno'時將護士排序規則變更為以病床排序

        Dim NurseList As DataTable = New DataTable

        NurseList = NsBedSort(DS.Tables("NurseList"), DS.Tables("BedList"), NsSort)


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

        '佔床病人名單
        Dim cmd4 As String = String.Format("SELECT A.bedno,A.opdate,B.pnofrom, B.admdate,B.arrivaltime,b.caseno , b.firstcaseno, round(to_number(sysdate-b.admdatetime)*24*60) as timetemp, E.etime  From BEDTBL A, CASETBL B,EMGshift E WHERE A.bedpno = B.pno AND A.bedcaseno = B.caseno And A.bedpno = E.pno(+) and a.bedcaseno = e.bedcaseno(+) and (A.bedns = {0}) AND A.Bedsts = '1' AND A.Bedroom != '99'", Wardcode_temp)
        Dim DA4 As OleDbDataAdapter = New OleDbDataAdapter(cmd4, Conn)
        DA4.Fill(DS, "InBed")


        Dim InBed_List As ArrayList = New ArrayList '目前佔床的床位清單
        Dim NewBed_List As ArrayList = New ArrayList '新住院病人清單 
        Dim ERBed_List As ArrayList = New ArrayList ' 從急診轉來的病患床位清單
        Dim Handover_List As ArrayList = New ArrayList '護理急診住院交班已交班清單
        Dim During3060m_List As ArrayList = New ArrayList '急診轉住院30-60分鐘
        Dim During6090m_List As ArrayList = New ArrayList '急診轉住院60-90分鐘
        Dim UP90m_List As ArrayList = New ArrayList '急診轉住院超過90分鐘

        '插尿管閃爍測試用院區
        Dim Testhost As ArrayList = New ArrayList
        Testhost.Add("1")
        Testhost.Add("2")
        '插尿管閃爍測試用護理站
        Dim TestStation As ArrayList = New ArrayList
        TestStation.Add("26")
        TestStation.Add("27")
        TestStation.Add("34")
        TestStation.Add("36")
        TestStation.Add("46")
        TestStation.Add("62")
        TestStation.Add("63")
        TestStation.Add("79")
        TestStation.Add("88")


        'GridView1.Caption = "住院病人列表"
        'GridView1.DataSource = DS.Tables("InBed")
        'GridView1.DataBind()
        For l = 0 To DS.Tables("InBed").Rows.Count - 1
            Dim bedno As String = Right(DS.Tables("InBed").Rows(l)("Bedno"), Bedwordcount) '顯示於白板後三碼病房號
            Dim bedfrom As String = DS.Tables("Inbed").Rows(l)("pnofrom").ToString '
            Dim admdate As String = String.Format("{0:yyyy/MM/dd 00:00:00}", DS.Tables("Inbed").Rows(l)("admdate"))
            Dim if_handover As Boolean = Not String.IsNullOrEmpty(DS.Tables("Inbed").Rows(l)("etime").ToString)
            Dim timetemp As Integer = DS.Tables("Inbed").Rows(l)("timetemp")

            If Not String.IsNullOrEmpty(DS.Tables("Inbed").Rows(l)("arrivaltime").ToString) Then
                Dim arrivaltime As DateTime = DateTime.Parse(DS.Tables("Inbed").Rows(l)("arrivaltime").ToString)
            End If

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
            '急診新進病人轉住院30-60分且未交班列表
            If (Not During3060m_List.Contains(bedno)) And (bedfrom = "2" Or bedfrom = "4" Or bedfrom = "6" Or bedfrom = "8") And FormatDateTime(admdate, DateFormat.ShortDate) = FormatDateTime(Nowdate, DateFormat.ShortDate) And caseno = firstcaseno And Not if_handover And timetemp > 30 And timetemp < 59 And ORSHOWList = "Y" Then
                During3060m_List.Add(bedno)
            End If
            '急診新進病人轉住院60-90分且未交班列表
            If (Not During6090m_List.Contains(bedno)) And (bedfrom = "2" Or bedfrom = "4" Or bedfrom = "6" Or bedfrom = "8") And FormatDateTime(admdate, DateFormat.ShortDate) = FormatDateTime(Nowdate, DateFormat.ShortDate) And caseno = firstcaseno And Not if_handover And timetemp > 60 And timetemp < 89 And ORSHOWList = "Y" Then
                During6090m_List.Add(bedno)
            End If
            '急診新進病人轉住院超過90分且未交班列表
            If (Not UP90m_List.Contains(bedno)) And (bedfrom = "2" Or bedfrom = "4" Or bedfrom = "6" Or bedfrom = "8") And FormatDateTime(admdate, DateFormat.ShortDate) = FormatDateTime(Nowdate, DateFormat.ShortDate) And caseno = firstcaseno And Not if_handover And timetemp > 90 And ORSHOWList = "Y" Then
                UP90m_List.Add(bedno)
            End If
            '急診新進病人已交班列表
            If (Not Handover_List.Contains(bedno)) And (bedfrom = "2" Or bedfrom = "4" Or bedfrom = "6" Or bedfrom = "8") And FormatDateTime(admdate, DateFormat.ShortDate) = FormatDateTime(Nowdate, DateFormat.ShortDate) And caseno = firstcaseno And if_handover And ORSHOWList = "Y" Then
                Handover_List.Add(bedno)
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

                                        '判斷是否要閃爍文字
                                        Dim front As String = Nothing
                                        Dim back As String = Nothing
                                        If cauti_List.Contains(Right(DVTable.Rows(i)("Bedno").ToString, Bedwordcount)) And TestStation.Contains(Wardcode) And Testhost.Contains(Hospcode) Then
                                            front = "<span class=""neon-effect"">"
                                            back = "</span>"
                                        End If


                                        '已完成交班,深藍底白字
                                        If Handover_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then
                                            BedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:32px;background-color:rgb(0,0,255);color:white; "">{1}{2}{3}</div>", bedfontsize, front, Right(BedNo, Bedwordcount), back)
                                            '急診新進病人30-60分,粉紅底白字
                                        ElseIf During3060m_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then
                                            BedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:32px;background-color:rgb(255,140,255);color:white; "">{1}{2}{3}</div>", bedfontsize, front, Right(BedNo, Bedwordcount), back)
                                            '急診新進病人60-90分,紅底白字
                                        ElseIf During6090m_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then
                                            BedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:32px;background-color:rgb(255,0,0);color:white; "">{1}{2}{3}</div>", bedfontsize, front, Right(BedNo, Bedwordcount), back)
                                            '急診新進病人90分以上,紫底白字
                                        ElseIf UP90m_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then
                                            BedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:32px;background-color:rgb(145,21,162);color:white; "">{1}{2}{3}</div>", bedfontsize, front, Right(BedNo, Bedwordcount), back)
                                            '判斷是否為急診新進病患,如為變更框架為綠底黑字(白字)
                                        ElseIf ERBed_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then
                                            BedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:32px;background-color:rgb(0,221,0); "">{1}{2}{3}</div>", bedfontsize, front, Right(BedNo, Bedwordcount), back)
                                            '判斷是否佔床,"書記,服務,組長,行政"
                                        ElseIf InBed_List.Contains(Right(BedNo, Bedwordcount)) Or BedNo = "書記" Or BedNo = "服務" Or BedNo = "組長" Or BedNo = "行政" Then
                                            BedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:32px; "">{1}{2}{3}</div>", bedfontsize, front, Right(BedNo, Bedwordcount), back)
                                        ElseIf BedNo = "助理員" Then '助理員時縮小字型
                                            BedList &= String.Format("<div class="" Nurse_ward_3 "">{0}</div>", BedNo)
                                        Else '空床
                                            BedList &= String.Format("<div class="" Nurse_ward_2"" style ="" font-size:{0}px; line-height:32px; "">{1}{2}{3}</div>", bedfontsize, front, Right(BedNo, Bedwordcount), back)
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

                                        '判斷是否要閃爍文字
                                        Dim front As String = Nothing
                                        Dim back As String = Nothing
                                        If cauti_List.Contains(Right(DVTable.Rows(i)("Bedno").ToString, Bedwordcount)) And TestStation.Contains(Wardcode) And Testhost.Contains(Hospcode) Then
                                            front = "<span class=""neon-effect"">"
                                            back = "</span>"
                                        End If


                                        '已完成交班,深藍底白字
                                        If Handover_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then
                                            BedList &= String.Format("<div class="" Nurse_ward4 "" style ="" font-size:{0}px; line-height:25px;height:24.5px;background-color:rgb(0,0,255);color:white; "">{1}{2}{3}</div>", bedfontsize, front, Right(BedNo, Bedwordcount), back)
                                            '急診新進病人30-60分,粉紅底白字
                                        ElseIf During3060m_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then
                                            BedList &= String.Format("<div class="" Nurse_ward4 "" style ="" font-size:{0}px; line-height:25px;height:24.5px;background-color:rgb(255,140,255);color:white; "">{1}{2}{3}</div>", bedfontsize, front, Right(BedNo, Bedwordcount), back)
                                            '急診新進病人60-90分,紅底白字
                                        ElseIf During6090m_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then
                                            BedList &= String.Format("<div class="" Nurse_ward4 "" style ="" font-size:{0}px; line-height:25px;height:24.5px;background-color:rgb(255,0,0);color:white; "">{1}{2}{3}</div>", bedfontsize, front, Right(BedNo, Bedwordcount), back)
                                            '急診新進病人90分以上,紫底白字
                                        ElseIf UP90m_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then
                                            BedList &= String.Format("<div class="" Nurse_ward4 "" style ="" font-size:{0}px; line-height:25px;height:24.5px;background-color:rgb(145,21,162);color:white; "">{1}{2}{3}</div>", bedfontsize, front, Right(BedNo, Bedwordcount), back)
                                            '判斷是否為急診病患,如為變更框架為綠底黑字
                                            '綠底黑字
                                        ElseIf ERBed_List.Contains(Right(BedNo, Bedwordcount)) And InBed_List.Contains(Right(BedNo, Bedwordcount)) Then
                                            BedList &= String.Format("<div class="" Nurse_ward4 "" style ="" font-size:{0}px; line-height:25px;height:24.5px;background-color:rgb(0,221,0); "">{1}{2}{3}</div>", bedfontsize, front, Right(BedNo, Bedwordcount), back)
                                            '判斷是否佔床和書記、服務、組長、行政
                                        ElseIf InBed_List.Contains(Right(BedNo, Bedwordcount)) Or BedNo = "書記" Or BedNo = "服務" Or BedNo = "組長" Or BedNo = "行政" Then
                                            BedList &= String.Format("<div class=""Nurse_ward4"" style ="" font-size:{0}px; line-height:25px; left:0px;top:0px;border-color:#1C42D8;color:#333;font-weight:bold;height:24.5px;padding-top:-3px;text-align:center;border-style:groove;border-width:1px;border-radius:10px;float:left;width:20%;background-color:#eece11;font-family:Arial; "">{1}{2}{3}</div>", bedfontsize, front, Right(BedNo, Bedwordcount), back)
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

                '#####################測試用程式碼--字體閃爍
                'Dim TestBedList As String = "<div class="" Nurse_ward "" style ="" font-size:35px; line-height:32px;background-color:rgb(0,0,255);color:white; "">BBB</div>"
                'TestBedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:32px;background-color:rgb(255,140,255);color:white; ""><span class=""neon-effect"">{1}</span></div>", 35, "BBB")
                'TestBedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:32px;background-color:rgb(255,0,0);color:white; ""><span class=""neon-effect"">{1}</span></div>", 35, "AAA")
                'TestBedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:32px;background-color:rgb(145,21,162);color:white; ""><span class=""neon-effect"">{1}</span></div>", 35, "GGG")
                'TestBedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:{0}px; line-height:32px;background-color:rgb(0,221,0);color:white; ""><span class=""neon-effect"">{1}</span></div>", 35, "333")



                'NurseInfo &= String.Format("<div class=""Nurse"">")
                'NurseInfo &= String.Format("     <div  style=""width:50px;height:95px; float:left;"">")
                'NurseInfo &= String.Format("           <div style=""width:50px; height:20px;""></div>")
                'NurseInfo &= String.Format("          <div style=""width:50px; height:75px; font-size:20px;  line-height: 30px;vertical-align:middle;""  class=Numeric-font >{0}</div>", "X")

                'NurseInfo &= String.Format("     </div>")
                'NurseInfo &= String.Format("     <div  style=""width:150px;height:95px;margin-left:0px; float:left;"">")
                ' DUTY(欄位中有L時, 將NAME顏色改黃色)

                'NurseInfo &= String.Format("          <div style=""width:150px; height:65px; font-size:50px;line-height:45px; color:rgb(239, 255, 0);"">{0}</div>", "Name")

                'NurseInfo &= String.Format("          <div   style=""width:150px; height:30px; font-size:40px;line-height:20px;  TEXT-ALIGN: center;color:#ffd800;"" class=Numeric-font >{0}  </div>", "X")
                'NurseInfo &= String.Format("     </div>")
                'NurseInfo &= String.Format("     <div class = ""btn-group"" style=""width:400px;height:100px;border:3px;"">")
                'NurseInfo &= String.Format("          <div class =""container-fluid"" style=""width:400px;height:100px;padding:0px;text-align:left; "">")
                'NurseInfo &= String.Format("               <div class =""row row_adjust"" style=""width:400px;height:33px;text-align:left;margin-right:0px;margin-left:0px;"">{0}", TestBedList)
                'NurseInfo &= String.Format("               </div>")
                'NurseInfo &= String.Format("          </div>")
                'NurseInfo &= String.Format("     </div>")
                'NurseInfo &= String.Format("</div>")


                '######################


        End Select

        NurseListLabel.Text = NurseInfo
        DS.Dispose()
        Conn.Close()
        Return DS.Tables("NurseList").Rows.Count
    End Function
    '====護士排序方式--以病床排序call by Nurse_List
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




    '############################################主治及住院醫師資料#####################################################
    '目前只有台北主責選IM的護理站顯示白天呈現科別,其它科都顯示目前主治醫師和R
    '護理站白班主治+combie醫師表(合併白天呈現科別的住院及實習醫師班表)
    Public Function BanList_NS_D(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Dr_Upper_limite As Integer, ByVal Bed_Upper_limite As Integer, Now_Time As DateTime)
        ' Response.Write("主治醫師班表")
        'Dr_Upper_limite 醫生顯示資料筆數
        'Bed_Upper_limite 病床顯示資料筆數
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)

        Dim Nowdate As String = Now_Time.ToString("yyyy/MM/dd 00:00:00")
        Dim DR_List As ArrayList = New ArrayList '主治醫師列表
        Dim New_Patient_List As ArrayList = New ArrayList  '今日新進病人名單,全五碼

        '所有病房資料==Bedtbl
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Conn.Open()
        Dim cmd As String = String.Format("SELECT  Bedno, Beddate, beddr, beddr2, beddr3, beddr4, beddr5, beddr6, beddr7, beddr8, beddr9   FROM Bedtbl  WHERE Bedns='{0}' And Bedsts='1' AND Bedroom!='99'", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataSet
        DA.Fill(DS, "BedInfo") '
        '顯示資料表
        Dim Doctor_info As DataTable = New DataTable
        Doctor_info.Columns.Add("AD_EMPNO")
        Doctor_info.Columns.Add("AD_NAME")
        Doctor_info.Columns.Add("HD_EMPNO")
        Doctor_info.Columns.Add("HD_NAME")
        Doctor_info.Columns.Add("INT_EMPNO")
        Doctor_info.Columns.Add("INT_NAME")
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
        Next
        DR_List.Sort()
        '===========================================

        '取出醫師姓名名單@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        Dim Doctor_name_list_cmd As String
        Dim Temp_name As String
        If DR_List.Count > 0 Then
            For u = 0 To (DR_List.Count - 1)
                If (u = DR_List.Count - 1) Then
                    Temp_name &= String.Format("EMPNO ='{0}'", DR_List(u))
                Else
                    Temp_name &= String.Format("EMPNO ='{0}' OR ", DR_List(u))
                End If
            Next
            Doctor_name_list_cmd = String.Format("SELECT * FROM PSN WHERE {0} ORDER BY NAMEC", Temp_name)
            Dim DA2 As OleDbDataAdapter = New OleDbDataAdapter(Doctor_name_list_cmd, Conn)
            DA2.Fill(DS, "Doctor_name_list")
        End If
        'GridView2.DataSource = DS.Tables("Doctor_name_list")
        'GridView2.DataBind()

        '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@





        '建立醫師資料總表****************
        Dim BanList_NS_D_DAY_DT As DataTable = New DataTable
        BanList_NS_D_DAY_DT = BanList_NS_D_Day(Hospcode, Wardcode, 14, 10) '醫師白天呈現班表


        If DS.Tables.Contains("Doctor_name_list") Then
            For i = 0 To DS.Tables("Doctor_name_list").Rows.Count - 1

                Dim BanLIST_VS() As DataRow = BanList_NS_D_DAY_DT.Select(String.Format("VSCODE='{0}'", DS.Tables("Doctor_name_list").Rows(i)("EMPNO").ToString))
                Dim VS_Count As Integer = BanLIST_VS.Count
                Dim BanList_R() As DataRow = BanList_NS_D_DAY_DT.Select(String.Format("VSCODE='{0}' AND (REMARK like '%R%' OR REMARK like '%NP%' OR REMARK like '%PGY%') ", DS.Tables("Doctor_name_list").Rows(i)("EMPNO").ToString))
                Dim R_Count As Integer = BanList_R.Count
                Dim BanLIST_INT() As DataRow = BanList_NS_D_DAY_DT.Select(String.Format("VSCODE='{0}' AND REMARK like '%實習%' ", DS.Tables("Doctor_name_list").Rows(i)("EMPNO").ToString))
                Dim INT_Count As Integer = BanLIST_INT.Count
                Dim MAXCount As Integer = MAXCountC(R_Count, INT_Count)

                '沒有選擇小科別時
                If R_Count = 0 And INT_Count = 0 Then
                    Dim AD_EMPNO As String = DS.Tables("Doctor_name_list").Rows(i)("EMPNO").ToString
                    Dim AD_NAME As String = DS.Tables("Doctor_name_list").Rows(i)("NAMEC").ToString
                    Dim HD_EMPNO As String = ""
                    Dim HD_NAME As String = ""
                    Dim INT_EMPNO As String = ""
                    Dim INT_NAME As String = ""
                    Dim New_row1 As DataRow = Doctor_info.NewRow()
                    New_row1("AD_EMPNO") = AD_EMPNO
                    New_row1("AD_NAME") = AD_NAME
                    New_row1("HD_EMPNO") = HD_EMPNO
                    New_row1("HD_NAME") = HD_NAME
                    New_row1("INT_EMPNO") = INT_EMPNO
                    New_row1("INT_NAME") = INT_NAME
                    Doctor_info.Rows.Add(New_row1)

                End If
                For k = 0 To MAXCount - 1


                    Dim AD_EMPNO As String = ""
                    Dim AD_NAME As String = ""
                    Dim HD_EMPNO As String = ""
                    Dim HD_NAME As String = ""
                    Dim INT_EMPNO As String = ""
                    Dim INT_NAME As String = ""
                    If k = 0 Then
                        AD_EMPNO = BanLIST_VS(k)("vscode").ToString
                        AD_NAME = BanLIST_VS(k)("VSNAME").ToString
                    End If
                    If R_Count > k Then
                        HD_EMPNO = BanList_R(k)("DRCODE").ToString
                        HD_NAME = BanList_R(k)("NAME").ToString
                    End If
                    If INT_Count > k Then
                        INT_EMPNO = BanLIST_INT(k)("DRCODE").ToString
                        INT_NAME = BanLIST_INT(k)("NAME").ToString
                    End If
                    Dim New_row1 As DataRow = Doctor_info.NewRow()
                    New_row1("AD_EMPNO") = AD_EMPNO
                    New_row1("AD_NAME") = AD_NAME
                    New_row1("HD_EMPNO") = HD_EMPNO
                    New_row1("HD_NAME") = HD_NAME
                    New_row1("INT_EMPNO") = INT_EMPNO
                    New_row1("INT_NAME") = INT_NAME
                    Doctor_info.Rows.Add(New_row1)
                Next

            Next
        End If

        '資料輸出$$$$$$$$$$$$$$$$$$$$$$$$$$
        If Doctor_info.Rows.Count > 28 Then
            Show_Doctor_40(Doctor_info, 40)
        Else
            Show_Doctor_28(Doctor_info, 28)
        End If
        'GridView1.DataSource = Doctor_info
        'GridView1.DataSource = DS.Tables("Doctor_name_list")
        'GridView1.DataBind()
        'Response.Write(DR_List(37))
        If (Conn.State = ConnectionState.Open) Then
            Conn.Close()
            Conn.Dispose()
        End If




    End Function
    '白天呈現科別醫師班表WEB SERVICE
    Public Function BanList_NS_D_S(ByVal Hospcode As String, ByVal Wardcode As String, Now_Time As DateTime)
        '畫面輸出資料表
        Dim Doctor_info As DataTable = New DataTable
        Doctor_info.Columns.Add("AD_EMPNO")
        Doctor_info.Columns.Add("AD_NAME")
        Doctor_info.Columns.Add("HD_EMPNO")
        Doctor_info.Columns.Add("HD_NAME")
        Doctor_info.Columns.Add("INT_EMPNO")
        Doctor_info.Columns.Add("INT_NAME")
        Doctor_info.Columns.Add("Bed_Qt", Type.GetType("System.Int16")) '將欄位宣告為Int,排序時才不會出錯

        Dim NS As String = Wardcode
        Dim Nowdate As String = Now_Time.ToString("yyyy/MM/dd")
        If Now_Time.Hour < 8 Then
            Nowdate = Now_Time.AddDays(-1).ToShortDateString
        End If


        Dim ws As ServiceReference5.ws_whiteSoapClient = New ServiceReference5.ws_whiteSoapClient
        Dim BanList As DataSet = New DataSet
        BanList = ws.Get_List(Hospcode, NS, Nowdate)
        'Web Service 回傳值不為空值時才執行以下動作
        If BanList.Tables.Count > 0 Then


            'GridView2.Caption = "白天醫師班表"
            'GridView2.DataSource = BanList.Tables(0)
            'GridView2.DataBind()


            '排班主治醫師陣列
            Dim VS_List As ArrayList = New ArrayList

            For j = 0 To BanList.Tables(0).Rows.Count - 1
                Dim vscode As String = BanList.Tables(0).Rows(j)("VSCODE").ToString
                If Not (vscode = String.Empty) Then
                    If Not (VS_List.Contains(vscode)) Then
                        VS_List.Add(vscode)
                    End If
                End If
            Next


            For i = 0 To VS_List.Count - 1
                Dim VSCODE As String = VS_List(i).ToString

                Dim DT_VS() As DataRow = BanList.Tables(0).Select(String.Format("VSCODE = '{0}'", VSCODE))
                Dim DT_VSCount As Integer = DT_VS.Count
                Dim DT_R() As DataRow = BanList.Tables(0).Select(String.Format("VSCODE = '{0}' AND (REMARK like '%R%' OR REMARK like '%NP%' OR REMARK like '%PGY%')", VSCODE))
                Dim DT_RCount As Integer = DT_R.Count
                Dim DT_INT() As DataRow = BanList.Tables(0).Select(String.Format("VSCODE = '{0}' AND REMARK like '%實習%' ", VSCODE))
                Dim DT_INTCount As Integer = DT_INT.Count
                Dim MAXcount As Integer = MAXCountC(DT_RCount, DT_INTCount)

                For k As Integer = 0 To MAXcount - 1
                    Dim AD_EMPNO As String = ""
                    Dim AD_NAME As String = ""
                    Dim HD_EMPNO As String = ""
                    Dim HD_NAME As String = ""
                    Dim INT_EMPNO As String = ""
                    Dim INT_NAME As String = ""
                    If k = 0 Then
                        AD_EMPNO = DT_VS(k)("VSCODE").ToString
                        AD_NAME = DT_VS(k)("VSNAME").ToString
                    End If
                    If DT_RCount > k Then
                        HD_EMPNO = DT_R(k)("DR1").ToString
                        HD_NAME = Replace(DT_R(k)("DR1NAME").ToString, "x", "")
                    End If
                    If DT_INTCount > k Then
                        INT_EMPNO = DT_INT(k)("DR2").ToString
                        INT_NAME = Replace(DT_INT(k)("DR2NAME").ToString, "x", "")
                    End If
                    Dim New_row1 As DataRow = Doctor_info.NewRow()
                    New_row1("AD_EMPNO") = AD_EMPNO
                    New_row1("AD_NAME") = AD_NAME
                    New_row1("HD_EMPNO") = HD_EMPNO
                    New_row1("HD_NAME") = HD_NAME
                    New_row1("INT_EMPNO") = INT_EMPNO
                    New_row1("INT_NAME") = INT_NAME
                    Doctor_info.Rows.Add(New_row1)
                Next

            Next

            ' GridView1.Caption = "醫師白天值班表-輸出格式"
            ' GridView1.DataSource = Doctor_info
            ' GridView1.DataBind()
            '資料輸出$$$$$$$$$$$$$$$$$$$$$$$$$$
            If Doctor_info.Rows.Count > 28 Then
                Show_Doctor_40(Doctor_info, 40)
            Else
                Show_Doctor_28(Doctor_info, 28)
            End If
        End If

    End Function
    '回傳護理站白班依白天呈現科別醫師班表
    Public Function BanList_NS_D_Day(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Dr_Upper_limite As Integer, ByVal Bed_Upper_limite As Integer) As DataTable
        Dim Dept As String = List_to_SQLin2(Get_Day(Hospcode, Wardcode).Split(","), "a.dept") ' 白天呈現科別 TO SQL IN
        Dim Master As String = List_to_SQLin2(Get_Day(Hospcode, Wardcode).Split(","), "a.Master") ' 白天呈現科別 TO SQL IN

        Dim Nowdate As String = String.Format("{0}/{1}/{2}", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.Day())
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Conn.Open()

        Dim cmd As String = " select a.ns,a.work,a.master,a.dept,c.cname1,a.vscode,d.name as vsname, a.drcode,b.name,a.remark"
        cmd &= " ,DECODE(b.CLASS,'XX',DECODE(SUBSTR(b.CODE,1,1),'Z','INT','PGY'),'XXX','NP',b.class) as CLASS"
        cmd &= " ,to_char(a.schdate,'yyyy/MM/dd') as schdate"
        cmd &= " from dgdrsch a, dr b,dept c, dr d"
        cmd &= " where "
        cmd &= "  a.work='Y' "
        cmd &= String.Format(" and   (a.ns='{0}' OR a.ns = 'XX' )", Wardcode)
        cmd &= String.Format("and  ({0} OR  {1})", Dept, Master)
        cmd &= String.Format("and  a.remark like '%{0}%'", Hospcode)
        'cmd &= String.Format(" and to_char(a.schdate,'yyyy/MM/dd')=to_char(to_date('{0}','yyyy/MM/dd'),'yyyy/MM/dd')", "2016/3/31") '時間測試欄位
        cmd &= String.Format(" and to_char(a.schdate,'yyyy/MM/dd')=to_char(to_date('{0}','yyyy/MM/dd'),'yyyy/MM/dd')", Nowdate)
        cmd &= " and a.drcode=b.code(+)"
        cmd &= " and a.vscode=d.code(+)"
        cmd &= " and a.schdate>=b.bdate and a.schdate<=b.edate"
        cmd &= " and a.dept=c.code(+)"
        cmd &= " ORDER BY  vscode"

        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As DataTable = New DataTable
        DA.Fill(DT)
        'GridView1.Caption = "主治VS 住院醫師白天值班表"
        'GridView1.DataSource = DT
        'GridView1.DataBind()

        Return DT
    End Function

    '取二個數裡的最大值
    Public Function MAXCountC(ByVal X1 As Integer, ByVal X2 As Integer) As Integer
        Dim MAX As Integer = X1
        If X2 > MAX Then
            MAX = X2
        End If

        Return MAX
    End Function

    '====顯示醫師資料====
    Public Sub Show_Doctor_28(ByVal doctor_info_sort As DataTable, ByVal Doctor_Limite As Integer)
        Dim Doctor_Info_show1 As String = Nothing 'Doctor1醫生資料輸出第一列
        Dim Doctor_Info_show2 As String = Nothing 'Doctor2醫生資料輸出第二列
        Dim Doctor_Row_count As Integer = 0
        For w = 0 To doctor_info_sort.Rows.Count - 1
            Dim Doctor_Info_show As String = Nothing '單筆醫生資料輸出

            Dim EMPNO1 As String = Nothing
            Dim NAME1 As String = Nothing
            Dim EMPNO2 As String = Nothing
            Dim NAME2 As String = Nothing
            Dim EMPNO3 As String = Nothing
            Dim NAME3 As String = Nothing
            If doctor_info_sort.Rows.Count > w Then


                EMPNO1 = doctor_info_sort.Rows(w)("AD_EMPNO").ToString
                NAME1 = Adjust_Font_size(doctor_info_sort.Rows(w)("AD_NAME").ToString, 4, 30)
                EMPNO2 = doctor_info_sort.Rows(w)("HD_EMPNO").ToString
                NAME2 = Adjust_Font_size(doctor_info_sort.Rows(w)("HD_NAME").ToString, 4, 30)
                EMPNO3 = doctor_info_sort.Rows(w)("INT_EMPNO").ToString
                NAME3 = Adjust_Font_size(doctor_info_sort.Rows(w)("INT_NAME").ToString, 4, 30)
            End If


            Doctor_Row_count = Doctor_Row_count + 1

            Doctor_Info_show &= String.Format("<div style=""width:375px;height:60px;border-bottom-style:groove;border-width:1px;margin:0px;"">")

            Doctor_Info_show &= String.Format("    <div style=""height:60px;width:125px;float:left;"">")
            Doctor_Info_show &= String.Format("          <div style=""height:40px; width:125px; float:left; font-size:40px; line-height:34px; text-align:center; background-color:#F0A75F;"">{0}</div>", NAME1)
            Doctor_Info_show &= String.Format("          <div style=""height:18px; width:125px; float:left; font-size:30px; line-height:16px; text-align:center; background-color:#F0A75F; font-family:Arial;"">{0}</div>", EMPNO1)
            Doctor_Info_show &= String.Format("    </div>")

            Doctor_Info_show &= String.Format("    <div style=""height:60px;width:125px;float:left;"">")
            Doctor_Info_show &= String.Format("          <div style=""height:40px; width:125px; float:left; font-size:40px; line-height:34px; text-align:center; background-color:#E0FFFF;"">{0}</div>", NAME2)
            Doctor_Info_show &= String.Format("          <div style=""height:18px; width:125px; float:left; font-size:30px; line-height:16px; text-align:center; background-color:#E0FFFF; font-family:Arial;"">{0}</div>", EMPNO2)
            Doctor_Info_show &= String.Format("    </div>")

            Doctor_Info_show &= String.Format("    <div style=""height:60px;width:125px;float:left;"">")
            Doctor_Info_show &= String.Format("          <div style=""height:40px; width:125px; float:left; font-size:40px; line-height:34px; text-align:center; background-color:#BAFDFD;"">{0}</div>", NAME3)
            Doctor_Info_show &= String.Format("          <div style=""height:18px; width:125px; float:left; font-size:30px; line-height:16px; text-align:center; background-color:#BAFDFD; font-family:Arial;"">{0}</div>", EMPNO3)
            Doctor_Info_show &= String.Format("    </div>")

            Doctor_Info_show &= String.Format("</div>")


            If (Doctor_Row_count <= (Doctor_Limite / 2)) Then
                Doctor_Info_show1 &= Doctor_Info_show
            ElseIf (Doctor_Row_count <= Doctor_Limite) Then
                Doctor_Info_show2 &= Doctor_Info_show
            Else
            End If
        Next
        Doctor_Info_Label1.Text = Doctor_Info_show1
        Doctor_Info_Label2.Text = Doctor_Info_show2
    End Sub
    Public Sub Show_Doctor_40(ByVal doctor_info_sort As DataTable, ByVal Doctor_Limite As Integer)
        Dim Doctor_Info_show1 As String = Nothing 'Doctor1醫生資料輸出第一列
        Dim Doctor_Info_show2 As String = Nothing 'Doctor2醫生資料輸出第二列
        Dim Doctor_Row_count As Integer = 0


        For w = 0 To doctor_info_sort.Rows.Count - 1
            Dim Doctor_Info_show As String = Nothing '單筆醫生資料輸出
            Dim EMPNO1 As String = Nothing
            Dim NAME1 As String = Nothing
            Dim EMPNO2 As String = Nothing
            Dim NAME2 As String = Nothing
            Dim EMPNO3 As String = Nothing
            Dim NAME3 As String = Nothing
            If doctor_info_sort.Rows.Count > w Then

                EMPNO1 = doctor_info_sort.Rows(w)("AD_EMPNO").ToString
                NAME1 = Adjust_Font_size(doctor_info_sort.Rows(w)("AD_NAME").ToString, 4, 20)
                EMPNO2 = doctor_info_sort.Rows(w)("HD_EMPNO").ToString
                NAME2 = Adjust_Font_size(doctor_info_sort.Rows(w)("HD_NAME").ToString, 4, 20)
                EMPNO3 = doctor_info_sort.Rows(w)("INT_EMPNO").ToString
                NAME3 = Adjust_Font_size(doctor_info_sort.Rows(w)("INT_NAME").ToString, 4, 20)
            End If


            Doctor_Row_count = Doctor_Row_count + 1

            Doctor_Info_show &= String.Format("<div style=""width:375px;height:42px;border-bottom-style:groove;border-width:1px;margin:0px;"">")

            Doctor_Info_show &= String.Format("    <div style=""height:42px;width:125px;float:left;"">")
            Doctor_Info_show &= String.Format("          <div style=""height:30px; width:125px; float:left; font-size:26px; line-height:24px; text-align:center; background-color:#F0A75F;"">{0}</div>", NAME1)
            Doctor_Info_show &= String.Format("          <div style=""height:10px; width:125px; float:left; font-size:22px; line-height:5px; text-align:center; background-color:#F0A75F; font-family:Arial;"">{0}</div>", EMPNO1)
            Doctor_Info_show &= String.Format("    </div>")

            Doctor_Info_show &= String.Format("    <div style=""height:42px;width:125px;float:left;"">")
            Doctor_Info_show &= String.Format("          <div style=""height:30px; width:125px; float:left; font-size:26px; line-height:24px; text-align:center; background-color:#E0FFFF;"">{0}</div>", NAME2)
            Doctor_Info_show &= String.Format("          <div style=""height:9px; width:125px; float:left; font-size:22px; line-height:5px;  text-align:center; background-color:#E0FFFF; font-family:Arial;"">{0}</div>", EMPNO2)
            Doctor_Info_show &= String.Format("    </div>")

            Doctor_Info_show &= String.Format("    <div style=""height:42px;width:125px;float:left;"">")
            Doctor_Info_show &= String.Format("          <div style=""height:30px; width:125px; float:left; font-size:26px; line-height:24px; text-align:center; background-color:#BAFDFD;"">{0}</div>", NAME3)
            Doctor_Info_show &= String.Format("          <div style=""height:9px; width:125px; float:left; font-size:22px; line-height:5px;  text-align:center; background-color:#BAFDFD; font-family:Arial;"">{0}</div>", EMPNO3)
            Doctor_Info_show &= String.Format("    </div>")

            Doctor_Info_show &= String.Format("</div>")


            If (Doctor_Row_count <= (Doctor_Limite / 2)) Then
                Doctor_Info_show1 &= Doctor_Info_show
            ElseIf (Doctor_Row_count <= Doctor_Limite) Then
                Doctor_Info_show2 &= Doctor_Info_show
            Else
            End If
        Next
        Doctor_Info_Label1.Text = Doctor_Info_show1
        Doctor_Info_Label2.Text = Doctor_Info_show2
    End Sub



    '###################################################下排值班人員資料###################################################
    '總值、值班**白天晚上(共用)
    Public Function OnDutyDoctor(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Now_Time As DateTime)

        Dim Master As String = Get_Master(Hospcode, Wardcode)
        Dim Nowdate As String = Now_Time.ToShortDateString
        If Now_Time.Hour < 8 Then
            Nowdate = Now_Time.AddDays(-1).ToShortDateString
        End If
        Dim Yesdate As String = Now_Time.AddDays(-1).ToShortDateString
        If Now_Time.Hour < 8 Then
            Yesdate = Now_Time.AddDays(-2).ToShortDateString
        End If

        Dim TITLE1 As String = "總值班醫師"
        Dim EMPNO1 As String = Nothing
        Dim NAME1 As String = Nothing
        Dim EMPNO1_1 As String = Nothing
        Dim NAME1_1 As String = Nothing

        Dim TITLE2 As String = "值班<BR>醫師"
        Dim EMPNO2 As String = Nothing
        Dim CLASS2 As String = Nothing
        Dim PSN_CLASS2 As String = Nothing
        Dim NAME2 As String = Nothing
        Dim EMPNO2_1 As String = Nothing
        Dim CLASS2_1 As String = Nothing
        Dim PSN_CLASS2_1 As String = Nothing
        Dim NAME2_1 As String = Nothing

        Dim TITLE3 As String = "昨日<BR>總值"
        Dim EMPNO3 As String = Nothing
        Dim NAME3 As String = Nothing
        Dim EMPNO3_1 As String = Nothing
        Dim NAME3_1 As String = Nothing

        Dim TITLE4 As String = "昨日<BR>值班"
        Dim EMPNO4 As String = Nothing
        Dim CLASS4 As String = Nothing
        Dim NAME4 As String = Nothing
        Dim EMPNO4_1 As String = Nothing
        Dim CLASS4_1 As String = Nothing
        Dim NAME4_1 As String = Nothing


        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        If Hospcode = "2" Then '因淡水資料都放在台北,先固定只撈台北資料庫???????????????????????????????????????
            ConnStr = SELECT_ORACLE("1")
        End If
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        '第一格資料總值班醫師處理

        Dim cmd As String = Nothing

        cmd &= "select a.schdate,a.dept,a.drcode,a.shift,b.location , a.master, c.name,b.hosp_use,a.hospid,b.bdate,b.edate,b.shift"
        cmd &= " from inpdrsch a, drschloc b, dr c"
        cmd &= " where a.shift=b.shift "
        cmd &= "  and a.drcode = c.code "
        cmd &= " and (a.dept = b.dept OR a.master = b.dept)"
        cmd &= " and b.location like '%總值'"
        cmd &= " and  a.sch_type = 'S' "
        cmd &= String.Format(" and b.hosp_use like '%{0}%'", Hospcode)
        cmd &= String.Format(" and trunc ( To_Date('{0}','yyyy/MM/DD')  ) >=  b.bdate", Nowdate)
        cmd &= String.Format(" and  trunc ( To_Date('{0}','yyyy/MM/DD')  ) <= b.Edate", Nowdate)
        cmd &= String.Format(" and a.schdate =     To_Date('{0}','yyyy/MM/DD')", Nowdate)
        'cmd &= " and (a.master = b.dept OR A.dept = B.dept)"
        cmd &= String.Format(" and (a.master = '{0}' OR a.dept = '{0}')", Master)
        cmd &= "ORDER by a.drcode"



        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim Dept_List_DT As DataTable = New DataTable
        DA.Fill(Dept_List_DT)
        '第一格資料寫入變數

        Dim EMPNO_DUTY_Master As String = ""
        If Dept_List_DT.Rows.Count > 0 Then
            If Dept_List_DT.Rows.Count >= 1 Then
                EMPNO1 = Dept_List_DT.Rows(0)("DRCODE").ToString
                NAME1 = Dept_List_DT.Rows(0)("NAME").ToString
            End If
            If Dept_List_DT.Rows.Count >= 2 Then
                EMPNO1_1 = Dept_List_DT.Rows(1)("DRCODE").ToString
                NAME1_1 = Dept_List_DT.Rows(1)("NAME").ToString
            End If

        End If
        'GridView1.DataSource = Dept_List_DT
        'GridView1.DataBind()


        '第二格資料值班醫師處理
        Dim cmd2 As String = Nothing
        cmd2 &= "select A.hospid, B.dept as deptcode  , C.cname1 , A.drcode , B.name , "
        cmd2 &= " E.group1 , E.code as phone ,B.CLASS, DECODE(B.CLASS,'XX',DECODE(SUBSTR(B.CODE,1,1),'Z','INT','PGY'),B.CLASS) as DCLASS ,P.tposit as PSN_CLASS, "
        cmd2 &= " C.master, A.shift , D.location, A.schdate, A.master as Amaster"
        cmd2 &= " from  inpdrsch A , dr B , dept C , drschloc D  , phsdb E, psn_duty P"
        cmd2 &= " where A.drcode = B.code "
        cmd2 &= "  and C.code = B.dept"
        cmd2 &= " and  A.shift = D.shift  and A.sch_type = 'S' "
        cmd2 &= " and E.EMPNO(+)  = A.DRCODE    "
        cmd2 &= " and (a.dept = d.dept OR a.master = d.dept)"
        'cmd2 &= " and B.code (+)= P.empno"
        cmd2 &= String.Format(" and D.hosp_use like '%{0}%'", Hospcode)
        cmd2 &= String.Format(" and (D.location = '{0}' ) ", Wardcode)
        cmd2 &= String.Format(" and (A.master = '{0}' OR A.dept = '{0}' )", Master)
        cmd2 &= String.Format(" and A.schdate = To_Date('{0}','yyyy/MM/DD') ", Nowdate)
        cmd2 &= " and D.bdate <= ( select sysdate from dual  ) "
        cmd2 &= " and D.edate >=  ( select sysdate from dual  ) "
        'cmd2 &= " and B.CLASS <> 'XX'"   '排除INT 或PGY    12/29
        cmd2 &= " and  nvl(D.pry,'0') <> '-99' " 'shift 為BP時不顯示
        'cmd2 &= " and SUBSTR(B.code,1,1) not like 'Z' " '排除INT    12/29
        cmd2 &= String.Format("and A.shift not in(   select  shift from drschloc where location = '總值'   and dept in ('{0}') and bdate <= ( select sysdate from dual  ) and edate >=  ( select sysdate from dual  )     ) ", Master) '排除兒科總值和值班醫師重複的情形
        cmd2 &= " order by C.master"
        Dim DA2 As OleDbDataAdapter = New OleDbDataAdapter(cmd2, Conn)
        Dim Dept_List_DT2 As DataTable = New DataTable
        DA2.Fill(Dept_List_DT2)

        GridView1.DataSource = Dept_List_DT2
        GridView1.DataBind()

        '第二格資料寫入變數
        If Dept_List_DT2.Rows.Count > 0 Then
            If Dept_List_DT2.Rows.Count >= 1 Then
                EMPNO2 = Dept_List_DT2.Rows(0)("DRCODE").ToString
                CLASS2 = Dept_List_DT2.Rows(0)("PSN_CLASS").ToString
                NAME2 = Dept_List_DT2.Rows(0)("NAME").ToString
            End If
            If Dept_List_DT2.Rows.Count >= 2 Then
                EMPNO2_1 = Dept_List_DT2.Rows(1)("DRCODE").ToString
                CLASS2_1 = Dept_List_DT2.Rows(1)("PSN_CLASS").ToString
                NAME2_1 = Dept_List_DT2.Rows(1)("NAME").ToString
            End If
        End If

        '第三格資料昨日總值處理
        Dim cmd3 As String = Nothing

        cmd3 &= "select a.schdate,a.dept,a.drcode,a.shift,b.location , a.master, c.name"
        cmd3 &= " from inpdrsch a, drschloc b, dr c"
        cmd3 &= " where a.shift=b.shift "
        cmd3 &= "  and a.drcode = c.code "
        cmd3 &= " and (a.dept = b.dept OR a.master = b.dept)"
        cmd3 &= " and b.location like '%總值'"
        cmd3 &= " and  a.sch_type = 'S' "
        cmd3 &= String.Format(" and b.hosp_use like '%{0}%'", Hospcode)
        cmd3 &= String.Format(" and trunc ( To_Date('{0}','yyyy/MM/DD')  ) >=  b.bdate", Yesdate)
        cmd3 &= String.Format(" and  trunc ( To_Date('{0}','yyyy/MM/DD')  ) <= b.Edate", Yesdate)
        cmd3 &= String.Format(" and a.schdate =     To_Date('{0}','yyyy/MM/DD')", Yesdate)
        ' cmd3 &= " and (a.master = b.dept OR A.dept = B.dept)"

        cmd3 &= String.Format(" and (a.master = '{0}' OR a.dept = '{0}')", Master)
        cmd3 &= "ORDER by a.drcode"
        Dim DA3 As OleDbDataAdapter = New OleDbDataAdapter(cmd3, Conn)
        Dim Dept_List_DT3 As DataTable = New DataTable
        DA3.Fill(Dept_List_DT3)
        '第三格資料寫入變數
        If Dept_List_DT3.Rows.Count > 0 Then
            If Dept_List_DT3.Rows.Count >= 1 Then
                EMPNO3 = Dept_List_DT3.Rows(0)("DRCODE").ToString
                NAME3 = Dept_List_DT3.Rows(0)("NAME").ToString
            End If
            If Dept_List_DT3.Rows.Count >= 2 Then
                EMPNO3_1 = Dept_List_DT3.Rows(1)("DRCODE").ToString
                NAME3_1 = Dept_List_DT3.Rows(1)("NAME").ToString
            End If
        End If
        ' GridView1.DataSource = Dept_List_DT3
        'GridView1.DataBind()
        '第四格資料昨日值班處理
        Dim cmd4 As String = Nothing
        cmd4 &= "select A.hospid, B.dept as deptcode  , C.cname1 , A.drcode , B.name ,B.class as trueclass, "
        cmd4 &= " E.group1 , E.code as phone ,B.CLASS, DECODE(B.CLASS,'XX',DECODE(SUBSTR(B.CODE,1,1),'Z','INT','PGY'),B.CLASS) as DCLASS, P.tposit as PSN_CLASS, "
        cmd4 &= " C.master, A.shift , D.location, A.schdate, A.master as Amaster"
        cmd4 &= " from  inpdrsch A , dr B , dept C , drschloc D  , phsdb E, psn_duty P"
        cmd4 &= " where A.drcode = B.code and C.code = B.dept"
        cmd4 &= " and  A.shift = D.shift  and A.sch_type = 'S' "
        cmd4 &= " and E.EMPNO(+)  = A.DRCODE    "
        cmd4 &= " and (a.dept = d.dept OR a.master = d.dept)"
        cmd4 &= " and B.code (+)= P.empno"
        'cmd4 &= String.Format("and A.HospID = '{0}'", Hospcode)
        cmd4 &= String.Format(" and D.hosp_use like '%{0}%'", Hospcode)
        cmd4 &= String.Format(" and D.location = '{0}'  ", Wardcode)
        cmd4 &= String.Format(" and (A.master = '{0}' OR A.dept='{0}')", Master)
        cmd4 &= String.Format(" and A.schdate = To_Date('{0}','yyyy/MM/DD') ", Yesdate)
        cmd4 &= " and D.bdate <= ( select sysdate from dual  ) "
        cmd4 &= " and D.edate >=  ( select sysdate from dual  ) "
        'cmd4 &= " and B.CLASS <> 'XX' "   '排除INT 或PGY
        cmd4 &= " and  nvl(D.pry,'0') <> '-99' " 'shift 為BP時不顯示
        cmd4 &= " and SUBSTR(B.code,1,1) not like 'Z' " '排除INT
        cmd4 &= String.Format("and A.shift not in(   select  shift from drschloc where location = '總值'   and dept in ('{0}') and bdate <= ( select sysdate from dual  ) and edate >=  ( select sysdate from dual  )     ) ", Master) '排除兒科總值和值班醫師重複的情形
        cmd4 &= " order by C.master"
        Dim DA4 As OleDbDataAdapter = New OleDbDataAdapter(cmd4, Conn)
        Dim Dept_List_DT4 As DataTable = New DataTable
        DA4.Fill(Dept_List_DT4)
        'GridView1.DataSource = Dept_List_DT4
        'GridView1.DataBind()

        '第四格資料寫入變數
        If Dept_List_DT4.Rows.Count > 0 Then
            If Dept_List_DT4.Rows.Count >= 1 Then
                EMPNO4 = Dept_List_DT4.Rows(0)("DRCODE").ToString
                CLASS4 = Dept_List_DT4.Rows(0)("PSN_CLASS").ToString
                NAME4 = Dept_List_DT4.Rows(0)("NAME").ToString
            End If
            If Dept_List_DT4.Rows.Count >= 2 Then
                EMPNO4_1 = Dept_List_DT4.Rows(1)("DRCODE").ToString
                CLASS4_1 = Dept_List_DT4.Rows(1)("PSN_CLASS").ToString
                NAME4_1 = Dept_List_DT4.Rows(1)("NAME").ToString
            End If
        End If

        '建立值班資料列表
        Dim DUTY_T As DataTable = New DataTable
        DUTY_T.Columns.Add("TITLE1")
        DUTY_T.Columns.Add("EMPNO1")
        DUTY_T.Columns.Add("NAME1")
        DUTY_T.Columns.Add("EMPNO1_1")
        DUTY_T.Columns.Add("NAME1_1")
        DUTY_T.Columns.Add("TITLE2")
        DUTY_T.Columns.Add("EMPNO2")
        DUTY_T.Columns.Add("CLASS2")
        DUTY_T.Columns.Add("NAME2")
        DUTY_T.Columns.Add("EMPNO2_1")
        DUTY_T.Columns.Add("CLASS2_1")
        DUTY_T.Columns.Add("NAME2_1")
        DUTY_T.Columns.Add("TITLE3")
        DUTY_T.Columns.Add("EMPNO3")
        DUTY_T.Columns.Add("NAME3")
        DUTY_T.Columns.Add("EMPNO3_1")
        DUTY_T.Columns.Add("NAME3_1")
        DUTY_T.Columns.Add("TITLE4")
        DUTY_T.Columns.Add("EMPNO4")
        DUTY_T.Columns.Add("CLASS4")
        DUTY_T.Columns.Add("NAME4")
        DUTY_T.Columns.Add("EMPNO4_1")
        DUTY_T.Columns.Add("CLASS4_1")
        DUTY_T.Columns.Add("NAME4_1")

        '將要顯示的資料放入最後顯示TABLE中
        Dim DUTY_R As DataRow = DUTY_T.NewRow
        DUTY_R("TITLE1") = TITLE1
        DUTY_R("EMPNO1") = EMPNO1
        DUTY_R("NAME1") = NAME1
        DUTY_R("EMPNO1_1") = EMPNO1_1
        DUTY_R("NAME1_1") = NAME1_1

        DUTY_R("TITLE2") = TITLE2
        DUTY_R("EMPNO2") = EMPNO2
        DUTY_R("CLASS2") = CLASS2

        DUTY_R("NAME2") = NAME2
        DUTY_R("EMPNO2_1") = EMPNO2_1
        DUTY_R("CLASS2_1") = CLASS2_1

        DUTY_R("NAME2_1") = NAME2_1

        DUTY_R("TITLE3") = TITLE3
        DUTY_R("EMPNO3") = EMPNO3
        DUTY_R("NAME3") = NAME3
        DUTY_R("EMPNO3_1") = EMPNO3_1
        DUTY_R("NAME3_1") = NAME3_1
        DUTY_R("TITLE4") = TITLE4
        DUTY_R("EMPNO4") = EMPNO4
        DUTY_R("CLASS4") = CLASS4
        DUTY_R("NAME4") = NAME4
        DUTY_R("EMPNO4_1") = EMPNO4_1
        DUTY_R("CLASS4_1") = CLASS4_1
        DUTY_R("NAME4_1") = NAME4_1

        DUTY_T.Rows.Add(DUTY_R)


        'GridView2.DataSource = DUTY_T
        'GridView2.DataBind()
        '輸出總值班醫師和值班醫師資料
        Show_OndutyDoctor(DUTY_T, Now_Time) '顯示資料

        If (Conn.State = ConnectionState.Open) Then
            Conn.Close()
            Conn.Dispose()
        End If
    End Function
    '顯示下排值班醫師資料
    Public Sub Show_OndutyDoctor(ByVal OndutyDoctor_T As DataTable, ByVal Now_Time As DateTime)
        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim DTR_LIST As ArrayList = ISBARREC_RECVING(HospCode, WardCode, 1700, 1000, Now_Time)
        Dim DTR_LIST_3 As ArrayList = ISBARREC_RECVING_3(HospCode, WardCode, Now_Time)
        Dim DTR_LIST_2 As ArrayList = ISBARREC_RECVING_2(HospCode, WardCode, Now_Time)
        Dim DTR_LIST_1 As ArrayList = ISBARREC_RECVING_1(HospCode, WardCode, Now_Time)
        'Response.Write(DTR_LIST(0).ToString)
        'Response.Write(DTR_LIST_1(0))
        'Response.Write(DTR_LIST_2(0))
        'Response.Write(DTR_LIST_3(0))
        '醫師的名字前面加上輕中重未交班提示圖示
        Dim EMPNO_SHOW_1 As String = Nothing '第一格顯示資料
        Dim EMPNO_SHOW_2 As String = Nothing '第二格顯示資料
        Dim EMPNO_SHOW_3 As String = Nothing '第三格顯示資料
        Dim EMPNO_SHOW_4 As String = Nothing '第四格顯示資料

        If OndutyDoctor_T.Rows.Count > 0 Then
            '第一格輕中重提示顯示
            Dim TITLE1 As String = OndutyDoctor_T.Rows(0)("TITLE1").ToString
            Dim EMPNO1 As String = OndutyDoctor_T.Rows(0)("EMPNO1").ToString
            Dim NAME1 As String = OndutyDoctor_T.Rows(0)("NAME1").ToString
            Dim EMPNO1_color As String = ""
            '輕度顯示
            If DTR_LIST.Contains(EMPNO1) And DTR_LIST_1.Contains(EMPNO1) And Not String.IsNullOrEmpty(EMPNO1) Then
                EMPNO1_color = "<img src=""Images/Light.png"" style="" height:35px;width:35px;"" />"
            End If
            '中度顯示
            If DTR_LIST.Contains(EMPNO1) And DTR_LIST_2.Contains(EMPNO1) And Not String.IsNullOrEmpty(EMPNO1) Then
                EMPNO1_color = "<img src=""Images/Medium.png"" style="" height:35px;width:35px;"" />"
            End If
            '重度顯示
            If DTR_LIST.Contains(EMPNO1) And DTR_LIST_3.Contains(EMPNO1) And Not String.IsNullOrEmpty(EMPNO1) Then
                EMPNO1_color = "<img src=""Images/Heavy.png"" style="" height:35px;width:35px;"" />"
            End If
            Dim EMPNO1_1 As String = OndutyDoctor_T.Rows(0)("EMPNO1_1").ToString
            Dim NAME1_1 As String = OndutyDoctor_T.Rows(0)("NAME1_1").ToString
            Dim EMPNO1_1_color As String = ""
            '輕度顯示
            If DTR_LIST.Contains(EMPNO1_1) And DTR_LIST_1.Contains(EMPNO1_1) And Not String.IsNullOrEmpty(EMPNO1_1) Then
                EMPNO1_1_color = "<img src=""Images/Light.png"" style="" height:35px;width:35px;"" />"
            End If
            '中度顯示
            If DTR_LIST.Contains(EMPNO1_1) And DTR_LIST_2.Contains(EMPNO1_1) And Not String.IsNullOrEmpty(EMPNO1_1) Then
                EMPNO1_1_color = "<img src=""Images/Medium.png"" style="" height:35px;width:35px;"" />"
            End If
            '重度顯示
            If DTR_LIST.Contains(EMPNO1_1) And DTR_LIST_3.Contains(EMPNO1_1) And Not String.IsNullOrEmpty(EMPNO1_1) Then
                EMPNO1_1_color = "<img src=""Images/Heavy.png"" style="" height:35px;width:35px;"" />"
            End If



            '第二格輕中重提示顯示
            Dim TITLE2 As String = OndutyDoctor_T.Rows(0)("TITLE2").ToString
            Dim EMPNO2 As String = OndutyDoctor_T.Rows(0)("EMPNO2").ToString
            Dim NAME2 As String = OndutyDoctor_T.Rows(0)("NAME2").ToString
            Dim CLASS2 As String = OndutyDoctor_T.Rows(0)("CLASS2").ToString
            Dim CLASS2_1 As String = OndutyDoctor_T.Rows(0)("CLASS2_1").ToString
            Dim EMPNO2_color As String = ""
            '
            '輕度顯示
            If DTR_LIST.Contains(EMPNO2) And DTR_LIST_1.Contains(EMPNO2) And Not String.IsNullOrEmpty(EMPNO2) Then
                EMPNO2_color = "<img src=""Images/Light.png"" style="" height:35px;width:35px;"" />"
            End If
            '中度顯示
            If DTR_LIST.Contains(EMPNO2) And DTR_LIST_2.Contains(EMPNO2) And Not String.IsNullOrEmpty(EMPNO2) Then
                EMPNO2_color = "<img src=""Images/Medium.png"" style="" height:35px;width:35px;"" />"
            End If
            '重度顯示
            If DTR_LIST.Contains(EMPNO2) And DTR_LIST_3.Contains(EMPNO2) And Not String.IsNullOrEmpty(EMPNO2) Then
                EMPNO2_color = "<img src=""Images/Heavy.png"" style="" height:35px;width:35px;"" />"
            End If

            Dim EMPNO2_1 As String = OndutyDoctor_T.Rows(0)("EMPNO2_1").ToString
            Dim NAME2_1 As String = OndutyDoctor_T.Rows(0)("NAME2_1").ToString
            Dim EMPNO2_1_color As String = ""
            '輕度顯示
            If DTR_LIST.Contains(EMPNO2_1) And DTR_LIST_1.Contains(EMPNO2_1) And Not String.IsNullOrEmpty(EMPNO2_1) Then
                EMPNO2_1_color = "<img src=""Images/Light.png"" style="" height:35px;width:35px;"" />"
            End If
            '中度顯示
            If DTR_LIST.Contains(EMPNO2_1) And DTR_LIST_2.Contains(EMPNO2_1) And Not String.IsNullOrEmpty(EMPNO2_1) Then
                EMPNO2_1_color = "<img src=""Images/Medium.png"" style="" height:35px;width:35px;"" />"
            End If
            '重度顯示
            If DTR_LIST.Contains(EMPNO2_1) And DTR_LIST_3.Contains(EMPNO2_1) And Not String.IsNullOrEmpty(EMPNO2_1) Then
                EMPNO2_1_color = "<img src=""Images/Heavy.png"" style="" height:35px;width:35px;"" />"
            End If

            '第二格醫生級職PGY1以藍色顯示
            If CLASS2 = "PGY1" Then
                EMPNO2 = String.Format("<span style = ""color:#0089ff;"">{0}</span>", EMPNO2)
                NAME2 = String.Format("<span style = ""color:#0089ff;"">{0}</span>", NAME2)
            End If
            If CLASS2_1 = "PGY1" Then
                EMPNO2_1 = String.Format("<span style = ""color:#0089ff;"">{0}</span>", EMPNO2_1)
                NAME2_1 = String.Format("<span style = ""color:#0089ff;"">{0}</span>", NAME2_1)
            End If




            '第三格輕中重提示顯示
            Dim TITLE3 As String = OndutyDoctor_T.Rows(0)("TITLE3").ToString
            Dim EMPNO3 As String = OndutyDoctor_T.Rows(0)("EMPNO3").ToString
            Dim NAME3 As String = OndutyDoctor_T.Rows(0)("NAME3").ToString
            Dim EMPNO3_color As String = ""
            '輕度顯示
            If DTR_LIST.Contains(EMPNO3) And DTR_LIST_1.Contains(EMPNO3) And Not String.IsNullOrEmpty(EMPNO3) Then
                EMPNO3_color = "<img src=""Images/Light.png"" style="" height:35px;width:35px;"" />"
            End If
            '中度顯示
            If DTR_LIST.Contains(EMPNO2) And DTR_LIST_2.Contains(EMPNO3) And Not String.IsNullOrEmpty(EMPNO3) Then
                EMPNO3_color = "<img src=""Images/Medium.png"" style="" height:35px;width:35px;"" />"

            End If
            '重度顯示
            If DTR_LIST.Contains(EMPNO2) And DTR_LIST_3.Contains(EMPNO3) And Not String.IsNullOrEmpty(EMPNO3) Then
                EMPNO3_color = "<img src=""Images/Heavy.png"" style="" height:35px;width:35px;"" />"
            End If
            Dim EMPNO3_1 As String = OndutyDoctor_T.Rows(0)("EMPNO3_1").ToString
            Dim NAME3_1 As String = OndutyDoctor_T.Rows(0)("NAME3_1").ToString
            Dim EMPNO3_1_color As String = ""
            '輕度顯示
            If DTR_LIST.Contains(EMPNO3_1) And DTR_LIST_1.Contains(EMPNO3_1) And Not String.IsNullOrEmpty(EMPNO3_1) Then
                EMPNO3_1_color = "<img src=""Images/Light.png"" style="" height:35px;width:35px;"" />"
            End If
            '中度顯示
            If DTR_LIST.Contains(EMPNO3_1) And DTR_LIST_2.Contains(EMPNO3_1) And Not String.IsNullOrEmpty(EMPNO3_1) Then
                EMPNO3_1_color = "<img src=""Images/Medium.png"" style="" height:35px;width:35px;"" />"
            End If
            '重度顯示
            If DTR_LIST.Contains(EMPNO3_1) And DTR_LIST_3.Contains(EMPNO3_1) And Not String.IsNullOrEmpty(EMPNO3_1) Then
                EMPNO3_1_color = "<img src=""Images/Heavy.png"" style="" height:35px;width:35px;"" />"
            End If


            Dim TITLE4 As String = OndutyDoctor_T.Rows(0)("TITLE4").ToString
            Dim EMPNO4 As String = OndutyDoctor_T.Rows(0)("EMPNO4").ToString
            Dim NAME4 As String = OndutyDoctor_T.Rows(0)("NAME4").ToString
            Dim EMPNO4_color As String = ""
            '輕度顯示
            If DTR_LIST.Contains(EMPNO4) And DTR_LIST_1.Contains(EMPNO4) And Not String.IsNullOrEmpty(EMPNO4) Then
                EMPNO4_color = "<img src=""Images/Light.png"" style="" height:35px;width:35px;"" />"
            End If
            '中度顯示
            If DTR_LIST.Contains(EMPNO4) And DTR_LIST_2.Contains(EMPNO4) And Not String.IsNullOrEmpty(EMPNO4) Then
                EMPNO4_color = "<img src=""Images/Medium.png"" style="" height:35px;width:35px;"" />"
                ' Response.Write("test")
            End If
            '重度顯示
            If DTR_LIST.Contains(EMPNO4) And DTR_LIST_3.Contains(EMPNO4) And Not String.IsNullOrEmpty(EMPNO4) Then
                EMPNO4_color = "<img src=""Images/Heavy.png"" style="" height:35px;width:35px;"" />"
            End If

            '第四格輕中重提示顯示
            Dim EMPNO4_1 As String = OndutyDoctor_T.Rows(0)("EMPNO4_1").ToString
            Dim NAME4_1 As String = OndutyDoctor_T.Rows(0)("NAME4_1").ToString
            Dim EMPNO4_1_color As String = ""
            Dim CLASS4 As String = OndutyDoctor_T.Rows(0)("CLASS4").ToString
            Dim CLASS4_1 As String = OndutyDoctor_T.Rows(0)("CLASS4_1").ToString
            '輕度顯示
            If DTR_LIST.Contains(EMPNO4) And DTR_LIST_1.Contains(EMPNO4_1) And Not String.IsNullOrEmpty(EMPNO4_1) Then
                EMPNO4_1_color = "<img src=""Images/Light.png"" style="" height:35px;width:35px;"" />"
            End If
            '中度顯示
            If DTR_LIST.Contains(EMPNO4) And DTR_LIST_2.Contains(EMPNO4_1) And Not String.IsNullOrEmpty(EMPNO4_1) Then
                EMPNO4_1_color = "<img src=""Images/Medium.png"" style="" height:35px;width:35px;"" />"
                ' Response.Write("test")
            End If
            '重度顯示
            If DTR_LIST.Contains(EMPNO4) And DTR_LIST_3.Contains(EMPNO4_1) And Not String.IsNullOrEmpty(EMPNO4_1) Then
                EMPNO4_1_color = "<img src=""Images/Heavy.png"" style="" height:35px;width:35px;"" />"
            End If
            '第四格醫生級職PGY1以藍色顯示
            If CLASS4 = "PGY1" Then
                EMPNO4 = String.Format("<span style = ""color:#0089ff;"">{0}</span>", EMPNO4)
                NAME4 = String.Format("<span style = ""color:#0089ff;"">{0}</span>", NAME4)
            End If
            If CLASS4_1 = "PGY1" Then
                EMPNO4_1 = String.Format("<span style = ""color:#0089ff;"">{0}</span>", EMPNO4_1)
                NAME4_1 = String.Format("<span style = ""color:#0089ff;"">{0}</span>", NAME4_1)
            End If


            '第一格
            '若有二筆資料,將字元縮小
            If Not String.IsNullOrEmpty(EMPNO1) And Not String.IsNullOrEmpty(EMPNO1_1) Then
                EMPNO_SHOW_1 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:26px;font-family:arial;line-height:50px;"">{1}{0}</div>", EMPNO1, EMPNO1_color)
                EMPNO_SHOW_1 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:left; font-size:40px;line-height:50px;"">{0}</div>", NAME1)
                EMPNO_SHOW_1 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:26px;font-family:arial;line-height:50px;"">{1}{0}</div>", EMPNO1_1, EMPNO1_1_color)
                EMPNO_SHOW_1 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:left; font-size:40px;line-height:50px;"">{0}</div>", NAME1_1)
            Else
                EMPNO_SHOW_1 &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:center; line-height:100px; font-size:40px;font-family:arial;"">{1}{0}</div>", EMPNO1, EMPNO1_color)
                EMPNO_SHOW_1 &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:left; line-height:100px; font-size:50px;"">{0}</div>", NAME1)
            End If
            TitleLabel1.Text = TITLE1
            DUTY_InfoLabe1.Text = EMPNO_SHOW_1

            '第二格
            '若有二筆資料,將字元縮小
            If Not String.IsNullOrEmpty(EMPNO2) And Not String.IsNullOrEmpty(EMPNO2_1) Then
                EMPNO_SHOW_2 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:26px;font-family:arial;line-height:50px;"">{1}{0}</div>", EMPNO2, EMPNO2_color)
                EMPNO_SHOW_2 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:left; font-size:40px;line-height:50px;"">{0}</div>", NAME2)
                EMPNO_SHOW_2 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:26px;font-family:arial;line-height:50px;"">{1}{0}</div>", EMPNO2_1, EMPNO2_1_color)
                EMPNO_SHOW_2 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:left; font-size:40px;line-height:50px;"">{0}</div>", NAME2_1)
            Else
                EMPNO_SHOW_2 &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:center; line-height:100px; font-size:40px;font-family:arial;"">{1}{0}</div>", EMPNO2, EMPNO2_color)
                EMPNO_SHOW_2 &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:left; line-height:100px; font-size:50px;"">{0}</div>", NAME2)
            End If
            TitleLabel2.Text = TITLE2
            DUTY_InfoLabe2.Text = EMPNO_SHOW_2

            '第三格
            '若有二筆資料,將字元縮小
            If Not String.IsNullOrEmpty(EMPNO3) And Not String.IsNullOrEmpty(EMPNO3_1) Then
                EMPNO_SHOW_3 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:26px;font-family:arial;line-height:50px;"">{1}{0}</div>", EMPNO3, EMPNO3_color)
                EMPNO_SHOW_3 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:left; font-size:40px;line-height:50px;"">{0}</div>", NAME3)
                EMPNO_SHOW_3 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:26px;font-family:arial;line-height:50px;"">{1}{0}</div>", EMPNO3_1, EMPNO3_1_color)
                EMPNO_SHOW_3 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:left; font-size:40px;line-height:50px;"">{0}</div>", NAME3_1)
            Else
                EMPNO_SHOW_3 &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:center; line-height:100px; font-size:30px;font-family:arial;"">{1}{0}</div>", EMPNO3, EMPNO3_color)
                EMPNO_SHOW_3 &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:left; line-height:100px; font-size:40px"">{0}</div>", NAME3)
            End If
            TitleLabel3.Text = TITLE3
            DUTY_InfoLabe3.Text = EMPNO_SHOW_3

            '第四格
            '若有二筆資料,將字元縮小
            If Not String.IsNullOrEmpty(EMPNO4) And Not String.IsNullOrEmpty(EMPNO4_1) Then
                EMPNO_SHOW_4 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:26px;font-family:arial;line-height:50px;"">{1}{0}</div>", EMPNO4, EMPNO4_color)
                EMPNO_SHOW_4 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:left; font-size:40px;line-height:50px;"">{0}</div>", NAME4)
                EMPNO_SHOW_4 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:26px;font-family:arial;line-height:50px;"">{0}</div>", EMPNO4_1)
                EMPNO_SHOW_4 &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:left; font-size:40px;line-height:50px;"">{0}</div>", NAME4_1)
            Else
                EMPNO_SHOW_4 &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:center; line-height:100px; font-size:30px;font-family:arial;"">{1}{0}</div>", EMPNO4, EMPNO4_color)
                EMPNO_SHOW_4 &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:left; line-height:100px; font-size:40px"">{0}</div>", NAME4)
            End If
            TitleLabel4.Text = TITLE4
            DUTY_InfoLabe4.Text = EMPNO_SHOW_4


        End If

    End Sub


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
                ContactInfo &= String.Format("<div  style=""height:73px; border-radius:15px; border-style:solid; border-width:1px; margin-bottom:1px;"">")
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
        Dim NsSort As String = Request.QueryString("NsSort")
        Dim MSTR As String = Get_Master(HospCode, WardCode)
        'ORACLE系統時間
        Dim Now_Time As DateTime = GET_Now_Time(HospCode)
        Dim key_time As String = Request.QueryString("time")

        If Not String.IsNullOrEmpty(key_time) Then
            Now_Time = DateTime.Parse(key_time)
            Response.Write("測試時間:" & key_time)
        End If
        '主治醫師依所選白天呈現科別呈現的大科別
        Dim DAY_Banlist() As String = {"IM"}
        Dim Dancag As String = Get_Dancag(HospCode, WardCode, 0, Now_Time) '控制護理站名稱和護士班表切換時點

        Dim NSENG = Show_Ward_Name(HospCode, WardCode, Dancag) '顯示護理站簡稱
        Show_Statistics_2(HospCode, WardCode,Now_Time) '畫面上方統計資料
        Nurse_List(HospCode, WardCode, 9, 20, NsSort, Dancag, Now_Time) '列出護士班表資料
        '台北選內科的病房顯示白天呈現科別、其它和新竹顯示目前護理站主治醫師班表
        If DAY_Banlist.Contains(MSTR) And (HospCode = "1" Or HospCode = "2") Then
            BanList_NS_D_S(HospCode, WardCode, Now_Time) '白天呈現科別醫師班表
            'Response.Write("白天呈現科別")
        Else
            BanList_NS_D(HospCode, WardCode, 14, 10, Now_Time) '目前護理站主治醫師班表
            'Response.Write("主治醫師班表")
        End If


        OnDutyDoctor(HospCode, WardCode, Now_Time) ' 顯示醫師值班資料
        Contact_List(HospCode, WardCode, 12) '列出連絡人資料
        Encoding_type("劉瑩", 36) '判斷是否有外字,將外字的字型設定為指定大小後回傳
        URL_Change_Label.Text = String.Format("<a href=""station-N.aspx?Hospcode={0}&Wardcode={1}"" target=""_self"" style =""color:white;"">輕</a><br>", HospCode, WardCode)
        URL_Reset_Label.Text = String.Format("<a href=""station.aspx?Hospcode={0}&Wardcode={1}"" target=""_self"" style =""color:white;"">中</a><br>", HospCode, WardCode)
        '測試

    End Sub

    '********************************************作廢程式碼****************************************************
    '實習醫師資料
    Public Function Intern_List(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Intern_count As Integer)
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim ConnORA As String = SELECT_ORACLE(0)

        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.Day())

        Dim DS As DataSet = New DataSet
        '實習醫生資料
        Dim Intern_List_cmd As String = String.Format("SELECT A.DUTY_DATE,A.NS, A.Intern_Empno, A.Intern_Name,A.Udate, B.group1 FROM WHTINT A, phsdb B  WHERE A.Intern_Empno = B.Empno(+) AND  A.DUTY_DATE='{0}' AND A.NS ='{1}'", Nowdate, Wardcode)
        Dim Intern_List_DA As OleDbDataAdapter = New OleDbDataAdapter(Intern_List_cmd, Conn)
        '實習醫生負責病床資料
        Dim Intern_Bed_cmd As String = String.Format("SELECT B.Bed, B.Intern_Empno  FROM WHTINT A, WHTINTB B WHERE A.NS = B.NS AND A.DUTY_DATE = B.DUTY_DATE AND A.INTERN_EMPNO = B.INTERN_EMPNO AND A.DUTY_DATE='{0}' AND A.NS ='{1}'", Nowdate, Wardcode)
        Dim Intern_Bed_DA As OleDbDataAdapter = New OleDbDataAdapter(Intern_Bed_cmd, Conn)

        Intern_List_DA.Fill(DS, "Intern_List")
        Intern_Bed_DA.Fill(DS, "Intern_Bed")


        'NEW PATIENT 病床ARRAY
        Dim cmd3 As String = String.Format("SELECT  Bedno, Beddate  FROM Bedtbl  WHERE Bedns='{0}' And Bedsts='1' AND Bedroom!='99'", Wardcode)
        Dim DA3 As OleDbDataAdapter = New OleDbDataAdapter(cmd3, Conn)
        Dim DT3 As New DataTable
        Dim New_Patient_List As ArrayList = New ArrayList
        DA3.Fill(DT3)
        For p = 0 To DT3.Rows.Count - 1
            If DT3.Rows(p)(1) = Nowdate Then
                New_Patient_List.Add(Right(DT3.Rows(p)(0), 3))
            End If
        Next
        '目前有佔床的床位號
        Dim cmd4 As String = String.Format("SELECT  Bedno  FROM Bedtbl  WHERE Bedns='{0}' And Bedsts='1' AND Bedroom!='99'", Wardcode)
        Dim DA4 As OleDbDataAdapter = New OleDbDataAdapter(cmd4, Conn)
        Dim InBed_List As ArrayList = New ArrayList
        DA4.Fill(DS, "InBed")
        For l = 0 To DS.Tables("InBed").Rows.Count - 1
            InBed_List.Add(Right(DS.Tables("InBed").Rows(l)("Bedno"), 3))
        Next
        '輸出資料
        Dim Intern_Show As String = Nothing
        For i = 0 To (DS.Tables("Intern_List").Rows.Count - 1)
            If i < Intern_count Then
                'Dim Intern_ID As String = DS.Tables("Intern_List").Rows(i)("Intern_Id")
                Dim Duty_Date As String = DS.Tables("Intern_List").Rows(i)("Duty_date")
                Dim NS As String = DS.Tables("Intern_List").Rows(i)("NS")
                Dim Intern_Empno As String = DS.Tables("Intern_List").Rows(i)("Intern_Empno")
                Dim Intern_Name As String = DS.Tables("Intern_List").Rows(i)("Intern_Name")
                Dim Udate As String = DS.Tables("Intern_List").Rows(i)("Udate")


                Dim Bed_Show As String = Nothing
                '篩選出目前實習醫師所負責的床位
                Dim Bed_row() As DataRow = DS.Tables("Intern_Bed").Select(String.Format("Intern_Empno = '{0}'", Intern_Empno))
                '輸出病床資料
                For j = 0 To Bed_row.GetUpperBound(0)
                    'For j = 0 To (DS.Tables("Intern_Bed").Rows.Count - 1)
                    'Dim Intern_Bed As String = DS.Tables("Intern_Bed").Rows(j)("Bed")
                    Dim Intern_Bed As String = Bed_row(j)(0)
                    '判斷是否為空床
                    If InBed_List.Contains(Right(Intern_Bed, 3)) Then
                        '判斷是否NEW PATIENT
                        If New_Patient_List.Contains(Right(Intern_Bed, 3)) Then
                            Bed_Show &= String.Format("<div class=""test "">{0}</div>", Right(Intern_Bed, 3))
                        Else
                            Bed_Show &= String.Format("<div class=""Nurse_ward"">{0}</div>", Right(Intern_Bed, 3))
                        End If
                    End If
                    '電話號碼的欄位不為空值,則輸出電話號碼
                    If Not IsDBNull(DS.Tables("Intern_List").Rows(i)("group1")) Then
                        Intern_Empno = DS.Tables("Intern_List").Rows(i)("group1")
                    End If
                Next
                Intern_Show &= String.Format("<div class ="" Nurse"">")
                Intern_Show &= String.Format("      <div style =""width:50px; height:75px;float:left;  font-size: 18px;color: #77db88;  line-height: 37px;font-family:arial; "">實習醫師</div>")
                Intern_Show &= String.Format("      <div style =""width:150px;height:75px;margin-left:0px; float:left;"">")
                Intern_Show &= String.Format("              <div style =""width:150px; height:45px; font-size:45px; line-height:45px;color:#77db88;"">{0}</div>", Intern_Name)
                Intern_Show &= String.Format("              <div style =""width:150px; height:30px; font-size:30px; line-height:30px;color:#77db88;font-family:arial;""  >{0}</div>", Intern_Empno)
                Intern_Show &= String.Format("      </div>")
                Intern_Show &= String.Format("      <div class =""bth-group"" style =""width:400px; height:75px; border:3px;float:left; "" >")
                Intern_Show &= String.Format("               <div class =""container-fluid"" style=""width:400px; height:75px; padding:0px; text-align:left;"">")
                Intern_Show &= String.Format("                      <div class =""container-fluid"" style=""width:400px; height:75px; padding:0px; text-align:left;"">")
                Intern_Show &= String.Format("                              <div class =""row row_adjust"" style="" width:400px; height:25px; text-align:left; margin-right:0px; margin-left:0px;"">{0}</div>", Bed_Show)
                Intern_Show &= String.Format("</div></div></div></div>")
            End If
        Next
        InternListLabel.Text = Intern_Show
        'GridView1.DataSource = DS.Tables("intern_list")
        'GridView1.DataBind()
    End Function
    '判斷外字變更字型大小
    Public Function Encoding_type(ByVal str As String, ByVal Font_size As Integer) As String
        'font_size  指定要變更的字型大小
        Dim sb As StringBuilder = New StringBuilder
        Dim big5 As Encoding = Encoding.GetEncoding("utf-8")

        For Each c As Char In str
            Dim cInBig5 As String = big5.GetString(big5.GetBytes(c))
            If (c <> "?" And cInBig5 = "?") Then
                '  Response.Write(cInBig5)
            End If
            '  Response.Write(cInBig5)
        Next
        Dim str2 As String = String.Format("<span style ="" font-size:10px""></span>無")
        Dim str_List() As Char = str.ToCharArray
        For i = 0 To str_List.Count - 1
            If (str_List(i) <> big5.GetString(big5.GetBytes(str_List(i)))) Then
                ' Response.Write("有外字!!")
            End If

            ' Response.Write(str_List(i))
        Next

        'Response.Write(big5.GetString(big5.GetBytes(".....")))

        'Response.Write(str.Length)



    End Function


    '新竹院區下半部總值和值班醫師先拿掉
    Protected Sub Page_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        Dim Hospcode As String = Request.QueryString("Hospcode")
        If Hospcode = "4" Then

            TitleLabel1.Visible = False
            TitleLabel2.Visible = False
            TitleLabel3.Visible = False
            TitleLabel4.Visible = False

        End If
    End Sub

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
End Class
