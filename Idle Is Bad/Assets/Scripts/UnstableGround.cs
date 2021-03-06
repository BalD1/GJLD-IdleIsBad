using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstableGround : MonoBehaviour
{
    [SerializeField] [Range(0, 1.75f)] private float timeBeforeDestruction = 1;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioSource audioSource;
    private AudioClip breakSound;

    private float animatorSpeed;

    private void Start()
    {
        animatorSpeed = 2 - timeBeforeDestruction;

        animator.speed = animatorSpeed;
        breakSound = AudioManager.Instance.GetAudioClip(AudioManager.ClipsTags.Wood);
    }


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
        audioSource.PlayOneShot(breakSound);
        yield return new WaitForSeconds(timer);
        Destroy(this.transform.parent.gameObject);
    }
}
