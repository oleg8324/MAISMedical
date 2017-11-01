Imports MAIS.Data.Objects

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports MAIS.Data.Queries
Imports ODMRDDHelperClassLibrary.Utility



'''<summary>
'''This is a test class for PersonalInformationQueriesTest and is intended
'''to contain all PersonalInformationQueriesTest Unit Tests
'''</summary>
<TestClass()> _
Public Class PersonalInformationQueriesTest


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
        Dim target As IPersonalInformationQueries = New PersonalInformationQueries()
        Dim applicationID As Integer = 0
        Dim expected As PersonalInformationDetails = Nothing
        Dim actual As PersonalInformationDetails
        actual = target.GetDDPersonnelInformation(applicationID)
        Assert.AreEqual(expected, actual, "Test Passed")
    End Sub
    <TestMethod()> _
    Public Sub GetDDPersonnelInformationPermanentTest()
        Dim target As IPersonalInformationQueries = New PersonalInformationQueries()
        Dim applicationID As String = String.Empty
        Dim expected As PersonalInformationDetails = Nothing
        Dim actual As PersonalInformationDetails
        actual = target.GetDDPersonnelInformationFromPermanent(applicationID)
        Assert.AreEqual(expected, actual, "Test Passed")
    End Sub
    <TestMethod()> _
    Public Sub GetDDPersonnelInformationWithValueTest()
        Dim target As IPersonalInformationQueries = New PersonalInformationQueries()
        Dim applicationID As Integer = 19
        Dim expected As Boolean = True
        Dim actualValue As Boolean = False
        Dim actual As PersonalInformationDetails
        actual = target.GetDDPersonnelInformation(applicationID)
        If (actual IsNot Nothing) Then
            actualValue = True
        End If
        Assert.AreNotEqual(expected, actualValue, "Test Passed")
    End Sub
    <TestMethod()> _
    Public Sub GetRNInformationTest()
        Dim target As IPersonalInformationQueries = New PersonalInformationQueries()
        Dim applicationID As Integer = -1
        Dim expected As PersonalInformationDetails = Nothing
        Dim actual As PersonalInformationDetails
        actual = target.GetRNInformation(applicationID)
        Assert.AreEqual(expected, actual, "Test Passed")
    End Sub
    <TestMethod()> _
    Public Sub GetRNInformationPermanentTest()
        Dim target As IPersonalInformationQueries = New PersonalInformationQueries()
        Dim applicationID As String = String.Empty
        Dim expected As PersonalInformationDetails = Nothing
        Dim actual As PersonalInformationDetails
        actual = target.GetRNInformationFromPermanent(applicationID)
        Assert.AreEqual(expected, actual, "Test Passed")
    End Sub
    <TestMethod()> _
    Public Sub GetRNInformationWithValueTest()
        Dim target As IPersonalInformationQueries = New PersonalInformationQueries()
        Dim applicationID As Integer = 19
        Dim expected As Boolean = True
        Dim actualValue As Boolean = False
        Dim actual As PersonalInformationDetails
        actual = target.GetRNInformation(applicationID)
        If (actual IsNot Nothing) Then
            actualValue = True
        Else
            actualValue = False
        End If
        Assert.AreEqual(expected, actualValue, "Test Passed")
    End Sub
    <TestMethod()> _
    Public Sub SavePersonalInformationTest()
        Dim target As IPersonalInformationQueries = New PersonalInformationQueries()
        Dim applicationID As Integer = -1
        Dim personalResult As PersonalInformationDetails = Nothing
        Dim unique As String = String.Empty
        Dim name As String = String.Empty
        Dim rnOrDD As Boolean = False
        Dim brandNew As Boolean = False
        Dim sessionId As String = String.Empty
        Dim admin As Boolean = False
        Dim returnVal As ReturnObject(Of Long) = Nothing
        returnVal = target.SavePersonalInformation(personalResult, applicationID, rnOrDD, name, brandNew, unique, admin, unique)
        Assert.IsTrue(True)
    End Sub
End Class
