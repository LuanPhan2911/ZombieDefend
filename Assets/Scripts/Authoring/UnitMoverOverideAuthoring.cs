using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class UnitMoverOverideAuthoring : MonoBehaviour
{
    public class UnitMoverOverideAuthoringBaker : Baker<UnitMoverOverideAuthoring>
    {
        public override void Bake(UnitMoverOverideAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new UnitMoverOveride());
            SetComponentEnabled<UnitMoverOveride>(entity, false);
        }
    }
}
public struct UnitMoverOveride : IComponentData, IEnableableComponent
{
    public float3 targetPosition;
}


