using UnityEngine;

public class MyGameEvents {
    public Color WeaponColor(int color)
    {
        switch (color)
        {
            case 1:
                return new Color32(30, 255, 0, 255);
            case 2:
                return new Color32(0, 255, 255, 255);
            case 3:
                return new Color32(125, 0, 255, 255);
            case 4:
                return new Color32(255, 128, 0, 255);
            case 5:
                return new Color32(255, 0, 0, 255);
            case 99:
                return new Color32(255, 100, 255, 255);
            default:
                return Color.white;
        }
    }
}
