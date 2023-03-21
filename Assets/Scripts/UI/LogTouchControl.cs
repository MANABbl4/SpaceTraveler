using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.UI
{
    public class LogTouchControl : TouchControl
    {
        public override float ChangeOutput(float input)
        {
            return input * input * input;
        }
    }
}
