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
        foreach (var (localTrasform, unitMover, velocity) in
            SystemAPI.Query<RefRW<LocalTransform>, RefRO<UnitMover>, RefRW<PhysicsVelocity>>())
        {


            float3 moveDirecttion = unitMover.ValueRO.targtePosition - localTrasform.ValueRO.Position;

            moveDirecttion = math.normalize(moveDirecttion);



            localTrasform.ValueRW.Rotation =
                math.slerp(localTrasform.ValueRW.Rotation,
                quaternion.LookRotation(moveDirecttion, math.up()),
                unitMover.ValueRO.rotateSpeed * SystemAPI.Time.DeltaTime);

            velocity.ValueRW.Linear = moveDirecttion * unitMover.ValueRO.moveSpeed;

            // can automaticly rotate
            velocity.ValueRW.Angular = float3.zero;
            //localTrasform.ValueRW.Position += moveDirecttion * SystemAPI.Time.DeltaTime;
        }


    }
}
