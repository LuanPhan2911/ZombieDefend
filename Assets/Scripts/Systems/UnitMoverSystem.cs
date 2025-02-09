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

        UnitMoverJob unitMoverJob = new UnitMoverJob
        {
            deltaTime = SystemAPI.Time.DeltaTime
        };

        unitMoverJob.ScheduleParallel();


        //foreach (var (localTransform, unitMover, physicsVelocity) in
        //    SystemAPI.Query<RefRW<LocalTransform>, RefRO<UnitMover>, RefRW<PhysicsVelocity>>())
        //{


        //    float3 moveDirecttion = unitMover.ValueRO.targtePosition - localTransform.ValueRO.Position;

        //    moveDirecttion = math.normalize(moveDirecttion);



        //    localTransform.ValueRW.Rotation =
        //        math.slerp(localTransform.ValueRW.Rotation,
        //        quaternion.LookRotation(moveDirecttion, math.up()),
        //        unitMover.ValueRO.rotateSpeed * SystemAPI.Time.DeltaTime);

        //    physicsVelocity.ValueRW.Linear = moveDirecttion * unitMover.ValueRO.moveSpeed;

        //    // can automaticly rotate
        //    physicsVelocity.ValueRW.Angular = float3.zero;
        //    //localTrasform.ValueRW.Position += moveDirecttion * SystemAPI.Time.DeltaTime;
        //}


    }
}
[BurstCompile]
public partial struct UnitMoverJob : IJobEntity
{
    public float deltaTime;
    public void Execute(ref LocalTransform localTransform, in UnitMover unitMover, ref PhysicsVelocity physicsVelocity)
    {
        float3 moveDirecttion = unitMover.targtePosition - localTransform.Position;

        float reachedTargetDistance = 2f;
        if (math.lengthsq(moveDirecttion) < reachedTargetDistance)
        {
            physicsVelocity.Linear = float3.zero;
            physicsVelocity.Angular = float3.zero;
            return;
        }

        moveDirecttion = math.normalize(moveDirecttion);
        localTransform.Rotation =
            math.slerp(localTransform.Rotation,
            quaternion.LookRotation(moveDirecttion, math.up()), unitMover.rotateSpeed * deltaTime);

        physicsVelocity.Linear = moveDirecttion * unitMover.moveSpeed;

        // can automaticly rotate
        physicsVelocity.Angular = float3.zero;

    }
}
