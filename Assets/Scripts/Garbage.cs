using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* Çöplerin kendi etrafýnda dönmeleri ve süpürge çöplere yakýn ise çöplerin süpürgeye doðru ilerlemelerini saðlayan script
 */
public class Garbage : MonoBehaviour
{
   private Transform _robotTransform;//ana karakterin transform componenet'i
    public float rotationSpeed = 400f;//çöplerin kendi etrafýnda dönme hýzý
    private void Awake()
    {
        _robotTransform = GameObject.Find("Robot").GetComponent<Transform>();//robotun transform component'ine eriþilmesi
    }


  
    void Update()
    {
        float distance = Vector3.Distance(transform.position,_robotTransform.position);//player ile arasýndaki mesafe
        

            if (distance <=Player.Current.vacuumDistance)//çöpler vakum çekme mesafesinde ise robot'a doðru ilerlemelerinin saðlanmasý
            {
                transform.position = Vector3.MoveTowards(transform.position, _robotTransform.position, Player.Current.vacuumSpeed * Time.deltaTime);
            Player.Current.vacuumParticleOn();//ilerleme olursa vacum efektinin oynatýlmasý

            }

        transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotationSpeed));//çöplerin z ekseni üzerinde kendi etraflarýnda dönmesi
        
    }
    private void OnTriggerEnter(Collider other)//robota temas edilirse
    {
        if (other.gameObject.name == "Robot")
        {
            GameUI.Current.upFinishGarbage();//toplanan çöp sayýsýnýn arttýrýlmasý
            gameObject.SetActive(false);// ilgili çöp nesnesinin durumunun pasif hale getirilmesi
        }
    }
}
