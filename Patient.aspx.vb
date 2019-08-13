Imports System
Imports System.Web.Configuration
Imports System.Data
Imports System.Data.SqlClient
Imports System.Web.Services
Imports System.Web.DynamicData
Partial Class Patient
    Inherits System.Web.UI.Page
    '讓網頁帶出目前網頁的參數
    Public Function hyperlink()
        Button1.PostBackUrl = String.Format("Ward.aspx{0}", Request.Url.Query())
        Button2.PostBackUrl = String.Format("Patient.aspx{0}", Request.Url.Query())
        Button3.PostBackUrl = String.Format("Surgery.aspx{0}", Request.Url.Query())
        Button4.PostBackUrl = String.Format("Hospitalize.aspx{0}", Request.Url.Query())
        Button5.PostBackUrl = String.Format("Nurse.aspx{0}", Request.Url.Query())
        Button6.PostBackUrl = String.Format("Doctor.aspx{0}", Request.Url.Query())
        Button7.PostBackUrl = String.Format("Board.aspx{0}", Request.Url.Query())
    End Function
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim ws As ServiceReference1.RB_WSSoapClient = New ServiceReference1.RB_WSSoapClient()
        Dim kDataSet As New DataSet
        Dim kDataTable As New DataTable
        Dim kView As New DataView
        Dim Hospcode As String = "4"

        Dim Wardcode As String = "17"
        Dim Pno As String = "*" '八碼
        Dim BedInfo As String = ""
        Dim SEX As String
        Dim BEDNO As String
        Dim WardAll As Integer = 40   '13F共有四十床
        Dim EmptyBed As Integer = 0
        hyperlink() '超連結參數重帶


        kDataSet = ws.Get_NSInfo(Hospcode, Wardcode, Pno)
        kDataSet.Tables(0).DefaultView.Sort = "BEDNO desc"
       

        For index = 0 To ((kDataSet.Tables(0).Rows.Count) - 1)
            '資料顯示前置處理
            '床號顯示
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
            '主治醫師資料顯示
            Dim BEDDR1 As String
            Dim BEDDR2 As String
            Dim BEDDR3 As String
            Dim BEDDR4 As String

            BEDDR1 = Mid(kDataSet.Tables(0).Rows(index)("BEDDR1").ToString, 6)
            If Not IsDBNull(kDataSet.Tables(0).Rows(index)("BEDDR2")) Then
                BEDDR2 = Mid(kDataSet.Tables(0).Rows(index)("BEDDR2").ToString, 6)
            End If
            If Not IsDBNull(kDataSet.Tables(0).Rows(index)("BEDDR3")) Then
                BEDDR3 = Mid(kDataSet.Tables(0).Rows(index)("BEDDR3").ToString, 6)
            End If
            If Not IsDBNull(kDataSet.Tables(0).Rows(index)("BEDDR4")) Then
                BEDDR4 = Mid(kDataSet.Tables(0).Rows(index)("BEDDR4").ToString, 6)
            End If
            BedInfo &= String.Format("<div class=""Patient_sub"">")
            BedInfo &= String.Format("     <table style=""width: 100%;"">")
            BedInfo &= String.Format("           <tr style=""width:100%; height:26px; background-color:{0};""  >", "#5cb85c") '標頭顏色
            BedInfo &= String.Format("                 <td style=""width:56px; height:26px; border-top-left-radius:15px;"">{0}</td>", SEX)
            BedInfo &= String.Format("                 <td style=""width:120px; height:26px;"">{0}</td>", BEDNO)
            BedInfo &= String.Format("                 <td style=""width:20px; height:26px;text-align:left; border-top-right-radius:15px;""><span style=""color:#ffd800; font-weight:bold;"">交</span></td>")
            BedInfo &= String.Format("           </tr>")
            BedInfo &= String.Format("           <tr style=""width:100%; height:26px;""  >")
            BedInfo &= String.Format("                 <td style=""width:56px; height:26px"">姓名：</td>")
            BedInfo &= String.Format("                 <td style=""width:120px; height:26px;"">{0}</td>", kDataSet.Tables(0).Rows(index)("PATIENT_NAME"))
            BedInfo &= String.Format("                 <td style=""width:20px; height:26px;""><span style=""color:#f00;font-weight:bold;"">敏</span></td>")
            BedInfo &= String.Format("           </tr>")
            BedInfo &= String.Format("           <tr style=""width:100%; height:52px;"">")
            BedInfo &= String.Format("                 <td style=""width:56px; height:52px;vertical-align:top"">主治：</td>")
            BedInfo &= String.Format("                 <td style=""width:120px; height:52px;"">")
            BedInfo &= String.Format("                       <div style=""width:120px;height:52px;"">")
            BedInfo &= String.Format("                           <div style=""width:60px; height:26px; float:left"">{0}</div>", BEDDR1)
            BedInfo &= String.Format("                           <div style=""width:60px; height:26px; float:left"">{0}</div>", BEDDR2)
            BedInfo &= String.Format("                           <div style=""width:60px; height:26px; float:left"">{0}</div>", BEDDR3)
            BedInfo &= String.Format("                           <div style=""width:60px; height:26px; float:left"">{0}</div>", BEDDR4)
            BedInfo &= String.Format("                       </div>")
            BedInfo &= String.Format("                 </td>")
            BedInfo &= String.Format("           </tr>")
            BedInfo &= String.Format("           <tr style=""width:100%; height:26px;""  >")
            BedInfo &= String.Format("               <td style=""width:56px; height:26px;border-bottom-left-radius:15px;"">護理師</td>")
            BedInfo &= String.Format("               <td style=""width:120px; height:26px;"">黃罔市</td>")
            BedInfo &= String.Format("               <td style=""width:20px; height:26px;""></td>")
            BedInfo &= String.Format("           </tr>")
            BedInfo &= String.Format("      </table>")
            BedInfo &= String.Format("</div>")

        Next
        LabelWard.Text = BedInfo
        GridView1.DataSource = kDataSet.Tables(0)
        GridView1.DataBind()
        ws.Close()
    End Sub
End Class
