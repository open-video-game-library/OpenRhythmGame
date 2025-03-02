using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum eRank
{
    S,
    A,
    B,
    C,
    D
}

public class PointData
{
    public int point { get; private set; }
    public float rate { get; private set; }

    public int perfect { get; private set; }
    public int good { get; private set; }
    public int bad { get; private set; }
    public int miss { get; private set; }

    public int fastCount { get; private set; }
    public int slowCount { get; private set; }

    public int maxCombo { get; private set; }

    public eRank rank
    {
        get
        {
            if (rate >= 90) return eRank.S;
            else if (rate >= 80) return eRank.A;
            else if (rate >= 70) return eRank.B;
            else if (rate >= 60) return eRank.C;
            else return eRank.D;
        }
    }

    public PointData(int point, float rate, int perfect, int good, int bad, int miss, int fastCount, int slowCount, int maxCombo)
    {
        this.point = point;
        this.rate = rate;
        this.perfect = perfect;
        this.good = good;
        this.bad = bad;
        this.miss = miss;
        this.fastCount = fastCount;
        this.slowCount = slowCount;
        this.maxCombo = maxCombo;
    }
}