Imports System.Data.SqlClient
Imports System.IO

Public Class PdfEncoder

    Sub readPdfBinaryEncodedFile()
        Dim filePath As String = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Test Optic Labels")
        Dim fileName As String = "SFPRobotBinary.txt"
        Dim pdfImageData As Byte() = File.ReadAllBytes(filePath)
        Dim cmdStoreBlob As New SqlCommand
        Dim param1 As New SqlParameter("@imageData", SqlDbType.VarBinary, pdfImageData.Length,
            ParameterDirection.Input, False, 0, 0, Nothing, DataRowVersion.Current, pdfImageData)

        Using connection = New SqlConnection()
            Using cmd As New SqlCommand()
                connection.ConnectionString = My.Settings.ConnStr
                With cmd
                    .Connection = connection
                    .CommandText = "UPDATE [AOF_LABELS] Set [LABEL_IMAGE] = @imageData WHERE [LABEL_TYPE] = 'O'"
                    connection.Open()
                    .Parameters.Add(param1)
                    .ExecuteNonQuery()
                End With
            End Using
        End Using
    End Sub

End Class