using System.Collections;
using UnityEngine;

[System.Serializable]
public class TrackAndPref
{
    public Cinemachine.CinemachineSmoothPath path;
    public Transform targetPrefab;
}


public class DollyCamTrackRandom : MonoBehaviour
{
    [SerializeField] Cinemachine.CinemachineDollyCart _cart;
    [SerializeField] Cinemachine.CinemachineVirtualCamera _virCam;

    [SerializeField] Vector2 _waitTimeRandom = new Vector2(3, 7);
    [SerializeField] Vector2 _positionRandom = new Vector2(0, 0.04f);
    [SerializeField] Vector2 _speedRandom = new Vector2(0.08f, 0.15f);

    public Cinemachine.CinemachineSmoothPath startPath;
    public TrackAndPref[] alterPath;

    int _previousNum;
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


        var path = alterPath[getNotOldNum()];

        _cart.m_Path = path.path;
        _virCam.LookAt = path.targetPrefab;
        StartCoroutine(ChangeTrack());

    }

    int getNotOldNum()
    {
        int newIndex = Random.Range(0, alterPath.Length);

        while (newIndex == _previousNum) // find another way some day
        {
            newIndex = Random.Range(0, alterPath.Length);
        }

        _previousNum = newIndex;
        return newIndex;
    }


}
