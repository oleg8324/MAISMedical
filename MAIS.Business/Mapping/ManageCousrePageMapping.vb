Imports MAIS.Data


Namespace Mapping
    Public Class ManageCousrePageMapping

        Public Shared Function MapCouresDataToDB(ByVal CourseResults As Model.CourseDetails) As Objects.CourseDetails

            Dim _dbCoures As New Objects.CourseDetails
            If CourseResults Is Nothing Then
                Return Nothing
            Else

                With _dbCoures
                    .Course_Sid = CourseResults.Course_Sid
                    .RN_Sid = CourseResults.RN_Sid
                    .StartDate = CourseResults.StartDate
                    .EndDate = CourseResults.EndDate
                    .OBNApprovalNumber = CourseResults.OBNApprovalNumber
                    .CategoryACEs = CourseResults.CategoryACEs
                    .TotalCEs = CourseResults.TotalCEs
                    .CourseDescription = CourseResults.CourseDescription
                    .Level = CourseResults.Level
                    .Category = CourseResults.Category
                End With
                Dim CSessionDetailList As New List(Of Objects.SessionAddressInformation)

                If CourseResults.SessionDetailList Is Nothing Then CourseResults.SessionDetailList = New List(Of Model.SessionAddressInformation)

                For Each cs In CourseResults.SessionDetailList
                    Dim CSessionDetail As New Objects.SessionAddressInformation
                    With CSessionDetail
                        .Course_SID = CourseResults.Course_Sid
                        .Session_Start_Date = cs.Session_Start_Date
                        .Session_End_Date = cs.Session_End_Date
                        .Sponsor = cs.Sponsor
                        .Location_Name = cs.Location_Name
                        .Total_CEs = cs.Total_CEs
                        .Public_Access_Flg = cs.Public_Access_Flg
                        Dim CSAddress As New Objects.SessionAddress
                        With CSAddress
                            .Address_Line1 = cs.SessionAddressInfo.Address_Line1
                            .Address_Line2 = cs.SessionAddressInfo.Address_Line2
                            .City = cs.SessionAddressInfo.City
                            .State = cs.SessionAddressInfo.State
                            .StateID = cs.SessionAddressInfo.StateID
                            .Zip_Code = cs.SessionAddressInfo.Zip_Code
                            .Zip_Code_Plus4 = cs.SessionAddressInfo.Zip_Code_Plus4
                            .County = cs.SessionAddressInfo.County
                            .CountyID = cs.SessionAddressInfo.countyID
                        End With

                        .SessionAddressInfo = CSAddress
                        Dim SDItemList As New List(Of Objects.SessionInformationDetails)

                        For Each SDitem In cs.SessionInformationDetailsList
                            Dim nSDitem As New Objects.SessionInformationDetails
                            With nSDitem
                                .Session_Date = SDitem.Session_Date
                                .Total_CEs = SDitem.Total_CEs
                            End With
                            SDItemList.Add(nSDitem)
                        Next
                        .SessionInformationDetailsList = SDItemList
                    End With
                    CSessionDetailList.Add(CSessionDetail)
                Next


                _dbCoures.SessionDetailList = CSessionDetailList

            End If

            Return _dbCoures
        End Function

        Public Shared Function MapLevelFromDB(ByVal dbLevel_List As List(Of Data.Level_Type)) As List(Of Business.Model.LevelsDetails)
            Dim _Levels As New List(Of Model.LevelsDetails)

            If dbLevel_List Is Nothing Then
                Return Nothing
            Else
                _Levels = (From mL In dbLevel_List _
                           Select New Model.LevelsDetails With {
                               .Level_Type_Sid = mL.Level_Type_Sid,
                               .Level_Code = mL.Level_Code,
                               .Level_Desc = mL.Level_Desc}).ToList

            End If
            Return _Levels

        End Function

        Public Shared Function MapCategoryfromDB(ByVal dbCategory As List(Of Data.Category_Type)) As List(Of Model.CategoryDetails)
            Dim _Category As New List(Of Model.CategoryDetails)
            If dbCategory Is Nothing Then
                Return Nothing
            Else
                _Category = (From mC In dbCategory _
                             Select New Model.CategoryDetails With {
                                 .Category_Type_Sid = mC.Category_Type_Sid,
                                 .Category_Code = mC.Category_Code,
                                 .Category_Desc = mC.Category_Desc}).ToList

            End If
            Return _Category

        End Function
    End Class
End Namespace

