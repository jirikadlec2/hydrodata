' trida DBUpdateClass
' obsahuje metody pro aktualizaci databaze

Option Strict On

Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Text.Encoding
Imports System.Text.RegularExpressions
Imports System.Collections.Generic


Public Class DbUpdateClassNew
    ' KONSTRUKTOR
    Public Sub New()
        Me.New("Integrated Security=SSPI;Persist Security Info=False;" & _
        "Initial Catalog=Hydro4;Data Source=(local);user=sa; password=2c506bbe")
    End Sub

    'konstruktor: vyuziva objekt sql_connstring
    Public Sub New(ByVal sql_connstring As String)
        Me.SqlDbConnectionString = sql_connstring
    End Sub

    'retezec pro spojeni s databazi
    Public Property SqlDbConnectionString() As String
        Get
            Return m_SqlDbConnectionString
        End Get
        Set(ByVal Value As String)
            m_SqlDbConnectionString = Value
        End Set
    End Property


    ' PRIVATE clenske polozky

    Private m_Ds As New DataSet
    Private m_SqlDbConnectionString As String
    Private m_SqlCn As New SqlConnection

    Private DbMissingValue As Integer = -9999

    'trida pro praci s webem (download)
    Private Class WebUtils
        Public Shared Function CreateWebClient() As WebClient
            Dim WebC As New WebClient
            'the "user-agent" should be set to enable free access..
            WebC.Headers("user-agent") = _
            "Mozilla/5.0 (Windows; U; Windows NT 5.1; cs; rv:1.8.1.16) Gecko/20080702 Firefox/2.0.0.16"
            Return WebC
        End Function
    End Class


    'struktura pro ulozeni dvojice(obs_time,obs_value)
    'vyuziva se ve funkci UpdateObsHydro
    Private Structure HydroTimeValue
        Dim Time As DateTime
        Dim Value As Double
    End Structure

    'structure to save (obs_time, stage, discharge)
    Private Structure HydroTimeValueHQ
        Dim Time As DateTime
        Dim H As Double
        Dim Q As Double
    End Structure



    '------------------------------------------------------------------
    '*************** RADAR (Czech and German)      ********************
    '------------------------------------------------------------------
    'Whether all files are saved or only files at measured precip. times,
    'is determined by RadarSaveMode in configuration database table.
    'RadarSaveMode can have values: 
    'PrecipHour ... saves radar file, only when hourly pcp > 0.2 mm occured
    'PrecipDay  ... saves radar files for given day (6ut-6ut), only when 
    'daily pcp > 0.2 mm was recorded in at least one station on given day
    'TODO: SIMPLIFY!!!!

    Public Function UpdateRadar() As String
        'promenne
        Dim LogStr As String = "Executing UpdateRadar.."

        Dim RadarFilePathBase As String = "E:\dev\Radar\"

        Try
            RadarFilePathBase = HttpContext.Current.Server.MapPath("/Radar/")
        Catch
            'TODO CHECK!!!!
            RadarFilePathBase = "D:\Websites\448cf9624b\www\Radar\"
        End Try

        If Not Directory.Exists(RadarFilePathBase) Then
            RadarFilePathBase = "E:\dev\Radar\"
            'Return String.Format("Directory {0} does not exist.", RadarFilePathBase)
        End If

        Dim RadarFileUriName As String
        Dim RadarFileSavedName As String
        Dim RadarFileSavedString As String

        Dim RadarFileSaved As Boolean = False

        'promenne pro sledovani prubehu funkce
        Dim FileCount As Integer = 0
        Dim StartTime As DateTime = DateTime.Now()
        Dim ComputationTime As TimeSpan

        Dim RadarFileUriPath_cz As String = ViewConfiguration("RadarFileUriPath_cz")
        Dim RadarFileUriPath_eu As String = ViewConfiguration("RadarFileUriPath_eu")
        Dim RadarFileUriPath_cz_1h As String = ViewConfiguration("RadarFileUriPath_cz_1h")
        Dim RadarFileUriPath_cz2 As String = ViewConfiguration("RadarFileUriPath_cz2")
        Dim RadarSaveMode As String = ViewConfiguration("RadarSaveMode")
        Dim RadarFilePath_cz As String
        Dim RadarFilePath_eu As String
        Dim RadarFilePath_cz2 As String
        Dim RadarFilePath As String
        Dim RadarFileUriPath As String = RadarFileUriPath_cz
        Dim RadarFileUriString As String
        Dim WebC As WebClient = WebUtils.CreateWebClient()

        Dim RadarFileTime As DateTime
        Dim RadarFileHourStart As DateTime
        Dim RadarFileHourEnd As DateTime
        Dim YearStr As String, MonthStr As String, DayStr As String
        Dim HourStr As String, MinuteStr As String
        Dim ShortYearStr As String
        Dim VariableId As Integer

        Dim ds As New DataSet
        Dim dr As DataRow

        'mohlo by byt i 2: posled. zaznam tentyz u obou
        Dim CurRadarnetId As Integer = 1
        Dim LastRadarObsTime As DateTime = ViewLastRadarFile(CurRadarnetId)

        Dim LastRadarObsTimeEU As DateTime = ViewLastRadarFile(2)
        Dim LastRadarObsTimeBourky As DateTime = ViewLastRadarFile(6)

        Dim cnn As New SqlConnection(SqlDbConnectionString)
        Dim cmd As SqlCommand = CreateRadarFileCommand(cnn)

        Dim i As Integer
        Dim TimeStepMinutes As Integer
        Dim TotalMinutes As Integer

        'get variable id and length of "Maxprecipobs" table time increment
        Select Case RadarSaveMode
            Case "preciphour"
                TotalMinutes = 60
                VariableId = 1
            Case "precipday"
            Case Else
                TotalMinutes = 1440
                VariableId = 2
        End Select

        'populate the dataset: hourly or daily times from LastObsTime, 
        'when rain > 0.2 mm was recorded
        'LogStr &= ViewMaxPrecipObsFrom(ds, LastRadarObsTime, VariableId)

        '------------------------------------------------------
        '******* CZECH RADARS *********************************
        '------------------------------------------------------
        cmd.Parameters("@radarnet_id").Value = 1
        RadarFileUriPath = RadarFileUriPath_cz
        RadarFilePath_cz = RadarFilePathBase & "cz\"
        TimeStepMinutes = 15

        'go through dataset rows and save the files
        RadarFileTime = LastRadarObsTime
        While RadarFileTime < DateTime.UtcNow

            'save file 15 minutes and 0 minutes before the hour

            'only proceed if the file is not yet saved and in database
            If RadarFileTime > LastRadarObsTime Then
                YearStr = Format(RadarFileTime.Year, "0000")

                MonthStr = Format(RadarFileTime.Month, "00")
                DayStr = Format(RadarFileTime.Day, "00")
                HourStr = Format(RadarFileTime.Hour, "00")
                MinuteStr = Format(RadarFileTime.Minute, "00")

                ShortYearStr = YearStr.Substring(2, 2)

                RadarFilePath = RadarFilePath_cz & YearStr & "\" & MonthStr & "\"

                RadarFileUriName = _
                    String.Format("pacz23.z_max3d.{0}{1}{2}.{3}{4}.0.png", YearStr, MonthStr, DayStr, HourStr, MinuteStr)

                RadarFileSavedName = _
                "cz" & YearStr & MonthStr & DayStr & HourStr & MinuteStr & ".png"

                RadarFileUriString = RadarFileUriPath & RadarFileUriName
                RadarFileSavedString = RadarFilePath & RadarFileSavedName

                'save the file to disk
                RadarFileSaved = SaveRadarFile(WebC, RadarFileUriString, RadarFileSavedString)

                'update the database
                If RadarFileSaved = True Then
                    cmd.Parameters("@obs_time").Value = RadarFileTime
                    Try
                        cnn.Open()
                        cmd.ExecuteNonQuery()
                        cnn.Close()
                        FileCount += 1
                    Catch ex As Exception
                        If Not cnn.State = ConnectionState.Closed Then cnn.Close()
                        LogStr &= ex.Message
                    End Try
                End If
            End If

            RadarFileTime = RadarFileTime.AddMinutes(TimeStepMinutes)

        End While


        '------------------------------------------------------
        '******* CZECH RADARS - BOURKY ************************
        '------------------------------------------------------
        cmd.Parameters("@radarnet_id").Value = 6
        RadarFileUriPath = RadarFileUriPath_cz2
        RadarFilePath_cz2 = RadarFilePathBase & "cz2\"
        TimeStepMinutes = 15

        'go through dataset rows and save the files
        RadarFileTime = LastRadarObsTimeBourky
        While RadarFileTime < DateTime.UtcNow

            'save file 15 minutes and 0 minutes before the hour

            'only proceed if the file is not yet saved and in database
            If RadarFileTime > LastRadarObsTime Then
                YearStr = Format(RadarFileTime.Year, "0000")

                MonthStr = Format(RadarFileTime.Month, "00")
                DayStr = Format(RadarFileTime.Day, "00")
                HourStr = Format(RadarFileTime.Hour, "00")
                MinuteStr = Format(RadarFileTime.Minute, "00")

                ShortYearStr = YearStr.Substring(2, 2)

                RadarFileUriName = _
                    String.Format("pacz2gmaps.z_max3d.{0}{1}{2}.{3}{4}.0.png", YearStr, MonthStr, DayStr, HourStr, MinuteStr)

                RadarFileSavedName = _
                "czx" & YearStr & MonthStr & DayStr & HourStr & MinuteStr & ".png"

                RadarFileUriString = RadarFileUriPath_cz2 & RadarFileUriName
                RadarFileSavedString = RadarFilePath_cz2 & RadarFileSavedName

                'save the file to disk
                RadarFileSaved = SaveRadarFile(WebC, RadarFileUriString, RadarFileSavedString)

                'update the database
                If RadarFileSaved = True Then
                    cmd.Parameters("@obs_time").Value = RadarFileTime
                    Try
                        cnn.Open()
                        cmd.ExecuteNonQuery()
                        cnn.Close()
                        FileCount += 1
                    Catch ex As Exception
                        If Not cnn.State = ConnectionState.Closed Then cnn.Close()
                        LogStr &= ex.Message
                    End Try
                End If
            End If

            RadarFileTime = RadarFileTime.AddMinutes(TimeStepMinutes)

        End While

        '------------------------------------------------------
        '******* EU RADARS ********************************
        '------------------------------------------------------
        cmd.Parameters("@radarnet_id").Value = 2
        RadarFileUriPath = RadarFileUriPath_eu
        RadarFilePath_eu = RadarFilePathBase & "\eu\"
        TimeStepMinutes = 15

        RadarFileTime = DateTime.UtcNow.AddDays(-1).Date
        While RadarFileTime < DateTime.UtcNow

            RadarFileTime = RadarFileTime.AddMinutes(TimeStepMinutes)

            'only proceed if the file is not yet saved and in database
            If RadarFileTime > LastRadarObsTimeEU Then

                YearStr = Format(RadarFileTime.Year, "0000")

                MonthStr = Format(RadarFileTime.Month, "00")
                DayStr = Format(RadarFileTime.Day, "00")
                HourStr = Format(RadarFileTime.Hour, "00")
                MinuteStr = Format(RadarFileTime.Minute, "00")

                RadarFileUriName = String.Format("radar.anim.{0}{1}{2}.{3}{4}.0.png", YearStr, MonthStr, DayStr, HourStr, MinuteStr)

                RadarFileSavedName = _
                "eu" & YearStr & MonthStr & DayStr & HourStr & MinuteStr & ".png"

                RadarFilePath = RadarFilePath_eu
                RadarFileUriString = RadarFileUriPath & RadarFileUriName
                RadarFileSavedString = RadarFilePath & RadarFileSavedName

                'save files to disk
                RadarFileSaved = SaveRadarFile(WebC, RadarFileUriString, RadarFileSavedString)

                'update the database
                If RadarFileSaved = True Then
                    cmd.Parameters("@obs_time").Value = RadarFileTime
                    Try
                        cnn.Open()
                        cmd.ExecuteNonQuery()
                        cnn.Close()
                        FileCount += 1
                    Catch ex As Exception
                        If Not cnn.State = ConnectionState.Closed Then cnn.Close()
                        LogStr &= ex.Message
                    End Try
                End If
            End If
        End While

        'skip czech radars now..
        ComputationTime = DateTime.Now.Subtract(StartTime)
        LogStr &= vbCrLf & "Files saved: " & FileCount.ToString & _
        vbCrLf & " Time taken: " & ComputationTime.ToString
        Return LogStr


        '------------------------------------------------------
        '******* CZECH RADAR+HOURLY PCP COMBINATION************
        '------------------------------------------------------
        cmd.Parameters("@radarnet_id").Value = 3
        RadarFileUriPath = RadarFileUriPath_cz_1h
        RadarFilePath_cz = RadarFilePathBase & "kombinace\1h\"
        CurRadarnetId = 3

        'select again last file in database!
        LastRadarObsTime = ViewLastRadarFile(CurRadarnetId)
        'VariableId = 1
        'LogStr &= ViewMaxPrecipObsFrom(ds, LastRadarObsTime, VariableId)
        TimeStepMinutes = 60
        'go through dataset rows and save the files
        For Each dr In ds.Tables("maxprecipobs").Rows
            RadarFileTime = CType(dr("obs_time"), DateTime)

            RadarFileTime = RadarFileTime.AddMinutes(-TotalMinutes)
            For i = -TotalMinutes + TimeStepMinutes To 0 Step TimeStepMinutes
                RadarFileTime = RadarFileTime.AddMinutes(TimeStepMinutes)

                If RadarFileTime > LastRadarObsTime Then
                    YearStr = Format(RadarFileTime.Year, "0000")
                    MonthStr = Format(RadarFileTime.Month, "00")
                    DayStr = Format(RadarFileTime.Day, "00")
                    HourStr = Format(RadarFileTime.Hour, "00")

                    RadarFilePath = RadarFilePath_cz & YearStr & "\" & MonthStr & "\"

                    RadarFileUriName = _
                        YearStr & MonthStr & DayStr & HourStr

                    RadarFileSavedName = _
                    "1h" & YearStr & MonthStr & DayStr & HourStr & ".png"

                    RadarFileUriString = RadarFileUriPath & RadarFileUriName
                    RadarFileSavedString = RadarFilePath & RadarFileSavedName

                    'save the file to disk
                    RadarFileSaved = SaveRadarFile(WebC, RadarFileUriString, RadarFileSavedString)

                    'update the database
                    If RadarFileSaved = True Then
                        cmd.Parameters("@obs_time").Value = RadarFileTime
                        Try
                            cnn.Open()
                            cmd.ExecuteNonQuery()
                            cnn.Close()
                            FileCount += 1
                        Catch ex As Exception
                            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
                            LogStr &= ex.Message
                        End Try
                    End If
                End If
            Next i

        Next dr


        '------------------------------------------------------
        '******* CZECH RADAR+6-hour PCP COMBINATION***********
        '------------------------------------------------------
        cmd.Parameters("@radarnet_id").Value = 4
        RadarFileUriPath = ViewConfiguration("RadarFileUriPath_cz_6h")
        RadarFilePath_cz = RadarFilePathBase & "kombinace\6h\"
        CurRadarnetId = 4

        'select again last file in database!
        LastRadarObsTime = ViewLastRadarFile(CurRadarnetId)
        'VariableId = 1
        'LogStr &= ViewMaxPrecipObsFrom(ds, LastRadarObsTime, VariableId)
        TimeStepMinutes = 360
        'go through dataset rows and save the files
        For Each dr In ds.Tables("maxprecipobs").Rows
            RadarFileTime = CType(dr("obs_time"), DateTime)

            RadarFileTime = RadarFileTime.AddMinutes(-TotalMinutes)
            For i = -TotalMinutes + TimeStepMinutes To 0 Step TimeStepMinutes
                RadarFileTime = RadarFileTime.AddMinutes(TimeStepMinutes)

                If RadarFileTime > LastRadarObsTime Then
                    YearStr = Format(RadarFileTime.Year, "0000")
                    MonthStr = Format(RadarFileTime.Month, "00")
                    DayStr = Format(RadarFileTime.Day, "00")
                    HourStr = Format(RadarFileTime.Hour, "00")

                    RadarFilePath = RadarFilePath_cz & YearStr & "\" & MonthStr & "\"

                    RadarFileUriName = _
                        YearStr & MonthStr & DayStr & HourStr

                    RadarFileSavedName = _
                    "6h" & YearStr & MonthStr & DayStr & HourStr & ".png"

                    RadarFileUriString = RadarFileUriPath & RadarFileUriName
                    RadarFileSavedString = RadarFilePath & RadarFileSavedName

                    'save the file to disk
                    RadarFileSaved = SaveRadarFile(WebC, RadarFileUriString, RadarFileSavedString)

                    'update the database
                    If RadarFileSaved = True Then
                        cmd.Parameters("@obs_time").Value = RadarFileTime
                        Try
                            cnn.Open()
                            cmd.ExecuteNonQuery()
                            cnn.Close()
                            FileCount += 1
                        Catch ex As Exception
                            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
                            LogStr &= ex.Message
                        End Try
                    End If
                End If
            Next i

        Next dr


        '------------------------------------------------------
        '******* CZECH RADAR+24-hour PCP COMBINATION***********
        '------------------------------------------------------
        cmd.Parameters("@radarnet_id").Value = 5
        RadarFileUriPath = ViewConfiguration("RadarFileUriPath_cz_24h")
        RadarFilePath_cz = RadarFilePathBase & "kombinace\24h\"
        CurRadarnetId = 5

        'go through dataset rows and save the files
        For Each dr In ds.Tables("maxprecipobs").Rows
            RadarFileTime = CType(dr("obs_time"), DateTime)

            If RadarFileTime > LastRadarObsTime Then
                YearStr = Format(RadarFileTime.Year, "0000")
                MonthStr = Format(RadarFileTime.Month, "00")
                DayStr = Format(RadarFileTime.Day, "00")
                HourStr = Format(RadarFileTime.Hour, "00")

                RadarFilePath = RadarFilePath_cz & YearStr & "\" & MonthStr & "\"

                RadarFileUriName = _
                    YearStr & MonthStr & DayStr & HourStr

                RadarFileSavedName = _
                "24h" & YearStr & MonthStr & DayStr & HourStr & ".png"

                RadarFileUriString = RadarFileUriPath & RadarFileUriName
                RadarFileSavedString = RadarFilePath & RadarFileSavedName

                'save the file to disk
                RadarFileSaved = SaveRadarFile(WebC, RadarFileUriString, RadarFileSavedString)

                'update the database
                If RadarFileSaved = True Then
                    cmd.Parameters("@obs_time").Value = RadarFileTime
                    Try
                        cnn.Open()
                        cmd.ExecuteNonQuery()
                        cnn.Close()
                        FileCount += 1
                    Catch ex As Exception
                        If Not cnn.State = ConnectionState.Closed Then cnn.Close()
                        LogStr &= ex.Message
                    End Try
                End If
            End If
        Next dr


        'time of computation
        ComputationTime = DateTime.Now.Subtract(StartTime)
        LogStr &= vbCrLf & "Files saved: " & FileCount.ToString & _
        vbCrLf & " Time taken: " & ComputationTime.ToString
        Return LogStr

    End Function

    'auxiliary function: saves the radar file to disk. 
    'CALLED BY: UpdateRadar()
    'RETURNS: TRUE, if file was successfully saved
    'FALSE, if the file was not saved

    Private Function SaveRadarFile(ByVal WebC As WebClient, _
    ByVal UriStr As String, ByVal FileNameStr As String) _
    As Boolean
        Dim FileSaved As Boolean = False

        Try
            WebC.DownloadFile(UriStr, FileNameStr)
            FileSaved = True
        Catch ex As Exception
            FileSaved = False
        End Try
        Return FileSaved
    End Function

    'get the elevation of any point using Google elevation API
    Private Function GetElevation(ByVal lat As Double, ByVal lon As Double) As Double
        Dim uri As String = String.Format("http://maps.googleapis.com/maps/api/elevation/json?locations={0},{1}&sensor=false", _
                                          lat, lon)
        Dim elev As Double = 0.0
        Dim cli As New WebClient()
        Try
            Dim resp As String = cli.DownloadString(uri)
            Dim reg As New Regex("\""elevation\""\s*\:\s*(?<1>[\d\.]+)\s*\,", RegexOptions.IgnoreCase Or RegexOptions.Compiled)
            Dim m As Match = reg.Match(resp)
            If m.Success Then
                Dim elevStr As String = m.Groups(1).Value
                elev = CDbl(elevStr)
            End If

        Catch ex As Exception

        End Try
        Return elev
    End Function


    '-----------------------------------------------------------------------------------------------
    '-----------------------------------------------------------------------------------------------
    '********************** PRECIPITATION **********************************************************
    '-----------------------------------------------------------------------------------------------

    '-----------------------------------------------------------------------------------------------
    '********************** DAILY PRECIPITATION ****************************************************
    '-----------------------------------------------------------------------------------------------


    'auxiliary subroutine
    'DESCRIPTION:
    'downloads and updates daily climate data (precip, snow) from CHMI synoptic stations
    '(website http://www.chmi.cz/meteo/opss/pocasi)
    'CALLED BY:
    'UpdateObsPrecipDay(), UpdateObsSnow2
    'PARAMETERS:
    'UriName ... name of the webpage uri in 'configuration' database table
    'VariableName ... name of the variable in 'variables' database table
    'LogStr ... string for reporting error messages
    'CALLS:
    '
    'RETURNS: number of updated and added data records
    Private Sub UpdateSnow_Chmi_Synop(ByVal DataUriStr As String,
    ByRef logstr As String, ByRef NumUpdatedRows As Integer, ByRef NumAddedRows As Integer)

        Dim VariableName As String = "snow"
        Dim WebC As WebClient = WebUtils.CreateWebClient()
        Dim buf() As Byte
        Dim FileString As String

        'variables for creating snow station table
        Dim cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim StationCmd As SqlCommand = Me.CreateStationIdForSnowCommand(cnn)
        Dim SnowCmd As SqlCommand = Me.CreateSnowCommand(cnn)
        Dim ZeroStationList As New ArrayList(20)
        Dim CoordType As Integer = 25

        'variables for reading files from the web
        Dim r1, r2 As Regex
        Dim m As Match
        Dim MatchCount As Integer = 0
        Dim CurStr As String
        Dim CurNameStr As String
        Dim CurValueStr As String
        Dim CoordTopStr As String
        Dim CoordLeftStr As String

        'observation variables
        Dim ObsTime As DateTime
        Dim ObsValue As Integer
        Dim NoDataValue As Integer = -9999
        Dim CurStationId As Integer
        Dim CurStationCoord As Integer
        Dim ScaleFactor As Single = 1
        Dim VariableId As Integer = 0

        'regex for reading observation data
        r1 = New Regex("class\s*=\s*[""]?vgtext[""]?\s+" & _
                "title\s*=\s*""(?<1>[^""]+)""\s+" & _
                "style\s*=\s*[""]?position:\s*absolute;\s*" & _
                "LEFT\:(?<2>\d+)\D+TOP\:\s*(?<3>\d+)[^>]*>(?<4>[^<]*)</DIV>", _
                RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'regex for reading observation time
        r2 = New Regex("<CENTER>\s*\(?\s*(?<1>\d{2})\.(?<2>\d{2})\.(?<3>\d{4})", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'get variable id
        VariableId = 8

        'read web data file
        Try
            buf = WebC.DownloadData(DataUriStr)
            FileString = System.Text.Encoding.GetEncoding(1250).GetString(buf)
        Catch ex As Exception
            FileString = ""
            logstr &= vbCrLf & "downloading file:" & ex.Message
        End Try

        'get observation date
        m = r2.Match(FileString)
        If m.Success Then
            CurStr = m.Value
            ObsTime = New DateTime(Integer.Parse(m.Groups(3).Value), _
            Integer.Parse(m.Groups(2).Value), _
            Integer.Parse(m.Groups(1).Value), 6, 0, 0)

        Else
            logstr &= vbCrLf & "CHMI lowlands: No regex match for ObsTime!"
        End If

        'read the observations
        MatchCount = 0
        m = r1.Match(FileString)
        While m.Success
            MatchCount = MatchCount + 1
            CurStr = m.Value
            CurNameStr = m.Groups(1).Value.Trim()
            CoordLeftStr = m.Groups(2).Value
            CoordTopStr = m.Groups(3).Value
            CurValueStr = m.Groups(4).Value
            CurValueStr = CurValueStr.ToLower()

            If IsValidNumber(CurValueStr) Then
                ObsValue = CInt(Math.Round(Me.String2Single(CurValueStr) * ScaleFactor))
            Else
                ObsValue = 0
                If CurValueStr.IndexOf("pop") >= 0 Then ObsValue = -2
                If CurValueStr.IndexOf("nem") >= 0 Then ObsValue = -2
                If CurValueStr.IndexOf("nes") >= 0 Then ObsValue = -1
            End If
            CurStationCoord = Integer.Parse(CoordTopStr & CoordLeftStr)
            'find station id in the database
            CurStationId = Me.GetStationIdForSnow(cnn, StationCmd, CurNameStr)
            'update observation
            If CurStationId > 0 Then
                logstr &= Me.ExecuteSnowCommand(SnowCmd, CurStationId, ObsTime, ObsValue, NumUpdatedRows, NumAddedRows)
            End If
            m = m.NextMatch()
        End While
        'write error message if there was no match
        If MatchCount = 0 Then
            logstr &= vbCrLf & "CHMI data file: No regex match for " & VariableName
        End If

    End Sub

    'auxiliary subroutine
    'DESCRIPTION:
    'downloads and updates daily climate data (precip, snow) from CHMI synoptic stations
    '(website http://www.chmi.cz/meteo/opss/pocasi)
    'CALLED BY:
    'UpdateObsPrecipDay(), UpdateObsSnow2
    'PARAMETERS:
    'UriName ... name of the webpage uri in 'configuration' database table
    'VariableName ... name of the variable in 'variables' database table
    'LogStr ... string for reporting error messages
    'CALLS:
    '
    'RETURNS: number of updated and added data records
    Private Sub UpdatePrecipitation_Daily_CHMI(ByRef logstr As String, _
                                               ByRef NumUpdatedRows As Integer, _
                                               ByRef NumAddedRows As Integer)

        Dim UriName As String = "PcpUriChmiDaily" 'set this to the chmi's precipitation URI
        Dim VariableName As String = "precip_day"

        Dim DataUriStr As String = Me.ViewConfiguration(UriName)
        Dim WebC As WebClient = WebUtils.CreateWebClient()
        Dim buf() As Byte
        Dim FileString As String

        'variables for creating snow station table
        Dim cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim StationCmd As SqlCommand = Me.CreateStationIdForSnowCommand(cnn)
        Dim ObsCmd As SqlCommand = Me.CreatePrecipitationDailyCommand(cnn)
        Dim ZeroStationList As New ArrayList(20)
        Dim CoordType As Integer = 25

        'variables for reading files from the web
        Dim r1, r2 As Regex
        Dim m As Match
        Dim MatchCount As Integer = 0
        Dim CurStr As String
        Dim CurNameStr As String
        Dim CurValueStr As String
        Dim CoordTopStr As String
        Dim CoordLeftStr As String

        'observation variables
        Dim ObsTime As DateTime
        Dim ObsValue As Integer
        Dim NoDataValue As Integer = -9999
        Dim CurStationId As Integer
        Dim CurStationCoord As Integer
        Dim ScaleFactor As Single = 1
        Dim VariableId As Integer = 0

        'regex for reading observation data
        r1 = New Regex("class\s*=\s*[""]?vgtext[""]?\s+" & _
                "title\s*=\s*""(?<1>[^""]+)""\s+" & _
                "style\s*=\s*[""]?position:\s*absolute;\s*" & _
                "LEFT\:(?<2>\d+)\D+TOP\:\s*(?<3>\d+)[^>]*>(?<4>[^<]*)</DIV>", _
                RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'regex for reading observation time
        r2 = New Regex("<CENTER>\s*\(?\s*(?<1>\d{2})\.(?<2>\d{2})\.(?<3>\d{4})", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'get variable scale factor and id
        ScaleFactor = Me.GetScaleFactor(cnn, VariableName)
        VariableId = Me.GetVariableId(cnn, VariableName)
        If ScaleFactor = -1 Then
            logstr &= vbCrLf & "Executing UpdatePrecipitation_Daily_CHMI.. " & _
            "No scale factor specified for variable " & VariableName
            Exit Sub
        End If
        'read web data file
        Try
            buf = WebC.DownloadData(DataUriStr)
            FileString = System.Text.Encoding.GetEncoding(1250).GetString(buf)
        Catch ex As Exception
            FileString = ""
            logstr &= vbCrLf & "downloading file:" & ex.Message
        End Try

        'get observation date
        m = r2.Match(FileString)
        If m.Success Then
            CurStr = m.Value
            ObsTime = New DateTime(Integer.Parse(m.Groups(3).Value), _
            Integer.Parse(m.Groups(2).Value), _
            Integer.Parse(m.Groups(1).Value), 6, 0, 0)
            'for 24h precipitation, change date to next day
            '(end of 24-h period)
            'NOTE that in climatology, the precipitation on day d measured
            'at 7:00 local time of day d is assigned to day d-1 !!! (PREVIOUS DAY)
            ObsTime = ObsTime.AddDays(1)
        Else
            logstr &= vbCrLf & "CHMI daily precipitation: No regex match for ObsTime!"
        End If

        'read the observations
        MatchCount = 0
        m = r1.Match(FileString)
        While m.Success
            MatchCount = MatchCount + 1
            CurStr = m.Value
            CurNameStr = m.Groups(1).Value.Trim()
            CoordLeftStr = m.Groups(2).Value
            CoordTopStr = m.Groups(3).Value
            CurValueStr = m.Groups(4).Value
            CurValueStr = CurValueStr.ToLower()

            If IsValidNumber(CurValueStr) Then
                ObsValue = CInt(Math.Round(Me.String2Single(CurValueStr) * ScaleFactor))
            Else
                ObsValue = 0
                If CurValueStr.IndexOf("pop") >= 0 Then ObsValue = -2
                If CurValueStr.IndexOf("nem") >= 0 Then ObsValue = -2
                If CurValueStr.IndexOf("nes") >= 0 Then ObsValue = -1
            End If
            CurStationCoord = Integer.Parse(CoordTopStr & CoordLeftStr)
            'find station id in the database
            CurStationId = Me.GetStationIdForSnow(cnn, StationCmd, CurNameStr)
            'update observation
            If CurStationId > 0 Then
                logstr &= Me.ExecutePrecipitationDailyCommand(ObsCmd, CurStationId, ObsTime, ObsValue, _
                                                              NumUpdatedRows, NumAddedRows)
            End If
            m = m.NextMatch()
        End While
        'write error message if there was no match
        If MatchCount = 0 Then
            logstr &= vbCrLf & "CHMI data file: No regex match for " & VariableName
        End If
    End Sub


    'auxiliary function
    'DESCRIPTION:
    'for a given variable name, find out the scale factor
    'PARAMETERS: VariableName ... name of the variable as
    ' specified in "variables" database table
    ' cnn: the sql connection
    'RETURNS: scale factor
    Private Function GetScaleFactor(ByVal cnn As SqlConnection, _
    ByVal VariableName As String) As Single
        Dim ScaleFactor As Single = -1
        Dim CmdStr As String = "SELECT scalefactor from variables WHERE var_name='"
        CmdStr = CmdStr & VariableName & "'"
        Dim cmd As New SqlCommand(CmdStr, cnn)
        Try
            cnn.Open()
            ScaleFactor = CType(cmd.ExecuteScalar, Single)
        Catch ex As Exception
        Finally
            cnn.Close()
        End Try
        Return ScaleFactor
    End Function

    Private Function GetVariableId(ByVal cnn As SqlConnection, _
        ByVal VariableName As String) As Integer
        Dim VariableId As Integer = 0
        Dim CmdStr As String = "SELECT var_id from variables WHERE var_name='"
        CmdStr = CmdStr & VariableName & "'"
        Dim cmd As New SqlCommand(CmdStr, cnn)
        Try
            cnn.Open()
            VariableId = CType(cmd.ExecuteScalar, Integer)
        Catch ex As Exception
        Finally
            cnn.Close()
        End Try
        Return VariableId
    End Function

    '********************** PRECIPITATION FROM POVODI (24-hour precipitation) **********************
    'public method
    'DESCRIPTION:
    ' downloads and updates hourly precipitation in Povodi stations
    'CALLS:
    ' UpdatePrecip_povodi_station(stUri,st,NumAddedRows)
    'RETURNS:
    ' LogStr: String containing report and exception messages
    Public Function UpdatePrecip_Hourly_Povodi() As String
        'log message variables
        Dim LogStr As String = "Executing UpdatePrecip_Hourly_Povodi.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        Dim NumAddedRows As Integer = 0
        Dim CurStationSeq As Integer = 0

        Dim povodiCodes() As String = {"PVL", "POH", "PLA", "PMO", "POD"}

        For Each povodiCode In povodiCodes

            StartTime = DateTime.Now
            NumAddedRows = 0
            LogStr &= vbCrLf & povodiCode & ":"

            '(1) get the table of precip stations
            Dim stationList As List(Of PrecipStation) = GetPrecipStationTable_Hourly(povodiCode)

            '(2) for each station in the table: download page and update values
            For Each st As PrecipStation In stationList
                Dim stUri As String
                stUri = GetStationUri_Povodi(st.MeteoCode, st.DivisionName, st.OperatorId)
                UpdatePrecip_povodi_station(stUri, st, NumAddedRows)
                'also save the graph...
                Dim graphUri As String = GetGraphUri_Povodi(st.MeteoCode, st.DivisionName, st.OperatorId)
                SaveGraph_Povodi(st, graphUri)
            Next st

            ComputationTime = DateTime.Now.Subtract(StartTime)
            LogStr &=
            "rows added: " & NumAddedRows & ", " & _
            "Time taken: " & ComputationTime.ToString

        Next povodiCode

        Return LogStr
    End Function

    '********************** PRECIPITATION FROM LVS (1-hour precipitation) **********************
    'public method
    'DESCRIPTION:
    ' downloads and updates hourly precipitation in LVS plzen stations
    'CALLS:
    ' UpdatePrecip_Hourly_LVS(LVS,NumUpdatedRows,NumAddedRows)
    'RETURNS:
    ' LogStr: String containing report and exception messages
    Public Function UpdatePrecipHourly_LVS() As String
        'log message variables
        Dim LogStr As String = "Executing UpdatePrecip_Hourly_LVS.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        Dim NumAddedRows As Integer = 0
        Dim CurStationSeq As Integer = 0

        Dim lvsName As String = "plzen"

        '(1) get the table of precip stations: for each lvs
        Dim stationList As List(Of PrecipStation) = GetPrecipStationTable_Hourly(lvsName)
        'TODO add the other LVS's

        '(2) for each station in the table: download page and update values
        Dim stBaseUri As String = String.Format("http://dvt-info.cz/web_{0}/dvtsite_public/SiteChartTable.aspx?site=", _
                                                lvsName)
        Dim dvtCookies As CookieContainer = GetDvtCookies(lvsName)
        For Each st As PrecipStation In stationList
            Dim stUri As String = stBaseUri & st.Code
            'stUri = "http://localhost/test/S_201.htm" 'JUST FOR TEST!!!
            UpdatePrecip_LVS_Station(stUri, st.MaxDbTime, st, lvsName, dvtCookies, NumAddedRows)
        Next

        'return result log of the function
        ComputationTime = DateTime.Now.Subtract(StartTime)
        Return LogStr & vbCrLf & _
        "rows added: " & NumAddedRows & vbCrLf & _
        "Time taken: " & ComputationTime.ToString
    End Function

    Private Function GetStationUri_Povodi(ByVal meteoCode As String, ByVal division As String, ByVal operatorId As Integer) As String

        Select Case operatorId
            Case 2
                'pvl
                Return String.Format("http://www.pvl.cz/portal/Srazky/en/PC/Mereni.aspx?id={0}&oid={1}", meteoCode, division)
            Case 3
                'poh
                Return String.Format("http://sap.poh.cz/portal/Srazky/en/PC/Mereni.aspx?id={0}&oid={1}", meteoCode, division)
            Case 4
                'pla
                Return String.Format("http://www.pla.cz/portal/srazky/en/PC/Mereni.aspx?id={0}&oid={1}", meteoCode, division)
            Case 5
                'pmo
                Return String.Format("http://www.pmo.cz/portal/srazky/en/mereni_{0}.htm", meteoCode)
            Case 6
                'pod
                Return String.Format("http://app.pod.cz/portal/srazky/en/PC/Mereni.aspx?id={0}&oid={1}", meteoCode, division)
            Case Else
                Return "BadURL"
        End Select

    End Function


    Private Function GetGraphUri_Povodi(ByVal meteoCode As String, ByVal division As String, ByVal operatorId As Integer) As String

        Select Case operatorId
            Case 2
                'pvl
                Return String.Format("http://www.pvl.cz/portal/Srazky/Images/grafy/GrafSS24T_{0}_{1}.png", division, meteoCode)
            Case 3
                'poh
                Return String.Format("http://sap.poh.cz/portal/Srazky/Images/grafy/GrafSS24T_{0}_{1}.png", division, meteoCode)
            Case 4
                'pla
                Return String.Format("http://www.pla.cz/portal/Srazky/Images/grafy/GrafSS24T_{0}_{1}.png", division, meteoCode)
            Case 5
                'pmo
                Return String.Format("http://www.pmo.cz/portal/srazky/grafy/sr{0}_en.gif", meteoCode)
            Case 6
                'pod
                Return String.Format("http://app.pod.cz/portal/Srazky/Images/GrafSS24T_{0}_{1}.png", division, meteoCode)
            Case Else
                Return "BadURL"
        End Select

    End Function

    'saves the screenshot of the graph for future usage
    Private Sub SaveGraph_Povodi(ByVal st As PrecipStation, ByVal graphUri As String)

        Dim fileDir As String = ConfigurationManager.AppSettings("files_dir1")
        Dim siteCode As String = st.Id.ToString("D4")
        Dim curDateStr As String = DateTime.Now.Date.ToString("yyyyMMdd")
        Dim graphFile As String = String.Format("{0}_{1}.png", curDateStr, siteCode)
        If st.OperatorId = 5 Then
            graphFile = String.Format("{0}_{1}.gif", curDateStr, siteCode)
        End If
        Dim graphPath = Path.Combine(fileDir, "grafy", graphFile)
        Dim wc As New WebClient()
        Try
            wc.DownloadFile(graphUri, graphPath)
        Catch ex As Exception
            Dim msg As String = ex.Message
        End Try
    End Sub

    'updates HOURLY precipitation for a station of POVODI !!!
    Private Function UpdatePrecip_povodi_station(ByVal Url As String, _
    ByVal st As PrecipStation, ByRef AddedRows As Integer) As String

        Dim LogStr As String = ""

        'declare local variables
        Dim Observations As New List(Of TimeValuePair)
        Dim ObsHours As Integer = 0
        Dim MaxObsHours As Integer = 0
        Dim AdditionalHour As Integer = 0

        Dim DbMissingValue As Single = 999.9

        'resolve the URL of povodi
        Dim webC As WebClient = WebUtils.CreateWebClient()
        Dim buf() As Byte
        Dim fileString As String

        'TODO change the regex expression
        Dim r1 As New Regex( _
        "<tr>\s*<td[^>]*>\s*<font[^>]+>(?<1>[\d\.\:\s]+)[^<]*</font></td>\s*" & _
        "<td[^>]*><font[^>]+>(?<2>[\d\.,]*)[^<]*</font></td>\s*" & _
        "<td[^>]*><font[^>]+>(?<3>[\d\.,-]*)[^<]*</font></td>\s*" & _
        "</tr>", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'Dim rPVL As New Regex( _
        '    "<td class\s*=\s*\""bunkaGridu\"">(?<1>[^<]*)</td>\s*" & _
        '    "<td[^>]*>\s*<span[^>]+>(?<2>[\d\.,]*)\s*</span>\s*</td>" & _
        '    "<td[^>]*>\s*<span[^>]+>(?<3>[\d\.,]*)\s*</span>\s*</td>\s*" & _
        '    "</tr>", _
        '    RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Dim rPVL As New Regex( _
            "<td class\s*=\s*\""bunkaGridu\"">(?<1>[^<]*)</td>\s*" & _
            "<td[^>]*>\s*<span[^>]+>(?<2>[\d\.,]*)\s*</span>\s*</td>" & _
            "<td[^>]*>\s*<span[^>]+>(?<3>[\d\.,-]*)\s*</span>\s*</td>\s*" & _
            "</tr>", _
            RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'download the file from web
        Try
            buf = webC.DownloadData(Url)
            fileString = System.Text.Encoding.GetEncoding(1250).GetString(buf)
        Catch ex As Exception
            LogStr &= vbCrLf & String.Format("Unable to download file for station {0}", Url)
            Return LogStr
        End Try

        'regex matching
        Dim MatchCount As Integer = 0
        Dim CurStr As String
        Dim m1 As Match
        Dim CurValue As Single = -1
        Dim CurTime As DateTime
        Dim i As Integer = 0

        'setup list of observations
        Observations.Clear()

        'set the times
        MaxObsHours = 23

        'special case for PMO
        If st.OperatorId = 5 Then
            m1 = r1.Match(fileString)
        Else
            m1 = rPVL.Match(fileString)
        End If

        While (m1.Success)
            MatchCount = MatchCount + 1
            CurStr = m1.Groups(2).Value
            If CurStr.Length > 0 Then
                CurValue = Me.String2Single(CurStr)
            Else
                CurValue = DbMissingValue
            End If

            Dim timeStr As String = m1.Groups(1).Value
            Dim day As Integer = CInt(timeStr.Substring(0, 2))
            Dim mon As Integer = CInt(timeStr.Substring(3, 2))
            Dim year As Integer
            Dim hour As Integer
            Dim minute As Integer

            If st.OperatorId = 5 Then
                year = 2000 + CInt(timeStr.Substring(6, 2))
                hour = CInt(timeStr.Substring(9, 2))
                minute = CInt(timeStr.Substring(12, 2))
            Else
                year = CInt(timeStr.Substring(6, 4))
                hour = CInt(timeStr.Substring(11, 2))
                minute = CInt(timeStr.Substring(14, 2))
            End If

            CurTime = New DateTime(year, mon, day, hour, minute, 0)

            Observations.Add(New TimeValuePair(CurTime, CurValue))

            m1 = m1.NextMatch()
        End While

        'write error message if there was no match
        If MatchCount = 0 Then
            LogStr &= vbCrLf & Url & ": No regex match!"
        End If

        'save the observations for this station!
        Dim observationList As New List(Of TimeValuePair)
        'add observations to the observation list in reverse
        For i = Observations.Count - 1 To 0 Step -1
            Dim obs As TimeValuePair = Observations(i)
            observationList.Add(New TimeValuePair(obs.DateTime, obs.Value))
        Next
        'process the observations - add them to the station!
        'write them to the binary file
        If observationList.Count > 0 Then

            Dim fileDir As String = ConfigurationManager.AppSettings("files_dir1")
            Dim siteCode As String = st.Id.ToString("D4")
            Dim binaryFile As String = "h" & "_" & "srazky" & "_" & siteCode & ".dat"

            Dim fileName As String = fileDir & "\h\srazky\" & binaryFile
            Dim bfm As New BinaryFileManager()
            bfm.AddValues(fileName, observationList, "h")

        End If

        Return LogStr
    End Function


    '-----------------------------------------------------------------------------------------------
    '********************** PRECIPITATION: CHMU (NEW VERSION ! *************************************
    '-----------------------------------------------------------------------------------------------
    'public method
    'DESCRIPTION:
    'downloads and updates hourly precipitation in CHMI stations
    'CALLS:
    'GetPrecipStationTable(),
    'DownloadPrecip(),
    'ViewConfiguration(),
    'methods of PrecipStation class
    'RETURNS:
    'LogStr: String containing exception messages
    Public Function UpdatePrecip_Hourly_CHMU() As String
        'log message variables
        Dim LogStr As String = "Executing UpdatePrecip.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        Dim DayOffset As Integer
        Dim UpdatedRows As Integer = 0
        Dim AddedRows As Integer = 0
        Dim curStation As PrecipStation
        Dim cnn As New SqlConnection(m_SqlDbConnectionString)

        'the max.number of days to go back downloading chmi pages
        Dim MaxDayOffset As Integer = -7
        'Dim stKey As Integer

        'first create a precip. station table from the DB
        Dim stationTable As New Hashtable
        LogStr &= vbCrLf & "Executing GetPrecipStationTable_Hourly.."
        Dim stList As List(Of PrecipStation) = GetPrecipStationTable_Hourly("CHMI")
        For Each st As PrecipStation In stList
            st.MaxDbTime = st.FindMaxDbTime()
            Try
                stationTable.Add(st.Seq, st)
            Catch ex As Exception
                Dim msg As String = ex.Message
            End Try
        Next

        'exclude the today's day
        For DayOffset = MaxDayOffset To -1 Step 1
            LogStr = LogStr & Me.DownloadPrecip(DayOffset, stationTable, UpdatedRows)
        Next

        'add the values for each station to database
        For Each stEntry As DictionaryEntry In stationTable
            curStation = CType(stEntry.Value, PrecipStation)
            If curStation.HasObservations Then
                curStation.UpdateBinaryFile(AddedRows)
                'curStation.UpdateDB(cnn, AddedRows)
            End If
        Next

        'return result log of the function
        ComputationTime = DateTime.Now.Subtract(StartTime)
        Return LogStr & vbCrLf & _
        "rows added: " & AddedRows & vbCrLf & _
        "Time taken: " & ComputationTime.ToString
    End Function

    Public Sub delete_chmi_binary_files()
        Dim stTable = GetPrecipStationTable_Hourly("CHMI")
        For Each st As PrecipStation In stTable
            Dim binFileName As String = st.BinaryFileName
            If File.Exists(binFileName) Then
                File.Delete(binFileName)
            End If
        Next
    End Sub

    'download metadata about CHMI precipitation stations
    Public Function DownloadPrecipMetadata(ByVal DayOffset As Integer) As DataTable
        Dim UriString As String
        Dim BaseUriString As String = "http://hydro.chmi.cz/hpps/hpps_act_rain.php?KRAJ=&UC_POV=&POB=&ordstr=nvys%20desc&recnum=50"
        Dim WebC As WebClient = WebUtils.CreateWebClient()

        Dim Buf() As Byte
        Dim FileString As String
        Dim CurValueStr As String
        Dim CurSeqStr As String
        Dim CurNameStr As String
        Dim CurElevStr As String

        Dim dt As New DataTable()
        dt.Columns.Add("seq")
        dt.Columns.Add("name")
        dt.Columns.Add("elev")


        Dim r1 As New Regex( _
            ";seq=(?<1>\d+)[^>]*>(?<2>[^<]+)</A>\s*" & _
            "<A[^>]+><img[^>]+></A>\s*</TD>\s*" & _
            "<TD\s*>(?<3>[^<]*)</TD>\s*",
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Dim recnum As Integer = 50
        For startPos = 0 To 350 Step recnum

            UriString = _
            BaseUriString & _
            String.Format("&startpos={0}&day_offset={1}", startPos, DayOffset)

            Try
                Buf = WebC.DownloadData(UriString)
                FileString = System.Text.Encoding.GetEncoding(1250).GetString(Buf)

                Dim m As Match
                Dim MatchCount As Integer = 0
                m = r1.Match(FileString)
                Dim stN As Integer = 0
                While m.Success
                    MatchCount = MatchCount + 1
                    CurValueStr = m.Value
                    CurSeqStr = m.Groups(1).Value
                    CurNameStr = m.Groups(2).Value
                    CurElevStr = m.Groups(3).Value
                    
                    Dim qry As String = "seq='" & CurSeqStr & "'"
                    Dim foundRow() As DataRow = dt.Select(qry)
                    If foundRow.Length = 0 Then
                        Dim r As DataRow = dt.NewRow
                        r("name") = CurNameStr
                        r("seq") = CInt(CurSeqStr)
                        r("elev") = CurElevStr
                        dt.Rows.Add(r)
                    End If
                    m = m.NextMatch()
                End While


            Catch ex As Exception
                FileString = ""
            End Try
        Next startPos

        Return dt

    End Function


    'download metadata about CHMI precipitation stations
    Public Function DownloadHydroMetadata() As DataTable
        Dim UriString As String = "http://hydro.chmi.cz/hpps/hpps_main.php"
        Dim WebC As WebClient = WebUtils.CreateWebClient()

        Dim Buf() As Byte
        Dim FileString As String
        Dim CurValueStr As String
        Dim CurNameStr As String
        Dim CurRiverStr As String
        Dim CurSeqStr As String

        Dim dt As New DataTable()
        dt.Columns.Add("seq")
        dt.Columns.Add("name")
        dt.Columns.Add("river")


        Dim r1 As New Regex( _
            "self.document\,'(?<1>[^']+)'\s*\,'(?<2>[^']+)'\s*\,'(?<3>[^']+)'\s*" & _
            "\,'(?<4>[^']+)'\s*\,'(?<5>[^']+)'\s*\,'(?<6>[^']+)'\s*\,'(?<7>[^']+)'",
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Try
            Buf = WebC.DownloadData(UriString)
            FileString = System.Text.Encoding.GetEncoding(1250).GetString(Buf)

            Dim m As Match
            Dim MatchCount As Integer = 0
            m = r1.Match(FileString)
            Dim stN As Integer = 0
            While m.Success
                MatchCount = MatchCount + 1
                CurValueStr = m.Value
                CurNameStr = m.Groups(1).Value
                CurRiverStr = m.Groups(2).Value
                CurSeqStr = m.Groups(7).Value


                Dim qry As String = "seq='" & CurSeqStr & "'"
                Dim foundRow() As DataRow = dt.Select(qry)
                If foundRow.Length = 0 Then
                    Dim r As DataRow = dt.NewRow
                    r("name") = CurNameStr
                    r("seq") = CInt(CurSeqStr)
                    r("river") = CurRiverStr
                    dt.Rows.Add(r)
                End If
                m = m.NextMatch()
            End While


        Catch ex As Exception
            FileString = ""
        End Try

        Return dt

    End Function

    Public Function DownloadHydroMetadata_Povodi() As DataTable

        Dim dt As New DataTable()
        dt.Columns.Add("operator")
        dt.Columns.Add("division")
        dt.Columns.Add("name2")
        dt.Columns.Add("name")
        dt.Columns.Add("river")

        DownloadHydroMetadata_Povodi_division("pvl", "1", dt)
        DownloadHydroMetadata_Povodi_division("pvl", "2", dt)
        DownloadHydroMetadata_Povodi_division("pvl", "3", dt)

        DownloadHydroMetadata_Povodi_division("poh", "1", dt)
        DownloadHydroMetadata_Povodi_division("poh", "2", dt)
        DownloadHydroMetadata_Povodi_division("poh", "3", dt)

        DownloadHydroMetadata_Povodi_division("pla", "1", dt)
        DownloadHydroMetadata_Povodi_division("pla", "2", dt)
        DownloadHydroMetadata_Povodi_division("pla", "3", dt)
        DownloadHydroMetadata_Povodi_division("pla", "4", dt)
        DownloadHydroMetadata_Povodi_division("pla", "5", dt)

        DownloadHydroMetadata_Povodi_division("pod", "1", dt)
        DownloadHydroMetadata_Povodi_division("pod", "2", dt)

        DownloadHydroMetadata_PMO(dt)

        Return dt

    End Function

    ' special function: download metadata from PMO's hydro stations
    ' the PMO stations are being 
    Public Sub DownloadHydroMetadata_PMO(ByRef dt As DataTable)

        Dim url As String = "http://www.pmo.cz/portal/sap/cz/menu.htm"

        Dim WebC As WebClient = WebUtils.CreateWebClient()

        Dim Buf() As Byte
        Dim FileString As String
        Dim CurValueStr As String
        Dim CurNameStr As String
        Dim CurRiverStr As String
        Dim CurName2Str As String
        Dim division As String = "1"

        Dim r1 As New Regex( _
            "option\s*value=""(?<1>\d+)"">(?<2>[^\-]+)\-\s*(?<3>[^<]+)<",
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Try
            Buf = WebC.DownloadData(url)
            FileString = System.Text.Encoding.GetEncoding(1250).GetString(Buf)

            Dim m As Match
            Dim MatchCount As Integer = 0
            m = r1.Match(FileString)
            Dim stN As Integer = 0
            While m.Success
                MatchCount = MatchCount + 1
                CurValueStr = m.Value
                CurName2Str = m.Groups(1).Value
                CurRiverStr = m.Groups(2).Value
                CurNameStr = m.Groups(3).Value
                If CurNameStr.StartsWith("LG") Then
                    CurNameStr = CurNameStr.Substring(3)
                End If

                Dim qry As String = "name2='" & CurName2Str & "'"
                Dim foundRow() As DataRow = dt.Select(qry)
                If foundRow.Length = 0 Then
                    Dim r As DataRow = dt.NewRow
                    r("name") = CurNameStr
                    r("name2") = CurName2Str
                    r("river") = CurRiverStr
                    r("division") = division
                    r("operator") = 5
                    dt.Rows.Add(r)
                End If
                m = m.NextMatch()
            End While


        Catch ex As Exception
            FileString = ""
        End Try


    End Sub

    Public Sub DownloadHydroMetadata_Povodi_division(ByVal povodi As String, ByVal division As String, ByRef dt As DataTable)

        Dim baseUrl As String = ""
        Dim operatorId As Integer = 2
        Select Case povodi
            Case "pvl"
                baseUrl = "http://www.pvl.cz/portal/SaP/cz/pc/"
                operatorId = 2
            Case "poh"
                baseUrl = "http://sap.poh.cz/portal/SaP/cz/PC/"
                operatorId = 3
            Case "pla"
                baseUrl = "http://www.pla.cz/portal/sap/cz/PC/"
                operatorId = 4
            Case "pod"
                baseUrl = "http://app.pod.cz/portal/SaP/cz/PC/"
                operatorId = 6

        End Select

        Dim url As String = String.Format("{0}?oid={1}&data=1", baseUrl, division)

        Dim WebC As WebClient = WebUtils.CreateWebClient()

        Dim Buf() As Byte
        Dim FileString As String
        Dim CurValueStr As String
        Dim CurNameStr As String
        Dim CurRiverStr As String
        Dim CurName2Str As String


        Dim r1 As New Regex( _
            "Hodnoty(?<1>[A-Z|0-9]+)""\s*value=""(?<2>[^\|]+)\|(?<3>[^\|]+)\|",
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Try
            Buf = WebC.DownloadData(url)
            FileString = System.Text.Encoding.UTF8.GetString(Buf)

            Dim m As Match
            Dim MatchCount As Integer = 0
            m = r1.Match(FileString)
            Dim stN As Integer = 0
            While m.Success
                MatchCount = MatchCount + 1
                CurValueStr = m.Value
                CurName2Str = m.Groups(1).Value
                CurRiverStr = m.Groups(2).Value
                CurNameStr = m.Groups(3).Value
                If CurNameStr.StartsWith("LG") Then
                    CurNameStr = CurNameStr.Substring(3)
                End If


                Dim qry As String = "name2='" & CurName2Str & "'"
                Dim foundRow() As DataRow = dt.Select(qry)
                If foundRow.Length = 0 Then
                    Dim r As DataRow = dt.NewRow
                    r("name") = CurNameStr
                    r("name2") = CurName2Str
                    r("river") = CurRiverStr
                    r("division") = division
                    r("operator") = operatorId
                    dt.Rows.Add(r)
                End If
                m = m.NextMatch()
            End While


        Catch ex As Exception
            FileString = ""
        End Try

    End Sub

    'auxiliary function
    'CALLED BY: [UpdatePrecip()]
    'downloads and updates precipitation in all stations for a given half-day
    'PARAMETERS:
    'DayOffset: Specifies the day (number of days before today)
    'stationTable: a Hashtable with the stations\
    'CALLS:
    'methods of 'PrecipStation' class
    'RETURNS:
    'AddedRows: Number of rows added to the database table
    'LogStr: String containing exception messages
    Private Function DownloadPrecip(ByVal DayOffset As Integer, _
    ByRef stationTable As Hashtable, _
    ByRef AddedRows As Integer) As String

        'log message variables
        Dim LogStr As String = ""
        Dim LogStrException As String = vbCrLf & "Executing UpdateObsPrecip2(" _
        & DayOffset.ToString & ")"

        Dim StartTime As DateTime = DateTime.Now

        'variables for web file access
        Dim UriString As String
        Dim BaseUriString As String = "http://hydro.chmi.cz/hpps/hpps_act_rain.php?KRAJ=&UC_POV=&POB=&ordstr=nvys%20desc&recnum=50"
        Dim WebC As WebClient = WebUtils.CreateWebClient()

        Dim Buf() As Byte
        Dim FileString As String

        Dim r1_old As New Regex( _
            "<TD[^>]*><A[^\?]+\?[^;]+;seq=(?<1>\d+)[^>]*>[^<]+</A>\s*</TD>\s*" & _
            "<TD[^<]*</TD>\s*" & _
            "(?:<TD[^>]*>(?<2>[^<]*)</TD>\s*){7}" & _
            "<TD[^<]*</TD>\s*" & _
            "(?:<TD[^>]*>(?<2>[^<]*)</TD>\s*){17}", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Dim r1 As New Regex( _
            ";seq=(?<1>\d+)[^>]*>[^<]+</A>\s*</TD>\s*" & _
            "<TD[^<]*</TD>\s*" & _
            "(?:<TD[^>]*>(?<2>[^<]*)</TD>\s*){7}" & _
            "<TD[^<]*</TD>\s*" & _
            "(?:<TD[^>]*>(?<2>[^<]*)</TD>\s*){17}", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Dim r1_mapa As New Regex( _
            ";seq=(?<1>\d+)[^>]*>(?<2>[^<]+)</A>\s*" & _
            "<A[^>]+><img[^>]+></A>\s*</TD>\s*" & _
            "<TD[^<]*</TD>\s*" & _
            "(?:<TD[^>]*>(?<3>[^<]*)</TD>\s*){7}" & _
            "<TD[^<]*</TD>\s*" & _
            "(?:<TD[^>]*>(?<3>[^<]*)</TD>\s*){17}", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Dim r2 As New Regex("<TH[^>]*>(?<1>\d{1,2})\.(?<2>\d{1,2})\.(?<3>\d{4})</TH>\s*", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Dim m As Match
        Dim MatchCount As Integer = 0

        'observation data variables
        Dim curStation As PrecipStation
        Dim CurValueStr As String
        Dim CurSeqStr As String
        Dim ObsDate As DateTime
        Dim BeginTime As DateTime
        Dim EndTime As DateTime
        Dim ToUTC As Integer
        Dim ObsHour As Integer
        Dim CurStationSeq As Integer
        Dim ObsValueStr As String
        Dim CurObsValue As Single
        Dim ValuesCount As Integer
        Dim CurObsTime As DateTime

        Dim DbMissingValue As Integer = -1

        'download web file
        'here we need to download multiple files (paging)
        Dim recnum As Integer = 50
        For startPos = 0 To 350 Step recnum

            UriString = _
            BaseUriString & _
            String.Format("&startpos={0}&day_offset={1}", startPos, DayOffset)

            Try
                Buf = WebC.DownloadData(UriString)
                FileString = System.Text.Encoding.GetEncoding(1250).GetString(Buf)
            Catch ex As Exception
                FileString = ""
                LogStr &= LogStrException & ex.Message
            End Try

            'find date of observations
            m = r2.Match(FileString)
            If m.Success Then
                CurValueStr = m.Value
                ObsDate = New DateTime(Integer.Parse(m.Groups(3).Value), _
                Integer.Parse(m.Groups(2).Value), _
                Integer.Parse(m.Groups(1).Value))
                'decide BeginTime according to DayObd function parameter
                BeginTime = ObsDate

                ToUTC = -1

                BeginTime = BeginTime.AddHours(ToUTC)
                EndTime = BeginTime.AddHours(23)
                'changed from 24 to 23 (row contains starting column + 23 values)
            Else
                LogStr &= LogStrException & vbCrLf & " No match for ObsDate!"
            End If

            'read observations matching r1 (without map..)
            m = r1.Match(FileString)
            While m.Success
                MatchCount = MatchCount + 1
                CurValueStr = m.Value
                CurSeqStr = m.Groups(1).Value
                Try
                    CurStationSeq = CInt(CurSeqStr)
                    'Console.WriteLine(CurStationSeq.ToString)

                    If stationTable.ContainsKey(CurStationSeq) Then
                        curStation = CType(stationTable(CurStationSeq), PrecipStation)

                        ObsHour = 0
                        If Not curStation.HasObservations Then 'CHECK!!!! POSSIBLE ERROR......
                            'first, check the observation time (are observations in database?)
                            If BeginTime > curStation.MaxDbTime Then
                                ObsHour = 0
                                curStation.StartWebTime = BeginTime
                            ElseIf EndTime > curStation.MaxDbTime Then
                                CurObsTime = curStation.MaxDbTime.AddHours(1 - ToUTC)
                                ObsHour = CurObsTime.Hour 'CHECK!!!!!
                                '+2 because we add one hour and convert to central European time
                                curStation.StartWebTime = curStation.MaxDbTime.AddHours(1)
                            End If
                        End If

                        ValuesCount = m.Groups(2).Captures.Count

                        'observation list
                        Dim ObservationList As New List(Of TimeValuePair)

                        If curStation.HasObservations Then
                            While ObsHour < ValuesCount
                                CurObsValue = DbMissingValue

                                ObsValueStr = m.Groups(2).Captures(ObsHour).Value
                                If (ObsValueStr.Length > 0 And ObsValueStr <> "&nbsp;") Then
                                    CurObsValue = Me.String2Single(ObsValueStr)

                                    'update station data!
                                    curStation.AddObservation(CurObsValue)
                                Else
                                    curStation.AddMissingValue()
                                End If

                                ObsHour = ObsHour + 1
                            End While
                        End If
                    End If

                Catch ex As Exception
                    LogStr &= vbCrLf & ex.Message
                End Try

                m = m.NextMatch()
            End While


            'read observations matching r1_map (with map..)
            m = r1_mapa.Match(FileString)
            While m.Success
                MatchCount = MatchCount + 1
                CurValueStr = m.Value
                CurSeqStr = m.Groups(1).Value
                Try
                    CurStationSeq = CInt(CurSeqStr)
                    'Console.WriteLine(CurStationSeq.ToString)

                    If stationTable.ContainsKey(CurStationSeq) Then
                        curStation = CType(stationTable(CurStationSeq), PrecipStation)

                        ObsHour = 0
                        If Not curStation.HasObservations Then 'CHECK!!!! POSSIBLE ERROR......
                            'first, check the observation time (are observations in database?)
                            If BeginTime > curStation.MaxDbTime Then
                                ObsHour = 0
                                curStation.StartWebTime = BeginTime
                            ElseIf EndTime > curStation.MaxDbTime Then
                                CurObsTime = curStation.MaxDbTime.AddHours(1 - ToUTC)
                                ObsHour = CurObsTime.Hour 'CHECK!!!!!
                                '+2 because we add one hour and convert to central European time
                                curStation.StartWebTime = curStation.MaxDbTime.AddHours(1)
                            End If
                        End If

                        ValuesCount = m.Groups(3).Captures.Count

                        'observation list
                        Dim ObservationList As New List(Of TimeValuePair)

                        If curStation.HasObservations Then
                            While ObsHour < ValuesCount
                                CurObsValue = DbMissingValue

                                ObsValueStr = m.Groups(3).Captures(ObsHour).Value
                                If (ObsValueStr.Length > 0 And ObsValueStr <> "&nbsp;") Then
                                    CurObsValue = Me.String2Single(ObsValueStr)

                                    'update station data!
                                    curStation.AddObservation(CurObsValue)
                                Else
                                    curStation.AddMissingValue()
                                End If

                                ObsHour = ObsHour + 1
                            End While
                        End If
                    End If

                Catch ex As Exception
                    LogStr &= vbCrLf & ex.Message
                End Try

                m = m.NextMatch()
            End While

        Next startPos

        If MatchCount = 0 Then
            LogStr &= LogStrException & vbCrLf & " No match for ObsDate!"
        End If
        Return LogStr
    End Function


    '-----------------------------------------------------------------------------------------------
    '********************** WATER LEVEL AND FLOW: CHMU New simplified version!**********************
    '-----------------------------------------------------------------------------------------------
    'public method
    'DESCRIPTION:
    ' update CHMI hydrological observations
    ' do this for stations that are present in internet station overview and
    ' are present also in the database (where tok <> "").
    ' if reading of internet observation table for a given station fails,
    ' only the latest observation from the station overview is added to 
    ' database
    'CALLS:
    ' GetHydroStationTable(),CreateHydroCommand(),
    ' DownloadObsHydro(),ExecuteHydroCommand
    'RETURNS:
    ' LogStr (message about the job)

    Public Function UpdateHydro_CHMU() As String

        'log message variables
        Dim LogStr As String = "Executing UpdateHydro_CHMU.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        'variables for temporary station data storage
        Dim HydroDs As New DataSet
        Dim StTab As DataTable
        Dim StRow As DataRow

        'variables with information about the station
        Dim CurStationId As Integer
        Dim CurStationSeq As Integer
        Dim LastDBObsTime As DateTime
        Dim ScalingQ_avg As Integer = 100

        'sql database connection variables
        Dim sql_cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim stageCmd As SqlCommand = Me.CreateStageCommand(sql_cnn)
        Dim dischargeCmd As SqlCommand = Me.CreateDischargeCommand(sql_cnn)
        Dim UpdatedRows As Integer = 0
        Dim AddedRows As Integer = 0

        'observation data variables
        Dim ObsListHQ As New ArrayList
        Dim CurObsHQ As HydroTimeValueHQ
        Dim CurListIndex As Integer
        Dim binManager As New BinaryFileManager

        'create station table and retrieve station data
        'from the website ('prehled mereni') and database
        LogStr &= Me.GetHydroStationTable_chmu2(HydroDs)

        If Not HydroDs.Tables.Contains("StTab") Then Return LogStr
        StTab = HydroDs.Tables("StTab")

        'process each station - get necessary information
        For Each StRow In StTab.Rows

            Try

                LastDBObsTime = DateTime.Now.AddDays(-6)
                If Not TypeOf (StRow("st_id")) Is DBNull Then
                    CurStationId = CType(StRow("st_id"), Integer)
                    CurStationSeq = CType(StRow("st_seq"), Integer)
                    'If Not TypeOf (StRow("latest_obs")) Is DBNull Then
                    'LastDBObsTime = CType(StRow("latest_obs"), DateTime)
                    'End If

                    'download observations from the Internet!
                    ObsListHQ.Clear()
                    LogStr = LogStr & Me.DownloadHydro_CHMU(CurStationSeq, LastDBObsTime, ObsListHQ)

                    Dim ObsListH As New List(Of TimeValuePair)
                    Dim ObsListQ As New List(Of TimeValuePair)

                    For CurListIndex = ObsListHQ.Count - 1 To 0 Step -1

                        CurObsHQ = CType(ObsListHQ(CurListIndex), HydroTimeValueHQ)
                        ObsListH.Add(New TimeValuePair(CurObsHQ.Time, CurObsHQ.H))
                        ObsListQ.Add(New TimeValuePair(CurObsHQ.Time, CurObsHQ.Q))

                    Next CurListIndex

                    'get site directory and code
                    Dim fileDir As String = ConfigurationManager.AppSettings("files_dir1")
                    Dim siteCode As String = CurStationId.ToString("D4")

                    'now update the values: stage
                    Dim binaryFileH As String = "h" & "_" & "vodstav" & "_" & siteCode & ".dat"
                    Dim fileNameH As String = fileDir & "\h\vodstav\" & binaryFileH

                    'now update the values: discharge
                    Dim binaryFileQ As String = "h" & "_" & "prutok" & "_" & siteCode & ".dat"
                    Dim fileNameQ As String = fileDir & "\h\prutok\" & binaryFileQ

                    AddedRows += binManager.AddValues(fileNameH, ObsListH, "h")
                    AddedRows += binManager.AddValues(fileNameQ, ObsListQ, "h")
                End If

            Catch ex As Exception
                LogStr &= ex.Message
            End Try

        Next StRow

        'return result log of the function
        ComputationTime = DateTime.Now.Subtract(StartTime)
        Return LogStr & vbCrLf & "Rows updated: " & UpdatedRows & vbCrLf & _
        "Rows added: " & AddedRows & vbCrLf & _
        "Time taken: " & ComputationTime.ToString
    End Function


    'auxiliary function
    'called by [UpdateHydro()]
    'downloads observation data for a given station from the Internet
    'and stores the stage Qand discharge observations in ArrayList
    'ObsListHQ
    'only observations later than LatestDbObs
    'are added to list
    'PrevDays specifies the time span of downloaded observations
    'Function returns a message in case of error
    Private Function DownloadHydro_CHMU(ByVal st_seq As Integer, _
    ByVal LatestDbObs As DateTime, _
    ByVal ObslistHQ As ArrayList) As String

        'variables for web file manipulation
        Dim UriStringBase As String = "http://hydro.chmi.cz/hpps/"
        Dim UriString As String
        Dim WebC As WebClient = WebUtils.CreateWebClient()
        Dim Buf() As Byte
        Dim FileString As String

        'regular expression variables
        Dim r As Regex
        Dim m As Match
        Dim c As Capture
        Dim CurTimeStr As String
        Dim CurValueStr As String

        'observation data variables
        Dim CurTime As DateTime = DateTime.Now.AddHours(-1)
        Dim ObsYear As Integer = CurTime.Year
        Dim CurMonth As Integer = CurTime.Month

        Dim Obs As HydroTimeValueHQ
        Dim ObsTime As DateTime

        'temporary observation lists
        Dim LogStr As String = String.Empty

        'initialize array lists
        ObslistHQ.Clear()

        'open www file
        Try
            UriString = UriStringBase & String.Format _
            ("hpps_prfdata.php?seq={0}", st_seq)

            'read html file and save it to a string
            Buf = WebC.DownloadData(UriString)
            FileString = System.Text.Encoding.GetEncoding(1250).GetString(Buf)
        Catch ex As Exception
            LogStr = String.Format("{0} DownloadObsHydro: unable to download {1} ({2})", _
            vbCrLf, UriString, ex.Message)
            'exits the sub and returns empty observation lists
            'and returns an error message

            'only write the very last obs.value to database!

            Return LogStr
        End Try

        r = New Regex("<TR[^>]*>\s*" & _
        "(?:<TD[^>]*>(?<1>[^<]*)</TD>\s*){3}", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        m = r.Match(FileString)
        'go through the webpage
        'for each row in the table of observations, add the row
        'data to a temporary list (starting with latest observation)
        While m.Success
            CurValueStr = m.Value
            CurTimeStr = m.Groups(1).Captures(0).Value
            If Me.IsValidDateStringDMYH(CurTimeStr) Then
                Obs.Time = Me.String2DateDMY(CurTimeStr)

                'convert Obs.Time to UT
                Obs.Time = Obs.Time.AddHours(-1)

                'set values of current observation to zero
                Obs.H = 0
                Obs.Q = 0
                'if the observation is already present in database
                '(indicated by LatestObsTime), stop adding data to both lists
                If DateTime.Compare(Obs.Time, LatestDbObs) <= 0 Then Exit While

                'if the observation is unrounded time, continue
                If (Obs.Time.Minute = 0) Then

                    'Add ALL data newer than LatestDbObs to a temporary list!
                    CurValueStr = m.Groups(1).Captures(1).Value
                    If CurValueStr.Length > 0 Then
                        If Char.IsDigit(CurValueStr.Chars(0)) Then
                            Obs.H = Me.String2Single(CurValueStr)
                        End If
                    End If

                    CurValueStr = m.Groups(1).Captures(2).Value
                    If CurValueStr.Length > 0 Then
                        If Char.IsDigit(CurValueStr.Chars(0)) Then
                            Obs.Q = Me.String2Single(CurValueStr)
                        End If
                    End If
                    ObslistHQ.Add(Obs)

                End If
            End If

            m = m.NextMatch()
        End While

        If ObslistHQ.Count = 0 Then
            LogStr = String.Format("DownloadObsHydro: zero observations read ({0})", UriString)
        End If

        Return LogStr
    End Function


    '-----------------------------------------------------------------------------------------------
    '********************** WATER LEVEL AND FLOW: POVODI (www.voda.gov.cz)    **********************
    '-----------------------------------------------------------------------------------------------
    'public method
    Public Function UpdateHydro_povodi() As String

        'log message variables
        Dim LogStr As String = "Executing UpdateHydro_povodi .."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan
        Dim UpdatedRows As Integer = 0
        Dim AddedRows As Integer = 0

        'observation data variables
        Dim ObsListHQ As New List(Of HydroTimeValueHQ)
        Dim ObsCount As Integer = 0
        Dim CurObsHQ As HydroTimeValueHQ
        Dim CurListIndex As Integer

        Dim Name2 As String = "NAME"
        Dim Id As Integer = 0
        Dim DivisionName As String = ""
        Dim OperatorID As Integer = 0
        Dim LatestObs As DateTime = DateTime.Now.AddDays(-6)

        'SQL database variables
        Dim HydroDs As New DataSet
        Dim StTab As DataTable
        Dim StRow As DataRow

        Dim BinManager As New BinaryFileManager

        'get table of limnigraphic stations to update
        LogStr &= Me.GetHydroStationTable_povodi(HydroDs)
        If Not HydroDs.Tables.Contains("StTab") Then Return "Station table ST_TAB not found"
        StTab = HydroDs.Tables("StTab")
        For Each StRow In StTab.Rows
            LatestObs = DateTime.Now.AddDays(-6)
            If Not TypeOf (StRow("st_id")) Is DBNull Then Id = CType(StRow("st_id"), Integer)
            If Not TypeOf (StRow("st_name2")) Is DBNull Then Name2 = CType(StRow("st_name2"), String)
            'If Not TypeOf (StRow("latest_obs")) Is DBNull Then LatestObs = CType(StRow("latest_obs"), DateTime)
            DivisionName = ""
            If Not TypeOf (StRow("division_name")) Is DBNull Then DivisionName = CType(StRow("division_name"), String)
            If Not TypeOf (StRow("operator_id")) Is DBNull Then OperatorID = CType(StRow("operator_id"), Integer)

            Try
                ObsCount = Me.DownloadHydro_povodi(Id, Name2, ObsListHQ, DivisionName, OperatorID)

                Dim ObsListH As New List(Of TimeValuePair)
                Dim ObsListQ As New List(Of TimeValuePair)

                For CurListIndex = 0 To ObsListHQ.Count - 1
                    CurObsHQ = CType(ObsListHQ(CurListIndex), HydroTimeValueHQ)
                    ObsListH.Add(New TimeValuePair(CurObsHQ.Time, CurObsHQ.H))
                    ObsListQ.Add(New TimeValuePair(CurObsHQ.Time, CurObsHQ.Q))
                Next CurListIndex

                'get site directory and code
                Dim fileDir As String = ConfigurationManager.AppSettings("files_dir1")
                Dim CurStationId As Integer = Id
                Dim siteCode As String = CurStationId.ToString("D4")

                'now update the values: stage
                Dim binaryFileH As String = "h" & "_" & "vodstav" & "_" & siteCode & ".dat"
                Dim fileNameH As String = fileDir & "\h\vodstav\" & binaryFileH

                'now update the values: discharge
                Dim binaryFileQ As String = "h" & "_" & "prutok" & "_" & siteCode & ".dat"
                Dim fileNameQ As String = fileDir & "\h\prutok\" & binaryFileQ

                AddedRows += binManager.AddValues(fileNameH, ObsListH, "h")
                AddedRows += binManager.AddValues(fileNameQ, ObsListQ, "h")

            Catch ex As Exception
                LogStr &= vbCrLf & "unknown error, station (" & ex.Message & ")" & Id
            End Try

        Next

        'return result log of the function
        ComputationTime = DateTime.Now.Subtract(StartTime)
        Return LogStr & vbCrLf & "updated: " & UpdatedRows.ToString & vbCrLf & _
        "added: " & AddedRows.ToString & vbCrLf & _
        "Time taken: " & ComputationTime.ToString

    End Function

    ' auxiliary function: creates a table of hydrologic stations for Povodi which are currently
    ' measuring.
    ' PARAMETERS: HydroDs, the DataSet where table is created (empty DataSet)
    ' CALLED BY:  UpdateHydro_povodi()
    Private Function GetHydroStationTable_povodi(ByVal HydroDs As DataSet) As String

        'variables for internet data download
        Dim LogStr As String = ""

        'variables for sql database access and data retrieval
        'data for connection to SQL server
        Dim sql_cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim sql_cmd As New SqlCommand
        Dim sql_da As New SqlDataAdapter(sql_cmd)
        Dim StationCount As Integer = 0
        'add new table to the dataset if necessary
        If Not HydroDs.Tables.Contains("StTab") Then _
        HydroDs.Tables.Add("StTab")

        'set sql command parameters
        sql_cmd.Connection = sql_cnn
        sql_cmd.CommandType = CommandType.StoredProcedure
        sql_cmd.CommandText = "plaveninycz.new_query_stationshydro_povodi"
        'get the table
        Try
            sql_da.Fill(HydroDs.Tables("StTab"))
        Catch ex As Exception
            LogStr &= vbCrLf & ex.Message
        End Try
        StationCount = HydroDs.Tables("StTab").Rows.Count
        Return LogStr
    End Function


    Private Function GetHydroStationTable_chmu2(ByVal HydroDs As DataSet) As String

        'variables for internet data download
        Dim LogStr As String = ""

        'variables for sql database access and data retrieval
        'data for connection to SQL server
        Dim sql_cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim sql_cmd As New SqlCommand
        Dim sql_da As New SqlDataAdapter(sql_cmd)
        Dim StationCount As Integer = 0
        'add new table to the dataset if necessary
        If Not HydroDs.Tables.Contains("StTab") Then _
        HydroDs.Tables.Add("StTab")

        'set sql command parameters
        sql_cmd.Connection = sql_cnn
        sql_cmd.CommandType = CommandType.StoredProcedure
        sql_cmd.CommandText = "plaveninycz.new_query_stationshydro_chmi"
        'get the table
        Try
            sql_da.Fill(HydroDs.Tables("StTab"))
        Catch ex As Exception
            LogStr &= vbCrLf & ex.Message
        End Try
        StationCount = HydroDs.Tables("StTab").Rows.Count
        Return LogStr
    End Function


    'auxiliary function
    'called by [UpdateHydro_povodi()]
    'downloads observation data for a given station from the Internet
    'and stores the stage and discharge observations in ArrayLists
    'ObsListH, ObsListQ
    'LatestObs specifies the date, which is already in the database and is not 
    'added to ObsListH or ObsListQ
    'Function returns the number of records added to both lists
    Private Function DownloadHydro_povodi(ByVal st_id As Integer, _
    ByVal st_name2 As String, ByVal ObsListHQ As List(Of HydroTimeValueHQ),
    ByVal DivisionName As String, ByVal OperatorID As Integer) As Integer

        'clear the list
        ObsListHQ.Clear()

        'variables for web file manipulation
        Dim Povodi As String = ""
        Select Case OperatorID
            Case 2
                Povodi = "pvl"
            Case 3
                Povodi = "poh"
            Case 4
                Povodi = "pla"
            Case 5
                Povodi = "pmo"
            Case 6
                Povodi = "pod"
        End Select

        Dim UriString As String
        Dim WebC As WebClient = WebUtils.CreateWebClient()
        Dim Buf() As Byte
        Dim FileString As String

        'regular expression variables
        Dim r3, r4 As Regex
        Dim m1, m2 As Match
        Dim CurTimeStr As String
        Dim CurValueStrH As String
        Dim CurValueStrQ As String

        'observation data variables
        Dim CurTime As DateTime = DateTime.Now.AddHours(-1)
        Dim ObsYear As Integer = CurTime.Year
        Dim CurMonth As Integer = CurTime.Month
       
        Dim ObsTime As DateTime

        'observation value
        Dim ObsValueH As Single
        Dim ObsValueQ As Single

        Dim ObsCount As Integer = 0
        Dim VariableId As Integer = 4

        Dim nH As Integer = 0
        Dim nQ As Integer = 0

        Dim iaQ As Boolean = True
        Dim iaH As Boolean = True
        Dim ListLength As Integer = 0
        Dim ObsListH As New ArrayList(32)
        Dim ObsListQ As New ArrayList(32)
        Dim ObsListHQ1 As New List(Of HydroTimeValueHQ)
        Dim ObsHQ As HydroTimeValueHQ
        Dim ext_i As Integer = 0
        Dim last_ext_i As Integer = 0

        Dim LogStr As String

        'open www file
        Try
            'Get the Uri
            Select Case Povodi
                Case "pla"
                    UriString = "http://www.pla.cz/portal/sap/cz/" & String.Format("PC/Mereni.aspx?id={0}&oid={1}", st_name2, DivisionName)
                Case "pvl"
                    UriString = "http://www.pvl.cz/SaP/cz/" & String.Format("PC/Mereni.aspx?id={0}&oid={1}", st_name2, DivisionName)
                Case "poh"
                    UriString = "http://sap.poh.cz/portal/SaP/cz/" & String.Format("PC/Mereni.aspx?id={0}&oid={1}", st_name2, DivisionName)
                Case "pod"
                    UriString = "http://app.pod.cz/portal/SaP/cz/" & String.Format("PC/Mereni.aspx?id={0}&oid={1}", st_name2, DivisionName)
                Case "pmo"
                    UriString = "http://www.pmo.cz/portal/sap/en/" & "mereni_" & st_name2.Substring(4).ToUpper() & ".htm"
                Case Else
                    UriString = "http://www." & Povodi & ".cz/portal/sap/en/" & "mereni_" & st_name2.Substring(4).ToUpper() & ".htm"
            End Select

            'read html file and save it to a string
            Buf = WebC.DownloadData(UriString)
            FileString = System.Text.Encoding.GetEncoding(1250).GetString(Buf)
        Catch ex As Exception
            LogStr = ex.Message
            'exits the sub and returns empty observation lists
            'and returns ObsCount = 0
            Return ObsCount
        End Try

        r3 = New Regex("(?<1>\d+)\.(?<2>\d+)\.(?<3>\d+)\D+(?<4>\d+)\D(?<5>\d+)", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        If Povodi = "pmo" Then
            r4 = New Regex("<tr>\s*" & _
                           "<td align=right nowrap><font[^>]*>(?<1>[^<]+)</font></td>\s*" & _
                           "<td[^>]*><font[^>]*>(?<2>[^<]+)</font></td>\s*" & _
                           "<td[^>]*><font[^>]*>(?<3>[^<]*)</font></td>\s*")
        Else
            r4 = New Regex("<td class=""bunkaGridu""[^>]*>\s*" & _
                       "<span>(?<1>[^<]+)</span>\s*</td>\s*" & _
            "<td class=""bunkaGriduBold""[^>]*>\s*<*s*p*a*n*>*\s*(?<2>[^<]+)<*/*s*p*a*n*>*\s*</td>\s*" & _
            "<td class=""bunkaGriduBold""[^>]*>\s*<*s*p*a*n*>*\s*(?<3>[^<]*)<*/*s*p*a*n*>*\s*</td>",
            RegexOptions.IgnoreCase Or RegexOptions.Compiled)
        End If

        m1 = r4.Match(FileString)
        While m1.Success

            'newly programmed by Jiri
            CurTimeStr = m1.Groups(1).Value
            CurValueStrH = m1.Groups(2).Value
            CurValueStrQ = m1.Groups(3).Value

            'find out the time
            m2 = r3.Match(CurTimeStr)
            If m2.Success Then
                Dim day As Integer = CInt(m2.Groups(1).Value)
                Dim mon As Integer = CInt(m2.Groups(2).Value)
                Dim yr As Integer = CInt(m2.Groups(3).Value)

                If yr < 2000 Then
                    yr = yr + 2000
                End If

                Dim hr As Integer = CInt(m2.Groups(4).Value)
                Dim min As Integer = CInt(m2.Groups(5).Value)
                ObsTime = New DateTime(yr, mon, day, hr, min, 0)
            End If

            'convert Obs.Time to UT
            ObsTime = ObsTime.AddHours(-1)

            'find out the stage (H)
            If CurValueStrH.Contains("&") Then
                ObsValueH = String2Single(CurValueStrH.Substring(0, CurValueStrH.IndexOf("&")))
            Else
                ObsValueH = String2Single(CurValueStrH.Trim())
            End If


            'find out the discharge (Q)
            'sometimes discharge is unknown..
            Try
                If CurValueStrQ.Contains("&") Then
                    ObsValueQ = String2Single(CurValueStrQ.Substring(0, CurValueStrQ.IndexOf("&")))
                Else
                    ObsValueQ = String2Single(CurValueStrQ.Trim())
                End If

            Catch
                ObsValueQ = 0.0
            End Try


            'create the observation (OBS) object
            ObsHQ.Time = ObsTime
            ObsHQ.H = ObsValueH
            ObsHQ.Q = ObsValueQ

            If ObsHQ.Time.Minute = 0 Then
                ObsListHQ1.Add(ObsHQ)
            End If

            'next match
            m1 = m1.NextMatch()
        End While

        'reverse the observations list
        ObsListHQ.Clear()
        For i As Integer = ObsListHQ1.Count - 1 To 0 Step -1
            ObsListHQ.Add(ObsListHQ1(i))
        Next

        Return ObsListHQ.Count
    End Function


    Public Function UpdatePrecipitation_Daily() As String
        Dim NumUpdatedRows As Integer = 0
        Dim NumAddedRows As Integer = 0
        Dim logstr As String = ""
        UpdatePrecipitation_Daily_CHMI(logstr, NumUpdatedRows, NumAddedRows)
        Return logstr
    End Function

    '-----------------------------------------------------------------------------------'
    '----------- SNOW: CHMU ------------------------------------------------------------'
    '-----------------------------------------------------------------------------------'
    'function for updating: snow
    'snow from chmi (text) and chmi snow map (map, text)
    'snow from povodi Ohre
    Public Function UpdateSnow() As String

        'log message variables
        Dim LogStr As String = "Executing UpdateSnow.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        'variables for creating snow station table
        Dim cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim SnowStationCmd1 As SqlCommand = Me.CreateStationIdCommand(cnn)
        Dim SnowStationCmd2 As SqlCommand = Me.CreateSynopStationCommand(cnn)
        Dim SnowCmd As SqlCommand = Me.CreateSnowCommand(cnn)
        Dim ZeroStationList As New ArrayList(20)

        'variables for reading internet data files
        Dim SnowUriChmi01 As String = "http://portal.chmi.cz/files/portal/docs/meteo/om/inform/p_scr.html"
        Dim SnowUriChmi02 As String = "http://pr-asv.chmi.cz/synopy-map/pocasiin.php?ukazatel=snih&pozadi=mapareg&graf=ano"
        Dim SnowUriChmi03 As String = Me.ViewConfiguration("SnowUri03")
        Dim buf() As Byte
        Dim WebC As WebClient = WebUtils.CreateWebClient()
        Dim FileString As String

        'regular expression variables
        Dim r1 As New Regex("bgColor\s*=\s*[""]?antiquewhite[""]?\s*>\s*" & _
        "<TD\s+align\s*=\s*[""]?left[""]?>(?<1>[^<]+)\s*" & _
        "<TD\s+align\s*=\s*[""]?right[""]?>\s*(?<2>\d+)\s*", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Dim r2 As New Regex("výška sněhové pokrývky\s*(?<1>\d{1,2})\.(?<2>\d{1,2})\.(?<3>\d{4})", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'variables for reading files from the web
        Dim m As Match
        Dim MatchCount As Integer = 0
        Dim CurValueStr As String
        Dim CurNameStr As String
        Dim CurSnowStr As String
        Dim DateStr As String

        'observation variables
        Dim ObsTime As DateTime
        Dim ObsValue As Integer
        Dim NoDataValue As Integer = -9999
        Dim CurStationId As Integer
        Dim NumUpdatedRows As Integer = 0
        Dim NumAddedRows As Integer = 0
        Dim RunOhre As Boolean = False

        '********************************************************************************
        '(1) UPDATE POVODI OHRE
        'read povodi ohre web file
        '********************************************************************************
        If (RunOhre And (DateTime.Now.Month > 10 Or DateTime.Now.Month < 5)) Then
            Try
                buf = WebC.DownloadData(SnowUriChmi03)
                FileString = System.Text.Encoding.GetEncoding(1250).GetString(buf)
            Catch ex As Exception
                FileString = ""
                LogStr &= vbCrLf & "Ohre:" & ex.Message
            End Try

            'parse FileString using regular expressions
            'find date of observations
            m = r2.Match(FileString)
            If m.Success Then
                CurValueStr = m.Value
                ObsTime = New DateTime(Integer.Parse(m.Groups(3).Value), _
                Integer.Parse(m.Groups(2).Value), _
                Integer.Parse(m.Groups(1).Value), 6, 0, 0)
            Else
                LogStr &= vbCrLf & "Ohre: No Regex match for ObsTime!"
            End If
            m = r1.Match(FileString)
            While m.Success
                MatchCount = MatchCount + 1
                CurValueStr = m.Value
                CurNameStr = m.Groups(1).Value.Trim()
                CurSnowStr = m.Groups(2).Value.Trim()
                If IsNumeric(CurSnowStr) Then
                    ObsValue = Integer.Parse(CurSnowStr)
                Else
                    ObsValue = 0
                End If
                'find station id in the database
                CurStationId = Me.GetStationId(cnn, SnowStationCmd1, "st_name2", CurNameStr)
                'update observation
                If CurStationId > 0 Then
                    LogStr &= Me.ExecuteSnowCommand(SnowCmd, CurStationId, ObsTime, ObsValue, NumUpdatedRows, NumAddedRows)
                End If
                m = m.NextMatch()
            End While
            'write error message if there was no match
            If MatchCount = 0 Then
                LogStr &= vbCrLf & "Ohre: No regex match for snow!"
            End If
        End If

        '*********************************************************************
        '(3) UPDATE SNOW OBSERVATIONS FROM CHMI SNOW MAP (lowland stations)
        '*********************************************************************
        UpdateSnow_Chmi_Synop(SnowUriChmi02, LogStr, NumUpdatedRows, NumAddedRows)

        '*********************************************************************
        '(2) UPDATE CHMI HILL STATIONS (snow table)
        '*********************************************************************
        If DateTime.Now.Month > 10 Or DateTime.Now.Month < 5 Then
            'regex for reading snow data
            r1 = New Regex("<tr [^>]+>\s*<td>\s*(?<1>[^<]+)</td>\s*<td>\s*[^<]+</td>\s*<td>[^<]*</td>\s*<td>\s*(?<2>[^<]+)</td>", _
            RegexOptions.IgnoreCase Or RegexOptions.Compiled)

            r2 = New Regex("Snih v CR a okoli dne: (?<1>\d{2})\.(?<2>\d{2})", _
            RegexOptions.IgnoreCase Or RegexOptions.Compiled)

            'read chmi snow web file
            Try
                buf = WebC.DownloadData(SnowUriChmi01)
                FileString = System.Text.Encoding.GetEncoding(1250).GetString(buf)
            Catch ex As Exception
                FileString = ""
                LogStr &= vbCrLf & "CHMI hills: " & ex.Message
            End Try

            'get observation date
            m = r2.Match(FileString)
            If m.Success Then
                CurValueStr = m.Value

                ObsTime = New DateTime(DateTime.Now.Year, _
                Integer.Parse(m.Groups(2).Value), _
                Integer.Parse(m.Groups(1).Value), 6, 0, 0)
            Else
                LogStr &= vbCrLf & "CHMI hills: No regex match for ObsTime!"
            End If

            MatchCount = 0
            m = r1.Match(FileString)
            While m.Success
                ObsValue = 0

                MatchCount = MatchCount + 1
                CurValueStr = m.Value
                CurNameStr = m.Groups(1).Value.Trim()
                CurSnowStr = m.Groups(2).Value.Trim()
                CurSnowStr = CurSnowStr.ToLower()
                If IsNumeric(CurSnowStr) Then
                    ObsValue = Integer.Parse(CurSnowStr)
                Else
                    If CurSnowStr.IndexOf("pop") >= 0 Then
                        ObsValue = -2
                    ElseIf CurSnowStr.IndexOf("nes") >= 0 Then
                        ObsValue = -1
                    Else
                        ObsValue = NoDataValue
                    End If
                End If
                'find station id in the database
                CurStationId = Me.GetStationId(cnn, SnowStationCmd1, "st_name2", CurNameStr)
                'update observation
                If CurStationId > 0 And ObsValue <> NoDataValue Then
                    LogStr &= Me.ExecuteSnowCommand(SnowCmd, CurStationId, ObsTime, ObsValue, NumUpdatedRows, NumAddedRows)
                End If
                m = m.NextMatch()
            End While
            'write error message if there was no match
            If MatchCount = 0 Then
                LogStr &= vbCrLf & "CHMI hills: No regex match for snow!"
            End If
        End If
        'return result log of the function
        ComputationTime = DateTime.Now.Subtract(StartTime)
        Return LogStr & vbCrLf & _
        "rows updated: " & NumUpdatedRows & vbCrLf & _
        "rows added: " & NumAddedRows & vbCrLf & _
        "Time taken: " & ComputationTime.ToString
    End Function


    'auxiliary function: view station id, when given an attribute
    'INPUT: Attrib: the table field, can have values:
    '       st_name2, st_name, st_seq, location_id, st_ind, st_uri
    '       Value: string value of the attribute
    'RETURNS: st_id if successful, -1 if no such station exists
    'CALLED BY: UpdateSnow()
    'CALLS:     nothing
    Private Function GetStationId _
    (ByVal cnn As SqlConnection, ByVal cmd As SqlCommand, ByVal AttribName As String, _
    ByVal AttribValue As String) As Integer
        Dim Result As Integer = -1
        Dim ParameterName As String
        ParameterName = "@" + AttribName

        If AttribName = "st_seq" Or AttribName = "location_id" Or AttribName = "st_ind" Then
            cmd.Parameters(ParameterName).Value = Integer.Parse(AttribValue)
        Else
            cmd.Parameters(ParameterName).Value = AttribValue
        End If

        Try
            cnn.Open()
            Result = CType(cmd.ExecuteScalar(), Integer)
            cnn.Close()
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
            Result = -1
        End Try
        Return Result
    End Function

    'auxiliary function: create sql command to get the station id, given st_name2
    'RETURNS: SqlCommand object, used by function GetStationId()
    'CALLED BY: UpdateObsSnow2(), UpdatePrecip_povodi()
    'CALLS:     nothing
    Private Function CreateStationIdCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Connection = cnn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "plaveninycz.query_stationid"
        cmd.Parameters.Add(New SqlParameter("@st_name2", SqlDbType.VarChar))
        cmd.Parameters.Add(New SqlParameter("@st_name", SqlDbType.VarChar))
        cmd.Parameters.Add(New SqlParameter("@st_uri", SqlDbType.VarChar))
        cmd.Parameters.Add(New SqlParameter("@st_seq", SqlDbType.Int))
        cmd.Parameters.Add(New SqlParameter("@location_id", SqlDbType.Int))
        cmd.Parameters.Add(New SqlParameter("@st_ind", SqlDbType.SmallInt))

        Return cmd
    End Function

    'auxiliary function: view station id, when given the st name for 
    'stations that are chmi climate stations
    'INPUT: stName: the name of the station
    '       st_name2, st_name, st_seq, location_id, st_ind, st_uri
    '       Value: string value of the attribute
    'RETURNS: st_id if successful, -1 if no such station exists
    'CALLED BY: UpdateSynop_24h()
    'CALLS:     nothing
    Private Function GetStationIdForSnow _
    (ByVal cnn As SqlConnection, ByVal cmd As SqlCommand, ByVal stName As String) As Integer
        Dim Result As Integer = -1

        cmd.Parameters("@st_name").Value = stName

        Try
            cnn.Open()
            Result = CType(cmd.ExecuteScalar(), Integer)
            cnn.Close()
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
            Result = -1
        End Try
        Return Result
    End Function

    'note: this applies to all stations from CHMI's synopy-map program
    Private Function CreateStationIdForSnowCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Connection = cnn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "plaveninycz.query_stationid_snow"
        cmd.Parameters.Add(New SqlParameter("@st_name", SqlDbType.VarChar))
        Return cmd
    End Function

    'auxiliary function: view synoptic station id, when given st_coord_topleft
    'RETURNS: st_id if successful, -1 if no such station exists
    'CALLED BY: UpdateSynop_24h()
    'PARAMETERS:
    ' Coord ....... the top-left screen station coordinate (integer from 100000 to 999999)
    ' CoordType ... the type of coordinates (25 for snow and precipitation, 26 for evaporation and soil moisture)
    'CALLS:     nothing
    Private Function GetStationIdByCoord _
    (ByVal cnn As SqlConnection, ByVal cmd As SqlCommand, ByVal Coord As Integer, ByVal CoordType As Integer) As Integer
        Dim Result As Integer = -1
        cmd.Parameters("@st_coord_topleft").Value = Coord
        cmd.Parameters("@coord_type").Value = CoordType
        Try
            cnn.Open()
            Result = CType(cmd.ExecuteScalar(), Integer)
            cnn.Close()
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
            Result = -1
        End Try
        Return Result
    End Function

    'auxiliary function: create sql command to get the synop station with a given Coord
    'RETURNS: SqlCommand object
    'CALLED BY: UpdateSynop_24h()
    'CALLS:     nothing
    Private Function CreateSynopStationCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Connection = cnn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "plaveninycz.query_stationbycoord"
        cmd.Parameters.Add(New SqlParameter("@st_coord_topleft", SqlDbType.Int))
        cmd.Parameters.Add(New SqlParameter("@coord_type", SqlDbType.TinyInt))
        Return cmd
    End Function


    'auxiliary function: Finds the list of stations with hourly precipitation
    'the operatorCode should be the name2 (PVL, PLA, POH, PMO, POD, ...)
    Private Function GetPrecipStationTable_Hourly(ByVal operatorCode As String) As List(Of PrecipStation)
        'private variables
        Dim LogStr As String = ""

        Dim stList As New List(Of PrecipStation)

        'variables for sql database access and data retrieval
        'data for connection to SQL server
        Dim sql_cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim cmd As New SqlCommand
        Dim r As SqlDataReader
        Dim curStationId As Integer
        Dim curStationName As String
        Dim curStationCode As String
        Dim curStationSeq As Integer
        Dim curOperatorId As Integer
        Dim curMeteoCode As String
        Dim MaxDbTime As DateTime

        Dim operatorId As Integer = GetOperatorId(operatorCode)

        'access to the database!
        'set sql command parameters
        cmd.Connection = sql_cnn
        cmd.CommandText = _
            "SELECT st.st_id, st.st_seq, st.st_name, st.st_name2, st.meteo_code, st.operator_id, st.division_name " & _
    "FROM plaveninycz.stationsvariables stvar " & _
    "INNER JOIN plaveninycz.stations st ON stvar.st_id = st.st_id " & _
    "WHERE st.operator_id = @p1 AND stvar.var_id = 1 " & _
    "ORDER BY st.st_id "

        cmd.Parameters.Add(New SqlParameter("p1", operatorId))

        Try
            sql_cnn.Open()
            r = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While r.Read()
                curStationId = CType(r("st_id"), Integer)
                curStationSeq = CType(r("st_seq"), Integer)
                curStationName = CType(r("st_name"), String)

                curOperatorId = CType(r("operator_id"), Integer)

                curStationCode = "unknown"
                If Not TypeOf (r("st_name2")) Is DBNull Then
                    curStationCode = CType(r("st_name2"), String)
                End If

                curMeteoCode = "unknown"
                If Not TypeOf (r("meteo_code")) Is DBNull Then
                    curMeteoCode = CType(r("meteo_code"), String)
                End If

                MaxDbTime = DateTime.Now.Date.AddDays(-8)

                Dim curDivisionName As String = "unknown"
                If Not TypeOf (r("division_name")) Is DBNull Then
                    curDivisionName = CType(r("division_name"), String)
                End If
                Dim st As New PrecipStation(curStationId, curStationSeq, _
                    curStationName, curMeteoCode, curOperatorId, curStationCode, MaxDbTime)
                st.DivisionName = curDivisionName
                stList.Add(st)
            End While

        Catch ex As Exception
            LogStr &= vbCrLf & ex.Message
        End Try
        Return stList
    End Function

    'auxiliary function: gets the OperatorID based on the Operator short name
    Private Function GetOperatorId(ByVal name2 As String) As Integer
        'get the operator ID
        Dim cmd As New SqlCommand
        Dim operatorId As Integer = 0
        cmd.CommandText = String.Format("SELECT id FROM plaveninycz.operator WHERE name2 ='{0}'", name2)
        cmd.Connection = New SqlConnection(Me.SqlDbConnectionString)
        Try
            cmd.Connection.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader(CommandBehavior.SingleResult)
            If dr.HasRows Then
                dr.Read()
                operatorId = CInt(dr.Item(0))
            End If
        Catch ex As Exception
        Finally
            cmd.Connection.Close()
        End Try
        Return operatorId
    End Function

    'auxiliary function: read data from internet with cookie
    Private Function GetWebString(ByVal url As String) As String
        'Read Html file from Internet
        Dim fileString As String
        Dim req As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
        req.Timeout = 10000
        req.Method = "GET"
        req.KeepAlive = True
        req.AllowAutoRedirect = True
        Dim cookieContainer As CookieContainer = New CookieContainer()
        req.CookieContainer = cookieContainer

        Try
            Dim res As WebResponse = req.GetResponse
            Dim dataStream As Stream = res.GetResponseStream
            Dim reader As New StreamReader(dataStream)
            fileString = reader.ReadToEnd()
            reader.Close()
            res.Close()
        Catch ex As Exception
            Return "error"
        End Try
        Return fileString
    End Function

    Public Function AccessDvt() As CookieContainer
        Dim url As String = "http://dvt-info.cz/web_plzen/dvtsite_public/SiteChartTable.aspx?site=S_209"
        Dim urlRef As String = "http://dvt-info.cz/web_plzen/dvtsite_public/OsmOverviewMap.aspx"
        Dim req As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
        req.Timeout = 10000
        req.Method = "GET"
        req.KeepAlive = True
        req.AllowAutoRedirect = True
        req.Referer = urlRef
        Dim cookieContainer As CookieContainer = New CookieContainer()
        req.CookieContainer = cookieContainer
        Try
            Dim res As WebResponse = req.GetResponse
            Dim dataStream As Stream = res.GetResponseStream
            Dim reader As New StreamReader(dataStream)
            Dim fileString As String = reader.ReadToEnd()
            reader.Close()
            res.Close()
        Catch ex As Exception

        End Try

        Dim req2 As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
        req2.Timeout = 10000
        req2.Method = "GET"
        req2.KeepAlive = True
        req2.AllowAutoRedirect = True
        req2.Referer = urlRef
        req2.CookieContainer = cookieContainer
        Try
            Dim res2 As WebResponse = req2.GetResponse
            Dim dataStream2 As Stream = res2.GetResponseStream
            Dim reader As New StreamReader(dataStream2)
            Dim fileString As String = reader.ReadToEnd()
            reader.Close()
            res2.Close()
        Catch ex As Exception

            Dim cookies As CookieCollection = req.CookieContainer.GetCookies(New Uri(url))
            Return cookieContainer
        End Try
        Return cookieContainer
    End Function

    Private Function GetDvtCookies(ByVal lvsName As String) As CookieContainer
        Dim defaultSiteCode As String = "S_209"
        Dim url As String = String.Format("http://dvt-info.cz/web_{0}/dvtsite_public/SiteChartTable.aspx?site={1}", _
                                          lvsName, defaultSiteCode)
        Dim urlRef As String = String.Format("http://dvt-info.cz/web_{0}/dvtsite_public/OsmOverviewMap.aspx", lvsName)
        Dim req As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
        req.Timeout = 10000
        req.Method = "GET"
        req.KeepAlive = True
        req.AllowAutoRedirect = True
        req.Referer = urlRef
        Dim cookieContainer As CookieContainer = New CookieContainer()
        req.CookieContainer = cookieContainer
        Try
            Dim res As WebResponse = req.GetResponse
            Dim dataStream As Stream = res.GetResponseStream
            Dim reader As New StreamReader(dataStream)
            Dim fileString As String = reader.ReadToEnd()
            reader.Close()
            res.Close()
        Catch ex As Exception

        End Try
        Return cookieContainer
    End Function

    'auxiliary function: read data from internet with cookie
    'it is necessary to pass the valid cookie container (ASP.NET_SessionId) if we want this to work
    Private Function GetDvtPage(ByVal url As String, ByVal lvsName As String, ByVal cookies As CookieContainer) As String
        'Read Html file from Internet
        Dim fileString As String
        Dim req As HttpWebRequest = CType(WebRequest.Create(url), HttpWebRequest)
        req.Timeout = 10000
        req.Method = "GET"
        req.KeepAlive = True
        req.AllowAutoRedirect = True
        req.CookieContainer = cookies
        req.Referer = String.Format("http://dvt-info.cz/web_{0}/dvtsite_public/OsmOverviewMap.aspx", lvsName)

        Try
            Dim res As WebResponse = req.GetResponse
            Dim dataStream As Stream = res.GetResponseStream
            Dim reader As New StreamReader(dataStream)
            fileString = reader.ReadToEnd()
            reader.Close()
            res.Close()
        Catch ex As Exception
            Return "error"
        End Try
        Return fileString
    End Function

    'auxiliary function: saves the stations from LVS to the Stations table and StationsVariables table
    'set lvsName to plzen
    Public Function SaveStations_LVS(ByVal lvsName As String) As String
        Dim LogStr As String = ""
        Dim FileString As String
        Dim CurLatStr As String
        Dim CurLonStr As String
        Dim CurStationStr As String
        Dim CurStationLat As Double
        Dim CurStationLon As Double
        Dim CurStationCode As String
        Dim CurStationName As String
        Dim CurStationUrl As String
        Dim operatorId As Integer = 0
        Dim stHash As New Hashtable

        operatorId = GetOperatorId(lvsName)
        If operatorId = 0 Then Return "error: cannot find operator ID"

        Dim StationUri As String = String.Format("http://dvt-info.cz/web_{0}/dvtsite_public/OsmOverviewMap.aspx", lvsName)
        'StationUri = "http://localhost/test/OsmOverviewMap.aspx.htm" 'TEMPORARY URI FOR DEV

        'open lvs page
        FileString = GetWebString(StationUri)
        If FileString = "error" Then
            LogStr = vbCrLf & "executing SaveStations_LVS: error accessing url " & StationUri
            Return LogStr
        End If

        'parse lvs page
        'Parse the file, with the use of Regex
        Dim r As New Regex("addPointMarker\((?<1>[\d\.]+),\s*(?<2>[\d\.]+),\s*'[^']+',\s*'(?<3>S_[\d]+)'", _
                RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Dim m As Match = r.Match(FileString)

        While m.Success
            CurStationStr = m.Value
            CurLonStr = m.Groups(1).Value
            If IsNumeric(CurLonStr) Then CurStationLon = Double.Parse(CurLonStr)
            CurLatStr = m.Groups(2).Value
            If IsNumeric(CurLatStr) Then CurStationLat = Double.Parse(CurLatStr)
            CurStationCode = m.Groups(3).Value
            Dim pSt As New PrecipStation(0, 0, "", operatorId, CurStationCode, DateTime.MinValue)
            pSt.Lat = CurStationLat
            pSt.Lon = CurStationLon
            stHash.Add(CurStationCode, pSt)
            m = m.NextMatch()
        End While

        'parse station name
        Dim r2 As New Regex("<td[^>]+>\s*<a title=\""(?<1>[^\""]+)""[^\?]+\?site=(?<2>S_[\d]+)", _
                RegexOptions.IgnoreCase Or RegexOptions.Compiled)
        Dim m2 = r2.Match(FileString)
        While m2.Success
            CurStationStr = m2.Value
            CurStationName = m2.Groups(1).Value
            CurStationCode = m2.Groups(2).Value
            CurStationName = CurStationName.Substring(0, CurStationName.IndexOf(CurStationCode) - 1)
            Dim st As PrecipStation = CType(stHash.Item(CurStationCode), PrecipStation)
            st.Name = CurStationName
            CurStationUrl = NameToUrl(CurStationName)
            st.Url = CurStationUrl
            m2 = m2.NextMatch()
        End While

        'save it to the DB
        Dim sql_cnn As New SqlConnection(Me.SqlDbConnectionString)
        For Each st As PrecipStation In stHash.Values
            'sync st with db
            st.SynchronizeFromDB(sql_cnn, st.Code)
            If (st.Id = 0) Then
                'get elevation
                Dim elev As Double = GetElevation(st.Lat, st.Lon)
                st.Elevation = elev
                st.AddStationToDB(sql_cnn)
            End If
        Next

        Return LogStr
    End Function


    '-----------------------------------------------------------------------------------------------
    '********************** TEMPERATURE (AIR) ******************************************************
    '-----------------------------------------------------------------------------------------------
    'public method
    'DESCRIPTION:
    ' downloads and updates hourly temperature in CHMI, PVL, POH, PLA, PMO, POD, IN-POCASI stations
    Public Function UpdateTemperature() As String
        'log message variables
        Dim LogStr As String = "Executing UpdateTemperature.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        Dim NumAddedRows As Integer = 0
        Dim NumUpdatedRows As Integer = 0
        Dim CurStationSeq As Integer = 0
        Dim CurStationUrl As String

        Dim chmi_base_url = "http://portal.chmi.cz/files/portal/docs/poboc/PR/grafy/"
        Dim inpocasi_base_url = Me.ViewConfiguration("InPocasiUri")

        '(1) get the table of temperature stations
        Dim stationList As List(Of TemperatureStation) = GetTemperatureStationTable()

        '(2) for each station in the table: download page and update values
        For Each st As TemperatureStation In stationList

            If st.OperatorId = 1 Then
                CurStationUrl = chmi_base_url & st.DivisionName & "/" & st.MeteoCode & ".PNG"
                UpdateTemperature_chmi_station(CurStationUrl, st, NumAddedRows)
            ElseIf st.OperatorId = 8 Then
                'in-pocasi.eu
                CurStationUrl = inpocasi_base_url & st.Name2
                Dim curDate As DateTime = DateTime.Now.Date.AddDays(-1)
                UpdateTemperature_inpocasi_station(CurStationUrl, curDate, st, NumAddedRows)
            Else
                CurStationUrl = GetStationUri_Povodi(st.MeteoCode, st.DivisionName, st.OperatorId)
                UpdateTemperature_povodi_station(CurStationUrl, st, NumAddedRows)
            End If
        Next

        'return result log of the function
        ComputationTime = DateTime.Now.Subtract(StartTime)
        Return LogStr & vbCrLf & _
        "rows updated: " & NumUpdatedRows & vbCrLf & _
        "rows added: " & NumAddedRows & vbCrLf & _
        "Time taken: " & ComputationTime.ToString
    End Function

    'gets the table of all temperature stations (chmu and also povodi's)
    Private Function GetTemperatureStationTable() As List(Of TemperatureStation)
        'private variables
        Dim LogStr As String = ""
        Dim stationList As New List(Of TemperatureStation)

        'variables for sql database access and data retrieval
        'data for connection to SQL server
        Dim sql_cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim sql_cmd As New SqlCommand
        Dim sql_reader As SqlDataReader
        Dim curStationId As Integer
        Dim curOperatorId As Integer
        Dim curStationName As String
        Dim curStationSeq As Integer
        Dim MaxDbTime As DateTime
        Dim DivisionName As String
        Dim MeteoCode As String
        Dim Name2 As String

        'access to the database!
        'set sql command parameters
        sql_cmd.Connection = sql_cnn
        sql_cmd.CommandType = CommandType.StoredProcedure
        sql_cmd.CommandText = "plaveninycz.new_query_temperaturestations" 'todo move to local qry

        stationList.Clear()
        Try
            sql_cnn.Open()
            sql_reader = sql_cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sql_reader.Read()
                curStationId = CType(sql_reader("st_id"), Integer)
                curOperatorId = CType(sql_reader("operator_id"), Integer)
                curStationSeq = CType(sql_reader("st_seq"), Integer)
                curStationName = CType(sql_reader("st_name"), String)

                If Not TypeOf (sql_reader("division_name")) Is DBNull Then
                    DivisionName = sql_reader("division_name").ToString()
                Else
                    DivisionName = "unknown"
                End If
                If Not TypeOf (sql_reader("meteo_code")) Is DBNull Then
                    MeteoCode = sql_reader("meteo_code").ToString()
                    If MeteoCode.StartsWith("pvl_") Then
                        MeteoCode = (MeteoCode.Substring(4)).ToUpper()
                    End If
                Else
                    MeteoCode = "unknown"
                End If
                If Not TypeOf (sql_reader("st_name2")) Is DBNull Then
                    Name2 = sql_reader("st_name2").ToString()
                Else
                    Name2 = "unknown"
                End If

                stationList.Add( _
                    New TemperatureStation(curStationId, curOperatorId, curStationSeq, _
                    curStationName, DivisionName, MeteoCode, Name2))
            End While

        Catch ex As Exception
            LogStr &= vbCrLf & ex.Message
        Finally
            sql_cnn.Close()
        End Try
        Return stationList
    End Function

    'auxiliary function: removes the diacritic from the station name
    Private Function NameToUrl(ByVal stName As String) As String
        Dim stName2 As String = stName.ToLower()
        stName2 = stName2.Replace("á", "a")
        stName2 = stName2.Replace("é", "e")
        stName2 = stName2.Replace("í", "i")
        stName2 = stName2.Replace("ó", "o")
        stName2 = stName2.Replace("ú", "u")
        stName2 = stName2.Replace("ů", "u")
        stName2 = stName2.Replace("ý", "y")
        stName2 = stName2.Replace("č", "c")
        stName2 = stName2.Replace("ě", "e")
        stName2 = stName2.Replace("ď", "d")
        stName2 = stName2.Replace("ň", "n")
        stName2 = stName2.Replace("š", "s")
        stName2 = stName2.Replace("ř", "r")
        stName2 = stName2.Replace("ž", "z")
        stName2 = stName2.Replace("ť", "t")
        stName2 = stName2.Replace(" ", "_")
        Return stName2
    End Function


    'updates HOURLY precipitation for a station from LVS !!!
    Private Function UpdatePrecip_LVS_Station(ByVal stationUrl As String, _
    ByVal datum As DateTime, ByVal st As PrecipStation, ByVal lvsName As String, _
    ByVal dvtCookies As CookieContainer, ByRef AddedRows As Integer) As String

        Dim LogStr As String = ""

        'declare local variables
        Dim Observations As New List(Of HydroTimeValue)
        Dim ListOfTimes As New List(Of DateTime)
        Dim ObsHours As Integer = 0
        Dim MaxObsHours As Integer = 0
        Dim AdditionalHour As Integer = 0

        Dim DbMissingValue As Single = 999.9

        'resolve the URL of povodi
        Dim webC As WebClient = WebUtils.CreateWebClient()
        Dim fileString As String

        Dim r As New Regex( _
        "<td>(?<1>[\d]{2}\.[\d]{2}\.[\d]{4})\s{1}(?<2>[\d]{2}\:[\d]{2}\:[\d]{2})\s*</td>\s*<td>(?<3>[\d\,\.]+)</td>\s*</tr>\s*",
         RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'download the file from web
        fileString = GetDvtPage(stationUrl, lvsName, dvtCookies)
        If fileString = "error" Then
            LogStr &= vbCrLf & String.Format("Unable to download file for station {0}", stationUrl)
            Return LogStr
        End If

        'regex matching
        Dim MatchCount As Integer = 0
        Dim CurStr As String
        Dim m1 As Match
        Dim CurValue As Single = -1
        Dim CurTime As DateTime
        Dim i As Integer = 0

        'setup list of observations
        Observations.Clear()
        ListOfTimes.Clear()

        m1 = r.Match(fileString)
        While (m1.Success)
            MatchCount = MatchCount + 1
            CurStr = m1.Groups(3).Value
            If CurStr.Length > 0 Then
                CurValue = Me.String2Single(CurStr)

                Dim timeStr As String = m1.Groups(2).Value
                Dim dateStr As String = m1.Groups(1).Value
                Dim day As Integer = CInt(dateStr.Substring(0, 2))
                Dim mon As Integer = CInt(dateStr.Substring(3, 2))
                Dim year As Integer = CInt(dateStr.Substring(6, 4))
                Dim hour As Integer = CInt(timeStr.Substring(0, 2))
                Dim minute As Integer = CInt(timeStr.Substring(3, 2))

                CurTime = New DateTime(year, mon, day, hour, minute, 0)

                If CurTime > st.MaxDbTime And (Not ListOfTimes.Contains(CurTime)) Then
                    Dim val As HydroTimeValue
                    val.Time = CurTime
                    val.Value = CurValue
                    Observations.Add(val)
                    ListOfTimes.Add(CurTime)
                End If

            End If
            m1 = m1.NextMatch()
        End While

        'write error message if there was no match
        If MatchCount = 0 Then
            LogStr &= vbCrLf & stationUrl & ": No regex match!"
        End If

        'process the observations - add them to the station!
        If Observations.Count > 0 Then
            st.StartWebTime = Observations(Observations.Count - 1).Time


            'add observations to the station object
            For i = Observations.Count - 1 To 0 Step -1
                st.AddObservation(Observations(i).Value)
            Next
            'write observations to database
            Dim sql_cnn As New SqlConnection(SqlDbConnectionString)
            st.UpdateDB(sql_cnn, AddedRows)
            sql_cnn.Close()
            sql_cnn = Nothing
        End If
        Return LogStr
    End Function


    'updates HOURLY temperature for a station of in-pocasi !!!
    Public Function UpdateTemperature_inpocasi_station(ByVal stationUrl As String, _
    ByVal datum As DateTime, ByVal st As TemperatureStation, ByRef AddedRows As Integer) As String

        Dim LogStr As String = ""

        'declare local variables
        Dim Observations As New List(Of HydroTimeValue)
        Dim ObsHours As Integer = 0
        Dim MaxObsHours As Integer = 0
        Dim AdditionalHour As Integer = 0

        Dim DbMissingValue As Single = 999.9

        'resolve the URL of povodi
        Dim webC As WebClient = WebUtils.CreateWebClient()
        Dim buf() As Byte
        Dim fileString As String

        Dim r2 As New Regex( _
        "hlavast[^>]+>\s*<td>\s*<b>(?<1>[^<]+)</b>\s*</td>\s*" & _
        "<td>\s*<b>(?<2>[\d.,-]+)[^<]+</b>",
         RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'download the file from web
        Dim Url As String = stationUrl & "&historie=" & datum.Month.ToString("00") & "-" & datum.Day.ToString("00") & "-" & datum.Year.ToString()
        Try
            buf = webC.DownloadData(Url)
            'fileString = New UTF8Encoding().GetString(buf)
            fileString = System.Text.Encoding.GetEncoding(1250).GetString(buf)
        Catch ex As Exception
            LogStr &= vbCrLf & String.Format("Unable to download file for station {0}", Url)
            Return LogStr
        End Try

        'regex matching
        Dim MatchCount As Integer = 0
        Dim CurStr As String
        Dim m1 As Match
        Dim CurValue As Single = -1
        Dim CurTime As DateTime
        Dim i As Integer = 0

        'setup list of observations
        Observations.Clear()

        m1 = r2.Match(fileString)
        While (m1.Success)
            MatchCount = MatchCount + 1
            CurStr = m1.Groups(2).Value
            If CurStr.Length > 0 Then
                CurValue = Me.String2Single(CurStr)

                Dim timeStr As String = m1.Groups(1).Value
                Dim day As Integer = datum.Day
                Dim mon As Integer = datum.Month
                Dim year As Integer = datum.Year
                Dim hour As Integer = CInt(timeStr.Substring(0, 2))
                Dim minute As Integer = CInt(timeStr.Substring(3, 2))

                CurTime = New DateTime(year, mon, day, hour, minute, 0)


                Dim val As HydroTimeValue
                val.Time = CurTime
                val.Value = CurValue
                Observations.Add(val)


            End If
            m1 = m1.NextMatch()
        End While

        'write error message if there was no match
        If MatchCount = 0 Then
            LogStr &= vbCrLf & Url & ": No regex match!"
        End If

        st.StartWebTime = datum
        For hr As Integer = 0 To 23
            Dim dt As DateTime = datum.AddHours(hr)
            Dim nearestVal As HydroTimeValue = FindNearestValue(Observations, dt)
            If nearestVal.Value > -999 Then
                st.AddObservation(nearestVal.Value)
            Else
                st.AddMissingValue()
            End If

        Next

        'process the observations - add them to the station!
        If st.NumObservations > 0 Then
            'write observations to database
            Dim sql_cnn As New SqlConnection(SqlDbConnectionString)
            st.UpdateDB(sql_cnn, AddedRows)
            sql_cnn.Close()
            sql_cnn = Nothing
        End If
        Return LogStr
    End Function

    Private Function FindNearestValue(ByVal obsList As List(Of HydroTimeValue), ByVal searchTime As DateTime) As HydroTimeValue
        Dim pDiff As Double = 24.0
        Dim cDiff As Double = 24.0
        Dim nearestObs As HydroTimeValue = Nothing
        For Each v As HydroTimeValue In obsList
            cDiff = Math.Abs((v.Time - searchTime).TotalHours)
            If (cDiff < pDiff) Then
                pDiff = cDiff
                nearestObs = v
            Else
                Exit For
            End If
        Next
        'check the dif!
        Dim result As New HydroTimeValue()
        result.Time = searchTime
        If pDiff > 0.6 Then
            result.Value = -999.9
        Else
            result.Value = nearestObs.Value
        End If
        Return result
    End Function

    'updates HOURLY temperature for a station of POVODI !!!
    Private Function UpdateTemperature_povodi_station(ByVal Url As String, _
    ByVal st As TemperatureStation, ByRef AddedRows As Integer) As String

        Dim LogStr As String = ""

        'declare local variables
        Dim Observations As New List(Of TimeValuePair)
        Dim ObsHours As Integer = 0
        Dim MaxObsHours As Integer = 0
        Dim AdditionalHour As Integer = 0

        Dim DbMissingValue As Single = 999.9

        'resolve the URL of povodi
        Dim webC As WebClient = WebUtils.CreateWebClient()
        Dim buf() As Byte
        Dim fileString As String

        'TODO change the regex expression
        Dim r1 As New Regex( _
        "<tr>\s*<td[^>]*>\s*<font[^>]+>(?<1>[\d\.\:\s]+)[^<]*</font></td>\s*" & _
        "<td[^>]*><font[^>]+>(?<2>[\d\.,]*)[^<]*</font></td>\s*" & _
        "<td[^>]*><font[^>]+>(?<3>[\d\.,-]*)[^<]*</font></td>\s*" & _
        "</tr>", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Dim rPVL As New Regex( _
            "<td class\s*=\s*\""bunkaGridu\"">(?<1>[^<]*)</td>\s*" & _
            "<td[^>]*>\s*<span[^>]+>(?<2>[\d\.,]*)\s*</span>\s*</td>" & _
            "<td[^>]*>\s*<span[^>]+>(?<3>[\d\.,-]*)\s*</span>\s*</td>\s*" & _
            "</tr>", _
            RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'download the file from web
        Try
            buf = webC.DownloadData(Url)
            fileString = System.Text.Encoding.GetEncoding(1250).GetString(buf)
        Catch ex As Exception
            LogStr &= vbCrLf & String.Format("Unable to download file for station {0}", Url)
            Return LogStr
        End Try

        'regex matching
        Dim MatchCount As Integer = 0
        Dim CurStr As String
        Dim m1 As Match
        Dim CurValue As Single = -1
        Dim CurTime As DateTime
        Dim MaxDbTime As DateTime
        Dim i As Integer = 0

        'setup list of observations
        Observations.Clear()

        'set the times
        MaxObsHours = 23

        'PMO uses old page layout, others use the new one
        If st.OperatorId = 5 Then
            m1 = r1.Match(fileString)
        Else
            m1 = rPVL.Match(fileString)
        End If


        While (m1.Success)
            MatchCount = MatchCount + 1
            CurStr = m1.Groups(3).Value
            If CurStr.Length > 0 Then
                CurValue = Me.String2Single(CurStr)

                Dim timeStr As String = m1.Groups(1).Value
                Dim day As Integer = CInt(timeStr.Substring(0, 2))
                Dim mon As Integer = CInt(timeStr.Substring(3, 2))
                Dim year As Integer
                Dim hour As Integer
                Dim minute As Integer

                If st.OperatorId = 5 Then
                    year = 2000 + CInt(timeStr.Substring(6, 2))
                    hour = CInt(timeStr.Substring(9, 2))
                    minute = CInt(timeStr.Substring(12, 2))
                Else
                    year = CInt(timeStr.Substring(6, 4))
                    hour = CInt(timeStr.Substring(11, 2))
                    minute = CInt(timeStr.Substring(14, 2))
                End If

                CurTime = New DateTime(year, mon, day, hour, minute, 0)

                Dim val As New TimeValuePair(CurTime, CurValue)
                Observations.Add(val)


            End If
            m1 = m1.NextMatch()
        End While

        'write error message if there was no match
        If MatchCount = 0 Then
            LogStr &= vbCrLf & Url & ": No regex match!"
        End If

        Dim observationList As New List(Of TimeValuePair)
        'add observations to the observation list in reverse
        For i = Observations.Count - 1 To 0 Step -1
            Dim obs As TimeValuePair = Observations(i)
            observationList.Add(New TimeValuePair(obs.DateTime, obs.Value))
        Next
        'process the observations - add them to the station!
        'write them to the binary file
        If observationList.Count > 0 Then

            Dim fileDir As String = ConfigurationManager.AppSettings("files_dir1")
            Dim siteCode As String = st.Id.ToString("D4")
            Dim binaryFile As String = "h" & "_" & "teplota" & "_" & siteCode & ".dat"

            Dim fileName As String = fileDir & "\h\teplota\" & binaryFile
            Dim bfm As New BinaryFileManager()
            bfm.AddValues(fileName, observationList, "h")

        End If

        Return LogStr
    End Function

    'updates HOURLY temperature for a station of CHMI !!!
    Private Function UpdateTemperature_chmi_station(ByVal Url As String, _
    ByVal st As TemperatureStation, ByRef AddedRows As Integer) As String

        Dim LogStr As String = ""

        Dim observationList As New List(Of TimeValuePair)
        Try
            observationList = GraphReader.ReadGraph(Url)
        Catch ex As Exception
            LogStr &= vbCrLf & Url & ": error extracting values!"
            Return LogStr
        End Try
        If observationList Is Nothing Then
            LogStr &= vbCrLf & Url & ": no values found in graph!"
            Return LogStr
        End If
        If observationList.Count = 0 Then
            LogStr &= vbCrLf & Url & ": no values found in graph!"
            Return LogStr
        End If


        'declare local variables
        Dim ObsHours As Integer = 0
        Dim MaxObsHours As Integer = 0
        Dim AdditionalHour As Integer = 0

        Dim DbMissingValue As Single = 999.9

        'process the observations - add them to the station!
        'write them to the binary file
        If observationList.Count > 0 Then

            Dim fileDir As String = ConfigurationManager.AppSettings("files_dir1")
            Dim siteCode As String = st.Id.ToString("D4")
            Dim binaryFile As String = "h" & "_" & "teplota" & "_" & siteCode & ".dat"

            Dim fileName As String = fileDir & "\h\teplota\" & binaryFile
            Dim bfm As New BinaryFileManager()
            bfm.AddValues(fileName, observationList, "h")

        End If
        Return LogStr
    End Function


    'pomocna funkce: vytvoreni objektu command pro ziskani posl. pozorovaneho 
    'vodniho stavu
    Private Function CreateObsByTimeCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.Connection = cnn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "plaveninycz.query_observationbytime"
        cmd.Parameters.Add(New SqlParameter("@station_id", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@variable_id", SqlDbType.TinyInt))
        cmd.Parameters.Add(New SqlParameter("@obs_time", SqlDbType.SmallDateTime))
        cmd.Parameters.Add(New SqlParameter("@obs_value", SqlDbType.SmallInt))
        cmd.Parameters("@obs_value").Direction = ParameterDirection.Output
        Return cmd
    End Function

    'pomocna funkce: vraci hodnotu pozorovani pro danou stanici, promennou a cas
    Private Function ViewObsByTime(ByVal cmd As SqlCommand, ByVal st_id As Integer, _
    ByVal var_id As Integer, ByVal obs_time As DateTime) As Integer
        Dim cnn As SqlConnection = cmd.Connection
        Dim obs_value As Integer = -9999
        cmd.Parameters("@station_id").Value = st_id
        cmd.Parameters("@variable_id").Value = var_id
        cmd.Parameters("@obs_time").Value = obs_time
        Try
            cnn.Open()
            cmd.ExecuteNonQuery()
            obs_value = CType(cmd.Parameters("@obs_value").Value, Integer)
            cnn.Close()

        Catch ex As Exception
        Finally
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
        End Try
        Return obs_value
    End Function


    'pomocna funkce: vytvoreni objektu command pro ziskani posledniho rad. mereni
    Private Function CreateLastRadarFileCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "plaveninycz.query_latestradarfile"
        cmd.Parameters.Add(New SqlParameter("@radarnet_id", SqlDbType.TinyInt))
        cmd.Parameters.Add(New SqlParameter("@radarnet_name", SqlDbType.VarChar))
        cmd.Connection = cnn
        Return cmd
    End Function

    'pomocna funkce: ziskani posledniho rad. mereni
    'pokud mereni neexistuje, vrati datum 2 dny pred nynejsi hodinou

    Private Function ViewLastRadarFile(ByVal radarnet_id As Integer) As DateTime
        Dim cmd As SqlCommand
        Dim cnn As New SqlConnection(SqlDbConnectionString)
        Dim CommandResult As Object

        'posledni zaznamenane radarove mereni v databazi
        'vychozi hodnota: cas pred 2 dny
        Dim TimeBefore2Days As DateTime = DateTime.Now.AddDays(-5)
        Dim LastRadarObsTime As New DateTime(TimeBefore2Days.Year, _
        TimeBefore2Days.Month, TimeBefore2Days.Day, TimeBefore2Days.Hour, 0, 0)

        'zjisteni posledniho radaroveho mereni
        cmd = CreateLastRadarFileCommand(cnn)
        cmd.Parameters("@radarnet_id").Value = radarnet_id
        Try
            cnn.Open()
            CommandResult = cmd.ExecuteScalar
            cnn.Close()
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
        End Try

        If Not CommandResult Is Nothing Then _
        LastRadarObsTime = CDate(CommandResult)

        If LastRadarObsTime < TimeBefore2Days Then
            LastRadarObsTime = TimeBefore2Days.Date
        End If

        Return LastRadarObsTime
    End Function

    'pomocna funkce: vytvoreni objektu command pro ulozeni radaroveho mereni
    Private Function CreateRadarFileCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "plaveninycz.update_radarfile"
        cmd.Parameters.Add(New SqlParameter("@radarnet_id", SqlDbType.TinyInt))
        cmd.Parameters.Add(New SqlParameter("@radarnet_name", SqlDbType.VarChar))
        cmd.Parameters.Add(New SqlParameter("@obs_time", SqlDbType.SmallDateTime))
        cmd.Connection = cnn
        Return cmd
    End Function

    'pomocna funkce: ziskani hodnot parametru z tabulky [configurations]
    Private Function ViewConfiguration(ByVal ConfigName As String) As String
        Dim cmd As New SqlCommand
        Dim cnn As New SqlConnection(SqlDbConnectionString)
        Dim CommandResult As Object
        Dim ConfigValue As String

        cmd.CommandText = _
        "SELECT config_value FROM configuration WHERE config_name = '" & ConfigName & "'"
        cmd.Connection = cnn
        Try
            cnn.Open()
            CommandResult = cmd.ExecuteScalar
            cnn.Close()
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
            Return "BadValue"
        End Try
        If Not CommandResult Is Nothing Then
            Return CStr(CommandResult)
        Else
            Return "BadValue"
        End If
    End Function

    'pomocna funkce: vytvoreni objektu command pro aktualizaci tabulky observations2
    Private Function CreateObservationCommand2(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "plaveninycz.update_observation2"
        cmd.Parameters.Add(New SqlParameter("@station_id", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@variable_name", SqlDbType.VarChar))
        cmd.Parameters.Add(New SqlParameter("@obs_time", SqlDbType.SmallDateTime))
        cmd.Parameters.Add(New SqlParameter("@obs_value", SqlDbType.Int))
        cmd.Parameters.Add(New SqlParameter("@status", SqlDbType.TinyInt))
        cmd.Parameters("@status").Direction = ParameterDirection.Output
        cmd.Connection = cnn
        Return cmd
    End Function

    Private Function CreatePrecipitationDailyCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "plaveninycz.new_update_rain_daily"
        cmd.Parameters.Add(New SqlParameter("@station_id", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@obs_time", SqlDbType.SmallDateTime))
        cmd.Parameters.Add(New SqlParameter("@rain", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@status", SqlDbType.TinyInt))
        cmd.Parameters("@status").Direction = ParameterDirection.Output
        cmd.Connection = cnn
        Return cmd
    End Function

    Private Function CreateSnowCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "plaveninycz.new_update_snow"
        cmd.Parameters.Add(New SqlParameter("@station_id", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@obs_time", SqlDbType.SmallDateTime))
        cmd.Parameters.Add(New SqlParameter("@snow", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@status", SqlDbType.TinyInt))
        cmd.Parameters("@status").Direction = ParameterDirection.Output
        cmd.Connection = cnn
        Return cmd
    End Function

    Private Function CreateStageCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "plaveninycz.new_update_stage"
        cmd.Parameters.Add(New SqlParameter("@station_id", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@obs_time", SqlDbType.SmallDateTime))
        cmd.Parameters.Add(New SqlParameter("@stage", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@status", SqlDbType.TinyInt))
        cmd.Parameters("@status").Direction = ParameterDirection.Output
        cmd.Connection = cnn
        Return cmd
    End Function

    Private Function CreateLogCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "plaveninycz.new_update_log"
        cmd.Parameters.Add(New SqlParameter("@log_time", SqlDbType.SmallDateTime))
        cmd.Parameters.Add(New SqlParameter("@log_text", SqlDbType.Text))
        cmd.Connection = cnn
        Return cmd
    End Function

    Private Function CreateDischargeCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "plaveninycz.new_update_discharge"
        cmd.Parameters.Add(New SqlParameter("@station_id", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@obs_time", SqlDbType.SmallDateTime))
        cmd.Parameters.Add(New SqlParameter("@discharge", SqlDbType.Real))
        cmd.Parameters.Add(New SqlParameter("@status", SqlDbType.TinyInt))
        cmd.Parameters("@status").Direction = ParameterDirection.Output
        cmd.Connection = cnn
        Return cmd
    End Function


    Private Function ExecuteDischargeCommand _
        (ByVal cmd As SqlCommand, ByVal st_id As Integer, ByVal obs_time As DateTime, _
    ByVal discharge_cms As Double,
    ByRef NumUpdatedRows As Integer, ByRef NumAddedRows As Integer) As String
        Dim LogStr As String = ""
        Dim cnn As SqlConnection = cmd.Connection
        'Dim log2_discharge As Integer

        cmd.Parameters("@station_id").Value = st_id
        cmd.Parameters("@obs_time").Value = obs_time
        If discharge_cms > 0 Then
            'log2_discharge = CInt(1000 * Math.Log(discharge_cms, 2))
            cmd.Parameters("@discharge").Value = discharge_cms
        Else
            Return ""
        End If

        Try
            cnn.Open()
            cmd.ExecuteNonQuery()
            If CInt(cmd.Parameters("@status").Value) = 1 Then
                NumUpdatedRows += 1
            Else
                NumAddedRows += 1
            End If
            cnn.Close()
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
            LogStr = vbCrLf & "Running ExecuteDischargeCommand,st_id=" & _
            st_id.ToString & " " & ex.Message
        End Try
        Return LogStr
    End Function

    Private Function ExecuteStageCommand _
        (ByVal cmd As SqlCommand, ByVal st_id As Integer, ByVal obs_time As DateTime, _
    ByVal stage_cm As Double,
    ByRef NumUpdatedRows As Integer, ByRef NumAddedRows As Integer) As String
        'all stages are multiplied by 10 (result is in mm!!)
        Dim ScalingH As Integer = 10
        Dim LogStr As String = ""
        Dim cnn As SqlConnection = cmd.Connection

        cmd.Parameters("@station_id").Value = st_id
        cmd.Parameters("@obs_time").Value = obs_time
        If stage_cm > 0 Then
            cmd.Parameters("@stage").Value = CInt(Math.Round(stage_cm * ScalingH))
        Else
            Return ""
        End If

        Try
            cnn.Open()
            cmd.ExecuteNonQuery()
            If CInt(cmd.Parameters("@status").Value) = 1 Then
                NumUpdatedRows += 1
            Else
                NumAddedRows += 1
            End If
            cnn.Close()
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
            LogStr = vbCrLf & "Running ExecuteHydroCommand,st_id=" & _
            st_id.ToString & " " & ex.Message
        End Try
        Return LogStr
    End Function

    Private Function ExecuteSnowCommand _
        (ByVal cmd As SqlCommand, ByVal st_id As Integer, ByVal obs_time As DateTime, _
    ByVal snow_cm As Integer,
    ByRef NumUpdatedRows As Integer, ByRef NumAddedRows As Integer) As String

        Dim LogStr As String = ""
        Dim cnn As SqlConnection = cmd.Connection

        cmd.Parameters("@station_id").Value = st_id
        cmd.Parameters("@obs_time").Value = obs_time
        cmd.Parameters("@snow").Value = snow_cm

        Try
            cnn.Open()
            cmd.ExecuteNonQuery()
            If CInt(cmd.Parameters("@status").Value) = 1 Then
                NumUpdatedRows += 1
            Else
                NumAddedRows += 1
            End If
            cnn.Close()
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
            LogStr = vbCrLf & "Running ExecuteSnowCommand,st_id=" & _
            st_id.ToString & " " & ex.Message
        End Try
        Return LogStr
    End Function

    Private Function ExecutePrecipitationDailyCommand _
        (ByVal cmd As SqlCommand, ByVal st_id As Integer, ByVal obs_time As DateTime, _
    ByVal precip_mm_10 As Integer,
    ByRef NumUpdatedRows As Integer, ByRef NumAddedRows As Integer) As String
        'all values are multiplied by 10 (result is in mm!!)
        Dim ScalingH As Integer = 10
        Dim LogStr As String = ""
        Dim cnn As SqlConnection = cmd.Connection

        cmd.Parameters("@station_id").Value = st_id
        cmd.Parameters("@obs_time").Value = obs_time
        cmd.Parameters("@rain").Value = precip_mm_10

        Try
            cnn.Open()
            cmd.ExecuteNonQuery()
            If CInt(cmd.Parameters("@status").Value) = 1 Then
                NumUpdatedRows += 1
            Else
                NumAddedRows += 1
            End If
            cnn.Close()
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
            LogStr = vbCrLf & "Running ExecutePrecipitationDailyCommand,st_id=" & _
            st_id.ToString & " " & ex.Message
        End Try
        Return LogStr
    End Function

    Private Sub ExecuteLogCommand _
        (ByVal cmd As SqlCommand, ByVal log_time As DateTime, _
    ByVal log_text As String)
        'all values are multiplied by 10 (result is in mm!!)
        Dim ScalingH As Integer = 10
        Dim LogStr As String = ""
        Dim cnn As SqlConnection = cmd.Connection

        cmd.Parameters("@log_time").Value = log_time
        cmd.Parameters("@log_text").Value = log_text


        Try
            cnn.Open()
            cmd.ExecuteNonQuery()
            cnn.Close()
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
        End Try
    End Sub

    'pomocna funkce: spusteni objektu command pri aktualizaci tabulky observations
    Private Function ExecuteObservationCommand _
    (ByVal cmd As SqlCommand, ByVal st_seq As Integer, _
    ByVal var_name As String, ByVal obs_time As DateTime, ByVal obs_value As Integer, _
    ByRef NumUpdatedRows As Integer) As String

        Dim LogStr As String
        Dim cnn As SqlConnection = cmd.Connection

        'osetreni vyjimek (hodnota obs_value mimo rozsah int 16
        If obs_value > 20000 And obs_value < 200000 Then
            Select Case var_name
                Case "discharge_big"
                Case "discharge_small"
                    var_name = "discharge_extreme"
                    obs_value = CInt(obs_value / 10)
                Case Else
                    obs_value = 20000
            End Select
        End If

        cmd.Parameters("@station_seq").Value = st_seq
        cmd.Parameters("@variable_name").Value = var_name
        cmd.Parameters("@obs_time").Value = obs_time
        cmd.Parameters("@obs_value").Value = obs_value
        Try
            cnn.Open()
            cmd.ExecuteNonQuery()
            NumUpdatedRows += 1
            cnn.Close()
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
            LogStr = vbCrLf & "Running ExecuteObservationCommand,st_seq=" & _
            st_seq.ToString & " " & ex.Message
        End Try
        Return LogStr
    End Function

    'auxiliary function: Executes a SqlCommand object and updates tables
    '"periods" and "observations2"
    'PARAMETERS:
    'cmd: The SqlCommand, created by CreateObservationCommand2() function
    'st_id: id of the station
    'var_name: variable name ("precip_hour","snow","stage","discharge"..)
    'obs_time: time of observation
    'obs_value: observation value (scaled to integer)
    'NumUpdatedRows: rows already existing in database, their values are changed
    'NumAddedRows: rows newly inserted into Observations2 database table
    'RETURNS:
    'LogStr: log string in case of exception
    'CALLED BY:
    'UpdateObsPrecip2(), UpdateObsHydro2(), UpdateObsSnow2(), UpdateRadar()
    'NOTE:
    'Whether a row is updated or inserted, is returned by the stored procedure
    'if  obs_value = 0, the period is updated in database but no value is 
    'inserted to "observations2" table
    Private Function ExecuteObservationCommand2 _
    (ByVal cmd As SqlCommand, ByVal st_id As Integer, _
    ByVal var_name As String, ByVal obs_time As DateTime, ByVal obs_value As Integer, _
    ByRef NumUpdatedRows As Integer, ByRef NumAddedRows As Integer) As String

        Dim LogStr As String
        Dim cnn As SqlConnection = cmd.Connection

        cmd.Parameters("@station_id").Value = st_id
        cmd.Parameters("@variable_name").Value = var_name
        cmd.Parameters("@obs_time").Value = obs_time
        cmd.Parameters("@obs_value").Value = obs_value
        Try
            cnn.Open()
            cmd.ExecuteNonQuery()
            Select Case CInt(cmd.Parameters("@status").Value)
                Case 1
                    NumUpdatedRows += 1
                Case 2
                    NumAddedRows += 1
            End Select
        Catch ex As Exception
            LogStr = vbCrLf & "Running ExecuteObservationCommand,st_id=" & _
            st_id.ToString & " " & ex.Message
        Finally
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
        End Try
        Return LogStr
    End Function

    'auxiliary function: checks if a string is a valid number
    Public Function IsValidNumber(ByVal str As String) As Boolean
        str = str.Trim
        Dim NumDecSeparators As Integer = 0
        Dim c As Char
        Dim result As Boolean = True
        For Each c In str
            If (c = "." Or c = ",") Then
                NumDecSeparators += 1
            ElseIf Not Char.IsDigit(c) Then
                result = False
                Exit For
            End If
            If NumDecSeparators > 1 Then
                result = False
                Exit For
            End If
        Next
        Return result
    End Function
    'auxiliary function: checks if a string is in "DD.MM hh:mm" format
    Public Function IsValidDateString(ByVal str As String) As Boolean
        Dim r As Regex
        Dim m As Match

        r = New Regex("(?<1>\d{2})\.(?<2>\d{2})[\.]??\s+(?<3>\d{2}):(?<4>\d{2})", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)
        m = r.Match(str)
        If m.Success Then
            Return True
        Else
            Return False
        End If
    End Function

    'auxiliary function: check if a string is in "DD.MM.YYYY hh:mm" format
    Public Function IsValidDateStringDMY(ByVal str As String) As Boolean
        Dim r As Regex
        Dim m As Match

        r = New Regex("(?<1>\d{2})\.(?<2>\d{2})\.(?<3>\d{4})[\.]??\s+(?<4>\d{2}):(?<5>\d{2})", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)
        m = r.Match(str)
        If m.Success Then
            Return True
        Else
            Return False
        End If
    End Function


    'auxiliary function: check if a string is in "DD.MM.YYYY hh:mm" format
    Public Function IsValidDateStringDMYH(ByVal str As String) As Boolean
        Dim r As Regex
        Dim m As Match

        r = New Regex("(?<1>\d{2})\.(?<2>\d{2})\.(?<3>\d{4})[\.]??\s+(?<4>\d{2}):00", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)
        m = r.Match(str)
        If m.Success Then
            Return True
        Else
            Return False
        End If
    End Function

    'auxiliary function: converts a string "DD.MM hh:mm" into a DateTime
    'if conversion failed, returns nothing
    'Year specifies the year of the date
    Public Function String2Date(ByVal Year As Integer, ByVal str As String) As DateTime
        Dim r As Regex
        Dim m As Match
        Dim ResultDate As DateTime

        r = New Regex("(?<1>\d{2})\.(?<2>\d{2})[\.]??\s+(?<3>\d{2}):(?<4>\d{2})", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)
        m = r.Match(str)
        If m.Success Then
            ResultDate = New DateTime( _
            Year, Integer.Parse(m.Groups(2).Value), _
            Integer.Parse(m.Groups(1).Value), _
            Integer.Parse(m.Groups(3).Value), _
            Integer.Parse(m.Groups(4).Value), _
            0)
        Else
            ResultDate = Nothing
        End If
        Return ResultDate
    End Function

    'auxiliary function: converts a string "DD.MM.YYYY hh:mm" into a DateTime
    'if conversion failed, returns nothing
    Public Function String2DateDMY(ByVal str As String) As DateTime
        Dim r As Regex
        Dim m As Match
        Dim ResultDate As DateTime

        r = New Regex("(?<1>\d{2})\.(?<2>\d{2})\.(?<3>\d{4})[\.]??\s+(?<4>\d{2}):(?<5>\d{2})", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)
        m = r.Match(str)
        If m.Success Then
            ResultDate = New DateTime( _
            Integer.Parse(m.Groups(3).Value), _
            Integer.Parse(m.Groups(2).Value), _
            Integer.Parse(m.Groups(1).Value), _
            Integer.Parse(m.Groups(4).Value), _
            Integer.Parse(m.Groups(5).Value), _
            0)
        Else
            ResultDate = Nothing
        End If
        Return ResultDate
    End Function

    'auxiliary function: safely converts a string with "." or "," to a "single"
    'data type number
    Public Function String2Single(ByVal str As String) As Single

        Dim nfi01 As System.Globalization.NumberFormatInfo = _
            New System.Globalization.CultureInfo("en-US", False).NumberFormat
        Dim nfi02 As System.Globalization.NumberFormatInfo = _
            New System.Globalization.CultureInfo("cs-CZ", False).NumberFormat
        If str.StartsWith(".") Or str.StartsWith(",") Then str = "0" + str
        If (str.IndexOf(".") > -1) Then
            Return (Single.Parse(str, System.Globalization.NumberStyles.Float, nfi01))
        Else
            Return (Single.Parse(str, System.Globalization.NumberStyles.Float, nfi02))
        End If
    End Function

    'pomocna funkce: spocte pocet vyskytu <TABLE v html souboru
    Private Function CountTables(ByVal FileStr As String) As Integer
        Dim sr As New StringReader(FileStr)
        Dim Count As Integer = 0
        Dim CurLine As String
        CurLine = sr.ReadLine()

        Do While sr.Peek >= 0
            If CurLine.IndexOf("<TABLE") >= 0 Then Count += 1
            CurLine = sr.ReadLine()
        Loop
        Return Count
    End Function


    'pomocna funkce: preskoceni radek
    Private Sub SkipRows(ByVal sr As StringReader, ByVal RowsCount As Integer)
        Dim i As Integer
        For i = 0 To RowsCount - 1
            sr.ReadLine()
        Next
    End Sub

    'prevod casu z UT 0-0 na UT 6-6
    Private Function Time00toDate66(ByVal Time00 As DateTime) As DateTime
        'prevedeni na interval od 6 do 6 hod UT
        'cele datum se pri prekroceni 6 hod UT zmeni
        Dim Date66 As DateTime
        If Time00.Hour <= 6 Then
            Date66 = Time00.AddHours(-24 - Time00.Hour)
        Else
            Date66 = Time00.AddHours(-Time00.Hour)
        End If
    End Function


    'prevod casu z typu DateTime na datovy retezec MS Access
    Private Function DateTime2AccessString(ByVal dt As DateTime) As String
        Dim y As String = dt.ToString("yyyy")
        Dim M As String = dt.ToString("MM")
        Dim d As String = dt.ToString("dd")
        Dim t As String = dt.ToString("HH:mm:ss")
        Return String.Format("{0}/{1}/{2} {3}", y, M, d, t)
    End Function

    'zapis retezce do 'log' souboru
    Public Sub WriteLogFileToDb(ByVal LogStr As String)
        'Dim LogFilePath As String = ViewConfiguration("LogFilePath")

        Dim cmdText As String = "plaveninycz.new_update_log"

        Dim sql_cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim logCmd As SqlCommand = Me.CreateLogCommand(sql_cnn)

        ExecuteLogCommand(logCmd, DateTime.Now, LogStr)
    End Sub

    Public Sub UpdateDailyData()
        UpdateDailyData_Variable("teplota", "avg")
        UpdateDailyData_Variable("srazky", "sum")
        UpdateDailyData_Variable("vodstav", "avg")
        UpdateDailyData_Variable("prutok", "avg")
        UpdateDailyData_Variable("tmax", "max")
        UpdateDailyData_Variable("tmin", "min")
    End Sub

    'updating the DAILY data based on the hourly data
    Public Function UpdateDailyData_Variable(ByVal variableName As String, ByVal stat As String) As String
        'now we can start updating the daily..data
        Dim dataFolder As String = ConfigurationManager.AppSettings("files_dir1")
        Dim binManager As New BinaryFileManager
        Dim msg As String

        'first: for Temperature
        Dim vFolder As String = Path.Combine(dataFolder, "h", variableName)
        If variableName = "tmax" Then
            vFolder = Path.Combine(dataFolder, "h", "teplota")
        End If
        If variableName = "tmin" Then
            vFolder = Path.Combine(dataFolder, "h", "teplota")
        End If

        Dim dFiles() As String = Directory.GetFiles(vFolder)
        For Each dFile In dFiles

            Dim outFile As String = dFile.Replace("\h", "\d")
            If variableName = "tmax" Then
                outFile = outFile.Replace("teplota", "tmax")
            End If
            If variableName = "tmin" Then
                outFile = outFile.Replace("teplota", "tmin")
            End If

            'binary file manager - hourly to daily conversion
            Try
                binManager.HourlyToDaily(dFile, outFile, stat)
            Catch ex As Exception
                msg = ex.Message
            End Try
        Next


    End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class