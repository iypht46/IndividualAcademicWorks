using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] Slider m_BGMslider;
    [SerializeField] Slider m_SFXslider;

    private void Start()
    {
        m_BGMslider.value = SoundPlayer.Instance.GetBgmVol();
        m_SFXslider.value = SoundPlayer.Instance.GetSfxVol();
    }

    // Update is called once per frame
    void Update()
    {
        SoundPlayer.Instance.SetBgmVol(m_BGMslider.value);
        SoundPlayer.Instance.SetSfxVol(m_SFXslider.value);
    }
}
