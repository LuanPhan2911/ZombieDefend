using Unity.Entities;
using UnityEngine;

class EntitiesReferencesAuthoring : MonoBehaviour
{
    public GameObject bulletGameObject;
    public GameObject zombieGameObject;
    public class EntitiesReferencesAuthoringBaker : Baker<EntitiesReferencesAuthoring>
    {
        public override void Bake(EntitiesReferencesAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new EntitiesReferences
            {
                bulletEntityPrefab = GetEntity(authoring.bulletGameObject, TransformUsageFlags.Dynamic),
                zombieEntityPrefab = GetEntity(authoring.zombieGameObject, TransformUsageFlags.Dynamic),
            });
        }
    }
}
public struct EntitiesReferences : IComponentData
{
    public Entity bulletEntityPrefab;
    public Entity zombieEntityPrefab;

}


