﻿Imports System.Threading
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
            Dim dbm As New DBManager(Connstr3, ConfigurationManager.AppSettings("files_dir1"))
            'dbm.Update_Daily_Snow_All()
            'dbm.UpdateStationsVariables()
            'Dim stTable As DataTable = dbcn.DownloadPrecipMetadata(-7)
            'Dim hydroStTable As DataTable = dbcn.DownloadHydroMetadata()
            'Dim hydroStTable2 As DataTable = dbcn.DownloadHydroMetadata_Povodi()
            'LogStr &= dbcn.SaveStations_LVS("seso")

            'LogStr &= dbcn.UpdatePrecipitation_Daily()

            LogStr &= dbcn.UpdateSnow()
            LogStr &= dbcn.UpdateTemperature()
            LogStr &= dbcn.UpdatePrecip_Hourly_Povodi()
            LogStr &= dbcn.UpdatePrecip_Hourly_CHMU()
            LogStr &= dbcn.UpdateHydro_CHMU()
            LogStr &= dbcn.UpdateHydro_povodi()
            'LogStr &= dbcn.UpdatePrecipHourly_LVS()
            LogStr &= dbcn.UpdateRadar()

            'now update the daily data
            'dbcn.UpdateDailyData()

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