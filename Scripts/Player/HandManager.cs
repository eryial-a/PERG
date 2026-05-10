using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HandManager : MonoBehaviour
{
    // refrences
    public Deck deck;
    public BattleManager battleManager;
    public TurnManager turnManager;
    AudioManager audioManager;
    // grab audio manager reference
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    // players items and abilities
    public bool abilityUsed = false;
    public int maxHandSize = 9;
    // hand and selected card list
    public List<Card> currentHand = new List<Card>();
    public List<Card> selectedCards = new List<Card>();
    // hand display for ui
    public HandDisplay handDisplay;
    // deck display
    public DeckDisplay deckDisplay;
    // ability button
    public UnityEngine.UI.Button abilityButton;
    // potion text
    public TextMeshProUGUI potionText;
    // heal popupUI
    public HealPopupUI healPopup;
    // start
    void Start()
    {
        EnsureDeck();
        UpdateDeckUI();
        DrawToFull();
        UpdatePotionUI();
    }
    // deck exists
    void EnsureDeck()
    {
        if (deck == null)
        {
            deck = new Deck();
            deck.InitializeDeck();
            deck.Shuffle();
        }
    }
    // draws to full function (if player has less than max cards)
    public void DrawToFull()
    {
        EnsureDeck();
        Debug.Log("Deck is: " + deck);
        while (currentHand.Count < maxHandSize)
        {
            //play sound effect for drawing cards
            audioManager.PlaySFX(audioManager.cardDraw);
            currentHand.Add(deck.Draw());
        }
        SortHand(); // sorts hand before display
        UpdateDeckUI(); // updates deck
        Debug.Log("Hand refilled to " + currentHand.Count);
        handDisplay.RefreshHand();
    }
    // resets deck once enemy is defeated
    public void ResetDeckAndHand()
    {
        deck = new Deck();
        deck.InitializeDeck();
        deck.Shuffle();
        // clears current hand
        currentHand.Clear();
        selectedCards.Clear();
        // sets ability to true once again
        abilityUsed = false;
        Player.Instance.strengthBuffActive = false;
        // makes it interactable
        if (abilityButton != null)
        {
            abilityButton.interactable = true;
            // restore normal color
            var colors = abilityButton.colors;
            colors.normalColor = Color.white;
            abilityButton.colors = colors;
        }
        // draws
        DrawToFull();
    }
        // plays selected (hand evaluation)
    public void PlaySelected()
    {
        if (!turnManager.CanPlayerAct())
        {
            Debug.Log("No actions remaining.");
            return;
        }
        if (selectedCards.Count == 0)
        {
            Debug.Log("No cards selected to play.");
            return;
        }

        if (selectedCards.Count > 5)
        {
            Debug.LogWarning("You can only play up to 5 cards. Only the first 5 will be played.");
            selectedCards = selectedCards.GetRange(0, 5);
        }
        else if (selectedCards.Count < 1)
        {
            return;
        }
        // function
        battleManager.PlayHand(selectedCards);
        DiscardUsedCards(battleManager.lastUsedCards);
        DrawToFull();
        handDisplay.RefreshHand();
        turnManager.PlayerUsedAction();
    }
    // discards selected hand
    public void DiscardSelected()
    {   
        if (!turnManager.CanPlayerAct())
        {
            Debug.Log("No actions remaining.");
            return;
        }
        // no cards to discard
        if (selectedCards.Count == 0)
        {
            Debug.Log("No cards selected to discard.");
            return;
        }
        // cards to discard
        List<Card> toDiscard = new List<Card>(selectedCards);
        if (toDiscard.Count > 3)
        {
            toDiscard = toDiscard.GetRange(0, 3);
        }
        // removes from current hand
        foreach (Card c in toDiscard)
        {
            currentHand.Remove(c);
            deck.AddToDiscard(c);
            selectedCards.Remove(c);
        }
        // draw to full
        DrawToFull();
        handDisplay.RefreshHand();
        turnManager.PlayerUsedAction();

        Debug.Log("Discarded selected cards.");
    }
    // discard played cards
    void DiscardUsedCards(List<Card> usedCards)
    {
        foreach (Card c in usedCards)
        {
            currentHand.Remove(c);
            deck.AddToDiscard(c);
        }

        // remove only used cards from selection
        foreach (Card c in usedCards)
        {
            selectedCards.Remove(c);
        }
        // log issues
        foreach (Card c in usedCards)
        {
            bool removed = currentHand.Remove(c);
            Debug.Log("Tried removing: " + c + " Result: " + removed);
            if (removed)
                deck.AddToDiscard(c);
        }

        UpdateDeckUI();
    }
    // selects card
    public void SelectCard(Card card)
    {
        if (!currentHand.Contains(card))
            return;

        if (selectedCards.Contains(card))
            return;
        // prevents more than 5 cards selected
        if (selectedCards.Count >= 5)
        {
            Debug.Log("You can only select up to 5 cards.");
            return;
        }
        // adds selected cards
        selectedCards.Add(card);
        // card select sfx
        audioManager.PlaySFX(audioManager.cardSelect);
        Debug.Log("Selected: " + card);
    }
    // unselect card, choose not to play
    public void UnselectCard(Card card)
    {
        if (selectedCards.Contains(card))
        {
            selectedCards.Remove(card);
            // card unselect sfx
            audioManager.PlaySFX(audioManager.cardUnselect);
            Debug.Log("Unselected: " + card);
        }
    }
    // Sort hand (hand display)
    private void SortHand()
    {
        currentHand.Sort((a, b) => b.Rank.CompareTo(a.Rank));
    }
    // updates deck ui
    void UpdateDeckUI()
    {
        if (deckDisplay != null && deck != null)
        {
            deckDisplay.UpdateDeckCount(deck.DeckCount);
            deckDisplay.UpdateDiscardCount(deck.DiscardCount);
        }
    }
    // test
    public List<Card> GetCurrentHand() => new List<Card>(currentHand);
    public void RefillHand() => DrawToFull();
    // uses ability
    public void UseAbility()
    {
        if (abilityUsed)
        {
            Debug.Log("Ability already used this battle.");
            return;
        }
        // sets ability to false once used
        Player.Instance.strengthBuffActive = true;
        abilityUsed = true;
        if (abilityButton != null)
        {
            abilityButton.interactable = false; // disables clicking
            // Turns the button grey after use
            var colors = abilityButton.colors;
            colors.normalColor = Color.gray;
            colors.disabledColor = Color.gray;
            abilityButton.colors = colors;
        }
        Debug.Log("Ability activated: next attack is 2x strength");
    }
    public void UseHealthPotion()
    {
        Player player = Player.Instance;
        // player is missing null 
        if (player == null)
        {
            Debug.LogError("Player missing!");
            return;
        }
        // no potions means no heal
        if (player.healthPotions <= 0)
        {
            Debug.Log("No health potions left.");
            return;
        }
        // does play have any actions left
        if (!turnManager.CanPlayerAct())
        {
            Debug.Log("No actions remaining.");
            return;
        }
        // if player current health already at max do not use item
        if (player.currentHealth >= player.maxHealth)
        {
            Debug.Log("Health already full.");
            return;
        }
        // before value
        int before = player.currentHealth;
        // healing amoubt
        int healAmount = Mathf.RoundToInt(player.maxHealth * 0.4f);
        // heal and use potion
        player.Heal(healAmount);
        // after
        int after = player.currentHealth;
        int actaulHealed = after - before;
        player.healthPotions--;
        Player.Instance.potionsUsed++;
        turnManager.UpdateBoards();
        // log
        Debug.Log("Healed amount: " + actaulHealed);
        Debug.Log("Used potion. Remaining: " + player.healthPotions);
        UpdatePotionUI();
        // pups up heal ui
        if (healPopup != null)
        {
            healPopup.Show(actaulHealed);
        }
    }
    // updates potion count
    void UpdatePotionUI()
    {
        if (potionText != null)
        {
            potionText.text = "" + Player.Instance.healthPotions;
        }
    }
}