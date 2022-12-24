using System.Collections;
using UnityEngine;

namespace RPG.Scene
{
    /// <summary>
    /// This script is a Unity component that allows you to fade in or out a canvas element over a specified time period.
    /// The component has a CanvasGroup field called _canvasGroup which is used to control the alpha property of the canvas element.
    /// 
    /// The Awake() method is called when the component is initialized, and it assigns the _canvasGroup field to the
    /// CanvasGroup component of the same GameObject that the script is attached to.
    /// 
    /// The FadeOut() and FadeIn() methods return coroutines that can be used to fade out or in the canvas element over a
    /// specified time period. They both use the private Fade() method to achieve this.
    /// 
    /// The FadeOutAndIn() method returns a coroutine that fades out the canvas element over 3 seconds, then fades it back in over 1 second.
    /// 
    /// The FadeOutImmediately() method sets the alpha of the canvas element to 1 immediately, making it fully opaque.
    /// 
    /// The FadeRoutine() method is a coroutine that gradually changes the alpha of the canvas element towards a target value over
    /// a specified time period. It does this by using the Mathf.MoveTowards() method in a loop until the target alpha value is reached.
    /// 
    /// The Fade() method is used to start a fade coroutine, and it also includes logic to stop any previously running fade
    /// coroutines before starting a new one. This ensures that only one fade coroutine is active at a time.
    /// </summary>
    public class FadeEffect : MonoBehaviour
    {
        private CanvasGroup _canvasGroup;
        private Coroutine activeCoroutine = null;

        // Start is called before the first frame update
        void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// Returns a coroutine that fades out the canvas element over a specified time period.
        /// </summary>
        /// <param name="time">The time period over which to fade out the canvas element.</param>
        /// <returns>A coroutine that fades out the canvas element over the specified time period.</returns>
        public IEnumerator FadeOut(float time)
        {
            return Fade(1, time);
        }

        /// <summary>
        /// Returns a coroutine that fades in the canvas element over a specified time period.
        /// </summary>
        /// <param name="time">The time period over which to fade in the canvas element.</param>
        /// <returns>A coroutine that fades in the canvas element over the specified time period.</returns>
        public IEnumerator FadeIn(float time)
        {
            return Fade(0, time);
        }

        /// <summary>
        /// Returns a coroutine that fades out the canvas element over 3 seconds, then fades it back in over 1 second.
        /// </summary>
        /// <returns>A coroutine that fades out the canvas element over 3 seconds, then fades it back in over 1 second.</returns>
        private IEnumerator FadeOutAndIn()
        {
            yield return FadeOut(3.0f);

            yield return FadeIn(1.0f);
        }

        /// <summary>
        /// Makes the canvas element fully opaque immediately.
        /// </summary>
        public void FadeOutImmediately()
        {
            _canvasGroup.alpha = 1;
        }

        /// <summary>
        /// A coroutine that gradually changes the alpha of the canvas element towards a target value over a specified time period.
        /// </summary>
        /// <param name="target">The target alpha value to fade towards.</param>
        /// <param name="time">The time period over which to fade the canvas element.</param>
        /// <returns>A coroutine that gradually changes the alpha of the canvas element towards the specified target value over the specified time period.</returns>
        private IEnumerator FadeRoutine(float target, float time)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, target))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);

                yield return null;
            }
        }

        /// <summary>
        /// A coroutine that gradually changes the alpha of the canvas element towards a target value over a specified time period.
        /// If another fade coroutine is already active, it will be stopped before starting the new fade.
        /// </summary>
        /// <param name="target">The target alpha value to fade towards.</param>
        /// <param name="time">The time period over which to fade the canvas element.</param>
        /// <returns>A coroutine that gradually changes the alpha of the canvas element towards the specified target value over the specified time period.</returns>
        private IEnumerator Fade(float target, float time)
        {
            // Stop any active fade coroutines before starting a new one
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }

            // Start the fade coroutine and store it in the activeCoroutine field
            activeCoroutine = StartCoroutine(FadeRoutine(target, time));
            // Wait for the fade coroutine to complete before continuing
            yield return activeCoroutine;
        }
    }
}