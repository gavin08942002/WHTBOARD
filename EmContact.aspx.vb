
Partial Class EmContact
    Inherits System.Web.UI.Page
    Public Function hyperlink()
        Dim uri As New Uri(Request.Url.AbsoluteUri)
        Dim collection As NameValueCollection = HttpUtility.ParseQueryString(uri.Query, System.Text.Encoding.UTF8)
        HyperLink1.NavigateUrl = String.Format("OnDutyDoctor.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
        HyperLink2.NavigateUrl = String.Format("EmContact.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
        HyperLink3.NavigateUrl = String.Format("BanList.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
        HyperLink4.NavigateUrl = String.Format("EditBoard.aspx?Hospcode={0}&Wardcode={1}", collection.Get("Hospcode"), collection("Wardcode"))
    End Function

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        hyperlink() '超連結參數重帶
        
    End Sub


End Class
