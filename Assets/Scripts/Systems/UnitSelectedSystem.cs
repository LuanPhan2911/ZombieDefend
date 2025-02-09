using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct UnitSelectedSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var selected in SystemAPI.Query<RefRO<Selected>>().WithDisabled<Selected>())
        {
            RefRW<LocalTransform> selectedVisualLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualEntity);
            selectedVisualLocalTransform.ValueRW.Scale = 0f;
        }

        foreach (var selected in SystemAPI.Query<RefRO<Selected>>())
        {
            RefRW<LocalTransform> selectedVisualLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualEntity);
            selectedVisualLocalTransform.ValueRW.Scale = selected.ValueRO.showScale;
        }

    }


}
