using DG.Tweening;
using UnityEngine;

namespace RSR.Curtain
{
    public sealed class CurtainsView : MonoBehaviour, ICurtainsView
    {
        [SerializeField] private CanvasGroup _loading;
        [SerializeField] private CanvasGroup _intro;
        [SerializeField] private CanvasGroup _outro;

        private float _fadeTime = 0.33f;

        public void ShowCurtain(CurtainType curtainType)
        {
            HideCurtains();

            switch (curtainType)
            {
                case CurtainType.Loading:
                    TurnOnWithFade(_loading); 
                    break;
                case CurtainType.Intro:
                    TurnOnWithFade(_intro);
                    break;
                case CurtainType.Outro:
                    TurnOnWithFade(_outro);
                    break;
            }
        }

        private void TurnOnWithFade(CanvasGroup curtain)
        {
            curtain.gameObject.SetActive(true);
            curtain.DOFade(1f, _fadeTime);
        }

        private void TurnOffWithFade(CanvasGroup curtain)
        {
            if (curtain.gameObject.activeInHierarchy)
            {
                curtain.DOFade(0f, _fadeTime).onComplete += () =>
                {
                    curtain.gameObject.SetActive(false);
                };
            }
        }

        public void HideCurtains()
        {
            TurnOffWithFade(_loading);
            TurnOffWithFade(_intro);
            TurnOffWithFade(_outro);
        }

        public void SetFadeTime(float time)
        {
            _fadeTime = time;
        }
    }
}