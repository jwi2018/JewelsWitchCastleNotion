using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrimiumTicketPopup : PopupSetting
{
    bool IsBuyTicket;
    private int StarCount;
    private int MaxStarCount;
    public GameObject gobDummy;
    public Transform trSlotParent;

    [SerializeField] Text starCounttxt;
    [SerializeField] Text Leveltxt;
    [SerializeField] Image GuageIMG;
    [SerializeField] ScrollRect contentPos;

    private List<PrimiumTicketEntity> ticketEntityList = new List<PrimiumTicketEntity>();


    private void Start()
    {
        if(PrimiumTicketSystem.GetInstance != null) IsBuyTicket = PrimiumTicketSystem.GetInstance.ISBUYPRIMIUMTICKET;
        gobDummy.SetActive(false);
        IsBuyTicket = true;
        
        OnPopupSetting();
    }

    public override void OnPopupSetting()
    {
        //레벨, 별획득 상태
        Leveltxt.text = PrimiumTicketSystem.GetInstance.LEVEL.ToString();
        StarCount = PrimiumTicketSystem.GetInstance.PRIMIUMTICKETSTAR;
        MaxStarCount = PrimiumTicketSystem.GetInstance.LEVEL;
        starCounttxt.text = $"{StarCount} / {MaxStarCount}";
        
        float amount = (float)StarCount / MaxStarCount;
        GuageIMG.fillAmount = amount;

        int nowLevel = 0;
        for (int i = 0; i < PrimiumTicketSystem.GetInstance.ItemValue.Count / 2; i++)
        {
            var loadedDailyMission = GameObject.Instantiate(gobDummy, trSlotParent);
            loadedDailyMission.SetActive(true);
            loadedDailyMission.name = $"Level_{i + 1}";

            var PTEntity = loadedDailyMission.GetComponent<PrimiumTicketEntity>();
            bool isthisLevel = false;
            bool isReceiveLevel = false;

            if (PrimiumTicketSystem.GetInstance.LEVEL.Equals(i + 1))
            {
                nowLevel = i + 1;
                isthisLevel = true;
            }

            if (PrimiumTicketSystem.GetInstance.RECEIVELEVEL.Equals(i + 1)) isReceiveLevel = true;



            PTEntity.SetTicketEntity(i, IsBuyTicket, isthisLevel, isReceiveLevel);

            ticketEntityList.Add(PTEntity);
        }

        //현재 레벨 위치 잡기
        var levelpercent = (float)nowLevel / 25;
        contentPos.verticalNormalizedPosition = 1f - levelpercent;
    }

    public override void OffPopupSetting()
    {
        this.GetComponent<Animator>().SetTrigger("Off");
    }

    public void GetItem(bool _Isprimium)
    {
       
        if(_Isprimium)
        {
            Debug.Log("Primium");
            var level = PrimiumTicketSystem.GetInstance.RECEIVELEVEL + PrimiumTicketSystem.MAXLEVEL;
            GetLevelValue(level);



        }
        else
        {
            Debug.Log("Free");
            GetLevelValue(PrimiumTicketSystem.GetInstance.RECEIVELEVEL);
        }
        //PrimiumTicketSystem.GetInstance.ItemValue[PrimiumTicketSystem.GetInstance.LEVEL].Item;

    }

    public void GetLevelValue(int _level)
    {
        if (PrimiumTicketSystem.GetInstance.ItemValue[_level].itemCount > 1)
        {
            for (int i = 0; i < PrimiumTicketSystem.GetInstance.ItemValue[_level].itemCount; i++)
            {
                string item_name = PrimiumTicketSystem.GetInstance.ItemValue[_level].Item;
                int item_count = 0;

                switch (i)
                {
                    case 0:
                        item_name = PrimiumTicketSystem.GetInstance.ItemValue[_level].Item;
                        item_count = PrimiumTicketSystem.GetInstance.ItemValue[_level].Count;
                        break;

                    case 1:
                        item_name = PrimiumTicketSystem.GetInstance.ItemValue[_level].Item_1;
                        item_count = PrimiumTicketSystem.GetInstance.ItemValue[_level].Count_1;
                        break;
                }

                switch (item_name)
                {
                    case "None":
                        PlayerData.GetInstance.Gold += item_count;
                        break;

                    case "Hammer":
                        PlayerData.GetInstance.ItemHammer += item_count;
                        break;

                    case "Bomb":
                        PlayerData.GetInstance.ItemBomb += item_count;
                        break;

                    case "Color":
                        PlayerData.GetInstance.ItemColor += item_count;
                        break;

                    case "Hammer+Bomb":
                        PlayerData.GetInstance.ItemHammer += item_count;
                        PlayerData.GetInstance.ItemBomb += item_count;
                        break;

                    case "Hammer+Color":
                        PlayerData.GetInstance.ItemHammer += item_count;
                        PlayerData.GetInstance.ItemColor += item_count;
                        break;

                    case "Bomb+Color":
                        PlayerData.GetInstance.ItemBomb += item_count;
                        PlayerData.GetInstance.ItemColor += item_count;
                        break;
                }
            }
        }
        else
        {
            string item_name = PrimiumTicketSystem.GetInstance.ItemValue[_level].Item;
            int item_count = PrimiumTicketSystem.GetInstance.ItemValue[_level].Count;

            switch (item_name)
            {
                case "None":
                    PlayerData.GetInstance.Gold += item_count;
                    break;

                case "Hammer":
                    PlayerData.GetInstance.ItemHammer += item_count;
                    break;

                case "Bomb":
                    PlayerData.GetInstance.ItemBomb += item_count;
                    break;

                case "Color":
                    PlayerData.GetInstance.ItemColor += item_count;
                    break;

                case "Hammer+Bomb":
                    PlayerData.GetInstance.ItemHammer += item_count;
                    PlayerData.GetInstance.ItemBomb += item_count;
                    break;

                case "Hammer+Color":
                    PlayerData.GetInstance.ItemHammer += item_count;
                    PlayerData.GetInstance.ItemColor += item_count;
                    break;

                case "Bomb+Color":
                    PlayerData.GetInstance.ItemBomb += item_count;
                    PlayerData.GetInstance.ItemColor += item_count;
                    break;
            }
        }
        PrimiumTicketSystem.GetInstance.RECEIVELEVEL++;
    }
        
    
    public void BuyTicket()
    {
        //사면
        if (PrimiumTicketSystem.GetInstance != null) PrimiumTicketSystem.GetInstance.ISBUYPRIMIUMTICKET = true;
    }
}
