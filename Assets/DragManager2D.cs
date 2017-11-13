using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager2D : MonoBehaviour
{
    Dictionary<int, GameObject> m_objectCache = new Dictionary<int, GameObject>();
	
    void Start()
    {
        if (Input.touchSupported)
            Debug.Log("Touch supported.");
        else
            Debug.Log("Touch is not supported.");
    }

	void Update () {
        
        if (Input.touchSupported)
        {
            foreach (var t in Input.touches)
            {
                if (t.phase == TouchPhase.Began)
                {
                    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(t.position);
                    RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                    if (hit)
                    {
                        if (hit.collider.gameObject.GetComponent<DraggableObject2D>())
                        {
                            m_objectCache.Remove(t.fingerId);
                            m_objectCache.Add(t.fingerId, hit.collider.gameObject);
                        }
                    }
                    break;
                }

                if ((t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled) && m_objectCache.ContainsKey(t.fingerId))
                {
                    m_objectCache.Remove(t.fingerId);
                    break;
                }

                GameObject go = null;

                if (t.phase == TouchPhase.Moved && m_objectCache.TryGetValue(t.fingerId, out go))
                {
                    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(t.position);
                    go.transform.position = worldPoint;
                }
            }
        }
	}
}
