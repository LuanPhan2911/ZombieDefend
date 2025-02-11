using UnityEngine;

public class GameAssets : MonoBehaviour
{

    public const int unitLayer = 6;
    public static GameAssets Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
    }

}
