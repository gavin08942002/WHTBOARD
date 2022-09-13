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
Partial Class Web_Default
    Inherits System.Web.UI.Page
    Public Function SELECT_ORACLE(ByVal str As String) As String
        Dim OraStr As String = Nothing

        Select Case str
            Case "1"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=tp_opd_ord; persist security info=true;password=his1160"
            Case "2"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=ts_opd_ord; persist security info=true;password=his1160"
            Case "3"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=tt_opd_ord; persist security info=true;password=his1160"
            Case "4"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=hc_opd_ord; persist security info=true;password=his1160"
            Case "5"
                OraStr = "Provider=OraOLEDB.Oracle;user id=his;data source=cc_opd_ord; persist security info=true;password=his1160"
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
        DA.Fill(DS) '連絡人資料
        If DS.Rows.Count > 0 Then
            Return DS.Rows(0)("nseng")
        End If
    End Function
    Protected Function Show_User() As String
        Dim User_Name As String = "Test"
        Return User_Name
    End Function
    Protected Sub Hyperlink_Default(ByVal HospCode As String, ByVal WardCode As String)
        HyperLink2.NavigateUrl = String.Format("Patients.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink3.NavigateUrl = String.Format("Operation.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink4.NavigateUrl = String.Format("Inout.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink5.NavigateUrl = String.Format("Nurses.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink6.NavigateUrl = String.Format("Doctors.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink7.NavigateUrl = String.Format("Contacts.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink8.NavigateUrl = String.Format("Info.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
        HyperLink1.NavigateUrl = String.Format("Index.aspx?HospCode={0}&WardCode={1}", HospCode, WardCode)
    End Sub
    '取得病床資料
    Protected Function Bed_Info(ByVal HospCode As String, ByVal WardCode As String, ByVal ConnStr As String) As ArrayList
        Dim DS As New DataSet
        Dim BedList As ArrayList = New ArrayList
        Dim Conn As OleDbConnection = New OleDbConnection

        Conn.ConnectionString = ConnStr
        Dim cmd As String = String.Format("SELECT *  FROM Bedtbl WHERE bedns='{0}' AND bedroom!='99'  ", WardCode)
        Dim DA1 As OleDbDataAdapter = New OleDbDataAdapter(cmd, Conn)
        DA1.Fill(DS, "BedAll")
        Conn.Close()
        For i = 0 To DS.Tables(0).Rows.Count - 1
            BedList.Add(DS.Tables(0).Rows(i)("BEDNO"))
        Next
        GridView2.DataSource = DS.Tables(0)
        GridView2.DataBind()

        Return BedList
    End Function
    '取得病人資料
    Public Function PAT_Info(ByVal Hospcode As String, ByVal Wardcode As String, ByVal Pno As String) As DataTable
        Dim ws As ServiceReference1.RB_WSSoapClient = New ServiceReference1.RB_WSSoapClient()
        Dim kDataSet As New DataSet
        Dim kDataTable As New DataTable
        kDataSet = ws.Get_NSInfo(Hospcode, Wardcode, Pno)
        kDataSet.Tables(0).DefaultView.Sort = "BEDNO"
        Return kDataSet.Tables(0)
    End Function
    '顯示病床資料
    Public Function SHOW_BED(ByVal PATINFOR As DataRow) As String
        Dim Str As String = Nothing

        Dim BEDNO As String = Right(PATINFOR("BEDNO").ToString, 3)
        Dim ACTIVITY As String = PATINFOR("ACTIVITY").ToString
        Dim PATIENT_NAME As String = PATINFOR("PATIENT_NAME").ToString
        Dim DR As String = GET_DR(PATINFOR("BEDDR1").ToString, PATINFOR("BEDDR2").ToString, PATINFOR("BEDDR3").ToString, PATINFOR("BEDDR4").ToString)
        Dim NurseNAme As String = Nothing '護士姓名


        Str &= String.Format("<div class =""Bed"">")
        Str &= String.Format("        <div class =""Bed_Head"">")
        Str &= String.Format("                 <div style ="" width:20%; height:40px; float:left; font-size:28px; color:black;"">{0}</div>", BEDNO)
        Str &= String.Format("                 <div style ="" width:10%; height:40px; float:left; font-size:28px; color:black;""></div>")
        Str &= String.Format("                 <div style ="" width:70%; height:40px; float:left; font-size:28px; text-align:left; padding-top:5px;"">")
        Str &= String.Format("                         <img src=""image/icon_sex_f.png"" height=""30px""; width=""30px"" />") '性別圖示
        Str &= String.Format("                         <img src=""image/icon_move_1.png"" width=""30px"" height=""30px"" />") '行動能圖示
        Str &= String.Format("                 </div>")
        Str &= String.Format("        </div>")
        Str &= String.Format("        <div class =""Bed_Body"">")
        Str &= String.Format("                <div style =""width:250px; height:40px; "">") '病人姓名
        Str &= String.Format("                        <div class =""Bed_Body_Div1"">姓名：</div>")
        Str &= String.Format("                        <div class =""Bed_Body_Div2"">{0}</div>", PATIENT_NAME)
        Str &= String.Format("                </div>")
        Str &= String.Format("                <div style =""width:250px; height:80px;"">") '主治醫師
        Str &= String.Format("                        <div class =""Bed_Body_Div1"">主治：</div>")
        Str &= String.Format("                        <div class =""Bed_Body_Div2"">{0}</div>", DR)
        Str &= String.Format("               </div>")
        Str &= String.Format("               <div style =""width:250px; height:40px;"">")
        Str &= String.Format("                       <div class =""Bed_Body_Div1"">護理師：</div>")
        Str &= String.Format("                       <div class =""Bed_Body_Div2"">白帶魚</div>")
        Str &= String.Format("              </div>")
        Str &= String.Format("       </div>")
        Str &= String.Format("       <div class =""Bed_footer"">")
        Str &= String.Format("               <img src=""image/icon_rem_1.png"" width=""30px"" height=""30px"" />")
        Str &= String.Format("               <img src=""image/icon_rem_2.png"" width=""30px"" height=""30px"" />")
        Str &= String.Format("               <img src=""image/icon_rem_r.png"" width=""30px"" height=""30px"" />")
        Str &= String.Format("               <img src=""image/icon_rem_st.png"" width=""30px"" height=""30px"" />")
        Str &= String.Format("        </div>")
        Str &= String.Format("</div>")
        Str &= String.Format("")

        Return Str
    End Function
    '取得主治醫師字串資料
    Public Function GET_DR(ByVal DR1 As String, ByVal DR2 As String, ByVal DR3 As String, ByVal DR4 As String) As String
        Dim Str As String = Nothing
        If Not String.IsNullOrEmpty(DR1) Then
            Str &= DR1.Split("/")(1)
        End If
        If Not String.IsNullOrEmpty(DR2) Then
            Dim temp() As String = Nothing
            Str &= "/"
            Str &= DR2.Split("/")(1)
        End If
        If Not String.IsNullOrEmpty(DR3) Then
            Str &= "/"
            Str &= DR3.Split("/")(1)
        End If
        If Not String.IsNullOrEmpty(DR4) Then
            Str &= "/"
            Str &= DR4.Split("/")(1)
        End If

        Return Str

    End Function
    '顯示空床資料
    '主程式
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim HospCode As String = Request.QueryString("Hospcode")
        Dim WardCode As String = Request.QueryString("WardCode")
        Dim Empno As String = Request.QueryString("Empno")
        Dim ConnStr As String = SELECT_ORACLE(HospCode)
        '超連結設定
        Hyperlink_Default(HospCode, WardCode)
        '顯示病房名稱, 查詢時間, 操作者
        Ward_Name_Label.Text = Title_default(HospCode, WardCode, ConnStr)
        Update_Time_Label.Text = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss")
        User_Label.Text = Show_User()
        Dim Show_BED_INFO As String = Nothing

        '取得病床資料
        Dim BEDLIST As ArrayList = Bed_Info(HospCode, WardCode, ConnStr)
        Dim BedCount As Integer = BEDLIST.Count '全部床位數
        '取得病人資料
        Dim PATTABLE As DataTable = PAT_Info(HospCode, WardCode, "*")


        '顯示病床資料
        For i = 0 To BEDLIST.Count - 1
            Dim PATINFOR() As DataRow = PATTABLE.Select(String.Format("BEDNO='{0}'", BEDLIST(i).ToString))
            If PATINFOR.Count > 0 Then
                Show_BED_INFO &= SHOW_BED(PATINFOR(0)) '顯示有病患的病床資料
            Else
                'SHOW_EMPTY_BED(BEDLIST(i).ToString))'顯示空資料
            End If

        Next

        PATLabel.Text = Show_BED_INFO
        GridView1.DataSource = PATTABLE
        GridView1.DataBind()



    End Sub
End Class
