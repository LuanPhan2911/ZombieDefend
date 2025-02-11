using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Physics;
using Unity.Transforms;
using UnityEngine;

public class UnitSelectionManager : MonoBehaviour
{
    public static UnitSelectionManager Instance { get; private set; }
    public event EventHandler OnSelectionAreaStart;
    public event EventHandler OnSelectionAreaEnd;
    private Vector2 selectionStartMousePosition;

    private void Awake()
    {
        Instance = this;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectionStartMousePosition = Input.mousePosition;
            OnSelectionAreaStart?.Invoke(this, EventArgs.Empty);
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 selectionEndMousePosition = Input.mousePosition;
            OnSelectionAreaEnd?.Invoke(this, EventArgs.Empty);

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<Selected>().Build(entityManager);

            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);
            NativeArray<Selected> selectedArray = entityQuery.ToComponentDataArray<Selected>(Allocator.Temp);
            //deselect all units
            for (int i = 0; i < selectedArray.Length; i++)
            {
                Selected selected = selectedArray[i];
                entityManager.SetComponentEnabled<Selected>(entityArray[i], false);
                selected.onDeselected = true;
                selectedArray[i] = selected;

                entityManager.SetComponentData(entityArray[i], selected);
            }

            Rect selectionArea = GetSelectionAreaRect();
            float selectionAreaSize = selectionArea.width * selectionArea.height;
            float minAreaSize = 50;
            if (selectionAreaSize > minAreaSize)
            {
                // select multiple units
                entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<LocalTransform, Unit>()
              .WithPresent<Selected>().Build(entityManager);

                NativeArray<LocalTransform> localTransformArray = entityQuery.ToComponentDataArray<LocalTransform>(Allocator.Temp);
                entityArray = entityQuery.ToEntityArray(Allocator.Temp);
                for (int i = 0; i < localTransformArray.Length; i++)
                {
                    LocalTransform localTransform = localTransformArray[i];
                    Vector2 screenPosition = Camera.main.WorldToScreenPoint(localTransform.Position);
                    if (GetSelectionAreaRect().Contains(screenPosition))
                    {
                        //unit is inside selected area
                        entityManager.SetComponentEnabled<Selected>(entityArray[i], true);
                        Selected selected = entityManager.GetComponentData<Selected>(entityArray[i]);
                        selected.onSelected = true;
                        entityManager.SetComponentData(entityArray[i], selected);
                    }

                }
            }
            else
            {
                // select single unit
                UnityEngine.Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
                entityQuery = entityManager.CreateEntityQuery(
                    new EntityQueryBuilder(Allocator.Temp).WithAll<PhysicsWorldSingleton>());
                PhysicsWorldSingleton physicsWorldSingleton = entityQuery.GetSingleton<PhysicsWorldSingleton>();
                CollisionWorld collisionWorld = physicsWorldSingleton.CollisionWorld;


                RaycastInput raycastInput = new RaycastInput
                {
                    Start = mouseRay.origin,
                    End = mouseRay.origin + mouseRay.direction * 1000,
                    Filter = new CollisionFilter
                    {
                        BelongsTo = ~0u,
                        CollidesWith = 1u << GameAssets.unitLayer,
                        GroupIndex = 0
                    }
                };
                if (collisionWorld.CastRay(raycastInput, out Unity.Physics.RaycastHit hit))
                {
                    if (entityManager.HasComponent<Unit>(hit.Entity) &&
                        entityManager.HasComponent<Selected>(hit.Entity))
                    {
                        entityManager.SetComponentEnabled<Selected>(hit.Entity, true);
                        Selected selected = entityManager.GetComponentData<Selected>(hit.Entity);
                        selected.onSelected = true;
                        entityManager.SetComponentData(hit.Entity, selected);
                    }
                }

            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 mouseWorldPosition = MouseWorldPosition.Instance.GetPosition();

            EntityManager entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;
            EntityQuery entityQuery = new EntityQueryBuilder(Allocator.Temp).WithAll<UnitMover, Selected>().Build(entityManager);

            NativeArray<UnitMover> unitMoverArray = entityQuery.ToComponentDataArray<UnitMover>(Allocator.Temp);
            NativeArray<Entity> entityArray = entityQuery.ToEntityArray(Allocator.Temp);

            NativeArray<float3> movePositionArray = GenerateMovePositionArray(mouseWorldPosition, entityArray.Length);
            for (int i = 0; i < unitMoverArray.Length; i++)
            {
                UnitMover unitMover = unitMoverArray[i];
                unitMover.targtePosition = movePositionArray[i];
                unitMoverArray[i] = unitMover;
            }
            entityQuery.CopyFromComponentDataArray(unitMoverArray);

        }

    }
    public Rect GetSelectionAreaRect()
    {
        Vector2 selectionEndMousePosition = Input.mousePosition;
        Vector2 lowerLeftCorner = new Vector2(
            Mathf.Min(selectionStartMousePosition.x, selectionEndMousePosition.x),
            Mathf.Min(selectionStartMousePosition.y, selectionEndMousePosition.y)
        );
        Vector2 upperRightCorner = new Vector2(
            Mathf.Max(selectionStartMousePosition.x, selectionEndMousePosition.x),
            Mathf.Max(selectionStartMousePosition.y, selectionEndMousePosition.y)
        );
        return new Rect(lowerLeftCorner, upperRightCorner - lowerLeftCorner);
    }

    private NativeArray<float3> GenerateMovePositionArray(float3 targetPosition, int positionCount)
    {
        NativeArray<float3> movePositionArray = new NativeArray<float3>(positionCount, Allocator.Temp);
        if (positionCount == 0)
        {
            return movePositionArray;
        }
        movePositionArray[0] = targetPosition;
        if (positionCount == 1)
        {
            return movePositionArray;
        }
        float ringSize = 2.2f;
        int ring = 0, ringIndex = 1;
        while (ringIndex < positionCount)
        {
            int ringPositionCount = 3 + ring * 2;
            for (int i = 0; i < ringPositionCount; i++)
            {
                float angle = i * (math.PI * 2 / ringPositionCount);
                float3 ringVector = math.rotate(quaternion.RotateY(angle), new float3(ringSize * (ring + 1), 0, 0));
                float3 ringPosition = targetPosition + ringVector;
                movePositionArray[ringIndex] = ringPosition;
                ringIndex++;
                if (ringIndex >= positionCount)
                {
                    break;
                }
            }
            ring++;
        }
        return movePositionArray;

    }
}
