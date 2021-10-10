using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* ��plerin kendi etraf�nda d�nmeleri ve s�p�rge ��plere yak�n ise ��plerin s�p�rgeye do�ru ilerlemelerini sa�layan script
 */
public class Garbage : MonoBehaviour
{
   private Transform _robotTransform;//ana karakterin transform componenet'i
    public float rotationSpeed = 400f;//��plerin kendi etraf�nda d�nme h�z�
    private void Awake()
    {
        _robotTransform = GameObject.Find("Robot").GetComponent<Transform>();//robotun transform component'ine eri�ilmesi
    }


  
    void Update()
    {
        float distance = Vector3.Distance(transform.position,_robotTransform.position);//player ile aras�ndaki mesafe
        

            if (distance <=Player.Current.vacuumDistance)//��pler vakum �ekme mesafesinde ise robot'a do�ru ilerlemelerinin sa�lanmas�
            {
                transform.position = Vector3.MoveTowards(transform.position, _robotTransform.position, Player.Current.vacuumSpeed * Time.deltaTime);
            Player.Current.vacuumParticleOn();//ilerleme olursa vacum efektinin oynat�lmas�

            }

        transform.Rotate(new Vector3(0, 0, Time.deltaTime * rotationSpeed));//��plerin z ekseni �zerinde kendi etraflar�nda d�nmesi
        
    }
    private void OnTriggerEnter(Collider other)//robota temas edilirse
    {
        if (other.gameObject.name == "Robot")
        {
            GameUI.Current.upFinishGarbage();//toplanan ��p say�s�n�n artt�r�lmas�
            gameObject.SetActive(false);// ilgili ��p nesnesinin durumunun pasif hale getirilmesi
        }
    }
}
