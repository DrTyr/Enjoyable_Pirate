INCLUDE ExternalFunctions.ink

//bois = WoodLogs

-> main

=== main ===

Il faut 5 bois pour consruire ce batiment, voulez vous lancer la construction ?
+ [OUI]
{
    - IsTheItemInInventory("WoodLogs", 5):
        Super ! La construction sera terminé dans quelques secondes
         #takeItem:WoodLogs=5
        ~ FulfillACondition(3, 0)
        -> END
    - else:
        Tu n'as pas assez de bois pour construire cette maison
        -> END
}
+ [NON]
-> END