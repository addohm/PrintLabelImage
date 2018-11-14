Imports System.Drawing.Imaging
Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.IO
Imports ImageMagick

Public Class FormImageConverter

    Public Function CreateBitmap(ByVal sourcePixels As Integer(), ByVal newSize As Size) As Bitmap

        Dim bm As New Bitmap(newSize.Width, newSize.Height, PixelFormat.Format1bppIndexed)
        Dim bounds As New Rectangle(0, 0, newSize.Width, newSize.Height)
        Dim bmd As BitmapData = bm.LockBits(bounds, ImageLockMode.WriteOnly, PixelFormat.Format1bppIndexed)
        ' We need bmd.stride * bmd.height bytes to store the pixels.
        Dim pixels(bmd.Stride * bmd.Height - 1) As Byte
        ' Now we need to do the pixels.
        ' We'll loop through the sourcePixels
        For index As Integer = 0 To sourcePixels.Length - 1
            If sourcePixels(index) = &HFFFFFFFF Then
                ' It is white. Wee need to set a bit.

                ' work out the x and y...
                ' x is the remainder when we divide the index by width.
                Dim x As Integer = index Mod bm.Width
                ' y is the number of times the width fits into the index.
                Dim y As Integer = index \ bm.Width
                ' So, what is the index for this (x,y) in the 1bpp array.
                ' We need to know which byte the bit is in.
                ' The index will be (all the bytes in the rows above) 
                ' + (number of pixels left of this one in the current row
                ' \ 8) as there are 8 bits in a byte.
                Dim index1bpp As Integer
                index1bpp = y * bmd.Stride
                index1bpp += x \ 8
                ' And which bit in the byte is it??
                ' This will create a bit mask based on x.
                Dim bitMask As Byte = &H80 >> (x Mod 8) ' or &H80 >> (x AND 7)
                ' Set the bit
                pixels(index1bpp) = pixels(index1bpp) Or bitMask
            End If
        Next
        ' And copy the pixels array to the bitmap
        Marshal.Copy(pixels, 0, bmd.Scan0, pixels.Length)
        bm.UnlockBits(bmd)
        Return bm
    End Function

    Private Function ResizeBitmap(ByVal bm As Bitmap, ByVal newSize As Size) As Bitmap
        Dim resizedBM As New Bitmap(newSize.Width, newSize.Height)
        Dim g As Graphics = Graphics.FromImage(resizedBM)
        g.DrawImage(bm, New Rectangle(0, 0, newSize.Width, newSize.Height))
        g.Dispose()
        Return resizedBM
    End Function

    Private Function GrabPixels(ByVal bm As Bitmap) As Integer()
        Dim pixels(bm.Width * bm.Height - 1) As Integer
        Dim bounds As New Rectangle(0, 0, bm.Width, bm.Height)
        Dim bmd As BitmapData = bm.LockBits(bounds, ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb)
        ' The memory holding the pixels is locked, we have a reference to the start of this
        ' memory provided by the bitmapdata object: the Scan0.
        ' We can copy all the pixels out into the array with
        ' System.Runtime.InteropServices.Marshal.Copy:
        Marshal.Copy(bmd.Scan0, pixels, 0, pixels.Length)
        ' unlock
        bm.UnlockBits(bmd)
        ' return the pixels in the array.
        ' The first element of the array is the top left pixel's color.
        ' Then come all the pixels in the first row. etc.
        Return pixels
    End Function

    Private filePath As String = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyPictures, "test.bmp")
    Private saveFilePath As String = IO.Path.Combine(My.Computer.FileSystem.SpecialDirectories.MyPictures, "test2.bmp")

    Public Sub New()
        InitializeComponent()
        Dim original As Bitmap = Bitmap.FromFile("C:\Users\Adam S. Leven\AppData\Local\Temp\IntegraOLabel.png")
        Dim newSize As New Size(original.Width * 2, original.Height * 2)
        Dim resized32bpp As Bitmap = ResizeBitmap(original, newSize)
        Dim pixels() As Integer = GrabPixels(resized32bpp)
        For Each p As Integer In pixels
        Next
        Dim resized1bpp As Bitmap = CreateBitmap(pixels, newSize)
        ' Finally we can save it!
        resized1bpp.Save(saveFilePath, ImageFormat.Bmp)
    End Sub

    ''This function is another attempt at redrawing the image as a bitmap
    Private Shared Function ConvertImageRedraw(filepath As String) As String
        Try

            'Open image
            Dim sourceImg As Bitmap = New Bitmap(filepath)
            Dim bmp As Bitmap = New Bitmap(sourceImg.Width, sourceImg.Height, PixelFormat.Format24bppRgb)

            Using gr As Graphics = Graphics.FromImage(bmp)
                gr.DrawImage(sourceImg, New Rectangle(0, 0, bmp.Width, bmp.Height))
            End Using

            'Set new path for the converted image
            filepath = IO.Path.GetTempPath & "IntegraCABLabel.bmp"

            'Save opened image as a bitmap file type
            sourceImg.Save(filepath, Imaging.ImageFormat.Bmp)

        Catch ex As Exception

            MsgBox("Failed to convert image, " & ex.Message, vbRetryCancel, "CAB Hermes+ Printer")

        End Try
        ConvertImageRedraw = filepath
    End Function

    'this function strips the color information from the image and resaves it
    Private Shared Sub To4BitGrayScale(ByVal b As Bitmap)
        Dim bd As BitmapData = b.LockBits(New Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb)
        Dim arr(bd.Width * bd.Height * 3 - 1) As Byte
        Marshal.Copy(bd.Scan0, arr, 0, arr.Length)

        For i As Integer = 0 To arr.Length - 1 Step 3
            Dim c As Color = Color.FromArgb(255, arr(i), arr(i + 1), arr(i + 2))

            ' Convert c to grayscale however you want; weighted, average, whatever.

            arr(i) = c.R
            arr(i + 1) = c.G
            arr(i + 2) = c.B
        Next

        Marshal.Copy(arr, 0, bd.Scan0, arr.Length)
        b.UnlockBits(bd)
    End Sub
    Private Shared Function ConvertImageBasic(filepath As String) As String

        Try

            'Open image
            Dim sourceImg As Bitmap = New Bitmap(filepath)

            'Set new path for the converted image
            filepath = IO.Path.GetTempPath & "IntegraCABLabel.Png"

            'Save opened image as a bitmap file type
            sourceImg.Save(filepath, ImageFormat.Png)

        Catch ex As Exception

            MsgBox("Failed to convert image, " & ex.Message, vbRetryCancel, "CAB Hermes+ Printer")

        End Try

        ConvertImageBasic = filepath
    End Function

    Public Shared Sub ConvertImageHex(filepath As String)
        Dim f_convOutBytes() As Byte = File.ReadAllBytes(filepath)
        Dim totalStreamLength As Int32 = f_convOutBytes.Length
        Dim labelOutStream(totalStreamLength) As Byte
        f_convOutBytes.CopyTo(labelOutStream, 0)
        Dim HexValue As String = String.Empty

        Dim labelPath As String = IO.Path.GetTempPath & "testasci.lbl"
        If File.Exists(labelPath) Then
            File.Delete(labelPath)
        End If
        Dim fs As FileStream = New FileStream(labelPath, FileMode.OpenOrCreate, FileAccess.ReadWrite)
        Dim s As StreamWriter = New StreamWriter(fs)
        s.Close()
        fs.Close()
        fs = New FileStream(labelPath, FileMode.OpenOrCreate, FileAccess.ReadWrite)
        s = New StreamWriter(fs)

        Dim i As Int32 = 0
        For Each c In f_convOutBytes
            i += 1
            If i > 38 Then
                s.Write(vbCrLf)
                i = 0
            End If
            HexValue = Convert.ToString(Convert.ToInt32(c), 16)
            s.Write(HexValue.ToUpper)
        Next
        s.Close()
        fs.Close()
        If Not s Is Nothing Then s = Nothing
        If Not fs Is Nothing Then fs = Nothing
    End Sub

    '--------------------------------------------------- NOT TRUE GRAYSCALE ---------------------------------------------

    'This function redraws the image as a 24bpp rgb image, and according to the file is a png
    Private Shared Function ConvertImageToPng(filepath As String) As String
        Dim org As Bitmap = New Bitmap(filepath)
        Dim bm As New Bitmap(org.Width, org.Height, PixelFormat.Format24bppRgb)
        bm.SetResolution(org.HorizontalResolution, org.VerticalResolution)
        Dim g As Graphics = Graphics.FromImage(bm)
        g.DrawImage(org, 0, 0)
        'Set new path for the converted image
        filepath = IO.Path.GetTempPath & "IntegraCABLabel.png"
        'Save opened image as a bitmap file type
        g.Dispose()
        bm.Save(filepath)
        ConvertImageToPng = filepath
    End Function

    Private Shared Function ConvertImageMagick(filepath As String) As String
        Using image As New MagickImage(filepath)
            Dim w As Int16 = image.Width()
            Dim h As Int16 = image.Height()
            image.Format = MagickFormat.Gif87
            filepath = IO.Path.GetTempPath & "IntegraCABLabel.gif"
            image.Write(filepath)
        End Using
        ConvertImageMagick = filepath
    End Function

    Private Shared Function ConvertImageManually(filepath As String) As String
        Using image As New Bitmap(filepath)
            'Do Stuff
            'Convert it to 4BPP
            Using imageConv = BitmapEncoder.ConvertBitmapToSpecified(image, 4)
                'Update the filepath for saving
                filepath = IO.Path.GetTempPath & "IntegraCABLabel.png"
                'Save to disk
                imageConv.Save(filepath)
            End Using
        End Using
        ConvertImageManually = filepath
    End Function
    ''Escape any ascii escapes
    'For i As Int32 = 0 To f_convOutBytes.Length
    'If f_convOutBytes(i) = &H1B Then
    'ReDim Preserve f_convOutBytes(f_convOutBytes.Length + 1)
    ''shift entire array
    'Dim arrCopy(f_convOutBytes.Length + 1) As Byte
    '        Array.Copy(f_convOutBytes, 0, arrCopy, 0, i)
    '        arrCopy(i) = &H1B
    '        Array.Copy(f_convOutBytes, i, arrCopy, i + 1, f_convOutBytes.Length - i)
    '        f_convOutBytes = arrCopy
    '        i = i + 1
    '    End If
    'Next
    '' 4-bit colormap, non-interlaced
    'Private Shared Function ConvertImage(filepath As String) As String
    '    'Load your image
    '    Using B As New Bitmap(filepath)
    '        'Do Stuff
    '        'Convert it to 4BPP
    '        Using I = BitmapEncoder.ConvertBitmapToSpecified(B, 8)
    '            'Update the filepath for saving
    '            filepath = IO.Path.GetTempPath & "IntegraCABLabel.png"
    '            'Save to disk
    '            I.Save(filepath)
    '        End Using
    '    End Using
    '    ConvertImage = filepath
    'End Function
End Class