Imports MAIS.Data

Namespace Mapping
    Public Class ResourceMapping
        Public Shared Function mapDBtoModelResources(ByVal dbCEUs As List(Of Data.Resource)) As List(Of Model.Resource)
            Dim retVal As New List(Of Model.Resource)
            If (dbCEUs.Count > 0) Then
                retVal = (From DB In dbCEUs _
                                    Select New Model.Resource With {
                                        .Active_Flag=DB.Active_Flg,
                                        .CreateBy=DB.Create_By,
                                        .CreateByName=DB.Create_By,
                                        .Description=DB.Description,
                                        .LastUpdateBy=DB.Last_Update_By,
                                        .lastUpdatedByName=DB.Last_Update_By,
                                        .Priority=DB.Priority,
                                        .Resource_SID=DB.Resource_Sid,
                                        .Subject=DB.Subject}).ToList
            End If
            Return retVal
        End Function
        Public Shared Function mapDBtoModelResources(ByVal dbCEUs As Data.Resource) As Model.Resource
            Dim retVal As New Model.Resource
            retVal = New Model.Resource With {
                                        .Active_Flag = dbCEUs.Active_Flg,
                                        .CreateBy = dbCEUs.Create_By,
                                        .CreateByName = dbCEUs.Create_By,
                                        .Description = dbCEUs.Description,
                                        .LastUpdateBy = dbCEUs.Last_Update_By,
                                        .lastUpdatedByName = dbCEUs.Last_Update_By,
                                        .Priority = dbCEUs.Priority,
                                        .Resource_SID = dbCEUs.Resource_Sid,
                                        .Subject = dbCEUs.Subject}
            Return retVal
        End Function

        Public Shared Function MapModelToDBResources(ByVal mCEU As Model.Resource) As Data.Resource
            Dim retval As New Data.Resource

            retval = New Data.Resource With {
                .Resource_Sid = mCEU.Resource_SID,
                                        .Active_Flg = mCEU.Active_Flag,
                                        .Create_By = mCEU.CreateBy,
                                        .Create_Date = Date.Today,
                                        .Description = mCEU.Description,
                                        .Last_Update_By = mCEU.LastUpdateBy,
                                        .Last_Update_Date = Date.Today,
                                        .Priority = mCEU.Priority,
                                        .Subject = mCEU.Subject}

            Return retval

        End Function
    End Class
End Namespace