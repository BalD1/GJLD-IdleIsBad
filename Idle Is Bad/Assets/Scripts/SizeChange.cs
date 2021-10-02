using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SizeChange : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private Vector2 size;
    [SerializeField] private bool changeColliderX = true;
    [SerializeField] private bool changeColliderY = true;

    void OnGUI()
    {
        if (changeColliderX && changeColliderY)
        {
        }
        if (!changeColliderX)
        {
            size.x = this.gameObject.GetComponent<BoxCollider2D>().size.x;
        }
        if (!changeColliderY)
        {
            size.y = this.gameObject.GetComponent<BoxCollider2D>().size.y;
        }
        this.gameObject.GetComponent<BoxCollider2D>().size = size;
        this.gameObject.GetComponent<SpriteRenderer>().size = size;
    }
#endif
}
