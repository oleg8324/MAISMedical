Imports MAIS.Data.Queries
Imports ODMRDDHelperClassLibrary.Utility
Imports System.Data.Entity.Validation
Imports System.Data.Objects

Namespace Queries
    Public Interface ITrainingSkillsPageQueires
        Inherits IQueriesBase
        Function GetRNCategoryAndLevel(ByVal RN_sid As Integer) As Data.Role_Category_Level_Xref
        Function GetCourseAll() As List(Of Objects.CourseDetails)
        Function GetApplicationCourseAndSessionsByAppID(ByVal AppID As Integer) As List(Of Objects.CourseDetails)
        Function GetCourseAndSessionFromPermByUserID(ByVal RN_ID As String) As List(Of Objects.CourseDetails)
        Function GetCourseSessionByRN_LicenseNumber(ByVal rn_LicenseNumber As String) As List(Of Objects.SessionCourseInfoDetails)
        Function GetCourseSessionByRN_Name(ByVal rn_Name As String) As List(Of Objects.SessionCourseInfoDetails)

        Function GetSecretaryRNS(ByVal SecretaryUserID As Integer) As List(Of Data.RN)

        Function GetCourseSessionByRN_LicenseNumber(ByVal rn_LicenseNumber As String, ByVal RoleCategoryLevelSid As Integer) As List(Of Objects.SessionCourseInfoDetails)
        Function GetCourseSessionByRN_Name(ByVal rn_Name As String, ByVal RoleCategoryLevelSid As Integer) As List(Of Objects.SessionCourseInfoDetails)

        Function GetCourseSessionByRN_LicenseNumber(ByVal rn_LicenseNumber As String, ByVal RoleCategoryLevelSid As Integer, ByVal StartDate As Date) As List(Of Objects.SessionCourseInfoDetails)

        Function GetCourseSessionByRN_Sid(ByVal RN_SID As Integer, ByVal RoleCategoryLevelSid As Integer) As List(Of Objects.SessionCourseInfoDetails)

        Function GetCourseSessionByRN_Name(ByVal rn_Name As String, ByVal RoleCategoryLevelSid As Integer, ByVal StartDate As Date) As List(Of Objects.SessionCourseInfoDetails)

        Function SaveCourseSessoin(ByVal AppID As Integer, ByVal SessionID As Integer, ByVal UserID As Integer) As Boolean

        Function DeleteCourseSession(ByVal AppID As Integer) As Boolean
        Function GetTrainingPageHelper(AppID As Integer) As Integer
        Function GetTrainingPageTotalHrOfSession(AppID As Integer) As Double
        Function GetTrainingPageTotalCEUs(ByVal AppID As Integer, ByVal RNDDUnique_Code As String) As Double
        Function GetTrainingPageTotalCESs(ByVal AppID As Integer, ByVal RNDDUnique_Code As String, ByVal CategoryID As Integer) As Double
        Function GetCEUByUserID(ByVal UserID As String, ByVal CategoryTypeID As Integer, Optional AppID As Integer = -1) As List(Of Objects.CEUsDetailsObject)
        Function GetAllCEUByUserID(UserID As String, CategoryTypeID As Integer, Optional AppID As Integer = -1) As List(Of Objects.CEUsDetailsObject)
        Function GetCategoryByRoleCategoryLevelSid(ByVal RoleCategoryLevelSid As Integer) As Data.Category_Type
        Function SaveCEUDetail(ByVal UserSavingID As Integer, ByVal CEUList As List(Of Objects.CEUsDetailsObject), ByVal AppID As Integer) As Boolean
        Function DeleteCEUByID(ByVal CEU_Sid As Integer) As Boolean

    End Interface
    Public Class TrainingSkillsPageQueires
        Inherits QueriesBase
        Implements ITrainingSkillsPageQueires


        Public Function GetRNCategoryAndLevel(RN_sid As Integer) As Role_Category_Level_Xref Implements ITrainingSkillsPageQueires.GetRNCategoryAndLevel
            Dim retVal As New Role_Category_Level_Xref
            Try
                Using Context As New MAISContext

                    retVal = (From a In Context.RN_DD_Person_Type_Xref _
                              Join b In Context.Role_RN_DD_Personnel_Xref On b.RN_DD_Person_Type_Xref_Sid Equals a.RN_DD_Person_Type_Xref_Sid _
                              Join c In Context.Role_Category_Level_Xref On c.Role_Category_Level_Sid Equals b.Role_Category_Level_Sid _
                              Where c.Active_Flg = True And a.RN_DDPersonnel_Sid = RN_sid _
                              Select c).FirstOrDefault

                End Using
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while Training Skills services.", True, False))
                Me.LogError("Error while pulling Training Skills page Category and Level services.", CInt(Me.UserID), ex)
            End Try
            Return retVal
        End Function

        Public Function GetCourseAll() As List(Of Objects.CourseDetails) Implements ITrainingSkillsPageQueires.GetCourseAll
            Dim retList As New List(Of Objects.CourseDetails)
            Try


                Using context As New MAISContext
                    For Each C As Data.Course In (From a In context.Courses Select a)
                        Dim hCourse As New Objects.CourseDetails
                        hCourse.Course_Sid = C.Course_sid
                        hCourse.RN_Sid = C.RN_Sid
                        hCourse.InstructorName = (From rnName In context.RNs Where rnName.RN_Sid = C.RN_Sid Select (rnName.First_Name & " " & rnName.Last_Name)).FirstOrDefault
                        hCourse.Role_Calegory_Level_Sid = C.Role_Category_Level_Sid
                        hCourse.StartDate = C.Start_Date
                        hCourse.EndDate = C.End_Date
                        hCourse.OBNApprovalNumber = C.OBN_Course_Number
                        hCourse.CategoryACEs = C.Category_A_CEs
                        hCourse.TotalCEs = C.Total_CEs
                        hCourse.CourseDescription = C.Course_Description
                        hCourse.Level = (From l In context.Role_Category_Level_Xref Where l.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select l.Level_Type_Sid).FirstOrDefault
                        hCourse.Category = (From CL In context.Role_Category_Level_Xref Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select CL.Category_Type_Sid).FirstOrDefault

                        Dim CourseSessionList As New List(Of Objects.SessionAddressInformation)
                        For Each S As Data.Session In (From cs In context.Sessions)
                            Dim CourseSession As New Objects.SessionAddressInformation
                            CourseSession.Session_Sid = S.Session_Sid
                            CourseSession.Course_SID = S.Course_Sid
                            CourseSession.Session_Start_Date = S.Start_Date
                            CourseSession.Session_End_Date = S.End_Date
                            CourseSession.Sponsor = S.Sponsor
                            CourseSession.Location_Name = S.Location_Name
                            CourseSession.Total_CEs = S.Total_CEs
                            Dim sal As New Data.Session_Address_Xref
                            sal = (From dsal In context.Session_Address_Xref Where dsal.Session_Sid = S.Session_Sid Select dsal).FirstOrDefault
                            'Dim sad As New Data.Address
                            'sad = (From dsad In context.Addresses Where dsad.Address_Sid = sal.Address_Sid Select dsad).FirstOrDefault
                            Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", sal.Address_Sid)
                            Dim sad As MAIS.Data.Address_Lookup_And_Insert_Result = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, 0, 0, 0, String.Empty, String.Empty, 0).FirstOrDefault


                            Dim SessonAddress As New Objects.SessionAddress
                            With SessonAddress
                                .Address_Sid = sal.Address_Sid
                                .Address_Line1 = sad.Address_Line1
                                .Address_Line2 = sad.Address_Line2
                                .City = sad.City
                                .State = sad.State_Abbr
                                If sad.Zip.Length > 5 Then
                                    .Zip_Code = Mid(sad.Zip, 1, 5)
                                    .Zip_Code_Plus4 = Mid(sad.Zip, 6, 4)
                                Else
                                    .Zip_Code = sad.Zip
                                End If
                                '.Zip_Code = sad.Zip_Code
                                '.Zip_Code_Plus4 = sad.Zip_Code_Plus4
                                .County = sad.County_Desc
                                .CountyID = sad.CountyID
                                .Session_Address_Xref_Sid = sal.Session_Address_Xref_Sid
                                .Session_Sid = sal.Session_Sid
                                .Address_Type_Sid = sal.Address_Type_Sid
                            End With
                            CourseSession.SessionAddressInfo = SessonAddress

                            Dim mSessionInfoList As New List(Of Objects.SessionInformationDetails)
                            For Each SInfo As Data.Session_Information In (From dSI In context.Session_Information Where dSI.Session_Sid = S.Session_Sid Select dSI)
                                Dim mSessionInfo As New Objects.SessionInformationDetails
                                With mSessionInfo
                                    .Session_Sid = SInfo.Session_Sid
                                    .Session_Information_SID = SInfo.Session_Informationl_Sid
                                    .Session_Date = SInfo.Session_Date
                                    .Total_CEs = SInfo.Total_CEs
                                End With
                                mSessionInfoList.Add(mSessionInfo)

                            Next
                            CourseSession.SessionInformationDetailsList = mSessionInfoList

                            CourseSessionList.Add(CourseSession)
                            hCourse.SessionDetailList = CourseSessionList

                        Next
                        retList.Add(hCourse)

                    Next


                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in Training Skills services.", True, False))
                Me.LogError("Error while pulling Training Skills page Course Details.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function


        Public Function GetCourseAndSessionFromPermByUserID(RN_ID As String) As List(Of Objects.CourseDetails) Implements ITrainingSkillsPageQueires.GetCourseAndSessionFromPermByUserID
            Dim retList As New List(Of Objects.CourseDetails)

            Using context As New MAISContext
                Dim rdPersonnelID As Integer
                If RN_ID.Contains("RN") Then
                    rdPersonnelID = (From mrn In context.RNs
                                     Where mrn.RNLicense_Number = RN_ID
                                     Select mrn.RN_Sid).FirstOrDefault
                Else
                    rdPersonnelID = (From DDN In context.DDPersonnels
                                     Where DDN.DDPersonnel_Code = RN_ID
                                     Select DDN.DDPersonnel_Sid).FirstOrDefault
                End If
                For Each AppCourseSession As Objects.ApplicationCourseSessionObject In (From CCS In context.Person_Course_Xref _
                                                                                       Join SCS In context.Person_Course_Session_Xref On SCS.Person_Course_Xref_Sid Equals CCS.Person_Course_Xref_Sid _
                                                                                       Join rRNDD In context.Role_RN_DD_Personnel_Xref On rRNDD.Role_RN_DD_Personnel_Xref_Sid Equals CCS.Role_RN_DD_Personnel_Xref_Sid _
                                                                                       Join rndd In context.RN_DD_Person_Type_Xref On rndd.RN_DD_Person_Type_Xref_Sid Equals rRNDD.RN_DD_Person_Type_Xref_Sid _
                                                                                       Join Pty In context.RN_DD_Person_Type_Xref On Pty.RN_DDPersonnel_Sid Equals rndd.RN_DDPersonnel_Sid
                                                                                       Where Pty.RN_DDPersonnel_Sid = rdPersonnelID
                                                                                       Select New Objects.ApplicationCourseSessionObject With {
                                                                                           .MA_Application_Course_Xref_Sid = CCS.Person_Course_Xref_Sid,
                                                                                           .MA_Application_Course_Sesson_Xref_Sid = SCS.Person_Course_Session_Xref1,
                                                                                           .Course_Sid = CCS.Course_Sid,
                                                                                           .Session_Sid = SCS.Session_Sid}).ToList

                    For Each C As Data.Course In (From a In context.Courses Where a.Course_sid = AppCourseSession.Course_Sid Select a)
                        Dim hCourse As New Objects.CourseDetails
                        hCourse.Course_Sid = C.Course_sid
                        hCourse.RN_Sid = C.RN_Sid
                        hCourse.InstructorName = (From rnName In context.RNs Where rnName.RN_Sid = C.RN_Sid Select (rnName.First_Name & " " & rnName.Last_Name)).FirstOrDefault
                        hCourse.Role_Calegory_Level_Sid = C.Role_Category_Level_Sid
                        hCourse.StartDate = C.Start_Date
                        hCourse.EndDate = C.End_Date
                        hCourse.OBNApprovalNumber = C.OBN_Course_Number
                        hCourse.CategoryACEs = C.Category_A_CEs
                        hCourse.TotalCEs = C.Total_CEs
                        hCourse.CourseDescription = C.Course_Description
                        hCourse.Level = (From l In context.Role_Category_Level_Xref Where l.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select l.Level_Type_Sid).FirstOrDefault
                        hCourse.Category = (From CL In context.Role_Category_Level_Xref Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select CL.Category_Type_Sid).FirstOrDefault

                        Dim CourseSessionList As New List(Of Objects.SessionAddressInformation)
                        For Each S As Data.Session In (From cs In context.Sessions Where cs.Session_Sid = AppCourseSession.Session_Sid)
                            Dim CourseSession As New Objects.SessionAddressInformation
                            CourseSession.Session_Sid = S.Session_Sid
                            CourseSession.Course_SID = S.Course_Sid
                            CourseSession.Session_Start_Date = S.Start_Date
                            CourseSession.Session_End_Date = S.End_Date
                            CourseSession.Sponsor = S.Sponsor
                            CourseSession.Location_Name = S.Location_Name
                            CourseSession.Total_CEs = S.Total_CEs
                            Dim sal As New Data.Session_Address_Xref
                            sal = (From dsal In context.Session_Address_Xref Where dsal.Session_Sid = S.Session_Sid Select dsal).FirstOrDefault
                            'Dim sad As New Data.Address
                            'sad = (From dsad In context.Addresses Where dsad.Address_Sid = sal.Address_Sid Select dsad).FirstOrDefault
                            Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", sal.Address_Sid)
                            Dim sad As MAIS.Data.Address_Lookup_And_Insert_Result = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, 0, 0, 0, String.Empty, String.Empty, 0).FirstOrDefault


                            Dim SessonAddress As New Objects.SessionAddress
                            With SessonAddress
                                .Address_Sid = sal.Address_Sid
                                .Address_Line1 = sad.Address_Line1
                                .Address_Line2 = sad.Address_Line2
                                .City = sad.City
                                .State = sad.State_Abbr
                                If sad.Zip.Length > 5 Then
                                    .Zip_Code = Mid(sad.Zip, 1, 5)
                                    .Zip_Code_Plus4 = Mid(sad.Zip, 6, 4)
                                Else
                                    .Zip_Code = sad.Zip
                                End If
                                '.Zip_Code = sad.Zip_Code
                                '.Zip_Code_Plus4 = sad.Zip_Code_Plus4
                                .County = sad.County_Desc
                                .CountyID = sad.CountyID
                                .Session_Address_Xref_Sid = sal.Session_Address_Xref_Sid
                                .Session_Sid = sal.Session_Sid
                                .Address_Type_Sid = sal.Address_Type_Sid
                            End With
                            CourseSession.SessionAddressInfo = SessonAddress

                            Dim mSessionInfoList As New List(Of Objects.SessionInformationDetails)
                            For Each SInfo As Data.Session_Information In (From dSI In context.Session_Information Where dSI.Session_Sid = S.Session_Sid Select dSI)
                                Dim mSessionInfo As New Objects.SessionInformationDetails
                                With mSessionInfo
                                    .Session_Sid = SInfo.Session_Sid
                                    .Session_Information_SID = SInfo.Session_Informationl_Sid
                                    .Session_Date = SInfo.Session_Date
                                    .Total_CEs = SInfo.Total_CEs
                                End With
                                mSessionInfoList.Add(mSessionInfo)

                            Next
                            CourseSession.SessionInformationDetailsList = mSessionInfoList

                            CourseSessionList.Add(CourseSession)
                            hCourse.SessionDetailList = CourseSessionList

                        Next
                        retList.Add(hCourse)

                    Next

                Next



            End Using
            Return retList
        End Function

        Public Function GetApplicationCourseAndSessionsByAppID(AppID As Integer) As List(Of Objects.CourseDetails) Implements ITrainingSkillsPageQueires.GetApplicationCourseAndSessionsByAppID
            Dim retList As New List(Of Objects.CourseDetails)
            Try


                Using context As New MAISContext

                    For Each AppCourseSession As Objects.ApplicationCourseSessionObject In (From ACS In context.Application_Course_Xref _
                                                                                            Join ACSLink In context.Application_Course_Session_Xref On ACSLink.Application_Course_Xref_Sid Equals ACS.Application_Course_Xref_Sid _
                                                                                            Where ACS.Application_Sid = AppID
                                                                                            Select New Objects.ApplicationCourseSessionObject With {
                                                                                                .MA_Application_Course_Xref_Sid = ACS.Application_Course_Xref_Sid,
                                                                                                .MA_Application_Course_Sesson_Xref_Sid = ACSLink.Application_Course_Session_Xref1,
                                                                                                .Application_Sid = ACS.Application_Sid,
                                                                                                .Course_Sid = ACS.Course_Sid,
                                                                                                .Session_Sid = ACSLink.Session_Sid}).ToList()


                        For Each C As Data.Course In (From a In context.Courses Where a.Course_sid = AppCourseSession.Course_Sid Select a)
                            Dim hCourse As New Objects.CourseDetails
                            hCourse.Course_Sid = C.Course_sid
                            hCourse.RN_Sid = C.RN_Sid
                            hCourse.InstructorName = (From rnName In context.RNs Where rnName.RN_Sid = C.RN_Sid Select (rnName.First_Name & " " & rnName.Last_Name)).FirstOrDefault
                            hCourse.Role_Calegory_Level_Sid = C.Role_Category_Level_Sid
                            hCourse.StartDate = C.Start_Date
                            hCourse.EndDate = C.End_Date
                            hCourse.OBNApprovalNumber = C.OBN_Course_Number
                            hCourse.CategoryACEs = C.Category_A_CEs
                            hCourse.TotalCEs = C.Total_CEs
                            hCourse.CourseDescription = C.Course_Description
                            hCourse.Level = (From l In context.Role_Category_Level_Xref Where l.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select l.Level_Type_Sid).FirstOrDefault
                            hCourse.Category = (From CL In context.Role_Category_Level_Xref Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select CL.Category_Type_Sid).FirstOrDefault

                            Dim CourseSessionList As New List(Of Objects.SessionAddressInformation)
                            For Each S As Data.Session In (From cs In context.Sessions Where cs.Session_Sid = AppCourseSession.Session_Sid)
                                Dim CourseSession As New Objects.SessionAddressInformation
                                CourseSession.Session_Sid = S.Session_Sid
                                CourseSession.Course_SID = S.Course_Sid
                                CourseSession.Session_Start_Date = S.Start_Date
                                CourseSession.Session_End_Date = S.End_Date
                                CourseSession.Sponsor = S.Sponsor
                                CourseSession.Location_Name = S.Location_Name
                                CourseSession.Total_CEs = S.Total_CEs
                                Dim sal As New Data.Session_Address_Xref
                                sal = (From dsal In context.Session_Address_Xref Where dsal.Session_Sid = S.Session_Sid Select dsal).FirstOrDefault
                                'Dim sad As New Data.Address
                                'sad = (From dsad In context.Addresses Where dsad.Address_Sid = sal.Address_Sid Select dsad).FirstOrDefault
                                Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", sal.Address_Sid)
                                Dim sad As MAIS.Data.Address_Lookup_And_Insert_Result = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, 0, 0, 0, String.Empty, String.Empty, 0).FirstOrDefault


                                Dim SessonAddress As New Objects.SessionAddress
                                With SessonAddress
                                    .Address_Sid = sal.Address_Sid
                                    .Address_Line1 = sad.Address_Line1
                                    .Address_Line2 = sad.Address_Line2
                                    .City = sad.City
                                    .State = sad.State_Abbr
                                    If sad.Zip.Length > 5 Then
                                        .Zip_Code = Mid(sad.Zip, 1, 5)
                                        .Zip_Code_Plus4 = Mid(sad.Zip, 6, 4)
                                    Else
                                        .Zip_Code = sad.Zip
                                    End If
                                    '.Zip_Code = sad.Zip_Code
                                    '.Zip_Code_Plus4 = sad.Zip_Code_Plus4
                                    .County = sad.County_Desc
                                    .CountyID = sad.CountyID
                                    .Session_Address_Xref_Sid = sal.Session_Address_Xref_Sid
                                    .Session_Sid = sal.Session_Sid
                                    .Address_Type_Sid = sal.Address_Type_Sid
                                End With
                                CourseSession.SessionAddressInfo = SessonAddress

                                Dim mSessionInfoList As New List(Of Objects.SessionInformationDetails)
                                For Each SInfo As Data.Session_Information In (From dSI In context.Session_Information Where dSI.Session_Sid = S.Session_Sid Select dSI)
                                    Dim mSessionInfo As New Objects.SessionInformationDetails
                                    With mSessionInfo
                                        .Session_Sid = SInfo.Session_Sid
                                        .Session_Information_SID = SInfo.Session_Informationl_Sid
                                        .Session_Date = SInfo.Session_Date
                                        .Total_CEs = SInfo.Total_CEs
                                    End With
                                    mSessionInfoList.Add(mSessionInfo)

                                Next
                                CourseSession.SessionInformationDetailsList = mSessionInfoList

                                CourseSessionList.Add(CourseSession)
                                hCourse.SessionDetailList = CourseSessionList

                            Next
                            retList.Add(hCourse)

                        Next
                    Next

                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in Training Skills services.", True, False))
                Me.LogError("Error while pulling Training Skills page Course Details.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetCourseSessionByRN_LicenseNumber(rn_LicenseNumber As String) As List(Of Objects.SessionCourseInfoDetails) Implements ITrainingSkillsPageQueires.GetCourseSessionByRN_LicenseNumber
            Dim retList As New List(Of Objects.SessionCourseInfoDetails)
            Try


                Using context As New MAISContext

                    retList = (From c In context.Courses _
                               Join s In context.Sessions On s.Course_Sid Equals c.Course_sid _
                               Join r In context.RNs On r.RN_Sid Equals c.RN_Sid _
                               Where r.RNLicense_Number = rn_LicenseNumber
                               Select New Objects.SessionCourseInfoDetails With {
                                   .RN_Sid = r.RN_Sid,
                                   .RN_Name = (r.First_Name & " " & r.Last_Name),
                                   .Course_Sid = c.Course_sid,
                                   .Session_sID = s.Session_Sid,
                                   .CourseNumber = c.OBN_Course_Number,
                                   .CourseDescription = c.Course_Description,
                                   .SessionLocation = s.Location_Name,
                                   .StartDate = s.Start_Date,
                                   .TotalCEs = c.Total_CEs}).ToList
                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in Training Skills services.", True, False))
                Me.LogError("Error while pulling Training Skills page Course Details.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetCourseSessionByRN_Name(rn_Name As String) As List(Of Objects.SessionCourseInfoDetails) Implements ITrainingSkillsPageQueires.GetCourseSessionByRN_Name
            Dim retList As New List(Of Objects.SessionCourseInfoDetails)
            Try


                Using context As New MAISContext

                    retList = (From c In context.Courses _
                               Join s In context.Sessions On s.Course_Sid Equals c.Course_sid _
                               Join r In context.RNs On r.RN_Sid Equals c.RN_Sid _
                               Where (r.First_Name & " " & r.Last_Name).Contains(rn_Name)
                               Select New Objects.SessionCourseInfoDetails With {
                                   .RN_Sid = r.RN_Sid,
                                   .RN_Name = (r.First_Name & " " & r.Last_Name),
                                   .Course_Sid = c.Course_sid,
                                   .Session_sID = s.Session_Sid,
                                   .CourseNumber = c.OBN_Course_Number,
                                   .CourseDescription = c.Course_Description,
                                   .SessionLocation = s.Location_Name,
                                   .StartDate = s.Start_Date,
                                   .TotalCEs = s.Total_CEs}).ToList
                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in Training Skills services.", True, False))
                Me.LogError("Error while pulling Training Skills page Course Details.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetCourseSessionByRN_LicenseNumber(rn_LicenseNumber As String, RoleCategoryLevelSid As Integer) As List(Of Objects.SessionCourseInfoDetails) Implements ITrainingSkillsPageQueires.GetCourseSessionByRN_LicenseNumber
            Dim retList As New List(Of Objects.SessionCourseInfoDetails)
            Try


                Using context As New MAISContext

                    retList = (From c In context.Courses _
                               Join s In context.Sessions On s.Course_Sid Equals c.Course_sid _
                               Join r In context.RNs On r.RN_Sid Equals c.RN_Sid _
                               Where r.RNLicense_Number = rn_LicenseNumber And c.Role_Category_Level_Sid = RoleCategoryLevelSid
                               Select New Objects.SessionCourseInfoDetails With {
                                   .RN_Sid = r.RN_Sid,
                                   .RN_Name = (r.First_Name & " " & r.Last_Name),
                                   .Course_Sid = c.Course_sid,
                                   .Session_sID = s.Session_Sid,
                                   .CourseNumber = c.OBN_Course_Number,
                                   .CourseDescription = c.Course_Description,
                                   .SessionLocation = s.Location_Name,
                                   .StartDate = s.Start_Date,
                                   .TotalCEs = s.Total_CEs}).ToList
                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in Training Skills services from GetCourseSessionByRN_LicenseNumber.", True, False))
                Me.LogError("Error while pulling Training Skills page Course Details from GetCourseSessionByRN_LicenseNumber.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetCourseSessionByRN_LicenseNumber(rn_LicenseNumber As String, RoleCategoryLevelSid As Integer, StartDate As Date) As List(Of Objects.SessionCourseInfoDetails) Implements ITrainingSkillsPageQueires.GetCourseSessionByRN_LicenseNumber
            Dim retList As New List(Of Objects.SessionCourseInfoDetails)
            Dim dateLess90 As Date = StartDate.AddDays(-90)
            Dim datePlus90 As Date = StartDate.AddDays(90)
            Try


                Using context As New MAISContext

                    retList = (From c In context.Courses _
                               Join s In context.Sessions On s.Course_Sid Equals c.Course_sid _
                               Join r In context.RNs On r.RN_Sid Equals c.RN_Sid _
                               Where r.RNLicense_Number = rn_LicenseNumber And c.Role_Category_Level_Sid = RoleCategoryLevelSid And s.Start_Date >= dateLess90 And s.Start_Date <= datePlus90
                               Select New Objects.SessionCourseInfoDetails With {
                                   .RN_Sid = r.RN_Sid,
                                   .RN_Name = (r.First_Name & " " & r.Last_Name),
                                   .Course_Sid = c.Course_sid,
                                   .Session_sID = s.Session_Sid,
                                   .CourseNumber = c.OBN_Course_Number,
                                   .CourseDescription = c.Course_Description,
                                   .SessionLocation = s.Location_Name,
                                   .StartDate = s.Start_Date,
                                   .TotalCEs = s.Total_CEs}).ToList
                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in Training Skills services from GetCourseSessionByRN_LicenseNumber.", True, False))
                Me.LogError("Error while pulling Training Skills page Course Details from GetCourseSessionByRN_LicenseNumber.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetCourseSessionByRN_Sid(RN_SID As Integer, RoleCategoryLevelSid As Integer) As List(Of Objects.SessionCourseInfoDetails) Implements ITrainingSkillsPageQueires.GetCourseSessionByRN_Sid
            Dim retList As New List(Of Objects.SessionCourseInfoDetails)
            Try


                Using context As New MAISContext

                    retList = (From c In context.Courses _
                               Join s In context.Sessions On s.Course_Sid Equals c.Course_sid _
                               Join r In context.RNs On r.RN_Sid Equals c.RN_Sid _
                               Where r.RN_Sid = RN_SID And c.Role_Category_Level_Sid = RoleCategoryLevelSid
                               Select New Objects.SessionCourseInfoDetails With {
                                   .RN_Sid = r.RN_Sid,
                                   .RN_Name = (r.First_Name & " " & r.Last_Name),
                                   .Course_Sid = c.Course_sid,
                                   .Session_sID = s.Session_Sid,
                                   .CourseNumber = c.OBN_Course_Number,
                                   .CourseDescription = c.Course_Description,
                                   .SessionLocation = s.Location_Name,
                                   .StartDate = s.Start_Date,
                                   .TotalCEs = s.Total_CEs}).ToList
                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in Training Skills services from GetCourseSessionByRN_Sid.", True, False))
                Me.LogError("Error while pulling Training Skills page Course Details from GetCourseSessionByRN_Sid.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetCourseSessionByRN_Name(rn_Name As String, RoleCategoryLevelSid As Integer) As List(Of Objects.SessionCourseInfoDetails) Implements ITrainingSkillsPageQueires.GetCourseSessionByRN_Name
            Dim retList As New List(Of Objects.SessionCourseInfoDetails)
            Try


                Using context As New MAISContext

                    retList = (From c In context.Courses _
                               Join s In context.Sessions On s.Course_Sid Equals c.Course_sid _
                               Join r In context.RNs On r.RN_Sid Equals c.RN_Sid _
                               Where (r.First_Name & " " & r.Last_Name).Contains(rn_Name) And c.Role_Category_Level_Sid = RoleCategoryLevelSid
                               Select New Objects.SessionCourseInfoDetails With {
                                   .RN_Sid = r.RN_Sid,
                                   .RN_Name = (r.First_Name & " " & r.Last_Name),
                                   .Course_Sid = c.Course_sid,
                                   .Session_sID = s.Session_Sid,
                                   .CourseNumber = c.OBN_Course_Number,
                                   .CourseDescription = c.Course_Description,
                                   .SessionLocation = s.Location_Name,
                                   .StartDate = s.Start_Date,
                                   .TotalCEs = s.Total_CEs}).ToList
                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in Training Skills services from GetCourseSessionByRN_Name.", True, False))
                Me.LogError("Error while pulling Training Skills page Course Details from GetCourseSessionByRN_Name.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetCourseSessionByRN_Name(rn_Name As String, RoleCategoryLevelSid As Integer, StartDate As Date) As List(Of Objects.SessionCourseInfoDetails) Implements ITrainingSkillsPageQueires.GetCourseSessionByRN_Name
            Dim retList As New List(Of Objects.SessionCourseInfoDetails)
            Try


                Using context As New MAISContext

                    retList = (From c In context.Courses _
                               Join s In context.Sessions On s.Course_Sid Equals c.Course_sid _
                               Join r In context.RNs On r.RN_Sid Equals c.RN_Sid _
                               Where (r.First_Name & " " & r.Last_Name).Contains(rn_Name) And c.Role_Category_Level_Sid = RoleCategoryLevelSid And s.Start_Date >= (StartDate.AddDays(-90)) And s.Start_Date <= (StartDate.AddDays(90))
                               Select New Objects.SessionCourseInfoDetails With {
                                   .RN_Sid = r.RN_Sid,
                                   .RN_Name = (r.First_Name & " " & r.Last_Name),
                                   .Course_Sid = c.Course_sid,
                                   .Session_sID = s.Session_Sid,
                                   .CourseNumber = c.OBN_Course_Number,
                                   .CourseDescription = c.Course_Description,
                                   .SessionLocation = s.Location_Name,
                                   .StartDate = s.Start_Date,
                                   .TotalCEs = s.Total_CEs}).ToList
                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in Training Skills services from GetCourseSessionByRN_Name.", True, False))
                Me.LogError("Error while pulling Training Skills page Course Details from GetCourseSessionByRN_Name.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function SaveCourseSessoin(AppID As Integer, SessionID As Integer, ByVal UserID As Integer) As Boolean Implements ITrainingSkillsPageQueires.SaveCourseSessoin
            Dim retVal As Boolean
            Dim appCourseInput As New Application_Course_Xref
            Dim appCourseSessionInput As New Application_Course_Session_Xref

            Dim hCourseID As Integer
            Using context As New MAISContext
                Try
                    hCourseID = (From c In context.Courses _
                                        Join s In context.Sessions On s.Course_Sid Equals c.Course_sid
                                        Where s.Session_Sid = SessionID _
                                        Select c.Course_sid).First




                    Dim _NewAppicationCourseXref As New Application_Course_Xref
                    With _NewAppicationCourseXref
                        .Course_Sid = hCourseID
                        .Application_Sid = AppID
                        .Active_Flg = 1
                        .Create_By = UserID
                        .Create_Date = Now()
                        .Last_Update_By = UserID
                        .Last_Update_Date = Now()
                    End With

                    Dim _NewApplicaitonCoursSessionXref As New Data.Application_Course_Session_Xref
                    With _NewApplicaitonCoursSessionXref
                        .Application_Course_Xref = _NewAppicationCourseXref
                        .Session_Sid = SessionID
                        .Active_Flg = 1
                        .Create_By = UserID
                        .Create_Date = Now()
                        .Last_Update_By = UserID
                        .Last_Update_Date = Now()

                    End With
                    context.Application_Course_Xref.Add(_NewAppicationCourseXref)
                    context.Application_Course_Session_Xref.Add(_NewApplicaitonCoursSessionXref)

                    'Below is the code to capture training ceus effected date in application history table JH 5/14/2013
                    If AppID > 0 Then
                        Dim existingAppHistory As Application_History = (From ah In context.Application_History
                                                                           Where ah.Application_Sid = AppID
                                                                           Select ah).FirstOrDefault()
                        If existingAppHistory IsNot Nothing Then
                            existingAppHistory.Training_CEUS_Date = Now()
                            existingAppHistory.Last_Update_By = Me.UserID
                            existingAppHistory.Last_Update_Date = Now()
                        End If
                    End If

                    retVal = context.SaveChanges()
                Catch ex As DbEntityValidationException
                    'strResult = ex.Message
                    'For Each eve As Object In ex.EntityValidationErrors
                    '    Dim s As String = eve.Entry.Enity.GetType().Name
                    '    Dim s1 As String = eve.Entry.State
                    '    For Each ve As Object In eve.ValidationErrors
                    '        Dim s2 As String = ve.PropertyName
                    '        Dim s3 As String = ve.ErrorMessage
                    '    Next
                    'Next
                    Me._messages.Add(New ReturnMessage("Error while saving Course Session to the Applicaiotn Query services.", True, False))
                    Me.LogError("Error while saving Course Session to the Applicaiotn in Query services.", CInt(Me.UserID), ex)
                    retVal = False
                End Try
            End Using

            Return retVal
        End Function


        Public Function DeleteCourseSession(AppID As Integer) As Boolean Implements ITrainingSkillsPageQueires.DeleteCourseSession
            Dim retVal As Boolean
            Try
                Using context As New MAISContext
                    Dim dApplicatin_Coures = (From c In context.Application_Course_Xref
                                              Where c.Application_Sid = AppID
                                              Select c).FirstOrDefault

                    Dim dApp_Coures_Session = (From s In context.Application_Course_Session_Xref
                                                Where s.Application_Course_Xref_Sid = dApplicatin_Coures.Application_Course_Xref_Sid
                                                Select s).FirstOrDefault


                    context.Application_Course_Session_Xref.Remove(dApp_Coures_Session)
                    context.Application_Course_Xref.Remove(dApplicatin_Coures)

                    retVal = context.SaveChanges()




                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while removing Course Session to the Applicaiotn Query services.", True, False))
                Me.LogError("Error while removing Course Session to the Applicaiotn in Query services.", CInt(Me.UserID), ex)
                retVal = False
            End Try
            Return retVal
        End Function

        Public Function GetTrainingPageHelper(AppID As Integer) As Integer Implements ITrainingSkillsPageQueires.GetTrainingPageHelper
            Dim flag As Integer = 0
            Using context As New MAISContext()
                Try
                    flag = (From app In context.Application_Course_Xref _
                                                            Where app.Application_Sid = AppID Select app).Count()
                    If (flag = 0) Then
                        flag = (From app In context.CEUs_Renewal_Application _
                                                            Where app.Application_Sid = AppID Select app).Count()
                    End If
                Catch ex As Exception
                    Me.LogError("Error Getting summary page complete rule.", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error summary page complete rule.", True, False))
                    Throw
                End Try
            End Using
            Return flag
        End Function

        Public Function GetTrainingPageTotalHrOfSession(AppID As Integer) As Double Implements ITrainingSkillsPageQueires.GetTrainingPageTotalHrOfSession
            Dim TotalHours As Double = 0.0
            Try


                Using context As New MAISContext

                    Dim RowCount As Integer
                    RowCount = (From s In context.Application_Course_Xref
                                  Join c In context.Courses On c.Course_sid Equals s.Course_Sid
                                  Where s.Application_Sid = AppID
                                  Select c.Total_CEs).Count

                    If RowCount > 0 Then
                        TotalHours = (From s In context.Application_Course_Xref
                                      Join c In context.Courses On c.Course_sid Equals s.Course_Sid
                                      Where s.Application_Sid = AppID
                                      Select c.Total_CEs).Sum
                    Else
                        TotalHours = 0.0
                    End If


                End Using
            Catch ex As Exception
                Me.LogError("Error getting sum of Course hours for completion rule.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting sum of Course hours for completion rule.", True, False))
            End Try
            Return TotalHours

        End Function

        Public Function GetTrainingPageTotalCEUs(AppID As Integer, RNDDUnique_Code As String) As Double Implements ITrainingSkillsPageQueires.GetTrainingPageTotalCEUs
            Dim TotalHours As Double = 0.0
            Try
                Using context As New MAISContext

                    Dim rnDDPerID As Integer
                    If RNDDUnique_Code.Contains("DD") Then
                        rnDDPerID = (From r In context.RN_DD_Person_Type_Xref _
                                                                    Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                                   Where ddP.DDPersonnel_Code = RNDDUnique_Code
                                                                   Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault

                    Else ' Search the RN Personnel
                        rnDDPerID = (From r In context.RN_DD_Person_Type_Xref _
                                                              Join RN In context.RNs On RN.RN_Sid Equals r.RN_DDPersonnel_Sid
                                                              Where RN.RNLicense_Number = RNDDUnique_Code
                                                              Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault
                    End If



                    TotalHours = (From e In context.CEUs_Renewal
                              Where (e.Application_Sid Is Nothing And e.RN_DD_Person_Type_Xref_Sid = rnDDPerID) OrElse
                              (e.Application_Sid = AppID And e.RN_DD_Person_Type_Xref_Sid = rnDDPerID)
                              Group By e.RN_Sid Into gp = Group
                              Select gp.Sum(Function(x) x.Total_CEUs)).FirstOrDefault



                End Using

            Catch ex As Exception
                Me.LogError("Error getting sum of CEUs hours for completion rule.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting sum of CEUs hours for completion rule.", True, False))
            End Try
            Return TotalHours

        End Function


        Public Function GetTrainingPageTotalCESs(AppID As Integer, RNDDUnique_Code As String, CategoryID As Integer) As Double Implements ITrainingSkillsPageQueires.GetTrainingPageTotalCESs
            Dim TotalHours As Double = 0.0
            Try
                Using context As New MAISContext

                    Dim rnDDPerID As Integer
                    If RNDDUnique_Code.Contains("DD") Then
                        rnDDPerID = (From r In context.RN_DD_Person_Type_Xref _
                                                                    Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                                   Where ddP.DDPersonnel_Code = RNDDUnique_Code
                                                                   Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault

                    Else ' Search the RN Personnel
                        rnDDPerID = (From r In context.RN_DD_Person_Type_Xref _
                                                              Join RN In context.RNs On RN.RN_Sid Equals r.RN_DDPersonnel_Sid
                                                              Where RN.RNLicense_Number = RNDDUnique_Code
                                                              Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault
                    End If


                    TotalHours = (From e In context.CEUs_Renewal
                              Where e.Application_Sid Is Nothing AndAlso e.RN_DD_Person_Type_Xref_Sid = rnDDPerID AndAlso e.Category_Type_Sid = CategoryID OrElse _
                              e.Application_Sid = AppID AndAlso e.RN_DD_Person_Type_Xref_Sid = rnDDPerID AndAlso e.Category_Type_Sid = CategoryID
                              Group By e.RN_Sid Into gp = Group
                              Select gp.Sum(Function(x) x.Total_CEUs)).FirstOrDefault



                End Using

            Catch ex As Exception
                Me.LogError("Error getting sum of CEUs hours for completion rule.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting sum of CEUs hours for completion rule.", True, False))
            End Try
            Return TotalHours

        End Function
#Region "CEU Renewal"
        Public Function GetCEUByUserID(UserID As String, CategoryTypeID As Integer, Optional AppID As Integer = -1) As List(Of Objects.CEUsDetailsObject) Implements ITrainingSkillsPageQueires.GetCEUByUserID
            Dim retval As New List(Of Objects.CEUsDetailsObject)
            Try
                Using context As New MAISContext
                    Dim rnDDPerID As Integer
                    'Dim certEndDate As DateTime

                    If UserID.Contains("DD") Then
                        rnDDPerID = (From r In context.RN_DD_Person_Type_Xref _
                                                                    Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                                   Where ddP.DDPersonnel_Code = UserID
                                                                   Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault

                    Else ' Search the RN Personnel
                        rnDDPerID = (From r In context.RN_DD_Person_Type_Xref _
                                                              Join RN In context.RNs On RN.RN_Sid Equals r.RN_DDPersonnel_Sid
                                                              Where RN.RNLicense_Number = UserID
                                                              Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault
                    End If
                    'Adding code to fetch cert end date to check to pull valid skill JH 7/9/2013
                    If (rnDDPerID > 0) Then
                        Dim certDate1 = (From role In context.Role_RN_DD_Personnel_Xref
                                                Join c In context.Certifications On role.Role_RN_DD_Personnel_Xref_Sid Equals c.Role_RN_DD_Personnel_Xref_Sid
                                                Join rlc In context.Role_Category_Level_Xref On rlc.Role_Category_Level_Sid Equals role.Role_Category_Level_Sid
                                                Where role.RN_DD_Person_Type_Xref_Sid = rnDDPerID And rlc.Category_Type_Sid = CategoryTypeID
                                                Select c.Certification_End_Date, c.Certification_Start_Date).FirstOrDefault()

                        retval = (From c In context.CEUs_Renewal _
                                  Where c.RN_DD_Person_Type_Xref_Sid = rnDDPerID And c.Application_Sid Is Nothing And c.Category_Type_Sid = CategoryTypeID And c.End_Date <= certDate1.Certification_End_Date And c.End_Date >= certDate1.Certification_Start_Date _
                                        Or (c.RN_DD_Person_Type_Xref_Sid = rnDDPerID And c.Application_Sid = AppID)
                                        Select New Objects.CEUsDetailsObject With {
                                      .CEUs_Renewal_Sid = c.CEUs_Renewal_Sid,
                                      .Application_Sid = c.Application_Sid,
                                      .Permanent_Flg = c.Permanent_Flg,
                                      .Start_Date = c.Start_Date,
                                      .End_Date = c.End_Date,
                                      .Category_Type_Sid = c.Category_Type_Sid,
                                      .Category_Type_Code = c.Category_Type.Category_Code,
                                      .Attended_Date = c.Attended_Date,
                                      .Total_CEUs = c.Total_CEUs,
                                      .RN_Sid = c.RN_Sid,
                                      .RN_Name = c.RN.Last_Name & ", " & c.RN.First_Name,
                                      .Instructor_Name = c.Instructor_Name,
                                      .Title = c.Title,
                                      .Course_Description = c.Course_Description,
                                      .Active_Flag = c.Active_Flg}).ToList
                    End If
                End Using
                Return retval

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while pulling CEU for person from Training Query services.", True, False))
                Me.LogError("Error while pulling CEUs for person from Training Query services.", CInt(Me.UserID), ex)
                Return retval
            End Try
        End Function
        Public Function GetAllCEUByUserID(UserID As String, CategoryTypeID As Integer, Optional AppID As Integer = -1) As List(Of Objects.CEUsDetailsObject) Implements ITrainingSkillsPageQueires.GetAllCEUByUserID
            Dim retval As New List(Of Objects.CEUsDetailsObject)
            Try
                Using context As New MAISContext
                    Dim rnDDPerID As Integer
                    'Dim certEndDate As DateTime

                    If UserID.Contains("DD") Then
                        rnDDPerID = (From r In context.RN_DD_Person_Type_Xref _
                                                                    Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                                   Where ddP.DDPersonnel_Code = UserID
                                                                   Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault

                    Else ' Search the RN Personnel
                        rnDDPerID = (From r In context.RN_DD_Person_Type_Xref _
                                                              Join RN In context.RNs On RN.RN_Sid Equals r.RN_DDPersonnel_Sid
                                                              Where RN.RNLicense_Number = UserID
                                                              Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault
                    End If
                    'Adding code to fetch cert end date to check to pull valid skill JH 7/9/2013
                    If (rnDDPerID > 0) Then

                        retval = (From c In context.CEUs_Renewal _
                                  Where (c.RN_DD_Person_Type_Xref_Sid = rnDDPerID And c.Application_Sid Is Nothing And c.Category_Type_Sid = CategoryTypeID) Or (c.Application_Sid = AppID And c.RN_DD_Person_Type_Xref_Sid = rnDDPerID)
                                     Select New Objects.CEUsDetailsObject With {
                                      .CEUs_Renewal_Sid = c.CEUs_Renewal_Sid,
                                      .Application_Sid = c.Application_Sid,
                                      .Permanent_Flg = c.Permanent_Flg,
                                      .Start_Date = c.Start_Date,
                                      .End_Date = c.End_Date,
                                      .Category_Type_Sid = c.Category_Type_Sid,
                                      .Category_Type_Code = c.Category_Type.Category_Code,
                                      .Attended_Date = c.Attended_Date,
                                      .Total_CEUs = c.Total_CEUs,
                                      .RN_Sid = c.RN_Sid,
                                      .RN_Name = c.RN.Last_Name & ", " & c.RN.First_Name,
                                      .Instructor_Name = c.Instructor_Name,
                                      .Title = c.Title,
                                      .Course_Description = c.Course_Description,
                                      .Active_Flag = c.Active_Flg}).ToList
                    End If
                End Using
                Return retval

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while pulling CEU for person from Training Query services.", True, False))
                Me.LogError("Error while pulling CEUs for person from Training Query services.", CInt(Me.UserID), ex)
                Return retval
            End Try
        End Function

        Public Function GetCategoryByRoleCategoryLevelSid(RoleCategoryLevelSid As Integer) As Category_Type Implements ITrainingSkillsPageQueires.GetCategoryByRoleCategoryLevelSid
            Dim retval As New Data.Category_Type
            Try
                Using context As New MAISContext
                    retval = (From RL In context.Role_Category_Level_Xref _
                              Join CT In context.Category_Type On RL.Category_Type_Sid Equals CT.Category_Type_Sid
                              Where RL.Role_Category_Level_Sid = RoleCategoryLevelSid
                              Select CT).FirstOrDefault
                End Using
                Return retval


            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while pulling Category Type form Training services.", True, False))
                Me.LogError("Error while  pulling Category Type form Training in Query services.", CInt(Me.UserID), ex)
                Return retval
            End Try
        End Function

        Public Function SaveCEUDetail(UserSavingID As Integer, CEUList As List(Of Objects.CEUsDetailsObject), AppID As Integer) As Boolean Implements ITrainingSkillsPageQueires.SaveCEUDetail
            Dim retval As Boolean
            Try
                Using context As New MAISContext
                    Dim NewCEU_Renewal As New Data.CEUs_Renewal

                    For Each ce In CEUList
                        Dim NewCe As Data.CEUs_Renewal

                        NewCe = (From f In context.CEUs_Renewal
                                     Where f.CEUs_Renewal_Sid = ce.CEUs_Renewal_Sid
                                     Select f).FirstOrDefault

                        If NewCe Is Nothing Then     'This is the new insert
                            NewCe = New Data.CEUs_Renewal
                            With NewCe
                                .Application_Sid = ce.Application_Sid
                                .Permanent_Flg = ce.Permanent_Flg
                                .Start_Date = ce.Start_Date
                                .End_Date = ce.End_Date
                                If ce.DD_RN_Personnel_SID.Contains("DD") Then
                                    .RN_DD_Person_Type_Xref_Sid = ((From r In context.RN_DD_Person_Type_Xref _
                                                                    Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                                   Where ddP.DDPersonnel_Code = ce.DD_RN_Personnel_SID
                                                                   Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault)
                                Else
                                    .RN_DD_Person_Type_Xref_Sid = ((From r In context.RN_DD_Person_Type_Xref _
                                                                  Join RN In context.RNs On RN.RN_Sid Equals r.RN_DDPersonnel_Sid
                                                                  Where RN.RNLicense_Number = ce.DD_RN_Personnel_SID
                                                                  Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault)
                                End If
                                .Category_Type_Sid = ce.Category_Type_Sid
                                .Attended_Date = ce.Attended_Date
                                .Total_CEUs = ce.Total_CEUs
                                .RN_Sid = ce.RN_Sid
                                .Instructor_Name = ce.Instructor_Name
                                .Title = ce.Title
                                .Course_Description = ce.Course_Description
                                .Active_Flg = True
                                .Create_By = UserSavingID
                                .Create_Date = Now()
                                .Last_Update_By = UserSavingID
                                .Last_Update_Date = Now()
                            End With
                            context.CEUs_Renewal.Add(NewCe)

                        Else 'This is the update

                            With NewCe
                                .Application_Sid = ce.Application_Sid
                                .Permanent_Flg = ce.Permanent_Flg
                                .Start_Date = ce.Start_Date
                                .End_Date = ce.End_Date
                                If ce.DD_RN_Personnel_SID.Contains("DD") Then
                                    .RN_DD_Person_Type_Xref_Sid = ((From r In context.RN_DD_Person_Type_Xref _
                                                                    Join ddP In context.DDPersonnels On ddP.DDPersonnel_Sid Equals r.RN_DDPersonnel_Sid
                                                                   Where ddP.DDPersonnel_Code = ce.DD_RN_Personnel_SID
                                                                   Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault)
                                Else
                                    .RN_DD_Person_Type_Xref_Sid = ((From r In context.RN_DD_Person_Type_Xref _
                                                                  Join RN In context.RNs On RN.RN_Sid Equals r.RN_DDPersonnel_Sid
                                                                  Where RN.RNLicense_Number = ce.DD_RN_Personnel_SID
                                                                  Select r.RN_DD_Person_Type_Xref_Sid).FirstOrDefault)
                                End If

                                .Category_Type_Sid = ce.Category_Type_Sid
                                .Attended_Date = ce.Attended_Date
                                .Total_CEUs = ce.Total_CEUs
                                .RN_Sid = ce.RN_Sid
                                .Instructor_Name = ce.Instructor_Name
                                .Title = ce.Title
                                .Course_Description = ce.Course_Description
                                .Active_Flg = ce.Active_Flag
                                .Last_Update_By = UserSavingID
                                .Last_Update_Date = Now()
                            End With
                        End If
                    Next

                    'Need to capture application history for training and CEUS JH 5/14/2013

                    If ((CEUList.Count > 0) And (AppID > 0)) Then
                        Dim AppHis As Application_History = (From ah In context.Application_History
                                                             Where ah.Application_Sid = AppID
                                                             Select ah).FirstOrDefault()
                        If AppHis IsNot Nothing Then
                            AppHis.Training_CEUS_Date = DateTime.Now
                            AppHis.Last_Update_By = Me.UserID
                            AppHis.Last_Update_Date = DateTime.Now
                        End If
                    End If

                    retval = context.SaveChanges

                End Using
                Return retval

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while saving CEUs from Training services.", True, False))
                Me.LogError("Error while saving CEUs from Training in Query services.", CInt(Me.UserID), ex)
                Return False
            End Try
        End Function
#End Region



        Public Function DeleteCEUByID(CEU_Sid As Integer) As Boolean Implements ITrainingSkillsPageQueires.DeleteCEUByID
            Dim retVal As Boolean = False
            Try
                Using context As New MAISContext
                    Dim CEU = (From c In context.CEUs_Renewal _
                               Where c.CEUs_Renewal_Sid = CEU_Sid
                               Select c).FirstOrDefault

                    context.CEUs_Renewal.Remove(CEU)
                    retVal = context.SaveChanges


                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while removing CEUs from Training services.", True, False))
                Me.LogError("Error while removing CEUs from Training in Query services.", CInt(Me.UserID), ex)
                Return False
            End Try
            Return retVal
        End Function



        Public Function GetSecretaryRNS(SecretaryUserID As Integer) As List(Of RN) Implements ITrainingSkillsPageQueires.GetSecretaryRNS
            Dim retVal As New List(Of Data.RN)
            Try
                Using resource As New MAISContext
                    retVal = (From r In resource.RNs
                              Join c In resource.RN_Secretary_Association On c.RN_Sid Equals r.RN_Sid
                              Join u In resource.User_Mapping On u.User_Mapping_Sid Equals c.User_Mapping_Sid
                              Where u.UserID = SecretaryUserID And c.Active_Flg = True
                              Select r).ToList


                End Using
                Return retVal

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while pulling Secretary RNs from GetSecretaryRNS services.", True, False))
                Me.LogError("Error while pulling Secretary RNs from GetSecretaryRNS Query services.", CInt(Me.UserID), ex)
                Return retVal
            End Try
        End Function


    End Class
End Namespace

