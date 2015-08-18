Imports System.Data.SqlClient

'trida pro ulozeni srazkomerne stanice a jejich 
'aktualnich udaju...
Public Class PrecipStation
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
    Public Property Name() As String
        Get
            Name = m_Name
        End Get
        Set(value As String)
            m_Name = value
        End Set
    End Property

    Public Property MeteoCode() As String
        Get
            MeteoCode = m_meteoCode
        End Get
        Set(value As String)
            m_meteoCode = value
        End Set
    End Property

    Public Property DivisionName() As String
        Get
            DivisionName = m_DivisionName
        End Get
        Set(value As String)
            m_DivisionName = value
        End Set
    End Property


    Public Property Url() As String
        Get
            Url = m_Url
        End Get
        Set(value As String)
            m_Url = value
        End Set
    End Property

    Public Property Elevation() As Double
        Get
            Elevation = m_elevation
        End Get
        Set(value As Double)
            m_elevation = value
        End Set
    End Property

    Public Property MaxDbTime() As DateTime
        Get
            MaxDbTime = m_MaxDbTime
        End Get
        Set(value As DateTime)
            m_MaxDbTime = value
        End Set
    End Property

    Public ReadOnly Property BinaryFileName() As String
        Get
            Dim fileDir As String = ConfigurationManager.AppSettings("files_dir1")
            Dim siteCode As String = Me.Id.ToString("D4")
            Dim binaryFile As String = "h" & "_" & "srazky" & "_" & siteCode & ".dat"
            Dim fileName As String = fileDir & "\h\srazky\" & binaryFile
            Return fileName
        End Get
    End Property

    Public ReadOnly Property OperatorId() As Integer
        Get
            OperatorId = m_OperatorId
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

    Public ReadOnly Property Observations_Array As List(Of Single)
        Get
            Return m_Observations_Array
        End Get
    End Property

    Public ReadOnly Property NumObservations() As Integer
        Get
            Return CInt(m_Observations.Length / 4)
        End Get
    End Property

    Public ReadOnly Property Code() As String
        Get
            Return m_Code
        End Get
    End Property

    Public Property Lat() As Double
        Get
            Return m_Lat
        End Get
        Set(value As Double)
            m_Lat = value
        End Set
    End Property

    Public Property Lon() As Double
        Get
            Return m_Lon
        End Get
        Set(value As Double)
            m_Lon = value
        End Set
    End Property

    Private m_Id As Integer
    Private m_Seq As Integer
    Private m_Name As String
    Private m_MaxDbTime As DateTime
    Private m_StartWebTime As DateTime
    Private m_Observations As System.Text.StringBuilder
    Private m_OperatorId As Integer
    Private m_Code As String
    Private m_Lat As Double
    Private m_Lon As Double
    Private m_Url As String
    Private m_elevation As Double
    Private m_DivisionName As String
    Private m_meteoCode As String
    Private m_Observations_Array As List(Of Single)

    Public Sub New(ByVal Id As Integer, ByVal Seq As Integer, ByVal Name As String, ByVal OperatorId As Integer, ByVal MaxDbTime As DateTime)
        m_Id = Id
        m_Seq = Seq
        m_Name = Name
        m_OperatorId = OperatorId
        m_MaxDbTime = MaxDbTime
        m_Observations = New System.Text.StringBuilder(200)
        m_StartWebTime = DateTime.MinValue
        m_Code = m_Id.ToString()
        m_Lat = 0.0
        m_Lon = 0.0
        m_Url = m_Code
        m_meteoCode = "unknown"
        m_Observations_Array = New List(Of Single)
    End Sub

    Public Function FindMaxDbTime() As DateTime
        Dim bfm As New BinaryFileManager
        Return bfm.GetLastDateInFile(Me.BinaryFileName)
    End Function

    Public Sub New(ByVal Id As Integer, ByVal Seq As Integer, ByVal Name As String, ByVal OperatorId As Integer, ByVal Code As String, ByVal MaxDbTime As DateTime)
        m_Id = Id
        m_Seq = Seq
        m_Name = Name
        m_OperatorId = OperatorId
        m_MaxDbTime = MaxDbTime
        m_Observations = New System.Text.StringBuilder(200)
        m_StartWebTime = DateTime.MinValue
        m_Code = Code
        m_Lat = 0.0
        m_Lon = 0.0
        m_Url = m_Code
        m_meteoCode = "unknown"
        m_Observations_Array = New List(Of Single)
    End Sub

    Public Sub New(ByVal Id As Integer, ByVal Seq As Integer, ByVal Name As String, ByVal Meteo As String, ByVal OperatorId As Integer, ByVal Code As String, ByVal MaxDbTime As DateTime)
        m_Id = Id
        m_Seq = Seq
        m_Name = Name
        m_OperatorId = OperatorId
        m_MaxDbTime = MaxDbTime
        m_Observations = New System.Text.StringBuilder(200)
        m_StartWebTime = DateTime.MinValue
        m_Code = Code
        m_Lat = 0.0
        m_Lon = 0.0
        m_Url = m_Code
        m_meteoCode = Meteo
        m_Observations_Array = New List(Of Single)
    End Sub

    'appends the observation to the internal stringBuilder list
    Public Sub AddObservation(ByVal obsValue As Double)
        If obsValue >= 0 And obsValue < 999 Then
            obsValue = CInt(Math.Round(obsValue * 10))
            Dim tempval As String = obsValue.ToString("0000")
            m_Observations.Append(tempval)
            m_Observations_Array.Add(CSng(CDbl(obsValue) / 10.0))
        End If
    End Sub

    'adds a ''missing'' observation value to the list
    Public Sub AddMissingValue()
        m_Observations.Append("9999")
    End Sub

    'check if the station is in the db
    'if yes, then synchronize station's id, seq and other properties
    Public Sub SynchronizeFromDB(ByVal cnn As SqlConnection, ByVal name2 As String)
        Dim cmd1 As New SqlCommand
        cmd1.CommandText = String.Format("SELECT st_id, st_seq, st_name FROM plaveninycz.stations WHERE st_name2='{0}'", name2)
        cmd1.Connection = cnn
        Try
            cnn.Open()
            Dim r1 As SqlDataReader = cmd1.ExecuteReader(CommandBehavior.SingleRow)
            r1.Read()
            If r1.HasRows Then
                Dim CurStationId As Integer = CType(r1.GetValue(r1.GetOrdinal("st_id")), Integer)
                Dim CurStationSeq As Integer = CType(r1.GetInt32(r1.GetOrdinal("st_seq")), Integer)
                m_Id = CurStationId
                m_Seq = CurStationSeq
            End If
        Catch ex As Exception
        Finally
            cnn.Close()
        End Try
    End Sub

    Public Sub AddStationToDB(ByVal cnn As SqlConnection)
        Dim cmd1 As New SqlCommand
        Dim maxId As Integer = 0
        cmd1.CommandText = "SELECT MAX(st_id) FROM plaveninycz.stations"
        cmd1.Connection = cnn
        Try
            cnn.Open()
            Dim r1 As SqlDataReader = cmd1.ExecuteReader(CommandBehavior.SingleResult)
            r1.Read()
            If r1.HasRows Then
                maxId = CType(r1.GetValue(0), Integer)
            End If
        Catch ex As Exception
        Finally
            cnn.Close()
        End Try

        Dim newId As Integer = maxId + 1
        Dim newSeq As Integer = 99990000 + newId
        Dim cmd2 As New SqlCommand
        cmd2.CommandText = "INSERT INTO plaveninycz.stations (st_id, st_seq, st_name, st_name2, st_uri, lat, lon, altitude, operator_id) " & _
        "VALUES(@p1,@p2,@p3,@p4,@p5,@p6,@p7,@p8,@p9)"
        cmd2.Parameters.Add(New SqlParameter("p1", newId))
        cmd2.Parameters.Add(New SqlParameter("p2", newSeq))
        cmd2.Parameters.Add(New SqlParameter("p3", Name))
        cmd2.Parameters.Add(New SqlParameter("p4", Code))
        cmd2.Parameters.Add(New SqlParameter("p5", Url))
        cmd2.Parameters.Add(New SqlParameter("p6", Lat))
        cmd2.Parameters.Add(New SqlParameter("p7", Lon))
        cmd2.Parameters.Add(New SqlParameter("p8", Elevation))
        cmd2.Parameters.Add(New SqlParameter("p9", OperatorId))

        cmd2.Connection = cnn
        Try
            cnn.Open()
            cmd2.ExecuteNonQuery()
            m_Id = newId
            m_Seq = newSeq
        Catch ex As Exception
        Finally
            cnn.Close()
        End Try

        Dim cmd3 As New SqlCommand
        cmd3.CommandText = "INSERT INTO plaveninycz.stationsvariables (st_id, var_id) VALUES (@p1, @p2)"
        cmd3.Parameters.Add(New SqlParameter("p1", newId))
        cmd3.Parameters.Add(New SqlParameter("p2", 1))
        cmd3.Connection = cnn
        Try
            cnn.Open()
            cmd3.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            cnn.Close()
        End Try
    End Sub

    'writes the observation values to binary file
    Public Function UpdateBinaryFile(ByRef AddedRows As Integer) As String

        Try

            Dim observationList As List(Of TimeValuePair) = Me.GetValuesList()

            If observationList.Count > 0 Then

                Dim fileDir As String = ConfigurationManager.AppSettings("files_dir1")
                Dim siteCode As String = Me.Id.ToString("D4")
                Dim binaryFile As String = "h" & "_" & "srazky" & "_" & siteCode & ".dat"

                Dim fileName As String = fileDir & "\h\srazky\" & binaryFile
                Dim bfm As New BinaryFileManager()
                AddedRows += bfm.AddValues(fileName, observationList, "h")

            End If

            Return String.Empty

        Catch ex As Exception
            Return ex.Message
        End Try

    End Function

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
            cmd.CommandText = "plaveninycz.new_update_rain_hourly"
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

    'gets the values in the form of a list of TimeValuePairs
    Public Function GetValuesList() As List(Of TimeValuePair)
        Dim result As New List(Of TimeValuePair)
        If Not HasObservations() Then
            Return result
        End If

        Dim curTime As DateTime = Me.StartWebTime
        Dim numObs As Integer = m_Observations_Array.Count - 1
        For plusHour As Integer = 0 To numObs
            curTime = Me.StartWebTime.AddHours(plusHour)
            result.Add(New TimeValuePair(curTime, m_Observations_Array(plusHour)))
        Next
        Return result
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
