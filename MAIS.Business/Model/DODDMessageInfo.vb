Namespace Model
    Public Class DODDMessageInfo
        Public Property DODD_Message_SID As Integer
        Public Property Subject As String
        Public Property Description As String
        Public Property Priority As Integer
        Public Property Message_Start_Date As Date
        Public Property Message_End_Date As Date
        Public Property Active_Flag As Boolean
        Public Property CreateBy As Integer
        Public Property CreateByName As String
        Public Property LastUpdateBy As Integer
        Public Property lastUpdatedByName As String
        Public Property RolesList As List(Of DODDMessageInfoMaisRoles)
        Public Property PersonList As List(Of DODDMessageInfoMaisRNDDPerson)
    End Class
End Namespace

