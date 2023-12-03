using MBT;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Has Found Collectible")]
public class HasFoundCollectibleCondition : Condition
{
    [SerializeField]
    private BoolReference m_hasBeenAssigned;


    public override bool Check()
    {
        //return TeamOrchestrator._Instance.KnownCollectibles.Count > 0;

        if (TeamOrchestrator._Instance.KnownCollectibles.Count == 0 ||
            m_hasBeenAssigned.Value == false)
        {
            return false;
        }

        return true;



    }
}
