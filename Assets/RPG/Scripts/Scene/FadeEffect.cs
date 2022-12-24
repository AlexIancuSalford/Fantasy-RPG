using System.Collections;
using UnityEngine;

namespace RPG.Scene
{
    public class FadeEffect : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Coroutine activeCoroutine = null;

        // Start is called before the first frame update
        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public IEnumerator FadeOut(float time)
        {
            return Fade(1, time);
        }

        public IEnumerator FadeIn(float time)
        {
            return Fade(0, time);
        }

        private IEnumerator FadeOutAndIn()
        {
            yield return FadeOut(3.0f);

            yield return FadeIn(1.0f);
        }

        public void FadeOutImmediately()
        {
            _canvasGroup.alpha = 1;
        }

        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);

                yield return null;
            }
        }

        private IEnumerator Fade(float target, float time)
        {
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }

            activeCoroutine = StartCoroutine(FadeRoutine(target, time));
            yield return activeCoroutine;
        }
    }
}