using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] Slider m_BGMslider;
    [SerializeField] Slider m_SFXslider;

    [SerializeField] Text m_BgmText;
    [SerializeField] Text m_SfxText;


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

        m_BgmText.text = ((int)(m_BGMslider.value * 100)).ToString();
        m_SfxText.text = ((int)(m_SFXslider.value * 100)).ToString();
    }
}
