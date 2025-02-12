using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

class RandomWalkingAuthoring : MonoBehaviour
{



    class RandomWalkingAuthoringBaker : Baker<RandomWalkingAuthoring>
    {
        public override void Bake(RandomWalkingAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new RandomWalking());
        }
    }
}
public struct RandomWalking : IComponentData
{
    public float3 originPosition;
    public float3 targetPosition;
    public float minDistance;
    public float maxDistance;
    public Unity.Mathematics.Random random;
}


