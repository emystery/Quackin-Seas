using UnityEngine;
using UnityEngine.UI;

public class TreasureElement : MonoBehaviour
{
    public Image oldSketch;
    public Image treasureSketch;
    public int index;

    private void Start()
    {
        treasureSketch = GetComponent<Image>();
    }

    public void LogTreasure(Treasure treasure)
    {
        treasureSketch.sprite = treasure.image;
        oldSketch.enabled = false;
        treasureSketch.enabled = true;
    }
}
