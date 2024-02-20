using TMPro;
using UnityEngine;


public class SlotManager : MonoBehaviour
{
    private GameObject quantityPanel;
    private TextMeshProUGUI quantityText;
    private GameObject itemDisplayPanel;
    public Sprite defaultSprite;

    private void Start()
    {
        itemDisplayPanel = transform.GetChild(0).gameObject;
        quantityPanel = transform.GetChild(1).gameObject;
        quantityPanel.SetActive(false);
        itemDisplayPanel.SetActive(false);
    }

    private void Awake()
    {
        itemDisplayPanel = transform.GetChild(0).gameObject;
        quantityPanel = transform.GetChild(1).gameObject;
        quantityText = quantityPanel.GetComponentInChildren<TextMeshProUGUI>();

    }

    public void ResetDisplay()
    {
        itemDisplayPanel.GetComponent<UnityEngine.UI.Image>().sprite = null;
        ChangeItemDisplayQuantity(0);
        itemDisplayPanel.SetActive(false);
    }

    public void DisplayItem(GeneralItem item)
    {

        itemDisplayPanel.SetActive(true);
        itemDisplayPanel.GetComponent<UnityEngine.UI.Image>().sprite = item.gameObject.GetComponent<SpriteRenderer>().sprite;

    }

    public void ChangeItemDisplayQuantity(int quantity)
    {
        if (quantity > 1)
        {
            quantityPanel.SetActive(true);
        }

        if (quantity <= 1)
        {
            quantityPanel.SetActive(false);
        }

        quantityText.text = quantity.ToString();
    }
}