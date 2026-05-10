using UnityEngine;
using TMPro;
// deck display
public class DeckDisplay : MonoBehaviour
{
    public TextMeshProUGUI deckCountText;
    public TextMeshProUGUI discardCountText;
    // size change
    public void UpdateDeckCount(int count)
    {
        deckCountText.text = count.ToString();
    }
    // discard pile
    public void UpdateDiscardCount(int count)
    {
        discardCountText.text = count.ToString();
    }
}