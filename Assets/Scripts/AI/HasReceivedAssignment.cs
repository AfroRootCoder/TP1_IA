using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/Has Received Assignment")]
public class HasReceivedAssignment : Condition
{
    [SerializeField]
    private BoolReference m_hasBeenAssigned;


    public override bool Check()
    {
        return m_hasBeenAssigned.Value;
    }
}
