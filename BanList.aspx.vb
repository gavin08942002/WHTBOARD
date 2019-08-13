Imports System
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.DynamicData
Imports System.Data.OleDb
Imports System.IO

Imports NPOI
Imports NPOI.XSSF.UserModel
Imports NPOI.POIFS
Imports NPOI.POIFS.FileSystem
Imports NPOI.Util

Partial Class UserControl
    Inherits System.Web.UI.Page

    Sub hyperlink()
        Dim uri As New Uri(Request.Url.AbsoluteUri)
        Dim collection As NameValueCollection = HttpUtility.ParseQueryString(uri.Query, System.Text.Encoding.UTF8)
        HyperLink1.NavigateUrl = String.Format("OnDutyDoctor.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
        HyperLink2.NavigateUrl = String.Format("EmContact.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
        HyperLink3.NavigateUrl = String.Format("BanList.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
        HyperLink4.NavigateUrl = String.Format("EditBoard.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        hyperlink() '超連結參數重帶

    End Sub
    '匯入實習醫生值班資料
    Public Function INTERN_INSERT(ByVal INTERN_sheet As XSSFSheet, ByVal HospCode As String, ByVal WardCode As String)

        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = "Provider=OraOLEDB.Oracle;user id=dr;data source=hcora2; persist security info=true;password=mmhedp"
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
            



            If END_DATE > BEGIN_DATE Then
                Dim Moment As Integer = DateDiff(DateInterval.Day, BEGIN_DATE, END_DATE)
                For t = 0 To Moment
                    Dim Temp_Date As Date = BEGIN_DATE.AddDays(t)
                    Dim INTERN_ID As String = Full_NS & Right(Temp_Date.ToString("yyyy"), 3) & Temp_Date.ToString("MM") & Temp_Date.ToString("dd") & INTERN_EMPNO
                    '輸入實習醫師單日值班資料
                    Dim Delete_Intern_CMD As String = String.Format("DELETE  FROM WHTBOARD_INTERN WHERE Intern_ID = '{0}'", INTERN_ID)
                    Dim Delete_Intern_DC As OleDbCommand = New OleDbCommand(Delete_Intern_CMD, Conn)

                    Dim Delete_Intern_Bed_Cmd As String = String.Format("DELETE  FROM WHTBOARD_INTERN_Bed WHERE Intern_ID = '{0}'", INTERN_ID)
                    Dim Delete_Intern_Bed_DC As OleDbCommand = New OleDbCommand(Delete_Intern_Bed_Cmd, Conn)

                    Dim Insert_Intern_CMD As String = String.Format("INSERT INTO WHTBOARD_INTERN (Intern_id, Duty_date, Ns, Intern_Empno, Intern_name, Udate) VALUES ('{0}', to_date('{1}','YYYY/MM/DD HH24:MI:SS'),'{2}', '{3}', '{4}', to_date(sysdate,'YYYY/MM/DD HH24:MI:SS'))", INTERN_ID, Temp_Date.ToString("yyyy/MM/dd 00:00:00"), NS, INTERN_EMPNO, INTERN_NAME)
                    Dim Insert_Intern_DC As OleDbCommand = New OleDbCommand(Insert_Intern_CMD, Conn)


                    Delete_Intern_DC.ExecuteNonQuery()
                    Delete_Intern_Bed_DC.ExecuteNonQuery()
                    Insert_Intern_DC.ExecuteNonQuery()
                    'Response.Write(INTERN_ID)
                    For Each BED As String In BED_LIST
                        '輸入病床資料
                        Dim Insert_Intern_Bed_CMD As String = String.Format("INSERT INTO WHTBOARD_INTERN_BED (Intern_id, Bed) VALUES ('{0}', '{1}')", INTERN_ID, BED)
                        Dim Insert_Intern_Bed_DC As OleDbCommand = New OleDbCommand(Insert_Intern_Bed_CMD, Conn)
                        Insert_Intern_Bed_DC.ExecuteNonQuery()
                    Next

                Next
            End If


        Next

        Conn.Close()
    End Function
    '匯入主治醫師和住院醫師配對表
    Public Function SCHED_INSERT(ByVal SCHED_Sheet As XSSFSheet, ByVal HospCode As String, ByVal WardCode As String)
        'INSERT 主治與住院醫師配對資料至資料庫==============
        For i As Integer = (SCHED_Sheet.FirstRowNum + 1) To SCHED_Sheet.LastRowNum


            Dim SCHED_ROW As XSSFRow = SCHED_Sheet.GetRow(i)
            If SCHED_ROW.GetCell(0).ToString Is String.Empty Or SCHED_ROW.GetCell(1).ToString Is String.Empty Or SCHED_ROW.GetCell(3).ToString Is String.Empty Then
                Response.Write(String.Format("第{0}行資料有誤,請重新檢查!!", i))
            Else

                '宣告所需要的字串
                Dim BDATE As DateTime = Convert.ToDateTime(SCHED_ROW.GetCell(0).ToString)
                Dim EDATE As DateTime = Convert.ToDateTime(SCHED_ROW.GetCell(1).ToString)
                Dim NS As String = SCHED_ROW.GetCell(2).ToString
                Dim AD_EMPNO As String = SCHED_ROW.GetCell(3).ToString
                Dim AD_NAME As String = SCHED_ROW.GetCell(4).ToString
                Dim HD_EMPNO As String = SCHED_ROW.GetCell(5).ToString
                Dim HD_NAME As String = SCHED_ROW.GetCell(6).ToString

                Dim Conn As OleDbConnection = New OleDbConnection
                Conn.ConnectionString = "Provider=OraOLEDB.Oracle;user id=dr;data source=hcora2; persist security info=true;password=mmhedp"
                Conn.Open()



                Dim Moment As Integer = DateDiff(DateInterval.Day, BDATE, EDATE)
                For t = 0 To Moment
                    Dim Temp_date As Date = BDATE.AddDays(t)
                    Dim SCHED_ID As String = NS & Right(Temp_date.ToString("yyyy"), 3) & Temp_date.ToString("MM") & Temp_date.ToString("dd") & AD_EMPNO

                    '將資料插入資料庫===============
                    Dim DeleteCmd As String = String.Format("DELETE  FROM WHTBOARD_SCHED WHERE SCHED_ID = '{0}'", SCHED_ID)
                    Dim DeleteDC As OleDbCommand = New OleDbCommand(DeleteCmd, Conn)

                    Dim InsertCmd As String = String.Format("INSERT INTO WHTBOARD_SCHED(SCHED_ID, Bdate, Edate, NS, AD_EMPNO, AD_NAME, HD_EMPNO, HD_NAME, UDATE) VALUES ('{0}',to_date('{1}','YYYY/MM/DD HH24:MI:SS'),to_date('{2}','YYYY/MM/DD HH24:MI:SS'),'{3}','{4}','{5}','{6}','{7}',to_date(sysdate,'YYYY/MM/DD HH24:MI:SS'))", SCHED_ID, Temp_date.ToString("yyyy/MM/dd 00:00:00"), EDATE.ToString("yyyy/MM/dd 23:59:59"), NS, AD_EMPNO, AD_NAME, HD_EMPNO, HD_NAME)
                    'Response.Write(String.Format("SCHED=={0},開始時間:{1},結束時間{2},{3},{4},{5},{6}", SCHED_ID, Temp_date, EDATE, NS, AD_EMPNO, AD_NAME, HD_EMPNO, HD_NAME))
                    Dim InsertDC As OleDbCommand = New OleDbCommand(InsertCmd, Conn)
                    DeleteDC.ExecuteNonQuery()
                    InsertDC.ExecuteNonQuery()
                    '===============================
                Next

                'Response.Write("<BR>")
                Conn.Close()
            End If
        Next
        '===================================================
    End Function
    '匯入醫師值班表
    Public Function DUTY_INSERT(ByVal DUTY_Sheet As XSSFSheet, ByVal HospCode As String, ByVal WardCode As String)
        'INSERT值班醫師名單
        For k As Integer = (DUTY_Sheet.FirstRowNum + 1) To DUTY_Sheet.LastRowNum
            Dim DUTY_ROW As XSSFRow = DUTY_Sheet.GetRow(k)
            '宣告所需要的字串
            Dim DUTY_DATE As DateTime = Convert.ToDateTime(DUTY_ROW.GetCell(0).ToString)
            Dim NS As String = DUTY_ROW.GetCell(1).ToString
            Dim MRD_EMPNO1 As String = DUTY_ROW.GetCell(2).ToString
            Dim MRD_NAME1 As String = DUTY_ROW.GetCell(3).ToString
            Dim MRD_EMPNO2 As String = DUTY_ROW.GetCell(4).ToString
            Dim MRD_NAME2 As String = DUTY_ROW.GetCell(5).ToString
            Dim R_EMPNO1 As String = DUTY_ROW.GetCell(6).ToString
            Dim R_NAME1 As String = DUTY_ROW.GetCell(7).ToString
            Dim R_EMPNO2 As String = DUTY_ROW.GetCell(8).ToString
            Dim R_NAME2 As String = DUTY_ROW.GetCell(9).ToString
            Dim DUTY_ID As String = NS.PadLeft(3, "0") & Right(DUTY_DATE.ToString("yyyy"), 3) & DUTY_DATE.ToString("MM") & DUTY_DATE.ToString("dd")

            'Response.Write(String.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10}", DUTY_ID, DUTY_DATE, NS, MRD_EMPNO1, MRD_NAME1, MRD_EMPNO2, MRD_NAME2, R_EMPNO1, R_NAME1, R_EMPNO2, R_NAME2))
            Dim Conn As OleDbConnection = New OleDbConnection
            Conn.ConnectionString = "Provider=OraOLEDB.Oracle;user id=dr;data source=hcora2; persist security info=true;password=mmhedp"
            Conn.Open()
            '清除重複資料
            Dim DeleteCmd As String = String.Format("DELETE FROM WHTBOARD_DUTY WHERE DUTY_ID = '{0}'", DUTY_ID)
            Dim DeleteDC As OleDbCommand = New OleDbCommand(DeleteCmd, Conn)
            DeleteDC.ExecuteNonQuery()

            '存入資料
            Dim INSERTCmd As String = String.Format(" INSERT INTO WHTBOARD_DUTY(DUTY_ID, DUTY_DATE, NS, MRD_EMPNO1, MRD_NAME1, MRD_EMPNO2, MRD_NAME2, R_EMPNO1, R_NAME1, R_EMPNO2, R_NAME2, UDATE) VALUES ('{0}',to_date('{1}','YYYY/MM/DD HH24:MI:SS'),'{2}', '{3}' , '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', '{10}', to_date(sysdate,'YYYY/MM/DD HH24:MI:SS'))", DUTY_ID, DUTY_DATE.ToString("yyyy/MM/dd 00:00:00"), NS, MRD_EMPNO1, MRD_NAME1, MRD_EMPNO2, MRD_NAME2, R_EMPNO1, R_NAME1, R_EMPNO2, R_NAME2)
            Dim INSERTDC As OleDbCommand = New OleDbCommand(INSERTCmd, Conn)
            INSERTDC.ExecuteNonQuery()


            'Response.Write("<Br>")
            Conn.Close()
        Next
    End Function
    '匯入按鈕
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim HospCode = Request.QueryString("HospCode")
        HospCode = HospCode.PadLeft(2, "0")
        Dim WardCode = Request.QueryString("WardCode")
        WardCode = WardCode.PadLeft(3, "0")

        If (FileUpload1.HasFile) Then
            Dim FileName As String = FileUpload1.FileName
            Dim FileExtension As String = System.IO.Path.GetExtension(FileName)
            Dim NewFileName = String.Format("{0}{1}", HospCode & WardCode, FileExtension)

            Label1.Text = "上傳成功!!"

            Dim WorkBook As XSSFWorkbook = New XSSFWorkbook(FileUpload1.FileContent)
            Dim SCHED_sheet As XSSFSheet = WorkBook.GetSheetAt(0) '主治與住院醫師配對
            Dim DUTY_sheet As XSSFSheet = WorkBook.GetSheetAt(1) '值班醫師名單
            Dim INTERN_sheet As XSSFSheet = WorkBook.GetSheetAt(2) '實習醫師名單

            '上傳實習醫師的資料
            INTERN_INSERT(INTERN_sheet, HospCode, WardCode) '輸入實習醫師的資料
            SCHED_INSERT(SCHED_sheet, HospCode, WardCode) '輸入主治醫師和住院醫師的配對資料
            DUTY_INSERT(DUTY_sheet, HospCode, WardCode)   '輸入值班師資料
        Else
            Label1.Text = "上傳失敗!!"
        End If
    End Sub

    '刪除主治醫師和住院醫師配對資料
    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = "Provider=OraOLEDB.Oracle;user id=dr;data source=hcora2; persist security info=true;password=mmhedp"
        Conn.Open()
        Dim cmd As String = String.Format("DELETE  FROM WHTBOARD_SCHED WHERE SCHED_ID='{0}'", DeleteTextBox.Text)
        Dim DC As OleDbCommand = New OleDbCommand(cmd, Conn)
        DC.ExecuteNonQuery()
    End Sub
    '刪除醫師值班資料
    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim Conn As OleDbConnection = New OleDbConnection
        Conn.ConnectionString = "Provider=OraOLEDB.Oracle;user id=dr;data source=hcora2; persist security info=true;password=mmhedp"
        Conn.Open()
        Dim cmd As String = String.Format("DELETE  FROM WHTBOARD_DUTY WHERE DUTY_ID='{0}'", TextBox1.Text)
        Dim DC As OleDbCommand = New OleDbCommand(cmd, Conn)
        DC.ExecuteNonQuery()
    End Sub
End Class
