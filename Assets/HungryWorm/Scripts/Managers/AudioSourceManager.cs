using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace HungryWorm
{
    public class AudioSourceManager : MonoBehaviour
    {
        [SerializeField] private int m_StartingSourceCount = 5;
        [SerializeField] private AudioMixerGroup audioMixerGroup;
        private List<AudioSource> m_AudioSources;

        private void Start()
        {
            m_AudioSources = new List<AudioSource>();
            for (int i = 0; i < m_StartingSourceCount; i++)
            {
                AudioSource audioSource = gameObject.AddComponent<AudioSource>();
                audioSource.playOnAwake = false;
                audioSource.loop = false;
                // Set the output to SFX group
                audioSource.outputAudioMixerGroup = audioMixerGroup; 
                
                m_AudioSources.Add(audioSource);
            }
        }
        
        public AudioSource PlayClip(AudioClip clip)
        {
            AudioSource source = GetAvailableSource();
            source.clip = clip;
            source.Play();
            return source;
        }
        
        private AudioSource GetAvailableSource()
        {
            foreach (AudioSource source in m_AudioSources)
            {
                if (!source.isPlaying)
                {
                    return source;
                }
            }
            // If no available source, create a new one
            AudioSource newSource = gameObject.AddComponent<AudioSource>();
            newSource.playOnAwake = false;
            newSource.loop = false;
            newSource.outputAudioMixerGroup = audioMixerGroup;
            m_AudioSources.Add(newSource);
            return newSource;
        }
    }
}