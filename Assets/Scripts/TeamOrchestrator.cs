using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;

public class TeamOrchestrator : MonoBehaviour
{

    private const float MIN_OBJECTS_DISTANCE = 0.1f; // remettre à 2.0f!!!!
    public List<Collectible> KnownCollectibles { get; private set; } = new List<Collectible>();
    public List<Camp> Camps { get; private set; } = new List<Camp>();
    public List<Worker> WorkersList { get; private set; } = new List<Worker>();

    [SerializeField]
    private TextMeshProUGUI m_scoreText;
    [SerializeField]
    private TextMeshProUGUI m_remainingTimeText;
    [SerializeField]
    private float m_timeScale = 1.0f;

    private float m_remainingTime;
    private int m_score = 0;

    [field: Header("SEARCH GRID")]
    [SerializeField]
    private GameObject m_gridMarker = null;
    private int m_distanceBetweenPoints = 6; // Calcul à l'oeil, c'est ce qui permet une couverture complète de la carte en suivant la taille des champs de vision. Needs ref to vision range or something                                             
    public Dictionary<Vector2Int, SearchGridCell> SearchGridCellsDictionary { get; private set; } = new Dictionary<Vector2Int, SearchGridCell>();
    public SearchGridBoundingBox FullGridBoundingBox { get; set; } = new SearchGridBoundingBox();
    public SearchGridBoundingBox InitialSearchGridAreaBoundingBox { get; set; } = new SearchGridBoundingBox();
    public SearchGridBoundingBox SecondarySearchGridAreaBoundingBox { get; set; } = new SearchGridBoundingBox();

    public bool FirstAssigned { get; set; } = false;
    public bool MovingFirst { get; set; } = false;
    public uint NumberOfWorkers { get; set; } = 0;



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

    private void FixedUpdate()
    {
        
        
        //foreach (var collectible in KnownCollectibles)
        //{
        //    float smallestDistance = 0;
        //    Worker idealWorker;
        //    
        //    foreach (var worker in WorkersList)
        //    {
        //        float distance = Vector2.Distance(collectible.transform.position, worker.transform.position);
        //
        //        if (distance < smallestDistance)
        //        {
        //            smallestDistance = distance;
        //            idealWorker = worker;
        //        }
        //    }
        //
        //    //Communicate to worker 
        //}



    }

    public void TryAddCollectible(Collectible collectible)
    {
        if (KnownCollectibles.Contains(collectible))
        {
            return;
        }

        KnownCollectibles.Add(collectible);
        Debug.Log("Collectible added");

        FindClosestWorker(collectible);
    }

    private void FindClosestWorker(Collectible collectible)
    {
        Worker idealWorker = WorkersList[0];
        float smallestDistance = Vector2.Distance(collectible.transform.position, idealWorker.transform.position);

        foreach (var worker in WorkersList)
        {
            float distance = Vector2.Distance(collectible.transform.position, worker.transform.position);

            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                idealWorker = worker;
            }
        }

        SetWorkerToThisCollectible(idealWorker); //check there is at least one worker
    }

    private void SetWorkerToThisCollectible(Worker worker)
    {
        worker.SetHasBeenAssignedToThisCollectibleBool(true);
    }

    public void GainResource(ECollectibleType collectibleType)
    {
        if (collectibleType == ECollectibleType.Regular)
        {
            m_score++;
        }
        if (collectibleType == ECollectibleType.Special)
        {
            m_score += 10;//TODO: Turn to const
        }

        Debug.Log("New score = " + m_score);
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

    public void OnWorkerCreated()
    {
        //TODO élèves. À vous de trouver quand utiliser cette méthode et l'utiliser.
        m_score -= MapGenerator.WORKER_COST;
    }

    public void GenerateSearchGrid(int mapDimensionValue)
    {
        Debug.Log("Map dimension value in Search grid : " + mapDimensionValue);

        //Pour fin de test et réduire la taille du grid
        //mapDimensionValue = 20;

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

    public void GenerateInitialSearchGridAreaBoundingBox(uint percentageOfOriginalBoundingBox)
    {
        Debug.Log("test");        

        if (percentageOfOriginalBoundingBox > 100)
        {
            Debug.LogError("Percentage number is too large, cannot generate initial search area bounding box.");
            return;
        }

        float percentage = percentageOfOriginalBoundingBox / 100.0f;

        Vector2 temporaryMin = new Vector2(FullGridBoundingBox.Min.x * percentage, FullGridBoundingBox.Min.y * percentage);
        Vector2 temporaryMax = new Vector2(FullGridBoundingBox.Max.x * percentage, FullGridBoundingBox.Max.y * percentage);

        InitialSearchGridAreaBoundingBox.Min = FindClosestCellToApproximatePosition(temporaryMin);
        InitialSearchGridAreaBoundingBox.Max = FindClosestCellToApproximatePosition(temporaryMax);

        Debug.Log("test");
    }

    public SearchGridBoundingBox GenerateSearchGridAreaBoundingBox(Vector2 min, Vector2 max)
    {
        // Mettre un check 

        SearchGridBoundingBox searchGridBoundingBox = new SearchGridBoundingBox();

        searchGridBoundingBox.Min = FindClosestCellToApproximatePosition(min);
        searchGridBoundingBox.Max = FindClosestCellToApproximatePosition(max);

        return searchGridBoundingBox;
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
    public bool PositionSearched { get; set; } = false;

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


//public struct SearchGridCell
//{
//    public Vector2 GridPosition { get; set; }
//    public bool GridCellAssignedForSearch { get; set; }
//    public bool PositionSearched { get; set; }    
//
//    public SearchGridCell(Vector2 position)
//    {
//        GridPosition = position;
//        GridCellAssignedForSearch = false;
//        PositionSearched = false;
//    }
//}



