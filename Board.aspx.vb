
Partial Class _Default
    Inherits System.Web.UI.Page
    '讓超連結帶出目前網頁的參數
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
        hyperlink() '超連結參數重帶

    End Sub
End Class
