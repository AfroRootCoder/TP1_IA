using UnityEngine;
using MBT;
using System.Collections.Generic;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Explore Quadrant")]
public class ExploreQuadrant : Leaf
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









        throw new System.NotImplementedException();
    }

   
}
