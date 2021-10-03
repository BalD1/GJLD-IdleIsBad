using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bzzter : MonoBehaviour
{
    [SerializeField] private bool shifted = false;
    [SerializeField] private float shiftTime = 0f;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        if(shifted)
            StartCoroutine(Shift(shiftTime));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag.Equals("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.Death();
        }

    }

    private IEnumerator Shift(float time)
    {
        animator.speed = 0;
        yield return new WaitForSeconds(time);
        animator.speed = 1;
    }
}
