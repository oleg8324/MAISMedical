Imports MAIS.Data.Objects

Imports MAIS.Data.Queries

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports MAIS.Data



'''<summary>
'''This is a test class for StartPageQueriesTest and is intended
'''to contain all StartPageQueriesTest Unit Tests
'''</summary>
<TestClass()> _
Public Class StartPageQueriesTest


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
    '''A test for GetCertificationDetails
    '''</summary>
    <TestMethod()> _
    Public Sub GetCertificationDetailsTest()
        Dim target As IStartPageQueries = New StartPageQueries()
        Dim role As Integer = 0
        Dim currentRole As Integer = 0
        Dim applicationType As String = "Initial"
        Dim expected As CertificationEligibleInfo = Nothing
        Dim actual As CertificationEligibleInfo
        actual = target.GetCertificationDetails(role, currentRole, applicationType)
        Assert.AreEqual(expected, actual)
    End Sub
    <TestMethod()> _
    Public Sub GetCertificationDetailsInitialTest()
        Dim target As IStartPageQueries = New StartPageQueries()
        Dim role As Integer = 4
        Dim actualVal As Boolean = False
        Dim currentRole As Integer = 4
        Dim applicationType As String = "Initial"
        Dim expected As CertificationEligibleInfo = Nothing
        Dim actual As CertificationEligibleInfo
        actual = target.GetCertificationDetails(role, currentRole, applicationType)
        If (actual IsNot Nothing) Then
            actualVal = True
        End If
        Assert.IsTrue(actualVal)
    End Sub

    <TestMethod()> _
    Public Sub GetCertificationDetailsAddOnTest()
        Dim target As IStartPageQueries = New StartPageQueries()
        Dim role As Integer = 4
        Dim currentRole As Integer = 7
        Dim actualVal As Boolean = False
        Dim applicationType As String = "AddOn"
        Dim expected As CertificationEligibleInfo = Nothing
        Dim actual As CertificationEligibleInfo
        actual = target.GetCertificationDetails(role, currentRole, applicationType)
        If (actual IsNot Nothing) Then
            actualVal = True
        End If
        Assert.IsTrue(actualVal)
    End Sub
    <TestMethod()> _
    Public Sub GetCertificationDetailsRenewalTest()
        Dim target As IStartPageQueries = New StartPageQueries()
        Dim role As Integer = 4
        Dim actualVal As Boolean = False
        Dim currentRole As Integer = 4
        Dim applicationType As String = "Renewal"
        Dim expected As CertificationEligibleInfo = Nothing
        Dim actual As CertificationEligibleInfo
        actual = target.GetCertificationDetails(role, currentRole, applicationType)
        If (actual IsNot Nothing) Then
            actualVal = True
        End If
        Assert.IsTrue(actualVal)
    End Sub
    '''<summary>
    '''A test for GetApplicationInformation
    '''</summary>
    <TestMethod()> _
    Public Sub GetApplicationInformationTest()
        Dim target As IStartPageQueries = New StartPageQueries()
        Dim applicationID As Integer = 0
        Dim applicationType As String = String.Empty
        Dim expected As ApplicationInformation = Nothing
        Dim actual As ApplicationInformation
        actual = target.GetApplicationInformation(applicationID)
        Assert.AreEqual(expected, actual)
    End Sub
    <TestMethod()> _
    Public Sub GetApplicationInformationWithDataTest()
        Dim target As IStartPageQueries = New StartPageQueries()
        Dim applicationID As Integer = 19
        Dim expected As ApplicationInformation = Nothing
        Dim actual As ApplicationInformation
        actual = target.GetApplicationInformation(applicationID)
        Assert.AreNotEqual(expected, actual)
    End Sub

    '''<summary>
    '''A test for SaveAppInfo
    '''</summary>
    <TestMethod()> _
    Public Sub SaveAppInfoTest()
        Dim target As IStartPageQueries = New StartPageQueries()
        Dim appDetails As ApplicationInformation = Nothing
        Dim expected As Integer = 0
        Dim actual As Integer
        actual = target.SaveAppInfo(appDetails)
        Assert.AreEqual(expected, actual)
    End Sub
End Class
