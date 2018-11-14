<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormSettingsDb
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.txtSetServer = New System.Windows.Forms.TextBox()
        Me.btnOkay = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtSetDatabase = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtSetUsername = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtSetPassword = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'txtSetServer
        '
        Me.txtSetServer.Location = New System.Drawing.Point(77, 12)
        Me.txtSetServer.Name = "txtSetServer"
        Me.txtSetServer.Size = New System.Drawing.Size(149, 20)
        Me.txtSetServer.TabIndex = 0
        '
        'btnOkay
        '
        Me.btnOkay.Location = New System.Drawing.Point(69, 137)
        Me.btnOkay.Name = "btnOkay"
        Me.btnOkay.Size = New System.Drawing.Size(75, 23)
        Me.btnOkay.TabIndex = 4
        Me.btnOkay.Text = "Okay"
        Me.btnOkay.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(151, 137)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(75, 23)
        Me.btnCancel.TabIndex = 5
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(30, 15)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(41, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Server:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 41)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(56, 13)
        Me.Label2.TabIndex = 8
        Me.Label2.Text = "Database:"
        '
        'txtSetDatabase
        '
        Me.txtSetDatabase.Location = New System.Drawing.Point(77, 38)
        Me.txtSetDatabase.Name = "txtSetDatabase"
        Me.txtSetDatabase.Size = New System.Drawing.Size(149, 20)
        Me.txtSetDatabase.TabIndex = 7
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(13, 67)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(58, 13)
        Me.Label3.TabIndex = 10
        Me.Label3.Text = "Username:"
        '
        'txtSetUsername
        '
        Me.txtSetUsername.Location = New System.Drawing.Point(77, 64)
        Me.txtSetUsername.Name = "txtSetUsername"
        Me.txtSetUsername.Size = New System.Drawing.Size(149, 20)
        Me.txtSetUsername.TabIndex = 9
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 93)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 13)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Password:"
        '
        'txtSetPassword
        '
        Me.txtSetPassword.Location = New System.Drawing.Point(77, 90)
        Me.txtSetPassword.Name = "txtSetPassword"
        Me.txtSetPassword.Size = New System.Drawing.Size(149, 20)
        Me.txtSetPassword.TabIndex = 11
        '
        'FormSettings_DB
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(239, 173)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.txtSetPassword)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtSetUsername)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.txtSetDatabase)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOkay)
        Me.Controls.Add(Me.txtSetServer)
        Me.Name = "FormSettingsDb"
        Me.Text = "Database Connection Settings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtSetServer As Windows.Forms.TextBox
    Friend WithEvents btnOkay As Windows.Forms.Button
    Friend WithEvents btnCancel As Windows.Forms.Button
    Friend WithEvents Label1 As Windows.Forms.Label
    Friend WithEvents Label2 As Windows.Forms.Label
    Friend WithEvents txtSetDatabase As Windows.Forms.TextBox
    Friend WithEvents Label3 As Windows.Forms.Label
    Friend WithEvents txtSetUsername As Windows.Forms.TextBox
    Friend WithEvents Label4 As Windows.Forms.Label
    Friend WithEvents txtSetPassword As Windows.Forms.TextBox
End Class
