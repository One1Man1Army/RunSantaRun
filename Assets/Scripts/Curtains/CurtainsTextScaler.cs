using DG.Tweening;
using System.Collections;
using UnityEngine;

namespace RSR.Curtains
{
    public sealed class CurtainsTextScaler : MonoBehaviour
    {
        [SerializeField]
        private float _duration = 0.35f;

        [SerializeField]
        private float _scale = 1.5f;

        private void Start()
        {
            var sequence = DOTween.Sequence()
                .Append(transform.DOScale(Vector3.one * _scale, _duration).SetEase(Ease.InBack))
                .Append(transform.DOScale(Vector3.one, _duration).SetEase(Ease.OutBack));

            sequence.SetLoops(-1, LoopType.Restart);
                
        }
        /*        [SerializeField] private float _scale = 1.35f;
                void Update()
                {
                    Vector3 scale = Vector3.one * Mathf.Abs(Mathf.Sin(Time.time) * _scale);
                    transform.localScale = scale;
                }*/
    }
}