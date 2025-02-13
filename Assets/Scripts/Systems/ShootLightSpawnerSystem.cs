using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct ShootLightSpawnerSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();
        foreach (var shootAttrack in SystemAPI.Query<RefRO<ShootAttack>>())
        {
            if (shootAttrack.ValueRO.onShoot.isTriggered)
            {
                Entity shootLightEntity = state.EntityManager.Instantiate(entitiesReferences.shootLightEntityPrefab);
                state.EntityManager.SetComponentData(shootLightEntity,
                    LocalTransform.FromPosition(shootAttrack.ValueRO.onShoot.spawnerPosition));
            }
        }
    }


}
