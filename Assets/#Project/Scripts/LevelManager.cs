using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject cardPrefab;
    public float columns;
    public float lines;
    public float padding;
    public Material[] materials;
    public float timeBeforeUnreveal = 0.5f;
    private List<Material> potentialMaterials;

    private List<CardBehaviour> cards = new List<CardBehaviour>();
    private List<CardBehaviour> cardsRevealed = new List<CardBehaviour>();
    private List<CardBehaviour> cardsMatched = new List<CardBehaviour>();


    void Start()
    {
        if (lines * columns % 2 != 0) {
            Debug.LogError("The level needs to have an even number of cards", gameObject); //modulo = reste d'une division entière //on précise le gameObject responsable de l'erreur
            return;
        }

        potentialMaterials = new List<Material>();

        for (int i=0; i<materials.Length; i++){
            potentialMaterials.Add(materials[i]);
            potentialMaterials.Add(materials[i]);
        }

        for (float y = 0; y < padding * columns; y += padding){
            for (float x = 0; x < padding * lines; x += padding){
                Vector3 position = new Vector3(x, y, 0f);
                Quaternion rotation = new Quaternion(270, 0, 0, 270);
                CreateCard(position, rotation);
        }
        }
    }

    IEnumerator UnrevealCards(){
        yield return new WaitForSeconds(timeBeforeUnreveal);
        for (int i = 0; i < cardsRevealed.Count ; i++){
            cardsRevealed[i].UnrevealCard();
        }
        cardsRevealed.Clear();
    }

    public void CardRevealed(CardBehaviour card){
        if (cardsRevealed.Contains(card)) return;
        if (cardsMatched.Contains(card)) return;
        if (cardsRevealed.Count >= 2) return;

        card.RevealCard();
        cardsRevealed.Add(card);

        if(cardsRevealed.Count >= 2){
            if (cardsRevealed[0].hiddenMaterial == cardsRevealed[1].hiddenMaterial){
                Debug.Log("It's a match!");
                cardsMatched.Add(cardsRevealed[0]);
                cardsMatched.Add(cardsRevealed[1]);
                card.MatchCard();
                cardsRevealed.Clear();
            }
            else{
                StartCoroutine(UnrevealCards());
            }
        }
    }

    private void CreateCard(Vector3 position, Quaternion rotation){
        GameObject cardGO = Instantiate(cardPrefab, position, rotation); //Instantiate retourne un objet qu'on stock dans la variable cardGO //si on passe la position, on doit aussi passer la rotation (par défaut : Quaternion.identity)
        CardBehaviour card = cardGO.GetComponent<CardBehaviour>();
        cards.Add(card); //le manager se fait connaître du card
        card.manager = this; //"je suis ton manager"

        int index = Random.Range(0, potentialMaterials.Count);
        // card.hiddenMaterial = materials[index];
        card.hiddenMaterial = potentialMaterials[index];
        potentialMaterials.RemoveAt(index); //ce qui retire celui sélectionné ?
        // ou potentialMaterials.Remove(potentialMaterials[index]); // ce qui retire la première occurence
        
    }

    void Update()
    {
        
    }
}
