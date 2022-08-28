using UnityEngine;
[CreateAssetMenu(fileName = "LevelData", menuName = "ScriptableObjects/Level", order = 1)]
public class LevelData : ScriptableObject
{
    public int numberOfTiles;
    public int enemyTurrets;
    public float previewTime;
}
