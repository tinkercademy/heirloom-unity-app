using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FabricPlaneScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameData gameData;
    public Camera mainCamera;
    public GameObject threadPrefab;

    private Vector3 threadStartPos, threadEndPos, invalidPos;

    void Start()
    {
        invalidPos = new Vector3(-1,-1,-1);

        threadStartPos = invalidPos;
        threadEndPos = invalidPos;
    }

    void Update()
    {   
        if (!Input.GetMouseButtonDown(0)) return;

        if (!IsTouchNearNeedle()) return;
        
        RaycastHit hit;
        if (Physics.Raycast(mainCamera.ScreenPointToRay(Input.mousePosition), out hit)) 
            DrawThread(hit.point);
    }

    bool IsTouchNearNeedle()
    {
        return Vector3.Distance(Input.mousePosition, gameData.needleScreenPosition) < gameData.needleScreenRadius;
    }
    void DrawThread(Vector3 p) 
    {   

        if (threadStartPos == invalidPos) 
        {
            threadStartPos = p;
            return;
        }

        // Set end pos of thread
        threadEndPos = p;

        // Create thread object
        Vector3 threadPos = (threadStartPos + threadEndPos)/2;
        Quaternion threadRotation = Quaternion.LookRotation(threadEndPos - threadStartPos);
        threadRotation *= Quaternion.Euler(90, 0, 0);
        GameObject threadObject = Instantiate(threadPrefab, threadPos, threadRotation, transform);

        float scaleFactor = Vector3.Magnitude(threadEndPos - threadStartPos)/2;
        threadObject.transform.localScale = new Vector3(0.1f, scaleFactor, 0.1f);

        // Move start pos of next thread to end pos of previous thread
        threadStartPos = threadEndPos;
    }

    void UpdateThreadPos(Vector3 displacement)
    {
        
        if (!(threadStartPos == invalidPos)) threadStartPos += displacement;

        if (!(threadEndPos == invalidPos)) threadEndPos += displacement;

    }

    void OnDrawGizmos()
    {
        Vector3 sphereScreenPos = new Vector3(gameData.needleScreenPosition.x, gameData.needleScreenPosition.y, mainCamera.transform.position.y);
        Vector3 spherePos = mainCamera.ScreenToWorldPoint(sphereScreenPos);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(spherePos, 0.1f);
    }
    
    private Vector3 initFabricPos;
    private Vector3 initTouchWorldPos;
    private bool isDraggingFabric = false;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsTouchNearNeedle())
        {
            isDraggingFabric = false;
            return;
        }
        
        isDraggingFabric = true;

        initFabricPos = transform.position;
        initTouchWorldPos
        = mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, mainCamera.transform.position.y));
        
        Debug.Log(Vector3.Distance(Input.mousePosition, gameData.needleScreenPosition));
        Debug.Log("OnBeginDrag");
    }
    float t = 0;
    public void OnDrag(PointerEventData data)
    {
        if (!isDraggingFabric) return;

        Vector3 currentTouchWorldPos 
        = mainCamera.ScreenToWorldPoint(new Vector3(data.position.x, data.position.y, mainCamera.transform.position.y));
        Vector3 displacementWorld = currentTouchWorldPos - initTouchWorldPos;

        transform.position = initFabricPos + displacementWorld; 

        initFabricPos = transform.position;
        initTouchWorldPos = currentTouchWorldPos;
        
        UpdateThreadPos(displacementWorld);

        t += Time.deltaTime;

        if (t > 0.25f)
        {
            Debug.Log("Dragging:" + data.position);
            t = 0;
        }

    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDraggingFabric) return;

        Debug.Log("OnEndDrag");
    } 
}
