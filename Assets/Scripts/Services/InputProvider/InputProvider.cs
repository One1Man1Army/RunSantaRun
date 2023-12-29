using RSR.InternalLogic;
using System;
using UnityEngine;

namespace RSR.ServicesLogic
{
    public sealed class InputProvider : IInputProvider
    {
        public bool IsFingerDown()
        {
            return Input.GetMouseButtonDown(0);
        }
    }
}
