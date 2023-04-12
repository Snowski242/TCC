using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CineMachineShake : MonoBehaviour
{
    public static CineMachineShake Instance { get; private set; }

    private CinemachineVirtualCamera virtualCamera;
    private float shakeTime;
    private void Awake()
    {
        Instance = this;
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelperlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        cinemachineBasicMultiChannelperlin.m_AmplitudeGain = intensity;
        shakeTime = time;
    }

    private void Update()
    {
        if (shakeTime > 0) 
        {
        shakeTime -= Time.deltaTime;
            if (shakeTime <= 0) 
            {
                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelperlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelperlin.m_AmplitudeGain = 0f;

            }
        }
    }
}
