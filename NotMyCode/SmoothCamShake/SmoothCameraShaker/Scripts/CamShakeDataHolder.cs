using UnityEngine;
using VInspector;

namespace FirstGearGames.SmoothCameraShaker
{
    public class CamShakeDataHolder : MonoBehaviour
    {
        public ShakeData Data;
        [Button] public void DoShake() => CameraShakerHandler.Shake(Data); // or Data.DoShakeThisData() can also work
    }
}