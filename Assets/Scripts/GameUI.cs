using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    Image progressBarImage;//toplanan çöplere göre ilerlemesini istediðim bar
    [SerializeField]
    Image starImage1;// bir yýldýz resmi
    [SerializeField]
    Image starImage2;// iki yýldýz resmi
    [SerializeField]
    Image starImage3;// üç yýldýz resmi
    [SerializeField]
    Image finishImage;// level geçilirse çýkacak resim
    [SerializeField]
    Image gameOverImage;// oyun biterse çýkacak resim
    public static GameUI Current;//eriþim gerekir ise diðer scriptler tarafýndan rahatlýkla public olan deðiþkenlere eriþmek amacýyla
    private float _finishGarbage;//toplanan çöp sayýsý
    private float _totalGarbage;//toplam çöp sayýsý
    private int _earnTotalStar;//levelden elde edilen toplam yýldýz
    private float _starStatus;//toplanan çöpler ile toplam çöpün yüzdelik oranýnýn hesaplanmasý için kullanýlacak deðiþken
    private bool _isPlayAgain;//tekrar oynama veya yeni levelin yüklenme durumu
    void Start()
    {
        _isPlayAgain = false;// varsayýlan deðeri
        _totalGarbage = GameObject.FindGameObjectsWithTag("garbage").Length;//sahne üzerinde yer alan çöplerin sayýsýný elde etmek amacýyla                                                               //  Debug.Log("Start toplam çöp sayýsý : " + _totalGarbage);
        _finishGarbage = 0;//toplanan çöp sayýsýný tutan deðiþken
        Current = this;
        
    }

    void Update()
    {
        progressBar();   //bar'ýn fill deðerininin deðiþtirilmesi
        checkPlayAgain();//tekrar oynama veya yeni level yükleme durumunun kontrol eden metot
    }
   private void progressBar()//çöp toplandýkça progress bar'ýn ilemesinin saðlanmasý
    {
        progressBarImage.fillAmount = (_finishGarbage / _totalGarbage);
        setStars();//toplanan çöplere göre yýldýz gelmesi 
    }
    public void upFinishGarbage()//çöp toplandýkça çöp nesneleri tarafýndan bu metot'un çaðrýlmasý
    {
        _finishGarbage++; 
    }
    private void setStars()//progress bar'ýn ilerleme durumuna göre oyuncunun kazanacaðý yýldýzlarýn belirlenmesi
    {
   _starStatus= (_finishGarbage / _totalGarbage) *100f;
        if (_starStatus > 40f && _starStatus<60f)
        {
            starImage1.gameObject.SetActive(true);
        }
       else if(_starStatus>60 && _starStatus < 80)
        {
            starImage2.gameObject.SetActive(true);
        }
        else if(_starStatus>80)
        {
            starImage3.gameObject.SetActive(true);
        }

    }
    public void gameOver()//oyun bitti ise
    {
        gameOverImage.gameObject.SetActive(true);
        gameOverImage.gameObject.GetComponent<Animator>().Play("gameOverAnim");
        Invoke("setTruePlayAgain", 0.3f);

    }
    public void checkGameStatus()//karakter kaza yapmadan durur ise yýldýzlara bakýp levelin geçilip geçilmediðine karar verilmesi
    {
        if (_starStatus > 40)
        {
            gameWin();
        }
        else
        {
            gameOver();
        }
    }
    public void gameWin()//oyun kazanýldý ise
    {
        LevelManager.Current.levelUp();
        finishImage.gameObject.SetActive(true);
        finishImage.gameObject.GetComponent<Animator>().Play("succesFinishAnim");
        Invoke("setTruePlayAgain", 1f);
    }
    private void checkPlayAgain()//oyun sonu ekranýnda ekrana dokunulduðunda istenilen levelin yüklenmesi
    {
        if(_isPlayAgain && Input.GetMouseButtonDown(0))
        {
            LevelManager.Current.loadLevel(LevelManager.Current.levelNum);
        }
    }
   private void setTruePlayAgain()//oyun sonu ekranýnda ekrana dokunulmasýnýn saðlanmasý
    {
        _isPlayAgain = true;
       
    }
    
}
