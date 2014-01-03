' trida StationUpdateClass
' methods for update of station and other geographic objects

Option Strict On

Imports System.Data.OleDb
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net
Imports System.Text.Encoding
Imports System.Text.RegularExpressions


Public Class StationUpdateClass
    ' CONSTRUCTOR
    Public Sub New()
        Me.new("Integrated Security=SSPI;Persist Security Info=False;" & _
        "Initial Catalog=Hydro4;Data Source=(local);user=sa; password=+chnbz+")
    End Sub

    'constructor with SQL connection string
    Public Sub New(ByVal sql_connstring As String)
        Me.SqlDbConnectionString = sql_connstring
    End Sub

    'SQL connection string for joining the database
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

    'structure for saving the data value pair(obs_time,obs_value)
    'used by function UpdateObsHydro
    Private Structure HydroTimeValue
        Dim Time As DateTime
        Dim Value As Integer
    End Structure

    ' PUBLIC functions
    Public Function UpdateHydroStations1() As String
        Dim LogStr As String = "executing UpdateHydroStations1.."
        Dim VariableName As String = "precip_day"
        Dim DataUriStr As String = "http://localhost/hydroweb/voda/seznam_hlasnych_profilu.htm"
        Dim WebC As New WebClient
        Dim buf() As Byte
        Dim FileString As String

        'variables for reading files from the web
        Dim r1, r2 As Regex
        Dim m1, m2 As Match
        Dim MatchCount As Integer = 0
        Dim CurStr As String
        Dim CurNameStr As String
        Dim CurValueStr As String

        'r1 = New Regex("<td[^>]*>\s*" & _
        '"<font[^>]*>(?<1>[^<]+)<br>[^<]*</font>\s*" & _
        '"<font[^>]*>[^0-9,\.]*(?<2>[\d\.,]+)[^<]*</font>\s*<br>\s*" & _
        '"</td>\s*</tr>\s*(</tbody>)?\s*</table>\s*</div>\s*" & _
        '"<div\s*id\s*=\s*[""]?bod(?<3>\w+)", _
        '        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        r1 = New Regex("a(?<1>[^>]+)><img[^>]*></a></td>\s*" & _
        "<td[^>]*>[^>]+</td>\s*" & _
        "<td[^>]*>(?<2>[^<]+)</td>\s*" & _
        "<td[^>]*>(?<3>[^<]+)</td>\s*" & _
        "<td[^>]*>[^<]+</td>\s*", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)


        r2 = New Regex("[\D]+(?<1>[\d]+)[\D]+", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

        'SQL database variables
        Dim cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim StationCmd As SqlCommand = Me.CreateUpdateStation1_command(cnn)

        'observation data values(number)
        Dim ObsValue As Integer
        Dim CurSeqStr As String
        Dim Seq As Integer
        Dim river_name As String
        Dim loc_name As String

        'read the html file
        Try
            buf = WebC.DownloadData(DataUriStr)
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
            CurStr = m1.Value
            'read station seq
            CurValueStr = m1.Groups(1).Value
            m2 = r2.Match(CurValueStr)
            If m2.Success Then
                CurSeqStr = m2.Groups(1).Value
                Seq = Integer.Parse(CurSeqStr)
            End If
            'read river name
            river_name = m1.Groups(2).Value
            'remove part after bracket from river name
            If river_name.IndexOf("(") >= 0 Then river_name = _
            river_name.Substring(0, river_name.IndexOf("(")).Trim
            'read station name
            loc_name = m1.Groups(3).Value

            'update river, location and instrument
            Me.ExectuteUpdateStation1_command(cnn, StationCmd, river_name, loc_name, Seq)

            m1 = m1.NextMatch()
        End While
        'write error message if there was no match
        If MatchCount = 0 Then
            LogStr &= vbCrLf & "web page : No regex match for " & VariableName
        End If

        Return LogStr
    End Function

    'auxiliary function: create SQL command to update the station
    Private Function CreateUpdateStation1_command(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Connection = cnn
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "update_station_hydro1"
        cmd.Parameters.Add(New SqlParameter("@river_name", SqlDbType.VarChar))
        cmd.Parameters.Add(New SqlParameter("@loc_name", SqlDbType.VarChar))
        cmd.Parameters.Add(New SqlParameter("@seq", SqlDbType.Int))
        Return cmd
    End Function

    'auxiliary function: execute SQL command and add a new station
    Private Sub ExectuteUpdateStation1_command(ByVal cnn As SqlConnection, _
    ByVal cmd As SqlCommand, _
    ByVal river_name As String, ByVal loc_name As String, ByVal seq As Integer)
        Dim ExStr As String
        cmd.Parameters("@river_name").Value = river_name
        cmd.Parameters("@loc_name").Value = loc_name
        cmd.Parameters("@seq").Value = seq

        Try
            cnn.Open()
            cmd.ExecuteNonQuery()
            cnn.Close()
        Catch ex As Exception
            ExStr = ex.Message
            If Not cnn.State = ConnectionState.Closed Then cnn.Close()
        End Try
    End Sub

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

    'auxiliary function: view synoptic station id, when given st_coord_topleft
    'RETURNS: st_id if successful, -1 if no such station exists
    'CALLED BY: UpdateSynop_24h()
    'PARAMETERS:
    ' Coord ... the top-left screen station coordinate (integer from 100000 to 999999)
    'CALLS:     nothing
    Private Function GetStationIdByCoord _
    (ByVal cnn As SqlConnection, ByVal cmd As SqlCommand, ByVal Coord As Integer) As Integer
        Dim Result As Integer = -1
        cmd.Parameters("@st_coord_topleft").Value = Coord
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



    'auxiliary function: checks if a string is a valid number
    Public Function IsValidNumber(ByVal str As String) As Boolean
        Dim r As Regex
        Dim m As Match

        r = New Regex("\d*[\.,]?\d*", _
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)
        m = r.Match(str)
        If m.Success Then
            Return True
        Else
            Return False
        End If
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

    'auxiliary function: safely converts a string with "." or "," to a "single"
    'data type number
    Private Function String2Single(ByVal str As String) As Single
        Dim strWholePart As String
        Dim strDecPart As String
        Dim strParts() As String
        Dim strDelims As String = ".,"
        Dim charDelims() As Char = strDelims.ToCharArray

        Dim DecimalPointPos As Integer
        Dim WholeValue As Single = 0
        Dim DecValue As Single = 0
        Dim Value As Single = 0

        If str.IndexOf(".") < 0 And str.IndexOf(",") < 0 Then
            Value = CSng(str)
        Else
            strParts = str.Split(charDelims)
            strWholePart = strParts(0)
            strDecPart = strParts(1)
            If strWholePart.Length > 0 Then WholeValue = CSng(strWholePart)
            If strDecPart.Length > 0 Then DecValue = CSng(strDecPart) / CSng(10 ^ strDecPart.Length)
            Value = DecValue + WholeValue
        End If

        Return Value
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