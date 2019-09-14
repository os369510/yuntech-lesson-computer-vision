<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LabelingSettingWindow
    Inherits System.Windows.Forms.Form

    'Form 覆寫 Dispose 以清除元件清單。
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

    '為 Windows Form 設計工具的必要項
    Private components As System.ComponentModel.IContainer

    '注意: 以下為 Windows Form 設計工具所需的程序
    '可以使用 Windows Form 設計工具進行修改。
    '請不要使用程式碼編輯器進行修改。
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.FourNeighborhoodRadioButton = New System.Windows.Forms.RadioButton()
        Me.EightNeighborhoodRadioButton = New System.Windows.Forms.RadioButton()
        Me.SaveAndLeaveButton = New System.Windows.Forms.Button()
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.LeaveButton = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ObjectWhile = New System.Windows.Forms.RadioButton()
        Me.ObjectBlack = New System.Windows.Forms.RadioButton()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'FourNeighborhoodRadioButton
        '
        Me.FourNeighborhoodRadioButton.AutoSize = True
        Me.FourNeighborhoodRadioButton.Location = New System.Drawing.Point(38, 21)
        Me.FourNeighborhoodRadioButton.Name = "FourNeighborhoodRadioButton"
        Me.FourNeighborhoodRadioButton.Size = New System.Drawing.Size(113, 16)
        Me.FourNeighborhoodRadioButton.TabIndex = 0
        Me.FourNeighborhoodRadioButton.TabStop = True
        Me.FourNeighborhoodRadioButton.Text = "four-neighborhood"
        Me.FourNeighborhoodRadioButton.UseVisualStyleBackColor = True
        '
        'EightNeighborhoodRadioButton
        '
        Me.EightNeighborhoodRadioButton.AutoSize = True
        Me.EightNeighborhoodRadioButton.Location = New System.Drawing.Point(38, 43)
        Me.EightNeighborhoodRadioButton.Name = "EightNeighborhoodRadioButton"
        Me.EightNeighborhoodRadioButton.Size = New System.Drawing.Size(116, 16)
        Me.EightNeighborhoodRadioButton.TabIndex = 1
        Me.EightNeighborhoodRadioButton.TabStop = True
        Me.EightNeighborhoodRadioButton.Text = "eight-neighborhood"
        Me.EightNeighborhoodRadioButton.UseVisualStyleBackColor = True
        '
        'SaveAndLeaveButton
        '
        Me.SaveAndLeaveButton.AutoSize = True
        Me.SaveAndLeaveButton.Location = New System.Drawing.Point(12, 158)
        Me.SaveAndLeaveButton.Name = "SaveAndLeaveButton"
        Me.SaveAndLeaveButton.Size = New System.Drawing.Size(88, 23)
        Me.SaveAndLeaveButton.TabIndex = 2
        Me.SaveAndLeaveButton.Text = "Save and Leave"
        Me.SaveAndLeaveButton.UseVisualStyleBackColor = True
        '
        'ImageList1
        '
        Me.ImageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit
        Me.ImageList1.ImageSize = New System.Drawing.Size(16, 16)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        '
        'LeaveButton
        '
        Me.LeaveButton.AutoSize = True
        Me.LeaveButton.Location = New System.Drawing.Point(106, 158)
        Me.LeaveButton.Name = "LeaveButton"
        Me.LeaveButton.Size = New System.Drawing.Size(88, 23)
        Me.LeaveButton.TabIndex = 3
        Me.LeaveButton.Text = "Leave"
        Me.LeaveButton.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.FourNeighborhoodRadioButton)
        Me.GroupBox1.Controls.Add(Me.EightNeighborhoodRadioButton)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(182, 67)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "NeighborhoodSelect"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.ObjectWhile)
        Me.GroupBox2.Controls.Add(Me.ObjectBlack)
        Me.GroupBox2.Location = New System.Drawing.Point(14, 85)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(182, 67)
        Me.GroupBox2.TabIndex = 5
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "ObjectSelect"
        '
        'ObjectWhile
        '
        Me.ObjectWhile.AutoSize = True
        Me.ObjectWhile.Location = New System.Drawing.Point(38, 21)
        Me.ObjectWhile.Name = "ObjectWhile"
        Me.ObjectWhile.Size = New System.Drawing.Size(91, 16)
        Me.ObjectWhile.TabIndex = 0
        Me.ObjectWhile.TabStop = True
        Me.ObjectWhile.Text = "Object is white"
        Me.ObjectWhile.UseVisualStyleBackColor = True
        '
        'ObjectBlack
        '
        Me.ObjectBlack.AutoSize = True
        Me.ObjectBlack.Location = New System.Drawing.Point(38, 43)
        Me.ObjectBlack.Name = "ObjectBlack"
        Me.ObjectBlack.Size = New System.Drawing.Size(91, 16)
        Me.ObjectBlack.TabIndex = 1
        Me.ObjectBlack.TabStop = True
        Me.ObjectBlack.Text = "Object is black"
        Me.ObjectBlack.UseVisualStyleBackColor = True
        '
        'LabelingSettingWindow
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(208, 192)
        Me.ControlBox = False
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.LeaveButton)
        Me.Controls.Add(Me.SaveAndLeaveButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "LabelingSettingWindow"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "LabelingSetting"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents FourNeighborhoodRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents EightNeighborhoodRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents SaveAndLeaveButton As System.Windows.Forms.Button
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents LeaveButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents GroupBox2 As System.Windows.Forms.GroupBox
    Friend WithEvents ObjectWhile As System.Windows.Forms.RadioButton
    Friend WithEvents ObjectBlack As System.Windows.Forms.RadioButton
End Class
