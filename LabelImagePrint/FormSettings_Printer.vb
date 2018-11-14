Imports System.Data.SqlClient
Imports System.Windows.Forms

Public Class FormSettingsPrinter

    Public location As Integer

    Private Sub ApplicationSetup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'load all txtboxes with data from db
    End Sub

    Private Sub btnSettingsApply_Click(sender As Object, e As EventArgs) Handles btnSettingsApply.Click
        'Write values into settings
        WriteSettingsToSettings()
    End Sub

    Private Sub btnSettingsCancel_Click(sender As Object, e As EventArgs) Handles btnSettingsCancel.Click
        'close the form and revert any changes
        ReadSettingsFromDBtoSettings(location)
        Close()
    End Sub

    Private Sub btnSettingsOkay_Click(sender As Object, e As EventArgs) Handles btnSettingsOkay.Click
        'Write values into settings and database, close the form
        WriteSettingsToSettings()
        WriteSettingsFromSettingsToDb()
        Close()
    End Sub

    Private Sub btnSettingsTestPrint_Click(sender As Object, e As EventArgs) Handles btnSettingsTestPrint.Click
        'Send test print job using the currently displayed settings (me.settings)
    End Sub

    Private Sub DatabaseSettingsToolStripMenuItem_Click(sender As Object, e As EventArgs) _
        Handles DatabaseSettingsToolStripMenuItem.Click
        Dim dbSettingsForm As New FormSettingsDb
        dbSettingsForm.Show()
    End Sub

    Private Sub ExitToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ExitToolStripMenuItem.Click
        Close()
    End Sub

    Private Sub AboutToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AboutToolStripMenuItem.Click
        Dim aboutForm As New FormAbout
        aboutForm.Show()
    End Sub

    Friend Sub UpdateFormFields()
        With My.Settings
            txtXFPIP.Text = .XFP_PrinterAddr
            txtSetXFPH.Text = .XFP_Horizontal
            txtSetXFPV.Text = .XFP_Vertical
            txtSetXFPS.Text = .XFP_Size
            txtSetXFPR.Text = .XFP_Rotation
            txtSFPIP.Text = .SFP_PrinterAddr
            txtSetSFPH.Text = .SFP_Horizontal
            txtSetSFPV.Text = .SFP_Vertical
            txtSetSFPS.Text = .SFP_Size
            txtSetSFPR.Text = .SFP_Rotation
            txtClamshellIP.Text = .Clamshell_PrinterAddr
            txtSetClamshellH.Text = .Clamshell_Horizontal
            txtSetClamshellV.Text = .Clamshell_Vertical
            txtSetClamshellS.Text = .Clamshell_Size
            txtSetClamshellR.Text = .Clamshell_Rotation
            txtShippingAIP.Text = .ShippingA_PrinterAddr
            txtSetShippingAH.Text = .ShippingA_Horizontal
            txtSetShippingAV.Text = .ShippingA_Vertical
            txtSetShippingAS.Text = .ShippingA_Size
            txtSetShippingAR.Text = .ShippingA_Rotation
            txtShippingMIP.Text = .ShippingM_PrinterAddr
            txtSetShippingMH.Text = .ShippingM_Horizontal
            txtSetShippingMV.Text = .ShippingM_Vertical
            txtSetShippingMS.Text = .ShippingM_Size
            txtSetShippingMR.Text = .ShippingM_Rotation
        End With
    End Sub

    Private Sub WriteSettingsToSettings()
        With My.Settings
            .XFP_PrinterAddr = txtXFPIP.Text
            .XFP_Horizontal = txtSetXFPH.Text
            .XFP_Vertical = txtSetXFPV.Text
            .XFP_Size = txtSetXFPS.Text
            .XFP_Rotation = txtSetXFPR.Text
            .SFP_PrinterAddr = txtSFPIP.Text
            .SFP_Horizontal = txtSetSFPH.Text
            .SFP_Vertical = txtSetSFPV.Text
            .SFP_Size = txtSetSFPS.Text
            .SFP_Rotation = txtSetSFPR.Text
            .Clamshell_PrinterAddr = txtClamshellIP.Text
            .Clamshell_Horizontal = txtSetClamshellH.Text
            .Clamshell_Vertical = txtSetClamshellV.Text
            .Clamshell_Size = txtSetClamshellS.Text
            .Clamshell_Rotation = txtSetClamshellR.Text
            .ShippingA_PrinterAddr = txtShippingAIP.Text
            .ShippingA_Horizontal = txtSetShippingAH.Text
            .ShippingA_Vertical = txtSetShippingAV.Text
            .ShippingA_Size = txtSetShippingAS.Text
            .ShippingA_Rotation = txtSetShippingAR.Text
            .ShippingM_PrinterAddr = txtShippingMIP.Text
            .ShippingM_Horizontal = txtSetShippingMH.Text
            .ShippingM_Vertical = txtSetShippingMV.Text
            .ShippingM_Size = txtSetShippingMS.Text
            .ShippingM_Rotation = txtSetShippingMR.Text
            .Save()
        End With
    End Sub

    Private Sub WriteSettingsFromSettingsToDb()
        Using connection = New SqlConnection()

            Using cmd As New SqlCommand()

                Try

                    connection.ConnectionString = My.Settings.ConnStr
                    With cmd
                        .Connection = connection
                        connection.Open()

                        .CommandText = "update system_settings set setting_value = '" & My.Settings.XFP_Horizontal &
                                       "' where setting_name = 'LABEL_XFPh'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.XFP_Vertical &
                                       "' where setting_name = 'LABEL_XFPv'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.XFP_Size &
                                       "' where setting_name = 'LABEL_XFPs'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.XFP_Rotation &
                                       "' where setting_name = 'LABEL_XFPr'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.SFP_Horizontal &
                                       "' where setting_name = 'LABEL_SFPh'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.SFP_Vertical &
                                       "' where setting_name = 'LABEL_SFPv'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.SFP_Size &
                                       "' where setting_name = 'LABEL_SFPs'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.SFP_Rotation &
                                       "' where setting_name = 'LABEL_SFPr'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.Clamshell_Horizontal &
                                       "' where setting_name = 'LABEL_Clamshellh'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.Clamshell_Vertical &
                                       "' where setting_name = 'LABEL_Clamshellv'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.Clamshell_Size &
                                       "' where setting_name = 'LABEL_Clamshells'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.Clamshell_Rotation &
                                       "' where setting_name = 'LABEL_Clamshellr'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.ShippingA_Horizontal &
                                       "' where setting_name = 'LABEL_ShippinghA'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.ShippingA_Vertical &
                                       "' where setting_name = 'LABEL_ShippingvA'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.ShippingA_Size &
                                       "' where setting_name = 'LABEL_ShippingsA'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.ShippingA_Rotation &
                                       "' where setting_name = 'LABEL_ShippingrA'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.ShippingM_Horizontal &
                                       "' where setting_name = 'LABEL_ShippinghM'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.ShippingM_Vertical &
                                       "' where setting_name = 'LABEL_ShippingvM'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.ShippingM_Size &
                                       "' where setting_name = 'LABEL_ShippingsM'"
                        .ExecuteNonQuery()
                        .CommandText = "update system_settings set setting_value = '" & My.Settings.ShippingM_Rotation &
                                       "' where setting_name = 'LABEL_ShippingrM'"
                        .ExecuteNonQuery()
                    End With
                Catch ex As Exception
                    MsgBox(ex.Message & " please check your database settings")
                Finally

                End Try
            End Using
            connection.Close()
        End Using
    End Sub

    Sub VisitLink(url As String)
        ' Change the color of the link text by setting LinkVisited
        ' to True.
        urlSFP.LinkVisited = True
        ' Call the Process.Start method to open the default browser
        ' with a URL:
        Process.Start("http://" & url)
    End Sub

    Private Sub urlXFP_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles urlXFP.LinkClicked
        VisitLink(txtXFPIP.Text)
    End Sub

    Private Sub urlSFP_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles urlSFP.LinkClicked
        VisitLink(txtSFPIP.Text)
    End Sub

    Private Sub urlClamshell_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) _
        Handles urlClamshell.LinkClicked
        VisitLink(txtClamshellIP.Text)
    End Sub

    Private Sub urlShippingA_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) _
        Handles urlShippingA.LinkClicked
        VisitLink(txtShippingAIP.Text)
    End Sub

    Private Sub urlShippingM_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) _
        Handles urlShippingM.LinkClicked
        VisitLink(txtShippingMIP.Text)
    End Sub

End Class