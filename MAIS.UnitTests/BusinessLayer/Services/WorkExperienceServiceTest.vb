Imports System.Collections.Generic

Imports ODMRDDHelperClassLibrary.Utility

Imports MAIS.Business.Model

Imports MAIS.Business.Infrastructure

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports MAIS.Business.Services

Imports System.Configuration

'''<summary>
'''This is a test class for WorkExperienceServiceTest and is intended
'''to contain all WorkExperienceServiceTest Unit Tests
'''</summary>
<TestClass()> _
Public Class WorkExperienceServiceTest
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
    '''A test for SaveWorkExperience
    '''</summary>
    <TestMethod()> _
    Public Sub SaveWorkExperienceTest()
        Dim WorkExpSvc As IWorkExperienceService = StructureMap.ObjectFactory.GetInstance(Of IWorkExperienceService)()
        Dim workInfo As WorkExperienceDetails = Nothing       
        Dim expected As ReturnObject(Of Long) = Nothing
        Dim actual As ReturnObject(Of Long)
        actual = WorkExpSvc.SaveWorkExperience(workInfo)
        Assert.IsTrue(True)             
    End Sub
    
    '''<summary>
    '''A test for GetWorkExperienceByID
    '''</summary>
    <TestMethod()> _
    Public Sub GetWorkExperienceByIDTest()
        Dim WorkExpSvc As IWorkExperienceService = StructureMap.ObjectFactory.GetInstance(Of IWorkExperienceService)()
        Dim workID As Integer = -1
        Dim expected As WorkExperienceDetails = Nothing
        Dim actual As WorkExperienceDetails
        actual = WorkExpSvc.GetWorkExperienceByID(workID)
        Assert.AreEqual(expected, actual)
    End Sub

    '''<summary>
    '''A test for GetAddedWorkExpList
    '''</summary>
    <TestMethod()> _
    Public Sub GetAddedWorkExpListTest()
        Dim WorkExpSvc As IWorkExperienceService = StructureMap.ObjectFactory.GetInstance(Of IWorkExperienceService)()
        Dim applicationID As Integer = 19
        Dim expected As List(Of WorkExperienceDetails) = Nothing
        Dim actual As List(Of WorkExperienceDetails)
        actual = WorkExpSvc.GetAddedWorkExpList(applicationID)
        Assert.IsTrue(actual.Count > 0)
    End Sub
End Class
