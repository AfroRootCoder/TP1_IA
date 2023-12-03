using UnityEngine;
using MBT;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Has nearest camp condition")]

public class HasCampNearCondition : Condition
{
    [SerializeField]
    private Vector2Reference m_nearestCampVec2 = new Vector2Reference();
    [SerializeField]
    private TransformReference m_workerTransform = new TransformReference();
    [SerializeField]
    private FloatReference m_maximumDistance = new FloatReference();




    public override bool Check()
    {
        float distance = Vector2.Distance(m_workerTransform.Value.position, m_nearestCampVec2.Value);

        if (distance < m_maximumDistance.Value)
        {
            return true;
        }

        return false;
    }

}
