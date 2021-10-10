using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField]
    Image progressBarImage;//toplanan ��plere g�re ilerlemesini istedi�im bar
    [SerializeField]
    Image starImage1;// bir y�ld�z resmi
    [SerializeField]
    Image starImage2;// iki y�ld�z resmi
    [SerializeField]
    Image starImage3;// �� y�ld�z resmi
    [SerializeField]
    Image finishImage;// level ge�ilirse ��kacak resim
    [SerializeField]
    Image gameOverImage;// oyun biterse ��kacak resim
    public static GameUI Current;//eri�im gerekir ise di�er scriptler taraf�ndan rahatl�kla public olan de�i�kenlere eri�mek amac�yla
    private float _finishGarbage;//toplanan ��p say�s�
    private float _totalGarbage;//toplam ��p say�s�
    private int _earnTotalStar;//levelden elde edilen toplam y�ld�z
    private float _starStatus;//toplanan ��pler ile toplam ��p�n y�zdelik oran�n�n hesaplanmas� i�in kullan�lacak de�i�ken
    private bool _isPlayAgain;//tekrar oynama veya yeni levelin y�klenme durumu
    void Start()
    {
        _isPlayAgain = false;// varsay�lan de�eri
        _totalGarbage = GameObject.FindGameObjectsWithTag("garbage").Length;//sahne �zerinde yer alan ��plerin say�s�n� elde etmek amac�yla                                                               //  Debug.Log("Start toplam ��p say�s� : " + _totalGarbage);
        _finishGarbage = 0;//toplanan ��p say�s�n� tutan de�i�ken
        Current = this;
        
    }

    void Update()
    {
        progressBar();   //bar'�n fill de�erininin de�i�tirilmesi
        checkPlayAgain();//tekrar oynama veya yeni level y�kleme durumunun kontrol eden metot
    }
   private void progressBar()//��p topland�k�a progress bar'�n ilemesinin sa�lanmas�
    {
        progressBarImage.fillAmount = (_finishGarbage / _totalGarbage);
        setStars();//toplanan ��plere g�re y�ld�z gelmesi 
    }
    public void upFinishGarbage()//��p topland�k�a ��p nesneleri taraf�ndan bu metot'un �a�r�lmas�
    {
        _finishGarbage++; 
    }
    private void setStars()//progress bar'�n ilerleme durumuna g�re oyuncunun kazanaca�� y�ld�zlar�n belirlenmesi
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
    public void checkGameStatus()//karakter kaza yapmadan durur ise y�ld�zlara bak�p levelin ge�ilip ge�ilmedi�ine karar verilmesi
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
    public void gameWin()//oyun kazan�ld� ise
    {
        LevelManager.Current.levelUp();
        finishImage.gameObject.SetActive(true);
        finishImage.gameObject.GetComponent<Animator>().Play("succesFinishAnim");
        Invoke("setTruePlayAgain", 1f);
    }
    private void checkPlayAgain()//oyun sonu ekran�nda ekrana dokunuldu�unda istenilen levelin y�klenmesi
    {
        if(_isPlayAgain && Input.GetMouseButtonDown(0))
        {
            LevelManager.Current.loadLevel(LevelManager.Current.levelNum);
        }
    }
   private void setTruePlayAgain()//oyun sonu ekran�nda ekrana dokunulmas�n�n sa�lanmas�
    {
        _isPlayAgain = true;
       
    }
    
}
