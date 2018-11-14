Imports System.Drawing
Imports System.IO
Imports System.Net.Sockets
Imports System.Text

''' <summary>
'''     Formats and sents Sends the bytes of the specified path to the printer
'''     Creates IntCAB.lbl in the temp folder to review the output data
''' </summary>
Public Class CabPrintJob

    Public Shared Property ThisSessionAddr
    Public Shared Property ThisSessionH
    Public Shared Property ThisSessionV
    Public Shared Property ThisSessionR
    Public Shared Property ThisSessionS

    'Set some ASCII Controls as variables
    Const Cr As String = Chr(13)

    Const Esc As String = Chr(27)

    ''' <summary>
    '''     Convert the image to a smaller more manageable 4-bit png
    ''' </summary>
    Private Shared Function ConvertImage(filepath As String) As String

        Using image As New Bitmap(filepath)
            'Do Stuff
            'Convert it to 4BPP
            Using imageConv = BitmapEncoder.ConvertBitmapToSpecified(image, 4)
                'Update the filepath for saving
                filepath = Path.GetTempPath & "IntegraCABLabel.png"
                'Save to disk
                imageConv.Save(filepath)
            End Using
        End Using
        ConvertImage = filepath
    End Function

    ''' <summary>
    '''     Takes the file from the specified path and converts it to a usable format
    '''     Adds in the required printer commands before and after the image data
    '''     then sends the data to the printer and\or creates a label file for review
    ''' </summary>

    Public Shared Sub TransmitImage(imagePath As String, transmit As Boolean)

        'check the file format
        Try
            Dim img As Image = Drawing.Image.FromFile(imagePath)
        Catch oome As OutOfMemoryException
            MsgBox("Imagefile not a valid image format")
            Exit Sub
        End Try

        'Dim fImagePath As String = ConvertImage(imagePath)
        Dim fImagePath As String = imagePath

        'prefix Text
        Dim eraseRamImage As Byte() = Encoding.ASCII.GetBytes("e IMG;*" & Cr)
        Dim downloadToPrinter As Byte() = Encoding.ASCII.GetBytes("d PNG;dblabel" & Cr)
        Dim startAndEndOfData As Byte() = Encoding.ASCII.GetBytes(Esc & ".")
        Dim prefix = eraseRamImage.Length +
                     downloadToPrinter.Length +
                     startAndEndOfData.Length
        Dim prefixOutStream(prefix - 1) As Byte
        eraseRamImage.CopyTo(prefixOutStream, 0)
        downloadToPrinter.CopyTo(prefixOutStream, eraseRamImage.Length)
        startAndEndOfData.CopyTo(prefixOutStream, eraseRamImage.Length + downloadToPrinter.Length)

        'suffix Text
        Dim carriageReturn As Byte() = Encoding.ASCII.GetBytes(Cr)
        Dim lang As Byte() = Encoding.ASCII.GetBytes("l US" & Cr)
        Dim meas As Byte() = Encoding.ASCII.GetBytes("m m" & Cr)
        Dim zeros As Byte() = Encoding.ASCII.GetBytes("J SFP" & Cr)
        Dim labelDimensions As Byte() = Encoding.ASCII.GetBytes("S l1;0.0,0.0,9.50,12.8,32.4;SFP" & Cr)
        Dim printSettings As Byte() = Encoding.ASCII.GetBytes("H 50,5,T,R0,B0" & Cr)
        Dim backfeed As Byte() = Encoding.ASCII.GetBytes("O P" & Cr)
        Dim peel As Byte() = Encoding.ASCII.GetBytes("P" & Cr)
        Dim image As Byte() =
                Encoding.ASCII.GetBytes($"I:labelimg;" & ThisSessionH &
                                        "," & ThisSessionV &
                                        "," & ThisSessionR &
                                        "," & ThisSessionS &
                                        "," & ThisSessionS &
                                        ",a;dblabel" & Cr)
        Dim jobQty As Byte() = Encoding.ASCII.GetBytes("A 1" & Cr)

        Dim suffix As Int32 = startAndEndOfData.Length +
                              carriageReturn.Length +
                              lang.Length +
                              meas.Length +
                              zeros.Length +
                              labelDimensions.Length +
                              printSettings.Length +
                              backfeed.Length +
                              peel.Length +
                              image.Length +
                              jobQty.Length

        Dim suffixOutStream(suffix - 1) As Byte

        startAndEndOfData.CopyTo(suffixOutStream, 0)
        carriageReturn.CopyTo(suffixOutStream, startAndEndOfData.Length)
        lang.CopyTo(suffixOutStream, startAndEndOfData.Length +
                                     carriageReturn.Length)
        meas.CopyTo(suffixOutStream, startAndEndOfData.Length +
                                     carriageReturn.Length + lang.Length)
        zeros.CopyTo(suffixOutStream, startAndEndOfData.Length +
                                      carriageReturn.Length + lang.Length + meas.Length)
        labelDimensions.CopyTo(suffixOutStream, startAndEndOfData.Length +
                                                carriageReturn.Length + lang.Length + meas.Length + zeros.Length)
        printSettings.CopyTo(suffixOutStream, startAndEndOfData.Length +
                                              carriageReturn.Length + lang.Length + meas.Length + zeros.Length +
                                              labelDimensions.Length)
        backfeed.CopyTo(suffixOutStream, startAndEndOfData.Length +
                                         carriageReturn.Length + lang.Length + meas.Length + zeros.Length +
                                         labelDimensions.Length + printSettings.Length)
        peel.CopyTo(suffixOutStream, startAndEndOfData.Length +
                                     carriageReturn.Length + lang.Length + meas.Length + zeros.Length +
                                     labelDimensions.Length + printSettings.Length + backfeed.Length)
        image.CopyTo(suffixOutStream, startAndEndOfData.Length +
                                      carriageReturn.Length + lang.Length + meas.Length + zeros.Length +
                                      labelDimensions.Length + printSettings.Length + backfeed.Length + peel.Length)
        jobQty.CopyTo(suffixOutStream, startAndEndOfData.Length +
                                       carriageReturn.Length + lang.Length + meas.Length + zeros.Length +
                                       labelDimensions.Length + printSettings.Length + backfeed.Length + peel.Length +
                                       image.Length)

        'Escape any ascii escapes
        Dim imageData() As Byte = File.ReadAllBytes(fImagePath)
        Dim newImgData As New List(Of Byte)
        For i = 0 To imageData.Length - 1
            If imageData(i) = &H1B Then
                'not sure about this,
                newImgData.Add(imageData(i)) 'add the &H1B
                newImgData.Add(&H1B) 'add the other character
            Else
                newImgData.Add(imageData(i))
            End If
        Next

        imageData = newImgData.ToArray

        Try
            If transmit = True Then
                Using skt As New TcpClient()
                    Try
                        skt.Connect(ThisSessionAddr, 9100)
                    Catch ex As Exception
                        MsgBox("CAB Connect Error (Label)")
                        Exit Sub
                    End Try
                    Using ns As NetworkStream = skt.GetStream()
                        If ns.CanWrite Then
                            ns.Write(prefixOutStream, 0, prefixOutStream.Length)
                            ns.Write(imageData, 0, imageData.Length)
                            ns.Write(suffixOutStream, 0, suffixOutStream.Length)
                            ns.Flush()
                        End If
                    End Using
                End Using
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        Dim labelPath As String = Path.GetTempPath & "IntCAB.lbl"
        If File.Exists(labelPath) Then
            File.Delete(labelPath)
        End If
        Using fs As New FileStream(labelPath, FileMode.Append, FileAccess.Write)
            'below for testing against download.exe
            'fs.Write(Encoding.ASCII.GetBytes("d PNG;INTEGRACABLABEL" & CR & ESC & "."), 0, Encoding.ASCII.GetBytes("d PNG;INTEGRACABLABEL" & CR & ESC & ".").Length)
            fs.Write(prefixOutStream, 0, prefixOutStream.Length)
            fs.Write(imageData, 0, imageData.Length)
            fs.Write(suffixOutStream, 0, suffixOutStream.Length)
            'below for testing against download.exe
            'fs.Write(Encoding.ASCII.GetBytes(ESC & "."), 0, Encoding.ASCII.GetBytes(ESC & ".").Length)
        End Using

    End Sub

End Class

''' <summary>
'''     Contains various utilities for dealing with the CAB printer
''' </summary>

Public Class CabUtilities

    Public Shared Property ThisSessionAddr

    Public _
        PrinterErrorCodes() As String =
            {"a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o", "r", "s", "u", "x",
             "A", "B", "C", "D", "E", "F", "G", "H", "M", "N", "O", "P", "R", "S", "V", "W", "X", "Y", "Z"}

    Const Esc As String = Chr(27)

    ''' <summary>
    '''     Query the status of the printer
    ''' </summary>

    Public Shared Sub PrinterStatus()

        Dim printerUtils As New CabUtilities()
        Using clientSocket As New TcpClient()

            Try
                clientSocket.Connect(ThisSessionAddr, 9100)
            Catch ex As Exception
                MsgBox("CAB Connect Error (Status)")
                Exit Sub
            End Try

            Using serverStream As NetworkStream = clientSocket.GetStream()

                'ESCs
                'Response Message XYNNNNNNZ
                'X – online Status Y/N
                'Y -Type of error (see table underneath)
                'NNNNNN -amount of labels to print
                'Z – Interpreter active Y = print job Is In process / N – printer In standby mode

                Dim queryPrinterStatus As Byte() = Encoding.ASCII.GetBytes(Esc & "s") 'Looking for all Ns
                'Dim queryPrinterExtStatus As Byte() = Encoding.ASCII.GetBytes(ESC & "z") 'Looking for 'Y-000000N'
                Dim totalStatusStreamLength As Int32 = queryPrinterStatus.Length
                Dim outStatusStream(totalStatusStreamLength) As Byte

                queryPrinterStatus.CopyTo(outStatusStream, 0)

                Try
                    'send the output stream to the printer
                    serverStream.Write(outStatusStream, 0, outStatusStream.Length)
                    serverStream.Flush()

                    Dim myReadBuffer(1024) As Byte
                    Dim myCompleteMessage = New StringBuilder()
                    Dim numberOfBytesRead

                    ' Incoming message may be larger than the buffer size.
                    Do
                        numberOfBytesRead = serverStream.Read(myReadBuffer, 0, myReadBuffer.Length)
                        myCompleteMessage.AppendFormat("{0}",
                                                       Encoding.ASCII.GetString(myReadBuffer, 0, numberOfBytesRead))
                    Loop While serverStream.DataAvailable

                    serverStream.Flush()
                    serverStream.Close()

                    Dim rawPrinterStatus As String = myCompleteMessage.ToString()
                    Dim onlineStatus As Char = rawPrinterStatus.Substring(0, 1)
                    Dim errorCode As Char = rawPrinterStatus.Substring(1, 1)

                    If onlineStatus = "N" Then
                        MsgBox("Printer not online, please correct the error and press okay to continue" _
                               & vbCrLf & vbCrLf & "Printer Error: " & printerUtils.PrinterStatusMessage(errorCode),
                               vbOKOnly, "CAB Printer Online Status")
                    Else
                        If Not errorCode = "-" Then
                            MsgBox(
                                "Printer Error: " & printerUtils.PrinterStatusMessage(errorCode) & vbCrLf & vbCrLf &
                                "Correct them problem then press OKAY to continue.", vbOK, "CAB Hermes+ Printer Status")
                        End If
                    End If
                Catch ex As Exception
                    MsgBox("Error reading CAB printer status from network stream" & vbCrLf & ex.Message)
                End Try
            End Using
        End Using
    End Sub

    ''' <summary>
    '''     Determine if the printer is in an error state
    ''' </summary>
    Friend Function PrinterInError(statusChar As String) As Boolean

        If statusChar = "Y" Then
            PrinterInError = True
        Else
            PrinterInError = False
        End If
    End Function

    ''' <summary>
    '''     Parse the status character definition
    ''' </summary>
    Friend Function PrinterStatusMessage(statusChar As String) As String
        Dim translation = ""
        Select Case statusChar
            Case "a"
                translation = "Applicator did Not reach the upper position"
            Case "b"
                translation = "Applicator did Not reach the lower position"
            Case "c"
                translation = "Vacuum plate Is empty"
            Case "d"
                translation = "Label Not deposited"
            Case "e"
                translation = "Host Stop / Error"
            Case "f"
                translation = "Reflective sensor blocked"
            Case "g"
                translation = "Tamp pad 90° error"
            Case "h"
                translation = "Tamp pad 0° error"
            Case "i"
                translation = "Table Not in front position"
            Case "j"
                translation = "Table Not in rear position"
            Case "k"
                translation = "Head lifted"
            Case "l"
                translation = "Head down"
            Case "m"
                translation = "Scan result negative"
            Case "n"
                translation = "Global network error"
            Case "o"
                translation = "Compressed air error"
            Case "r"
                translation = "RFID error"
            Case "s"
                translation = "System fault (immediately after power on)"
            Case "u"
                translation = "USB error"
            Case "x"
                translation = "Stacker full / printer goes on pause (only with a specified cutter)"
            Case "A"
                translation = "Applicator error"
            Case "B"
                translation = "Protocol error / invalid barcode data"
            Case "C"
                translation = "Memory card error"
            Case "D"
                translation = "Printhead open"
            Case "E"
                translation = "Synchronization error (no label found)"
            Case "F"
                translation = "Out of ribbon"
            Case "G"
                translation = "PPP reload required"
            Case "H"
                translation = "Heating voltage problem"
            Case "M"
                translation = "Cutter jammed"
            Case "N"
                translation = "Label material too thick (cutter)"
            Case "O"
                translation = "Out of memory"
            Case "P"
                translation = "Out of paper"
            Case "R"
                translation = "Ribbon detected in thermal direct mode"
            Case "S"
                translation = "Ribbonsaver malfunction"
            Case "V"
                translation = "Input buffer overflow"
            Case "W"
                translation = "Printhead overheated. Printer goes on pause"
            Case "X"
                translation = "External I/O error"
            Case "Y"
                translation = "Printhead error"
            Case "Z"
                translation = "Printhead damaged"
        End Select
        PrinterStatusMessage = translation
    End Function

    Friend Function PrinterExtendedStatusMessage(code As Int32) As String
        Dim translation = ""
        Select Case code
            Case 1
                translation = "Printer is paused"
            Case 2
                translation = "Printer has a job"
            Case 3
                translation = "Printer not ready for print data"
            Case 4
                translation = "Paper is moving"
            Case 5
                translation = "Ribbon warning (hardware dependend)"
            Case 6
                translation = "Paperend warning (hardware dependend)"
            Case 7
                translation = "Label in demand position"
            Case 8
                translation = "Label on vacuum plate (hardware dependend)"
            Case 9
                translation = "Applicator ready (hardware dependend)"
            Case 10
                translation = "External pause signal active (hardware dependend)"
            Case 11
                translation = "External print signal active (hardware dependend)"
            Case 12
                translation = "Printhead Cleaning required (cleaning interval)"
        End Select
        PrinterExtendedStatusMessage = translation
    End Function

End Class