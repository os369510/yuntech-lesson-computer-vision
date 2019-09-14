Public Class LabelingSettingWindow

    Private Sub LeaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LeaveButton.Click
        Me.Close()
    End Sub

    Private Sub SaveAndLeaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAndLeaveButton.Click
        If (FourNeighborhoodRadioButton.Checked = True) Then
            Form1.LabelingMatrixFlag = 0
        ElseIf (EightNeighborhoodRadioButton.Checked = True) Then
            Form1.LabelingMatrixFlag = 1
        End If
        If (ObjectWhile.Checked = True) Then
            Form1.ObjectColorFlag = 0
        ElseIf (ObjectBlack.Checked = True) Then
            Form1.ObjectColorFlag = 1
        End If
        LabelingMatrixSetting()
        Me.Close()
    End Sub

    Private Sub LabelingSettingWindow_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Me.Dispose()
    End Sub

    Private Sub LabelingSettingWindow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If (Form1.LabelingMatrixFlag = 0) Then
            FourNeighborhoodRadioButton.Checked = True
            EightNeighborhoodRadioButton.Checked = False
        ElseIf (Form1.LabelingMatrixFlag = 1) Then
            FourNeighborhoodRadioButton.Checked = False
            EightNeighborhoodRadioButton.Checked = True
        End If
        If (Form1.ObjectColorFlag = 0) Then
            ObjectWhile.Checked = True
            ObjectBlack.Checked = False
        ElseIf (Form1.ObjectColorFlag = 1) Then
            ObjectWhile.Checked = False
            ObjectBlack.Checked = True
        End If
    End Sub

    Private Sub LabelingMatrixSetting()
        If (FourNeighborhoodRadioButton.Checked = True) Then
            Form1.LabelingMatrix(0, 0) = 0
            Form1.LabelingMatrix(0, 2) = 0
            Form1.LabelingMatrix(2, 0) = 0
            Form1.LabelingMatrix(2, 2) = 0
        ElseIf (EightNeighborhoodRadioButton.Checked = True) Then
            Form1.LabelingMatrix(0, 0) = 255
            Form1.LabelingMatrix(0, 2) = 255
            Form1.LabelingMatrix(2, 0) = 255
            Form1.LabelingMatrix(2, 2) = 255
        End If
    End Sub

End Class