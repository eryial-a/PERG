using System.Collections.Generic;
using UnityEngine;

public class HandDisplay : MonoBehaviour
{
    // variables and such
    public HandManager handManager;
    public GameObject cardPrefab;
    public Transform handArea;
    // hand layout 
    public HandLayout handLayout;
    // scales cell size for cards
    public DynamicGridScaler scaler;
    // for selecting sprites from files
    public CardSpriteDatabase spriteDatabase;
    // refreshes hand
    public void RefreshHand()
    {
        // safegaurds
        if (handManager == null)
        {
            Debug.LogError("HandManager is NULL in HandDisplay");
            return;
        }

        if (handArea == null)
        {
            Debug.LogError("HandArea is NULL in HandDisplay");
            return;
        }

        if (cardPrefab == null)
        {
            Debug.LogError("CardPrefab is NULL in HandDisplay");
            return;
        }
        foreach (Transform child in handArea)
            Destroy(child.gameObject);
        // spawn cards list
        List<CardUI> spawnedCards = new List<CardUI>();
        // each card is ordered
        foreach (Card card in handManager.GetCurrentHand())
        {
            GameObject obj = Instantiate(cardPrefab, handArea);

            Sprite sprite = spriteDatabase.GetSprite(card);

            CardUI ui = obj.GetComponent<CardUI>();
            ui.Setup(card, handManager, sprite);
            // adds ui
            spawnedCards.Add(ui);
        }

        handLayout.Arrange(spawnedCards);
    }
    // used for discard pile, and possibly other features
    public void ShowCustomCards(List<Card> cards)
    {
        if (handArea == null || cardPrefab == null)
        {
            Debug.LogError("HandDisplay missing references!");
            return;
        }
        foreach (Transform child in handArea)
            Destroy(child.gameObject);
        // logs
        Debug.Log("Showing discard cards: " + cards.Count);
        // checks scaler
        if (scaler != null)
            scaler.UpdateGrid(cards.Count);
        // checks cards
        foreach (Card card in cards)
        {
            if (card == null) continue;

            GameObject obj = Instantiate(cardPrefab, handArea);
            // grabs sprites
            Sprite sprite = spriteDatabase.GetSprite(card);
            // grabs component
            CardUI ui = obj.GetComponent<CardUI>();

            if (ui != null)
                ui.Setup(card, handManager, sprite);
        }
    }
}