using System;
using Firebase.Firestore;

public static class DateTimeExtension
{
    public static DateTime CurrentDateTime()
    {
        return Timestamp.GetCurrentTimestamp().ToDateTime().ToLocalTime();
    }
}
