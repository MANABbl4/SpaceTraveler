using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Tasks
{
    public class FuelEndedTask : Task
    {
        [SerializeField]
        private Ship _playerShip = null;

        private bool _completed = false;

        private void Start()
        {
            _completed = false;
            _playerShip.FuelEndedEvent += OnFuelEnded;
        }

        private void Update()
        {
        }

        public override bool Completed()
        {
            return _completed;
        }

        public override int CompletedCount()
        {
            return 0;
        }

        public override int TotalCountToComplete()
        {
            return 0;
        }

        private void OnFuelEnded()
        {
            _completed = true;

            OnCompleted?.Invoke(this);
        }
    }
}