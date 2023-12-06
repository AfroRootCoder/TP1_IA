using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Check For Collectibles")]
public class CheckForCollectibles : Condition
{

    public override bool Check()
    {
        return TeamOrchestrator._Instance.KnownCollectibles.Count - TeamOrchestrator._Instance.OverUsedCollectibles.Count > 0;
    }
}
