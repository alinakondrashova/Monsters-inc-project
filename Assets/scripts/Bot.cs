using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Bot : MonoBehaviour
{
    AudioSource instruction;
    [SerializeField]
    GameObject player;
    bool hello = false;
    float weight = 0;
    NavMeshAgent botagent; // ссылка на агент навигации
    Animator animbot; // ссылка на аниматор бота
    [SerializeField]
    GameObject[] points; // массив точек для переходов
    //перечисление состояний бота
    enum states
    {
        waiting, // ожидает
        going, // идёт
        dialog
    }
    states state = states.waiting; // изначальное состояние ожидания
  

    void Start()
    {
        animbot = GetComponent<Animator>(); // берем компонент аниматора
        botagent = GetComponent<NavMeshAgent>(); // берем компонент агента
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
            case states.going:
                {
                    if (PlayerNear()) PrepareToDialog();
                    else if ((Vector3.Distance(transform.position, botagent.destination)) < 1) // если дистанция до пункта назначения меньше заданного расстояния (т.е. бот дошел до выданной ему точки)
                    {
                        StartCoroutine(Wait()); // вызываем корутину ожидания
                    }
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
            botagent.SetDestination(transform.position); // обнуляем точку, чтобы бот никуда не шёл
            animbot.SetBool("walk", false); // останавливаем анимацию ходьбы
            state = states.dialog; // устанавливаем состояние подхода к объекту в который попали лучом         
            Dialog_Bot();

        }

    }

    IEnumerator Wait() // корутина ожидания
    {
        botagent.SetDestination(transform.position); // обнуляем точку, чтобы бот никуда не шёл
        animbot.SetBool("walk", false); // останавливаем анимацию ходьбы
        state = states.waiting; // указываем, что бот перешел в режим ожидания

        yield return new WaitForSeconds(3f); // ждем 10 секунд

        botagent.SetDestination(points[Random.Range(0, points.Length)].transform.position);
        // destination – куда идти боту, передаем ему рандомно одну из наших точек
        animbot.SetBool("walk", true); // включаем анимацию ходьбы
        state = states.going; // указываем, что бот находится в движении  
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
    }

    void OnAnimatorIK()
    {
     
        if (state == states.dialog)
        {
           if (weight < 1) weight += 0.1f;
            //animbot.SetVector(1, player.transform.TransformPoint(Vector3.up * 0.5f));
           // animbot.SetLookAtPosition(player.transform.TransformPoint(Vector3.up * 0.5f));
          
            // animbot.SetLookAtPosition(player.transform.TransformPoint(Vector3.right));
            // указываем куда смотреть
        }
        else if (weight > 0)
        {
        /*   weight -= 0.1f;
            animbot.SetLookAtWeight(weight);
            animbot.SetLookAtPosition(player.transform.TransformPoint(Vector3.up * 0.5f));
            */

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
        if (!rotating)  StartCoroutine(Rotate(new Vector3(0, rot_angle , 0), 0f));
        
    }

}
