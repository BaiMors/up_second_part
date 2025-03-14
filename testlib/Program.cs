using SF2022User_NN_Lib;

namespace testlib
{
    internal class Program
    {
        static TimeSpan[] startTimes = new TimeSpan[] 
        {
            new TimeSpan(10, 0, 0),
            new TimeSpan(11, 0, 0),
            new TimeSpan(15, 0, 0),
            new TimeSpan(15, 30, 0),
            new TimeSpan(16, 50, 0),
        };

        static int[] durations = new int[]
        {
            60, 30, 10, 10, 40
        };

        static TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
        static TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);

        static int consultationTime = 30;

        static void Main(string[] args)
        {
            Calculations calculations = new Calculations();

            string[] availablePeriods = calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            foreach (string period in availablePeriods)
            {
                Console.WriteLine(period);
            }
        }
    }
}
