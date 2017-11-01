Namespace Objects
    Public Class SkillsVerificationDetailsObject
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
        Public Property SkillCheckList As List(Of SkillVerificatonTypeCheckListDetailsObject)
    End Class
    Public Class SkillVerificatonTypeCheckListDetailsObject
        Public Property Skill_Verification_Type_CheckList_Xref_Sid As Integer
        Public Property Skill_Verification_Skill_Type_Xref_Sid As Integer
        Public Property Skill_CheckList_Sid As Integer
        Public Property Skill_CheckList_Name As String
        Public Property Verification_Date As Date
        Public Property Verified_Person_Name As String
        Public Property Verified_Person_Title As String
        Public Property Active_Flg As Boolean
    End Class

    Public Class SkillVerificationTypeCheckListOnlyObject

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

