Public Class Histogram

    Private Sub Histogram_FormClosed(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles MyBase.FormClosed
        Me.Dispose()
    End Sub

    Private Sub Histogram_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim StartTime As DateTime = DateTime.Now
        Form1.MaxHistogram = 0
        For i As Integer = 0 To 255 Step 1
            Form1.HistogramArray(i) = 0
        Next
        For y As Integer = 0 To Form1.Image_Height - 1 Step 1
            For x As Integer = 0 To Form1.Image_Width - 1 Step 1
                Form1.HistogramArray(Form1.Image.GetPixel(x, y).R) += 1
            Next
        Next
        For i As Integer = 0 To 255 Step 1
            Me.Chart1.Series("Value").Points.AddXY(i.ToString(), Form1.HistogramArray(i))
            Form1.MaxHistogram += Form1.HistogramArray(i)
        Next
        Dim StopTime As DateTime = DateTime.Now
        Form1.FunctionNameTextBox.Text = "Histogram"
        Form1.UseTimeTextBox.Text = (StopTime - StartTime).ToString()
    End Sub

End Class