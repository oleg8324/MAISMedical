Imports ODMRDDHelperClassLibrary.Utility

Namespace Queries
    Public Interface IMAISDODDQueries
        Function GetRNData(ByVal lastName As String, ByVal firstname As String, ByVal RnlicenseNumber As String) As List(Of Objects.RNInformationDetailsForWS)
        Function GetDDData(ByVal DDpersonnelCode As String, ByVal ddssn As Integer?, ByVal lastname As String, ByVal firstname As String, ByVal EmployerName As String) As List(Of Objects.DDDetailInformation)
        Function GetTrainingSessionData(ByVal sessionStartDate As DateTime?, ByVal sessionEndDate As DateTime?, ByVal county As String, ByVal lastName As String, ByVal firstname As String, ByVal rnsession As Boolean) As List(Of Objects.TrainingSessionResults)
    End Interface

    Public Class MAISDODDQueries
        Inherits QueriesBase
        Implements IMAISDODDQueries

        Public Sub New(ByVal userID As String)
            Me.UserID = userID
        End Sub

        Public Function GetDDData(DDpersonnelCode As String, ddssn As Integer?, lastname As String, firstname As String, EmployerName As String) As List(Of Objects.DDDetailInformation) Implements IMAISDODDQueries.GetDDData
            Dim result As New List(Of Objects.DDDetailInformation)
            Using context As New MAISContext
                Try
                    result = (From dd In context.DDPersonnels _
                              Join rnDD In context.RN_DD_Person_Type_Xref On rnDD.RN_DDPersonnel_Sid Equals dd.DDPersonnel_Sid
                             Group Join empRef In context.Employer_RN_DD_Person_Type_Xref On empRef.RN_DD_Person_Type_Xref_Sid Equals rnDD.RN_DD_Person_Type_Xref_Sid Into emmr = Group
                              From emmr1 In emmr.DefaultIfEmpty
                              Group Join emp In context.Employers On emp.Employer_Sid Equals emmr1.Employer_Sid Into emm = Group
                              From emm1 In emm.DefaultIfEmpty
                              Join rnDDRole In context.Role_RN_DD_Personnel_Xref On rnDDRole.RN_DD_Person_Type_Xref_Sid Equals rnDD.RN_DD_Person_Type_Xref_Sid
                                Where If(String.IsNullOrEmpty(DDpersonnelCode), True, dd.DDPersonnel_Code = DDpersonnelCode) AndAlso _
                                If(ddssn Is Nothing, True, dd.SSN = ddssn) AndAlso If(String.IsNullOrEmpty(lastname), True, dd.Last_Name.Contains(lastname)) _
                                AndAlso If(String.IsNullOrEmpty(firstname), True, dd.First_Name.Contains(firstname)) AndAlso If(String.IsNullOrEmpty(EmployerName), True, If(emm1.Employer_Name Is Nothing, False, emm1.Employer_Name.Contains(EmployerName)))
                                Select New Objects.DDDetailInformation With {
                                    .dob = dd.DOB,
                                    .DDPersonnelCode = dd.DDPersonnel_Code,
                                    .FirstName = dd.First_Name,
                                    .Last4SSN = dd.SSN,
                                    .LastName = dd.Last_Name
                                    }).Distinct.ToList

                    If result IsNot Nothing Then
                        If result.Count > 0 Then
                            For Each r As Objects.DDDetailInformation In result
                                r.CertificateDetails = (From roleRNDD In context.Role_RN_DD_Personnel_Xref
                                                        Join cert In context.Certifications On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRNDD.Role_RN_DD_Personnel_Xref_Sid
                                                        Join certStatus In context.Certification_Status On certStatus.Certification_Sid Equals cert.Certification_Sid
                                                        Join certType In context.Certification_Status_Type On certStatus.Certification_Status_Type_Sid Equals certType.Certification_Status_Type_Sid
                                                        Join role In context.Role_Category_Level_Xref On role.Role_Category_Level_Sid Equals roleRNDD.Role_Category_Level_Sid
                                                        Join roleDes In context.MAIS_Role On roleDes.Role_Sid Equals role.Role_Sid
                                                        Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals roleRNDD.RN_DD_Person_Type_Xref_Sid
                                                        Join rn In context.DDPersonnels On rn.DDPersonnel_Sid Equals rnRef.RN_DDPersonnel_Sid
                                                        Group Join renewal In context.Renewal_History_Count On renewal.RN_DD_Person_Type_Xref_Sid Equals roleRNDD.RN_DD_Person_Type_Xref_Sid Into rc = Group
                                                        From rc1 In rc.DefaultIfEmpty()
                                                        Where rn.DDPersonnel_Code = r.DDPersonnelCode AndAlso If(rc1.RN_DD_Person_Type_Xref Is Nothing, True, (rc1.Role_Category_Level_sid = roleRNDD.Role_Category_Level_Sid)) And _
                                                        certStatus.Status_Start_Date = (From cs In context.Certification_Status
                                                        Where cs.Certification_Sid = cert.Certification_Sid Select cs.Status_Start_Date).Max()
                                                        Order By certStatus.Status_Start_Date Descending
                                                        Select New Objects.CertificateDetails With {
                                                            .ConsectiveRenewals = If(rc1.Renewal_Count > 0, rc1.Renewal_Count, 0),
                                                            .CurrentStatus = certType.Certification_Status_Desc,
                                                            .EffectiveDate = cert.Certification_Start_Date,
                                                            .ExpirationDate = cert.Certification_End_Date,
                                                            .RoleDescription = role.Category_Type.Category_Desc
                                                        }).Distinct.ToList
                            Next
                        End If
                    End If

                Catch ex As Exception
                    Me.LogError("Error Getting dd Info using Webservice", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting dd Info using Webservice.", True, False))
                    Throw
                End Try
            End Using
            Return result
        End Function

        Public Function GetRNData(lastName As String, firstname As String, RnlicenseNumber As String) As List(Of Objects.RNInformationDetailsForWS) Implements IMAISDODDQueries.GetRNData
            Dim result As New List(Of Objects.RNInformationDetailsForWS)
            Using context As New MAISContext
                Try
                    result = (From dd In context.RNs _
                              Join rnDD In context.RN_DD_Person_Type_Xref On rnDD.RN_DDPersonnel_Sid Equals dd.RN_Sid
                              Group Join empRef In context.Employer_RN_DD_Person_Type_Xref On empRef.RN_DD_Person_Type_Xref_Sid Equals rnDD.RN_DD_Person_Type_Xref_Sid Into emmr = Group
                              From emmr1 In emmr.DefaultIfEmpty
                              Group Join emp In context.Employers On emp.Employer_Sid Equals emmr1.Employer_Sid Into emm = Group
                              From emm1 In emm.DefaultIfEmpty
                              Join rnDDRole In context.Role_RN_DD_Personnel_Xref On rnDDRole.RN_DD_Person_Type_Xref_Sid Equals rnDD.RN_DD_Person_Type_Xref_Sid
                    Where If(String.IsNullOrEmpty(RnlicenseNumber), True, dd.RNLicense_Number = RnlicenseNumber) _
                    AndAlso If(String.IsNullOrEmpty(lastName), True, dd.Last_Name.Contains(lastName)) _
                    AndAlso If(String.IsNullOrEmpty(firstname), True, dd.First_Name.Contains(firstname))
                                Select New Objects.RNInformationDetailsForWS With {
                                    .RNLicenseNumber = dd.RNLicense_Number,
                                    .FirstName = dd.First_Name,
                                    .LastName = dd.Last_Name
                                    }).Distinct.ToList

                    If result IsNot Nothing Then
                        If result.Count > 0 Then
                            For Each r As Objects.RNInformationDetailsForWS In result
                                r.CertificateDetails = (From roleRNDD In context.Role_RN_DD_Personnel_Xref
                                                        Join cert In context.Certifications On cert.Role_RN_DD_Personnel_Xref_Sid Equals roleRNDD.Role_RN_DD_Personnel_Xref_Sid
                                                        Join certStatus In context.Certification_Status On certStatus.Certification_Sid Equals cert.Certification_Sid
                                                        Join certType In context.Certification_Status_Type On certStatus.Certification_Status_Type_Sid Equals certType.Certification_Status_Type_Sid
                                                        Join role In context.Role_Category_Level_Xref On role.Role_Category_Level_Sid Equals roleRNDD.Role_Category_Level_Sid
                                                        Join roleDes In context.MAIS_Role On roleDes.Role_Sid Equals role.Role_Sid
                                                        Join rnRef In context.RN_DD_Person_Type_Xref On rnRef.RN_DD_Person_Type_Xref_Sid Equals roleRNDD.RN_DD_Person_Type_Xref_Sid
                                                        Join rn In context.RNs On rn.RN_Sid Equals rnRef.RN_DDPersonnel_Sid
                                                        Group Join renewal In context.Renewal_History_Count On renewal.RN_DD_Person_Type_Xref_Sid Equals roleRNDD.RN_DD_Person_Type_Xref_Sid Into rc = Group
                                                        From rc1 In rc.DefaultIfEmpty()
                                Where rn.RNLicense_Number = r.RNLicenseNumber AndAlso If(rc1.RN_DD_Person_Type_Xref Is Nothing, True, (rc1.Role_Category_Level_sid = roleRNDD.Role_Category_Level_Sid)) And _
                                certStatus.Status_Start_Date = (From cs In context.Certification_Status
                                Where cs.Certification_Sid = cert.Certification_Sid
                                                                       Select cs.Status_Start_Date).Max()
                                                                   Order By certStatus.Status_Start_Date Descending
                                                        Select New Objects.CertificateDetails With {
                                                            .ConsectiveRenewals = If(rc1.Renewal_Count > 0, rc1.Renewal_Count, 0),
                                                            .CurrentStatus = certType.Certification_Status_Desc,
                                                            .EffectiveDate = cert.Certification_Start_Date,
                                                            .ExpirationDate = cert.Certification_End_Date,
                                                            .RoleDescription = roleDes.Role_Desc
                                                        }).Distinct.ToList
                            Next
                        End If
                    End If

                Catch ex As Exception
                    Me.LogError("Error Getting rn Info using Webservice", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting rn Info using Webservice.", True, False))
                    Throw
                End Try
            End Using
            Return result
        End Function

        Public Function GetTrainingSessionData(sessionStartDate As DateTime?, sessionEndDate As DateTime?, county As String, lastName As String, firstname As String, rnsession As Boolean) As List(Of Objects.TrainingSessionResults) Implements IMAISDODDQueries.GetTrainingSessionData
            Dim result As New List(Of Objects.TrainingSessionResults)
            If (sessionStartDate IsNot Nothing And sessionEndDate Is Nothing) Then
                sessionEndDate = Convert.ToDateTime("12/31/9999")
            End If
            Using context As New MAISContext
                Try
                    result = (From sessionDetails In context.Sessions
                              Join coursedetails In context.Courses On sessionDetails.Course_Sid Equals coursedetails.Course_sid
                              Join rn In context.RNs On rn.RN_Sid Equals coursedetails.RN_Sid
                              Join sessionaddRef In context.Session_Address_Xref On sessionaddRef.Session_Sid Equals sessionDetails.Session_Sid
                              Join addRef In context.Address1 On addRef.Address_Sid Equals sessionaddRef.Address_Sid
                              Join cityInfoRef In context.City_Information On addRef.City_Information_Sid Equals cityInfoRef.City_Information_Sid
                              Join countyRef In context.Counties On countyRef.County_ID Equals cityInfoRef.CountyID
                              Join roleCat In context.Role_Category_Level_Xref On roleCat.Role_Category_Level_Sid Equals coursedetails.Role_Category_Level_Sid
                              Join roles In context.MAIS_Role On roles.Role_Sid Equals roleCat.Role_Sid
                              Join rnRef In context.RN_DD_Person_Type_Xref On rn.RN_Sid Equals rnRef.RN_DDPersonnel_Sid
                              Join rnEmail In context.RN_DD_Person_Type_Email_Xref On rnEmail.RN_DD_Person_Type_Xref_Sid Equals rnRef.RN_DD_Person_Type_Xref_Sid
                              Join email In context.Email1 On email.Email_SID Equals rnEmail.Email_Sid
                    Where If(String.IsNullOrEmpty(lastName), True, rn.Last_Name.Contains(lastName)) AndAlso If(rnsession = False, coursedetails.OBN_Course_Number.Contains("DODD"), (coursedetails.OBN_Course_Number.Contains("DODD") = False)) _
                      AndAlso If(String.IsNullOrEmpty(firstname), True, rn.First_Name.Contains(firstname)) _
                      AndAlso If(sessionStartDate Is Nothing, True, (sessionDetails.Start_Date >= sessionStartDate And sessionDetails.End_Date <= sessionEndDate)) _
                      AndAlso If(String.IsNullOrEmpty(county), True, countyRef.County_Alias = county) _
                      AndAlso sessionDetails.End_Date >= Date.Today AndAlso sessionDetails.Public_Access_Flg = True
                                Select New Objects.TrainingSessionResults With {
                                    .County = countyRef.County_Desc,
                                    .CourseCategory = roles.Role_Desc,
                                    .EndDate = sessionDetails.End_Date,
                                    .RNTrainerFirstName = rn.First_Name,
                                    .RNTrainerlastName = rn.Last_Name,
                                    .StartDate = sessionDetails.Start_Date,
                                    .RNTrainerEmail = email.Email_Address,
                    .OBNNumber = coursedetails.OBN_Course_Number
                                    }).Distinct.ToList
                Catch ex As Exception
                    Me.LogError("Error Getting training info using Webservice", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting training Info using Webservice.", True, False))
                    Throw
                End Try
            End Using
            Return result
        End Function
    End Class
End Namespace
