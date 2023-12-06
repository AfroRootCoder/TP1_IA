using UnityEngine;
using MBT;

[AddComponentMenu("")]
[MBTNode("Engin2/Follower Move To Leader")]

public class FollowerMoveToLeader : Leaf
{
    public Vector2Reference m_targetPosition2D = new Vector2Reference(VarRefMode.DisableConstant);
    public override NodeResult Execute()
    {
        /*if (worker.GetLeadershipStatus())
        {
            m_targetPosition2D.Value = worker.transform.position;
            m_targetPosition2D.Value = worker.GetPosition();
            worker.GetFollower().
        }*/

        return NodeResult.success;
    }
}
