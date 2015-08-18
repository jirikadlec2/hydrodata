Imports System.Data.SqlClient

'trida pro ulozeni teplotni stanice a jejich 
'aktualnich udaju...
Public Class TemperatureStation
    Public ReadOnly Property Id() As Integer
        Get
            Id = m_Id
        End Get
    End Property
    Public ReadOnly Property OperatorId() As Integer
        Get
            OperatorId = m_OperatorId
        End Get
    End Property
    Public ReadOnly Property Seq() As Integer
        Get
            Seq = m_Seq
        End Get
    End Property
    Public ReadOnly Property Name() As String
        Get
            Name = m_Name
        End Get
    End Property

    Public ReadOnly Property DivisionName() As String
        Get
            DivisionName = m_DivisionName
        End Get
    End Property
    Public ReadOnly Property MeteoCode() As String
        Get
            MeteoCode = m_MeteoCode
        End Get
    End Property

    Public ReadOnly Property Name2() As String
        Get
            Name2 = m_Name2
        End Get
    End Property

    'return false if the start time of observation series is not
    'initialized or if there are no observations in the list
    Public ReadOnly Property HasObservations() As Boolean
        Get
            If m_Observations.Length >= 4 Or m_StartWebTime > DateTime.MinValue Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

    Public Property StartWebTime() As DateTime
        Get
            Return m_StartWebTime
        End Get
        Set(ByVal Value As DateTime)
            m_StartWebTime = Value
        End Set
    End Property

    'the observations are being stored as:
    '0000111122223333....string, where each group of
    'four digits represents one observation
    Public ReadOnly Property Observations() As String
        Get
            Return m_Observations.ToString()
        End Get
    End Property

    Public ReadOnly Property NumObservations() As Integer
        Get
            Return CInt(m_Observations.Length / 4)
        End Get
    End Property

    Private m_Id As Integer
    Private m_OperatorId As Integer
    Private m_Seq As Integer
    Private m_Name As String
    Private m_StartWebTime As DateTime
    Private m_Observations As System.Text.StringBuilder
    Private m_DivisionName As String
    Private m_MeteoCode As String
    Private m_Name2 As String

    Public Sub New(ByVal Id As Integer, ByVal OperatorId As Integer, ByVal Seq As Integer, ByVal Name As String,
                   ByVal DivisionName As String, ByVal MeteoCode As String, ByVal Name2 As String)
        m_Id = Id
        m_OperatorId = OperatorId
        m_Seq = Seq
        m_Name = Name
        m_DivisionName = DivisionName
        m_MeteoCode = MeteoCode
        m_Observations = New System.Text.StringBuilder(200)
        m_StartWebTime = DateTime.MinValue
        m_Name2 = Name2
    End Sub

    Public Overrides Function ToString() As String
        Return Id.ToString() & " " & Name
    End Function

    Public Sub ClearObservations()
        m_Observations.Clear()
    End Sub

    'appends the observation to the internal stringBuilder list
    Public Sub AddObservation(ByVal obsValue As Double)
        If obsValue < 999 Then
            obsValue = CInt(Math.Round(obsValue * 10))
            If obsValue >= 0 Then
                'positiveT: use four digits
                Dim tempval As String = obsValue.ToString("0000")
                m_Observations.Append(tempval)
            Else
                'negativeT: need room for minus sign
                Dim tempval As String = obsValue.ToString("000")
                m_Observations.Append(tempval)
            End If
        End If
    End Sub

    'adds a ''missing'' observation value to the list
    Public Sub AddMissingValue()
        m_Observations.Append("9999")
    End Sub

    'writes the observation values to database and
    'returns a string with eventual error
    'connStr is the database connection string..
    Public Function UpdateDB(ByVal cnn As SqlConnection, ByRef AddedRows As Integer) As String
        Dim cmd As New SqlCommand
        Dim errorMessage As String
        Dim removeIndex As Integer
        Dim MissingValue As String = "9999"

        If Me.HasObservations Then

            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = "update_temperature"
            cmd.Connection = cnn
            cmd.Parameters.Add(New SqlParameter("@observations", SqlDbType.VarChar))
            cmd.Parameters.Add(New SqlParameter("@station_id", SqlDbType.SmallInt))
            cmd.Parameters.Add(New SqlParameter("@start_time", SqlDbType.SmallDateTime))
            cmd.Parameters.Add(New SqlParameter("@added_rows", SqlDbType.SmallInt))
            cmd.Parameters("@added_rows").Direction = ParameterDirection.Output

            'remove any characters at the end of m_observations having missing values
            removeIndex = m_Observations.Length
            While removeIndex >= 4
                If m_Observations.ToString(removeIndex - 4, 4) <> MissingValue Then
                    Exit While
                End If
                removeIndex = removeIndex - 4
            End While
            If removeIndex < m_Observations.Length Then
                m_Observations.Remove(removeIndex, m_Observations.Length - removeIndex)
            End If
            Dim obsString As String = m_Observations.ToString()
            cmd.Parameters("@observations").Value = obsString
            cmd.Parameters("@station_id").Value = m_Id
            cmd.Parameters("@start_time").Value = m_StartWebTime

            Try
                cnn.Open()
                cmd.ExecuteNonQuery()
                AddedRows += CInt(cmd.Parameters("@added_rows").Value)
            Catch ex As Exception
                errorMessage = ex.Message
            Finally
                cnn.Close()
            End Try
        End If
        Return errorMessage
    End Function

    'override equals and gethashcode!!
    Overrides Function Gethashcode() As Integer
        Return m_Seq
    End Function

    Overloads Overrides Function Equals(ByVal o As Object) As Boolean
        Dim result As Boolean = False
        If o.GetHashCode() = Me.Gethashcode() Then
            result = True
        End If
        Return result
    End Function
End Class
