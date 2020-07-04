using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FabricPlaneTouchScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameData gameData;
    public Camera mainCamera;
    public GameObject threadPrefab;

    private Vector3? threadStartPos = null, threadEndPos = null;

    void Update()
    {   
        if (Input.touchCount == 0) return;

        foreach (Touch t in Input.touches)
        {
            if (t.phase == TouchPhase.Began && IsTouchNearNeedle(t.position))
            {
                RaycastHit hit;
                if (Physics.Raycast(mainCamera.ScreenPointToRay(t.position), out hit)) 
                    DrawThread(hit.point);
            }
        }
    }

    bool IsTouchNearNeedle(Vector3 touchPos)
    {
        return Vector3.Distance(touchPos, gameData.needleScreenPosition) < gameData.needleScreenRadius;
    }
    void DrawThread(Vector3 p) 
    {   

        if (threadStartPos == null) 
        {
            threadStartPos = p;
            return;
        }

        // Set end pos of thread
        threadEndPos = p;

        // Create thread object
        Vector3 threadPos = (Vector3) (threadStartPos + threadEndPos)/2;
        Quaternion threadRotation = Quaternion.LookRotation((Vector3) (threadEndPos - threadStartPos));
        threadRotation *= Quaternion.Euler(90, 0, 0);
        GameObject threadObject = Instantiate(threadPrefab, threadPos, threadRotation, transform);

        float scaleFactor = Vector3.Magnitude((Vector3) (threadEndPos - threadStartPos))/2;
        threadObject.transform.localScale = new Vector3(0.1f, scaleFactor, 0.1f);

        // Move start pos of next thread to end pos of previous thread
        threadStartPos = threadEndPos;
    }

    void UpdateThreadPos(Vector3 displacement)
    {
        
        if (!(threadStartPos == null)) threadStartPos += displacement;

        if (!(threadEndPos == null)) threadEndPos += displacement;

    }

    void OnDrawGizmos()
    {
        Vector3 sphereScreenPos = new Vector3(gameData.needleScreenPosition.x, gameData.needleScreenPosition.y, mainCamera.transform.position.y);
        Vector3 spherePos = mainCamera.ScreenToWorldPoint(sphereScreenPos);
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(spherePos, 0.3f);

        if (threadStartPos == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere((Vector3) threadStartPos, 0.1f);

        if (threadEndPos == null) return;
        Gizmos.color = Color.red;
        Gizmos.DrawSphere((Vector3) threadEndPos, 0.1f);

    }
    
    private Vector3 initFabricPos;
    private Vector3 initTouchWorldPos;
    private bool isDraggingFabric = false;
    private int? fingerID = null;
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (IsTouchNearNeedle(eventData.position)) return;
        if (!(fingerID == null)) return;

        fingerID = eventData.pointerId;
        
        isDraggingFabric = true;

        initFabricPos = transform.position;
        initTouchWorldPos
        = mainCamera.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, mainCamera.transform.position.y));
    
        Debug.Log("OnBeginDrag");
    }
    float t = 0;
    public void OnDrag(PointerEventData data)
    {
        if (!isDraggingFabric) return;
        if (!(data.pointerId == fingerID)) return;

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
        fingerID = null;

        isDraggingFabric = false;

        Debug.Log("OnEndDrag");
    } 
}
