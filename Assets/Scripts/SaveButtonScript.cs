using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SaveButtonScript : ButtonScript
{
    public GameData gameData;
    public RenderCameraScript renderCamera;
    private GameObject extendedPanel;
    
    private bool displaying = false;
    public override void DisplayExtended(Transform parentTransform)
    {
        if (!displaying)
        {
            // If not displaying, create extended panel
            extendedPanel = Instantiate(extendedPanelPrefab, parentTransform);
            
            Button b = extendedPanel.GetComponentInChildren<Button>();
            TMP_InputField tmpInputField = extendedPanel.GetComponentInChildren<TMP_InputField>();
            b.onClick.AddListener(delegate{EditFileName(tmpInputField.text);});
            b.onClick.AddListener(delegate{renderCamera.SavePNG();});
        } else {
            Destroy(extendedPanel);
        }

        displaying = !displaying;
    }

    public override void DestroyExtended()
    {
        Destroy(extendedPanel);
    }

    public void EditFileName(string fileName)
    {
        gameData.fileName = fileName;
    }
}
