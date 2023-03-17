using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "CustomTileData", menuName = "Data/TilesData", order = 10)]
public class CustomTileData : ScriptableObject
{
    [SerializeField] private Tile tile;
    [SerializeField] private bool isValidTile = true;
    [SerializeField] private int cost = 1;

    public int Cost
    {
        get
        {
            if (isValidTile) return cost;
            return int.MaxValue;
        }
    }

    public Tile Tile => tile;
    public bool IsValidTile => isValidTile;
}
