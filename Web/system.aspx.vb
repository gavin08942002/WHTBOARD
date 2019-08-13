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
Imports NPOI.SS.UserModel

Partial Class Web_Default
    Inherits System.Web.UI.Page
    Public Function SELECT_ORACLE(ByVal str As String) As String
        Dim OraStr As String = Nothing

        Select Case str
            Case "1"
                OraStr = "Provider=OraOLEDB.Oracle;user id=ord;data source=tp_opd_ord; persist security info=true;password=mmhedp"
            Case "2"
                OraStr = "Provider=OraOLEDB.Oracle;user id=ord;data source=ts_opd_ord; persist security info=true;password=mmhedp"
            Case "3"
                OraStr = "Provider=OraOLEDB.Oracle;user id=ord;data source=tt_opd_ord; persist security info=true;password=mmhedp"
            Case "4"
                OraStr = "Provider=OraOLEDB.Oracle;user id=ord;data source=hc_opd_ord; persist security info=true;password=mmhedp"
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
        DA.Fill(DS)
        If DS.Rows.Count > 0 Then
            Return DS.Rows(0)("nseng")
        End If
    End Function
    Protected Function Show_User() As String
        Dim User_Name As String = "Test"
        Return User_Name
    End Function
    Protected Sub Hyperlink_Default(ByVal HospCode As String, ByVal WardCode As String)

    End Sub

    '匯入主治、住院、實習醫師配對表
    Public Function SCHED_INSERT(ByVal SCHED_Sheet As ISheet, ByVal HospCode As String, ByVal WardCode As String, ByVal ComName As String, ByVal UOPER As String)
        'INSERT 主治與住院醫師配對資料至資料庫==============
        For i As Integer = (SCHED_Sheet.FirstRowNum + 1) To SCHED_Sheet.LastRowNum
            Dim SCHED_ROW As IRow = SCHED_Sheet.GetRow(i)
            '當起始日期,結束日期,護理站代號有其中一個為空值時,跳出迴圈
            If String.IsNullOrEmpty(SCHED_ROW.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString) Or String.IsNullOrEmpty(SCHED_ROW.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString) Or String.IsNullOrEmpty(SCHED_ROW.GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString) Then
                Exit For
            Else

                '宣告所需要的字串
                Dim BDATE As DateTime = Convert.ToDateTime(SCHED_ROW.GetCell(0).ToString)
                Dim EDATE As DateTime = Convert.ToDateTime(SCHED_ROW.GetCell(1).ToString)
                Dim NS As String = SCHED_ROW.GetCell(2).ToString
                Dim AD_EMPNO As String = Nothing
                '主治員編
                If Not (SCHED_ROW.GetCell(3) Is Nothing) Then
                    AD_EMPNO = SCHED_ROW.GetCell(3).ToString()
                End If
                '主治姓名
                Dim AD_NAME As String = Nothing
                If Not (SCHED_ROW.GetCell(4) Is Nothing) Then
                    AD_NAME = SCHED_ROW.GetCell(4).ToString()
                End If
                '住院員編
                Dim HD_EMPNO As String = Nothing
                If Not (SCHED_ROW.GetCell(5) Is Nothing) Then
                    HD_EMPNO = SCHED_ROW.GetCell(5).ToString()
                End If
                '住院姓名
                Dim HD_NAME As String = Nothing
                If Not (SCHED_ROW.GetCell(6) Is Nothing) Then
                    HD_NAME = SCHED_ROW.GetCell(6).ToString()
                End If
                '實習員編
                Dim INT_Empno As String = Nothing
                If Not (SCHED_ROW.GetCell(7) Is Nothing) Then
                    INT_Empno = SCHED_ROW.GetCell(7).ToString()
                End If
                '實習電話
                Dim INT_Phone As String = Nothing
                If Not (SCHED_ROW.GetCell(8) Is Nothing) Then
                    INT_Phone = SCHED_ROW.GetCell(8).ToString()
                End If
                '實習姓名
                Dim INT_Name As String = Nothing
                If Not (SCHED_ROW.GetCell(9) Is Nothing) Then
                    INT_Name = SCHED_ROW.GetCell(9).ToString()
                End If
                '住院休假
                Dim Conn As OleDbConnection = New OleDbConnection

                Dim Connstr As String = SELECT_ORACLE(HospCode)
                Conn.ConnectionString = Connstr
                Conn.Open()

                Dim Moment As Integer = DateDiff(DateInterval.Day, BDATE, EDATE)
                For t = 0 To Moment
                    Dim Temp_date As Date = BDATE.AddDays(t)
                    Dim Temp_Date_Str As String = String.Format("{0}/{1}/{2} 00:00:00", Temp_date.Year(), Temp_date.Month(), Temp_date.Day())
                    '新增時間判斷
                    Dim temp_Bdate_Str As String = String.Format("{0}/{1}/{2} 00:08:00", Temp_date.Year(), Temp_date.Month(), Temp_date.Day())
                    Dim temp_Edate_Str As String = String.Format("{0}/{1}/{2} 00:08:00", Temp_date.Year(), Temp_date.Month(), Temp_date.AddDays(1).Day())
                    Dim SCHED_ID As String = NS & Right(Temp_date.ToString("yyyy"), 3) & Temp_date.ToString("MM") & Temp_date.ToString("dd") & AD_EMPNO

                    '將資料插入資料庫===============
                    Dim DeleteCmd As String = String.Format("DELETE  FROM WHTSCHED WHERE NS='{0}' AND BDATE = '{1}'  AND AD_EMPNO = '{2}'", NS, Temp_Date_Str, AD_EMPNO)
                    Dim DeleteDC As OleDbCommand = New OleDbCommand(DeleteCmd, Conn)

                    Dim InsertCmd As String = String.Format("INSERT INTO WHTSCHED (Bdate, Edate, NS, AD_EMPNO, AD_NAME, HD_EMPNO, HD_NAME, UDATE,UOPER, Termid, INT_Empno, INT_Phone, INT_Name) VALUES (to_date('{0}','YYYY/MM/DD HH24:MI:SS'),to_date('{1}','YYYY/MM/DD HH24:MI:SS'),'{2}','{3}','{4}','{5}','{6}',to_date(sysdate,'YYYY/MM/DD HH24:MI:SS'), '{7}', '{8}', '{9}', '{10}', '{11}')", Temp_date.ToString("yyyy/MM/dd 00:00:00"), EDATE.ToString("yyyy/MM/dd 23:59:59"), NS, AD_EMPNO, AD_NAME, HD_EMPNO, HD_NAME, UOPER, ComName, INT_Empno, INT_Phone, INT_Name)
                    'Response.Write(String.Format("SCHED=={0},開始時間:{1},結束時間{2},{3},{4},{5},{6}", SCHED_ID, Temp_date, EDATE, NS, AD_EMPNO, AD_NAME, HD_EMPNO, HD_NAME))
                    Dim InsertDC As OleDbCommand = New OleDbCommand(InsertCmd, Conn)
                    DeleteDC.ExecuteNonQuery()
                    InsertDC.ExecuteNonQuery()
                    '===============================
                Next
                Conn.Close()
            End If
        Next
        '===================================================
    End Function
    '匯入下排醫師總值、值班表
    Public Function DUTY_INSERT(ByVal DUTY_Sheet As ISheet, ByVal HospCode As String, ByVal WardCode As String, ByVal ComNAME As String, ByVal UOPER As String)

        '連結資料庫
        Dim Conn As OleDbConnection = New OleDbConnection
        Dim Connstr As String = SELECT_ORACLE(HospCode)
        Conn.ConnectionString = Connstr
        Conn.Open()

        'INSERT值班醫師名單
        For k As Integer = (DUTY_Sheet.FirstRowNum + 1) To DUTY_Sheet.LastRowNum

            Dim DUTY_ROW As IRow = DUTY_Sheet.GetRow(k)
            '日期,NS,總值編其一有空值時跳出迴圈
            If String.IsNullOrEmpty(DUTY_ROW.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString) Or String.IsNullOrEmpty(DUTY_ROW.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString) Or String.IsNullOrEmpty(DUTY_ROW.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString) Then

                Exit For
            Else



                '宣告所需要的字串
                Dim DUTY_DATE As DateTime = Convert.ToDateTime(DUTY_ROW.GetCell(0).ToString)
                Dim DUTY_DATE_Str As String = Format(DUTY_DATE, "yyyy/MM/dd 00:00:00")


                Dim NS As String = Nothing
                If Not (DUTY_ROW.GetCell(1) Is Nothing) Then
                    NS = DUTY_ROW.GetCell(1).ToString
                End If

                Dim MRD_EMPNO1 As String = Nothing
                If Not (DUTY_ROW.GetCell(2) Is Nothing) Then
                    MRD_EMPNO1 = DUTY_ROW.GetCell(2).ToString()
                End If

                Dim MRD_NAME1 As String = Nothing
                If Not (DUTY_ROW.GetCell(3) Is Nothing) Then
                    MRD_NAME1 = DUTY_ROW.GetCell(3).ToString()
                End If

                Dim MRD_EMPNO2 As String = Nothing
                If Not (DUTY_ROW.GetCell(4) Is Nothing) Then
                    MRD_EMPNO2 = DUTY_ROW.GetCell(4).ToString()
                End If

                Dim MRD_NAME2 As String = Nothing
                If Not (DUTY_ROW.GetCell(5) Is Nothing) Then
                    MRD_NAME2 = DUTY_ROW.GetCell(5).ToString()
                End If

                Dim RD_EMPNO1 As String = Nothing
                If Not (DUTY_ROW.GetCell(6) Is Nothing) Then
                    RD_EMPNO1 = DUTY_ROW.GetCell(6).ToString()
                End If

                Dim RD_NAME1 As String = Nothing
                If Not (DUTY_ROW.GetCell(7) Is Nothing) Then
                    RD_NAME1 = DUTY_ROW.GetCell(7).ToString()
                End If

                Dim RD_EMPNO2 As String = Nothing
                If Not (DUTY_ROW.GetCell(8) Is Nothing) Then
                    RD_EMPNO2 = DUTY_ROW.GetCell(8).ToString()
                End If


                Dim RD_NAME2 As String = Nothing
                If Not (DUTY_ROW.GetCell(9) Is Nothing) Then
                    RD_NAME2 = DUTY_ROW.GetCell(9).ToString()
                End If

                Dim INT_EMPNO As String = Nothing
                If Not (DUTY_ROW.GetCell(10) Is Nothing) Then
                    INT_EMPNO = DUTY_ROW.GetCell(10).ToString()
                End If

                Dim INT_NAME As String = Nothing
                If Not (DUTY_ROW.GetCell(11) Is Nothing) Then
                    INT_NAME = DUTY_ROW.GetCell(11).ToString()
                End If

                Dim INT_PHONE As String = Nothing
                If Not (DUTY_ROW.GetCell(12) Is Nothing) Then
                    INT_PHONE = DUTY_ROW.GetCell(12).ToString()
                End If


                Dim NSP_EMPNO As String = Nothing
                If Not (DUTY_ROW.GetCell(13) Is Nothing) Then
                    NSP_EMPNO = DUTY_ROW.GetCell(13).ToString()
                End If

                Dim NSP_NAME As String = Nothing
                If Not (DUTY_ROW.GetCell(14) Is Nothing) Then
                    NSP_NAME = DUTY_ROW.GetCell(14).ToString()
                End If

                Dim NSP_PHONE As String = Nothing
                If Not (DUTY_ROW.GetCell(15) Is Nothing) Then
                    NSP_PHONE = DUTY_ROW.GetCell(15).ToString()
                End If
                Console.WriteLine(INT_PHONE)
                Dim DUTY_ID As String = NS.PadLeft(3, "0") & Right(DUTY_DATE.ToString("yyyy"), 3) & DUTY_DATE.ToString("MM") & DUTY_DATE.ToString("dd")

                '清除重複資料
                Dim DeleteCmd As String = String.Format("DELETE FROM WHTDUTY WHERE NS = '{0}' AND DUTY_DATE = '{1}'", NS, DUTY_DATE_Str)
                Dim DeleteDC As OleDbCommand = New OleDbCommand(DeleteCmd, Conn)
                DeleteDC.ExecuteNonQuery()

                '存入資料
                Dim INSERTCmd As String = String.Format(" INSERT INTO WHTDUTY (DUTY_DATE, NS, MRD_EMPNO1, MRD_NAME1, MRD_EMPNO2, MRD_NAME2, RD_EMPNO1, RD_NAME1, RD_EMPNO2, RD_NAME2, UDATE,UOPER,TERMID,INT_Empno, INT_NAME,INT_PHONE, NSP_Empno, NSP_NAME,NSP_PHONE) VALUES ('{0}','{1}', '{2}' , '{3}', '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', to_date(sysdate,'YYYY/MM/DD HH24:MI:SS'),'{10}','{11}','{12}','{13}','{14}','{15}','{16}','{17}' )", DUTY_DATE_Str, NS, MRD_EMPNO1, MRD_NAME1, MRD_EMPNO2, MRD_NAME2, RD_EMPNO1, RD_NAME1, RD_EMPNO2, RD_NAME2, UOPER, ComNAME, INT_EMPNO, INT_NAME, INT_PHONE, NSP_EMPNO, NSP_NAME, NSP_PHONE)
                'DUTY_DATE_Str, NS, MRD_EMPNO1, MRD_NAME1, MRD_EMPNO2, MRD_NAME2, RD_EMPNO1, RD_NAME1, RD_EMPNO2, RD_NAME2, UOPER, ComNAME, INT_EMPNO, INT_NAME, INT_PHONE, NSP_EMPNO, NSP_NAME, NSP_PHONE
                Dim INSERTDC As OleDbCommand = New OleDbCommand(INSERTCmd, Conn)
                INSERTDC.ExecuteNonQuery()
                'Response.Write("<Br>")
            End If
        Next
        Conn.Close()
    End Function
    '匯入代班醫師表
    Public Function SUBT_INSERT(ByVal SUBT_sheet As ISheet, ByVal HospCode As String, ByVal WardCode As String, ByVal ComNAME As String, ByVal UOPER As String) As String
        '連結資料庫
        Dim Conn As OleDbConnection = New OleDbConnection
        Dim Connstr As String = SELECT_ORACLE(HospCode)
        Conn.ConnectionString = Connstr
        Conn.Open()

        For k As Integer = (SUBT_sheet.FirstRowNum + 1) To SUBT_sheet.LastRowNum
            Dim SUBT_ROW As IRow = SUBT_sheet.GetRow(k)
            '開始時期,結束日期,NS有其一為空,跳出迴圈
            If String.IsNullOrEmpty(SUBT_ROW.GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString) Or String.IsNullOrEmpty(SUBT_ROW.GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString) Or String.IsNullOrEmpty(SUBT_ROW.GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString) Then
                Exit For
            Else

                '宣告所需要的字串
                Dim BEGIN_DATE As Date = SUBT_ROW.GetCell(0).DateCellValue
                Dim END_DATE As Date = SUBT_ROW.GetCell(1).DateCellValue
                Dim NS As String = SUBT_ROW.GetCell(2).ToString
                Dim HD_EMPNO As String = SUBT_ROW.GetCell(3).ToString
                If Not (SUBT_ROW.GetCell(3) Is Nothing) Then
                    HD_EMPNO = SUBT_ROW.GetCell(3).ToString()
                End If

                Dim HD_NAME As String = Nothing
                If Not (SUBT_ROW.GetCell(4) Is Nothing) Then
                    HD_NAME = SUBT_ROW.GetCell(4).ToString()
                End If

                Dim SD_EMPNO As String = Nothing
                If Not (SUBT_ROW.GetCell(5) Is Nothing) Then
                    SD_EMPNO = SUBT_ROW.GetCell(5).ToString()
                End If

                Dim SD_PHONE As String = Nothing
                If Not (SUBT_ROW.GetCell(6) Is Nothing) Then
                    SD_PHONE = SUBT_ROW.GetCell(6).ToString()
                End If

                Dim SD_NAME As String = Nothing
                If Not (SUBT_ROW.GetCell(7) Is Nothing) Then
                    SD_NAME = SUBT_ROW.GetCell(7).ToString()
                End If

                If END_DATE >= BEGIN_DATE Then
                    Dim Moment As Integer = DateDiff(DateInterval.Day, BEGIN_DATE, END_DATE)
                    For t As Integer = 0 To Moment
                        Dim Temp_Date As Date = BEGIN_DATE.AddDays(t)
                        Dim Temp_Date_Str As String = String.Format("{0}/{1}/{2} 00:00:00", Temp_Date.Year(), Temp_Date.Month(), Temp_Date.Day())


                        '刪除重複資料
                        Dim DeleteCmd As String = String.Format("DELETE FROM WHTSUBT WHERE NS='{0}' AND Bdate='{1}' AND HD_Empno='{2}'", NS, Temp_Date_Str, HD_EMPNO)
                        Dim DeleteDC As OleDbCommand = New OleDbCommand(DeleteCmd, Conn)
                        DeleteDC.ExecuteNonQuery()
                        'Response.Write("test--")
                        '存入資料
                        Dim InsertCmd As String = String.Format("INSERT INTO WHTSUBT (NS, Bdate, Hd_Empno, Hd_Name, Sd_Empno, Sd_Phone, Sd_Name,UDATE) VALUES ('{0}','{1}','{2}','{3}','{4}','{5}','{6}',to_date(sysdate,'YYYY/MM/DD HH24:MI:SS'))", NS, Temp_Date_Str, HD_EMPNO, HD_NAME, SD_EMPNO, SD_PHONE, SD_NAME)
                        Dim INSERTDC As OleDbCommand = New OleDbCommand(InsertCmd, Conn)
                        INSERTDC.ExecuteNonQuery()

                    Next
                End If
            End If
        Next
        Conn.Close()
    End Function
    '取得電腦名稱
    Protected Function PcName() As String
        Dim ip As String = HttpContext.Current.Request.UserHostAddress
        Dim hostinfo As System.Net.IPHostEntry = System.Net.Dns.GetHostEntry(ip)
        Dim name As String = hostinfo.HostName
        Dim str() As String
        str = Split(name, ".")
        Return str(0)
    End Function
    '匯入按鈕
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim HospCode = Request.QueryString("HospCode")
        'HospCode = HospCode.PadLeft(2, "0")
        Dim WardCode = Request.QueryString("WardCode")
        'WardCode = WardCode.PadLeft(3, "0")
        Dim ComName As String = PcName()
        Dim UOPER As String = "XXXX"

        If (FileUpload1.HasFile) Then
            Dim FileName As String = FileUpload1.FileName
            Dim FileExtension As String = System.IO.Path.GetExtension(FileName)
            Dim NewFileName = String.Format("{0}{1}", HospCode & WardCode, FileExtension)

            Label1.Text = "上傳成功!!"

            ' Dim WorkBook As XSSFWorkbook = New XSSFWorkbook(FileUpload1.FileContent)
            Dim WorkBook As IWorkbook = WorkbookFactory.Create(FileUpload1.FileContent)
            Dim SCHED_sheet As ISheet = WorkBook.GetSheetAt(0) '主治與住院醫師配對
            Dim DUTY_sheet As ISheet = WorkBook.GetSheetAt(1) '值班醫師名單
            Dim SUBT_sheet As ISheet = WorkBook.GetSheetAt(2) '代班醫師表
            'Dim INTERN_sheet As XSSFSheet = WorkBook.GetSheetAt(2) '實習醫師名單

            '上傳實習醫師的資料
            'INTERN_INSERT(INTERN_sheet, HospCode, WardCode, ComName, UOPER) '輸入實習醫師的資料
            SCHED_INSERT(SCHED_sheet, HospCode, WardCode, ComName, UOPER) '輸入主治醫師和住院醫師的配對資料
            DUTY_INSERT(DUTY_sheet, HospCode, WardCode, ComName, UOPER)   '輸入值班師資料
            SUBT_INSERT(SUBT_sheet, HospCode, WardCode, ComName, UOPER) ' 上傳代班醫師表
        Else
            Label1.Text = "上傳失敗!!"
        End If
    End Sub
    '主程式
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim Empno As String = Request.QueryString("Empno")
        Dim ConnStr As String = SELECT_ORACLE(HospCode)
        '超連結設定
        'Hyperlink_Default(HospCode, WardCode)
        '顯示病房名稱, 查詢時間, 操作者
        Ward_Name_Label.Text = Title_default(HospCode, WardCode, ConnStr)
        Update_Time_Label.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        User_Label.Text = Show_User()


    End Sub

    '################################作廢############################################
    '匯入實習醫生值班資料
    Public Function INTERN_INSERT(ByVal INTERN_sheet As XSSFSheet, ByVal HospCode As String, ByVal WardCode As String, ByVal ComName As String, ByVal UOPER As String)

        Dim Conn As OleDbConnection = New OleDbConnection
        Dim Connstr As String = SELECT_ORACLE(HospCode)
        Conn.ConnectionString = Connstr
        Conn.Open()
        For i As Integer = INTERN_sheet.FirstRowNum + 1 To INTERN_sheet.LastRowNum
            Dim INTERN_ROW As XSSFRow = INTERN_sheet.GetRow(i)
            Dim BEGIN_DATE As Date = INTERN_ROW.GetCell(0).DateCellValue
            Dim END_DATE As Date = INTERN_ROW.GetCell(1).DateCellValue
            Dim NS As String = INTERN_ROW.GetCell(2).ToString
            Dim Full_NS As String = NS.PadLeft(3, "0")
            Dim INTERN_NAME As String = INTERN_ROW.GetCell(3).ToString
            Dim INTERN_EMPNO As String = INTERN_ROW.GetCell(4).ToString
            Dim BED_LIST As ArrayList = New ArrayList
            '將病床資料輸入BED_LIST並排序
            For j = 5 To INTERN_ROW.LastCellNum - 1
                BED_LIST.Add(INTERN_ROW.GetCell(j).ToString)
            Next
            BED_LIST.Sort()

            If END_DATE >= BEGIN_DATE Then
                Dim Moment As Integer = DateDiff(DateInterval.Day, BEGIN_DATE, END_DATE)
                For t = 0 To Moment
                    Dim Temp_Date As Date = BEGIN_DATE.AddDays(t)
                    Dim Temp_Date_Str As String = String.Format("{0}/{1}/{2} 00:00:00", Temp_Date.Year(), Temp_Date.Month(), Temp_Date.Day())
                    Dim INTERN_ID As String = Full_NS & Right(Temp_Date.ToString("yyyy"), 3) & Temp_Date.ToString("MM") & Temp_Date.ToString("dd") & INTERN_EMPNO
                    '輸入實習醫師單日值班資料
                    Dim Delete_Intern_CMD As String = String.Format("DELETE  FROM WHTINT WHERE NS = '{0}' AND DUTY_DATE = '{1}' AND Intern_EMPNO = '{2}' ", NS, Temp_Date_Str, INTERN_EMPNO)
                    Dim Delete_Intern_DC As OleDbCommand = New OleDbCommand(Delete_Intern_CMD, Conn)

                    Dim Delete_Intern_Bed_Cmd As String = String.Format("DELETE  FROM WHTINTB WHERE NS = '{0}' AND DUTY_DATE = '{1}' AND Intern_EMPNO = '{2}' ", NS, Temp_Date_Str, INTERN_EMPNO)
                    Dim Delete_Intern_Bed_DC As OleDbCommand = New OleDbCommand(Delete_Intern_Bed_Cmd, Conn)

                    Dim Insert_Intern_CMD As String = String.Format("INSERT INTO WHTINT ( Duty_date, Ns, Intern_Empno, Intern_name, Udate,TERMID,UOPER) VALUES (to_date('{0}','YYYY/MM/DD HH24:MI:SS'),'{1}', '{2}', '{3}', to_date(sysdate,'YYYY/MM/DD HH24:MI:SS'),'{4}','{5}')", Temp_Date.ToString("yyyy/MM/dd 00:00:00"), NS, INTERN_EMPNO, INTERN_NAME, ComName, UOPER)
                    Dim Insert_Intern_DC As OleDbCommand = New OleDbCommand(Insert_Intern_CMD, Conn)


                    Delete_Intern_DC.ExecuteNonQuery()
                    Delete_Intern_Bed_DC.ExecuteNonQuery()
                    Insert_Intern_DC.ExecuteNonQuery()
                    For Each BED As String In BED_LIST
                        '輸入病床資料
                        Dim Insert_Intern_Bed_CMD As String = String.Format("INSERT INTO WHTINTB (NS,DUTY_DATE, INTERN_EMPNO, Bed) VALUES ('{0}','{1}','{2}','{3}')", NS, Temp_Date_Str, INTERN_EMPNO, BED)
                        Dim Insert_Intern_Bed_DC As OleDbCommand = New OleDbCommand(Insert_Intern_Bed_CMD, Conn)
                        Insert_Intern_Bed_DC.ExecuteNonQuery()
                    Next
                Next
            End If
        Next

        Conn.Close()
    End Function

End Class
