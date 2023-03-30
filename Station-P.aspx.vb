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
    '取得護士班別
    Public Function Get_Dancag(ByVal adjust_m As Integer) As String
        '切換時點的分鐘調整數
        Dim DanCag As String
        Dim NowHour As Integer = Now.AddMinutes(adjust_m).Hour
        Dim NowMinute As Integer = Now.Minute
        If NowHour >= 23 Then
            DanCag = "N"
        ElseIf NowHour <= 6 Then
            DanCag = "N"
        ElseIf NowHour >= 7 And NowHour <= 14 Then
            DanCag = "D"
        Else
            DanCag = "E"
        End If
        Return DanCag
    End Function


    '################################################值班護士資料#############################################
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

        If NowHour <= 6 And DanCag = "N" Then
            '夜班時間調回前一天
            FromDate = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.AddDays(-1).Year(), DateTime.Today.AddDays(-1).Month(), DateTime.Now.AddDays(-1).Day())
            ToDate = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.Day())
        End If


        '值班護士名單資料
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  DISTINCT (AL1.EMPNO), AL1.DEN, AL1.DUTY, AL1.XFIRE, AL1.NAME, AL1.BACKUP1 FROM MMH.NSASSIST AL1 WHERE AL1.NS='{0}' AND AL1.DEN='{1}' AND AL1.FDATE >= to_date('{2}','YYYY/MM/DD HH24:MI:SS') AND AL1.FDATE < to_date('{3}','YYYY/MM/DD HH24:MI:SS') AND AL1.EFFECT='Y' ORDER BY AL1.DUTY", Wardcode, DanCag, FromDate, ToDate)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataSet
        DA.Fill(DS, "NurseList") '護理人員資料
        'GridView1.DataSource = DS.Tables("NurseList")
        'GridView1.DataBind()


        'H班值班護士名單資料
        Dim H_cmd As String = String.Format("SELECT  DISTINCT (AL1.EMPNO), AL1.DEN, AL1.DUTY, AL1.XFIRE, AL1.NAME, AL1.BACKUP1 FROM MMH.NSASSIST AL1 WHERE AL1.NS='{0}' AND AL1.DEN='H' AND AL1.FDATE <=to_date('{2}','YYYY/MM/DD HH24:MI:SS') AND  AL1.TDATE > to_date('{3}','YYYY/MM/DD HH24:MI:SS')  AND AL1.EFFECT='Y'  ORDER BY AL1.DUTY", Wardcode, DanCag, Nowtime, Nowtime)
        Dim H_DA As OleDbDataAdapter = New OleDbDataAdapter(H_cmd, Conn)
        H_DA.Fill(DS, "HNurseList")
        DS.Tables("NurseList").Merge(DS.Tables("HNurseList")) '合併正常班護理人員和H班護理人員的資料



        '病床資料
        Dim cmd2 As String = String.Format("SELECT  AL1.EMPNO, AL1.Bedno, AL1.Pno FROM MMH.NSASSIST AL1 WHERE AL1.NS='{0}' AND AL1.DEN='{1}' AND AL1.FDATE >= to_date('{2}','YYYY/MM/DD HH24:MI:SS') AND AL1.FDATE < to_date('{3}','YYYY/MM/DD HH24:MI:SS') AND AL1.EFFECT='Y'", Wardcode, DanCag, FromDate, ToDate)
        Dim DA2 As OleDbDataAdapter = New OleDbDataAdapter(cmd2, Conn)
        DA2.Fill(DS, "BedList") '病床資料

        'H班病床資料
        Dim H_cmd2 As String = String.Format("SELECT  AL1.EMPNO, AL1.Bedno, AL1.Pno FROM MMH.NSASSIST AL1 WHERE AL1.NS='{0}' AND AL1.DEN='H' AND AL1.FDATE <= to_date('{2}','YYYY/MM/DD HH24:MI:SS') AND AL1.TDATE > to_date('{3}','YYYY/MM/DD HH24:MI:SS') AND AL1.EFFECT='Y'", Wardcode, DanCag, Nowtime, Nowtime)
        Dim H_DA2 As OleDbDataAdapter = New OleDbDataAdapter(H_cmd2, Conn)
        H_DA2.Fill(DS, "H_BedList")

        DS.Tables("BedList").Merge(DS.Tables("H_BedList")) '合併病床和H班病床資料
        '護理人員病床排序,NSSort的參數為'bedno'時將護士排序規則變更為以病床排序

        Dim NurseList As DataTable = New DataTable
        If NsSort = "bedno" Then  '
            NurseList = NsBedSort(DS.Tables("NurseList"), DS.Tables("BedList"), NsSort)
        Else
            NurseList = NsDutySort(DS.Tables("NurseList"), DS.Tables("BedList"))
        End If

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
                    ' If InBed_List.Contains(Right(DVTable.Rows(i)(1).ToString, 3)) Then '判斷是否為空床

                    If Not String.IsNullOrEmpty(DVTable.Rows(i)("Bedno").ToString) Then
                        If Not (DVTable.Rows(i)("Bedno").ToString = "書記" Or DVTable.Rows(i)("Bedno").ToString = "服務") Then
                            BedCount += 1  '計算非書記、服務的床位數
                        End If

                        If i <= Bed_Count Then '控制所要顯示的病床數
                            If New_Patient_List.Contains(DVTable.Rows(i)("Bedno")) Then '判斷是否為NEW PATIENT,如為New Patient變更框架為藍底白字
                                BedList &= String.Format("<div class="" New_nurse_ward"">{0}</div>", Right(DVTable.Rows(i)("Bedno").ToString, 3))
                            Else
                                BedList &= String.Format("<div class="" Nurse_ward "">{0}</div>", Right(DVTable.Rows(i)("Bedno").ToString, 3))
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
                Dim EMPNO As String = NurseList.Rows(index)("Empno").ToString
                Dim Name As String = Adjust_Font_size(NurseList.Rows(index)("Name").ToString, 4, 36)
                Dim Duty As String = NurseList.Rows(index)("Duty").ToString
                Dim Xfire As String = NurseList.Rows(index)("Xfire").ToString
                Dim Den As String = NurseList.Rows(index)("DEN").ToString

                Dim Foundrows() As DataRow
                '判斷是否有ip  phone 的資料
                If IP_PHONE_TB.Rows.Count > 0 Then
                    If IP_PHONE_TB.Select(String.Format("Cat = '{0}'", Left(Duty, 1))).Count Then
                        Foundrows = IP_PHONE_TB.Select(String.Format("Cat = '{0}'", Left(Duty, 1)))
                        EMPNO = Foundrows(0)("phone").ToString
                    End If
                    If IP_PHONE_TB.Select(String.Format("Cat = '{0}'", Right(Duty, 1))).Count Then
                        Foundrows = IP_PHONE_TB.Select(String.Format("Cat = '{0}'", Right(Duty, 1)))
                        EMPNO = Foundrows(0)("phone").ToString
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
    '################################################主治及住院醫師資料############################################
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

        '主治醫師資料==Bedtbl
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
        GridView1.DataSource = DS.Tables("SCHED")
        GridView1.DataBind()
        Conn1.Close()
        '建立最後班表輸出時的TABLE
        Dim Doctor_info As DataTable = New DataTable
        '主治醫師資料
        Doctor_info.Columns.Add("AD_EMPNO")
        Doctor_info.Columns.Add("AD_NAME")
        '住院醫師資料
        Doctor_info.Columns.Add("HD_EMPNO")
        Doctor_info.Columns.Add("HD_NAME")
        Doctor_info.Columns.Add("Bed_Qt", Type.GetType("System.Int16")) '將欄位宣告為Int,排序時才不會出錯
        '實習醫師資料
        Doctor_info.Columns.Add("INT_EMPNO")
        Doctor_info.Columns.Add("INT_NAME")
        Doctor_info.Columns.Add("INT_Phone")
        '代班醫師資料
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

            show_doctor_26(Doctor_info_sort)





        '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        If (Conn.State = ConnectionState.Open) Then
            Conn.Close()
            Conn.Dispose()
        End If


    End Function
    '====顯示醫師資料
    Public Sub show_doctor_26(ByVal doctor_info_sort As DataTable)
        Dim Doctor_Info_show1 As String = Nothing 'Doctor1醫生資料輸出第一列
        Dim Doctor_Info_show2 As String = Nothing 'Doctor2醫生資料輸出第二列
        Dim Doctor_Row_count As Integer = 0

        For w = 0 To (doctor_info_sort.Rows.Count - 1)
            Dim Doctor_Info_show As String = Nothing '單筆醫生資料輸出
            Dim AD_EMPNO As String = doctor_info_sort.Rows(w)("AD_EMPNO").ToString
            Dim AD_NAME As String = Adjust_Font_size(doctor_info_sort.Rows(w)("AD_NAME").ToString, 4, 30)
            Dim HD_EMPNO As String = doctor_info_sort.Rows(w)("HD_EMPNO").ToString
            Dim HD_NAME As String = Adjust_Font_size(doctor_info_sort.Rows(w)("HD_NAME").ToString, 4, 30)
            '須新增資料
            Dim INT_EMPNO As String = doctor_info_sort.Rows(w)("INT_EMPNO").ToString
            Dim INT_Phone As String = doctor_info_sort.Rows(w)("INT_Phone").ToString
            Dim INT_NAME As String = Adjust_Font_size(doctor_info_sort.Rows(w)("INT_NAME").ToString, 4, 30)
            Dim SD_EMPNO As String = doctor_info_sort.Rows(w)("SD_EMPNO").ToString
            Dim SD_Phone As String = doctor_info_sort.Rows(w)("SD_Phone").ToString
            Dim SD_NAME As String = Adjust_Font_size(doctor_info_sort.Rows(w)("SD_NAME").ToString, 4, 30)
            '判斷是否有輸入實習醫師電話,若有輸入,在員編欄位上顯示
            If Not String.IsNullOrEmpty(INT_Phone) Then
                INT_EMPNO = INT_Phone
            End If
            '判斷是否有輸入代班醫師電話,若有輸入,在員編欄位上顯示
            If Not String.IsNullOrEmpty(SD_Phone) Then
                SD_EMPNO = SD_Phone
            End If


            Doctor_Row_count = Doctor_Row_count + 1

            Doctor_Info_show &= String.Format("<div style=""width:500px;height:60px;border-bottom-style:groove;border-width:1px;margin:0px;"">")

            Doctor_Info_show &= String.Format("    <div style=""height:60px;width:125px;float:left;"">")
            Doctor_Info_show &= String.Format("          <div style=""height:40px; width:125px; float:left; font-size:40px; line-height:40px; text-align:center; background-color:#F0A75F;"">{0}</div>", AD_NAME)
            Doctor_Info_show &= String.Format("          <div style=""height:18px; width:125px; float:left; font-size:30px; line-height:20px; text-align:center; background-color:#F0A75F; font-family:Arial;"">{0}</div>", AD_EMPNO)
            Doctor_Info_show &= String.Format("    </div>")

            Doctor_Info_show &= String.Format("    <div style=""height:60px;width:125px;float:left;"">")
            Doctor_Info_show &= String.Format("          <div style=""height:40px; width:125px; float:left; font-size:40px; line-height:40px; text-align:center; background-color:#E0FFFF;"">{0}</div>", HD_NAME)
            Doctor_Info_show &= String.Format("          <div style=""height:18px; width:125px; float:left; font-size:30px; line-height:20px; text-align:center; background-color:#E0FFFF; font-family:Arial;"">{0}</div>", HD_EMPNO)
            Doctor_Info_show &= String.Format("    </div>")

            Doctor_Info_show &= String.Format("    <div style=""height:60px;width:125px;float:left;"">")
            Doctor_Info_show &= String.Format("          <div style=""height:40px; width:125px; float:left; font-size:40px; line-height:40px; text-align:center; background-color:#BAFDFD;"">{0}</div>", INT_NAME)
            Doctor_Info_show &= String.Format("          <div style=""height:18px; width:125px; float:left; font-size:30px; line-height:20px; text-align:center; background-color:#BAFDFD; font-family:Arial;"">{0}</div>", INT_EMPNO)
            Doctor_Info_show &= String.Format("    </div>")

            Doctor_Info_show &= String.Format("    <div style=""height:60px;width:125px;float:left;"">")
            Doctor_Info_show &= String.Format("          <div style=""height:40px; width:125px; float:left; font-size:40px; line-height:40px; text-align:center; background-color:#87FDFD;"">{0}</div>", SD_NAME)
            Doctor_Info_show &= String.Format("          <div style=""height:18px; width:125px; float:left; font-size:30px; line-height:20px; text-align:center; background-color:#87FDFD; font-family:Arial;"">{0}</div>", SD_EMPNO)
            Doctor_Info_show &= String.Format("    </div>")

            Doctor_Info_show &= String.Format("</div>")


            If (Doctor_Row_count <= 14) Then
                Doctor_Info_show1 &= Doctor_Info_show
            ElseIf (Doctor_Row_count <= 28) Then
                Doctor_Info_show2 &= Doctor_Info_show
            Else
            End If
        Next
        Doctor_Info_Label1.Text = Doctor_Info_show1
        Doctor_Info_Label2.Text = Doctor_Info_show2
    End Sub


    

    '#################################################下排值班醫師資料#################################################3
    Public Function OnDutyDoctor(ByVal Hospcode As String, ByVal Wardcode As String)

        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim NOWDATE As DateTime = Now
        Dim NowHour As Integer = Now.Hour
        '換班時間為八點,八點前顯示前一日班表
        If NowHour < 8 Then
            NOWDATE = Now.AddDays(-1)
        End If
        Dim NOWDATE_Str As String = Format(NOWDATE, "yyyy/MM/dd 00:00:00")
        Dim YESDATE_Str As String = Format(NOWDATE.AddDays(-1), "yyyy/MM/dd 00:00:00")
        Dim DUTY_ID As String = Wardcode.PadLeft(3, "0") & Right(NOWDATE.ToString("yyyy"), 3) & NOWDATE.ToString("MM") & NOWDATE.ToString("dd")

        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Conn.Open()
        '當日總值
        Dim cmd As String = String.Format("SELECT * FROM WHTDUTY WHERE NS ='{0}' AND DUTY_DATE = '{1}'", Wardcode, NOWDATE_Str)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As DataTable = New DataTable
        '前日總值
        Dim cmd3 As String = String.Format("SELECT * FROM WHTDUTY WHERE NS ='{0}' AND DUTY_DATE = '{1}'", Wardcode, YESDATE_Str)
        Dim DA3 As OleDbDataAdapter = New OleDbDataAdapter(cmd3, Conn)
        Dim DT3 As DataTable = New DataTable



        Dim MRD_INFO As String = Nothing '總值班醫師的輸出資料
        Dim RD_INFO As String = Nothing '值班醫師的輸出資料
        Dim Nurse_Pno As String = Nothing '專科護理師的輸出資料
        Dim LAST_RD_INFO As String = Nothing '昨日值班醫師的輸出資料
        Dim Nurse_Name As String = Nothing
        DA.Fill(DT)
        DA3.Fill(DT3)
        'GridView1.Caption = "兒科上傳昨日值班醫師資料"
        'GridView1.DataSource = DT3
        'GridView1.DataBind()

        '###################################################################
        Dim ConnStr1 As String = SELECT_ORACLE("1") '因淡水資料都放在台北,先固定只撈台北資料庫???????????????????????????????????????
        Dim Conn1 As OleDbConnection = New OleDbConnection
        Conn1.ConnectionString = ConnStr1
        Dim Master As String = Get_Master(Hospcode, Wardcode)
        Dim Nowdate1 As String = String.Format("{0}/{1}/{2}", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.Day())
        If Now.Hour < 8 Then
            Nowdate1 = String.Format("{0}/{1}/{2}", DateTime.Today.AddDays(-1).Year(), DateTime.Today.AddDays(-1).Month(), DateTime.Today.AddDays(-1).Day())
        End If
        '前一日日期
        Dim Nowdate2 As String = String.Format("{0}/{1}/{2}", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.AddDays(-1).Day())
        If Now.Hour < 8 Then
            Nowdate1 = String.Format("{0}/{1}/{2}", DateTime.Today.Year(), DateTime.Today.AddDays(-1).Month(), DateTime.Today.AddDays(-1).Day())
        End If



        '第一格資料處理(停用)

        Dim cmd1 As String = Nothing

        cmd1 &= "select a.schdate,a.dept,a.drcode,a.shift,b.location , a.master, c.name"
        cmd1 &= " from inpdrsch a, drschloc b, dr c"
        cmd1 &= " where a.shift=b.shift "
        cmd1 &= "  and a.drcode = c.code "
        cmd1 &= " and b.location='總值'"
        cmd1 &= String.Format(" and a.hospid='{0}'", Hospcode)
        cmd1 &= String.Format(" and trunc ( To_Date('{0}','yyyy/MM/DD')  ) >=  b.bdate", Nowdate1)
        cmd1 &= String.Format(" and  trunc ( To_Date('{0}','yyyy/MM/DD')  ) <= b.Edate", Nowdate1)
        cmd1 &= String.Format(" and a.schdate =     To_Date('{0}','yyyy/MM/DD')", Nowdate1)
        cmd1 &= " and a.master = b.dept "
        'cmd1 &= String.Format(" and a.master = '{0}'", Master)
        cmd1 &= String.Format(" and a.master = '{0}'", "PD") '==========================test
        cmd1 &= "ORDER by a.drcode"



        Dim DA1 As OleDbDataAdapter = New OleDbDataAdapter(cmd1, Conn1)
        Dim Dept_List_DT1 As DataTable = New DataTable
        DA1.Fill(Dept_List_DT1)
        'GridView1.Caption = "總值班醫師"
        'GridView1.DataSource = Dept_List_DT1
        'GridView1.DataBind()





        '第二格資料處理 值班醫師(停用)
        Dim cmd2 As String = Nothing
        cmd2 &= "select A.hospid, B.dept as deptcode  , C.cname1 , A.drcode , B.name ,"
        cmd2 &= " E.group1 , E.code as phone , DECODE(B.CLASS,'XX',DECODE(SUBSTR(B.CODE,1,1),'Z','INT','PGY'),B.CLASS) as CLASS , "
        cmd2 &= " C.master, A.shift , D.location, A.schdate, A.master as Amaster"
        cmd2 &= " from  inpdrsch A , dr B , dept C , drschloc D  , phsdb E"
        cmd2 &= " where A.drcode = B.code and C.code = B.dept"
        cmd2 &= " and  A.shift = D.shift  and A.sch_type = 'S' "
        cmd2 &= " and E.EMPNO  = A.DRCODE (+)   "
        cmd2 &= String.Format(" and (D.location = '{0}' ) and D.dept = A.master ", Wardcode)
        'cmd2 &= String.Format(" and A.master = '{0}'", Master)
        cmd2 &= String.Format(" and A.master = '{0}'", "PD") '===================================test
        cmd2 &= String.Format(" and A.schdate = To_Date('{0}','yyyy/MM/DD') ", Nowdate1)
        cmd2 &= " and D.bdate <= ( select sysdate from dual  ) "
        cmd2 &= " and D.edate >=  ( select sysdate from dual  ) "
        'cmd2 &= " and A.shift = '13'"
        'cmd2 &= " and B.CLASS <> 'XX'"   '排除INT 或PGY
        cmd2 &= " order by C.master"
        Dim DA2 As OleDbDataAdapter = New OleDbDataAdapter(cmd2, Conn1)
        Dim Dept_List_DT2 As DataTable = New DataTable
        ' DA2.Fill(Dept_List_DT2)
        'GridView2.Caption = "值班醫師資料"
        'GridView2.DataSource = Dept_List_DT2
        'GridView2.DataBind()


        '第四格資料處理 已更改程式....此段程式碼不再使用++++++++++++++++
        Dim cmd4 As String = Nothing
        cmd4 &= "select A.hospid, B.dept as deptcode  , C.cname1 , A.drcode , B.name ,"
        cmd4 &= " E.group1 , E.code as phone , DECODE(B.CLASS,'XX',DECODE(SUBSTR(B.CODE,1,1),'Z','INT','PGY'),B.CLASS) as CLASS , "
        cmd4 &= " C.master, A.shift , D.location, A.schdate, A.master as Amaster"
        cmd4 &= " from  inpdrsch A , dr B , dept C , drschloc D  , phsdb E"
        cmd4 &= " where A.drcode = B.code and C.code = B.dept"
        cmd4 &= " and  A.shift = D.shift  and A.sch_type = 'S' "
        cmd4 &= " and E.EMPNO  = A.DRCODE (+)   "
        cmd4 &= String.Format(" and (D.location = '{0}' ) and D.dept = A.master ", Wardcode)
        'cmd4 &= String.Format(" and A.master = '{0}'", Master)
        cmd4 &= String.Format(" and A.master = '{0}'", "PD") '===================================test
        cmd4 &= String.Format(" and A.schdate = To_Date('{0}','yyyy/MM/DD') ", Nowdate2)
        cmd4 &= " and D.bdate <= ( select sysdate from dual  ) "
        cmd4 &= " and D.edate >=  ( select sysdate from dual  ) "
        'cmd4 &= " and A.shift = '13'"
        'cmd4 &= " and B.CLASS <> 'XX'"   '排除INT 或PGY
        cmd4 &= " order by C.master"
        Dim DA4 As OleDbDataAdapter = New OleDbDataAdapter(cmd4, Conn1)
        Dim Dept_List_DT4 As DataTable = New DataTable
        ' DA4.Fill(Dept_List_DT4)
        '###################################################################
        'GridView3.Caption = "前日值班醫師資料"
        'GridView3.DataSource = Dept_List_DT4
        'GridView3.DataBind()
        '++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++



        '輸出總值班醫師和值班醫師資料
        If (DT.Rows.Count > 0) Then
            '宣告所須要的變數
            Dim MRD_Empno1 As String = Nothing
            Dim MRD_NAME1 As String = Nothing
            Dim MRD_Empno2 As String = Nothing
            Dim MRD_NAME2 As String = Nothing

            Dim RD_Empno1 As String = Nothing
            Dim RD_NAME1 As String = Nothing
            Dim RD_Empno2 As String = Nothing
            Dim RD_NAME2 As String = Nothing
            Dim INT_Empno As String = Nothing
            Dim INT_NAME As String = Nothing
            Dim INT_PHONE As String = Nothing
            Dim NSP_Empno As String = Nothing
            Dim NSP_NAME As String = Nothing
            Dim NSP_PHONE As String = Nothing
            Dim LAST_RD_Empno1 As String = Nothing
            Dim LAST_RD_NAME1 As String = Nothing
            Dim LAST_RD_Empno2 As String = Nothing
            Dim LAST_RD_NAME2 As String = Nothing

            '總值班醫師資料
            If Dept_List_DT1.Rows.Count >= 1 Then
                MRD_Empno1 = Dept_List_DT1.Rows(0)("DRCODE")
                MRD_NAME1 = Dept_List_DT1.Rows(0)("NAME")
            End If
            If Dept_List_DT1.Rows.Count >= 2 Then
                MRD_Empno2 = Dept_List_DT1.Rows(1)("DRCODE")
                MRD_NAME2 = Dept_List_DT1.Rows(1)("NAME")
            End If
           
            '值班醫師資料
            If Dept_List_DT2.Rows.Count >= 1 Then
                RD_Empno1 = Dept_List_DT2.Rows(0)("DRCODE")
                RD_NAME1 = Dept_List_DT2.Rows(0)("NAME")
            End If
            If Dept_List_DT2.Rows.Count >= 2 Then
                RD_Empno2 = Dept_List_DT2.Rows(1)("DRCODE")
                RD_NAME2 = Dept_List_DT2.Rows(1)("NAME")
            End If


            MRD_Empno1 = DT.Rows(0)("MRD_EMPNO1").ToString
            MRD_NAME1 = DT.Rows(0)("MRD_NAME1").ToString
            MRD_Empno2 = DT.Rows(0)("MRD_EMPNO2").ToString
            MRD_NAME2 = DT.Rows(0)("MRD_NAME2").ToString
            RD_Empno1 = DT.Rows(0)("RD_EMPNO1").ToString
            RD_NAME1 = DT.Rows(0)("RD_NAME1").ToString
            RD_Empno2 = DT.Rows(0)("RD_EMPNO2").ToString
            RD_NAME2 = DT.Rows(0)("RD_NAME2").ToString
            INT_Empno = DT.Rows(0)("INT_EMPNO").ToString
            INT_NAME = DT.Rows(0)("INT_NAME").ToString
            INT_PHONE = DT.Rows(0)("INT_PHONE").ToString
            NSP_Empno = DT.Rows(0)("NSP_EMPNO").ToString
            NSP_NAME = DT.Rows(0)("NSP_NAME").ToString
            NSP_PHONE = DT.Rows(0)("NSP_PHONE").ToString
            If DT3.Rows.Count > 0 Then
                LAST_RD_Empno1 = DT3.Rows(0)("RD_EMPNO1").ToString
                LAST_RD_NAME1 = DT3.Rows(0)("RD_NAME1").ToString
                LAST_RD_Empno2 = DT3.Rows(0)("RD_EMPNO2").ToString
                LAST_RD_NAME2 = DT3.Rows(0)("RD_NAME2").ToString
            End If

            If Not String.IsNullOrEmpty(NSP_PHONE) Then
                NSP_Empno = NSP_PHONE
            End If
            '判斷昨日值班醫師是否有資料,有則顯示抬頭
            Dim NSP_Title1 As String = ""
            Dim NSP_Title2 As String = ""
            If Not String.IsNullOrEmpty(LAST_RD_Empno1) Then
                NSP_Title1 = "昨日"
                NSP_Title2 = "值班醫師"
            End If
            '判斷值班實習醫師是否有值,有則顯示抬頭
            Dim INT_Title1 As String = Nothing
            Dim INT_Title2 As String = Nothing
            If Not String.IsNullOrEmpty(INT_Empno) Or Not String.IsNullOrEmpty(INT_NAME) Then
                INT_Title1 = "值班實習"
                INT_Title2 = "醫學生"
            End If
            If Not String.IsNullOrEmpty(INT_PHONE) Then
                INT_Empno = INT_PHONE
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

            '輸出昨日值班醫師的資料,若有二筆資料,則將字元縮小
            If Not String.IsNullOrEmpty(LAST_RD_Empno1) And Not String.IsNullOrEmpty(LAST_RD_Empno2) Then
                'R_INFO &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:40px;font-family:arial;line-height:50px;"">{0}</div>", "7777777")
                LAST_RD_INFO &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:40px;font-family:arial;line-height:50px;"">{0}</div>", LAST_RD_Empno1)
                LAST_RD_INFO &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:left; font-size:40px;line-height:50px;"">{0}</div>", LAST_RD_NAME1)
                LAST_RD_INFO &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:center; font-size:40px;font-family:arial;line-height:50px;"">{0}</div>", LAST_RD_Empno2)
                LAST_RD_INFO &= String.Format("<div style=""width:160px; height:50px; float:left; text-align:left; font-size:40px;line-height:50px;"">{0}</div>", LAST_RD_NAME2)

            Else
                LAST_RD_INFO &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:center; line-height:100px; font-size:40px;font-family:arial;"">{0}</div>", LAST_RD_Empno1)
                LAST_RD_INFO &= String.Format("<div style=""width:160px; height:100px; float:left; text-align:left; line-height:100px; font-size:50px"">{0}</div>", LAST_RD_NAME1)
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
            LAST_RD_INFO_Label.Text = LAST_RD_INFO
            '  NSPEmpnoLabel.Text = NSP_Empno
            '  NSPNAMElabel.Text = NSP_NAME
        End If

            If (Conn.State = ConnectionState.Open) Then
                Conn.Close()
                Conn.Dispose()
            End If



    End Function

    '撈出護理站的主責科別並判別大科別(兒科PD、婦科GO、大內科IM、大外科SG)
    Public Function Get_Master(ByVal Hospcode As String, ByVal Wardcode As String) As String
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim DT As DataTable = New DataTable
        'Dim HD_cmd As String = String.Format("SELECT A.NS,B.master FROM WHTBOARD A, DEPT B WHERE A.NS='{0}' and A.GRP='主責科別' and A.empno='mstr' AND A.NAME = B.code", Wardcode)
        Dim HD_cmd As String = String.Format("SELECT A.NAME  FROM WHTBOARD A  WHERE A.NS='{0}' and A.GRP='主責科別' and A.empno='mstr' ", Wardcode)
        Dim HD_DA As OleDbDataAdapter = New OleDbDataAdapter(HD_cmd, Conn)
        HD_DA.Fill(DT)

        Dim Master As String = Nothing
        If DT.Rows.Count > 0 Then
            Master = DT.Rows(0)("NAME").ToString
        End If
        Return Master


        If (Conn.State = ConnectionState.Open) Then
            Conn.Close()
            Conn.Dispose()
        End If
    End Function
    '###############################################團隊連絡資料######################################################
    Public Function Contact_List(ByVal Hospcode As String, ByVal Wardcode As String, ByVal ContactCount As Integer)
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM MMH.Whtboard AL1 WHERE AL1.NS='{0}' AND seqno != 'xx'  and (empno not in ('mstr','day','duty','DEN') OR empno is  NULL) ORDER BY AL1.Seqno", Wardcode)
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
            If index < ContactCount Then '只顯示12筆資料
                ContactInfo &= String.Format("<div  style=""height:74px; vertical-align:top; border-radius:15px; border-style:solid; border-width:1px; margin-bottom:1px;"">")
                ContactInfo &= String.Format("       <div style=""height:37px; font-size:{2}px; text-align:center; line-height:37px; font-weight:bold;"">{0}  {1}</div>", Grp, Name, Font_size)
                ContactInfo &= String.Format("       <div style=""height:37px; font-size:35px; text-align:center; line-height:37px; font-weight:bold;font-family:arial;"">{0}</div>", Tel)
                ContactInfo &= String.Format("</div>")
            End If
        Next
        ContactListLabel.Text = ContactInfo
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

    '主程式
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim NsSort As String = Request.QueryString("NsSort")
        Dim Nurse_count As Integer
        Dim Intern_count As Integer
        Dim Dancag As String = Get_Dancag(0) '控制護理站名稱和護士班表切換時點
        Dim NowHour As Integer = DateTime.Now.Hour
        Dim Version As String = VersionP(HospCode, WardCode) '以院區代號和護理站代號判別版本傳回版本代號
        Dim NSENG = Show_Ward_Name(HospCode, WardCode, Dancag) '顯示護理站簡稱

        SERVER_TIME() '抓取系統時間
        Nurse_count = Nurse_List(HospCode, WardCode, 9, 15, NsSort, Dancag) '列出護士班表資料
        Intern_count = 11 - Nurse_count
        BanList(HospCode, WardCode) '顯示主治醫師和住院醫師所負責的病床
        OnDutyDoctor(HospCode, WardCode) ' 顯示醫師值班資料
        Contact_List(HospCode, WardCode, 12) '列出連絡人資料
        Encoding_type("劉瑩", 36) '判斷是否有外字,將外字的字型設定為指定大小後回傳
        UpdateTimeLabel.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") '顯示更新時間

    End Sub
End Class
