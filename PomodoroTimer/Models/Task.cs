namespace PomodoroTimer.Models
{
    public class Task
    {
        public string Name { get; set; }
        public int NumberOfPomodorosToComplete { get; set; }
        public int CurrentNumberOfPomodoros { get; set; }
    }
}
