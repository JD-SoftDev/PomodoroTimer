using PomodoroTimer.Utilities;

namespace PomodoroTimer.Components
{
    public partial class PomodoroTimerManagerComponent
    {
        private TimeSpan pomodoroLength;
        private TimeSpan shortBreakLength;
        private TimeSpan longBreakLength;

        private int pomodorosBetweenLongBreaks;
        private int pomodorosSinceLongBreak;

        private CountdownTimer currentTimer = new CountdownTimer();
        private TimerType currentTimerType;

        public PomodoroTimerManagerComponent()
        {
            pomodoroLength = TimeSpan.FromSeconds(5);
            shortBreakLength = TimeSpan.FromSeconds(5);
            longBreakLength = TimeSpan.FromSeconds(5);

            pomodorosBetweenLongBreaks = 2;

            currentTimer = new CountdownTimer(pomodoroLength);
            currentTimerType = TimerType.Pomodoro;
        }

        private void HandleTimerCompleted()
        {
            if(currentTimerType == TimerType.Pomodoro)
            {
                pomodorosSinceLongBreak++;
                if (pomodorosSinceLongBreak >= pomodorosBetweenLongBreaks)
                {
                    StartLongBreakTimer();
                }
                else
                {
                    StartShortBreakTimer();
                }
            }
            else
            {
                if(currentTimerType == TimerType.LongBreak)
                {
                    pomodorosSinceLongBreak = 0;
                }
                StartPomodoroTimer();
            }
        }

        private void StartPomodoroTimer()
        {
            currentTimer = new CountdownTimer(pomodoroLength);
            currentTimerType = TimerType.Pomodoro;
            StateHasChanged();
        }

        private void StartShortBreakTimer()
        {
            currentTimer = new CountdownTimer(shortBreakLength);
            currentTimerType = TimerType.ShortBreak;
            StateHasChanged();
        }

        private void StartLongBreakTimer()
        {
            currentTimer = new CountdownTimer(longBreakLength);
            currentTimerType = TimerType.LongBreak;
            StateHasChanged();
        }
    }

    public enum TimerType
    {
        Pomodoro,
        ShortBreak,
        LongBreak
    }
}
