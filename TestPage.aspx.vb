
Partial Class TestPage
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim uri As New Uri(Request.Url.AbsoluteUri)
        Dim collection As NameValueCollection = HttpUtility.ParseQueryString(uri.Query, System.Text.Encoding.UTF8)

        Response.Write(String.Format("keyandvalue:{0}/{1}", "Hospcode", collection.Get("Hospcode")))


        'Response.Write(Request.Url.Query())

    End Sub
End Class
