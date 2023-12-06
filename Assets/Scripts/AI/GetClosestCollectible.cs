using MBT;
using UnityEngine;

namespace MBT
{
    [AddComponentMenu("")]
    [MBTNode("Engin2/Get closest collectible")]
    public class GetClosestCollectible : Leaf
    {
        [Space]
        [SerializeField]
        private Vector2Reference m_closestCollectiblePos = new Vector2Reference();
        [SerializeField]
        private TransformReference m_workerTransform = new TransformReference();

        public override NodeResult Execute()
        {
            Debug.Log(TeamOrchestrator._Instance.KnownCollectibles.Count - TeamOrchestrator._Instance.OverUsedCollectibles.Count);
            
            if (TeamOrchestrator._Instance.KnownCollectibles.Count == 0)
            {
                //On n'a pas trouvé de collectible. On retourne sans avoir updaté
                return NodeResult.failure;
            }

            Collectible nearestCollectible = null;
            foreach (var collectible in TeamOrchestrator._Instance.KnownCollectibles)
            {
                bool notAvailable = false;
                for (int i = 0; i < TeamOrchestrator._Instance.OverUsedCollectibles.Count; i++)
                {
                    if (collectible.GetPosition() == TeamOrchestrator._Instance.OverUsedCollectibles[i].GetPosition())
                    {
                        notAvailable = true;
                        Debug.Log("not available");
                        break;
                    }
                }
                if (notAvailable == true)
                {
                    continue;
                }
                nearestCollectible = collectible;

                if (Vector3.Distance(nearestCollectible.transform.position, m_workerTransform.Value.position)
                    > Vector3.Distance(collectible.transform.position, m_workerTransform.Value.position))
                {
                    nearestCollectible = collectible;
                }
            }

            //Ceci est le camp le plus près. On update sa valeur dans le blackboard et retourne true
            if (nearestCollectible != null)
            {
                TeamOrchestrator._Instance.OverUsedCollectibles.Add(nearestCollectible);
                m_closestCollectiblePos.Value =
                    new Vector2(nearestCollectible.transform.position.x, nearestCollectible.transform.position.y);
                Debug.Log(m_closestCollectiblePos.Value);
                return NodeResult.success;
            }
            else
            {
                Debug.Log("Empty collectible");
                return NodeResult.failure;
            }

        }
    }
}

[AddComponentMenu("")]
[MBTNode("Example/Set Random Position", 500)]
public class SetRandomPosition : Leaf
{
    public Bounds bounds;
    public Vector3Reference blackboardVariable = new Vector3Reference(VarRefMode.DisableConstant);

    public override NodeResult Execute()
    {
        // Random values per component inside bounds
        blackboardVariable.Value = new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            Random.Range(bounds.min.z, bounds.max.z)
        );
        return NodeResult.success;
    }
}