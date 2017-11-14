using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// GameObject をドラッグで動かす機能を持つ。
/// ドラッグによって動かしたい GameObject に追加して使う。
/// 2D でのみ使用可能。
/// タッチ非対応。タッチに対応させたい場合は、このクラスを GameObject に追加した上で DragManager2D を併用する。
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DraggableObject2D : MonoBehaviour
{
    bool m_isDragging = false;

    void OnMouseDown()
    {
        if (!Input.touchSupported)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Vector2.zero);
            if (hit && hit.collider.gameObject.Equals(this.gameObject))
                m_isDragging = true;
        }
    }

    void OnMouseUp()
    {
        // マウス使用時、及びシングルタッチの時の処理
        if (!Input.touchSupported)
            m_isDragging = false;
    }

    void OnMouseDrag()
    {
        // マウス使用時、及びシングルタッチの時の処理
        if (!Input.touchSupported)
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));
            this.transform.position = worldPoint;
        }
    }
}
