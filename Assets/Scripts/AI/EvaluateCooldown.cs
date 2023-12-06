using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/EvaluateCooldown")]
public class EvaluateCooldown : Condition
{
    [SerializeField]
    private BoolReference m_bool;

    public override bool Check()
    {
        return m_bool.Value;
    }
}
