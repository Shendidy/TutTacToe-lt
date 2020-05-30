using System;

[Serializable]
public class GameData
{
    public int _keys { get; set; }
    public DateTime _date { get; set; }

    public GameData(int keys, DateTime date)
    {
        _keys = keys;
        _date = date;
    }
}