using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.UI.Dialogs
{
    public class DialogManager : MonoBehaviour
    {
        public Action OnGameShouldBePaused;
        public Action OnGameShouldBeResumed;
        public Action OnRestartLevelEvent;
        public Action<string> OnGoToSceneEvent;
        public Action OnGoToNextSceneEvent;
        public Action OnGameExitEvent;

        [SerializeField]
        private string _mainSceneName = "MainScene";
        [SerializeField]
        private PauseDialog _pauseDialog = null;
        [SerializeField]
        private LevelStartDialog _levelStartDialog = null;
        [SerializeField]
        private LevelCompletedDialog _levelCompletedDialog = null;
        [SerializeField]
        private LevelFailedDialog _levelFailedDialog = null;

        private Dictionary<DialogEnum, Dialog> _dialogs = new Dictionary<DialogEnum, Dialog>();
        private Stack<KeyValuePair<DialogEnum, Dialog>> _activeDialogs = new Stack<KeyValuePair<DialogEnum, Dialog>>();

        private void Awake()
        {
            _dialogs.Add(DialogEnum.LevelStart, _levelStartDialog);
            _dialogs.Add(DialogEnum.Pause, _pauseDialog);
            _dialogs.Add(DialogEnum.LevelCompleted, _levelCompletedDialog);
            _dialogs.Add(DialogEnum.LevelFailed, _levelFailedDialog);

            _pauseDialog.OnContinueEvent += () => OnDialogClosed(DialogEnum.Pause);
            _pauseDialog.OnExitEvent += () => OnGoToSceneEvent(_mainSceneName);
            _pauseDialog.OnRestartEvent += () => OnRestartLevelEvent();

            _levelCompletedDialog.OnExitEvent += () => OnGoToSceneEvent(_mainSceneName);
            _levelCompletedDialog.OnNextEvent += () => OnGoToNextSceneEvent();
            _levelCompletedDialog.OnRestartEvent += () => OnRestartLevelEvent();

            _levelFailedDialog.OnExitEvent += () => OnGoToSceneEvent(_mainSceneName);
            _levelFailedDialog.OnRestartEvent += () => OnRestartLevelEvent();

            foreach (var dialog in _dialogs)
            {
                dialog.Value.gameObject.SetActive(false);
            }
        }

        public void ShowDialog(DialogEnum id)
        {
            if (_dialogs.ContainsKey(id))
            {
                var dialog = _dialogs[id];
                _activeDialogs.Push(new KeyValuePair<DialogEnum, Dialog>(id, dialog));
                dialog.OnClosed += () => OnDialogClosed(id);
                dialog.gameObject.SetActive(true);
                /*var zPos = dialog.Rect.localPosition;
                zPos.z = _activeDialogs.Count;
                dialog.Rect.localPosition = zPos;*/
                //dialog.Rect.anchoredPosition3D = new Vector3(dialog.Rect.anchoredPosition3D.x, dialog.Rect.anchoredPosition3D.y, zPos);

                if (OnGameShouldBePaused != null)
                {
                    OnGameShouldBePaused.Invoke();
                }
            }
        }

        private void OnDialogClosed(DialogEnum id)
        {
            var dialog = _activeDialogs.Pop();
            if (dialog.Key != id)
            {
                throw new Exception("OnDialogClosed exception");
            }

            dialog.Value.OnClosed = null;
            dialog.Value.gameObject.SetActive(false);

            if (_activeDialogs.Count <= 0 && OnGameShouldBeResumed != null)
            {
                OnGameShouldBeResumed.Invoke();
            }
        }
    }
}
