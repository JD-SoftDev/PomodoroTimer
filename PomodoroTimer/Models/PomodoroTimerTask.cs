namespace PomodoroTimer.Models
{
    public class PomodoroTimerTask
    {
        public string Name { get; set; }
        public int NumberOfPomodorosToComplete { get; set; }
        public int CurrentNumberOfPomodoros { get; set; }
        public bool Completed { get; set; }
    }
}
