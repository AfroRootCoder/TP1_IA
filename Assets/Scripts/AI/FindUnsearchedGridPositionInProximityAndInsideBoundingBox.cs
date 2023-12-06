using UnityEngine;
using MBT;
using System.Collections.Generic;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Find Unsearched Grid Position In Proximity And Inside Bounding Box")]
public class FindUnsearchedGridPositionInProximityAndInsideBoundingBox : Leaf
{
    private Vector2 m_currentPosition = new Vector2();
    private TeamOrchestrator m_teamOrchestrator = null;
    private Dictionary<Vector2Int, SearchGridCell> m_searchGridCellDictionary = null;
    private SearchGridBoundingBox m_boundingBox;
    public TransformReference m_agentTransform = new TransformReference();
    public Vector2Reference m_targetPosition2D = new Vector2Reference(VarRefMode.DisableConstant);

    public override void OnEnter()
    {
        m_currentPosition = new Vector2(m_agentTransform.Value.position.x, m_agentTransform.Value.position.y);
        m_teamOrchestrator = TeamOrchestrator._Instance;
        m_searchGridCellDictionary = m_teamOrchestrator.SearchGridCellsDictionary;
        m_boundingBox = m_teamOrchestrator.SearchGridAreaBoundingBox;
    }

    public override NodeResult Execute()
    {
        Vector2Int dictionaryKey = Vector2Int.zero;

        dictionaryKey = FindNearestUnassignedSearchGridCellInsideBoundingBox();

        if (m_searchGridCellDictionary.ContainsKey(dictionaryKey))
        {
            m_searchGridCellDictionary[dictionaryKey].GridCellAssignedForSearch = true;
        }

        m_targetPosition2D.Value = dictionaryKey;

        return NodeResult.success;        
    }

    private Vector2Int FindNearestUnassignedSearchGridCellInsideBoundingBox()
    {
        float minDistance = float.MaxValue;
        Vector2Int nearestPosition = Vector2Int.zero;

        foreach (var EntryInDictionary in m_searchGridCellDictionary)
        {
            Vector2Int gridPosition = EntryInDictionary.Key;
            SearchGridCell gridCell = EntryInDictionary.Value;

            if(gridPosition.x > m_boundingBox.Max.x
                 || gridPosition.x < m_boundingBox.Min.x
                 || gridPosition.y > m_boundingBox.Max.y
                 || gridPosition.y < m_boundingBox.Min.y)
            {
                continue;
            }

            if (gridCell.GridCellAssignedForSearch
                || gridCell.PositionSearched)
            {
                continue;
            }

            float distance = Vector2.Distance(m_currentPosition, gridPosition);

            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPosition = gridPosition;
            }
        }

        if (nearestPosition == Vector2Int.zero)
        {
            TeamOrchestrator._Instance.IncrementBoundingBox();
        }

        return nearestPosition;
    }


}
