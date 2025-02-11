using Unity.Burst;
using Unity.Entities;

partial struct TestingSystem : ISystem
{


    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        //int count = 0;
        //foreach (var friendly in SystemAPI.Query<RefRW<Friendly>>())
        //{
        //    count++;

        //}
        //UnityEngine.Debug.Log($"Fiendly {count}");
    }


}
