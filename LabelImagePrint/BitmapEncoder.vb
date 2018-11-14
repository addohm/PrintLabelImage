Imports System.Drawing
Imports System.Runtime.InteropServices

''' <summary>
'''     Copies a bitmap into a 1bpp/4bpp/8bpp bitmap of the same dimensions, fast
''' </summary>
Public Class BitmapEncoder

    ''' <param name="b">original bitmap</param>
    ''' <param name="bpp">1 or 8, target bpp</param>
    ''' <returns>a 1bpp copy of the bitmap</returns>
    Public Shared Function ConvertBitmapToSpecified(b As Bitmap, bpp As Integer) As Bitmap
        Select Case bpp
            Case 1
            Case 4
            Case 8
            Case Else
                Throw New ArgumentException("bpp must be 1, 4 or 8")
        End Select

        ' Plan: built into Windows GDI is the ability to convert
        ' bitmaps from one format to another. Most of the time, this
        ' job is actually done by the graphics hardware accelerator card
        ' and so is extremely fast. The rest of the time, the job is done by
        ' very fast native code.
        ' We will call into this GDI functionality from C#. Our plan:
        ' (1) Convert our Bitmap into a GDI hbitmap (ie. copy unmanaged->managed)
        ' (2) Create a GDI monochrome hbitmap
        ' (3) Use GDI "BitBlt" function to copy from hbitmap into monochrome (as above)
        ' (4) Convert the monochrone hbitmap into a Bitmap (ie. copy unmanaged->managed)

        Dim w As Integer = b.Width, h As Integer = b.Height
        Dim hbm As IntPtr = b.GetHbitmap()
        ' this is step (1)
        '
        ' Step (2): create the monochrome bitmap.
        ' "BITMAPINFO" is an interop-struct which we define below.
        ' In GDI terms, it's a BITMAPHEADERINFO followed by an array of two RGBQUADs
        Dim bmi As New Bitmapinfo()
        bmi.biSize = 40
        ' the size of the BITMAPHEADERINFO struct
        bmi.biWidth = w
        bmi.biHeight = h
        bmi.biPlanes = 1
        ' "planes" are confusing. We always use just 1. Read MSDN for more info.
        bmi.biBitCount = CShort(bpp)
        ' ie. 1bpp or 8bpp
        bmi.biCompression = _biRgb
        ' ie. the pixels in our RGBQUAD table are stored as RGBs, not palette indexes
        bmi.biSizeImage = CUInt((((w + 7) And &HFFFFFFF8) * h / 8))
        bmi.biXPelsPerMeter = 1000000
        ' not really important
        bmi.biYPelsPerMeter = 1000000
        ' not really important
        ' Now for the colour table.
        Dim ncols As UInteger = CUInt(1) << bpp
        ' 2 colours for 1bpp; 256 colours for 8bpp
        bmi.biClrUsed = ncols
        bmi.biClrImportant = ncols
        bmi.cols = New UInteger(255) {}
        ' The structure always has fixed size 256, even if we end up using fewer colours
        If bpp = 1 Then
            bmi.cols(0) = Makergb(0, 0, 0)
            bmi.cols(1) = Makergb(255, 255, 255)
        ElseIf bpp = 4 Then
            bmi.biClrUsed = 16
            bmi.biClrImportant = 16
            Dim colv1 = New Integer(15) {8, 24, 38, 56, 72, 88, 104, 120, 136, 152, 168, 184, 210, 216, 232, 248}

            For i = 0 To 15
                bmi.cols(i) = Makergb(colv1(i), colv1(i), colv1(i))
            Next
        ElseIf bpp = 8 Then
            For i = 0 To ncols - 1
                bmi.cols(i) = Makergb(i, i, i)
            Next
        End If
        ' For 8bpp we've created an palette with just greyscale colours.
        ' You can set up any palette you want here. Here are some possibilities:
        ' greyscale: for (int i=0; i<256; i++) bmi.cols[i]=MAKERGB(i,i,i);
        ' rainbow: bmi.biClrUsed=216; bmi.biClrImportant=216; int[] colv=new int[6]{0,51,102,153,204,255};
        '          for (int i=0; i<216; i++) bmi.cols[i]=MAKERGB(colv[i/36],colv[(i/6)%6],colv[i%6]);
        ' optimal: a difficult topic: http://en.wikipedia.org/wiki/Color_quantization
        '
        ' Now create the indexed bitmap "hbm0"
        Dim bits0 As IntPtr
        ' not used for our purposes. It returns a pointer to the raw bits that make up the bitmap.
        Dim hbm0 As IntPtr = CreateDIBSection(IntPtr.Zero, bmi, _dibRgbColors, bits0, IntPtr.Zero, 0)
        '
        ' Step (3): use GDI's BitBlt function to copy from original hbitmap into monocrhome bitmap
        ' GDI programming is kind of confusing... nb. The GDI equivalent of "Graphics" is called a "DC".
        Dim sdc As IntPtr = GetDC(IntPtr.Zero)
        ' First we obtain the DC for the screen
        ' Next, create a DC for the original hbitmap
        Dim hdc As IntPtr = CreateCompatibleDC(sdc)
        SelectObject(hdc, hbm)
        ' and create a DC for the monochrome hbitmap
        Dim hdc0 As IntPtr = CreateCompatibleDC(sdc)
        SelectObject(hdc0, hbm0)
        ' Now we can do the BitBlt:
        BitBlt(hdc0, 0, 0, w, h, hdc,
               0, 0, _srccopy)
        ' Step (4): convert this monochrome hbitmap back into a Bitmap:
        Dim b0 As Bitmap = Bitmap.FromHbitmap(hbm0)
        '
        ' Finally some cleanup.
        DeleteDC(hdc)
        DeleteDC(hdc0)
        ReleaseDC(IntPtr.Zero, sdc)
        DeleteObject(hbm)
        DeleteObject(hbm0)
        '
        Return b0
    End Function

    Private Shared _srccopy As Integer = &HCC0020
    Private Shared _biRgb As UInteger = 0
    Private Shared _dibRgbColors As UInteger = 0

    <DllImport("gdi32.dll")>
    Private Shared Function DeleteObject(hObject As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll")>
    Private Shared Function GetDC(hwnd As IntPtr) As IntPtr
    End Function

    <DllImport("gdi32.dll")>
    Private Shared Function CreateCompatibleDC(hdc As IntPtr) As IntPtr
    End Function

    <DllImport("user32.dll")>
    Private Shared Function ReleaseDC(hwnd As IntPtr, hdc As IntPtr) As Integer
    End Function

    <DllImport("gdi32.dll")>
    Private Shared Function DeleteDC(hdc As IntPtr) As Integer
    End Function

    <DllImport("gdi32.dll")>
    Private Shared Function SelectObject(hdc As IntPtr, hgdiobj As IntPtr) As IntPtr
    End Function

    <DllImport("gdi32.dll")>
    Private Shared Function BitBlt(hdcDst As IntPtr, xDst As Integer, yDst As Integer, w As Integer, h As Integer,
                                   hdcSrc As IntPtr,
                                   xSrc As Integer, ySrc As Integer, rop As Integer) As Integer
    End Function

    <DllImport("gdi32.dll")>
    Private Shared Function CreateDIBSection(hdc As IntPtr, ByRef bmi As Bitmapinfo, usage As UInteger,
                                             ByRef bits As IntPtr, hSection As IntPtr, dwOffset As UInteger) As IntPtr
    End Function

    <StructLayout(LayoutKind.Sequential)>
    Private Structure Bitmapinfo
        Public biSize As UInteger
        Public biWidth As Integer, biHeight As Integer
        Public biPlanes As Short, biBitCount As Short
        Public biCompression As UInteger, biSizeImage As UInteger
        Public biXPelsPerMeter As Integer, biYPelsPerMeter As Integer
        Public biClrUsed As UInteger, biClrImportant As UInteger
        <MarshalAs(UnmanagedType.ByValArray, SizeConst:=256)> Public cols As UInteger()
    End Structure

    Private Shared Function Makergb(r As Integer, g As Integer, b As Integer) As UInteger
        Return CUInt((b And 255)) Or CUInt(((r And 255) << 8)) Or CUInt(((g And 255) << 16))
    End Function

    Private Sub New()
    End Sub

End Class