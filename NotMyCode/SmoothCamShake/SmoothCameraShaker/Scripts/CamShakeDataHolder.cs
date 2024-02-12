using UnityEngine;

namespace FirstGearGames.SmoothCameraShaker
{
    public class CamShakeDataHolder : MonoBehaviour
    {
        public ShakeData Data;
        public void DoShake() => CameraShakerHandler.Shake(Data);
    }
}