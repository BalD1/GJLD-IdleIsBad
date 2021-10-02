using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableGround : MonoBehaviour
{
    [SerializeField] private float timeBeforeDestruction = 1;
    [SerializeField] private Animator animator;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
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
