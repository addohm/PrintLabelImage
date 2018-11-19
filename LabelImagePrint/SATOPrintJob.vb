Imports System.Drawing
Imports System.IO
Imports System.Net.Sockets
Imports System.Text
Imports Sato.Graphics

''' <summary>
'''     Formats and sents Sends the bytes of the specified path to the printer
'''     Creates IntSATO.lbl in the temp folder to review the output data
''' </summary>
Public Class SatoPrintJob

    Public Shared Property ThisSessionAddr
    Public Shared Property ThisSessionH
    Public Shared Property ThisSessionV
    Public Shared Property ThisSessionR
    Public Shared Property ThisSessionS

    'Set some ASCII Controls as variables
    Const Sot As String = Chr(2)
    Const Eot As String = Chr(3)
    Const Esc As String = Chr(27)

    ''' <summary>
    '''     Convert the image to a smaller more manageable 4-bit png
    ''' </summary>
    Private Shared Function ConvertImage(filepath As String) As String
        Using image As New Bitmap(filepath)
            'Do Stuff
            'Convert it to 4BPP
            Using imageConv = BitmapEncoder.ConvertBitmapToSpecified(image, 8)
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

    Friend Shared Sub TransmitImage(imagePath As String, transmit As Boolean)

        'Check the file format
        Try
            Dim img As Image = Image.FromFile(imagePath)
        Catch oome As OutOfMemoryException
            MsgBox("Imagefile not a valid image format")
            Exit Sub
        End Try

        'convert the image to a bitmap and return the new path
        'Dim fImagePath As String = ConvertImage(imagePath)
        Dim fImagePath As String = imagePath

        'prefix text
        Dim startOfText As Byte() = Encoding.ASCII.GetBytes(Sot)
        Dim startJob As Byte() = Encoding.ASCII.GetBytes(Esc & "A")
        Dim fHPos As Byte() = Encoding.ASCII.GetBytes(Esc & "H" & ThisSessionH)
        Dim fVPos As Byte() = Encoding.ASCII.GetBytes(Esc & "V" & ThisSessionV)
        Dim fEnlargement As Byte() = Encoding.ASCII.GetBytes(Esc & "L0" & ThisSessionS & "0" & ThisSessionS)
        Dim fRotation As Byte() = Encoding.ASCII.GetBytes(Esc & "%" & ThisSessionR)
        Dim prefix = startOfText.Length +
                     startJob.Length +
                     fHPos.Length +
                     fVPos.Length +
                     fEnlargement.Length +
                     fRotation.Length
        Dim prefixOutStream(prefix - 1) As Byte

        startOfText.CopyTo(prefixOutStream, 0)
        startJob.CopyTo(prefixOutStream, startOfText.Length)
        fHPos.CopyTo(prefixOutStream, startOfText.Length + startJob.Length)
        fVPos.CopyTo(prefixOutStream, startOfText.Length + startJob.Length + fHPos.Length)
        fEnlargement.CopyTo(prefixOutStream, startOfText.Length + startJob.Length + fHPos.Length + fVPos.Length)
        fRotation.CopyTo(prefixOutStream,
                         startOfText.Length + startJob.Length + fHPos.Length + fVPos.Length + fEnlargement.Length)
        'image text
        'create a file that contains the sato bitmap conversion
        Dim tempRawFilePath As String = Path.GetTempPath & "IntegraSATOLabel.tmp"
        If File.Exists(tempRawFilePath) Then
            File.Delete(tempRawFilePath)
        End If
        'convert the format to a sato encoded string
        SbplConverter.Convert(fImagePath, tempRawFilePath, True)
        'read the bytes from the converted data
        Dim fConvOutBytes() As Byte = File.ReadAllBytes(tempRawFilePath)
        Dim imageOutStream(fConvOutBytes.Length) As Byte
        fConvOutBytes.CopyTo(imageOutStream, 0)
        'clean up the temp file conversion
        File.Delete(tempRawFilePath)

        'suffix text
        Dim endOfText As Byte() = Encoding.ASCII.GetBytes(Eot)
        Dim endJob As Byte() = Encoding.ASCII.GetBytes(Esc & "Z")
        Dim jobQty As Byte() = Encoding.ASCII.GetBytes(Esc & "Q1")
        Dim suffix = endOfText.Length +
                     endJob.Length +
                     jobQty.Length
        Dim suffixOutStream(suffix - 1) As Byte
        jobQty.CopyTo(suffixOutStream, 0)
        endJob.CopyTo(suffixOutStream, jobQty.Length)
        endOfText.CopyTo(suffixOutStream, jobQty.Length + endJob.Length)

        Try
            'transmite the outstream byte array to the specified address
            If transmit = True Then
                Using skt As New TcpClient()
                    Try
                        skt.Connect(ThisSessionAddr, 9100)
                    Catch ex As Exception
                        MsgBox("SATO Connect Error (Label)")
                    End Try
                    Using ns As NetworkStream = skt.GetStream()
                        If ns.CanWrite Then
                            ns.Write(prefixOutStream, 0, prefixOutStream.Length)
                            ns.Write(imageOutStream, 0, imageOutStream.Length)
                            ns.Write(suffixOutStream, 0, suffixOutStream.Length)
                            ns.Flush()
                        End If
                    End Using
                End Using
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        'save the outstream to a file
        Dim labelPath As String = Path.GetTempPath & "IntSATO.lbl"
        If File.Exists(labelPath) Then
            File.Delete(labelPath)
        End If
        Using fs As New FileStream(labelPath, FileMode.Append, FileAccess.Write)
            fs.Write(prefixOutStream, 0, prefixOutStream.Length)
            fs.Write(imageOutStream, 0, imageOutStream.Length)
            fs.Write(suffixOutStream, 0, suffixOutStream.Length)
        End Using

    End Sub

    ''' <summary>
    ''' A second type of printing which prints only the select text passed
    ''' </summary>
    ''' <param name="transmit"></param>
    ''' <param name="stringText"></param>
    Friend Shared Sub TransmitString(transmit As Boolean, stringText As String)

        'prefix text
        Dim startOfText As Byte() = Encoding.ASCII.GetBytes(Sot)
        Dim startJob As Byte() = Encoding.ASCII.GetBytes(Esc & "A")
        Dim fHPos As Byte() = Encoding.ASCII.GetBytes(Esc & "H" & ThisSessionH)
        Dim fVPos As Byte() = Encoding.ASCII.GetBytes(Esc & "V" & ThisSessionV)
        Dim fEnlargement As Byte() = Encoding.ASCII.GetBytes(Esc & "L0" & ThisSessionS & "0" & ThisSessionS)
        Dim fRotation As Byte() = Encoding.ASCII.GetBytes(Esc & "%" & ThisSessionR)
        Dim prefix As Integer = startOfText.Length +
                     startJob.Length +
                     fHPos.Length +
                     fVPos.Length +
                     fEnlargement.Length +
                     fRotation.Length
        Dim prefixOutStream(prefix - 1) As Byte

        startOfText.CopyTo(prefixOutStream, 0)
        startJob.CopyTo(prefixOutStream, startOfText.Length)
        fHPos.CopyTo(prefixOutStream, startOfText.Length + startJob.Length)
        fVPos.CopyTo(prefixOutStream, startOfText.Length + startJob.Length + fHPos.Length)
        fEnlargement.CopyTo(prefixOutStream, startOfText.Length + startJob.Length + fHPos.Length + fVPos.Length)
        fRotation.CopyTo(prefixOutStream,
                         startOfText.Length + startJob.Length + fHPos.Length + fVPos.Length + fEnlargement.Length)

        'string text
        Dim fstringPrefix As Byte() = Encoding.ASCII.GetBytes(Esc & "CC2" & Esc & "RDX01")
        Dim fstringText As Byte() = Encoding.ASCII.GetBytes(stringText)
        Dim ftext As Integer = fstringPrefix.Length + fstringText.Length
        Dim textOutStream(ftext - 1) As Byte

        fstringPrefix.CopyTo(textOutStream, 0)
        fstringText.CopyTo(textOutStream, fstringPrefix.Length)

        'suffix text
        Dim endOfText As Byte() = Encoding.ASCII.GetBytes(Eot)
        Dim endJob As Byte() = Encoding.ASCII.GetBytes(Esc & "Z")
        Dim jobQty As Byte() = Encoding.ASCII.GetBytes(Esc & "Q1")
        Dim suffix = endOfText.Length +
                     endJob.Length +
                     jobQty.Length
        Dim suffixOutStream(suffix - 1) As Byte
        jobQty.CopyTo(suffixOutStream, 0)
        endJob.CopyTo(suffixOutStream, jobQty.Length)
        endOfText.CopyTo(suffixOutStream, jobQty.Length + endJob.Length)

        Try
            'transmite the outstream byte array to the specified address
            If transmit = True Then
                Using skt As New TcpClient()
                    Try
                        skt.Connect(ThisSessionAddr, 9100)
                    Catch ex As Exception
                        MsgBox("SATO Connect Error (Label)")
                    End Try
                    Using ns As NetworkStream = skt.GetStream()
                        If ns.CanWrite Then
                            ns.Write(prefixOutStream, 0, prefixOutStream.Length)
                            ns.Write(textOutStream, 0, textOutStream.Length)
                            ns.Write(suffixOutStream, 0, suffixOutStream.Length)
                            ns.Flush()
                        End If
                    End Using
                End Using
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        'save the outstream to a file
        Dim labelPath As String = Path.GetTempPath & "IntSATO.lbl"
        If File.Exists(labelPath) Then
            File.Delete(labelPath)
        End If
        Using fs As New FileStream(labelPath, FileMode.Append, FileAccess.Write)
            fs.Write(prefixOutStream, 0, prefixOutStream.Length)
            fs.Write(textOutStream, 0, textOutStream.Length)
            fs.Write(suffixOutStream, 0, suffixOutStream.Length)
        End Using

    End Sub
End Class

''' <summary>
'''     Contains various utilities for dealing with the SATO printer
''' </summary>

Public Class SatoUtilities
    Public Shared Property ThisSessionAddr

    Public _
        PrinterErrorCodes() As String = {"a", "b", "c", "d", "E", "f", "g", "h", "i", "j", "k", "l", "m", "o", "p", "q"}

    Const Sot As String = Chr(2)
    Const Eot As String = Chr(3)
    Const Enq As String = Chr(5)

    ''' <summary>
    '''     Query the status of the printer
    ''' </summary>
    Public Shared Sub PrinterStatus()

        Dim printerUtils As New SatoUtilities

        Using clientSocket As New TcpClient()
            Try
                clientSocket.Connect(ThisSessionAddr, 9100)
            Catch ex As Exception
                MsgBox("SATO Connect Error (Status)")
                Exit Sub
            End Try

            Using serverStream As NetworkStream = clientSocket.GetStream()

                Dim queryStatus As Byte() = Encoding.ASCII.GetBytes(Sot & Enq & Eot)

                Try
                    serverStream.Write(queryStatus, 0, queryStatus.Length)
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
                    Dim startIndex As Integer = rawPrinterStatus.IndexOf(ChrW(2)) + 1
                    Dim endIndex As Integer = rawPrinterStatus.IndexOf(ChrW(3))
                    Dim subPrinterStatus As String = rawPrinterStatus.Substring(startIndex, endIndex - startIndex)
                    Dim cleanedPrinterStatus As String = subPrinterStatus.Trim()

                    If printerUtils.PrinterErrorCodes.Contains(cleanedPrinterStatus(0)) Then
                        MsgBox(
                            "Printer Error: " & printerUtils.PrinterStatusMessage(0) & vbCrLf & vbCrLf &
                            "Correct them problem then press OKAY to continue.", vbOK, "SATO S84ex Printer Status")
                    End If
                Catch ex As Exception
                    MsgBox("Error reading CAB printer status from network stream" & vbCrLf & ex.Message)
                End Try

            End Using

        End Using

        If Not printerUtils Is Nothing Then printerUtils = Nothing
    End Sub

    ''' <summary>
    '''     Parse the status character definition
    ''' </summary>
    Friend Function PrinterStatusMessage(statusChar As String) As String
        Dim translation = ""
        Select Case statusChar
            'Error Detection
            Case "a"
                translation = "Error - Buffer Over"
            Case "b"
                translation = "Error - Head Open"
            Case "c"
                translation = "Error - Paper End"
            Case "d"
                translation = "Error - Ribbon End"
            Case "E"
                translation = "Error - Media Error (print skip error)"
            Case "f"
                translation = "Error - Sensor Error, " _
                              & "(Unused) Barcode read/verification error, " _
                              & "(Unused) Barcode reader connection error"
            Case "g"
                translation = "Error - Head Error"
            Case "h"
                translation = "Error - (Unused) Cover open, " _
                              & "(Unused) Cutter open error, " _
                              & "(Unused) Cover open"
            Case "i"
                translation = "Error - Card Error"
            Case "j"
                translation = "Error - (Unused) Cutter error"
            Case "k"
                translation = "Error - Other Error"
            Case "l"
                translation = "Error - (Unused) Cutter sensor error"
            Case "m"
                translation = "Error - (Unused) Stacker or rewinder full Rewind full"
            Case "o"
                translation = "Error - RFID Tag error"
            Case "p"
                translation = "Error - RFID protect error"
            Case "q"
                translation = "Error - (Unused) Battery Error"
        End Select

        PrinterStatusMessage = translation
    End Function

End Class