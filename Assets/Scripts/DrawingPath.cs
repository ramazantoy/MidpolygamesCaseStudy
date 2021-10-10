using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]//oto component eklenmesi amac�yla
public class DrawingPath : MonoBehaviour
{
    public static DrawingPath Current;//di�er script'lerde bu script'in �zelliklerine eri�mek amac�yla
    private LineRenderer _lineRenderer;//LineRenderer component'ine er�isen
    public float timeForNextRay;//yol �izme h�z�
    public List<Vector3> Waypoints;//yollar�n her bir par�as�n�n  yer ald��� liste
    float timer = 0;//yol �izme zaman�n� hesaplamak amac�yla kullan�lan de�i�ken
   int wayIndex;//yol say�s�
    private bool _isMoved;//harekete ba�land� m�


    void Start()
    {
        _isMoved = false;//oyun bittikten sonra tekrar yol �izilmesini engellemek amac�yla
        Current = this;//Static de�i�ken di�er scriptler taraf�ndan bu script'e gerekirse eri�im sa�lama amac�yla
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;//ilk ba�ta �izgi olu�turmamak amac�yla
        wayIndex = 0;// yol indeks'i
    }


    void Update()
    {
        timer += Time.deltaTime;//ge�en s�re
        if (Input.GetMouseButtonDown(0) && !_isMoved)//ekrana dokunma var ise
        {
            _lineRenderer.enabled = true;//linerenderer'in kullan�lmaya ba�lanmas�
            _lineRenderer.positionCount = 0;//pozisyonun s�f�rlanmas�
            Player.Current.clearAgent();//var ise di�er de�erlerin s�f�rlanmas�
        }
        if (Input.GetMouseButton(0) && timer > timeForNextRay && !_isMoved)//istenilen saniyede bir ray �izdirme
        {
            //mouse konumunun raycast ile yakalan�p ilgili konumu waypoint listesine atma ve linerender ile �izgi olu�turma i�lemi
            Vector3 WorlfromMousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,100));
            Vector3 direction = WorlfromMousePos - Camera.main.transform.position;
            RaycastHit hit;

            if(Physics.Raycast(Camera.main.transform.position,direction,out hit, 100f))
            {
                Vector3 newWayPoint;
                newWayPoint= new Vector3(hit.point.x, transform.position.y, hit.point.z);
         
                Waypoints.Add(newWayPoint);
                _lineRenderer.positionCount = wayIndex + 1;
                _lineRenderer.SetPosition(wayIndex, newWayPoint);
                timer = 0;
                wayIndex += 1;
            }
        }
        if (Input.GetMouseButtonUp(0))//ekrana dokunma kesildi ise karakterin hareketinin durmas� ve tekar dokunulursa �izgi yap�lmas�n�n engellenmesi
        {
            Player.Current.isMove = true;

            _isMoved = true;
        }
    }
}
