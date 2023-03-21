using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class GameSpeedButtonManager : MonoBehaviour
    {
        public Action<int> GameSpeedSelectedEvent;

        [System.Serializable]
        public class SpeedButtonInfo
        {
            [SerializeField]
            public Button Button;

            [SerializeField]
            public int SpeedMultiplier;
        }

        [SerializeField]
        private List<SpeedButtonInfo> _gameSpeeds = null;

        // Start is called before the first frame update
        private void Start()
        {
            if (_gameSpeeds != null)
            {
                foreach (var gameSpeed in _gameSpeeds)
                {
                    Action buttonClicker = () => OnButtonClicked(gameSpeed);
                    gameSpeed.Button.onClick.AddListener(() => buttonClicker());
                }
            }
        }

        // Update is called once per frame
        private void Update()
        {

        }

        // Update is called once per frame
        private void OnButtonClicked(SpeedButtonInfo button)
        {
            if (GameSpeedSelectedEvent != null)
            {
                GameSpeedSelectedEvent.Invoke(button.SpeedMultiplier);
            }
        }
    }
}