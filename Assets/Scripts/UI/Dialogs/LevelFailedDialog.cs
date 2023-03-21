using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Dialogs
{
    public class LevelFailedDialog : Dialog
    {
        public Action OnRestartEvent;
        public Action OnExitEvent;

        [SerializeField]
        private Button _repeat = null;
        [SerializeField]
        private Button _mainMenu = null;

        [SerializeField]
        private RectTransform _repeatHorizontal = null;
        [SerializeField]
        private RectTransform _mainMenuHorizontal = null;

        [SerializeField]
        private RectTransform _repeatVertical = null;
        [SerializeField]
        private RectTransform _mainMenuVertical = null;

        private RectTransform _repeatRect = null;
        private RectTransform _mainMenuRect = null;

        private Resolution _resolution;

        // Start is called before the first frame update
        private void Start()
        {
            _repeat.onClick.AddListener(() => OnRestartEvent());
            _mainMenu.onClick.AddListener(() => OnExitEvent());

            _resolution = new Resolution();
            _resolution.height = Camera.main.pixelHeight;
            _resolution.width = Camera.main.pixelWidth;

            ChangeDialog();
        }

        // Update is called once per frame
        private void Update()
        {
            if (_resolution.height != Camera.main.pixelHeight ||
                _resolution.width != Camera.main.pixelWidth)
            {
                ChangeDialog();

                _resolution.height = Camera.main.pixelHeight;
                _resolution.width = Camera.main.pixelWidth;
            }
        }

        private void ChangeDialog()
        {
            _repeatRect = _repeat.gameObject.GetComponent<RectTransform>();
            _mainMenuRect = _mainMenu.gameObject.GetComponent<RectTransform>();

            if (_resolution.height > _resolution.width)
            {
                SetPosition(_repeatRect, _repeatVertical);
                SetPosition(_mainMenuRect, _mainMenuVertical);
            }
            else
            {
                SetPosition(_repeatRect, _repeatHorizontal);
                SetPosition(_mainMenuRect, _mainMenuHorizontal);
            }
        }

        private void OnEnable()
        {
            _resolution.height = Camera.main.pixelHeight;
            _resolution.width = Camera.main.pixelWidth;

            ChangeDialog();
        }

        private void SetPosition(RectTransform to, RectTransform from)
        {
            to.anchoredPosition = from.anchoredPosition;
            to.localPosition = from.localPosition;
        }
    }
}
