using System.Collections;
using RPG.Save;
using UnityEngine;

namespace RPG.Scene
{
    public class SaveWrapper : MonoBehaviour
    {
        public const string _defaultSaveFile = "save";
        private float _fadeInTime = 1f;

        private FadeEffect FadeEffect { get; set; }

        private void Awake()
        {
            FadeEffect = FindObjectOfType<FadeEffect>();
        }

        private IEnumerator Start()
        {
            FadeEffect.FadeOutImmediately();
            yield return GetComponent<SaveManager>().LoadLastScene(_defaultSaveFile);
            yield return FadeEffect.FadeIn(_fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }
        }

        public void Save()
        {
            GetComponent<SaveManager>().Save(_defaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SaveManager>().Load(_defaultSaveFile);
        }
    }
}
