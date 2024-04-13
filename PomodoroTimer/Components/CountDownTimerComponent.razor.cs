using Microsoft.AspNetCore.Components;
using PomodoroTimer.Utilities;

namespace PomodoroTimer.Components
{
    public partial class CountDownTimerComponent
    {
        private CountDownTimer countDownTimer;
        [Parameter]public int TimerLengthMinutes { get; set; }
        [Parameter]public EventCallback TimerEndedCallback { get; set; }
        private string timeLeftString = string.Empty;
        private Timer systemTimer;

        protected override async Task OnInitializedAsync()
        {
            countDownTimer = new CountDownTimer();
            countDownTimer.OnCountdownCompleted += TimerEnded;
            systemTimer = new Timer(CheckTimer, null, 100, 100);
        }

        private void CheckTimer(Object? stateInfo)
        {
            timeLeftString = countDownTimer.GetTimeRemaining().ToString(@"m\:ss");
            StateHasChanged();
        }

        private void StartTimer() 
        {
            if(countDownTimer.GetTimerState() == CountDownTimerState.Stopped)
            {
                countDownTimer.StartTimer(TimerLengthMinutes);
            }
            else
            {
                countDownTimer.UnPauseTimer();
            }
        }

        private void PauseTimer()
        {
            countDownTimer.PauseTimer();
        }

        private void StopTimer()
        {
            countDownTimer.StopTimer();
        }

        private async void TimerEnded()
        {
            await TimerEndedCallback.InvokeAsync(this);
        }

        ~CountDownTimerComponent()
        {
            countDownTimer.OnCountdownCompleted -= TimerEnded;
            systemTimer.Dispose();
        }
    }
}
