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
        //! Name of the object that has to be around the support
        FriendName = name;
        //! Quantity of this object
        FriendQuantity = 300; // By default, the quantity is too high
        //! Where the suport need to be to generate
        RequiredZone = "zone";
        //! Item added if conditions fullfilled
        //! Prefab adress, root is Resources/...
        //! Exemple : "Items/" + "Carrot"
        LoadedObjectAdress = "";
        DetectionTimer = 600.0f; // By default, the time is too high
        ItemRewardName = "";
        CoRoutineInProgress = false;
        //! On what support this object can appear
        SupportName = "";
    }

}