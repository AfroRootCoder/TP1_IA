using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class TeamOrchestrator : MonoBehaviour
{
    private const int BOUNDING_BOX_INCREMENTATION = 50;
    private const float MIN_OBJECTS_DISTANCE = 2.0f;
    private const int SPECIAL_COLLECTIBLE_VALUE = 10;
    public const int MAX_WORKERS = 40;
    public const int WORKERS_STARTING_AMOUNT = 5;

    public List<Collectible> KnownCollectibles { get; private set; } = new List<Collectible>();
    public List<Collectible> OverUsedCollectibles { get; private set; } = new List<Collectible>();
    public List<Camp> Camps { get; private set; } = new List<Camp>();
    public List<Worker> WorkersList { get; private set; } = new List<Worker>();

    [SerializeField]
    private TextMeshProUGUI m_scoreText;
    [SerializeField]
    private TextMeshProUGUI m_remainingTimeText;
    [SerializeField]
    private float m_timeScale = 1.0f;
    [SerializeField]
    private GameObject m_workerPrefab;

    private float m_remainingTime;
    private int m_score = 0;
    private int m_boundingBoxMultiplier = 1;

    [field: Header("SEARCH GRID")]
    [SerializeField]
    private GameObject m_gridMarker = null;
    private int m_distanceBetweenPoints = 6;                                    
    public Dictionary<Vector2Int, SearchGridCell> SearchGridCellsDictionary { get; private set; } = new Dictionary<Vector2Int, SearchGridCell>();
    public SearchGridBoundingBox FullGridBoundingBox { get; set; } = new SearchGridBoundingBox();    
    public SearchGridBoundingBox SearchGridAreaBoundingBox { get; set; } = new SearchGridBoundingBox();
    [field: SerializeField] public Vector2 BoundingBoxMin { get; private set; }
    [field:SerializeField] public Vector2 BoundingBoxMax { get; private set; }


    public static TeamOrchestrator _Instance
    {
        get;
        private set;
    }

    private void Awake()
    {
        if (_Instance == null || _Instance == this)
        {
            _Instance = this;            
            return;
        }

        Destroy(this);        
    }

    private void Start()
    {
        m_remainingTime = MapGenerator.SimulationDuration.Value;

        Time.timeScale = m_timeScale;
    }

    private void Update()
    {
        m_remainingTime -= Time.deltaTime;
        m_remainingTimeText.text = "Remaining time: " + m_remainingTime.ToString("#.00");
    }

    public void TryAddCollectible(Collectible collectible)
    {
        if (KnownCollectibles.Contains(collectible))
        {
            return;
        }

        KnownCollectibles.Add(collectible);        
    }

    public float GetRemainingTime()
    {
        return m_remainingTime;
    }

    public void GainResource(ECollectibleType collectibleType)
    {
        if (collectibleType == ECollectibleType.Regular)
        {
            m_score++;
        }
        if (collectibleType == ECollectibleType.Special)
        {
            m_score += SPECIAL_COLLECTIBLE_VALUE;
        }
                
        m_scoreText.text = "Score: " + m_score.ToString();
    }

    public void OnGameEnded()
    {
        PrintTextFile();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }

    private void PrintTextFile()
    {
        string path = Application.persistentDataPath + "/Results.txt";
        File.AppendAllText(path, "Score of simulation with seed: " + MapGenerator.Seed +  ": " + m_score.ToString() + "\n");

#if UNITY_EDITOR
        UnityEditor.EditorUtility.RevealInFinder(path);
        UnityEditor.EditorUtility.OpenWithDefaultApp(path);
#endif
    }

    public bool CanPlaceObject(Vector2 coordinates)
    {
        foreach (var collectible in KnownCollectibles)
        {
            var collectibleLocation = new Vector2(collectible.transform.position.x, collectible.transform.position.y);
            if (Vector2.Distance(coordinates, collectibleLocation) < MIN_OBJECTS_DISTANCE)
            {
                return false;
            }
        }

        return true;
    }

    public void OnCampPlaced()
    {
        m_score -= MapGenerator.CampCost.Value;
    }

    public void SpawnWorker()
    {
        var newWorker = Instantiate(m_workerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        WorkersList.Add(newWorker.GetComponent<Worker>());
        m_score -= MapGenerator.WORKER_COST;
    }

    public void SpawnStartingWorkers()
    {
        for (int i = 0; i < WORKERS_STARTING_AMOUNT; i++)
        {
            var newWorker = Instantiate(m_workerPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            WorkersList.Add(newWorker.GetComponent<Worker>());
        }
    }

    public void GenerateSearchGrid(int mapDimensionValue)
    {
        Debug.Log("Map dimension value in Search grid : " + mapDimensionValue);

        int numberOfPointsOnRowOrColumn = (mapDimensionValue / m_distanceBetweenPoints) + 1;
        int gridCenterOffset = (mapDimensionValue / m_distanceBetweenPoints) * m_distanceBetweenPoints / 2;

        for (int i = 0; i < numberOfPointsOnRowOrColumn; i++)
        {
            for (int j = 0; j < numberOfPointsOnRowOrColumn; j++)
            {
                int xPosition = i * m_distanceBetweenPoints - gridCenterOffset;
                int yPosition = j * m_distanceBetweenPoints - gridCenterOffset;

                Vector2Int gridPosition = new Vector2Int(xPosition, yPosition);
                SearchGridCell searchGridCell = new SearchGridCell(gridPosition.x, gridPosition.y);

                SearchGridCellsDictionary.TryAdd(gridPosition, searchGridCell);
            }
        }
    }

    public void GenerateFullSearchGridAreaBoundingBox()
    {        
        if (SearchGridCellsDictionary.Count == 0)
        {            
            return;
        }
        
        Vector2Int firstDictionaryEntryKey = SearchGridCellsDictionary.Keys.First();
        FullGridBoundingBox.Min = firstDictionaryEntryKey;
        FullGridBoundingBox.Max = firstDictionaryEntryKey;
                
        foreach (Vector2Int key in SearchGridCellsDictionary.Keys)
        {            
            FullGridBoundingBox.Min = new Vector2Int(Mathf.Min(FullGridBoundingBox.Min.x, key.x), Mathf.Min(FullGridBoundingBox.Min.y, key.y));                        
            FullGridBoundingBox.Max = new Vector2Int(Mathf.Max(FullGridBoundingBox.Max.x, key.x), Mathf.Max(FullGridBoundingBox.Max.y, key.y));
        }
    }

    public void GenerateSearchGridAreaBoundingBox(Vector2 min, Vector2 max)
    {
        SearchGridAreaBoundingBox.Min = FindClosestCellToApproximatePosition(min);
        SearchGridAreaBoundingBox.Max = FindClosestCellToApproximatePosition(max);
    }

    public void IncrementBoundingBox()
    {
        float incrementation = BOUNDING_BOX_INCREMENTATION * m_boundingBoxMultiplier;
        Vector2 min = new Vector2(BoundingBoxMin.x - incrementation, BoundingBoxMin.y - incrementation);
        Vector2 max = new Vector2(BoundingBoxMax.x + incrementation, BoundingBoxMax.y + incrementation);

        GenerateSearchGridAreaBoundingBox(min, max);

        m_boundingBoxMultiplier++;
    }

    private Vector2Int FindClosestCellToApproximatePosition(Vector2 approximatePosition)
    {
        float minDistance = float.MaxValue;
        Vector2Int closestCell = Vector2Int.zero;

        foreach (Vector2Int key in SearchGridCellsDictionary.Keys)
        {            
            float distance = Vector2.Distance(key, approximatePosition);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestCell = key;
            }
        }

        return closestCell;
    }

    public void InstantiateMarkersForSearchGridVisualRepresentation()
    {
        foreach (var key in SearchGridCellsDictionary)
        {
            Vector2 gridCellPosition = key.Key;
            Instantiate(m_gridMarker, new Vector3(gridCellPosition.x, gridCellPosition.y, 0.0f), Quaternion.identity);
        }
    }
}


public class SearchGridCell
{
    public int X { get; private set; }
    public int Y { get; private set; }
    public bool GridCellAssignedForSearch { get; set; } = false;
    
    public SearchGridCell(int x, int y)
    {
        X = x;
        Y = y;
    }
}

public class SearchGridBoundingBox
{
    public Vector2Int Min { get; set; }
    public Vector2Int Max { get; set; }
}


