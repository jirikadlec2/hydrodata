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


Public Class DbUpdateClass
    ' KONSTRUKTOR
    Public Sub New()
        Me.new("Integrated Security=SSPI;Persist Security Info=False;" & _
        "Initial Catalog=Hydro4;Data Source=(local);user=sa; password=+chnbz+")
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


    'trida pro ulozeni srazkomerne stanice a jejich 
    'aktualnich udaju...
    Private Class PrecipStation
        Public ReadOnly Property Id() As Integer
            Get
                Id = m_Id
            End Get
        End Property
        Public ReadOnly Property Seq() As Integer
            Get
                Seq = m_Seq
            End Get
        End Property
        Public ReadOnly Property Name() As String
            Get
                Name = m_Name
            End Get
        End Property
        Public ReadOnly Property MaxDbTime() As DateTime
            Get
                MaxDbTime = m_MaxDbTime
            End Get
        End Property

        'return false if the start time of observation series is not
        'initialized or if there are no observations in the list
        Public ReadOnly Property HasObservations() As Boolean
            Get
                If m_Observations.Length >= 4 Or m_StartWebTime > DateTime.MinValue Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public Property StartWebTime() As DateTime
            Get
                Return m_StartWebTime
            End Get
            Set(ByVal Value As DateTime)
                m_StartWebTime = Value
            End Set
        End Property

        Public ReadOnly Property Observations() As String
            Get
                Return m_Observations.ToString()
            End Get
        End Property

        Public ReadOnly Property NumObservations() As Integer
            Get
                Return CInt(m_Observations.Length / 4)
            End Get
        End Property

        Private m_Id As Integer
        Private m_Seq As Integer
        Private m_Name As String
        Private m_MaxDbTime As DateTime
        Private m_StartWebTime As DateTime
        Private m_Observations As System.Text.StringBuilder

        Public Sub New(ByVal Id As Integer, ByVal Seq As Integer, ByVal Name As String, ByVal MaxDbTime As DateTime)
            m_Id = Id
            m_Seq = Seq
            m_Name = Name
            m_MaxDbTime = MaxDbTime
            m_Observations = New System.Text.StringBuilder(200)
            m_StartWebTime = DateTime.MinValue
        End Sub

        'appends the observation to the internal stringBuilder list
        Public Sub AddObservation(ByVal obsValue As Double)
            If obsValue >= 0 And obsValue < 999 Then
                obsValue = CInt(Math.Round(obsValue * 10))
                Dim tempval As String = obsValue.ToString("0000")
                m_Observations.Append(tempval)
            End If
        End Sub

        'adds a ''missing'' observation value to the list
        Public Sub AddMissingValue()
            m_Observations.Append("9999")
        End Sub

        'writes the observation values to database and
        'returns a string with eventual error
        'connStr is the database connection string..
        Public Function UpdateDB(ByVal cnn As SqlConnection, ByRef AddedRows As Integer) As String
            Dim cmd As New SqlCommand
            Dim errorMessage As String
            Dim removeIndex As Integer
            Dim MissingValue As String = "9999"

            If Me.HasObservations Then

                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "update_precip_hour"
                cmd.Connection = cnn
                cmd.Parameters.Add(New SqlParameter("@observations", SqlDbType.VarChar))
                cmd.Parameters.Add(New SqlParameter("@station_id", SqlDbType.SmallInt))
                cmd.Parameters.Add(New SqlParameter("@start_time", SqlDbType.SmallDateTime))
                cmd.Parameters.Add(New SqlParameter("@added_rows", SqlDbType.SmallInt))
                cmd.Parameters("@added_rows").Direction = ParameterDirection.Output

                'remove any characters at the end of m_observations having missing values
                removeIndex = m_Observations.Length
                While removeIndex >= 4
                    If m_Observations.ToString(removeIndex - 4, 4) <> MissingValue Then
                        Exit While
                    End If
                    removeIndex = removeIndex - 4
                End While
                If removeIndex < m_Observations.Length Then
                    m_Observations.Remove(removeIndex, m_Observations.Length - removeIndex)
                End If
                Dim obsString As String = m_Observations.ToString()
                cmd.Parameters("@observations").Value = obsString
                cmd.Parameters("@station_id").Value = m_Id
                cmd.Parameters("@start_time").Value = m_StartWebTime

                Try
                    cnn.Open()
                    cmd.ExecuteNonQuery()
                    AddedRows += CInt(cmd.Parameters("@added_rows").Value)
                Catch ex As Exception
                    errorMessage = ex.Message
                Finally
                    cnn.Close()
                End Try
            End If
            Return errorMessage
        End Function

        'override equals and gethashcode!!
        Overrides Function Gethashcode() As Integer
            Return m_Seq
        End Function

        Overloads Overrides Function Equals(ByVal o As Object) As Boolean
            Dim result As Boolean = False
            If o.GetHashCode() = Me.Gethashcode() Then
                result = True
            End If
            Return result
        End Function
    End Class



    'trida pro ulozeni teplotni stanice a jejich 
    'aktualnich udaju...
    Private Class TemperatureStation
        Public ReadOnly Property Id() As Integer
            Get
                Id = m_Id
            End Get
        End Property
        Public ReadOnly Property OperatorId() As Integer
            Get
                OperatorId = m_OperatorId
            End Get
        End Property
        Public ReadOnly Property Seq() As Integer
            Get
                Seq = m_Seq
            End Get
        End Property
        Public ReadOnly Property Name() As String
            Get
                Name = m_Name
            End Get
        End Property

        Public ReadOnly Property DivisionName() As String
            Get
                DivisionName = m_DivisionName
            End Get
        End Property
        Public ReadOnly Property MeteoCode() As String
            Get
                MeteoCode = m_MeteoCode
            End Get
        End Property
        Public ReadOnly Property MaxDbTime() As DateTime
            Get
                MaxDbTime = m_MaxDbTime
            End Get
        End Property

        'return false if the start time of observation series is not
        'initialized or if there are no observations in the list
        Public ReadOnly Property HasObservations() As Boolean
            Get
                If m_Observations.Length >= 4 Or m_StartWebTime > DateTime.MinValue Then
                    Return True
                Else
                    Return False
                End If
            End Get
        End Property

        Public Property StartWebTime() As DateTime
            Get
                Return m_StartWebTime
            End Get
            Set(ByVal Value As DateTime)
                m_StartWebTime = Value
            End Set
        End Property

        'the observations are being stored as:
        '0000111122223333....string, where each group of
        'four digits represents one observation
        Public ReadOnly Property Observations() As String
            Get
                Return m_Observations.ToString()
            End Get
        End Property

        Public ReadOnly Property NumObservations() As Integer
            Get
                Return CInt(m_Observations.Length / 4)
            End Get
        End Property

        Private m_Id As Integer
        Private m_OperatorId As Integer
        Private m_Seq As Integer
        Private m_Name As String
        Private m_MaxDbTime As DateTime
        Private m_StartWebTime As DateTime
        Private m_Observations As System.Text.StringBuilder
        Private m_DivisionName As String
        Private m_MeteoCode As String

        Public Sub New(ByVal Id As Integer, ByVal OperatorId As Integer, ByVal Seq As Integer, ByVal Name As String,
                       ByVal DivisionName As String, ByVal MeteoCode As String, ByVal MaxDbTime As DateTime)
            m_Id = Id
            m_OperatorId = OperatorId
            m_Seq = Seq
            m_Name = Name
            m_DivisionName = DivisionName
            m_MeteoCode = MeteoCode
            m_MaxDbTime = MaxDbTime
            m_Observations = New System.Text.StringBuilder(200)
            m_StartWebTime = DateTime.MinValue
        End Sub

        Public Overrides Function ToString() As String
            Return Id.ToString() & " " & Name
        End Function

        'appends the observation to the internal stringBuilder list
        Public Sub AddObservation(ByVal obsValue As Double)
            If obsValue < 999 Then
                obsValue = CInt(Math.Round(obsValue * 10))
                If obsValue >= 0 Then
                    Dim tempval As String = obsValue.ToString("0000")
                    m_Observations.Append(tempval)
                Else
                    Dim tempval As String = obsValue.ToString("000")
                    m_Observations.Append(tempval)
                End If
            End If
        End Sub

        'adds a ''missing'' observation value to the list
        Public Sub AddMissingValue()
            m_Observations.Append("9999")
        End Sub

        'writes the observation values to database and
        'returns a string with eventual error
        'connStr is the database connection string..
        Public Function UpdateDB(ByVal cnn As SqlConnection, ByRef AddedRows As Integer) As String
            Dim cmd As New SqlCommand
            Dim errorMessage As String
            Dim removeIndex As Integer
            Dim MissingValue As String = "9999"

            If Me.HasObservations Then

                cmd.CommandType = CommandType.StoredProcedure
                cmd.CommandText = "update_temperature"
                cmd.Connection = cnn
                cmd.Parameters.Add(New SqlParameter("@observations", SqlDbType.VarChar))
                cmd.Parameters.Add(New SqlParameter("@station_id", SqlDbType.SmallInt))
                cmd.Parameters.Add(New SqlParameter("@start_time", SqlDbType.SmallDateTime))
                cmd.Parameters.Add(New SqlParameter("@added_rows", SqlDbType.SmallInt))
                cmd.Parameters("@added_rows").Direction = ParameterDirection.Output

                'remove any characters at the end of m_observations having missing values
                removeIndex = m_Observations.Length
                While removeIndex >= 4
                    If m_Observations.ToString(removeIndex - 4, 4) <> MissingValue Then
                        Exit While
                    End If
                    removeIndex = removeIndex - 4
                End While
                If removeIndex < m_Observations.Length Then
                    m_Observations.Remove(removeIndex, m_Observations.Length - removeIndex)
                End If
                Dim obsString As String = m_Observations.ToString()
                cmd.Parameters("@observations").Value = obsString
                cmd.Parameters("@station_id").Value = m_Id
                cmd.Parameters("@start_time").Value = m_StartWebTime

                Try
                    cnn.Open()
                    cmd.ExecuteNonQuery()
                    AddedRows += CInt(cmd.Parameters("@added_rows").Value)
                Catch ex As Exception
                    errorMessage = ex.Message
                Finally
                    cnn.Close()
                End Try
            End If
            Return errorMessage
        End Function

        'override equals and gethashcode!!
        Overrides Function Gethashcode() As Integer
            Return m_Seq
        End Function

        Overloads Overrides Function Equals(ByVal o As Object) As Boolean
            Dim result As Boolean = False
            If o.GetHashCode() = Me.Gethashcode() Then
                result = True
            End If
            Return result
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

    Public Function UpdateRadar() As String
        'promenne
        Dim LogStr As String = "Executing UpdateRadar.."

        Dim RadarFilePathBase As String = "J:\dev\Radar\"

        Try
            RadarFilePathBase = HttpContext.Current.Server.MapPath("/Radar/")
        Catch
            RadarFilePathBase = "D:\Websites\448cf9624b\www\Radar\"
        End Try

        If Not Directory.Exists(RadarFilePathBase) Then
            RadarFilePathBase = "J:\dev\Radar\"
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
        Dim RadarFileUriPath_de As String = ViewConfiguration("RadarFileUriPath_de")
        Dim RadarFileUriPath_cz_1h As String = ViewConfiguration("RadarFileUriPath_cz_1h")
        Dim RadarSaveMode As String = ViewConfiguration("RadarSaveMode")
        Dim RadarFilePath_cz As String
        Dim RadarFilePath_de As String
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
                ShortYearStr & MonthStr & DayStr & HourStr & MinuteStr & ".gif"

                RadarFileSavedName = _
                "cz" & YearStr & MonthStr & DayStr & HourStr & MinuteStr & ".gif"

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
        '******* GERMAN RADARS ********************************
        '------------------------------------------------------
        cmd.Parameters("@radarnet_id").Value = 2
        RadarFileUriPath = RadarFileUriPath_de
        RadarFilePath_de = RadarFilePathBase & "\de\"
        TimeStepMinutes = 15

        RadarFileTime = DateTime.UtcNow.AddDays(-1).Date
        While RadarFileTime < DateTime.UtcNow

            RadarFileTime = RadarFileTime.AddMinutes(TimeStepMinutes)

            'only proceed if the file is not yet saved and in database
            If RadarFileTime > LastRadarObsTime Then

                YearStr = Format(RadarFileTime.Year, "0000")

                MonthStr = Format(RadarFileTime.Month, "00")
                DayStr = Format(RadarFileTime.Day, "00")
                HourStr = Format(RadarFileTime.Hour, "00")
                MinuteStr = Format(RadarFileTime.Minute, "00")

                RadarFileUriName = _
                YearStr & MonthStr & DayStr & HourStr & MinuteStr

                RadarFileSavedName = _
                "de" & YearStr & MonthStr & DayStr & HourStr & MinuteStr & ".gif"

                RadarFilePath = RadarFilePath_de & YearStr & "\" & MonthStr & "\"
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

    '-----------------------------------------------------------------------------------------------
    '-----------------------------------------------------------------------------------------------
    '********************** PRECIPITATION **********************************************************
    '-----------------------------------------------------------------------------------------------

    '-----------------------------------------------------------------------------------------------
    '********************** DAILY PRECIPITATION ****************************************************
    '-----------------------------------------------------------------------------------------------

    'public method
    'DESCRIPTION:
    ' downloads and updates daily precipitation in CHMI and Povodi stations
    'CALLS:
    ' UpdateSynop_24h()
    'RETURNS:
    ' LogStr: String containing report and exception messages
    Public Function UpdatePrecip_24h() As String
        'log message variables
        Dim LogStr As String = "Executing UpdatePrecip_24h.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        Dim VariableName As String = "precip_day"
        Dim NumAddedRows As Integer = 0
        Dim NumUpdatedRows As Integer = 0
        UpdateSynop_24h("PcpUri01", VariableName, LogStr, NumUpdatedRows, NumAddedRows)

        'return result log of the function
        ComputationTime = DateTime.Now.Subtract(StartTime)
        Return LogStr & vbCrLf & _
        "rows updated: " & NumUpdatedRows & vbCrLf & _
        "rows added: " & NumAddedRows & vbCrLf & _
        "Time taken: " & ComputationTime.ToString
    End Function

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
    Private Sub UpdateSynop_24h(ByVal UriName As String, ByVal VariableName As String, _
    ByRef logstr As String, ByRef NumUpdatedRows As Integer, ByRef NumAddedRows As Integer)

        Dim DataUriStr As String = Me.ViewConfiguration(UriName)
        Dim WebC As WebClient = WebUtils.CreateWebClient()
        Dim buf() As Byte
        Dim FileString As String

        'variables for creating snow station table
        Dim cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim StationCmd As SqlCommand = Me.CreateStationIdForSnowCommand(cnn)
        Dim ObsCmd As SqlCommand = Me.CreateObservationCommand2(cnn)
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
            logstr &= vbCrLf & "Executing UpdateSynop_24h.. " & _
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
            If VariableName = "precip_day" Then ObsTime = ObsTime.AddDays(1)
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
            'CurStationId = Me.GetStationIdByCoord(cnn, StationCmd, CurStationCoord, CoordType)
            CurStationId = Me.GetStationIdForSnow(cnn, StationCmd, CurNameStr)
            'update observation
            If CurStationId > 0 Then
                logstr &= Me.ExecuteObservationCommand2(ObsCmd, CurStationId, _
                VariableName, ObsTime, ObsValue, NumUpdatedRows, NumAddedRows)
            End If
            m = m.NextMatch()
        End While
        'write error message if there was no match
        If MatchCount = 0 Then
            logstr &= vbCrLf & "CHMI data file: No regex match for " & VariableName
        End If

        'set values of remaining stations to zero
        logstr &= Me.GetZeroStations(ObsTime, VariableId, ZeroStationList)
        ObsValue = 0
        For Each CurStationId In ZeroStationList
            logstr &= Me.ExecuteObservationCommand2(ObsCmd, CurStationId, _
            VariableName, ObsTime, ObsValue, NumUpdatedRows, NumAddedRows)
        Next
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
    ' downloads and updates daily precipitation in CHMI and Povodi stations
    'CALLS:
    ' UpdatePrecip_povodi(Povodi,NumUpdatedRows,NumAddedRows)
    'RETURNS:
    ' LogStr: String containing report and exception messages
    Public Overloads Function UpdatePrecip_povodi() As String
        'log message variables
        Dim LogStr As String = "Executing UpdatePrecip_povodi.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan
        Dim NumAddedRows As Integer = 0
        Dim NumUpdatedRows As Integer = 0

        'table of all stations
        Dim stationTable As New Hashtable(200)
        LogStr &= vbCrLf & GetPrecipStationTable(stationTable, False)

        LogStr &= vbCrLf & UpdatePrecip_povodi("pvl", stationTable, NumAddedRows)
        LogStr &= vbCrLf & UpdatePrecip_povodi("poh", stationTable, NumAddedRows)
        LogStr &= vbCrLf & UpdatePrecip_povodi("pmo", stationTable, NumAddedRows)
        LogStr &= vbCrLf & UpdatePrecip_povodi("pod", stationTable, NumAddedRows)
        LogStr &= vbCrLf & UpdatePrecip_povodi("pla", stationTable, NumAddedRows)

        'return result log of the function
        ComputationTime = DateTime.Now.Subtract(StartTime)
        Return LogStr & vbCrLf & _
        "rows added: " & NumAddedRows & vbCrLf & _
        "Time taken: " & ComputationTime.ToString
    End Function


    'testing version of the function!
    'works with the table of stations for povodi
    Private Overloads Function UpdatePrecip_povodi(ByVal Povodi As String, _
    ByVal StationTable As Hashtable, ByRef NumAddedRows As Integer) As String
        Dim LogStr As String = "executing UpdatePrecip_povodi(" & Povodi & ").."
        Dim VariableName As String = "precip_day"
        Dim DataUriStr As String = Me.ViewConfiguration("PcpUri_" & Povodi)
        Dim stationUriStr As String
        Dim WebC As WebClient = WebUtils.CreateWebClient()
        Dim buf() As Byte
        Dim FileString As String

        'variables for reading files from the web
        Dim r1, r2 As Regex
        Dim m1, m2 As Match
        Dim MatchCount As Integer = 0
        Dim CurStr As String
        Dim CurNameStr As String
        Dim CurSeqStr As String
        Dim CurValueStr As String

        'specify regular expression
        r1 = New Regex("<td[^>]*>\s*" & _
        "<font[^>]*>(?<1>[^<]+)<br>[^<]*</font>\s*" & _
        "<font[^>]*>[^0-9,\.]*(?<2>[\d\.,]+)[^<]*</font>\s*<br>\s*" & _
        "</td>\s*</tr>\s*(</tbody>)?\s*</table>\s*</div>\s*" & _
        "<div\s*id\s*=\s*[""]?bod(?<3>\w+)", _
                RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        r2 = New Regex("(?<1>\d{2})\.(?<2>\d{2})\.(?<3>\d{4})\s+(?<4>\d{2})", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'observation time and value and location variables
        Dim ObsTime As DateTime
        Dim ObsValue As Integer
        Dim ScaleFactor As Integer = 10
        Dim CurStation As PrecipStation
        Dim Seq As Integer
        Dim StationId As Integer = 0
        Dim NumUpdatedRows As Integer = 0

        'SQL database variables
        Dim cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim cnn2 As New SqlConnection(Me.SqlDbConnectionString)
        'Dim StationCmd As SqlCommand = Me.CreateStationIdCommand(cnn)
        Dim ObsCmd As SqlCommand = Me.CreateObservationCommand2(cnn)

        'get the seq base number for povodi (used to identify station in database)
        Dim PovodiSeq As Integer
        Select Case Povodi
            Case "pvl"
                PovodiSeq = 4100000
            Case "poh"
                PovodiSeq = 4200000
            Case "pla"
                PovodiSeq = 4300000
            Case "pmo"
                PovodiSeq = 4400000
            Case "pod"
                PovodiSeq = 4500000
        End Select

        'read the html file
        Try
            buf = WebC.DownloadData(DataUriStr & "mapa_vse.htm")
            FileString = System.Text.Encoding.GetEncoding(1250).GetString(buf)
        Catch ex As Exception
            FileString = ""
            LogStr &= vbCrLf & "downloading file:" & ex.Message
        End Try

        'read the observations
        MatchCount = 0
        m1 = r1.Match(FileString)

        While m1.Success
            MatchCount = MatchCount + 1
            'read observation time
            CurValueStr = m1.Groups(1).Value
            m2 = r2.Match(CurValueStr)
            If m2.Success Then
                ObsTime = New DateTime(Integer.Parse(m2.Groups(3).Value), _
                Integer.Parse(m2.Groups(2).Value), Integer.Parse(m2.Groups(1).Value), _
                Integer.Parse(m2.Groups(4).Value), 0, 0)
                'conversion to UT
                ObsTime = ObsTime.AddHours(-1)
            End If
            'read observation value
            CurValueStr = m1.Groups(2).Value
            ObsValue = CInt(Math.Round(Me.String2Single(CurValueStr) * ScaleFactor))
            'read station web code and get station id
            CurValueStr = m1.Groups(3).Value
            CurSeqStr = CurValueStr.ToLower()
            Seq = PovodiSeq + Integer.Parse(CurSeqStr)

            'retrieve other station information from the hashtable
            If StationTable.ContainsKey(Seq) Then
                CurStation = CType(StationTable(Seq), PrecipStation)
                StationId = CurStation.Id

                'update the hourly observations for the station
                If CurStation.MaxDbTime < ObsTime Then
                    stationUriStr = DataUriStr & "mereni_" & CurSeqStr & ".htm"
                    LogStr &= UpdatePrecip_povodi_hour _
                    (stationUriStr, CurStation, ObsTime, ObsValue, cnn2, NumAddedRows)
                End If
                'update 24-hour-observation
                LogStr &= Me.ExecuteObservationCommand2(ObsCmd, StationId, _
                        VariableName, ObsTime, ObsValue, NumUpdatedRows, NumAddedRows)
            Else
                'write a message that the online station is not in our database..
                LogStr &= vbCrLf & "station seq=" & CurSeqStr & "not in database"
            End If
            m1 = m1.NextMatch()
        End While
        'write error message if there was no match
        If MatchCount = 0 Then
            LogStr &= vbCrLf & Povodi & "web page : No regex match for " & VariableName
        End If

        Return LogStr
    End Function

    'updates HOURLY precipitation for a station of POVODI !!!
    Private Function UpdatePrecip_povodi_hour(ByVal Url As String, _
    ByVal st As PrecipStation, ByVal Time24h As DateTime, ByVal Total24h As Integer, _
    ByVal sql_cnn As SqlConnection, ByRef AddedRows As Integer) As String

        Dim LogStr As String = ""

        'declare local variables
        Dim Observations(24) As Single
        Dim ObsHours As Integer = 0
        Dim MaxObsHours As Integer = 0
        Dim AdditionalHour As Integer = 0
        Dim ObsSum As Single = 0
        Dim Sum24h As Single = Total24h / 10.0F
        Dim DbMissingValue As Single = -1

        'resolve the URL of povodi
        Dim webC As WebClient = WebUtils.CreateWebClient()
        Dim buf() As Byte
        Dim fileString As String

        Dim r1 As New Regex( _
        "<tr>\s*<td[^>]*>\s*<font[^>]+>(?<1>\d{2}\.\d{2}\.\d{2}\s{1}\d{2}\:\d{2})[^<]*</font></td>\s*" & _
        "<td[^>]*><font[^>]+>(?<2>[\d\.,]+)[^<]*</font></td>\s*" & _
        "<td[^>]*>", _
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
        Dim CurValueStr As String
        Dim m1 As Match
        Dim CurValue As Single = -1
        Dim MaxDbTime As DateTime
        Dim TimeDiff As Double
        Dim universalTime As DateTime
        'DateTime prevTime = DateTime.Now;

        'set all values in observations array to missing
        Dim i As Integer
        For i = 0 To Observations.Length - 1
            Observations(i) = DbMissingValue
        Next

        'set the times
        MaxDbTime = st.MaxDbTime
        MaxObsHours = CInt(Time24h.Subtract(MaxDbTime).TotalHours - 1)
        If MaxObsHours > 23 Then MaxObsHours = 23

        m1 = r1.Match(fileString)
        While (m1.Success)
            MatchCount = MatchCount + 1
            CurStr = m1.Groups(2).Value
            If CurStr.Length > 0 Then
                CurValue = Me.String2Single(CurStr)
                ObsSum = ObsSum + CurValue
                Observations(ObsHours) = CurValue
            End If
            ObsHours = ObsHours + 1
            If ObsHours > MaxObsHours Then Exit While
            m1 = m1.NextMatch()
        End While

        'write error message if there was no match
        If MatchCount = 0 Then
            LogStr &= vbCrLf & Url & ": No regex match!"
        End If

        'process the observations - add them to the station!
        While ObsHours < MaxObsHours
            If Sum24h - ObsSum < 0.4F Then
                CurValue = 0.0F
            Else
                CurValue = Sum24h - ObsSum
            End If
            ObsSum = ObsSum + CurValue
            Observations(ObsHours) = CurValue
            ObsHours = ObsHours + 1
        End While
        st.StartWebTime = Time24h.AddHours(-ObsHours + 1)

        'add observations to the station object
        For i = ObsHours - 1 To 0 Step -1
            st.AddObservation(Observations(i))
        Next
        'write observations to database
        st.UpdateDB(sql_cnn, AddedRows)
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
    Public Function UpdatePrecip() As String
        'log message variables
        Dim LogStr As String = "Executing UpdatePrecip.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        Dim DayOffset As Integer
        Dim DayObd As Integer
        Dim UpdatedRows As Integer = 0
        Dim AddedRows As Integer = 0
        Dim curStation As PrecipStation
        Dim cnn As New SqlConnection(m_SqlDbConnectionString)

        'the max.number of days to go back downloading chmi pages
        Dim MaxDayOffset As Integer = -7
        'Dim stKey As Integer

        'first create a precip. station table from the DB
        Dim stationTable As New Hashtable
        LogStr &= vbCrLf & GetPrecipStationTable(stationTable, True)

        For DayOffset = MaxDayOffset To 0 Step 1
            LogStr = LogStr & Me.DownloadPrecip(DayOffset, stationTable, UpdatedRows)
        Next

        'add the values for each station to database
        For Each stEntry As DictionaryEntry In stationTable
            curStation = CType(stEntry.Value, PrecipStation)
            If curStation.HasObservations Then
                curStation.UpdateDB(cnn, AddedRows)
            End If
        Next

        'return result log of the function
        ComputationTime = DateTime.Now.Subtract(StartTime)
        Return LogStr & vbCrLf & _
        "rows added: " & AddedRows & vbCrLf & _
        "Time taken: " & ComputationTime.ToString
    End Function

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
        Dim LogStr As String
        Dim LogStrException As String = vbCrLf & "Executing UpdateObsPrecip2(" _
        & DayOffset.ToString & ")"

        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        'variables for web file access
        Dim UriString As String
        Dim BaseUriString As String = "http://hydro.chmi.cz/hpps/hpps_act_rain.php?KRAJ=&UC_POV=&POB=&ordstr=nvys%20desc&recnum=50"
        Dim WebC As WebClient = WebUtils.CreateWebClient()

        Dim Buf() As Byte
        Dim FileString As String

        'regular expression variables
        'Dim r1 As New Regex("<TD[^>]*><A[^\?]+\?" & _
        '"[^=]+=[^=]+=[^=]+=(?<1>\d+)[^>]*>[^<]+</A>\s*</TD>\s*" & _
        '"<TD[^<]*</TD>\s*" & _
        '"(?:<TD\s+class\s*=\s*[""]?tbl[""]?[^>]*>(?<2>[^<]*)</TD>\s*){8}" & _
        '"<TD\s+class\s*=\s*[""]?tbl1S6[""]?[^>]*>[^<]*</TD>\s*" & _
        '"(?:<TD\s+class\s*=\s*[""]?tbl[""]?[^>]*>(?<2>[^<]*)</TD>\s*){16}", _
        'RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        ' Dim r1 As New Regex("<TD[^>]*><A[^\?]+\?" & _
        '"[^=]+=[^=]+=[^=]+=(?<1>\d+)[^>]*>[^<]+</A>\s*</TD>\s*" & _
        '"<TD[^<]*</TD>\s*" & _
        '"(?:<TD[^>]*>(?<2>[^<]*)</TD>\s*){8}" & _
        '"<TD[^>]*>[^<]*</TD>\s*" & _
        '"(?:<TD[^>]*>(?<2>[^<]*)</TD>\s*){16}", _
        'RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        Dim r1 As New Regex( _
            "<TD[^>]*><A[^\?]+\?[^;]+;seq=(?<1>\d+)[^>]*>[^<]+</A>\s*</TD>\s*" & _
            "<TD[^<]*</TD>\s*" & _
            "(?:<TD[^>]*>(?<2>[^<]*)</TD>\s*){7}" & _
            "<TD[^<]*</TD>\s*" & _
            "(?:<TD[^>]*>(?<2>[^<]*)</TD>\s*){17}", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'Dim r2 As New Regex("<TH[^>]*>(?<1>\d{1,2})\.(?<2>\d{1,2})\.(?<3>\d{4})</TH>\s*" & _
        '"<TD[^>]*>\s*<[^<]+</TD>\s*<TH[^>]*>(?<4>[^<]+)</TH>", _
        'RegexOptions.IgnoreCase Or RegexOptions.Compiled)

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
        Dim TimeZoneStr As String
        Dim ToUTC As Integer
        Dim ObsHour As Integer
        Dim ObsTime As DateTime
        Dim CurStationSeq As Integer
        Dim CurStationId As Integer
        Dim ObsValueStr As String
        Dim CurObsValue As Single
        Dim ValuesCount As Integer

        Dim DbMissingValue As Integer = -1

        'download web file
        'here we need to download multiple files (paging)
        Dim recnum As Integer = 50
        For startPos = 0 To 250 Step recnum

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
                'convert to UTC
                'TimeZoneStr = m.Groups(4).Value.Trim()
                'If TimeZoneStr = "SEC" Then
                '    ToUTC = -1
                'Else
                '    ToUTC = -2
                'End If
                ToUTC = -1

                BeginTime = BeginTime.AddHours(ToUTC)
                EndTime = BeginTime.AddHours(23)
                'changed from 24 to 23 (row contains starting column + 23 values)
            Else
                LogStr &= LogStrException & vbCrLf & " No match for ObsDate!"
            End If

            'read observations
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
                                ObsHour = curStation.MaxDbTime.AddHours(1 - ToUTC).Hour 'CHECK!!!!!
                                '+2 because we add one hour and convert to central European time
                                curStation.StartWebTime = curStation.MaxDbTime.AddHours(1)
                            End If
                        End If

                        ValuesCount = m.Groups(2).Captures.Count

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

    Public Function UpdateHydro() As String

        'log message variables
        Dim LogStr As String = "Executing UpdateHydro.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        'variables for temporary station data storage
        Dim HydroDs As New DataSet
        Dim StTab As DataTable
        Dim StRow As DataRow

        'variables with information about the station
        Dim CurStationId As Integer
        Dim CurStationSeq As Integer
        Dim CurStationQa As Double
        Dim CurStationHa As Integer
        Dim LastWebObsTime As DateTime
        Dim LastDBObsTime As DateTime
        Dim PrevHours As Integer
        Dim ScalingQ_avg As Integer = 100

        'sql database connection variables
        Dim sql_cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim sql_cmd2 As SqlCommand = Me.CreateHydroCommand(sql_cnn)
        Dim UpdatedRows As Integer = 0
        Dim AddedRows As Integer = 0

        'observation data variables
        Dim ObsListHQ As New ArrayList
        Dim CurObsHQ As HydroTimeValueHQ
        Dim CurListIndex As Integer

        'create station table and retrieve station data
        'from the website ('prehled mereni') and database
        LogStr &= Me.GetHydroStationTable(HydroDs)

        If Not HydroDs.Tables.Contains("StTab") Then Exit Function
        StTab = HydroDs.Tables("StTab")

        'process each station - get necessary information
        For Each StRow In StTab.Rows

            LastDBObsTime = DateTime.Now.AddDays(-6)
            If Not TypeOf (StRow("st_id")) Is DBNull Then
                CurStationId = CType(StRow("st_id"), Integer)
                CurStationSeq = CType(StRow("st_seq"), Integer)
                CurStationQa = CType(StRow("q_avg"), Integer) / ScalingQ_avg
                CurStationHa = CType(StRow("h_avg"), Integer)
                LastWebObsTime = CType(StRow("last_obs_time"), DateTime)
                CurObsHQ.Time = LastWebObsTime
                CurObsHQ.H = CType(StRow("last_obs_h"), Double)
                CurObsHQ.Q = CType(StRow("last_obs_q"), Double)
                If Not TypeOf (StRow("latest_obs_time")) Is DBNull Then _
                    LastDBObsTime = CType(StRow("latest_obs_time"), DateTime)
                'PrevHours specify if we need to download an internet file.
                PrevHours = CInt((LastWebObsTime.Subtract(LastDBObsTime)).TotalHours)

                'download observations from the Internet!
                'do this only when new data are available 
                'on the web (LastWebObsTime > LastDBObsTime)

                If PrevHours > 1 Then

                    ObsListHQ.Clear()
                    LogStr = LogStr & Me.DownloadObsHydro(CurStationSeq, LastDBObsTime, _
                    LastWebObsTime, CurStationHa, CurStationQa, ObsListHQ)

                    'update the database with data from obs_list_h,
                    'obs_list_q
                    'main DATABASE UPDATE for a given station
                    '**********************************************
                    'the lists should be already filtered !

                    'update stage and discharge, go through ObsListHQ from
                    'oldest to most recent record
                    'CurListIndex = ObsListHQ.Count - 1

                    'first, always add the most recent observation (from HydroStationTable)
                    If ObsListHQ.Count > 0 Then
                        ObsListHQ(0) = CurObsHQ
                    Else
                        ObsListHQ.Add(CurObsHQ)
                    End If

                    For CurListIndex = ObsListHQ.Count - 1 To 0 Step -1
                        CurObsHQ = CType(ObsListHQ(CurListIndex), HydroTimeValueHQ)
                        LogStr = LogStr & ExecuteHydroCommand( _
                        sql_cmd2, CurStationId, CurObsHQ.Time, CurObsHQ.H, CurObsHQ.Q, _
                        UpdatedRows, AddedRows)
                    Next CurListIndex
                End If
            End If
        Next StRow

        'return result log of the function
        ComputationTime = DateTime.Now.Subtract(StartTime)
        Return LogStr & vbCrLf & "Rows updated: " & UpdatedRows & vbCrLf & _
        "Rows added: " & AddedRows & vbCrLf & _
        "Time taken: " & ComputationTime.ToString
    End Function


    Public Function UpdateHydro_sql() As String
        'log message variables
        Dim LogStr As String = "Executing UpdateHydro.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        'variables for temporary station data storage
        Dim HydroDs As New DataSet
        Dim StTab As DataTable
        Dim StRow As DataRow

        'variables with information about the station
        Dim CurStationId As Integer
        Dim CurStationName As String
        Dim CurStationQa As Double
        Dim CurStationHa As Integer
        Dim PrevDays As Integer
        Dim ScalingQ_avg As Integer = 100

        'sql database connection variables
        Dim sql_cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim sql_cmd2 As SqlCommand = Me.CreateHydroCommand(sql_cnn)
        Dim UpdatedRows As Integer = 0
        Dim AddedRows As Integer = 0

        'observation data variables
        Dim ObsListH As New ArrayList
        Dim ObsListQ As New ArrayList
        Dim ObsListHQ As New ArrayList
        Dim ObsCount As Integer = 0
        Dim hq_list_count As Integer
        Dim CurObs As HydroTimeValue
        Dim CurObsHQ As HydroTimeValueHQ
        Dim CurListIndex As Integer

        'create station table and retrieve data
        LogStr &= Me.GetHydroStationTable_sql(HydroDs)
        If Not HydroDs.Tables.Contains("StTab") Then Exit Function
        StTab = HydroDs.Tables("StTab")

        'process each station - get necessary information
        For Each StRow In StTab.Rows
            If Not TypeOf (StRow("st_id")) Is DBNull Then
                CurStationId = CType(StRow("st_id"), Integer)
                CurStationQa = CType(StRow("q_avg"), Integer) / ScalingQ_avg
                CurStationHa = CType(StRow("h_avg"), Integer)
                CurStationName = CType(StRow("st_name"), String)

                'download observations from the Internet!
                ObsListHQ.Clear()
                Dim Qlimit As Double = 5600 / ScalingQ_avg
                If CurStationQa > Qlimit Then
                    ObsCount = Me.DownloadHydro_SqlServer(CurStationId, CurStationHa, CurStationQa, ObsListHQ)
                End If
                hq_list_count = ObsListHQ.Count

                'update the database with data from obs_list_h,
                'obs_list_q
                'main DATABASE UPDATE for a given station
                '**********************************************
                'the lists should be already filtered !

                'update stage and discharge, go through ObsListHQ from
                'oldest to most recent record
                UpdatedRows = 0
                AddedRows = 0
                For CurListIndex = ObsListHQ.Count - 1 To 0 Step -1
                    CurObsHQ = CType(ObsListHQ(CurListIndex), HydroTimeValueHQ)
                    LogStr = LogStr & ExecuteHydroCommand( _
                    sql_cmd2, CurStationId, CurObsHQ.Time, CurObsHQ.H, CurObsHQ.Q, _
                    UpdatedRows, AddedRows)
                Next CurListIndex
            End If

            'write to output console
            Console.WriteLine("station: " & CurStationName & _
            " added: " & AddedRows.ToString & " updated: " & UpdatedRows.ToString)

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
    'only observations greater than specified H_avg, Q_avg and later than LatestDbObs
    'are added to list
    'PrevDays specifies the time span of downloaded observations
    'Function returns a message in case of error
    Private Function DownloadObsHydro(ByVal st_seq As Integer, _
    ByVal LatestDbObs As DateTime, ByVal LastWebObs As DateTime, ByVal H_avg As Integer, _
    ByVal Q_avg As Double, ByVal ObslistHQ As ArrayList) As String

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
        Dim TmpList As New ArrayList(24)
        Dim DayAvgH As Double = 0
        Dim DayAvgQ As Double = 0
        Dim DayMaxH As Double = 0
        Dim DayMaxQ As Double = 0
        Dim nH As Integer = 0
        Dim nQ As Integer = 0
        Dim ObsDay1 As Integer
        Dim ObsDay2 As Integer
        Dim LastWebObsDay As DateTime = LastWebObs.Date
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
            If Me.IsValidDateStringDMY(CurTimeStr) Then
                Obs.Time = Me.String2DateDMY(CurTimeStr)

                'convert Obs.Time to UT
                Obs.Time = Obs.Time.AddHours(-1)

                If Obs.Time < LastWebObs Then
                    'set values of current observation to zero
                    Obs.H = 0
                    Obs.Q = 0
                    'if the observation is already present in database
                    '(indicated by LatestObsTime), stop adding data to both lists
                    If DateTime.Compare(Obs.Time, LatestDbObs) <= 0 Then Exit While

                    'Add ALL data newer than LatestDbObs to a temporary list!
                    CurValueStr = m.Groups(1).Captures(1).Value
                    If CurValueStr.Length > 0 Then
                        If Char.IsDigit(CurValueStr.Chars(0)) Then
                            Obs.H = Me.String2Single(CurValueStr)
                            DayAvgH = (nH * DayAvgH + Obs.H) / (nH + 1)
                            If Obs.H > DayMaxH Then DayMaxH = Obs.H
                            nH = nH + 1
                        End If
                    End If

                    CurValueStr = m.Groups(1).Captures(2).Value
                    If CurValueStr.Length > 0 Then
                        If Char.IsDigit(CurValueStr.Chars(0)) Then
                            Obs.Q = Me.String2Single(CurValueStr)
                            DayAvgQ = (nQ * DayAvgQ + Obs.Q) / (nQ + 1)
                            If Obs.Q > DayMaxQ Then DayMaxQ = Obs.Q
                            nQ = nQ + 1
                        End If
                    End If
                    TmpList.Add(Obs)
                End If


                'check if new day reached
                ObsDay1 = 0
                ObsDay2 = 0
                If TmpList.Count > 0 Then ObsDay1 = _
                CType(TmpList(TmpList.Count - 1), HydroTimeValueHQ).Time.AddMinutes(-1).Day
                If TmpList.Count > 1 Then ObsDay2 = _
                CType(TmpList(TmpList.Count - 2), HydroTimeValueHQ).Time.Day
                'check daily maximums and averages - add to list!
                If ObsDay1 > 0 And ObsDay2 > 0 And ObsDay1 <> ObsDay2 Then

                    If DayMaxQ < Q_avg Then
                        'in case of below-average flow, save only daily average!
                        Obs.Time = Obs.Time.Date.AddHours(12)
                        Obs.H = DayAvgH
                        Obs.Q = DayAvgQ
                        ObslistHQ.Add(Obs)
                    Else
                        'above-average flow: save all values
                        For Each Obs In TmpList
                            ObslistHQ.Add(Obs)
                        Next
                    End If
                    'reset whole-day values to default
                    nH = 0
                    nQ = 0
                    DayAvgH = 0
                    DayAvgQ = 0
                    DayMaxH = 0
                    DayMaxQ = 0
                    TmpList.Clear()
                End If
            End If

            m = m.NextMatch()
        End While

        'process the remaining current day's observations if they are above average
        'we have to test if this really works!!!!
        If TmpList.Count > 0 And (DayMaxQ > Q_avg Or DayMaxH > H_avg) Then
            For Each Obs In TmpList
                ObslistHQ.Add(Obs)
            Next
        End If

        If ObslistHQ.Count = 0 Then
            LogStr = String.Format("DownloadObsHydro: zero observations read ({0})", UriString)
        End If

        Return LogStr
    End Function

    'auxiliary function, used to transfer data from old database tables
    '(space reduction - save only daily averages for below-average flows)
    Private Function DownloadHydro_SqlServer(ByVal st_id As Integer, _
        ByVal H_avg As Integer, _
        ByVal Q_avg As Double, ByVal ObslistHQ As ArrayList) As Integer

        'variables
        Dim Obs As HydroTimeValueHQ
        Dim ObsCount As Integer = 0

        Dim ScalingQ As Integer = 100
        Dim ScalingH As Integer = 10

        'temporary observation lists
        Dim TmpList As New ArrayList(24)
        Dim DayAvgH As Double = 0
        Dim DayAvgQ As Double = 0
        Dim DayMaxH As Double = 0
        Dim DayMaxQ As Double = 0
        Dim nH As Integer = 0
        Dim nQ As Integer = 0
        Dim ObsDay1 As Integer
        Dim ObsDay2 As Integer
        Dim LastWebObs As DateTime = DateTime.Now.AddDays(-1)
        Dim LastWebObsDay As DateTime = LastWebObs.Date
        Dim LogStr As String

        'initialize array list
        ObslistHQ.Clear()


        'get results: H for the station
        Dim cnn As New SqlConnection(SqlDbConnectionString)
        Dim cmd As SqlCommand = CreateHydroSqlCommand(cnn)
        Dim ds As New DataSet

        'variables to extract discharge
        Dim cmd2 As SqlCommand = CreateHydroSqlCommand2(cnn)
        Dim ds2 As New DataSet
        Dim dr2 As DataRow

        ExecuteHydroSqlCommand(cmd, st_id, 4, ds)
        Dim dr As DataRow

        For Each dr In ds.Tables(0).Rows

            Obs.Time = CType(dr("obs_time"), DateTime)

            'only add data to list, when data for the whole day is available on the web
            If DateTime.Compare(Obs.Time, LastWebObsDay) < 0 Then
                Obs.H = CType(dr("obs_value"), Integer)

                DayAvgH = (nH * DayAvgH + Obs.H) / (nH + 1)
                If Obs.H > DayMaxH Then DayMaxH = Obs.H
                nH = nH + 1
                Obs.Q = 0
                TmpList.Add(Obs)
            End If

            'check if new day reached
            ObsDay1 = 0
            ObsDay2 = 0
            If TmpList.Count > 0 Then ObsDay1 = _
            CType(TmpList(TmpList.Count - 1), HydroTimeValueHQ).Time.AddMinutes(-1).Day
            If TmpList.Count > 1 Then ObsDay2 = _
            CType(TmpList(TmpList.Count - 2), HydroTimeValueHQ).Time.Day
            'check daily maximums and averages - add to list!
            If ObsDay1 > 0 And ObsDay2 > 0 And ObsDay1 <> ObsDay2 Then
                If DayMaxH < H_avg Then
                    Obs.Time = Obs.Time.Date.AddHours(12)
                    Obs.H = CInt(Math.Round(DayAvgH * ScalingH))

                    'retrieve discharge from database!
                    cmd2.Parameters("@st_id").Value = st_id
                    cmd2.Parameters("@var_id").Value = 5
                    cmd2.Parameters("@date").Value = Obs.Time
                    cmd2.Parameters("@iavg").Value = 1
                    Try
                        cnn.Open()
                        Obs.Q = CType(cmd2.ExecuteScalar(), Integer) / 100 / ScalingQ
                    Catch ex As Exception
                        LogStr &= ex.Message
                    Finally
                        If Not cnn.State = ConnectionState.Closed Then cnn.Close()
                    End Try


                    ObslistHQ.Add(Obs)
                    ObsCount = ObsCount + 1
                Else
                    For Each Obs In TmpList

                        'retrieve discharge from database!
                        cmd2.Parameters("@st_id").Value = st_id
                        cmd2.Parameters("@var_id").Value = 5
                        cmd2.Parameters("@date").Value = Obs.Time
                        cmd2.Parameters("@iavg").Value = 0
                        Try
                            cnn.Open()
                            Obs.H = Obs.H * ScalingH
                            Obs.Q = 0
                            If Not TypeOf (cmd2.ExecuteScalar) Is DBNull Then
                                Obs.Q = CType(cmd2.ExecuteScalar(), Integer) / ScalingQ
                            End If
                        Catch ex As Exception
                            LogStr &= ex.Message
                        Finally
                            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
                        End Try

                        ObslistHQ.Add(Obs)
                        ObsCount = ObsCount + 1
                    Next
                End If
                nH = 0
                nQ = 0
                DayAvgH = 0
                DayAvgQ = 0
                DayMaxH = 0
                DayMaxQ = 0
                TmpList.Clear()
            End If
        Next dr
    End Function

    '-----------------------------------------------------------------------------------------------
    '********************** WATER LEVEL AND FLOW: POVODI (www.voda.gov.cz)    **********************
    '-----------------------------------------------------------------------------------------------
    'public method
    Public Function UpdateHydro_povodi() As String

        'log message variables
        Dim LogStr As String = "Executing UpdateHydro_povodi.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan
        Dim UpdatedRows As Integer = 0
        Dim AddedRows As Integer = 0

        'observation data variables
        Dim ObsListHQ As New List(Of HydroTimeValueHQ)
        Dim ObsCount As Integer = 0
        Dim CurObsHQ As HydroTimeValueHQ
        Dim CurListIndex As Integer

        'observation time and value and location variables
        Dim ObsTime As DateTime
        Dim ObsValue As Integer
        Dim ScaleFactor As Integer = 10
        Dim ScalingQavg As Integer = 100
        Dim Seq As Integer
        Dim Name2 As String
        Dim Id As Integer = 0
        Dim Q_avg As Double = 0
        Dim H_avg As Integer = 0
        Dim LatestObs As DateTime = DateTime.Now.AddDays(-6)

        'SQL database variables
        Dim HydroDs As New DataSet
        Dim StTab As DataTable
        Dim StRow As DataRow
        Dim cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim StationCmd As SqlCommand = Me.CreateStationIdCommand(cnn)
        Dim ObsCmd As SqlCommand = Me.CreateHydroCommand(cnn)
        Dim Q_avgCmd As SqlCommand = Me.CreateLongrecordValue_command(cnn)

        Dim dbg As String

        'get table of limnigraphic stations to update
        LogStr &= Me.GetHydroStationTable_povodi(HydroDs)
        If Not HydroDs.Tables.Contains("StTab") Then Exit Function
        StTab = HydroDs.Tables("StTab")
        For Each StRow In StTab.Rows
            LatestObs = DateTime.Now.AddDays(-6)
            If Not TypeOf (StRow("st_id")) Is DBNull Then Id = CType(StRow("st_id"), Integer)
            If Not TypeOf (StRow("st_name2")) Is DBNull Then Name2 = CType(StRow("st_name2"), String)
            If Not TypeOf (StRow("h_avg")) Is DBNull Then H_avg = CType(StRow("h_avg"), Integer)
            If Not TypeOf (StRow("latest_obs")) Is DBNull Then LatestObs = CType(StRow("latest_obs"), DateTime)

            'debug info
            If Id = 905 Then
                Id = 905
            End If

            'get average discharge for station
            Try
                Q_avg = Me.GetLongrecordValue(Q_avgCmd, cnn, Id, "discharge_avg") / ScalingQavg
                ObsCount = Me.DownloadHydro_povodi(Id, Name2, LatestObs, H_avg, Q_avg, ObsListHQ)

                'update the database with data from obs_list_h,
                'obs_list_q
                'main DATABASE UPDATE for a given station
                '**********************************************
                'the lists should be already filtered !

                'update stage and discharge, go through ObsListHQ from
                'oldest to most recent record
                If ObsListHQ.Count > 0 Then
                    For Each hq As HydroTimeValueHQ In ObsListHQ
                        LogStr = LogStr & ExecuteHydroCommand( _
                       ObsCmd, Id, hq.Time, hq.H, hq.Q, _
                       UpdatedRows, AddedRows)
                    Next
                    For CurListIndex = 0 To ObsListHQ.Count - 1
                        CurObsHQ = CType(ObsListHQ(CurListIndex), HydroTimeValueHQ)

                    Next CurListIndex
                Else
                    LogStr &= vbCrLf & "UpdateHydro_povodi:zero records in ObsListHQ (station " & Id.ToString & ")"
                End If
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

    'auxiliary function: creates a table of hydrologic stations for Povodi which are currently
    'measuring. Adds Qa, Ha, last H measured, last Q measured and last time of
    'measuring to each station in the table
    'PARAMETERS: HydroDs, the DataSet where table is created (empty DataSet)
    'CALLED BY:  UpdateHydro_povodi()

    Private Function GetHydroStationTable_povodi(ByVal HydroDs As DataSet) As String
        'EXPLANATION !!!:
        'LastObsTime: Time in the table 'POSLEDNI MERENI'
        'it is the time of most recent observation for a given
        'station available on the Internet
        'LatestObsTime: Time of most recent observation
        'already stored in the database

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
        sql_cmd.CommandText = "query_stationshydro_povodi"
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
    'finds the value of longrecord (for example q_avg), given the 
    'station id and longrecord type name (for example 'discharge_avg')
    Private Function GetLongrecordValue(ByVal cmd As SqlCommand, ByVal cnn As SqlConnection, _
    ByVal st_id As Integer, ByVal longrec_name As String) As Integer
        Dim result As Integer
        Dim ExStr As String
        cmd.Parameters("@longrectype_name").Value = longrec_name
        cmd.Parameters("@station_id").Value = st_id
        Try
            cnn.Open()
            result = CType(cmd.ExecuteScalar(), Integer)
            cnn.Close()
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
            ExStr = ex.Message
            result = -1
        End Try
        Return result

    End Function

    'auxiliary function
    'called by [UpdateHydro_povodi()]
    'used by [GetLongrecordValue()]
    'creates the command object used to get longrecord value for a given station
    Private Function CreateLongrecordValue_command(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Connection = cnn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "query_longrecordbystation"
        cmd.Parameters.Add(New SqlParameter("@longrectype_name", SqlDbType.VarChar))
        cmd.Parameters.Add(New SqlParameter("@station_id", SqlDbType.SmallInt))
        Return cmd
    End Function

    'auxiliary function
    'called by [UpdateHydro_povodi()]
    'downloads observation data for a given station from the Internet
    'and stores the stage and discharge observations in ArrayLists
    'ObsListH, ObsListQ
    'only observations greater than specified H_avg, Q_avg are added to list
    'LatestObs specifies the date, which is already in the database and is not 
    'added to ObsListH or ObsListQ
    'Function returns the number of records added to both lists
    Private Function DownloadHydro_povodi(ByVal st_id As Integer, _
    ByVal st_name2 As String, ByVal LatestObs As DateTime, _
    ByVal H_avg As Integer, ByVal Q_avg As Double, _
    ByVal ObsListHQ As List(Of HydroTimeValueHQ)) As Integer

        'clear the list
        ObsListHQ.Clear()

        'variables for web file manipulation
        Dim Povodi As String = st_name2.Substring(0, 3)
        Dim UriStringBase As String = ""
        Dim UriString As String
        Dim WebC As WebClient = WebUtils.CreateWebClient()
        Dim Buf() As Byte
        Dim FileString As String

        'regular expression variables
        Dim r1, r2, r3, r4, r5 As Regex
        Dim m1, m2, m3 As Match
        Dim CurStr As String
        Dim CurTimeStr As String
        Dim CurValueStr As String
        Dim CurValueStrH As String
        Dim CurValueStrQ As String

        'observation data variables
        Dim CurTime As DateTime = DateTime.Now.AddHours(-1)
        Dim ObsYear As Integer = CurTime.Year
        Dim CurMonth As Integer = CurTime.Month
        'Dim ScalingQ As Integer = 100
        'Dim ScalingH As Integer = 10
        'Dim Obs As HydroTimeValue
        Dim ObsTime As DateTime

        'observation value
        Dim ObsValueH As Single
        Dim ObsValueQ As Single

        Dim ObsHour As Integer
        Dim ObsCount As Integer = 0
        Dim VariableId As Integer = 4

        Dim DayAvgH As Double = 0
        Dim DayAvgQ As Double = 0
        Dim DayMaxH As Double = 0
        Dim DayMaxQ As Double = 0
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

        'get www base address
        UriStringBase = "http://www." & Povodi & ".cz/portal/sap/en/"

        'open www file
        Try


            UriString = UriStringBase & "mereni_" & st_name2.Substring(4).ToUpper() & ".htm"

            If Povodi = "pla" Then
                Dim indexOf = st_name2.LastIndexOf("_")
                Dim len As Integer = indexOf - 4
                UriString = UriStringBase & String.Format("PC/Mereni.aspx?id={0}&oid={1}", st_name2.Substring(4, len), st_name2.Substring(indexOf + 1))
            End If

            'read html file and save it to a string
            Buf = WebC.DownloadData(UriString)
            FileString = System.Text.Encoding.GetEncoding(1250).GetString(Buf)
        Catch ex As Exception
            LogStr = ex.Message
            'exits the sub and returns empty observation lists
            'and returns ObsCount = 0
            Return ObsCount
        End Try

        'r1 = New Regex("<tr>\s*<td[^>]*>\s*<font[^>]+>(?<1>[^<]+)" & _
        '"(?:<sup>[^<]+</sup>[^<]+)*</font></td></tr>\s*" & _
        '"(?:<tr[^>]*>\s*" & _
        '"<td[^>]*>\s*<font[^>]*>[^<]+</font>\s*</td>\s*" & _
        '"<td[^>]*>\s*<font[^>]*>[^<]+</font>\s*</td>\s*</tr>\s*)+", _
        'RegexOptions.IgnoreCase Or RegexOptions.Compiled) 

        'r1 = New Regex("<tr>\s*<td[^>]*>\s*<font[^>]+>\s*" & _
        '"(?<1>(water level H|Discharge Q))\s*" & _
        '"\[[^\]]+\]\:\s*" & _
        '"(<span[^>]*><sup>[^<]*</sup></span>\s*)?" & _
        '"</font></td></tr>\s*" & _
        '"(?:<tr[^>]*>\s*" & _
        '"<td[^>]*>\s*<font[^>]*>[^<]+</font>\s*</td>\s*" & _
        '"<td[^>]*>\s*<font[^>]*>[^<]+</font>\s*</td>\s*</tr>\s*)+", _
        'RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'r2 = New Regex("<tr[^>]*>\s*" & _
        '"<td[^>]*>\s*<font[^>]*>(?<1>[^<]+)</font>\s*</td>\s*" & _
        '"<td[^>]*>\s*<font[^>]*>(?<2>[\d\.,]+)[^<]+</font>\s*</td>\s*</tr>", _
        '                RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        r3 = New Regex("(?<1>\d+)\.(?<2>\d+)\.(?<3>\d+)\D+(?<4>\d+)\D(?<5>\d+)", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'new regeX added by JK
        If Povodi = "pla" Then

            r4 = New Regex("<td class=""bunkaGridu""[^>]*>\s*" & _
                       "<span>(?<1>[^<]+)</span>\s*</td>\s*" & _
            "<td class=""bunkaGriduBold""[^>]*>(?<2>[^<]+)</td>\s*" & _
            "<td class=""bunkaGriduBold""[^>]*>\s*<span>(?<3>[^<]+)</span>",
            RegexOptions.IgnoreCase Or RegexOptions.Compiled)
        Else
            r4 = New Regex("<tr>\s*" & _
                           "<td align=right nowrap><font[^>]*>(?<1>[^<]+)</font></td>\s*" & _
                           "<td[^>]*><font[^>]*>(?<2>[^<]+)</font></td>\s*" & _
                           "<td[^>]*><font[^>]*>(?<3>[^<]+)</font></td>\s*")
        End If


        'm1 = r1.Match(FileString)
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
            If ObsTime <= LatestObs Then Exit While

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
            ObsListHQ1.Add(ObsHQ)

            'next match
            m1 = m1.NextMatch()
        End While

        'reverse the observations list
        ObsListHQ.Clear()
        For i As Integer = ObsListHQ1.Count - 1 To 0 Step -1
            ObsListHQ.Add(ObsListHQ1(i))
        Next

        ''get observation value (H, Q)
        'If VariableId = 4 Then
        '    Obs.Value = Me.String2Single(CurValueStr)
        '    ObsHour = Obs.Time.Hour
        '    ObsListH.Add(Obs)
        '    If (iaH = True) Then
        '        DayAvgH = (nH * DayAvgH + Obs.Value) / (nH + 1)
        '        If Obs.Value > DayMaxH Then DayMaxH = Obs.Value
        '        nH = nH + 1
        '        ObsCount = ObsCount + 1
        '    End If
        '    If ObsHour = 23 Then iaH = False
        'Else
        '    Obs.Value = Me.String2Single(CurValueStr)
        '    ObsHour = Obs.Time.Hour
        '    ObsListQ.Add(Obs)
        '    If (iaQ = True) Then
        '        DayAvgQ = (nQ * DayAvgQ + Obs.Value) / (nQ + 1)
        '        If Obs.Value > DayMaxQ Then DayMaxQ = Obs.Value
        '        nQ = nQ + 1
        '        ObsCount = ObsCount + 1
        '    End If
        '    If ObsHour = 23 Then
        '        iaQ = False
        '    End If
        'End If







        ''find out the variable (H or Q)
        'CurStr = m1.Value
        'CurValueStr = m1.Groups(1).Value
        'If CurValueStr.IndexOf("Q") >= 0 Then VariableId = 5
        'm2 = r2.Match(CurStr)
        'While m2.Success
        '    CurValueStr = m2.Value

        '    'get observation time (H, Q)
        '    CurTimeStr = m2.Groups(1).Value
        '    m3 = r3.Match(CurTimeStr)
        '    If m3.Success Then
        '        CurTimeStr = m3.Value
        '        Obs.Time = New DateTime(Integer.Parse(m3.Groups(3).Value) + 2000, _
        '        Integer.Parse(m3.Groups(2).Value), Integer.Parse(m3.Groups(1).Value), _
        '        Integer.Parse(m3.Groups(4).Value), 0, 0)
        '        'convert Obs.Time to UT
        '        Obs.Time = Obs.Time.AddHours(-1)
        '        If Obs.Time <= LatestObs Then Exit While
        '    End If
        '    CurValueStr = m2.Groups(2).Value

        '    'get observation value (H, Q)
        '    If VariableId = 4 Then
        '        Obs.Value = Me.String2Single(CurValueStr)
        '        ObsHour = Obs.Time.Hour
        '        ObsListH.Add(Obs)
        '        If (iaH = True) Then
        '            DayAvgH = (nH * DayAvgH + Obs.Value) / (nH + 1)
        '            If Obs.Value > DayMaxH Then DayMaxH = Obs.Value
        '            nH = nH + 1
        '            ObsCount = ObsCount + 1
        '        End If
        '        If ObsHour = 23 Then iaH = False
        '    Else
        '        Obs.Value = Me.String2Single(CurValueStr)
        '        ObsHour = Obs.Time.Hour
        '        ObsListQ.Add(Obs)
        '        If (iaQ = True) Then
        '            DayAvgQ = (nQ * DayAvgQ + Obs.Value) / (nQ + 1)
        '            If Obs.Value > DayMaxQ Then DayMaxQ = Obs.Value
        '            nQ = nQ + 1
        '            ObsCount = ObsCount + 1
        '        End If
        '        If ObsHour = 23 Then
        '            iaQ = False
        '        End If
        '    End If

        '    m2 = m2.NextMatch()
        'End While

        'm1 = m1.NextMatch()
        'End While

        ''determine if whole list or only the average is saved
        'If ObsListQ.Count > 0 Then
        '    If DayMaxQ < Q_avg Then
        '        ObsHQ.Time = CType(ObsListQ(0), HydroTimeValue).Time.Date.AddHours(12)
        '        ObsHQ.H = DayAvgH
        '        ObsHQ.Q = DayAvgQ
        '        ObsListHQ.Add(ObsHQ)
        '        If nQ < ObsListQ.Count Then
        '            last_ext_i = ObsListQ.Count - 1
        '            For ext_i = nQ To last_ext_i
        '                ObsHQ.Time = CType(ObsListQ(ext_i), HydroTimeValue).Time
        '                ObsHQ.H = CType(ObsListH(ext_i), HydroTimeValue).Value
        '                ObsHQ.Q = CType(ObsListQ(ext_i), HydroTimeValue).Value
        '                ObsListHQ.Add(ObsHQ)
        '            Next
        '        End If
        '    Else
        '        ext_i = 0
        '        last_ext_i = Math.Min(ObsListQ.Count, ObsListH.Count) - 1
        '        For ext_i = 0 To last_ext_i
        '            ObsHQ.Time = CType(ObsListQ(ext_i), HydroTimeValue).Time
        '            ObsHQ.H = CType(ObsListH(ext_i), HydroTimeValue).Value
        '            ObsHQ.Q = CType(ObsListQ(ext_i), HydroTimeValue).Value
        '            ObsListHQ.Add(ObsHQ)
        '        Next
        '    End If

        'Else
        'only water level (stage) is displayed on the internet
        'in this case, only the stages are added to database and
        'discharge field is left blank (null)
        '    If DayMaxH < H_avg And ObsListH.Count > 0 Then
        '        ObsHQ.Time = CType(ObsListH(0), HydroTimeValue).Time.Date.AddHours(12)
        '        ObsHQ.H = DayAvgH
        '        ObsHQ.Q = 0
        '        ObsListHQ.Add(ObsHQ)
        '        If nH < ObsListH.Count Then
        '            last_ext_i = ObsListH.Count - 1
        '            For ext_i = nH To last_ext_i
        '                ObsHQ.Time = CType(ObsListH(ext_i), HydroTimeValue).Time
        '                ObsHQ.H = CType(ObsListH(ext_i), HydroTimeValue).Value
        '                ObsHQ.Q = 0
        '                ObsListHQ.Add(ObsHQ)
        '            Next
        '        End If
        '    Else
        '        ext_i = 0
        '        last_ext_i = Math.Min(ObsListH.Count, ObsListH.Count) - 1
        '        For ext_i = 0 To last_ext_i
        '            ObsHQ.Time = CType(ObsListH(ext_i), HydroTimeValue).Time
        '            ObsHQ.H = CType(ObsListH(ext_i), HydroTimeValue).Value
        '            ObsHQ.Q = 0
        '            ObsListHQ.Add(ObsHQ)
        '        Next
        '    End If
        'End If

        Return ObsListHQ.Count
    End Function


    '-----------------------------------------------------------------------------------'
    '----------- SNOW: CHMU ------------------------------------------------------------'
    '-----------------------------------------------------------------------------------'
    'function for updating: snow
    'new version, works with Regex and observations2 table
    'added 16.02.2006
    'snow from chmi (text) and chmi snow map (map, text)
    'snow from povodi Ohre
    Public Function UpdateObsSnow2() As String

        'log message variables
        Dim LogStr As String = "Executing UpdateObsSnow2.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        'variables for creating snow station table
        Dim cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim SnowStationCmd1 As SqlCommand = Me.CreateStationIdCommand(cnn)
        Dim SnowStationCmd2 As SqlCommand = Me.CreateSynopStationCommand(cnn)
        Dim ObsCmd As SqlCommand = Me.CreateObservationCommand2(cnn)
        Dim ZeroStationList As New ArrayList(20)

        'variables for reading internet data files
        Dim SnowUriChmi01 As String = Me.ViewConfiguration("SnowUri01")
        Dim SnowUriChmi02 As String = Me.ViewConfiguration("SnowUri02")
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
        Dim CoordTopStr As String
        Dim CoordLeftStr As String

        'observation variables
        Dim ObsTime As DateTime
        Dim ObsValue As Integer
        Dim NoDataValue As Integer = -9999
        Dim CurStationId As Integer
        Dim CurStationCoord As Integer
        Dim NumUpdatedRows As Integer = 0
        Dim NumAddedRows As Integer = 0

        '********************************************************************************
        '(1) UPDATE POVODI OHRE
        'read povodi ohre web file
        '********************************************************************************
        If DateTime.Now.Month > 10 Or DateTime.Now.Month < 5 Then
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
                    LogStr &= Me.ExecuteObservationCommand2(ObsCmd, CurStationId, _
                    "snow", ObsTime, ObsValue, NumUpdatedRows, NumAddedRows)
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
        UpdateSynop_24h("SnowUri02", "snow", LogStr, NumUpdatedRows, NumAddedRows)

        '*********************************************************************
        '(2) UPDATE CHMI HILL STATIONS (snow table)
        '*********************************************************************
        If DateTime.Now.Month > 10 Or DateTime.Now.Month < 5 Then
            'regex for reading snow data
            r1 = New Regex("(?<1>\D{19})\s?\d+\s{5}.{3}\s{3}(?<2>.{3})", _
            RegexOptions.IgnoreCase Or RegexOptions.Compiled)
            'regex for reading observation time
            r2 = New Regex("ipravena\s+(?<1>\d{2})\.(?<2>\d{2})\.(?<3>\d{4})", _
            RegexOptions.IgnoreCase Or RegexOptions.Compiled)

            'read chmi snow web file
            Try
                'buf = WebC.DownloadData("http://localhost/hydroweb/snow/sncz20060217.htm")
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
                ObsTime = New DateTime(Integer.Parse(m.Groups(3).Value), _
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
                    LogStr &= Me.ExecuteObservationCommand2(ObsCmd, CurStationId, _
                    "snow", ObsTime, ObsValue, NumUpdatedRows, NumAddedRows)
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
    'auxiliary function: get list of synoptic lowland stations that
    'have no records of the variable for a given date
    'RETURNS: StList (list of station_id values, ByRef)
    '         LogStr (string with error messages)
    'INPUT: SnowDate, the actual date
    ' VariableId .. Id of the observed variable (eg. 8 for snow)
    'CALLED BY: UpdateObsSnow2()
    Private Function GetZeroStations(ByVal ZeroDate As DateTime, _
    ByVal VariableId As Integer, ByVal StList As ArrayList) As String
        Dim LogStr As String
        Dim cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim cmd As New SqlCommand("query_zero_snow", cnn)
        Dim r As SqlDataReader
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters.Add(New SqlParameter("@date", SqlDbType.SmallDateTime))
        cmd.Parameters.Add(New SqlParameter("@var_id", SqlDbType.TinyInt))
        cmd.Parameters("@date").Value = ZeroDate
        cmd.Parameters("@var_id").Value = VariableId
        If StList.Count > 0 Then StList.Clear()
        Try
            cnn.Open()
            r = cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While r.Read()
                StList.Add(r.GetInt16(0))
            End While
            r.Close()
        Catch ex As Exception
            LogStr = vbCrLf & "Executing GetZeroStations: " & ex.Message
        End Try
        Return LogStr
    End Function
    'auxiliary function: view station id, when given an attribute
    'INPUT: Attrib: the table field, can have values:
    '       st_name2, st_name, st_seq, location_id, st_ind, st_uri
    '       Value: string value of the attribute
    'RETURNS: st_id if successful, -1 if no such station exists
    'CALLED BY: UpdateObsSnow2(), UpdatePrecip_povodi()
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
        cmd.CommandText = "query_stationid"
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

    Private Function CreateStationIdForSnowCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Connection = cnn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "query_stationid_snow"
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
        cmd.CommandText = "query_stationbycoord"
        cmd.Parameters.Add(New SqlParameter("@st_coord_topleft", SqlDbType.Int))
        cmd.Parameters.Add(New SqlParameter("@coord_type", SqlDbType.TinyInt))
        Return cmd
    End Function


    'auxiliary function: transfer snow data from SQL server to 
    'the new observations2 table
    Public Function UpdateObsSnow_SqlServer() As String

        'log message variables
        Dim LogStr As String = "Executing UpdateObsSnow_SqlServer.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        'variables for connection to the original database
        Dim sql_cnn As SqlConnection
        Dim sql_cmd As SqlCommand
        Dim sql_reader As SqlDataReader
        Dim sql_cmd_text As String
        'Dim ConnStr3 As String = _
        '"Integrated Security=SSPI;Persist Security Info=False; " & _
        '"Database=plaveninycz;Data Source=(local);user=sa; password=+chnbz+"
        Dim Connstr3 As String = Me.SqlDbConnectionString

        'variables for the update command
        Dim sql_cnn2 As New SqlConnection(Me.SqlDbConnectionString)
        Dim sql_cmd2 As SqlCommand

        'observation data variables
        Dim CurSnowValue As Integer = 0
        Dim CurTime As DateTime
        Dim CurStationId As Integer
        Dim CurVariableId As Integer
        Dim AddedRows As Integer = 0
        Dim UpdatedRows As Integer = 0

        sql_cmd_text = _
        "SELECT variable_id, station_id, obs_time, obs_value " & _
        "FROM observations " & _
        "WHERE variable_id = 8 " & _
        "ORDER BY station_id, obs_time"
        sql_cnn = New SqlConnection(Connstr3)
        sql_cmd = New SqlCommand(sql_cmd_text, sql_cnn)

        sql_cmd2 = Me.CreateObservationCommand2(sql_cnn2)

        Try
            sql_cnn.Open()
            sql_reader = sql_cmd.ExecuteReader
            While sql_reader.Read()
                CurStationId = CType(sql_reader("station_id"), Integer)
                CurVariableId = CType(sql_reader("variable_id"), Integer)
                CurTime = CType(sql_reader("obs_time"), DateTime)
                CurSnowValue = CType(sql_reader("obs_value"), Integer)

                Me.ExecuteObservationCommand2(sql_cmd2, CurStationId, _
                "snow", CurTime, CurSnowValue, UpdatedRows, AddedRows)
            End While
        Catch ex As Exception
            LogStr &= " " & ex.Message
        Finally
            If Not sql_reader Is Nothing Then sql_reader.Close()
            If Not sql_cnn.State = ConnectionState.Closed Then sql_cnn.Close()
        End Try

        'return result log of the function
        ComputationTime = DateTime.Now.Subtract(StartTime)
        Return LogStr & vbCrLf & "snow updated: " & UpdatedRows & vbCrLf & _
        "snow added: " & AddedRows & vbCrLf & _
        "Time taken: " & ComputationTime.ToString
    End Function

    'auxiliary function: creates a table of CHMI or POVODI precipitation stations which should
    'have hourly observations
    'PARAMETERS: onlyCHMI: set to true for table of CHMI stations, set to false for POVODI stations
    Private Function GetPrecipStationTable(ByVal stationTable As Hashtable, ByVal onlyCHMI As Boolean) As String
        'private variables
        Dim LogStr As String = ""

        'variables for sql database access and data retrieval
        'data for connection to SQL server
        Dim sql_cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim sql_cmd As New SqlCommand
        Dim sql_reader As SqlDataReader
        Dim curStationId As Integer
        Dim curStationName As String
        Dim curStationSeq As Integer
        Dim MaxDbTime As DateTime

        'Dim curStation As PrecipStation

        'access to the database!
        'set sql command parameters
        sql_cmd.Connection = sql_cnn
        sql_cmd.CommandType = CommandType.StoredProcedure
        sql_cmd.CommandText = "query_precipstationtable"
        sql_cmd.Parameters.Add(New SqlParameter("@only_chmu", SqlDbType.TinyInt))
        If onlyCHMI Then
            sql_cmd.Parameters("@only_chmu").Value = 1
        Else
            sql_cmd.Parameters("@only_chmu").Value = 0
        End If

        stationTable.Clear()
        Try
            sql_cnn.Open()
            sql_reader = sql_cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sql_reader.Read()
                curStationId = CType(sql_reader("st_id"), Integer)
                curStationSeq = CType(sql_reader("st_seq"), Integer)
                curStationName = CType(sql_reader("st_name"), String)
                If Not TypeOf (sql_reader("max_obs_time")) Is DBNull Then
                    MaxDbTime = CType(sql_reader("max_obs_time"), DateTime)
                Else
                    MaxDbTime = DateTime.Now.Date.AddDays(-8)
                End If

                stationTable.Add(curStationSeq, _
                    New PrecipStation(curStationId, curStationSeq, _
                    curStationName, MaxDbTime))
            End While

        Catch ex As Exception
            LogStr &= vbCrLf & ex.Message
        Finally
            sql_reader.Close()
        End Try
        Return LogStr

    End Function




    'auxiliary function: creates a table of hydrologic stations which are currently
    'measuring. Adds Qa, Ha, last H measured, last Q measured and last time of
    'measuring to each station in the table
    'PARAMETERS: HydroDs, the DataSet where table is created
    'CALLED BY:  UpdateObsHydro2()
    Private Function GetHydroStationTable(ByVal HydroDs As DataSet) As String

        'variables for internet data download
        Dim LogStr As String = ""

        Dim WebC As WebClient = WebUtils.CreateWebClient()

        Dim CurTime As DateTime = DateTime.Now.AddHours(-1)
        Dim ObsYear As Integer = CurTime.Year
        Dim CurMonth As Integer = CurTime.Month

        Dim CurStationSeq As Integer
        Dim IsMeasuringStation As Boolean = False

        'Dim HydroDs As New DataSet
        Dim StTab As New DataTable
        Dim StRow As DataRow
        Dim DbMissingValue As Integer = -9999

        'variables for sql database access and data retrieval
        'data for connection to SQL server
        Dim sql_cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim sql_cmd As New SqlCommand
        Dim sql_reader As SqlDataReader = Nothing
        Dim CurStationId As Integer
        Dim cur_h_avg As Integer
        Dim cur_q_avg As Integer
        Dim latest_obs_time As DateTime

        'create a new table
        StTab.TableName = "StTab"
        StTab.Columns.Add(New DataColumn("st_id", GetType(Integer)))
        StTab.Columns.Add(New DataColumn("st_seq", GetType(Integer)))
        StTab.Columns.Add(New DataColumn("last_obs_time", GetType(DateTime)))
        StTab.Columns.Add(New DataColumn("last_obs_h", GetType(Integer)))
        StTab.Columns.Add(New DataColumn("last_obs_q", GetType(Double)))
        StTab.Columns.Add(New DataColumn("h_avg", GetType(Integer)))
        StTab.Columns.Add(New DataColumn("q_avg", GetType(Integer)))
        StTab.Columns.Add(New DataColumn("latest_obs_time", GetType(DateTime)))

        If Not HydroDs.Tables.Contains("HydroStationTable") Then
            HydroDs.Tables.Add(StTab)
        End If

        'For-Loop: Read the table for each page
        Dim startPos As Integer = 0
        Dim recnum As Integer = 50

        'each page has 50 records. For each page, download table from web and add it to the DataTable.
        For startPos = 0 To 300 Step 50
            LogStr &= vbCrLf & GetHydroStationTablePage(startPos, recnum, StTab)
        Next startPos

        'set sql command parameters
        sql_cmd.Connection = sql_cnn
        sql_cmd.CommandType = CommandType.StoredProcedure
        sql_cmd.CommandText = "query_stationhydro2"
        sql_cmd.Parameters.Add(New SqlParameter("@st_seq", SqlDbType.Int))

        'add other data from database for each station row
        For Each StRow In StTab.Rows
            CurStationSeq = CType(StRow("st_seq"), Integer)

            'debug-DECIN
            If CurStationSeq = 2497644 Then
                CurStationSeq = 2497644
            End If


            sql_cmd.Parameters("@st_seq").Value = CurStationSeq
            Try
                sql_cnn.Open()
                sql_reader = sql_cmd.ExecuteReader(CommandBehavior.CloseConnection)
                While sql_reader.Read()
                    'default value for latest_obs_time, used in case of no database entry
                    latest_obs_time = DateTime.Now.AddDays(-6)
                    CurStationId = CType(sql_reader("st_id"), Integer)
                    If Not TypeOf (sql_reader("latest_obs_time")) Is DBNull Then _
                    latest_obs_time = CType(sql_reader("latest_obs_time"), DateTime)

                    'latest_obs_time = CType(sql_reader("latest_obs_time"), DateTime)
                    If CType(sql_reader("longrectype_name"), String) = "stage_avg" Then
                        cur_h_avg = CType(sql_reader("disch_stage_avg"), Integer)
                    Else
                        cur_q_avg = CType(sql_reader("disch_stage_avg"), Integer)
                    End If
                    StRow("st_id") = CurStationId
                    StRow("h_avg") = cur_h_avg
                    StRow("q_avg") = cur_q_avg
                    StRow("latest_obs_time") = latest_obs_time

                End While
            Catch ex As Exception
                LogStr &= " " & ex.Message
            Finally
                If Not sql_reader Is Nothing Then sql_reader.Close()
                If Not sql_cnn.State = ConnectionState.Closed Then sql_cnn.Close()
            End Try
        Next StRow
        Return LogStr
    End Function


    Private Function GetHydroStationTablePage(ByVal startPos As Integer, ByVal recnum As Integer, ByVal StTab As DataTable) As String

        Dim CurTime As DateTime = DateTime.Now.AddHours(-1)
        Dim ObsYear As Integer = CurTime.Year
        Dim CurMonth As Integer = CurTime.Month

        Dim HydroStationUri As String
        Dim LastObsH As Double
        Dim LastObsTime As DateTime
        Dim LastObsQ As Double
        Dim CurStationStr As String
        Dim CurValueStr As String
        Dim CurStationSeq As Integer

        Dim WebC As WebClient = WebUtils.CreateWebClient()
        Dim Buf() As Byte
        Dim FileString As String
        Dim LogStr As String = String.Empty

        Dim IsMeasuringStation As Boolean = False
        Dim StRow As DataRow

        'Read Html file from Internet
        Try
            'HydroStationUri = Me.ViewConfiguration("HydroStationTableUri01")
            'HydroStationUri = "http://hydro.chmi.cz/hpps/hpps_oplist.php?recnum=400" 'to ensure that all stations are shown
            HydroStationUri = String.Format("http://hydro.chmi.cz/hpps/hpps_oplist.php?srt=&fkraj=&fpob=&fucpov=&kat=&startpos={0}&recnum={1}", startPos, recnum)

            Buf = WebC.DownloadData(HydroStationUri)
            FileString = System.Text.Encoding.GetEncoding(1250).GetString(Buf)
        Catch ex As Exception
            LogStr = vbCrLf & "executing GetHydroStationTable: " & ex.Message
            Return LogStr
        End Try

        'Parse the file, with the use of Regex
        Dim r As New Regex("<TR[^>]*>\s*" & _
                "<TD[^>]*>\s*<A[^\?]+\?seq=(?<1>[\d]+)[^>]*>\s*<IMG[^>]*>\s*</A>\s*</TD>\s*" & _
                "<TD[^>]*>[^<]*</TD>\s*" & _
                "<TD[^>]*>\s*<A[^>]*>[^<]*</A>\s*</TD>\s*" & _
                "<TD[^>]*>[^<]*</TD>\s*" & _
                "<TD[^>]*>[^<]*</TD>\s*" & _
                "<TD[^>]*>[^<]*</TD>\s*" & _
                "<TD[^>]*>[^T]+TD>\s*" & _
                "<TD[^>]*>[^T]+TD>\s*" & _
                "<TD[^>]*>(?<2>[^<]*)</TD>\s*" & _
                "<TD[^>]*>(?<3>[^<]*)</TD>\s*" & _
                "<TD[^>]*>(?<4>[^<]*)</TD>\s*", _
                RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        '"<TD[^>]*>\s*<A[^>]*>", _

        Dim m As Match = r.Match(FileString)

        While m.Success
            CurStationStr = m.Value
            CurValueStr = m.Groups(1).Value
            If IsNumeric(CurStationSeq) Then CurStationSeq = Integer.Parse(CurValueStr)

            'date of last observation..
            CurValueStr = m.Groups(2).Value
            If Me.IsValidDateString(CurValueStr) Then
                LastObsTime = Me.String2Date(ObsYear, CurValueStr)
                'change the year if necessary
                If LastObsTime.Month = 12 And CurTime.Month < 4 Then _
                LastObsTime = LastObsTime.AddYears(-1)
                'convert LastObsTime to UT!!!
                LastObsTime = LastObsTime.AddHours(-1)
                IsMeasuringStation = True
            End If

            'read H (last observed stage)
            CurValueStr = m.Groups(3).Value
            If CurValueStr.Length > 0 And CurValueStr.IndexOf("&nbsp;") < 0 Then
                LastObsH = Me.String2Single(CurValueStr)
            Else
                LastObsH = DbMissingValue
            End If

            'read Q (last observed discharge)
            CurValueStr = m.Groups(4).Value
            If CurValueStr.Length > 0 And CurValueStr.IndexOf("&nbsp;") < 0 Then
                LastObsQ = Me.String2Single(CurValueStr)
            Else
                LastObsQ = DbMissingValue
            End If

            'add row to the station table:
            If IsMeasuringStation = True Then
                StRow = StTab.NewRow
                StRow("st_seq") = CurStationSeq
                StRow("last_obs_time") = LastObsTime
                StRow("last_obs_h") = LastObsH
                StRow("last_obs_q") = LastObsQ
                StTab.Rows.Add(StRow)
                IsMeasuringStation = False
            End If

            m = m.NextMatch()
        End While
        Return LogStr
    End Function


    Private Function GetHydroStationTable_sql(ByVal HydroDs As DataSet) As String
        'EXPLANATION !!!:
        'get table of stations - from SQL server!

        'variables for internet data download
        Dim LogStr As String = ""
        Dim HydroStationUri As String
        Dim WebC As WebClient = WebUtils.CreateWebClient()
        Dim Buf() As Byte
        Dim FileString As String
        Dim sr As StringReader
        Dim CurLine As String
        Dim StationSeqIndex As Integer

        Dim i As Integer
        Dim CurTime As DateTime = DateTime.Now.AddHours(-1)
        Dim ObsYear As Integer = CurTime.Year
        Dim CurMonth As Integer = CurTime.Month
        Dim ObsMonth As Integer
        Dim CurValueStr As String
        Dim CurStationSeq As Integer
        Dim IsMeasuringStation As Boolean = False

        Dim LastObsH As Integer
        Dim LastObsTime As DateTime
        Dim LastObsQ As Integer


        Dim StRow As DataRow

        'variables for sql database access and data retrieval
        'data for connection to SQL server
        Dim sql_cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim sql_cmd As New SqlCommand
        Dim sql_cmd2 As New SqlCommand
        Dim sql_reader As SqlDataReader
        Dim CurStationId As Integer
        Dim cur_h_avg As Integer
        Dim cur_q_avg As Integer
        Dim latest_obs_time As DateTime

        'create a new table
        'StTab.TableName = "StTab"
        'StTab.Columns.Add(New DataColumn("st_id", GetType(Integer)))
        'StTab.Columns.Add(New DataColumn("st_seq", GetType(Integer)))
        'StTab.Columns.Add(New DataColumn("last_obs_time", GetType(DateTime)))
        'StTab.Columns.Add(New DataColumn("last_obs_h", GetType(Integer)))
        'StTab.Columns.Add(New DataColumn("last_obs_q", GetType(Integer)))
        'StTab.Columns.Add(New DataColumn("h_avg", GetType(Integer)))
        'StTab.Columns.Add(New DataColumn("q_avg", GetType(Integer)))
        'StTab.Columns.Add(New DataColumn("latest_obs_time", GetType(DateTime)))

        If Not HydroDs.Tables.Contains("StTab") Then HydroDs.Tables.Add("StTab")
        Dim StTab As DataTable = HydroDs.Tables("StTab")

        'set sql command parameters
        sql_cmd.Connection = sql_cnn
        sql_cmd.CommandType = CommandType.StoredProcedure
        sql_cmd.CommandText = "query_stationhydro3"
        Dim da As New SqlDataAdapter(sql_cmd)
        Try
            da.Fill(StTab)
        Catch ex As Exception

        End Try
        HydroDs.Tables(0).Columns.Add(New DataColumn("h_avg", GetType(Integer)))

        'change sql command
        sql_cmd2.Connection = sql_cnn
        sql_cmd2.CommandType = CommandType.StoredProcedure
        sql_cmd2.CommandText = "query_stationhydro4"
        sql_cmd2.Parameters.Add(New SqlParameter("@st_id", SqlDbType.Int))

        'add other data from database for each station row
        For Each StRow In HydroDs.Tables(0).Rows
            CurStationId = CType(StRow("st_id"), Integer)

            sql_cmd2.Parameters("@st_id").Value = CurStationId
            Try
                sql_cnn.Open()
                cur_h_avg = CType(sql_cmd2.ExecuteScalar, Integer)
                StRow("h_avg") = cur_h_avg
                sql_cnn.Close()
            Catch ex As Exception
                LogStr &= " " & ex.Message
            Finally
                If Not sql_cnn.State = ConnectionState.Closed Then sql_cnn.Close()
            End Try
        Next StRow
    End Function

    '-----------------------------------------------------------------------------------------------
    '********************** TEMPERATURE (AIR) ******************************************************
    '-----------------------------------------------------------------------------------------------
    'public method
    'DESCRIPTION:
    ' downloads and updates hourly temperature in POH, PLA, PMO and POD stations
    Public Function UpdateTemperature() As String
        'log message variables
        Dim LogStr As String = "Executing UpdateTemperature.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        Dim NumAddedRows As Integer = 0
        Dim NumUpdatedRows As Integer = 0
        Dim CurStationSeq As Integer = 0
        Dim CurStationUrl As String
        Dim configName As String

        Dim chmi_base_url = Me.ViewConfiguration("TmpGraphUri")

        '(1) get the table of temperature stations
        Dim stationList As List(Of TemperatureStation) = GetTemperatureStationTable()

        '(2) for each station in the table: download page and update values
        For Each st As TemperatureStation In stationList
            'to create the corresponding url
            Math.DivRem(st.Seq, 10000, CurStationSeq)
            Select Case st.OperatorId
                Case 1
                    configName = "chmi"
                Case 3
                    configName = "poh"
                Case 4
                    configName = "pla"
                Case 5
                    configName = "pmo"
                Case 6
                    configName = "pod"
                Case Else
                    configName = "pvl"
            End Select

            If configName = "chmi" Then
                CurStationUrl = chmi_base_url & st.DivisionName & "/" & st.MeteoCode & ".PNG"
                UpdateTemperature_chmi_station(CurStationUrl, st, NumAddedRows)
            Else
                CurStationUrl = Me.ViewConfiguration("PcpUri_" & configName) & "mereni_" & CurStationSeq.ToString() & ".htm"
                UpdateTemperature_povodi_station(CurStationUrl, st, NumAddedRows)
            End If
        Next


        'UpdateTemperature_Povodi("poh", LogStr, NumUpdatedRows, NumAddedRows)
        'UpdateTemperature_Povodi("pla", LogStr, NumUpdatedRows, NumAddedRows)
        'UpdateTemperature_Povodi("pmo", LogStr, NumUpdatedRows, NumAddedRows)
        'UpdateTemperature_Povodi("pod", LogStr, NumUpdatedRows, NumAddedRows)


        'return result log of the function
        ComputationTime = DateTime.Now.Subtract(StartTime)
        Return LogStr & vbCrLf & _
        "rows updated: " & NumUpdatedRows & vbCrLf & _
        "rows added: " & NumAddedRows & vbCrLf & _
        "Time taken: " & ComputationTime.ToString
    End Function

    'gets the table of all temperature stations
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

        'access to the database!
        'set sql command parameters
        sql_cmd.Connection = sql_cnn
        sql_cmd.CommandType = CommandType.StoredProcedure
        sql_cmd.CommandText = "query_temperaturestations"

        stationList.Clear()
        Try
            sql_cnn.Open()
            sql_reader = sql_cmd.ExecuteReader(CommandBehavior.CloseConnection)
            While sql_reader.Read()
                curStationId = CType(sql_reader("st_id"), Integer)
                curOperatorId = CType(sql_reader("operator_id"), Integer)
                curStationSeq = CType(sql_reader("st_seq"), Integer)
                curStationName = CType(sql_reader("st_name"), String)
                If Not TypeOf (sql_reader("max_obs_time")) Is DBNull Then
                    MaxDbTime = CType(sql_reader("max_obs_time"), DateTime)
                Else
                    MaxDbTime = DateTime.Now.Date.AddDays(-2)
                End If
                If Not TypeOf (sql_reader("division_name")) Is DBNull Then
                    DivisionName = sql_reader("division_name").ToString()
                Else
                    DivisionName = "unknown"
                End If
                If Not TypeOf (sql_reader("meteo_code")) Is DBNull Then
                    MeteoCode = sql_reader("meteo_code").ToString()
                Else
                    MeteoCode = "unknown"
                End If

                stationList.Add( _
                    New TemperatureStation(curStationId, curOperatorId, curStationSeq, _
                    curStationName, DivisionName, MeteoCode, MaxDbTime))
            End While

        Catch ex As Exception
            LogStr &= vbCrLf & ex.Message
        Finally
            sql_cnn.Close()
        End Try
        Return stationList
    End Function

    'updates HOURLY temperature for a station of POVODI !!!
    Private Function UpdateTemperature_povodi_station(ByVal Url As String, _
    ByVal st As TemperatureStation, ByRef AddedRows As Integer) As String

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

        'TODO change the regex expression
        Dim r1 As New Regex( _
        "<tr>\s*<td[^>]*>\s*<font[^>]+>(?<1>[^<]*)</font></td>\s*" & _
        "<td[^>]*><font[^>]+>(?<2>[\d\.,]+)[^<]*</font></td>\s*" & _
        "<td[^>]*><font[^>]+>(?<3>[\d\.,-]+)[^<]*</font></td>\s*" & _
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
        Dim CurValueStr As String
        Dim m1 As Match
        Dim CurValue As Single = -1
        Dim CurTime As DateTime
        Dim MaxDbTime As DateTime
        Dim TimeDiff As Double
        Dim universalTime As DateTime
        Dim i As Integer = 0
        'DateTime prevTime = DateTime.Now;

        'setup list of observations
        Observations.Clear()

        'set the times
        MaxDbTime = st.MaxDbTime
        MaxObsHours = 23

        m1 = r1.Match(fileString)
        While (m1.Success)
            MatchCount = MatchCount + 1
            CurStr = m1.Groups(3).Value
            If CurStr.Length > 0 Then
                CurValue = Me.String2Single(CurStr)

                Dim timeStr As String = m1.Groups(1).Value
                Dim day As Integer = CInt(timeStr.Substring(0, 2))
                Dim mon As Integer = CInt(timeStr.Substring(3, 2))
                Dim year As Integer = 2000 + CInt(timeStr.Substring(6, 2))
                Dim hour As Integer = CInt(timeStr.Substring(9, 2))
                Dim minute As Integer = CInt(timeStr.Substring(12, 2))

                CurTime = New DateTime(year, mon, day, hour, minute, 0)

                If CurTime > st.MaxDbTime Then
                    Dim val As HydroTimeValue
                    val.Time = CurTime
                    val.Value = CurValue
                    Observations.Add(val)
                End If

            End If
            m1 = m1.NextMatch()
        End While

        'write error message if there was no match
        If MatchCount = 0 Then
            LogStr &= vbCrLf & Url & ": No regex match!"
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
        Dim Observations As New List(Of HydroTimeValue)
        Dim ObsHours As Integer = 0
        Dim MaxObsHours As Integer = 0
        Dim AdditionalHour As Integer = 0

        Dim DbMissingValue As Single = 999.9

        For Each tvp As TimeValuePair In observationList
            If tvp.DateTime > st.MaxDbTime Then
                Dim htv As New HydroTimeValue
                htv.Time = tvp.DateTime
                htv.Value = Math.Round(tvp.Value, 1)
                Observations.Add(htv)
            End If
        Next


        'process the observations - add them to the station!
        If Observations.Count > 0 Then
            st.StartWebTime = Observations(0).Time

            'add observations to the station object
            For i = 0 To Observations.Count - 1
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

    '-----------------------------------------------------------------------------------------------
    '********************** SOIL WATER (top 10 and 50 cm) ******************************************
    '-----------------------------------------------------------------------------------------------

    'public method
    'DESCRIPTION:
    ' downloads and updates daily soil moisture category in CHMI stations
    'CALLS:
    ' UpdateDrought()
    'RETURNS:
    ' LogStr: String containing report and exception messages
    Public Function UpdateSoilWater() As String
        'log message variables
        Dim LogStr As String = "Executing UpdateSoilWater.."
        Dim StartTime As DateTime = DateTime.Now
        Dim ComputationTime As TimeSpan

        Dim VariableName As String = "soil_moisture_category"
        Dim NumAddedRows As Integer = 0
        Dim NumUpdatedRows As Integer = 0
        UpdateDrought("SoilUri10cm", "soil_water_10cm", LogStr, NumUpdatedRows, NumAddedRows)
        UpdateDrought("SoilUri50cm", "soil_water_50cm", LogStr, NumUpdatedRows, NumAddedRows)

        'return result log of the function
        ComputationTime = DateTime.Now.Subtract(StartTime)
        Return LogStr & vbCrLf & _
        "rows updated: " & NumUpdatedRows & vbCrLf & _
        "rows added: " & NumAddedRows & vbCrLf & _
        "Time taken: " & ComputationTime.ToString
    End Function

    'auxiliary subroutine
    'DESCRIPTION:
    'downloads and updates daily climate data (evaporation, soil moisture category) from CHMI stations
    '(website http://www.chmi.cz/meteo/ok)
    'CALLED BY:
    'UpdateEvap(), UpdateSoilWater()
    'PARAMETERS:
    'UriName ... name of the webpage uri in 'configuration' database table
    'VariableName ... name of the variable in 'variables' database table
    'LogStr ... string for reporting error messages
    'CALLS:
    'CreateSynopStationCommand(), CreateObservationCommand2(), GetStationIdByCoord(), 
    'ExecuteObservationCommand2()
    '
    'RETURNS: number of updated and added data records
    Private Sub UpdateDrought(ByVal UriName As String, ByVal VariableName As String, _
    ByRef logstr As String, ByRef NumUpdatedRows As Integer, ByRef NumAddedRows As Integer)

        Dim DataUriStr As String = Me.ViewConfiguration(UriName)
        Dim WebC As WebClient = WebUtils.CreateWebClient()
        Dim buf() As Byte
        Dim FileString As String

        'variables for creating snow station table
        Dim cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim StationCmd As SqlCommand = Me.CreateSynopStationCommand(cnn)
        Dim ObsCmd As SqlCommand = Me.CreateObservationCommand2(cnn)

        Dim CoordType As Integer = 26

        'variables for reading files from the web
        Dim r1, r2 As Regex
        Dim m As Match
        Dim MatchCount As Integer = 0
        Dim CurStr As String
        Dim CurNameStr As String
        Dim CurValueStr As String
        Dim DateStr As String
        Dim CoordTop As Integer
        Dim CoordLeft As Integer

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
                "style\s*=\s*[""]?\s*" & _
                "LEFT\:\s*(?<1>\d+)px;\D+TOP\:\s*(?<2>\d+)px[""]?>" & _
                "\D+(?<3>\d+[\.,]?\d?)\s*</span>", _
                RegexOptions.IgnoreCase Or RegexOptions.Compiled)


        'regex for reading observation time
        r2 = New Regex("Datum[^>]+>\s*(?<1>\d+)\.(?<2>\d+)\.(?<3>\d+)\s*</span>", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'get variable scale factor and id
        ScaleFactor = Me.GetScaleFactor(cnn, VariableName)
        VariableId = Me.GetVariableId(cnn, VariableName)
        If ScaleFactor = -1 Then
            logstr &= vbCrLf & "Executing UpdateSynop_24h.. " & _
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
            'for evaporation, change date to next day
            '(end of 24-h period)
            'NOTE that in climatology, the previous 24-hour-evaporation measured
            'at 7:00 local time of day d is assigned to day d-1 !!! (PREVIOUS DAY)
            ObsTime = ObsTime.AddDays(1)
        Else
            logstr &= vbCrLf & "CHMI lowlands: No regex match for ObsTime!"
        End If

        'read the observations
        MatchCount = 0
        m = r1.Match(FileString)
        While m.Success
            MatchCount = MatchCount + 1
            CurStr = m.Value
            CoordLeft = Integer.Parse(m.Groups(1).Value)
            CoordTop = Integer.Parse(m.Groups(2).Value)
            CurStationCoord = CoordTop * 1000 + CoordLeft
            CurValueStr = m.Groups(3).Value
            CurValueStr = CurValueStr.ToLower()

            If IsValidNumber(CurValueStr) Then
                ObsValue = CInt(Math.Round(Me.String2Single(CurValueStr) * ScaleFactor))
            Else
                ObsValue = 0
                If CurValueStr.IndexOf("pop") >= 0 Then ObsValue = -2
                If CurValueStr.IndexOf("nem") >= 0 Then ObsValue = -2
                If CurValueStr.IndexOf("nes") >= 0 Then ObsValue = -1
            End If

            'find station id in the database
            CurStationId = Me.GetStationIdByCoord(cnn, StationCmd, CurStationCoord, CoordType)
            'update observation
            If CurStationId > 0 Then
                logstr &= Me.ExecuteObservationCommand2(ObsCmd, CurStationId, _
                VariableName, ObsTime, ObsValue, NumUpdatedRows, NumAddedRows)
            Else
                logstr &= vbCrLf & CurStationCoord.ToString & ": coordinates not found."
            End If
            m = m.NextMatch()
        End While
        'write error message if there was no match
        If MatchCount = 0 Then
            logstr &= vbCrLf & "CHMI data file: No regex match for " & VariableName
        End If

    End Sub




    'pomocna funkce: vytvoreni objektu command pro ziskani posl. pozorovaneho 
    'vodniho stavu
    Private Function CreateObsByTimeCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.Connection = cnn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "query_observationbytime"
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

    Private Function CreateLatestObservationQCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.Connection = cnn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "query_latestobservationQ"
        cmd.Parameters.Add(New SqlParameter("@station_id", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@obs_time", SqlDbType.SmallDateTime))
        cmd.Parameters.Add(New SqlParameter("@obs_value", SqlDbType.SmallInt))
        cmd.Parameters("@obs_value").Direction = ParameterDirection.Output
        cmd.Parameters("@obs_time").Direction = ParameterDirection.Output
        Return cmd
    End Function

    'pomocna funkce: vraci posledni cas a hodnotu pozorovani Q
    've strukture HydroTimeValue
    'pokud pro stanici neni nalezeno zadne pozorovani Q, vraci vychozi
    'hodnoty obs_time = den pred nynejsim dnem a obs_value = -9999

    Private Function ViewLatestObsQ(ByVal cmd As SqlCommand, _
    ByVal st_id As Integer) As HydroTimeValue
        Dim cnn As SqlConnection = cmd.Connection
        Dim obs_value As Integer = -9999
        Dim obs_time As DateTime
        Dim ObsTimeValue As HydroTimeValue
        cmd.Parameters("@station_id").Value = st_id
        Try
            cnn.Open()
            cmd.ExecuteNonQuery()
            obs_time = CType(cmd.Parameters("@obs_time").Value, DateTime)
            obs_value = CType(cmd.Parameters("@obs_value").Value, Integer)
            cnn.Close()

        Catch ex As Exception
            obs_time = DateTime.Now.AddDays(-1)
            obs_value = -9999
        Finally
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
        End Try
        ObsTimeValue.Time = obs_time
        ObsTimeValue.Value = obs_value
        Return ObsTimeValue
    End Function

    'pomocna funkce: vytvoreni objektu command pro ziskani posl. terminu se srazkami
    Private Function CreateMaxPrecipObsCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "query_maxprecipobs2"
        cmd.Parameters.Add(New SqlParameter("@obs_time_from", SqlDbType.SmallDateTime))
        cmd.Parameters.Add(New SqlParameter("@variable_id", SqlDbType.TinyInt))
        cmd.Connection = cnn
        Return cmd
    End Function

    'auxiliary function: fills the table "maxprecipobs" in the dataset with
    'hours (days) that have recorded precipitation
    'CALLED BY: UpdateRadar()
    'PARAMETERS:
    'ds..dataset, must countain table "maxprecipobs"
    'obs_time_from .. the days(hours) with precipitation are searched from 
    '                 obs_time_from until now
    'VariableId .. can have values 1 (hourly precip) or 2 (daily precip at 6 UT)
    'NOTE: only hours(days) with precip > 0.2 mm are added to "maxprecipobs" table

    Private Function ViewMaxPrecipObsFrom _
    (ByVal ds As DataSet, _
    ByVal obs_time_from As DateTime, ByVal VariableId As Integer) As String

        Dim LogStr As String
        Dim new_dr As DataRow
        Dim obs_time_to As DateTime = DateTime.Now.AddHours(-3) 'because of UT

        Dim cmd As SqlCommand
        Dim cnn As New SqlConnection(SqlDbConnectionString)
        cmd = CreateMaxPrecipObsCommand(cnn)
        cmd.Parameters("@obs_time_from").Value = obs_time_from
        cmd.Parameters("@variable_id").Value = VariableId
        Dim da As New SqlDataAdapter(cmd)

        'priprava DataSetu
        If Not ds.Tables.Contains("maxprecipobs") Then ds.Tables.Add("maxprecipobs")
        Try
            da.Fill(ds, "maxprecipobs")
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
            LogStr = ex.Message
        End Try

        Return LogStr
    End Function

    'pomocna funkce: naplni dataset ds vsemi stanicemi 
    '(atribut st_seq, st_name2)
    Private Function ViewAllStations _
        (ByVal ds As DataSet) As String

        Dim LogStr As String
        Dim cmd As SqlCommand
        Dim cnn As New SqlConnection(SqlDbConnectionString)
        cmd = New SqlCommand("SELECT st_seq, st_name2 from stations", cnn)
        Dim da As New SqlDataAdapter(cmd)

        'priprava DataSetu
        If Not ds.Tables.Contains("stations01") Then ds.Tables.Add("stations01")
        Try
            da.Fill(ds, "stations01")
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
            LogStr = ex.Message
        End Try
        Return LogStr
    End Function

    'pomocna funkce: naplni dataset (tabulka "stations02" ds vsemi stanicemi,
    'pro ktere existuje hodnota st_coord_topleft
    'z mapy chmu
    Private Function ViewStationSnowCoords(ByVal ds As DataSet) As String
        Dim logstr As String = ""
        Dim cnn As New SqlConnection(SqlDbConnectionString)
        Dim cmd As New SqlCommand
        cmd.Connection = cnn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "query_snow_coord"
        Dim da As New SqlDataAdapter(cmd)

        'priprava DataSetu
        If Not ds.Tables.Contains("stations02") Then ds.Tables.Add("stations02")
        Try
            da.Fill(ds, "stations02")
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
            logstr = ex.Message
        End Try
        Return logstr
    End Function

    'pomocna funkce: vytvoreni objektu command pro ziskani posledniho rad. mereni
    Private Function CreateLastRadarFileCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "query_latestradarfile"
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
        cmd.CommandText = "update_radarfile"
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

    'pomocna funkce: vytvoreni objektu command pro aktualizaci tabulky observations
    Private Function CreateObservationCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "update_observation"
        cmd.Parameters.Add(New SqlParameter("@station_id", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@station_seq", SqlDbType.Int))
        cmd.Parameters.Add(New SqlParameter("@variable_name", SqlDbType.VarChar))
        cmd.Parameters.Add(New SqlParameter("@obs_time", SqlDbType.SmallDateTime))
        cmd.Parameters.Add(New SqlParameter("@obs_value", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@nodata_value", SqlDbType.SmallInt))
        cmd.Connection = cnn
        Return cmd
    End Function

    'pomocna funkce: vytvoreni objektu command pro aktualizaci tabulky observations2
    Private Function CreateObservationCommand2(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "update_observation2"
        cmd.Parameters.Add(New SqlParameter("@station_id", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@variable_name", SqlDbType.VarChar))
        cmd.Parameters.Add(New SqlParameter("@obs_time", SqlDbType.SmallDateTime))
        cmd.Parameters.Add(New SqlParameter("@obs_value", SqlDbType.Int))
        cmd.Parameters.Add(New SqlParameter("@status", SqlDbType.TinyInt))
        cmd.Parameters("@status").Direction = ParameterDirection.Output
        cmd.Connection = cnn
        Return cmd
    End Function

    Private Function CreateHydroCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "update_hydrodata"
        cmd.Parameters.Add(New SqlParameter("@station_id", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@obs_time", SqlDbType.SmallDateTime))
        cmd.Parameters.Add(New SqlParameter("@stage", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@discharge", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@status", SqlDbType.TinyInt))
        cmd.Parameters("@status").Direction = ParameterDirection.Output
        cmd.Connection = cnn
        Return cmd
    End Function

    'used to extract data from sql server!
    '(all observations for a given variable and station)
    Private Function CreateHydroSqlCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "query_observations_sql"
        cmd.Parameters.Add(New SqlParameter("@st_id", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@var_id", SqlDbType.TinyInt))
        cmd.Connection = cnn
        Return cmd
    End Function
    'auxiliary function - creates command for extracting data from sql server
    'for a given station, variable and time
    '@iavg = 1 - returns daily average of the observed variable
    Private Function CreateHydroSqlCommand2(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "query_day_avg"
        cmd.Parameters.Add(New SqlParameter("@st_id", SqlDbType.SmallInt))
        cmd.Parameters.Add(New SqlParameter("@var_id", SqlDbType.TinyInt))
        cmd.Parameters.Add(New SqlParameter("@date", SqlDbType.DateTime))
        cmd.Parameters.Add(New SqlParameter("@iavg", SqlDbType.TinyInt))
        cmd.Connection = cnn
        Return cmd
    End Function

    'used to get the hydrology observations from SQL server!
    Private Sub ExecuteHydroSqlCommand(ByVal cmd As SqlCommand, _
    ByVal st_id As Integer, ByVal var_id As Integer, ByVal ds As DataSet)
        Dim LogStr As String
        Dim cnn As SqlConnection = cmd.Connection
        cmd.CommandType = CommandType.StoredProcedure
        cmd.Parameters("@st_id").Value = st_id
        cmd.Parameters("@var_id").Value = var_id

        Dim da As New SqlDataAdapter(cmd)

        'priprava DataSetu
        If Not ds.Tables.Contains("observations") Then ds.Tables.Add("observations")
        Try
            da.Fill(ds, "observations")
        Catch ex As Exception
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
            LogStr = ex.Message
        End Try
    End Sub

    'auxiliary function: adds stage and discharge to hydrodata table!
    Private Function ExecuteHydroCommand _
    (ByVal cmd As SqlCommand, ByVal st_id As Integer, ByVal obs_time As DateTime, _
    ByVal stage_cm As Double, ByVal discharge_cms As Double, _
    ByRef NumUpdatedRows As Integer, ByRef NumAddedRows As Integer) As String

        'all stages are multiplied by 10 (result is in mm!!)
        Dim ScalingH As Integer = 10
        Dim LogStr As String
        Dim cnn As SqlConnection = cmd.Connection
        Dim log2_discharge As Integer

        cmd.Parameters("@station_id").Value = st_id
        cmd.Parameters("@obs_time").Value = obs_time
        If stage_cm > 0 Then cmd.Parameters("@stage").Value = CInt(Math.Round(stage_cm * ScalingH))
        If discharge_cms > 0 Then
            log2_discharge = CInt(1000 * Math.Log(discharge_cms, 2))
            cmd.Parameters("@discharge").Value = log2_discharge
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
    End Function

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
    Public Sub WriteLogFile(ByVal LogStr As String)
        Dim LogFilePath As String = ViewConfiguration("LogFilePath")
        Dim sw As System.IO.StreamWriter

        If File.Exists(LogFilePath) = False Then
            sw = File.CreateText(LogFilePath)
            sw.Write(LogStr)
            sw.Flush()
            sw.Close()
        End If

        sw = File.AppendText(LogFilePath)
        sw.Write(LogStr)
        sw.Flush()
        sw.Close()
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class