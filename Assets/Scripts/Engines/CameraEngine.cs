using UnityEngine;

public class CameraEngine : Singelton<CameraEngine>
{
    public Camera MainCamera { get; private set; }
    public float KillZone { get; private set; }

    private void Awake()
    {
        MainCamera = GetComponent<Camera>();
    }

    private void Start()
    {
        KillZone = -MainCamera.orthographicSize;

        var height = 2 * Camera.main.orthographicSize;
        var width = height * Camera.main.aspect;
    }
}
