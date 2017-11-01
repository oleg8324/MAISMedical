Imports ODMRDDHelperClassLibrary.Utility

Imports System.Collections.Generic

Imports MAIS.Business.Model

Imports MAIS.Business.Infrastructure

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports MAIS.Business.Services



'''<summary>
'''This is a test class for EmployerInformationServiceTest and is intended
'''to contain all EmployerInformationServiceTest Unit Tests
'''</summary>
<TestClass()> _
Public Class EmployerInformationServiceTest
    <TestInitialize()>
    Public Sub Initialize()
        MAISTestConfiguration.Initialize()
    End Sub

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
    '''A test for GetRecentlyAddedEmplyerInfo
    '''</summary>
    <TestMethod()> _
    Public Sub GetRecentlyAddedEmplyerInfoTest()
        Dim empSvc As IEmployerInformationService = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
        Dim applicationID As Integer = -1
        Dim expected As List(Of EmployerInformationDetails) = Nothing
        Dim actual As List(Of EmployerInformationDetails)
        actual = empSvc.GetRecentlyAddedEmplyerInfo(applicationID)
        If (actual.Count = 0) Then
            actual = Nothing
        End If
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>
    '''A test for SaveEmployerInformation
    '''</summary>
    <TestMethod()> _
    Public Sub SaveEmployerInformationTest()
        Dim empSvc As IEmployerInformationService = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
        Dim employerInfo As EmployerInformationDetails = Nothing
        Dim applicationID As Integer = -1
        Dim employerId As Integer = 0
        Dim expected As ReturnObject(Of Long) = Nothing
        Dim actual As ReturnObject(Of Long)
        actual = empSvc.SaveEmployerInformation(employerInfo, applicationID, employerId, False, False)
        If (actual.ReturnValue = -1) Then
            actual = Nothing
        End If
        Assert.AreEqual(expected, actual)
    End Sub
    '''<summary>
    '''A test for GetRecentlyAddedEmplyerInfoWithDataTest
    '''</summary>
    <TestMethod()> _
    Public Sub GetRecentlyAddedEmplyerInfoWithDataTest()
        Dim empSvc As IEmployerInformationService = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
        Dim applicationID As Integer = 19
        Dim expected As List(Of EmployerInformationDetails) = Nothing
        Dim actual As List(Of EmployerInformationDetails)
        actual = empSvc.GetRecentlyAddedEmplyerInfo(applicationID)
        Dim actualBool As Boolean = False
        If (actual.Count > 0) Then
            actualBool = True
        End If
        Assert.IsTrue(actualBool)
    End Sub

    '''<summary>
    '''A test for GetDataRecentlyAddedEmplyerInfo
    '''</summary>
    <TestMethod()> _
    Public Sub GetDataRecentlyAddedEmplyerInfoTest()
        Dim empSvc As IEmployerInformationService = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
        Dim employerID As Integer = -1
        Dim expected As EmployerInformationDetails = Nothing
        Dim actual As EmployerInformationDetails
        actual = empSvc.GetDataRecentlyAddedEmplyerInfo(employerID)
        If (actual.EmployerID = 0) Then
            actual = Nothing
        End If
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>
    '''A test for GetDataRecentlyAddedEmplyerInfoWithDataTest
    '''</summary>
    <TestMethod()> _
    Public Sub GetDataRecentlyAddedEmplyerInfoWithDataTest()
        Dim empSvc As IEmployerInformationService = StructureMap.ObjectFactory.GetInstance(Of IEmployerInformationService)()
        Dim employerID As Integer = 2
        Dim expected As EmployerInformationDetails = Nothing
        Dim actual As EmployerInformationDetails
        actual = empSvc.GetDataRecentlyAddedEmplyerInfo(employerID)
        Dim actualBool As Boolean = False
        If (actual.EmployerID > 0) Then
            actualBool = True
        End If
        Assert.IsTrue(actualBool)
    End Sub
End Class
