using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;

partial struct UnitMoverSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (localTrasform, moveSpeed, velocity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<MoveSpeed>, RefRW<PhysicsVelocity>>())
        {
            float3 targetPosition = localTrasform.ValueRO.Position + new float3(5f, 0, 0);
            float3 moveDirecttion = targetPosition - localTrasform.ValueRO.Position;
            moveDirecttion = math.normalize(moveDirecttion);


            localTrasform.ValueRW.Rotation = quaternion.LookRotation(moveDirecttion, math.up());

            velocity.ValueRW.Linear = moveDirecttion * moveSpeed.ValueRO.value;

            // can automaticly rotate
            velocity.ValueRW.Angular = float3.zero;
            //localTrasform.ValueRW.Position += moveDirecttion * SystemAPI.Time.DeltaTime;
        }


    }
}
