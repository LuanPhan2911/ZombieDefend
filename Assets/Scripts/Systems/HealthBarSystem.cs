using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateInGroup(typeof(LateSimulationSystemGroup))]
partial struct HealthBarSystem : ISystem
{



    public void OnUpdate(ref SystemState state)
    {
        UnityEngine.Vector3 cameraForward = Vector3.zero;
        if (Camera.main != null)
        {
            cameraForward = Camera.main.transform.forward;
        }
        foreach (var (healthBar, localTransform) in
            SystemAPI.Query<RefRW<HealthBar>, RefRW<LocalTransform>>())
        {

            LocalTransform parentLocalTransform = SystemAPI.GetComponent<LocalTransform>(healthBar.ValueRO.healthEntity);

            if (localTransform.ValueRO.Scale == 1f)
            {
                localTransform.ValueRW.Rotation =
               parentLocalTransform.InverseTransformRotation(quaternion.LookRotation(cameraForward, math.up()));
            }


            Health health = SystemAPI.GetComponent<Health>(healthBar.ValueRO.healthEntity);
            if (!health.onHealthChanged)
            {
                continue;
            }


            float healthNormalized = (float)health.healthAmount / health.healthAmountMax;

            if (healthNormalized == 1f)
            {
                localTransform.ValueRW.Scale = 0;
            }
            else
            {
                localTransform.ValueRW.Scale = 1;
            }

            RefRW<PostTransformMatrix> postTranformMatrix =
                  SystemAPI.GetComponentRW<PostTransformMatrix>(healthBar.ValueRO.healthBarVisualEntity);

            postTranformMatrix.ValueRW.Value = float4x4.Scale(healthNormalized, 1, 1);
        }
    }


}
