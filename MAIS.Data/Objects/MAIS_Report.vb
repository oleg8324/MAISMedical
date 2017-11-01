Namespace Objects         
    Public Class Course_Info
        Public Property Course_Sid As Integer
        Public Property Trainer_Name_FN As String
        Public Property Trainer_Name_LN As String
        Public Property Course_Number As String
        Public Property Session_Sid As Integer
        Public Property Session_Start_Date As DateTime?
        Public Property Session_End_Date As DateTime?
        Public Property Session_CEUs As Integer
    End Class
    Public Class DDSkills_Info
        Public Property RN_DD_Person_Type_Xref_Sid As Integer
        Public Property Category_Type_Sid As Integer
        Public Property Category_Desc As String
        Public Property Skill_Type_Sid As Integer
        Public Property Skill_Desc As String
        Public Property Skill_CheckList_Desc As String
        Public Property Verification_Date As DateTime
        Public Property Verified_Person_Name As String
        Public Property Verified_Person_Title As String
    End Class
    Public Class SearchParameters
        Public Property Licence_Code As String
        Public Property Last4SSN As Integer
        Public Property EmployerName As String
        Public Property FirstName As String
        Public Property LastName As String
        Public Property SupFirstName As String
        Public Property SupLastName As String
        Public Property CEOFirstName As String
        Public Property CEOLastName As String
        Public Property Role_Level_Cat_Sid As Integer
        Public Property Course_Sid As Integer
        Public Property ExpDateFrom As DateTime?
        Public Property ExpDateTo As DateTime?
        Public Property ExpWithinLast30Days As Boolean
        Public Property ExpWithinLast60Days As Boolean
        Public Property ExpWithinLast90Days As Boolean
        Public Property ExpWithinLast180Days As Boolean
        Public Property Cert_Status_Type_Sid As Integer
        Public Property Session_sid As Integer
        Public Property Trainer_RN_Sid As Integer
        Public Property workAddr_Sid As Integer = 0
        Public Property AdminFlg_GetAllRecords As Boolean
    End Class
    Public Class Cert_Info
        Public Property Certification_Sid As Integer = 0
        Public Property Certification_Type As String 'Role to which they certified
        Public Property Certification_Status As String 'Current active status
        Public Property Certification_Start_Date As DateTime
        Public Property Certification_End_Date As DateTime
        Public Property Attestant_Sid As Integer
        Public Property Attestant_Name_FN As String
        Public Property Attestant_Name_LN As String
        Public Property RenewalCount As Integer
        Public Property Category_Code As String
    End Class
    Public Class Employer_Info
        Public Property Employer_Sid As Integer = 0
        Public Property Employer_Name As String
        Public Property CEO_First_Name As String
        Public Property CEO_Last_Name As String
        Public Property Supervisor_First_Name As String
        Public Property Supervisor_Last_Name As String
        Public Property Work_Location_Addr_Sid As Integer = 0
        Public Property WorkAddress As String
        Public Property WorkCounty As String
    End Class
   
End Namespace