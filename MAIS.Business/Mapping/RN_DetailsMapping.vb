Imports MAIS.Data

Namespace Mapping
    Public Class RN_DetailsMapping
        Public Shared Function MappingToModelRN_User(ByVal dbRN_UserObject As Data.RN) As Model.RN_UserDetails
            Dim RN_UserModel As New Model.RN_UserDetails

            With RN_UserModel
                .RN_Sid = dbRN_UserObject.RN_Sid
                .RNLicense_Number = dbRN_UserObject.RNLicense_Number
                .FirstName = dbRN_UserObject.First_Name
                .MiddleName = dbRN_UserObject.Middle_Name
                .LastName = dbRN_UserObject.Last_Name
                .CreateDate = dbRN_UserObject.Create_Date
                .CreateBy = dbRN_UserObject.Create_By
                .LastUpdatedDate = dbRN_UserObject.Last_Updated_Date
                .LastUpdateBy = dbRN_UserObject.Last_Update_By
                .DateOfOriginalRNLicIssuance = dbRN_UserObject.Date_Of_Original_Issuance
                .Gender = dbRN_UserObject.Gender
                .Active_Flg = dbRN_UserObject.Active_Flg
            End With
            Return RN_UserModel

        End Function

        Public Shared Function MappingToModelRn_userList(ByVal dbRn_UserObject As List(Of Data.RN)) As List(Of Model.RN_UserDetails)
            Dim RN_userModleList As New List(Of Model.RN_UserDetails)

            RN_userModleList = (From a In dbRn_UserObject _
                                Select New Model.RN_UserDetails With {
                                    .RN_Sid = a.RN_Sid,
                                    .RNLicense_Number = a.RNLicense_Number,
                                    .FirstName = a.First_Name,
                                    .MiddleName = a.Middle_Name,
                                    .LastName = a.Last_Name,
                                    .CreateDate = a.Create_Date,
                                    .CreateBy = a.Create_By,
                                    .LastUpdatedDate = a.Last_Updated_Date,
                                    .LastUpdateBy = a.Last_Update_By,
                                    .DateOfOriginalRNLicIssuance = a.Date_Of_Original_Issuance,
                                    .Gender = a.Gender,
                                    .Active_Flg = a.Active_Flg}).ToList

            Return RN_userModleList
        End Function
        Public Shared Function Map_DDPersonnelList(ByVal ddPersonnelLsit As List(Of Data.DDPersonnel)) As List(Of Model.DD_Personnel)
            Dim lst_DDPersonnel As New List(Of Model.DD_Personnel)

            lst_DDPersonnel = (From a In ddPersonnelLsit _
                                Select New Model.DD_Personnel With {
                                    .DDPersonnel_Sid = a.DDPersonnel_Sid,
                                    .DDPersonnelCode = a.DDPersonnel_Code,
                                    .DDPersonnelNameSSN = a.First_Name + "," + a.Last_Name + " " + a.DDPersonnel_Code + ",SSN:" + a.SSN.ToString()
                                    }).ToList
            Return lst_DDPersonnel
        End Function

        Public Shared Function Map_DBtoRN_UserDetailsWithEmail(ByVal db As Objects.RN_UserDetailsObject) As Model.RN_UserDetails
            Dim RN_UserModel As New Model.RN_UserDetails

            With RN_UserModel
                .RN_Sid = db.RN_Sid
                .RNLicense_Number = db.RNLicense_Number
                .FirstName = db.FirstName
                .MiddleName = db.MiddleName
                .LastName = db.LastName
                .CreateDate = db.CreateDate
                .CreateBy = db.CreateBy
                .LastUpdatedDate = db.LastUpdatedDate
                .LastUpdateBy = db.LastUpdateBy
                .DateOfOriginalRNLicIssuance = db.DateOfOriginalRNLicIssuance
                .Gender = db.Gender
                .Active_Flg = db.Active_Flg
                .EmailList = (From e In db.EmailList
                              Select New Model.EmailAddressDetails With {
                                  .EmailAddressSID = e.EmailAddressSID,
                                  .EmailAddress = e.EmailAddress,
                                  .ContactType = e.ContactType}).ToList
            End With
            Return RN_UserModel
        End Function

        Public Shared Function Map_DBtoRN_UserDetailsWithEmail(ByVal db As List(Of Objects.RN_UserDetailsObject)) As List(Of Model.RN_UserDetails)
            Dim RN_userModleList As New List(Of Model.RN_UserDetails)

            RN_userModleList = (From r In db
                                Select New Model.RN_UserDetails With {
                                     .RN_Sid = r.RN_Sid,
                                     .RNLicense_Number = r.RNLicense_Number,
                                     .FirstName = r.FirstName,
                                     .MiddleName = r.MiddleName,
                                     .LastName = r.LastName,
                                     .CreateDate = r.CreateDate,
                                     .CreateBy = r.CreateBy,
                                     .LastUpdatedDate = r.LastUpdatedDate,
                                     .LastUpdateBy = r.LastUpdateBy,
                                     .DateOfOriginalRNLicIssuance = r.DateOfOriginalRNLicIssuance,
                                     .Gender = r.Gender,
                                     .Active_Flg = r.Active_Flg,
                                    .EmailList = (From e In r.EmailList
                                                  Select New Model.EmailAddressDetails With {
                                                            .EmailAddressSID = e.EmailAddressSID,
                                                            .EmailAddress = e.EmailAddress,
                                                            .ContactType = e.ContactType}).ToList}).ToList
            Return RN_userModleList
        End Function
    End Class

End Namespace
