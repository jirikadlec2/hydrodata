Public Class BinaryFileTester

    Public Sub RunBinaryFileTest()
        Dim bfm As New BinaryFileManager
        'Dim tvp As List(Of TimeValuePair) = bfm.GetDataFromAPI(89, "prutok")
        'Dim len As Integer = tvp.Count
        'bfm.SaveToBinaryFile(tvp, "C:\temp\data\flw_0089.dat")

        'test reading from the binary file
        'Dim startTime As New DateTime(2013, 6, 1, 0, 0, 0)
        'Dim endTime As New DateTime(2013, 6, 2, 23, 0, 0)
        'Dim out As List(Of TimeValuePair) = bfm.OpenBinaryFile("C:\temp\data\flw_0089.dat", startTime, endTime)

        'try to save all files

        bfm.SaveBinaryFiles("C:\temp\data\h\vodstav", "vodstav", "h")
        bfm.SaveBinaryFiles("C:\temp\data\h\prutok", "prutok", "h")
        bfm.SaveBinaryFiles("C:\temp\data\h\teplota", "teplota", "h")
        bfm.SaveBinaryFiles("C:\temp\data\h\srazky", "srazky", "h")
        bfm.SaveBinaryFiles("C:\temp\data\d\srazky", "srazky", "d")
        bfm.SaveBinaryFiles("C:\temp\data\d\snih", "snih", "d")
    End Sub

End Class
