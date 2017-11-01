Imports MAIS.Data


Namespace Mapping
    Public Class SkillsPageMapping
        Public Shared Function MapDBtoCategoryDetails(ByVal dbCategory As List(Of Data.Category_Type)) As List(Of Model.CategoryDetails)
            Dim retVal As New List(Of Model.CategoryDetails)
            retVal = (From db In dbCategory _
                      Select New Model.CategoryDetails With {
                          .Category_Type_Sid = db.Category_Type_Sid,
                          .Category_Code = db.Category_Code,
                          .Category_Desc = db.Category_Desc}).ToList

            Return retVal

        End Function

        Public Shared Function MapDBtoSkillDeatils(ByVal dbSkills As List(Of Data.Skill_Type)) As List(Of Model.SkillDetails)
            Dim retVal As New List(Of Model.SkillDetails)
            retVal = (From db In dbSkills _
                      Select New Model.SkillDetails With {
                          .SKill_Type_Sid = db.Skill_Type_Sid,
                          .Skill_Code = db.Skill_Code,
                          .Skill_Desc = db.Skill_Desc}).ToList
            Return retVal

        End Function

        Public Shared Function MapDBtoSkillCheckListDetails(ByVal dbSkillCheckList As List(Of Data.Skill_CheckList)) As List(Of Model.SkillCheckListDetails)
            Dim retVal As New List(Of Model.SkillCheckListDetails)
            retVal = (From skl In dbSkillCheckList _
                      Select New Model.SkillCheckListDetails With {
                          .Skill_CheckList_Sid = skl.Skill_CheckList_Sid,
                          .Skill_CheckList_Code = skl.Skill_CheckList_Code,
                          .Skill_checkList_Desc = skl.Skill_CheckList_Desc}).ToList
            Return retVal
        End Function

        Public Shared Function MapDBtoSkillCheckListWithSkilltypeDetails(ByVal db As List(Of Objects.SkillVerificationTypeCheckListOnlyObject)) As List(Of Model.SkillVerificationTypeCheckListOnly)
            Dim retVal As New List(Of Model.SkillVerificationTypeCheckListOnly)
            retVal = (From skl In db
                      Select New Model.SkillVerificationTypeCheckListOnly With {
                                .Skill_Verification_Type_CheckList_Xref_Sid = skl.Skill_Verification_Type_CheckList_Xref_Sid,
                                .Skill_Verification_Skill_Type_Xref_Sid = skl.Skill_Verification_Skill_Type_Xref_Sid,
                                .Skill_Verification_Skill_Type_Sid = skl.Skill_Verification_Skill_Type_Sid,
                                .Skill_Verification_Skill_Type = skl.Skill_Verification_Skill_Type,
                                .Category_Type_Sid = skl.Category_Type_Sid,
                                .CategoryName = skl.CategoryName,
                                .Application_Sid = skl.Application_Sid,
                                .Skill_CheckList_Sid = skl.Skill_CheckList_Sid,
                                .Skill_CheckList_Name = skl.Skill_CheckList_Name,
                                .Verification_Date = skl.Verification_Date,
                                .Verified_Person_Name = skl.Verified_Person_Name,
                                .Verified_Person_Title = skl.Verified_Person_Title,
                                .Active_Flg = skl.Active_Flg}).ToList
            Return retVal
        End Function

        Public Shared Function MapModelSkillVerificationToDb(ByVal SkillVerificationData As Model.SkillsVerificationDetails) As Objects.SkillsVerificationDetailsObject
            Dim retVal As New Objects.SkillsVerificationDetailsObject

            With retVal
                .RN_DD_Person_Type_Xref_SID_string = SkillVerificationData.RN_DD_Person_Type_Xref_SID_string
                .Category_Type_Sid = SkillVerificationData.Category_Type_Sid
                .Application_Sid = SkillVerificationData.Application_Sid
                .Permanent_Flg = SkillVerificationData.Permanent_Flg
                .Skill_Verification_Start_Date = SkillVerificationData.Skill_Verification_Start_Date
                .Skill_Verification_End_Date = SkillVerificationData.Skill_Verification_End_Date
                .Skill_Verification_Active_Flg = SkillVerificationData.Skill_Verification_Active_Flg
                .Skill_Verification_Skill_Type_Sid = SkillVerificationData.Skill_Verification_Skill_Type_Sid
                .Skill_Verification_Skill_Type_Active_Flag = SkillVerificationData.Skill_Verification_Skill_Type_Active_Flag
                .SkillCheckList = (From sk In SkillVerificationData.SkillCheckList _
                                   Select New Objects.SkillVerificatonTypeCheckListDetailsObject With {
                                       .Skill_CheckList_Sid = sk.Skill_CheckList_Sid,
                                       .Verification_Date = sk.Verification_Date,
                                       .Verified_Person_Name = sk.Verified_Person_Name,
                                       .Verified_Person_Title = sk.Verified_Person_Title,
                                       .Active_Flg = sk.Active_Flg}).ToList
            End With

            Return retVal
        End Function

        Public Shared Function MapModelSkillVerificationToDb(ByVal SkillVerificationData As List(Of Model.SkillsVerificationDetails)) As List(Of Objects.SkillsVerificationDetailsObject)
            Dim retVal As New List(Of Objects.SkillsVerificationDetailsObject)
            retVal = (From skl In SkillVerificationData _
                      Select New Objects.SkillsVerificationDetailsObject With {
                          .RN_DD_Person_Type_Xref_SID_string = skl.RN_DD_Person_Type_Xref_SID_string,
                           .Category_Type_Sid = skl.Category_Type_Sid,
                            .Application_Sid = skl.Application_Sid,
                            .Permanent_Flg = skl.Permanent_Flg,
                            .Skill_Verification_Start_Date = skl.Skill_Verification_Start_Date,
                            .Skill_Verification_End_Date = skl.Skill_Verification_End_Date,
                            .Skill_Verification_Active_Flg = skl.Skill_Verification_Active_Flg,
                            .Skill_Verification_Skill_Type_Sid = skl.Skill_Verification_Skill_Type_Sid,
                            .Skill_Verification_Skill_Type_Active_Flag = skl.Skill_Verification_Skill_Type_Active_Flag,
                          .SkillCheckList = (From skd In skl.SkillCheckList _
                                             Select New Objects.SkillVerificatonTypeCheckListDetailsObject With {
                                       .Skill_CheckList_Sid = skd.Skill_CheckList_Sid,
                                       .Verification_Date = skd.Verification_Date,
                                       .Verified_Person_Name = skd.Verified_Person_Name,
                                       .Verified_Person_Title = skd.Verified_Person_Title,
                                       .Active_Flg = skd.Active_Flg}).ToList}).ToList

            Return retVal
        End Function

        Public Shared Function MapDBtoSkillVerification(ByVal db As List(Of Objects.SkillsVerificationDetailsObject)) As List(Of Model.SkillsVerificationDetails)
            Dim retVal As New List(Of Model.SkillsVerificationDetails)

            retVal = (From s In db
                      Select New Model.SkillsVerificationDetails With {
                          .Skill_Verification_Sid = s.Skill_Verification_Sid,
                          .RN_DD_Person_Type_Xref_Sid = s.RN_DD_Person_Type_Xref_Sid,
                          .RN_DD_Person_Type_Xref_SID_string = s.RN_DD_Person_Type_Xref_SID_string,
                          .Category_Type_Sid = s.Category_Type_Sid,
                          .CategoryName = s.CategoryName,
                          .Application_Sid = s.Application_Sid,
                          .Permanent_Flg = s.Permanent_Flg,
                          .Skill_Verification_Start_Date = s.Skill_Verification_Start_Date,
                          .Skill_Verification_End_Date = s.Skill_Verification_End_Date,
                          .Skill_Verification_Active_Flg = s.Skill_Verification_Active_Flg,
                          .Skill_Verification_Skill_Type_Xref_Sid = s.Skill_Verification_Skill_Type_Xref_Sid,
                          .Skill_Verification_Skill_Type_Sid = s.Skill_Verification_Skill_Type_Sid,
                          .Skill_Verification_Skill_Type = s.Skill_Verification_Skill_Type,
                          .Skill_Verification_Skill_Type_Active_Flag = s.Skill_Verification_Skill_Type_Active_Flag,
                          .SkillCheckList = (From cl In s.SkillCheckList
                                             Select New Model.SkillVerificatonTypeCheckListDetails With {
                                                 .Skill_Verification_Type_CheckList_Xref_Sid = cl.Skill_Verification_Type_CheckList_Xref_Sid,
                                                 .Skill_Verification_Skill_Type_Xref_Sid = cl.Skill_Verification_Skill_Type_Xref_Sid,
                                                 .Skill_CheckList_Sid = cl.Skill_CheckList_Sid,
                                                 .Skill_CheckList_Name = cl.Skill_CheckList_Name,
                                                 .Verification_Date = cl.Verification_Date,
                                                 .Verified_Person_Name = cl.Verified_Person_Name,
                                                 .Verified_Person_Title = cl.Verified_Person_Title,
                                                 .Active_Flg = cl.Active_Flg}).ToList}).ToList


            Return retVal

        End Function


    End Class
End Namespace

