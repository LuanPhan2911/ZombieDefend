using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
[UpdateBefore(typeof(ResetEventSystem))]
partial struct UnitSelectedSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {

        foreach (var selected in SystemAPI.Query<RefRO<Selected>>().WithPresent<Selected>())
        {
            if (selected.ValueRO.onDeselected)
            {

                RefRW<LocalTransform> selectedVisualLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualEntity);
                selectedVisualLocalTransform.ValueRW.Scale = 0f;
            }
            if (selected.ValueRO.onSelected)
            {

                RefRW<LocalTransform> selectedVisualLocalTransform = SystemAPI.GetComponentRW<LocalTransform>(selected.ValueRO.visualEntity);
                selectedVisualLocalTransform.ValueRW.Scale = selected.ValueRO.showScale;
            }

        }


    }


}
