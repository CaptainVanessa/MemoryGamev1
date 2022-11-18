using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBehaviour : MonoBehaviour
{
    public Material _cardMaterial;
    public LevelManager manager;
    private Animator animator;
    public AudioSource source;
    public AudioClip clipHover;
    public AudioClip clipSelect;
    public AudioClip clipDeselect;
    public AudioClip clipMatch;

    void Start()
    {
        GetComponent<Renderer>().material = _cardMaterial;
        animator = GetComponent<Animator>();
    }

    void OnMouseUp(){
        manager.CardRevealed(this);
    }

    void OnMouseEnter(){
        animator.SetBool("mouseHovering", true);
        PlaySound(clipHover);
    }
    void OnMouseExit(){
        animator.SetBool("mouseHovering", false);
    }
        
    public void RevealCard(){
        animator.SetBool("cardSelected", true);
        PlaySound(clipSelect);
    }

    public void UnrevealCard(){
        animator.SetBool("cardSelected", false);
        PlaySound(clipDeselect);
    }
        
    public void MatchCard(){
        PlaySound(clipMatch);
        Destroy(gameObject, clipMatch.length);
    }

    public void PlaySound(AudioClip currentClip){
        source.clip = currentClip;
        if(source.isPlaying) return;
        if(!source.isPlaying){
            source.Play();
        }
    }
}
