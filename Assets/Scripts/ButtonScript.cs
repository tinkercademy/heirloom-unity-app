using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ButtonScript : MonoBehaviour
{

    public GameObject extendedPanelPrefab;
    public abstract void DisplayExtended(Transform parentTransform);

    public abstract void DestroyExtended();

}
