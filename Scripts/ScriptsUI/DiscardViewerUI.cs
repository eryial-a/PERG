using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiscardViewerUI : MonoBehaviour
{
    public HandManager handManager;
    public DiscardDisplay discardDisplay;
    public GameObject discardPopupPanel;
    // opens
    public void OpenDiscard()
    {
        discardPopupPanel.SetActive(true);

        List<Card> discard = handManager.deck.GetDiscardSnapshot();
        discardDisplay.Show(discard);
    }
    // closes
    public void CloseDiscard()
    {
        discardPopupPanel.SetActive(false);
    }
}