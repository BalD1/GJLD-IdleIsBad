using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableGround : MonoBehaviour
{
    [SerializeField] private float timeBeforeDestruction = 1;
    [SerializeField] private Animator animator;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("PlayerFeets"))
        {
#if UNITY_EDITOR
            if(!GameManager.Instance.canDestroy)
                return;
#endif
            StartCoroutine(destructionTimer(timeBeforeDestruction));
        }
    }

    private IEnumerator destructionTimer(float timer)
    {
        animator.SetTrigger("Break");
        yield return new WaitForSeconds(timer);
        Destroy(this.gameObject);
    }
}
