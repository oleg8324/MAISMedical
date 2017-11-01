Imports System.Data.Linq
Imports ODMRDDHelperClassLibrary.Utility
Imports System.Configuration
Imports System.Data.Objects
Imports MAIS.Data
Namespace Queries
    Public Interface INotationQueries
        Inherits IQueriesBase
        Function GetRole(ByVal rolexrefID As Integer) As String
        Function CheckCertSDateCorrect(ByVal RoleDDRNXrefSid As Integer, ByVal RN_flg As Boolean, ByVal newSDate As Date, Optional oldDate As Date = #1/1/1990#) As String
        Function GetNotationsByApp(ByVal appid As Integer, ByVal RN_flg As Boolean) As List(Of Data.Objects.NotationObject)
        Function SetAppStatus(ByVal appid As Integer, ByVal appstat As String) As Long
        Function GetNotations(ByVal code As String, ByVal RN_flg As Boolean) As List(Of Data.Objects.NotationObject)
        Function GetNotationByNotID(ByVal nid As Integer) As Data.Objects.NotationObject
        Function SaveNotation(ByVal n As Objects.NotationObject, ByVal code As String, ByVal RN_flg As Boolean) As ReturnObject(Of Long)
        Function UpdateNotation(ByVal n As Objects.NotationObject, ByVal nid As Integer) As ReturnObject(Of Long)
        Function GetNotationTypes() As List(Of Objects.NType)
        Function GetNotationReasons() As List(Of Objects.NReason)
        Function GetCertStatuses() As List(Of Objects.CertStatus)
    End Interface
    Public Class NotationQueries
        Inherits QueriesBase
        Implements INotationQueries
        Public Function GetRole(ByVal rolexrefID As Integer) As String Implements INotationQueries.GetRole
            Dim s As String = String.Empty
            Try
                Dim context As New MAISContext()
                s = (From rxr In context.Role_Category_Level_Xref Join rl In context.MAIS_Role On rxr.Role_Sid Equals rl.Role_Sid
                   Where rxr.Role_Category_Level_Sid = rolexrefID
                   Select rl.Role_Desc).FirstOrDefault

            Catch ex As Exception
                Me.LogError("Error Getting Role.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting Role.", True, False))
            End Try
            Return s
        End Function
        'Public Function GetApplicantSid(ByVal code As String, ByVal RN_flg As Boolean) As Integer
        '    Using context As New MAISContext
        '        If RN_flg Then
        '            Return (From r In context.RNs
        '                           Where r.RNLicense_Number = code
        '                           Select r.RN_Sid).FirstOrDefault()
        '        Else
        '            Return (From dd In context.DDPersonnels
        '                          Where dd.DDPersonnel_Code = code
        '                          Select dd.DDPersonnel_Sid).FirstOrDefault()
        '        End If
        '    End Using
        'End Function
        Public Function CheckCertSDateCorrect(ByVal RoleDDRNXrefSid As Integer, ByVal RN_flg As Boolean, ByVal newSDate As Date, Optional oldDate As Date = #1/1/1990#) As String Implements INotationQueries.CheckCertSDateCorrect
            Dim retstr As String = String.Empty
            Try
                Using context As New MAISContext
                    Dim DDRNSid As Integer = (From rl In context.Role_RN_DD_Personnel_Xref Where rl.Role_RN_DD_Personnel_Xref_Sid = RoleDDRNXrefSid Select rl.RN_DD_Person_Type_Xref_Sid).FirstOrDefault
                    Dim DDPersonalInfo As Objects.PersonalInformationDetails = Nothing
                    DDPersonalInfo = (From dd In context.RNs _
                                           Join ddRef In context.RN_DD_Person_Type_Xref On dd.RN_Sid Equals ddRef.RN_DDPersonnel_Sid
                                        Where dd.RNLicense_Number = DDRNSid
                                    Select New Objects.PersonalInformationDetails() With {
                                        .FirstName = dd.First_Name,
                                        .LastName = dd.Last_Name,
                                        .Gender = dd.Gender,
                                        .DOBDateOfIssuance = dd.Date_Of_Original_Issuance,
                                        .RNLicenseOrSSN = dd.RNLicense_Number,
                                        .MiddleName = dd.Middle_Name,
                                               .ApplicationSID = dd.RN_Sid
                                               }).FirstOrDefault()
                    'If RN_flg Then
                    '    Return (From r In context.RNs
                    '                   Where r.RNLicense_Number = code
                    '                   Select r.RN_Sid).FirstOrDefault()
                    'Else
                    '    Return (From dd In context.DDPersonnels
                    '                  Where dd.DDPersonnel_Code = code
                    '                  Select dd.DDPersonnel_Sid).FirstOrDefault()
                    'End If
                End Using
            Catch ex As Exception
                Me.LogError("Error Getting cert date correction.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting cert date correction.", True, False))
            End Try

            Return retstr
        End Function
        Public Function GetNotationsByApp(ByVal appid As Integer, ByVal RN_flg As Boolean) As List(Of Data.Objects.NotationObject) Implements INotationQueries.GetNotationsByApp
            Dim NotationList As List(Of Data.Objects.NotationObject) = Nothing
            Dim NReasons As List(Of String) = Nothing
            Try
                Using context As New MAISContext
                    NotationList = (From nt In context.Notations
                                  Where nt.Application_Sid = appid
                                Select New Data.Objects.NotationObject() With {
                        .NotationType = New Objects.NType() With {
                        .NTypeSid = nt.Notation_Type_Sid, .NTypeDesc = nt.Notation_Type.Notation_Desc
                        },
                        .OccurenceDate = nt.Occurrence_Date,
                        .NotationDate = nt.Notation_Date,
                        .PersonEnteringNotation = nt.Person_entering_Notation,
                        .PersonTitle = nt.Person_Title,
                        .AppId = nt.Application_Sid,
                        .UnflaggedDate = nt.UnFlagged_Date,
                    .AppNotId = nt.Notation_Sid}).ToList

                    For Each k In NotationList
                        Dim i As Integer
                        i = k.AppNotId
                        k.AllReasons = ""
                        k.NotationReasons = (From an2 In context.Notations _
                            Join rx In context.Notation_Reason_RN_DD_Xref On an2.Notation_Sid Equals rx.Notation_Sid
                            Join notr In context.Reasons_For_Notation On notr.Reasons_For_Notation_Sid Equals rx.Reasons_For_Notation_Sid
                            Where an2.Notation_Sid = i
                            Select New Objects.NReason With {
                                .NReasonSid = notr.Reasons_For_Notation_Sid, .NReasonDesc = notr.Reason_Desc
                        }).ToList
                        For Each m In k.NotationReasons
                            k.AllReasons = m.NReasonDesc & ", " & k.AllReasons
                        Next
                        If k.AllReasons.Length > 2 Then
                            k.AllReasons = k.AllReasons.Substring(0, k.AllReasons.Length - 2)
                        End If

                    Next
                End Using
            Catch ex As Exception
                Me.LogError("Error Getting Notation Info by application.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting Notation Info by application.", True, False))
            End Try
            Return NotationList
        End Function
        Public Function SetAppStatus(ByVal appid As Integer, ByVal appstat As String) As Long Implements INotationQueries.SetAppStatus
            Dim rv As Long = -1
            Try
                Using context As New MAISContext
                    Dim appstatsid As Integer = (From at In context.Application_Status_Type Where at.Application_Status_Desc = appstat Select at.Application_Status_Type_Sid).FirstOrDefault

                    Dim app As Application = (From a In context.Applications Where a.Application_Sid = appid Select a).FirstOrDefault
                    If app IsNot Nothing Then
                        app.Application_Status_Type_Sid = appstatsid
                        app.Last_Update_By = Me.UserID
                        app.Last_Update_Date = Date.Today
                    End If
                    context.SaveChanges()
                    rv = 0
                End Using
            Catch ex As Exception
                Me.LogError("Error seting application status.", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error seting application status.", True, False))
            End Try

            Return rv
        End Function
        Public Function GetNotations(ByVal code As String, ByVal RN_flg As Boolean) As List(Of Data.Objects.NotationObject) Implements INotationQueries.GetNotations
            Dim NotationList As List(Of Data.Objects.NotationObject) = Nothing
            Dim NReasons As List(Of String) = Nothing
            Dim sum As ISummaryQueries = StructureMap.ObjectFactory.GetInstance(Of ISummaryQueries)()
            Dim personSid As Integer = sum.GetApplicantSid(code, RN_flg)
            Dim personTypeSid As Integer = IIf(RN_flg, 1, 2)
            Try
                Using context As New MAISContext
                    NotationList = (From nt In context.Notations
                                  Where nt.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid = personSid And nt.RN_DD_Person_Type_Xref.Person_Type_Sid = personTypeSid
                                Select New Data.Objects.NotationObject() With {
                        .NotationType = New Objects.NType() With {
                        .NTypeSid = nt.Notation_Type_Sid, .NTypeDesc = nt.Notation_Type.Notation_Desc
                        },
                        .OccurenceDate = nt.Occurrence_Date,
                        .NotationDate = nt.Notation_Date,
                        .PersonEnteringNotation = nt.Person_entering_Notation,
                        .PersonTitle = nt.Person_Title,
                        .AppId = If(nt.Application_Sid.HasValue, nt.Application_Sid, 0),
                        .UnflaggedDate = nt.UnFlagged_Date,
                    .AppNotId = nt.Notation_Sid}).ToList

                    For Each k As Data.Objects.NotationObject In NotationList
                        Dim i As Integer
                        i = k.AppNotId
                        k.AllReasons = ""
                        k.NotationReasons = (From an2 In context.Notations _
                            Join rx In context.Notation_Reason_RN_DD_Xref On an2.Notation_Sid Equals rx.Notation_Sid
                            Join notr In context.Reasons_For_Notation On notr.Reasons_For_Notation_Sid Equals rx.Reasons_For_Notation_Sid
                            Where an2.Notation_Sid = i
                            Select New Objects.NReason With {
                                .NReasonSid = notr.Reasons_For_Notation_Sid, .NReasonDesc = notr.Reason_Desc
                        }).ToList
                        For Each m As Objects.NReason In k.NotationReasons
                            k.AllReasons = m.NReasonDesc & ", " & k.AllReasons
                        Next
                        If k.AllReasons.Length > 2 Then
                            If k.AllReasons.Contains(", ") Then
                                k.AllReasons = k.AllReasons.Substring(0, k.AllReasons.Length - 2)
                            End If
                        End If
                    Next

                End Using
            Catch ex As Exception
                Me.LogError("Error Getting Notations ", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting Notations.", True, False))
            End Try
            Return NotationList
        End Function
        Public Function GetNotationByNotID(ByVal nid As Integer) As Data.Objects.NotationObject Implements INotationQueries.GetNotationByNotID
            Dim Notation As Data.Objects.NotationObject = Nothing
            Dim NReasons As List(Of String) = Nothing

            Try
                Using context As New MAISContext
                    Notation = (From nt In context.Notations
                                  Where nt.Notation_Sid = nid
                                Select New Data.Objects.NotationObject() With {
                        .NotationType = New Objects.NType() With {
                        .NTypeSid = nt.Notation_Type_Sid, .NTypeDesc = nt.Notation_Type.Notation_Desc
                        },
                        .OccurenceDate = nt.Occurrence_Date,
                        .NotationDate = nt.Notation_Date,
                        .PersonEnteringNotation = nt.Person_entering_Notation,
                        .PersonTitle = nt.Person_Title,
                        .UnflaggedDate = nt.UnFlagged_Date,
                        .AppId = nt.Application_Sid,
                    .AppNotId = nt.Notation_Sid}).FirstOrDefault
                    Notation.AllReasons = ""
                    Notation.NotationReasons = (From an2 In context.Notations _
                            Join rx In context.Notation_Reason_RN_DD_Xref On an2.Notation_Sid Equals rx.Notation_Sid
                            Join notr In context.Reasons_For_Notation On notr.Reasons_For_Notation_Sid Equals rx.Reasons_For_Notation_Sid
                            Where an2.Notation_Sid = nid
                            Select New Objects.NReason With {
                                .NReasonSid = notr.Reasons_For_Notation_Sid, .NReasonDesc = notr.Reason_Desc
                        }).ToList

                    For Each m In Notation.NotationReasons
                        Notation.AllReasons = m.NReasonDesc & ", " & Notation.AllReasons
                    Next
                    If Notation.AllReasons.Length > 2 Then
                        Notation.AllReasons = Notation.AllReasons.Substring(0, Notation.AllReasons.Length - 2)
                    End If

                End Using
            Catch ex As Exception
                Me.LogError("Error Getting Notation Info by ID", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting Notations by ID.", True, False))
            End Try
            Return Notation
        End Function
        Public Function SaveNotation(ByVal n As Objects.NotationObject, ByVal code As String, ByVal RN_flg As Boolean) As ReturnObject(Of Long) Implements INotationQueries.SaveNotation
            Dim retval As New ReturnObject(Of Long)(-1L)

            Try
                Using context As New MAISContext
                    Dim _n As New Notation
                    Dim _nxl As New List(Of Notation_Reason_RN_DD_Xref)
                    Dim sum As ISummaryQueries = StructureMap.ObjectFactory.GetInstance(Of ISummaryQueries)()
                    Dim personSid As Integer = sum.GetApplicantSid(code, RN_flg)
                    Dim personType As Integer = IIf(RN_flg = True, 1, 2)
                    _n.Notation_Date = n.NotationDate
                    _n.Application_Sid = n.AppId
                    _n.Occurrence_Date = n.OccurenceDate
                    _n.Person_entering_Notation = n.PersonEnteringNotation
                    _n.Person_Title = n.PersonTitle
                    _n.RN_DD_Person_Type_Xref_Sid = (From px In context.RN_DD_Person_Type_Xref Where px.RN_DDPersonnel_Sid = personSid And px.Person_Type_Sid = personType Select px.RN_DD_Person_Type_Xref_Sid).FirstOrDefault
                    _n.Create_By = Me.UserID
                    _n.Create_Date = DateTime.Now
                    _n.Last_Update_By = Me.UserID
                    _n.Last_Update_Date = DateTime.Now
                    _n.Notation_Type_Sid = n.NotationType.NTypeSid
                    If Not IsNothing(n.UnflaggedDate) Then
                        _n.UnFlagged_Date = n.UnflaggedDate
                    End If
                    context.Notations.Add(_n)
                    For Each l In n.NotationReasons
                        _nxl.Add(New Notation_Reason_RN_DD_Xref)
                        _nxl.Item(_nxl.Count - 1).Reasons_For_Notation_Sid = l.NReasonSid
                        _nxl.Item(_nxl.Count - 1).Create_By = Me.UserID
                        _nxl.Item(_nxl.Count - 1).Create_Date = DateTime.Now
                        _nxl.Item(_nxl.Count - 1).Last_Update_By = Me.UserID
                        _nxl.Item(_nxl.Count - 1).Last_Update_Date = DateTime.Now
                        _nxl.Item(_nxl.Count - 1).Notation = _n
                        context.Notation_Reason_RN_DD_Xref.Add(_nxl.Item(_nxl.Count - 1))
                    Next

                    context.SaveChanges()
                    retval.ReturnValue = _n.Notation_Sid
                End Using
            Catch ex As Exception
                'strResult = ex.Message
                Me._messages.Add(New ReturnMessage("Error while saving Notation.", True, False))
                Me.LogError("Error while saving Notation.", CInt(Me.UserID), ex)
                retval.AddErrorMessage(ex.Message)
                'Finally
            End Try

            Return retval
        End Function
        Public Function UpdateNotation(ByVal n As Objects.NotationObject, ByVal nid As Integer) As ReturnObject(Of Long) Implements INotationQueries.UpdateNotation 'When they unflag or add docs-TODO
            Dim retval As New ReturnObject(Of Long)(-1L)
            Try
                Using context As New MAISContext
                    Dim _n As Notation
                    Dim _nxl As New List(Of Notation_Reason_RN_DD_Xref)
                    _n = (From an In context.Notations Where an.Notation_Sid = nid Select an).FirstOrDefault
                    If _n IsNot Nothing Then
                        _n.Notation_Date = n.NotationDate
                        _n.Occurrence_Date = n.OccurenceDate
                        _n.Person_entering_Notation = n.PersonEnteringNotation
                        _n.Person_Title = n.PersonTitle
                        '_n.RN_DD_Person_Type_Xref_Sid = appId ''********
                        _n.Create_By = Me.UserID
                        _n.Create_Date = DateTime.Now
                        _n.Last_Update_By = Me.UserID
                        _n.Last_Update_Date = DateTime.Now
                        If Not IsNothing(n.UnflaggedDate) Then
                            _n.UnFlagged_Date = n.UnflaggedDate
                        End If
                        _n.Notation_Type_Sid = n.NotationType.NTypeSid
                        For Each exnr As Notation_Reason_RN_DD_Xref In (From nr In context.Notation_Reason_RN_DD_Xref Where nr.Notation_Sid = _n.Notation_Sid Select nr).ToList
                            context.Notation_Reason_RN_DD_Xref.Remove(exnr)
                        Next
                        For Each l In n.NotationReasons
                            _nxl.Add(New Notation_Reason_RN_DD_Xref)
                            _nxl.Item(_nxl.Count - 1).Reasons_For_Notation_Sid = l.NReasonSid
                            _nxl.Item(_nxl.Count - 1).Create_By = Me.UserID
                            _nxl.Item(_nxl.Count - 1).Create_Date = DateTime.Now
                            _nxl.Item(_nxl.Count - 1).Last_Update_By = Me.UserID
                            _nxl.Item(_nxl.Count - 1).Last_Update_Date = DateTime.Now
                            _nxl.Item(_nxl.Count - 1).Notation = _n
                            context.Notation_Reason_RN_DD_Xref.Add(_nxl.Item(_nxl.Count - 1))
                        Next
                    End If
                    context.SaveChanges()
                    retval.ReturnValue = _n.Notation_Sid
                    'Dim _n As New Application_Notation
                    ''Dim _nxl As New List(Of Application_Notation_Reason_Xref)
                    '_n = (From an In context.Application_Notation Where an.Application_Notation_Sid = nid Select an).FirstOrDefault

                    ''_n.NotationDate = n.NotationDate
                    ''_n.OccurrenceDate = n.OccurenceDate
                    ''_n.Person_entering_Notation = n.PersonEnteringNotation
                    ''_n.PersonTitle = n.PersonTitle
                    ''_n.Application_Sid = appId
                    ''_n.Create_By = Me.UserID
                    ''_n.Create_Date = DateTime.Now
                    '_n.Last_Update_By = Me.UserID
                    '_n.Last_Update_Date = DateTime.Now
                    'If Not IsNothing(n.UnflaggedDate) Then
                    '    _n.UnFlagged_Date = n.UnflaggedDate
                    'End If
                    ''_n.Notation_Type_Sid = n.NotationType.NTypeSid

                    ''For Each l In n.NotationReasons
                    ''    _nxl.Add(New Application_Notation_Reason_Xref)
                    ''    _nxl.Item(_nxl.Count - 1).Reasons_For_Notation_Sid = l.NReasonSid
                    ''    _nxl.Item(_nxl.Count - 1).Create_By = Me.UserID
                    ''    _nxl.Item(_nxl.Count - 1).Create_Date = DateTime.Now
                    ''    _nxl.Item(_nxl.Count - 1).Last_Update_By = Me.UserID
                    ''    _nxl.Item(_nxl.Count - 1).Last_Update_Date = DateTime.Now
                    ''    _nxl.Item(_nxl.Count - 1).Application_Notation = _n
                    ''    context.Application_Notation_Reason_Xref.Add(_nxl.Item(_nxl.Count - 1))
                    ''Next

                    'context.SaveChanges()
                End Using
            Catch ex As Exception
                'strResult = ex.Message
                Me._messages.Add(New ReturnMessage("Error while updating Notation.", True, False))
                Me.LogError("Error while updating Notation.", CInt(Me.UserID), ex)
                retval.AddErrorMessage(ex.Message)
            Finally
            End Try
            Return retval
        End Function
        Public Function GetNotationTypes() As List(Of Objects.NType) Implements INotationQueries.GetNotationTypes
            Dim NotationTypesList As List(Of Objects.NType) = Nothing
            'Dim SSN As Integer = DDPersonnelID

            Try
                Using context As New MAISContext()

                    Return (From nt In context.Notation_Type Select New Objects.NType() With {
                        .NTypeSid = nt.Notation_Type_Sid, .NTypeDesc = nt.Notation_Desc
                        }
                    ).ToList
                End Using
            Catch ex As Exception
                Me.LogError("Error Getting Notation type", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error geting Notation type.", True, False))
            End Try
            Return NotationTypesList
        End Function

        Public Function GetNotationReasons() As List(Of Objects.NReason) Implements INotationQueries.GetNotationReasons
            Dim NotationRList As List(Of Objects.NReason) = Nothing
            Try
                Using context As New MAISContext()

                    Return (From nr In context.Reasons_For_Notation Select New Objects.NReason() With {
                    .NReasonSid = nr.Reasons_For_Notation_Sid, .NReasonDesc = nr.Reason_Desc
                    }
                    ).ToList
                End Using
            Catch ex As Exception
                Me.LogError("Error Getting Notation reason", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error geting Notation reason.", True, False))
            End Try
            Return NotationRList
        End Function
        Public Function GetCertStatuses() As List(Of Objects.CertStatus) Implements INotationQueries.GetCertStatuses
            Dim CList As List(Of Objects.CertStatus) = Nothing
            Try
                Using context As New MAISContext()
                    Return (From cs In context.Certification_Status_Type Select New Objects.CertStatus() With {
                        .CertStatusSid = cs.Certification_Status_Type_Sid,
                        .CertStatusDesc = cs.Certification_Status_Desc}).ToList
                End Using
            Catch ex As Exception
                Me.LogError("Error Getting Cert Status Info", CInt(Me.UserID), ex)
                Me._messages.Add(New ReturnMessage("Error Getting Cert Status Info.", True, False))
            End Try
            Return CList
        End Function
    End Class

End Namespace
