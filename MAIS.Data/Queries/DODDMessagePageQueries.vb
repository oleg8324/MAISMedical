Imports MAIS.Data.Queries
Imports ODMRDDHelperClassLibrary.Utility
Imports System.Data.Entity.Validation
Imports System.Data.Objects

Namespace Queries
    Public Interface IDODDMessagePageQueries
        Inherits IQueriesBase

        Function Save_DODDMessage(ByVal iMessage As Objects.DODDMessageInfo) As Integer
        Function GetCurrentMessage() As List(Of Objects.DODDMessageInfo)
        Function GetMessageDataByMessageID(ByVal iMessageID As Integer) As Objects.DODDMessageInfo
        Function GetMessageDataByUserRoles(ByVal iRoleList As String, ByVal userID As Integer) As List(Of Objects.DODDMessageInfo)
        Function GetMessageDataArchivedDataByUserRolesAndPersionID(ByVal iRoleList As String, ByVal UserID As Integer) As List(Of Objects.DODDMessageInfo)
        Function SearchMessageDataByDates(ByVal iStartDate As Date, ByVal iEndDate As Date) As List(Of Objects.DODDMessageInfo)
    End Interface
    Public Class DODDMessagePageQueries
        Inherits QueriesBase
        Implements IDODDMessagePageQueries

        Public Function Save_DODDMessage(iMessage As Objects.DODDMessageInfo) As Integer Implements IDODDMessagePageQueries.Save_DODDMessage
            Dim retVal As Boolean = False
            Dim MessageID As Integer = -1
            Try
                Using context As New MAISContext
                    If iMessage IsNot Nothing Then
                        If iMessage.DODD_Message_SID = -1 Then ' This is a new message 
                            Dim _newMessage As New Data.DODD_Message
                            With _newMessage
                                .Subject = iMessage.Subject
                                .Description = iMessage.Description
                                .Priority = iMessage.Priority
                                .Message_Start_Date = iMessage.Message_Start_Date
                                .Message_End_Date = iMessage.Message_End_Date
                                .Active_Flg = iMessage.Active_Flag
                                .Create_By = UserID
                                .Create_Date = Now()
                                .Last_Update_By = UserID
                                .Last_Update_Date = Now()
                                If iMessage.RolesList.Count > 0 Then
                                    For Each r In iMessage.RolesList
                                        Dim roleList As New Data.DODD_Message_MAIS_Role_XRef
                                        With roleList
                                            .DODD_Message = _newMessage
                                            .MAIS_Role_Sid = r.MAISRolesSid
                                            .Active_Flg = r.Active_Flg
                                            .CreatedBy = UserID
                                            .Created = Now()
                                        End With
                                        context.DODD_Message_MAIS_Role_XRef.Add(roleList)
                                    Next

                                End If
                                If iMessage.PersonList.Count > 0 Then
                                    For Each p In iMessage.PersonList
                                        Dim personList As New Data.DODD_Message_RN_DD_Person_Type_Xref_Xref
                                        With personList
                                            .DODD_Message = _newMessage
                                            .RN_DD_Person_Type_XRef_SID = (From RNNumber In context.RN_DD_Person_Type_Xref
                                                                                             Where RNNumber.RN_DDPersonnel_Sid = p.RN_Sid
                                                                                             Select RNNumber.RN_DD_Person_Type_Xref_Sid).FirstOrDefault
                                            .Active_Flg = p.Active_Flg
                                            .CreatedBy = UserID
                                            .Created = Now()
                                            context.DODD_Message_RN_DD_Person_Type_Xref_Xref.Add(personList)
                                        End With
                                    Next
                                End If
                            End With
                            retVal = context.SaveChanges()
                            If retVal = True Then
                                MessageID = _newMessage.DODD_Message_Sid
                            Else
                                MessageID = -1
                            End If

                        Else ' Need to update the data
                            Dim M As New Data.DODD_Message
                            M = (From MM In context.DODD_Message
                                 Where MM.DODD_Message_Sid = iMessage.DODD_Message_SID
                                 Select MM).FirstOrDefault
                            With M
                                .Subject = iMessage.Subject
                                .Description = iMessage.Description
                                .Priority = iMessage.Priority
                                .Message_Start_Date = iMessage.Message_Start_Date
                                .Message_End_Date = iMessage.Message_End_Date
                                .Active_Flg = iMessage.Active_Flag
                                .Last_Update_Date = Now()
                                .Last_Update_By = UserID
                            End With

                            Dim RemoveRoleList = (From rRl In context.DODD_Message_MAIS_Role_XRef Where rRl.DODD_Message_Sid = M.DODD_Message_Sid Select rRl).ToList

                            For Each rl In RemoveRoleList
                                context.DODD_Message_MAIS_Role_XRef.Remove(rl)
                            Next
                            M.DODD_Message_MAIS_Role_XRef = (From nRl In iMessage.RolesList _
                                                             Select New Data.DODD_Message_MAIS_Role_XRef With {
                                                                 .DODD_Message_Sid = nRl.DODD_Message_SID,
                                                                 .MAIS_Role_Sid = nRl.MAISRolesSid,
                                                                 .Active_Flg = nRl.Active_Flg,
                                                                 .CreatedBy = UserID,
                                                                 .Created = Today}).ToList

                            Dim RemovePersonList = (From rPL In context.DODD_Message_RN_DD_Person_Type_Xref_Xref Where rPL.DODD_Message_Sid = M.DODD_Message_Sid Select rPL).ToList

                            For Each pl In RemovePersonList
                                context.DODD_Message_RN_DD_Person_Type_Xref_Xref.Remove(pl)
                            Next

                            M.DODD_Message_RN_DD_Person_Type_Xref_Xref = (From nPl In iMessage.PersonList _
                                                                          Select New Data.DODD_Message_RN_DD_Person_Type_Xref_Xref With {
                                                                              .DODD_Message_Sid = nPl.DODD_Message_Sid,
                                                                              .Active_Flg = nPl.Active_Flg,
                                                                              .CreatedBy = UserID,
                                                                              .Created = Today,
                                                                              .RN_DD_Person_Type_XRef_SID = (From RNNumber In context.RN_DD_Person_Type_Xref
                                                                                             Where RNNumber.RN_DDPersonnel_Sid = nPl.RN_Sid
                                                                                             Select RNNumber.RN_DD_Person_Type_Xref_Sid).FirstOrDefault}).ToList

                            retVal = context.SaveChanges
                            If retVal = True Then
                                Return M.DODD_Message_Sid
                            Else
                                Return -1
                            End If
                        End If


                    End If

                End Using



                Return MessageID


            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while saving DODD Message from the DoDD Message Query services.", True, False))
                Me.LogError("Error while saving DODD Message from the DoDD Message Query services.", CInt(Me.UserID), ex)
                MessageID = -1
                Return MessageID
            End Try

        End Function

        Public Function GetCurrentMessage() As List(Of Objects.DODDMessageInfo) Implements IDODDMessagePageQueries.GetCurrentMessage
            Dim retVal As New List(Of Objects.DODDMessageInfo)
            Try
                Using context As New MAISContext
                    retVal = (From dm In context.DODD_Message
                              Where Today >= dm.Message_Start_Date And Today <= dm.Message_End_Date
                              Select New Objects.DODDMessageInfo With {
                                  .DODD_Message_SID = dm.DODD_Message_Sid,
                                  .Subject = dm.Subject,
                                  .Description = dm.Description,
                                  .Message_Start_Date = dm.Message_Start_Date,
                                  .Message_End_Date = dm.Message_End_Date,
                                  .Active_Flag = dm.Active_Flg,
                                  .Priority = dm.Priority}).ToList
                    '              .RolesList = (From rl In dm.DODD_Message_MAIS_Role_XRef
                    '                            Select New Objects.DODDMessageInfoMaisRoles With {
                    '                                .DODD_Message_MAIS_Role_XRef_SID = rl.DODD_Message_MAIS_Role_XRef_SID,
                    '                                .DODD_Message_SID = rl.DODD_Message_Sid,
                    '                                .MAISRoleName = (From RT In context.MAIS_Role _
                    '                                                Where RT.Role_Sid = rl.MAIS_Role_Sid _
                    '                                                Select RT.Role_Desc).First,
                    '                                .MAISRolesSid = rl.MAIS_Role_Sid}).ToList,
                    '.PersonList = (From Pl In dm.DODD_Message_RN_DD_Person_Type_Xref_Xref
                    '               Select New Objects.DODDMessageInfoMaisRNDDPerson With {
                    '                   .DODD_Message_RN_DD_Person_Type_Xref_Sid = Pl.DODD_Message_RN_DD_Person_Type_Xref_Xref_Sid,
                    '                   .RN_Sid = (From r In context.RN_DD_Person_Type_Xref
                    '                              Where r.RN_DD_Person_Type_Xref_Sid = Pl.RN_DD_Person_Type_XRef_SID
                    '                              Select r.RN_DDPersonnel_Sid).First,
                    '                   .RN_Name = (From RName In context.RNs
                    '                               Join RNDDList In context.RN_DD_Person_Type_Xref On RName.RN_Sid Equals RNDDList.RN_DDPersonnel_Sid
                    '                               Where RNDDList.RN_DD_Person_Type_Xref_Sid = Pl.RN_DD_Person_Type_XRef_SID _
                    '                               Select RName.Last_Name + "," + RName.First_Name).First,
                    '                   .RN_DD_Person_Type_XRef_SID = Pl.RN_DD_Person_Type_XRef_SID,
                    '                   .Active_Flg = Pl.Active_Flg,
                    '                   .DODD_Message_Sid = Pl.DODD_Message_Sid}).ToList

                    For Each RVlist In retVal
                        Dim rl As New List(Of Objects.DODDMessageInfoMaisRoles)
                        rl = (From mRL In context.DODD_Message_MAIS_Role_XRef
                              Where mRL.DODD_Message_Sid = RVlist.DODD_Message_SID
                              Select New Objects.DODDMessageInfoMaisRoles With {
                                  .Active_Flg = mRL.Active_Flg,
                                  .DODD_Message_MAIS_Role_XRef_SID = mRL.DODD_Message_MAIS_Role_XRef_SID,
                                  .DODD_Message_SID = mRL.DODD_Message_Sid,
                                  .MAISRolesSid = mRL.MAIS_Role_Sid,
                                  .MAISRoleName = mRL.MAIS_Role.Role_Desc}).ToList
                        RVlist.RolesList = rl

                    Next

                    For Each Rvlist In retVal
                        Dim pl As New List(Of Objects.DODDMessageInfoMaisRNDDPerson)
                        pl = (From mPL In context.DODD_Message_RN_DD_Person_Type_Xref_Xref
                              Where mPL.DODD_Message_Sid = Rvlist.DODD_Message_SID
                              Select New Objects.DODDMessageInfoMaisRNDDPerson With {
                                  .Active_Flg = mPL.Active_Flg,
                                  .DODD_Message_RN_DD_Person_Type_Xref_Sid = mPL.DODD_Message_RN_DD_Person_Type_Xref_Xref_Sid,
                                  .DODD_Message_Sid = mPL.DODD_Message_Sid,
                                  .RN_DD_Person_Type_XRef_SID = mPL.RN_DD_Person_Type_XRef_SID,
                                  .RN_Sid = mPL.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid,
                                  .RN_Name = (From RnList In context.RNs _
                                              Where RnList.RN_Sid = mPL.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid _
                              Select RnList.Last_Name + "," + RnList.First_Name).FirstOrDefault}).ToList
                        Rvlist.PersonList = pl

                    Next
                End Using

                Return retVal
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while pulling DODD Message from the DoDD Message Query services.", True, False))
                Me.LogError("Error while pulling DODD Message from the DoDD Message Query services.", CInt(Me.UserID), ex)

                Return retVal
            End Try
        End Function

        Public Function GetMessageDataByMessageID(iMessageID As Integer) As Objects.DODDMessageInfo Implements IDODDMessagePageQueries.GetMessageDataByMessageID
            Dim retVal As New Objects.DODDMessageInfo
            Try
                Using context As New MAISContext
                    retVal = (From dm In context.DODD_Message
                            Where dm.DODD_Message_Sid = iMessageID
                            Select New Objects.DODDMessageInfo With {
                                .DODD_Message_SID = dm.DODD_Message_Sid,
                                .Subject = dm.Subject,
                                .Description = dm.Description,
                                .Message_Start_Date = dm.Message_Start_Date,
                                .Message_End_Date = dm.Message_End_Date,
                                .Active_Flag = dm.Active_Flg,
                                .Priority = dm.Priority}).FirstOrDefault


                    Dim rl As New List(Of Objects.DODDMessageInfoMaisRoles)
                    rl = (From mRL In context.DODD_Message_MAIS_Role_XRef
                          Where mRL.DODD_Message_Sid = retVal.DODD_Message_SID
                          Select New Objects.DODDMessageInfoMaisRoles With {
                              .Active_Flg = mRL.Active_Flg,
                              .DODD_Message_MAIS_Role_XRef_SID = mRL.DODD_Message_MAIS_Role_XRef_SID,
                              .DODD_Message_SID = mRL.DODD_Message_Sid,
                              .MAISRolesSid = mRL.MAIS_Role_Sid,
                              .MAISRoleName = mRL.MAIS_Role.Role_Desc}).ToList
                    retVal.RolesList = rl

                    Dim pl As New List(Of Objects.DODDMessageInfoMaisRNDDPerson)
                    pl = (From mPL In context.DODD_Message_RN_DD_Person_Type_Xref_Xref
                          Where mPL.DODD_Message_Sid = retVal.DODD_Message_SID
                          Select New Objects.DODDMessageInfoMaisRNDDPerson With {
                              .Active_Flg = mPL.Active_Flg,
                              .DODD_Message_RN_DD_Person_Type_Xref_Sid = mPL.DODD_Message_RN_DD_Person_Type_Xref_Xref_Sid,
                              .DODD_Message_Sid = mPL.DODD_Message_Sid,
                              .RN_DD_Person_Type_XRef_SID = mPL.RN_DD_Person_Type_XRef_SID,
                              .RN_Sid = mPL.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid,
                              .RN_Name = (From RnList In context.RNs _
                                          Where RnList.RN_Sid = mPL.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid _
                          Select RnList.Last_Name + "," + RnList.First_Name).FirstOrDefault}).ToList
                    retVal.PersonList = pl

                End Using

                Return retVal

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while pulling DODD Message by Message ID from the DoDD Message Query services.", True, False))
                Me.LogError("Error while pulling DODD Message by Message ID from the DoDD Message Query services.", CInt(Me.UserID), ex)

                Return retVal
            End Try
        End Function

        Public Function GetMessageDataByUserRoles(iRoleList As String, userID As Integer) As List(Of Objects.DODDMessageInfo) Implements IDODDMessagePageQueries.GetMessageDataByUserRoles
            Dim retVal As New List(Of Objects.DODDMessageInfo)
            Try
                Using context As New MAISContext

                    Dim MIDList As String = Nothing

                    Dim RoleList = (From dm In context.DODD_Message
                               Join Rm In context.DODD_Message_MAIS_Role_XRef On Rm.DODD_Message_Sid Equals dm.DODD_Message_Sid
                        Where Today >= dm.Message_Start_Date And Today <= dm.Message_End_Date And iRoleList.Contains(Rm.MAIS_Role_Sid)
                        Select dm.DODD_Message_Sid).ToList

                    Dim EmplTypeID = (From RNPT In context.RN_DD_Person_Type_Xref
                                    Join RN In context.RNs On RN.RN_Sid Equals RNPT.RN_DDPersonnel_Sid
                                    Join URN In context.User_RN_Mapping On URN.RN_Sid Equals RN.RN_Sid
                                    Where URN.UserID = userID
                                    Select RNPT.RN_DD_Person_Type_Xref_Sid).FirstOrDefault

                    Dim PerList = (From DDP In context.DODD_Message_RN_DD_Person_Type_Xref_Xref
                              Join dm In context.DODD_Message On dm.DODD_Message_Sid Equals DDP.DODD_Message_Sid
                        Where DDP.RN_DD_Person_Type_XRef_SID = EmplTypeID
                               Select dm.DODD_Message_Sid).ToList

                    For Each mi In RoleList
                        If MIDList IsNot Nothing Then
                            MIDList = MIDList & ", " & mi
                        Else
                            MIDList = mi
                        End If
                    Next
                    For Each mi In PerList
                        If MIDList IsNot Nothing Then
                            MIDList = MIDList & ", " & mi
                        Else
                            MIDList = mi
                        End If
                    Next

                    retVal = (From dm In context.DODD_Message
                        Where MIDList.Contains(dm.DODD_Message_Sid)
                     Select New Objects.DODDMessageInfo With {
                                              .DODD_Message_SID = dm.DODD_Message_Sid,
                                              .Subject = dm.Subject,
                                              .Description = dm.Description,
                                              .Message_Start_Date = dm.Message_Start_Date,
                                              .Message_End_Date = dm.Message_End_Date,
                                              .Active_Flag = dm.Active_Flg,
                                              .Priority = dm.Priority}).ToList


                    For Each RVlist In retVal
                        Dim rl As New List(Of Objects.DODDMessageInfoMaisRoles)
                        rl = (From mRL In context.DODD_Message_MAIS_Role_XRef
                              Where mRL.DODD_Message_Sid = RVlist.DODD_Message_SID
                              Select New Objects.DODDMessageInfoMaisRoles With {
                                  .Active_Flg = mRL.Active_Flg,
                                  .DODD_Message_MAIS_Role_XRef_SID = mRL.DODD_Message_MAIS_Role_XRef_SID,
                                  .DODD_Message_SID = mRL.DODD_Message_Sid,
                                  .MAISRolesSid = mRL.MAIS_Role_Sid,
                                  .MAISRoleName = mRL.MAIS_Role.Role_Desc}).ToList
                        RVlist.RolesList = rl

                    Next

                    For Each Rvlist In retVal
                        Dim pl As New List(Of Objects.DODDMessageInfoMaisRNDDPerson)
                        pl = (From mPL In context.DODD_Message_RN_DD_Person_Type_Xref_Xref
                              Where mPL.DODD_Message_Sid = Rvlist.DODD_Message_SID
                              Select New Objects.DODDMessageInfoMaisRNDDPerson With {
                                  .Active_Flg = mPL.Active_Flg,
                                  .DODD_Message_RN_DD_Person_Type_Xref_Sid = mPL.DODD_Message_RN_DD_Person_Type_Xref_Xref_Sid,
                                  .DODD_Message_Sid = mPL.DODD_Message_Sid,
                                  .RN_DD_Person_Type_XRef_SID = mPL.RN_DD_Person_Type_XRef_SID,
                                  .RN_Sid = mPL.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid,
                                  .RN_Name = (From RnList In context.RNs _
                                              Where RnList.RN_Sid = mPL.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid _
                              Select RnList.Last_Name + "," + RnList.First_Name).FirstOrDefault}).ToList
                        Rvlist.PersonList = pl

                    Next


                End Using

                Return retVal

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while pulling DODD Message by user role id's from the DoDD Message Query services.", True, False))
                Me.LogError("Error while pulling DODD Message by user role id's from the DoDD Message Query services.", CInt(Me.UserID), ex)

                Return retVal
            End Try
        End Function

        Public Function GetMessageDataArchivedDataByUserRolesAndPersionID(iRoleList As String, UserID As Integer) As List(Of Objects.DODDMessageInfo) Implements IDODDMessagePageQueries.GetMessageDataArchivedDataByUserRolesAndPersionID
            Dim retVal As New List(Of Objects.DODDMessageInfo)
            Try
                Using context As New MAISContext
                    Dim MIDList As String = Nothing

                    Dim RoleList = (From DDP In context.DODD_Message_RN_DD_Person_Type_Xref_Xref
                              Join dm In context.DODD_Message On dm.DODD_Message_Sid Equals DDP.DODD_Message_Sid
                               Join Rm In context.DODD_Message_MAIS_Role_XRef On Rm.DODD_Message_Sid Equals dm.DODD_Message_Sid
                        Where iRoleList.Contains(Rm.MAIS_Role_Sid)
                        Select dm.DODD_Message_Sid).ToList

                    Dim EmplTypeID = (From RNPT In context.RN_DD_Person_Type_Xref
                                    Join RN In context.RNs On RN.RN_Sid Equals RNPT.RN_DDPersonnel_Sid
                                    Join URN In context.User_RN_Mapping On URN.RN_Sid Equals RN.RN_Sid
                                    Where URN.UserID = UserID
                                    Select RNPT.RN_DD_Person_Type_Xref_Sid).FirstOrDefault

                    Dim PerList = (From DDP In context.DODD_Message_RN_DD_Person_Type_Xref_Xref
                              Join dm In context.DODD_Message On dm.DODD_Message_Sid Equals DDP.DODD_Message_Sid
                        Where DDP.RN_DD_Person_Type_XRef_SID = (From RNPT In context.RN_DD_Person_Type_Xref
                                                                                                        Join RN In context.RNs On RN.RN_Sid Equals RNPT.RN_DDPersonnel_Sid
                                                                                                        Join URN In context.User_RN_Mapping On URN.RN_Sid Equals RN.RN_Sid
                                                                                                        Where URN.UserID = UserID
                                                                                                        Select RNPT.RN_DD_Person_Type_Xref_Sid).FirstOrDefault
                               Select dm.DODD_Message_Sid).ToList

                    For Each mi In RoleList
                        If MIDList IsNot Nothing Then
                            MIDList = MIDList & ", " & mi
                        Else
                            MIDList = mi
                        End If
                    Next
                    For Each mi In PerList
                        If MIDList IsNot Nothing Then
                            MIDList = MIDList & ", " & mi
                        Else
                            MIDList = mi
                        End If
                    Next


                    retVal = (From dm In context.DODD_Message
                        Where MIDList.Contains(dm.DODD_Message_Sid)
                        Select New Objects.DODDMessageInfo With {
                            .DODD_Message_SID = dm.DODD_Message_Sid,
                            .Subject = dm.Subject,
                            .Description = dm.Description,
                            .Message_Start_Date = dm.Message_Start_Date,
                            .Message_End_Date = dm.Message_End_Date,
                            .Active_Flag = dm.Active_Flg,
                            .Priority = dm.Priority}).ToList



                    For Each Rvlist In retVal
                        Dim rl As New List(Of Objects.DODDMessageInfoMaisRoles)
                        rl = (From mRL In context.DODD_Message_MAIS_Role_XRef
                              Where mRL.DODD_Message_Sid = Rvlist.DODD_Message_SID
                              Select New Objects.DODDMessageInfoMaisRoles With {
                                  .Active_Flg = mRL.Active_Flg,
                                  .DODD_Message_MAIS_Role_XRef_SID = mRL.DODD_Message_MAIS_Role_XRef_SID,
                                  .DODD_Message_SID = mRL.DODD_Message_Sid,
                                  .MAISRolesSid = mRL.MAIS_Role_Sid,
                                  .MAISRoleName = mRL.MAIS_Role.Role_Desc}).ToList
                        Rvlist.RolesList = rl

                    Next

                    For Each Rvlist In retVal
                        Dim pl As New List(Of Objects.DODDMessageInfoMaisRNDDPerson)
                        pl = (From mPL In context.DODD_Message_RN_DD_Person_Type_Xref_Xref
                              Where mPL.DODD_Message_Sid = Rvlist.DODD_Message_SID
                              Select New Objects.DODDMessageInfoMaisRNDDPerson With {
                                  .Active_Flg = mPL.Active_Flg,
                                  .DODD_Message_RN_DD_Person_Type_Xref_Sid = mPL.DODD_Message_RN_DD_Person_Type_Xref_Xref_Sid,
                                  .DODD_Message_Sid = mPL.DODD_Message_Sid,
                                  .RN_DD_Person_Type_XRef_SID = mPL.RN_DD_Person_Type_XRef_SID,
                                  .RN_Sid = mPL.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid,
                                  .RN_Name = (From RnList In context.RNs _
                                              Where RnList.RN_Sid = mPL.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid _
                              Select RnList.Last_Name + "," + RnList.First_Name).FirstOrDefault}).ToList
                        Rvlist.PersonList = pl

                    Next


                End Using

                Return retVal
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while pulling DODD Message by user role id's  and Personal ID from the DoDD Message Query services.", True, False))
                Me.LogError("Error while pulling DODD Message by user role id's and Personal ID  from the DoDD Message Query services.", CInt(Me.UserID), ex)

                Return retVal
            End Try
        End Function

        Public Function SearchMessageDataByDates(iStartDate As Date, iEndDate As Date) As List(Of Objects.DODDMessageInfo) Implements IDODDMessagePageQueries.SearchMessageDataByDates
            Dim retVal As New List(Of Objects.DODDMessageInfo)
            Dim TestDate As New Date

            Try

         
            Using context As New MAISContext

                Select Case True
                        Case iStartDate <> TestDate And iEndDate <> TestDate
                            retVal = (From dm In context.DODD_Message
                                Where dm.Message_Start_Date >= iStartDate And dm.Message_End_Date <= iEndDate
                                Select New Objects.DODDMessageInfo With {
                                    .DODD_Message_SID = dm.DODD_Message_Sid,
                                    .Subject = dm.Subject,
                                    .Description = dm.Description,
                                    .Message_Start_Date = dm.Message_Start_Date,
                                    .Message_End_Date = dm.Message_End_Date,
                                    .Active_Flag = dm.Active_Flg,
                                    .Priority = dm.Priority}).ToList
                        Case iStartDate <> TestDate And iEndDate = TestDate
                            retVal = (From dm In context.DODD_Message
                                Where dm.Message_Start_Date >= iStartDate
                                Select New Objects.DODDMessageInfo With {
                                    .DODD_Message_SID = dm.DODD_Message_Sid,
                                    .Subject = dm.Subject,
                                    .Description = dm.Description,
                                    .Message_Start_Date = dm.Message_Start_Date,
                                    .Message_End_Date = dm.Message_End_Date,
                                    .Active_Flag = dm.Active_Flg,
                                    .Priority = dm.Priority}).ToList

                        Case iStartDate = TestDate And iEndDate <> TestDate
                            retVal = (From dm In context.DODD_Message
                               Where iEndDate <= dm.Message_End_Date
                               Select New Objects.DODDMessageInfo With {
                                   .DODD_Message_SID = dm.DODD_Message_Sid,
                                   .Subject = dm.Subject,
                                   .Description = dm.Description,
                                   .Message_Start_Date = dm.Message_Start_Date,
                                   .Message_End_Date = dm.Message_End_Date,
                                   .Active_Flag = dm.Active_Flg,
                                   .Priority = dm.Priority}).ToList
                    End Select

                For Each RVlist In retVal
                    Dim rl As New List(Of Objects.DODDMessageInfoMaisRoles)
                    rl = (From mRL In context.DODD_Message_MAIS_Role_XRef
                          Where mRL.DODD_Message_Sid = RVlist.DODD_Message_SID
                          Select New Objects.DODDMessageInfoMaisRoles With {
                              .Active_Flg = mRL.Active_Flg,
                              .DODD_Message_MAIS_Role_XRef_SID = mRL.DODD_Message_MAIS_Role_XRef_SID,
                              .DODD_Message_SID = mRL.DODD_Message_Sid,
                              .MAISRolesSid = mRL.MAIS_Role_Sid,
                              .MAISRoleName = mRL.MAIS_Role.Role_Desc}).ToList
                    RVlist.RolesList = rl

                Next

                For Each Rvlist In retVal
                    Dim pl As New List(Of Objects.DODDMessageInfoMaisRNDDPerson)
                    pl = (From mPL In context.DODD_Message_RN_DD_Person_Type_Xref_Xref
                          Where mPL.DODD_Message_Sid = Rvlist.DODD_Message_SID
                          Select New Objects.DODDMessageInfoMaisRNDDPerson With {
                              .Active_Flg = mPL.Active_Flg,
                              .DODD_Message_RN_DD_Person_Type_Xref_Sid = mPL.DODD_Message_RN_DD_Person_Type_Xref_Xref_Sid,
                              .DODD_Message_Sid = mPL.DODD_Message_Sid,
                              .RN_DD_Person_Type_XRef_SID = mPL.RN_DD_Person_Type_XRef_SID,
                              .RN_Sid = mPL.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid,
                              .RN_Name = (From RnList In context.RNs _
                                          Where RnList.RN_Sid = mPL.RN_DD_Person_Type_Xref.RN_DDPersonnel_Sid _
                          Select RnList.Last_Name + "," + RnList.First_Name).FirstOrDefault}).ToList
                    Rvlist.PersonList = pl

                Next
            End Using

                Return retVal
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while pulling DODD Message from Search by Dates form DODD Message Query services.", True, False))
                Me.LogError("Error while pulling DODD Message from Search by Dates form DODD Message Query services.", CInt(Me.UserID), ex)

                Return retVal
            End Try
        End Function
    End Class
End Namespace

