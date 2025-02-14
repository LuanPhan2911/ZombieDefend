using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct FindTargetSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        PhysicsWorldSingleton physicsWorld = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorld.CollisionWorld;

        NativeList<DistanceHit> distanceHitList = new NativeList<DistanceHit>(Allocator.Temp);
        foreach (var (localTransform, findTarget, target) in SystemAPI.Query<RefRO<LocalTransform>,
            RefRW<FindTarget>, RefRW<Target>>())
        {
            if (findTarget.ValueRO.timer > 0)
            {
                findTarget.ValueRW.timer -= SystemAPI.Time.DeltaTime;
                continue;
            }
            findTarget.ValueRW.timer = findTarget.ValueRO.timerMax;


            distanceHitList.Clear();
            CollisionFilter collisionFilter = new CollisionFilter
            {
                BelongsTo = ~0u,
                CollidesWith = 1u << GameAssets.unitLayer,
                GroupIndex = 0
            };

            if (collisionWorld.OverlapSphere(localTransform.ValueRO.Position, findTarget.ValueRO.range,
                ref distanceHitList, collisionFilter))
            {
                Entity closetTargetEntity = Entity.Null;
                float closetDistanceSq = float.MaxValue;

                foreach (var distanceHit in distanceHitList)
                {
                    if (!SystemAPI.Exists(distanceHit.Entity) || !SystemAPI.HasComponent<Unit>(distanceHit.Entity))
                    {
                        continue;
                    };
                    Unit targetUnit = SystemAPI.GetComponent<Unit>(distanceHit.Entity);
                    if (targetUnit.faction == findTarget.ValueRO.targetFaction)
                    {
                        // target found
                        float distanceSq = math.distancesq(localTransform.ValueRO.Position, distanceHit.Position);

                        if (distanceSq < closetDistanceSq)
                        {
                            closetDistanceSq = distanceSq;
                            closetTargetEntity = distanceHit.Entity;
                        }
                    }

                }

                target.ValueRW.targetEntity = closetTargetEntity;
            }
        }
    }


}
