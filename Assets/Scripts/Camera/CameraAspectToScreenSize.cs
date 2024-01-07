using UnityEngine;

namespace RSR.CameraLogic
{
    [RequireComponent(typeof(Camera))]
    public sealed class CameraAspectToScreenSize : MonoBehaviour
    {
        private void Awake()
        {
            if (Screen.width / Screen.height >= 2)
                SetAspectToScreenSize();
        }

        private void SetAspectToScreenSize()
        {
            GetComponent<Camera>().aspect = Screen.width / Screen.height;
        }
    }
}