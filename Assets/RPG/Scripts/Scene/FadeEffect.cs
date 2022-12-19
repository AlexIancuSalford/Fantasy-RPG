using System.Collections;
using UnityEngine;

namespace RPG.Scene
{
    public class FadeEffect : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;

        // Start is called before the first frame update
        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public IEnumerator FadeOut(float time)
        {
            while (!_canvasGroup.alpha.Equals(1))
            {
                _canvasGroup.alpha += Time.deltaTime / time;

                yield return null;
            }
        }

        public IEnumerator FadeIn(float time)
        {
            while (!_canvasGroup.alpha.Equals(0))
            {
                _canvasGroup.alpha -= Time.deltaTime / time;

                yield return null;
            }
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
    }
}