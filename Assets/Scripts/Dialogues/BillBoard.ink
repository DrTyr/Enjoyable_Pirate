INCLUDE ExternalFunctions.ink

VAR questInProgress = false

{
    - GetQuestStatus(2) == "NotStarted" :
    Bonjour, voici la liste des quêtes disponible :
        + [Lire]
            -> QUEST_START
        + [Partir]
            -> END
    - GetQuestStatus(2) == "Completed" :
         -> QUEST_CONTINUE
}


=== QUEST_START ===

Bonjour, aventurier ! Je suis Gérard, le gardien du village. Nous avons un problème : trois de nos sacs ont disparu mystérieusement. Pourriez-vous nous aider à les retrouver ?

    + [Oui]
    #changeQuestStatus:2=inProgress 
    //~ questInProgress = true
    - Ah, merci, aventurier ! Je suis sûr que vous allez bien nous aider. Revenez me voir dès que vous avez trouvé les sacs.
    -> END

    + [Non]
    - Ah, dommage. Si vous changez d'avis, je serai là.
    -> END

=== QUEST_CONTINUE ===
//~ questStatus = GetQuestStatus(2)
                                                                                     
//Le status est actuellement {GetQuestStatus(2)}

{
    - GetQuestStatus(2) == "Completed":
        Ah, vous avez trouvé les sacs ! Merci beaucoup, aventurier. Vous êtes vraiment notre héros.
        //-> END

    - GetQuestStatus(2) == "InProgress":
        Ah, vous êtes toujours à la recherche des sacs ? Pas de soucis, prenez votre temps. Nous avons confiance en vous.
        //-> END
    - else:
        ça a buggué quelque part
        //-> END
}
-> END

