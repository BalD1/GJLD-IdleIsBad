using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player player = collision.gameObject.GetComponent<Player>();
        if(player != null)
        {
            AudioManager.Instance.Play2DSound(AudioManager.ClipsTags.Key);
            player.HasKey = true;
            Destroy(this.gameObject);
        }
    }
}
