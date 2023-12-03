using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MBT;


[AddComponentMenu("")]
[MBTNode(name = "Engin2/Generate Camp Placement")]
public class GenerateCampPlacement : Leaf
{
    public Vector2Reference m_targetPosition2D = new Vector2Reference(VarRefMode.DisableConstant);


    public override NodeResult Execute()
    {
        Vector2 targetPosition = new Vector2(transform.position.x + 10, transform.position.y + 10);

        m_targetPosition2D.Value = targetPosition;

        return NodeResult.success;
    }
}
