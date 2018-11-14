Imports System.Drawing
Imports System.Drawing.Printing

Public Class ShippingMPrintForm
    Public ImagePath As String = ""

    Public Sub New()
        ' The Windows Forms Designer requires the following call.
        InitializeComponent()
    End Sub

    ' Specifies what happens when the user clicks the Button.
    Friend Sub Print()
        Dim margins As New Margins(70, 0, 35, 0)
        Dim paperSize As New PaperSize("Custom Paper Size", 400, 600)
        Try
            printDocument.PrinterSettings.PrinterName = "Manual Ship Printer"
            'Set internally at the printer
            'printDocument.DefaultPageSettings.PaperSize = paperSize
            'printDocument.DefaultPageSettings.Margins = margins
            printDocument.Print()
        Catch ex As Exception
            MsgBox("An error occurred while printing",
                   ex.ToString())
        Finally
            Close()
        End Try
    End Sub

    ' Specifies what happens when the PrintPage event is raised.
    Private Sub printDocument_PrintPage(sender As Object, ev As PrintPageEventArgs) Handles printDocument.PrintPage

        ' Draw a picture.
        ev.Graphics.DrawImage(Image.FromFile(ImagePath),
                              ev.Graphics.VisibleClipBounds)

        ' Indicate that this is the last page to print.
        ev.HasMorePages = False
    End Sub

End Class