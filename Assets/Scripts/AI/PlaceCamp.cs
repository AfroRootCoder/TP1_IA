using MBT;
using UnityEngine;

[MBTNode("Engin2/Place Camp")]
[AddComponentMenu("")]
public class PlaceCamp : Leaf
{
    [SerializeField]
    private GameObject m_campPrefab;

    public BoolReference m_assignedToBuildCamp;

    public override NodeResult Execute()
    {
        Instantiate(m_campPrefab, transform.position, Quaternion.identity);
        TeamOrchestrator._Instance.OnCampPlaced();
        m_assignedToBuildCamp.Value = false;
        return NodeResult.success;
    }
}
