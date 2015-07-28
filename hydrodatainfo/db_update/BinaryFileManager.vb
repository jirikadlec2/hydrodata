Imports System.Net
Imports System.IO

'Auxiliary class for managing the binary files
Public Class BinaryFileManager

    'gets the existing data from the API
    'timeStep can be "h" for hourly or "d" for daily
    Public Function GetDataFromAPI(ByVal siteID As String, ByVal czechVarName As String, ByVal timeStep As String) As List(Of TimeValuePair)

        Dim url As String = "http://hydrodata.info/api/values?site=" & siteID & "&variable=" & czechVarName & "&step=" & timeStep

        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim reader As New StreamReader(response.GetResponseStream())
        Dim line As String = reader.ReadLine()

        'reads each line from the request
        Dim output As New List(Of TimeValuePair)

        If timeStep = "h" Then

            While line IsNot Nothing

                line = reader.ReadLine()

                If line IsNot Nothing Then

                    Dim tv() As String = line.Split(vbTab)
                    Dim t As String = tv(0)
                    Dim y As Integer = CInt(t.Substring(0, 4))
                    Dim m As Integer = CInt(t.Substring(5, 2))
                    Dim d As Integer = CInt(t.Substring(8, 2))
                    Dim h As Integer = CInt(t.Substring(11, 2))
                    Dim dt As New DateTime(y, m, d, h, 0, 0)

                    Dim v As String = tv(1)
                    Dim val As Single = -9999.0
                    If (v <> "NA") Then
                        val = CSng(v)
                    End If
                    Dim tvp As New TimeValuePair(dt, val)

                    output.Add(tvp)
                End If

            End While

        Else

            While line IsNot Nothing

                line = reader.ReadLine()

                If line IsNot Nothing Then

                    Dim tv() As String = line.Split(vbTab)
                    Dim t As String = tv(0)
                    Dim y As Integer = CInt(t.Substring(0, 4))
                    Dim m As Integer = CInt(t.Substring(5, 2))
                    Dim d As Integer = CInt(t.Substring(8, 2))

                    Dim dt As New DateTime(y, m, d, 0, 0, 0)

                    Dim v As String = tv(1)
                    Dim val As Single = -9999.0
                    If (v <> "NA") Then
                        val = CSng(v)
                    End If
                    Dim tvp As New TimeValuePair(dt, val)

                    output.Add(tvp)
                End If

            End While

        End If

        Return output
    End Function

    'gets the existing data from the API
    Public Function GetSitesFromAPI(ByVal czechVarName As String) As List(Of Integer)

        Dim url As String = "http://hydrodata.info/api/sites?variable=" & czechVarName

        Dim request As HttpWebRequest = DirectCast(WebRequest.Create(url), HttpWebRequest)
        Dim response As HttpWebResponse = DirectCast(request.GetResponse(), HttpWebResponse)
        Dim reader As New StreamReader(response.GetResponseStream())
        Dim line As String = reader.ReadLine()

        'reads each line from the request
        Dim output As New List(Of Integer)
        While line IsNot Nothing

            line = reader.ReadLine()

            If line IsNot Nothing Then

                Dim row() As String = line.Split(vbTab)
                Dim siteID As String = row(0)
                output.Add(CInt(siteID))
            End If

        End While

        Return output
    End Function

    'saves the data to a binary file
    Public Sub SaveToBinaryFile(ByVal values As List(Of TimeValuePair), ByVal outFileName As String)
        Using fs As New FileStream(outFileName, FileMode.Create)

            'populate valuesArray
            Dim N As Integer = values.Count
            Dim valuesArray(N - 1) As Single
            For i As Integer = 0 To N - 1
                valuesArray(i) = values(i).Value
            Next

            'find out the startDate
            Dim firstDate As DateTime = values(0).DateTime
            Dim binaryStartDate As Long = firstDate.ToBinary()
            Dim startDateBytes(7) As Byte
            System.Buffer.BlockCopy(New Long() {binaryStartDate}, 0, startDateBytes, 0, 8)

            Dim NBytes As Integer = 4 * N
            Dim bytesOriginal(NBytes - 1) As Byte
            System.Buffer.BlockCopy(valuesArray, 0, bytesOriginal, 0, NBytes)

            'write to binary file
            fs.Write(startDateBytes, 0, 8)
            fs.Write(bytesOriginal, 0, NBytes)
            fs.Flush()

        End Using
    End Sub

    'saves all of the binary files for the given variable
    'use "h" or "d" for time step
    Public Sub SaveBinaryFiles(ByVal folder As String, ByVal variable As String, ByVal timeStep As String)

        'first of all fetch all sites who measure this variable
        Dim siteIDs As List(Of Integer) = GetSitesFromAPI(variable)
        For Each Site As Integer In siteIDs
            Dim siteCode As String = Site.ToString("D4")
            Dim fileName As String = timeStep & "_" & variable & "_" & siteCode & ".dat"
            Dim filePath As String = System.IO.Path.Combine(folder, fileName)

            Try
                Dim data As List(Of TimeValuePair) = GetDataFromAPI(Site, variable, timeStep)
                SaveToBinaryFile(data, filePath)
            Catch ex As Exception
                Console.WriteLine(ex.Message)
            End Try

        Next

    End Sub

    'reads the data from the binary file
    Public Function OpenBinaryFile(ByVal fileName As String, ByVal startTime As DateTime, ByVal endTime As DateTime) As List(Of TimeValuePair)

        Dim output As New List(Of TimeValuePair)

        Using stream As New FileStream(fileName, FileMode.Open)

            'constants
            Dim SIZEOF_FLOAT As Integer = 4
            Dim SIZEOF_LONG As Integer = 8

            'reads the startDate
            Dim startDateBytes(7) As Byte
            stream.Read(startDateBytes, 0, SIZEOF_LONG)
            Dim startDateBinary(0) As Long
            Buffer.BlockCopy(startDateBytes, 0, startDateBinary, 0, SIZEOF_LONG)
            Dim startDateFromFile As DateTime = DateTime.FromBinary(startDateBinary(0))

            'check the startTime
            If (startTime < startDateFromFile) Then
                startTime = startDateFromFile
            End If

            'find position of query start time
            Dim startTimePositionHours = CInt((startTime - startDateFromFile).TotalHours)
            If startTimePositionHours < 0 Then startTimePositionHours = 0

            Dim numHoursInFile = CInt((stream.Length - SIZEOF_LONG) / SIZEOF_FLOAT)

            Dim endDateFromFile As DateTime = startDateFromFile.AddHours(numHoursInFile)

            If endTime < startDateFromFile Then Return Nothing
            If startTime > endDateFromFile Then Return Nothing

            If endTime > endDateFromFile Then
                endTime = endDateFromFile
            End If

            'find position of query start time
            Dim startTimePositionInBytes As Long = SIZEOF_LONG + startTimePositionHours * SIZEOF_FLOAT
            Dim numHoursStartEnd As Long = CInt((endTime - startTime).TotalHours)
            Dim numBytesStartEnd = numHoursStartEnd * SIZEOF_FLOAT

            If startTimePositionInBytes + numBytesStartEnd > stream.Length Then
                numBytesStartEnd = stream.Length - startTimePositionInBytes
                numHoursStartEnd = CInt(numBytesStartEnd / SIZEOF_FLOAT)
            End If
            Dim endTimePositionInBytes = startTimePositionInBytes + numBytesStartEnd
            Dim resultBytes(numBytesStartEnd - 1) As Byte

            'read data from the file
            stream.Seek(startTimePositionInBytes, SeekOrigin.Begin)
            stream.Read(resultBytes, 0, resultBytes.Length)

            Dim result(numHoursStartEnd) As Single
            Buffer.BlockCopy(resultBytes, 0, result, 0, resultBytes.Length)

            'put into result list of TimeValuePairs
            Dim curTime As DateTime = startTime
            For i = 0 To result.Length - 1
                output.Add(New TimeValuePair(curTime, result(i)))
                curTime = curTime.AddHours(1)
            Next

        End Using

        Return output

    End Function
End Class
