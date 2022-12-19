using System.Collections;
using System.Reflection;
using RPG.Save;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPG.Scene
{
    public class SaveWrapper : MonoBehaviour
    {
        public const string _defaultSaveFile = "save";
        private float _fadeInTime = 2f;

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
            Load();
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
