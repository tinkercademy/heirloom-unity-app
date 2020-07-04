using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ErrorScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("This be normal log!");
        Debug.LogWarning("This be warning log!!");
        Debug.LogError("This be error log!!!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
