Imports ODMRDDHelperClassLibrary.Utility
Imports System.Configuration
Imports System.Data.Objects
Imports System.Data.Entity.Validation
Imports MAIS.Data.Objects


Namespace Queries
    Public Interface IWorkExperienceQueries
        Inherits IQueriesBase
        Function SaveWorkExperience(ByVal workExpInfo As Objects.WorkExperienceDetails) As ReturnObject(Of Long)
        Function GetAddedWorkExpList(ByVal applicationID As Integer) As List(Of WorkExperienceDetails)
        Function GetWorkExperienceByID(ByVal workID As Integer) As WorkExperienceDetails
        Function GetExistingWorkExperience(ByVal RNLicense As String) As List(Of WorkExperienceDetails)
        Function GetAddedIDs(ByVal applicationID As Integer) As List(Of Integer)
        Function DeleteWorkExperienceByID(ByVal workID As Integer) As ReturnObject(Of Boolean)
        Function GetDDExperienceFlg(ByVal RNDDUnique_Code As String, ByVal maisAppID As Integer) As RN_DD_Flags
        Function GetExperience(ByVal UniqueCode As String, ByVal AppID As Integer) As Integer
    End Interface
    Public Class WorkExperienceQueries
        Inherits QueriesBase
        Implements IWorkExperienceQueries
        Public Function GetDDExperienceFlg(ByVal RNDDUnique_Code As String, ByVal maisAppID As Integer) As RN_DD_Flags Implements IWorkExperienceQueries.GetDDExperienceFlg
            Dim retObj As New RN_DD_Flags
            Dim RN_Flg As Boolean = False
            Dim DD_Flg As Boolean = False
            Try
                Using context As New MAISContext
                    If Not (String.IsNullOrWhiteSpace(RNDDUnique_Code)) Then
                        Dim retCount = (From r In context.RNs
                                   Join rn In context.RN_DD_Person_Type_Xref On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                  Join rwe In context.RN_Work_Experience On rwe.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid
                                  Where r.RNLicense_Number = RNDDUnique_Code And rn.Person_Type_Sid = 1 And rwe.DD_Experience_Flg = True
                                  Select rwe).ToList()
                        If retCount.Count > 0 Then
                            DD_Flg = True
                        End If
                        Dim retCount1 = (From r In context.RNs
                                   Join rn In context.RN_DD_Person_Type_Xref On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                  Join rwe In context.RN_Work_Experience On rwe.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid
                                  Where r.RNLicense_Number = RNDDUnique_Code And rn.Person_Type_Sid = 1 And rwe.RN_Experience_Flg = True
                                  Select rwe).ToList()
                        If retCount1.Count > 0 Then
                            RN_Flg = True
                        End If
                    End If
                    If maisAppID > 0 Then
                        Dim weCount = (From a In context.RN_Application_Work_Experience
                                  Where a.Application_Sid = maisAppID And a.DD_Experience_flg = True
                                  Select a).ToList()
                        If weCount.Count > 0 Then
                            DD_Flg = True
                        End If
                        Dim weCount1 = (From a In context.RN_Application_Work_Experience
                                 Where a.Application_Sid = maisAppID And a.RN_Experience_flg = True
                                 Select a).ToList()
                        If weCount1.Count > 0 Then
                            RN_Flg = True
                        End If
                    End If
                    retObj.ChkDDFlg = DD_Flg
                    retObj.ChkRNFlg = RN_Flg
                End Using
            Catch ex As Exception
                Me.LogError("Error getting DD experience flag for RN.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting DD experience flag for RN.", True, False))
            End Try
            Return retObj
        End Function
        Public Function GetExperience(ByVal UniqueCode As String, ByVal AppID As Integer) As Integer Implements IWorkExperienceQueries.GetExperience
            Dim exp As Integer = 0
            Dim expActual As Double = 0
            Try
                Dim retList As New List(Of WorkExperienceSpanDates)
                Dim wePerm As New List(Of WorkExperienceSpanDates)
                Dim weTemp As New List(Of WorkExperienceSpanDates)
                Dim finallist As New List(Of WorkExperienceSpanDates)
                Dim readylist As New List(Of WorkExperienceSpanDates)
                Using context As New MAISContext
                    If Not (String.IsNullOrWhiteSpace(UniqueCode)) Then
                        wePerm = (From r In context.RNs
                                  Join rn In context.RN_DD_Person_Type_Xref On r.RN_Sid Equals rn.RN_DDPersonnel_Sid
                                  Join rwe In context.RN_Work_Experience On rwe.RN_DD_Person_Type_Xref_Sid Equals rn.RN_DD_Person_Type_Xref_Sid
                                  Where r.RNLicense_Number = UniqueCode And rn.Person_Type_Sid = 1 Order By rwe.WE_Start_Date
                                  Select New Objects.WorkExperienceSpanDates() With {
                                      .WeStartDate = rwe.WE_Start_Date,
                                      .WeEndDate = rwe.WE_End_Date,
                                      .KeyStringValue = rwe.WE_Start_Date + "-" + rwe.WE_End_Date
                                      }).Distinct.ToList()
                    End If
                    If (wePerm.Count > 0) Then
                        retList.AddRange(wePerm)
                    End If
                    If (AppID > 0) Then
                        weTemp = (From a In context.RN_Application_Work_Experience
                                  Where a.Application_Sid = AppID Order By a.Start_Date
                                  Select New Objects.WorkExperienceSpanDates() With {
                                        .WeStartDate = a.Start_Date,
                                        .WeEndDate = a.End_Date,
                                        .KeyStringValue = a.Start_Date + "-" + a.End_Date
                                      }).Distinct.ToList()
                    End If
                    If (weTemp.Count > 0) Then
                        retList.AddRange(weTemp)
                    End If
                    finallist = (From r In retList
                                Order By r.WeStartDate
                                Select r).ToList()
                    If finallist.Count > 1 Then
                        Dim firstSpan = finallist(0)
                        For i As Integer = 1 To finallist.Count - 1
                            If (Date.Parse(finallist(i).WeStartDate) <= Date.Parse(firstSpan.WeEndDate)) Then
                                If ((Date.Parse(finallist(i).WeStartDate) < Date.Parse(firstSpan.WeStartDate)) And (Date.Parse(finallist(i).WeEndDate) > Date.Parse(firstSpan.WeEndDate))) Then
                                    firstSpan.WeStartDate = finallist(i).WeStartDate
                                    firstSpan.WeEndDate = finallist(i).WeEndDate
                                ElseIf ((Date.Parse(finallist(i).WeStartDate) < Date.Parse(firstSpan.WeStartDate)) And (Date.Parse(finallist(i).WeEndDate) <= Date.Parse(firstSpan.WeEndDate))) Then
                                    firstSpan.WeStartDate = finallist(i).WeStartDate
                                ElseIf ((Date.Parse(finallist(i).WeStartDate) >= Date.Parse(firstSpan.WeStartDate)) And (Date.Parse(finallist(i).WeEndDate) > Date.Parse(firstSpan.WeEndDate))) Then
                                    firstSpan.WeEndDate = finallist(i).WeEndDate
                                End If
                            Else
                                expActual = expActual + MonthDifference(firstSpan.WeStartDate, firstSpan.WeEndDate)
                                firstSpan.WeStartDate = finallist(i).WeStartDate
                                firstSpan.WeEndDate = finallist(i).WeEndDate
                            End If
                        Next
                        'For Each TimeSpan In finallist

                        'Next
                        expActual = expActual + MonthDifference(firstSpan.WeStartDate, firstSpan.WeEndDate)
                    ElseIf finallist.Count > 0 Then
                        ' exp = Convert.ToInt32(retList(0).WeStartDate - retList(0).WeEndDate)
                        expActual = expActual + MonthDifference(finallist(0).WeStartDate, finallist(0).WeEndDate)
                    Else
                        '  exp = 0
                    End If
                End Using
            Catch ex As Exception
                Me.LogError("Error getting experience count for RN.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error getting experience count for RN.", True, False))
            End Try

            ' exp = Convert.ToInt32(expActual.ToString().Substring(0, expActual.ToString().IndexOf(".")))
            exp = Math.Floor(expActual)
            Return exp
        End Function
        Public Shared Function MonthDifference(ByVal first As DateTime, ByVal second As DateTime) As Double
            Dim dd As Double
            dd = second.Subtract(first).TotalDays / 30
            Return dd
        End Function
        Public Function DeleteWorkExperienceID(ByVal workID As Integer) As ReturnObject(Of Boolean) Implements IWorkExperienceQueries.DeleteWorkExperienceByID
            Dim retObj As New ReturnObject(Of Boolean)(False)
            Using context As New MAISContext()
                Try
                    If (workID > 0) Then
                        'get list of phone from cross reference table
                        Dim ifPhoneExists = (From ph In context.RN_Application_Work_Experience_Phone_Xref
                                             Where ph.RN_Application_Work_Experience_Sid = workID
                                             Select ph).ToList()
                        If ifPhoneExists IsNot Nothing Then
                            For Each phExists In ifPhoneExists
                                context.RN_Application_Work_Experience_Phone_Xref.Remove(phExists)
                            Next
                        End If

                        'get list of email from cross reference table
                        Dim ifEmailExists = (From em In context.RN_Application_Work_Experience_Email_Xref
                                            Where em.RN_Application_Work_Experience_Sid = workID
                                            Select em).ToList()
                        If ifEmailExists IsNot Nothing Then
                            For Each emExists In ifEmailExists
                                context.RN_Application_Work_Experience_Email_Xref.Remove(emExists)
                            Next
                        End If


                        'get list of address from cross reference table
                        Dim ifAddressExists = (From ad In context.RN_Application_Work_Experience_Address_Xref
                                           Where ad.RN_Application_Work_Experience_Sid = workID
                                           Select ad).ToList()
                        If ifAddressExists IsNot Nothing Then
                            For Each addrExists In ifAddressExists
                                context.RN_Application_Work_Experience_Address_Xref.Remove(addrExists)
                            Next
                        End If

                        'get exisitng work experience
                        Dim ifWorkExperienceExists = (From we In context.RN_Application_Work_Experience
                                                    Where we.RN_Application_Work_Experience_Sid = workID
                                                    Select we).FirstOrDefault()
                        If ifWorkExperienceExists IsNot Nothing Then
                            context.RN_Application_Work_Experience.Remove(ifWorkExperienceExists)
                        End If
                        context.SaveChanges()
                        retObj.ReturnValue = True
                    End If

                Catch ex As Exception
                    Me.LogError("Error deleting Currently Added Work Experience.", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error deleting Currently Added Work Experience.", True, False))
                End Try
            End Using
            Return retObj
        End Function
        Public Function GetAddedIDs(ByVal applicationID As Integer) As List(Of Integer) Implements IWorkExperienceQueries.GetAddedIDs
            Dim idlist As New List(Of Integer)
            Using context As New MAISContext()
                Try
                    idlist = (From we In context.RN_Application_Work_Experience
                                             Where we.Application_Sid = applicationID
                                             Select we.RN_Application_Work_Experience_Sid).ToList()

                Catch ex As Exception
                    Me.LogError("Error Getting Currently Added Work Experience List.", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting Currently Added Work Experience List.", True, False))
                End Try
            End Using
            Return idlist
        End Function
        Public Function SaveWorkExperience(workExpInfo As Objects.WorkExperienceDetails) As ReturnObject(Of Long) Implements IWorkExperienceQueries.SaveWorkExperience
            Dim retval As New ReturnObject(Of Long)(-1L)
            Using context As New MAISContext()
                Try
                    If ((workExpInfo IsNot Nothing) And (workExpInfo.AppID > 0)) Then
                        Dim _WorkInfo As RN_Application_Work_Experience = Nothing
                        Dim _weInfo As RN_Application_Work_Experience = Nothing
                        _WorkInfo = (From we In context.RN_Application_Work_Experience
                                     Where we.Application_Sid = workExpInfo.AppID And we.RN_Application_Work_Experience_Sid = workExpInfo.WorkID
                                     Select we).FirstOrDefault()
                        If (_WorkInfo Is Nothing) Then
                            'Insert work experience
                            _weInfo = New RN_Application_Work_Experience()
                            _weInfo.Application_Sid = workExpInfo.AppID
                            _weInfo.Agency_Name = workExpInfo.EmpName
                            _weInfo.RN_Experience_flg = workExpInfo.ChkRNFlg
                            _weInfo.DD_Experience_flg = workExpInfo.ChkDDFlg
                            _weInfo.Start_Date = workExpInfo.EmpStartDate
                            _weInfo.End_Date = workExpInfo.EmpEndDate
                            _weInfo.Title = workExpInfo.Title
                            _weInfo.Role_Description = workExpInfo.JobDuties
                            _weInfo.Active_Flg = True
                            _weInfo.Create_By = Me.UserID
                            _weInfo.Create_Date = DateTime.Now
                            _weInfo.Last_Update_By = Me.UserID
                            _weInfo.Last_Update_Date = DateTime.Now
                            context.RN_Application_Work_Experience.Add(_weInfo)
                        Else
                            'Update work experience
                            _WorkInfo.RN_Application_Work_Experience_Sid = workExpInfo.WorkID
                            _WorkInfo.Application_Sid = workExpInfo.AppID
                            _WorkInfo.Agency_Name = workExpInfo.EmpName
                            _WorkInfo.RN_Experience_flg = workExpInfo.ChkRNFlg
                            _WorkInfo.DD_Experience_flg = workExpInfo.ChkDDFlg
                            _WorkInfo.Start_Date = workExpInfo.EmpStartDate
                            _WorkInfo.End_Date = workExpInfo.EmpEndDate
                            _WorkInfo.Title = workExpInfo.Title
                            _WorkInfo.Role_Description = workExpInfo.JobDuties
                            _WorkInfo.Active_Flg = True
                            _WorkInfo.Last_Update_By = Me.UserID
                            _WorkInfo.Last_Update_Date = DateTime.Now
                        End If


                        'Address insert or update
                        Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", GetType(Integer))
                        Dim _address As Address_Lookup_And_Insert_Result = context.Address_Lookup_And_Insert(parameter, String.Empty, workExpInfo.Address.AddressLine1, workExpInfo.Address.AddressLine2, String.Empty, Convert.ToInt32(workExpInfo.Address.CountyID) _
                                                                                                                      , Convert.ToInt32(workExpInfo.Address.StateID), workExpInfo.Address.Zip + workExpInfo.Address.ZipPlus, workExpInfo.Address.City, 0).FirstOrDefault

                        ' Dim _existingAddress As New Application_Address
                        Dim addr_Xref_exist As RN_Application_Work_Experience_Address_Xref = Nothing

                        If (workExpInfo.WorkID > 0) Then
                            addr_Xref_exist = (From a In context.RN_Application_Work_Experience_Address_Xref
                                             Where a.RN_Application_Work_Experience_Sid = _WorkInfo.RN_Application_Work_Experience_Sid And
                                                   a.Address_Type_Sid = workExpInfo.Address.AddressType And a.RN_Application_Work_Experience_Sid = workExpInfo.WorkID And a.Active_Flg = True
                                             Select a).FirstOrDefault()
                        End If


                        If (addr_Xref_exist Is Nothing) Then
                            Dim ad As RN_Application_Work_Experience_Address_Xref = New RN_Application_Work_Experience_Address_Xref()
                            If (_WorkInfo IsNot Nothing) Then
                                ad.RN_Application_Work_Experience = _WorkInfo
                            Else
                                ad.RN_Application_Work_Experience = _weInfo
                            End If
                            ad.Address_Type_Sid = workExpInfo.Address.AddressType
                            ad.Address_Sid = parameter.Value
                            ad.Active_Flg = True
                            ad.Create_By = Me.UserID
                            ad.Create_Date = DateTime.Now
                            ad.Last_Update_By = Me.UserID
                            ad.Last_Update_Date = DateTime.Now
                            context.RN_Application_Work_Experience_Address_Xref.Add(ad)
                        Else
                            'Make active existing address Xref
                            addr_Xref_exist.Address_Sid = parameter.Value
                            addr_Xref_exist.Active_Flg = True
                            addr_Xref_exist.Last_Update_By = Me.UserID
                            addr_Xref_exist.Last_Update_Date = DateTime.Now
                        End If


                        If (workExpInfo.Address.Phone.Length > 0) Then  'Insert or update only when phone number is entered
                            'Phone insert or update
                            Dim phoneParameter As System.Data.Objects.ObjectParameter = New ObjectParameter("PhoneSID", GetType(Integer))
                            Dim _phoneaddress As Phone_Number_Lookup_And_Insert_Result = context.Phone_Number_Lookup_And_Insert(phoneParameter, workExpInfo.Address.Phone).FirstOrDefault()

                            'check for existing phone in Xref 
                            Dim Ph_Xref_exist As RN_Application_Work_Experience_Phone_Xref = Nothing
                            If (workExpInfo.WorkID > 0) Then
                                Ph_Xref_exist = (From px In context.RN_Application_Work_Experience_Phone_Xref
                                                Where px.RN_Application_Work_Experience_Sid = _WorkInfo.RN_Application_Work_Experience_Sid And
                                                        px.Contact_Type_Sid = workExpInfo.Address.ContactType And px.RN_Application_Work_Experience_Sid = workExpInfo.WorkID And px.Active_Flg = True
                                                Select px).FirstOrDefault()
                            End If
                            If (Ph_Xref_exist Is Nothing) Then
                                Dim phx As RN_Application_Work_Experience_Phone_Xref = New RN_Application_Work_Experience_Phone_Xref()
                                If (_WorkInfo IsNot Nothing) Then
                                    phx.RN_Application_Work_Experience = _WorkInfo
                                Else
                                    phx.RN_Application_Work_Experience = _weInfo
                                End If
                                phx.Contact_Type_Sid = workExpInfo.Address.ContactType
                                phx.Phone_Sid = phoneParameter.Value
                                phx.Active_Flg = True
                                phx.Create_By = Me.UserID
                                phx.Create_Date = DateTime.Now
                                phx.Last_Update_By = Me.UserID
                                phx.Last_Update_Date = DateTime.Now
                                context.RN_Application_Work_Experience_Phone_Xref.Add(phx)
                            Else
                                'Make active existing phone Xref    
                                Ph_Xref_exist.Phone_Sid = phoneParameter.Value
                                Ph_Xref_exist.Active_Flg = True
                                Ph_Xref_exist.Last_Update_By = Me.UserID
                                Ph_Xref_exist.Last_Update_Date = DateTime.Now
                            End If
                        End If

                        If (workExpInfo.Address.Email.Length > 0) Then 'Insert or update only when email  is entered
                            'Email insert or update
                            Dim emailParameter As System.Data.Objects.ObjectParameter = New ObjectParameter("EmailSID", GetType(Integer))
                            Dim _emailaddress As Email_Lookup_And_Insert_Result = context.Email_Lookup_And_Insert(emailParameter, workExpInfo.Address.Email).FirstOrDefault()


                            'Check for exisitng Email Xref
                            Dim em_Xref_exist As RN_Application_Work_Experience_Email_Xref = Nothing
                            If (workExpInfo.WorkID > 0) Then
                                em_Xref_exist = (From ex In context.RN_Application_Work_Experience_Email_Xref
                                                Where ex.RN_Application_Work_Experience_Sid = _WorkInfo.RN_Application_Work_Experience_Sid And
                                                        ex.Contact_Type_Sid = workExpInfo.Address.ContactType And ex.RN_Application_Work_Experience_Sid = workExpInfo.WorkID And ex.Active_Flg = True
                                                Select ex).FirstOrDefault()
                            End If

                            If (em_Xref_exist Is Nothing) Then
                                Dim e As RN_Application_Work_Experience_Email_Xref = New RN_Application_Work_Experience_Email_Xref()
                                If (_WorkInfo IsNot Nothing) Then
                                    e.RN_Application_Work_Experience = _WorkInfo
                                Else
                                    e.RN_Application_Work_Experience = _weInfo
                                End If
                                e.Contact_Type_Sid = workExpInfo.Address.ContactType
                                e.Email_Sid = emailParameter.Value
                                e.Active_Flg = True
                                e.Create_By = Me.UserID
                                e.Create_Date = DateTime.Now
                                e.Last_Update_By = Me.UserID
                                e.Last_Update_Date = DateTime.Now
                                context.RN_Application_Work_Experience_Email_Xref.Add(e)
                            Else
                                'Make exisitng Email Active
                                em_Xref_exist.Email_Sid = emailParameter.Value
                                em_Xref_exist.Active_Flg = True
                                em_Xref_exist.Last_Update_By = Me.UserID
                                em_Xref_exist.Last_Update_Date = DateTime.Now
                            End If

                        End If
                        context.SaveChanges()
                    End If
                Catch ex As DbEntityValidationException
                    Me._messages.Add(New ReturnMessage("Error while saving work experience information queries.", True, False))
                    Me.LogError("Error while saving work experience information queries.", CInt(Me.UserID), ex)
                    retval.AddErrorMessage(ex.Message)
                Catch ex As Exception
                    Me._messages.Add(New ReturnMessage("Error while saving work experience information queries.", True, False))
                    Me.LogError("Error while saving work experience information queries.", CInt(Me.UserID), ex)
                    retval.AddErrorMessage(ex.Message)
                End Try
            End Using
            Return retval
        End Function
        Public Function GetAddedWorkExpList(ByVal applicationID As Integer) As List(Of WorkExperienceDetails) Implements IWorkExperienceQueries.GetAddedWorkExpList
            Dim recentlyAddedWorkExpList As New List(Of Objects.WorkExperienceDetails)
            Using context As New MAISContext()
                Try
                    recentlyAddedWorkExpList = (From we In context.RN_Application_Work_Experience
                                             Where we.Application_Sid = applicationID
                                             Select New Objects.WorkExperienceDetails() With {
                                                 .WorkID = we.RN_Application_Work_Experience_Sid,
                                                 .ChkDDFlg = we.DD_Experience_flg,
                                                 .ChkRNFlg = we.RN_Experience_flg,
                                                 .EmpName = we.Agency_Name,
                                                 .EmpStartDate = we.Start_Date,
                                                 .EmpEndDate = we.End_Date,
                                                 .JobDuties = we.Role_Description,
                                                .Title = we.Title
                                                 }).ToList()

                Catch ex As Exception
                    Me.LogError("Error Getting Currently Added Work Experience List.", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting Currently Added Work Experience List.", True, False))
                End Try
            End Using
            Return recentlyAddedWorkExpList
        End Function
        Public Function GetExistingWorkExperience(ByVal RNLicense As String) As List(Of WorkExperienceDetails) Implements IWorkExperienceQueries.GetExistingWorkExperience
            Dim existingWorkExpList As New List(Of Objects.WorkExperienceDetails)
            Using context As New MAISContext()
                Try
                    Dim rns = (From r In context.RNs
                               Join pr In context.RN_DD_Person_Type_Xref On pr.RN_DDPersonnel_Sid Equals r.RN_Sid
                               Join we In context.RN_Work_Experience On we.RN_DD_Person_Type_Xref_Sid Equals pr.RN_DD_Person_Type_Xref_Sid
                               Where r.RNLicense_Number = RNLicense And pr.Active_Flg = True And r.Active_Flg = True
                               Select New Objects.WorkExperienceDetails() With {
                                    .WorkID = we.RN_Work_Experience_Sid,
                                    .EmpName = we.Agency_Name,
                                    .EmpStartDate = we.WE_Start_Date,
                                    .EmpEndDate = we.WE_End_Date,
                                    .Title = we.Title,
                                    .JobDuties = we.Role_Description,
                                    .ChkDDFlg = we.DD_Experience_Flg,
                                    .ChkRNFlg = we.RN_Experience_Flg
                                   }).ToList()

                    For Each RNW In rns
                        Dim Address1 As New AddressControlDetails
                        Address1 = (From addr In context.RN_Work_Experience_Address_Xref
                                    Where addr.RN_Work_Experience_Sid = RNW.WorkID
                                    Select New Objects.AddressControlDetails With {
                                        .AddressSID = addr.Address_Sid}).FirstOrDefault
                        If Address1.AddressSID > 0 Then
                            Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", Address1.AddressSID)
                            Dim address As Address_Lookup_And_Insert_Result = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).FirstOrDefault()
                            Address1.AddressLine1 = address.Address_Line1
                            Address1.AddressLine2 = address.Address_Line2
                            Address1.County = address.County_Desc
                            Address1.CountyID = address.CountyID
                            Address1.State = address.State_Abbr
                            Address1.StateID = address.StateID
                            If address.Zip.Length > 5 Then
                                Address1.Zip = address.Zip.Substring(0, 5)
                                Address1.ZipPlus = address.Zip.Substring(5, address.Zip.Length - 5)
                            Else
                                Address1.Zip = address.Zip.Substring(0, 5)
                                Address1.ZipPlus = String.Empty
                            End If

                            Address1.City = address.City
                            Address1.AddressSID = parameter.Value
                        End If

                        'Fetch contact type
                        Address1.ContactType = (From ph In context.RN_Work_Experience_Phone_Xref
                                               Where ph.RN_Work_Experience_Sid = RNW.WorkID And ph.Active_Flg = True
                                               Select ph.Contact_Type_Sid).FirstOrDefault()
                        'Fetch phone number
                        Dim Ph_Sid As Integer = (From ph In context.RN_Work_Experience_Phone_Xref
                                          Where ph.RN_Work_Experience_Sid = RNW.WorkID And ph.Active_Flg = True
                                          Select ph.Phone_Sid).FirstOrDefault()

                        If Ph_Sid > 0 Then
                            Dim phoneparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("PhoneSID", Ph_Sid)
                            Dim phoneaddress As Phone_Number_Lookup_And_Insert_Result = context.Phone_Number_Lookup_And_Insert(phoneparameter, String.Empty).FirstOrDefault()
                            Address1.Phone = phoneaddress.Phone_Number
                        Else
                            Address1.Phone = String.Empty
                        End If

                        'Fetch email address
                        Dim email_id As Integer = (From em In context.RN_Work_Experience_Email_Xref
                                                      Where em.RN_Work_Experience_Sid = RNW.WorkID And em.Active_Flg = True
                                                      Select em.Email_Sid).FirstOrDefault()

                        If (email_id > 0) Then
                            Dim emailparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("EmailSID", email_id)
                            Dim emailaddress As Email_Lookup_And_Insert_Result = context.Email_Lookup_And_Insert(emailparameter, String.Empty).FirstOrDefault()
                            Address1.Email = emailaddress.Email_Address
                        Else
                            Address1.Email = String.Empty
                        End If

                        'add the address info to the list
                        RNW.Address = Address1

                    Next
                    If (rns IsNot Nothing) Then
                        existingWorkExpList = rns
                    End If


                Catch ex As Exception
                    Me.LogError("Error Getting Existing Work Experience List.", CInt(Me.UserID), ex)
                    Me._messages.Add(New ReturnMessage("Error Getting Existing Work Experience List.", True, False))
                End Try
            End Using
            Return existingWorkExpList
        End Function
        Public Function GetWorkExperienceByID(ByVal workID As Integer) As WorkExperienceDetails Implements IWorkExperienceQueries.GetWorkExperienceByID
            Dim weInfo As New WorkExperienceDetails
            Dim address1 As New AddressControlDetails
            Try
                Using context As New MAISContext()
                    'Fetch work experience 
                    weInfo = (From w In context.RN_Application_Work_Experience
                              Where w.RN_Application_Work_Experience_Sid = workID And w.Active_Flg = True
                              Select New Objects.WorkExperienceDetails() With {
                                  .AppID = w.Application_Sid,
                                  .WorkID = w.RN_Application_Work_Experience_Sid,
                                  .EmpName = w.Agency_Name,
                                  .ChkDDFlg = w.DD_Experience_flg,
                                  .ChkRNFlg = w.RN_Experience_flg,
                                  .EmpStartDate = w.Start_Date,
                                  .EmpEndDate = w.End_Date,
                                  .Title = w.Title,
                                  .JobDuties = w.Role_Description
                              }).FirstOrDefault()
                    'Fetch address  
                    address1 = (From addr In context.RN_Application_Work_Experience_Address_Xref
                                Where addr.RN_Application_Work_Experience_Sid = workID And addr.Active_Flg = True
                                Select New Objects.AddressControlDetails() With {
                                    .AddressSID = addr.Address_Sid
                                }).FirstOrDefault()
                    If address1.AddressSID > 0 Then
                        Dim parameter As System.Data.Objects.ObjectParameter = New ObjectParameter("AddressSID", address1.AddressSID)
                        Dim address As Address_Lookup_And_Insert_Result = context.Address_Lookup_And_Insert(parameter, String.Empty, String.Empty, String.Empty, String.Empty, 0, 0, String.Empty, String.Empty, 0).FirstOrDefault()
                        address1.AddressLine1 = address.Address_Line1
                        address1.AddressLine2 = address.Address_Line2
                        address1.County = address.County_Desc
                        address1.CountyID = address.CountyID
                        address1.State = address.State_Abbr
                        address1.StateID = address.StateID
                        If address.Zip.Length > 5 Then
                            address1.Zip = address.Zip.Substring(0, 5)
                            address1.ZipPlus = address.Zip.Substring(5, address.Zip.Length - 5)
                        Else
                            address1.Zip = address.Zip.Substring(0, 5)
                            address1.ZipPlus = String.Empty
                        End If

                        address1.City = address.City
                        address1.AddressSID = parameter.Value
                    End If

                    'Fetch contact type
                    address1.ContactType = (From ph In context.RN_Application_Work_Experience_Phone_Xref
                                           Where ph.RN_Application_Work_Experience_Sid = workID And ph.Active_Flg = True
                                           Select ph.Contact_Type_Sid).FirstOrDefault()
                    'Fetch phone number
                    Dim Ph_Sid As Integer = (From ph In context.RN_Application_Work_Experience_Phone_Xref
                                      Where ph.RN_Application_Work_Experience_Sid = workID And ph.Active_Flg = True
                                      Select ph.Phone_Sid).FirstOrDefault()
                    If Ph_Sid > 0 Then
                        Dim phoneparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("PhoneSID", Ph_Sid)
                        Dim phoneaddress As Phone_Number_Lookup_And_Insert_Result = context.Phone_Number_Lookup_And_Insert(phoneparameter, String.Empty).FirstOrDefault()
                        address1.Phone = phoneaddress.Phone_Number
                    Else
                        address1.Phone = String.Empty
                    End If

                    'Fetch email address
                    Dim email_id As Integer = (From em In context.RN_Application_Work_Experience_Email_Xref
                                                  Where em.RN_Application_Work_Experience_Sid = workID And em.Active_Flg = True
                                                  Select em.Email_Sid).FirstOrDefault()
                    If (email_id > 0) Then
                        Dim emailparameter As System.Data.Objects.ObjectParameter = New ObjectParameter("EmailSID", email_id)
                        Dim emailaddress As Email_Lookup_And_Insert_Result = context.Email_Lookup_And_Insert(emailparameter, String.Empty).FirstOrDefault()
                        address1.Email = emailaddress.Email_Address
                    Else
                        address1.Email = String.Empty
                    End If
                    weInfo.Address = address1
                End Using
            Catch ex As Exception
                Me.LogError("Error fetching work experience information by ID.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error fetching work experience information by ID.", True, False))
            End Try
            Return weInfo
        End Function


    End Class
End Namespace
