using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VInspector;

namespace Meangpu
{
    public class DollyCamTrackRandom : MonoBehaviour
    {
        [SerializeField] Cinemachine.CinemachineDollyCart _cart;
        [SerializeField] Cinemachine.CinemachineVirtualCamera _virCam;

        [SerializeField] Vector2 _waitTimeRandom = new(3, 7);
        [SerializeField] Vector2 _positionRandom = new(0, 0.04f);
        [SerializeField] Vector2 _speedRandom = new(0.08f, 0.15f);
        [SerializeField] bool _isUseWaitRealTime;

        public List<TrackWithTargetLookAt> PathOption = new();
        List<TrackWithTargetLookAt> _nowAvailablePath = new();
        TrackWithTargetLookAt _lastPath;

        void Start() => ResetPath();

        void ResetPath()
        {
            StopAllCoroutines();
            ChangeToRandomTrack();
            StartCoroutine(ChangeTrack());
        }

        IEnumerator ChangeTrack()
        {
            if (_isUseWaitRealTime) yield return new WaitForSecondsRealtime(Random.Range(_waitTimeRandom.x, _waitTimeRandom.y));
            else yield return new WaitForSeconds(Random.Range(_waitTimeRandom.x, _waitTimeRandom.y));
            ChangeToRandomTrack();
            StartCoroutine(ChangeTrack());
        }

        [Button]
        private void ChangeToRandomTrack()
        {
            _cart.m_Position = Random.Range(_positionRandom.x, _positionRandom.y);
            _cart.m_Speed = Random.Range(_speedRandom.x, _speedRandom.y);

            if (_nowAvailablePath.Count == 0) SetupPathOption();
            int nowIndex = Random.Range(0, _nowAvailablePath.Count);

            TrackWithTargetLookAt nowPath = SetupCamAndPathByIndex(nowIndex);
            _lastPath = nowPath;

            if (_nowAvailablePath.Contains(nowPath)) _nowAvailablePath.Remove(nowPath);
        }


        private TrackWithTargetLookAt SetupCamAndPathByIndex(int nowIndex)
        {
            TrackWithTargetLookAt nowPath = _nowAvailablePath[nowIndex];
            _cart.m_Path = nowPath.path;
            if (nowPath.targetPrefab != null) _virCam.LookAt = nowPath.targetPrefab;
            return nowPath;
        }

        private void SetupPathOption()
        {
            _nowAvailablePath.Clear();
            _nowAvailablePath = new(PathOption);
            if (_nowAvailablePath.Count > 1)
            {
                _nowAvailablePath.Remove(_lastPath);
            }
        }

        [System.Serializable]
        public class TrackWithTargetLookAt
        {
            public Cinemachine.CinemachineSmoothPath path;
            public Transform targetPrefab;
        }
    }
}