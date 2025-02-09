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
            float3 targetPosition = MouseWorldPosition.Instance.GetPosition();
            float3 moveDirecttion = targetPosition - localTrasform.ValueRO.Position;
            moveDirecttion = math.normalize(moveDirecttion);

            float rotateSpeed = 5f;

            localTrasform.ValueRW.Rotation =
                math.slerp(localTrasform.ValueRW.Rotation,
                quaternion.LookRotation(moveDirecttion, math.up()), rotateSpeed * SystemAPI.Time.DeltaTime);

            velocity.ValueRW.Linear = moveDirecttion * moveSpeed.ValueRO.value;

            // can automaticly rotate
            velocity.ValueRW.Angular = float3.zero;
            //localTrasform.ValueRW.Position += moveDirecttion * SystemAPI.Time.DeltaTime;
        }


    }
}
