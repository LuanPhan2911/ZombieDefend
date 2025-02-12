using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct ZombieSpawnerSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
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
        }
    }



}
