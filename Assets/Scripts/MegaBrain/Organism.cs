using System.Collections.Generic;
using Vector2 = UnityEngine.Vector2;

public class Organisms
{
    private const int WORKER_DISTANCE_PER_SECOND = 5;
    private const int COOLDOWN_TIME = 5;
    private int m_collectiblesAmount;
    private List<Vector2> m_assignedCollectibles;
    private Vector2 m_campPlacement;
    private List<Worker> m_assignedWorkers;

    public Organisms(int amount, List<Vector2> collectibles, List<Worker> workers)
    {
        m_collectiblesAmount = amount;
        m_assignedCollectibles = collectibles;
        m_assignedWorkers = workers;
        m_campPlacement = EvaluateCampPlacement();

        AssignWorkers();
    }

    public Organisms(Worker worker, Collectible collectible)
    {
        m_collectiblesAmount = 1;

        worker.SetIsAssignedBool(true);
        worker.SetIsAssignedToBuildCampBool(true);
        worker.SetAssignedCampPosition(collectible.GetPosition());
        worker.SetAssignedCollectiblePosition(collectible.GetPosition());
    }

    private Vector2 EvaluateCampPlacement()
    {
        Vector2 idealPosition = new Vector2();

        for (int i = 0; i < m_collectiblesAmount; i++)
        {
            idealPosition += m_assignedCollectibles[i];
        }
        idealPosition /= m_collectiblesAmount;

        return idealPosition;
    }

    private void AssignWorkers()
    {
        for (int i = 0; i < m_collectiblesAmount; i++)
        {
            m_assignedWorkers[i].SetIsAssignedBool(true);

            if (i == 0)
            {
                m_assignedWorkers[0].SetIsAssignedToBuildCampBool(true);
            }

            m_assignedWorkers[i].SetAssignedCollectiblePosition(m_assignedCollectibles[i]);
            m_assignedWorkers[i].SetAssignedCampPosition(m_campPlacement);

            float waitingTime = CalculateWaitingTime(m_assignedCollectibles[i]);
            m_assignedWorkers[i].SetWaitingTime(waitingTime);
        }
    }

    private float CalculateWaitingTime(Vector2 collectiblePos)
    {
        float distance = Vector2.Distance(collectiblePos, m_campPlacement);

        return COOLDOWN_TIME - (distance / WORKER_DISTANCE_PER_SECOND);
    }
}
