public class FriendsCondition
{
    public string FriendName;
    public int FriendQuantity;
    public string RequiredZone;
    public string LoadedObjectAdress;
    public float DetectionTimer; //! Time to detecte sourrounding friend, in seconds IRL (ex : 60f = every 60 secondes)
    //public bool RightZone;
    public string ItemRewardName;
    public bool CoRoutineInProgress;
    public string SupportName;

    public FriendsCondition(string name)
    {
        //! Conditions to Add a object to itemList
        FriendName = name;
        FriendQuantity = 300; // By default, the quantity is too high
        RequiredZone = "zone";
        //string requiredZone = "";
        //! Item added if conditions fullfilled
        //! Folder adress, root is Resources/...
        //! Exemple : "Items/" + "Carrot"
        LoadedObjectAdress = "";
        DetectionTimer = 600.0f; // By default, the time is too high
        ItemRewardName = "";
        CoRoutineInProgress = false;
        SupportName = "";
    }

}