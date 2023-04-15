namespace ASP.Services
{
    public class DateService
    {
        public DateOnly GetDate()
        {
            return DateOnly.FromDateTime(DateTime.Now);
        }
    }
}
