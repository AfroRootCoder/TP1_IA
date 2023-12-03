using UnityEngine;
using MBT;
using System.Collections.Generic;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Find Close By Unassigned Positions And Choose Randomly")]
public class FindCloseByUnassignedPositionsAndChooseRandomly : Leaf
{
    // Pas utilisé/terminé

    private int m_numberOfElementsToFind = 3;
    private Vector2 m_currentPosition = new Vector2();
    private TeamOrchestrator m_teamOrchestrator = null;
    private Dictionary<Vector2Int, SearchGridCell> m_searchGridCellDictionary = null;
    private List<Vector2Int> m_nearestSearchGridCellsPositions = null;    
    public TransformReference m_agentTransform = new TransformReference();
    public Vector2Reference m_targetPosition2D = new Vector2Reference(VarRefMode.DisableConstant);


    public override void OnEnter()
    {
        m_currentPosition = new Vector2(m_agentTransform.Value.position.x, m_agentTransform.Value.position.y);
        m_teamOrchestrator = TeamOrchestrator._Instance;
        m_searchGridCellDictionary = m_teamOrchestrator.SearchGridCellsDictionary;        
    }


    public override NodeResult Execute()
    {
        m_nearestSearchGridCellsPositions = FindNearestUnassignedSearchGridCellPositions(m_numberOfElementsToFind);

        int randomListIndex = Random.Range(0, m_numberOfElementsToFind);

        m_targetPosition2D.Value = m_nearestSearchGridCellsPositions[randomListIndex];

        return NodeResult.success;        
    }

    private List<Vector2Int> FindNearestUnassignedSearchGridCellPositions(int numberOfElementsToFind)
    {
        float minDistance = float.MaxValue;
        List<Vector2Int> nearestPositions = new List<Vector2Int>();

        foreach (var EntryInDictionary in m_searchGridCellDictionary)
        {
            Vector2Int gridPosition = EntryInDictionary.Key;
            SearchGridCell gridCell = EntryInDictionary.Value;

            if (gridCell.GridCellAssignedForSearch || gridCell.PositionSearched)
            {
                continue;
            }

            float distance = Vector2.Distance(m_currentPosition, -gridPosition);

            if (nearestPositions.Count == 0 || distance < minDistance)
            {
                nearestPositions.Add(gridPosition);

                if (nearestPositions.Count > numberOfElementsToFind)
                {
                    nearestPositions.RemoveAt(0);
                }

                minDistance = Vector2.Distance(m_currentPosition, nearestPositions[nearestPositions.Count - 1]);
            }
        }

        return nearestPositions;
    }

}






//private List<Vector2Int> FindNearestUnassignedSearchGridCellPositions(int numberOfElementsToFind)
//{
//    float minDistance = float.MaxValue;
//    List<Vector2Int> nearestPositions = new List<Vector2Int>();
//
//    foreach (var EntryInDictionary in m_searchGridCellDictionary)
//    {
//        Vector2Int gridPosition = EntryInDictionary.Key;
//        SearchGridCell gridCell = EntryInDictionary.Value;
//
//        if (gridCell.GridCellAssignedForSearch
//            || gridCell.PositionSearched)
//        {
//            continue;
//        }
//
//        float distance = Vector2.Distance(m_currentPosition, gridPosition);
//
//        if (nearestPositions.Count == 0 || distance < minDistance)
//        {
//            nearestPositions.Add(gridPosition);
//
//            if (nearestPositions.Count > numberOfElementsToFind)
//            {
//                nearestPositions.RemoveAt(0);
//            }
//
//            //minDistance = distance;
//
//            // Mettez à jour la distance minimale avec la nouvelle plus petite distance
//            minDistance = Vector2.Distance(m_currentPosition, nearestPositions[nearestPositions.Count - 1]);
//        }
//    }
//
//    return nearestPositions;
//}
//
//





