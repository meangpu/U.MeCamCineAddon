All credit to - Email: firstgeargames@gmail.com

All Asset and code From:
[Smooth Camera Shaker | Camera | Unity Asset Store](https://assetstore.unity.com/packages/tools/camera/smooth-camera-shaker-162991)

None of script in this folder is my original code, but I modify some of it to suit my need.

call shake by

```C
[SerializeField] private ShakeData _shakeData = null;

void Shake()
{
    ShakerInstance instance = CameraShakerHandler.Shake(_shakeData);
    CameraShakerHandler.Shake(_shakeData);
}
```
