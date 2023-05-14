namespace ASP.Services.Display;

public interface IDisplayService
{
    public string DateString(DateTime dt);
    public string ReduceString(string str, int length);
    public string NumberOfDaysBetweenDates(DateTime dt1, DateTime dt2);
}