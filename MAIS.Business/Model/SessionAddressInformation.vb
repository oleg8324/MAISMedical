Namespace Model
    Public Class SessionAddressInformation
        Public Property Session_Sid As Integer
        Public Property Course_SID As Integer
        Public Property Session_Start_Date As Date
        Public Property Session_End_Date As Date
        Public Property Sponsor As String
        Public Property Location_Name As String
        Public Property Total_CEs As Double
        Public Property Public_Access_Flg As Boolean
        Public Property AttendeeCount As Integer
        Public Property SessionAddressInfo As SessionAddress
        Public Property SessionInformationDetailsList As List(Of SessionInformationDetails)
        Public ReadOnly Property PublicAccessYesNo As String
            Get
                If Public_Access_Flg = True Then
                    Return "Yes"
                Else
                    Return "No"
                End If
            End Get
        End Property
        Public ReadOnly Property AllowSessionDelete As Boolean
            Get
                If AttendeeCount > 0 Then
                    Return False
                Else
                    Return True
                End If
            End Get
        End Property
    End Class
End Namespace

