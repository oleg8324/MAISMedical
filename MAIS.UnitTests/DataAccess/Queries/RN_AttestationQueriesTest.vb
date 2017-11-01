Imports System.Collections.Generic

Imports MAIS.Data.Objects

Imports Microsoft.VisualStudio.TestTools.UnitTesting

Imports MAIS.Data.Queries



'''<summary>
'''This is a test class for RN_AttestationQueriesTest and is intended
'''to contain all RN_AttestationQueriesTest Unit Tests
'''</summary>
<TestClass()> _
Public Class RN_AttestationQueriesTest


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

    Friend Overridable Function CreateIAttestionQueries() As IRN_AttestationQueries
        Dim target As IRN_AttestationQueries = StructureMap.ObjectFactory.GetInstance(Of IRN_AttestationQueries)()
        Return target

    End Function
    '''<summary>
    '''A test for GetRN_AttestationQuestionForPage
    '''</summary>
    <TestMethod()> _
    Public Sub GetRN_AttestationQuestionForPageTestForRNINSTR_INT()
        Dim target As IRN_AttestationQueries = New RN_AttestationQueries() ' TODO: Initialize to an appropriate value
        Dim RoleID As Integer = 4 ' TODO: Initialize to an appropriate value
        Dim ApplicationTypeID As Integer = 1 ' TODO: Initialize to an appropriate value
        Dim expected As List(Of AttestationQuestions) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As List(Of AttestationQuestions)
        actual = target.GetRN_AttestationQuestionForPage(RoleID, ApplicationTypeID)
        Dim testVal As Boolean
        If actual.Count = 2 Then
            For Each i In actual
                Select Case i.Attestation_SID
                    Case 1, 8
                        testVal = True
                    Case Else
                        testVal = False
                        Exit For
                End Select
            Next
        Else
            testVal = False
        End If

        Assert.IsTrue(testVal)

        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetRN_AttestationQuestionForPage
    '''</summary>
    <TestMethod()> _
    Public Sub GetRN_AttestationQuestionForPageTestForRNI_RNWL()
        Dim target As IRN_AttestationQueries = New RN_AttestationQueries() ' TODO: Initialize to an appropriate value
        Dim RoleID As Integer = 4 ' TODO: Initialize to an appropriate value
        Dim ApplicationTypeID As Integer = 2 ' TODO: Initialize to an appropriate value
        Dim expected As List(Of AttestationQuestions) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As List(Of AttestationQuestions)
        actual = target.GetRN_AttestationQuestionForPage(RoleID, ApplicationTypeID)
        Dim testVal As Boolean
        If actual.Count = 1 Then
            For Each i In actual
                Select Case i.Attestation_SID
                    Case 1
                        testVal = True
                    Case Else
                        testVal = False
                        Exit For
                End Select
            Next
        Else
            testVal = False
        End If

        Assert.IsTrue(testVal)

        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetRN_AttestationQuestionForPage
    '''</summary>
    <TestMethod()> _
    Public Sub GetRN_AttestationQuestionForPageTestForRNT_INIT()
        Dim target As IRN_AttestationQueries = New RN_AttestationQueries() ' TODO: Initialize to an appropriate value
        Dim RoleID As Integer = 1 ' TODO: Initialize to an appropriate value
        Dim ApplicationTypeID As Integer = 1 ' TODO: Initialize to an appropriate value
        Dim expected As List(Of AttestationQuestions) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As List(Of AttestationQuestions)
        actual = target.GetRN_AttestationQuestionForPage(RoleID, ApplicationTypeID)
        Dim testVal As Boolean
        If actual.Count = 3 Then
            For Each i In actual
                Select Case i.Attestation_SID
                    Case 1, 7, 8
                        testVal = True
                    Case Else
                        testVal = False
                        Exit For
                End Select
            Next
        Else
            testVal = False
        End If

        Assert.IsTrue(testVal)

        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetRN_AttestationQuestionForPage
    '''</summary>
    <TestMethod()> _
    Public Sub GetRN_AttestationQuestionForPageTestForQARN_INT()
        Dim target As IRN_AttestationQueries = New RN_AttestationQueries() ' TODO: Initialize to an appropriate value
        Dim RoleID As Integer = 2 ' TODO: Initialize to an appropriate value
        Dim ApplicationTypeID As Integer = 1 ' TODO: Initialize to an appropriate value
        Dim expected As List(Of AttestationQuestions) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As List(Of AttestationQuestions)
        actual = target.GetRN_AttestationQuestionForPage(RoleID, ApplicationTypeID)
        Dim testVal As Boolean
        If actual.Count = 3 Then
            For Each i In actual
                Select Case i.Attestation_SID
                    Case 1, 2, 6
                        testVal = True
                    Case Else
                        testVal = False
                        Exit For
                End Select
            Next
        Else
            testVal = False
        End If

        Assert.IsTrue(testVal)

        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetRN_AttestationQuestionForPage
    '''</summary>
    <TestMethod()> _
    Public Sub GetRN_AttestationQuestionForPageTestFor17RN_INIT()
        Dim target As IRN_AttestationQueries = New RN_AttestationQueries() ' TODO: Initialize to an appropriate value
        Dim RoleID As Integer = 3 ' TODO: Initialize to an appropriate value
        Dim ApplicationTypeID As Integer = 1 ' TODO: Initialize to an appropriate value
        Dim expected As List(Of AttestationQuestions) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As List(Of AttestationQuestions)
        actual = target.GetRN_AttestationQuestionForPage(RoleID, ApplicationTypeID)
        Dim testVal As Boolean
        If actual.Count = 3 Then
            For Each i In actual
                Select Case i.Attestation_SID
                    Case 1, 7, 8
                        testVal = True
                    Case Else
                        testVal = False
                        Exit For
                End Select
            Next
        Else
            testVal = False
        End If

        Assert.IsTrue(testVal)

        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetRN_AttestationQuestionForPage
    '''</summary>
    <TestMethod()> _
    Public Sub GetRN_AttestationQuestionForPageTestForRNT_RNWL()
        Dim target As IRN_AttestationQueries = New RN_AttestationQueries() ' TODO: Initialize to an appropriate value
        Dim RoleID As Integer = 1 ' TODO: Initialize to an appropriate value
        Dim ApplicationTypeID As Integer = 2 ' TODO: Initialize to an appropriate value
        Dim expected As List(Of AttestationQuestions) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As List(Of AttestationQuestions)
        actual = target.GetRN_AttestationQuestionForPage(RoleID, ApplicationTypeID)
        Dim testVal As Boolean
        If actual.Count = 5 Then
            For Each i In actual
                Select Case i.Attestation_SID
                    Case 1, 2, 6, 8, 9
                        testVal = True
                    Case Else
                        testVal = False
                        Exit For
                End Select
            Next
        Else
            testVal = False
        End If

        Assert.IsTrue(testVal)

        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetRN_AttestationQuestionForPage
    '''</summary>
    <TestMethod()> _
    Public Sub GetRN_AttestationQuestionForPageTestFor17Rn_RNWL()
        Dim target As IRN_AttestationQueries = New RN_AttestationQueries() ' TODO: Initialize to an appropriate value
        Dim RoleID As Integer = 3 ' TODO: Initialize to an appropriate value
        Dim ApplicationTypeID As Integer = 2 ' TODO: Initialize to an appropriate value
        Dim expected As List(Of AttestationQuestions) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As List(Of AttestationQuestions)
        actual = target.GetRN_AttestationQuestionForPage(RoleID, ApplicationTypeID)
        Dim testVal As Boolean
        If actual.Count = 4 Then
            For Each i In actual
                Select Case i.Attestation_SID
                    Case 1, 2, 6, 9
                        testVal = True
                    Case Else
                        testVal = False
                        Exit For
                End Select
            Next
        Else
            testVal = False
        End If

        Assert.IsTrue(testVal)

        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetRN_AttestationQuestionForPage
    '''</summary>
    <TestMethod()> _
    Public Sub GetRN_AttestationQuestionForPageTestForRNMINSTR_ADDon()
        Dim target As IRN_AttestationQueries = New RN_AttestationQueries() ' TODO: Initialize to an appropriate value
        Dim RoleID As Integer = 5 ' TODO: Initialize to an appropriate value
        Dim ApplicationTypeID As Integer = 3 ' TODO: Initialize to an appropriate value
        Dim expected As List(Of AttestationQuestions) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As List(Of AttestationQuestions)
        actual = target.GetRN_AttestationQuestionForPage(RoleID, ApplicationTypeID)
        Dim testVal As Boolean
        If actual.Count = 1 Then
            For Each i In actual
                Select Case i.Attestation_SID
                    Case 1
                        testVal = True
                    Case Else
                        testVal = False
                        Exit For
                End Select
            Next
        Else
            testVal = False
        End If

        Assert.IsTrue(testVal)

        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetRN_AttestationQuestionForPage
    '''</summary>
    <TestMethod()> _
    Public Sub GetRN_AttestationQuestionForPageTestForDD_INIT()
        Dim target As IRN_AttestationQueries = New RN_AttestationQueries() ' TODO: Initialize to an appropriate value
        Dim RoleID As Integer = 7 ' TODO: Initialize to an appropriate value
        Dim ApplicationTypeID As Integer = 1 ' TODO: Initialize to an appropriate value
        Dim expected As List(Of AttestationQuestions) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As List(Of AttestationQuestions)
        actual = target.GetRN_AttestationQuestionForPage(RoleID, ApplicationTypeID)
        Dim testVal As Boolean
        If actual.Count = 5 Then
            For Each i In actual
                Select Case i.Attestation_SID
                    Case 1, 3, 4, 5, 7
                        testVal = True
                    Case Else
                        testVal = False
                        Exit For
                End Select
            Next
        Else
            testVal = False
        End If

        Assert.IsTrue(testVal)

        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetRN_AttestationQuestionForPage
    '''</summary>
    <TestMethod()> _
    Public Sub GetRN_AttestationQuestionForPageTestForDD_RNW()
        Dim target As IRN_AttestationQueries = New RN_AttestationQueries() ' TODO: Initialize to an appropriate value
        Dim RoleID As Integer = 7 ' TODO: Initialize to an appropriate value
        Dim ApplicationTypeID As Integer = 2 ' TODO: Initialize to an appropriate value
        Dim expected As List(Of AttestationQuestions) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As List(Of AttestationQuestions)
        actual = target.GetRN_AttestationQuestionForPage(RoleID, ApplicationTypeID)
        Dim testVal As Boolean
        If actual.Count = 2 Then
            For Each i In actual
                Select Case i.Attestation_SID
                    Case 1, 3
                        testVal = True
                    Case Else
                        testVal = False
                        Exit For
                End Select
            Next
        Else
            testVal = False
        End If

        Assert.IsTrue(testVal)

        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub

    '''<summary>
    '''A test for GetRN_AttestationQuestionForPage
    '''</summary>
    <TestMethod()> _
    Public Sub GetRN_AttestationQuestionForPageTestForDD_ADDon()
        Dim target As IRN_AttestationQueries = New RN_AttestationQueries() ' TODO: Initialize to an appropriate value
        Dim RoleID As Integer = 7 ' TODO: Initialize to an appropriate value
        Dim ApplicationTypeID As Integer = 3 ' TODO: Initialize to an appropriate value
        Dim expected As List(Of AttestationQuestions) = Nothing ' TODO: Initialize to an appropriate value
        Dim actual As List(Of AttestationQuestions)
        actual = target.GetRN_AttestationQuestionForPage(RoleID, ApplicationTypeID)
        Dim testVal As Boolean
        If actual.Count = 2 Then
            For Each i In actual
                Select Case i.Attestation_SID
                    Case 1, 3
                        testVal = True
                    Case Else
                        testVal = False
                        Exit For
                End Select
            Next
        Else
            testVal = False
        End If

        Assert.IsTrue(testVal)

        'Assert.AreEqual(expected, actual)
        'Assert.Inconclusive("Verify the correctness of this test method.")
    End Sub
End Class
