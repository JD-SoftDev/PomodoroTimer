using PomodoroTimer.Controllers;

namespace PomodoroTimer.Pages
{
    public partial class Home
    {
        private CountDownTimerController countDownTimerController;
        private string timeLeftString = string.Empty;
        private Timer systemTimer;

        public Home()
        {
            countDownTimerController = new CountDownTimerController();
            countDownTimerController.StartTimer(5);
            systemTimer = new Timer(CheckTimer, null, 100, 100);
        }

        public Home(CountDownTimerController countDownTimerController)
        {
            this.countDownTimerController = countDownTimerController;
            countDownTimerController.StartTimer(5);
            systemTimer = new Timer(CheckTimer, null, 100, 100);
        }

        private void CheckTimer(Object? stateInfo) 
        {
            timeLeftString = countDownTimerController.GetTimeRemaining();
            StateHasChanged();
        }

        ~Home() 
        {
            systemTimer.Dispose();
        }
    }
}
