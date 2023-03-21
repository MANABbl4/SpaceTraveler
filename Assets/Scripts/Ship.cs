using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Scripts.UI;

namespace Scripts
{
    public class Ship : MonoBehaviour
    {
        public Action<Vector3> AccelerationChangedEvent;
        public Action<Vector3> AccelerationChangingEvent;
        public Action FuelEndedEvent;
        public Action FuelChangedEvent;

        [SerializeField]
        private TouchControl Control = null;

        [SerializeField]
        private FillIndicator FuelIndicator = null;

        [SerializeField]
        private float MaxAcceleration = 0.1f;

        [SerializeField]
        private float MaxAccelerationFuelConsumption = 0.1f;

        [SerializeField]
        private float MaxFuel = 1f;

        [SerializeField]
        private float Fuel = 1f;

        private Vector3 _tapPosition;

        // Start is called before the first frame update
        private void Start()
        {
            Control.ChangingEvent += OnChanging;
            Control.ChangedEvent += OnChanged;

            UpdateFuel(Vector3.zero);
        }

        private void OnChanging(Vector3 dir)
        {
            var acc = dir * MaxAcceleration;
            AccelerationChangingEvent?.Invoke(acc);

            UpdateFuel(acc);
        }

        private void OnChanged(Vector3 dir)
        {
            var acc = dir * MaxAcceleration;
            AccelerationChangedEvent?.Invoke(acc);

            UpdateFuel(acc);
        }

        private void UpdateFuel(Vector3 acc)
        {
            Fuel -= MaxAccelerationFuelConsumption * Time.deltaTime * acc.magnitude / MaxAcceleration;

            if (Fuel <= 0f)
            {
                Fuel = 0f;

                Control.ChangingEvent -= OnChanging;
                Control.ChangedEvent -= OnChanged;

                Control.Reset();

                AccelerationChangedEvent?.Invoke(acc);
                FuelEndedEvent?.Invoke();
            }
            else
            {
                FuelChangedEvent?.Invoke();
            }

            FuelIndicator.SetValue(Fuel / MaxFuel);
        }

        // Update is called once per frame
        private void Update()
        {

        }

        public float GetFuel()
        {
            return Fuel;
        }

        public float GetMaxFuel()
        {
            return MaxFuel;
        }
    }
}