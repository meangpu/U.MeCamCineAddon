using UnityEngine;

namespace FirstGearGames.SmoothCameraShaker
{
    public class CamShakeOnCollideRb : CamShakeDataHolder
    {
        [Header("setting")]
        [Tooltip("If this not true, then use _massScaleMul as default shake value")]
        [SerializeField] bool _localScaleInfluenceShake = true;
        [SerializeField] bool _velocityInfluenceShake = true;

        [Header("scaleMultiply")]
        [SerializeField] float _massScaleMul = .3f;
        [SerializeField] float _velocityScaleMul = 1f;
        private float _mass;
        private bool _shook = false;
        private Rigidbody _rb;

        void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            if (_localScaleInfluenceShake)
            {
                _mass = (transform.localScale.x + transform.localScale.y + transform.localScale.z) * _massScaleMul;
            }
            else
            {
                _mass = _massScaleMul;
            }
        }

        private void OnCollisionEnter(Collision collision) { DoCollisionEntered(); }
        private void OnCollisionEnter2D(Collision2D collision) { DoCollisionEntered(); }

        private void DoCollisionEntered()
        {
            if (_shook) return;

            _shook = true;
            ShakerInstance instance = CameraShakerHandler.Shake(Data);
            float _finalMultiply = GetShakeMultiplier();
            instance.MultiplyMagnitude(_finalMultiply, -1);
        }

        float GetShakeMultiplier()
        {
            if (_velocityInfluenceShake) return _mass * _rb.velocity.magnitude * _velocityScaleMul;
            else return _mass;
        }

        public void ResetShake() => _shook = false;
    }
}