using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Tasks
{
    public class VisitSomeStarsTask : Task
    {
        [SerializeField]
        private List<Star> _starsToVisit = null;

        [SerializeField]
        private int _minCountToVisit = 0;
        
        private int _lastCount = 0;

        private void Start()
        {
            _lastCount = 0;
        }

        private void Update()
        {
            var completedCount = CompletedCount();
            if (_lastCount != completedCount)
            {
                OnProgressChanged?.Invoke(this);

                if (Completed())
                {
                    OnCompleted?.Invoke(this);
                }

                _lastCount = completedCount;
            }
        }

        public override bool Completed()
        {
            return _starsToVisit.Count() > _minCountToVisit;
        }

        public override int CompletedCount()
        {
            return _starsToVisit.Count(x => x.Visited());
        }

        public override int TotalCountToComplete()
        {
            return _starsToVisit.Count();
        }
    }
}
