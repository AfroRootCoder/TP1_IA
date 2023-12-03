using UnityEngine;
using MBT;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Assign First Worker Move Position")]
public class AssignFirstWorkerMovePosition : Leaf
{    
    private Vector2Int m_position = new Vector2Int(8, 10);    
    public Vector2Reference m_targetPosition2D = new Vector2Reference(VarRefMode.DisableConstant);
    
    public override NodeResult Execute()
    {
        m_targetPosition2D.Value = m_position;

        return NodeResult.success;        
    }
}
