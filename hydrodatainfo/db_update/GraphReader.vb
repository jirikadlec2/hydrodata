Imports System.Drawing
Imports System.Net

Public Class ArrayScore
    Implements IComparable(Of ArrayScore)

    Public Function CompareTo(other As ArrayScore) As Integer Implements IComparable(Of ArrayScore).CompareTo
        Return Me.Score.CompareTo(other.Score)
    End Function

    Public Digit As Integer
    Public Score As Double

    Public Sub New(ByVal myDigit As Integer, ByVal myScore As Double)
        Digit = myDigit
        Score = myScore
    End Sub
End Class

Public Class PixelNumber

    Public Sub New()
        digitArrays = New ArrayList()
        digitArrays.Add(GetZero())
        digitArrays.Add(GetOne())
        digitArrays.Add(GetTwo())
        digitArrays.Add(GetThree())
        digitArrays.Add(GetFour())
        digitArrays.Add(GetFive())
    End Sub

    Private digitArrays As ArrayList

    Public Sub TestTheNumbers()
        Dim dir As String = "E:\dev\HydroDataInfo\teplota-grafy\"
        For Each fn As String In System.IO.Directory.GetFiles(dir)
            If fn.ToLower().EndsWith("png") Then
                Dim bmp As Bitmap = Bitmap.FromFile(fn)
                Dim maxT As Integer = GetDigits(bmp, 806, 21)
                If fn = "E:\dev\HydroDataInfo\teplota-grafy\horska_kvilda_graf_1267986235-C1HKVI01.PNG" Then
                    maxT = GetDigits(bmp, 806, 21)
                End If
            End If
        Next
    End Sub

    Public Function GetZero() As Integer()
        Dim a(39) As Integer
        a = {0, 1, 1, 1, 0,
             1, 0, 0, 0, 1,
             1, 0, 0, 0, 1,
             1, 0, 0, 0, 1,
             1, 0, 0, 0, 1,
             1, 0, 0, 0, 1,
             1, 0, 0, 0, 1,
             0, 1, 1, 1, 0}
        Return a
    End Function

    Public Function GetThree() As Integer()
        Dim a(39) As Integer
        a = {0, 1, 1, 1, 0,
             1, 0, 0, 0, 1,
             0, 0, 0, 0, 1,
             0, 0, 1, 1, 0,
             0, 0, 0, 0, 1,
             0, 0, 0, 0, 1,
             1, 0, 0, 0, 1,
             0, 1, 1, 1, 0}
        Return a
    End Function

    Public Function GetFour() As Integer()
        Dim a(39) As Integer
        a = {0, 0, 0, 1, 0,
             0, 0, 1, 1, 0,
             0, 0, 1, 1, 0,
             0, 1, 0, 1, 0,
             1, 0, 0, 1, 0,
             1, 1, 1, 1, 1,
             0, 0, 0, 1, 0,
             0, 0, 0, 1, 0}
        Return a
    End Function

    Public Function GetOne() As Integer()
        Dim a(39) As Integer
        a = {0, 0, 0, 1, 0,
             0, 1, 1, 1, 0,
             0, 0, 0, 1, 0,
             0, 0, 0, 1, 0,
             0, 0, 0, 1, 0,
             0, 0, 0, 1, 0,
             0, 0, 0, 1, 0,
             0, 0, 0, 1, 0}
        Return a
    End Function

    Public Function GetTwo() As Integer()
        Dim a(39) As Integer
        a = {0, 1, 1, 1, 0,
             1, 0, 0, 0, 1,
             0, 0, 0, 0, 1,
             0, 0, 0, 0, 1,
             0, 0, 0, 1, 0,
             0, 0, 1, 0, 0,
             0, 1, 0, 0, 0,
             1, 1, 1, 1, 1}
        Return a
    End Function

    Public Function GetFive() As Integer()
        Dim a(39) As Integer
        a = {1, 1, 1, 1, 1,
             1, 0, 0, 0, 0,
             1, 0, 0, 0, 0,
             1, 1, 1, 1, 0,
             0, 0, 0, 0, 1,
             0, 0, 0, 0, 1,
             1, 0, 0, 0, 1,
             0, 1, 1, 1, 0}
        Return a
    End Function

    Public Shared Function IsArrayEqual(ByRef first() As Integer, ByRef second() As Integer) As Boolean
        Dim len As Integer = first.Length()
        Dim numMatches As Integer = 0
        For i As Integer = 0 To len - 1
            If first(i) = second(i) Then numMatches = numMatches + 1
        Next
        If numMatches = len Then Return True

        Dim matchRatio As Double = CDbl(numMatches) / CDbl(len)
        If matchRatio > 0.78 Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function GetArrayDigitScore(ByRef arr() As Integer) As ArrayScore

        Dim scores As New List(Of ArrayScore)
        scores.Add(New ArrayScore(0, GetMatchRatio(digitArrays(0), arr)))
        scores.Add(New ArrayScore(1, GetMatchRatio(digitArrays(1), arr)))
        scores.Add(New ArrayScore(2, GetMatchRatio(digitArrays(2), arr)))
        scores.Add(New ArrayScore(3, GetMatchRatio(digitArrays(3), arr)))
        scores.Add(New ArrayScore(4, GetMatchRatio(digitArrays(4), arr)))
        scores.Add(New ArrayScore(5, GetMatchRatio(digitArrays(5), arr)))
        scores.Sort()
        Dim result As ArrayScore = scores(5)
        Return result
    End Function

    Public Shared Function GetMatchRatio(ByRef first() As Integer, ByRef second() As Integer) As Double
        Dim len As Integer = first.Length()
        Dim numMatches As Integer = 0
        For i As Integer = 0 To len - 1
            If first(i) = second(i) Then numMatches = numMatches + 1
        Next
        If numMatches = len Then Return 1.0

        Dim matchRatio As Double = CDbl(numMatches) / CDbl(len)
        Return matchRatio
    End Function

    'reads the two-number temperature digit
    Public Function GetDigits(ByRef bmp As Bitmap, ByRef topLeftX As Integer, ByRef topLeftY As Integer) As Integer
        Dim arr1 As Integer() = GetArray(bmp, topLeftX, topLeftY)
        Dim arr2 As Integer() = GetArray(bmp, topLeftX + 6, topLeftY)

        'second digit is 0 or 5 or invalid number
        Dim score2 As ArrayScore = GetArrayDigitScore(arr2)
        Dim digit2 As Integer = score2.Digit
        Dim score1 As ArrayScore = GetArrayDigitScore(arr1)
        Dim digit1 As Integer = score1.Digit

        Return (10 * digit1) + digit2

    End Function

    Public Shared Function GetArray(ByRef bmp As Bitmap, ByVal topLeftX As Integer, ByVal topLeftY As Integer) As Integer()
        Dim redColor = Color.FromArgb(204, 0, 0)
        Dim gray1 = Color.FromArgb(153, 153, 114)
        Dim gray2 = Color.FromArgb(153, 153, 134)
        Dim gray3 = Color.FromArgb(162, 153, 153)
        Dim gray4 = Color.FromArgb(171, 153, 153)
        Dim gray5 = Color.FromArgb(180, 134, 153)
        Dim gray = Color.FromArgb(153, 153, 153)

        Dim ncols As Integer = 5
        Dim nrows As Integer = 8
        Dim arr(39) As Integer
        Dim index As Integer = 0
        Dim curi, curj
        curj = topLeftX
        curi = topLeftY
        For curi = topLeftY To topLeftY + nrows - 1
            For curj = topLeftX To topLeftX + ncols - 1
                Dim curColor As Color = bmp.GetPixel(curj, curi)
                If curColor = redColor Then
                    arr(index) = 1
                ElseIf curColor = gray Then
                    arr(index) = 0
                Else
                    Select Case curColor
                        Case gray1
                            arr(index) = 0
                        Case gray2
                            arr(index) = 0
                        Case gray3
                            arr(index) = 0
                        Case gray4
                            arr(index) = 0
                        Case gray5
                            arr(index) = 0
                        Case Else
                            arr(index) = 1
                    End Select
                End If
                index += 1
            Next
        Next

        Return arr
    End Function

End Class

Public Class GraphReader

    Public Shared Function ReadGraph(ByVal graphURL As String) As List(Of TimeValuePair)

        Dim redColor = Color.FromArgb(204, 0, 0)
        Dim blackColor = Color.FromArgb(0, 0, 0)

        'y-axis-max-value number pixels
        Dim maxTnumberPixelx As Integer = 806
        Dim maxTnumberPixely As Integer = 21
        
        Dim bmp As Bitmap = BitmapFromWeb(graphURL)

        If bmp Is Nothing Then
            Return Nothing
        End If

        'temperature scaling ratio
        Dim pixNum As New PixelNumber()
        Dim maxT As Integer = pixNum.GetDigits(bmp, maxTnumberPixelx, maxTnumberPixely)
        If maxT = -99 Then
            maxT = 40
        End If

        Dim minT As Integer = maxT - 40
        Dim maxTPixel As Integer = 25
        Dim minTPixel As Integer = 405
        Dim degreesPerPixel As Double = Convert.ToDouble(maxT - minT) / Convert.ToDouble(minTPixel - maxTPixel)



        Dim baseAxisY As Integer = 407
        Dim beginBaseTime As DateTime
        Dim endBaseTime As DateTime
        Dim beginBasePixel As Integer = 0
        Dim endBasePixel As Integer = 0

        'find pixel-coordinates of multiples-of-24
        'these markers are 3-pixel wide
        For xPixel = 0 To bmp.Width - 2
            If bmp.GetPixel(xPixel, baseAxisY) = blackColor Then
                If bmp.GetPixel(xPixel + 1, baseAxisY) = blackColor And bmp.GetPixel(xPixel - 1, baseAxisY) = blackColor Then
                    If beginBasePixel = 0 Then
                        beginBasePixel = xPixel
                    ElseIf endBasePixel = 0 Then
                        endBasePixel = xPixel
                        Exit For
                    Else
                        Exit For
                    End If
                End If
            End If
        Next

        'find pixel coordinates of multiples-of-12
        'these markers are 2-pixel wide
        Dim halfPixel1 As Integer = 0
        Dim halfPixel2 As Integer = 0
        For xPixel = 0 To bmp.Width - 2
            If bmp.GetPixel(xPixel, baseAxisY) = blackColor Then
                If bmp.GetPixel(xPixel + 1, baseAxisY) = blackColor And bmp.GetPixel(xPixel - 1, baseAxisY) <> blackColor And bmp.GetPixel(xPixel + 2, baseAxisY) <> blackColor Then
                    If halfPixel1 = 0 Then
                        halfPixel1 = xPixel
                    ElseIf halfPixel2 = 0 Then
                        halfPixel2 = xPixel
                        Exit For
                    Else
                        Exit For
                    End If
                End If
            End If
        Next

        'beginBasePixel and endBasePixel must be found
        If beginBasePixel = 0 Or halfPixel1 = 0 Or halfPixel2 = 0 Then
            Throw New InvalidOperationException("BeginBasePixel (24 hour marker) or EndBasePixel were not found.")
        End If


        'find exact times corresponding to the multiples-of-12
        Dim cetNow = DateTime.UtcNow.AddHours(1)

        'only one BasePixel was found...special case
        If beginBasePixel > 0 And endBasePixel = 0 And halfPixel1 > 0 And halfPixel2 > 0 Then
            Dim cetNow1 = DateTime.UtcNow.AddHours(1)
            endBaseTime = cetNow1.Date.AddHours(12)
            beginBaseTime = endBaseTime.AddDays(-1)
            beginBasePixel = halfPixel1
            endBasePixel = halfPixel2
        Else
            endBaseTime = cetNow.Date
            beginBaseTime = endBaseTime.AddDays(-1)
        End If

        'find whole-hour pixels
        Dim pixelSpan As Integer = endBasePixel - beginBasePixel
        Dim hourPixelSpan As Double = Convert.ToDouble(pixelSpan) / 24.0

        'traverse x-axis for every hour
        Dim curXPixel As Integer = beginBasePixel
        Dim result As New List(Of TimeValuePair)
        For Hour As Integer = 0 To 24
            curXPixel = Math.Round(beginBasePixel + Hour * hourPixelSpan)

            'traverse by y-axis until reaching red
            For curYPixel = 0 To bmp.Height
                If bmp.GetPixel(curXPixel, curYPixel) = redColor Then
                    'red color found
                    Dim temperature As Double = maxT - (curYPixel - maxTPixel) * degreesPerPixel
                    result.Add(New TimeValuePair(beginBaseTime.AddHours(Hour), temperature))
                    Exit For
                End If
            Next
        Next

        'todo find the y-axis factor

        'Dim fn = graphURL.Substring(graphURL.LastIndexOf("/") + 1)
        'bmp.Save(System.IO.Path.Combine(System.IO.Path.GetTempPath(), fn))
        Return result
    End Function

    Public Shared Function BitmapFromWeb(ByVal URL As String) As Bitmap
        Try
            ' create a web request to the url of the image
            Dim myRequest As HttpWebRequest = DirectCast(WebRequest.Create(URL), HttpWebRequest)
            ' set the method to GET to get the image
            myRequest.Method = "GET"
            ' get the response from the webpage
            Dim myResponse As HttpWebResponse = DirectCast(myRequest.GetResponse(), HttpWebResponse)
            ' create a bitmap from the stream of the response
            Dim bmp = New Bitmap(myResponse.GetResponseStream())
            ' close off the stream and the response
            myResponse.Close()
            ' return the Bitmap of the image
            Return bmp
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Shared Function GetMaxT(ByRef bmp As Bitmap)

    End Function
End Class
