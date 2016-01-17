Imports System.IO
Imports System.Data.SqlClient

''' <summary>
''' Management of the database
''' </summary>
''' <remarks>Add or update metadata to the db</remarks>
Public Class DBManager

    Dim cnn As String
    Dim dataFolder As String

    Public Sub New(ByVal connectionString As String, ByVal dataFolderName As String)
        Me.cnn = connectionString
        Me.dataFolder = dataFolderName
    End Sub

    Public Sub UpdateStationsVariables()

        'get the data folder
        AddStationVariable_Entries("prutok")
        AddStationVariable_Entries("vodstav")
        AddStationVariable_Entries("srazky")
        AddStationVariable_Entries("snih")
        AddStationVariable_Entries("teplota")
    End Sub

    Public Sub Update_Daily_Snow_All()
        Dim ids = New Integer() {49, 51, 52, 53, 60, 62, 63, 73, 76, 78, _
                                 80, 81, 82, 218, 219, 220, 221, 222, 223, 224, 225, 226, 227, 228, 229, 230, 231, _
                                 232, 233, 236, 237, 238, 239, 240, 253, 528, 531, 533, 801, 849, 1163, 1314, 1315, 1316}
        For Each id In ids
            Try
                Update_Daily_Snow(id)
            Catch ex As Exception
                Dim msg As String = ex.Message
            End Try
        Next
    End Sub

    'updates daily snow data (from API) to database
    Public Function Update_Daily_Snow(ByVal stID As String) As String
        'first get data from api
        Dim logStr As String = ""
        Dim fileString As String
        Dim webc As New System.Net.WebClient()
        Dim Url As String = "http://hydrodata.info/api/values?var=snih&st=" & stID.ToString()

        Try
            fileString = webc.DownloadString(Url)
        Catch ex As Exception
            logStr &= String.Format("Unable to download api file for station {0}", stID)
            Return logStr
        End Try

        'next parse the api data
        Dim lines2() As String = fileString.Split()

        Dim conn As New SqlConnection(Me.cnn)
        Dim snowCmd As SqlCommand = CreateSnowCommand(conn)

        Dim nrow As String = lines2.Length
        Dim updatedRows As Integer = 0
        Dim addedRows As Integer = 0
        For i As Integer = 2 To nrow - 2 Step 2
            Dim dat As String = lines2(i)
            Dim year As Integer = CInt(dat.Substring(0, 4))
            Dim mon As String = CInt(dat.Substring(5, 2))
            Dim day As String = CInt(dat.Substring(8, 2))
            Dim obsDat As New Date(year, mon, day)

            Dim val As String = lines2(i + 1)
            Dim obsVal As Integer = -9999
            If val <> "NA" Then
                obsVal = CInt(val)
            End If

            ExecuteSnowCommand(snowCmd, stID, obsDat, obsVal, updatedRows, addedRows)
        Next i
       

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

    Public Sub AddStationVariable_Entries(ByVal variableName As String)

        'variableName
        Dim vFolder As String = Path.Combine(dataFolder, "h", variableName)
        Dim dFiles() As String = Directory.GetFiles(vFolder)
        For Each dFile In dFiles
            Dim fn As String = dFile.Substring(dFile.LastIndexOf("\") + 1)
            Dim stCode As String = fn.Substring(fn.Length - 8, 4)
            Dim stId As Integer = CInt(stCode)
            Dim varID As Integer = 1
            Select Case variableName
                Case "srazky"
                    varID = 1
                Case "vodstav"
                    varID = 4
                Case "prutok"
                    varID = 5
                Case "snih"
                    varID = 8
                Case "teplota"
                    varID = 16
            End Select
            AddStationVariable_Entry(varID, stId)
        Next

    End Sub

    'stID, varID
    Public Sub AddStationVariable_Entry(ByVal varID As Integer, ByVal stID As Integer)

        Dim connection As New SqlConnection(cnn)

        Dim cmd As New SqlCommand
        cmd.CommandText = "INSERT INTO plaveninycz.stationsvariables (st_id, var_id) VALUES(@p1,@p2)"
        cmd.Parameters.Add(New SqlParameter("p1", stID))
        cmd.Parameters.Add(New SqlParameter("p2", varID))
        cmd.Connection = connection
        Try
            connection.Open()
            cmd.ExecuteNonQuery()
        Catch ex As Exception
        Finally
            connection.Close()
        End Try

    End Sub

End Class
