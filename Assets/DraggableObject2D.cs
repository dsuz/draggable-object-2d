using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameObject をドラッグ・タッチで動かす機能を持つ。
/// ドラッグまたはタッチによって動かしたい GameObject に追加して使う。
/// 2D でのみ使用可能。
/// <todo>マルチタッチ時、激しく操作すると誤動作することがある。おそらく fingerId の取扱いに問題がある。
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DraggableObject2D : MonoBehaviour
{
    /// <summary>
    /// タッチ操作時に、動かしている指のID。マウス使用時は fingerId = 0 とする。操作されていない時は -1 とする。
    /// </summary>
    int m_fingerId = -1;

    void OnMouseDown()
    {
        // マウス使用時、及びシングルタッチの時の処理
        if (Input.touchCount < 2)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit && hit.collider.gameObject.Equals(this.gameObject))
            {
                m_fingerId = 0;
            }
        }
    }

    void OnMouseUp()
    {
        // マウス使用時、及びシングルタッチの時の処理
        if (Input.touchCount < 2)
            m_fingerId = -1;
    }

    void OnMouseDrag()
    {
        // マウス使用時、及びシングルタッチの時の処理
        if (Input.touchCount < 2 && m_fingerId > -1)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            this.transform.position = worldPoint;
        }
    }

    void Update()
    {
        // マルチタッチの時の処理
        if (Input.touchCount > 1)
        {
            foreach (var t in Input.touches)
            {
                if (t.phase == TouchPhase.Began)
                {
                    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(t.position);
                    RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
                    if (hit && hit.collider.gameObject.Equals(this.gameObject))
                        m_fingerId = t.fingerId;
                }

                if (t.phase == TouchPhase.Moved && t.fingerId == m_fingerId)
                {
                    Vector2 worldPoint = Camera.main.ScreenToWorldPoint(t.position);
                    this.transform.position = worldPoint;
                }

                if (t.fingerId == m_fingerId && (t.phase == TouchPhase.Ended || t.phase == TouchPhase.Canceled))
                    m_fingerId = -1;
            }
        }
    }
}
