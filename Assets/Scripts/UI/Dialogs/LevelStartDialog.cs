using Scripts.Tasks;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Dialogs
{
    public class LevelStartDialog : Dialog
    {
        [SerializeField]
        private Button _closeButton = null;

        [SerializeField]
        private Text _text = null;

        [SerializeField]
        private TaskManager _taskManager = null;

        private void Start()
        {
            _closeButton.onClick.AddListener(() => OnClosed());

            var sb = new StringBuilder();
            foreach (var task in _taskManager.GetTasksToComplete())
            {
                sb.Append("- ").AppendLine(task.Description);
            }

            _text.text = sb.ToString();
        }
    }
}
