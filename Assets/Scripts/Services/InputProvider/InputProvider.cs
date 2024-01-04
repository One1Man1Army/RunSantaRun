using UnityEngine;

namespace RSR.ServicesLogic
{
    public sealed class InputProvider : IInputProvider
    {
        public bool HasPlayerTapped()
        {
            return Input.GetMouseButtonDown(0);
        }
    }
}
