using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// オブジェクトをタッチで動かす機能を持つ。
/// 任意の GameObject に追加して使う。ただし、シーン上に一つだけ存在するようにすること。
/// 動かしたい対象のオブジェクトには DraggableObject2D を追加すること。
/// 現状では TouchPhase.Began/TouchPhase.Ended を見逃す問題が起きている。
/// </summary>
public class DragManager2D : MonoBehaviour
{
    Dictionary<int, GameObject> m_objectCache = new Dictionary<int, GameObject>();
	
    void Start()
    {
        Application.targetFrameRate = 60;   // 低フレームレートだと TouchPhase.Began を見逃す問題が多発する。
        Debug.Log("touchSupported: " + Input.touchSupported);
    }

	void Update ()
    {
        // TouchPhase.Ended を見逃すバグ対応
        if (Input.touchSupported && Input.touchCount == 0 && m_objectCache.Count > 0)
        {
            Debug.LogWarning("Invalid status.");
            m_objectCache.Clear();
        }

        if (Input.touchCount == 0 || !Input.touchSupported)
            return;

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
                VerifyTouchStatus(Input.touches);
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

    // TouchPhase.Ended を見逃すバグ対応
    bool VerifyTouchStatus(Touch[] touchArray)
    {
        List<int> keyList = new List<int>(m_objectCache.Keys);
        foreach (var k in keyList)
        {
            bool isFound = false;

            foreach (var t in touchArray)
            {
                if (t.fingerId == k)
                {
                    isFound = true;
                    break;
                }
            }

            if (!isFound)
            {
                Debug.LogWarning("invalid key: " + k + ". Remove...");
                m_objectCache.Remove(k);
                return false;
            }
        }
        return true;
    }
}
