using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer Instance { get { return _instance; } }
    private static SoundPlayer _instance;

    [SerializeField] private AudioSource m_BGM;
    [SerializeField] private AudioSource m_SFX;
    [SerializeField] private AudioSource m_SFX_1;

    [SerializeField] private AudioClip[] m_audioClips;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public void PlayBGM()
    {
        if (m_BGM.isPlaying) 
            return;

        m_BGM.Play();
    }

    public void StopBGM()
    {
        m_BGM.Stop();
    }

    public void PlaySFX(int index)
    {
        m_SFX.clip = m_audioClips[index];
        m_SFX.Play();
    }
    public void PlaySFX1(int index)
    {
        m_SFX_1.clip = m_audioClips[index];
        m_SFX_1.Play();
    }

    public void SetBgmVol(float vol)
    {
        m_BGM.volume = vol;
    }

    public void SetSfxVol(float vol)
    {
        m_SFX.volume = vol;
        m_SFX_1.volume = vol;
    }

    public float GetBgmVol()
    {
        return m_BGM.volume;
    }

    public float GetSfxVol()
    {
        return m_SFX.volume;
    }


}
