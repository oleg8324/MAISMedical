Imports MAIS.Data
Imports MAIS.Data.Objects

Namespace Mapping
    Public Class MAISReportMapping
        Public Shared Function MapDDNotation(ByVal dd As List(Of DDPersonnelSearchResult)) As List(Of Model.DDPersonnelSearchResult)
            Dim retList As New List(Of Model.DDPersonnelSearchResult)
            If (dd.Count > 0) Then
                retList = (From d In dd _
                               Select New Model.DDPersonnelSearchResult With {
                                    .County = d.County,
                                    .DateOfBirth = d.DateOfBirth,
                                    .DDPersonnelCode = d.DDPersonnelCode,
                                    .FirstName = d.FirstName,
                                    .HomeAddress = d.Address1 + " " + d.Address2 + " " + d.City + " " + d.State + " " + d.Zip + " " + d.ZipPlus,
                                    .Last4SSN = String.Format("{0:0000}", CInt(d.Last4SSN)),
                                    .LastName = d.LastName,
                                    .MiddleName = d.MiddleName,
                                    .CAT1 = d.Cat1,
                                    .CAT2 = d.Cat2,
                                    .CAT3 = d.Cat3
                               }).ToList
               
            End If
            Return retList
        End Function
        Public Shared Function MapParametersToDB(ByVal param As Model.SearchParameters) As Objects.SearchParameters
            Dim DBParams As New Objects.SearchParameters
            If param IsNot Nothing Then
                DBParams.CEOFirstName = param.CEOFirstName
                DBParams.CEOLastName = param.CEOLastName
                DBParams.Cert_Status_Type_Sid = param.Cert_Status_Type_Sid
                DBParams.Course_Sid = param.Course_Sid
                DBParams.Last4SSN = param.Last4SSN
                DBParams.EmployerName = param.EmployerName
                DBParams.ExpDateFrom = param.ExpDateFrom
                DBParams.ExpDateTo = param.ExpDateTo
                DBParams.ExpWithinLast180Days = param.ExpWithinLast180Days
                DBParams.ExpWithinLast30Days = param.ExpWithinLast30Days
                DBParams.ExpWithinLast60Days = param.ExpWithinLast60Days
                DBParams.ExpWithinLast90Days = param.ExpWithinLast90Days
                DBParams.FirstName = param.FirstName
                DBParams.LastName = param.LastName
                DBParams.Licence_Code = param.Licence_Code
                DBParams.Role_Level_Cat_Sid = param.Role_Level_Cat_Sid
                DBParams.Session_sid = param.Session_sid
                DBParams.SupFirstName = param.SupFirstName
                DBParams.SupLastName = param.SupLastName
                DBParams.Trainer_RN_Sid = param.Trainer_RN_Sid
                DBParams.AdminFlg_GetAllRecords = param.AdminFlg_GettAllRecords
            End If
            Return DBParams
        End Function
        Public Shared Function MapDDList(ByVal rrInfo As List(Of USP_Get_DDPersonnel_MAIS_Report_Results_Result)) As List(Of Model.MAIS_Report)
            Try
                Dim retList As New List(Of Model.MAIS_Report)
                If (rrInfo.Count > 0) Then
                    Dim rnSid As List(Of Integer) = (From ri In rrInfo
                                                     Order By ri.RN_DD_Person_Type_Xref_Sid
                                                     Select ri.RN_DD_Person_Type_Xref_Sid).Distinct.ToList()
                  

                    For Each rid In rnSid
                        Dim retInfo As New Model.MAIS_Report
                        Dim oneSet = (From o In rrInfo
                                      Where o.RN_DD_Person_Type_Xref_Sid = rid
                                      Select o).Distinct
                        retInfo = (From r In oneSet
                                 Where r.RN_DD_Person_Type_Xref_Sid = rid
                                 Select New Model.MAIS_Report With {
                                           .Last4SSN = String.Format("{0:0000}", r.SSN),
                                           .DOB_LicenseIssue = Convert.ToDateTime(r.DOB).ToShortDateString(),
                                           .FirstName = If(((r.First_Name IsNot Nothing) AndAlso (r.First_Name.Length > 0)), r.First_Name, String.Empty),
                                           .HAddr_Sid = If(((r.Homeaddress_Sid IsNot Nothing) AndAlso (r.Homeaddress_Sid > 0)), r.Homeaddress_Sid, 0),
                                           .HomeAddress = If(((r.HomeAddress IsNot Nothing) AndAlso (r.HomeAddress.Length > 0)), r.HomeAddress, String.Empty),
                                           .HCounty = If(((r.HomeCounty IsNot Nothing) AndAlso (r.HomeCounty.Length > 0)), r.HomeCounty, String.Empty),
                                           .LastName = If(((r.Last_Name IsNot Nothing) AndAlso (r.Last_Name.Length > 0)), r.Last_Name, String.Empty),
                                           .MiddleName = If(((r.Middle_Name IsNot Nothing) AndAlso (r.Middle_Name.Length > 0)), r.Middle_Name, String.Empty),
                                           .RN_DD_Person_Type_Xref_Sid = r.RN_DD_Person_Type_Xref_Sid,
                                           .RN_DD_Sid = r.DDPersonnel_Sid,
                                           .RNLicence_DDPersonnel = r.DDPersonnel_Code
                                     }).Distinct.FirstOrDefault()
                        Dim roles = (From rol In oneSet
                                       Where rol.RN_DD_Person_Type_Xref_Sid = rid
                                        Select rol.Role_RN_DD_Personnel_Xref_Sid, rol.Certification_Sid).Distinct.ToList()
                        If roles.Count > 0 Then
                            Dim certlst As New List(Of Model.Cert_Info)
                            For Each rolesid In roles
                                Dim CertInfo As New Model.Cert_Info
                                CertInfo = (From c In oneSet
                                                Where c.Certification_Sid = rolesid.Certification_Sid
                                                Select New Model.Cert_Info With {
                                                    .Attestant_Name = If(((c.Attestant_FN IsNot Nothing) AndAlso (c.Attestant_FN.Length > 0)), c.Attestant_FN, String.Empty) & " " & If(((c.Attestant_LN IsNot Nothing) AndAlso (c.Attestant_LN.Length > 0)), c.Attestant_LN, String.Empty),
                                                    .Attestant_Sid = If(((c.Attestant IsNot Nothing) AndAlso (c.Attestant > 0)), c.Attestant, 0),
                                                    .Certification_End_Date = If(IsDate(c.Certification_End_Date), c.Certification_End_Date, "12/31/9999"),
                                                    .Certification_Start_Date = If(IsDate(c.Certification_Start_Date), c.Certification_Start_Date, "12/31/9999"),
                                                    .Certification_Sid = c.Certification_Sid,
                                                    .Certification_Status = If(((c.Certification_Status_Desc IsNot Nothing) AndAlso (c.Certification_Status_Desc.Length > 0)), c.Certification_Status_Desc, String.Empty),
                                                    .Certification_Type = If(((c.Role_Desc IsNot Nothing) AndAlso (c.Role_Desc.Length > 0)), c.Role_Desc, String.Empty),
                                                    .RenewalCount = If(((c.Renewal_Count IsNot Nothing) AndAlso (c.Renewal_Count > 0)), c.Renewal_Count, 0),
                                                    .Category_Code = If(((c.Category_Code IsNot Nothing) AndAlso (c.Category_Code.Length > 0)), c.Category_Code, String.Empty)
                                                    }).FirstOrDefault()
                                certlst.Add(CertInfo)
                            Next
                            retInfo.Cert_Info = certlst
                        Else
                            retInfo.Cert_Info = Nothing
                        End If
                        Dim emps = (From e In oneSet
                                    Where e.RN_DD_Person_Type_Xref_Sid = rid And e.Employer_Sid.HasValue
                                    Select e.Employer_Sid).Distinct.ToList()
                        If emps.Count > 0 Then
                            Dim emplst As New List(Of Model.Employer_Info)
                            For Each eid In emps
                                Dim empInfo As New Model.Employer_Info
                                empInfo = (From ep In oneSet
                                                Where ep.Employer_Sid = eid
                                                Select New Model.Employer_Info With {
                                                    .CEO_First_Name = If(((ep.CEO_First_Name IsNot Nothing) AndAlso (ep.CEO_First_Name.Length > 0)), ep.CEO_First_Name, String.Empty),
                                                    .CEO_Last_Name = If(((ep.CEO_Last_Name IsNot Nothing) AndAlso (ep.CEO_Last_Name.Length > 0)), ep.CEO_Last_Name, String.Empty),
                                                    .Employer_Name = If(((ep.Employer_Name IsNot Nothing) AndAlso (ep.Employer_Name.Length > 0)), ep.Employer_Name, String.Empty),
                                                    .Employer_Sid = If(((ep.Employer_Sid IsNot Nothing) AndAlso (ep.Employer_Sid > 0)), ep.Employer_Sid, 0),
                                                    .Supervisor_First_Name = If(((ep.Supervisor_First_Name IsNot Nothing) AndAlso (ep.Supervisor_First_Name.Length > 0)), ep.Supervisor_First_Name, String.Empty),
                                                    .Supervisor_Last_Name = If(((ep.Supervisor_Last_Name IsNot Nothing) AndAlso (ep.Supervisor_Last_Name.Length > 0)), ep.Supervisor_Last_Name, String.Empty),
                                                    .Work_Location_Addr_Sid = If(((ep.Workaddress_Sid IsNot Nothing) AndAlso (ep.Workaddress_Sid > 0)), ep.Workaddress_Sid, 0),
                                                    .WorkAddress = If(((ep.WorkAddress IsNot Nothing) AndAlso (ep.WorkAddress.Length > 0)), ep.WorkAddress, String.Empty),
                                                    .WorkCounty = If(((ep.WorkCounty IsNot Nothing) AndAlso (ep.WorkCounty.Length > 0)), ep.WorkCounty, String.Empty)
                                                    }).FirstOrDefault()
                                emplst.Add(empInfo)
                            Next
                            retInfo.Emp_Info = emplst
                        Else
                            retInfo.Emp_Info = Nothing
                        End If
                        Dim courses = (From cc In oneSet
                                       Where cc.RN_DD_Person_Type_Xref_Sid = rid And cc.Course_Sid.HasValue
                                       Select cc.Course_Sid).Distinct.ToList()

                        If courses.Count > 0 Then
                            Dim coulst As New List(Of Model.Course_Info)
                            For Each cid In courses
                                Dim couInfo As New Model.Course_Info
                                couInfo = (From cou In oneSet
                                            Where cou.Course_Sid = cid
                                            Select New Model.Course_Info With {
                                                .Course_Number = If(((cou.OBN_Course_Number IsNot Nothing) AndAlso (cou.OBN_Course_Number.Length > 0)), cou.OBN_Course_Number, String.Empty),
                                                .Course_Sid = If(((cou.Course_Sid IsNot Nothing) AndAlso (cou.Course_Sid > 0)), cou.Course_Sid, 0),
                                                .Session_CEUs = If(((cou.Total_CEs IsNot Nothing) AndAlso (cou.Total_CEs > 0)), cou.Total_CEs, 0),
                                                .Session_Start_Date = If(cou.Start_Date.HasValue, cou.Start_Date, "12/31/9999"),
                                                .Session_Sid = If(((cou.Session_Sid IsNot Nothing) AndAlso (cou.Session_Sid > 0)), cou.Session_Sid, 0),
                                                .Session_End_Date = If(cou.End_Date.HasValue, cou.End_Date, "12/31/9999"),
                                                .Trainer_Name = If(((cou.Trainer_FN IsNot Nothing) AndAlso (cou.Trainer_FN.Length > 0)), cou.Trainer_FN, String.Empty) & " " & If(((cou.Trainer_LN IsNot Nothing) AndAlso (cou.Trainer_LN.Length > 0)), cou.Trainer_LN, String.Empty)
                                                }).FirstOrDefault()
                                coulst.Add(couInfo)
                            Next
                            retInfo.Course_Inof = coulst
                        Else
                            retInfo.Course_Inof = Nothing
                        End If

                        retList.Add(retInfo)
                    Next

                End If
                Return retList
            Catch ex As Exception
                Throw ex
            End Try
        End Function
        Public Shared Function MapRNList(ByVal rrInfo As List(Of USP_Get_RN_MAIS_Report_Results_Result)) As List(Of Model.MAIS_Report)
            Try
            Dim retList As New List(Of Model.MAIS_Report)
            If (rrInfo.Count > 0) Then
                Dim rnSid As List(Of Integer) = (From ri In rrInfo
                                                 Order By ri.RN_DD_Person_Type_Xref_Sid
                                                 Select ri.RN_DD_Person_Type_Xref_Sid).Distinct.ToList()
                For Each rid In rnSid
                    Dim retInfo As New Model.MAIS_Report
                    Dim oneSet = (From o In rrInfo
                                  Where o.RN_DD_Person_Type_Xref_Sid = rid
                                  Select o).Distinct
                        retInfo = (From r In oneSet
                                 Where r.RN_DD_Person_Type_Xref_Sid = rid
                                 Select New Model.MAIS_Report With {
                                           .DOB_LicenseIssue = Convert.ToDateTime(r.Date_Of_Original_Issuance).ToShortDateString(),
                                           .FirstName = If(((r.First_Name IsNot Nothing) AndAlso (r.First_Name.Length > 0)), r.First_Name, String.Empty),
                                           .HAddr_Sid = If(((r.Homeaddress_Sid IsNot Nothing) AndAlso (r.Homeaddress_Sid > 0)), r.Homeaddress_Sid, 0),
                                           .HomeAddress = If(((r.HomeAddress IsNot Nothing) AndAlso (r.HomeAddress.Length > 0)), r.HomeAddress, String.Empty),
                                           .HCounty = If(((r.HomeCounty IsNot Nothing) AndAlso (r.HomeCounty.Length > 0)), r.HomeCounty, String.Empty),
                                           .LastName = If(((r.Last_Name IsNot Nothing) AndAlso (r.Last_Name.Length > 0)), r.Last_Name, String.Empty),
                                           .MiddleName = If(((r.Middle_Name IsNot Nothing) AndAlso (r.Middle_Name.Length > 0)), r.Middle_Name, String.Empty),
                                           .RN_DD_Person_Type_Xref_Sid = r.RN_DD_Person_Type_Xref_Sid,
                                           .RN_DD_Sid = r.RN_Sid,
                                           .RNLicence_DDPersonnel = r.RNLicense_Number
                                     }).Distinct.FirstOrDefault()
                    Dim roles = (From rol In oneSet
                                   Where rol.RN_DD_Person_Type_Xref_Sid = rid
                                    Select rol.Role_RN_DD_Personnel_Xref_Sid, rol.Certification_Sid).Distinct.ToList()
                    If roles.Count > 0 Then
                        Dim certlst As New List(Of Model.Cert_Info)
                        For Each rolesid In roles
                            Dim CertInfo As New Model.Cert_Info
                                CertInfo = (From c In oneSet
                                                Where c.Certification_Sid = rolesid.Certification_Sid
                                                Select New Model.Cert_Info With {
                                                    .Attestant_Name = If(((c.Attestant_FN IsNot Nothing) AndAlso (c.Attestant_FN.Length > 0)), c.Attestant_FN, String.Empty) & " " & If(((c.Attestant_LN IsNot Nothing) AndAlso (c.Attestant_LN.Length > 0)), c.Attestant_LN, String.Empty),
                                                    .Attestant_Sid = If(((c.Attestant IsNot Nothing) AndAlso (c.Attestant > 0)), c.Attestant, 0),
                                                    .Certification_End_Date = If(IsDate(c.Certification_End_Date), c.Certification_End_Date, "12/31/9999"),
                                                    .Certification_Start_Date = If(IsDate(c.Certification_Start_Date), c.Certification_Start_Date, "12/31/9999"),
                                                    .Certification_Sid = c.Certification_Sid,
                                                    .Certification_Status = If(((c.Certification_Status_Desc IsNot Nothing) AndAlso (c.Certification_Status_Desc.Length > 0)), c.Certification_Status_Desc, String.Empty),
                                                    .Certification_Type = If(((c.Role_Desc IsNot Nothing) AndAlso (c.Role_Desc.Length > 0)), c.Role_Desc, String.Empty),
                                                    .RenewalCount = If(((c.Renewal_Count IsNot Nothing) AndAlso (c.Renewal_Count > 0)), c.Renewal_Count, 0)
                                                    }).FirstOrDefault()
                            certlst.Add(CertInfo)
                        Next
                        retInfo.Cert_Info = certlst
                    Else
                        retInfo.Cert_Info = Nothing
                    End If
                        Dim emps = (From e In oneSet
                                    Where e.RN_DD_Person_Type_Xref_Sid = rid And e.Employer_Sid.HasValue
                                    Select e.Employer_Sid).Distinct.ToList()
                    If emps.Count > 0 Then
                        Dim emplst As New List(Of Model.Employer_Info)
                        For Each eid In emps
                            Dim empInfo As New Model.Employer_Info
                            empInfo = (From ep In oneSet
                                            Where ep.Employer_Sid = eid
                                            Select New Model.Employer_Info With {
                                                .CEO_First_Name = If(((ep.CEO_First_Name IsNot Nothing) AndAlso (ep.CEO_First_Name.Length > 0)), ep.CEO_First_Name, String.Empty),
                                                .CEO_Last_Name = If(((ep.CEO_Last_Name IsNot Nothing) AndAlso (ep.CEO_Last_Name.Length > 0)), ep.CEO_Last_Name, String.Empty),
                                                .Employer_Name = If(((ep.Employer_Name IsNot Nothing) AndAlso (ep.Employer_Name.Length > 0)), ep.Employer_Name, String.Empty),
                                                .Employer_Sid = If(((ep.Employer_Sid IsNot Nothing) AndAlso (ep.Employer_Sid > 0)), ep.Employer_Sid, 0),
                                                .Supervisor_First_Name = If(((ep.Supervisor_First_Name IsNot Nothing) AndAlso (ep.Supervisor_First_Name.Length > 0)), ep.Supervisor_First_Name, String.Empty),
                                                .Supervisor_Last_Name = If(((ep.Supervisor_Last_Name IsNot Nothing) AndAlso (ep.Supervisor_Last_Name.Length > 0)), ep.Supervisor_Last_Name, String.Empty),
                                                .Work_Location_Addr_Sid = If(((ep.Workaddress_Sid IsNot Nothing) AndAlso (ep.Workaddress_Sid > 0)), ep.Workaddress_Sid, 0),
                                                .WorkAddress = If(((ep.WorkAddress IsNot Nothing) AndAlso (ep.WorkAddress.Length > 0)), ep.WorkAddress, String.Empty),
                                                .WorkCounty = If(((ep.WorkCounty IsNot Nothing) AndAlso (ep.WorkCounty.Length > 0)), ep.WorkCounty, String.Empty)
                                                }).FirstOrDefault()
                            emplst.Add(empInfo)
                        Next
                        retInfo.Emp_Info = emplst
                    Else
                        retInfo.Emp_Info = Nothing
                    End If
                        Dim courses = (From cc In oneSet
                                       Where cc.RN_DD_Person_Type_Xref_Sid = rid And cc.Course_Sid.HasValue
                                       Select cc.Course_Sid).Distinct.ToList()

                    If courses.Count > 0 Then
                        Dim coulst As New List(Of Model.Course_Info)
                        For Each cid In courses
                            Dim couInfo As New Model.Course_Info
                                couInfo = (From cou In oneSet
                                            Where cou.Course_Sid = cid
                                            Select New Model.Course_Info With {
                                                .Course_Number = If(((cou.OBN_Course_Number IsNot Nothing) AndAlso (cou.OBN_Course_Number.Length > 0)), cou.OBN_Course_Number, String.Empty),
                                                .Course_Sid = If(cou.Course_Sid.HasValue, cou.Course_Sid, 0),
                                                .Session_CEUs = If(cou.Total_CEs.HasValue, cou.Total_CEs, 0),
                                                .Session_End_Date = If(cou.End_Date.HasValue, cou.End_Date, "12/31/9999"),
                                                .Session_Sid = If(cou.Session_Sid.HasValue, cou.Session_Sid, 0),
                                                .Session_Start_Date = If(cou.Start_Date.HasValue, cou.Start_Date, "12/31/9999"),
                                                .Trainer_Name = If(((cou.Trainer_FN IsNot Nothing) AndAlso (cou.Trainer_FN.Length > 0)), cou.Trainer_FN, String.Empty) & " " & If(((cou.Trainer_LN IsNot Nothing) AndAlso (cou.Trainer_LN.Length > 0)), cou.Trainer_LN, String.Empty)
                                                }).FirstOrDefault()
                            coulst.Add(couInfo)
                        Next
                        retInfo.Course_Inof = coulst
                    Else
                        retInfo.Course_Inof = Nothing
                    End If

                    retList.Add(retInfo)
                Next

                End If
                Return retList
            Catch ex As Exception
                Throw ex
            End Try

        End Function
        Public Shared Function CourseInfo(ByVal c As List(Of Course_Info)) As List(Of Model.Course_Info)
            Dim cc As New List(Of Model.Course_Info)
            If (c.Count > 0) Then
                cc = (From y In c _
                        Select New Model.Course_Info With {
                            .Course_Sid = y.Course_Sid,
                            .Course_Number = y.Course_Number,
                            .Session_CEUs = y.Session_CEUs,
                            .Session_End_Date = If(y.Session_End_Date.HasValue, y.Session_End_Date.Value.ToShortDateString(), "12/31/9999"),
                            .Session_Sid = y.Session_Sid,
                            .Session_Start_Date = If(y.Session_Start_Date.HasValue, y.Session_Start_Date.Value.ToShortDateString(), "12/31/9999"),
                            .Trainer_Name = y.Trainer_Name_FN + " " + y.Trainer_Name_LN,
                            .Course_Trainer_Dropdown = y.Course_Number + "," + y.Trainer_Name_FN + " " + y.Trainer_Name_LN,
                            .Session_Display_Dropdown = If(y.Session_Sid > 0, If(y.Session_Start_Date.HasValue, y.Session_Start_Date.Value.ToShortDateString(), "12/31/9999") + "-" + If(y.Session_End_Date.HasValue, y.Session_End_Date.Value.ToShortDateString(), "12/31/9999") + ",CEUS:" + y.Session_CEUs.ToString(), String.Empty)
                        }).ToList

            End If
            Return cc
        End Function
        Public Shared Function DDSkillsInfo(ByVal sk As List(Of DDSkills_Info)) As List(Of Model.DDSkills_Info)
            Dim skill_list As New List(Of Model.DDSkills_Info)
            If (sk.Count > 0) Then
                skill_list = (From s In sk _
                        Select New Model.DDSkills_Info With {
                            .Category_Desc = s.Category_Desc,
                            .Skill_Desc = s.Skill_Desc,
                            .Skill_Type_Sid = s.Skill_Type_Sid,
                            .Category_Type_Sid = s.Category_Type_Sid,
                            .RN_DD_Person_Type_Xref_Sid = s.RN_DD_Person_Type_Xref_Sid,
                            .Verification_Date = s.Verification_Date,
                            .Verified_Person_Name = s.Verified_Person_Name,
                            .Verified_Person_Title = s.Verified_Person_Title,
                            .Skill_CheckList_Desc = s.Skill_CheckList_Desc
                      }).ToList
            End If
            Return skill_list
        End Function
        Public Shared Function CertInfo(ByVal cInfo As List(Of Cert_Info)) As List(Of Model.Cert_Info)
            Dim cc As New List(Of Model.Cert_Info)
            If (cInfo.Count > 0) Then
                cc = (From c In cInfo _
                       Select New Model.Cert_Info With {
                            .Attestant_Name = If(((c.Attestant_Name_FN IsNot Nothing) AndAlso (c.Attestant_Name_FN.Length > 0)), c.Attestant_Name_FN, String.Empty) & " " & If(((c.Attestant_Name_LN IsNot Nothing) AndAlso (c.Attestant_Name_LN.Length > 0)), c.Attestant_Name_LN, String.Empty),
                            .Attestant_Sid = If(c.Attestant_Sid > 0, c.Attestant_Sid, 0),
                            .Certification_End_Date = If(IsDate(c.Certification_End_Date), c.Certification_End_Date, "12/31/9999"),
                            .Certification_Start_Date = If(IsDate(c.Certification_Start_Date), c.Certification_Start_Date, "12/31/9999"),
                            .Certification_Sid = c.Certification_Sid,
                            .Certification_Status = If(((c.Certification_Status IsNot Nothing) AndAlso (c.Certification_Status.Length > 0)), c.Certification_Status, String.Empty),
                            .Certification_Type = If(((c.Certification_Type IsNot Nothing) AndAlso (c.Certification_Type.Length > 0)), c.Certification_Type, String.Empty),
                            .RenewalCount = If(c.RenewalCount > 0, c.RenewalCount, 0)
                            }).ToList()
            End If
            Return cc
        End Function
     
        Public Shared Function MapRNNotation(ByVal dd As List(Of RNSearchResult)) As List(Of Model.RNSearchResult)
            Dim retList As New List(Of Model.RNSearchResult)
            If (dd.Count > 0) Then
                retList = (From d In dd _
                    Select New Model.RNSearchResult With {
                        .County = d.County,
                        .DateRNIssuance = d.DateRNIssuance,
                        .RNLicenseNumber = d.RNLicenseNumber,
                        .FirstName = d.FirstName,
                        .HomeAddress = d.Address1 + " " + d.Address2 + " " + d.City + " " + d.State + " " + d.Zip + " " + d.ZipPlus,
                        .LastName = d.LastName,
                        .MiddleName = d.MiddleName,
                        .ICFRN = d.RN_17Bed,
                        .RNInstructor = d.RN_Instructor,
                        .QARN = d.RN_QA,
                        .RNMaster = d.RN_Master,
                        .RNTrainer = d.RN_Trainer
                    }).ToList
             
            End If
            Return retList
        End Function
        Public Shared Function MapSupervisorList(ByVal s As List(Of SupervisorDetails)) As List(Of Model.SupervisorDetails)
            Dim retList As New List(Of Model.SupervisorDetails)
            If (s.Count > 0) Then
                retList = (From ss In s
                          Select New Model.SupervisorDetails With {
                                .supFirstName = ss.supFirstName,
                                .supLastName = ss.supLastName,
                                .EmailAddress = ss.EmailAddress,
                                .supEndDate = ss.supEndDate.ToShortDateString(),
                                .supStartDate = ss.supStartDate.ToShortDateString(),
                                .PhoneNumber = ss.PhoneNumber
                              }).ToList()
               
            End If
            Return retList
        End Function
        Public Shared Function MapEmployerList(ByVal e As List(Of EmployerDetails)) As List(Of Model.EmployerDetails)
            Dim retList As New List(Of Model.EmployerDetails)
            If (e.Count > 0) Then
                retList = (From ee In e
                           Select New Model.EmployerDetails With {
                                .CEOFirstName = ee.CEOFirstName,
                                .CEOLastName = ee.CEOLastName,
                                .EmailAddress = ee.EmailAddress,
                                .EmpEndDate = ee.EmpEndDate.ToShortDateString(),
                                .EmployerName = ee.EmployerName,
                                .EmpStartDate = ee.EmpStartDate.ToShortDateString(),
                                .IdentificationValue = ee.IdentificationValue,
                                .PhoneNumber = ee.PhoneNumber
                               }).ToList()

            End If
            Return retList
        End Function
        Public Shared Function MapStatus(ByVal s As List(Of Certification_Status_Type)) As List(Of Model.ReportCertificationStatus)
            Dim ret As New List(Of Model.ReportCertificationStatus)
            If s.Count > 0 Then
                ret = (From certStatus In s
                       Select New Model.ReportCertificationStatus With {
                            .StatusCode = certStatus.Certification_Status_Desc,
                            .StatusID = certStatus.Certification_Status_Type_Sid
                           }).ToList()
                
            End If
            Return ret
        End Function
        Public Shared Function MapTypes(ByVal mr As List(Of MAIS_Role)) As List(Of Model.ReportCertificationType)
            Dim ret As New List(Of Model.ReportCertificationType)
            If mr.Count > 0 Then
                ret = (From certType In mr
                       Select New Model.ReportCertificationType With {
                          .TypeCode = certType.Role_Desc,
                          .TypeID = certType.Role_Sid
                         }).ToList()             
            End If
            Return ret
        End Function
        Public Shared Function MapRNStoreProcResultsForExcel(ByVal retList As List(Of USP_Get_RN_MAIS_Report_Results_Result)) As List(Of Model.MAISReportDetails)
            Dim ret As New List(Of Model.MAISReportDetails)
            Try
                If retList.Count > 0 Then
                    ret = (From sp In retList
                           Select New Model.MAISReportDetails With {
                              .Attestant_FN = If(((sp.Attestant_FN IsNot Nothing) AndAlso (sp.Attestant_FN.Length > 0)), sp.Attestant_FN, String.Empty),
                              .Attestant_LN = If(((sp.Attestant_LN IsNot Nothing) AndAlso (sp.Attestant_LN.Length > 0)), sp.Attestant_LN, String.Empty),
                              .Attestant_Sid = If(sp.Attestant.HasValue, sp.Attestant.HasValue, 0),
                              .CEO_First_Name = If(((sp.CEO_First_Name IsNot Nothing) AndAlso (sp.CEO_First_Name.Length > 0)), sp.CEO_First_Name, String.Empty),
                              .CEO_Last_Name = If(((sp.CEO_Last_Name IsNot Nothing) AndAlso (sp.CEO_Last_Name.Length > 0)), sp.CEO_Last_Name, String.Empty),
                              .Certification_End_Date = If(IsDate(sp.Certification_End_Date), sp.Certification_End_Date.ToShortDateString(), "12/31/9999"),
                              .Certification_Sid = If(sp.Certification_Sid > 0, sp.Certification_Sid, 0),
                              .Certification_Start_Date = If(IsDate(sp.Certification_Start_Date), sp.Certification_Start_Date.ToShortDateString(), "12/31/9999"),
                              .Certification_Status_Desc = If(((sp.Certification_Status_Desc IsNot Nothing) AndAlso (sp.Certification_Status_Desc.Length > 0)), sp.Certification_Status_Desc, String.Empty),
                              .Certification_Status_Sid = If(sp.Certification_Status_Sid > 0, sp.Certification_Status_Sid, 0),
                              .Certification_Status_Type_Sid = If(sp.Certification_Status_Type_Sid > 0, sp.Certification_Status_Type_Sid, 0),
                              .Course_Sid = If(sp.Course_Sid > 0, sp.Course_Sid, 0),
                              .Date_Of_Original_Issuance = Convert.ToDateTime(sp.Date_Of_Original_Issuance).ToShortDateString(),
                              .Employer_Name = If(((sp.Employer_Name IsNot Nothing) AndAlso (sp.Employer_Name.Length > 0)), sp.Employer_Name, String.Empty),
                              .Employer_Sid = If(sp.Employer_Sid.HasValue, sp.Employer_Sid, 0),
                              .First_Name = If(((sp.First_Name IsNot Nothing) AndAlso (sp.First_Name.Length > 0)), sp.First_Name, String.Empty),
                              .HomeAddress = If(((sp.HomeAddress IsNot Nothing) AndAlso (sp.HomeAddress.Length > 0)), sp.HomeAddress, String.Empty),
                              .Homeaddress_Sid = If(sp.Homeaddress_Sid.HasValue, sp.Homeaddress_Sid, 0),
                              .HomeCounty = If(((sp.HomeCounty IsNot Nothing) AndAlso (sp.HomeCounty.Length > 0)), sp.HomeCounty, String.Empty),
                              .Last_Name = If(((sp.Last_Name IsNot Nothing) AndAlso (sp.Last_Name.Length > 0)), sp.Last_Name, String.Empty),
                              .Middle_Name = If(((sp.Middle_Name IsNot Nothing) AndAlso (sp.Middle_Name.Length > 0)), sp.Middle_Name, String.Empty),
                              .OBN_Course_Number = If(((sp.OBN_Course_Number IsNot Nothing) AndAlso (sp.OBN_Course_Number.Length > 0)), sp.OBN_Course_Number, String.Empty),
                              .Renewal_Count = If(sp.Renewal_Count.HasValue, sp.Renewal_Count, 0),
                              .RN_DD_Person_Type_Xref_Sid = sp.RN_DD_Person_Type_Xref_Sid,
                              .RN_DD_Sid = sp.RN_Sid,
                              .RNLicense_DDCode = sp.RNLicense_Number,
                              .Role_Category_Level_Sid = If(sp.Role_Category_Level_Sid > 0, sp.Role_Category_Level_Sid, 0),
                              .Role_Desc = If(((sp.Role_Desc IsNot Nothing) AndAlso (sp.Role_Desc.Length > 0)), sp.Role_Desc, String.Empty),
                              .Role_RN_DD_Personnel_Xref_Sid = If(sp.Role_RN_DD_Personnel_Xref_Sid > 0, sp.Role_RN_DD_Personnel_Xref_Sid, 0),
                              .Session_End_Date = If(sp.End_Date.HasValue, sp.End_Date.Value.ToShortDateString(), "12/31/9999"),
                              .Session_Sid = If(sp.Session_Sid.HasValue, sp.Session_Sid, 0),
                              .Session_Start_Date = If(sp.Start_Date.HasValue, sp.Start_Date.Value.ToShortDateString(), "12/31/9999"),
                              .Supervisor_First_Name = If(((sp.Supervisor_First_Name IsNot Nothing) AndAlso (sp.Supervisor_First_Name.Length > 0)), sp.Supervisor_First_Name, String.Empty),
                              .Supervisor_Last_Name = If(((sp.Supervisor_Last_Name IsNot Nothing) AndAlso (sp.Supervisor_Last_Name.Length > 0)), sp.Supervisor_Last_Name, String.Empty),
                              .Total_CEs = If(sp.Total_CEs.HasValue, sp.Total_CEs, 0),
                              .Trainer_FN = If(((sp.Trainer_FN IsNot Nothing) AndAlso (sp.Trainer_FN.Length > 0)), sp.Trainer_FN, String.Empty),
                              .Trainer_LN = If(((sp.Trainer_LN IsNot Nothing) AndAlso (sp.Trainer_LN.Length > 0)), sp.Trainer_LN, String.Empty),
                              .WorkAddress = If(((sp.WorkAddress IsNot Nothing) AndAlso (sp.WorkAddress.Length > 0)), sp.WorkAddress, String.Empty),
                              .Workaddress_Sid = If(sp.Workaddress_Sid.HasValue, sp.Workaddress_Sid, 0),
                              .WorkCounty = If(((sp.WorkCounty IsNot Nothing) AndAlso (sp.WorkCounty.Length > 0)), sp.WorkCounty, String.Empty)
                             }).ToList()
                End If
            Catch ex As Exception

            End Try
            Return ret
        End Function
        Public Shared Function MapDDStoreProcResultsForExcel(ByVal retList As List(Of USP_Get_DDPersonnel_MAIS_Report_Results_Result)) As List(Of Model.MAISReportDetails)
            Dim ret As New List(Of Model.MAISReportDetails)
            Try
                If retList.Count > 0 Then
                    ret = (From sp In retList
                           Select New Model.MAISReportDetails With {
                              .Attestant_FN = If(((sp.Attestant_FN IsNot Nothing) AndAlso (sp.Attestant_FN.Length > 0)), sp.Attestant_FN, String.Empty),
                              .Attestant_LN = If(((sp.Attestant_LN IsNot Nothing) AndAlso (sp.Attestant_LN.Length > 0)), sp.Attestant_LN, String.Empty),
                              .Attestant_Sid = If(sp.Attestant.HasValue, sp.Attestant.HasValue, 0),
                              .CEO_First_Name = If(((sp.CEO_First_Name IsNot Nothing) AndAlso (sp.CEO_First_Name.Length > 0)), sp.CEO_First_Name, String.Empty),
                              .CEO_Last_Name = If(((sp.CEO_Last_Name IsNot Nothing) AndAlso (sp.CEO_Last_Name.Length > 0)), sp.CEO_Last_Name, String.Empty),
                              .Certification_End_Date = If(IsDate(sp.Certification_End_Date), sp.Certification_End_Date.ToShortDateString(), "12/31/9999"),
                              .Certification_Sid = If(sp.Certification_Sid > 0, sp.Certification_Sid, 0),
                              .Certification_Start_Date = If(IsDate(sp.Certification_Start_Date), sp.Certification_Start_Date.ToShortDateString(), "12/31/9999"),
                              .Certification_Status_Desc = If(((sp.Certification_Status_Desc IsNot Nothing) AndAlso (sp.Certification_Status_Desc.Length > 0)), sp.Certification_Status_Desc, String.Empty),
                              .Certification_Status_Sid = If(sp.Certification_Status_Sid > 0, sp.Certification_Status_Sid, 0),
                              .Certification_Status_Type_Sid = If(sp.Certification_Status_Type_Sid > 0, sp.Certification_Status_Type_Sid, 0),
                              .Course_Sid = If(sp.Course_Sid > 0, sp.Course_Sid, 0),
                              .Date_Of_Original_Issuance = Convert.ToDateTime(sp.DOB).ToShortDateString(),
                              .Employer_Name = If(((sp.Employer_Name IsNot Nothing) AndAlso (sp.Employer_Name.Length > 0)), sp.Employer_Name, String.Empty),
                              .Employer_Sid = If(sp.Employer_Sid.HasValue, sp.Employer_Sid, 0),
                              .First_Name = If(((sp.First_Name IsNot Nothing) AndAlso (sp.First_Name.Length > 0)), sp.First_Name, String.Empty),
                              .HomeAddress = If(((sp.HomeAddress IsNot Nothing) AndAlso (sp.HomeAddress.Length > 0)), sp.HomeAddress, String.Empty),
                              .Homeaddress_Sid = If(sp.Homeaddress_Sid.HasValue, sp.Homeaddress_Sid, 0),
                              .HomeCounty = If(((sp.HomeCounty IsNot Nothing) AndAlso (sp.HomeCounty.Length > 0)), sp.HomeCounty, String.Empty),
                              .Last_Name = If(((sp.Last_Name IsNot Nothing) AndAlso (sp.Last_Name.Length > 0)), sp.Last_Name, String.Empty),
                              .Middle_Name = If(((sp.Middle_Name IsNot Nothing) AndAlso (sp.Middle_Name.Length > 0)), sp.Middle_Name, String.Empty),
                              .OBN_Course_Number = If(((sp.OBN_Course_Number IsNot Nothing) AndAlso (sp.OBN_Course_Number.Length > 0)), sp.OBN_Course_Number, String.Empty),
                              .Renewal_Count = If(sp.Renewal_Count.HasValue, sp.Renewal_Count, 0),
                              .RN_DD_Person_Type_Xref_Sid = sp.RN_DD_Person_Type_Xref_Sid,
                              .RN_DD_Sid = sp.DDPersonnel_Sid,
                              .RNLicense_DDCode = sp.DDPersonnel_Code,
                              .Last4SSN = sp.SSN,
                              .Role_Category_Level_Sid = If(sp.Role_Category_Level_Sid > 0, sp.Role_Category_Level_Sid, 0),
                              .Role_Desc = If(((sp.Role_Desc IsNot Nothing) AndAlso (sp.Role_Desc.Length > 0)), sp.Role_Desc, String.Empty),
                              .Role_RN_DD_Personnel_Xref_Sid = If(sp.Role_RN_DD_Personnel_Xref_Sid > 0, sp.Role_RN_DD_Personnel_Xref_Sid, 0),
                              .Session_End_Date = If(sp.End_Date.HasValue, sp.End_Date.Value.ToShortDateString(), "12/31/9999"),
                              .Session_Sid = If(sp.Session_Sid.HasValue, sp.Session_Sid, 0),
                              .Session_Start_Date = If(sp.Start_Date.HasValue, sp.Start_Date.Value.ToShortDateString(), "12/31/9999"),
                              .Supervisor_First_Name = If(((sp.Supervisor_First_Name IsNot Nothing) AndAlso (sp.Supervisor_First_Name.Length > 0)), sp.Supervisor_First_Name, String.Empty),
                              .Supervisor_Last_Name = If(((sp.Supervisor_Last_Name IsNot Nothing) AndAlso (sp.Supervisor_Last_Name.Length > 0)), sp.Supervisor_Last_Name, String.Empty),
                              .Total_CEs = If(sp.Total_CEs.HasValue, sp.Total_CEs, 0),
                              .Trainer_FN = If(((sp.Trainer_FN IsNot Nothing) AndAlso (sp.Trainer_FN.Length > 0)), sp.Trainer_FN, String.Empty),
                              .Trainer_LN = If(((sp.Trainer_LN IsNot Nothing) AndAlso (sp.Trainer_LN.Length > 0)), sp.Trainer_LN, String.Empty),
                              .WorkAddress = If(((sp.WorkAddress IsNot Nothing) AndAlso (sp.WorkAddress.Length > 0)), sp.WorkAddress, String.Empty),
                              .Workaddress_Sid = If(sp.Workaddress_Sid.HasValue, sp.Workaddress_Sid, 0),
                               .WorkCounty = If(((sp.WorkCounty IsNot Nothing) AndAlso (sp.WorkCounty.Length > 0)), sp.WorkCounty, String.Empty)
                             }).ToList()

                End If

            Catch ex As Exception
                Throw ex
            End Try
            Return ret
        End Function
    End Class
End Namespace

