' NOTE: You can use the "Rename" command on the context menu to change the interface name "IService1" in both code and config file together.
<ServiceContract>
Public Interface IMAIS_DODD_WCF

    <OperationContract()>
        <WebGet(UriTemplate:="/GetRNDataForMobile?lastname={lastname}&firstname={firstname}&RnlicenseNumber={RnlicenseNumber}", ResponseFormat:=WebMessageFormat.Json)> _
    Function GetRNData(ByVal lastName As String, ByVal firstname As String, ByVal RnlicenseNumber As String) As List(Of DTO.RNDetailInformation)

    <OperationContract()>
        <WebGet(UriTemplate:="/GetDDDataForMobile?DDpersonnelCode={DDpersonnelCode}&SSN={SSN}&lastname={lastname}&firstname={firstname}&EmployerName={EmployerName}", ResponseFormat:=WebMessageFormat.Json)> _
    Function GetDDData(ByVal DDpersonnelCode As String, ByVal SSN As String, ByVal lastname As String, ByVal firstname As String, ByVal EmployerName As String) As List(Of DTO.DDDetailInformation)

    <OperationContract()>
        <WebGet(UriTemplate:="/GetTrainingSessionDataForMobile?sessionStartDate={sessionStartDate}&sessionEndDate={sessionEndDate}&county={county}&lastName={lastName}&firstname={firstname}&rnsession={rnsession}", ResponseFormat:=WebMessageFormat.Json)> _
    Function GetTrainingSessionData(ByVal sessionStartDate As String, ByVal sessionEndDate As String, ByVal county As String, ByVal lastName As String, ByVal firstname As String, ByVal rnsession As String) As List(Of DTO.TrainingSessionResults)

    ' TODO: Add your service operations here

End Interface
