using UnityEngine;

public class Slot
{
    public string SName { get; set; }
    public Vector2 SVector2 { get; set; }
    public Rigidbody2D SOccupier { get; set; }

    public Slot(string sName, Vector2 sVector2)
    {
        SName = sName;
        SVector2 = sVector2;
    }
}
