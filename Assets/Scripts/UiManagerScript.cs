using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiManagerScript : MonoBehaviour
{
    public GameData gameData;
    public Transform parentTransform;

    public GameObject backgroundPanel;

    public List<ButtonScript> buttonScripts;

    public void SwitchMode()
    {

        switch (gameData.gameMode)
        {
            case GameData.GameMode.Sew:
                gameData.gameMode = GameData.GameMode.Options;
                backgroundPanel.SetActive(true);
                DestroyAllExtended();
                break;
            case GameData.GameMode.Options:
                gameData.gameMode = GameData.GameMode.Sew;
                backgroundPanel.SetActive(false);
                break;
        }
    }
    
    public void DestroyAllExtended()
    {
        foreach (ButtonScript b in buttonScripts)
        {
            b.DestroyExtended();
        }
    }
    public void DisplayButtonExtended(ButtonScript b)
    {
        b.DisplayExtended(parentTransform);
    }
}
