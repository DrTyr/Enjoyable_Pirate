using UnityEngine;
using TMPro;


public class QuestUIPanelManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questNameUI;
    [SerializeField] private TextMeshProUGUI questStatus;
    [SerializeField] private TextMeshProUGUI questTextUI;
    [SerializeField] private TextMeshProUGUI[] questConditionsUI;
    public void DisplayQuestUIInfo(Quest quest)
    {

        this.questNameUI.text = quest.NameUI;
        this.questTextUI.text = quest.Description;
        this.questStatus.text = quest.Status.ToString();

        int index = 0;
        foreach (var conditionUI in questConditionsUI)
        {
            if (index < quest.Conditions.Count)
            {
                conditionUI.text = quest.Conditions[index].ConditionName;

                if (quest.Conditions[index].IsFulfilled)
                {
                    conditionUI.color = new Color32(15, 98, 230, 255);
                }

                index++;
            }
        }

        //! Go through the remining panels not in use and hide them
        for (int i = index; i < questConditionsUI.Length; i++)
        {
            questConditionsUI[i].gameObject.SetActive(false);
        }


    }
}
