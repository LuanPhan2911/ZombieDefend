using Unity.Burst;
using Unity.Entities;

partial struct UnitDeathSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntityCommandBuffer entityCommandBuffer =
            SystemAPI.GetSingleton<EndSimulationEntityCommandBufferSystem.Singleton>()
                      .CreateCommandBuffer(state.WorldUnmanaged);

        foreach (var (health, entity) in SystemAPI.Query<RefRW<Health>>().WithEntityAccess())
        {
            if (health.ValueRO.healthAmount <= 0)
            {
                // unit death
                entityCommandBuffer.DestroyEntity(entity);
            }
        }

    }


}
