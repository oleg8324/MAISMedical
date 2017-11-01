Imports ODMRDDHelperClassLibrary.Utility

Imports System.Collections.Generic

Imports MAIS.Data.Objects

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports MAIS.Data.Queries



'''<summary>
'''This is a test class for EmployerInformationQueriesTest and is intended
'''to contain all EmployerInformationQueriesTest Unit Tests
'''</summary>
<TestClass()> _
Public Class EmployerInformationQueriesTest


    Private testContextInstance As TestContext

    '''<summary>
    '''Gets or sets the test context which provides
    '''information about and functionality for the current test run.
    '''</summary>
    Public Property TestContext() As TestContext
        Get
            Return testContextInstance
        End Get
        Set(value As TestContext)
            testContextInstance = Value
        End Set
    End Property

#Region "Additional test attributes"
    '
    'You can use the following additional attributes as you write your tests:
    '
    'Use ClassInitialize to run code before running the first test in the class
    '<ClassInitialize()>  _
    'Public Shared Sub MyClassInitialize(ByVal testContext As TestContext)
    'End Sub
    '
    'Use ClassCleanup to run code after all tests in a class have run
    '<ClassCleanup()>  _
    'Public Shared Sub MyClassCleanup()
    'End Sub
    '
    'Use TestInitialize to run code before running each test
    '<TestInitialize()>  _
    'Public Sub MyTestInitialize()
    'End Sub
    '
    'Use TestCleanup to run code after each test has run
    '<TestCleanup()>  _
    'Public Sub MyTestCleanup()
    'End Sub
    '
#End Region


    '''<summary>
    '''A test for GetRecentlyAddedEmplyeInfo
    '''</summary>
    <TestMethod()> _
    Public Sub GetRecentlyAddedEmplyeInfoTest()
        Dim target As IEmployerInformationQueries = New EmployerInformationQueries()
        Dim applicationID As Integer = 19
        Dim expected As List(Of EmployerInformationDetails) = Nothing
        Dim actual As List(Of EmployerInformationDetails)
        actual = target.GetRecentlyAddedEmplyeInfo(applicationID)
        Dim actaulVal As Boolean = False
        If (actual.Count > 0) Then
            actaulVal = True
        End If
        Assert.IsTrue(actaulVal)
    End Sub
    <TestMethod()> _
    Public Sub GetRecentlyAddedEmplyeInfoWithOutValueTest()
        Dim target As IEmployerInformationQueries = New EmployerInformationQueries()
        Dim applicationID As Integer = 0
        Dim expected As List(Of EmployerInformationDetails) = Nothing
        Dim actual As List(Of EmployerInformationDetails)
        actual = target.GetRecentlyAddedEmplyeInfo(applicationID)
        If (actual.Count = 0) Then
            actual = Nothing
        End If
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>
    '''A test for GetDataRecentlyAddedEmplyerInfo
    '''</summary>
    <TestMethod()> _
    Public Sub GetDataRecentlyAddedEmplyerInfoTest()
        Dim target As IEmployerInformationQueries = New EmployerInformationQueries()
        Dim employerID As Integer = 0
        Dim expected As EmployerInformationDetails = Nothing
        Dim actual As EmployerInformationDetails
        actual = target.GetDataRecentlyAddedEmplyerInfo(employerID)
        Assert.AreEqual(expected, actual)
    End Sub
    '''<summary>
    '''A test for GetDataRecentlyAddedEmplyerInfoWithDataTest
    '''</summary>
    <TestMethod()> _
    Public Sub GetDataRecentlyAddedEmplyerInfoWithDataTest()
        Dim target As IEmployerInformationQueries = New EmployerInformationQueries()
        Dim employerID As Integer = 1
        Dim expected As EmployerInformationDetails = Nothing
        Dim actual As EmployerInformationDetails
        actual = target.GetDataRecentlyAddedEmplyerInfo(employerID)
        Dim actualValue As Boolean = False
        If (actual.EmployerID = employerID) Then
            actualValue = True
        End If
        Assert.IsTrue(actualValue)
    End Sub

    '''<summary>
    '''A test for GetDataRecentlyAddedEmplyerInfoWithDataEmpTest
    '''</summary>
    <TestMethod()> _
    Public Sub GetDataRecentlyAddedEmplyerInfoWithDataEmpTest()
        Dim target As IEmployerInformationQueries = New EmployerInformationQueries()
        Dim employerID As Integer = 2
        Dim expected As EmployerInformationDetails = Nothing
        Dim actual As EmployerInformationDetails
        actual = target.GetDataRecentlyAddedEmplyerInfo(employerID)
        Dim actualValue As Boolean = False
        If (actual.EmployerID = employerID) Then
            actualValue = True
        End If
        Assert.IsTrue(actualValue)
    End Sub

    '''<summary>
    '''A test for SaveEmployerInformation
    '''</summary>
    <TestMethod()> _
    Public Sub SaveEmployerInformationTest()
        Dim target As IEmployerInformationQueries = New EmployerInformationQueries()
        Dim employerInfo As EmployerInformationDetails = Nothing
        Dim applicationID As Integer = 0
        Dim employerId As Integer = 0
        Dim expected As ReturnObject(Of Long) = Nothing
        Dim actual As ReturnObject(Of Long)
        actual = target.SaveEmployerInformation(employerInfo, applicationID, employerId, False, False)
        If (actual.ReturnValue = -1) Then
            actual = Nothing
        End If
        Assert.AreEqual(expected, actual)
    End Sub
End Class
