Imports MAIS.Data.Queries
Imports ODMRDDHelperClassLibrary.Utility
Imports System.Data.Entity.Validation
Imports System.Data.Objects

Namespace Queries

    Public Interface IResourcePageQueries
        Inherits IQueriesBase

        Function Save_Resource(iMessage As Data.Resource) As Integer
        Function GetCurrentResource() As List(Of Data.Resource)
        Function GetResourceDataByResourceID(iMessageID As Integer) As Data.Resource
    End Interface
    Public Class ResourcePageQueries
        Inherits QueriesBase
        Implements IResourcePageQueries

        Public Function Save_Resource(iMessage As Data.Resource) As Integer Implements IResourcePageQueries.Save_Resource
            Dim retVal As Boolean = False
            Dim MessageID As Integer = -1
            Try
                Using context As New MAISContext
                    If iMessage IsNot Nothing Then
                        If iMessage.Resource_Sid = -1 Then ' This is a new message 
                            Dim _newMessage As New Data.Resource
                            With _newMessage
                                .Subject = iMessage.Subject
                                .Description = iMessage.Description
                                .Priority = iMessage.Priority
                                .Active_Flg = iMessage.Active_Flg
                                .Create_By = UserID
                                .Create_Date = Now()
                                .Last_Update_By = UserID
                                .Last_Update_Date = Now()
                            End With
                            context.Resources.Add(_newMessage)
                            retVal = context.SaveChanges()
                            If retVal = True Then
                                MessageID = _newMessage.Resource_Sid
                            Else
                                MessageID = -1
                            End If

                        Else ' Need to update the data
                            Dim M As New Data.Resource
                            M = (From MM In context.Resources
                                 Where MM.Resource_Sid = iMessage.Resource_Sid
                                 Select MM).FirstOrDefault
                            With M
                                .Subject = iMessage.Subject
                                .Description = iMessage.Description
                                .Priority = iMessage.Priority
                                .Active_Flg = iMessage.Active_Flg
                                .Last_Update_Date = Now()
                                .Last_Update_By = UserID
                            End With


                            retVal = context.SaveChanges
                            If retVal = True Then
                                Return M.Resource_Sid
                            Else
                                Return -1
                            End If
                        End If


                    End If

                End Using

                Return MessageID

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while saving resource from the resource Query services.", True, False))
                Me.LogError("Error while saving resourcefrom the resourcee Query services.", CInt(Me.UserID), ex)
                MessageID = -1
                Return MessageID
            End Try

        End Function

        Public Function GetCurrentResource() As List(Of Data.Resource) Implements IResourcePageQueries.GetCurrentResource
            Dim retVal As New List(Of Data.Resource)
            Try
                Using context As New MAISContext
                    retVal = (From dm In context.Resources
                              Select dm).ToList

                End Using

                Return retVal
            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while pulling resource from the resource Query services.", True, False))
                Me.LogError("Error while pulling resourcefrom the resourcee Query services.", CInt(Me.UserID), ex)

                Return retVal
            End Try
        End Function

        Public Function GetResourceDataByResourceID(iMessageID As Integer) As Data.Resource Implements IResourcePageQueries.GetResourceDataByResourceID
            Dim retVal As New Data.Resource
            Try
                Using context As New MAISContext
                    retVal = (From dm In context.Resources
                            Where dm.Resource_Sid = iMessageID
                            Select dm).FirstOrDefault
                End Using

                Return retVal

            Catch ex As Exception
                Me._messages.Add(New ReturnMessage("Error while pulling resource by Message ID from the resource Query services.", True, False))
                Me.LogError("Error while pulling resource by Message ID from the resource Query services.", CInt(Me.UserID), ex)

                Return retVal
            End Try
        End Function

    End Class
End Namespace
