using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Scripts.UI.Dialogs;

namespace Scripts.UI.Dialogs
{
    public class PauseDialog : Dialog
    {
        public Action OnContinueEvent;
        public Action OnRestartEvent;
        public Action OnExitEvent;

        [SerializeField]
        private Button _continue = null;
        [SerializeField]
        private Button _restart = null;
        [SerializeField]
        private Button _exit = null;

        [SerializeField]
        private RectTransform _continueHorizontal = null;
        [SerializeField]
        private RectTransform _restartHorizontal = null;
        [SerializeField]
        private RectTransform _exitHorizontal = null;

        [SerializeField]
        private RectTransform _continueVertical = null;
        [SerializeField]
        private RectTransform _restartVertical = null;
        [SerializeField]
        private RectTransform _exitVertical = null;

        private RectTransform _continueRect = null;
        private RectTransform _restartRect = null;
        private RectTransform _exitRect = null;

        private Resolution _resolution;

        // Start is called before the first frame update
        private void Start()
        {
            _continue.onClick.AddListener(() => OnContinueEvent());
            _restart.onClick.AddListener(() => OnRestartEvent());
            _exit.onClick.AddListener(() => OnExitEvent());
            
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
            _continueRect = _continue.gameObject.GetComponent<RectTransform>();
            _restartRect = _restart.gameObject.GetComponent<RectTransform>();
            _exitRect = _exit.gameObject.GetComponent<RectTransform>();

            if (_resolution.height > _resolution.width)
            {
                SetPosition(_continueRect, _continueVertical);
                SetPosition(_restartRect, _restartVertical);
                SetPosition(_exitRect, _exitVertical);
            }
            else
            {
                SetPosition(_continueRect, _continueHorizontal);
                SetPosition(_restartRect, _restartHorizontal);
                SetPosition(_exitRect, _exitHorizontal);
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