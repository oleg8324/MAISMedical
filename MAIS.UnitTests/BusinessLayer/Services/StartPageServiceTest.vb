Imports MAIS.Business.Model

Imports MAIS.Business.Services

Imports MAIS.Business.Infrastructure

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports MAIS.Business



'''<summary>
'''This is a test class for StartPageServiceTest and is intended
'''to contain all StartPageServiceTest Unit Tests
'''</summary>
<TestClass()> _
Public Class StartPageServiceTest
    <TestInitialize()>
    Public Sub Initialize()
        MAISTestConfiguration.Initialize()
    End Sub

    Private testContextInstance As TestContext
    Dim startPageSvc As IStartPageService = StructureMap.ObjectFactory.GetInstance(Of IStartPageService)()
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
    '''A test for GetCertificationDetails
    '''</summary>
    <TestMethod()> _
    Public Sub GetCertificationDetailsTest()
        Dim role As Integer = 0
        Dim currentRole As Integer = 0
        Dim applicationType As String = String.Empty
        Dim expected As CertificationEligibleInfo = Nothing
        Dim actual As CertificationEligibleInfo
        actual = startPageSvc.GetCertificationDetails(role, currentRole, applicationType)
        Assert.AreNotEqual(expected, actual)
    End Sub

    '''<summary>
    '''A test for SaveAppInfo
    '''</summary>
    <TestMethod()> _
    Public Sub SaveAppInfoTest()
        Dim appDetails As MAISApplicationDetails = Nothing
        Dim expected As Integer = 0
        Dim actual As Integer
        actual = startPageSvc.SaveAppInfo(appDetails)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>
    '''A test for GetApplicationInformation
    '''</summary>
    <TestMethod()> _
    Public Sub GetApplicationInformationTest()
        Dim applicationID As Integer = 0
        Dim expected As MAISApplicationDetails = Nothing
        Dim actual As MAISApplicationDetails
        actual = startPageSvc.GetApplicationInformation(applicationID)
        Assert.AreNotEqual(expected, actual)
    End Sub
End Class
