Imports MAIS.Data

Namespace Mapping

    Public Class CEUMapping
        Public Shared Function mapDBtoModelCEUDetails(ByVal dbCEUs As List(Of Objects.CEUsDetailsObject)) As List(Of Model.CEUDetails)
            Dim retVal As New List(Of Model.CEUDetails)
            If (dbCEUs.Count > 0) Then
                retVal = (From DB In dbCEUs _
                                    Select New Model.CEUDetails With {
                                        .CEUs_Renewal_Sid = DB.CEUs_Renewal_Sid,
                                        .Application_Sid = DB.Application_Sid,
                                        .Permanent_Flg = DB.Permanent_Flg,
                                        .Start_Date = DB.Start_Date,
                                        .End_Date = DB.End_Date,
                                        .Category_Type_Sid = DB.Category_Type_Sid,
                                        .Category_Type_Code = DB.Category_Type_Code,
                                        .Attended_Date = DB.Attended_Date,
                                        .Total_CEUs = DB.Total_CEUs,
                                        .RN_Sid = DB.RN_Sid,
                                        .RN_Name = DB.RN_Name,
                                        .Instructor_Name = DB.Instructor_Name,
                                        .Title = DB.Title,
                                        .Course_Description = DB.Course_Description}).ToList

            End If
            Return retVal
        End Function

        Public Shared Function MapModelCEUDetailsToDB(ByVal mCEU As List(Of Model.CEUDetails)) As List(Of Data.CEUs_Renewal)
            Dim retval As New List(Of Data.CEUs_Renewal)

            retval = (From db In mCEU _
                      Select New Data.CEUs_Renewal With {
                          .CEUs_Renewal_Sid = db.CEUs_Renewal_Sid,
                          .Application_Sid = db.Application_Sid,
                          .Permanent_Flg = db.Permanent_Flg,
                          .Start_Date = db.Start_Date,
                          .End_Date = db.Start_Date,
                          .Category_Type_Sid = db.Category_Type_Sid,
                          .Attended_Date = db.Attended_Date,
                          .Total_CEUs = db.Total_CEUs,
                          .RN_Sid = db.RN_Sid,
                          .Instructor_Name = db.Instructor_Name,
                          .Title = db.Title,
                          .Course_Description = db.Course_Description}).ToList

            Return retval

        End Function
    End Class
End Namespace