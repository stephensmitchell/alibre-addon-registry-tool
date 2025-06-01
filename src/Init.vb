Option Strict On
Option Explicit On
Imports System.Runtime.Versioning
Imports Microsoft.Win32
Imports System.Reflection
<SupportedOSPlatform("windows")>
Module Init
    Private Const RegPath As String = "SOFTWARE\Alibre Design Add-Ons"
    Private ReadOnly Hklm As RegistryKey = Registry.LocalMachine
    Sub Main()
        PrintBanner()
        Try
            Using key As RegistryKey = Hklm.OpenSubKey(RegPath, writable:=False)
                If key Is Nothing Then
                    Console.Error.WriteLine($"Error: Registry key not found – HKLM\{RegPath}")
                    Environment.Exit(1)
                End If
                Dim names As String() = key.GetValueNames()
                If names.Length = 0 Then
                    Console.WriteLine("(No values present)")
                    Return
                End If
                Dim maxNameLen As Integer = names.Max(Function(n) n.Length)
                Dim fmt As String = $"  {{0,-{maxNameLen}}} : {{1}}"
                For Each name In names
                    Dim valueObj = key.GetValue(name)
                    Dim valueStr As String = If(valueObj Is Nothing, "<null>", valueObj.ToString())
                    Console.WriteLine(String.Format(fmt, name, valueStr))
                Next
            End Using
        Catch ex As Exception
            Console.Error.WriteLine($"Unhandled error: {ex.Message}")
            Environment.Exit(2)
        End Try
    End Sub
    Private Sub PrintBanner()
        Dim asm = Assembly.GetExecutingAssembly().GetName()
        Console.WriteLine("Get-Installed-Addons – Alibre Design Add-Ons Registry Information")
        Console.WriteLine($"Version {asm.Version}")
        Console.WriteLine("Target key : HKLM\" & RegPath)
        Console.WriteLine(New String("-"c, 60))
    End Sub
End Module