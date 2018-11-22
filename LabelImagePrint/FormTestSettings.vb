Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms

Public Class FormTestSettings
    Dim pb As New PictureBox
    Dim fnt As New Font("Arial", 10)
    Dim g As Graphics
    Dim canvass As New PaintCanvass
    Dim pencil As New Pen(Color.Black, 2)

    Private Sub FormTestSettings_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cbResolution.SelectedText = "609 dpi"
        cbUOM.SelectedText = "in"
    End Sub

    Private Sub pb_Paint(sender As Object, e As PaintEventArgs)
        g = e.Graphics
        g.DrawString("This will display a representation of\nwhat the sample label will look like",
    fnt, Brushes.Black, New Point(15, 50))
    End Sub

    Private Sub btnShow_Click(sender As Object, e As EventArgs) Handles btnShow.Click
        pb.BackColor = Color.White
        pb.Location = New Point(270, 15)
        pb.Width = 250
        pb.Height = 130
        AddHandler pb.Paint, AddressOf Me.pb_Paint
        Controls.Add(pb)
        Me.Width = 550
        SetupCanvass()
        Using g = pb.CreateGraphics()
            g.Clear(Color.White)

            Dim hpfact As Integer = Convert.ToInt32(CDbl(pb.Height) / CDbl(canvass.horizontalLinesQuantity))
            Dim hp As Integer = hpfact
            For i As Integer = 0 To canvass.horizontalLinesQuantity - 2 Step 1
                g.DrawLine(pencil, New Point(0, hp), New Point(pb.Width, hp))
                hp = hp + hpfact
            Next
            Dim vpfact As Integer = Convert.ToInt32(CDbl(pb.Width) / CDbl(canvass.verticalLinesQuantity))
            Dim vp As Integer = vpfact
            For i As Integer = 0 To canvass.verticalLinesQuantity - 2 Step 1
                g.DrawLine(pencil, New Point(vp, 0), New Point(vp, pb.Height))
                vp = vp + vpfact
            Next
        End Using
    End Sub

    Private Sub SetupCanvass()
        canvass.Width = Convert.ToDouble(tbWidth.Text)
        canvass.Height = Convert.ToDouble(tbHeight.Text)
        canvass.Resolution = Convert.ToInt32(cbResolution.Text.Substring(0, 3))
        canvass.UOM = cbUOM.Text
        canvass.Interval = Convert.ToInt32(tbInterval.Text)
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        SetupCanvass()
        Using bmp As New Bitmap(canvass.hdots, canvass.vdots)
            Using g As Graphics = Graphics.FromImage(bmp)
                Using brush As New SolidBrush(Color.FromArgb(255, 255, 255))
                    g.FillRectangle(brush, 0, 0, canvass.hdots, canvass.vdots)
                End Using
                Dim hp As Integer = canvass.Interval
                For i As Integer = 0 To canvass.horizontalLinesQuantity - 2 Step 1
                    g.DrawLine(pencil, New Point(0, hp), New Point(canvass.hdots, hp))
                    hp = hp + canvass.Interval
                Next
                Dim vp As Integer = canvass.Interval
                For i As Integer = 0 To canvass.verticalLinesQuantity - 2 Step 1
                    g.DrawLine(pencil, New Point(vp, 0), New Point(vp, canvass.vdots))
                    vp = vp + canvass.Interval
                Next
            End Using
            Dim filePath As String = Path.Combine(Path.GetTempPath(), "IntegraLabelGrid.png")
            If File.Exists(filepath) Then
                File.Delete(filepath)
            End If
            bmp.Save(filePath)
        End Using
    End Sub
End Class

Class PaintCanvass
    Public Property Width() As Double
    Public Property Height() As Double
    Public Property UOM As String
    Public Property Resolution As Integer
    Public Property Interval As Integer

    Public Property hdots As Integer
        Get
            If UOM.ToLower() = "mm" Then
                Return Convert.ToInt32(Width * 0.03937008F * Resolution)
            Else
                Return Convert.ToInt32(Width * Resolution)
            End If
        End Get
        Private Set(value As Integer)

        End Set
    End Property

    Public Property vdots As Integer
        Get
            If UOM.ToLower() = "mm" Then
                Return Convert.ToInt32(Height * 0.03937008F * Resolution)
            Else
                Return Convert.ToInt32(Height * Resolution)
            End If
        End Get
        Private Set(value As Integer)

        End Set
    End Property

    Public Property horizontalLinesQuantity As Integer
        Get
            Return vdots / Interval
        End Get
        Private Set(value As Integer)

        End Set
    End Property

    Public Property verticalLinesQuantity As Integer
        Get
            Return hdots / Interval
        End Get
        Private Set(value As Integer)

        End Set
    End Property

End Class