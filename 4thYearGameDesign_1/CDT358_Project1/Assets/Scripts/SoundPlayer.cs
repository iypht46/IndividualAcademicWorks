using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public static SoundPlayer Instance { get { return _instance; } }
    private static SoundPlayer _instance;

    [SerializeField] private AudioSource m_BGM;

    float m_SFXvolume = 1f;

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

    public void SetBgmVol(float vol)
    {
        m_BGM.volume = vol;
    }

    public void SetSfxVol(float vol)
    {
        m_SFXvolume = vol;
    }


    public float GetBgmVol()
    {
        return m_BGM.volume;
    }

    public float GetSfxVol()
    {
        return m_SFXvolume;
    }


}
