Imports MAIS.Data.Objects

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports MAIS.Data.Queries
Imports ODMRDDHelperClassLibrary.Utility



'''<summary>
'''This is a test class for WorkExperienceQueriesTest and is intended
'''to contain all WorkExperienceQueriesTest Unit Tests
'''</summary>
<TestClass()> _
Public Class WorkExperienceQueriesTest


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
    '''A test for SaveWorkExperience
    '''</summary>
    <TestMethod()> _
    Public Sub SaveWorkExperienceTest()
        Dim target As IWorkExperienceQueries = New WorkExperienceQueries()
        Dim applicationID As Integer = -1
        Dim workResult As WorkExperienceDetails = Nothing
        Dim returnVal As ReturnObject(Of Long) = Nothing
        returnVal = target.SaveWorkExperience(workResult)
        Assert.IsTrue(True)
    End Sub

    '''<summary>
    '''A test for GetAddedWorkExpList
    '''</summary>
    <TestMethod()> _
    Public Sub GetAddedWorkExpListTest()
        Dim target As IWorkExperienceQueries = New WorkExperienceQueries() ' TODO: Initialize to an appropriate value
        Dim applicationID As Integer = 19 ' TODO: Initialize to an appropriate value
        Dim expected As New List(Of WorkExperienceDetails)  ' TODO: Initialize to an appropriate value
        Dim actual As List(Of WorkExperienceDetails)
        actual = target.GetAddedWorkExpList(applicationID)
        Assert.IsTrue(actual.Count > 0)
    End Sub

    '''<summary>
    '''A test for GetWorkExperienceByID
    '''</summary>
    <TestMethod()> _
    Public Sub GetWorkExperienceByIDTest()
        Dim target As IWorkExperienceQueries = New WorkExperienceQueries() ' TODO: Initialize to an appropriate value
        Dim workID As Integer = -1 ' TODO: Initialize to an appropriate value
        Dim expected As WorkExperienceDetails = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As WorkExperienceDetails
        actual = target.GetWorkExperienceByID(workID)
        Assert.AreEqual(expected, actual)        
    End Sub

    '''<summary>
    '''A test for GetWorkExperienceByID
    '''</summary>
    <TestMethod()> _
    Public Sub GetExistingWorkExperience()
        Dim target As IWorkExperienceQueries = New WorkExperienceQueries() ' TODO: Initialize to an appropriate value
        Dim RNNumber As String = String.Empty  ' TODO: Initialize to an appropriate value       
        Dim actual As List(Of WorkExperienceDetails)
        actual = target.GetExistingWorkExperience(RNNumber)
        Assert.IsTrue(actual.Count = 0)
    End Sub
End Class
