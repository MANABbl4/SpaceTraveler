using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class LevelsButtonManager : MonoBehaviour
    {
        public Action<string> LevelChoosedEvent;

        [System.Serializable]
        public class LevelButtonInfo
        {
            [SerializeField]
            public Button Button;

            [SerializeField]
            public string SceneName;
        }

        [SerializeField]
        private List<LevelButtonInfo> _levels = null;

        // Start is called before the first frame update
        private void Start()
        {
            if (_levels != null)
            {
                foreach (var level in _levels)
                {
                    Action buttonClicker = () => OnButtonClicked(level);
                    level.Button.onClick.AddListener(() => buttonClicker());
                }
            }
        }

        // Update is called once per frame
        private void Update()
        {

        }

        // Update is called once per frame
        private void OnButtonClicked(LevelButtonInfo button)
        {
            if (LevelChoosedEvent != null)
            {
                LevelChoosedEvent.Invoke(button.SceneName);
            }
        }
    }
}