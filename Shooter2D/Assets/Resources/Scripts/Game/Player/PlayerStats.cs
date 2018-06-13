[System.Serializable]
public static class PlayerStats {

    static float _TotalLife = 5;
    static float _HealBox = 1;
    static float _Shooter = 1;
    static float _Helper = 0;
    static float _Misile_Power = 1;
    static float _Coins = 0;

    static public float TotalLife
    {
        get
        {
            return _TotalLife;
        }
        set
        {
            _TotalLife = value;
        }
    }

    static public float HealBox
    {
        get
        {
            return _HealBox;
        }
        set
        {
            _HealBox = value;
        }
    }

    static public float Shooter
    {
        get
        {
            return _Shooter;
        }
        set
        {
            _Shooter = value;
        }
    }

    static public float Helper
    {
        get
        {
            return _Helper;
        }
        set
        {
            _Helper = value;
        }
    }

    static public float Misile_Power
    {
        get
        {
            return _Misile_Power;
        }
        set
        {
            _Misile_Power = value;
        }
    }

    static public float Coins
    {
        get
        {
            return _Coins;
        }
        set
        {
            _Coins = value;
        }
    }
}
