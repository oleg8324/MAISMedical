Public Class AdminEditCourse
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Master.HideLink = True
        Master.HideProgressBar = True
    End Sub
    Protected Sub ServerValidate(source As Object, args As ServerValidateEventArgs)
        args.IsValid = True

    End Sub
End Class