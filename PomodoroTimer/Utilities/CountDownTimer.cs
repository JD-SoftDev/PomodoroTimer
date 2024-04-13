namespace PomodoroTimer.Utilities
{
    public class CountDownTimer
    {
        private CountDownTimerData _countdownTimerData;
        private Timer threadingTimer;
        public event Action OnCountdownCompleted;

        public CountDownTimer()
        {
            _countdownTimerData = new CountDownTimerData()
            {
                TimerState = CountDownTimerState.Stopped
            };
        }

        ~CountDownTimer()
        {
            threadingTimer.Dispose();
        }

        public void StartTimer(int minutes)
        {
            _countdownTimerData = new CountDownTimerData()
            {
                StartTime = DateTime.Now,
                EndTime = DateTime.Now + TimeSpan.FromMinutes(minutes)
            };
            SetInternalThreadingTimer();
        }

        public void UnPauseTimer()
        {
            if (_countdownTimerData.TimerState != CountDownTimerState.Paused)
            {
                return;
            }

            var remainingTimeSpan = _countdownTimerData.EndTime - _countdownTimerData.PauseTime;
            _countdownTimerData.StartTime = DateTime.Now;
            _countdownTimerData.EndTime = DateTime.Now + remainingTimeSpan;
            _countdownTimerData.PauseTime = DateTime.UnixEpoch;
            _countdownTimerData.TimerState = CountDownTimerState.Running;
            SetInternalThreadingTimer();
        }

        public void PauseTimer()
        {
            if (_countdownTimerData.TimerState != CountDownTimerState.Running)
            {
                return;
            }

            _countdownTimerData.PauseTime = DateTime.Now;
            _countdownTimerData.TimerState = CountDownTimerState.Paused;
            threadingTimer.Dispose();
        }

        public void StopTimer()
        {
            _countdownTimerData.TimerState = CountDownTimerState.Stopped;
        }

        public CountDownTimerState GetTimerState() {  return _countdownTimerData.TimerState; }

        public TimeSpan GetTimeRemaining()
        {
            switch (_countdownTimerData.TimerState)
            {
                case CountDownTimerState.Running:
                    return _countdownTimerData.EndTime - DateTime.Now;

                case CountDownTimerState.Paused:
                    return _countdownTimerData.EndTime - _countdownTimerData.PauseTime;

                case CountDownTimerState.Stopped:
                    return TimeSpan.Zero;

                default:
                    return _countdownTimerData.EndTime - DateTime.Now;
            }
        }

        private void SetInternalThreadingTimer()
        {
            if (threadingTimer != null)
            {
                threadingTimer.Dispose();
            }

            var ms = (_countdownTimerData.EndTime - DateTime.Now).Milliseconds;
            threadingTimer = new Timer(HandleThreadingTimerDue, null, ms, -1);
        }

        private void HandleThreadingTimerDue(object? obj)
        {
            OnCountdownCompleted?.Invoke();
        }
    }
}
