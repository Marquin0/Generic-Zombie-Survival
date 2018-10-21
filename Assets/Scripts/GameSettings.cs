using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSettings : MonoBehaviour {

    public static BoardSize BoardSize;
    public static bool MazeMode;

    public static readonly Vector2 SmallSize = new Vector2(50, 50);
    public static readonly Vector2 MediumSize = new Vector2(100, 100);
    public static readonly Vector2 LargeSize = new Vector2(150, 150);

    public static void SetGameSettings(BoardSize boardSize, bool mazeMode)
    {
        BoardSize = boardSize;
        MazeMode = mazeMode;
    }

    public static Vector2 GetGameBoardSize()
    {
        switch(BoardSize)
        {
            case BoardSize.Small:
                return SmallSize;
            case BoardSize.Medium:
                return MediumSize;
            case BoardSize.Large:
                return LargeSize;
            default:
                return new Vector2();
        }
    }
}

public enum BoardSize
{
    Small,
    Medium,
    Large
}