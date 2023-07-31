using UnityEngine;
using Cinemachine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cvc;
    CinemachineBasicMultiChannelPerlin perlin;
    float shakeTimer;
    float shakeTotal;
    float startIntensity;

    [SerializeField] float _intensity = 12f;
    [SerializeField] float _duration = .8f;

    public static CameraShake Instance;

    private void Awake()
    {
        cvc = GetComponent<CinemachineVirtualCamera>();
        perlin = cvc.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        if (Instance != null && Instance != this) Destroy(this);
        else Instance = this;

        // CameraShake.Instance.ShakeCamera(12, 2);
    }

    public void ShakeCamera(float intensity, float time)
    {
        perlin.m_AmplitudeGain = intensity;
        startIntensity = intensity;
        shakeTimer = time;
        shakeTotal = time;
    }

    private void Update()
    {
        if (shakeTimer <= 0) return;
        shakeTimer -= Time.deltaTime;
        perlin.m_AmplitudeGain = Mathf.Lerp(startIntensity, 0f, 1 - (shakeTimer / shakeTotal));
    }

    void ShakeOnWithValue(int dmg)
    {
        ShakeCamera(_intensity * dmg, _duration);
    }

}
