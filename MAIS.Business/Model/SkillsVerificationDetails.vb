Namespace Model
    Public Class SkillsVerificationDetails
        Public Property Skill_Verification_Sid As Integer
        Public Property RN_DD_Person_Type_Xref_Sid As Integer
        Public Property RN_DD_Person_Type_Xref_SID_string As String
        Public Property Category_Type_Sid As Integer
        Public Property CategoryName As String
        Public Property Application_Sid As Integer?
        Public Property Permanent_Flg As Boolean
        Public Property Skill_Verification_Start_Date As Date
        Public Property Skill_Verification_End_Date As Date
        Public Property Skill_Verification_Active_Flg As Boolean
        Public Property Skill_Verification_Skill_Type_Xref_Sid As Integer
        Public Property Skill_Verification_Skill_Type_Sid As Integer
        Public Property Skill_Verification_Skill_Type As String
        Public Property Skill_Verification_Skill_Type_Active_Flag As Boolean
        Public Property SkillCheckList As List(Of SkillVerificatonTypeCheckListDetails)

        Public ReadOnly Property Verified_Skill_Date As Date
            Get
                Dim vSkillDate As Date? = "1/1/9999"
                If SkillCheckList.Count > 0 Then
                    vSkillDate = SkillCheckList(0).Verification_Date
                End If
                Return vSkillDate
            End Get
        End Property

        Public ReadOnly Property Verified_Person_Title As String
            Get
                Dim Title As String = String.Empty
                If SkillCheckList.Count > 0 Then
                    Title = SkillCheckList(0).Verified_Person_Title
                End If
                Return Title
            End Get
        End Property

        Public ReadOnly Property Verified_Person_Name As String
            Get
                Dim PersonName As String = String.Empty
                If SkillCheckList.Count > 0 Then
                    PersonName = SkillCheckList(0).Verified_Person_Name
                End If
                Return PersonName
            End Get
        End Property
        Public ReadOnly Property SkillListAsString As String
            Get
                Dim mSkillCheckList As String = String.Empty
                For Each skl In Me.SkillCheckList
                    If String.IsNullOrEmpty(mSkillCheckList) Then
                        mSkillCheckList = skl.Skill_CheckList_Name
                    Else
                        mSkillCheckList += ", " + skl.Skill_CheckList_Name
                    End If
                Next
                Return mSkillCheckList
            End Get
        End Property
    End Class

    Public Class SkillVerificatonTypeCheckListDetails
        Public Property Skill_Verification_Type_CheckList_Xref_Sid As Integer
        Public Property Skill_Verification_Skill_Type_Xref_Sid As Integer
        Public Property Skill_CheckList_Sid As Integer
        Public Property Skill_CheckList_Name As String
        Public Property Verification_Date As Date
        Public Property Verified_Person_Name As String
        Public Property Verified_Person_Title As String
        Public Property Active_Flg As Boolean
    End Class

    Public Class SkillVerificationTypeCheckListOnly

        Public Property Skill_Verification_Type_CheckList_Xref_Sid As Integer
        Public Property Skill_Verification_Skill_Type_Xref_Sid As Integer
        Public Property Skill_Verification_Skill_Type_Sid As Integer
        Public Property Skill_Verification_Skill_Type As String
        Public Property Category_Type_Sid As Integer
        Public Property CategoryName As String
        Public Property Application_Sid As Integer?
        Public Property Skill_CheckList_Sid As Integer
        Public Property Skill_CheckList_Name As String
        Public Property Verification_Date As Date
        Public Property Verified_Person_Name As String
        Public Property Verified_Person_Title As String
        Public Property Active_Flg As Boolean
    End Class

End Namespace

