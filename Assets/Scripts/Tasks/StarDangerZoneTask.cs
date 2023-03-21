using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Tasks
{
    public class StarDangerZoneTask : Task
    {
        [SerializeField]
        private List<Star> _stars = null;

        private void Start()
        {
            foreach (var star in _stars)
            {
                star.OnShipEnteredDangerZone += () => OnCompleted(this);
            }
        }

        private void Update()
        {
        }

        public override bool Completed()
        {
            return false;
        }

        public override int CompletedCount()
        {
            return 0;
        }

        public override int TotalCountToComplete()
        {
            return 1;
        }
    }
}
