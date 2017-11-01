Imports MAIS.Data

Namespace Mapping
    Public Class DODDMessageMapping
        Public Shared Function mapDODDMessageToDB(ByVal inMessage As Model.DODDMessageInfo) As Objects.DODDMessageInfo
            Dim RetVal As New Objects.DODDMessageInfo
            With RetVal
                .DODD_Message_SID = inMessage.DODD_Message_SID
                .Subject = inMessage.Subject
                .Description = inMessage.Description
                .Priority = inMessage.Priority
                .Message_Start_Date = inMessage.Message_Start_Date
                .Message_End_Date = inMessage.Message_End_Date
                .Active_Flag = inMessage.Active_Flag
                .CreateBy = inMessage.CreateBy
                .LastUpdateBy = inMessage.LastUpdateBy
                .RolesList = (From r In inMessage.RolesList
                              Select New Objects.DODDMessageInfoMaisRoles With {
                                .DODD_Message_MAIS_Role_XRef_SID = r.DODD_Message_MAIS_Role_XRef_SID,
                                  .DODD_Message_SID = r.DODD_Message_SID,
                                  .MAISRolesSid = r.MAISRolesSid,
                                  .Active_Flg = r.Active_Flg}).ToList
                .PersonList = (From p In inMessage.PersonList
                              Select New Objects.DODDMessageInfoMaisRNDDPerson With {
                                  .DODD_Message_RN_DD_Person_Type_Xref_Sid = p.DODD_Message_RN_DD_Person_Type_Xref_Sid,
                                  .DODD_Message_Sid = p.DODD_Message_Sid,
                                    .RN_Sid = p.RN_Sid,
                                  .RN_DD_Person_Type_XRef_SID = p.RN_DD_Person_Type_XRef_SID,
                                  .Active_Flg = p.Active_Flg}).ToList

            End With

            Return RetVal

        End Function

        Public Shared Function mapDBtoDODDMessage(ByVal inMessage As List(Of Objects.DODDMessageInfo)) As List(Of Model.DODDMessageInfo)
            Dim retVal As New List(Of Model.DODDMessageInfo)

            retVal = (From db In inMessage
                      Select New Model.DODDMessageInfo With {
                          .DODD_Message_SID = db.DODD_Message_SID,
                          .Subject = db.Subject,
                          .Description = db.Description,
                          .Priority = db.Priority,
                          .Message_Start_Date = db.Message_Start_Date,
                          .Message_End_Date = db.Message_End_Date,
                          .Active_Flag = db.Active_Flag,
                          .CreateBy = db.CreateBy,
                          .LastUpdateBy = db.LastUpdateBy,
                          .RolesList = (From rl In db.RolesList
                                         Select New Model.DODDMessageInfoMaisRoles With {
                                            .DODD_Message_SID = rl.DODD_Message_SID,
                                            .Active_Flg = rl.Active_Flg,
                                            .DODD_Message_MAIS_Role_XRef_SID = rl.DODD_Message_MAIS_Role_XRef_SID,
                                            .MAISRolesSid = rl.MAISRolesSid,
                                            .MAISRoleName = rl.MAISRoleName}).ToList,
            .PersonList = (From PL In db.PersonList
                           Select New Model.DODDMessageInfoMaisRNDDPerson With {
                               .Active_Flg = PL.Active_Flg,
                               .DODD_Message_RN_DD_Person_Type_Xref_Sid = PL.DODD_Message_RN_DD_Person_Type_Xref_Sid,
                               .DODD_Message_Sid = PL.DODD_Message_Sid,
                               .RN_DD_Person_Type_XRef_SID = PL.RN_DD_Person_Type_XRef_SID,
                               .RN_Name = PL.RN_Name,
                               .RN_Sid = PL.RN_Sid}).ToList}).ToList


            Return retVal

        End Function

        Public Shared Function mapDBToDODDMessage(ByVal inMessage As Objects.DODDMessageInfo) As Model.DODDMessageInfo
            Dim retVal As New Model.DODDMessageInfo

            With retVal
                .DODD_Message_SID = inMessage.DODD_Message_SID
                .Subject = inMessage.Subject
                .Description = inMessage.Description
                .Priority = inMessage.Priority
                .Message_Start_Date = inMessage.Message_Start_Date
                .Message_End_Date = inMessage.Message_End_Date
                .Active_Flag = inMessage.Active_Flag
                .CreateBy = inMessage.CreateBy
                .LastUpdateBy = inMessage.LastUpdateBy
                .RolesList = (From rl In inMessage.RolesList
                              Select New Model.DODDMessageInfoMaisRoles With {
                                   .DODD_Message_SID = rl.DODD_Message_SID,
                                            .Active_Flg = rl.Active_Flg,
                                            .DODD_Message_MAIS_Role_XRef_SID = rl.DODD_Message_MAIS_Role_XRef_SID,
                                            .MAISRolesSid = rl.MAISRolesSid,
                                            .MAISRoleName = rl.MAISRoleName}).ToList
                .PersonList = (From PL In inMessage.PersonList
                                 Select New Model.DODDMessageInfoMaisRNDDPerson With {
                               .Active_Flg = PL.Active_Flg,
                               .DODD_Message_RN_DD_Person_Type_Xref_Sid = PL.DODD_Message_RN_DD_Person_Type_Xref_Sid,
                               .DODD_Message_Sid = PL.DODD_Message_Sid,
                               .RN_DD_Person_Type_XRef_SID = PL.RN_DD_Person_Type_XRef_SID,
                               .RN_Name = PL.RN_Name,
                               .RN_Sid = PL.RN_Sid}).ToList
            End With

            Return retVal
        End Function
    End Class
End Namespace

