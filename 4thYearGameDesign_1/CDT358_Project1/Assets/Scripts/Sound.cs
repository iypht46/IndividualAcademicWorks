using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Sound : MonoBehaviour
{
    [SerializeField] AudioClip[] Clips;

    AudioSource m_AudioSource;

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();

        m_AudioSource.volume = SoundPlayer.Instance.GetSfxVol();
    }

    public void PlayAt(int index)
    {
        if (index < Clips.Length)
        {
            m_AudioSource.clip = Clips[index];
            m_AudioSource.Play();
        }
    }

    public void PlayRandom()
    {
        int rnd = Random.Range(0, Clips.Length);
        PlayAt(rnd);
    }
}
