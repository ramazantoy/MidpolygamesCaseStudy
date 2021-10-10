using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
  public static LevelManager Current;//ihtiyaç halinde diðer scriptlerden public olan metotlara ve deðiþkenlere eriþilmesi amacýyla
   public int levelNum;//level numarasýný tutan deðiþken
   
    void Start()
    {
        levelNum = PlayerPrefs.GetInt("levelNum", 1);//kayýtlý levelin alýnmasý
        Current = this;
        int checkScaneID = SceneManager.GetActiveScene().buildIndex;//sahne id'sinin kontrolü
        checkScaneID++;
        if (checkScaneID != levelNum)//yanlýþ sahne yükleniyor ise sahnenin deðiþmesi amacýyla
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
    public void loadLevel(int levelId)//istenilen id'deki levelin yüklenmesi
    {
        SceneManager.LoadScene("Level" + levelId);
    }
    public void levelUp()//level deðerinin arttýrýlmasý
    {
        PlayerPrefs.SetInt("levelNum", levelNum + 1);
        levelNum++;
    }

}
