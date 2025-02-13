using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct UnitMoverOverideSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (localTransform, unitMover, unitMoverOveride, unitMoverOverideEnable) in
            SystemAPI.Query<RefRO<LocalTransform>,
            RefRW<UnitMover>, RefRO<UnitMoverOveride>, EnabledRefRW<UnitMoverOveride>>())
        {
            if (math.distancesq(localTransform.ValueRO.Position, unitMoverOveride.ValueRO.targetPosition) > UnitMoverSystem.READ_TARGET_DISTANCE_SQ)
            {
                // move closer
                unitMover.ValueRW.targetPosition = unitMoverOveride.ValueRO.targetPosition;
            }
            else
            {
                //reached target
                unitMoverOverideEnable.ValueRW = false;
            }
        }
    }


}
