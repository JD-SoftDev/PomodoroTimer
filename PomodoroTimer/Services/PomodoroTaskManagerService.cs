using PomodoroTimer.Models;
using System.Collections.Immutable;

namespace PomodoroTimer.Services
{
    public class PomodoroTaskManagerService
    {
        private List<PomodoroTimerTask> todoList = new List<PomodoroTimerTask>();
        private List<PomodoroTimerTask> completedTasks = new List<PomodoroTimerTask>();

        public PomodoroTaskManagerService()
        {
            
        }

        public void AddTask(PomodoroTimerTask task)
        {
            todoList.Add(task);
        }

        public void AddTask(string name, int estNumPomodoro)
        {
            AddTask(new PomodoroTimerTask()
            {
                Name = name,
                NumberOfPomodorosToComplete = estNumPomodoro
            });
        }

        public void RemoveTask(PomodoroTimerTask task)
        {
            todoList.Remove(task);
        }

        public PomodoroTimerTask? GetCurrentTask()
        {
            if (!todoList.Any()) { return null; }
            return todoList[0];
        }

        public IEnumerable<PomodoroTimerTask> GetToDoList()
        {
            return todoList.ToImmutableArray();
        }

        public IEnumerable<PomodoroTimerTask> GetCompletedList()
        {
            return completedTasks.ToImmutableArray();
        }

        public void PomodoroCompleted()
        {
            todoList[0].CurrentNumberOfPomodoros++;
        }

        public void TaskCompleted()
        {
            todoList[0].Completed = true;
            completedTasks.Add(todoList[0]);
            todoList.RemoveAt(0);
        }
    }
}
