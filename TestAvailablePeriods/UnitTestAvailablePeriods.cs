using SF2022User_NN_Lib;

namespace TestAvailablePeriods
{
    [TestClass]
    public class UnitTestAvailablePeriods
    {
        Calculations _calculations = new Calculations();

        [TestMethod]
        public void Test_NoBusyIntervals()
        {
            TimeSpan[] startTimes = Array.Empty<TimeSpan>();
            int[] durations = Array.Empty<int>();
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[] result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);
            Assert.IsTrue(result.Contains("08:00-08:30"));
        }

        [TestMethod]
        public void Test_FreeIntervalAtEndOfDay()
        {
            TimeSpan[] startTimes = { new TimeSpan(8, 0, 0) };
            int[] durations = { 9 * 60 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[] result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.AreEqual(2, result.Length);
        }

        [TestMethod]
        public void Test_NoFreeIntervalsBetweenEvents()
        {
            TimeSpan[] startTimes = { new TimeSpan(10, 0, 0), new TimeSpan(10, 30, 0) };
            int[] durations = { 30, 30 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[] result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.IsFalse(result.Contains("10:00-10:30"));
        }

        [TestMethod]
        public void Test_AllDayBusy()
        {
            TimeSpan[] startTimes = { new TimeSpan(8, 0, 0) };
            int[] durations = { 10 * 60 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 30;

            string[] result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void Test_ShortConsultationTime()
        {
            TimeSpan[] startTimes = { new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0) };
            int[] durations = { 90, 10 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 1;

            string[] result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.IsTrue(result.Contains("08:00-08:01"));
        }

        [TestMethod]
        public void Test_LongConsultationTime()
        {
            TimeSpan[] startTimes = { new TimeSpan(10, 0, 0) };
            int[] durations = { 30 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 540;

            string[] result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.AreEqual(0, result.Length);
        }

        [TestMethod]
        public void Test_OverlappingEvents()
        {
            TimeSpan[] startTimes = { new TimeSpan(10, 0, 0), new TimeSpan(10, 15, 0) };
            int[] durations = { 30, 30 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = 15;

            string[] result = _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime);

            Assert.IsFalse(result.Contains("10:00-10:15"));
        }

        [TestMethod]
        public void Test_NegativeConsultationTime()
        {
            TimeSpan[] startTimes = { new TimeSpan(10, 0, 0) };
            int[] durations = { 30 };
            TimeSpan beginWorkingTime = new TimeSpan(8, 0, 0);
            TimeSpan endWorkingTime = new TimeSpan(18, 0, 0);
            int consultationTime = -10;

            Assert.ThrowsException<Exception>(() => _calculations.AvailablePeriods(startTimes, durations, beginWorkingTime, endWorkingTime, consultationTime));
        }

        [TestMethod]
        public void Test_InvalidDurationArray()
        {
            TimeSpan[] startTimes = { new TimeSpan(10, 0, 0) };
            int[] durations = { };

            Assert.ThrowsException<Exception>(() => _calculations.AvailablePeriods(startTimes, durations, new TimeSpan(8, 0, 0), new TimeSpan(18, 0, 0), 30));
        }

        [TestMethod]
        public void Test_InvalidStartTimesArray()
        {
            TimeSpan[] startTimes = { };
            int[] durations = { 30 };

            Assert.ThrowsException<Exception>(() => _calculations.AvailablePeriods(startTimes, durations, new TimeSpan(8, 0, 0), new TimeSpan(18, 0, 0), 30));
        }
    }
}