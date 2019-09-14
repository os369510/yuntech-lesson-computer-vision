Public Class Form1

    'Openfile
    Private openfile As New OpenFileDialog()
    'Image
    Public Image As Bitmap
    Private Image_name As String
    Public Image_Height As Integer
    Public Image_Width As Integer
    'Histogram
    Public MaxHistogram As Integer
    Public HistogramArray(256) As Integer
    'Otsu
    Public T As Integer
    'Morphology
    Public ImageBuffer As Bitmap
    Public MorphologyMatrix(,) As Integer = {{0, 255, 255, 255, 0},
                                             {0, 255, 255, 255, 0},
                                             {0, 255, 255, 255, 0},
                                             {0, 255, 255, 255, 0},
                                             {0, 255, 255, 255, 0}}
    Private ErosionCount As Integer = 0
    Private DilationCount As Integer = 0
    Public MorphologyMatrixFlag As Integer = 1 ' 0:3x3 Matrix , 1:5x5 Matrix
    Public MorphologyMatrixCheck As Integer = 2
    'Labeling
    Public LabelingMatrixFlag As Integer = 0 ' 0:four-neighborhood  ,  1:eight-neighborhood
    Public ObjectColorFlag As Integer = 0 ' 0:object is white , 1:object is black
    Public LabelingMatrix(,) As Integer = {{0, 255, 0},
                                           {255, 255, 255},
                                           {0, 255, 0}}
    Private LabelingMark As Integer = 1
    Private LabelingEqualMatrix(256) As Integer

    Private Function GetGrayColor(ByVal input As Color) As Color
        Dim Result As Color
        Dim GrayColor As Double
        GrayColor = input.R * 0.299 + input.G * 0.587 + input.B * 0.114
        Result = Color.FromArgb(GrayColor, GrayColor, GrayColor)
        Return Result
    End Function

    Private Sub LoadButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LoadButton.Click
        openfile.Filter = "PNG(*.PNG)|*.png" & "|JPG(*.JPG)|*.jpg" & "|BMP(*.BMP)|*.bmp" & "|所有檔案|*.*"
        If (openfile.ShowDialog() = DialogResult.OK) Then
            LoadInitialize()
            Image_name = openfile.FileName
            Image = New Bitmap(Image_name)
            Image_Height = Image.Height
            Image_Width = Image.Width
            PictureBox1.LoadAsync(Image_name)
            ImageTypeTextBox.Text = Image.PixelFormat.ToString()
            ThresholdValueTextBox.Text = ""
        End If
    End Sub

    Private Sub ToGrayButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ToGrayButton.Click
        Dim StartTime As DateTime = DateTime.Now
        For x As Integer = 0 To (Image_Width - 1) Step 1
            For y As Integer = 0 To (Image_Height - 1) Step 1
                Image.SetPixel(x, y, GetGrayColor(Image.GetPixel(x, y)))
            Next
        Next
        Dim StopTime As DateTime = DateTime.Now
        FunctionNameTextBox.Text = "ToGray"
        UseTimeTextBox.Text = (StopTime - StartTime).ToString()
        PictureBox1.Image = Image
    End Sub

    Private Sub HistogramButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles HistogramButton.Click
        Histogram.Show()
    End Sub

    Private Sub OtsuButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OtsuButton.Click
        Dim StartTime As DateTime = DateTime.Now
        Dim p As Double
        Dim q1 As Double
        Dim q2 As Double
        Dim u As Double
        Dim u1 As Double
        Dim u2 As Double
        Dim CompareValue As Double
        Dim MaxValue As Double = 0
        For i As Integer = 0 To 255 Step 1
            p = HistogramArray(i) / MaxHistogram
            u += (i + 1) * p
        Next
        For i As Integer = 0 To 255 Step 1
            p = HistogramArray(i) / MaxHistogram
            q1 += p
            q2 = 1 - q1
            If (q1 <> 0) Then
                u1 += ((i + 1) * p)
                u1 /= q1
            End If
            u2 = (u - (q1 * u1)) / q2
            CompareValue = q1 * q2 * Math.Pow(u1 - u2, 2)
            If (CompareValue > MaxValue) Then
                MaxValue = CompareValue
                T = i
                ThresholdValueTextBox.Text = T.ToString()
            End If
            If (q1 <> 0) Then
                u1 *= q1
            End If
        Next
        For y As Integer = 0 To Image_Height - 1 Step 1
            For x As Integer = 0 To Image_Width - 1 Step 1
                If (Image.GetPixel(x, y).R >= T) Then
                    Image.SetPixel(x, y, Color.FromArgb(255, 255, 255, 255))   'white
                ElseIf (Image.GetPixel(x, y).R < T) Then
                    Image.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0))   'black
                End If
            Next
        Next
        Dim StopTime As DateTime = DateTime.Now
        FunctionNameTextBox.Text = "Otsu"
        UseTimeTextBox.Text = (StopTime - StartTime).ToString()
        PictureBox1.Image = Image
    End Sub

    Private Sub DilationButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles DilationButton.Click
        Dim StartTime As DateTime = DateTime.Now
        ImageBuffer = New Bitmap(Image_Width, Image_Height)
        For y As Integer = 2 To Image_Height - 2 - 1
            For x As Integer = 2 To Image_Width - 2 - 1
                If (Image.GetPixel(x, y).R = MorphologyMatrix(2, 2)) Then
                    For checky As Integer = -1 * MorphologyMatrixCheck To MorphologyMatrixCheck Step 1
                        For checkx As Integer = -1 * MorphologyMatrixCheck To MorphologyMatrixCheck Step 1
                            If (MorphologyMatrix(2 + checky, 2 + checkx) <> 0) Then
                                ImageBuffer.SetPixel(x + checkx, y + checky, Color.FromArgb(255, MorphologyMatrix(2 + checky, 2 + checkx), MorphologyMatrix(2 + checky, 2 + checkx), MorphologyMatrix(2 + checky, 2 + checkx)))
                            End If
                        Next
                    Next
                ElseIf (Image.GetPixel(x, y).R <> MorphologyMatrix(2, 1)) Then
                    ImageBuffer.SetPixel(x, y, Color.Black)
                End If
            Next
        Next
        Image = ImageBuffer
        Dim StopTime As DateTime = DateTime.Now
        FunctionNameTextBox.Text = "Dilation"
        DilationCount += 1
        DilationCountTextBox.Text = DilationCount.ToString()
        UseTimeTextBox.Text = (StopTime - StartTime).ToString()
        PictureBox1.Image = Image
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'Initialize()
    End Sub

    Private Sub LoadInitialize()
        LabelingMark = 1
        ErosionCount = 0
        DilationCount = 0
        ImageTypeTextBox.Text = ""
        ThresholdValueTextBox.Text = ""
        FunctionNameTextBox.Text = ""
        UseTimeTextBox.Text = ""
        ErosionCountTextBox.Text = ErosionCount.ToString()
        DilationCountTextBox.Text = DilationCount.ToString()
    End Sub

    Private Sub ErosionButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ErosionButton.Click
        Dim StartTime As DateTime = DateTime.Now
        ImageBuffer = New Bitmap(Image_Width, Image_Height)
        Dim CheckFlag As Integer = 0
        For y As Integer = 0 To Image_Height - 1
            For x As Integer = 0 To Image_Width - 1
                If (Image.GetPixel(x, y).R = MorphologyMatrix(2, 2)) Then
                    CheckFlag = 0
                    For checky As Integer = -1 * MorphologyMatrixCheck To MorphologyMatrixCheck Step 1
                        For checkx As Integer = -1 * MorphologyMatrixCheck To MorphologyMatrixCheck Step 1
                            If (MorphologyMatrix(2 + checky, 2 + checkx) <> 0) Then
                                Try
                                    If (Image.GetPixel(x + checkx, y + checky).R <> MorphologyMatrix(2 + checky, 2 + checkx)) Then
                                        CheckFlag = 1
                                        ImageBuffer.SetPixel(x, y, Color.Black)
                                        Exit For
                                    End If
                                Catch
                                    ImageBuffer.SetPixel(x, y, Color.Black)
                                End Try
                            End If
                        Next
                        If (CheckFlag = 1) Then
                            Exit For
                        End If
                    Next
                    If (CheckFlag = 0) Then
                        ImageBuffer.SetPixel(x, y, Color.White)
                    End If
                ElseIf (Image.GetPixel(x, y).R = 0) Then
                    ImageBuffer.SetPixel(x, y, Color.Black)
                End If
            Next
        Next
        Image = ImageBuffer
        ImageBuffer = New Bitmap(Image_Width, Image_Height)
        Dim StopTime As DateTime = DateTime.Now
        FunctionNameTextBox.Text = "Erosion"
        ErosionCount += 1
        ErosionCountTextBox.Text = ErosionCount.ToString()
        UseTimeTextBox.Text = (StopTime - StartTime).ToString()
        PictureBox1.Image = Image
    End Sub

    Private Sub LabelingButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelingButton.Click
        Dim StartTime As DateTime = DateTime.Now
        Dim MarkImage(Image_Width, Image_Height) As UInteger
        Dim ObjectNumber(1) As UInteger
        Dim CheckFlag As Integer = 0
        Dim Mark As Integer = 0
        LabelingMark = 1
        If (ObjectColorFlag = 1) Then
            For y As Integer = 0 To Image_Height - 1
                For x As Integer = 0 To Image_Width - 1
                    If (Image.GetPixel(x, y).R = 0) Then ' black
                        Image.SetPixel(x, y, Color.FromArgb(255, 255, 255, 255))
                    Else
                        Image.SetPixel(x, y, Color.FromArgb(255, 0, 0, 0))
                    End If
                Next
            Next
        End If
        For y As Integer = 0 To Image_Height - 1
            For x As Integer = 0 To Image_Width - 1
                If (Image.GetPixel(x, y).R = LabelingMatrix(1, 1)) Then
                    CheckFlag = 0
                    Mark = LabelingMark - 1
                    For checky As Integer = -1 To 0 Step 1
                        For checkx As Integer = -1 To 1 Step 1
                            If (LabelingMatrix(1 + checky, 1 + checkx) <> 0) Then
                                If (checky = 0 And checkx >= 0) Then
                                    Exit For
                                End If
                                Try
                                    If (Image.GetPixel(x + checkx, y + checky).R = LabelingMatrix(1 + checky, 1 + checkx) And Image.GetPixel(x + checkx, y + checky).R = 255) Then
                                        If (Mark < MarkImage(x + checkx, y + checky)) Then
                                            LabelingEqualMatrix(MarkImage(x + checkx, y + checky)) = Mark
                                        End If
                                        If (Mark > MarkImage(x + checkx, y + checky)) Then
                                            Mark = MarkImage(x + checkx, y + checky)
                                            checkx = -1
                                            checky = -1
                                            Continue For
                                        End If
                                        CheckFlag = 1
                                    End If
                                Catch
                                End Try
                            End If
                        Next
                    Next
                    If (CheckFlag = 0) Then
                        MarkImage(x, y) = LabelingMark
                        If (LabelingEqualMatrix.Length <= LabelingMark) Then
                            ReDim Preserve LabelingEqualMatrix(LabelingMark + 1)
                        End If
                        LabelingEqualMatrix(LabelingMark) = LabelingMark
                        LabelingMark += 1
                    ElseIf (CheckFlag = 1) Then
                        MarkImage(x, y) = Mark
                    End If
                End If
            Next
        Next
        For i As Integer = 0 To LabelingEqualMatrix.Length - 1
            LabelingEqualMatrix(i) = FindLeast(LabelingEqualMatrix(i))
        Next

        Dim MarkColor As Integer = 256 * 3 / 13
        Dim count = 0
        For y As Integer = 0 To Image_Height - 1
            For x As Integer = 0 To Image_Width - 1
                MarkImage(x, y) = LabelingEqualMatrix(MarkImage(x, y))
            Next
        Next

        For y As Integer = 0 To Image_Height - 1
            For x As Integer = 0 To Image_Width - 1
                If (MarkImage(x, y) <> 0) Then
                    Dim flag = 0
                    For i As Integer = 0 To count
                        If (MarkImage(x, y) = ObjectNumber(i)) Then
                            flag = 1
                        End If
                    Next
                    If (flag = 0) Then
                        ObjectNumber(count) = MarkImage(x, y)
                        ReDim Preserve ObjectNumber(ObjectNumber.Length)
                        count += 1
                    End If
                End If
                If (Image.GetPixel(x, y).R <> 0) Then
                    Image.SetPixel(x, y, MarkColorDraw(MarkColor * MarkImage(x, y)))
                End If
            Next
        Next
        Dim StopTime As DateTime = DateTime.Now
        FunctionNameTextBox.Text = "Labeling"
        LabelingTextBox.Text = count.ToString()
        UseTimeTextBox.Text = (StopTime - StartTime).ToString()
        PictureBox1.Image = Image
        Image.Save("debug")
    End Sub

    Private Function FindLeast(ByVal number) As Integer
        If (LabelingEqualMatrix(number) = 0) Then
            Return number
        End If
        number = LabelingEqualMatrix(number)
        If (number = LabelingEqualMatrix(number)) Then
            Return number
        Else
            Return FindLeast(number)
        End If
        Return number
    End Function

    Private Function MarkColorDraw(ByVal Number) As Color
        Dim R As Integer
        Dim G As Integer
        Dim B As Integer
        If (Number <= 255) Then
            B = (Number Mod 200) + 50
        ElseIf (Number <= 511) Then
            B = (Number Mod 200) + 50
            G = ((Number - 256) Mod 200) + 50
        Else
            B = (Number Mod 200) + 50
            G = ((Number - 256) Mod 200) + 50
            R = ((Number - 512) Mod 200) + 50
        End If
        Return Color.FromArgb(255, R, G, B)
    End Function

    Private Function CheckMin(ByVal CompareValue, ByVal NowValue) As Integer
        If (CompareValue > NowValue) Then
            Return NowValue
        End If
        Return CompareValue
    End Function

    Private Function CheckMax(ByVal CompareValue, ByVal NowValue) As Integer
        If (CompareValue < NowValue) Then
            Return NowValue
        End If
        Return CompareValue
    End Function

    Private Sub LabelingSettingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LabelingSettingToolStripMenuItem.Click
        LabelingSettingWindow.Show()
    End Sub

    Private Sub MorphologySettingToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MorphologySettingToolStripMenuItem.Click
        MorphologySettingWindow.Show()
    End Sub
End Class

