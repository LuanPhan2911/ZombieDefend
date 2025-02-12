using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct BulletMoverSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer commandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (localTransform, bullet, target, entity)
            in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Bullet>, RefRO<Target>>().WithEntityAccess())
        {

            if (target.ValueRO.targetEntity == Entity.Null)
            {
                commandBuffer.DestroyEntity(entity);
                continue;
            }

            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);

            ShootVictim shootVictim = SystemAPI.GetComponent<ShootVictim>(target.ValueRO.targetEntity);
            float3 targetPosition = targetLocalTransform.TransformPoint(shootVictim.hitLocalPosition);

            float distanceSqBefore = math.distancesq(targetPosition, localTransform.ValueRO.Position);
            float3 moveDirection = targetPosition - localTransform.ValueRO.Position;
            moveDirection = math.normalize(moveDirection);

            localTransform.ValueRW.Position += moveDirection * bullet.ValueRO.speed * SystemAPI.Time.DeltaTime;
            float distanceSqAfter = math.distancesq(targetPosition, localTransform.ValueRO.Position);
            if (distanceSqAfter > distanceSqBefore)
            {
                //overshot target, snap to target
                localTransform.ValueRW.Position = targetPosition;
            }
            float destroyDistanceSq = 0.2f;
            if (math.distancesq(targetPosition, localTransform.ValueRO.Position) < destroyDistanceSq)
            {
                //close enouh to target

                RefRW<Health> health = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                health.ValueRW.healthAmount -= bullet.ValueRO.damageAmount;
                health.ValueRW.onHealthChanged = true;
                commandBuffer.DestroyEntity(entity);
            }
        }
    }


}
