using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
   // private NavMeshAgent _navMeshAgent;//navmeshagent component de�i�keni
   /*ba�lang��ta istenilen konumlara navmeshAgent ile gitmek istedim ama hem h�z� hemde ilerleyi� bi�imi �izgi �zerinden sapmalar oldu�u i�in
    * istenilen hedef noktaya vector3 movetowards metodu ile gitmeyi denedim.
  */
    private Vector3 _targetDestination;//gidilecek hedef konum
    private int _succesDestinationNumber;//ba�ar�l� olarak gidilen yer say�s�
    public bool isMove;//hareket de�i�keni
    public static Player Current; // bu script'in public olan de�i�kenlerine veya metotlar�na ihtiya� halinde eri�ebilmek amac�yla
    public float playerSpeed=10.0f;//karakterin noktalar aras�ndaki gidi� h�z�n� ayarlamak amac�yla
    public float vacuumSpeed = 10f;//karakterin ��pleri �ekme h�z�
    public float vacuumDistance = 0.7f;// karakterin ��pleri �ekmeye ba�lad��� mesafe
    [SerializeField]
  ParticleSystem vacuumParticle;//s�p�rme i�lemi var ise oynat�lacak olan efekt
    [SerializeField]
    ParticleSystem explosionParticle;// s�p�rge kaza  yapar ise oynat�lacak olan efekt
    private void Start()
    {
       
       // Time.timeScale = 1;
       // _navMeshAgent = GetComponent<NavMeshAgent>();//navmeshagent componenet'ine eri�mek amac�yla
       // _navMeshAgent.speed = playerSpeed;
        isMove = false;//ba�lang��ta hareket etmedi�i i�in
        _succesDestinationNumber = 0;//ba�ar�l� olarak ziyaret edilen waypoint say�s�
        Current = this;//Player t�r�ndeki bu de�i�kenin program �al��t���nda bu script'in sahip oldu�u �eylere sahip olmas�
    }
    public void clearAgent()//yeni rota �izilirse diye ajan�n s�f�rlanmas�
    {
        isMove = false;//hareket durumunun varsay�lana d�nd�r�lmesi
        _succesDestinationNumber = 0;
        Current = this;

    }
   
    private void UpdatePathing()//yeni hedef noktan�n belirlenmesini sa�layan fonksiyon
    {
        if (ShouldSetDestination())//hedef nokta se�melimiyim isimli fonksiyonun cevab�na g�re yeni hedef ayarlanmas�
        {
           //ziyaret edilen noktalar�n say�s�n�n artt�r�lmas�
            if (checkDist())//mesafe kontrol�ne g�re ba�ar�l� nokta say�s�n�n artt�r�lmas�
            {
                _succesDestinationNumber++;
            }
            if (_succesDestinationNumber < DrawingPath.Current.Waypoints.Count)//t�m noktalar ziyaret edilmedi ise yeni nokta ayarlanmas�
            {
                // _navMeshAgent.SetDestination(DrawingPath.Current.Waypoints[_succesDestinationNumber]);
                transform.position = Vector3.MoveTowards(transform.position, DrawingPath.Current.Waypoints[_succesDestinationNumber], playerSpeed * Time.deltaTime); ;
            }
            else//t�m noktalar ziyaret edilmi� karakter duruyor ise
            {
                isMove = false;
                GameUI.Current.checkGameStatus();//oyunun kazan�l�p kazan�lmad���n�n kontrol edilip ui'a yans�t�lmas�
            }
          
       
        }
    }
    private bool ShouldSetDestination()//harekete devam edilip edilmeyece�ine karar veren metot
    {
        if (_succesDestinationNumber>=DrawingPath.Current.Waypoints.Count)//_ba�ar�l� nokta say�s� boyutu a�arsa
        {
            return false;
        }/*
        else if(_navMeshAgent.hasPath == false || _navMeshAgent.remainingDistance < 0.5f)
        {
            return true;
        }*/
        else 
        {
            return true;
        }
    }
    private bool checkDist()//kalan mesafeye bak�lmas� amac�yla
    {
        float dist = Vector3.Distance(transform.position, DrawingPath.Current.Waypoints[_succesDestinationNumber]);//gidilecek nokta ile olan mesafenin �l��lmesi
        if (dist < 0.1)//noktaya yak�n ise true de�il isek false
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void Update()
    {
        if (isMove)//hareket durumu true ise
        {
            UpdatePathing();//yol kontrol�
        }
       
    }
    public void vacuumParticleOn()//s�p�rme efektinin etkinle�tirilmesi
    {
        //Debug.Log("s�p�rme particle aktif");
        vacuumParticle.Play();
        CancelInvoke("vacuumParticle"); //�st �ste invoke �a�r�m�n� �nlemek amac�yla
        Invoke("vacuumParticleOff", 3f);// 3f saniye i�inde efektin kapanmas� �ekim var ise zaten efekt devam edicek.
    }
    private void vacuumParticleOff()// �ekme efekt'ini kapamak amac�yla
    {
        vacuumParticle.Stop();
    }
    public void explosionParticleOn()
    {
        //Debug.Log("patlama efekt'i etkin ");
        vacuumParticle.Stop();
        vacuumParticle.gameObject.SetActive(false);//patlad�ktan sonra vakum efektinin oynamas�n� durdurmak amac�yla
        isMove = false;//karakterin kaza olursa hereketini kesmesi amac�yla
        explosionParticle.gameObject.SetActive(true);//patlama efektinin aktif edilmesi
        explosionParticle.Play();//patlama efektinin oynat�lmas�
        Invoke("gameOver",0.7f);//g�z karar� patlama efekt'i oynad�ktan sonra oyunun bitmesi
    }
    public void gameOver()// istenmeyen nesnelerle temas durumunda oyunun sonlanmas� di�er scriptler taraf�ndan kullan�l�yor
    {
        GameUI.Current.gameOver();
    }
}
