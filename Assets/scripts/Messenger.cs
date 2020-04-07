using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Messenger : MonoBehaviour
{

    public static GameObject msgPanel;
    
   /* [SerializeField]
    public static GameObject msgPanel;*/
    Text message; // ссылка на текст
    static Coroutine RunMessage; // ссылка на запущенную корутину


    
    private void Start()
    {
        msgPanel = GameObject.Find("Message panel");
        message = GetComponent<Text>();
        WriteMessage("Сначала найдите сумку"); 
    }

    public void WriteMessage(string text) // метод для запуска корутины с выводом сообщения
    {
        if (RunMessage != null) StopCoroutine(RunMessage);
        this.message.text = ""; // очистка строки
        RunMessage = StartCoroutine(Message(text));
        // запуск корутины с выводом нового сообщения
        //msgPanel.gameObject.SetActive(true);

    }

    IEnumerator Message(string message) // корутина для вывода сообщений
    {
        msgPanel.GetComponent<Image>().enabled = true;

        this.message.text = message; 
        yield return new WaitForSeconds(4f); 
        this.message.text = "";
        msgPanel.GetComponent<Image>().enabled = false;

        // msgPanel.gameObject.SetActive(false);

    }




    void Update()
    {
        
    }
}
