using UnityEngine;

namespace RPG.Helper
{
    /// <summary>
    /// A script for randomly playing an audio clip with randomly adjusted volume and pitch.
    ///
    /// The class has four public variables:
    /// 
    /// 1. AudioClips: an array of AudioClip objects that can be assigned from the Unity editor.
    /// These audio clips will be played by the script.
    ///
    /// 2. VolumeMultiplier: a float value that can be set in the Unity editor and is used to adjust
    /// the volume of the audio that is played. The value must be between 0.1 and 0.5.
    /// 
    /// 3. PitchMultiplier: a float value that can be set in the Unity editor and is used to adjust
    /// the pitch of the audio that is played. The value must be between 0.1 and 0.5.
    /// 
    /// 4. IsInterruptible: a boolean value that can be set in the Unity editor and determines whether
    /// the audio can be interrupted by other audio.
    ///
    /// The script also has a private variable, audioSource, which is an AudioSource component attached to the same
    /// game object as the script.
    /// 
    /// The script has a Start() function, which is called when the game object is initialized,
    /// and sets the audioSource variable to the AudioSource component attached to the game object.
    /// 
    /// The script has a public function called PlaySound(), which plays a randomly selected
    /// audio clip from the AudioClips array with a randomly adjusted volume and pitch.
    /// If IsInterruptible is true, the audio is played using the Play() function of the AudioSource.
    /// If IsInterruptible is false, the audio is played using the PlayOneShot() function of the AudioSource,
    /// which allows the audio to play without interrupting other audio that may already be playing.
    /// </summary>
    [RequireComponent(typeof(AudioSource))]
    public class CSoundRandomizer : MonoBehaviour
    {
        /// <summary>
        /// An array of audio clips that can be played.
        /// </summary>
        [field : SerializeField] public AudioClip[] AudioClips { get; private set; } = null;

        /// <summary>
        /// A value used to adjust the volume of the audio that is played. The value must be between 0.1 and 0.5.
        /// </summary>
        [field : SerializeField, Range(0.1f, 0.5f)] public float VolumeMultiplier { get; private set; } = .2f;

        /// <summary>
        /// A value used to adjust the pitch of the audio that is played. The value must be between 0.1 and 0.5.
        /// </summary>
        [field : SerializeField, Range(0.1f, 0.5f)] public float PitchMultiplier { get; private set; } = .2f;

        /// <summary>
        /// A boolean value that determines whether the audio can be interrupted by other audio.
        /// </summary>
        [field : SerializeField] public bool IsInterruptible { get; private set; } = true;

        /// <summary>
        /// The AudioSource component attached to the same game object as this script.
        /// </summary>
        private AudioSource audioSource;

        /// <summary>
        /// Sets the audioSource variable to the AudioSource component attached to the game object.
        /// </summary>
        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        /// <summary>
        /// Plays a randomly selected audio clip from the AudioClips array with a randomly adjusted volume and pitch.
        /// If IsInterruptible is true, the audio is played using the Play() function of the AudioSource.
        /// If IsInterruptible is false, the audio is played using the PlayOneShot() function of the AudioSource,
        /// which allows the audio to play without interrupting other audio that may already be playing.
        /// </summary>
        public void PlaySound()
        {
            // If there are no sounds in the array, log an error and return.
            if (AudioClips.Length == 0)
            {
                Debug.LogError("No audio clips found in the array");
                return;
            }

            // Set the audioSource's clip to a randomly selected audio clip from the AudioClips array.
            audioSource.clip = AudioClips[Random.Range(0, AudioClips.Length)];
            // Set the audioSource's volume to a random value between 1 - VolumeMultiplier and 1.
            audioSource.volume = Random.Range(1 - VolumeMultiplier, 1);
            // Set the audioSource's pitch to a random value between 1 - PitchMultiplier and 1 + PitchMultiplier.
            audioSource.pitch = Random.Range(1 - PitchMultiplier, 1 + PitchMultiplier);

            if (IsInterruptible)
            {
                // If IsInterruptible is true, play the audio using the Play() function of the audioSource.
                audioSource.Play();
            }
            else
            {
                // If IsInterruptible is false, play the audio using the PlayOneShot() function of the audioSource
                // that will not interrupt a sound to play another.
                audioSource.PlayOneShot(audioSource.clip);
            }
        }
    }
}
