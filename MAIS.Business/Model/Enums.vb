Imports System.ComponentModel  
Namespace Model.Enums
Public Enum EmployerType
        <Description("RN is Self Employed")> _
SelfEmployed = 1
        <Description("Other Employer")> _
OtherAgencyEmployer = 2
        <Description("DD Personnel is a DODD Independent Provider")> _
DODDIndependantProvider = 3
        <Description("Employee of DODD Agency Provider")> _
DODDAgencyProvider = 4
End Enum
Public Enum ContactType
<Description("Home")> _
Home=1
<Description("Work")> _
Work=2
<Description("Cell/Other")> _
CellOther=3
<Description("Agency/CEO")> _
AgencyCEO=4
<Description("Work location")> _
WorkLocation=5
<Description("Work Experience")> _
WorkExperience=6
<Description("Supervisor")> _
Supervisor=7
End Enum
Public Enum AddressType
<Description("Personal Mailing Address")> _
Personal=2
<Description("Agency Address")> _
Agency=3
<Description("Work Location Address")> _
WorkLocation=4
<Description("Work Experience Address")> _
WorkExperience=5
End Enum
Public Enum ApplicationType
<Description("Initial")> _
Initial=1
<Description("Renewal")> _
Renewal=2
<Description("AddOn")> _
AddOn=3
        <Description("Update Profile")> _
UpdateProfile = 4
End Enum
Public Enum ApplicationStatusType
        <Description("Pending")> _
        Pending = 1
        <Description("Did Not Meet Requirements")> _
        DNMR = 2
        <Description("Meets Requirements")> _
        MeetsRequirements = 3
        <Description("Added To Registry")> _
        AddedToRegistry = 4
        <Description("Removed From Registry")> _
        RemovedFromRegistry = 5
        <Description("DODD Review")> _
        DODD_Review = 6
        <Description("Voided Application")> _
        VoidedApplication = 7
        <Description("Certified")> _
        Certified = 10
        <Description("Denied")> _
        Denied = 11
        <Description("Intent to Deny")> _
        IntentToDeny = 12
End Enum
Public Enum CertificationStatusType
<Description("Certified")> _
Certified=1
<Description("Expired")> _
Expired=2
<Description("Denied")> _
Denied=3
<Description("Revoked")> _
Revoked=4
<Description("Intent to Revoke")> _
IntentToRevoke=5
        <Description("Suspended")> _
        Suspended = 6
        <Description("Intent to Deny")> _
        IntenttoDeny = 7
        <Description("Registered")> _
Registered = 8
        <Description("Unregistered")> _
Unregistered = 9
        <Description("Did Not Meet Requirements")> _
DNMR = 10
        <Description("Voluntary Withdrawal")> _
Voluntary = 11

End Enum

    'Changing taxID to Provider since we r not using taxid any more JH
    Public Enum IdentificationType
        <Description("SSN")> _
        SSN = 1
        <Description("Provider#")> _
        TaxID = 2
        <Description("RN License")> _
        RNLicense = 3
        <Description("Provider#")> _
        Provider = 4
    End Enum

    Public Enum Mais_Roles
        <Description("RN Trainer")> _
            RNTrainer = 1
        <Description("QA RN")> _
            QARN = 2
        <Description("17 Bed")> _
            Bed17 = 3
        <Description("RN Instructor")> _
            RNInstructor = 4
        <Description("RN Master")> _
            RNMaster = 5
        <Description("Secretary")> _
            Secretary = 6
        <Description("DDPersonnel")> _
            DDPersonnel = 7
        <Description("Admin")> _
            Admin = 8
    End Enum

    Public Enum LevelType
        <Description("Admin")> _
            Admin = 1
        <Description("RN")> _
            RN = 2
        <Description("DDPersonnel")> _
            DDPersonnel = 3
        <Description("17Bed")> _
            Bed17 = 6
        <Description("Secretary")> _
            Secretary = 7
    End Enum

    Public Enum CategoryType
        <Description("Cat-1")> _
         Cat1 = 1
        <Description("Cat-II")> _
             CatII = 2
        <Description("Cat_III")> _
            CatIII = 3
        <Description("Cat-IV")> _
            CatIV = 4
        <Description("Cat-V")> _
            CatV = 6
        <Description("Cat-VI")> _
            CatVI = 7
        <Description("Cat-VII")> _
            CatVII = 8
        <Description("Cat-VIII")> _
            CatVIII = 10
        <Description("Cat-0")> _
            Cat0 = 11

    End Enum
    Public Enum RoleLevelCategory
        <Description("RNTrainer")> _
            RNTrainer_RLC = 4
        <Description("QARN")> _
            QARN_RLC = 7
        <Description("17Bed")> _
            Bed17_RLC = 8
        <Description("RNInstructor")> _
            RNInstructor_RLC = 5
        <Description("RNMaster")> _
            RNMaster_RLC = 6
        <Description("Secretary")> _
            Secretary_RLC = 14
        <Description("DDPersonnelCat1")> _
            DDPersonnel_RLC = 15
        <Description("DDPersonnelCat2")> _
            DDPersonnel2_RLC = 16
        <Description("DDPersonnelCat3")> _
            DDPersonnel3_RLC = 17
    End Enum
End Namespace
