using System;
using Cinemachine;
using UnityEngine;

namespace DefaultNamespace.Additional
{
    public class CameraShake : MonoBehaviour
    {
        public static CameraShake Instance;
        [SerializeField] private CinemachineVirtualCamera Camera;

        private void Start()
        {
            _perlin = Camera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        }

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
        private CinemachineBasicMultiChannelPerlin _perlin;

        public void ShakeCamera(float intensity, float time)
        {
            _perlin.m_AmplitudeGain = intensity;
            timer = time;
        }

        private void Update()
        {
            if (timer <= 0) return;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                _perlin.m_AmplitudeGain = 0;
            }
        }
    }
}