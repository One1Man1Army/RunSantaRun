using UnityEngine;

namespace RSR.InternalLogic
{
    public static class DataExtensions
    {
        public static T GetComponentWithAdd<T>(this GameObject gameObject) where T : Component
        {
            var component = gameObject.GetComponent<T>() ?? gameObject.AddComponent<T>();
            return component;
        }
    }
}