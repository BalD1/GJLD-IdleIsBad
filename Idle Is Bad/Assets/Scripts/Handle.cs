using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Handle : MonoBehaviour
{
    [SerializeField] private Slider parentSlider;
    [SerializeField] private List<Sprites> sprites;
    
    [System.Serializable]
    private struct Sprites
    {
        [Range(0, 1)] public float minValue;
        [Range(0, 1)] public float maxValue;
        public Sprite targetSprite;
    }

    private void Start()
    {
        AudioManager.Instance.OnVolumeChange += ChangeSprite;
        foreach(Sprites s in sprites)
        {
            if(parentSlider.value >= s.minValue && parentSlider.value <= s.maxValue)
            {
                this.GetComponent<Image>().sprite = s.targetSprite;
            }
        }
    }

    private void ChangeSprite(object sender, AudioManager.OnVolumeChangeEventArgs VolumeArgs)
    {
        foreach(Sprites s in sprites)
        {
            if(parentSlider.value >= s.minValue && parentSlider.value <= s.maxValue)
            {
                this.GetComponent<Image>().sprite = s.targetSprite;
            }
        }
    }
}
