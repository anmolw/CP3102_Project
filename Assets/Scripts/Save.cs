using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save {
    int levelReached = 1;
    int[] highScores = new int[20];
    Vector2[] spritePositions = new Vector2[20];
}
