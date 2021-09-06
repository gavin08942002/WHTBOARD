Imports System
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.DynamicData

Partial Class label1

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
        Dim ws As ServiceReference1.RB_WSSoapClient = New ServiceReference1.RB_WSSoapClient()
        Dim kDataSet As New DataSet
        Dim Hospcode As String = "1"
        Dim Wardcode As String = "17"
        Dim Pno As String = "*" '八碼
        Dim BedInfo As String
        Dim SEX As String
        Dim BEDNO As String
        Dim WardAll As Integer = 40   '13F共有四十床
        Dim EmptyBed As Integer = 0
        hyperlink() '超連結參數重帶


        kDataSet = ws.Get_NSInfo(Hospcode, Wardcode, Pno)
        For index = 0 To ((kDataSet.Tables(0).Rows.Count) - 1)
            '資料顯示前置處理
            BEDNO = "" '床號--取後3碼
            BEDNO = Right(kDataSet.Tables(0).Rows(index)("BEDNO"), 3)
            'Response.Write(Right(kDataSet.Tables(0).Rows(index)("BEDNO"), 5) & ",")
            SEX = "" '性別
            Select Case kDataSet.Tables(0).Rows(index)("SEX")
                Case "F"
                    SEX = "女"
                Case "M"
                    SEX = "男"
                Case Else
            End Select



            BedInfo = "" '清空病床資料,有抓到資料時放入資料
            BedInfo &= String.Format("<td style=""vertical-align:middle;"">")
            BedInfo &= String.Format("   <div style=""width:100px; height:72px; "">")
            BedInfo &= String.Format("          <div style=""width:100px;height:48px;background-color:{0};border-radius:10px;"">", "#5cb85c") '病床外框顏色
            BedInfo &= String.Format("                  <div style=""width:100px;height:24px"">")
            BedInfo &= String.Format("                  <div style=""width:30px;height:24px;float:left;"">{0}</div><!--病床號-->", BEDNO)
            BedInfo &= String.Format("                  <div style=""width:20px;height:24px;float:left;"">{0}</div><!--性別-->", SEX)
            BedInfo &= String.Format("                  <div style=""width:50px;height:24px;float:left;"">{0}</div><!--行動狀態-->", kDataSet.Tables(0).Rows(index)("ACTIVITY"))
            BedInfo &= String.Format("          </div>")
            BedInfo &= String.Format("          <div style=""width:100px;height:24px;color:#fff"">{0}({1})</div><!--姓名、年齡-->", kDataSet.Tables(0).Rows(index)("PATIENT_NAME"), kDataSet.Tables(0).Rows(index)("AGE"))
            BedInfo &= String.Format("          </div>")
            BedInfo &= String.Format("          <div style=""width:100px;height:24px"">")
            BedInfo &= String.Format("                  <div style="" width:25px; height:24px; float:left; color:#f00"">{0}</div>", "交")
            BedInfo &= String.Format("                  <div style="" width:25px; height:24px; float:left; color:#ff6a00"">{0}</div>", "敏")
            BedInfo &= String.Format("          </div>")
            BedInfo &= String.Format("   </div>")
            BedInfo &= String.Format("</td>")

            Select Case kDataSet.Tables(0).Rows(index)("BEDNO")
                '左上病房區
                Case "1311A"
                    label1311A.Text = BedInfo
                Case "1311B"
                    label1311B.Text = BedInfo
                Case "1315A"
                    label1315A.Text = BedInfo
                Case "1315B"
                    label1315B.Text = BedInfo
                Case "1317A"
                    label1317A.Text = BedInfo
                Case "1317B"
                    label1317B.Text = BedInfo
                Case "1319A"
                    label1319A.Text = BedInfo
                Case "1319B"
                    label1319B.Text = BedInfo
                Case "1321A"
                    label1321A.Text = BedInfo
                Case "1321B"
                    label1321B.Text = BedInfo
                Case "1323A"
                    label1323A.Text = BedInfo
                Case "1323B"
                    label1323B.Text = BedInfo
                Case "1325A"
                    label1325A.Text = BedInfo
                Case "1325B"
                    label1325B.Text = BedInfo
                    '右上病房區
                Case "1327A"
                    label1327A.Text = BedInfo
                Case "1327B"
                    label1327B.Text = BedInfo
                Case "1329A"
                    label1329A.Text = BedInfo
                Case "1329B"
                    label1329B.Text = BedInfo
                Case "1331A"
                    label1331A.Text = BedInfo
                Case "1331B"
                    label1331B.Text = BedInfo
                Case "1333A"
                    label1333A.Text = BedInfo
                Case "1335A"
                    label1335A.Text = BedInfo
                    '左下病房區
                Case "1312A"
                    labe11312A.Text = BedInfo
                Case "1312B"
                    label1312B.Text = BedInfo
                Case "1316A"
                    Labe11316A.Text = BedInfo
                Case "1316B"
                    label1316B.Text = BedInfo
                Case "1318A"
                    label1318A.Text = BedInfo
                Case "1318B"
                    label1318B.Text = BedInfo
                Case "1320A"
                    label1320A.Text = BedInfo
                Case "1320B"
                    label1320B.Text = BedInfo
                Case "1322A"
                    label1322A.Text = BedInfo
                Case "1322B"
                    label1322B.Text = BedInfo
                    '右下病房區
                Case "1326A"
                    label1326A.Text = BedInfo
                Case "1326B"
                    label1326B.Text = BedInfo
                Case "1328A"
                    label1328A.Text = BedInfo
                Case "1328B"
                    label1328B.Text = BedInfo
                Case "1330A"
                    label1330A.Text = BedInfo
                Case "1330B"
                    label1330B.Text = BedInfo
                Case "1332A"
                    label1332A.Text = BedInfo
                Case "1332B"
                    label1332B.Text = BedInfo

                Case Else
            End Select
        Next
        EmptyBed = (WardAll - (kDataSet.Tables(0).Rows.Count))
        LabelWardAll.Text = WardAll
        LabelEmptyBed.Text = EmptyBed
        LabelWardRate.Text = FormatPercent(((WardAll - EmptyBed) / WardAll), 1)
        ws.Close()

        Label1.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")

        GridView1.DataSource = kDataSet.Tables(0)
        GridView1.DataBind()

    End Sub



    
End Class
