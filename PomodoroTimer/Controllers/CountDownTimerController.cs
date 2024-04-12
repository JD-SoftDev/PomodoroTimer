using PomodoroTimer.Models;

namespace PomodoroTimer.Controllers
{
    public class CountDownTimerController
    {
        private CountDownTimerData _countdownTimerData;

        public CountDownTimerController()
        {
            _countdownTimerData = new CountDownTimerData();
        }

        public CountDownTimerController(DateTime startTime, int minutes)
        {
            _countdownTimerData = new CountDownTimerData()
            {
                StartTime = startTime,
                EndTime = startTime + TimeSpan.FromMinutes(minutes)
            };
        }

        public void StartTimer(int minutes) 
        {
            _countdownTimerData = new CountDownTimerData()
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now + TimeSpan.FromMinutes(minutes)
            };
        }

        public string GetTimeRemaining()
        {
            var timeRemaining = _countdownTimerData.EndTime - DateTime.Now;
            return timeRemaining.ToString("mmss");
        }
    }
}
