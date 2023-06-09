using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrimiumTicket : MonoBehaviour
{
    [SerializeField] GameObject primiumticketObj;
    [SerializeField] Text starCounttxt;
    [SerializeField] Text Leveltxt;
    [SerializeField] Image GuageIMG;
    [SerializeField] Animator GetStarAnim;
    

    private int StarCount;
    private int MaxStarCount;

    bool PlayingAnim = false;
    void Start()
    {
        CheckStarCount();
    }

    public void click()
    {
        //테스트
        PrimiumTicketSystem.GetInstance.GETGAMECLEARSTARCOUNT = 6;
        StartCoroutine(StarAnim());
    }

    void CheckStarCount()
    {
        if(PrimiumTicketSystem.GetInstance != null)
        {
            Leveltxt.text = PrimiumTicketSystem.GetInstance.LEVEL.ToString();
            StarCount = PrimiumTicketSystem.GetInstance.PRIMIUMTICKETSTAR;
            MaxStarCount = PrimiumTicketSystem.GetInstance.LEVEL;
            starCounttxt.text = $"{StarCount} / {MaxStarCount}";
            float amount = (float)StarCount / MaxStarCount;
            GuageIMG.fillAmount = amount;

            //애니메이션 실행
            switch (PrimiumTicketSystem.GetInstance.GETGAMECLEARSTARCOUNT)
            {
                case 0:
                    break;

                case 1:
                    GetStarAnim.SetTrigger("GetStarAnim_1");
                    break;

                case 2:
                    GetStarAnim.SetTrigger("GetStarAnim_2");
                    break;

                default:
                    GetStarAnim.SetTrigger("GetStarAnim_3");
                    break;

            }
        }
    }



    #region 애니메이션 함수
    public void FirstStar()
    {
        if(PrimiumTicketSystem.GetInstance.GETGAMECLEARSTARCOUNT > 0)
        {
            //30레벨이 만렙
            CalculateStar();
        }
    }

    public void SecondStar()
    {
        if (PrimiumTicketSystem.GetInstance.GETGAMECLEARSTARCOUNT > 0)
        {
            CalculateStar();
        }
    }

    public void AnimEnd()
    {
        if (PrimiumTicketSystem.GetInstance.GETGAMECLEARSTARCOUNT > 0)
        {
            CalculateStar();



            if (PrimiumTicketSystem.GetInstance.GETGAMECLEARSTARCOUNT > 0)
            {
                StartCoroutine(StarAnim());
            }
        }
    }

    public void MoreStarAnim()
    {
        if (PrimiumTicketSystem.GetInstance.GETGAMECLEARSTARCOUNT > 0)
        {
            CalculateStar();

            PlayingAnim = true;
        }
    }

    public void OnePlayAnim()
    {
        GetStarAnim.SetBool("GetStarAnim_4", PlayingAnim);
    }

    #endregion

    public void CalculateStar()
    {
        if (!PrimiumTicketSystem.GetInstance.PRIMIUMTICKETSTAR.Equals(PrimiumTicketSystem.MAXLEVEL)) PrimiumTicketSystem.GetInstance.PRIMIUMTICKETSTAR++;

        if (PrimiumTicketSystem.GetInstance.PRIMIUMTICKETSTAR.Equals(PrimiumTicketSystem.GetInstance.LEVEL))
        {
            if (!PrimiumTicketSystem.GetInstance.LEVEL.Equals(PrimiumTicketSystem.MAXLEVEL))
            {
                PrimiumTicketSystem.GetInstance.LEVEL++;
                PrimiumTicketSystem.GetInstance.PRIMIUMTICKETSTAR = 0;
            }
        }


        Leveltxt.text = PrimiumTicketSystem.GetInstance.LEVEL.ToString();
        StarCount = PrimiumTicketSystem.GetInstance.PRIMIUMTICKETSTAR;
        MaxStarCount = PrimiumTicketSystem.GetInstance.LEVEL;
        starCounttxt.text = $"{StarCount} / {MaxStarCount}";
        float amount = (float)StarCount / MaxStarCount;
        GuageIMG.fillAmount = amount;

        PrimiumTicketSystem.GetInstance.GETGAMECLEARSTARCOUNT--;
    }


 

    IEnumerator StarAnim()
    {
        PlayingAnim = true;

        while (PrimiumTicketSystem.GetInstance.GETGAMECLEARSTARCOUNT > 0)
        {
            yield return null;

            if (PrimiumTicketSystem.GetInstance.GETGAMECLEARSTARCOUNT == 0) break;
            if (PlayingAnim) GetStarAnim.SetBool("GetStarAnim_4", PlayingAnim);
            PlayingAnim = false;
        }
    }
    
}
