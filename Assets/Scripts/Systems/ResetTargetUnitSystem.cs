using Unity.Burst;
using Unity.Entities;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct ResetUnitSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var target in SystemAPI.Query<RefRW<Target>>())
        {
            if (!SystemAPI.Exists(target.ValueRO.targetEntity))
            {
                target.ValueRW.targetEntity = Entity.Null;
            }
        }
    }


}
