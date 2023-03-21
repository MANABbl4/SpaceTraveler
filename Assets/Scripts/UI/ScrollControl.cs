using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UI
{
    public class ScrollControl : MonoBehaviour
    {
        public Action<float> ScrollEvent;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            var scroll = Input.GetAxis("Mouse ScrollWheel");
            if (scroll != 0f && ScrollEvent != null)
            {
                ScrollEvent.Invoke(scroll);
            }
        }
    }
}