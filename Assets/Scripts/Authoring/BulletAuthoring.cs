using Unity.Entities;
using UnityEngine;

class BulletAuthoring : MonoBehaviour
{
    public int speed;
    public int damageAmount;
    public class BulletAuthoringBaker : Baker<BulletAuthoring>
    {
        public override void Bake(BulletAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new Bullet
            {
                speed = authoring.speed,
                damageAmount = authoring.damageAmount
            });
        }
    }
}
public struct Bullet : IComponentData
{
    public int speed;
    public int damageAmount;
}


