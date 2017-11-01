Imports MAIS.Business.Infrastructure

Public Class MAISTestConfiguration
    Public Shared Sub Initialize()
        StructureMap.ObjectFactory.Initialize(Sub(c)
                                                  c.Scan(Sub(s)
                                                             s.TheCallingAssembly()
                                                             s.Assembly("MAIS.Business")
                                                             s.Assembly("MAIS.Data")
                                                             s.Exclude(Function(r) r = GetType(IUserIdentity))
                                                             s.WithDefaultConventions()
                                                         End Sub)

                                                  c.For(Of IUserIdentity) _
                                                   .Use(Of UserIdentity)() _
                                                   .EnrichWith(Function(i) New UserIdentity() With
                                                                           {
                                                                               .UserID = 0
                                                                           })
                                              End Sub)
    End Sub
End Class
