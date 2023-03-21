using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Tasks
{
    public class TaskManager : MonoBehaviour
    {
        public Action OnTaskProgressChanged;
        public Action OnTaskCompleted;
        public Action OnLevelCompleted;
        public Action OnTaskFailed;
        public Action OnLevelFailed;

        [SerializeField]
        private List<Task> _tasks = null;

        private void Start()
        {
            foreach (var task in _tasks)
            {
                task.OnCompleted += TaskCompleted;
                task.OnProgressChanged += TaskProgressChanged;
            }
        }

        private void TaskProgressChanged(Task task)
        {
            OnTaskProgressChanged?.Invoke();
        }

        private void TaskCompleted(Task task)
        {
            if (task.CompleteToFail)
            {
                if (task.LevelFinishTask)
                {
                    OnLevelFailed?.Invoke();
                }
                else
                {
                    OnTaskFailed?.Invoke();
                }
            }
            else
            {
                if (task.LevelFinishTask)
                {
                    OnLevelCompleted?.Invoke();
                }
                else
                {
                    OnTaskCompleted?.Invoke();
                }
            }
        }

        public IEnumerable<Task> GetTasksToComplete()
        {
            return _tasks.Where(x => x.CompleteToFail == false);
        }

        public IEnumerable<Task> GetTasksToFail()
        {
            return _tasks.Where(x => x.CompleteToFail == true);
        }
    }
}
