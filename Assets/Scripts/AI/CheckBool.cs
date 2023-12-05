using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/CheckBool")]
public class CheckBool : Condition
{
    [SerializeField]
    private BoolReference m_bool;


    public override bool Check()
    {
        return m_bool.Value;
    }
}
