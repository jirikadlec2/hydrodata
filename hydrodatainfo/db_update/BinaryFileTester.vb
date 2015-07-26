Public Class BinaryFileTester

    Public Sub RunBinaryFileTest()
        Dim bfm As New BinaryFileManager
        Dim tvp As List(Of TimeValuePair) = bfm.GetDataFromAPI(89, "prutok")
        Dim len As Integer = tvp.Count
        bfm.SaveToBinaryFile(tvp, "C:\temp\data\flw_0089.dat")

        'test reading from the binary file
        Dim startTime As New DateTime(2013, 6, 1, 0, 0, 0)
        Dim endTime As New DateTime(2013, 6, 2, 23, 0, 0)
        Dim out As List(Of TimeValuePair) = bfm.OpenBinaryFile("C:\temp\data\flw_0089.dat", startTime, endTime)
    End Sub

End Class
