using System;
using UnityEngine;

namespace Scripts.Tasks
{
    public abstract class Task : MonoBehaviour
    {
        public Action<Task> OnCompleted;
        public Action<Task> OnProgressChanged;

        [SerializeField]
        private string _description = "";
        [SerializeField]
        private bool _completeToFail = false;
        [SerializeField]
        private bool _levelFinishTask = false;

        public string Description { get { return _description; } }
        public bool CompleteToFail { get { return _completeToFail; } }
        public bool LevelFinishTask { get { return _levelFinishTask; } }

        public abstract bool Completed();
        public abstract int TotalCountToComplete();
        public abstract int CompletedCount();
    }
}
