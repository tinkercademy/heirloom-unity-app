using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New GameData", menuName = "ScriptableObjects/GameData")]
public class GameData : ScriptableObject
{

    public enum GameMode {Sew, Options};

    public GameMode gameMode;
    public string fileName;

    public Vector3 needleScreenPosition;
    public float needleScreenRadius;

}
