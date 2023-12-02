using System;
using Cinemachine;
using UnityEngine;

namespace DefaultNamespace.Additional
{
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance;
        [SerializeField] private CinemachineVirtualCamera Camera;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private float timer;

        public void ShakeCamera(float intensity, float time)
        {
            var perlin = Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = intensity;
            timer = time;
        }

        private void Update()
        {
            if (timer <= 0) return;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                var perlin = Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = 0;
            }
        }
    }
}