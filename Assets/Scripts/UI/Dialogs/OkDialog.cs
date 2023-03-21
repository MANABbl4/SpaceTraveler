using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI.Dialogs
{
    public class OkDialog : Dialog
    {
        [SerializeField]
        private Button _closeButton = null;

        private void Start()
        {
            _closeButton.onClick.AddListener(() => OnClosed());
        }
    }
}
