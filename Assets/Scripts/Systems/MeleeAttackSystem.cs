using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct MeleeAttackSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        PhysicsWorldSingleton physicsWorldSingleton = SystemAPI.GetSingleton<PhysicsWorldSingleton>();
        CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;
        NativeList<RaycastHit> raycastHitList = new NativeList<RaycastHit>(Allocator.Temp);
        foreach (var (localTransform, meleeAttack, target, unitMover)
            in SystemAPI.Query<RefRO<LocalTransform>, RefRW<MeleeAttack>,
            RefRO<Target>, RefRW<UnitMover>>().WithDisabled<UnitMoverOveride>())
        {
            if (target.ValueRO.targetEntity == Entity.Null)
            {
                continue;
            }

            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);

            float meleeAttackDistanceSq = 2f;
            bool isCloseEnoughToAttack = math.distancesq(localTransform.ValueRO.Position,
                targetLocalTransform.Position) < meleeAttackDistanceSq;
            bool isTouchingTarget = false;

            if (!isCloseEnoughToAttack)
            {
                float3 directionToTarget = math.normalize(targetLocalTransform.Position - localTransform.ValueRO.Position);
                float distanceExtraToTestRayCast = 0.4f;
                RaycastInput raycastInput = new RaycastInput
                {
                    Start = localTransform.ValueRO.Position,
                    End = localTransform.ValueRO.Position +
                    directionToTarget * (meleeAttack.ValueRO.colliderSize + distanceExtraToTestRayCast),
                    Filter = CollisionFilter.Default
                };
                raycastHitList.Clear();
                if (collisionWorld.CastRay(raycastInput, ref raycastHitList))
                {
                    foreach (RaycastHit hit in raycastHitList)
                    {
                        if (hit.Entity == target.ValueRO.targetEntity)
                        {
                            isTouchingTarget = true;
                            break;
                        }
                    }
                }

            }
            if (!isCloseEnoughToAttack && !isTouchingTarget)
            {
                //target is so far
                unitMover.ValueRW.targetPosition = targetLocalTransform.Position;
            }
            else
            {
                //target is close enough to attack
                unitMover.ValueRW.targetPosition = localTransform.ValueRO.Position;
                meleeAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;
                if (meleeAttack.ValueRW.timer > 0)
                {
                    continue;
                }

                meleeAttack.ValueRW.timer = meleeAttack.ValueRO.timerMax;
                //attack
                RefRW<Health> health = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                health.ValueRW.healthAmount -= meleeAttack.ValueRO.damageAmount;
                health.ValueRW.onHealthChanged = true;

            }

        }
    }


}
