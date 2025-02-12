using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

partial struct ShootAttackSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();
        foreach (var (target, shootAttack, localTransform, unitMover) in
            SystemAPI.Query<RefRO<Target>, RefRW<ShootAttack>, RefRW<LocalTransform>, RefRW<UnitMover>>())
        {


            if (target.ValueRO.targetEntity == Entity.Null)
            {
                continue;
            }

            RefRO<LocalTransform> targetLocalTransform = SystemAPI.GetComponentRO<LocalTransform>(target.ValueRO.targetEntity);
            if (math.distance(localTransform.ValueRO.Position, targetLocalTransform.ValueRO.Position) > shootAttack.ValueRO.attackDistance)
            {
                // target is out of range, move closer

                unitMover.ValueRW.targetPosition = targetLocalTransform.ValueRO.Position;
                continue;
            }
            else
            {
                // target is in range, stop moving
                unitMover.ValueRW.targetPosition = localTransform.ValueRO.Position;
            }
            float3 aimDirection = math.normalize(targetLocalTransform.ValueRO.Position - localTransform.ValueRO.Position);

            localTransform.ValueRW.Rotation = math.slerp(localTransform.ValueRO.Rotation,
                quaternion.LookRotation(aimDirection, math.up()), unitMover.ValueRO.rotateSpeed * SystemAPI.Time.DeltaTime);

            if (shootAttack.ValueRO.timer > 0f)
            {
                shootAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;
                continue;
            }
            shootAttack.ValueRW.timer = shootAttack.ValueRO.timerMax;


            // instantiate bullet
            Entity bulletEntity = state.EntityManager.Instantiate(entitiesReferences.bulletPrefabEntity);

            float3 bulletSpawnPosition = localTransform.ValueRO.TransformPoint(shootAttack.ValueRO.bulletSpawnLocalPosition);
            state.EntityManager.SetComponentData(bulletEntity,
                LocalTransform.FromPosition(bulletSpawnPosition));

            RefRW<Bullet> bullet = SystemAPI.GetComponentRW<Bullet>(bulletEntity);
            bullet.ValueRW.damageAmount = shootAttack.ValueRO.damgeAmount;

            RefRW<Target> bulletTarget = SystemAPI.GetComponentRW<Target>(bulletEntity);
            bulletTarget.ValueRW.targetEntity = target.ValueRO.targetEntity;




        }

    }
}
