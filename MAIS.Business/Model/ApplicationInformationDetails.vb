Namespace Model

    Public Class ApplicationInformationDetails
        Public Property Application_SID As Integer
        Public Property ApplicationType_SID As Integer
        Public Property RN_Flg As Boolean
        Public Property ApplicationStatusType_SID As Integer
        Public Property CreateDate As DateTime?
        Public Property CreateBy As Integer
        Public Property LastUpdateDate As DateTime?
        Public Property LastUpdateBy As Integer
        Public Property Signature As String
        Public Property RoleCategoryLevel_SID As Integer       
        Public Property RNDDUnique_Code As String
        Public Property Attestant_SID As Integer?
        Public Property IsAdminFlag As Boolean
    End Class
End Namespace