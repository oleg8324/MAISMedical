Imports MAIS.Data

Namespace Mapping
    Public Class TrainingSkillsPageMapping
        Public Shared Function mapDBtoModelSessionCourseDetails(ByVal dbSessionCourseDetails As List(Of Objects.SessionCourseInfoDetails)) As List(Of Model.SessionCourseInfoDetails)
            Dim retList As New List(Of Model.SessionCourseInfoDetails)
            retList = (From a In dbSessionCourseDetails _
                       Select New Model.SessionCourseInfoDetails With {
                           .RN_Sid = a.RN_Sid,
                           .Course_Sid = a.Course_Sid,
                           .Session_sID = a.Session_sID,
                           .RN_Name = a.RN_Name,
                           .CourseDescription = a.CourseDescription,
                           .CourseNumber = a.CourseNumber,
                           .StartDate = a.StartDate,
                           .EndDate = a.EndDate,
                           .SessionLocation = a.SessionLocation,
                           .TotalCEs = a.TotalCEs}).ToList
            Return retList

        End Function

        Public Shared Function MapDBToModelCourseDetailesList(ByVal dbCourse As Objects.CourseDetails) As Model.CourseDetails
            Dim retval As New Model.CourseDetails
            Dim HoldData As New Model.CourseDetails
            With HoldData
                .Course_Sid = dbCourse.Course_Sid
                .RN_Sid = dbCourse.RN_Sid
                .InstructorName = dbCourse.InstructorName
                .Role_Calegory_Level_Sid = dbCourse.Role_Calegory_Level_Sid
                .StartDate = dbCourse.StartDate
                .EndDate = dbCourse.EndDate
                .OBNApprovalNumber = dbCourse.OBNApprovalNumber
                .CategoryACEs = dbCourse.CategoryACEs
                .Level = dbCourse.Level
                .TotalCEs = dbCourse.TotalCEs
                .Category = dbCourse.Category
                .CourseDescription = dbCourse.CourseDescription
                Dim CourseSessionList As New List(Of Model.SessionAddressInformation)

                If dbCourse.SessionDetailList Is Nothing Then dbCourse.SessionDetailList = New List(Of Objects.SessionAddressInformation)

                For Each b As Objects.SessionAddressInformation In dbCourse.SessionDetailList

                    Dim SAList As New Model.SessionAddressInformation
                    SAList.Session_Sid = b.Session_Sid
                    SAList.Course_SID = b.Course_SID
                    SAList.Session_Start_Date = b.Session_Start_Date
                    SAList.Session_End_Date = b.Session_End_Date
                    SAList.Sponsor = b.Sponsor
                    SAList.Location_Name = b.Location_Name
                    SAList.Total_CEs = b.Total_CEs
                    SAList.Public_Access_Flg = b.Public_Access_Flg
                    SAList.AttendeeCount = b.Public_Access_Flg
                    Dim SAI As New Model.SessionAddress


                    SAI.Address_Sid = b.SessionAddressInfo.Address_Sid
                    SAI.Address_Line1 = b.SessionAddressInfo.Address_Line1
                    SAI.Address_Line2 = b.SessionAddressInfo.Address_Line2
                    SAI.City = b.SessionAddressInfo.City
                    SAI.State = b.SessionAddressInfo.State
                    SAI.Zip_Code = b.SessionAddressInfo.Zip_Code
                    SAI.Zip_Code_Plus4 = b.SessionAddressInfo.Zip_Code_Plus4
                    SAI.County = b.SessionAddressInfo.County
                    SAI.Session_Address_Xref_Sid = b.SessionAddressInfo.Session_Address_Xref_Sid
                    SAI.Address_Type_Sid = b.SessionAddressInfo.Address_Type_Sid

                    SAList.SessionAddressInfo = SAI
                    Dim SaInfoList As New List(Of Model.SessionInformationDetails)
                    For Each C As Objects.SessionInformationDetails In b.SessionInformationDetailsList
                        Dim SAInfo As New Model.SessionInformationDetails
                        SAInfo.Session_Information_SID = C.Session_Information_SID
                        SAInfo.Session_Sid = C.Session_Sid
                        SAInfo.Session_Date = C.Session_Date
                        SAInfo.Total_CEs = C.Total_CEs
                        SaInfoList.Add(SAInfo)
                    Next
                    SAList.SessionInformationDetailsList = SaInfoList
                    CourseSessionList.Add(SAList)
                    HoldData.SessionDetailList = CourseSessionList
                Next

            End With
            retval = HoldData
            Return retval

        End Function

        Public Shared Function MapDBToModelCourseDetailesList(ByVal dbCourse As List(Of Objects.CourseDetails)) As List(Of Model.CourseDetails)
            Dim retList As New List(Of Model.CourseDetails)
            For Each a As Objects.CourseDetails In dbCourse
                Dim HoldData As New Model.CourseDetails
                With HoldData
                    .Course_Sid = a.Course_Sid
                    .RN_Sid = a.RN_Sid
                    .InstructorName = a.InstructorName
                    .Role_Calegory_Level_Sid = a.Role_Calegory_Level_Sid
                    .StartDate = a.StartDate
                    .EndDate = a.EndDate
                    .OBNApprovalNumber = a.OBNApprovalNumber
                    .CategoryACEs = a.CategoryACEs
                    .Level = a.Level
                    .TotalCEs = a.TotalCEs
                    .Category = a.Category
                    .CourseDescription = a.CourseDescription
                    Dim CourseSessionList As New List(Of Model.SessionAddressInformation)

                    If a.SessionDetailList Is Nothing Then a.SessionDetailList = New List(Of Objects.SessionAddressInformation)

                    For Each b As Objects.SessionAddressInformation In a.SessionDetailList
                        Dim SAList As New Model.SessionAddressInformation
                        SAList.Session_Sid = b.Session_Sid
                        SAList.Course_SID = b.Course_SID
                        SAList.Session_Start_Date = b.Session_Start_Date
                        SAList.Session_End_Date = b.Session_End_Date
                        SAList.Sponsor = b.Sponsor
                        SAList.Location_Name = b.Location_Name
                        SAList.Total_CEs = b.Total_CEs
                        SAList.Public_Access_Flg = b.Public_Access_Flg
                        SAList.AttendeeCount = b.AttendeeCount
                        Dim SAI As New Model.SessionAddress


                        SAI.Address_Sid = b.SessionAddressInfo.Address_Sid
                        SAI.Address_Line1 = b.SessionAddressInfo.Address_Line1
                        SAI.Address_Line2 = b.SessionAddressInfo.Address_Line2
                        SAI.City = b.SessionAddressInfo.City
                        SAI.State = b.SessionAddressInfo.State
                        SAI.Zip_Code = b.SessionAddressInfo.Zip_Code
                        SAI.Zip_Code_Plus4 = b.SessionAddressInfo.Zip_Code_Plus4
                        SAI.County = b.SessionAddressInfo.County
                        SAI.Session_Address_Xref_Sid = b.SessionAddressInfo.Session_Address_Xref_Sid
                        SAI.Address_Type_Sid = b.SessionAddressInfo.Address_Type_Sid

                        SAList.SessionAddressInfo = SAI
                        Dim SaInfoList As New List(Of Model.SessionInformationDetails)
                        For Each C As Objects.SessionInformationDetails In b.SessionInformationDetailsList
                            Dim SAInfo As New Model.SessionInformationDetails
                            SAInfo.Session_Information_SID = C.Session_Information_SID
                            SAInfo.Session_Sid = C.Session_Sid
                            SAInfo.Session_Date = C.Session_Date
                            SAInfo.Total_CEs = C.Total_CEs
                            SaInfoList.Add(SAInfo)
                        Next
                        SAList.SessionInformationDetailsList = SaInfoList
                        CourseSessionList.Add(SAList)
                        HoldData.SessionDetailList = CourseSessionList

                    Next
                End With
                retList.Add(HoldData)
            Next

            Return retList

        End Function

        Public Shared Function mapDBtoModelCacategoryDetails(ByVal dbCategoryDetails As Data.Category_Type) As Model.CategoryDetails
            Dim retData As New Model.CategoryDetails
            If dbCategoryDetails IsNot Nothing Then
                With retData
                    .Category_Type_Sid = dbCategoryDetails.Category_Type_Sid
                    .Category_Code = dbCategoryDetails.Category_Code
                    .Category_Desc = dbCategoryDetails.Category_Desc
                End With
            End If
            Return retData

        End Function

        Public Shared Function MapModelCEUsDetailsToDB(ByVal CEUDetailList As List(Of Model.CEUDetails)) As List(Of Objects.CEUsDetailsObject)
            Dim retVal As New List(Of Objects.CEUsDetailsObject)

            retVal = (From cList In CEUDetailList _
                      Select New Objects.CEUsDetailsObject With {
                          .Permanent_Flg = cList.Permanent_Flg,
                          .Start_Date = cList.Start_Date,
                          .End_Date = cList.End_Date,
                          .DD_RN_Personnel_SID = cList.DD_RN_Personnel_SID,
                          .Category_Type_Sid = cList.Category_Type_Sid,
                          .Attended_Date = cList.Attended_Date,
                          .Total_CEUs = cList.Total_CEUs,
                          .RN_Sid = cList.RN_Sid,
                          .Instructor_Name = cList.Instructor_Name,
                          .Title = cList.Title,
                          .Course_Description = cList.Course_Description}).ToList

            Return retVal

        End Function
    End Class
End Namespace

