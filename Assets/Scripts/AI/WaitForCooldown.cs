using MBT;
using UnityEngine;

[AddComponentMenu("")]
[MBTNode(name = "Engin2/WaitForCooldown")]
public class WaitForCooldown : Leaf
{
    public FloatReference time;
    public float randomDeviation = 0f;
    public bool continueOnRestart = false;
    private float timer;

    public override void OnEnter()
    {
        if (!continueOnRestart)
        {
            timer = (randomDeviation == 0f) ? 0f : Random.Range(-randomDeviation, randomDeviation);
        }
    }

    public override NodeResult Execute()
    {
        if (timer >= time.Value)
        {
            if (continueOnRestart)
            {
                timer = (randomDeviation == 0f) ? 0f : Random.Range(-randomDeviation, randomDeviation);
            }
            return NodeResult.success;
        }
        timer += this.DeltaTime;
        return NodeResult.running;
    }

    void OnValidate()
    {
        if (time.isConstant)
        {
            randomDeviation = Mathf.Clamp(randomDeviation, 0f, time.GetConstant());
        }
        else
        {
            randomDeviation = Mathf.Clamp(randomDeviation, 0f, 600f);
        }
    }
}

