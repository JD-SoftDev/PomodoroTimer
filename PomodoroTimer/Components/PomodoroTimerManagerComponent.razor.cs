using Howler.Blazor.Components;
using Microsoft.AspNetCore.Components;
using PomodoroTimer.Models;
using PomodoroTimer.Utilities;
using Radzen;

namespace PomodoroTimer.Components
{
    public partial class PomodoroTimerManagerComponent
    {
        private TimeSpan m_PomodoroLength;
        
        public TimeSpan PomodoroLength
        {
            get { return m_PomodoroLength; }
            set { 
                m_PomodoroLength = value;
                UpdateTimerDisplay();
            }
        }

        private TimeSpan m_shortBreakLength;

        public TimeSpan ShortBreakLength
        {
            get { return m_shortBreakLength; }
            set { m_shortBreakLength = value;
                UpdateTimerDisplay();
            }
        }

        private TimeSpan m_LongBreakLength;

        public TimeSpan LongBreakLength
        {
            get { return m_LongBreakLength; }
            set { m_LongBreakLength = value;
                UpdateTimerDisplay();
            }
        }

        private int m_PomodorosBetweenLongBreaks;

        public int PomodorosBetweenLongBreaks
        {
            get { return m_PomodorosBetweenLongBreaks; }
            set { m_PomodorosBetweenLongBreaks = value; }
        }
            
        private int pomodorosSinceLongBreak;

        private CountdownTimer currentTimer = new CountdownTimer();
        private TimerType currentTimerType;
        [Parameter] public EventCallback<TimerFinishedDialogResult> PomodoroCompleted { get; set; }
        [Inject] private DialogService diaglogService { get; set; }
        bool settingMenuActive = true;
        AlarmPlayerComponent alarmPlayer;

        public PomodoroTimerManagerComponent()
        {
            m_PomodoroLength = TimeSpan.FromMinutes(25);
            m_shortBreakLength = TimeSpan.FromMinutes(5);
            m_LongBreakLength = TimeSpan.FromMinutes(15);

            m_PomodorosBetweenLongBreaks = 3;

            currentTimer = new CountdownTimer(m_PomodoroLength);
            currentTimerType = TimerType.Pomodoro;
        }

        private async Task HandleTimerCompleted()
        {
            alarmPlayer.Play();
            var dialogResult = await diaglogService.OpenAsync<TimerCompletedPopup>("Timer Finshed",
                new Dictionary<string, object>() { { "TimerType", currentTimerType } });
            alarmPlayer.Stop();
            if(currentTimerType == TimerType.Pomodoro)
            {
                pomodorosSinceLongBreak++;
                if (pomodorosSinceLongBreak >= PomodorosBetweenLongBreaks)
                {
                    StartLongBreakTimer();
                }
                else
                {
                    StartShortBreakTimer();
                }
                await PomodoroCompleted.InvokeAsync((TimerFinishedDialogResult)dialogResult);
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
            currentTimer = new CountdownTimer(m_PomodoroLength);
            currentTimerType = TimerType.Pomodoro;
            StateHasChanged();
        }

        private void StartShortBreakTimer()
        {
            currentTimer = new CountdownTimer(m_shortBreakLength);
            currentTimerType = TimerType.ShortBreak;
            StateHasChanged();
        }

        private void StartLongBreakTimer()
        {
            currentTimer = new CountdownTimer(m_LongBreakLength);
            currentTimerType = TimerType.LongBreak;
            StateHasChanged();
        }

        private void SettingsButtonClicked()
        {
            settingMenuActive = !settingMenuActive;
            StateHasChanged();
        }

        private void UpdateTimerDisplay()
        {
            if (currentTimer.IsActive) { return; }

            currentTimer = new CountdownTimer(CurrentTimerLength());
            StateHasChanged();
        }

        private TimeSpan CurrentTimerLength()
        {
            switch (currentTimerType)
            {
                case TimerType.LongBreak:
                    return m_LongBreakLength;
                case TimerType.ShortBreak:
                    return m_shortBreakLength;
                case TimerType.Pomodoro:
                    return m_PomodoroLength;
            }

            return TimeSpan.Zero;
        }

        private TimeSpan ParseToTimeSpan(string value)
        {
            if(TimeSpan.TryParse(value, out var result))
            {
                return result;
            }
            return TimeSpan.Zero;
        }
    }

    public enum TimerType
    {
        Pomodoro,
        ShortBreak,
        LongBreak
    }
}
