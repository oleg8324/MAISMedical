Imports MAIS.Business.Infrastructure
Imports ODMRDDHelperClassLibrary.Utility
Imports MAIS.Business.Model
Imports System.Data.Linq
Imports System.Configuration
Namespace Services
    Public Interface IMAISReportService
        Inherits IBusinessBase
        Function GetAllCourses() As List(Of Course_Info)
        Function GetAllCoursesByUniqueID(ByVal uniqueid As String) As List(Of Course_Info)
        Function GetDDPersonnelSkillsByUniqueID(ByVal uniqueID As String) As List(Of DDSkills_Info)
        Function GetAllCourseByRNID(ByVal rn_Sid As Integer) As List(Of Course_Info)
        Function GetSessionsByCourseID(ByVal c_id As Integer) As List(Of Course_Info)
        Function GetCertificationTypes() As List(Of ReportCertificationType)
        Function GetCertificationStaus() As List(Of ReportCertificationStatus)
        Function GetEmployersList(ByVal ID As Integer, ByVal EmpName As String, ByVal rblValue As String) As List(Of EmployerDetails)
        Function GetEmployersListByUniqueID(ByVal uniqueID As String) As List(Of EmployerDetails)
        Function GetSupervisorList(ByVal ID As Integer, ByVal FirstName As String, ByVal LastName As String, ByVal rblValue As String) As List(Of SupervisorDetails)
        Function GetSupervisorListByUniqueID(ByVal UniqueID As String) As List(Of SupervisorDetails)
        Function GetALLDDs() As List(Of MAIS_Report)
        Function GetALLRNs() As List(Of MAIS_Report)
        Function GetALLDDsForExcel() As List(Of MAISReportDetails)
        Function GetALLRNsForExcel() As List(Of MAISReportDetails)
        Function GetRNSearchReportForExcel(ByVal params As SearchParameters) As List(Of MAISReportDetails)
        Function GetDDSearchReportForExcel(ByVal params As SearchParameters) As List(Of MAISReportDetails)
        Function GetDDNotations(ByVal notationTypeId As Integer, ByVal notationReasonId As Integer, ByVal dateFrom As Date, ByVal dateTo As Date) As List(Of DDPersonnelSearchResult)
        Function GetRNNotations(ByVal notationTypeId As Integer, ByVal notationReasonId As Integer, ByVal dateFrom As Date, ByVal dateTo As Date) As List(Of RNSearchResult)
        Function GetRNSearchReport(ByVal params As SearchParameters) As List(Of MAIS_Report)
        Function GetDDSearchReport(ByVal params As SearchParameters) As List(Of MAIS_Report)
        Function GetCertificationHistory(ByVal uniqueID As String) As List(Of Cert_Info)
        Function GetDDRNCEUSRenewal(ByVal uniqueID As String) As List(Of CEUDetails)
    End Interface
    Public Class MAISReportService
        Inherits BusinessBase
        Implements IMAISReportService

        Private _queries As Data.Queries.IMAISReportQueries
        Public Sub New(ByVal user As IUserIdentity, ByVal connectionstring As IConnectionIdentity)
            _queries = StructureMap.ObjectFactory.GetInstance(Of Data.Queries.IMAISReportQueries)()
            _queries.UserID = user.UserID.ToString()
            _queries.MAISConnectionString = connectionstring.ConnectionString
        End Sub

        Public Function GetAllCourses() As List(Of Course_Info) Implements IMAISReportService.GetAllCourses
            Dim couLst As New List(Of Course_Info)
            Try
                couLst = Mapping.MAISReportMapping.CourseInfo(_queries.GetAllCourses)
            Catch ex As Exception
                Me.LogError("Error getting ALL Courses.", CInt(Me.UserID), ex)
            End Try
            Return couLst
        End Function
        Public Function GetRNSearchReport(ByVal params As SearchParameters) As List(Of MAIS_Report) Implements IMAISReportService.GetRNSearchReport
            Dim retList As New List(Of MAIS_Report)
            Try
                retList = Mapping.MAISReportMapping.MapRNList(_queries.GetRNSearchReport(Mapping.MAISReportMapping.MapParametersToDB(params)))
            Catch ex As Exception
                Me.LogError("Error getting ALL RN search results.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function
        Public Function GetDDSearchReport(ByVal params As SearchParameters) As List(Of MAIS_Report) Implements IMAISReportService.GetDDSearchReport
            Dim retList As New List(Of MAIS_Report)
            Try
                retList = Mapping.MAISReportMapping.MapDDList(_queries.GetDDSearchReport(Mapping.MAISReportMapping.MapParametersToDB(params)))
            Catch ex As Exception
                Me.LogError("Error getting ALL DD's search results.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function
        Public Function GetALLDDs() As List(Of MAIS_Report) Implements IMAISReportService.GetALLDDs
            Dim retList As New List(Of MAIS_Report)
            Try
                retList = Mapping.MAISReportMapping.MapDDList(_queries.GetALLDDs())
            Catch ex As Exception
                Me.LogError("Error getting ALL DD's.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetALLRNs() As List(Of MAIS_Report) Implements IMAISReportService.GetALLRNs
            Dim retList As New List(Of MAIS_Report)
            Try
                retList = Mapping.MAISReportMapping.MapRNList(_queries.GetALLRNs())
            Catch ex As Exception
                Me.LogError("Error getting All RN's.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function
        Public Function GetRNNotations(ByVal notationTypeId As Integer, ByVal notationReasonId As Integer, ByVal dateFrom As Date, ByVal dateTo As Date) As List(Of RNSearchResult) Implements IMAISReportService.GetRNNotations
            Dim retList As New List(Of RNSearchResult)
            Try
                retList = Mapping.MAISReportMapping.MapRNNotation(_queries.GetRNNotations(notationTypeId, notationReasonId, dateFrom, dateTo))
            Catch ex As Exception
                Me.LogError("Error getting RN's with notation list.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function
        Public Function GetDDNotations(ByVal notationTypeId As Integer, ByVal notationReasonId As Integer, ByVal dateFrom As Date, ByVal dateTo As Date) As List(Of DDPersonnelSearchResult) Implements IMAISReportService.GetDDNotations
            Dim retList As New List(Of DDPersonnelSearchResult)
            Try
                retList = Mapping.MAISReportMapping.MapDDNotation(_queries.GetDDNotations(notationTypeId, notationReasonId, dateFrom, dateTo))
            Catch ex As Exception
                Me.LogError("Error getting DD personnels with notation list.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetSupervisorList(ByVal ID As Integer, ByVal FirstName As String, ByVal LastName As String, ByVal rblValue As String) As List(Of SupervisorDetails) Implements IMAISReportService.GetSupervisorList
            Dim retList As New List(Of SupervisorDetails)
            Try
                retList = Mapping.MAISReportMapping.MapSupervisorList(_queries.GetSupervisorList(ID, FirstName, LastName, rblValue))
            Catch ex As Exception
                Me.LogError("Error getting Supervisor List.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function
        Public Function GetEmployersList(ByVal ID As Integer, ByVal empName As String, ByVal rblValue As String) As List(Of EmployerDetails) Implements IMAISReportService.GetEmployersList
            Dim retList As New List(Of EmployerDetails)
            Try
                retList = Mapping.MAISReportMapping.MapEmployerList(_queries.GetEmployersList(ID, empName, rblValue))
            Catch ex As Exception
                Me.LogError("Error getting Employer List.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function
        Public Function GetEmployersListByUniqueID(ByVal uniqueID As String) As List(Of EmployerDetails) Implements IMAISReportService.GetEmployersListByUniqueID
            Dim retList As New List(Of EmployerDetails)
            Try
                retList = Mapping.MAISReportMapping.MapEmployerList(_queries.GetEmployersListByUniqueID(uniqueID))
            Catch ex As Exception
                Me.LogError("Error getting Employer List by UniqueID.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function
        Public Function GetCertificationTypes() As List(Of ReportCertificationType) Implements IMAISReportService.GetCertificationTypes
            Dim retObj As New List(Of ReportCertificationType)
            Try
                retObj = Mapping.MAISReportMapping.MapTypes(_queries.GetCertificationTypes())
            Catch ex As Exception
                Me.LogError("Error getting Certification Types.", CInt(Me.UserID), ex)
            End Try
            Return retObj
        End Function
        Public Function GetCertificationStaus() As List(Of ReportCertificationStatus) Implements IMAISReportService.GetCertificationStaus
            Dim retObj As New List(Of ReportCertificationStatus)
            Try
                retObj = Mapping.MAISReportMapping.MapStatus(_queries.GetCertificationStaus())
            Catch ex As Exception
                Me.LogError("Error getting Certification Staus.", CInt(Me.UserID), ex)
            End Try
            Return retObj
        End Function

        Public Function GetSessionsByCourseID(ByVal c_sid As Integer) As List(Of Course_Info) Implements IMAISReportService.GetSessionsByCourseID
            Dim ses As New List(Of Course_Info)
            Try
                ses = Mapping.MAISReportMapping.CourseInfo(_queries.GetSessionsByCourseID(c_sid))
            Catch ex As Exception
                Me.LogError("Error getting sessions by course id.", CInt(Me.UserID), ex)
            End Try
            Return ses
        End Function

        Public Function GetAllCourseByRNID(rn_Sid As Integer) As List(Of Course_Info) Implements IMAISReportService.GetAllCourseByRNID
            Dim courselst As New List(Of Course_Info)
            Try
                courselst = Mapping.MAISReportMapping.CourseInfo(_queries.GetAllCourseByRNID(rn_Sid))
            Catch ex As Exception
                Me.LogError("Error getting sessions by course id.", CInt(Me.UserID), ex)
            End Try
            Return courselst
        End Function

        Public Function GetALLDDsForExcel() As List(Of MAISReportDetails) Implements IMAISReportService.GetALLDDsForExcel
            Dim retList As New List(Of MAISReportDetails)
            Try
                retList = Mapping.MAISReportMapping.MapDDStoreProcResultsForExcel(_queries.GetALLDDs())
            Catch ex As Exception
                Me.LogError("Error getting ALL DD's for Excel.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetALLRNsForExcel() As List(Of MAISReportDetails) Implements IMAISReportService.GetALLRNsForExcel
            Dim retList As New List(Of MAISReportDetails)
            Try
                retList = Mapping.MAISReportMapping.MapRNStoreProcResultsForExcel(_queries.GetALLRNs())
            Catch ex As Exception
                Me.LogError("Error getting ALL RN's for Excel.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetDDSearchReportForExcel(params As SearchParameters) As List(Of MAISReportDetails) Implements IMAISReportService.GetDDSearchReportForExcel
            Dim retList As New List(Of MAISReportDetails)
            Try
                retList = Mapping.MAISReportMapping.MapDDStoreProcResultsForExcel(_queries.GetDDSearchReport(Mapping.MAISReportMapping.MapParametersToDB(params)))
            Catch ex As Exception
                Me.LogError("Error getting search results for DD's in Excel.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetRNSearchReportForExcel(params As SearchParameters) As List(Of MAISReportDetails) Implements IMAISReportService.GetRNSearchReportForExcel
            Dim retList As New List(Of MAISReportDetails)
            Try
                retList = Mapping.MAISReportMapping.MapRNStoreProcResultsForExcel(_queries.GetRNSearchReport(Mapping.MAISReportMapping.MapParametersToDB(params)))
            Catch ex As Exception
                Me.LogError("Error getting search results for RN's in Excel.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetCertificationHistory(uniqueID As String) As List(Of Cert_Info) Implements IMAISReportService.GetCertificationHistory
            Dim retList As New List(Of Cert_Info)
            Try

                If Not String.IsNullOrWhiteSpace(uniqueID) Then
                    retList = Mapping.MAISReportMapping.CertInfo(_queries.GetCertificationHistory(uniqueID))
                End If
                '   retList = Mapping.MAISReportMapping.MapRNStoreProcResultsForExcel(_queries.GetRNSearchReport(Mapping.MAISReportMapping.MapParametersToDB(params)))
            Catch ex As Exception
                Me.LogError("Error getting certification history.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetSupervisorListByUniqueID(UniqueID As String) As List(Of SupervisorDetails) Implements IMAISReportService.GetSupervisorListByUniqueID
            Dim retList As New List(Of SupervisorDetails)
            Try
                retList = Mapping.MAISReportMapping.MapSupervisorList(_queries.GetSupervisorListByUniqueID(UniqueID))
            Catch ex As Exception
                Me.LogError("Error getting supervisor List by UniqueID.", CInt(Me.UserID), ex)
            End Try
            Return retList
        End Function

        Public Function GetAllCoursesByUniqueID(ByVal uniqueid As String) As List(Of Course_Info) Implements IMAISReportService.GetAllCoursesByUniqueID
            Dim couLst As New List(Of Course_Info)
            Try
                couLst = Mapping.MAISReportMapping.CourseInfo(_queries.GetAllCoursesByUniqueID(uniqueid))
            Catch ex As Exception
                Me.LogError("Error getting ALL Courses by uniqueid.", CInt(Me.UserID), ex)
            End Try
            Return couLst
        End Function

        Public Function GetDDPersonnelSkillsByUniqueID(uniqueID As String) As List(Of DDSkills_Info) Implements IMAISReportService.GetDDPersonnelSkillsByUniqueID
            Dim couLst As New List(Of DDSkills_Info)
            Try
                couLst = Mapping.MAISReportMapping.DDSkillsInfo(_queries.GetDDPersonnelSkillsByUniqueID(uniqueID))
            Catch ex As Exception
                Me.LogError("Error getting ALL Skills by uniqueid.", CInt(Me.UserID), ex)
            End Try
            Return couLst
        End Function

        Public Function GetDDRNCEUSRenewal(uniqueID As String) As List(Of CEUDetails) Implements IMAISReportService.GetDDRNCEUSRenewal
            Dim ceuslst As New List(Of CEUDetails)
            Try
                ceuslst = Mapping.CEUMapping.mapDBtoModelCEUDetails(_queries.GetDDRNCEUSRenewal(uniqueID))
            Catch ex As Exception
                Me.LogError("Error getting ALL ceus renewal by uniqueid.", CInt(Me.UserID), ex)
            End Try
            Return ceuslst
        End Function
    End Class
End Namespace