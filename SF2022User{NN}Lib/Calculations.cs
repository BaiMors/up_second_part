namespace SF2022User_NN_Lib
{
    public class Calculations
    {
        /// <summary>
        /// Метод расчета списка свободных временных интервалов в графике сотрудника
        /// </summary>
        /// <param name="startTimes">массив времени начала рабочего промежутка</param>
        /// <param name="durations">массив длительности рабочих промежутков</param>
        /// <param name="beginWorkingTime">время начала рабочего дня</param>
        /// <param name="endWorkingTime">время окончания рабочего дня</param>
        /// <param name="consultationTime">минимальное необходимое время для работы менеджера</param>
        /// <returns>список (массив строк) подходящих свободных временных промежутков</returns>
        /// <exception cref="Exception"></exception>
        public string[] AvailablePeriods(TimeSpan[] startTimes, int[] durations, TimeSpan beginWorkingTime, TimeSpan endWorkingTime, int consultationTime)
        {
            if (startTimes.Length != durations.Length)
                throw new Exception("Каждому элементу массива startTimes должен соответствовать элемент массива durations");
            if (consultationTime < 1)
                throw new Exception("Время консультации должно быть натуральным числом");

            List<string> _freePeriods = new();
            DateTime _timeNow = DateTime.Today.Add(beginWorkingTime);
            DateTime _endWorkingTime = DateTime.Today.Add(endWorkingTime);
            Array.Sort(startTimes, durations, Comparer<TimeSpan>.Create((a, b) => a.CompareTo(b)));

            int _counter = 0;
            while (_timeNow <= _endWorkingTime.AddMinutes(-consultationTime))
            {
                DateTime _startOfWork;
                DateTime _endOfWork;

                if (_counter < startTimes.Length)
                {
                    _startOfWork = DateTime.Today.Add(startTimes[_counter]);
                    _endOfWork = _startOfWork.AddMinutes(durations[_counter]);
                }
                else
                {
                    _startOfWork = _endWorkingTime;
                    _endOfWork = _endWorkingTime;
                }
                DateTime _freeTimeEnd = _startOfWork;
                while (_timeNow.AddMinutes(consultationTime) <= _freeTimeEnd)
                {
                    _freePeriods.Add($"{_timeNow:HH:mm}-{_timeNow.AddMinutes(consultationTime):HH:mm}");
                    _timeNow = _timeNow.AddMinutes(consultationTime);
                }
                if (_counter < startTimes.Length)
                {
                    if (_timeNow < _endOfWork)
                        _timeNow = _endOfWork;
                    _counter++;
                }
            }

            return _freePeriods.ToArray();
        }
    }
}
