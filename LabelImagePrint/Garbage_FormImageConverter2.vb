
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Windows.Forms

Public Class FormImageConverter2

    ' This example creates a PictureBox control on the form and draws to it. 
    ' This example assumes that the Form_Load event handler method is connected 
    ' to the Load event of the form.
    Private pictureBox1 As New PictureBox()
    Private fnt As New Font("Arial", 10)

    Private Sub Form1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load
        ' Dock the PictureBox to the form and set its background to white.
        pictureBox1.Dock = DockStyle.Fill
        pictureBox1.BackColor = Color.White
        ' Connect the Paint event of the PictureBox to the event handler method.
        AddHandler pictureBox1.Paint, AddressOf Me.pictureBox1_Paint

        ' Add the PictureBox control to the Form.
        Me.Controls.Add(pictureBox1)
    End Sub 'Form1_Load

    Private Sub pictureBox1_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs)

        ' Add any initialization after the InitializeComponent() call.
        Dim image As New Bitmap("C:\Users\Adam S. Leven\AppData\Local\Temp\IntegraOLabel.png")
        Dim imageAttributes As New ImageAttributes()
        Dim width As Integer = image.Width
        Dim height As Integer = image.Height
        Dim colorMap As New ColorMap()

        colorMap.OldColor = Color.FromArgb(255, 255, 0, 0) ' opaque red
        colorMap.NewColor = Color.FromArgb(255, 0, 0, 255) ' opaque blue
        Dim remapTable As ColorMap() = {colorMap}

        imageAttributes.SetRemapTable(remapTable, ColorAdjustType.Bitmap)

        e.Graphics.DrawImage(image, 10, 10, width, height)

        ' Pass in the destination rectangle (2nd argument), the upper-left corner 
        ' (3rd and 4th arguments), width (5th argument),  and height (6th 
        ' argument) of the source rectangle.
        e.Graphics.DrawImage(
           image,
           New Rectangle(10, image.Height + 10, width, height),
           0, 0,
           width,
           height,
           GraphicsUnit.Pixel,
           imageAttributes)
        image.Save("C:\Users\Adam S. Leven\AppData\Local\Temp\IntegraCABLabel.png", Imaging.ImageFormat.Png)
    End Sub 'pictureBox1_Paint

End Class