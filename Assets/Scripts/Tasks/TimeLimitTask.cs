using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Tasks
{
    public class TimeLimitTask : Task
    {
        [SerializeField]
        private int TimeLimit = 0;

        [SerializeField]
        private TimeController _timeController = null;

        private float _currentTime = 0;

        private void Start()
        {
            _currentTime = 0;
        }

        private void Update()
        {
            var wasCompleted = Completed();

            _currentTime = _timeController.GetGameTime();

            if (!wasCompleted && Completed())
            {
                OnCompleted?.Invoke(this);
            }
        }

        public override bool Completed()
        {
            return _currentTime < TimeLimit;
        }

        public override int CompletedCount()
        {
            return (int)_currentTime;
        }

        public override int TotalCountToComplete()
        {
            return TimeLimit;
        }
    }
}