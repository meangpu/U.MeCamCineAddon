using UnityEngine;
using Cinemachine;

namespace Meangpu
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class CameraShake : MonoBehaviour
    {
        /// <summary>
        /// - Add noise to cinemachine first
        /// - Add 6D noise
        /// - call by "CameraShake.Instance.ShakeCamera(12, 2);"
        /// </summary>
        private CinemachineVirtualCamera cvc;
        CinemachineBasicMultiChannelPerlin perlin;
        float shakeTimer;
        float shakeTotal;
        float startIntensity;
        [SerializeField] float _defaultInten = 12;
        [SerializeField] float _defaultDuration = .8f;

        public static CameraShake Instance;

        private void Awake()
        {
            cvc = GetComponent<CinemachineVirtualCamera>();
            perlin = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = 0;
            if (Instance != null && Instance != this) Destroy(this);
            else Instance = this;
        }

        public void ShakeCamera(float intensity = 12f, float time = .8f)
        {
            perlin.m_AmplitudeGain = intensity;
            startIntensity = intensity;
            shakeTimer = time;
            shakeTotal = time;
        }

        public void ShakeCamera()
        {
            perlin.m_AmplitudeGain = _defaultInten;
            startIntensity = _defaultInten;
            shakeTimer = _defaultDuration;
            shakeTotal = _defaultDuration;
        }

        private void Update()
        {
            if (shakeTimer <= 0) return;
            shakeTimer -= Time.deltaTime;
            perlin.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0f, 1 - (shakeTimer / shakeTotal));
        }
    }
}