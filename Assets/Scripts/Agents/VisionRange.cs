using MBT;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class VisionRange : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        var collectible = other.GetComponent<Collectible>();
        if (collectible == null )
        {
            return;
        }

        Vector2 position = new Vector2(other.transform.position.x, other.transform.position.y);
        collectible.AddPosition(position);

        TeamOrchestrator._Instance.TryAddCollectible(collectible);
    }
}
