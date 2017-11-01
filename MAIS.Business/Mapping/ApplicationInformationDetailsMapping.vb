Imports MAIS.Data

Namespace Mapping

    Public Class ApplicationInformationDetailsMapping
        Public Shared Function MapToModelApplicationInformationDetails(ByVal dbMA_Application As Application) As Model.ApplicationInformationDetails
            Dim returnModelApp As New Model.ApplicationInformationDetails
            If dbMA_Application IsNot Nothing Then
                With returnModelApp
                    .Application_SID = dbMA_Application.Application_Sid
                    .ApplicationType_SID = dbMA_Application.Application_Type_Sid
                    .RN_Flg = dbMA_Application.RN_flg
                    .ApplicationStatusType_SID = dbMA_Application.Application_Status_Type_Sid
                    .Signature = dbMA_Application.Signature
                    .RoleCategoryLevel_SID = dbMA_Application.Role_Category_Level_Sid
                    .RNDDUnique_Code = dbMA_Application.RN_DD_Unique_Code
                    .Attestant_SID = dbMA_Application.Attestant_Sid
                    .CreateDate = dbMA_Application.Create_Date
                    .IsAdminFlag = dbMA_Application.Is_Admin_Flg
                End With
            End If
            Return returnModelApp
        End Function
    End Class
End Namespace