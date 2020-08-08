using UnityEngine;

public class ResourceManager : Singelton<ResourceManager>
{
    #region PROPERTIES

    public BallEngine BallPrefab
    {
        get;
        private set;
    }
    public Line LinePrefab
    {
        get;
        private set;
    }
    public Effect ShatterEffect
    {
        get;
        private set;
    }
    public GameObject PlacementCircle
    {
        get;
        private set;
    }
    public Collectable Collectable
    {
        get;
        private set;
    }

    #endregion PROPERTIES

    #region UNITY_FUNCTIONS

    private void Awake()
    {
        Initialize();
    }

    #endregion UNITY_FUNCTIONS

    #region CUSTOM_FUNCTIONS

    private void Initialize()
    {
        BallPrefab = GetFromResources<BallEngine>("Prefabs/LevelElements/", "Ball");
        LinePrefab = GetFromResources<Line>("Prefabs/LevelElements/", "Line");
        ShatterEffect = GetFromResources<Effect>("Prefabs/Effects/", "ShatterEffect");
        PlacementCircle = GetFromResources<GameObject>("Prefabs/LevelElements/", "PlacementCircle");
        Collectable = GetFromResources<Collectable>("Prefabs/LevelElements/", "Collectable");
    }

    public T GetFromResources<T>(string resourcePath, string resourceName) where T : Object
    {
        return Resources.Load<T>(resourcePath + resourceName);
    }

    public T[] GetFromResourcesAsArray<T>(string resourcePath) where T : Object
    {
        return Resources.LoadAll<T>(resourcePath);
    }

    #endregion CUSTOM_FUNCTIONS
}
