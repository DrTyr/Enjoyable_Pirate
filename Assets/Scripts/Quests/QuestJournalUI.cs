using UnityEngine;
using TMPro;
using System.Collections.Generic;


public class QuestJournalUI : MonoBehaviour
{

    //[Header("Quest journal canvas to display the content")]
    //[SerializeField] private GameObject questJournalUI;
    //[SerializeField] private GameObject questJounalButton;

    [Header("Quest journal UI slots")]
    [SerializeField] private QuestUIPanelManager[] questUIPanelManager;

    private bool JournalUIIsActive;

    void Start()
    {
        gameObject.SetActive(false);
        JournalUIIsActive = gameObject.activeSelf;
    }

    public void DisplayJournalUI()
    {
        gameObject.SetActive(!JournalUIIsActive);
        JournalUIIsActive = gameObject.activeSelf;

        List<Quest> questsInProgress = QuestManager.GetInstance().GetAllQuestsInProgress();

        //! Display all InProgress quests
        int index = 0;
        foreach (var questPanel in questUIPanelManager)
        {
            if (index < questsInProgress.Count)
            {
                questPanel.gameObject.SetActive(true);
                questPanel.DisplayQuestUIInfo(questsInProgress[index]);
                index++;
            }
        }

        //! Go through the remining quest panels not in use and hide them
        for (int i = index; i < questUIPanelManager.Length; i++)
        {
            questUIPanelManager[i].gameObject.SetActive(false);
        }

    }
}