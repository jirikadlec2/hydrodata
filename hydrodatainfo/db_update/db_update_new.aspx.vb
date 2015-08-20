Imports System.Threading
Imports System.Net

Public Class db_update_new
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        RunProcess()

    End Sub

    Private Sub RunProcess()
        Try
            ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf LoadLog))
        Catch ex As Exception
            Me.Label1.Text = "Your job could not be submittited due to following error:" & ex.Message
        End Try
    End Sub

    'In above code LoadLog is the function which starts long running process
    'Following is the code for LoadLog
    Private Sub LoadLog(ByVal state As Object)
        Dim mutexKey As String = Thread.CurrentPrincipal.Identity.Name
        mutexKey = mutexKey.Replace("\", "_") 'Required otherwise mutex will consider its a file system path
        Dim mMutex As Mutex = New Mutex(False, mutexKey + "_LoadLogs")
        mMutex.WaitOne()
        Try
            'Here your long running process
            Thread.Sleep(5000) 't try this line, just for example.
            ProcessUpdate()
        Catch ex As Exception

        Finally
            mMutex.ReleaseMutex()
        End Try
    End Sub

    Private Sub ProcessUpdate()

        Dim Connstr1 As String = _
        "data source=.\SQLEXPRESS; Initial Catalog=plaveninycz1; User Id=sa; password=2c506bbe"

        Dim Connstr2 As String = _
        "Data Source=sql2005.dotnethosting.cz;Initial Catalog=plaveninycz1;User Id=plaveninycz1;Password=Ziqwdwq1;"

        Dim Connstr3 As String = _
            "Data Source=sql4.aspone.cz;Initial Catalog=db1856;User Id=db1856;Password=2c506bbe;"

        Dim LogStr As String = vbCrLf & vbCrLf & DateTime.Now.ToString
        Dim ExStr As String = ""

        Dim dbcn As New DbUpdateClassNew(Connstr3)
        Try
            dbcn.WriteLogFileToDb("Starting db_update_new..")
        Catch
        End Try

        Try
            Dim bft As New BinaryFileTester
            'bft.RunBinaryFileTest()
            Dim dbm As New DBManager(Connstr3, "C:\Temp\data")
            'dbm.UpdateStationsVariables()
            'Dim stTable As DataTable = dbcn.DownloadPrecipMetadata(-7)
            Dim hydroStTable As DataTable = dbcn.DownloadHydroMetadata()
            'LogStr &= dbcn.SaveStations_LVS("seso")

            'LogStr &= dbcn.UpdateTemperature()
            'LogStr &= dbcn.UpdateSnow()
            'LogStr &= dbcn.UpdatePrecipitation_Daily()
            'LogStr &= dbcn.UpdatePrecip_Hourly_Povodi()
            'LogStr &= dbcn.UpdatePrecip_Hourly_CHMU()

            'LogStr &= dbcn.UpdatePrecipHourly_LVS()
            'LogStr &= dbcn.UpdateHydro_povodi()
            'LogStr &= dbcn.UpdateHydro_CHMU()
            'LogStr &= dbcn.UpdateRadar()

        Catch ex As Exception
            ExStr = ex.Message
            If ExStr.Length > 0 Then ExStr = vbCrLf & ExStr
            LogStr &= ExStr
        End Try

        Try
            LogStr &= "..Finished db_update_new"
            dbcn.WriteLogFileToDb(LogStr)
        Catch ex As Exception
        End Try

        'Dim dbc As New DbUpdateClass(Connstr3)



        ''vlastni vypocty
        'Try
        '    LogStr &= vbCrLf & dbc.UpdateObsSnow2()
        '    LogStr &= vbCrLf & dbc.UpdateTemperature()
        '    LogStr &= vbCrLf & dbc.UpdatePrecip_povodi()
        '    LogStr &= vbCrLf & dbc.UpdatePrecip()
        '    LogStr &= vbCrLf & dbc.UpdateHydro()
        '    LogStr &= vbCrLf & dbc.UpdateHydro_povodi()
        '    LogStr &= vbCrLf & dbc.UpdateRadar()

        '    'LogStr &= vbCrLf & dbc.UpdateEvap()
        '    'LogStr &= vbCrLf & dbc.UpdateSoilWater()


        'Catch ex As Exception
        '    ExStr = ex.Message
        '    If ExStr.Length > 0 Then ExStr = vbCrLf & ExStr
        '    LogStr &= ExStr
        'End Try

        'write protocol to log file!
        'Try
        '    dbc.WriteLogFile(LogStr)
        'Catch ex As Exception
        'End Try
    End Sub


    Private Function GetLocalPath(ByVal path As String)
        Dim LocalPath As String
        If path.IndexOf("/") >= 0 Then
            LocalPath = Server.MapPath(path)
        Else
            LocalPath = path
        End If
        Return LocalPath
    End Function



End Class