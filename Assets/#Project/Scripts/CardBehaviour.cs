using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// bugs:
// doesn't load pairs necessarily
// only destroys one from the pair
// doesn't deselect properly? or does at the same time as select
// deselect animation has wrong rotation
// to add:
// sfx
// replay button

public class CardBehaviour : MonoBehaviour
{
    public Material hiddenMaterial;
    private Material originalMaterial;
    public LevelManager manager;
    private Animator animator;

    void Start()
    {
        originalMaterial = GetComponent<Renderer>().material; //ou materials[0] si plusieurs dans mesh Renderer
        animator = GetComponent<Animator>(); //ou materials[0] si plusieurs dans mesh Renderer
    }

    void OnMouseUp(){ //que s'il y a collider
        // RevealColor();
        manager.CardRevealed(this);
    }

    void OnMouseEnter(){
        animator.SetBool("mouseHovering", true);
    }
    void OnMouseExit(){
        animator.SetBool("mouseHovering", false);
    }
        
    public void RevealCard(){ //mettre en public pour que l'autre y accède, sinon erreur "inaccessible"
        GetComponent<Renderer>().material = hiddenMaterial;
        animator.SetBool("cardSelected", true);
    }

    public void UnrevealCard(){
        animator.SetBool("cardSelected", true);
        animator.SetBool("cardDeselect", true);
    }
        
    public void MatchCard(){
        // animator.SetBool("cardSelected", true);
        // animator.SetBool("cardDeselect", true);
        Destroy(gameObject); //ne détruit que la dernière de la paire
    }

    // void OnMouseExit(){
    //     GetComponent<Renderer>().material = originalMaterial;
    // }

    void Update()
    {
        
    }
}
