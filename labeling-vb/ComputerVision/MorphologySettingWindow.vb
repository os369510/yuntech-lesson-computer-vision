Public Class MorphologySettingWindow

    Private Sub MorphologySettingWindow_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Me.Dispose()
    End Sub

    Private Sub MorphologySettingWindow_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        If (Form1.MorphologyMatrixFlag = 0) Then
            SelectMorphologyMatrixSize.SelectedIndex = 0
        ElseIf (Form1.MorphologyMatrixFlag = 1) Then
            SelectMorphologyMatrixSize.SelectedIndex = 1
        End If
        UpdateColor()
    End Sub

    Private Sub SaveAndLeaveButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SaveAndLeaveButton.Click
        If (SelectMorphologyMatrixSize.SelectedIndex = 0) Then
            Form1.MorphologyMatrixFlag = 0
            Form1.MorphologyMatrixCheck = 1
        ElseIf (SelectMorphologyMatrixSize.SelectedIndex = 1) Then
            Form1.MorphologyMatrixFlag = 1
            Form1.MorphologyMatrixCheck = 2
        End If
        UpdateMatrix()
        Me.Close()
    End Sub

    Private Sub SelectMorphologyMatrixSize_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SelectMorphologyMatrixSize.SelectedIndexChanged
        If (SelectMorphologyMatrixSize.SelectedIndex = 0) Then
            Form1.MorphologyMatrixFlag = 0
        ElseIf (SelectMorphologyMatrixSize.SelectedIndex = 1) Then
            Form1.MorphologyMatrixFlag = 1
        End If
        UpdateColor()
    End Sub

    Private Sub UpdateMatrix()
        Form1.MorphologyMatrix(0, 0) = ColorToMatrix(TextBox11.BackColor)
        Form1.MorphologyMatrix(0, 1) = ColorToMatrix(TextBox12.BackColor)
        Form1.MorphologyMatrix(0, 2) = ColorToMatrix(TextBox13.BackColor)
        Form1.MorphologyMatrix(0, 3) = ColorToMatrix(TextBox14.BackColor)
        Form1.MorphologyMatrix(0, 4) = ColorToMatrix(TextBox15.BackColor)

        Form1.MorphologyMatrix(1, 0) = ColorToMatrix(TextBox21.BackColor)
        Form1.MorphologyMatrix(1, 1) = ColorToMatrix(TextBox22.BackColor)
        Form1.MorphologyMatrix(1, 2) = ColorToMatrix(TextBox23.BackColor)
        Form1.MorphologyMatrix(1, 3) = ColorToMatrix(TextBox24.BackColor)
        Form1.MorphologyMatrix(1, 4) = ColorToMatrix(TextBox25.BackColor)

        Form1.MorphologyMatrix(2, 0) = ColorToMatrix(TextBox31.BackColor)
        Form1.MorphologyMatrix(2, 1) = ColorToMatrix(TextBox32.BackColor)
        Form1.MorphologyMatrix(2, 2) = ColorToMatrix(TextBox33.BackColor)
        Form1.MorphologyMatrix(2, 3) = ColorToMatrix(TextBox34.BackColor)
        Form1.MorphologyMatrix(2, 4) = ColorToMatrix(TextBox35.BackColor)

        Form1.MorphologyMatrix(3, 0) = ColorToMatrix(TextBox41.BackColor)
        Form1.MorphologyMatrix(3, 1) = ColorToMatrix(TextBox42.BackColor)
        Form1.MorphologyMatrix(3, 2) = ColorToMatrix(TextBox43.BackColor)
        Form1.MorphologyMatrix(3, 3) = ColorToMatrix(TextBox44.BackColor)
        Form1.MorphologyMatrix(3, 4) = ColorToMatrix(TextBox45.BackColor)

        Form1.MorphologyMatrix(4, 0) = ColorToMatrix(TextBox51.BackColor)
        Form1.MorphologyMatrix(4, 1) = ColorToMatrix(TextBox52.BackColor)
        Form1.MorphologyMatrix(4, 2) = ColorToMatrix(TextBox53.BackColor)
        Form1.MorphologyMatrix(4, 3) = ColorToMatrix(TextBox54.BackColor)
        Form1.MorphologyMatrix(4, 4) = ColorToMatrix(TextBox55.BackColor)

    End Sub

    Private Sub UpdateColor()
        If (Form1.MorphologyMatrixFlag = 1) Then
            TextBox11.Enabled = True
            TextBox12.Enabled = True
            TextBox13.Enabled = True
            TextBox14.Enabled = True
            TextBox15.Enabled = True
            TextBox21.Enabled = True
            TextBox25.Enabled = True
            TextBox31.Enabled = True
            TextBox35.Enabled = True
            TextBox41.Enabled = True
            TextBox45.Enabled = True
            TextBox51.Enabled = True
            TextBox52.Enabled = True
            TextBox53.Enabled = True
            TextBox54.Enabled = True
            TextBox55.Enabled = True
        End If

        TextBox11.BackColor = MatrixToColor(Form1.MorphologyMatrix(0, 0))
        TextBox12.BackColor = MatrixToColor(Form1.MorphologyMatrix(0, 1))
        TextBox13.BackColor = MatrixToColor(Form1.MorphologyMatrix(0, 2))
        TextBox14.BackColor = MatrixToColor(Form1.MorphologyMatrix(0, 3))
        TextBox15.BackColor = MatrixToColor(Form1.MorphologyMatrix(0, 4))

        TextBox21.BackColor = MatrixToColor(Form1.MorphologyMatrix(1, 0))
        TextBox22.BackColor = MatrixToColor(Form1.MorphologyMatrix(1, 1))
        TextBox23.BackColor = MatrixToColor(Form1.MorphologyMatrix(1, 2))
        TextBox24.BackColor = MatrixToColor(Form1.MorphologyMatrix(1, 3))
        TextBox25.BackColor = MatrixToColor(Form1.MorphologyMatrix(1, 4))

        TextBox31.BackColor = MatrixToColor(Form1.MorphologyMatrix(2, 0))
        TextBox32.BackColor = MatrixToColor(Form1.MorphologyMatrix(2, 1))
        TextBox33.BackColor = MatrixToColor(Form1.MorphologyMatrix(2, 2))
        TextBox34.BackColor = MatrixToColor(Form1.MorphologyMatrix(2, 3))
        TextBox35.BackColor = MatrixToColor(Form1.MorphologyMatrix(2, 4))

        TextBox41.BackColor = MatrixToColor(Form1.MorphologyMatrix(3, 0))
        TextBox42.BackColor = MatrixToColor(Form1.MorphologyMatrix(3, 1))
        TextBox43.BackColor = MatrixToColor(Form1.MorphologyMatrix(3, 2))
        TextBox44.BackColor = MatrixToColor(Form1.MorphologyMatrix(3, 3))
        TextBox45.BackColor = MatrixToColor(Form1.MorphologyMatrix(3, 4))

        TextBox51.BackColor = MatrixToColor(Form1.MorphologyMatrix(4, 0))
        TextBox52.BackColor = MatrixToColor(Form1.MorphologyMatrix(4, 1))
        TextBox53.BackColor = MatrixToColor(Form1.MorphologyMatrix(4, 2))
        TextBox54.BackColor = MatrixToColor(Form1.MorphologyMatrix(4, 3))
        TextBox55.BackColor = MatrixToColor(Form1.MorphologyMatrix(4, 4))

        If (Form1.MorphologyMatrixFlag = 0) Then
            TextBox11.BackColor = SystemColors.Control
            TextBox12.BackColor = SystemColors.Control
            TextBox13.BackColor = SystemColors.Control
            TextBox14.BackColor = SystemColors.Control
            TextBox15.BackColor = SystemColors.Control
            TextBox21.BackColor = SystemColors.Control
            TextBox25.BackColor = SystemColors.Control
            TextBox31.BackColor = SystemColors.Control
            TextBox35.BackColor = SystemColors.Control
            TextBox41.BackColor = SystemColors.Control
            TextBox45.BackColor = SystemColors.Control
            TextBox51.BackColor = SystemColors.Control
            TextBox52.BackColor = SystemColors.Control
            TextBox53.BackColor = SystemColors.Control
            TextBox54.BackColor = SystemColors.Control
            TextBox55.BackColor = SystemColors.Control
            TextBox11.Enabled = False
            TextBox12.Enabled = False
            TextBox13.Enabled = False
            TextBox14.Enabled = False
            TextBox15.Enabled = False
            TextBox21.Enabled = False
            TextBox25.Enabled = False
            TextBox31.Enabled = False
            TextBox35.Enabled = False
            TextBox41.Enabled = False
            TextBox45.Enabled = False
            TextBox51.Enabled = False
            TextBox52.Enabled = False
            TextBox53.Enabled = False
            TextBox54.Enabled = False
            TextBox55.Enabled = False
        End If
    End Sub

    Private Function MatrixToColor(ByVal number As Integer) As Color
        If (number = 255) Then
            Return Color.White
        ElseIf (number = 0) Then
            Return Color.Black
        End If
    End Function

    Private Function ColorToMatrix(ByVal c As Color) As Integer
        If (c = Color.White) Then
            Return 255
        ElseIf (c = Color.Black) Then
            Return 0
        End If
        Return 0
    End Function

    Private Sub TextBox11_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox11.Click
        If (TextBox11.BackColor = Color.White) Then
            TextBox11.BackColor = Color.Black
        Else
            TextBox11.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox12_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox12.Click
        If (TextBox12.BackColor = Color.White) Then
            TextBox12.BackColor = Color.Black
        Else
            TextBox12.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox13_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox13.Click
        If (TextBox13.BackColor = Color.White) Then
            TextBox13.BackColor = Color.Black
        Else
            TextBox13.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox14_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox14.Click
        If (TextBox14.BackColor = Color.White) Then
            TextBox14.BackColor = Color.Black
        Else
            TextBox14.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox15_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox15.Click
        If (TextBox15.BackColor = Color.White) Then
            TextBox15.BackColor = Color.Black
        Else
            TextBox15.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox21_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox21.Click
        If (TextBox21.BackColor = Color.White) Then
            TextBox21.BackColor = Color.Black
        Else
            TextBox21.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox22_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox22.Click
        If (TextBox22.BackColor = Color.White) Then
            TextBox22.BackColor = Color.Black
        Else
            TextBox22.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox23_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox23.Click
        If (TextBox23.BackColor = Color.White) Then
            TextBox23.BackColor = Color.Black
        Else
            TextBox23.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox24_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox24.Click
        If (TextBox24.BackColor = Color.White) Then
            TextBox24.BackColor = Color.Black
        Else
            TextBox24.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox25_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox25.Click
        If (TextBox25.BackColor = Color.White) Then
            TextBox25.BackColor = Color.Black
        Else
            TextBox25.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox31_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox31.Click
        If (TextBox31.BackColor = Color.White) Then
            TextBox31.BackColor = Color.Black
        Else
            TextBox31.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox32_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox32.Click
        If (TextBox32.BackColor = Color.White) Then
            TextBox32.BackColor = Color.Black
        Else
            TextBox32.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox33_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox33.Click
        If (TextBox33.BackColor = Color.White) Then
            TextBox33.BackColor = Color.Black
        Else
            TextBox33.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox34_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox34.Click
        If (TextBox34.BackColor = Color.White) Then
            TextBox34.BackColor = Color.Black
        Else
            TextBox34.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox35_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox35.Click
        If (TextBox35.BackColor = Color.White) Then
            TextBox35.BackColor = Color.Black
        Else
            TextBox35.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox41_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox41.Click
        If (TextBox41.BackColor = Color.White) Then
            TextBox41.BackColor = Color.Black
        Else
            TextBox41.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox42_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox42.Click
        If (TextBox42.BackColor = Color.White) Then
            TextBox42.BackColor = Color.Black
        Else
            TextBox42.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox43_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox43.Click
        If (TextBox43.BackColor = Color.White) Then
            TextBox43.BackColor = Color.Black
        Else
            TextBox43.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox44_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox44.Click
        If (TextBox44.BackColor = Color.White) Then
            TextBox44.BackColor = Color.Black
        Else
            TextBox44.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox45_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox45.Click
        If (TextBox45.BackColor = Color.White) Then
            TextBox45.BackColor = Color.Black
        Else
            TextBox45.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox51_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox51.Click
        If (TextBox51.BackColor = Color.White) Then
            TextBox51.BackColor = Color.Black
        Else
            TextBox51.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox52_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox52.Click
        If (TextBox52.BackColor = Color.White) Then
            TextBox52.BackColor = Color.Black
        Else
            TextBox52.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox53_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox53.Click
        If (TextBox53.BackColor = Color.White) Then
            TextBox53.BackColor = Color.Black
        Else
            TextBox53.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox54_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox54.Click
        If (TextBox54.BackColor = Color.White) Then
            TextBox54.BackColor = Color.Black
        Else
            TextBox54.BackColor = Color.White
        End If
    End Sub

    Private Sub TextBox55_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox55.Click
        If (TextBox55.BackColor = Color.White) Then
            TextBox55.BackColor = Color.Black
        Else
            TextBox55.BackColor = Color.White
        End If
    End Sub
End Class