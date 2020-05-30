public class KeyService
{
    public static void setTotalKeys(int keys = 10)
    {
        GameManager.keysTotal = keys;
    }

    public static int getTotalKeys()
    {
        return GameManager.keysTotal;
    }
}
