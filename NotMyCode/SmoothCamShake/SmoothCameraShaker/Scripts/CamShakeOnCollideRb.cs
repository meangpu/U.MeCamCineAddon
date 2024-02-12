using System.ComponentModel;
using UnityEngine;
using VInspector;

namespace FirstGearGames.SmoothCameraShaker
{
    public class CamShakeOnCollideRb : CamShakeDataHolder
    {
        [Tooltip("if not use scale it will use 1 as default value")]
        [SerializeField] bool _localScaleInfluenceShake = true;

        [ShowIf("_localScaleInfluenceShake")]
        [SerializeField] float _massScaleMul = .3f;
        [EndIf]

        [SerializeField] bool _velocityInfluenceShake = true;

        [ShowIf("_velocityInfluenceShake")]
        [SerializeField] float _velocityScaleMul = 1f;
        [EndIf]

        [SerializeField] int _maxCollisionCount = 1;
        [SerializeField] int _nowCollisionCount;

        private float _mass;
        private Rigidbody _rb;

        void Awake()
        {
            _nowCollisionCount = _maxCollisionCount;
            _rb = GetComponent<Rigidbody>();
            if (_localScaleInfluenceShake)
            {
                _mass = (transform.localScale.x + transform.localScale.y + transform.localScale.z) * _massScaleMul;
            }
            else
            {
                _mass = 1;
            }
        }

        private void OnCollisionEnter(Collision collision) { DoCollisionEntered(); }
        private void OnCollisionEnter2D(Collision2D collision) { DoCollisionEntered(); }

        private void DoCollisionEntered()
        {
            if (_nowCollisionCount <= 0) return;

            _nowCollisionCount -= 1;
            ShakerInstance instance = CameraShakerHandler.Shake(Data);
            float _finalMultiply = GetShakeMultiplier();
            instance.MultiplyMagnitude(_finalMultiply, -1);
        }

        float GetShakeMultiplier()
        {
            if (_velocityInfluenceShake) return _mass * _rb.velocity.magnitude * _velocityScaleMul;
            else return _mass;
        }

        [Button] public void ResetShake() => _nowCollisionCount = _maxCollisionCount;
    }
}