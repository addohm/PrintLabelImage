Imports System.IO

Public Class Logging

    ReadOnly _
        _userAppFolder As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                                                "Integra Optics", "Shipping Label")

    Dim _boxId As Integer = ArgFromCommandLine("boxID")

    '*************************************************************
    'NAME:          WriteToErrorLog
    'PURPOSE:       Open or create an error log and submit error message
    'PARAMETERS:    msg - message to be written to error file
    '               stkTrace - stack trace from error message
    '               title - title of the error file entry
    'RETURNS:       Nothing
    '*************************************************************
    Public Sub WriteToErrorLog(level As Integer, msg As String,
                               Optional ByVal stkTrace As String = "", Optional ByVal title As String = "")

        If Not Directory.Exists(_userAppFolder & "\Errors\") Then
            Directory.CreateDirectory(_userAppFolder & "\Errors\")
        End If

        Dim errLogPath As String = _userAppFolder & "\Errors\" & _boxId & " - ErrorLog.txt"

        'log it
        Using fs = New FileStream(errLogPath, FileMode.Append, FileAccess.Write)
            Using sw = New StreamWriter(fs)
                With sw
                    If level = 1 Then
                        .Write("Title: " & title & vbCrLf)
                        .Write("Message: " & msg & vbCrLf)
                        .Write("StackTrace: " & stkTrace & vbCrLf)
                        .Write("Date/Time: " & Date.Now.ToString() & vbCrLf)
                        .Write("================================================" & vbCrLf)
                    ElseIf level = 2 Then
                        .Write("Message: " & msg & vbCrLf)
                        .Write("Date/Time: " & Date.Now.ToString() & vbCrLf)
                        .Write("================================================" & vbCrLf)
                    End If
                End With
            End Using

        End Using
    End Sub

    '*************************************************************
    'NAME:          if verboseLogging = True then WriteToMessageLog
    'PURPOSE:       Open or create an message log and submit general message
    'PARAMETERS:    msg - message to be written to error file
    'RETURNS:       Nothing
    '*************************************************************
    Public Sub WriteToMessageLog(msg As String)

        'Check for and\or create the logs directory
        If Not Directory.Exists(_userAppFolder & "\Logs\") Then
            Directory.CreateDirectory(_userAppFolder & "\Logs\")
        End If

        'Set full log path
        Dim logPath As String = _userAppFolder & "\Logs\" & _boxId & " - RunLog.txt"

        'Delete files after 90 days
        Dim orderedFiles = New DirectoryInfo(_userAppFolder & "\Logs\").GetFiles().OrderBy(Function(x) x.CreationTime)
        For Each f As FileInfo In orderedFiles
            If (Now - f.CreationTime).Days > 90 Then f.Delete()
        Next

        'log it
        Using fs = New FileStream(logPath, FileMode.Append, FileAccess.Write)
            Using sw = New StreamWriter(fs)
                sw.Write(Date.Now.ToString() & " - Message: " & vbCrLf & msg & vbCrLf)
            End Using
        End Using
    End Sub

End Class