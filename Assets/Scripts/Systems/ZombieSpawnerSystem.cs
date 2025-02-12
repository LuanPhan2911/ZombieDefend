using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct ZombieSpawnerSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer commandBuffer = SystemAPI
            .GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged);
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();
        foreach (var (localTransform, zombieSpawner) in
            SystemAPI.Query<RefRO<LocalTransform>, RefRW<ZombieSpawner>>())
        {
            if (zombieSpawner.ValueRO.timer > 0f)
            {
                zombieSpawner.ValueRW.timer -= SystemAPI.Time.DeltaTime;
                continue;
            }
            zombieSpawner.ValueRW.timer = zombieSpawner.ValueRO.timerMax;

            Entity zombieEntity = state.EntityManager.Instantiate(entitiesReferences.zombieEntityPrefab);
            SystemAPI.SetComponent(zombieEntity, LocalTransform.FromPosition(localTransform.ValueRO.Position));

            commandBuffer.AddComponent(zombieEntity,
                new RandomWalking
                {
                    originPosition = localTransform.ValueRO.Position,
                    targetPosition = localTransform.ValueRO.Position,
                    minDistance = zombieSpawner.ValueRO.randomWalkingMinDistance,
                    maxDistance = zombieSpawner.ValueRO.randomWalkingMaxDistance,
                    random = new Unity.Mathematics.Random((uint)zombieEntity.Index)
                });


        }
    }



}
