using PomodoroTimer.Models;

namespace PomodoroTimer.Services
{
    public interface IPomodoroTaskManagerService
    {
        void AddTask(PomodoroTimerTask task);
        void AddTask(string name, int estNumPomodoro);
        IEnumerable<PomodoroTimerTask> GetCompletedList();
        PomodoroTimerTask? GetCurrentTask();
        IEnumerable<PomodoroTimerTask> GetToDoList();
        void MoveItem(PomodoroTimerTask task, int moveToIndex);
        void PomodoroCompleted();
        void RemoveTask(PomodoroTimerTask task);
        void TaskCompleted();
    }
}
