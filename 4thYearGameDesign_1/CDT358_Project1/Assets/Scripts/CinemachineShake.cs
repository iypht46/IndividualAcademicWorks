using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake instance { get; private set; }

    private CinemachineVirtualCamera m_cinemachineVirtual;
    private float m_ShakeTimer;
    private float m_ShakeTimerTotal;
    private float m_StartingIntensity;

    private void Awake()
    {
        instance = this;
        m_cinemachineVirtual = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = m_cinemachineVirtual.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;

        m_StartingIntensity = intensity;
        m_ShakeTimer = time;
        m_ShakeTimerTotal = time;
    }

    private void Update()
    {
        if (m_ShakeTimer > 0)
        {
            m_ShakeTimer -= Time.deltaTime;

            CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin = m_cinemachineVirtual.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = Mathf.Lerp(m_StartingIntensity, 0f, 1 - (m_ShakeTimer / m_ShakeTimerTotal));

        }
    }
}
