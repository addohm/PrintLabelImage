<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FormTestSettings
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
        Me.tbInterval = New System.Windows.Forms.TextBox()
        Me.label4 = New System.Windows.Forms.Label()
        Me.cbUOM = New System.Windows.Forms.ComboBox()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnShow = New System.Windows.Forms.Button()
        Me.label3 = New System.Windows.Forms.Label()
        Me.cbResolution = New System.Windows.Forms.ComboBox()
        Me.tbHeight = New System.Windows.Forms.TextBox()
        Me.tbWidth = New System.Windows.Forms.TextBox()
        Me.label2 = New System.Windows.Forms.Label()
        Me.label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'tbInterval
        '
        Me.tbInterval.Location = New System.Drawing.Point(81, 68)
        Me.tbInterval.MaxLength = 200
        Me.tbInterval.Name = "tbInterval"
        Me.tbInterval.Size = New System.Drawing.Size(55, 20)
        Me.tbInterval.TabIndex = 21
        Me.tbInterval.Text = "50"
        '
        'label4
        '
        Me.label4.AutoSize = True
        Me.label4.Location = New System.Drawing.Point(13, 71)
        Me.label4.Name = "label4"
        Me.label4.Size = New System.Drawing.Size(62, 13)
        Me.label4.TabIndex = 20
        Me.label4.Text = "Dot Interval"
        '
        'cbUOM
        '
        Me.cbUOM.FormattingEnabled = True
        Me.cbUOM.Items.AddRange(New Object() {"mm", "in"})
        Me.cbUOM.Location = New System.Drawing.Point(142, 15)
        Me.cbUOM.Name = "cbUOM"
        Me.cbUOM.Size = New System.Drawing.Size(75, 21)
        Me.cbUOM.TabIndex = 19
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(142, 121)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 23)
        Me.btnSave.TabIndex = 18
        Me.btnSave.Text = "Save"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnShow
        '
        Me.btnShow.Location = New System.Drawing.Point(21, 121)
        Me.btnShow.Name = "btnShow"
        Me.btnShow.Size = New System.Drawing.Size(75, 23)
        Me.btnShow.TabIndex = 17
        Me.btnShow.Text = "Show"
        Me.btnShow.UseVisualStyleBackColor = True
        '
        'label3
        '
        Me.label3.AutoSize = True
        Me.label3.Location = New System.Drawing.Point(18, 97)
        Me.label3.Name = "label3"
        Me.label3.Size = New System.Drawing.Size(57, 13)
        Me.label3.TabIndex = 16
        Me.label3.Text = "Resolution"
        '
        'cbResolution
        '
        Me.cbResolution.FormattingEnabled = True
        Me.cbResolution.Items.AddRange(New Object() {"203 dpi", "305 dpi", "609 dpi"})
        Me.cbResolution.Location = New System.Drawing.Point(81, 94)
        Me.cbResolution.Name = "cbResolution"
        Me.cbResolution.Size = New System.Drawing.Size(136, 21)
        Me.cbResolution.TabIndex = 15
        '
        'tbHeight
        '
        Me.tbHeight.Location = New System.Drawing.Point(81, 41)
        Me.tbHeight.Name = "tbHeight"
        Me.tbHeight.Size = New System.Drawing.Size(55, 20)
        Me.tbHeight.TabIndex = 14
        Me.tbHeight.Text = "0.333"
        '
        'tbWidth
        '
        Me.tbWidth.Location = New System.Drawing.Point(81, 15)
        Me.tbWidth.Name = "tbWidth"
        Me.tbWidth.Size = New System.Drawing.Size(55, 20)
        Me.tbWidth.TabIndex = 13
        Me.tbWidth.Text = "1.4"
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.Location = New System.Drawing.Point(37, 44)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(38, 13)
        Me.label2.TabIndex = 12
        Me.label2.Text = "Height"
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.Location = New System.Drawing.Point(40, 18)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(35, 13)
        Me.label1.TabIndex = 11
        Me.label1.Text = "Width"
        '
        'FormTestSettings
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(237, 161)
        Me.Controls.Add(Me.tbInterval)
        Me.Controls.Add(Me.label4)
        Me.Controls.Add(Me.cbUOM)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.btnShow)
        Me.Controls.Add(Me.label3)
        Me.Controls.Add(Me.cbResolution)
        Me.Controls.Add(Me.tbHeight)
        Me.Controls.Add(Me.tbWidth)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.label1)
        Me.Name = "FormTestSettings"
        Me.Text = "Label Test Settings"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Private WithEvents tbInterval As Windows.Forms.TextBox
    Private WithEvents label4 As Windows.Forms.Label
    Private WithEvents cbUOM As Windows.Forms.ComboBox
    Private WithEvents btnSave As Windows.Forms.Button
    Private WithEvents btnShow As Windows.Forms.Button
    Private WithEvents label3 As Windows.Forms.Label
    Private WithEvents cbResolution As Windows.Forms.ComboBox
    Private WithEvents tbHeight As Windows.Forms.TextBox
    Private WithEvents tbWidth As Windows.Forms.TextBox
    Private WithEvents label2 As Windows.Forms.Label
    Private WithEvents label1 As Windows.Forms.Label
End Class
