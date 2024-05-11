namespace PomodoroTimer.Utilities
{
    public class CountdownTimer
    {
        DateTime endTime;
        DateTime pauseTime;
        CountDownTimerState timerState;
        private Timer threadingTimer;
        private TimeSpan timerLength;
        public static event Action<CountdownTimer> OnCountdownCompleted;
        public bool IsActive { get { return timerState == CountDownTimerState.Running; } }

        public CountdownTimer()
        {
            timerState = CountDownTimerState.Stopped;
        }

        public CountdownTimer(TimeSpan length)
        {
            timerLength = length;
            timerState = CountDownTimerState.Stopped;
        }

        ~CountdownTimer()
        {
            threadingTimer?.Change(-1, -1);
            threadingTimer?.Dispose();
        }

        public void StartTimer()
        { 
            endTime = DateTime.Now + timerLength;
            timerState = CountDownTimerState.Running;
            SetInternalThreadingTimer();
        }

        public void SetTimerLength(TimeSpan length)
        {
            timerLength = length;
        }

        public TimeSpan GetTimerLength()
        {
            return timerLength;
        }

        public void UnPauseTimer()
        {
            if (timerState != CountDownTimerState.Paused)
            {
                return;
            }

            var remainingTimeSpan = endTime - pauseTime;
            endTime = DateTime.Now + remainingTimeSpan;
            pauseTime = DateTime.UnixEpoch;
            timerState = CountDownTimerState.Running;
            SetInternalThreadingTimer();
        }

        public void PauseTimer()
        {
            if (timerState != CountDownTimerState.Running)
            {
                return;
            }

            pauseTime = DateTime.Now;
            timerState = CountDownTimerState.Paused;
            threadingTimer.Dispose();
        }

        public void StopTimer()
        {
            timerState = CountDownTimerState.Stopped;
            threadingTimer.Dispose();
        }

        public CountDownTimerState GetTimerState() {  return timerState; }

        public TimeSpan GetTimeRemaining()
        {
            switch (timerState)
            {
                case CountDownTimerState.Running:
                    return endTime - DateTime.Now;

                case CountDownTimerState.Paused:
                    return endTime - pauseTime;

                case CountDownTimerState.Stopped:
                    return timerLength;

                default:
                    return endTime - DateTime.Now;
            }
        }

        private void SetInternalThreadingTimer()
        {
            if (threadingTimer != null)
            {
                threadingTimer.Dispose();
            }

            var ms = (endTime - DateTime.Now).TotalMilliseconds;
            threadingTimer = new Timer(HandleThreadingTimerDue, null, (int)ms, -1);
        }

        private void HandleThreadingTimerDue(object? obj)
        {
            StopTimer();
            OnCountdownCompleted?.Invoke(this);
        }
    }
    public enum CountDownTimerState
    {
        Running,
        Paused,
        Stopped
    }
}
