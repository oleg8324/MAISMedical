Imports System.Data.Linq
Imports ODMRDDHelperClassLibrary.Utility
Imports System.Configuration
Imports System.Data.Objects
Imports MAIS.Data
Imports System.Data.Entity.Validation
Imports System.Transactions
'Imports Business.Model.Enums
'Imports MAIS.MAIS.Business.Helpers
Namespace Queries
    Public Interface IMoveTempToPermQueries
        Inherits IQueriesBase
        Function GetRnOrDDSid(ByVal rnorddCode As String, ByVal rnflag As Boolean) As Integer
        Function GetCertStatSid(ByVal sDesc As String, ByVal asDesc As String) As Integer
        Function InsertPersonIfNotExists(ByVal appId As Integer, ByVal rnflag As Boolean) As ReturnObject(Of Long)
        Function SaveToPerm(ByVal appId As Integer, ByVal rnflag As Boolean, ByVal currentRCL As Integer, ByVal stDate As Date, ByVal endDate As Date, ByVal statusDesc As String, ByVal adminStatus As String) As ReturnObject(Of Long)
    End Interface
    Public Class MoveTempToPermQueries
        Inherits QueriesBase
        Implements IMoveTempToPermQueries
        Public Function GetRnOrDDSid(ByVal rnorddCode As String, ByVal rnflag As Boolean) As Integer Implements IMoveTempToPermQueries.GetRnOrDDSid
            Dim rs As Integer = -1
            If (rnflag = False) Then
                Try
                    Using Context As New MAISContext
                        rs = (From a In Context.DDPersonnels _
                                  Where a.DDPersonnel_Code = rnorddCode _
                                  Select a.DDPersonnel_Sid).FirstOrDefault

                    End Using

                Catch ex As Exception
                    Me._messages.Add(New ReturnMessage("Error while getting DD info.", True, False))
                    Me.LogError("Error while getting DD info.", CInt(Me.UserID), ex)
                    'retVal.AddErrorMessage(ex.Message)
                End Try
            Else
                Try
                    Using Context As New MAISContext
                        rs = (From a In Context.RNs _
                                  Where a.RNLicense_Number = rnorddCode _
                                  Select a.RN_Sid).FirstOrDefault

                    End Using

                Catch ex As Exception
                    Me._messages.Add(New ReturnMessage("Error while getting RN info.", True, False))
                    Me.LogError("Error while getting RN info.", CInt(Me.UserID), ex)
                    'retVal.AddErrorMessage(ex.Message)
                End Try
            End If
            Return rs
        End Function
        Public Function GetCertStatSid(ByVal sDesc As String, ByVal asDesc As String) As Integer Implements IMoveTempToPermQueries.GetCertStatSid
            Dim certDesc As String = ""
            Try
                Using Context As New MAISContext
                    If sDesc = "Did Not Meet Requirements" Then
                        certDesc = "Did Not Meet Requirements"
                    ElseIf sDesc = "Meets Requirements" Then
                        certDesc = "Certified"
                    ElseIf sDesc = "Added To Registry" Then
                        certDesc = "Registered"
                    ElseIf sDesc = "Removed From Registry" Then
                        certDesc = "Unregistered"
                    ElseIf sDesc = "DODD Review" Then
                        If Not String.IsNullOrEmpty(asDesc) Then
                            If asDesc <> "Select Decision Status" Then
                                certDesc = asDesc
                            End If
                        End If
                    End If
                    If certDesc <> "" Then
                        Return (From cs In Context.Certification_Status_Type Where cs.Certification_Status_Desc = certDesc Select cs.Certification_Status_Type_Sid).FirstOrDefault
                    Else
                        Return -1
                    End If
                End Using

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while getting certification status sid.", True, False))
                Me.LogError("Error while getting certification status sid.", CInt(Me.UserID), ex)
                Return -1
                'retVal.AddErrorMessage(ex.Message)
            End Try

        End Function
        Public Function InsertPersonIfNotExists(ByVal appId As Integer, ByVal rnflag As Boolean) As ReturnObject(Of Long) Implements IMoveTempToPermQueries.InsertPersonIfNotExists
            Dim rntx As RN_DD_Person_Type_Xref = New RN_DD_Person_Type_Xref()
            Dim retval As New ReturnObject(Of Long)(-1L)
            Dim rntable As Data.RN = Nothing
            Dim ddtable As Data.DDPersonnel = Nothing
            Dim ptypeSid As Integer = IIf(rnflag, 1, 2)
            Dim ddorrncode As String
            Try
                Using sc As New TransactionScope()
                    Using context As New MAISContext()

                        Dim _rndetails As RN_Application = Nothing
                        If (rnflag) Then

                            _rndetails = (From rn In context.RN_Application
                                          Where rn.Application_Sid = appId
                                          Select rn).FirstOrDefault()
                            ddorrncode = _rndetails.RNLicense_Number
                            rntable = (From r In context.RNs Where r.RNLicense_Number = _rndetails.RNLicense_Number
                                Select r).FirstOrDefault()
                            If (rntable Is Nothing) Then
                                Dim _rnInsert As RN = New RN()
                                _rnInsert.RNLicense_Number = _rndetails.RNLicense_Number
                                _rnInsert.Date_Of_Original_Issuance = _rndetails.Date_Of_Original_Issuance
                                _rnInsert.Middle_Name = _rndetails.Middle_Name
                                _rnInsert.First_Name = _rndetails.First_Name
                                _rnInsert.Last_Name = _rndetails.Last_Name
                                _rnInsert.Gender = _rndetails.Gender
                                _rnInsert.Create_By = Me.UserID
                                _rnInsert.Create_Date = DateTime.Now
                                _rnInsert.Last_Update_By = Me.UserID
                                _rnInsert.Last_Updated_Date = DateTime.Now
                                _rnInsert.Active_Flg = True
                                context.RNs.Add(_rnInsert)
                                context.SaveChanges()
                                rntx.Person_Type_Sid = ptypeSid
                                rntx.RN_DDPersonnel_Sid = _rnInsert.RN_Sid
                                rntx.Active_Flg = True
                                rntx.Create_By = Me.UserID
                                rntx.Create_Date = DateTime.Now
                                rntx.Last_Update_By = Me.UserID
                                rntx.Last_Update_Date = DateTime.Now
                                context.RN_DD_Person_Type_Xref.Add(rntx)
                            Else
                                rntx = (From r In context.RN_DD_Person_Type_Xref Where r.RN_DDPersonnel_Sid = rntable.RN_Sid And r.Person_Type_Sid = ptypeSid Select r).FirstOrDefault
                            End If
                        Else
                            Dim _dddetails As DDPersonnel_Application = Nothing
                            _dddetails = (From rn In context.DDPersonnel_Application
                                          Where rn.Application_Sid = appId
                                          Select rn).FirstOrDefault()
                            Dim ddCode As String = ""
                            ddCode = (From aa In context.Applications Where aa.Application_Sid = appId Select aa.RN_DD_Unique_Code).FirstOrDefault
                            If (String.IsNullOrEmpty(ddCode) = False) Then
                                ddtable = (From d In context.DDPersonnels Where d.DDPersonnel_Code = ddCode Select d).FirstOrDefault
                            End If
                            If (ddtable Is Nothing) Then
                                Dim maxSid As Integer
                                maxSid = (From dd In context.DDPersonnels Select dd.DDPersonnel_Sid).Max()
                                Dim _ddInsert As DDPersonnel = New DDPersonnel()
                                _ddInsert.SSN = _dddetails.SSN
                                _ddInsert.DOB = _dddetails.DOB
                                _ddInsert.Middle_Name = _dddetails.Middle_Name
                                _ddInsert.First_Name = _dddetails.First_Name
                                _ddInsert.Last_Name = _dddetails.Last_Name
                                _ddInsert.Gender = _dddetails.Gender
                                _ddInsert.Create_By = Me.UserID
                                _ddInsert.Create_Date = DateTime.Now
                                _ddInsert.Last_Update_By = Me.UserID
                                _ddInsert.Last_Updated_Date = DateTime.Now
                                _ddInsert.Active_Flg = True
                                ddorrncode = "DD" & (maxSid + 2).ToString.PadLeft(8, "0")
                                _ddInsert.DDPersonnel_Code = ddorrncode

                                context.DDPersonnels.Add(_ddInsert)
                                context.SaveChanges()
                                _ddInsert.DDPersonnel_Code = "DD" & (_ddInsert.DDPersonnel_Sid).ToString.PadLeft(8, "0")
                                ddorrncode = _ddInsert.DDPersonnel_Code
                                rntx.Person_Type_Sid = ptypeSid
                                rntx.RN_DDPersonnel_Sid = _ddInsert.DDPersonnel_Sid
                                rntx.Active_Flg = True
                                rntx.Create_By = Me.UserID
                                rntx.Create_Date = DateTime.Now
                                rntx.Last_Update_By = Me.UserID
                                rntx.Last_Update_Date = DateTime.Now
                                context.RN_DD_Person_Type_Xref.Add(rntx)
                            Else
                                ddorrncode = ddtable.DDPersonnel_Code
                                rntx = (From r In context.RN_DD_Person_Type_Xref Where r.RN_DDPersonnel_Sid = ddtable.DDPersonnel_Sid And r.Person_Type_Sid = ptypeSid Select r).FirstOrDefault

                            End If
                        End If
                        Dim app As Application = (From ap In context.Applications Where ap.Application_Sid = appId Select ap).FirstOrDefault
                        If Not IsNothing(app) Then
                            app.RN_DD_Unique_Code = ddorrncode
                        End If
                        Dim appHis As Application_History = (From ah In context.Application_History Where ah.Application_Sid = appId Select ah).FirstOrDefault
                        If Not IsNothing(appHis) Then
                            appHis.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid
                        End If
                        context.SaveChanges()
                        retval.ReturnValue = 0
                        retval.AddGeneralMessage("UniqueCode," & ddorrncode)

                    End Using
                    sc.Complete()
                End Using
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while saving information to database.", True, False))
                Me.LogError("Error while saving information to database.", CInt(Me.UserID), ex)
                retval.AddErrorMessage(ex.Message)
                Throw
            End Try
            Return retval
        End Function
        Public Function SaveToPerm(ByVal appId As Integer, ByVal rnflag As Boolean, ByVal currentRCL As Integer, ByVal stDate As Date, ByVal endDate As Date, ByVal statusDesc As String, ByVal adminStatus As String) As ReturnObject(Of Long) Implements IMoveTempToPermQueries.SaveToPerm 'add startdate and endate
            Dim retval As New ReturnObject(Of Long)(-1L)
            Dim rntable As Data.RN = Nothing
            Dim ddtable As Data.DDPersonnel = Nothing
            Dim ptypeSid As Integer = IIf(rnflag, 1, 2)
            Dim ddorrncode As String
            Dim notFinal As Boolean = False
            'Dim rnddsid As Integer = GetRnOrDDSid(appId, rnflag)

            Dim rntx As RN_DD_Person_Type_Xref = New RN_DD_Person_Type_Xref()
            Try
                Using scope As New TransactionScope()
                    Using context As New MAISContext()
                        'Try
                        Dim finStatDesc As String = ""
                        finStatDesc = IIf(adminStatus = "", statusDesc, adminStatus)
                        Dim statusSid As Integer = 0

                        Dim app As Application = (From a In context.Applications Where a.Application_Sid = appId Select a).FirstOrDefault
                        If app IsNot Nothing Then
                            If finStatDesc = "Pending" And app.Application_Type_Sid = 4 Then
                                finStatDesc = "Meets Requirements"
                                statusDesc = "Meets Requirements"
                            End If
                            statusSid = (From s In context.Application_Status_Type Where s.Application_Status_Desc = finStatDesc Select s.Application_Status_Type_Sid).FirstOrDefault

                            app.Application_Status_Type_Sid = statusSid
                            app.Last_Update_By = Me.UserID
                            app.Last_Update_Date = Date.Today
                        End If
                        If ((statusDesc = "Pending" And app.Application_Type_Sid <> 4) Or (statusDesc = "DODD Review" And (String.IsNullOrEmpty(adminStatus) OrElse adminStatus = "Select Decision Status" OrElse adminStatus = "Intent to Deny"))) Then
                            notFinal = True
                        End If
                        If (statusDesc <> "Pending" And statusDesc <> "Voided Application") Then
                            Dim certStSid As Integer = GetCertStatSid(statusDesc, adminStatus)
                            Dim _rndetails As RN_Application = Nothing
                            If (rnflag) Then

                                _rndetails = (From rn In context.RN_Application
                                              Where rn.Application_Sid = appId
                                              Select rn).FirstOrDefault()
                                ddorrncode = _rndetails.RNLicense_Number
                                rntable = (From r In context.RNs Where r.RNLicense_Number = _rndetails.RNLicense_Number
                                    Select r).FirstOrDefault()
                                app.RN_DD_Unique_Code = _rndetails.RNLicense_Number
                                If (rntable Is Nothing) Then

                                    Dim _rnInsert As RN = New RN()
                                    _rnInsert.RNLicense_Number = _rndetails.RNLicense_Number
                                    _rnInsert.Date_Of_Original_Issuance = _rndetails.Date_Of_Original_Issuance
                                    _rnInsert.Middle_Name = _rndetails.Middle_Name
                                    _rnInsert.First_Name = _rndetails.First_Name
                                    _rnInsert.Last_Name = _rndetails.Last_Name
                                    _rnInsert.Gender = _rndetails.Gender
                                    _rnInsert.Create_By = Me.UserID
                                    _rnInsert.Create_Date = DateTime.Now
                                    _rnInsert.Last_Update_By = Me.UserID
                                    _rnInsert.Last_Updated_Date = DateTime.Now
                                    _rnInsert.Active_Flg = True
                                    context.RNs.Add(_rnInsert)
                                    context.SaveChanges()
                                    rntx.Person_Type_Sid = ptypeSid
                                    rntx.RN_DDPersonnel_Sid = _rnInsert.RN_Sid
                                    rntx.Active_Flg = True
                                    rntx.Create_By = Me.UserID
                                    rntx.Create_Date = DateTime.Now
                                    rntx.Last_Update_By = Me.UserID
                                    rntx.Last_Update_Date = DateTime.Now
                                    context.RN_DD_Person_Type_Xref.Add(rntx)
                                Else
                                    'Assign RNTX to the right XRef here
                                    rntx = (From r In context.RN_DD_Person_Type_Xref Where r.RN_DDPersonnel_Sid = rntable.RN_Sid And r.Person_Type_Sid = ptypeSid Select r).FirstOrDefault
                                    If rntable.Middle_Name <> _rndetails.Middle_Name Or rntable.First_Name <> _rndetails.First_Name Or rntable.Last_Name <> _rndetails.Last_Name Then
                                        retval.AddGeneralMessage("Name Changed")
                                    End If
                                    rntable.Middle_Name = _rndetails.Middle_Name
                                    rntable.First_Name = _rndetails.First_Name
                                    rntable.Last_Name = _rndetails.Last_Name
                                    rntable.Gender = _rndetails.Gender
                                    rntable.Date_Of_Original_Issuance = _rndetails.Date_Of_Original_Issuance
                                    rntable.RNLicense_Number = _rndetails.RNLicense_Number
                                    rntable.Last_Update_By = Me.UserID
                                    rntable.Last_Updated_Date = DateTime.Now
                                End If
                            Else
                                Dim _dddetails As DDPersonnel_Application = Nothing
                                _dddetails = (From rn In context.DDPersonnel_Application
                                              Where rn.Application_Sid = appId
                                              Select rn).FirstOrDefault()
                                Dim ddCode As String = ""
                                ddCode = (From aa In context.Applications Where aa.Application_Sid = appId Select aa.RN_DD_Unique_Code).FirstOrDefault
                                If (String.IsNullOrEmpty(ddCode) = False) Then
                                    ddtable = (From d In context.DDPersonnels Where d.DDPersonnel_Code = ddCode Select d).FirstOrDefault
                                End If

                                If (ddtable Is Nothing) Then
                                    Dim maxSid As Integer
                                    maxSid = (From dd In context.DDPersonnels Select dd.DDPersonnel_Sid).Max()
                                    Dim _ddInsert As DDPersonnel = New DDPersonnel()
                                    _ddInsert.SSN = _dddetails.SSN
                                    _ddInsert.DOB = _dddetails.DOB
                                    _ddInsert.Middle_Name = _dddetails.Middle_Name
                                    _ddInsert.First_Name = _dddetails.First_Name
                                    _ddInsert.Last_Name = _dddetails.Last_Name
                                    _ddInsert.Gender = _dddetails.Gender
                                    _ddInsert.Create_By = Me.UserID
                                    _ddInsert.Create_Date = DateTime.Now
                                    _ddInsert.Last_Update_By = Me.UserID
                                    _ddInsert.Last_Updated_Date = DateTime.Now
                                    _ddInsert.Active_Flg = True
                                    ddorrncode = "DD" & (maxSid + 2).ToString.PadLeft(8, "0")
                                    _ddInsert.DDPersonnel_Code = ddorrncode

                                    context.DDPersonnels.Add(_ddInsert)
                                    context.SaveChanges()
                                    _ddInsert.DDPersonnel_Code = "DD" & (_ddInsert.DDPersonnel_Sid).ToString.PadLeft(8, "0")
                                    ddorrncode = _ddInsert.DDPersonnel_Code
                                    app.RN_DD_Unique_Code = _ddInsert.DDPersonnel_Code
                                    rntx.Person_Type_Sid = ptypeSid
                                    rntx.RN_DDPersonnel_Sid = _ddInsert.DDPersonnel_Sid
                                    rntx.Active_Flg = True
                                    rntx.Create_By = Me.UserID
                                    rntx.Create_Date = DateTime.Now
                                    rntx.Last_Update_By = Me.UserID
                                    rntx.Last_Update_Date = DateTime.Now
                                    context.RN_DD_Person_Type_Xref.Add(rntx)
                                Else
                                    ddorrncode = ddtable.DDPersonnel_Code
                                    'If appType = "Initial" Then
                                    '    Me._messages.Add(New ReturnMessage("Error while saving personal information. Person with this ID already exist. Cannot do Initial application type ", True, False))
                                    '    Me.LogError("Error while saving personal information. Person with this ID already exist. Cannot do Initial application type")
                                    '    retval.AddErrorMessage("Error while saving personal information. Person with this ID already exist. Cannot do Initial application type")
                                    '    Return retval
                                    'End If
                                    app.RN_DD_Unique_Code = ddorrncode
                                    rntx = (From r In context.RN_DD_Person_Type_Xref Where r.RN_DDPersonnel_Sid = ddtable.DDPersonnel_Sid And r.Person_Type_Sid = ptypeSid Select r).FirstOrDefault
                                    If ddtable.Middle_Name <> _dddetails.Middle_Name Or ddtable.First_Name <> _dddetails.First_Name Or ddtable.Last_Name <> _dddetails.Last_Name Then
                                        retval.AddGeneralMessage("Name Changed")
                                    End If
                                    ddtable.Middle_Name = _dddetails.Middle_Name
                                    ddtable.First_Name = _dddetails.First_Name
                                    ddtable.Last_Name = _dddetails.Last_Name
                                    ddtable.DOB = _dddetails.DOB
                                    ddtable.Gender = _dddetails.Gender
                                    ddtable.SSN = _dddetails.SSN
                                    ddtable.Last_Update_By = Me.UserID
                                    ddtable.Last_Updated_Date = DateTime.Now

                                End If
                            End If
                            If Not IsNothing(app) Then
                                app.RN_DD_Unique_Code = ddorrncode
                            End If
                            Dim appHis As Application_History = (From ah In context.Application_History Where ah.Application_Sid = appId Select ah).FirstOrDefault
                            If Not IsNothing(appHis) Then
                                appHis.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid
                            End If

                            context.SaveChanges()
                            retval.AddGeneralMessage("UniqueCode," & ddorrncode)
                            Dim appDetails As Objects.ApplicationInformation = (From application In context.Applications
                               Where application.Application_Sid = appId
                               Select New Objects.ApplicationInformation() With {
                                       .ApplicationID = application.Application_Sid,
                                       .ApplicationTypeID = application.Application_Type_Sid,
                                       .RNFlag = application.RN_flg,
                                       .RoleCategoryLevelID = application.Role_Category_Level_Sid,
                                       .UniqueCode = application.RN_DD_Unique_Code,
                            .ApplicationStatusTypeID = application.Application_Status_Type_Sid
                                   }
                                   ).FirstOrDefault()
                            '.AttestantID = IIf(IsNothing(application.Attestant_Sid), 0, application.Attestant_Sid),
                            Dim AttestID As Integer = IIf(IsNothing((From appl In context.Applications Where appl.Application_Sid = appId Select appl.Attestant_Sid).FirstOrDefault), 0, (From appl In context.Applications Where appl.Application_Sid = appId Select appl.Attestant_Sid).FirstOrDefault)
                            Dim AttestByAdmin As Boolean = (From appli In context.Applications Where appli.Application_Sid = appId Select appli.Is_Admin_Flg).FirstOrDefault
                            Dim appliedRCL As Integer = appDetails.RoleCategoryLevelID
                            Dim ddorrnSid As Integer = GetRnOrDDSid(ddorrncode, rnflag)
                            If appDetails.ApplicationTypeID = 2 Then
                                If Not IsNothing((From rp In context.Role_RN_DD_Personnel_Xref Where rp.Active_Flg = True And rp.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid And rp.Role_End_Date >= stDate And rp.Role_Category_Level_Sid = appliedRCL Select rp).FirstOrDefault) Then
                                    retval.ReturnValue = -1
                                    retval.AddGeneralMessage("Applied certificate dates overlap with the previous certification, please correct certificate dates.")
                                    Return retval
                                End If
                            End If
                            Dim ListEmail As New List(Of Application_Email_Xref)
                            Dim listPhone As New List(Of Application_Phone_Xref)
                            Dim appAddressList As New List(Of Application_Address_Xref) 'addresscontroldetails
                            appAddressList = (From addRef In context.Application_Address_Xref Where addRef.Application_Sid = appId And addRef.Active_Flg = True Select addRef).ToList
                            For Each ea As Application_Address_Xref In appAddressList
                                Dim _existingAddRef As RN_DD_Person_Type_Address_Xref = (From addR In context.RN_DD_Person_Type_Address_Xref Where addR.RN_DD_Person_Type_Xref.Person_Type_Sid = ptypeSid And addR.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid = ddorrnSid And addR.Address_Type_Sid = ea.Address_Type_Sid _
                                                   And addR.Active_Flg = True Select addR).FirstOrDefault()
                                If (_existingAddRef IsNot Nothing) Then
                                    _existingAddRef.Active_Flg = True
                                    _existingAddRef.Last_Update_By = Me.UserID
                                    _existingAddRef.Last_Update_Date = Date.Today
                                    _existingAddRef.Address_Sid = ea.Address_Sid
                                Else
                                    Dim _addressRef As RN_DD_Person_Type_Address_Xref = New RN_DD_Person_Type_Address_Xref()
                                    _addressRef.Address_Sid = ea.Address_Sid
                                    _addressRef.Address_Type_Sid = ea.Address_Type_Sid
                                    _addressRef.Active_Flg = True
                                    _addressRef.RN_DD_Person_Type_Xref = rntx
                                    _addressRef.Create_By = Me.UserID
                                    _addressRef.Create_Date = Date.Today 'DateTime.Now
                                    _addressRef.Last_Update_By = Me.UserID
                                    _addressRef.Last_Update_Date = Date.Today ' DateTime.Now
                                    _addressRef.Start_Date = Date.Today 'DateTime.Now
                                    _addressRef.End_Date = CDate("12/31/9999") 'DateTime.Now 'Convert.ToDateTime("12/12/2033")
                                    context.RN_DD_Person_Type_Address_Xref.Add(_addressRef)
                                End If
                                context.SaveChanges()
                            Next
                            ListEmail = (From EmailRef In context.Application_Email_Xref
                                         Where EmailRef.Application_Sid = appId And EmailRef.Active_Flg = True Select EmailRef).ToList() 'new objects.emailaddressdetails
                            listPhone = (From PhoneRef In context.Application_Phone_Xref Where PhoneRef.Application_Sid = appId And PhoneRef.Active_Flg = True Select PhoneRef).ToList()
                            For Each em As Application_Email_Xref In ListEmail
                                'Dim _existingEmail As Email = (From e In context.Emails Where e.Email_Address = em.EmailAddress Select e).FirstOrDefault()
                                Dim _existingRefEmail As RN_DD_Person_Type_Email_Xref = (From emailRef In context.RN_DD_Person_Type_Email_Xref Where emailRef.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid = ddorrnSid And emailRef.RN_DD_Person_Type_Xref.Person_Type_Sid = ptypeSid And emailRef.Contact_Type_Sid = em.Contact_Type_Sid And emailRef.Active_Flg = True
                            Select emailRef).FirstOrDefault()
                                If (_existingRefEmail IsNot Nothing) Then
                                    _existingRefEmail.Active_Flg = True
                                    _existingRefEmail.Last_Update_By = Me.UserID
                                    _existingRefEmail.Last_Update_Date = DateTime.Now
                                    _existingRefEmail.Email_Sid = em.Email_Sid
                                Else
                                    Dim _emailRef As RN_DD_Person_Type_Email_Xref = New RN_DD_Person_Type_Email_Xref()
                                    _emailRef.RN_DD_Person_Type_Xref = rntx
                                    _emailRef.Email_Sid = em.Email_Sid
                                    _emailRef.Contact_Type_Sid = em.Contact_Type_Sid
                                    _emailRef.Active_Flg = True
                                    _emailRef.Create_Date = DateTime.Now
                                    _emailRef.Create_By = Me.UserID
                                    _emailRef.Last_Update_By = Me.UserID
                                    _emailRef.Last_Update_Date = DateTime.Now
                                    _emailRef.Start_Date = Date.Today
                                    _emailRef.End_Date = CDate("12/31/9999")
                                    context.RN_DD_Person_Type_Email_Xref.Add(_emailRef)
                                End If
                                context.SaveChanges()
                            Next
                            For Each ph As Application_Phone_Xref In listPhone
                                'Dim _existingPhone As Phone = (From phone In context.Phones Where phone.Active_Flg = True And phone.Phone1 = ph.PhoneNumber Select phone).FirstOrDefault() 'why is it called phone1?
                                Dim _existingRefPhone As RN_DD_Person_Type_Phone_Xref = (From phoneRef In context.RN_DD_Person_Type_Phone_Xref Where phoneRef.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid = ddorrnSid And phoneRef.Contact_Type_Sid = ph.Contact_Type_Sid _
                                                And phoneRef.Active_Flg = True Select phoneRef).FirstOrDefault()
                                If (_existingRefPhone IsNot Nothing) Then
                                    _existingRefPhone.Active_Flg = True
                                    _existingRefPhone.Last_Update_By = Me.UserID
                                    _existingRefPhone.Last_Update_Date = DateTime.Now
                                    _existingRefPhone.Phone_Sid = ph.Phone_Sid
                                Else
                                    Dim _phoneRef As RN_DD_Person_Type_Phone_Xref = New RN_DD_Person_Type_Phone_Xref()
                                    _phoneRef.RN_DD_Person_Type_Xref = rntx
                                    _phoneRef.Phone_Sid = ph.Phone_Sid
                                    _phoneRef.Contact_Type_Sid = ph.Contact_Type_Sid
                                    _phoneRef.Active_Flg = True
                                    _phoneRef.Create_Date = DateTime.Now
                                    _phoneRef.Last_Update_By = Me.UserID
                                    _phoneRef.Create_By = Me.UserID
                                    _phoneRef.Last_Update_Date = DateTime.Now
                                    _phoneRef.Start_Date = Date.Today
                                    _phoneRef.End_Date = CDate("12/31/9999")
                                    context.RN_DD_Person_Type_Phone_Xref.Add(_phoneRef)
                                End If
                                context.SaveChanges()
                            Next
                            If notFinal = False Then
                                Dim recentlyAddedWorkExpList As New List(Of Objects.WorkExperienceDetails)
                                recentlyAddedWorkExpList = (From we In context.RN_Application_Work_Experience
                                                         Where we.Application_Sid = appId
                                                         Select New Objects.WorkExperienceDetails() With {
                                                             .WorkID = we.RN_Application_Work_Experience_Sid,
                                                             .ChkDDFlg = we.DD_Experience_flg,
                                                             .ChkRNFlg = we.RN_Experience_flg,
                                                             .EmpName = we.Agency_Name,
                                                             .JobDuties = we.Role_Description,
                                                             .EmpStartDate = we.Start_Date,
                                                             .EmpEndDate = we.End_Date,
                                                             .Title = we.Title
                                                             }).ToList()
                                For Each rwe As Objects.WorkExperienceDetails In recentlyAddedWorkExpList
                                    If IsNothing((From pwe In context.RN_Work_Experience Where pwe.Agency_Name = rwe.EmpName And pwe.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid And pwe.WE_Start_Date = rwe.EmpStartDate And pwe.WE_End_Date = rwe.EmpEndDate Select pwe).FirstOrDefault) Then
                                        Dim we As RN_Work_Experience = New RN_Work_Experience()
                                        'Insert work experience
                                        we.RN_DD_Person_Type_Xref = rntx
                                        we.Agency_Name = rwe.EmpName
                                        we.RN_Experience_Flg = rwe.ChkRNFlg
                                        we.DD_Experience_Flg = rwe.ChkDDFlg
                                        we.WE_Start_Date = rwe.EmpStartDate
                                        we.WE_End_Date = rwe.EmpEndDate
                                        we.Title = rwe.Title
                                        we.Role_Description = rwe.JobDuties & " "
                                        we.Active_Flg = True
                                        we.Create_By = Me.UserID
                                        we.Create_Date = DateTime.Now
                                        we.Last_Update_By = Me.UserID
                                        we.Last_Update_Date = DateTime.Now
                                        context.RN_Work_Experience.Add(we)

                                        Dim aaddr As RN_Application_Work_Experience_Address_Xref = (From addRef In context.RN_Application_Work_Experience_Address_Xref
                                        Where addRef.RN_Application_Work_Experience_Sid = rwe.WorkID Select addRef).FirstOrDefault
                                        If Not aaddr Is Nothing Then
                                            Dim _addressRef As RN_Work_Experience_Address_Xref = New RN_Work_Experience_Address_Xref()
                                            _addressRef.Address_Type_Sid = 5
                                            _addressRef.Address_Sid = aaddr.Address_Sid
                                            _addressRef.Active_Flg = True
                                            _addressRef.RN_Work_Experience = we
                                            _addressRef.Create_By = Me.UserID
                                            _addressRef.Create_Date = Date.Today 'DateTime.Now
                                            _addressRef.Last_Update_By = Me.UserID
                                            _addressRef.Last_Update_Date = Date.Today ' DateTime.Now
                                            _addressRef.Start_Date = Date.Today 'DateTime.Now
                                            _addressRef.End_Date = CDate("12/31/9999") 'DateTime.Now 'Convert.ToDateTime("12/12/2033")
                                            context.RN_Work_Experience_Address_Xref.Add(_addressRef)
                                        End If

                                        Dim aphone As RN_Application_Work_Experience_Phone_Xref = (From ph In context.RN_Application_Work_Experience_Phone_Xref
                                        Where ph.RN_Application_Work_Experience_Sid = rwe.WorkID And ph.Active_Flg = True Select ph).FirstOrDefault()
                                        If Not aphone Is Nothing Then

                                            Dim _phRef As RN_Work_Experience_Phone_Xref = New RN_Work_Experience_Phone_Xref()
                                            _phRef.Contact_Type_Sid = 6
                                            _phRef.Phone_Sid = aphone.Phone_Sid
                                            _phRef.Active_Flg = True
                                            _phRef.RN_Work_Experience = we
                                            _phRef.Create_By = Me.UserID
                                            _phRef.Create_Date = Date.Today 'DateTime.Now
                                            _phRef.Last_Update_By = Me.UserID
                                            _phRef.Last_Update_Date = Date.Today ' DateTime.Now
                                            _phRef.Start_Date = Date.Today 'DateTime.Now
                                            _phRef.End_Date = CDate("12/31/9999") 'DateTime.Now 'Convert.ToDateTime("12/12/2033")
                                            context.RN_Work_Experience_Phone_Xref.Add(_phRef)

                                        End If
                                        Dim aemail As RN_Application_Work_Experience_Email_Xref = (From em In context.RN_Application_Work_Experience_Email_Xref
                                        Where em.RN_Application_Work_Experience_Sid = rwe.WorkID And em.Active_Flg = True Select em).FirstOrDefault()
                                        If Not aemail Is Nothing Then

                                            Dim _emRef As RN_Work_Experience_Email_Xref = New RN_Work_Experience_Email_Xref()
                                            _emRef.Contact_Type_Sid = 6
                                            _emRef.Active_Flg = True
                                            _emRef.Email_Sid = aemail.Email_Sid
                                            _emRef.RN_Work_Experience = we
                                            _emRef.Create_By = Me.UserID
                                            _emRef.Create_Date = Date.Today 'DateTime.Now
                                            _emRef.Last_Update_By = Me.UserID
                                            _emRef.Last_Update_Date = Date.Today ' DateTime.Now
                                            _emRef.Start_Date = Date.Today 'DateTime.Now
                                            _emRef.End_Date = CDate("12/31/9999") 'DateTime.Now 'Convert.ToDateTime("12/12/2033")
                                            context.RN_Work_Experience_Email_Xref.Add(_emRef)
                                        End If
                                        context.SaveChanges()
                                    End If
                                Next

                                Dim recentlyAddedEmployers As New List(Of Application_Employer)
                                recentlyAddedEmployers = (From emp In context.Application_Employer _
                                                         Where emp.Application_Sid = appId AndAlso emp.Pending_Information_Flg = False
                                                         Select emp).ToList()
                                For Each remp As Application_Employer In recentlyAddedEmployers

                                    Dim aemp As Employer = New Employer()
                                    Dim epxref As Employer_RN_DD_Person_Type_Xref = Nothing
                                    Dim exemp As Employer = (From ee In context.Employers Where ee.Employer_Name = remp.Employer_Name And ee.Identification_Type_Sid = remp.Identification_Type_Sid And ee.Identification_Value = remp.Identification_Value Select ee).FirstOrDefault
                                    If Not IsNothing(exemp) Then
                                        epxref = (From pemp In context.Employer_RN_DD_Person_Type_Xref
                                        Where pemp.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid And pemp.Employer_Type_Sid = remp.Employer_Type_Sid And pemp.Employer_Sid = exemp.Employer_Sid
                                        Select pemp).FirstOrDefault
                                    End If
                                    If (exemp Is Nothing) OrElse IsNothing(epxref) Then
                                        If exemp Is Nothing Then

                                            aemp.Employer_Name = remp.Employer_Name
                                            aemp.Identification_Type_Sid = remp.Identification_Type_Sid
                                            aemp.Identification_Value = remp.Identification_Value
                                            aemp.Active_Flg = True
                                            aemp.Create_By = Me.UserID
                                            aemp.Create_Date = DateTime.Now
                                            aemp.Last_Update_By = Me.UserID
                                            aemp.Last_Update_Date = DateTime.Now
                                            context.Employers.Add(aemp)
                                        End If
                                        Dim aempxref As Employer_RN_DD_Person_Type_Xref = New Employer_RN_DD_Person_Type_Xref()
                                        aempxref.CEO_First_Name = remp.CEO_First_Name
                                        aempxref.CEO_Last_Name = remp.CEO_Last_Name
                                        aempxref.CEO_Middle_Name = remp.CEO_Middle_Name
                                        If exemp Is Nothing Then
                                            aempxref.Employer = aemp
                                        Else
                                            aempxref.Employer = exemp
                                        End If
                                        aempxref.Supervisor_Start_date = remp.Supervisor_Start_date
                                        aempxref.Supervisor_End_date = remp.Supervisor_End_date
                                        aempxref.Employer_Type_Sid = remp.Employer_Type_Sid
                                        aempxref.Employment_Start_Date = remp.Employment_Start_Date
                                        aempxref.Employment_End_Date = remp.Employment_End_Date
                                        aempxref.Personal_Mailing_Address_Different_Flg = remp.Personal_Mailing_Address_Different_Flg
                                        aempxref.Provider_Contract_Number = remp.Provider_Contract_Number
                                        aempxref.RN_DD_Person_Type_Xref = rntx
                                        aempxref.Work_Address_Same_As_Agency_Flg = remp.Work_Address_Same_As_Agency_Flg
                                        aempxref.Supervisor_First_Name = remp.Supervisor_First_Name
                                        aempxref.Supervisor_Last_Name = remp.Supervisor_Last_Name
                                        aempxref.Supervisor_Middle_Name = remp.Supervisor_Middle_Name

                                        aempxref.Active_Flg = True ' IIf(remp.Employment_End_Date > Date.Today, True, False)
                                        aempxref.Create_By = Me.UserID
                                        aempxref.Create_Date = DateTime.Now
                                        aempxref.Last_Update_By = Me.UserID
                                        aempxref.Last_Update_Date = DateTime.Now
                                        context.Employer_RN_DD_Person_Type_Xref.Add(aempxref)
                                        'add all those contacts!
                                        Dim _EmpAddressList As List(Of Application_Employer_Address_Xref) = (From addrReference In context.Application_Employer_Address_Xref
                                                                                                             Where addrReference.Application_Employer_Sid = remp.Application_Employer_Sid And addrReference.Active_Flg = True Select addrReference).ToList()
                                        For Each _empadd As Application_Employer_Address_Xref In _EmpAddressList
                                            Dim aEmpAddXref As Employer_RN_DD_Person_Type_Address_Xref = New Employer_RN_DD_Person_Type_Address_Xref()
                                            aEmpAddXref.Address_Type_Sid = _empadd.Address_Type_Sid
                                            aEmpAddXref.Active_Flg = True
                                            aEmpAddXref.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid
                                            aEmpAddXref.Employer = aempxref.Employer
                                            aEmpAddXref.Create_By = Me.UserID
                                            aEmpAddXref.Create_Date = Date.Today
                                            aEmpAddXref.Last_Update_by = Me.UserID
                                            aEmpAddXref.Last_Update_Date = Date.Today
                                            aEmpAddXref.Agency_Work_Location_Start_Date = _empadd.Agency_Work_Location_Start_date
                                            aEmpAddXref.Agency_Work_Location_End_Date = _empadd.Agency_Work_Location_End_date
                                            aEmpAddXref.Address_Sid = _empadd.Address_Sid
                                            context.Employer_RN_DD_Person_Type_Address_Xref.Add(aEmpAddXref)
                                            context.SaveChanges()
                                        Next
                                        'now move all types of phones and email(agency, work location, supervisor)
                                        Dim _EmpPhoneList As List(Of Application_Employer_Phone_Xref) = (From phoneRef In context.Application_Employer_Phone_Xref
                                                Where phoneRef.Active_Flg = True And phoneRef.Application_Employer_Sid = remp.Application_Employer_Sid Select phoneRef).ToList
                                        For Each _empPh As Application_Employer_Phone_Xref In _EmpPhoneList
                                            Dim aEmpPhXref As Employer_RN_DD_Person_Type_Phone_Xref = New Employer_RN_DD_Person_Type_Phone_Xref()
                                            aEmpPhXref.Contact_Type_Sid = _empPh.Contact_Type_Sid
                                            aEmpPhXref.Active_Flg = True
                                            aEmpPhXref.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid
                                            aEmpPhXref.Employer = aempxref.Employer
                                            aEmpPhXref.Create_By = Me.UserID
                                            aEmpPhXref.Create_Date = Date.Today
                                            aEmpPhXref.Last_Update_by = Me.UserID
                                            aEmpPhXref.Last_Update_Date = Date.Today
                                            aEmpPhXref.Phone_Sid = _empPh.Phone_Sid
                                            context.Employer_RN_DD_Person_Type_Phone_Xref.Add(aEmpPhXref)
                                            context.SaveChanges()
                                        Next

                                        Dim _EmpEmailList As List(Of Application_Employer_Email_Xref) = (From EmpEmailRef In context.Application_Employer_Email_Xref
                                                Where EmpEmailRef.Active_Flg = True And EmpEmailRef.Application_Employer_Sid = remp.Application_Employer_Sid Select EmpEmailRef).ToList
                                        For Each _empEm As Application_Employer_Email_Xref In _EmpEmailList
                                            Dim aEmpEmXref As Employer_RN_DD_Person_Type_Email_Xref = New Employer_RN_DD_Person_Type_Email_Xref()
                                            aEmpEmXref.Contact_Type_Sid = _empEm.Contact_Type_Sid
                                            aEmpEmXref.Active_Flg = True
                                            aEmpEmXref.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid
                                            aEmpEmXref.Employer = aempxref.Employer
                                            aEmpEmXref.Create_By = Me.UserID
                                            aEmpEmXref.Create_Date = Date.Today
                                            aEmpEmXref.Last_Update_by = Me.UserID
                                            aEmpEmXref.Last_Update_Date = Date.Today
                                            aEmpEmXref.Email_Sid = _empEm.Email_Sid
                                            context.Employer_RN_DD_Person_Type_Email_Xref.Add(aEmpEmXref)
                                            context.SaveChanges()
                                        Next
                                    ElseIf Not IsNothing(epxref) Then
                                        epxref.CEO_First_Name = remp.CEO_First_Name
                                        epxref.CEO_Last_Name = remp.CEO_Last_Name
                                        epxref.CEO_Middle_Name = remp.CEO_Middle_Name
                                        epxref.Supervisor_Start_date = remp.Supervisor_Start_date
                                        epxref.Supervisor_End_date = remp.Supervisor_End_date
                                        epxref.Employer_Type_Sid = remp.Employer_Type_Sid
                                        epxref.Employment_Start_Date = remp.Employment_Start_Date
                                        epxref.Employment_End_Date = remp.Employment_End_Date
                                        epxref.Personal_Mailing_Address_Different_Flg = remp.Personal_Mailing_Address_Different_Flg
                                        epxref.Provider_Contract_Number = remp.Provider_Contract_Number
                                        epxref.RN_DD_Person_Type_Xref = rntx
                                        epxref.Work_Address_Same_As_Agency_Flg = remp.Work_Address_Same_As_Agency_Flg
                                        epxref.Supervisor_First_Name = remp.Supervisor_First_Name
                                        epxref.Supervisor_Last_Name = remp.Supervisor_Last_Name
                                        epxref.Supervisor_Middle_Name = remp.Supervisor_Middle_Name
                                        epxref.Active_Flg = True ' IIf(remp.Employment_End_Date > Date.Today, True, False)

                                        epxref.Last_Update_By = Me.UserID
                                        epxref.Last_Update_Date = DateTime.Now

                                        Dim _EmpAddressList As List(Of Application_Employer_Address_Xref) = (From addrReference In context.Application_Employer_Address_Xref
                                                                                                             Where addrReference.Application_Employer_Sid = remp.Application_Employer_Sid And addrReference.Active_Flg = True Select addrReference).ToList()
                                        For Each _empadd As Application_Employer_Address_Xref In _EmpAddressList

                                            Dim aEmpAddXref As Employer_RN_DD_Person_Type_Address_Xref = (From eax In context.Employer_RN_DD_Person_Type_Address_Xref Where eax.Employer_Sid = epxref.Employer_Sid And eax.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid And eax.Address_Type_Sid = _empadd.Address_Type_Sid Select eax).FirstOrDefault
                                            'New Employer_RN_DD_Person_Type_Address_Xref()
                                            Dim isxref As Boolean = True
                                            If IsNothing(aEmpAddXref) Then
                                                isxref = False
                                                aEmpAddXref = New Employer_RN_DD_Person_Type_Address_Xref()
                                            End If
                                            aEmpAddXref.Address_Type_Sid = _empadd.Address_Type_Sid
                                            aEmpAddXref.Active_Flg = True
                                            aEmpAddXref.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid
                                            aEmpAddXref.Employer = epxref.Employer
                                            aEmpAddXref.Last_Update_by = Me.UserID
                                            aEmpAddXref.Last_Update_Date = Date.Today
                                            aEmpAddXref.Agency_Work_Location_Start_Date = _empadd.Agency_Work_Location_Start_date
                                            aEmpAddXref.Agency_Work_Location_End_Date = _empadd.Agency_Work_Location_End_date
                                            aEmpAddXref.Address_Sid = _empadd.Address_Sid
                                            If isxref = False Then
                                                aEmpAddXref.Create_By = Me.UserID
                                                aEmpAddXref.Create_Date = Date.Today
                                                context.Employer_RN_DD_Person_Type_Address_Xref.Add(aEmpAddXref)
                                            End If

                                            context.SaveChanges()
                                        Next
                                        'now move all types of phones and email(agency, work location, supervisor)
                                        Dim _EmpPhoneList As List(Of Application_Employer_Phone_Xref) = (From phoneRef In context.Application_Employer_Phone_Xref
                                                Where phoneRef.Active_Flg = True And phoneRef.Application_Employer_Sid = remp.Application_Employer_Sid Select phoneRef).ToList
                                        For Each _empPh As Application_Employer_Phone_Xref In _EmpPhoneList
                                            Dim aEmpPhXref As Employer_RN_DD_Person_Type_Phone_Xref = (From pex In context.Employer_RN_DD_Person_Type_Phone_Xref Where pex.Employer_Sid = epxref.Employer_Sid And pex.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid And pex.Contact_Type_Sid = _empPh.Contact_Type_Sid Select pex).FirstOrDefault
                                            '= New Employer_RN_DD_Person_Type_Phone_Xref()
                                            Dim isxref As Boolean = True
                                            If IsNothing(aEmpPhXref) Then
                                                isxref = False
                                                aEmpPhXref = New Employer_RN_DD_Person_Type_Phone_Xref()
                                            End If
                                            aEmpPhXref.Contact_Type_Sid = _empPh.Contact_Type_Sid
                                            aEmpPhXref.Active_Flg = True
                                            aEmpPhXref.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid
                                            aEmpPhXref.Employer = epxref.Employer
                                            aEmpPhXref.Create_By = Me.UserID
                                            aEmpPhXref.Create_Date = Date.Today
                                            aEmpPhXref.Last_Update_by = Me.UserID
                                            aEmpPhXref.Last_Update_Date = Date.Today
                                            aEmpPhXref.Phone_Sid = _empPh.Phone_Sid
                                            If isxref = False Then
                                                aEmpPhXref.Create_By = Me.UserID
                                                aEmpPhXref.Create_Date = Date.Today
                                                context.Employer_RN_DD_Person_Type_Phone_Xref.Add(aEmpPhXref)
                                            End If
                                            context.SaveChanges()
                                        Next

                                        Dim _EmpEmailList As List(Of Application_Employer_Email_Xref) = (From EmpEmailRef In context.Application_Employer_Email_Xref
                                                Where EmpEmailRef.Active_Flg = True And EmpEmailRef.Application_Employer_Sid = remp.Application_Employer_Sid Select EmpEmailRef).ToList
                                        For Each _empEm As Application_Employer_Email_Xref In _EmpEmailList
                                            Dim aEmpEmXref As Employer_RN_DD_Person_Type_Email_Xref = (From eex In context.Employer_RN_DD_Person_Type_Email_Xref Where eex.Employer_Sid = epxref.Employer_Sid And eex.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid And eex.Contact_Type_Sid = _empEm.Contact_Type_Sid Select eex).FirstOrDefault

                                            '= New Employer_RN_DD_Person_Type_Email_Xref()
                                            Dim isxref As Boolean = True
                                            If IsNothing(aEmpEmXref) Then
                                                isxref = False
                                                aEmpEmXref = New Employer_RN_DD_Person_Type_Email_Xref()
                                            End If
                                            aEmpEmXref.Contact_Type_Sid = _empEm.Contact_Type_Sid
                                            aEmpEmXref.Active_Flg = True
                                            aEmpEmXref.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid
                                            aEmpEmXref.Employer = epxref.Employer
                                            aEmpEmXref.Create_By = Me.UserID
                                            aEmpEmXref.Create_Date = Date.Today
                                            aEmpEmXref.Last_Update_by = Me.UserID
                                            aEmpEmXref.Last_Update_Date = Date.Today
                                            aEmpEmXref.Email_Sid = _empEm.Email_Sid
                                            If isxref = False Then
                                                aEmpEmXref.Create_By = Me.UserID
                                                aEmpEmXref.Create_Date = Date.Today
                                                context.Employer_RN_DD_Person_Type_Email_Xref.Add(aEmpEmXref)
                                            End If
                                            context.SaveChanges()
                                        Next
                                    End If

                                Next
                                context.SaveChanges()
                                Dim ex_role_rndd As Role_RN_DD_Personnel_Xref
                                ex_role_rndd = (From rnd In context.Role_RN_DD_Personnel_Xref Where rnd.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid And rnd.Role_Category_Level_Sid = appliedRCL Select rnd).FirstOrDefault
                                Dim newRole As Role_RN_DD_Personnel_Xref = New Role_RN_DD_Personnel_Xref()
                                'If finStatDesc = "Did Not Meet Requirements" Or finStatDesc = "Denied" Then
                                '    Dim CurCertHist2 As New Certification
                                '    CurCertHist2 = (From chist In context.Certifications Where chist.Application_Sid = appId Select chist).FirstOrDefault
                                '    'If finStatDesc <> "Did Not Meet Requirements" And finStatDesc <> "Removed From Registry" And finStatDesc <> "Denied" And finStatDesc <> "Intent to Deny" And app.Application_Type_Sid <> 4 Then
                                '    If IsNothing(CurCertHist2) Then
                                '        Dim newHist As Certification = New Certification()
                                '        newHist.Active_Flg = True
                                '        newHist.Application_Sid = appId
                                '        If AttestID > 0 Then
                                '            newHist.Attestant = AttestID
                                '        Else
                                '        End If
                                '        newHist.Certification_Start_Date = Date.Today
                                '        newHist.Certification_End_Date = Date.Today
                                '        newHist.Create_By = Me.UserID
                                '        newHist.Create_Date = DateTime.Now
                                '        newHist.Last_Update_By = Me.UserID
                                '        newHist.Last_Update_Date = DateTime.Now
                                '        Dim statHist As Certification_Status = New Certification_Status()
                                '        statHist.Certification_Status_Type_Sid = certStSid 'should be cert status
                                '        statHist.Status_Start_Date = Date.Today
                                '        statHist.Status_End_Date = CDate("12/31/9999")
                                '        statHist.Active_Flg = True
                                '        statHist.Create_By = Me.UserID
                                '        statHist.Create_Date = DateTime.Now
                                '        statHist.Last_Update_By = Me.UserID
                                '        statHist.Last_Update_Date = DateTime.Now

                                '        If ex_role_rndd Is Nothing Then
                                '            'add to cert history
                                '            newHist.Role_RN_DD_Personnel_Xref = newRole
                                '            context.Certifications.Add(newHist)
                                '        Else
                                '            'edit xref and make active
                                '            ' If finStatDesc <> "Did Not Meet Requirements" And finStatDesc <> "Removed From Registry" And finStatDesc <> "Denied" And finStatDesc <> "Intent to Deny" Then
                                '            'add to cert history
                                '            newHist.Role_RN_DD_Personnel_Xref = ex_role_rndd
                                '            context.Certifications.Add(newHist)
                                '            'End If
                                '        End If
                                '        statHist.Certification = newHist
                                '        context.Certification_Status.Add(statHist)
                                '    Else

                                '    End If
                                'End If
                                If appDetails.ApplicationTypeID <> 4 And finStatDesc <> "Removed From Registry" And finStatDesc <> "Intent to Deny" Then

                                    If ex_role_rndd Is Nothing Then
                                        'insert role_rndd xref
                                        newRole.RN_DD_Person_Type_Xref = rntx
                                        newRole.Role_Category_Level_Sid = appliedRCL
                                        If finStatDesc = "Did Not Meet Requirements" Or finStatDesc = "Denied" Then
                                            newRole.Active_Flg = False
                                            newRole.Role_End_Date = Date.Today
                                            newRole.Role_Start_Date = Date.Today
                                        Else
                                            newRole.Active_Flg = True
                                            newRole.Role_End_Date = endDate
                                            newRole.Role_Start_Date = stDate
                                        End If
                                        newRole.Create_By = Me.UserID
                                        newRole.Create_Date = DateTime.Now
                                        newRole.Last_Update_By = Me.UserID
                                        newRole.Last_Update_Date = DateTime.Now

                                        context.Role_RN_DD_Personnel_Xref.Add(newRole)
                                    Else
                                        Dim expCert As Certification = (From c In context.Certifications Where c.Role_RN_DD_Personnel_Xref_Sid = ex_role_rndd.Role_RN_DD_Personnel_Xref_Sid AndAlso c.Certification_End_Date < Date.Today Select c).FirstOrDefault
                                        If Not IsNothing(expCert) Then
                                            expCert.Active_Flg = False
                                            expCert.Last_Update_By = Me.UserID
                                            expCert.Last_Update_Date = DateTime.Now
                                            For Each ecs As Certification_Status In (From cs In context.Certification_Status Where cs.Certification_Sid = expCert.Certification_Sid Select cs).ToList
                                                ecs.Active_Flg = False
                                                ecs.Last_Update_By = Me.UserID
                                                ecs.Last_Update_Date = DateTime.Now
                                            Next
                                        End If
                                        'edit xref and make active
                                        'If finStatDesc <> "Did Not Meet Requirements" And finStatDesc <> "Removed From Registry" And finStatDesc <> "Denied" And finStatDesc <> "Intent to Deny" Then
                                        'ex_role_rndd.Role_Start_Date = stDate
                                        If finStatDesc = "Did Not Meet Requirements" Or finStatDesc = "Denied" Then
                                            ex_role_rndd.Role_End_Date = Date.Today
                                        Else
                                            ex_role_rndd.Role_End_Date = endDate
                                        End If

                                        ex_role_rndd.Active_Flg = True
                                        ex_role_rndd.Last_Update_By = Me.UserID
                                        ex_role_rndd.Last_Update_Date = DateTime.Now
                                        'End If
                                    End If
                                    Dim CurCertHist As Certification = New Certification()
                                    CurCertHist = (From chist In context.Certifications Where chist.Application_Sid = appId Select chist).FirstOrDefault
                                    'If finStatDesc <> "Did Not Meet Requirements" And finStatDesc <> "Removed From Registry" And finStatDesc <> "Denied" And finStatDesc <> "Intent to Deny" And app.Application_Type_Sid <> 4 Then
                                    If IsNothing(CurCertHist) Then
                                        Dim newHist As Certification = New Certification()
                                        newHist.Active_Flg = True
                                        newHist.Application_Sid = appId
                                        If AttestID > 0 Then
                                            newHist.Attestant = AttestID
                                        Else

                                        End If
                                        If finStatDesc = "Did Not Meet Requirements" Or finStatDesc = "Denied" Then
                                            newHist.Certification_Start_Date = Date.Today
                                            newHist.Certification_End_Date = Date.Today
                                        Else
                                            newHist.Certification_Start_Date = stDate
                                            newHist.Certification_End_Date = endDate
                                        End If

                                        newHist.Create_By = Me.UserID
                                        newHist.Create_Date = DateTime.Now
                                        newHist.Last_Update_By = Me.UserID
                                        newHist.Last_Update_Date = DateTime.Now
                                        newHist.Attested_By_Admin_Flg = AttestByAdmin
                                        Dim statHist As Certification_Status = New Certification_Status()
                                        statHist.Certification_Status_Type_Sid = certStSid 'should be cert status
                                        statHist.Status_Start_Date = stDate
                                        statHist.Status_End_Date = endDate
                                        statHist.Active_Flg = True
                                        statHist.Create_By = Me.UserID
                                        statHist.Create_Date = DateTime.Now
                                        statHist.Last_Update_By = Me.UserID
                                        statHist.Last_Update_Date = DateTime.Now

                                        If ex_role_rndd Is Nothing Then
                                            'add to cert history
                                            newHist.Role_RN_DD_Personnel_Xref = newRole
                                            context.Certifications.Add(newHist)
                                        Else
                                            'edit xref and make active
                                            ' If finStatDesc <> "Did Not Meet Requirements" And finStatDesc <> "Removed From Registry" And finStatDesc <> "Denied" And finStatDesc <> "Intent to Deny" Then
                                            'add to cert history
                                            newHist.Role_RN_DD_Personnel_Xref = ex_role_rndd
                                            context.Certifications.Add(newHist)
                                            'End If
                                        End If
                                        statHist.Certification = newHist
                                        context.Certification_Status.Add(statHist)
                                    Else

                                        If IsNothing(From csta In context.Certification_Status Where csta.Certification_Sid = CurCertHist.Certification_Sid And csta.Certification_Status_Type_Sid = certStSid) Then
                                            Dim sh1 As Certification_Status = (From cs1 In context.Certification_Status Where cs1.Certification_Sid = CurCertHist.Certification_Sid).FirstOrDefault
                                            If Not IsNothing(sh1) Then
                                                sh1.Active_Flg = False
                                                sh1.Last_Update_By = Me.UserID
                                                sh1.Last_Update_Date = DateTime.Now
                                            End If
                                            Dim statHist1 As Certification_Status = New Certification_Status()
                                            statHist1.Certification_Status_Type_Sid = certStSid 'should be cert status
                                            statHist1.Status_Start_Date = stDate
                                            statHist1.Status_End_Date = endDate
                                            statHist1.Certification_Sid = CurCertHist.Certification_Sid
                                            statHist1.Active_Flg = True
                                            statHist1.Create_By = Me.UserID
                                            statHist1.Create_Date = DateTime.Now
                                            statHist1.Last_Update_By = Me.UserID
                                            statHist1.Last_Update_Date = DateTime.Now
                                            context.Certification_Status.Add(statHist1)
                                        End If

                                    End If
                                End If
                                'update ceus renewal table if any, ++renewal count
                                If appDetails.ApplicationTypeID <> 4 And finStatDesc <> "Removed From Registry" And finStatDesc <> "Intent to Deny" And finStatDesc <> "Did Not Meet Requirements" And finStatDesc <> "Denied" Then
                                    Dim renewalHC As Renewal_History_Count = (From rhc In context.Renewal_History_Count Where rhc.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid And rhc.Role_Category_Level_sid = appliedRCL Select rhc).FirstOrDefault
                                    If IsNothing(renewalHC) Then
                                        Dim NewRenewalHC As Renewal_History_Count = New Renewal_History_Count()
                                        NewRenewalHC.Renewal_Count = 0
                                        NewRenewalHC.Application_type_SId = appDetails.ApplicationTypeID
                                        NewRenewalHC.RN_DD_Person_Type_Xref = rntx
                                        NewRenewalHC.Role_Category_Level_sid = appliedRCL
                                        NewRenewalHC.Create_By = Me.UserID
                                        NewRenewalHC.Create_Date = DateTime.Now
                                        NewRenewalHC.Last_Update_By = Me.UserID
                                        NewRenewalHC.Last_Update_Date = DateTime.Now
                                        context.Renewal_History_Count.Add(NewRenewalHC)
                                    Else
                                        renewalHC.Renewal_Count = renewalHC.Renewal_Count + 1
                                    End If
                                End If
                                '*****
                                context.SaveChanges()

                                'write-update history tables
                                Dim AppHist As Application_History = (From ah In context.Application_History Where ah.Application_Sid = appId Select ah).FirstOrDefault
                                If Not IsNothing(AppHist) Then
                                    AppHist.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid
                                    AppHist.Last_Update_By = Me.UserID
                                    AppHist.Last_Update_Date = Date.Today
                                    If appDetails.ApplicationTypeID <> 4 Then
                                        If IsNothing((From ahs In context.Application_History_Status Where ahs.Application_History_Sid = AppHist.Application_History_Sid And ahs.Application_Status_Type_Sid = statusSid Select ahs).FirstOrDefault) Then
                                            Dim newahs As Application_History_Status = New Application_History_Status()
                                            newahs.Application_History_Sid = AppHist.Application_History_Sid
                                            newahs.Application_Status_Type_Sid = statusSid
                                            newahs.Create_By = Me.UserID
                                            newahs.Create_Date = DateTime.Now
                                            newahs.Last_Update_By = Me.UserID
                                            newahs.Last_Update_Date = DateTime.Now
                                            context.Application_History_Status.Add(newahs)
                                        End If
                                    End If
                                End If
                                context.SaveChanges()

                                If appDetails.ApplicationTypeID <> 4 And finStatDesc <> "Removed From Registry" And finStatDesc <> "Intent to Deny" Then
                                    Dim CEUCat As Integer = 0
                                    If appDetails.ApplicationTypeID <> 4 Then
                                        CEUCat = (From rclx In context.Role_Category_Level_Xref Where rclx.Role_Category_Level_Sid = appliedRCL Select rclx.Category_Type_Sid).FirstOrDefault
                                        Dim renCEUs As CEUs_Renewal = (From ceu In context.CEUs_Renewal Where ceu.Category_Type_Sid = CEUCat And ceu.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid And (ceu.Application_Sid.HasValue = False OrElse ceu.Application_Sid = 0) Select ceu).FirstOrDefault
                                        If Not IsNothing(renCEUs) Then
                                            renCEUs.Application_Sid = appId
                                            renCEUs.Last_Update_By = Me.UserID
                                            renCEUs.Last_Update_Date = Date.Now
                                        End If
                                        context.SaveChanges()
                                    End If
                                    If rnflag = False Then
                                        'If appDetails.ApplicationTypeID = 1 Then
                                        For Each asv As Application_Skill_Verification In (From cav In context.Application_Skill_Verification Where cav.Application_Sid = appId Select cav).ToList
                                            If IsNothing((From ssv In context.Skill_Verification Where ssv.Application_Sid = appId AndAlso ssv.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid Select ssv).FirstOrDefault) Then

                                                Dim psv As Skill_Verification = New Skill_Verification()
                                                psv.Active_Flg = True
                                                psv.Application_Sid = appId
                                                psv.Category_Type_Sid = asv.Category_Type_Sid
                                                psv.Create_By = Me.UserID
                                                psv.Create_Date = asv.Create_Date
                                                psv.Last_Update_By = Me.UserID
                                                psv.Last_Update_Date = Date.Today
                                                psv.Permanent_Flg = True
                                                psv.RN_DD_Person_Type_Xref = rntx
                                                psv.Start_Date = Date.Today
                                                psv.End_Date = endDate
                                                context.Skill_Verification.Add(psv)
                                                For Each astype As Application_Skill_Type_Xref In (From atp In context.Application_Skill_Type_Xref Where atp.Application_Skill_Verification_Sid = asv.Application_Skill_Verification_Sid Select atp).ToList
                                                    Dim ptp As Skill_Verification_Skill_Type_Xref = New Skill_Verification_Skill_Type_Xref()
                                                    ptp.Active_Flg = True
                                                    ptp.Skill_Type_Sid = astype.Skill_Type_Sid
                                                    ptp.Skill_Verification = psv
                                                    ptp.Create_By = Me.UserID
                                                    ptp.Create_Date = Date.Today
                                                    ptp.Last_Update_By = Me.UserID
                                                    ptp.Last_Update_Date = Date.Today
                                                    context.Skill_Verification_Skill_Type_Xref.Add(ptp)
                                                    For Each achklist As Application_Skill_Type_CheckList_Xref In (From achk In context.Application_Skill_Type_CheckList_Xref Where achk.Application_Skill_Type_Xref_Sid = astype.Application_Skill_Type_Xref_Sid Select achk).ToList
                                                        Dim pchklist As Skill_Verification_Type_CheckList_Xref = New Skill_Verification_Type_CheckList_Xref()
                                                        pchklist.Active_Flg = True
                                                        pchklist.Skill_Verification_Skill_Type_Xref = ptp
                                                        pchklist.Skill_CheckList_Sid = achklist.Skill_CheckList_Sid
                                                        pchklist.Verification_Date = achklist.Verification_Date
                                                        pchklist.Verified_Person_Name = achklist.Verified_Person_Name
                                                        pchklist.Verified_Person_Title = achklist.Verified_Person_Title
                                                        pchklist.Create_By = Me.UserID
                                                        pchklist.Create_Date = Date.Today
                                                        pchklist.Last_Update_By = Me.UserID
                                                        pchklist.Last_Update_Date = Date.Today
                                                        context.Skill_Verification_Type_CheckList_Xref.Add(pchklist)

                                                    Next
                                                Next
                                            End If
                                        Next
                                        'Else
                                        For Each p As Skill_Verification In (From sv In context.Skill_Verification Where (sv.Application_Sid = 0 OrElse sv.Application_Sid.HasValue = False) AndAlso (sv.End_Date <= Now AndAlso sv.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid) Select sv).ToList
                                            p.Application_Sid = appId
                                        Next

                                        'End If
                                    End If
                                    Dim curroleperson As Integer = (From rnd In context.Role_RN_DD_Personnel_Xref Where rnd.RN_DD_Person_Type_Xref_Sid = rntx.RN_DD_Person_Type_Xref_Sid And rnd.Role_Category_Level_Sid = appliedRCL Select rnd.Role_RN_DD_Personnel_Xref_Sid).FirstOrDefault
                                    For Each cr As Application_Course_Xref In (From course In context.Application_Course_Xref Where course.Application_Sid = appId Select course).ToList
                                        If IsNothing((From pc In context.Person_Course_Xref Where pc.Course_Sid = cr.Course_Sid And pc.Role_RN_DD_Personnel_Xref_Sid = curroleperson Select pc).FirstOrDefault) Then

                                            Dim personCourseXref As Person_Course_Xref = New Person_Course_Xref()
                                            personCourseXref.Course_Sid = cr.Course_Sid
                                            If ex_role_rndd Is Nothing Then
                                                personCourseXref.Role_RN_DD_Personnel_Xref = newRole
                                            Else
                                                personCourseXref.Role_RN_DD_Personnel_Xref = ex_role_rndd
                                            End If
                                            'personCourseXref.MAIS_Role_RN_DD_Personnel_Xref = rntx
                                            personCourseXref.Active_Flg = True
                                            personCourseXref.Create_By = Me.UserID
                                            personCourseXref.Create_Date = Date.Today
                                            personCourseXref.Last_Update_By = Me.UserID
                                            personCourseXref.Last_Update_Date = Date.Today
                                            context.Person_Course_Xref.Add(personCourseXref)
                                            For Each sess As Application_Course_Session_Xref In (From session In context.Application_Course_Session_Xref Where session.Application_Course_Xref_Sid = cr.Application_Course_Xref_Sid Select session).ToList
                                                Dim personSessionXref As Person_Course_Session_Xref = New Person_Course_Session_Xref()
                                                personSessionXref.Person_Course_Xref = personCourseXref
                                                personSessionXref.Session_Sid = sess.Session_Sid
                                                personSessionXref.Active_Flg = True
                                                personSessionXref.Create_By = Me.UserID
                                                personSessionXref.Create_Date = Date.Today
                                                personSessionXref.Last_Update_By = Me.UserID
                                                personSessionXref.Last_Update_Date = Date.Today
                                                context.Person_Course_Session_Xref.Add(personSessionXref)
                                            Next
                                        End If
                                    Next
                                    'If appliedRCL <> currentRCL Then
                                    '    'check if record exists somehow-update otherwise insert new
                                    'Else
                                End If
                            End If
                            context.SaveChanges()
                            retval.ReturnValue = 0
                        Else
                            context.SaveChanges()
                            retval.ReturnValue = 0
                        End If
                        'Catch ex As DbEntityValidationException
                        '    Dim strResult As String = ex.Message & "\n"
                        '    For Each eve As Object In ex.EntityValidationErrors
                        '        'Dim s As String = eve.Entry.Enity.GetType().Name
                        '        'Dim s1 As String = eve.Entry.ToString
                        '        'strResult = strResult & s & s1
                        '        For Each ve As Object In eve.ValidationErrors
                        '            Dim s2 As String = ve.PropertyName
                        '            Dim s3 As String = ve.ErrorMessage
                        '            strResult = strResult & s2 & s3
                        '        Next
                        '    Next
                        '    'Debug.Print(strResult)
                        '    Me._messages.Add(New ReturnMessage("Error while saving queries in permanent DB.", True, False))
                        '    Me.LogError("Error while saving queries in permanent DB.", ex)
                        '    retval.AddErrorMessage(ex.Message)


                        'Catch ex As Exception
                        '    Me._messages.Add(New ReturnMessage("Error while saving information in permanent DB.", True, False))
                        '    Me.LogError("Error while saving information in permanent DB.", ex)
                        '    retval.AddErrorMessage(ex.Message)
                        '    retval.ReturnValue = -1
                        '    Throw
                        'End Try
                    End Using
                    scope.Complete()
                End Using
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while saving information in permanent DB. Rolled back", True, False))
                Me.LogError("Error while saving information in permanent DB.Rolled back", CInt(Me.UserID), ex)
                Me.LogError("Error while saving information in permanent DB.Rolled back", CInt(Me.UserID), ex.InnerException)
                retval.AddErrorMessage(ex.Message)
                retval.ReturnValue = -1
                Throw
            End Try

            Return retval
        End Function
    End Class
End Namespace
