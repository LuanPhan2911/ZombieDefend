using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct BulletSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer commandBuffer = SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
            .CreateCommandBuffer(state.WorldUnmanaged);
        foreach (var (localTransform, bullet, target, entity)
            in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Bullet>, RefRO<Target>>().WithEntityAccess())
        {
            LocalTransform targetLocalTransform = SystemAPI.GetComponent<LocalTransform>(target.ValueRO.targetEntity);

            float distanceSqBefore = math.distancesq(targetLocalTransform.Position, localTransform.ValueRO.Position);
            float3 moveDirection = targetLocalTransform.Position - localTransform.ValueRO.Position;
            moveDirection = math.normalize(moveDirection);

            localTransform.ValueRW.Position += moveDirection * bullet.ValueRO.speed * SystemAPI.Time.DeltaTime;
            float distanceSqAfter = math.distancesq(targetLocalTransform.Position, localTransform.ValueRO.Position);
            if (distanceSqAfter > distanceSqBefore)
            {
                //overshot target, snap to target
                localTransform.ValueRW.Position = targetLocalTransform.Position;
            }
            float destroyDistanceSq = 0.2f;
            if (math.distancesq(targetLocalTransform.Position, localTransform.ValueRO.Position) < destroyDistanceSq)
            {
                //close enouh to target

                RefRW<Health> health = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
                health.ValueRW.healthAmount -= bullet.ValueRO.damageAmount;
                commandBuffer.DestroyEntity(entity);
            }
        }
    }


}
