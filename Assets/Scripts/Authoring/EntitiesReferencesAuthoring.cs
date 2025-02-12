using Unity.Entities;
using UnityEngine;

class EntitiesReferencesAuthoring : MonoBehaviour
{
    public GameObject bulletGameObject;
    public class EntitiesReferencesAuthoringBaker : Baker<EntitiesReferencesAuthoring>
    {
        public override void Bake(EntitiesReferencesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntitiesReferences
            {
                bulletPrefabEntity = GetEntity(authoring.bulletGameObject, TransformUsageFlags.Dynamic)
            });
        }
    }
}
public struct EntitiesReferences : IComponentData
{
    public Entity bulletPrefabEntity;
}


