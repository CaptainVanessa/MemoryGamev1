using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public float columns;
    public float lines;
    public float padding;
    public float timer = 0.5f;
    public GameObject restartButton;
    private Material _cardMaterial;
    public Material[] materials;
    public GameObject cardPrefab;
    private List<Material> _potentialMaterials;
    private List<CardBehaviour> _cards = new List<CardBehaviour>();
    private List<CardBehaviour> _cardsRevealed = new List<CardBehaviour>();
    private List<CardBehaviour> _cardsMatched = new List<CardBehaviour>();


    void Start()
    {
        restartButton.SetActive(false);
        if (lines * columns % 2 != 0) {
            Debug.LogError("The level needs to have an even number of cards", gameObject);
            return;
        }

        _potentialMaterials = new List<Material>();

        for (int i=0; i<materials.Length; i++){
            _potentialMaterials.Add(materials[i]);
            _potentialMaterials.Add(materials[i]);
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
        yield return new WaitForSeconds(timer);
        for (int i = 0; i < _cardsRevealed.Count ; i++){
            _cardsRevealed[i].UnrevealCard();
        }
        _cardsRevealed.Clear();
    }

    public void DestroyCards(){
        for (int i = 0; i < _cardsRevealed.Count ; i++){
            _cardsRevealed[i].MatchCard();
        }
        _cardsRevealed.Clear();
    }

    public void CardRevealed(CardBehaviour card){
        if (_cardsRevealed.Contains(card)) return;
        if (_cardsMatched.Contains(card)) return;
        if (_cardsRevealed.Count >= 2) return;

        card.RevealCard();
        _cardsRevealed.Add(card);

        if(_cardsRevealed.Count >= 2){
            if (_cardsRevealed[0]._cardMaterial == _cardsRevealed[1]._cardMaterial){
                Debug.Log("It's a match!");
                _cardsMatched.Add(_cardsRevealed[0]);
                _cardsMatched.Add(_cardsRevealed[1]);
                DestroyCards();
                if(_cardsMatched.Count == _cards.Count){
                    restartButton.SetActive(true);
                }       
            }
            else{
                Debug.Log("It's not a match...");
                StartCoroutine(UnrevealCards());
            }
        }
    }

    public void Restart(){
        Start();
    }

    private void CreateCard(Vector3 position, Quaternion rotation){
        GameObject cardGO = Instantiate(cardPrefab, position, rotation);
        CardBehaviour card = cardGO.GetComponent<CardBehaviour>();
        _cards.Add(card);
        card.manager = this;

        int index = Random.Range(0, _potentialMaterials.Count);
        card._cardMaterial = _potentialMaterials[index];
        _potentialMaterials.RemoveAt(index);
    }
}
