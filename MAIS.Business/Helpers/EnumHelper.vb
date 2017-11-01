Imports System
Imports System.ComponentModel
Imports System.Reflection

Namespace Helpers
    Public Class EnumHelper
        Public Shared Function GetEnumDescription(ByVal enumToPass As [Enum]) As String
            Dim enumType As Type = enumToPass.GetType()
            Dim enumDescription As DescriptionAttribute = DescriptionAttribute.Default
            Dim enumMembership As MemberInfo() = enumType.GetMember(enumToPass.ToString)

            Try
                If enumMembership IsNot Nothing And enumMembership.Length > 0 Then
                    Dim enumAttributes As Object() = enumMembership(0).GetCustomAttributes( _
                        GetType(DescriptionAttribute), True)

                    If enumAttributes IsNot Nothing And enumAttributes.Length > 0 Then
                        enumDescription = DirectCast(enumAttributes(0), DescriptionAttribute)
                    End If

                End If
            Catch ex As Exception
                'TODO: error handling for EnumHelper Class
            End Try

            Return enumDescription.Description
        End Function

    End Class
End Namespace
