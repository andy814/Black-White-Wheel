using System;

public class Record
{
    public float SurviveTime;
    public DateTime Timestamp;

    public Record(float surviveTime, DateTime timestamp)
    {
        SurviveTime = surviveTime;
        Timestamp = timestamp;
    }
}