using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderCameraScript : MonoBehaviour
{

    public GameData gameData;
    public RenderTexture Rt;

    public Transform planeTransform;
    void Update()
    {
        transform.position = new Vector3(planeTransform.position.x, transform.position.y, planeTransform.position.z);
    }

    public void SavePNG() {
        StartCoroutine(SavePNGCoro());
    }
    IEnumerator SavePNGCoro() {
        yield return new WaitForEndOfFrame();

        RenderTexture.active = Rt;

        Texture2D tex = new Texture2D(Rt.width, Rt.height);
        tex.ReadPixels(new Rect(0, 0, Rt.width, Rt.height), 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);

        string finalFileName = gameData.fileName;
        if (finalFileName == string.Empty)
            finalFileName = System.DateTime.Now.ToString("ddMMyyyy HHmm");
            
        File.WriteAllBytes(Application.dataPath + "/PNG/" + finalFileName + ".png", bytes);

        Debug.Log("Saved image!");
    }   
}
