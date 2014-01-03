Public Class TimeValuePair
    Public Sub New(ByVal t As DateTime, ByVal val As Double)
        DateTime = t
        Value = val
    End Sub

    Public Property DateTime As DateTime
    Public Property Value As Double
End Class
