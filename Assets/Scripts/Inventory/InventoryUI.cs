using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [HideInInspector] public List<SlotManager> slots;
    [HideInInspector] public List<Button> buttons;


    void Start()
    {
        GetSlotsManagerForEachSlots();
        GetButtonsForEachSlot();
    }

    private void GetSlotsManagerForEachSlots()
    {

        foreach (Transform child in transform)
        {

            // Vérifiez si le composant existe
            if (child.TryGetComponent<SlotManager>(out var slotManager))
            {
                // Ajoutez le composant à la liste des slots
                slots.Add(slotManager);
            }
        }
    }

    private void GetButtonsForEachSlot()
    {

        foreach (Transform child in transform)
        {

            // Vérifiez si le composant existe
            if (child.TryGetComponent<Button>(out var button))
            {
                // Ajoutez le composant à la liste des slots
                buttons.Add(button);
            }
        }
    }




}
