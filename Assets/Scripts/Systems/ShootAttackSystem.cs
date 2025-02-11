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

            UnityEngine.Debug.Log("Shoot!");

        }

    }
}
