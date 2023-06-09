using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ItemValue
{
    public int Level;
    public string Item;
    public string Item_1;
    public int Count;
    public int Count_1;

    public int itemCount=1;
}

[Serializable]
public class ItemValueList
{
    public ItemValue[] ItemData;
    public ItemValue[] ItemPrimiumData;
}


public class PrimiumTicketSystem : Singleton<PrimiumTicketSystem>
{
    public static int MAXLEVEL= 30;
    private int getGameClearStarCount;
    //0,30 세트
    public Dictionary<int, ItemValue> ItemValue = new Dictionary<int, ItemValue>();
 

    public int GETGAMECLEARSTARCOUNT
    {
        get => getGameClearStarCount;
        set { getGameClearStarCount = value; }
    }

    public int LEVEL
    {
        get => PlayerData.GetInstance.PrimiumTicketLevel;
        set { PlayerData.GetInstance.PrimiumTicketLevel = value; }
    }

    public int RECEIVELEVEL
    {
        get => PlayerData.GetInstance.PrimiumTicketReceiveLevel;
        set { PlayerData.GetInstance.PrimiumTicketReceiveLevel = value; }
    }

    public int PRIMIUMTICKETSTAR
    {
        get => PlayerData.GetInstance.PrimiumTicketStar;
        set { PlayerData.GetInstance.PrimiumTicketStar = value; }
    }

    public bool ISBUYPRIMIUMTICKET
    {
        get => PlayerData.GetInstance.IsBuyPrimiumTicket;
        set { PlayerData.GetInstance.IsBuyPrimiumTicket = value; }
    }


    public void Init()
    {
        if(PlayerData.GetInstance != null)
        {
            if (PlayerData.GetInstance.PrimiumTicketLevel.Equals(0)) PlayerData.GetInstance.PrimiumTicketLevel = 1;
            if (PlayerData.GetInstance.PrimiumTicketReceiveLevel.Equals(0)) PlayerData.GetInstance.PrimiumTicketReceiveLevel = 1;
        }

        //여기서 ItemValue 세팅
        var items = ResourceLoader<ItemValueList>.LoadResource("PrimiumTicketData");

        //무료아이템 저장
        for (int i = 0; i < items.ItemData.Length; i++)
        {
            var level = items.ItemData[i].Level;


            var LevelItemvalue = new ItemValue();
            LevelItemvalue.Level = level;
            LevelItemvalue.Item = items.ItemData[i].Item;

            if (items.ItemData[i].Item_1 != null)
            {
                LevelItemvalue.Item_1 = items.ItemData[i].Item_1;
                items.ItemData[i].itemCount++;
            }
            
            LevelItemvalue.Count = items.ItemData[i].Count;
            LevelItemvalue.Count_1 = items.ItemData[i].Count_1;
            LevelItemvalue.itemCount = items.ItemData[i].itemCount;


            if (!ItemValue.ContainsKey(i)) ItemValue.Add(i, LevelItemvalue);
        }

        // 프리미엄 아이템 저장
        for (int i = 0; i < items.ItemPrimiumData.Length; i++)
        {
            var level = items.ItemPrimiumData[i].Level;


            var LevelItemvalue = new ItemValue();
            LevelItemvalue.Level = level;
            LevelItemvalue.Item = items.ItemPrimiumData[i].Item;

            if (items.ItemPrimiumData[i].Item_1 != null)
            {
                LevelItemvalue.Item_1 = items.ItemPrimiumData[i].Item_1;
                items.ItemPrimiumData[i].itemCount++;
            }

            LevelItemvalue.Count = items.ItemPrimiumData[i].Count;
            LevelItemvalue.Count_1 = items.ItemPrimiumData[i].Count_1;
            LevelItemvalue.itemCount = items.ItemPrimiumData[i].itemCount;

            if (!ItemValue.ContainsKey(i + MAXLEVEL)) ItemValue.Add(i + MAXLEVEL, LevelItemvalue);
        }
    }
}
