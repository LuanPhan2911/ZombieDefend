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

    }
}
[BurstCompile]
public partial struct UnitMoverJob : IJobEntity
{
    public float deltaTime;
    public void Execute(ref LocalTransform localTransform, in UnitMover unitMover, ref PhysicsVelocity physicsVelocity)
    {
        float3 moveDirecttion = unitMover.targetPosition - localTransform.Position;

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
