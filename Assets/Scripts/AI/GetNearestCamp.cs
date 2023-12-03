using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Get nearest camp")]
public class GetNearestCamp : Leaf
{
    [SerializeField]
    private TransformReference m_workerTransform = new TransformReference();
    [SerializeField]
    private Vector2Reference m_nearestCampVec2 = new Vector2Reference();

    public override NodeResult Execute()
    {
        Debug.Log(this.name);
        
        if (TeamOrchestrator._Instance.Camps.Count == 0)
        {
            //On n'a pas trouvé de camp. On retourne faux
            m_nearestCampVec2.Value = new Vector2(1, 1);
            return NodeResult.success;
        }

        Camp nearestCamp = TeamOrchestrator._Instance.Camps[0];

        foreach (var camp in TeamOrchestrator._Instance.Camps)
        {
            if (Vector3.Distance(nearestCamp.transform.position, m_workerTransform.Value.position)
                > Vector3.Distance(camp.transform.position, m_workerTransform.Value.position))
            {
                nearestCamp = camp;
            }
        }

        //Ceci est le camp le plus près. On update sa valeur dans le blackboard et retourne true
        m_nearestCampVec2.Value = new Vector2(nearestCamp.transform.position.x, nearestCamp.transform.position.y);
        return NodeResult.success;

    }

    //public override bool Check()
    //{
    //    if (TeamOrchestrator._Instance.Camps.Count == 0)
    //    {
    //        //On n'a pas trouvé de camp. On retourne faux
    //        return false;
    //    }
    //
    //    Camp nearestCamp = TeamOrchestrator._Instance.Camps[0];
    //
    //    foreach (var camp in TeamOrchestrator._Instance.Camps)
    //    {
    //        if (Vector3.Distance(nearestCamp.transform.position, m_workerTransform.Value.position) 
    //            > Vector3.Distance(camp.transform.position, m_workerTransform.Value.position))
    //        {
    //            nearestCamp = camp;
    //        }
    //    }
    //
    //    //Ceci est le camp le plus près. On update sa valeur dans le blackboard et retourne true
    //    m_nearestCampVec2.Value = new Vector2(nearestCamp.transform.position.x, nearestCamp.transform.position.y);
    //
    //    return true;
    //}
}
