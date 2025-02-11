using Unity.Entities;
using UnityEngine;

public class ShootAttackAuthoring : MonoBehaviour
{

    public float timerMax;
    public class ShootAttackAuthoringBaker : Baker<ShootAttackAuthoring>
    {
        public override void Bake(ShootAttackAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ShootAttack
            {
                timer = authoring.timerMax,
                timerMax = authoring.timerMax
            });
        }
    }
}

public struct ShootAttack : IComponentData
{
    public float timer;
    public float timerMax;
}


