using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct RandomWalkingSystem : ISystem
{

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (randomWalking, unitMover, localTransform) in
            SystemAPI.Query<RefRW<RandomWalking>, RefRW<UnitMover>, RefRO<LocalTransform>>())
        {
            if (math.distancesq(localTransform.ValueRO.Position, randomWalking.ValueRO.targetPosition) < UnitMoverSystem.READ_TARGET_DISTANCE_SQ)
            {
                // reached target position
                Random random = randomWalking.ValueRO.random;
                float3 randomDirection = new float3(random.NextFloat(-1f, 1f), 0, random.NextFloat(-1f, 1f));
                randomDirection = math.normalize(randomDirection);

                randomWalking.ValueRW.targetPosition = randomWalking.ValueRO.originPosition +
                    randomDirection * random.NextFloat(randomWalking.ValueRO.minDistance, randomWalking.ValueRO.maxDistance);

                randomWalking.ValueRW.random = random;

            }
            else
            {
                // move closer to target position
                unitMover.ValueRW.targetPosition = randomWalking.ValueRO.targetPosition;
            }
        }
    }


}
