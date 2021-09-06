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
    '撈取NEW _PATIENT資料
    Public Function Get_NewPatient(ByVal Hospcode As String, ByVal Wardcode As String, ByVal BedIDSE As String) As ArrayList
        Dim NewPatient_List As ArrayList = New ArrayList
        Dim ws As ServiceReference1.RB_WSSoapClient = New ServiceReference1.RB_WSSoapClient()
        Dim kDataSet As New DataSet
        Dim kDataTable As New DataTable
        kDataSet = ws.Get_NSInfo(Hospcode, Wardcode, BedIDSE)

        Return NewPatient_List
    End Function
    '依目前時間取得護理班別
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
    '依星期切換護理版醫師排班表
    '護理站名稱
    Public Function Show_Ward_Name(ByVal Hospcode As String, ByVal Wardcode As String, ByVal DanCag As String) As String
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)


        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM NSTBL  WHERE NS='{0}' ", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataTable
        Dim ContactInfo As String = Nothing
        DA.Fill(DS) '連絡人資料


        If DS.Rows.Count > 0 Then
            Show_Ward_Name_Label.Text = String.Format("<span class = Numeric-font>{0}-{1}</span>", DS.Rows(0)("nseng"), DanCag)
            Return DS.Rows(0)("NSENG").ToString
        End If
    End Function
    '程式版本判斷
    Public Function VersionP(ByVal Hospcode As String, ByVal Wardcode As String) As String
        '開啟EXCEL檔
        Dim SavePath As String = Request.PhysicalApplicationPath & "Upload\"
        Dim FileName As String = String.Format("{0}sNS.xlsx", SavePath)
        Dim ExcelFile As FileStream = File.Open(FileName, FileMode.Open)
        Dim Workbook As XSSFWorkbook = New XSSFWorkbook(ExcelFile)
        ExcelFile.Close() '取得資料後立即釋放,減少檔案被暫用的情形
        ExcelFile.Dispose()

        Dim U_sheet As XSSFSheet = Workbook.GetSheetAt(0)
        Dim HeaderRow As XSSFRow = U_sheet.GetRow(0)

        Dim D_table As DataTable = New DataTable()
        Dim Table_column As DataColumn
        Dim Table_Row As DataRow
        '建置TABLE架構
        Table_column = New DataColumn
        Table_column.DataType = Type.GetType("System.String")
        Table_column.ColumnName = "Hospcode"
        D_table.Columns.Add(Table_column)

        Table_column = New DataColumn
        Table_column.DataType = Type.GetType("System.String")
        Table_column.ColumnName = "Wardcode"
        D_table.Columns.Add(Table_column)
        '逐列處理資料
        For i As Integer = (U_sheet.FirstRowNum + 1) To (U_sheet.LastRowNum)
            Dim Row As XSSFRow = U_sheet.GetRow(i)
            Table_Row = D_table.NewRow
            '逐欄處理資料
            If Row.GetCell(0).ToString = Hospcode And Row.GetCell(1).ToString = Wardcode Then   '判斷第一欄是否為空值
                Table_Row("Hospcode") = Row.GetCell(0).ToString
                Table_Row("Wardcode") = Row.GetCell(1).ToString
                D_table.Rows.Add(Table_Row)
            End If
        Next

        '判斷院區代號和護理站代號是否存在sNS.xlxs中
        Dim D_table_rows() As DataRow = D_table.Select(String.Format("Hospcode='{0}' AND Wardcode='{1}'", Hospcode, Wardcode))
        Dim D_table_rows_count As Integer = D_table_rows.Count
        If D_table_rows_count > 0 Then
            Return "2"
        Else
            Return "1"
        End If

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


    '###############################################值班護士資料########################################################
    Public Function Nurse_List(ByVal Hospcode As String, ByVal Wardcode As String, ByVal NSCount As Integer, ByVal Bed_Count As Integer, ByVal NsSort As String, ByVal Dancag As String) As Integer
        'NsSort 控制護理人員排序的參數,其值為bedno時以床病排序
        'NSCount 控制護理人員的顯示筆數
        'Bed_Count 控制病床的顯示筆數
        Dim IP_PHONE_TB As DataTable = New DataTable
        '撈取ip-phone資料
        IP_PHONE_TB = IP_PHONE(Hospcode, Wardcode)

        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim ConnORA As String = SELECT_ORACLE(0)
        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.Day()) ' 2015/07/13 00:00:00
        Dim Nowtime As String = String.Format("{0}/{1}/{2} {3}:00:00", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.Day(), DateTime.Now.AddHours(0).Hour) '2015/07/13 11:00:00
        'Dim DanCag As String = Nothing '判斷目前時間的班別
        Dim NowHour As Integer = Now.Hour
        Dim addDay As Integer = 5

        '開始時間
        Dim FromDate As String = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.Day())
        '結束時間
        Dim ToDate As String = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.AddDays(1).Year(), DateTime.Today.AddDays(1).Month(), DateTime.Now.AddDays(1).Day)

        If NowHour <= 6 And Dancag = "N" Then
            '夜班時間調回前一天
            FromDate = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.AddDays(-1).Year(), DateTime.Today.AddDays(-1).Month(), DateTime.Now.AddDays(-1).Day())
            ToDate = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.Day())
        End If


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
        '


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
                New_Patient_List.Add(DT3.Rows(p)(0))
            End If
        Next


        '目前有佔床的床位號
        Dim cmd4 As String = String.Format("SELECT  Bedno  FROM Bedtbl  WHERE Bedns='{0}' And Bedsts='1' AND Bedroom!='99'", Wardcode)
        Dim DA4 As OleDbDataAdapter = New OleDbDataAdapter(cmd4, Conn)
        Dim InBed_List As ArrayList = New ArrayList '目前佔床的床位清單
        DA4.Fill(DS, "InBed")
        For l = 0 To DS.Tables("InBed").Rows.Count - 1
            Dim bedno As String = Right(DS.Tables("InBed").Rows(l)("Bedno"), 3)
            If Not InBed_List.Contains(bedno) Then
                InBed_List.Add(bedno)
            End If

        Next

        For index = 0 To (NurseList.Rows.Count - 1)
            If index < NSCount Then
                '處理護士所負責的病床資料==========
                Dim DV As DataView = DS.Tables("BedList").DefaultView
                Dim DVTable As DataTable = Nothing
                Dim BedList As String = Nothing '床位顯示字串
                Dim BedCount As Integer = 0 '非空床床位數

                DV.RowFilter = ""
                DV.RowFilter = String.Format("EMPNO = '{0}'", NurseList.Rows(index)("EMPNO"))
                DV.Sort = "Bedno"
                DVTable = DV.ToTable 'DataView無法直接抓取己處理的資料,須轉為Table,DataView.table內是DataView的原始資料
                For i = 0 To (DV.Count - 1)
                    Dim BedNo As String = DVTable.Rows(i)("Bedno").ToString



                    If Not String.IsNullOrEmpty(DVTable.Rows(i)("Bedno").ToString) Then
                        If Not (BedNo = "書記" Or BedNo = "服務" Or BedNo = "助理員") Then
                            BedCount += 1  '計算非書記、服務的床位數
                        End If

                        If i <= Bed_Count Then '控制所要顯示的病床數
                            If New_Patient_List.Contains(BedNo) Then '判斷是否為NEW PATIENT,如為New Patient變更框架為藍底白字
                                BedList &= String.Format("<div class="" New_nurse_ward"">{0}</div>", Right(BedNo, 3))
                            ElseIf InBed_List.Contains(Right(BedNo, 3)) Or BedNo = "書記" Or BedNo = "服務" Or BedNo = "行政" Then '判斷是否為空床
                                BedList &= String.Format("<div class="" Nurse_ward "">{0}</div>", Right(BedNo, 3))
                            ElseIf BedNo = "助理員" Then

                                BedList &= String.Format("<div class="" Nurse_ward "" style ="" font-size:25px; line-height:28px; "">{0}</div>", BedNo)
                            Else
                                BedList &= String.Format("<div class="" Nurse_ward_2 "">{0}</div>", Right(BedNo, 3))
                            End If
                        End If
                    End If
                Next
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
                '判斷是否有ip  phone 的資料
                If IP_PHONE_TB.Rows.Count > 0 Then
                    If IP_PHONE_TB.Select(String.Format("Cat = '{0}'", Left(Duty, 1))).Count Then
                        Foundrows = IP_PHONE_TB.Select(String.Format("Cat = '{0}'", Left(Duty, 1)))
                        EMPNO = Foundrows(0)("phone")
                    End If
                    If IP_PHONE_TB.Select(String.Format("Cat = '{0}'", Right(Duty, 1))).Count Then
                        Foundrows = IP_PHONE_TB.Select(String.Format("Cat = '{0}'", Right(Duty, 1)))
                        EMPNO = Foundrows(0)("phone")
                    End If
                End If
                NurseInfo &= String.Format("<div class=""Nurse"">")
                NurseInfo &= String.Format("     <div  style=""width:50px;height:95px; float:left;"">")
                NurseInfo &= String.Format("          <div style=""width:40px; height:30px; font-size:20px;  line-height: 20px;color:#ffd800""  class=Numeric-font >{0}</div>", Duty)
                NurseInfo &= String.Format("          <div style=""width:40px; height:30px;"">")
                NurseInfo &= String.Format("                <div style=""width:30px; height:20px; font-size:20px;text-align: center;  line-height: 20px;  color: rgb(0, 253, 22);border-style: groove;  border-width: 2px;  border-radius: 5px;  border-color: rgb(56, 238, 245);"" class=Numeric-font >{0}</div>", BackupCode)
                NurseInfo &= String.Format("          </div>")
                NurseInfo &= String.Format("          <div style=""width:40px; height:35px; font-size:16px;  line-height: 17px;"">{0}</div>", Xfire)
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
                NurseInfo &= String.Format("          <div style=""width:150px; height:30px; font-size:30px;line-height:30px;  TEXT-ALIGN: right;"" class=Numeric-font >{0}  ({1})</div>", EMPNO, BedCount)
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
    '====護士排序方式--以職務排序
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
    '====主治醫師資料
    Public Function BanList(ByVal Hospcode As String, ByVal Wardcode As String)

        Dim ConnStr As String = SELECT_ORACLE(Hospcode)

        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.Day())

        '醫生換班時間為早上八點
        Dim NowHour As Integer = Now.Hour
        If NowHour < 8 Then
            Nowdate = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.AddDays(-1).Day)
        End If

        Dim DR_List As ArrayList = New ArrayList '主治醫師員編列表
        Dim New_Patient_List As ArrayList = New ArrayList  '今日新進病人名單,全五碼

        '所有病房內病床和主治醫師資料==Bedtbl
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Conn.Open()
        Dim cmd As String = String.Format("SELECT  AD_EMPNO   FROM WHTSCHED  WHERE Bdate = '{0}' AND NS ='{1}' ", Nowdate, Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataSet
        DA.Fill(DS, "BedInfo") '
        Conn.Close()

        '取出所有主治醫師員編名單=============================
        For i = 0 To (DS.Tables("BedInfo").Rows.Count - 1)
            'For j = 2 To 10
            ' If Not (DS.Tables("BedInfo").Rows(i)(j).ToString = String.Empty) Then
            'If Not (DR_List.Contains(DS.Tables("BedInfo").Rows(i)(j).ToString)) Then
            DR_List.Add(DS.Tables("BedInfo").Rows(i)(0))
            'End If
            ' End If
            'Next
        Next
        DR_List.Sort()

        '===========================================

        '主治醫師和住院醫師配對資料
        Dim Conn1 As OleDbConnection = New OleDbConnection
        Conn1.ConnectionString = ConnStr
        '下SQL
        Dim cmd1 As String = String.Format("SELECT * FROM WHTSCHED A, WHTSUBT B WHERE A.Hd_Empno = B.Hd_Empno(+) AND A.NS=B.NS(+) AND A.NS='{0}'  AND A.Bdate = to_date('{1}','YYYY/MM/DD HH24:MI:SS') AND B.Bdate(+) =A.Bdate   ", Wardcode, Nowdate)
        Dim DA1 As OleDbDataAdapter = New OleDbDataAdapter(cmd1, Conn1)
        DA1.Fill(DS, "SCHED")
        Conn1.Close()
        '建立最後班表輸出時的TABLE
        Dim Doctor_info As DataTable = New DataTable
        Doctor_info.Columns.Add("AD_EMPNO")
        Doctor_info.Columns.Add("AD_NAME")
        Doctor_info.Columns.Add("HD_EMPNO")
        Doctor_info.Columns.Add("HD_NAME")
        Doctor_info.Columns.Add("Bed_Qt", Type.GetType("System.Int16")) '將欄位宣告為Int,排序時才不會出錯
        Doctor_info.Columns.Add("INT_EMPNO")
        Doctor_info.Columns.Add("INT_NAME")
        Doctor_info.Columns.Add("INT_Phone")

        Doctor_info.Columns.Add("SD_EMPNO")
        Doctor_info.Columns.Add("SD_NAME")
        Doctor_info.Columns.Add("SD_PHONE")

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

            Dim Conn2 As OleDbConnection = New OleDbConnection
            Conn2.ConnectionString = ConnStr
            Conn2.Open()
            Doctor_name_list_cmd = String.Format("SELECT EMPNO,NAMEC FROM PSN_HIS WHERE {0}", Temp_name)
            Dim DA2 As OleDbDataAdapter = New OleDbDataAdapter(Doctor_name_list_cmd, Conn2)
            DA2.Fill(DS, "Doctor_name_list")
            Conn2.Close()
        End If
        '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        '===========================================

        '取主治醫師所負責的病床資料#################
        Dim DV As DataView = New DataView(DS.Tables("BedInfo").DefaultView.ToTable)
        Dim BedCount = DV.ToTable.Rows.Count

        For i = 0 To (DR_List.Count - 1)

            '醫師資料總表****************

            Dim New_row1 As DataRow = Doctor_info.NewRow()
            '取出SCHED Table內指定醫師的主治和任院醫師關聯資料
            Dim SCHED_DV As DataView = New DataView(DS.Tables("SCHED").DefaultView.ToTable)
            SCHED_DV.RowFilter = String.Format("AD_EMPNO = '{0}'", DR_List(i))

            '將資料放入輸出班表中
            Dim AD_NAME() As DataRow = DS.Tables("Doctor_name_list").Select(String.Format("EMPNO = '{0}'", DR_List(i)))
            If AD_NAME.Count > 0 Then
                New_row1("AD_EMPNO") = DR_List(i)
                New_row1("AD_NAME") = AD_NAME(0)("NAMEC").ToString
            End If
            If SCHED_DV.ToTable.Rows.Count > 0 Then
                Dim SCHED_TEMP As DataTable = SCHED_DV.ToTable
                New_row1("HD_EMPNO") = SCHED_TEMP.Rows(0)("HD_EMPNO").ToString
                New_row1("HD_NAME") = SCHED_TEMP.Rows(0)("HD_NAME").ToString
                '修改--須新增資料INT_EMPNO, INT_NAME
                New_row1("INT_EMPNO") = SCHED_TEMP.Rows(0)("INT_EMPNO").ToString
                New_row1("INT_Phone") = SCHED_TEMP.Rows(0)("INT_Phone").ToString
                New_row1("INT_NAME") = SCHED_TEMP.Rows(0)("INT_NAME").ToString
                New_row1("SD_EMPNO") = SCHED_TEMP.Rows(0)("Sd_Empno").ToString
                New_row1("SD_Phone") = SCHED_TEMP.Rows(0)("Sd_Phone").ToString
                New_row1("SD_NAME") = SCHED_TEMP.Rows(0)("Sd_Name").ToString

            End If
            Doctor_info.Rows.Add(New_row1)
            '*****************************
        Next
        '以醫生員編排序
        Dim Doctor_info_sortV As DataView = New DataView(Doctor_info.DefaultView.ToTable)
        Doctor_info_sortV.Sort = "AD_EMPNO"
        Dim Doctor_info_sort As DataTable = Doctor_info_sortV.ToTable
        '###########################################

        '資料輸出$$$$$$$$$$$$$$$$$$$$$$$$$$
        Dim doctor_info_count As Integer = Doctor_info_sort.Rows.Count '醫生資料顯示筆數

        show_doctor(Doctor_info_sort, 28)
        '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        If (Conn.State = ConnectionState.Open) Then
            Conn.Close()
            Conn.Dispose()
        End If
    End Function
    '====顯示主治醫師資料
    Public Sub show_doctor(ByVal doctor_info_sort As DataTable, ByVal Doctor_Limite As Integer)
        Dim Doctor_Info_show1 As String = Nothing 'Doctor1醫生資料輸出第一列
        Dim Doctor_Info_show2 As String = Nothing 'Doctor2醫生資料輸出第二列
        Dim Doctor_Row_count As Integer = 0

        For w = 0 To (doctor_info_sort.Rows.Count - 1)
            Dim Doctor_Info_show As String = Nothing '單筆醫生資料輸出
            Dim EMPNO1 As String = doctor_info_sort.Rows(w)("AD_EMPNO").ToString
            Dim NAME1 As String = Adjust_Font_size(doctor_info_sort.Rows(w)("AD_NAME").ToString, 4, 30)
            Dim EMPNO2 As String = doctor_info_sort.Rows(w)("HD_EMPNO").ToString
            Dim NAME2 As String = Adjust_Font_size(doctor_info_sort.Rows(w)("HD_NAME").ToString, 4, 30)
            '須新增資料
            Dim EMPNO3 As String = doctor_info_sort.Rows(w)("INT_EMPNO").ToString
            Dim Phone3 As String = doctor_info_sort.Rows(w)("INT_Phone").ToString
            Dim NAME3 As String = Adjust_Font_size(doctor_info_sort.Rows(w)("INT_NAME").ToString, 4, 30)

            '判斷是否有輸入實習醫師電話,若有輸入,在員編欄位上顯示
            If Not String.IsNullOrEmpty(Phone3) Then
                EMPNO3 = Phone3
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


    '====顯示值班醫班資料
    Public Sub BanListV2(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Bedcount As Integer, ByVal NSENG As String)
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.Day())
        'Dim cmd As String = String.Format("SELECT DISTINCT   Beddept FROM Bedtbl AL1 WHERE AL1.bedNS='{0}' ORDER BY AL1.Beddept", Wardcode)
        Dim cmd As String = String.Format("SELECT    Nwsdept FROM Nstbl  WHERE  NS='{0}' ", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim Dept_List_DT As DataTable = New DataTable
        Dim Dept_list() As String    'WARDCODE 內所有的科別代號
        DA.Fill(Dept_List_DT)
        Dept_list = Split(Dept_List_DT.Rows(0)("nwsdept").ToString, ",")

        '最後輸出資料表
        Dim Info_Show As DataTable = New DataTable
        Info_Show.Columns.Add("NSdept")
        Info_Show.Columns.Add("deptname")
        Info_Show.Columns.Add("MRD_EMPNO")
        Info_Show.Columns.Add("MRD_NAME")
        Info_Show.Columns.Add("AD_EMPNO")
        Info_Show.Columns.Add("AD_NAME")
        Info_Show.Columns.Add("NSP_EMPNO")
        Info_Show.Columns.Add("NSP_NAME")

        Dim ws As ServiceReference3.ws_psnSoap = New ServiceReference3.ws_psnSoapClient
        Dim Temp() As ServiceReference3.PeopleData
        Temp = ws.GetPeopleData(Nowdate)

        Dim MRD_TB As DataTable = GetPeopleData_to_table(Temp, Hospcode, Wardcode, NSENG)  '將Web Service轉成DATATABLE
        ' GridView1.DataSource = MRD_TB
        ' GridView1.DataBind()

        show_doctor_14(MRD_TB, 14)
        '將資料放入最後輸出資料表
        Conn.Close()


    End Sub
    '====將WebService資料轉成DATABLE
    Public Function GetPeopleData_to_table(ByVal Temp() As ServiceReference3.PeopleData, ByVal Hospcode As String, ByVal Wardcode As String, ByVal NSENG As String) As DataTable

        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM WHTBOARD  WHERE NS='{0}' AND GRP='值班醫師科別' AND EMPNO='duty' ", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As New DataTable
        Dim ContactInfo As String = Nothing
        DA.Fill(DT) '連絡人資料
        'GridView2.DataSource = DT
        ' GridView2.DataBind()


        Dim Dept_List As DataTable = New DataTable
        '建立TABLE的欄位
        Dept_List.Columns.Add("DEPT")
        Dept_List.Columns.Add("CNAME1")
        Dept_List.Columns.Add("DR")
        Dept_List.Columns.Add("DRNAME")
        Dept_List.Columns.Add("SHIFT")
        Dept_List.Columns.Add("SCHDATE")
        Dept_List.Columns.Add("NSENG")
        Dept_List.Columns.Add("HOSPID")
        Dept_List.Columns.Add("GROUP1")
        Dept_List.Columns.Add("PHONE")
        Dept_List.Columns.Add("MASTER")


        Dim DUTY_LIST() As String
        If DT.Rows.Count > 0 Then
            DUTY_LIST = DT.Rows(0)("NAME").ToString.Split(",")
            Dim Dept_List_Rows() As DataRow

            For Each P As ServiceReference3.PeopleData In Temp
                Dim dept_Rows As DataRow = Dept_List.NewRow()
                If DUTY_LIST.Contains(P.DEPT.ToString) Then


                    If Not String.IsNullOrEmpty(P.NSENG) Then
                        dept_Rows("NSENG") = P.NSENG.ToString
                    End If
                    dept_Rows("DEPT") = P.DEPT.ToString
                    dept_Rows("CNAME1") = P.CNAME1.ToString
                    'dept_Rows("DR") = P.DR.ToString
                    dept_Rows("DRNAME") = P.DRNAME.ToString
                    dept_Rows("SHIFT") = P.SHIFT.ToString
                    dept_Rows("SCHDATE") = P.SCHDATE.ToString


                    If Not String.IsNullOrEmpty(P.HOSPID) Then
                        dept_Rows("HOSPID") = P.HOSPID.ToString
                    End If
                    If Not String.IsNullOrEmpty(P.GROUP1) Then
                        dept_Rows("GROUP1") = P.GROUP1.ToString
                    End If
                    If Not String.IsNullOrEmpty(P.PHONE) Then
                        dept_Rows("PHONE") = P.PHONE.ToString
                    End If
                    If Not String.IsNullOrEmpty(P.MASTER) Then
                        dept_Rows("MASTER") = P.MASTER.ToString
                    End If
                    '回傳TABLE資料

                    Dept_List.Rows.Add(dept_Rows)
                End If
            Next
        End If
        Return Dept_List
    End Function
    Public Sub show_doctor_14(ByVal doctor_info_sort As DataTable, ByVal Row_Limite As Integer)



        Dim Doctor_Info_show1 As String = Nothing 'Doctor1醫生資料輸出第一列
        Dim Doctor_Info_show2 As String = Nothing 'Doctor2醫生資料輸出第二列
        Dim Doctor_Row_count As Integer = 0

        For w = 0 To (doctor_info_sort.Rows.Count - 1)
            Dim Doctor_Info_show As String = Nothing '單筆醫生資料輸出
            Dim DRNAME As String = doctor_info_sort.Rows(w)("DRNAME")
            Dim Group1 As String = doctor_info_sort.Rows(w)("Group1")
            Dim CNAME1 As String = doctor_info_sort.Rows(w)("CNAME1")

            Doctor_Row_count = Doctor_Row_count + 1
            Doctor_Info_show &= String.Format("<div style=""width:250px;height:80px;border-bottom-style:groove;border-width:1px;margin:2px;"">")
            '科別
            Doctor_Info_show &= String.Format("    <div style=""height:80px;width:125px;float:left;"">")
            Doctor_Info_show &= String.Format("          <div style=""height:80px; width:125px; float:left; font-size:40px; line-height:80px; text-align:center; background-color:#F0A75F;"">{0}</div>", CNAME1)
            Doctor_Info_show &= String.Format("    </div>")
            '值班醫師姓名、代號
            Doctor_Info_show &= String.Format("    <div style=""height:80px;width:125px;float:left;"">")
            Doctor_Info_show &= String.Format("          <div style=""height:50px; width:125px; float:left; font-size:40px; line-height:50px; text-align:center; background-color:#E0FFFF;"">{0}</div>", DRNAME)
            Doctor_Info_show &= String.Format("          <div style=""height:28px; width:125px; float:left; font-size:30px; line-height:28px; text-align:center; background-color:#E0FFFF; font-family:Arial;"">{0}</div>", Group1)
            Doctor_Info_show &= String.Format("    </div>")


            Doctor_Info_show &= String.Format("</div>")

            If (Doctor_Row_count <= 14) Then
                Doctor_Info_show1 &= Doctor_Info_show
            ElseIf (Doctor_Row_count <= 20) Then
                Doctor_Info_show2 &= Doctor_Info_show
            Else
            End If
        Next
        Doctor_info_Label3.Text = Doctor_Info_show1
        'Doctor_Info_Label2.Text = Doctor_Info_show2

    End Sub







    '####################################################值班醫師資料###################################################
    Public Function OnDutyDoctor(ByVal Hospcode As String, ByVal Wardcode As String)
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim NOWDATE As DateTime = Now
        Dim NowHour As Integer = Now.Hour
        '換班時間為八點,八點前顯示前一日班表
        If NowHour < 8 Then
            NOWDATE = Now.AddDays(-1)
        End If
        Dim NOWDATE_Str As String = Format(NOWDATE, "yyyy/MM/dd 00:00:00")
        Dim DUTY_ID As String = Wardcode.PadLeft(3, "0") & Right(NOWDATE.ToString("yyyy"), 3) & NOWDATE.ToString("MM") & NOWDATE.ToString("dd")

        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Conn.Open()
        Dim cmd As String = String.Format("SELECT * FROM WHTDUTY WHERE NS ='{0}' AND DUTY_DATE = '{1}'", Wardcode, NOWDATE_Str)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As DataTable = New DataTable

        Dim MRD_INFO As String = Nothing '總值班醫師的輸出資料
        Dim RD_INFO As String = Nothing '值班醫師的輸出資料
        Dim Nurse_Pno As String = Nothing '專科護理師的輸出資料
        Dim Nurse_Name As String = Nothing
        DA.Fill(DT)

        '輸出總值班醫師和值班醫師資料
        If (DT.Rows.Count > 0) Then
            '宣告所須要的變數
            Dim MRD_Empno1 As String = DT.Rows(0)("MRD_EMPNO1").ToString
            Dim MRD_NAME1 As String = DT.Rows(0)("MRD_NAME1").ToString
            Dim MRD_Empno2 As String = DT.Rows(0)("MRD_EMPNO2").ToString
            Dim MRD_NAME2 As String = DT.Rows(0)("MRD_NAME2").ToString
            Dim RD_Empno1 As String = DT.Rows(0)("RD_EMPNO1").ToString
            Dim RD_NAME1 As String = DT.Rows(0)("RD_NAME1").ToString
            Dim RD_Empno2 As String = DT.Rows(0)("RD_EMPNO2").ToString
            Dim RD_NAME2 As String = DT.Rows(0)("RD_NAME2").ToString
            Dim INT_Empno As String = DT.Rows(0)("INT_EMPNO").ToString
            Dim INT_NAME As String = DT.Rows(0)("INT_NAME").ToString
            Dim INT_PHONE As String = DT.Rows(0)("INT_PHONE").ToString
            Dim NSP_Empno As String = DT.Rows(0)("NSP_EMPNO").ToString
            Dim NSP_NAME As String = DT.Rows(0)("NSP_NAME").ToString
            Dim NSP_PHONE As String = DT.Rows(0)("NSP_PHONE").ToString

            If Not String.IsNullOrEmpty(NSP_PHONE) Then
                NSP_Empno = NSP_PHONE
            End If
            '判斷值班護理師是否有資料,有則顯示抬頭
            Dim NSP_Title1 As String = Nothing
            Dim NSP_Title2 As String = Nothing
            If Not String.IsNullOrEmpty(NSP_Empno) Then
                NSP_Title1 = "小夜專科"
                NSP_Title2 = "護理師"
            End If
            '判斷值班實習醫師是否有值,有則顯示抬頭
            Dim INT_Title1 As String = Nothing
            Dim INT_Title2 As String = Nothing
            If Not String.IsNullOrEmpty(INT_Empno) Or Not String.IsNullOrEmpty(INT_NAME) Then
                INT_Title1 = "值班實"
                INT_Title2 = "習醫師"

            End If

            '輸出總值班醫師的資料,若有二筆資料,將字元縮小
            If Not String.IsNullOrEmpty(MRD_Empno1) And Not String.IsNullOrEmpty(MRD_Empno2) Then
                '判斷總值班醫師1, 2 是否都有值
                MRD_INFO &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:26px;font-family:arial;line-height:50px;"">{0}</div>", MRD_Empno1)
                MRD_INFO &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:left; font-size:40px;line-height:50px;"">{0}</div>", MRD_NAME1)
                MRD_INFO &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:26px;font-family:arial;line-height:50px;"">{0}</div>", MRD_Empno2)
                MRD_INFO &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:left; font-size:40px;line-height:50px;"">{0}</div>", MRD_NAME2)
            Else
                MRD_INFO &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:center; line-height:100px; font-size:40px;font-family:arial;"">{0}</div>", MRD_Empno1)
                'MRD_INFO &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:center; line-height:100px; font-size:36px;font-family:arial;"">{0}</div>", "7777777")
                MRD_INFO &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:left; line-height:100px; font-size:50px"">{0}</div>", MRD_NAME1)
            End If


            '輸出值班醫師的資料,若有二筆資料,則將字元縮小
            If Not String.IsNullOrEmpty(RD_Empno1) And Not String.IsNullOrEmpty(RD_Empno2) Then
                'R_INFO &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:40px;font-family:arial;line-height:50px;"">{0}</div>", "7777777")
                RD_INFO &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:40px;font-family:arial;line-height:50px;"">{0}</div>", RD_Empno1)
                RD_INFO &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:left; font-size:40px;line-height:50px;"">{0}</div>", RD_NAME1)
                RD_INFO &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:40px;font-family:arial;line-height:50px;"">{0}</div>", RD_Empno2)
                RD_INFO &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:left; font-size:40px;line-height:50px;"">{0}</div>", RD_NAME2)

            Else
                RD_INFO &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:center; line-height:100px; font-size:40px;font-family:arial;"">{0}</div>", RD_Empno1)
                RD_INFO &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:left; line-height:100px; font-size:50px"">{0}</div>", RD_NAME1)
            End If

            R_InfoLabel.Text = RD_INFO
            MRD_ImfoLabe.Text = MRD_INFO
            '值班表實習醫師抬頭
            INTTitleLabel1.Text = INT_Title1
            INTTitleLabel2.Text = INT_Title2
            '值班表實習醫師內容
            INTEmpnolabel.Text = INT_Empno
            INTNAMElabel.Text = INT_NAME

            '值班表專科護理師抬頭
            NSPTitleLabel1.Text = NSP_Title1
            NSPTitleLabel2.Text = NSP_Title2
            '值班表專科護理師內容
            NSPEmpnoLabel.Text = NSP_Empno
            NSPNAMElabel.Text = NSP_NAME
        End If

        If (Conn.State = ConnectionState.Open) Then
            Conn.Close()
            Conn.Dispose()
        End If



    End Function


    '####################################################團隊連絡資料###################################################
    Public Function Contact_List(ByVal Hospcode As String, ByVal Wardcode As String, ByVal ContactCount As Integer)
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM MMH.Whtboard AL1 WHERE AL1.NS='{0}' AND seqno != 'xx'ORDER BY AL1.Seqno", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataSet
        Dim ContactInfo As String = Nothing
        DA.Fill(DS, "ContactList") '連絡人資料
        For index = 0 To (DS.Tables("ContactList").Rows.Count - 1)
            Dim Grp As String = DS.Tables("ContactList").Rows(index)("Grp").ToString
            Dim Name As String = DS.Tables("ContactList").Rows(index)("Name").ToString
            Dim Tel As String = DS.Tables("ContactList").Rows(index)("Tel").ToString
            Dim Strtemp As String = Grp + Name
            Dim Font_size As Integer = 30
            If Len(Strtemp) > 8 Then
                Font_size = 20
            End If
            If index < (ContactCount) Then '只顯示12筆資料
                ContactInfo &= String.Format("<div  style=""height:74px; vertical-align:top; border-radius:15px; border-style:solid; border-width:1px; margin-bottom:1px;"">")
                ContactInfo &= String.Format("       <div style=""height:37px; font-size:{2}px; text-align:center; line-height:37px; font-weight:bold;"">{0}  {1}</div>", Grp, Name, Font_size)
                ContactInfo &= String.Format("       <div style=""height:37px; font-size:35px; text-align:center; line-height:37px; font-weight:bold;font-family:arial;"">{0}</div>", Tel)
                ContactInfo &= String.Format("</div>")
            End If
        Next
        ContactListLabel.Text = ContactInfo
    End Function


    '主程式
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim NsSort As String = Request.QueryString("NsSort")
        Dim Nurse_count As Integer
        Dim Intern_count As Integer
        Dim Dancag As String = Get_Dancag(HospCode, WardCode, 0) '控制護理站名稱和護士班表切換時點
        Dim NowHour As Integer = DateTime.Now.Hour
        Dim Version As String = VersionP(HospCode, WardCode) '以院區代號和護理站代號判別版本傳回版本代號
        Dim NSENG = Show_Ward_Name(HospCode, WardCode, Dancag) '顯示護理站簡稱

        SERVER_TIME() '抓取系統時間

        Nurse_count = Nurse_List(HospCode, WardCode, 9, 15, NsSort, Dancag) '列出護士班表資料
        BanList(HospCode, WardCode) '顯示主治醫師和住院醫師所負責的病床
        BanListV2(HospCode, WardCode, 14, NSENG)
        OnDutyDoctor(HospCode, WardCode) ' 顯示醫師值班資料
        Contact_List(HospCode, WardCode, 12) '顯示連絡人資料


        Encoding_type("劉瑩", 36) '判斷是否有外字,將外字的字型設定為指定大小後回傳
        UpdateTimeLabel.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") '顯示更新時間

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


End Class
