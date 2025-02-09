using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class UnitMoverAuthoring : MonoBehaviour
{
    public float moveSpeed;
    public float rotateSpeed;

    public class Baker : Baker<UnitMoverAuthoring>
    {
        public override void Bake(UnitMoverAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new UnitMover
            {
                moveSpeed = authoring.moveSpeed,
                rotateSpeed = authoring.rotateSpeed
            });
        }
    }
}
public struct UnitMover : IComponentData
{
    public float moveSpeed;
    public float rotateSpeed;
    public float3 targtePosition;
}
