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


Public Class LahtiUpdateClass
    ' KONSTRUKTOR
    Public Sub New()
        Me.New("Integrated Security=SSPI;Persist Security Info=False;" & _
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

    Public Function UpdateLahti() As String
        Dim logstr As String = "executing updateLahti.."
        Dim dataUriStr As String = "https://geoinformatics.aalto.fi/kala/charts/call_extract_csvs.cgi"
        Dim myC As New WebClient
        myC.Credentials = New NetworkCredential("jkadlec", "dz74isnuj")

        Dim buf() As Byte
        Dim FileString As String
        Try
            buf = myC.DownloadData(dataUriStr)
            FileString = System.Text.Encoding.GetEncoding(1250).GetString(buf)
        Catch ex As Exception
            FileString = ""
            logstr &= vbCrLf & "downloading file:" & ex.Message
        End Try

        Return logstr
    End Function

    Private Function CreateLogCommand(ByVal cnn As SqlConnection) As SqlCommand
        Dim cmd As New SqlCommand
        cmd.Parameters.Clear()
        cmd.CommandType = CommandType.StoredProcedure
        cmd.CommandText = "new_update_log"
        cmd.Parameters.Add(New SqlParameter("@log_time", SqlDbType.SmallDateTime))
        cmd.Parameters.Add(New SqlParameter("@log_text", SqlDbType.Text))
        cmd.Connection = cnn
        Return cmd
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

        Dim cmdText As String = "new_update_log"

        Dim sql_cnn As New SqlConnection(Me.SqlDbConnectionString)
        Dim logCmd As SqlCommand = Me.CreateLogCommand(sql_cnn)

        ExecuteLogCommand(logCmd, DateTime.Now, LogStr)
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class