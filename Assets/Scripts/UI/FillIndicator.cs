using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class FillIndicator : MonoBehaviour
    {
        [SerializeField]
        private Image Background = null;

        [SerializeField]
        private Image Indicator = null;

        [SerializeField]
        private Text TextValue = null;

        private float _value = 1.0f;

        // Start is called before the first frame update
        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {

        }

        public void SetValue(float value)
        {
            if (value < 0f)
            {
                value = 0f;
            }

            if (value > 1.0f)
            {
                value = 1.0f;
            }

            _value = value;

            Indicator.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _value * Background.rectTransform.rect.height);
            TextValue.text = (int)(_value * 100) + "%";
        }

        public float GetValue()
        {
            return _value;
        }
    }
}
