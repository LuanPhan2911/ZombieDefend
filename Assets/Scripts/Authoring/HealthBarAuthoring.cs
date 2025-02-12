using Unity.Entities;
using UnityEngine;

public class HealthBarAuthoring : MonoBehaviour
{
    public GameObject healthBarVisualGameObject;
    public GameObject healthGameObject;
    public class HealthBarAuthoringBaker : Baker<HealthBarAuthoring>
    {
        public override void Bake(HealthBarAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new HealthBar
            {
                healthBarVisualEntity = GetEntity(authoring.healthBarVisualGameObject, TransformUsageFlags.NonUniformScale),
                healthEntity = GetEntity(authoring.healthGameObject, TransformUsageFlags.Dynamic)
            });
        }
    }
}
public struct HealthBar : IComponentData
{
    public Entity healthBarVisualEntity;
    public Entity healthEntity;
}


