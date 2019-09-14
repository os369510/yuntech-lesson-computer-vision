<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form1
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
        Me.label2 = New System.Windows.Forms.Label()
        Me.ThresholdValueTextBox = New System.Windows.Forms.TextBox()
        Me.ImageTypeTextBox = New System.Windows.Forms.TextBox()
        Me.label1 = New System.Windows.Forms.Label()
        Me.HistogramButton = New System.Windows.Forms.Button()
        Me.OtsuButton = New System.Windows.Forms.Button()
        Me.ToGrayButton = New System.Windows.Forms.Button()
        Me.LoadButton = New System.Windows.Forms.Button()
        Me.OpenFileDialog1 = New System.Windows.Forms.OpenFileDialog()
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.UseTimeTextBox = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.DilationButton = New System.Windows.Forms.Button()
        Me.LabelingButton = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ErosionButton = New System.Windows.Forms.Button()
        Me.ErosionCountTextBox = New System.Windows.Forms.TextBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.DilationCountTextBox = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.FunctionNameTextBox = New System.Windows.Forms.TextBox()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.SettingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MorphologySettingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LabelingSettingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.LabelingTextBox = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'label2
        '
        Me.label2.AutoSize = True
        Me.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.label2.Location = New System.Drawing.Point(818, 90)
        Me.label2.Name = "label2"
        Me.label2.Size = New System.Drawing.Size(81, 14)
        Me.label2.TabIndex = 17
        Me.label2.Text = "ThresholdValue"
        '
        'ThresholdValueTextBox
        '
        Me.ThresholdValueTextBox.Location = New System.Drawing.Point(865, 107)
        Me.ThresholdValueTextBox.Name = "ThresholdValueTextBox"
        Me.ThresholdValueTextBox.Size = New System.Drawing.Size(107, 22)
        Me.ThresholdValueTextBox.TabIndex = 16
        Me.ThresholdValueTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'ImageTypeTextBox
        '
        Me.ImageTypeTextBox.Location = New System.Drawing.Point(865, 55)
        Me.ImageTypeTextBox.Name = "ImageTypeTextBox"
        Me.ImageTypeTextBox.Size = New System.Drawing.Size(107, 22)
        Me.ImageTypeTextBox.TabIndex = 15
        Me.ImageTypeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'label1
        '
        Me.label1.AutoSize = True
        Me.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.label1.Location = New System.Drawing.Point(818, 38)
        Me.label1.Name = "label1"
        Me.label1.Size = New System.Drawing.Size(60, 14)
        Me.label1.TabIndex = 14
        Me.label1.Text = "ImageType"
        '
        'HistogramButton
        '
        Me.HistogramButton.Location = New System.Drawing.Point(174, 51)
        Me.HistogramButton.Name = "HistogramButton"
        Me.HistogramButton.Size = New System.Drawing.Size(75, 23)
        Me.HistogramButton.TabIndex = 13
        Me.HistogramButton.Text = "Histogram"
        Me.HistogramButton.UseVisualStyleBackColor = True
        '
        'OtsuButton
        '
        Me.OtsuButton.Location = New System.Drawing.Point(255, 51)
        Me.OtsuButton.Name = "OtsuButton"
        Me.OtsuButton.Size = New System.Drawing.Size(75, 23)
        Me.OtsuButton.TabIndex = 12
        Me.OtsuButton.Text = "Otsu"
        Me.OtsuButton.UseVisualStyleBackColor = True
        '
        'ToGrayButton
        '
        Me.ToGrayButton.Location = New System.Drawing.Point(93, 51)
        Me.ToGrayButton.Name = "ToGrayButton"
        Me.ToGrayButton.Size = New System.Drawing.Size(75, 23)
        Me.ToGrayButton.TabIndex = 11
        Me.ToGrayButton.Text = "ToGray"
        Me.ToGrayButton.UseVisualStyleBackColor = True
        '
        'LoadButton
        '
        Me.LoadButton.Location = New System.Drawing.Point(12, 51)
        Me.LoadButton.Name = "LoadButton"
        Me.LoadButton.Size = New System.Drawing.Size(75, 23)
        Me.LoadButton.TabIndex = 10
        Me.LoadButton.Text = "Load"
        Me.LoadButton.UseVisualStyleBackColor = True
        '
        'OpenFileDialog1
        '
        Me.OpenFileDialog1.FileName = "OpenFileDialog1"
        '
        'PictureBox1
        '
        Me.PictureBox1.Location = New System.Drawing.Point(12, 90)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(800, 600)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 18
        Me.PictureBox1.TabStop = False
        '
        'UseTimeTextBox
        '
        Me.UseTimeTextBox.Location = New System.Drawing.Point(865, 187)
        Me.UseTimeTextBox.Name = "UseTimeTextBox"
        Me.UseTimeTextBox.Size = New System.Drawing.Size(107, 22)
        Me.UseTimeTextBox.TabIndex = 20
        Me.UseTimeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label3.Location = New System.Drawing.Point(818, 142)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(48, 14)
        Me.Label3.TabIndex = 19
        Me.Label3.Text = "UseTime"
        '
        'DilationButton
        '
        Me.DilationButton.Location = New System.Drawing.Point(119, 24)
        Me.DilationButton.Name = "DilationButton"
        Me.DilationButton.Size = New System.Drawing.Size(75, 23)
        Me.DilationButton.TabIndex = 21
        Me.DilationButton.Text = "Dilation"
        Me.DilationButton.UseVisualStyleBackColor = True
        '
        'LabelingButton
        '
        Me.LabelingButton.Location = New System.Drawing.Point(542, 51)
        Me.LabelingButton.Name = "LabelingButton"
        Me.LabelingButton.Size = New System.Drawing.Size(75, 23)
        Me.LabelingButton.TabIndex = 22
        Me.LabelingButton.Text = "Labeling"
        Me.LabelingButton.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.ErosionButton)
        Me.GroupBox1.Controls.Add(Me.DilationButton)
        Me.GroupBox1.Location = New System.Drawing.Point(336, 31)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(200, 53)
        Me.GroupBox1.TabIndex = 23
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Morphology"
        '
        'ErosionButton
        '
        Me.ErosionButton.Location = New System.Drawing.Point(38, 24)
        Me.ErosionButton.Name = "ErosionButton"
        Me.ErosionButton.Size = New System.Drawing.Size(75, 23)
        Me.ErosionButton.TabIndex = 22
        Me.ErosionButton.Text = "Erosion"
        Me.ErosionButton.UseVisualStyleBackColor = True
        '
        'ErosionCountTextBox
        '
        Me.ErosionCountTextBox.Location = New System.Drawing.Point(865, 239)
        Me.ErosionCountTextBox.Name = "ErosionCountTextBox"
        Me.ErosionCountTextBox.Size = New System.Drawing.Size(107, 22)
        Me.ErosionCountTextBox.TabIndex = 25
        Me.ErosionCountTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label4.Location = New System.Drawing.Point(818, 222)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(72, 14)
        Me.Label4.TabIndex = 24
        Me.Label4.Text = "ErosionCount"
        '
        'DilationCountTextBox
        '
        Me.DilationCountTextBox.Location = New System.Drawing.Point(865, 291)
        Me.DilationCountTextBox.Name = "DilationCountTextBox"
        Me.DilationCountTextBox.Size = New System.Drawing.Size(107, 22)
        Me.DilationCountTextBox.TabIndex = 27
        Me.DilationCountTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label5.Location = New System.Drawing.Point(818, 274)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(73, 14)
        Me.Label5.TabIndex = 26
        Me.Label5.Text = "DilationCount"
        '
        'FunctionNameTextBox
        '
        Me.FunctionNameTextBox.Location = New System.Drawing.Point(865, 159)
        Me.FunctionNameTextBox.Name = "FunctionNameTextBox"
        Me.FunctionNameTextBox.Size = New System.Drawing.Size(107, 22)
        Me.FunctionNameTextBox.TabIndex = 28
        Me.FunctionNameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SettingToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(984, 24)
        Me.MenuStrip1.TabIndex = 29
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'SettingToolStripMenuItem
        '
        Me.SettingToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.MorphologySettingToolStripMenuItem, Me.LabelingSettingToolStripMenuItem})
        Me.SettingToolStripMenuItem.Name = "SettingToolStripMenuItem"
        Me.SettingToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.SettingToolStripMenuItem.Text = "Setting"
        '
        'MorphologySettingToolStripMenuItem
        '
        Me.MorphologySettingToolStripMenuItem.Name = "MorphologySettingToolStripMenuItem"
        Me.MorphologySettingToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.MorphologySettingToolStripMenuItem.Text = "MorphologySetting"
        '
        'LabelingSettingToolStripMenuItem
        '
        Me.LabelingSettingToolStripMenuItem.Name = "LabelingSettingToolStripMenuItem"
        Me.LabelingSettingToolStripMenuItem.Size = New System.Drawing.Size(186, 22)
        Me.LabelingSettingToolStripMenuItem.Text = "LabelingSetting"
        '
        'LabelingTextBox
        '
        Me.LabelingTextBox.Location = New System.Drawing.Point(865, 343)
        Me.LabelingTextBox.Name = "LabelingTextBox"
        Me.LabelingTextBox.Size = New System.Drawing.Size(107, 22)
        Me.LabelingTextBox.TabIndex = 31
        Me.LabelingTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Label6.Location = New System.Drawing.Point(818, 326)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(48, 14)
        Me.Label6.TabIndex = 30
        Me.Label6.Text = "Labeling"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange
        Me.ClientSize = New System.Drawing.Size(984, 702)
        Me.Controls.Add(Me.LabelingTextBox)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.FunctionNameTextBox)
        Me.Controls.Add(Me.DilationCountTextBox)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.ErosionCountTextBox)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.LabelingButton)
        Me.Controls.Add(Me.UseTimeTextBox)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.label2)
        Me.Controls.Add(Me.ThresholdValueTextBox)
        Me.Controls.Add(Me.ImageTypeTextBox)
        Me.Controls.Add(Me.label1)
        Me.Controls.Add(Me.HistogramButton)
        Me.Controls.Add(Me.OtsuButton)
        Me.Controls.Add(Me.ToGrayButton)
        Me.Controls.Add(Me.LoadButton)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form1"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "ComputerVision"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Private WithEvents label2 As System.Windows.Forms.Label
    Private WithEvents ThresholdValueTextBox As System.Windows.Forms.TextBox
    Private WithEvents ImageTypeTextBox As System.Windows.Forms.TextBox
    Private WithEvents label1 As System.Windows.Forms.Label
    Private WithEvents HistogramButton As System.Windows.Forms.Button
    Private WithEvents OtsuButton As System.Windows.Forms.Button
    Private WithEvents ToGrayButton As System.Windows.Forms.Button
    Private WithEvents LoadButton As System.Windows.Forms.Button
    Friend WithEvents OpenFileDialog1 As System.Windows.Forms.OpenFileDialog
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Private WithEvents Label3 As System.Windows.Forms.Label
    Public WithEvents UseTimeTextBox As System.Windows.Forms.TextBox
    Private WithEvents DilationButton As System.Windows.Forms.Button
    Private WithEvents LabelingButton As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Private WithEvents ErosionButton As System.Windows.Forms.Button
    Public WithEvents ErosionCountTextBox As System.Windows.Forms.TextBox
    Private WithEvents Label4 As System.Windows.Forms.Label
    Public WithEvents DilationCountTextBox As System.Windows.Forms.TextBox
    Private WithEvents Label5 As System.Windows.Forms.Label
    Public WithEvents FunctionNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents SettingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents MorphologySettingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents LabelingSettingToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Public WithEvents LabelingTextBox As System.Windows.Forms.TextBox
    Private WithEvents Label6 As System.Windows.Forms.Label

End Class
