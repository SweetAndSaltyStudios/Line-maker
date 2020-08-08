using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public struct MapElement
{
    public Color elementColorID;
    public GameObject elementPrefab;
}

public class LevelManager : Singelton<LevelManager>
{
    #region VARIABLES  

    private readonly float MAX_ENERGY = 100f;

    [SerializeField]
    private Transform circle;

    public float CurrentEnergy
    {
        get;
        private set;
    }

    public MapElement[] LevelMapElements;

    private LayerMask touchingLayerMask;

    private const float MAX_LINE_LENGHT = 12f;
    private const float MIN_LINE_LENGHT = 2f;
    private const int MAX_LINE_COUNT = 4;

    private int spawnPointIndex;
    private Vector2[] spawnPoints = new Vector2[4];

    private bool isLevelStarted;
    private readonly float startDelay = 2f;
    private readonly float MIN_SPAWN_DELAY = 2f;
    private readonly float MAX_SPAWN_DELAY = 4f;

    private Line currentLine;
    private bool isValidPlacement;
    private bool isGamePaused;

    // private Queue<Line> createdLines = new Queue<Line>();

    #endregion VARIABLES

    #region PROPERTIES

    public Texture2D[] LevelMaps
    {
        get;
        private set;
    }
    public bool IsCorrectLenght
    {
        get
        {
            return currentLine.CurrentLenght <= MAX_LINE_LENGHT && currentLine.CurrentLenght >= MIN_LINE_LENGHT;
        }
    }
    public bool IsColliding
    {
        get
        {
            return currentLine != null ? currentLine.EdgeCollider2D.IsTouchingLayers(touchingLayerMask) : false;     
        }
    }
    public int Collectables
    {
        get;
        private set;
    }


    #endregion PROPERTIES

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        OnLevelStart();

        UIManager.Instance.ChangeUILayout(GAME_STATE.IN_GAME);
    }

    private void Initialize()
    {
        touchingLayerMask = LayerMask.GetMask("Default", "Ball", "Collectable");
        LevelMaps = ResourceManager.Instance.GetFromResourcesAsArray<Texture2D>("LevelMaps");
    }

    private void OnLevelStart()
    {
        var newLevel = LevelMaps[0];
        GenerateMap(newLevel);

        isLevelStarted = true;
        StartSpawn();

        AddEnergy(MAX_ENERGY);
    }

    private void StartSpawn()
    {
        var placementCircle = Instantiate(ResourceManager.Instance.PlacementCircle, transform);
        placementCircle.name = ResourceManager.Instance.PlacementCircle.name;

        StartCoroutine(IStartSpawn(startDelay));
    }

    public void CreateNewLine()
    {
        currentLine = Instantiate(ResourceManager.Instance.LinePrefab);
        // createdLines.Enqueue(currentLine);
        circle.transform.position = currentLine.LineCenterPoint;
    }

    public void UpdateCurrentLine()
    {
        if(currentLine != null)
        {    
            currentLine.UpdateLine(InputManager.Instance.CurrentTouchPoint);

            isValidPlacement = CheckValidPlacement();
            currentLine.CanPlace(isValidPlacement);         
        }      
    }   

    public void TryPlaceLine()
    {
        if(currentLine != null /*&& createdLines.Count < MAX_LINE_COUNT*/)
        {
            if (isValidPlacement)
            {
                currentLine.PlaceLine();             
            }
            else
            {
                // createdLines.Dequeue();
                currentLine.DestroyLine();
            }
        }    
    }

    public void PauseGame()
    {
        isGamePaused = !isGamePaused;

        if (isGamePaused)
        {
            Time.timeScale = 0;
            UIManager.Instance.ChangeUILayout(GAME_STATE.PAUSED);
        }
        else
        {
            Time.timeScale = 1;
            UIManager.Instance.ChangeUILayout(GAME_STATE.IN_GAME);
        }
    }

    public void AddCollectable(int amount)
    {
        Collectables += amount;
        UIManager.Instance.InGamePanel.UpdateCollectableText();
    }

    private bool CheckValidPlacement()
    {
        return IsCorrectLenght && IsColliding.Equals(false);      
    }

    public void AddEnergy(float amount)
    {
        CurrentEnergy += amount;

        if(CurrentEnergy >= MAX_ENERGY)
        {
            CurrentEnergy = MAX_ENERGY;
        }

        if(CurrentEnergy <= 0)
        {
            SceneManager.LoadScene(0);
        }
    }

    private Vector2 GetRandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    private IEnumerator IStartSpawn(float startDelay)
    {
        yield return new WaitForSeconds(startDelay);

        while (isLevelStarted)
        {
            var spawnDelay = Random.Range(MIN_SPAWN_DELAY, MAX_SPAWN_DELAY);
            yield return new WaitForSeconds(spawnDelay);

            Instantiate(ResourceManager.Instance.BallPrefab, GetRandomSpawnPoint(), Quaternion.identity);
        }

        isLevelStarted = false;
    }

    private void GenerateMap(Texture2D levelMapTexture)
    {
        for (int x = 0; x < levelMapTexture.width; x++)
        {
            for (int y = 0; y < levelMapTexture.height; y++)
            {
                GenerateTile(levelMapTexture, x, y);
            }
        }
    }

    private void GenerateTile(Texture2D levelMapTexture, int x, int y)
    {
        var pixelColor = levelMapTexture.GetPixel(x, y);

        var parent = transform.GetChild(0);

        if (pixelColor.a.Equals(0))
        {
            return;
        }

        foreach (var levelMapElement in LevelMapElements)
        {
            if (levelMapElement.elementColorID.Equals(pixelColor))
            {
                if (pixelColor.Equals(Color.green))
                {                
                    var spawnPointElement = SpawnElement(levelMapElement.elementPrefab, new Vector2(x, y), Quaternion.identity, parent);

                    spawnPoints[spawnPointIndex] = spawnPointElement.transform.position;

                    spawnPointIndex++;

                }
                else if (pixelColor.Equals(Color.red))
                {
                    SpawnElement(levelMapElement.elementPrefab, new Vector2(x, y), Quaternion.identity, parent);
                }
                else
                {
                    SpawnElement(levelMapElement.elementPrefab, new Vector2(x, y), Quaternion.identity, parent);
                }
            }
        }
    }

    public GameObject SpawnElement(GameObject prefab, Vector2 position, Quaternion rotation, Transform parent)
    {
        var newElement = Instantiate(prefab, position, rotation, parent);
        newElement.name = prefab.name;
        return newElement;
    }
}
