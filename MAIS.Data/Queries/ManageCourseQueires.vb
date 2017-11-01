Imports ODMRDDHelperClassLibrary.Utility
Imports System.Configuration
Imports System.Data.Objects
Imports System.Data.Entity.Validation


Namespace Queries
    Public Interface IManageCourseQueires
        Inherits IQueriesBase
        Function SaveMangeCourse(ByVal Course As Objects.CourseDetails) As Integer
        Function GetLevelsByRoleID(ByVal RoleID As Integer) As List(Of Data.Level_Type)
        Function GetCategories(ByVal LevelID As Integer) As List(Of Data.Category_Type)
        Function getCourseByCourseID(ByVal CourseID As Integer) As List(Of Objects.CourseDetails)
        Function GetAllCourseAndSessionInfoByCourseID(ByVal CourseID As Integer) As List(Of Objects.CourseDetails)
        Function DoesCourseExitAlready(ByVal CourseNumber As String) As Boolean
        Function DoeseCourseSessionOverLap(ByVal CourseNumber As String, ByVal SessionStartDate As Date, ByVal SessionEndDate As Date) As Boolean
        Function SearchCourseByRN(ByVal RN_Number As String) As List(Of Objects.CourseDetails)
        Function SearchCourseByFirstName(ByVal Name As String) As List(Of Objects.CourseDetails)
        Function SearchCourseByLastName(ByVal Name As String) As List(Of Objects.CourseDetails)
        Function SearchSessionByStartDate(ByVal StartDate As Date) As List(Of Objects.CourseDetails)
        Function SearchMangeCourse(Optional ByVal RNNumber As String = Nothing, Optional ByVal FirstName As String = Nothing, Optional ByVal LastName As String = Nothing, Optional ByVal SessionStartDate As Date = Nothing) As List(Of Objects.CourseDetails)
        Function DeleteSessionBySessionSid(ByVal SessionSid As Integer) As Boolean
    End Interface
    Public Class ManageCourseQueires
        Inherits QueriesBase
        Implements IManageCourseQueires


        Public Function SaveMangeCourse(Course As Objects.CourseDetails) As Integer Implements IManageCourseQueires.SaveMangeCourse
            Dim retVal As Boolean = False
            Dim RetCourseID As Integer = -1
            Using context As New MAISContext
                Try


                    'Need to save the coures first
                    Dim c As New Data.Course
                    c = (From mC In context.Courses _
                         Where mC.Course_sid = Course.Course_Sid Or mC.OBN_Course_Number = Course.OBNApprovalNumber
                         Select mC).FirstOrDefault

                    If c Is Nothing Then
                        c = New Data.Course
                        With c
                            .RN_Sid = Course.RN_Sid
                            .Role_Category_Level_Sid = (From rclx In context.Role_Category_Level_Xref Where rclx.Level_Type_Sid = Course.Level And rclx.Category_Type_Sid = Course.Category Select rclx.Role_Category_Level_Sid).FirstOrDefault
                            .Start_Date = Course.StartDate
                            .End_Date = Course.EndDate
                            If String.IsNullOrWhiteSpace(Course.OBNApprovalNumber) Then
                                'DD Personal was selected
                                .OBN_Course_Number = CreateDODDCourseNumber(Course.RN_Sid, Course.Level, Course.Category)
                            Else
                                'RN Selected was selected
                                .OBN_Course_Number = Course.OBNApprovalNumber
                            End If

                            .Category_A_CEs = Course.CategoryACEs
                            .Total_CEs = Course.TotalCEs
                            .Course_Description = Course.CourseDescription
                            .Create_By = UserID
                            .Create_Date = Now()
                            .Last_Update_By = UserID
                            .Last_Update_Date = Now()
                        End With
                        context.Courses.Add(c)

                    End If
                    If Not (Course.SessionDetailList Is Nothing) Then
                        For Each SDL In Course.SessionDetailList
                            Dim SD As New Data.Session
                            With SD
                                .Course = c
                                .Start_Date = SDL.Session_Start_Date
                                .End_Date = SDL.Session_End_Date
                                .Sponsor = SDL.Sponsor
                                .Location_Name = SDL.Location_Name
                                .Total_CEs = SDL.Total_CEs
                                .Public_Access_Flg = SDL.Public_Access_Flg
                                .Active_Flg = True
                                .Create_By = UserID
                                .Create_Date = Now()
                                .Last_Update_By = UserID
                                .Last_Update_Date = Now()
                            End With

                            'Test for address in the new Shard Address table. This will replace the MAIS Address Tables.
                            Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", GetType(Integer))
                            Dim _AddressSharedExist As List(Of Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(parameter, String.Empty, SDL.SessionAddressInfo.Address_Line1, SDL.SessionAddressInfo.Address_Line2, String.Empty, Convert.ToInt32(SDL.SessionAddressInfo.CountyID) _
                                                                                                                          , Convert.ToInt32(SDL.SessionAddressInfo.StateID), SDL.SessionAddressInfo.Zip_Code + SDL.SessionAddressInfo.Zip_Code_Plus4, SDL.SessionAddressInfo.City, 0).ToList()


                            ''Test of address 
                            'Dim _AddressExist As New Data.Address
                            '_AddressExist = (From adexit In context.Addresses _
                            '                 Where adexit.Address_Line1 = Trim(SDL.SessionAddressInfo.Address_Line1) And _
                            '                 adexit.Address_Line2 = Trim(SDL.SessionAddressInfo.Address_Line2) And _
                            '                 adexit.City = SDL.SessionAddressInfo.City And _
                            '                 adexit.State = SDL.SessionAddressInfo.State And _
                            '                 adexit.Zip_Code = SDL.SessionAddressInfo.Zip_Code And _
                            '                 adexit.Zip_Code_Plus4 = SDL.SessionAddressInfo.Zip_Code_Plus4
                            '                 Select adexit).FirstOrDefault

                            'If _AddressSharedExist Is Nothing Then
                            '    'Need to add new address 
                            '    Dim newAddress As New Data.Address
                            '    With newAddress
                            '        .Address_Line1 = SDL.SessionAddressInfo.Address_Line1
                            '        .Address_Line2 = SDL.SessionAddressInfo.Address_Line2
                            '        .City = SDL.SessionAddressInfo.City
                            '        .State = SDL.SessionAddressInfo.State
                            '        .Zip_Code = SDL.SessionAddressInfo.Zip_Code
                            '        .Zip_Code_Plus4 = SDL.SessionAddressInfo.Zip_Code_Plus4
                            '        .County = SDL.SessionAddressInfo.County
                            '        .Create_By = UserID
                            '        .Create_Date = Now()
                            '        .Last_Update_By = UserID
                            '        .Last_Update_Date = Now()
                            '    End With
                            '    context.Addresses.Add(newAddress)

                            '    'Add to SessionAddressLink 
                            '    Dim SDALink As New Data.Session_Address_Xref
                            '    With SDALink
                            '        .Address = newAddress
                            '        .Session = SD
                            '        .Address_Type_Sid = 6
                            '        .Active_Flg = True
                            '        .Start_Date = Now()
                            '        .End_Date = CDate("1/1/2999")
                            '        .Create_By = UserID
                            '        .Create_Date = Now()
                            '        .Last_Update_By = UserID
                            '        .Last_Update_Date = Now()
                            '    End With
                            '    context.Session_Address_Xref.Add(SDALink)

                            'Else
                            Dim SDALink As New Data.Session_Address_Xref
                            With SDALink
                                .Address_Sid = parameter.Value
                                .Session = SD
                                .Address_Type_Sid = 6
                                .Active_Flg = True
                                .Start_Date = Now()
                                .End_Date = CDate("1/1/2999")
                                .Create_By = UserID
                                .Create_Date = Now()
                                .Last_Update_By = UserID
                                .Last_Update_Date = Now()
                            End With
                            context.Session_Address_Xref.Add(SDALink)
                            'End If
                            context.Sessions.Add(SD)

                            For Each SDIList In SDL.SessionInformationDetailsList
                                Dim sdi As New Data.Session_Information
                                With sdi
                                    .Session = SD
                                    .Session_Date = SDIList.Session_Date
                                    .Total_CEs = SDIList.Total_CEs
                                    .Active_Flg = True
                                    .Create_By = UserID
                                    .Create_Date = Now()
                                    .Last_Update_By = UserID
                                    .Last_Update_Date = Now()
                                End With
                                context.Session_Information.Add(sdi)
                            Next
                        Next

                    End If
                    retVal = context.SaveChanges()
                    If retVal = True Then
                        RetCourseID = c.Course_sid
                    Else
                        RetCourseID = -1
                    End If



                Catch ex As Exception
                    Me._messages.Add(New ReturnMessage("Error while saving Course Session from the Manage Course Query services.", True, False))
                    Me.LogError("Error while saving Course Session from the Manage Course Query services.", CInt(Me.UserID), ex)
                    Me.LogError("Error while saving Course Session from the Manage Course Query services.", CInt(Me.UserID), ex.InnerException)
                    retVal = False
                    RetCourseID = -1
                End Try
            End Using

            Return RetCourseID

        End Function

        Public Function GetCategories(LevelID As Integer) As List(Of Category_Type) Implements IManageCourseQueires.GetCategories
            'This is to return the Category with link to the Level ids
            Dim _Category As New List(Of Data.Category_Type)
            Try

                Using context As New MAISContext
                    _Category = (From mC In context.Category_Type _
                                 Join mRC In context.Role_Category_Level_Xref On mRC.Category_Type_Sid Equals mC.Category_Type_Sid
                                 Where mRC.Level_Type_Sid = LevelID
                                 Select mC Distinct).ToList
                End Using
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error GetCategories.", True, False))
                Me.LogError("Error GetCategories.", CInt(Me.UserID), ex)
            End Try

            Return _Category
        End Function

        Public Function GetLevelsByRoleID(RoleID As Integer) As List(Of Level_Type) Implements IManageCourseQueires.GetLevelsByRoleID
            'using this for the Manage Course roleID 
            ' if Role ID = 2  This is to pull RN and 17Bed Level Codes
            '   This is Level_type_SID of {2,6} 
            ' if role ID = 3 This is for the DD Personnel
            '   this is Level_type_Sid of {3}

            Dim _Level As New List(Of Data.Level_Type)
            Try
                Using context As New MAISContext
                    Select Case RoleID
                        Case 2
                            _Level = (From mL In context.Level_Type _
                            Where mL.Level_Type_Sid = 2 Or mL.Level_Type_Sid = 6 _
                            Select mL).ToList
                        Case 3
                            _Level = (From mL In context.Level_Type _
                         Where mL.Level_Type_Sid = 3 _
                         Select mL).ToList
                    End Select
                End Using
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error GetLevelsByRoleID.", True, False))
                Me.LogError("Error GetLevelsByRoleID.", CInt(Me.UserID), ex)
            End Try

            Return _Level

        End Function

        Public Function getCourseByCourseID(CourseID As Integer) As List(Of Objects.CourseDetails) Implements IManageCourseQueires.getCourseByCourseID
            Dim retVal As New List(Of Objects.CourseDetails)
            Try
                Using Context As New MAISContext


                    retVal = (From c In Context.Courses _
                              Where c.Course_sid = CourseID
                              Select New Objects.CourseDetails With {.Course_Sid = c.Course_sid,
                                                                     .RN_Sid = c.RN_Sid,
                                                                     .InstructorName = (From rnName In Context.RNs Where rnName.RN_Sid = c.RN_Sid Select (rnName.First_Name & " " & rnName.Last_Name)).FirstOrDefault,
                                                                     .Role_Calegory_Level_Sid = c.Role_Category_Level_Sid,
                                                                     .StartDate = c.Start_Date,
                                                                     .EndDate = c.End_Date,
                                                                     .OBNApprovalNumber = c.OBN_Course_Number,
                                                                     .CategoryACEs = c.Category_A_CEs,
                                                                     .TotalCEs = c.Total_CEs,
                                                                     .CourseDescription = c.Course_Description,
                                                                     .Level = (From l In Context.Role_Category_Level_Xref Where l.Role_Category_Level_Sid = c.Role_Category_Level_Sid Select l.Level_Type_Sid).FirstOrDefault,
                                                                     .Category = (From CL In Context.Role_Category_Level_Xref Where CL.Role_Category_Level_Sid = c.Role_Category_Level_Sid Select CL.Category_Type_Sid).FirstOrDefault}).ToList

                End Using
                Return retVal


            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in Manage Course services.", True, False))
                Me.LogError("Error while pulling Manage Course page Course Details.", CInt(Me.UserID), ex)
                Return retVal
            End Try
        End Function


        Public Function SearchCourseByRN(RN_Number As String) As List(Of Objects.CourseDetails) Implements IManageCourseQueires.SearchCourseByRN
            Dim retList As New List(Of Objects.CourseDetails)
            Try


                Using context As New MAISContext

                    For Each AppCourseSession As Data.Course In (From ACS In context.Courses _
                                                                                            Join RN In context.RNs On RN.RN_Sid Equals ACS.RN_Sid _
                                                                                            Where RN.RNLicense_Number = RN_Number
                                                                                            Select ACS).ToList()


                        For Each C As Data.Course In (From a In context.Courses Where a.Course_sid = AppCourseSession.Course_sid Select a)
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
                            'hCourse.Category = (From CL In context.Role_Category_Level_Xref Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select CL.Category_Type_Sid).FirstOrDefault
                            hCourse.Category = (From CL In context.Role_Category_Level_Xref Join lkC In context.Category_Type On lkC.Category_Type_Sid Equals CL.Category_Type_Sid Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select lkC.Category_Code).FirstOrDefault

                            Dim CourseSessionList As New List(Of Objects.SessionAddressInformation)
                            For Each S As Data.Session In (From cs In context.Sessions Where cs.Course_Sid = AppCourseSession.Course_sid)
                                Dim CourseSession As New Objects.SessionAddressInformation
                                CourseSession.Session_Sid = S.Session_Sid
                                CourseSession.Course_SID = S.Course_Sid
                                CourseSession.Session_Start_Date = S.Start_Date
                                CourseSession.Session_End_Date = S.End_Date
                                CourseSession.Sponsor = S.Sponsor
                                CourseSession.Location_Name = S.Location_Name
                                CourseSession.Total_CEs = S.Total_CEs
                                CourseSession.Public_Access_Flg = S.Public_Access_Flg
                                Dim sal As New Data.Session_Address_Xref
                                sal = (From dsal In context.Session_Address_Xref Where dsal.Session_Sid = S.Session_Sid Select dsal).FirstOrDefault

                                'Dim sad As New Data.Address
                                'sad = (From dsad In context.Addresses Where dsad.Address_Sid = sal.Address_Sid Select dsad).FirstOrDefault
                                Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", sal.Address_Sid)
                                Dim sad As List(Of MAIS.Data.Address_Lookup_And_Insert_Result) = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).ToList


                                Dim SessonAddress As New Objects.SessionAddress
                                With SessonAddress
                                    .Address_Sid = sal.Address_Sid
                                    .Address_Line1 = sad(0).Address_Line1
                                    .Address_Line2 = sad(0).Address_Line2
                                    .City = sad(0).City
                                    .State = sad(0).State_Abbr
                                    If sad(0).Zip.Length > 5 Then
                                        .Zip_Code = Mid(sad(0).Zip, 1, 5)
                                        .Zip_Code_Plus4 = Mid(sad(0).Zip, 6, 4)
                                    Else
                                        .Zip_Code = sad(0).Zip
                                    End If
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
                Me._messages.Add(New ReturnMessage("Error in Manage Course services.", True, False))
                Me.LogError("Error while pulling Manage Course page Course Details.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function SearchCourseByFirstName(Name As String) As List(Of Objects.CourseDetails) Implements IManageCourseQueires.SearchCourseByFirstName
            Dim retList As New List(Of Objects.CourseDetails)


            Try


                Using context As New MAISContext

                    For Each AppCourseSession As Data.Course In (From ACS In context.Courses _
                                                                Join RN In context.RNs On RN.RN_Sid Equals ACS.RN_Sid _
                                                                Where RN.First_Name.Contains(Name)
                                                                Select ACS).ToList()


                        For Each C As Data.Course In (From a In context.Courses Where a.Course_sid = AppCourseSession.Course_sid Select a)
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
                            'hCourse.Category = (From CL In context.Role_Category_Level_Xref Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select CL.Category_Type_Sid).FirstOrDefault
                            hCourse.Category = (From CL In context.Role_Category_Level_Xref Join lkC In context.Category_Type On lkC.Category_Type_Sid Equals CL.Category_Type_Sid Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select lkC.Category_Code).FirstOrDefault

                            Dim CourseSessionList As New List(Of Objects.SessionAddressInformation)
                            For Each S As Data.Session In (From cs In context.Sessions Where cs.Course_Sid = AppCourseSession.Course_sid)
                                Dim CourseSession As New Objects.SessionAddressInformation
                                CourseSession.Session_Sid = S.Session_Sid
                                CourseSession.Course_SID = S.Course_Sid
                                CourseSession.Session_Start_Date = S.Start_Date
                                CourseSession.Session_End_Date = S.End_Date
                                CourseSession.Sponsor = S.Sponsor
                                CourseSession.Location_Name = S.Location_Name
                                CourseSession.Total_CEs = S.Total_CEs
                                CourseSession.Public_Access_Flg = S.Public_Access_Flg
                                Dim sal As New Data.Session_Address_Xref
                                sal = (From dsal In context.Session_Address_Xref Where dsal.Session_Sid = S.Session_Sid Select dsal).FirstOrDefault
                                'Dim sad As New Data.Address
                                'sad = (From dsad In context.Addresses Where dsad.Address_Sid = sal.Address_Sid Select dsad).FirstOrDefault
                                Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", sal.Address_Sid)
                                Dim sad As MAIS.Data.Address_Lookup_And_Insert_Result = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).FirstOrDefault


                                Dim SessonAddress As New Objects.SessionAddress
                                With SessonAddress
                                    .Address_Sid = sal.Address_Sid
                                    .Address_Line1 = sad.Address_Line1
                                    .Address_Line2 = sad.Address_Line2
                                    .City = sad.City
                                    .State = sad.State_Abbr
                                    If sad.Zip > 5 Then
                                        .Zip_Code = Mid(sad.Zip, 1, 5)
                                        .Zip_Code_Plus4 = Mid(sad.Zip, 6, 4)
                                    Else
                                        .Zip_Code = sad.Zip
                                    End If
                                    '.Zip_Code = sad.Zip_Code
                                    '.Zip_Code_Plus4 = sad.Zip_Code_Plus4
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
                Me._messages.Add(New ReturnMessage("Error in Manage Course services.", True, False))
                Me.LogError("Error while pulling Manage Course page Course Details.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function SearchCourseByLastName(Name As String) As List(Of Objects.CourseDetails) Implements IManageCourseQueires.SearchCourseByLastName
            Dim retList As New List(Of Objects.CourseDetails)


            Try


                Using context As New MAISContext

                    For Each AppCourseSession As Data.Course In (From ACS In context.Courses _
                                                                Join RN In context.RNs On RN.RN_Sid Equals ACS.RN_Sid _
                                                                Where RN.Last_Name.Contains(Name)
                                                                Select ACS).ToList()


                        For Each C As Data.Course In (From a In context.Courses Where a.Course_sid = AppCourseSession.Course_sid Select a)
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
                            'hCourse.Category = (From CL In context.Role_Category_Level_Xref Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select CL.Category_Type_Sid).FirstOrDefault
                            hCourse.Category = (From CL In context.Role_Category_Level_Xref Join lkC In context.Category_Type On lkC.Category_Type_Sid Equals CL.Category_Type_Sid Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select lkC.Category_Code).FirstOrDefault

                            Dim CourseSessionList As New List(Of Objects.SessionAddressInformation)
                            For Each S As Data.Session In (From cs In context.Sessions Where cs.Course_Sid = AppCourseSession.Course_sid)
                                Dim CourseSession As New Objects.SessionAddressInformation
                                CourseSession.Session_Sid = S.Session_Sid
                                CourseSession.Course_SID = S.Course_Sid
                                CourseSession.Session_Start_Date = S.Start_Date
                                CourseSession.Session_End_Date = S.End_Date
                                CourseSession.Sponsor = S.Sponsor
                                CourseSession.Location_Name = S.Location_Name
                                CourseSession.Total_CEs = S.Total_CEs
                                CourseSession.Public_Access_Flg = S.Public_Access_Flg
                                Dim sal As New Data.Session_Address_Xref
                                sal = (From dsal In context.Session_Address_Xref Where dsal.Session_Sid = S.Session_Sid Select dsal).FirstOrDefault
                                'Dim sad As New Data.Address
                                'sad = (From dsad In context.Addresses Where dsad.Address_Sid = sal.Address_Sid Select dsad).FirstOrDefault
                                Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", sal.Address_Sid)
                                Dim sad As MAIS.Data.Address_Lookup_And_Insert_Result = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).FirstOrDefault


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
                Me._messages.Add(New ReturnMessage("Error in Manage Course services.", True, False))
                Me.LogError("Error while pulling Manage Course page Course Details.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetAllCourseAndSessionInfoByCourseID(CourseID As Integer) As List(Of Objects.CourseDetails) Implements IManageCourseQueires.GetAllCourseAndSessionInfoByCourseID
            Dim retList As New List(Of Objects.CourseDetails)


            Try


                Using context As New MAISContext

                    For Each AppCourseSession As Data.Course In (From ACS In context.Courses _
                                                                Where ACS.Course_sid = CourseID
                                                                Select ACS).ToList()


                        For Each C As Data.Course In (From a In context.Courses Where a.Course_sid = AppCourseSession.Course_sid Select a)
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
                            'hCourse.Category = (From CL In context.Role_Category_Level_Xref Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select CL.Category_Type_Sid).FirstOrDefault
                            hCourse.Category = (From CL In context.Role_Category_Level_Xref Join lkC In context.Category_Type On lkC.Category_Type_Sid Equals CL.Category_Type_Sid Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select lkC.Category_Code).FirstOrDefault

                            Dim CourseSessionList As New List(Of Objects.SessionAddressInformation)
                            For Each S As Data.Session In (From cs In context.Sessions Where cs.Course_Sid = AppCourseSession.Course_sid)
                                Dim CourseSession As New Objects.SessionAddressInformation
                                CourseSession.Session_Sid = S.Session_Sid
                                CourseSession.Course_SID = S.Course_Sid
                                CourseSession.Session_Start_Date = S.Start_Date
                                CourseSession.Session_End_Date = S.End_Date
                                CourseSession.Sponsor = S.Sponsor
                                CourseSession.Location_Name = S.Location_Name
                                CourseSession.Total_CEs = S.Total_CEs
                                CourseSession.Public_Access_Flg = S.Public_Access_Flg
                                Dim sal As New Data.Session_Address_Xref
                                sal = (From dsal In context.Session_Address_Xref Where dsal.Session_Sid = S.Session_Sid Select dsal).FirstOrDefault
                                'Dim sad As New Data.Address
                                'sad = (From dsad In context.Addresses Where dsad.Address_Sid = sal.Address_Sid Select dsad).FirstOrDefault
                                Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", sal.Address_Sid)
                                Dim sad As MAIS.Data.Address_Lookup_And_Insert_Result = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).FirstOrDefault


                                Dim SessonAddress As New Objects.SessionAddress
                                With SessonAddress
                                    .Address_Sid = sal.Address_Sid
                                    .Address_Line1 = sad.Address_Line1
                                    .Address_Line2 = sad.Address_Line2
                                    .City = sad.City
                                    .State = sad.State_Abbr
                                    If sad.Zip.Length > 5 Then
                                        .Zip_Code = Mid(sad.Zip, 1, 5)
                                        .Zip_Code_Plus4 = Mid(6, 4)
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
                Me._messages.Add(New ReturnMessage("Error in Manage Course services.", True, False))
                Me.LogError("Error while pulling Manage Course page Course Details.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function SearchSessionByStartDate(StartDate As Date) As List(Of Objects.CourseDetails) Implements IManageCourseQueires.SearchSessionByStartDate
            Dim retList As New List(Of Objects.CourseDetails)


            Try


                Using context As New MAISContext

                    For Each AppCourseSession As Data.Course In (From ACS In context.Courses _
                                                                 Join S In context.Sessions On S.Course_Sid Equals ACS.Course_sid _
                                                                Where S.Start_Date >= StartDate
                                                                Select ACS Distinct).ToList()


                        For Each C As Data.Course In (From a In context.Courses Where a.Course_sid = AppCourseSession.Course_sid Select a)
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
                            'hCourse.Category = (From CL In context.Role_Category_Level_Xref Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select CL.Category_Type_Sid).FirstOrDefault
                            hCourse.Category = (From CL In context.Role_Category_Level_Xref Join lkC In context.Category_Type On lkC.Category_Type_Sid Equals CL.Category_Type_Sid Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select lkC.Category_Code).FirstOrDefault

                            Dim CourseSessionList As New List(Of Objects.SessionAddressInformation)
                            For Each S As Data.Session In (From cs In context.Sessions Where cs.Course_Sid = AppCourseSession.Course_sid And cs.Start_Date >= StartDate)
                                Dim CourseSession As New Objects.SessionAddressInformation
                                CourseSession.Session_Sid = S.Session_Sid
                                CourseSession.Course_SID = S.Course_Sid
                                CourseSession.Session_Start_Date = S.Start_Date
                                CourseSession.Session_End_Date = S.End_Date
                                CourseSession.Sponsor = S.Sponsor
                                CourseSession.Location_Name = S.Location_Name
                                CourseSession.Total_CEs = S.Total_CEs
                                CourseSession.Public_Access_Flg = S.Public_Access_Flg
                                Dim sal As New Data.Session_Address_Xref
                                sal = (From dsal In context.Session_Address_Xref Where dsal.Session_Sid = S.Session_Sid Select dsal).FirstOrDefault
                                'Dim sad As New Data.Address
                                'sad = (From dsad In context.Addresses Where dsad.Address_Sid = sal.Address_Sid Select dsad).FirstOrDefault
                                Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", sal.Address_Sid)
                                Dim sad As MAIS.Data.Address_Lookup_And_Insert_Result = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).FirstOrDefault


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
                Me._messages.Add(New ReturnMessage("Error in Manage Course services.", True, False))
                Me.LogError("Error while pulling Manage Course page Course Details.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function DoesCourseExitAlready(CourseNumber As String) As Boolean Implements IManageCourseQueires.DoesCourseExitAlready
            Dim test As Boolean
            Try
                Using context As New MAISContext
                    test = (From t In context.Courses _
                            Where t.OBN_Course_Number = CourseNumber
                            Select t).Count



                End Using
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in DoesCourseExitAlready.", True, False))
                Me.LogError("Error in DoesCourseExitAlready.", CInt(Me.UserID), ex)
            End Try

            Return test
        End Function

        Public Function DoeseCourseSessionOverLap(CourseNumber As String, SessionStartDate As Date, SessionEndDate As Date) As Boolean Implements IManageCourseQueires.DoeseCourseSessionOverLap
            Dim test As Boolean
            Try
                Using Context As New MAISContext
                    test = (From t In Context.Courses _
                            Join s In Context.Sessions On s.Course_Sid Equals t.Course_sid
                            Where t.OBN_Course_Number = CourseNumber And SessionEndDate >= s.Start_Date And SessionStartDate <= s.End_Date
                            Select t).Count

                End Using
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in DoeseCourseSessionOverLap.", True, False))
                Me.LogError("Error in DoeseCourseSessionOverLap.", CInt(Me.UserID), ex)
            End Try

            Return test
        End Function



        Public Function SearchMangeCourse(Optional RNNumber As String = Nothing, Optional FirstName As String = Nothing, Optional LastName As String = Nothing, Optional SessionStartDate As Date = #12:00:00 AM#) As List(Of Objects.CourseDetails) Implements IManageCourseQueires.SearchMangeCourse
            Dim retList As New List(Of Objects.CourseDetails)
            Dim SearchCourse As New List(Of Data.Course)
            Try
                Select Case True


                    Case ((String.IsNullOrWhiteSpace(RNNumber) = False) AndAlso (String.IsNullOrWhiteSpace(FirstName) = False) AndAlso (String.IsNullOrWhiteSpace(LastName) = False) AndAlso (SessionStartDate = #12:00:00 AM#) = False)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                    Join S In resource.Sessions.DefaultIfEmpty On S.Course_Sid Equals ACS.Course_sid _
                                                                 Join RN In resource.RNs.DefaultIfEmpty On RN.RN_Sid Equals ACS.RN_Sid _
                                                                   Where RN.RNLicense_Number = RNNumber And RN.First_Name.Contains(FirstName) And RN.Last_Name.Contains(LastName) And S.Start_Date >= SessionStartDate
                                                                   Select ACS Distinct).ToList()
                        End Using




                    Case ((String.IsNullOrWhiteSpace(RNNumber) = False) AndAlso (String.IsNullOrWhiteSpace(FirstName) = True) AndAlso (String.IsNullOrWhiteSpace(LastName) = True) AndAlso (SessionStartDate = #12:00:00 AM#) = False)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                    Join S In resource.Sessions.DefaultIfEmpty On S.Course_Sid Equals ACS.Course_sid _
                                                                 Join RN In resource.RNs.DefaultIfEmpty On RN.RN_Sid Equals ACS.RN_Sid _
                                                                   Where RN.RNLicense_Number = RNNumber And S.Start_Date >= SessionStartDate
                                                                   Select ACS Distinct).ToList()
                        End Using

                    Case ((String.IsNullOrWhiteSpace(RNNumber) = False) AndAlso (String.IsNullOrWhiteSpace(FirstName) = False) AndAlso (String.IsNullOrWhiteSpace(LastName) = True) AndAlso (SessionStartDate = #12:00:00 AM#) = False)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                    Join S In resource.Sessions.DefaultIfEmpty On S.Course_Sid Equals ACS.Course_sid _
                                                                 Join RN In resource.RNs.DefaultIfEmpty On RN.RN_Sid Equals ACS.RN_Sid _
                                                                   Where RN.RNLicense_Number = RNNumber And RN.First_Name.Contains(FirstName) And S.Start_Date >= SessionStartDate
                                                                   Select ACS Distinct).ToList()
                        End Using


                    Case ((String.IsNullOrWhiteSpace(RNNumber) = False) AndAlso (String.IsNullOrWhiteSpace(FirstName) = True) AndAlso (String.IsNullOrWhiteSpace(LastName) = False) AndAlso (SessionStartDate = #12:00:00 AM#) = False)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                    Join S In resource.Sessions.DefaultIfEmpty On S.Course_Sid Equals ACS.Course_sid _
                                                                 Join RN In resource.RNs.DefaultIfEmpty On RN.RN_Sid Equals ACS.RN_Sid _
                                                                   Where RN.RNLicense_Number = RNNumber And RN.Last_Name.Contains(LastName) And S.Start_Date >= SessionStartDate
                                                                   Select ACS Distinct).ToList()
                        End Using


                    Case ((String.IsNullOrWhiteSpace(RNNumber) = False) AndAlso (String.IsNullOrWhiteSpace(FirstName) = False) AndAlso (String.IsNullOrWhiteSpace(LastName) = False) AndAlso (SessionStartDate = #12:00:00 AM#) = True)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                 Join RN In resource.RNs.DefaultIfEmpty On RN.RN_Sid Equals ACS.RN_Sid _
                                                                   Where RN.RNLicense_Number = RNNumber And RN.First_Name.Contains(FirstName) And RN.Last_Name.Contains(LastName)
                                                                   Select ACS Distinct).ToList()
                        End Using


                    Case ((String.IsNullOrWhiteSpace(RNNumber) = False) AndAlso (String.IsNullOrWhiteSpace(FirstName) = False) AndAlso (String.IsNullOrWhiteSpace(LastName) = True) AndAlso (SessionStartDate = #12:00:00 AM#) = True)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                 Join RN In resource.RNs.DefaultIfEmpty On RN.RN_Sid Equals ACS.RN_Sid _
                                                                   Where RN.RNLicense_Number = RNNumber And RN.First_Name.Contains(FirstName)
                                                                   Select ACS Distinct).ToList()
                        End Using


                    Case ((String.IsNullOrWhiteSpace(RNNumber) = False) AndAlso (String.IsNullOrWhiteSpace(FirstName) = True) AndAlso (String.IsNullOrWhiteSpace(LastName) = False) AndAlso (SessionStartDate = #12:00:00 AM#) = True)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                 Join RN In resource.RNs.DefaultIfEmpty On RN.RN_Sid Equals ACS.RN_Sid _
                                                                   Where RN.RNLicense_Number = RNNumber And RN.Last_Name.Contains(LastName)
                                                                   Select ACS Distinct).ToList()
                        End Using
                    Case ((String.IsNullOrWhiteSpace(RNNumber) = False) AndAlso (String.IsNullOrWhiteSpace(FirstName) = True) AndAlso (String.IsNullOrWhiteSpace(LastName) = True) AndAlso (SessionStartDate = #12:00:00 AM#) = True)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                 Join RN In resource.RNs.DefaultIfEmpty On RN.RN_Sid Equals ACS.RN_Sid _
                                                                   Where RN.RNLicense_Number = RNNumber
                                                                   Select ACS Distinct).ToList()
                        End Using
                    Case ((String.IsNullOrWhiteSpace(RNNumber) = True) AndAlso (String.IsNullOrWhiteSpace(FirstName) = False) AndAlso (String.IsNullOrWhiteSpace(LastName) = True) AndAlso (SessionStartDate = #12:00:00 AM#) = True)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                 Join RN In resource.RNs.DefaultIfEmpty On RN.RN_Sid Equals ACS.RN_Sid _
                                                                   Where RN.First_Name.Contains(FirstName)
                                                                   Select ACS Distinct).ToList()
                        End Using
                    Case ((String.IsNullOrWhiteSpace(RNNumber) = True) AndAlso (String.IsNullOrWhiteSpace(FirstName) = True) AndAlso (String.IsNullOrWhiteSpace(LastName) = False) AndAlso (SessionStartDate = #12:00:00 AM#) = True)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                 Join RN In resource.RNs.DefaultIfEmpty On RN.RN_Sid Equals ACS.RN_Sid _
                                                                   Where RN.Last_Name.Contains(LastName)
                                                                   Select ACS Distinct).ToList()
                        End Using
                    Case ((String.IsNullOrWhiteSpace(RNNumber) = True) AndAlso (String.IsNullOrWhiteSpace(FirstName) = False) AndAlso (String.IsNullOrWhiteSpace(LastName) = False) AndAlso (SessionStartDate = #12:00:00 AM#) = True)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                 Join RN In resource.RNs.DefaultIfEmpty On RN.RN_Sid Equals ACS.RN_Sid _
                                                                   Where RN.First_Name.Contains(FirstName) And RN.Last_Name.Contains(LastName)
                                                                   Select ACS Distinct).ToList()
                        End Using
                    Case ((String.IsNullOrWhiteSpace(RNNumber) = True) AndAlso (String.IsNullOrWhiteSpace(FirstName) = False) AndAlso (String.IsNullOrWhiteSpace(LastName) = False) AndAlso (SessionStartDate = #12:00:00 AM#) = False)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                    Join S In resource.Sessions.DefaultIfEmpty On S.Course_Sid Equals ACS.Course_sid _
                                                                 Join RN In resource.RNs.DefaultIfEmpty On RN.RN_Sid Equals ACS.RN_Sid _
                                                                   Where RN.First_Name.Contains(FirstName) And RN.Last_Name.Contains(LastName) And S.Start_Date >= SessionStartDate
                                                                   Select ACS Distinct).ToList()
                        End Using

                    Case ((String.IsNullOrWhiteSpace(RNNumber) = True) AndAlso (String.IsNullOrWhiteSpace(FirstName) = False) AndAlso (String.IsNullOrWhiteSpace(LastName) = True) AndAlso (SessionStartDate = #12:00:00 AM#) = False)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                    Join S In resource.Sessions.DefaultIfEmpty On S.Course_Sid Equals ACS.Course_sid _
                                                                 Join RN In resource.RNs.DefaultIfEmpty On RN.RN_Sid Equals ACS.RN_Sid _
                                                                   Where RN.First_Name.Contains(FirstName) And S.Start_Date >= SessionStartDate
                                                                   Select ACS Distinct).ToList()
                        End Using
                    Case ((String.IsNullOrWhiteSpace(RNNumber) = True) AndAlso (String.IsNullOrWhiteSpace(FirstName) = True) AndAlso (String.IsNullOrWhiteSpace(LastName) = False) AndAlso (SessionStartDate = #12:00:00 AM#) = False)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                    Join S In resource.Sessions.DefaultIfEmpty On S.Course_Sid Equals ACS.Course_sid _
                                                                 Join RN In resource.RNs.DefaultIfEmpty On RN.RN_Sid Equals ACS.RN_Sid _
                                                                   Where RN.Last_Name.Contains(LastName) And S.Start_Date >= SessionStartDate
                                                                   Select ACS Distinct).ToList()
                        End Using

                    Case ((String.IsNullOrWhiteSpace(RNNumber) = True) AndAlso (String.IsNullOrWhiteSpace(FirstName) = True) AndAlso (String.IsNullOrWhiteSpace(LastName) = True) AndAlso (SessionStartDate = #12:00:00 AM#) = False)
                        Using resource As New MAISContext
                            SearchCourse = (From ACS In resource.Courses _
                                                                    Join S In resource.Sessions.DefaultIfEmpty On S.Course_Sid Equals ACS.Course_sid _
                                                                    Where S.Start_Date >= SessionStartDate
                                                                   Select ACS Distinct).ToList()
                        End Using
                End Select



                Using context As New MAISContext

                    For Each AppCourseSession As Data.Course In (From ACS In SearchCourse
                                                                Select ACS Distinct).ToList()


                        For Each C As Data.Course In (From a In context.Courses Where a.Course_sid = AppCourseSession.Course_sid Select a)
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
                            'hCourse.Category = (From CL In context.Role_Category_Level_Xref Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select CL.Category_Type_Sid).FirstOrDefault
                            hCourse.Category = (From CL In context.Role_Category_Level_Xref Join lkC In context.Category_Type On lkC.Category_Type_Sid Equals CL.Category_Type_Sid Where CL.Role_Category_Level_Sid = C.Role_Category_Level_Sid Select lkC.Category_Code).FirstOrDefault
                            'Test to test if the Session Start Date is in user.
                            Dim SessionList As New List(Of Data.Session)
                            If (SessionStartDate = #12:00:00 AM#) = False Then
                                SessionList = (From cs In context.Sessions Where cs.Course_Sid = AppCourseSession.Course_sid And cs.Start_Date >= SessionStartDate).ToList
                            Else
                                SessionList = (From cs In context.Sessions Where cs.Course_Sid = AppCourseSession.Course_sid).ToList
                            End If

                            Dim CourseSessionList As New List(Of Objects.SessionAddressInformation)
                            For Each S As Data.Session In (From cs In SessionList)
                                Dim CourseSession As New Objects.SessionAddressInformation
                                CourseSession.Session_Sid = S.Session_Sid
                                CourseSession.Course_SID = S.Course_Sid
                                CourseSession.Session_Start_Date = S.Start_Date
                                CourseSession.Session_End_Date = S.End_Date
                                CourseSession.Sponsor = S.Sponsor
                                CourseSession.Location_Name = S.Location_Name
                                CourseSession.Total_CEs = S.Total_CEs
                                CourseSession.Public_Access_Flg = S.Public_Access_Flg
                                Dim ACount As List(Of Integer) = (From a In context.Application_Course_Xref _
                                                               Join b In context.Application_Course_Session_Xref On a.Application_Course_Xref_Sid Equals b.Application_Course_Xref_Sid _
                                                               Where b.Session_Sid = S.Session_Sid
                                                               Select a.Application_Sid).ToList

                                Dim ACount2 As List(Of Integer) = (From aa In context.Person_Course_Xref Join bb In context.Person_Course_Session_Xref On aa.Person_Course_Xref_Sid Equals bb.Person_Course_Xref_Sid _
                                                                    Join cc In context.Certifications On cc.Role_RN_DD_Personnel_Xref_Sid Equals aa.Role_RN_DD_Personnel_Xref_Sid _
                                                                     Where bb.Session_Sid = S.Session_Sid _
                                                                    Select CType(cc.Application_Sid, Integer)).ToList

                                CourseSession.AttendeeCount = (From u In ACount.AsQueryable().Union(ACount2) _
                                                               Select u).Count

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
                Me._messages.Add(New ReturnMessage("Error in Manage Course services.", True, False))
                Me.LogError("Error while pulling Manage Course page Course Details.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

#Region "Class function "
        Private Function CreateDODDCourseNumber(ByVal RNID As Integer, ByVal LevelSelected As Integer, ByVal Cat As Integer) As String
            Dim RetVal As String
            Dim mRNNumber As String = ""
            Dim mLevel As String = LevelSelected
            Dim mCat As String = ""
            Dim CourseCount As Integer = 0
            Using context As New MAISContext
                mRNNumber = (From r In context.RNs _
                             Where r.RN_Sid = RNID _
                             Select r.RNLicense_Number).FirstOrDefault

                CourseCount = (From c In context.Courses _
                               Select c.Course_sid).Max
            End Using
            mRNNumber = mRNNumber.Replace("RN", "")
            CourseCount += 1

            Select Case Cat
                Case 1
                    mCat = "01"
                Case 3
                    mCat = "02"
                Case 4
                    mCat = "03"
            End Select

            RetVal = "DODD-" & mRNNumber & "-" & mLevel & "-" & mCat & "-" & CStr(CourseCount + 1)
            Return RetVal

        End Function
#End Region

        Public Function DeleteSessionBySessionSid(SessionSid As Integer) As Boolean Implements IManageCourseQueires.DeleteSessionBySessionSid
            Dim retVal As Boolean = False
            Try
                Using resource As New MAISContext
                    Dim RSessionInof = (From r In resource.Session_Information
                                         Where r.Session_Sid = SessionSid
                                         Select r).ToList
                    Dim AddresseSession = (From r In resource.Session_Address_Xref
                                           Where r.Session_Sid = SessionSid
                                           Select r).ToList
                    Dim RSession = (From r In resource.Sessions
                                    Where r.Session_Sid = SessionSid
                                    Select r).ToList

                    For Each rs In RSessionInof
                        resource.Session_Information.Remove(rs)
                    Next
                    For Each a In AddresseSession
                        resource.Session_Address_Xref.Remove(a)
                    Next
                    For Each s In RSession
                        resource.Sessions.Remove(s)
                    Next

                    retVal = resource.SaveChanges
                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error in Manage Course services removing Seession from DeleteSessionBySessionSid.", True, False))
                Me.LogError("Error in Manage Course services removing Session from DeleteSessionBySessionSid.", CInt(Me.UserID), ex)

            End Try
            Return retVal
        End Function
    End Class

End Namespace

