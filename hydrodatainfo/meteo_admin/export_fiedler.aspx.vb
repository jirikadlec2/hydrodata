Imports System.Threading
Imports System.Web.Configuration

Partial Class export_fiedler
    Inherits System.Web.UI.Page

    Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        RunProcess()
    End Sub

    Private Sub RunProcess()
        Try
            ThreadPool.QueueUserWorkItem(New WaitCallback(AddressOf LoadLog))
        Catch ex As Exception
            Label1.Text = "Your job could not be submittited due to following error:" & ex.Message
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
            ProcessGraphs()
            '
        Finally
            mMutex.ReleaseMutex()
        End Try
    End Sub

    Private Sub ProcessGraphs()
        Dim imp As New Fiedler.Importer
        'read settings from config file..
        imp.ConnectionString = WebConfigurationManager.ConnectionStrings("cnn1").ConnectionString
        imp.LocalDataDir = GetLocalPath(WebConfigurationManager.AppSettings("local_data_dir"))
        imp.logFile = GetLocalPath(WebConfigurationManager.AppSettings("log_file"))
        imp.StationXmlFile = GetLocalPath(WebConfigurationManager.AppSettings("station_xml_file"))
        imp.GraphDir = GetLocalPath(WebConfigurationManager.AppSettings("graph_dir"))
        imp.TableDir = GetLocalPath(WebConfigurationManager.AppSettings("station_dir"))
        imp.DownloadInterval = Integer.Parse(WebConfigurationManager.AppSettings("download_interval"))

        imp.ImportAll()
        imp.DrawGraphs()
        imp.WriteTables()
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
