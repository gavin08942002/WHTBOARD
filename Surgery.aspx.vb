Imports System
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.DynamicData
Partial Class _Default
    Inherits System.Web.UI.Page
    '讓網頁帶出目前網頁的參數
    Public Function hyperlink()
        Dim uri As New Uri(Request.Url.AbsoluteUri)
        Dim collection As NameValueCollection = HttpUtility.ParseQueryString(uri.Query, System.Text.Encoding.UTF8)
        Button1.PostBackUrl = String.Format("Ward.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
        Button2.PostBackUrl = String.Format("Patient.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
        Button3.PostBackUrl = String.Format("Surgery.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
        Button4.PostBackUrl = String.Format("Hospitalize.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
        Button5.PostBackUrl = String.Format("Nurse.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
        Button6.PostBackUrl = String.Format("Doctor.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
        Button7.PostBackUrl = String.Format("Board.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
    End Function
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim ws1 As ServiceReference1.RB_WSSoapClient = New ServiceReference1.RB_WSSoapClient()
        Dim kDaset As DataSet = New DataSet() '手術排程資料
        Dim kDaset2 As DataSet = New DataSet()
        Dim Hospcode As String = "1"  '醫院代碼
        Dim Wardcode As String = "17" '病房代碼
        Dim BedIDSe As String = "*"  '病床代號
        Dim Op As String
        Dim DateF As DateTime = Date.Now.ToString()
        Dim DateTo As DateTime = Date.Now.AddMonths(1)
        Dim Exam As String
        Dim test As String '測試
        hyperlink() '超連結參數重帶

        Label1.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") '目前時間顯示

        kDaset = ws1.Get_OPInfo(Hospcode, Wardcode, BedIDSe, DateF, DateTo)
        'Label4.Text = "資料筆數為:" & kDaset.Tables(0).Rows.Count & "資料欄位數為；" & kDaset.Tables(0).Columns.Count '測試用
        '列出所有欄位名稱
        'For index = 0 To ((kDaset.Tables(0).Columns.Count) - 1)
        'test &= kDaset.Tables(0).Columns(index).ColumnName & ","
        'Next
        'Response.Write(test)



        Dim SCH_OPAREA As String
        Dim SCH_OPFLOOR As String
        Dim SCH_ACTFG As String

        For index = 0 To ((kDaset.Tables(0).Rows.Count) - 1) '手術排程資料輸出區
            '手術院區
            SCH_OPAREA = "" '清空變數
            Select Case kDaset.Tables(0).Rows(index)("SCH_OPAREA")
                Case "1"
                    SCH_OPAREA = "台北"
                Case "2"
                    SCH_OPAREA = "淡水"
                Case Else
            End Select

            '手術地點
            SCH_OPFLOOR = "" '清空變數
            Select Case kDaset.Tables(0).Rows(index)("SCH_OPFLOOR")
                Case "M"
                    SCH_OPFLOOR = "手術室七樓"
                Case "TM"
                    SCH_OPFLOOR = "手術室二樓"
                Case "F1"
                    SCH_OPFLOOR = "手術室一樓"
                Case Else
            End Select
            '結合手術院區和院區
            SCH_OPAREA &= SCH_OPFLOOR


            '手術狀態
            SCH_ACTFG = "" '清空變數
            Select Case kDaset.Tables(0).Rows(index)("SCH_ACTFG")
                Case "1"
                    SCH_ACTFG = "未報到"
                Case "2"
                    SCH_ACTFG = "報到"
                Case "3"
                    SCH_ACTFG = "手術中"
                Case "4"
                    SCH_ACTFG = "完成手術"
                Case "5"
                    SCH_ACTFG = "取消排程"
                Case "6"
                    SCH_ACTFG = "取消報到"
                Case "7"
                    SCH_ACTFG = "恢復室"
                Case "8"
                    SCH_ACTFG = "己離開"
                Case Else
            End Select


            Label2.Text &= "<tr>"
            Label2.Text &= String.Format("<td style=""width:150px;height:50px;vertical-align:middle;"">{0}</td>", kDaset.Tables(0).Rows(index)("SCH_OPDT"))
            Label2.Text &= String.Format("<td style=""width:150px;height:50px;vertical-align:middle;"">{0}</td>", kDaset.Tables(0).Rows(index)("BEDNO"))
            Label2.Text &= String.Format("<td style=""width:100px;height:50px;vertical-align:middle;"">{0}</td>", kDaset.Tables(0).Rows(index)("PNO"))
            Label2.Text &= String.Format("<td style=""width:200px;height:50px;vertical-align:middle;"">{0}</td>", kDaset.Tables(0).Rows(index)("PATIENT_NAME"))
            Label2.Text &= String.Format("<td style=""width:200px;height:50px;vertical-align:middle;"">{0}</td>", SCH_OPAREA)
            Label2.Text &= String.Format("<td style=""width:450px;height:50px;vertical-align:middle;"">{0}</td>", kDaset.Tables(0).Rows(index)("SCH_SCD2"))
            Label2.Text &= String.Format("<td style=""width:150px;height:50px;vertical-align:middle;"">{0}</td>", (kDaset.Tables(0).Rows(index)("SCH_OPDR")) & (kDaset.Tables(0).Rows(index)("DRNAME")))
            Label2.Text &= String.Format("<td style=""width:332px;height:50px;vertical-align:middle;"">{0}</td>", SCH_ACTFG)
            Label2.Text &= "</tr>"
        Next



        Label3.Text &= "<tr>"
        Label3.Text &= String.Format("<td style=""width:150px;height:50px;vertical-align:middle;"">{0}</td>", "2012")
        Label3.Text &= String.Format("<td style=""width:150px;height:50px;vertical-align:middle;"">{0}</td>", 111)
        Label3.Text &= String.Format("<td style=""width:200px;height:50px;vertical-align:middle;"">{0}</td>", "姓名")
        Label3.Text &= String.Format("<td style=""width:200px;height:50px;vertical-align:middle;"">地點</td>", "銅鑼灣")
        Label3.Text &= String.Format("<td style=""width:600px;height:50px;vertical-align:middle;"">檢查項目</td>", "檢查項目")
        Label3.Text &= String.Format("<td style=""width:432px;height:50px;vertical-align:middle;"">行動能力</td>", "行動能力")
        Label3.Text &= "</tr>"
    End Sub
End Class
