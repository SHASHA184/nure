namespace Course_Work_OOP;

public abstract class TimeHandler
{
    public static string AddDuration(string duration1, string duration2)
    {
        string[] duration1Array = duration1.Split(":");
        string[] duration2Array = duration2.Split(":");
        int hours = int.Parse(duration1Array[0]) + int.Parse(duration2Array[0]);
        int minutes = int.Parse(duration1Array[1]) + int.Parse(duration2Array[1]);
        int seconds = int.Parse(duration1Array[2]) + int.Parse(duration2Array[2]);
        if (seconds >= 60)
        {
            minutes++;
            seconds -= 60;
        }
        if (minutes >= 60)
        {
            hours++;
            minutes -= 60;
        }
        string hoursString = hours.ToString();
        string minutesString = minutes.ToString();
        string secondsString = seconds.ToString();
        if (hours < 10)
        {
            hoursString = "0" + hoursString;
        }
        if (minutes < 10)
        {
            minutesString = "0" + minutesString;
        }
        if (seconds < 10)
        {
            secondsString = "0" + secondsString;
        }
        return $"{hoursString}:{minutesString}:{secondsString}";
    }
    
    public static string SubtractDuration(string duration1, string duration2)
    {
        string[] duration1Array = duration1.Split(":");
        string[] duration2Array = duration2.Split(":");
        int hours = int.Parse(duration1Array[0]) - int.Parse(duration2Array[0]);
        int minutes = int.Parse(duration1Array[1]) - int.Parse(duration2Array[1]);
        int seconds = int.Parse(duration1Array[2]) - int.Parse(duration2Array[2]);
        if (seconds < 0)
        {
            minutes--;
            seconds += 60;
        }
        if (minutes < 0)
        {
            hours--;
            minutes += 60;
        }
        string hoursString = hours.ToString();
        string minutesString = minutes.ToString();
        string secondsString = seconds.ToString();
        if (hours < 10)
        {
            hoursString = "0" + hoursString;
        }
        if (minutes < 10)
        {
            minutesString = "0" + minutesString;
        }
        if (seconds < 10)
        {
            secondsString = "0" + secondsString;
        }
        return $"{hoursString}:{minutesString}:{secondsString}";
    }
    
    public static string CalculateDuration(List<string> allDurations)
    {
        string totalDuration = "00:00:00";
        foreach (var duration in allDurations)
        {
            totalDuration = AddDuration(totalDuration, duration);
        }
        return totalDuration;
    }
    
    public static bool CompareDurations(string duration1, string duration2)
    {
        string[] duration1Array = duration1.Split(":");
        string[] duration2Array = duration2.Split(":");
        int hours1 = int.Parse(duration1Array[0]);
        int minutes1 = int.Parse(duration1Array[1]);
        int seconds1 = int.Parse(duration1Array[2]);
        int hours2 = int.Parse(duration2Array[0]);
        int minutes2 = int.Parse(duration2Array[1]);
        int seconds2 = int.Parse(duration2Array[2]);
        if (hours1 > hours2)
        {
            return true;
        }
        else if (hours1 == hours2)
        {
            if (minutes1 > minutes2)
            {
                return true;
            }
            else if (minutes1 == minutes2)
            {
                if (seconds1 > seconds2)
                {
                    return true;
                }
                else if (seconds1 == seconds2)
                {
                    return true;
                }
            }
        }
        return false;
    }
    
    public static bool IsValidDuration(string duration, string format)
    {
        string[] durationArray = duration.Split(":");
        if (format == "hhmmss")
        {
            if (durationArray.Length != 3)
            {
                return false;
            }
            int hours = int.Parse(durationArray[0]);
            int minutes = int.Parse(durationArray[1]);
            int seconds = int.Parse(durationArray[2]);
            if (hours is < 0 or > 23)
            {
                return false;
            }
            if (minutes is < 0 or > 59)
            {
                return false;
            }
            if (seconds is < 0 or > 59)
            {
                return false;
            }
            return true;
            
        }

        else if (format == "mmss")
        {
            if (durationArray.Length != 2 || durationArray[0].Length != 2 || durationArray[1].Length != 2)
            {
                return false;
            }
            int minutes = int.Parse(durationArray[0]);
            int seconds = int.Parse(durationArray[1]);
            if (minutes is < 0 or > 59)
            {
                return false;
            }
            if (seconds is < 0 or > 59)
            {
                return false;
            }
            return true;
        }
        return false;
    }


}