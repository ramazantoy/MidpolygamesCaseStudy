using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(LineRenderer))]//oto component eklenmesi amacýyla
public class DrawingPath : MonoBehaviour
{
    public static DrawingPath Current;//diðer script'lerde bu script'in özelliklerine eriþmek amacýyla
    private LineRenderer _lineRenderer;//LineRenderer component'ine erþisen
    public float timeForNextRay;//yol çizme hýzý
    public List<Vector3> Waypoints;//yollarýn her bir parçasýnýn  yer aldýðý liste
    float timer = 0;//yol çizme zamanýný hesaplamak amacýyla kullanýlan deðiþken
   int wayIndex;//yol sayýsý
    private bool _isMoved;//harekete baþlandý mý


    void Start()
    {
        _isMoved = false;//oyun bittikten sonra tekrar yol çizilmesini engellemek amacýyla
        Current = this;//Static deðiþken diðer scriptler tarafýndan bu script'e gerekirse eriþim saðlama amacýyla
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.enabled = false;//ilk baþta çizgi oluþturmamak amacýyla
        wayIndex = 0;// yol indeks'i
    }


    void Update()
    {
        timer += Time.deltaTime;//geçen süre
        if (Input.GetMouseButtonDown(0) && !_isMoved)//ekrana dokunma var ise
        {
            _lineRenderer.enabled = true;//linerenderer'in kullanýlmaya baþlanmasý
            _lineRenderer.positionCount = 0;//pozisyonun sýfýrlanmasý
            Player.Current.clearAgent();//var ise diðer deðerlerin sýfýrlanmasý
        }
        if (Input.GetMouseButton(0) && timer > timeForNextRay && !_isMoved)//istenilen saniyede bir ray çizdirme
        {
            //mouse konumunun raycast ile yakalanýp ilgili konumu waypoint listesine atma ve linerender ile çizgi oluþturma iþlemi
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
        if (Input.GetMouseButtonUp(0))//ekrana dokunma kesildi ise karakterin hareketinin durmasý ve tekar dokunulursa çizgi yapýlmasýnýn engellenmesi
        {
            Player.Current.isMove = true;

            _isMoved = true;
        }
    }
}
