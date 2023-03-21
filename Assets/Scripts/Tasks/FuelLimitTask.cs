using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Scripts.Tasks
{
    public class FuelLimitTask : Task
    {
        [SerializeField]
        private int _fuelLimitSpentPercent = 0;

        [SerializeField]
        private Ship _playerShip = null;

        private float _currentFuelSpent = 0;
        private float _startFuelAmount = 0;

        private void Start()
        {
            _currentFuelSpent = 0;
            _startFuelAmount = _playerShip.GetFuel();
        }

        private void Update()
        {
            var wasCompleted = Completed();

            _currentFuelSpent = _startFuelAmount - _playerShip.GetFuel();

            if (!wasCompleted && Completed())
            {
                OnCompleted?.Invoke(this);
            }
        }

        public override bool Completed()
        {
            return _currentFuelSpent * 100 / _playerShip.GetFuel() < _fuelLimitSpentPercent;
        }

        public override int CompletedCount()
        {
            return (int)_currentFuelSpent;
        }

        public override int TotalCountToComplete()
        {
            return _fuelLimitSpentPercent;
        }
    }
}