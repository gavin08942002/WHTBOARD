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
        Dim ws2 As ServiceReference2.ServiceSoapClient = New ServiceReference2.ServiceSoapClient()
        Dim kDatable As DataTable = New DataTable()
        Dim kDatable2 As DataTable = New DataTable()
        Dim Hospcode As String = "1"  '醫院代碼
        Dim Wardcode As String = "17" '病房代碼
        Dim OutWard As String
        Dim InWard As String
        Dim PATGENDER As String
        hyperlink() '超連結參數重帶

        kDatable = ws2.GetTrnsList(Hospcode, Wardcode)
        'Label2.Text = "資料筆數為:" & kDatable.Rows.Count & "資料欄位數為；" & kDatable.Columns.Count '測試用
        'Label2.Text = kDatable.Columns(10).ColumnName & "資料" & kDatable.Rows(0)(10)
        '判別性別資料
        For index = 0 To ((kDatable.Rows.Count) - 1)
            PATGENDER = "" '清空變數
            Select Case kDatable.Rows(index)("PATGENDER")
                Case "F"
                    PATGENDER = "女"
                Case "M"
                    PATGENDER = "男"
                Case Else
            End Select
            InWard &= String.Format("<tr>")
            InWard &= String.Format("<td style=""width:150px;height:50px;vertical-align:middle;"">{0}</td>", kDatable.Rows(index)("BedNo"))
            InWard &= String.Format("<td style=""width:200px;height:50px;vertical-align:middle;"">{0}</td>", kDatable.Rows(index)("PatName"))
            InWard &= String.Format("<td style=""width:200px;height:50px;vertical-align:middle;"">{0}</td>", kDatable.Rows(index)("PatPno"))
            InWard &= String.Format("<td style=""width:50px;height:50px;vertical-align:middle;"">{0}</td>", PATGENDER)
            InWard &= String.Format("<td style=""width:50px;height:50px;vertical-align:middle;"">{0}</td>", kDatable.Rows(index)("PATAGE"))
            InWard &= String.Format("<td style=""width:150px;height:50px;vertical-align:middle;"">{0}{1}</td>", kDatable.Rows(index)("VSEMPNO"), kDatable.Rows(index)("VSNAME"))
            InWard &= String.Format("<td style=""width:600px;height:50px;vertical-align:middle;"">{0}</td>", kDatable.Rows(index)("MAINDIAGNOSIS"))
            InWard &= String.Format("<td style=""width:352px;height:50px;vertical-align:middle;"">{0}</td>", kDatable.Rows(index)("REMARK"))
            InWard &= String.Format("</tr>")
        Next
        Label3.Text = InWard '入院患者資料顯示
        Label6.Text = kDatable.Rows.Count '入院患者資料統計

        '出院病患資料處理開始
        kDatable2 = ws2.GetDisList(Hospcode, Wardcode)
        'Label2.Text = "資料筆數為:" & kDatable2.Rows.Count & "資料欄位數為；" & kDatable2.Columns.Count '測試用
        For index = 0 To ((kDatable2.Rows.Count) - 1)
            OutWard &= String.Format("<tr>")
            OutWard &= String.Format("<td style=""width:150px;height:50px;vertical-align:middle;"">{0}</td>", kDatable2.Rows(index)("BedNo"))
            OutWard &= String.Format("<td style=""width:200px;height:50px;vertical-align:middle;"">{0}</td>", kDatable2.Rows(index)("PatName"))
            OutWard &= String.Format("<td style=""width:200px;height:50px;vertical-align:middle;"">{0}</td>", kDatable2.Rows(index)("PatPno"))
            OutWard &= String.Format("<td style=""width:1082px;height:50px;vertical-align:middle;"">{0}</td>", kDatable2.Rows(index)("Remark"))
            OutWard &= String.Format("</tr>")
        Next
        Label4.Text = OutWard '出院患者資料顯示
        Label5.Text = kDatable2.Rows.Count '出院病患統計數

        ws2.Close()



        Label1.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")


    End Sub
End Class
