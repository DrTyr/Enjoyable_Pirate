Salut mon ami ! #speaker:green #portrait:dr_green_neutral #layout:left #changeQuestStatus:1=inProgress 
-> TestGiveAndTakeObjects

=== TestGiveAndTakeObjects ===
Tu veux un objet ?
+ [Oui]
    Voilà #portrait:dr_green_happy #giveItem:XpCollectible=5
+ [Non]
    Ok, dommage #portrait:dr_green_sad
    
- Il est super cet objet, tu me le donne ? #speaker:yellow #portrait:ms_yellow_neutral #layout:right
+[Oui]
    Merci #portrait:ms_yellow_happy #takeItem:XpCollectible=1
+[Non]
    Ho, d'accord, dommage #portrait:ms_yellow_sad

- Tu veux autre chose ? #speaker:green #portrait:dr_green_neutral #layout:left
+[Yep]
    Voilà #portrait:dr_green_happy #giveItem:HealthCollectible=4
+[Nope]

-A bientôt

-> END

=== TestUseObjectFromDialogue ===
Voila un object #quantity:1 #giveItem:XpCollectible

Je vais l'utiliser pour toi #quantity:1 #useItem:XpCollectible

-> END