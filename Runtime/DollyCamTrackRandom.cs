using System.Collections;
using UnityEngine;

namespace Meangpu
{
    public class DollyCamTrackRandom : MonoBehaviour
    {
        [SerializeField] Cinemachine.CinemachineDollyCart _cart;
        [SerializeField] Cinemachine.CinemachineVirtualCamera _virCam;

        [SerializeField] Vector2 _waitTimeRandom = new(3, 7);
        [SerializeField] Vector2 _positionRandom = new(0, 0.04f);
        [SerializeField] Vector2 _speedRandom = new(0.08f, 0.15f);

        public Cinemachine.CinemachineSmoothPath startPath;
        public TrackWithTargetLookAt[] alterPath;

        void Start() => ResetPath();

        void ResetPath()
        {
            StopAllCoroutines();
            _cart.m_Path = startPath;
            StartCoroutine(ChangeTrack());
        }

        IEnumerator ChangeTrack()
        {
            yield return new WaitForSeconds(Random.Range(_waitTimeRandom.x, _waitTimeRandom.y));
            _cart.m_Position = Random.Range(_positionRandom.x, _positionRandom.y);
            _cart.m_Speed = Random.Range(_speedRandom.x, _speedRandom.y);
            TrackWithTargetLookAt nowTargetPath = alterPath[Random.Range(0, alterPath.Length)];

            _cart.m_Path = nowTargetPath.path;
            _virCam.LookAt = nowTargetPath.targetPrefab;
            StartCoroutine(ChangeTrack());
        }

        [System.Serializable]
        public class TrackWithTargetLookAt
        {
            public Cinemachine.CinemachineSmoothPath path;
            public Transform targetPrefab;
        }
    }
}