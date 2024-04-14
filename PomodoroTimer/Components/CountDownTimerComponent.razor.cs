using Microsoft.AspNetCore.Components;
using PomodoroTimer.Utilities;

namespace PomodoroTimer.Components
{
    public partial class CountDownTimerComponent
    {
        [Parameter]public CountdownTimer CountdownTimer { get; set; } = new CountdownTimer();
        [Parameter]public EventCallback TimerEndedCallback { get; set; }
        private string timeLeftString = string.Empty;
        private Timer systemTimer;

        protected override async Task OnInitializedAsync()
        {
            CountdownTimer.OnCountdownCompleted += TimerEnded;
            systemTimer = new Timer(CheckTimer, null, 100, 100);
        }

        private void CheckTimer(Object? stateInfo)
        {
            timeLeftString = CountdownTimer.GetTimeRemaining().ToString(@"m\:ss");
            StateHasChanged();
        }

        private void OnCountDownTimerSet(CountdownTimer timer)
        {
            CountdownTimer.OnCountdownCompleted -= TimerEnded;
            CountdownTimer = timer;
            CountdownTimer.OnCountdownCompleted += TimerEnded;
            StateHasChanged();
        }

        private void StartTimer() 
        {
            if(CountdownTimer.GetTimerState() == CountDownTimerState.Stopped)
            {
                CountdownTimer.StartTimer();
            }
            else
            {
                CountdownTimer.UnPauseTimer();
            }
        }

        private void PauseTimer()
        {
            CountdownTimer.PauseTimer();
        }

        private void StopTimer()
        {
            CountdownTimer.StopTimer();
        }

        private async void TimerEnded(CountdownTimer timer)
        {
            if(timer == CountdownTimer)
            {
                await TimerEndedCallback.InvokeAsync(this);
            }
            
        }

        ~CountDownTimerComponent()
        {
            CountdownTimer.OnCountdownCompleted -= TimerEnded;
            systemTimer.Dispose();
        }
    }
}
