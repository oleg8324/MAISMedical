Imports MAIS.Data

Namespace Mapping

    Public Class User_RN_Mapping_Mapping
        Public Shared Function MappingDBtoModelUserRN_Mapping(ByRef dbUserRN_Mapping As Data.User_RN_Mapping) As Model.RN_UserMappingDetails
            Dim RNUserMapping As New Model.RN_UserMappingDetails
            If Not (dbUserRN_Mapping Is Nothing) Then
                With RNUserMapping
                    .UserMapping_Sid = dbUserRN_Mapping.User_Mapping_Sid
                    .UserID = dbUserRN_Mapping.UserID
                    .RN_Sid = dbUserRN_Mapping.RN_Sid
                    .CreateDate = dbUserRN_Mapping.Create_Date
                    .CreateBy = dbUserRN_Mapping.Create_By
                    .LastUpdateDate = dbUserRN_Mapping.Last_Update_Date
                    .LastUpdateBy = dbUserRN_Mapping.Last_Update_By
                End With

            Else
                RNUserMapping = Nothing
            End If

            Return RNUserMapping

        End Function

        Public Shared Function MappingDBtoModleUserRN_MappingList(ByVal dbUserRN_Mapping As List(Of Data.User_RN_Mapping)) As List(Of Model.RN_UserMappingDetails)
            Dim RnUserMappingList As New List(Of Model.RN_UserMappingDetails)

            RnUserMappingList = (From a In dbUserRN_Mapping _
                                  Select New Model.RN_UserMappingDetails With {
                                      .UserMapping_Sid = a.User_Mapping_Sid,
                                      .UserID = a.UserID,
                                      .RN_Sid = a.RN_Sid,
                                      .CreateDate = a.Create_Date,
                                      .CreateBy = a.Create_By,
                                      .LastUpdateDate = a.Last_Update_Date,
                                      .LastUpdateBy = a.Last_Update_By}).ToList

            Return RnUserMappingList
        End Function
    End Class
End Namespace

