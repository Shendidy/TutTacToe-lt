using System;

public static class ArrayExtender
{
    public static bool ContainsOnly(this String[] mainArray, int howMany, String[] arrayToTest)
    {
        int counter = 0;

        foreach (String item in arrayToTest)
        {
            counter += Array.Exists(mainArray, element => element == item) ? 1 : 0;
        }
        return counter == howMany ? true : false;
    }
}
