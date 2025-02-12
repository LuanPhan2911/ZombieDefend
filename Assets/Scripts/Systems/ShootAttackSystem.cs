using Unity.Burst;
using Unity.Entities;
using Unity.Transforms;

partial struct ShootAttackSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        EntitiesReferences entitiesReferences = SystemAPI.GetSingleton<EntitiesReferences>();
        foreach (var (target, shootAttack, localTransform) in
            SystemAPI.Query<RefRO<Target>, RefRW<ShootAttack>, RefRO<LocalTransform>>())
        {
            if (target.ValueRO.targetEntity == Entity.Null)
            {
                continue;
            }

            if (shootAttack.ValueRO.timer > 0f)
            {
                shootAttack.ValueRW.timer -= SystemAPI.Time.DeltaTime;
                continue;
            }
            shootAttack.ValueRW.timer = shootAttack.ValueRO.timerMax;

            // instantiate bullet
            Entity bulletEntity = state.EntityManager.Instantiate(entitiesReferences.bulletPrefabEntity);
            state.EntityManager.SetComponentData(bulletEntity,
                LocalTransform.FromPosition(localTransform.ValueRO.Position));

            RefRW<Bullet> bullet = SystemAPI.GetComponentRW<Bullet>(bulletEntity);
            bullet.ValueRW.damageAmount = shootAttack.ValueRO.damgeAmount;

            RefRW<Target> bulletTarget = SystemAPI.GetComponentRW<Target>(bulletEntity);
            bulletTarget.ValueRW.targetEntity = target.ValueRO.targetEntity;


        }

    }
}
