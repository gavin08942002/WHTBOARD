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
    '依目前時間取得護理班別0800,1600,0000(可調整時間差)
    Public Function Get_Dancag2(ByVal Hospcode As String, ByVal Wardcode As String, ByVal adjust_m As Integer) As String
        'adjust_m    切換時點的分鐘調整數
        '取得護理站班表切換時間
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM WHTBOARD  WHERE NS='{0}' AND EMPNO='DEN' ", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DT As New DataTable

        DA.Fill(DT)

        Dim D_Time As Integer = 800 '預設日班換班時間
        Dim E_Time As Integer = 1600 ' 預設中班換班時間
        Dim N_Time As Integer = 0 ' 預設大夜換班時間
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


    '值班護士資料
    Public Function Nurse_List(ByVal Hospcode As String, ByVal Wardcode As String) As Integer
        Dim IP_PHONE_TB As DataTable = New DataTable
        '撈取ip-phone資料
        IP_PHONE_TB = IP_PHONE(Hospcode, Wardcode)

        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim ConnORA As String = SELECT_ORACLE(0)
        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.Day())
        Dim DanCag As String = Nothing '判斷目前時間的班別
        Dim NowHour As Integer = Now.Hour
        Dim addDay As Integer = 5

        '開始時間
        Dim FromDate As String = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.Day())
        '結束時間
        Dim ToDate As String = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.AddDays(1).Year(), DateTime.Today.AddDays(1).Month(), DateTime.Now.AddDays(1).Day)

        DanCag = Get_Dancag(Hospcode, Wardcode, 0)

        '值班護士名單資料
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  DISTINCT (AL1.EMPNO), AL1.DEN, AL1.DUTY, AL1.XFIRE, AL1.NAME, AL1.BACKUP1 FROM MMH.NSASSIST AL1 WHERE AL1.NS='{0}' AND AL1.DEN='{1}' AND AL1.FDATE >= to_date('{2}','YYYY/MM/DD HH24:MI:SS') AND AL1.FDATE < to_date('{3}','YYYY/MM/DD HH24:MI:SS') AND AL1.EFFECT='Y' ORDER BY AL1.DUTY", Wardcode, DanCag, FromDate, ToDate)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataSet
        DA.Fill(DS, "NurseList") '護理人員資料
        '病床資料
        Dim cmd2 As String = String.Format("SELECT  AL1.EMPNO, AL1.Bedno, AL1.Pno FROM MMH.NSASSIST AL1 WHERE AL1.NS='{0}' AND AL1.DEN='{1}' AND AL1.FDATE >= to_date('{2}','YYYY/MM/DD HH24:MI:SS') AND AL1.FDATE < to_date('{3}','YYYY/MM/DD HH24:MI:SS') AND AL1.EFFECT='Y'", Wardcode, DanCag, FromDate, ToDate)
        Dim DA2 As OleDbDataAdapter = New OleDbDataAdapter(cmd2, Conn)
        DA2.Fill(DS, "BedList") '病床資料

        Dim NurseInfo As String = Nothing
        'NEW PATIENT 病床ARRAY
        Dim cmd3 As String = String.Format("SELECT  Bedno, Beddate  FROM Bedtbl  WHERE Bedns='{0}' And Bedsts='1' AND Bedroom!='99'", Wardcode)
        Dim DA3 As OleDbDataAdapter = New OleDbDataAdapter(cmd3, Conn)
        Dim DT3 As New DataTable
        Dim New_Patient_List As ArrayList = New ArrayList '新進病人ARRAY
        DA3.Fill(DT3)
        For p = 0 To DT3.Rows.Count - 1
            If DT3.Rows(p)("Beddate") = Nowdate Then
                New_Patient_List.Add(DT3.Rows(p)(0))
            End If
        Next



        '目前有佔床的床位號
        Dim cmd4 As String = String.Format("SELECT  Bedno  FROM Bedtbl  WHERE Bedns='{0}' And Bedsts='1' AND Bedroom!='99'", Wardcode)
        Dim DA4 As OleDbDataAdapter = New OleDbDataAdapter(cmd4, Conn)
        Dim InBed_List As ArrayList = New ArrayList '目前佔床床位後三碼ARRAY
        DA4.Fill(DS, "InBed")
        For l = 0 To DS.Tables("InBed").Rows.Count - 1
            InBed_List.Add(Right(DS.Tables("InBed").Rows(l)("Bedno").ToString, 3))
        Next

        For index = 0 To (DS.Tables("NurseList").Rows.Count - 1)
            '處理護士所負責的病床資料==========
            Dim DV As DataView = DS.Tables("BedList").DefaultView
            Dim DVTable As DataTable = Nothing
            Dim BedList As String = Nothing '床位顯示字串
            Dim BedCount As Integer = 0 '非空床床位數
            DV.RowFilter = ""
            DV.RowFilter = String.Format("EMPNO = '{0}'", DS.Tables("NurseList").Rows(index)("EMPNO").ToString)
            DV.Sort = "Bedno"
            DVTable = DV.ToTable 'DataView無法直接抓取己處理的資料,須轉為Table,DataView.table內是DataView的原始資料

            For i = 0 To (DV.Count - 1)
                'If InBed_List.Contains(Right(DVTable.Rows(i)(1).ToString, 3)) Then '判斷是否為空床
                'BedCount += 1  '計算非空床的床位數
                If New_Patient_List.Contains(Right(DVTable.Rows(i)("BEDNO").ToString, 3)) Then '判斷是否為NEW PATIENT,如為New Patient變更框架為藍底白字
                    BedList &= String.Format("<div class="" New_nurse_ward"">{0}</div>", Right(DVTable.Rows(i)(1).ToString, 3))
                    BedCount += 1  '計算非空床的床位數
                ElseIf Not String.IsNullOrEmpty(DVTable.Rows(i)("BEDNO").ToString) Then
                    BedList &= String.Format("<div class="" Nurse_ward "">{0}</div>", Right(DVTable.Rows(i)(1).ToString, 3))
                    BedCount += 1  '計算非空床的床位數
                End If
                'End If
            Next
            '==================================
            '處理備援資料====
            Dim DVNurseList As DataView = DS.Tables("NurseList").DefaultView
            Dim DVNurseTable As DataTable = Nothing
            Dim BackupCode As String = Nothing
            DVNurseList.RowFilter = String.Format("EMPNO = '{0}'", DS.Tables("NurseList").Rows(index)("BACKUP1").ToString)
            DVNurseTable = DVNurseList.ToTable
            If (DVNurseTable.Rows.Count > 0) Then
                BackupCode = Left(DVNurseTable.Rows(0)("DUTY"), 1)
            End If
            '================
            '處理護理人員資料
            Dim EMPNO As String = DS.Tables("NurseList").Rows(index)("Empno").ToString
            Dim Name As String = DS.Tables("NurseList").Rows(index)("Name").ToString
            Dim Duty As String = DS.Tables("NurseList").Rows(index)("Duty").ToString
            Dim Xfire As String = DS.Tables("NurseList").Rows(index)("Xfire").ToString

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
            If InStr(1, Duty, "L") = 0 Then
                NurseInfo &= String.Format("          <div style=""width:150px; height:65px; font-size:50px;line-height:45px;"">{0}</div>", Name)
            Else
                NurseInfo &= String.Format("          <div style=""width:150px; height:65px; font-size:50px;line-height:45px; color:rgb(239, 255, 0);"">{0}</div>", Name)
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
        Next
        NurseListLabel.Text = NurseInfo
        DS.Dispose()
        Conn.Close()
        

        Return DS.Tables("NurseList").Rows.Count
    End Function
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
    '主治及住院醫師資料
    Public Function BanList(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Dr_Upper_limite As Integer, ByVal Bed_Upper_limite As Integer)
        'Dr_Upper_limite 醫生顯示資料筆數
        'Bed_Upper_limite 病床顯示資料筆數
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)

        Dim Nowdate As String = String.Format("{0}/{1}/{2} 00:00:00", DateTime.Today.Year(), DateTime.Today.Month(), DateTime.Today.Day())
        Dim DR_List As ArrayList = New ArrayList '主治醫師列表
        Dim New_Patient_List As ArrayList = New ArrayList  '今日新進病人名單,全五碼

        '所有病房內病床和主治醫師資料==Bedtbl
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Conn.Open()
        Dim cmd As String = String.Format("SELECT  Bedno, Beddate, beddr, beddr2, beddr3, beddr4, beddr5, beddr6, beddr7, beddr8, beddr9   FROM Bedtbl  WHERE Bedns='{0}' And Bedsts='1' AND Bedroom!='99'", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataSet
        DA.Fill(DS, "BedInfo") '
        Conn.Close()

        '主治醫師和住院醫師配對資料
        Dim Conn1 As OleDbConnection = New OleDbConnection
        Conn1.ConnectionString = ConnStr
        Dim cmd1 As String = String.Format("SELECT * FROM WHTSCHED WHERE NS='{0}' AND Bdate = to_date('{1}','YYYY/MM/DD HH24:MI:SS') ", Wardcode, Nowdate)
        Dim DA1 As OleDbDataAdapter = New OleDbDataAdapter(cmd1, Conn1)
        DA1.Fill(DS, "SCHED")
        Conn1.Close()

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
                'Response.Write(DS.Tables("BedInfo").Rows(i)(0))
            End If
        Next
        DR_List.Sort()
        '===========================================

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
        '@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
        '===========================================

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
            '取出SCHED Table內指定醫師的主治和任院醫師關聯資料
            Dim SCHED_DV As DataView = New DataView(DS.Tables("SCHED").DefaultView.ToTable)
            SCHED_DV.RowFilter = String.Format("AD_EMPNO = '{0}'", DR_List(i))
            '取出醫生所負責的病房個數
            Dim SCHED_COUNT As DataView = New DataView(DS.Tables("BedInfo").DefaultView.ToTable)
            SCHED_COUNT.RowFilter = String.Format("Beddr = '{0}'", DR_List(i))
            Dim Bed_Qt As Integer = SCHED_COUNT.Count
            Dim AD_NAME() As DataRow = DS.Tables("Doctor_name_list").Select(String.Format("CODE = '{0}'", DR_List(i)))
            If AD_NAME.Count > 0 Then
                New_row1("AD_EMPNO") = DR_List(i).ToString
                New_row1("AD_NAME") = AD_NAME(0)("NAME").ToString
                New_row1("Bed_Qt") = Bed_Qt
                If SCHED_DV.ToTable.Rows.Count > 0 Then
                    Dim SCHED_TEMP As DataTable = SCHED_DV.ToTable
                    New_row1("HD_EMPNO") = SCHED_TEMP.Rows(0)("HD_EMPNO")
                    New_row1("HD_NAME") = SCHED_TEMP.Rows(0)("HD_NAME")
                End If
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

        'GridView1.DataSource = Doctor_info_sort
        'GridView1.DataBind()
        'GridView2.DataSource = DS.Tables("Doctor_name_list")
        ' GridView2.DataBind()
        'GridView2.DataSource = Bed_info_sort
        'GridView2.DataBind()
        '###########################################
        '資料輸出$$$$$$$$$$$$$$$$$$$$$$$$$$
        Dim Doctor_Info_show1 As String = Nothing 'Doctor1醫生資料輸出
        Dim Doctor_Info_show2 As String = Nothing 'Doctor2醫生資料輸出
        Dim Doctor_Row_count As Integer = 0

        For w = 0 To (Doctor_info_sort.Rows.Count - 1)
            Dim Doctor_Info_show As String = Nothing '單筆醫生資料輸出
            '病床資料輸出
            Doctor_Row_count = Doctor_Row_count + 1
            Dim Bedrow() As DataRow = Bed_info_sort.Select(String.Format("BEDDR = '{0}'", Doctor_info_sort.Rows(w)("AD_EMPNO")))
            Dim BedRow_Show As String = Nothing

            Dim h As Integer = 0
            For Each Data_Row As DataRow In Bedrow
                h = h + 1
                If h <= Bed_Upper_limite Then '只顯示前10筆資料
                    If New_Patient_List.Contains(Data_Row(0)) Then '判斷是否NEW PATIENT
                        BedRow_Show &= String.Format("<div class=""new_doctor_ward"">{0}</div>", Right(Data_Row(0), 3))
                    Else
                        BedRow_Show &= String.Format("<div class=""doctor_ward"">{0}</div>", Right(Data_Row(0), 3))
                    End If
                End If

            Next

            Doctor_Info_show &= String.Format("<div style=""width:500px;height:80px;border-bottom-style:groove;border-width:1px;margin:2px;"">")
            Doctor_Info_show &= String.Format("     <div style=""height:80px;width:80px;float:left;"">")
            Doctor_Info_show &= String.Format("                    <div style=""height:50px; width:80px; float:left; font-size:26px; line-height:50px; text-align:center; background-color:#F0A75F;"">{0}</div>", Doctor_info_sort.Rows(w)("AD_NAME"))
            ' Doctor_Info_show &= String.Format("                    <div style=""height:50px; width:120px; float:left; font-size:40px; line-height:50px; text-align:center; background-color:#E0FFFF;"">{0}</div>", Doctor_info_sort.Rows(w)("HD_NAME"))
            Doctor_Info_show &= String.Format("                    <div style=""height:28px; width:80px; float:left; font-size:24px; line-height:28px; text-align:center; background-color:#F0A75F; font-family:Arial;"">{0}</div>", Doctor_info_sort.Rows(w)("AD_EMPNO"))
            ' Doctor_Info_show &= String.Format("                    <div style=""height:28px; width:120px; float:left; font-size:28px; line-height:28px; text-align:center; background-color:#E0FFFF; font-family:Arial;"">{0}</div>", Doctor_info_sort.Rows(w)("HD_EMPNO"))
            Doctor_Info_show &= String.Format("     </div>")
            Doctor_Info_show &= String.Format("     <div style=""height:80px;width:420px;float:left;"">{0}", BedRow_Show)
            Doctor_Info_show &= String.Format("      </div>")
            Doctor_Info_show &= String.Format("</div>")

            If (Doctor_Row_count <= Dr_Upper_limite) Then
                Doctor_Info_show1 &= Doctor_Info_show
            ElseIf (Doctor_Row_count <= (Dr_Upper_limite * 2)) Then
                Doctor_Info_show2 &= Doctor_Info_show
            Else
            End If
        Next

        Doctor_Info_Label1.Text = Doctor_Info_show1
        Doctor_Info_Label2.Text = Doctor_Info_show2
        '$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$$
        If (Conn.State = ConnectionState.Open) Then
            Conn.Close()
            Conn.Dispose()
        End If




    End Function
    '連絡資料
    Public Function Contact_List(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Upper_limit As Integer)
        'Upper_limit  資料顯示筆數的上限
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
            If index < Upper_limit Then
                ContactInfo &= String.Format("<div  style=""height:74px; vertical-align:top; border-radius:15px; border-style:solid; border-width:1px; margin-bottom:1px;"">")
                ContactInfo &= String.Format("       <div style=""height:37px; font-size:{2}px; text-align:center; line-height:37px; font-weight:bold;"">{0}  {1}</div>", Grp, Name, Font_size)
                ContactInfo &= String.Format("       <div style=""height:37px; font-size:35px; text-align:center; line-height:37px; font-weight:bold;font-family:arial;"">{0}</div>", Tel)
                ContactInfo &= String.Format("</div>")
            End If
        Next
        ContactListLabel.Text = ContactInfo
    End Function
    '護理站名稱
    Public Function Show_Ward_Name(ByVal Hospcode As String, ByVal Wardcode As String)
        Dim ConnStr As String = SELECT_ORACLE(Hospcode)

        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT  * FROM NSTBL  WHERE NS='{0}' ", Wardcode)
        Dim Dancag As String = Get_Dancag(Hospcode, Wardcode, 0)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataTable
        Dim ContactInfo As String = Nothing
        DA.Fill(DS) '連絡人資料
        If DS.Rows.Count > 0 Then
            Show_Ward_Name_Label.Text = String.Format("<span class = Numeric-font>{0}-{1}</span>", DS.Rows(0)("nseng"), Dancag)
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

    
    '輸出V2醫師值班內容
    
    Public Sub BanListV2(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Bedcount As Integer)

        Dim ConnStr As String = SELECT_ORACLE(Hospcode)
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT DISTINCT   Beddept FROM Bedtbl AL1 WHERE AL1.bedNS='{0}' ORDER BY AL1.Beddept", Wardcode)
        Dim DA As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        Dim DS As New DataSet

        Dim DDA As List(Of String) = New List(Of String)
        DDA.Add("30")



        DA.Fill(DS, "Beddept_List")

        ' Dim ws As GetPeopleData.ws_psnSoapClient = New GetPeopleData.ws_psnSoapClient
        ' Dim ttt As List(Of PeopleData) = New List(Of PeopleData)
        ' ttt = ws.GetPeopleData("2015/6/26")


        'GridView1.DataSource = DS.Tables("Beddept_List")
        'GridView1.DataBind()
        'Conn.Close()


    End Sub
    '主程式
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim Nurse_count As Integer
        Dim Intern_count As Integer
        Dim Dancag As String = Get_Dancag2(HospCode, WardCode, 0) '控制護理站名稱和護士班表切換時點
        Dim NowHour As Integer = DateTime.Now.Hour
        
        SERVER_TIME() '抓取系統時間
        Nurse_count = Nurse_List(HospCode, WardCode) '列出護士班表資料
        Intern_count = 11 - Nurse_count
        ' Intern_List(HospCode, WardCode, Intern_count) '列出實習醫師資料
        BanList(HospCode, WardCode, 11, 14) '顯示主治醫師和住院醫師所負責的病床
        ' OnDutyDoctor(HospCode, WardCode) ' 顯示醫師值班資料


        Contact_List(HospCode, WardCode, 13) '列出連絡人資料
        Show_Ward_Name(HospCode, WardCode) '顯示護理站簡稱
        UpdateTimeLabel.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") '顯示更新時間

    End Sub
End Class
