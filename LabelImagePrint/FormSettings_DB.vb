Public Class FormSettingsDb

    Private Sub SettingsForm_DB_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        With My.Settings
            txtSetServer.Text = .sqlServer
            txtSetDatabase.Text = .sqlDBName
            txtSetUsername.Text = .sqlUsername
            txtSetPassword.Text = .sqlPassword
        End With
    End Sub

    Private Sub btnOkay_Click(sender As Object, e As EventArgs) Handles btnOkay.Click
        With My.Settings
            .sqlServer = txtSetServer.Text
            .sqlDBName = txtSetDatabase.Text
            .sqlUsername = txtSetUsername.Text
            .sqlPassword = txtSetPassword.Text
            .Save()
            Me.Close()
        End With
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

End Class