using System;
using System.Reflection;
using UnityEngine;

public class Grab : MonoBehaviour
{
    [SerializeField]
    private Transform grabPoint; // Point où l'objet sera déplacé lorsqu'il est saisi

    [SerializeField]
    private Transform rayPoint; // Point de départ du rayon pour détecter les objets

    [SerializeField]
    private float rayDistance; // Distance maximale du rayon

    private GameObject grabbedObject = null; // Référence à l'objet actuellement saisi
    private int LayerIndex; // Index de la couche des objets interactifs

    Vector3 objectPositionBeforeGrab; // Position de l'objet avant d'être saisi

    void Start()
    {
        // Obtenir l'index de la couche des objets interactifs
        LayerIndex = LayerMask.NameToLayer("InteractiveObjects");
    }

    void Update()
    {
        // Lancer un rayon pour détecter les objets devant le joueur
        RaycastHit2D[] hitInfos = Physics2D.RaycastAll(rayPoint.position, transform.right, rayDistance);

        foreach (RaycastHit2D hitInfo in hitInfos)
        {
            if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == LayerIndex)
            {
                // Saisir l'objet si le bouton de saisie est pressé et qu'aucun objet n'est déjà saisi
                if (GetComponent<PlayerController>().grab.action.WasPressedThisFrame() && grabbedObject == null)
                {
                    grabbedObject = hitInfo.collider.gameObject;
                    grabbedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic; // Changer le type de corps à Kinematic
                    grabbedObject.GetComponent<Rigidbody2D>().collisionDetectionMode = CollisionDetectionMode2D.Continuous; // Mode de détection de collision continu
                    objectPositionBeforeGrab = grabbedObject.transform.position; // Enregistrer la position avant saisie
                    grabbedObject.transform.position = grabPoint.position; // Déplacer l'objet au point de saisie
                    grabbedObject.transform.SetParent(transform); // Définir le joueur comme parent de l'objet
                    TestReactionToBeeingGrab(objectPositionBeforeGrab); // Tester la réaction de l'objet à la saisie
                    break;
                }
            }
        }
        // Relâcher l'objet devant le personnage si le bouton de saisie est relâché et qu'un objet est saisi
        if (GetComponent<PlayerController>().grab.action.WasReleasedThisFrame() && grabbedObject != null)
        {
            grabbedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; // Changer le type de corps à Dynamic
            grabbedObject.transform.SetParent(null); // Retirer le parent de l'objet
            grabbedObject.transform.position = rayPoint.position + transform.right; // Déplacer l'objet devant le personnage
            grabbedObject = null; // Réinitialiser la référence à l'objet saisi
        }
        /*        // Relâcher l'objet si le bouton de saisie est relâché et qu'un objet est saisi
               if (GetComponent<PlayerController>().grab.action.WasReleasedThisFrame() && grabbedObject != null)
               {
                   grabbedObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static; // Changer le type de corps à Static
                   grabbedObject.transform.SetParent(null); // Retirer le parent de l'objet
                   grabbedObject = null; // Réinitialiser la référence à l'objet saisi
               } */
    }

    public void TestReactionToBeeingGrab(Vector3 objectPositionBeforeGrab)
    {
        // Vérifier si l'objet saisi est un rocher interactif et générer des espèces sous celui-ci
        InteractiveBeachRocks rock = grabbedObject.GetComponent<InteractiveBeachRocks>();
        if (rock != null)
        {
            rock.GenerateSpeciesUnderIt(objectPositionBeforeGrab);
        }
    }
}