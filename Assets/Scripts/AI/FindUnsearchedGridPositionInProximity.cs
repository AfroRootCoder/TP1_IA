using UnityEngine;
using MBT;
using System.Collections.Generic;



[AddComponentMenu("")]
[MBTNode(name = "Engin2/Find Unsearched Grid Position In Proximity")]
public class FindUnsearchedGridPositionInProximity : Leaf
{
    private Vector2 m_currentPosition = new Vector2();
    private TeamOrchestrator m_teamOrchestrator = null;
    private Dictionary<Vector2Int, SearchGridCell> m_searchGridCellDictionary = null;
    public TransformReference m_agentTransform = new TransformReference();
    public Vector2Reference m_targetPosition2D = new Vector2Reference(VarRefMode.DisableConstant);


    public override void OnEnter()
    {
        //Debug.Log("Entered full search");

        m_currentPosition = new Vector2(m_agentTransform.Value.position.x, m_agentTransform.Value.position.y);
        m_teamOrchestrator = TeamOrchestrator._Instance;
        m_searchGridCellDictionary = m_teamOrchestrator.SearchGridCellsDictionary;
    }

    public override NodeResult Execute()
    {
        Vector2Int dictionaryKey = Vector2Int.zero;

        dictionaryKey = FindNearestUnassignedSearchGridCell();

        if (m_searchGridCellDictionary.ContainsKey(dictionaryKey))
        {
            m_searchGridCellDictionary[dictionaryKey].GridCellAssignedForSearch = true;
        }        

        m_targetPosition2D.Value = dictionaryKey;
        
        return NodeResult.success;
    }

    private Vector2Int FindNearestUnassignedSearchGridCell()
    {        
        float minDistance = float.MaxValue;
        Vector2Int nearestPosition = Vector2Int.zero;        

        foreach (var EntryInDictionary in m_searchGridCellDictionary) 
        {
            Vector2Int gridPosition = EntryInDictionary.Key;
            SearchGridCell gridCell = EntryInDictionary.Value;
            
            if(gridCell.GridCellAssignedForSearch
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

        return nearestPosition;
    }
}