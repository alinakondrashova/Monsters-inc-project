using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Baby : MonoBehaviour
{
    AudioSource instruction;
    [SerializeField]
    GameObject player;
    bool hello = false;
    float weight = 0;

    Animator animbot; // ссылка на аниматор бота

    enum states
    {
        waiting, // ожидает
        dialog
    }
    states state = states.waiting; // изначальное состояние ожидания


    void Start()
    {
        animbot = GetComponent<Animator>(); // берем компонент аниматора
        StartCoroutine(Wait()); // запускаем корутину ожидания
        player = FindObjectOfType<PlayerAnimation>().gameObject;
        instruction = GetComponent<AudioSource>();
        objectToRotate = animbot.gameObject;
    }


    void FixedUpdate()
    {
        switch (state)
        {
            case (states.waiting):
                {
                    if (PlayerNear()) PrepareToDialog();
                    break;
                }

            case states.dialog:
                {
                    if (!PlayerNear()) StartCoroutine(Wait());
                    break;
                }
        }
        bool PlayerNear()
        {
            if (Vector3.Distance(gameObject.transform.position, player.transform.position) < 2) return true;
            else return false;
        }

        void PrepareToDialog()
        {
            animbot.SetBool("walk", false); // останавливаем анимацию ходьбы
            state = states.dialog; // устанавливаем состояние подхода к объекту в который попали лучом         
            Dialog_Bot();

        }

    }

    IEnumerator Wait() // корутина ожидания
    {
        animbot.SetBool("walk", false); // останавливаем анимацию ходьбы
        state = states.waiting; // указываем, что бот перешел в режим ожидания
        yield return new WaitForSeconds(3f); // ждем 10 секунд

    }

    void Dialog_Bot()
    {
        StartRotation();
        if (!hello)
        {
            instruction.Play();
            animbot.SetBool("hello", true);
            hello = true;
        }
        MainManager.game.WinGame();

    }

    void OnAnimatorIK()
    {
   
        if (state == states.dialog)
        {
           
        }
        else if (weight > 0)
        {
            
        }

    }

    public GameObject objectToRotate;
    private bool rotating;

    private IEnumerator Rotate(Vector3 angles, float duration)
    {
        rotating = true;
        Quaternion startRotation = objectToRotate.transform.rotation;
        Quaternion endRotation = Quaternion.Euler(angles) * startRotation;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            objectToRotate.transform.rotation = Quaternion.Lerp(startRotation, endRotation, t / duration);
            yield return null;
        }
        objectToRotate.transform.rotation = endRotation;
        rotating = false;
    }

    public void StartRotation()
    {
        float rot = player.transform.eulerAngles.y + 180;
        float rot_angle = 0;
        float k = gameObject.transform.eulerAngles.y;
        rot_angle = rot - k;
        if (!rotating) StartCoroutine(Rotate(new Vector3(0, rot_angle, 0), 0f));

    }

}
