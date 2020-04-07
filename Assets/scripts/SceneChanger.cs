using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
 
    private GameObject Rules;

    private void Start()
    {
        Rules = GameObject.Find("Rules");
        Rules.SetActive(false);
    }

    public void OpenRules() {
        Rules.SetActive(true); 
    }

    public void CloseRules()
    {
        Rules.SetActive(false); 
    }

    public void OpenNewScene() // метод для смены сцены
    {
        int index = SceneManager.GetActiveScene().buildIndex; // берем индекс запущенной сцены
        if (index == 0) index = 1; // меняем индекс с 0 на 1 или с 1 на 0
        else index = 0;
        StartCoroutine(AsyncLoad(index)); // запускаем асинхронную загрузку сцены
    }

  
    IEnumerator AsyncLoad(int index)
    {
        //yield return new WaitForSeconds(4f);
        AsyncOperation ready = null;
        ready = SceneManager.LoadSceneAsync(index);
        while (!ready.isDone) // пока сцена не загрузилась
        {
            yield return null; // ждём следующий кадр
        }
    }


}
