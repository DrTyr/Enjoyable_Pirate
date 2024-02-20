using System.Collections;
using UnityEngine;

public class House : MonoBehaviour
{
    public GameObject buildingStage;
    public GameObject finalStage;
    public GameObject intermediateStage;
    private QuestManager questManager;

    private bool isBuild = false;

    // Start is called before the first frame update
    void Start()
    {
        finalStage.SetActive(false);
        intermediateStage.SetActive(false);
        buildingStage.SetActive(true);
        questManager = FindObjectOfType<QuestManager>();
    }

    void Update()
    {
        if (!isBuild) { StartBuilding(); }
    }

    private void StartBuilding()
    {

        if (questManager.GetQuestStatus(3) == "Completed")
        {
            StartCoroutine(ChangeBuildingState());
            //ChangeBuildingState();
        }

    }

    IEnumerator ChangeBuildingState()
    {
        isBuild = true;
        intermediateStage.SetActive(true);
        buildingStage.SetActive(false);
        yield return new WaitForSeconds(3f);
        finalStage.SetActive(true);
        intermediateStage.SetActive(false);

    }

}



