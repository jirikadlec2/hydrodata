Imports System.IO

Public Class MyFileInfo
    Public Property Name As String

    Public Property LastWriteTime As DateTime
    Public Property Url As String
    Public Property Length As Long
End Class

Public Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim RadarFilePathBase = "D:\Websites\448cf9624b\www\Radar\"

        Dim RadarFullPath = RadarFilePathBase & "cz\2012\04\"

        Dim dirInfo As New DirectoryInfo(RadarFullPath)

        'label1.Text = dirInfo.FullName
        Dim fileList As New List(Of MyFileInfo)
        For Each fn As FileInfo In dirInfo.GetFiles("*.gif")
            Dim fi As New MyFileInfo
            fi.Name = fn.Name
            fi.Url = "http://hydrodata.info/radar/cz/2012/04/" + fn.Name
            fi.LastWriteTime = fn.LastWriteTime
            fi.Length = fn.Length
            fileList.Add(fi)
        Next

        grid1.DataSource = fileList
        grid1.DataBind()
    End Sub
End Class