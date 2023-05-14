namespace ASP.Services.Display;

public class DisplayServiceUkr : IDisplayService
{
    public string DateString(DateTime dt)
    {
        return dt.ToString("dd.MM.yyyy");
    }

    public string ReduceString(string str, int length)
    {
        if (str.Length > length)
        {
            return str.Substring(0, length) + "...";
        }

        return str;
    }
    
    public string NumberOfDaysBetweenDates(DateTime dt1, DateTime dt2)
    {
        return (dt1 - dt2).Days.ToString();
    }
}