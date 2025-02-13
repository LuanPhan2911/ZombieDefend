using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class ShootAttackAuthoring : MonoBehaviour
{

    public float timerMax;
    public int damgeAmount;
    public float attackDistance;
    public Transform bulletSpawnTransform;
    public class ShootAttackAuthoringBaker : Baker<ShootAttackAuthoring>
    {
        public override void Bake(ShootAttackAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ShootAttack
            {
                timer = authoring.timerMax,
                timerMax = authoring.timerMax,
                damgeAmount = authoring.damgeAmount,
                attackDistance = authoring.attackDistance,
                bulletSpawnLocalPosition = authoring.bulletSpawnTransform.localPosition
            });
        }
    }
}

public struct ShootAttack : IComponentData
{
    public float timer;
    public float timerMax;
    public int damgeAmount;
    public float attackDistance;
    public float3 bulletSpawnLocalPosition;

    public OnShootEvent onShoot;
    public struct OnShootEvent
    {
        public bool isTriggered;
        public float3 spawnerPosition;
    }
}


