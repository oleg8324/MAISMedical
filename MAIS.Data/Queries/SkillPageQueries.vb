
Imports MAIS.Data.Queries
Imports ODMRDDHelperClassLibrary.Utility
Imports System.Data.Entity.Validation

Namespace Queries
    Public Interface ISkillPageQueries
        Inherits IQueriesBase
        Function GetSkillCategorys() As List(Of Data.Category_Type)
        Function GetSKillListByCategory(ByVal CategoryID As Integer) As List(Of Data.Skill_Type)
        Function GetSkillCheckListbySkillID(ByVal SkillID As Integer) As List(Of Data.Skill_CheckList)
        Function SaveSkillVerificationData(ByVal SkillData As Objects.SkillsVerificationDetailsObject) As Boolean
        Function SaveSkillVerificationData(ByVal SkillData As List(Of Objects.SkillsVerificationDetailsObject)) As Boolean
        Function SaveSkillVerificationDataToTemp(ByVal SkillData As Objects.SkillsVerificationDetailsObject) As Boolean
        Function SaveSkillVerificationDataToTemp(ByVal SkillData As List(Of Objects.SkillsVerificationDetailsObject)) As Boolean

        Function GetSkillVerificationData(ByVal User As String, Optional ByVal ApplicationID As Integer = 0) As List(Of Objects.SkillsVerificationDetailsObject)
        Function GetSkillVerificationCheckListData(ByVal user As String, Optional ByVal ApplicationID As Integer = 0) As List(Of Objects.SkillVerificationTypeCheckListOnlyObject)
        Function GetSkillVerificationPageCompletion(ByVal User As String, ByVal CategoryID As Integer, ByVal CheckTemp As Boolean, Optional ByVal ApplicationID As Integer = 0) As Boolean

        Function DeleteSkillVerificationDataFromTemp(ByVal SkillVerificationSid As Integer) As Boolean
        Function DeleteSkillVerificationDataFromPerm(ByVal SkillVerificationSid As Integer) As Boolean

    End Interface

    Public Class SkillPageQueries
        Inherits QueriesBase
        Implements ISkillPageQueries


        Public Function GetSkillCategorys() As List(Of Category_Type) Implements ISkillPageQueries.GetSkillCategorys
            Dim retVal As New List(Of Data.Category_Type)
            Try
                Using context As New MAISContext
                    retVal = (From c In context.Category_Type _
                              Where c.Category_Type_Sid = 1 Or c.Category_Type_Sid = 3 Or c.Category_Type_Sid = 4
                              Select c).ToList
                End Using

                Return retVal
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while pulling categories from Skills services query.", True, False))
                Me.LogError("Error while pulling Categories form Skills page services query.", CInt(Me.UserID), ex)
                Return retVal
            End Try
        End Function

        Public Function GetSKillListByCategory(CategoryID As Integer) As List(Of Skill_Type) Implements ISkillPageQueries.GetSKillListByCategory
            Dim retVal As New List(Of Data.Skill_Type)
            Try
                Using context As New MAISContext
                    retVal = (From sK In context.Skill_Type
                              Join Csk In context.Category_Type_Skill_Type_Xref On Csk.Skill_Type_Sid Equals sK.Skill_Type_Sid
                              Where Csk.Category_Type_Sid = CategoryID
                              Select sK).ToList

                End Using


                Return retVal

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while pulling Skills from Skill types services query.", True, False))
                Me.LogError("Error while pulling Skill types form Skills page services query.", CInt(Me.UserID), ex)
                Return retVal
            End Try
        End Function

        Public Function GetSkillCheckListbySkillID(SkillID As Integer) As List(Of Skill_CheckList) Implements ISkillPageQueries.GetSkillCheckListbySkillID
            Dim retVal As New List(Of Data.Skill_CheckList)
            Try
                Using context As New MAISContext
                    retVal = (From s In context.Skill_Type_Skill_CheckList_Xref
                              Where s.Skill_Type_Sid = SkillID AndAlso s.Skill_CheckList.Active_Flg = True
                              Select s.Skill_CheckList).ToList

                End Using
                Return retVal
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while pulling Skill CheckList from Skill types services query.", True, False))
                Me.LogError("Error while pulling Skill CheckList types form Skills page services query.", CInt(Me.UserID), ex)
                Return retVal
            End Try
        End Function

        Public Function SaveSkillVerificationData(SkillData As Objects.SkillsVerificationDetailsObject) As Boolean Implements ISkillPageQueries.SaveSkillVerificationData
            Dim retVal As Boolean = False

            Try
                Using context As New MAISContext
                    Dim SV As New Data.Skill_Verification
                    Dim RNDDPersonTypexrefSid As Integer

                    'Test for the user
                    If (SkillData.RN_DD_Person_Type_Xref_SID_string.Contains("DD")) Then
                        RNDDPersonTypexrefSid = ((From r In context.RN_DD_Person_Type_Xref _
                                                                Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                               Where ddP.DDPersonnel_Code = SkillData.RN_DD_Person_Type_Xref_SID_string
                                                               Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault)
                    Else
                        RNDDPersonTypexrefSid = ((From r In context.RN_DD_Person_Type_Xref _
                                                        Join RN In context.RNs On RN.RN_Sid Equals r.RN_DDPersonnel_Sid
                                                        Where RN.RNLicense_Number = SkillData.RN_DD_Person_Type_Xref_SID_string
                                                        Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault)
                    End If
                    '------------------------- end of Test for user -----------------------------------------
                    's.Skill_Verification_Sid = SkillData.Skill_Verification_Sid AndAlso

                    SV = (From s In context.Skill_Verification _
                          Where s.Application_Sid = SkillData.Application_Sid And s.Category_Type_Sid = SkillData.Category_Type_Sid And s.RN_DD_Person_Type_Xref_Sid = RNDDPersonTypexrefSid _
                          Select s).FirstOrDefault
                    If SV Is Nothing Then  ' Need to do insert
                        SV = New Data.Skill_Verification
                        With SV

                            .RN_DD_Person_Type_Xref_Sid = RNDDPersonTypexrefSid


                            .Category_Type_Sid = SkillData.Category_Type_Sid
                            .Application_Sid = SkillData.Application_Sid
                            .Permanent_Flg = SkillData.Permanent_Flg
                            .Start_Date = SkillData.Skill_Verification_Start_Date
                            .End_Date = SkillData.Skill_Verification_End_Date
                            .Active_Flg = SkillData.Skill_Verification_Active_Flg
                            .Create_By = UserID
                            .Create_Date = Now()
                            .Last_Update_By = UserID
                            .Last_Update_Date = Now()
                        End With

                        Dim SVKT As New Data.Skill_Verification_Skill_Type_Xref
                        With SVKT
                            .Skill_Verification = SV
                            .Skill_Type_Sid = SkillData.Skill_Verification_Skill_Type_Sid
                            .Active_Flg = SkillData.Skill_Verification_Skill_Type_Active_Flag
                            .Create_By = UserID
                            .Create_Date = Now()
                            .Last_Update_By = UserID
                            .Last_Update_Date = Now()
                            For Each scklist In SkillData.SkillCheckList
                                Dim skitme As New Data.Skill_Verification_Type_CheckList_Xref
                                With skitme
                                    .Skill_Verification_Skill_Type_Xref = SVKT
                                    .Skill_CheckList_Sid = scklist.Skill_CheckList_Sid
                                    .Verification_Date = scklist.Verification_Date
                                    .Verified_Person_Name = scklist.Verified_Person_Name
                                    .Verified_Person_Title = scklist.Verified_Person_Title
                                    .Active_Flg = scklist.Active_Flg
                                    .Create_By = UserID
                                    .Create_Date = Now
                                    .Last_Update_By = UserID
                                    .Last_Update_Date = Now()
                                End With
                                context.Skill_Verification_Type_CheckList_Xref.Add(skitme)
                            Next
                        End With



                        context.Skill_Verification.Add(SV)
                        context.Skill_Verification_Skill_Type_Xref.Add(SVKT)

                    Else 'Need to update
                        With SV
                            '.Category_Type_Sid = SkillData.Category_Type_Sid
                            .Last_Update_Date = Now
                            .Last_Update_By = UserID
                            'Need to test if the Skill Type exist

                            Dim Found As Boolean
                            Found = (From SkillType In .Skill_Verification_Skill_Type_Xref
                                     Where SkillType.Skill_Type_Sid = SkillData.Skill_Verification_Skill_Type_Sid
                                     Select SkillType).Count

                            If Found = True Then ' Need to do update
                                Dim foundType = (From st In .Skill_Verification_Skill_Type_Xref
                                                  Where st.Skill_Type_Sid = SkillData.Skill_Verification_Skill_Type_Sid
                                                  Select st).ToList

                                For Each st In foundType

                                    st.Skill_Type_Sid = SkillData.Skill_Verification_Skill_Type_Sid
                                    st.Active_Flg = SkillData.Skill_Verification_Skill_Type_Active_Flag
                                    st.Last_Update_By = UserID
                                    st.Last_Update_Date = Now
                                    For Each SKV In SkillData.SkillCheckList
                                        Dim STypeCheckList As New Data.Skill_Verification_Type_CheckList_Xref
                                        With STypeCheckList
                                            .Skill_Verification_Skill_Type_Xref = st
                                            .Skill_CheckList_Sid = SKV.Skill_CheckList_Sid
                                            .Verification_Date = SKV.Verification_Date
                                            .Verified_Person_Name = SKV.Verified_Person_Name
                                            .Verified_Person_Title = SKV.Verified_Person_Title
                                            .Active_Flg = SV.Active_Flg
                                            .Create_By = UserID
                                            .Create_Date = Now
                                            .Last_Update_By = UserID
                                            .Last_Update_Date = Now()
                                        End With
                                        'context.Skill_Verification_Type_CheckList_Xref.Add(STypeCheckList)
                                    Next

                                Next
                            Else ' need to insert
                                Dim sTypeData As New Skill_Verification_Skill_Type_Xref
                                With sTypeData
                                    .Skill_Verification = SV
                                    .Skill_Type_Sid = SkillData.Skill_Verification_Skill_Type_Sid
                                    .Active_Flg = SkillData.Skill_Verification_Skill_Type_Active_Flag
                                    .Create_By = UserID
                                    .Create_Date = Now
                                    .Last_Update_By = UserID
                                    .Last_Update_Date = Now
                                    For Each SKV In SkillData.SkillCheckList
                                        Dim STypeCheckList As New Data.Skill_Verification_Type_CheckList_Xref
                                        With STypeCheckList
                                            .Skill_Verification_Skill_Type_Xref = sTypeData
                                            .Skill_CheckList_Sid = SKV.Skill_CheckList_Sid
                                            .Verification_Date = SKV.Verification_Date
                                            .Verified_Person_Name = SKV.Verified_Person_Name
                                            .Verified_Person_Title = SKV.Verified_Person_Title
                                            .Active_Flg = SV.Active_Flg
                                            .Create_By = UserID
                                            .Create_Date = Now
                                            .Last_Update_By = UserID
                                            .Last_Update_Date = Now()
                                        End With
                                        sTypeData.Skill_Verification_Type_CheckList_Xref.Add(STypeCheckList)
                                    Next
                                End With

                                'context.Application_Skill_Verification.Add(SData)
                                context.Skill_Verification_Skill_Type_Xref.Add(sTypeData)

                            End If
                        End With

                    End If
                    'Below code is to make entry in application history to capture skills effective date JH 5/14/2013
                    If ((SkillData IsNot Nothing) And (SkillData.Application_Sid > 0)) Then
                        Dim AppHis As Application_History = (From ah In context.Application_History
                                                             Where ah.Application_Sid = SkillData.Application_Sid
                                                             Select ah).FirstOrDefault()
                        If AppHis IsNot Nothing Then
                            AppHis.DDPersonnel_Skills_Date = DateTime.Now
                            AppHis.Last_Update_Date = DateTime.Now
                            AppHis.Last_Update_By = Me.UserID
                        End If
                    End If
                    retVal = context.SaveChanges()
                End Using

                Return retVal
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while saving Skill CheckList from Skill types services query.", True, False))
                Me.LogError("Error while saving Skill CheckList types form Skills page services query.", CInt(Me.UserID), ex)
                Return retVal
            End Try
        End Function

        Public Function SaveSkillVerificationData(SkillData As List(Of Objects.SkillsVerificationDetailsObject)) As Boolean Implements ISkillPageQueries.SaveSkillVerificationData
            Dim retVal As Boolean = False
            Try
                For Each sl In SkillData
                    retVal = SaveSkillVerificationData(sl)
                    If retVal = False Then
                        Exit For
                    End If
                Next
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in SaveSkillVerificationData .", True, False))
                Me.LogError("Error in SaveSkillVerificationData.", CInt(Me.UserID), ex)
            End Try

            Return retVal
        End Function

        Public Function SaveSkillVerificationDataToTemp(SkillData As Objects.SkillsVerificationDetailsObject) As Boolean Implements ISkillPageQueries.SaveSkillVerificationDataToTemp
            Dim retVal As Boolean = False

            Try
                Using context As New MAISContext
                    Dim SData As New Data.Application_Skill_Verification
                    SData = (From sd In context.Application_Skill_Verification _
                             Where sd.Application_Sid = SkillData.Application_Sid AndAlso sd.Category_Type_Sid = SkillData.Category_Type_Sid
                             Select sd).FirstOrDefault


                    If SData Is Nothing Then 'This is the insert
                        SData = New Data.Application_Skill_Verification
                        With SData
                            .Application_Sid = SkillData.Application_Sid
                            .Category_Type_Sid = SkillData.Category_Type_Sid
                            .Create_Date = Now
                            .Create_By = UserID
                            .Last_Update_By = UserID
                            .Last_Update_Date = Now
                        End With
                        Dim sTypeData As New Application_Skill_Type_Xref
                        With sTypeData
                            .Application_Skill_Verification = SData
                            .Skill_Type_Sid = SkillData.Skill_Verification_Skill_Type_Sid
                            .Active_Flg = SkillData.Skill_Verification_Skill_Type_Active_Flag
                            .Create_BY = UserID
                            .Create_Date = Now
                            .Last_Update_By = UserID
                            .Last_Update_Date = Now
                            For Each SV In SkillData.SkillCheckList
                                Dim STypeCheckList As New Data.Application_Skill_Type_CheckList_Xref
                                With STypeCheckList
                                    .Application_Skill_Type_Xref = sTypeData
                                    .Skill_CheckList_Sid = SV.Skill_CheckList_Sid
                                    .Verification_Date = SV.Verification_Date
                                    .Verified_Person_Name = SV.Verified_Person_Name
                                    .Verified_Person_Title = SV.Verified_Person_Title
                                    .Active_Flg = SV.Active_Flg
                                    .Create_By = UserID
                                    .Create_Date = Now
                                    .Last_Update = UserID
                                    .Last_Update_Date = Now()
                                End With
                                sTypeData.Application_Skill_Type_CheckList_Xref.Add(STypeCheckList)
                            Next
                        End With

                        context.Application_Skill_Verification.Add(SData)
                        context.Application_Skill_Type_Xref.Add(sTypeData)

                    Else ' this is the update
                        With SData
                            .Category_Type_Sid = SkillData.Category_Type_Sid
                            .Last_Update_Date = Now
                            .Last_Update_By = UserID

                            'Need to test if the Skill type exist.
                            Dim Found As Boolean
                            Found = (From SkillType In .Application_Skill_Type_Xref
                                     Where SkillType.Skill_Type_Sid = SkillData.Skill_Verification_Skill_Type_Sid
                                     Select SkillType).Count

                            If Found = True Then ' Need to do update
                                Dim foundType = (From st In .Application_Skill_Type_Xref
                                                  Where st.Skill_Type_Sid = SkillData.Skill_Verification_Skill_Type_Sid
                                                  Select st).ToList

                                For Each st In foundType

                                    st.Skill_Type_Sid = SkillData.Skill_Verification_Skill_Type_Sid
                                    st.Active_Flg = SkillData.Skill_Verification_Skill_Type_Active_Flag
                                    st.Last_Update_By = UserID
                                    st.Last_Update_Date = Now
                                    For Each SV In SkillData.SkillCheckList
                                        'Dim STypeCheckList As New Data.Application_Skill_Type_CheckList_Xref
                                        Dim STypeCheckList = (From AT In context.Application_Skill_Type_CheckList_Xref
                                                              Where AT.Skill_CheckList_Sid = SV.Skill_CheckList_Sid And AT.Application_Skill_Type_Xref_Sid = st.Application_Skill_Type_Xref_Sid _
                                                              Select AT).FirstOrDefault
                                        With STypeCheckList
                                            .Application_Skill_Type_Xref = st
                                            .Skill_CheckList_Sid = SV.Skill_CheckList_Sid
                                            .Verification_Date = SV.Verification_Date
                                            .Verified_Person_Name = SV.Verified_Person_Name
                                            .Verified_Person_Title = SV.Verified_Person_Title

                                            .Active_Flg = SV.Active_Flg
                                            .Create_By = UserID
                                            .Create_Date = Now
                                            .Last_Update = UserID
                                            .Last_Update_Date = Now()
                                        End With
                                        'context.Application_Skill_Type_CheckList_Xref.Add(STypeCheckList)
                                    Next

                                Next
                            Else ' need to insert
                                Dim sTypeData As New Application_Skill_Type_Xref
                                With sTypeData
                                    .Application_Skill_Verification = SData
                                    .Skill_Type_Sid = SkillData.Skill_Verification_Skill_Type_Sid
                                    .Active_Flg = SkillData.Skill_Verification_Skill_Type_Active_Flag
                                    .Create_BY = UserID
                                    .Create_Date = Now
                                    .Last_Update_By = UserID
                                    .Last_Update_Date = Now
                                    For Each SV In SkillData.SkillCheckList
                                        Dim STypeCheckList As New Data.Application_Skill_Type_CheckList_Xref
                                        With STypeCheckList
                                            .Application_Skill_Type_Xref = sTypeData
                                            .Skill_CheckList_Sid = SV.Skill_CheckList_Sid
                                            .Verification_Date = SV.Verification_Date
                                            .Verified_Person_Name = SV.Verified_Person_Name
                                            .Verified_Person_Title = SV.Verified_Person_Title
                                            .Active_Flg = SV.Active_Flg
                                            .Create_By = UserID
                                            .Create_Date = Now
                                            .Last_Update = UserID
                                            .Last_Update_Date = Now()
                                        End With
                                        sTypeData.Application_Skill_Type_CheckList_Xref.Add(STypeCheckList)
                                    Next
                                End With

                                'context.Application_Skill_Verification.Add(SData)
                                context.Application_Skill_Type_Xref.Add(sTypeData)

                            End If



                        End With

                    End If

                    'Below code is to make entry in application history to capture skills effective date JH 5/14/2013
                    If ((SkillData IsNot Nothing) And (SkillData.Application_Sid > 0)) Then
                        Dim AppHis As Application_History = (From ah In context.Application_History
                                                             Where ah.Application_Sid = SkillData.Application_Sid
                                                             Select ah).FirstOrDefault()
                        If AppHis IsNot Nothing Then
                            AppHis.DDPersonnel_Skills_Date = DateTime.Now
                            AppHis.Last_Update_Date = DateTime.Now
                            AppHis.Last_Update_By = Me.UserID
                        End If
                    End If
                    retVal = context.SaveChanges()

                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while saving Skill data to the Temp tables from Skill types services query.", True, False))
                Me.LogError("Error while saving Skill data to the Temp talbes form Skills page services query.", CInt(Me.UserID), ex)
                retVal = False
            End Try
            Return retVal

        End Function

        Public Function SaveSkillVerificationDataToTemp(SkillData As List(Of Objects.SkillsVerificationDetailsObject)) As Boolean Implements ISkillPageQueries.SaveSkillVerificationDataToTemp
            Dim retVal As Boolean = False
            Try
                For Each sk In SkillData
                    retVal = SaveSkillVerificationDataToTemp(sk)
                    If retVal = False Then
                        Exit For
                    End If
                Next
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while saving Skill data to the Temp tables from Skill types services query.", True, False))
                Me.LogError("Error while saving Skill data to the Temp talbes form Skills page services query.", CInt(Me.UserID), ex)
            End Try
            Return retVal

        End Function

        Public Function GetSkillVerificationData(User As String, Optional ApplicationID As Integer = 0) As List(Of Objects.SkillsVerificationDetailsObject) Implements ISkillPageQueries.GetSkillVerificationData
            Dim retVal As New List(Of Objects.SkillsVerificationDetailsObject)

            Try
                Using context As New MAISContext
                    Dim TempList As New List(Of Objects.SkillsVerificationDetailsObject)
                    Dim PermList As New List(Of Objects.SkillsVerificationDetailsObject)

                    TempList = (From sv In context.Application_Skill_Verification
                                Join st In context.Application_Skill_Type_Xref On st.Application_Skill_Verification_Sid Equals sv.Application_Skill_Verification_Sid
                                    Where sv.Application_Sid = ApplicationID
                                    Select New Objects.SkillsVerificationDetailsObject With {
                                        .Skill_Verification_Sid = sv.Application_Skill_Verification_Sid,
                                        .Category_Type_Sid = sv.Category_Type_Sid,
                                        .CategoryName = sv.Category_Type.Category_Desc,
                                        .Application_Sid = sv.Application_Sid,
                                        .Skill_Verification_Skill_Type_Xref_Sid = st.Application_Skill_Type_Xref_Sid,
                                        .Skill_Verification_Skill_Type_Sid = st.Skill_Type_Sid,
                                        .Skill_Verification_Skill_Type = st.Skill_Type.Skill_Desc,
                                        .Skill_Verification_Skill_Type_Active_Flag = st.Active_Flg}).ToList

                    For Each TmList In TempList
                        Dim NSkillCheckList As New List(Of Objects.SkillVerificatonTypeCheckListDetailsObject)
                        NSkillCheckList = (From cl In context.Application_Skill_Type_CheckList_Xref
                                           Where cl.Application_Skill_Type_Xref_Sid = TmList.Skill_Verification_Skill_Type_Xref_Sid
                                           Select New Objects.SkillVerificatonTypeCheckListDetailsObject With {
                                                .Skill_Verification_Type_CheckList_Xref_Sid = cl.Application_Skill_Type_CheckList_Xref_Sid,
                                                .Skill_Verification_Skill_Type_Xref_Sid = cl.Application_Skill_Type_Xref_Sid,
                                                .Skill_CheckList_Sid = cl.Skill_CheckList_Sid,
                                                .Skill_CheckList_Name = cl.Skill_CheckList.Skill_CheckList_Desc,
                                                .Verification_Date = cl.Verification_Date,
                                                .Verified_Person_Name = cl.Verified_Person_Name,
                                                .Verified_Person_Title = cl.Verified_Person_Title,
                                                .Active_Flg = cl.Active_Flg}).ToList
                        TmList.SkillCheckList = NSkillCheckList

                    Next


                    PermList = (From sv In context.Skill_Verification
                                Join st In context.Skill_Verification_Skill_Type_Xref On st.Skill_Verification_Sid Equals sv.Skill_Verification_Sid
                                    Where sv.Application_Sid = 0 And sv.End_Date <= Now() And sv.RN_DD_Person_Type_Xref_Sid = (((From r In context.RN_DD_Person_Type_Xref _
                                                                        Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                                       Where ddP.DDPersonnel_Code = User
                                                                       Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault))
                                    Select New Objects.SkillsVerificationDetailsObject With {
                                        .Skill_Verification_Sid = sv.Skill_Verification_Sid,
                                        .Category_Type_Sid = sv.Category_Type_Sid,
                                        .CategoryName = sv.Category_Type.Category_Desc,
                                        .Application_Sid = sv.Application_Sid,
                                        .Skill_Verification_Skill_Type_Xref_Sid = st.Skill_Verification_Skill_Type_Xref_Sid,
                                        .Skill_Verification_Skill_Type_Sid = st.Skill_Type_Sid,
                                         .Skill_Verification_Skill_Type = st.Skill_Type.Skill_Desc,
                                        .Skill_Verification_Skill_Type_Active_Flag = st.Active_Flg}).ToList


                    For Each PmList In PermList
                        Dim PSkillcheckList As New List(Of Objects.SkillVerificatonTypeCheckListDetailsObject)
                        PSkillcheckList = (From cl In context.Skill_Verification_Type_CheckList_Xref
                                           Where cl.Skill_Verification_Skill_Type_Xref_Sid = PmList.Skill_Verification_Skill_Type_Xref_Sid
                                           Select New Objects.SkillVerificatonTypeCheckListDetailsObject With {
                                                               .Skill_Verification_Type_CheckList_Xref_Sid = cl.Skill_Verification_Type_CheckList_Xref_Sid,
                                                               .Skill_Verification_Skill_Type_Xref_Sid = cl.Skill_Verification_Skill_Type_Xref_Sid,
                                                               .Skill_CheckList_Sid = cl.Skill_CheckList_Sid,
                                                               .Skill_CheckList_Name = cl.Skill_CheckList.Skill_CheckList_Desc,
                                                               .Verification_Date = cl.Verification_Date,
                                                               .Verified_Person_Name = cl.Verified_Person_Name,
                                                               .Verified_Person_Title = cl.Verified_Person_Title,
                                                               .Active_Flg = cl.Active_Flg}).ToList
                        PmList.SkillCheckList = PSkillcheckList


                    Next

                    If (TempList Is Nothing) = False Then
                        retVal.AddRange(TempList)
                    End If
                    If (PermList Is Nothing) = False Then
                        retVal.AddRange(PermList)
                    End If
                End Using

                Return retVal

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while fetching skill from the Skill Verification tables from services query.", True, False))
                Me.LogError("Error while fetching skill from the Skill Verification tables from services query.", CInt(Me.UserID), ex)
                Return retVal
            End Try



        End Function

        Public Function GetSkillVerificationCheckListData(user As String, Optional ApplicationID As Integer = 0) As List(Of Objects.SkillVerificationTypeCheckListOnlyObject) Implements ISkillPageQueries.GetSkillVerificationCheckListData
            Dim retVal As New List(Of Objects.SkillVerificationTypeCheckListOnlyObject)
            Try
                Using context As New MAISContext
                    Dim TempList As New List(Of Objects.SkillVerificationTypeCheckListOnlyObject)
                    Dim PermList As New List(Of Objects.SkillVerificationTypeCheckListOnlyObject)

                    TempList = (From skl In context.Application_Skill_Type_CheckList_Xref
                                Where skl.Application_Skill_Type_Xref.Application_Skill_Verification.Application_Sid = ApplicationID
                                Select New Objects.SkillVerificationTypeCheckListOnlyObject With {
                                    .Skill_Verification_Type_CheckList_Xref_Sid = skl.Application_Skill_Type_CheckList_Xref_Sid,
                                    .Skill_Verification_Skill_Type_Xref_Sid = skl.Application_Skill_Type_Xref_Sid,
                                    .Skill_Verification_Skill_Type_Sid = skl.Application_Skill_Type_Xref.Skill_Type_Sid,
                                    .Skill_Verification_Skill_Type = skl.Application_Skill_Type_Xref.Skill_Type.Skill_Desc,
                                    .Category_Type_Sid = skl.Application_Skill_Type_Xref.Application_Skill_Verification.Category_Type_Sid,
                                    .CategoryName = skl.Application_Skill_Type_Xref.Application_Skill_Verification.Category_Type.Category_Code,
                                    .Application_Sid = skl.Application_Skill_Type_Xref.Application_Skill_Verification.Application_Sid,
                                    .Skill_CheckList_Sid = skl.Skill_CheckList_Sid,
                                    .Skill_CheckList_Name = skl.Skill_CheckList.Skill_CheckList_Desc,
                                    .Verification_Date = skl.Verification_Date,
                                    .Verified_Person_Name = skl.Verified_Person_Name,
                                    .Verified_Person_Title = skl.Verified_Person_Title,
                                    .Active_Flg = skl.Active_Flg}).ToList

                    PermList = (From skl In context.Skill_Verification_Type_CheckList_Xref
                                Where (skl.Skill_Verification_Skill_Type_Xref.Skill_Verification.Application_Sid = ApplicationID And skl.Skill_Verification_Skill_Type_Xref.Skill_Verification.RN_DD_Person_Type_Xref_Sid = (From r In context.RN_DD_Person_Type_Xref _
                                                                        Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                                       Where ddP.DDPersonnel_Code = user
                                                                       Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault) _
                                OrElse (skl.Skill_Verification_Skill_Type_Xref.Skill_Verification.Application_Sid = 0 AndAlso Now <= skl.Skill_Verification_Skill_Type_Xref.Skill_Verification.End_Date _
                                AndAlso skl.Skill_Verification_Skill_Type_Xref.Skill_Verification.RN_DD_Person_Type_Xref_Sid = (((From r In context.RN_DD_Person_Type_Xref _
                                                                        Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                                       Where ddP.DDPersonnel_Code = user
                                                                       Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault)))
                             Select New Objects.SkillVerificationTypeCheckListOnlyObject With {
                                    .Skill_Verification_Type_CheckList_Xref_Sid = skl.Skill_Verification_Type_CheckList_Xref_Sid,
                                    .Skill_Verification_Skill_Type_Xref_Sid = skl.Skill_Verification_Skill_Type_Xref_Sid,
                                    .Skill_Verification_Skill_Type_Sid = skl.Skill_Verification_Skill_Type_Xref.Skill_Type_Sid,
                                    .Skill_Verification_Skill_Type = skl.Skill_Verification_Skill_Type_Xref.Skill_Type.Skill_Desc,
                                    .Category_Type_Sid = skl.Skill_Verification_Skill_Type_Xref.Skill_Verification.Category_Type_Sid,
                                    .CategoryName = skl.Skill_Verification_Skill_Type_Xref.Skill_Verification.Category_Type.Category_Code,
                                    .Application_Sid = skl.Skill_Verification_Skill_Type_Xref.Skill_Verification.Application_Sid,
                                    .Skill_CheckList_Sid = skl.Skill_CheckList_Sid,
                                    .Skill_CheckList_Name = skl.Skill_CheckList.Skill_CheckList_Desc,
                                    .Verification_Date = skl.Verification_Date,
                                    .Verified_Person_Name = skl.Verified_Person_Name,
                                    .Verified_Person_Title = skl.Verified_Person_Title,
                                    .Active_Flg = skl.Active_Flg}).ToList

                    If TempList IsNot Nothing Then
                        retVal.AddRange(TempList)
                    End If
                    If PermList IsNot Nothing Then
                        retVal.AddRange(PermList)
                    End If
                    'retVal = TempList

                End Using
                Return retVal

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while fetching skill from the Skill Verification tables from services query.", True, False))
                Me.LogError("Error while fetching skill from the Skill Verification tables from services query.", CInt(Me.UserID), ex)
                Return retVal
            End Try
        End Function

        Public Function GetSkillVerificationPageCompletion(User As String, CategoryID As Integer, ByVal CheckTemp As Boolean, Optional ApplicationID As Integer = 0) As Boolean Implements ISkillPageQueries.GetSkillVerificationPageCompletion
            Dim retVal As Boolean = False
            Try
                Using context As New MAISContext
                    If CheckTemp = True Then ' Test the Temp tables for the Correct Skill with Categroy and Application ID
                        'Dim TempList As New List(Of Objects.SkillsVerificationDetailsObject)
                        retVal = (From sv In context.Application_Skill_Verification
                              Join st In context.Application_Skill_Type_Xref On st.Application_Skill_Verification_Sid Equals sv.Application_Skill_Verification_Sid
                                  Where sv.Application_Sid = ApplicationID And sv.Category_Type_Sid = CategoryID
                                  Select sv).Count

                    Else ' Test the Perm tables
                        'Dim PermList As New List(Of Objects.SkillsVerificationDetailsObject)
                        retVal = (From sv In context.Skill_Verification
                               Join st In context.Skill_Verification_Skill_Type_Xref On st.Skill_Verification_Sid Equals sv.Skill_Verification_Sid
                                   Where (sv.Application_Sid = ApplicationID And sv.Category_Type_Sid = CategoryID) OrElse (sv.Application_Sid = 0 And Now() <= sv.End_Date And sv.RN_DD_Person_Type_Xref_Sid = (((From r In context.RN_DD_Person_Type_Xref _
                                                                       Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                                      Where ddP.DDPersonnel_Code = User
                                                                      Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault)) And sv.Category_Type_Sid = CategoryID)
                                                          Select sv).Count
                    End If

                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while fetching Skill Verification for count on tables from services query.", True, False))
                Me.LogError("Error while fetching Skill Verification for count on tables from services query.", CInt(Me.UserID), ex)
            End Try
            Return retVal

        End Function

        Public Function DeleteSkillVerificationDataFromTemp(ByVal SkillChecklistXrefSid As Integer) As Boolean Implements ISkillPageQueries.DeleteSkillVerificationDataFromTemp
            Dim retVal As Boolean = False
            Try
                Using context As New MAISContext

                    'Remove CheckList data first. 
                    Dim iSkilltypeList = (From skckl In context.Application_Skill_Type_CheckList_Xref
                                          Where skckl.Application_Skill_Type_CheckList_Xref_Sid = SkillChecklistXrefSid
                                          Select skckl).FirstOrDefault
                    Dim SkillTypeXrefSid As Integer
                    If iSkilltypeList IsNot Nothing Then
                        SkillTypeXrefSid = iSkilltypeList.Application_Skill_Type_Xref_Sid

                        context.Application_Skill_Type_CheckList_Xref.Remove(iSkilltypeList)

                    End If

                    Dim iSkillType = (From Skt In context.Application_Skill_Type_Xref
                                      Where Skt.Application_Skill_Type_Xref_Sid = SkillTypeXrefSid
                                      Select Skt).FirstOrDefault
                    Dim SkillVerificationSID As Integer
                    If iSkillType IsNot Nothing Then
                        If iSkillType.Application_Skill_Type_CheckList_Xref.Count = 0 Then
                            SkillVerificationSID = iSkillType.Application_Skill_Verification_Sid

                            context.Application_Skill_Type_Xref.Remove(iSkillType)

                        End If

                    End If

                    If SkillVerificationSID > 0 Then
                        Dim iSkillV = (From skv In context.Application_Skill_Verification
                                       Where skv.Application_Skill_Verification_Sid = SkillVerificationSID
                                       Select skv).FirstOrDefault
                        If iSkillV IsNot Nothing Then
                            If iSkillV.Application_Skill_Type_Xref.Count = 0 Then
                                context.Application_Skill_Verification.Remove(iSkillV)
                            End If

                        End If
                    End If


                    retVal = context.SaveChanges
                End Using
                Return retVal

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while removing skill from the Skill Verification tables from services query.", True, False))
                Me.LogError("Error while removing skill from the Skill Verification tables from services query DeleteSkillVerificationDataFromTemp.", CInt(Me.UserID), ex)
                Return retVal
            End Try
        End Function

        Public Function DeleteSkillVerificationDataFromPerm(SkillChecklistXrefSid As Integer) As Boolean Implements ISkillPageQueries.DeleteSkillVerificationDataFromPerm
            Dim retVal As Boolean = False
            Try
                Using context As New MAISContext

                    Dim iSkilltypeCheckList = (From SktckList In context.Skill_Verification_Type_CheckList_Xref
                                               Where SktckList.Skill_Verification_Type_CheckList_Xref_Sid = SkillChecklistXrefSid
                                               Select SktckList).FirstOrDefault
                    Dim SkillTypeXrefSid As Integer
                    If iSkilltypeCheckList IsNot Nothing Then
                        SkillTypeXrefSid = iSkilltypeCheckList.Skill_Verification_Skill_Type_Xref_Sid
                        context.Skill_Verification_Type_CheckList_Xref.Remove(iSkilltypeCheckList)

                    End If

                    Dim IsSkillType = (From SkillT In context.Skill_Verification_Skill_Type_Xref
                                      Where SkillT.Skill_Verification_Skill_Type_Xref_Sid = SkillTypeXrefSid
                                      Select SkillT).FirstOrDefault

                    Dim SkillVerificationSID As Integer
                    If IsSkillType IsNot Nothing Then
                        If IsSkillType.Skill_Verification_Type_CheckList_Xref.Count = 0 Then
                            SkillVerificationSID = IsSkillType.Skill_Verification_Sid
                            context.Skill_Verification_Skill_Type_Xref.Remove(IsSkillType)
                        End If

                    End If



                    If SkillVerificationSID > 0 Then
                        Dim iSkillVer = (From SkillV In context.Skill_Verification
                                         Where SkillV.Skill_Verification_Sid = SkillVerificationSID
                                         Select SkillV).FirstOrDefault
                        If iSkillVer IsNot Nothing Then
                            If iSkillVer.Skill_Verification_Skill_Type_Xref.Count = 0 Then
                                context.Skill_Verification.Remove(iSkillVer)
                            End If

                        End If
                    End If

                    retVal = context.SaveChanges
                End Using
                Return retVal

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while removing skill from the Skill Verification tables from services query.", True, False))
                Me.LogError("Error while removing skill from the Skill Verification tables from services query DeleteSkillVerificationDataFromPerm.", CInt(Me.UserID), ex)
                Return retVal
            End Try
        End Function





    End Class
End Namespace

