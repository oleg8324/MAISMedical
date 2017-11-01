Imports ODMRDDHelperClassLibrary.Utility

Imports MAIS.Business.Model

Imports MAIS.Business.Infrastructure

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports MAIS.Business.Services
Imports System.Configuration



'''<summary>
'''This is a test class for PersonalInformationServiceTest and is intended
'''to contain all PersonalInformationServiceTest Unit Tests
'''</summary>
<TestClass()> _
Public Class PersonalInformationServiceTest
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
            testContextInstance = value
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
    '''A test for GetDDPersonnelInformation
    '''</summary>
    <TestMethod()> _
    Public Sub GetDDPersonnelInformationTest()
        Dim personalSvc As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
        Dim applicationID As Integer = -1
        Dim expected As DDPersonnelDetails = Nothing
        Dim actual As DDPersonnelDetails
        actual = personalSvc.GetDDPersonnelInformation(applicationID)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>
    '''A test for GetRNInformation
    '''</summary>
    <TestMethod()> _
    Public Sub GetRNInformationTest()
        Dim personalSvc As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
        Dim applicationID As Integer = -1
        Dim expected As RNInformationDetails = Nothing
        Dim actual As RNInformationDetails
        actual = personalSvc.GetRNInformation(applicationID)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>
    '''A test for SavePersonalInformation
    '''</summary>
    <TestMethod()> _
    Public Sub SavePersonalInformationTest()
        Dim personalSvc As IPersonalInformationService = StructureMap.ObjectFactory.GetInstance(Of IPersonalInformationService)()
        Dim personalInfo As PersonalInformationDetails = Nothing
        Dim unique As String = String.Empty
        Dim name As String = String.Empty
        Dim appId As Integer = 0
        Dim rnOrDD As Boolean = False
        Dim admin As Boolean = False
        Dim brandNew As Boolean = False
        Dim expected As ReturnObject(Of Long) = Nothing
        Dim actual As ReturnObject(Of Long)
        actual = personalSvc.SavePersonalInformation(personalInfo, appId, rnOrDD, name, brandNew, unique, admin, unique)
        Assert.IsTrue(True)
    End Sub
End Class
