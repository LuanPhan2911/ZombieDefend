using Unity.Entities;
using UnityEngine;

public class ZombieSpawnerAuthoring : MonoBehaviour
{
    public float timerMax;
    public float randomWalkingMinDistance;
    public float randomWalkingMaxDistance;

    public class ZombieSpawnerAuthoringBaker : Baker<ZombieSpawnerAuthoring>
    {
        public override void Bake(ZombieSpawnerAuthoring authoring)
        {
            Entity entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponent(entity, new ZombieSpawner
            {
                timerMax = authoring.timerMax,
                randomWalkingMinDistance = authoring.randomWalkingMinDistance,
                randomWalkingMaxDistance = authoring.randomWalkingMaxDistance

            });
        }
    }
}
public struct ZombieSpawner : IComponentData
{
    public float timer;
    public float timerMax;

    public float randomWalkingMinDistance;
    public float randomWalkingMaxDistance;

}


