Imports System.Data.SqlClient
Imports System.IO

Module Main

    ''' <summary>
    '''     Parse command line arguments
    ''' </summary>
    Function ArgFromCommandLine(argName As String) As String

        Dim inputArg = $"/{argName}="
        Dim inputVal = ""

        For Each s As String In My.Application.CommandLineArgs
            If s.ToLower.StartsWith(inputArg.ToLower) Then
                inputVal = s.Remove(0, inputArg.Length)
            End If

        Next

        If inputVal = "" Then
            inputVal = "0"
        End If

        ArgFromCommandLine = inputVal
    End Function

    ''' <summary>
    '''     Gets an image from the database and saves it to a file(s)
    ''' </summary>
    Private Function WriteImageFromDb(printLocation As String, serialNo As String) As String
        WriteImageFromDb = ""

        Dim imagePath
        ' Writes the BLOB to a file (*.bmp).
        ' Streams the binary data to the FileStream object.
        ' The bytes returned from GetBytes.
        Dim retval As Long
        ' The starting position in the BLOB output.
        Dim startIndex As Long
        Dim labelType As String
        If printLocation = 1 Or printLocation = 2 Then
            labelType = "O"
        ElseIf printLocation = 3 Then
            labelType = "C"
        ElseIf printLocation = 4 Or printLocation = 5 Then
            labelType = "S"
        Else
            labelType = String.Empty
            MsgBox("Labeltype argument missing!")
        End If
        Try
            Using connection = New SqlConnection()
                connection.ConnectionString = My.Settings.ConnStr
                connection.Open() 'open connection
                'sql command specific for the label type
                Using cmd As New SqlCommand With {
                    .Connection = connection,
                    .CommandText = "select label_image from aof_labels " _
                                 & "where label_type = '" & labelType & "'" _
                                 & "And SO_LINE_NUMBER = ( " _
                                                        & "SELECT TOP 1 SO_LINE_NUMBER FROM AOF_ORDER_OPTICS " _
                                                        & "where SERIAL_NUMBER = '" & serialNo & "' " _
                                                        & ")"
                    }

                    Using cmdReader As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SequentialAccess)
                        If Not cmdReader.Read() = False Then

                            'Set the full file path for the image
                            imagePath = Path.GetTempPath & "Integra" & labelType & "Label.png"

                            'if the file already exists, delete and create a new one
                            If File.Exists(imagePath) Then
                                File.Delete(imagePath)
                            End If

                            ' Create a file to hold the output.
                            Using stream = New FileStream(imagePath, FileMode.OpenOrCreate, FileAccess.Write)
                                Using writer = New BinaryWriter(stream)
                                    ' The size of the BLOB buffer.
                                    Dim bufferSize = 100
                                    ' The BLOB byte() buffer to be filled by GetBytes.
                                    Dim outByte(bufferSize - 1) As Byte
                                    ' Reset the starting byte for a new BLOB.
                                    startIndex = 0

                                    ' Read bytes into outByte() and retain the number of bytes returned.
                                    retval = cmdReader.GetBytes(0, startIndex, outByte, 0, bufferSize)

                                    ' Continue while there are bytes beyond the size of the buffer.
                                    Do While retval = bufferSize
                                        writer.Write(outByte)
                                        writer.Flush()

                                        ' Reposition start index to end of the last buffer and fill buffer.
                                        startIndex += bufferSize
                                        retval = cmdReader.GetBytes(0, startIndex, outByte, 0, bufferSize)
                                    Loop
                                    ' Write the remaining buffer.
                                    writer.Write(outByte, 0, retval)
                                    writer.Flush()
                                    writer.Close()
                                    WriteImageFromDb = imagePath
                                End Using
                                stream.Close()
                            End Using
                        Else
                            MsgBox("Label image data query returned no results!")
                            Environment.Exit(0)
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            MsgBox("Error in writing an image file from the database: " & ex.Message)
        End Try
    End Function

    ''' <summary>
    '''     Gets the printer settings from the database and writes it to the user settings
    ''' </summary>
    Friend Sub ReadSettingsFromDBtoSettings(location As Integer)
        Using connection = New SqlConnection()
            Using cmd As New SqlCommand()
                connection.ConnectionString = My.Settings.ConnStr
                With cmd
                    .Connection = connection
                    connection.Open()
                    If location = 1 Then
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_XFPh'"
                        My.Settings.XFP_Horizontal = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_XFPv'"
                        My.Settings.XFP_Vertical = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_XFPs'"
                        My.Settings.XFP_Size = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_XFPr'"
                        My.Settings.XFP_Rotation = Convert.ToInt32(.ExecuteScalar)
                    ElseIf location = 2 Then
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_SFPh'"
                        My.Settings.SFP_Horizontal = Convert.ToDecimal(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_SFPv'"
                        My.Settings.SFP_Vertical = Convert.ToDecimal(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_SFPs'"
                        My.Settings.SFP_Size = Convert.ToDecimal(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_SFPr'"
                        My.Settings.SFP_Rotation = Convert.ToInt32(.ExecuteScalar)
                    ElseIf location = 3 Then
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_Clamshellh'"
                        My.Settings.Clamshell_Horizontal = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_Clamshellv'"
                        My.Settings.Clamshell_Vertical = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_Clamshells'"
                        My.Settings.Clamshell_Size = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_Clamshellr'"
                        My.Settings.Clamshell_Rotation = Convert.ToInt32(.ExecuteScalar)
                    ElseIf location = 4 Then
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_ShippinghA'"
                        My.Settings.ShippingA_Horizontal = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_ShippingvA'"
                        My.Settings.ShippingA_Vertical = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_ShippingsA'"
                        My.Settings.ShippingA_Size = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_ShippingrA'"
                        My.Settings.ShippingA_Rotation = Convert.ToInt32(.ExecuteScalar)
                    ElseIf location = 5 Then
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_ShippinghM'"
                        My.Settings.ShippingM_Horizontal = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_ShippingvM'"
                        My.Settings.ShippingM_Vertical = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_ShippingsM'"
                        My.Settings.ShippingM_Size = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_ShippingrM'"
                        My.Settings.ShippingM_Rotation = Convert.ToInt32(.ExecuteScalar)
                    End If
                End With
            End Using
        End Using
        My.Settings.Save()
    End Sub

    Sub Main()
        Dim setupMode As Boolean = Convert.ToBoolean(Convert.ToInt32(ArgFromCommandLine("setup")))
        Dim serialNo As String = ArgFromCommandLine("serial")
        Dim printLocation As Integer = Convert.ToInt32(ArgFromCommandLine("location"))
        Dim statusQuery As Boolean = Convert.ToBoolean(Convert.ToInt32(ArgFromCommandLine("status")))
        Dim printEnable As Boolean = Convert.ToBoolean(Convert.ToInt32(ArgFromCommandLine("print")))
        Dim cb As New SqlConnectionStringBuilder With {
            .DataSource = My.Settings.sqlServer,
            .InitialCatalog = My.Settings.sqlDBName,
            .UserID = My.Settings.sqlUsername,
            .Password = My.Settings.sqlPassword
         }

        With My.Settings
            .ConnStr = cb.ToString()
            .Save()
        End With

        'Open setup dialoge
        If setupMode = True Then
            Dim printSettingsForm As New FormSettingsPrinter
            printSettingsForm.location = printLocation
            'load all txtboxes with data from db before the form load
            printSettingsForm.UpdateFormFields()
            'load settings form
            printSettingsForm.ShowDialog()
        End If

        ReadSettingsFromDBtoSettings(printLocation)

        'Attain and print label at the defined location using the saved settings
        If setupMode = False Then
            'Save the image and return the file path
            Dim imagePath As String = WriteImageFromDb(printLocation, serialNo)

            Select Case printLocation
                'Sato printers
                Case 1, 3, 4
                    With My.Settings
                        Select Case printLocation
                            Case 1
                                SatoUtilities.ThisSessionAddr = .XFP_PrinterAddr
                                SatoPrintJob.ThisSessionAddr = .XFP_PrinterAddr
                                SatoPrintJob.ThisSessionH = .XFP_Horizontal
                                SatoPrintJob.ThisSessionV = .XFP_Vertical
                                SatoPrintJob.ThisSessionR = .XFP_Rotation
                                SatoPrintJob.ThisSessionS = .XFP_Size
                            Case 3
                                SatoUtilities.ThisSessionAddr = .Clamshell_PrinterAddr
                                SatoPrintJob.ThisSessionAddr = .Clamshell_PrinterAddr
                                SatoPrintJob.ThisSessionH = .Clamshell_Horizontal
                                SatoPrintJob.ThisSessionV = .Clamshell_Vertical
                                SatoPrintJob.ThisSessionR = .Clamshell_Rotation
                                SatoPrintJob.ThisSessionS = .Clamshell_Size
                            Case 4
                                SatoUtilities.ThisSessionAddr = .ShippingA_PrinterAddr
                                SatoPrintJob.ThisSessionAddr = .ShippingA_PrinterAddr
                                SatoPrintJob.ThisSessionH = .ShippingA_Horizontal
                                SatoPrintJob.ThisSessionV = .ShippingA_Vertical
                                SatoPrintJob.ThisSessionR = .ShippingA_Rotation
                                SatoPrintJob.ThisSessionS = .ShippingA_Size
                        End Select
                    End With
                    If statusQuery = True Then SatoUtilities.PrinterStatus()
                    SatoPrintJob.TransmitImage(imagePath, printEnable)
                'Cab printer
                Case 2
                    With My.Settings
                        CabUtilities.ThisSessionAddr = .ShippingA_PrinterAddr
                        CabPrintJob.ThisSessionAddr = .ShippingA_PrinterAddr
                        CabPrintJob.ThisSessionH = .ShippingA_Horizontal
                        CabPrintJob.ThisSessionV = .ShippingA_Vertical
                        CabPrintJob.ThisSessionR = .ShippingA_Rotation
                        CabPrintJob.ThisSessionS = .ShippingA_Size
                    End With
                    If statusQuery = True Then CabUtilities.PrinterStatus()
                    CabPrintJob.TransmitImage(imagePath, printEnable)
                'Zebra Printer
                Case 5
                    Dim manualShipForm As New ShippingMPrintForm With {
                            .ImagePath = imagePath
                            }
                    manualShipForm.Show()
                    manualShipForm.Print()
                Case Else
                    MsgBox("Error - either no location, or an invalid location was specified", vbOK,
                           "Integra Optics Printer Controller")
            End Select

        End If
    End Sub

End Module