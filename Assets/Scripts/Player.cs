using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
   // private NavMeshAgent _navMeshAgent;//navmeshagent component deðiþkeni
   /*baþlangýçta istenilen konumlara navmeshAgent ile gitmek istedim ama hem hýzý hemde ilerleyiþ biçimi çizgi üzerinden sapmalar olduðu için
    * istenilen hedef noktaya vector3 movetowards metodu ile gitmeyi denedim.
  */
    private Vector3 _targetDestination;//gidilecek hedef konum
    private int _succesDestinationNumber;//baþarýlý olarak gidilen yer sayýsý
    public bool isMove;//hareket deðiþkeni
    public static Player Current; // bu script'in public olan deðiþkenlerine veya metotlarýna ihtiyaç halinde eriþebilmek amacýyla
    public float playerSpeed=10.0f;//karakterin noktalar arasýndaki gidiþ hýzýný ayarlamak amacýyla
    public float vacuumSpeed = 10f;//karakterin çöpleri çekme hýzý
    public float vacuumDistance = 0.7f;// karakterin çöpleri çekmeye baþladýðý mesafe
    [SerializeField]
  ParticleSystem vacuumParticle;//süpürme iþlemi var ise oynatýlacak olan efekt
    [SerializeField]
    ParticleSystem explosionParticle;// süpürge kaza  yapar ise oynatýlacak olan efekt
    private void Start()
    {
       
       // Time.timeScale = 1;
       // _navMeshAgent = GetComponent<NavMeshAgent>();//navmeshagent componenet'ine eriþmek amacýyla
       // _navMeshAgent.speed = playerSpeed;
        isMove = false;//baþlangýçta hareket etmediði için
        _succesDestinationNumber = 0;//baþarýlý olarak ziyaret edilen waypoint sayýsý
        Current = this;//Player türündeki bu deðiþkenin program çalýþtýðýnda bu script'in sahip olduðu þeylere sahip olmasý
    }
    public void clearAgent()//yeni rota çizilirse diye ajanýn sýfýrlanmasý
    {
        isMove = false;//hareket durumunun varsayýlana döndürülmesi
        _succesDestinationNumber = 0;
        Current = this;

    }
   
    private void UpdatePathing()//yeni hedef noktanýn belirlenmesini saðlayan fonksiyon
    {
        if (ShouldSetDestination())//hedef nokta seçmelimiyim isimli fonksiyonun cevabýna göre yeni hedef ayarlanmasý
        {
           //ziyaret edilen noktalarýn sayýsýnýn arttýrýlmasý
            if (checkDist())//mesafe kontrolüne göre baþarýlý nokta sayýsýnýn arttýrýlmasý
            {
                _succesDestinationNumber++;
            }
            if (_succesDestinationNumber < DrawingPath.Current.Waypoints.Count)//tüm noktalar ziyaret edilmedi ise yeni nokta ayarlanmasý
            {
                // _navMeshAgent.SetDestination(DrawingPath.Current.Waypoints[_succesDestinationNumber]);
                transform.position = Vector3.MoveTowards(transform.position, DrawingPath.Current.Waypoints[_succesDestinationNumber], playerSpeed * Time.deltaTime); ;
            }
            else//tüm noktalar ziyaret edilmiþ karakter duruyor ise
            {
                isMove = false;
                GameUI.Current.checkGameStatus();//oyunun kazanýlýp kazanýlmadýðýnýn kontrol edilip ui'a yansýtýlmasý
            }
          
       
        }
    }
    private bool ShouldSetDestination()//harekete devam edilip edilmeyeceðine karar veren metot
    {
        if (_succesDestinationNumber>=DrawingPath.Current.Waypoints.Count)//_baþarýlý nokta sayýsý boyutu aþarsa
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
    private bool checkDist()//kalan mesafeye bakýlmasý amacýyla
    {
        float dist = Vector3.Distance(transform.position, DrawingPath.Current.Waypoints[_succesDestinationNumber]);//gidilecek nokta ile olan mesafenin ölçülmesi
        if (dist < 0.1)//noktaya yakýn ise true deðil isek false
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
            UpdatePathing();//yol kontrolü
        }
       
    }
    public void vacuumParticleOn()//süpürme efektinin etkinleþtirilmesi
    {
        //Debug.Log("süpürme particle aktif");
        vacuumParticle.Play();
        CancelInvoke("vacuumParticle"); //üst üste invoke çaðrýmýný önlemek amacýyla
        Invoke("vacuumParticleOff", 3f);// 3f saniye içinde efektin kapanmasý çekim var ise zaten efekt devam edicek.
    }
    private void vacuumParticleOff()// çekme efekt'ini kapamak amacýyla
    {
        vacuumParticle.Stop();
    }
    public void explosionParticleOn()
    {
        //Debug.Log("patlama efekt'i etkin ");
        vacuumParticle.Stop();
        vacuumParticle.gameObject.SetActive(false);//patladýktan sonra vakum efektinin oynamasýný durdurmak amacýyla
        isMove = false;//karakterin kaza olursa hereketini kesmesi amacýyla
        explosionParticle.gameObject.SetActive(true);//patlama efektinin aktif edilmesi
        explosionParticle.Play();//patlama efektinin oynatýlmasý
        Invoke("gameOver",0.7f);//göz kararý patlama efekt'i oynadýktan sonra oyunun bitmesi
    }
    public void gameOver()// istenmeyen nesnelerle temas durumunda oyunun sonlanmasý diðer scriptler tarafýndan kullanýlýyor
    {
        GameUI.Current.gameOver();
    }
}
