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
