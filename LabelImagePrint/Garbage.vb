Public Class Garbage

    '*************************************************************
    'NAME:          if verboseLogging = True then WriteToMessageLog
    'PURPOSE:       Open or create an message log and submit general message
    'PARAMETERS:    msg - message to be written to error file
    'RETURNS:       Nothing
    '*************************************************************


    Private Shared Sub ftpUpload(labelFilePath As String)
        ' Get the object used to communicate with the server.
        Dim request As FtpWebRequest = CType(WebRequest.Create("ftp://" & thisSessionAddr & "/iffs/IntegraS.lbl"), FtpWebRequest)
        request.Method = WebRequestMethods.Ftp.UploadFile

        ' This example assumes the FTP site uses anonymous logon.
        request.Credentials = New NetworkCredential("root", "1111")

        ' Copy the contents of the file to the request stream.
        Dim fileContents As Byte()

        Using sourceStream As StreamReader = New StreamReader(labelFilePath)
            fileContents = Encoding.UTF8.GetBytes(sourceStream.ReadToEnd())
        End Using

        request.ContentLength = fileContents.Length

        Using requestStream As Stream = request.GetRequestStream()
            requestStream.Write(fileContents, 0, fileContents.Length)
        End Using

        Using response As FtpWebResponse = CType(request.GetResponse(), FtpWebResponse)
            Console.WriteLine($"Upload File Complete, status {response.StatusDescription}")
        End Using
    End Sub

    Public Shared Sub WriteLabelFile(imagePath As String, one As Boolean)

        'Set full log path
        Dim labelPath As String = IO.Path.GetTempPath & "IntegraS.lbl"

        If File.Exists(labelPath) Then
            File.Delete(labelPath)
        End If

        Dim f_imagePath As String = ConvertImage(imagePath)

        'write it
        Dim labelFileStream As FileStream = New FileStream(labelPath, FileMode.Append, FileAccess.Write)
        Dim labelStreamWriter As StreamWriter = New StreamWriter(labelFileStream)
        Dim f_convOutBytes() As Byte = File.ReadAllBytes(f_imagePath)

        'Escape any ascii escapes
        For i As Int32 = 0 To f_convOutBytes.Length
            If f_convOutBytes(i) = &H1B Then
                ReDim Preserve f_convOutBytes(f_convOutBytes.Length + 1)
                'shift entire array
                Dim arrCopy(f_convOutBytes.Length + 1) As Byte
                Array.Copy(f_convOutBytes, 0, arrCopy, 0, i)
                arrCopy(i) = &H1B
                Array.Copy(f_convOutBytes, i, arrCopy, i + 1, f_convOutBytes.Length - i)
                f_convOutBytes = arrCopy
                i = i + 1
            End If
        Next

        Dim eraseRamImage As Byte() = Encoding.ASCII.GetBytes("e IMG;*" & CR)
        Dim downloadToPrinter As Byte() = Encoding.ASCII.GetBytes("d PNG;dblabel" & CR)
        Dim startAndEndOfData As Byte() = Encoding.ASCII.GetBytes(ESC & ".")
        Dim carriageReturn As Byte() = Encoding.ASCII.GetBytes(CR)
        Dim lang As Byte() = Encoding.ASCII.GetBytes("l US" & CR)
        Dim meas As Byte() = Encoding.ASCII.GetBytes("m m" & CR)
        Dim zeros As Byte() = Encoding.ASCII.GetBytes("J SFP" & CR)
        Dim labelDimensions As Byte() = Encoding.ASCII.GetBytes("S l1;0.0,0.0,9.50,12.8,32.4;SFP" & CR)
        Dim printSettings As Byte() = Encoding.ASCII.GetBytes("H 50,5,T,R0,B0" & CR)
        Dim backfeed As Byte() = Encoding.ASCII.GetBytes("O P" & CR)
        Dim peel As Byte() = Encoding.ASCII.GetBytes("P" & CR)
        Dim image As Byte() = Encoding.ASCII.GetBytes("I:labelimg;" & thisSessionH & "," & thisSessionV & "," & thisSessionR & "," & thisSessionS & "," & thisSessionS & ",a;dblabel" & CR)
        Dim jobQty As Byte() = Encoding.ASCII.GetBytes("A [?]" & CR)

        Dim totalStreamLength As Int32 = eraseRamImage.Length +
                                                downloadToPrinter.Length +
                                                (startAndEndOfData.Length * 2) +
                                                carriageReturn.Length +
                                                f_convOutBytes.Length +
                                                lang.Length +
                                                meas.Length +
                                                zeros.Length +
                                                labelDimensions.Length +
                                                printSettings.Length +
                                                backfeed.Length +
                                                peel.Length +
                                                image.Length +
                                                jobQty.Length

        Dim labelOutStream(totalStreamLength - 1) As Byte

        'concat bytes onto the output stream
        eraseRamImage.CopyTo(labelOutStream, 0)
        downloadToPrinter.CopyTo(labelOutStream, eraseRamImage.Length)
        startAndEndOfData.CopyTo(labelOutStream, eraseRamImage.Length + downloadToPrinter.Length)
        labelStreamWriter.Write(Encoding.UTF8.GetString(labelOutStream))
        labelStreamWriter.Write(f_convOutBytes)

        f_convOutBytes.CopyTo(labelOutStream, eraseRamImage.Length + downloadToPrinter.Length + startAndEndOfData.Length)
        startAndEndOfData.CopyTo(labelOutStream, eraseRamImage.Length + downloadToPrinter.Length + startAndEndOfData.Length + f_convOutBytes.Length)
        carriageReturn.CopyTo(labelOutStream, eraseRamImage.Length + downloadToPrinter.Length + startAndEndOfData.Length + f_convOutBytes.Length + startAndEndOfData.Length)

        lang.CopyTo(labelOutStream, eraseRamImage.Length + downloadToPrinter.Length + startAndEndOfData.Length + f_convOutBytes.Length + startAndEndOfData.Length +
                    carriageReturn.Length)
        meas.CopyTo(labelOutStream, eraseRamImage.Length + downloadToPrinter.Length + startAndEndOfData.Length + f_convOutBytes.Length + startAndEndOfData.Length +
                    carriageReturn.Length + lang.Length)
        zeros.CopyTo(labelOutStream, eraseRamImage.Length + downloadToPrinter.Length + startAndEndOfData.Length + f_convOutBytes.Length + startAndEndOfData.Length +
                     carriageReturn.Length + lang.Length + meas.Length)
        labelDimensions.CopyTo(labelOutStream, eraseRamImage.Length + downloadToPrinter.Length + startAndEndOfData.Length + f_convOutBytes.Length + startAndEndOfData.Length +
                               carriageReturn.Length + lang.Length + meas.Length + zeros.Length)
        printSettings.CopyTo(labelOutStream, eraseRamImage.Length + downloadToPrinter.Length + startAndEndOfData.Length + f_convOutBytes.Length + startAndEndOfData.Length +
                             carriageReturn.Length + lang.Length + meas.Length + zeros.Length + labelDimensions.Length)
        backfeed.CopyTo(labelOutStream, eraseRamImage.Length + downloadToPrinter.Length + startAndEndOfData.Length + f_convOutBytes.Length + startAndEndOfData.Length +
                        carriageReturn.Length + lang.Length + meas.Length + zeros.Length + labelDimensions.Length + printSettings.Length)
        peel.CopyTo(labelOutStream, eraseRamImage.Length + downloadToPrinter.Length + startAndEndOfData.Length + f_convOutBytes.Length + startAndEndOfData.Length +
                    carriageReturn.Length + lang.Length + meas.Length + zeros.Length + labelDimensions.Length + printSettings.Length + backfeed.Length)
        image.CopyTo(labelOutStream, eraseRamImage.Length + downloadToPrinter.Length + startAndEndOfData.Length + f_convOutBytes.Length + startAndEndOfData.Length +
                     carriageReturn.Length + lang.Length + meas.Length + zeros.Length + labelDimensions.Length + printSettings.Length + backfeed.Length + peel.Length)
        jobQty.CopyTo(labelOutStream, eraseRamImage.Length + downloadToPrinter.Length + startAndEndOfData.Length + f_convOutBytes.Length + startAndEndOfData.Length +
                      carriageReturn.Length + lang.Length + meas.Length + zeros.Length + labelDimensions.Length + printSettings.Length + backfeed.Length + peel.Length + image.Length)


        labelStreamWriter.Write(Encoding.UTF8.GetString(labelOutStream))

        labelStreamWriter.Close()
        labelFileStream.Close()

        'ftpUpload(logPath)

        If Not labelStreamWriter Is Nothing Then labelStreamWriter = Nothing
        If Not labelFileStream Is Nothing Then labelFileStream = Nothing

    End Sub



    '*************************************************************
    'NAME:          if verboseLogging = True then WriteToMessageLog
    'PURPOSE:       Open or create an message log and submit general message
    'PARAMETERS:    msg - message to be written to error file
    'RETURNS:       Nothing
    '*************************************************************

    Friend Shared Sub TransmitImage(imagePath As String)
        Dim clientSocket As New Sockets.TcpClient()
        Dim imageJob As New CABPrintJob()

        Dim f_imagePath As String = ConvertImage(imagePath)

retryConnect:
        Try
            clientSocket.Connect(thisSessionAddr, 9100)
        Catch ex As Exception
            Dim exResponse = MsgBox("Failed to connect to printer, " & ex.Message & "Client connect error", vbRetryCancel, "CAB Hermes+ Printer")
            If exResponse = vbRetry Then
                GoTo retryConnect
            ElseIf exResponse = vbCancel Then
                Exit Sub
            End If

        End Try

retryImage:
        Try

            Dim f_convOutBytes() As Byte = File.ReadAllBytes(f_imagePath)



            Dim serverStream As Sockets.NetworkStream = clientSocket.GetStream()

            'd type;name[SAVE] [B:± value]CR ESC.binary data ESC.
            Dim eraseRamImage As Byte() = Encoding.ASCII.GetBytes("e IMG;*" & CR)
            Dim downloadToPrinter As Byte() = Encoding.ASCII.GetBytes("d BMP;dblabel[SAVE]" & CR)
            Dim startAndEndOfData As Byte() = Encoding.ASCII.GetBytes(ESC & ".")
            Dim startOfData As Byte() = Encoding.ASCII.GetBytes(ESC & ":")
            Dim endOfData As Byte() = Encoding.ASCII.GetBytes(ESC & "end-of-data")
            Dim totalImageStreamLength As Int32 = downloadToPrinter.Length +
                                                    (startAndEndOfData.Length * 2) +
                                                    f_convOutBytes.Length



            Dim imageOutStream(totalImageStreamLength) As Byte

            'concat bytes onto the output stream
            downloadToPrinter.CopyTo(imageOutStream, 0)
            startAndEndOfData.CopyTo(imageOutStream, downloadToPrinter.Length)
            f_convOutBytes.CopyTo(imageOutStream, downloadToPrinter.Length + startAndEndOfData.Length)
            startAndEndOfData.CopyTo(imageOutStream, downloadToPrinter.Length + startAndEndOfData.Length + f_convOutBytes.Length)

            ' Check to see if this NetworkStream is writable.
            If serverStream.CanWrite Then
                'send a CAN to clear out previous jobs (DO  ES NOT CLEAR BUFFER)
                WriteToMessageLog(Encoding.UTF8.GetString(imageJob.cancelJobs))
                'serverStream.Write(imageJob.cancelJobs, 0, imageJob.cancelJobs.Length)
                'serverStream.Flush()
                'clear images from the memory buffer
                WriteToMessageLog(Encoding.UTF8.GetString(eraseRamImage))
                'serverStream.Write(eraseRamImage, 0, eraseRamImage.Length)
                'serverStream.Flush()
                'send the output stream to the printer
                WriteToMessageLog(Encoding.UTF8.GetString(imageOutStream))
                serverStream.Write(imageOutStream, 0, imageOutStream.Length)
                serverStream.Flush()
            End If

            serverStream.Flush()
            serverStream.Close()

            If Not imageOutStream Is Nothing Then imageOutStream = Nothing
            If Not serverStream Is Nothing Then serverStream = Nothing
            If Not f_convOutBytes Is Nothing Then f_convOutBytes = Nothing

            'imageJob.TransmitJob()

        Catch ex As Exception
            If MsgBox("Failed to transmit image, " & ex.Message & "Socket Stream Error" & vbCrLf _
                & "The label was saved at ... with the file name ...", vbOK, "CAB Hermes+ Printer") = vbRetry Then
                GoTo retryImage
            Else
                Exit Sub
            End If
        End Try

        clientSocket.Close()

        If Not clientSocket Is Nothing Then clientSocket = Nothing
        If Not imageJob Is Nothing Then imageJob = Nothing

    End Sub

    '*************************************************************
    'NAME:          if verboseLogging = True then WriteToMessageLog
    'PURPOSE:       Open or create an message log and submit general message
    'PARAMETERS:    msg - message to be written to error file
    'RETURNS:       Nothing
    '*************************************************************

    Private Sub TransmitJob()
        Dim clientSocket As New Sockets.TcpClient()
        Dim printJob As New CABPrintJob()

retryConnect:
        Try
            clientSocket.Connect(thisSessionAddr, 9100)
        Catch ex As Exception
            Dim exResponse = MsgBox("Failed to retreive the printer status, " & ex.Message & "Client connect error", vbRetryCancel, "CAB Hermes+ Printer")
            If exResponse = vbRetry Then
                GoTo retryConnect
            ElseIf exResponse = vbCancel Then
                Exit Sub
            End If

        End Try

retryJob:
        Try
            Dim serverStream As Sockets.NetworkStream = clientSocket.GetStream()

            'l US	                                language US
            'm m	                                    metric measuring units
            'zO                                     Print unslashed zero
            'j SFP	                                job start w/ comment
            'S l1;0.0, 0.0, 9.5, 12.8, 32.4;SFP	    Label Size gap, 0 horiz displacement, 0 vert displacement, 9.5mm height, 12.8mm height + gap, 32.4mm label width, label name
            'h 50, 5, T, R0, B0	                    Heat 50, speed 5, Type thermal Transfer, Ribbon saver off, backfeed 0mm
            'O P                                	    Option smart backfeed
            'P                                      Peel off mode
            'I:Image;-1.0,0.56,0.0,1,1,a;dblabel    	print image named integra
            'a 1	                                    Job quantity
            Dim lang As Byte() = Encoding.ASCII.GetBytes("l US" & CR)
            Dim meas As Byte() = Encoding.ASCII.GetBytes("m m" & CR)
            Dim zeros As Byte() = Encoding.ASCII.GetBytes("J SFP" & CR)
            Dim labelDimensions As Byte() = Encoding.ASCII.GetBytes("S l1;0.0,0.0,9.50,12.8,32.4;SFP" & CR)
            Dim printSettings As Byte() = Encoding.ASCII.GetBytes("H 50,5,T,R0,B0" & CR)
            Dim backfeed As Byte() = Encoding.ASCII.GetBytes("O P" & CR)
            Dim peel As Byte() = Encoding.ASCII.GetBytes("P" & CR)
            Dim image As Byte() = Encoding.ASCII.GetBytes("I:labelimg;" & thisSessionH & "," & thisSessionV & "," & thisSessionR & "," & thisSessionS & "," & thisSessionS & ",a;dblabel" & CR)
            Dim jobQty As Byte() = Encoding.ASCII.GetBytes("A [?]" & CR)

            Dim totalJobStreamLength As Int32 = lang.Length +
                                            meas.Length +
                                            zeros.Length +
                                            labelDimensions.Length +
                                            printSettings.Length +
                                            backfeed.Length +
                                            peel.Length +
                                            image.Length +
                                            jobQty.Length

            Dim jobOutStream(totalJobStreamLength) As Byte

            'concat bytes onto the output stream
            lang.CopyTo(jobOutStream, 0)
            meas.CopyTo(jobOutStream, lang.Length)
            zeros.CopyTo(jobOutStream, lang.Length + meas.Length)
            labelDimensions.CopyTo(jobOutStream, lang.Length + meas.Length + zeros.Length)
            printSettings.CopyTo(jobOutStream, lang.Length + meas.Length + zeros.Length + labelDimensions.Length)
            backfeed.CopyTo(jobOutStream, lang.Length + meas.Length + zeros.Length + labelDimensions.Length + printSettings.Length)
            peel.CopyTo(jobOutStream, lang.Length + meas.Length + zeros.Length + labelDimensions.Length + printSettings.Length + backfeed.Length)
            image.CopyTo(jobOutStream, lang.Length + meas.Length + zeros.Length + labelDimensions.Length + printSettings.Length + backfeed.Length + peel.Length)
            jobQty.CopyTo(jobOutStream, lang.Length + meas.Length + zeros.Length + labelDimensions.Length + printSettings.Length + backfeed.Length + peel.Length + image.Length)

            Dim str As String = Encoding.UTF8.GetString(jobOutStream)
            WriteToMessageLog(str)

            ' Check to see if this NetworkStream is writable.
            If serverStream.CanWrite Then
                'send a CAN to clear out previous jobs (DOES NOT CLEAR BUFFER)
                'serverStream.Write(printJob.cancelJobs, 0, printJob.cancelJobs.Length)
                'serverStream.Flush()
                'send the output stream to the printer
                serverStream.Write(jobOutStream, 0, jobOutStream.Length)
                serverStream.Flush()
            End If

            serverStream.Flush()
            serverStream.Close()
            If Not jobOutStream Is Nothing Then jobOutStream = Nothing
            If Not serverStream Is Nothing Then serverStream = Nothing

        Catch ex As Exception
            If MsgBox("Failed to transmit job, " & ex.Message & "Socket Stream Error" & vbCrLf _
                   & "The label was saved at ... with the file name ...", vbOK, "CAB Hermes+ Printer") = vbRetry Then
                GoTo retryJob
            Else
                Exit Sub
            End If
        End Try

        clientSocket.Close()

        If Not clientSocket Is Nothing Then clientSocket = Nothing
        If Not printJob Is Nothing Then printJob = Nothing
    End Sub

End Class
