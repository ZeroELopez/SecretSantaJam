using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

namespace Kari.SoundManagement
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] Sound[] sounds;
        [SerializeField] AudioSource originalSource;
        public static AudioManager instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        public static void PlaySound(string name, AudioSource source = null, string backup = "")
        {
            if (source == null)
                source = instance.originalSource;

            foreach (Sound sound in instance.sounds)
            {
                if (sound.name == name)
                {
                    //Debug.Log(source);
                    //Debug.Log(sound.clip);
                    source.clip = sound.clip;

                    if (sound.clipVariants.Length > 0)
                        source.clip = sound.clipVariants[
                            Mathf.Clamp(Random.Range(0, sound.clipVariants.Length), 0, sound.clipVariants.Length - 1)
                            ];


                    source.volume = Random.Range(sound.volumeMin, sound.volumeMax);
                    source.pitch = Random.Range(sound.pitchMin, sound.pitchMax);
                    if (sound.startingPoints.Length >0)
                        source.timeSamples = sound.startingPoints[Random.Range(0, sound.startingPoints.Length - 1)];

                    source.Play();
                    return;
                }
            }

            if (backup != "")
                PlaySound(backup, source);
        }
    }
    [System.Serializable]
    public class Sound
    {
        public string name;

        public AudioClip clip;

        public AudioClip[] clipVariants;


        [Range(0f, 1f)]
        public float volumeMin = 1;
        [Range(0f, 1f)]
        public float volumeMax = 1;

        [Range(.1f, 3f)]
        public float pitchMin = 1;
        [Range(.1f, 3f)]
        public float pitchMax = 1;

        public int[] startingPoints = new int[1] { 0 };
    }

}
