using Unity.Entities;
using UnityEngine;

class ShootLightAuhtoring : MonoBehaviour
{
    public float timer;
    public class ShootLightAuhtoringBaker : Baker<ShootLightAuhtoring>
    {
        public override void Bake(ShootLightAuhtoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ShootLight
            {
                timer = authoring.timer
            });
        }
    }
}
public struct ShootLight : IComponentData
{
    public float timer;
}


