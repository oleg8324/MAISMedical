'Imports MAIS.Web.Model
Imports MAIS.Business
Imports MAIS.Business.Model

Public Class ProgressBar
    Inherits System.Web.UI.UserControl

    Private _pageName As String
    Private _maisApp As MAIS.Business.Model.MAISApplicationDetails

    Protected Sub ProgressBar_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        pnlProgressBar.Visible = True
    End Sub
    Protected Sub ProgressBar_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        InitializeControl()
    End Sub

    Private Sub InitializeControl()
        Dim previousPagesCompleted As Boolean = True
        Dim pageCompletionRules As New Rules.PageCompletionRules(SessionHelper.ApplicationID, SessionHelper.ExistingUserRole) 'Me.MAISApplication)
        Dim navigationEnabled As Boolean = False
        'IsPartialPersonalInformationPageComplete looks for minimum 5 mandatary fields in database for a person to complete
        If (pageCompletionRules.IsPartialPersonalInformationPageComplete() And pageCompletionRules.IsStartPageComplete()) Then
            navigationEnabled = True
        End If
        'If pageCompletionRules.IsStartPageComplete() Then
        '    navigationEnabled = True
        'End If
        Dim pageList As List(Of PageModel)

        pageList = PagesHelper.GetPageList()

        For i As Integer = 1 To pageList.Count - 1
            Dim tableCell1 As New TableCell()
            Dim tableCell2 As New TableCell()
            Dim cellStyle1 As New TableItemStyle()
            Dim cellStyle2 As New TableItemStyle()

            With cellStyle1
                .BorderStyle = BorderStyle.Solid
                .BorderColor = Drawing.Color.Black
                .BorderWidth = System.Web.UI.WebControls.Unit.Pixel(1)
                .Width = System.Web.UI.WebControls.Unit.Pixel(80)
                .Height = System.Web.UI.WebControls.Unit.Pixel(7)
                .Font.Size = 7

                If Me.PageName.ToLower().Equals(pageList(i).PageAddress.ToLower()) Then
                    .BackColor = Drawing.Color.LightBlue
                Else
                    .BackColor = Drawing.Color.White
                    End If
            End With

            With cellStyle2
                .BorderStyle = BorderStyle.None
                .BorderWidth = System.Web.UI.WebControls.Unit.Pixel(0)
                .Width = System.Web.UI.WebControls.Unit.Pixel(45)
                .Height = System.Web.UI.WebControls.Unit.Pixel(7)
                .BackColor = Drawing.Color.White
            End With

            tableCell1.Text = pageList(i).PageName
            tableCell1.ApplyStyle(cellStyle1)
            tblProgBar.Rows(0).Cells.Add(tableCell1)

            tableCell2.ApplyStyle(cellStyle2)
            tblProgBar.Rows(1).Cells.Add(tableCell2)

            Dim currentPageCompleted As Boolean = False

            Select Case pageList(i).PageAddress
                Case "StartPage.aspx"
                    currentPageCompleted = pageCompletionRules.IsStartPageComplete()
                Case "PersonalInformation.aspx"
                    currentPageCompleted = pageCompletionRules.IsPersonalInformationPageComplete()
                Case "EmployerInformation.aspx"
                    currentPageCompleted = pageCompletionRules.IsEmployerInformationPageComplete()
                Case "WorkExperience.aspx"
                    currentPageCompleted = pageCompletionRules.IsWorkExperienceInformationPageComplete()
                Case "TrainingSkills.aspx"
                    currentPageCompleted = pageCompletionRules.IsTrainingPageComplete()
                Case "Skills.aspx"
                    currentPageCompleted = pageCompletionRules.IsSkillsPageComplete()
                Case "DocumentUpload.aspx"
                    currentPageCompleted = pageCompletionRules.IsDocumentUploadPageComplete()
                Case "RNAttestation.aspx"
                    currentPageCompleted = pageCompletionRules.IsAttestationPageComplete()
                Case "Summary.aspx"
                    currentPageCompleted = pageCompletionRules.IsSummaryPageComplete()
                Case "ViewCertificate.aspx"
                    currentPageCompleted = False
                Case "Notation.aspx"
                    currentPageCompleted = False
            End Select

            With tblProgBar.Rows(1).Cells(i - 1)
                If Not pageList(i).PageAddress.Equals("ViewCertificate.aspx") And Not pageList(i).PageAddress.Equals("Notation.aspx") Then '.FinalConfirmationPage) Then
                    previousPagesCompleted = previousPagesCompleted And currentPageCompleted

                    If currentPageCompleted Then
                        .Text = "<img src=""images/rightArrowCompleted.gif"" />"
                    Else
                        .Text = "<img src=""images/rightArrowPending.gif"" />"
                    End If

                    If navigationEnabled Then
                        .Attributes.Add("onclick", "window.location='" & pageList(i).PageAddress & "'")
                    End If
                Else
                    If (pageList(i).PageAddress.Equals("ViewCertificate.aspx")) Then
                        If currentPageCompleted Then
                            .Text = <b><font style="color:Grey; font-size:28pt">FINISH</font></b>
                        Else
                            .Text = <b><font style="color:Blue; font-size:28pt">FINISH</font></b>
                        End If

                        If navigationEnabled Then
                            If previousPagesCompleted Then
                                .Attributes.Add("onclick", "window.location='" & pageList(i).PageAddress & "'")
                            Else
                                .Attributes.Add("onclick", "window.location='#'")
                            End If
                        End If
                    Else
                        .Text = <b><font style="color:Blue; font-size:28pt">NOTATION</font></b>
                        .Attributes.Add("onclick", "window.location='" & pageList(i).PageAddress & "'")
                    End If
                End If
            End With
        Next
    End Sub

    Public Property MAISApplication As Business.Model.MAISApplicationDetails
        Private Get
            Return _maisApp
        End Get
        Set(ByVal value As Business.Model.MAISApplicationDetails)
            _maisApp = value
        End Set
    End Property

    Public Property PageName As String
        Private Get
            Return _pageName
        End Get
        Set(ByVal value As String)
            _pageName = value
        End Set
    End Property
End Class