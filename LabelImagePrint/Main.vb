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
    Private Function WriteImageFromDb(printposition As String, serialNo As String) As String
        WriteImageFromDb = ""

        Dim imagePath
        ' Writes the BLOB to a file (*.bmp).
        ' Streams the binary data to the FileStream object.
        ' The bytes returned from GetBytes.
        Dim retval As Long
        ' The starting position in the BLOB output.
        Dim startIndex As Long
        Dim labelType As String
        If printposition = 1 Or printposition = 2 Then
            labelType = "O"
        ElseIf printposition = 3 Then
            labelType = "C"
        ElseIf printposition = 4 Or printposition = 5 Then
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
    Friend Sub ReadSettingsFromDBtoSettings(position As Integer)
        Using connection = New SqlConnection()
            Using cmd As New SqlCommand()
                connection.ConnectionString = My.Settings.ConnStr
                With cmd
                    .Connection = connection
                    connection.Open()
                    If position = 1 Then
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_XFPh'"
                        My.Settings.XFP_Horizontal = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_XFPv'"
                        My.Settings.XFP_Vertical = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_XFPs'"
                        My.Settings.XFP_Size = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_XFPr'"
                        My.Settings.XFP_Rotation = Convert.ToInt32(.ExecuteScalar)
                    ElseIf position = 2 Then
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_SFPh'"
                        My.Settings.SFP_Horizontal = Convert.ToDecimal(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_SFPv'"
                        My.Settings.SFP_Vertical = Convert.ToDecimal(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_SFPs'"
                        My.Settings.SFP_Size = Convert.ToDecimal(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_SFPr'"
                        My.Settings.SFP_Rotation = Convert.ToInt32(.ExecuteScalar)
                    ElseIf position = 3 Then
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_Clamshellh'"
                        My.Settings.Clamshell_Horizontal = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_Clamshellv'"
                        My.Settings.Clamshell_Vertical = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_Clamshells'"
                        My.Settings.Clamshell_Size = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_Clamshellr'"
                        My.Settings.Clamshell_Rotation = Convert.ToInt32(.ExecuteScalar)
                    ElseIf position = 4 Then
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_ShippinghA'"
                        My.Settings.ShippingA_Horizontal = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_ShippingvA'"
                        My.Settings.ShippingA_Vertical = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_ShippingsA'"
                        My.Settings.ShippingA_Size = Convert.ToInt32(.ExecuteScalar)
                        .CommandText = "select setting_value from system_settings where setting_name = 'LABEL_ShippingrA'"
                        My.Settings.ShippingA_Rotation = Convert.ToInt32(.ExecuteScalar)
                    ElseIf position = 5 Then
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
        Dim setupMode As Boolean = False
        Dim serialNo As String = String.Empty
        Dim printposition As Integer = 0
        Dim statusQuery As Boolean = False
        Dim printEnable As Boolean = False

        'remove the first array element (file path) and shift the rest
        Dim args() As String = Environment.GetCommandLineArgs()
        args = args.Skip(1).ToArray
        If args.Length = 1 Then
            If HelpRequired(args(0)) Then
                DisplayHelp()
            End If
        Else
            'Parse all the command line arguments
            For Each c In args
                Dim argVal As String = String.Empty
                'Return the argument name
                Dim arg As String = c.Split("=")(0).Replace("/", "").ToLower
                'return the argument value
                If c.Contains("=") Then
                    argVal = c.Split("=")(1).ToLower
                End If
                Select Case arg
                    Case "setup"
                        setupMode = Convert.ToBoolean(Convert.ToInt32(argVal))
                    Case "serial"
                        serialNo = argVal
                    Case "position"
                        printposition = Convert.ToInt32(argVal)
                    Case "status"
                        statusQuery = True
                    Case "print"
                        printEnable = True
                End Select
            Next

            'Open setup dialoge
            If setupMode = True Then
                Dim printSettingsForm As New FormSettingsPrinter
                printSettingsForm.position = printposition
                'load all txtboxes with data from db before the form load
                printSettingsForm.UpdateFormFields()
                'load settings form
                printSettingsForm.ShowDialog()
            Else

                If Not serialNo = String.Empty And Not printposition = 0 Then

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

                    ReadSettingsFromDBtoSettings(printposition)

                    'Attain and print label at the defined position using the saved settings
                    If setupMode = False Then
                        'Save the image and return the file path
                        Dim imagePath As String = WriteImageFromDb(printposition, serialNo)

                        Select Case printposition
                'Sato printers
                            Case 1, 3, 4
                                With My.Settings
                                    Select Case printposition
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
                                MsgBox("Error - either no position, or an invalid position was specified", vbOK,
                                   "Integra Optics Printer Controller")
                        End Select

                    End If
                Else
                    Console.WriteLine("A serial number and print position is required to run this application")
                    Console.ReadLine()
                End If
            End If
        End If
        GC.Collect()
    End Sub

    Private Function HelpRequired(param As String)
        If param = "-h" Or param = "--help" Or param = "/?" Then
            Return True
        End If
        Return False
    End Function

    Private Sub DisplayHelp()
        Console.WriteLine("======================================================================")
        Console.WriteLine("Robotics Label Image Print Application")
        Console.WriteLine("Written by: Adam S. Leven of Automation Integrity")
        Console.WriteLine("http://automationintegrity.net")
        Console.WriteLine("======================================================================")
        Console.WriteLine("Arguments:")
        Console.WriteLine("setup (1 or 0) - Access the setup menu")
        Console.WriteLine("serial (string) - The serial number appropriate to the position being printed")
        Console.WriteLine("position (1 to 5) - The printer position where the label is expected")
        Console.WriteLine("status (1 or 0 - Includes querying the printer status")
        Console.WriteLine("print (1 or 0) - Includes print control")
        Console.WriteLine("======================================================================")
        Console.WriteLine("Positions:")
        Console.WriteLine("1 - XFP Printer (SATO)")
        Console.WriteLine("2 - SFP Printer (CAB)")
        Console.WriteLine("3 - Clamshell Printer (SATO)")
        Console.WriteLine("4 - Auto Shipping Printer (SATO)")
        Console.WriteLine("5 - Manual Shipping Printer (Zebra)")
        Console.WriteLine("======================================================================")
        Console.WriteLine("Example:")
        Console.WriteLine("printlabel.exe /setup")
        Console.WriteLine("> Would bring up the setup screen to edit the connection values or label settings")
        Console.WriteLine("printlabel.exe /serial=EOXJ2003001 /position=2 /print")
        Console.WriteLine("> Would print an optic label at the SFP printer for serial EOXJ2003001")
        Console.WriteLine("printlabel.exe /serial=EOXJ2003001 /position=3")
        Console.WriteLine("> Would would produce a label but not print it.")
        Console.WriteLine("======================================================================")
        Console.WriteLine("> In %TEMP% you can view the image and label file produced by the code.")
        Console.WriteLine("> The final product label would be called 'IntegraCLabel.png' where C is a clamshell label.")
        Console.WriteLine("> The final label syntax files would be called IntSATO.lbl or IntCAB.lbl")
    End Sub

End Module