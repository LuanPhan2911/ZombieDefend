using Unity.Burst;
using Unity.Entities;

partial struct ShootAttackSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (target, shootAttack) in SystemAPI.Query<RefRO<Target>, RefRW<ShootAttack>>())
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

            int damage = 1;
            RefRW<Health> health = SystemAPI.GetComponentRW<Health>(target.ValueRO.targetEntity);
            health.ValueRW.healthAmount -= damage;

        }

    }
}
