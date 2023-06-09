using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PrimiumTicketEntity : MonoBehaviour
{
    [SerializeField] Text Leveltxt;
    [SerializeField] List<Sprite> Item_IMG = new List<Sprite>();

    [SerializeField] GameObject Lockobj;
    [SerializeField] GameObject[] receiveItemCheck;
    [Header("橇府固决 粮")]
    [SerializeField] GameObject primium_obj;
    [SerializeField] List<Image> primium_img = new List<Image>();
    [SerializeField] List<Text> primium_txt = new List<Text>();
    [SerializeField] GameObject primium_LockObj;
    [SerializeField] GameObject primiumGetBtn;

    [Header("公丰 粮")]
    [SerializeField] GameObject Free_obj;
    [SerializeField] List<Image> Free_img = new List<Image>();
    [SerializeField] List<Text> Free_txt = new List<Text>();
    [SerializeField] GameObject freeGetBtn;



    public void SetTicketEntity(int _level, bool _isBuyTicket, bool _Isthislevel, bool _isReceiveLevel)
    {
        if (_isBuyTicket) primium_LockObj.SetActive(false);

        if (_Isthislevel) Lockobj.SetActive(_Isthislevel);
         
        if(_isReceiveLevel)
        {
            if (_isBuyTicket) primiumGetBtn.SetActive(_isBuyTicket);
            freeGetBtn.SetActive(true);
        }

        Leveltxt.text = (_level + 1).ToString();
        if(PrimiumTicketSystem.GetInstance != null)
        {
            //橇府固决率
            {
                int level = PrimiumTicketSystem.MAXLEVEL + _level;
                
                if(PrimiumTicketSystem.GetInstance.ItemValue[level].itemCount > 1)
                {
                    primium_obj.SetActive(true);
                    for (int i=0; i< PrimiumTicketSystem.GetInstance.ItemValue[level].itemCount; i++)
                    {
                        var item_name = PrimiumTicketSystem.GetInstance.ItemValue[level].Item;
                        switch (i)
                        {
                            case 0:
                                item_name = PrimiumTicketSystem.GetInstance.ItemValue[level].Item;
                                primium_txt[i].text = PrimiumTicketSystem.GetInstance.ItemValue[level].Count.ToString();
                                break;

                            case 1:
                                item_name = PrimiumTicketSystem.GetInstance.ItemValue[level].Item_1;
                                primium_txt[i].text = PrimiumTicketSystem.GetInstance.ItemValue[level].Count_1.ToString();
                                break;
                        }

                        switch (item_name)
                        {
                            case "None":
                                primium_img[i].sprite = Item_IMG[0];
                                break;

                            case "Hammer":
                                primium_img[i].sprite = Item_IMG[1];
                                break;

                            case "Bomb":
                                primium_img[i].sprite = Item_IMG[2];
                                break;

                            case "Color":
                                primium_img[i].sprite = Item_IMG[3];
                                break;

                            case "Hammer+Bomb":
                                primium_img[i].sprite = Item_IMG[4];
                                break;

                            case "Hammer+Color":
                                primium_img[i].sprite = Item_IMG[5];
                                break;

                            case "Bomb+Color":
                                primium_img[i].sprite = Item_IMG[6];
                                break;
                        }
                    }
                        
                }
                else
                {
                    var item_name = PrimiumTicketSystem.GetInstance.ItemValue[level].Item;
                    switch (item_name)
                    {
                        case "None":
                            primium_img[0].sprite = Item_IMG[0];
                            break;

                        case "Hammer":
                            primium_img[0].sprite = Item_IMG[1];
                            break;

                        case "Bomb":
                            primium_img[0].sprite = Item_IMG[2];
                            break;

                        case "Color":
                            primium_img[0].sprite = Item_IMG[3];
                            break;

                        case "Hammer+Bomb":
                            primium_img[0].sprite = Item_IMG[4];
                            break;

                        case "Hammer+Color":
                            primium_img[0].sprite = Item_IMG[5];
                            break;

                        case "Bomb+Color":
                            primium_img[0].sprite = Item_IMG[6];
                            break;



                    }

                    primium_txt[0].text = PrimiumTicketSystem.GetInstance.ItemValue[level].Count.ToString();
                }
                
            }


            //橇府率
            {
                if (PrimiumTicketSystem.GetInstance.ItemValue[_level].itemCount > 1)
                {
                    Free_obj.SetActive(true);
                    for (int i = 0; i < PrimiumTicketSystem.GetInstance.ItemValue[_level].itemCount; i++)
                    {
                        var item_name = PrimiumTicketSystem.GetInstance.ItemValue[_level].Item;
                        switch (i)
                        {
                            case 0:
                                item_name = PrimiumTicketSystem.GetInstance.ItemValue[_level].Item;
                                Free_txt[i].text = PrimiumTicketSystem.GetInstance.ItemValue[_level].Count.ToString();
                                break;

                            case 1:
                                item_name = PrimiumTicketSystem.GetInstance.ItemValue[_level].Item_1;
                                Free_txt[i].text = PrimiumTicketSystem.GetInstance.ItemValue[_level].Count_1.ToString();
                                break;
                        }

                        switch (item_name)
                        {
                            case "None":
                                Free_img[i].sprite = Item_IMG[0];
                                break;

                            case "Hammer":
                                Free_img[i].sprite = Item_IMG[1];
                                break;

                            case "Bomb":
                                Free_img[i].sprite = Item_IMG[2];
                                break;

                            case "Color":
                                Free_img[i].sprite = Item_IMG[3];
                                break;

                            case "Hammer+Bomb":
                                Free_img[i].sprite = Item_IMG[4];
                                break;

                            case "Hammer+Color":
                                Free_img[i].sprite = Item_IMG[5];
                                break;

                            case "Bomb+Color":
                                Free_img[i].sprite = Item_IMG[6];
                                break;
                        }
                    }

                }
                else
                {
                    var item_name = PrimiumTicketSystem.GetInstance.ItemValue[_level].Item;
                    switch (item_name)
                    {
                        case "None":
                            Free_img[0].sprite = Item_IMG[0];
                            break;

                        case "Hammer":
                            Free_img[0].sprite = Item_IMG[1];
                            break;

                        case "Bomb":
                            Free_img[0].sprite = Item_IMG[2];
                            break;

                        case "Color":
                            Free_img[0].sprite = Item_IMG[3];
                            break;

                        case "Hammer+Bomb":
                            Free_img[0].sprite = Item_IMG[4];
                            break;

                        case "Hammer+Color":
                            Free_img[0].sprite = Item_IMG[5];
                            break;

                        case "Bomb+Color":
                            Free_img[0].sprite = Item_IMG[6];
                            break;
                    }

                    Free_txt[0].text = PrimiumTicketSystem.GetInstance.ItemValue[_level].Count.ToString();
                }
            }


        }


    }

    public void ChangeEntity()
    {
        primiumGetBtn.SetActive(true);
        freeGetBtn.SetActive(true);

        receiveItemCheck[0].SetActive(true);
        receiveItemCheck[1].SetActive(true);
    }
}
