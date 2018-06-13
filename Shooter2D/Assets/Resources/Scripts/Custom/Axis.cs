using System.Collections.Generic;
using UnityEngine;

public class Axis
{
    public string name;
    public KeyCode negative;
    public KeyCode positive;

    public Axis(string _name, KeyCode _negative, KeyCode _positive)
    {
        name = _name;
        negative = _negative;
        positive = _positive;
    }

}
