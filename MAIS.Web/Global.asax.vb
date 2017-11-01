Imports System.Web.SessionState
Imports StructureMap
Imports MAIS.Business.Infrastructure

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ObjectFactory.Initialize(Sub(c)
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
                                                                  .UserID = MAIS_Helper.GetUserId()
                                                              })
                                 End Sub)

        System.Net.ServicePointManager.Expect100Continue = False
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
        'HttpContext.Current.Response.Cache.SetAllowResponseInBrowserHistory(False)
        'HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        'HttpContext.Current.Response.Cache.SetNoStore()
        'Response.Cache.SetExpires(DateTime.Now.AddSeconds(60))
        'Response.Cache.SetValidUntilExpires(True)
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when an error occurs
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
    End Sub
End Class