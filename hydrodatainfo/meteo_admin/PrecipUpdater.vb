Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.Threading

Public Class PrecipUpdater

    Public ConnectionString As String
    Public LocalDataDir As String

    Private log As String

    Private tokens As New List(Of String)
    Private _tokenId As Integer = 0


    Public Sub New()
        tokens.Add("xzBDDCVBGeGmflGfJn")
        tokens.Add("xxDCVVUVDfeKneKGGf")
        tokens.Add("VDVzwUAxDzleInenlHm")
        tokens.Add("VCDAUBAxUxLLMrTqnpnp")
        tokens.Add("bajiEbchhLSLMrTKqLn")
        tokens.Add("zVyyUyCxVLpnqnMoMnK")
        tokens.Add("DiDgEhghEnKHHlelle")
        tokens.Add("wUDzCzxBDwLprqLqoMrq")
        tokens.Add("VzDyzBAAyyflGGIKIeGl")
        tokens.Add("VzxADxUyxyLrppqLLnqp")
        tokens.Add("hgcibDDgjoonMqqTp")
        tokens.Add("VADzUxBzAyLKKrLSKLqK")
        tokens.Add("bFgiaaEhaFfGlJJIHeKl")
        tokens.Add("bgEhcbEjhiTSKKKqSnp")
        tokens.Add("xUyyVVBzVSpoLSrqKq")
        tokens.Add("ACVzCzUCwLLKLSroTnr")
        tokens.Add("AzwAzABxUnKGmfJGnm")
        tokens.Add("VBBxwxDUCCGeIGmKJlIf")
        tokens.Add("bbaaDgbhjhflKeGmIlmK")
        tokens.Add("VCDAyBxUxypqpSMqoMp")
        tokens.Add("jEcibEDEafJJnKKIHfG")

    End Sub

    Private Function GetRandomToken() As String
        Dim rndNum As Integer = GetTokenId()
        Return tokens(rndNum)
    End Function

    Private Function GetTokenId() As String
        Try
            Thread.Sleep(10000)
            Dim rndNum As Integer = _tokenId + 1
            If rndNum > 19 Then
                rndNum = 0
            End If
            _tokenId = rndNum
            WriteLog("GetTokenId - " & _tokenId)
            Return _tokenId

        Catch ex As Exception
            WriteLog("GetTokenId - " & ex.Message)
            Return 0
        End Try
    End Function


    Public Sub RunUpdate()

        Dim ncdcVarCodes() As String = {"TMP", "AA1"}

        For Each vCode As String In ncdcVarCodes
            'get all station that observe this variable
            Dim stList As List(Of MyStationInfo) = GetStations(vCode)

            For Each stInfo As MyStationInfo In stList
                Try
                    ImportStation(stInfo.WmoId, vCode, stInfo.EndDate)
                Catch ex As Exception
                    WriteLog("ImportStation + " + stInfo.WmoId + " " + vCode + " " + ex.Message)
                End Try
            Next
        Next

    End Sub

    
    Private Function GetStations(ByVal ncdcVarCode As String) As List(Of MyStationInfo)
        WriteLog("Starting GetStations - " & ncdcVarCode)

        Dim maxDateNewTmp = New DateTime(1998, 1, 2)
        Dim maxDateNewPcp = New DateTime(1998, 1, 2)

        'precipitation - query pcp table
        Dim sql As String = "select st.st_id, st.wmo_id, MAX(pcp.obs_date) as 'max_date' FROM st LEFT JOIN " & _
"pcp on st.st_id = pcp.st_id group by st.st_id, st.wmo_id"
        'temperature - query tmp table
        If ncdcVarCode = "TMP" Then
            sql = "SELECT st.st_id, st.wmo_id, MAX(tmp.obs_date) as 'max_date' FROM st LEFT JOIN tmp on st.st_id = tmp.st_id GROUP BY st.st_id, st.wmo_id"
        End If
        'TODO replace previous listings by querying directly SERIES table

        Dim stList As New List(Of MyStationInfo)
        Using cnn As New SqlConnection(ConnectionString)
            Using cmd As New SqlCommand(sql, cnn)
                Try
                    cmd.Connection.Open()
                    Dim rdr As SqlDataReader = cmd.ExecuteReader()
                    While rdr.Read()
                        Dim dbId As Integer = Convert.ToInt32(rdr("st_id"))
                        Dim wmoId As Integer = Convert.ToInt32(rdr("wmo_id"))
                        'find the maxDate. If no values in db yet, assign default maxDate
                        Dim maxDate As DateTime = New DateTime(1998, 1, 2)
                        If Not IsDBNull(rdr("max_date")) Then
                            maxDate = Convert.ToDateTime(rdr("max_date"))
                        ElseIf ncdcVarCode = "AA1" Then
                            maxDate = maxDateNewPcp
                        Else
                            maxDate = maxDateNewTmp
                        End If

                        Dim station As New MyStationInfo()
                        station.DbId = dbId
                        station.WmoId = wmoId
                        station.EndDate = maxDate
                        stList.Add(station)
                    End While
                Catch ex As Exception
                    WriteLog("GetStations - " & ncdcVarCode & "_" & ex.Message)
                Finally
                    cmd.Connection.Close()
                End Try

            End Using
        End Using
        Return stList
    End Function

    ''' <summary>
    ''' Import the station time series data from ncdc web service to local database
    ''' </summary>
    ''' <param name="stID"></param>
    ''' <param name="ncdcVarCode"></param>
    ''' <param name="startDate"></param>
    ''' <remarks></remarks>
    Public Sub ImportStation(ByVal stID As Integer, ByVal ncdcVarCode As String, ByVal startDate As DateTime)

        WriteLog("ImportStation: " & stID & " " & ncdcVarCode & " " & startDate)

        Try

            Dim cli As New WebClient
            Dim baseUrl As String = String.Format("http://www7.ncdc.noaa.gov/rest/services/values/ish/{0}99999/{1}/", stID, ncdcVarCode)
            Dim data As String = String.Empty

            'case 1 - short update period
            If DateTime.Now.Date.Subtract(startDate).TotalDays < 30 Then

                Dim startDateStr As String = startDate.AddDays(-1).ToString("yyyyMMdd")
                Dim endDateStr As String = DateTime.Now.Date.ToString("yyyyMMdd")
                Dim tok As String = GetRandomToken()
                Dim url As String = baseUrl & String.Format("{0}/{1}/?output=csv&token={2}", startDateStr, endDateStr, tok)
                data = String.Empty
                Try
                    data = cli.DownloadString(url)
                Catch
                    url = baseUrl & String.Format("{0}/{1}/?output=csv&token={2}", startDateStr, endDateStr, GetRandomToken())
                End Try

                If ncdcVarCode = "AA1" Then
                    Dim dailyValsP As List(Of TimeValue) = ReadData(data)
                    InsertValuesP(dailyValsP, stID)
                Else
                    Dim dailyValsT As List(Of TimeValueT) = ReadTemperature(data)
                    InsertValuesT(dailyValsT, stID)
                End If
                'case 2 - long update period of several years
            Else

                Dim curY As Integer = startDate.AddDays(10).Year

                While curY <= DateTime.Now.Year

                    Dim startDateStr As String = (New DateTime(curY, 1, 1)).ToString("yyyyMMdd")
                    Dim endDateStr As String = (New DateTime(curY + 1, 1, 1)).ToString("yyyyMMdd")
                    Dim tok As String = GetRandomToken()
                    Dim url As String = baseUrl & String.Format("{0}/{1}/?output=csv&token={2}", startDateStr, endDateStr, tok)
                    data = String.Empty
                    Try
                        data = cli.DownloadString(url)
                    Catch ex As Exception
                        WriteLog("downloadData - " & startDateStr & " " & ex.Message)
                        Thread.Sleep(61000)

                        url = baseUrl & String.Format("{0}/{1}/?output=csv&token={2}", startDateStr, endDateStr, GetRandomToken())
                        Try
                            data = cli.DownloadString(url)
                        Catch ex2 As Exception
                            WriteLog("downloadData - try2" & startDateStr & " " & ex2.Message)
                        End Try

                    End Try

                    If ncdcVarCode = "AA1" Then
                        Dim dailyValsP As List(Of TimeValue) = ReadData(data)
                        InsertValuesP(dailyValsP, stID)
                    Else
                        Dim dailyValsT As List(Of TimeValueT) = ReadTemperature(data)
                        InsertValuesT(dailyValsT, stID)
                    End If

                    curY += 1
                End While
            End If
        Catch ex1 As IndexOutOfRangeException
            WriteLog("ImportStation - " & ex1.Message & " " & ex1.StackTrace)
        Catch ex As Exception
            WriteLog("ImportStation - " & ex.Message)
        End Try

    End Sub

    Private Function CalculateSum(ByVal lst As List(Of TimeValue)) As Integer
        Dim sum As Integer = 0
        For Each tv As TimeValue In lst
            If tv.ObsValue > 0 Then
                sum += tv.ObsValue
            End If
        Next
        Return sum
    End Function

    Private Sub InsertValuesP(ByVal valueList As List(Of TimeValue), ByVal wmoId As Integer)
        Try
            WriteLog("InsertValuesP - " & " " & wmoId & " " & valueList.Count & "values")

            'check if any values should be inserted
            If valueList.Count = 0 Then Return


            'first, get station ID
            Dim stId As Integer = GetStationId(wmoId)



            Using cnn As New SqlConnection(ConnectionString)
                Using cmd As New SqlCommand("INSERT INTO pcp(st_id, obs_date, obs_value) VALUES (@p1, @p2, @p3)", cnn)
                    cmd.Parameters.Add(New SqlParameter("@p1", SqlDbType.SmallInt))
                    cmd.Parameters.Add(New SqlParameter("@p2", SqlDbType.Date))
                    cmd.Parameters.Add(New SqlParameter("@p3", SqlDbType.SmallInt))

                    'second, insert values
                    For Each tv As TimeValue In valueList
                        Try
                            cmd.Connection.Open()
                            cmd.Parameters("@p1").Value = stId
                            cmd.Parameters("@p2").Value = tv.ObsTime
                            cmd.Parameters("@p3").Value = tv.ObsValue
                            cmd.ExecuteNonQuery()
                        Catch ex As Exception
                        Finally
                            cmd.Connection.Close()
                        End Try
                    Next

                End Using
            End Using

            'updating series catalog
            UpdateSeriesCatalog(stId, "PRCP")

        Catch ex As Exception
            WriteLog("InsertValues - " & wmoId & " " & ex.Message)
        End Try
    End Sub

    Private Sub InsertValuesT(ByVal valueList As List(Of TimeValueT), ByVal wmoId As Integer)
        Try
            'check if any values should be inserted
            If valueList.Count = 0 Then Return

            'first, get station ID
            Dim stId As Integer = GetStationId(wmoId)

            WriteLog("InsertValuesT - " & stId & " " & wmoId & " " & valueList.Count & "values")

            Using cnn As New SqlConnection(ConnectionString)
                Using cmd As New SqlCommand("INSERT INTO tmp(st_id, obs_date, tmin, tavg, tmax) VALUES (@st_id, @obs_date, @tmin, @tavg, @tmax)", cnn)
                    cmd.Parameters.Add(New SqlParameter("@st_id", SqlDbType.SmallInt))
                    cmd.Parameters.Add(New SqlParameter("@obs_date", SqlDbType.Date))
                    cmd.Parameters.Add(New SqlParameter("@tmin", SqlDbType.SmallInt))
                    cmd.Parameters.Add(New SqlParameter("@tavg", SqlDbType.SmallInt))
                    cmd.Parameters.Add(New SqlParameter("@tmax", SqlDbType.SmallInt))

                    'second, insert values
                    For Each tv As TimeValueT In valueList
                        Try
                            cmd.Connection.Open()
                            cmd.Parameters("@st_id").Value = stId
                            cmd.Parameters("@obs_date").Value = tv.ObsTime
                            cmd.Parameters("@tmin").Value = tv.Tmin
                            cmd.Parameters("@tavg").Value = tv.Tavg
                            cmd.Parameters("@tmax").Value = tv.Tmax
                            cmd.ExecuteNonQuery()
                        Catch ex As Exception
                        Finally
                            cmd.Connection.Close()
                        End Try
                    Next

                End Using
            End Using

            'updating series catalog
            UpdateSeriesCatalog(stId, "TMIN")
            UpdateSeriesCatalog(stId, "TAVG")
            UpdateSeriesCatalog(stId, "TMAX")
        Catch ex As Exception
            WriteLog("InsertValuesT - " & wmoId & " " & ex.Message)
        End Try
    End Sub

    Private Sub UpdateSeriesCatalog(ByVal stID As Integer, ByVal varCode As String)
        Try

            WriteLog("Executing UpdateSeriesCatalog - " & varCode & " " & stID)

            'get table name
            Dim tableName As String = "pcp"
            If varCode = "TMIN" Or varCode = "TAVG" Or varCode = "TMAX" Then tableName = "tmp"

            Dim sqlDates As String = String.Format("SELECT MIN(obs_date), MAX(obs_date), COUNT(obs_date) FROM {0} WHERE st_id={1}", tableName, stID)
            Dim sqlSeries As String = String.Format("SELECT st_id FROM series WHERE var_code = '{0}' AND st_id={1}", varCode, stID)
            Dim sqlUpdate As String = String.Format("UPDATE series SET start_date=@p1, end_date=@p2, value_count=@p3 WHERE var_code = '{0}' AND st_id={1}", varCode, stID)
            Dim sql_Insert As String = "INSERT INTO series(var_id, st_id, var_code, start_date, end_date, value_count) VALUES (@p1,@p2,@p3,@p4,@p5,@p6)"
            Dim startDate As DateTime = DateTime.MinValue
            Dim endDate As DateTime = DateTime.MinValue
            Dim valueCount As Integer = 0
            Dim seriesExists As Boolean = False

            'get variable id
            Dim varId As Integer = 1
            Select Case varCode
                Case "PRCP"
                    varId = 1
                Case "TMIN"
                    varId = 2
                Case "TAVG"
                    varId = 3
                Case "TMAX"
                    varId = 4
                Case Else
                    varId = 1
            End Select

            Using cnn As New SqlConnection(ConnectionString)

                Using cmd1 As New SqlCommand(sqlDates, cnn)
                    Try
                        'WriteLog("UpdateSeriesCatalog - " & stID & " get dates")
                        cmd1.Connection.Open()
                        Dim r1 As SqlDataReader = cmd1.ExecuteReader()
                        r1.Read()
                        startDate = Convert.ToDateTime(r1(0))
                        endDate = Convert.ToDateTime(r1(1))
                        valueCount = Convert.ToInt32(r1(2))
                        WriteLog(String.Format("SeriesCatalog: {0} {1} {2}", stID, startDate, endDate))
                    Catch ex As Exception
                        WriteLog("UpdateSeriesCatalog - GetStartDate " + sqlDates + " " + ex.Message) '+ ex.Message)
                    Finally
                        cmd1.Connection.Close()
                    End Try
                End Using
                
                'find if series exists
                Using cmd1a As New SqlCommand(sqlSeries, cnn)

                    Try
                        cmd1a.Connection.Open()
                        Dim resObj As Object = cmd1a.ExecuteScalar()

                        If Not IsNothing(resObj) Then
                            seriesExists = True
                        Else
                            seriesExists = False
                        End If
                        WriteLog("SeriesExists " & seriesExists)
                    Catch ex As Exception
                        seriesExists = False
                        WriteLog("UpdateSeriesCatalog - FindIfSeriesExists - exc") ' + ex.Message)
                    Finally
                        cmd1a.Connection.Close()
                    End Try

                End Using

                'for update execution..
                If seriesExists Then
                    Using cmd2 As New SqlCommand(sqlUpdate, cnn)

                        cmd2.Parameters.Add(New SqlParameter("@p1", SqlDbType.Date))
                        cmd2.Parameters.Add(New SqlParameter("@p2", SqlDbType.Date))
                        cmd2.Parameters.Add(New SqlParameter("@p3", SqlDbType.Int))
                        cmd2.Parameters("@p1").Value = startDate
                        cmd2.Parameters("@p2").Value = endDate
                        cmd2.Parameters("@p3").Value = valueCount
                        Try
                            cmd2.Connection.Open()
                            cmd2.ExecuteNonQuery()
                        Catch ex As Exception
                            WriteLog("UpdateSeriesCatalog - Update exception" + ex.Message)
                        Finally
                            cmd2.Connection.Close()
                        End Try

                    End Using
                Else
                    Using cmd3 As New SqlCommand(sql_Insert, cnn)
                        cmd3.Parameters.Add(New SqlParameter("@p1", SqlDbType.TinyInt))
                        cmd3.Parameters.Add(New SqlParameter("@p2", SqlDbType.SmallInt))
                        cmd3.Parameters.Add(New SqlParameter("@p3", SqlDbType.VarChar))
                        cmd3.Parameters.Add(New SqlParameter("@p4", SqlDbType.Date))
                        cmd3.Parameters.Add(New SqlParameter("@p5", SqlDbType.Date))
                        cmd3.Parameters.Add(New SqlParameter("@p6", SqlDbType.Int))
                        cmd3.Parameters("@p1").Value = varID
                        cmd3.Parameters("@p2").Value = stID
                        cmd3.Parameters("@p3").Value = varCode
                        cmd3.Parameters("@p4").Value = startDate
                        cmd3.Parameters("@p5").Value = endDate
                        cmd3.Parameters("@p6").Value = valueCount

                        Try
                            cmd3.Connection.Open()
                            cmd3.ExecuteNonQuery()
                        Catch ex As Exception
                            WriteLog("UpdateSeriesCatalog - Insert " + ex.Message)
                        Finally
                            cmd3.Connection.Close()
                        End Try
                    End Using
                End If
            End Using
        Catch ex As Exception
            WriteLog(ex.Message)
        End Try

    End Sub



    Private Function GetStationId(ByVal wmoID As Integer)
        Using cnn As New SqlConnection(ConnectionString)
            Using cmd As New SqlCommand("SELECT st_id FROM st WHERE wmo_id=" & wmoID, cnn)
                cmd.Connection.Open()
                Dim obj As Object = cmd.ExecuteScalar
                If Not obj Is Nothing Then
                    Return obj
                Else
                    Return -1
                End If
            End Using
        End Using
    End Function


    Public Function ReadTemperature(ByVal str As String) As List(Of TimeValueT)
        Dim dailyVals As New List(Of TimeValueT)

        Dim line As String = Nothing

        Dim prevTime As New DateTime(999, 1, 1) 'the previous date/time
        Dim curTime As New DateTime(999, 1, 1) 'the current date/time

        Dim tmpEntries As New List(Of TimeValue)

        Using r As New StringReader(str)
            'line = r.ReadLine()
            While True
                'work with the line
                line = r.ReadLine()
                If line Is Nothing Then Exit While

                Dim entries() As String = line.Split(",")
                Dim hr As String = entries(3)
                Dim tmp As String = entries(5)

                'handle /previous time/
                curTime = GetCurTime(entries(2), hr)
                Dim d77 As DateTime = GetDate77(curTime)
                If prevTime.Year < 1000 Then prevTime = curTime

                'date checking --> database entry
                Dim prevDate77 = GetDate77(prevTime)
                If d77 > prevDate77 Then
                    'process vals and reinitialize
                    If tmpEntries.Count > 0 Then
                        AddDailyTmp(tmpEntries, dailyVals)
                        tmpEntries.Clear()
                    Else
                        dailyVals.Add(New TimeValueT(prevDate77, -9999, -9999, -9999))
                    End If
                End If

                'compare tspan and consecutive time interval
                Dim prevDiff As Integer = (curTime.Subtract(prevTime)).TotalHours


                'If tSpan > prevDiff And pcpEntries.Count > 0 Then

                '    Dim prevT As DateTime = curTime.AddHours(-1 * tSpan)

                '    For i As Integer = pcpEntries.Count - 1 To 0 Step -1
                '        If pcpEntries(i).ObsTime > prevT Then pcpEntries(i).ObsValue = 0
                '    Next

                'End If

                'add the tmp entry
                If tmp <> "null" Then
                    tmpEntries.Add(New TimeValue(curTime, tmp))
                End If

                'set previous to current
                prevTime = curTime
            End While
        End Using

        Return dailyVals
    End Function

    Private Sub AddDailyTmp(ByVal incrementList As List(Of TimeValue), ByVal DailyValues As List(Of TimeValueT))

        If incrementList Is Nothing Then Return
        If incrementList.Count = 0 Then Return

        Dim totalSum As Integer = 0
        Dim min As Integer = incrementList(0).ObsValue
        Dim max As Integer = incrementList(0).ObsValue
        For i = 0 To incrementList.Count - 1
            Dim obsVal As Integer = incrementList(i).ObsValue
            totalSum += incrementList(i).ObsValue
            If obsVal < min Then min = obsVal
            If obsVal > max Then max = obsVal
        Next
        Dim tmpDate As DateTime = GetDate77(incrementList(incrementList.Count - 1).ObsTime)
        Dim avgVal As Integer = (totalSum / incrementList.Count)
        DailyValues.Add(New TimeValueT(tmpDate, min, avgVal, max))
    End Sub

    Public Function ReadData(ByVal str As String) As List(Of TimeValue)

        Dim dailyVals As New List(Of TimeValue)

        Dim line As String = Nothing
        'Dim accPrecip As Integer = 0 'accumulated precip. for current date
        'Dim accHours As Integer = 0  'accumulated hours for current date

        Dim prevTime As New DateTime(999, 1, 1) 'the previous date/time
        Dim curTime As New DateTime(999, 1, 1) 'the current date/time

        Dim pcpEntries As New List(Of TimeValue)

        Using r As New StringReader(str)
            'line = r.ReadLine()
            While True
                'work with the line
                line = r.ReadLine()
                If line Is Nothing Then Exit While

                Dim entries() As String = line.Split(",")
                Dim hr As String = entries(3)
                Dim tSpan As String = entries(5)
                Dim pcp As String = entries(6)

                'handle /previous time/
                curTime = GetCurTime(entries(2), hr)
                Dim d77 As DateTime = GetDate77(curTime)
                If prevTime.Year < 1000 Then prevTime = curTime

                'date checking --> database entry
                Dim prevDate77 = GetDate77(prevTime)
                If d77 > prevDate77 Then
                    'process vals and reinitialize
                    If pcpEntries.Count > 0 Then
                        AddDailyPrecip(pcpEntries, dailyVals)
                        pcpEntries.Clear()
                    Else
                        dailyVals.Add(New TimeValue(prevDate77, -9999))
                    End If
                End If

                'guess the time span
                If tSpan = "null" Then
                    If hr = "0600" Or hr = "1800" Then
                        tSpan = "12"
                    ElseIf hr = "1200" Or hr = "0000" Then
                        tSpan = "06"
                    ElseIf hr = "0300" Or hr = "0900" Or hr = "1500" Or hr = "2100" Then
                        tSpan = "03"
                    Else
                        tSpan = "01"
                    End If
                End If

                'compare tspan and consecutive time interval
                Dim prevDiff As Integer = (curTime.Subtract(prevTime)).TotalHours

                'If tSpan < prevDiff Then
                '    'todo check for data gaps!
                '    pcpEntries.Add(New TimeValue(curTime, pcp))
                'ElseIf tSpan = prevDiff Then
                '    'consecutive intervals - add
                '    pcpEntries.Add(New TimeValue(curTime, pcp))
                '    'accPrecip += pcp

                If tSpan > prevDiff And pcpEntries.Count > 0 Then

                    Dim prevT As DateTime = curTime.AddHours(-1 * tSpan)

                    For i As Integer = pcpEntries.Count - 1 To 0 Step -1
                        If pcpEntries(i).ObsTime > prevT Then pcpEntries(i).ObsValue = 0
                    Next

                End If

                'add the pcp entry
                If pcp <> "null" Then
                    pcpEntries.Add(New TimeValue(curTime, pcp))
                End If

                'set previous to current
                prevTime = curTime
            End While
        End Using

        Return dailyVals
    End Function

    Private Sub AddDailyPrecip(ByVal incrementList As List(Of TimeValue), ByVal DailyValues As List(Of TimeValue))

        If incrementList Is Nothing Then Return
        If incrementList.Count = 0 Then Return

        Dim totalSum As Integer = 0
        For i = 0 To incrementList.Count - 1
            totalSum += incrementList(i).ObsValue
        Next
        Dim pcpDate As DateTime = GetDate77(incrementList(incrementList.Count - 1).ObsTime)
        DailyValues.Add(New TimeValue(pcpDate, totalSum))
    End Sub

    Private Function GetDate77(ByVal dt As DateTime) As DateTime
        If dt.Hour < 7 Then
            Return dt.AddDays(-1).Date
        Else
            Return dt.Date
        End If
    End Function

    Private Function GetCurTime(ByVal yyyyMMdd As String, ByVal hourUtc As String) As DateTime
        Dim dat As New DateTime(yyyyMMdd.Substring(0, 4), yyyyMMdd.Substring(4, 2), yyyyMMdd.Substring(6, 2), hourUtc.Substring(0, 2), hourUtc.Substring(2, 2), 0)
        Return dat
    End Function

    Public Sub WriteLog(ByVal msg As String)

        Dim insertQuery As String = "INSERT INTO log_messages(log_time, log_msg) VALUES (@p1, @p2)"
        Dim cmd As New SqlCommand(insertQuery)
        cmd.Connection = New SqlConnection(ConnectionString)
        Dim p1 As New SqlParameter("@p1", SqlDbType.DateTime)
        Dim p2 As New SqlParameter("@p2", SqlDbType.VarChar)
        p1.Value = DateTime.Now
        p2.Value = msg
        cmd.Parameters.Add(p1)
        cmd.Parameters.Add(p2)

        Try
            cmd.Connection.Open()
            cmd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            cmd.Connection.Close()
        End Try

    End Sub
End Class

Public Class TimeValue
    Public ObsTime As DateTime
    Public ObsValue As Integer
    Public Sub New(ByVal t As DateTime, ByVal v As Integer)
        ObsTime = t
        ObsValue = v
    End Sub
End Class

Public Class TimeValueT
    Public ObsTime As DateTime
    Public Tmin As Integer
    Public Tavg As Integer
    Public Tmax As Integer

    Public Sub New(ByVal dat As DateTime, ByVal tmin As Integer, ByVal tavg As Integer, ByVal tmax As Integer)
        ObsTime = dat
        Me.Tmin = tmin
        Me.Tavg = tavg
        Me.Tmax = tmax
    End Sub

End Class

Public Class PcpEntry
    Public ObsTime As DateTime   'hr from start of day (7UT=0, 8UT=1..)
    Public AccPcp As Integer 'accumulated pcp from start of day
End Class

Public Class MyStationInfo
    Public DbId As Integer
    Public WmoId As Integer
    Public EndDate As DateTime
End Class