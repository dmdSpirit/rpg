using UnityEngine;

public class GameHandler : MonoSingleton<GameHandler>
{
    public int maxActionPoints = 6;

    public Color actionPointsColor;
    public Color usedActionPointsColor;

    public Color sourcePointsColor;
    public Color usedSourcePointsColor;
}