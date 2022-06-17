using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SC_Rank : MonoBehaviour
{
    public enum ERank
    {
        Bronze,
        Silver,
        Gold,

        Count
    }

    static private Dictionary<ERank, Dictionary<int, int>> rowToWin = new Dictionary<ERank, Dictionary<int, int>>()
    {
        {
            ERank.Bronze,
            new Dictionary<int,int>()
            {
                {1,5 },
                {2,4 },
                {3,1 }
            }
        },

        {
            ERank.Silver,
            new Dictionary<int,int>()
            {
                {1,5 },
                {2,4 },
                {3,3 }
            }
        },

        {
            ERank.Gold,
            new Dictionary<int,int>()
            {
                {1,5 },
                {2,4 },
                {3,3 }
            }
        }
    };

    static public string GetRankString(ERank pRank)
    {
        switch (pRank)
        {
            case ERank.Bronze:
                return "Bronze";
            case ERank.Silver:
                return "Silver";
            case ERank.Gold:
                return "Gold";
            default:
                return "Error";
        }
    }

    static public int RowToRankUp(ERank pRank, int pRankLevel)
    {
        return rowToWin[pRank][pRankLevel];
    }

    static public string GetLevelName(ERank pRank,int pRankLevel,int pRow)
    {
        return "Level_" + GetRankString(pRank) + "_" + pRankLevel + "_" + pRow;
    }
}
