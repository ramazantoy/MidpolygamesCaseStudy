using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
  public static LevelManager Current;//ihtiya� halinde di�er scriptlerden public olan metotlara ve de�i�kenlere eri�ilmesi amac�yla
   public int levelNum;//level numaras�n� tutan de�i�ken
   
    void Start()
    {
        levelNum = PlayerPrefs.GetInt("levelNum", 1);//kay�tl� levelin al�nmas�
        Current = this;
        int checkScaneID = SceneManager.GetActiveScene().buildIndex;//sahne id'sinin kontrol�
        checkScaneID++;
        if (checkScaneID != levelNum)//yanl�� sahne y�kleniyor ise sahnenin de�i�mesi amac�yla
        {
            PlayerPrefs.SetInt("levelNum", checkScaneID);
            loadLevel(checkScaneID);
           // Debug.Log("wrong level");
        }
       // Debug.Log(checkScaneID);

   
        
    }
    void Update()
    {
        
    }
    public void loadLevel(int levelId)//istenilen id'deki levelin y�klenmesi
    {
        SceneManager.LoadScene("Level" + levelId);
    }
    public void levelUp()//level de�erinin artt�r�lmas�
    {
        PlayerPrefs.SetInt("levelNum", levelNum + 1);
        levelNum++;
    }

}
