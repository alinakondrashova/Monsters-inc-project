using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    Transform interactObject; // объект для взаимодействия
    AudioSource bell;

    [SerializeField]
    IKAnimation playerIK; // ссылка на экземпляр скрипта IKAnimation
    Transform inHand;
    int count_items = 0;
    int left_to_take = 4;
    GameObject light;
    bool door_rot = false;
    private void Start()
    {
        bell = GetComponent<AudioSource>();
    }

    void TakeItemInHand(Transform item) // добавим метод для переноса объекта
    {
        inHand = item; // запоминаем объект для взаимодействия
        inHand.parent = transform; // делаем руку родителем объекта
        inHand.localPosition = new Vector3(-0.12f, 0.582f, -0.016f); // устанавливаем положение
        inHand.localEulerAngles = new Vector3(2.793f, 192.86f, -193.209f); // устанавливаем поворот
        playerIK.StopInteraction(); // останавливаем IK-анимацию
        bell.Play();
        MainManager.Messenger.WriteMessage("Вы подобрали " + item.name + ". Теперь можете собрать остальные предметы. " + "Они подсвечены фиолетовым цветом.");
      
    }

    void TakeItemInPocket(GameObject item)
    {
        left_to_take--;
        bell.Play();
        playerIK.StopInteraction(); // прекращение IK-анимации
        if(left_to_take > 0) 
            MainManager.Messenger.WriteMessage("Вы подобрали " + item.name );
        
        else if(left_to_take==0) MainManager.Messenger.WriteMessage("Вы подобрали " + item.name + ". Это последний предмет. Теперь подойдите к Майку и он подскажет вам, куда идти дальше.");
        MainManager.Inventory.AddItem(interactObject.gameObject);
        Destroy(interactObject.gameObject); // удалить объект
    
    }

    void ThroughItem()
    {
        if (inHand != null) // если персонаж держит объект
        {
            inHand.parent = null; // отвязываем объект 
            StartCoroutine(ReadyToTake()); // запускаем корутину 
        }
    }

    IEnumerator ReadyToTake()
    {
        Rigidbody rigidbod = inHand.gameObject.AddComponent<Rigidbody>();
        rigidbod.isKinematic = false;
        rigidbod.useGravity = true;
        yield return new WaitForSeconds(2f); // ждем один кадр
        inHand = null; // обнуляем ссылку
        Destroy(rigidbod);
        interactObject = null;
    }

    private void OnCollisionEnter(Collision collision) // при коллизии с коллайдером предмета 
    {

        if (collision.gameObject.CompareTag("Item") && inHand) // только объекты с тегом item будем удалять
        {
            light = GameObject.Find(collision.gameObject.name + " light");
            light.GetComponent<Light>().enabled = false;
            TakeItemInPocket(collision.gameObject); // передаем в метод объект для удаления
            count_items++;
            
        }
        if (collision.gameObject.CompareTag("ItemForTransfer") && !inHand)
        // если это объект для перемещения и в руке нет другого предмета
        {
            light = GameObject.Find(collision.gameObject.name + " light");
            light.GetComponent<Light>().enabled = false;
            TakeItemInHand(collision.gameObject.transform);
        }
       
       

        if (collision.gameObject.CompareTag("door") && count_items>=4)
        {
            if (door_rot == false) {
                objectToRotate = collision.gameObject;
                StartRotation();
                MainManager.Messenger.WriteMessage("Вы почти достигли своей цели. Теперь вам нужно найти девочку");
                objectToRotate.GetComponent<BoxCollider>().isTrigger = false;
                objectToRotate.GetComponent<CharacterController>().enabled = false;
            }
            door_rot = true;
        }
        else if(collision.gameObject.CompareTag("door") && count_items <4)  MainManager.Messenger.WriteMessage("Чтобы зайти в дом, у вас должна быть сумка со всеми предметами");
    

}

    private void OnTriggerEnter(Collider other) // рука попадает в триггер
    {
        if (other.CompareTag("Item") && inHand)
        {
            interactObject = other.transform; // записываем объект для взаимодействия
            playerIK.StartInteraction(other.gameObject.transform.position); // сообщаем скрипту 
            //IKAnimation о начале взаимодействия для запуска IK - анимации
        }


        if (other.CompareTag("Item") && !inHand)
        {
            MainManager.Messenger.WriteMessage("Чтобы подобрать предмет, у вас должна быть сумка");
        }


        if (other.CompareTag("ItemForTransfer"))
        {
            interactObject = other.transform; // записываем объект для взаимодействия
            playerIK.StartInteraction(other.gameObject.transform.position); // сообщаем скрипту 
        }

    }
    private void FixedUpdate()
    {
        CheckDistance(); // проверка дистанции с объектом
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) { ThroughItem(); }
    }

    void CheckDistance() // метод для проверки дистанции, чтобы была возможность прекратить взаимодействие с объектом при отдалении
    {
        if (interactObject != null && Vector3.Distance(transform.position, interactObject.position) > 2)
        // если происходит взаимодействие и дистанция стала больше 2-ух
        {
            interactObject = null; // обнуляем ссылку на объект
            playerIK.StopInteraction(); // прекращаем IK-анимацию
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
      
        StartCoroutine(Rotate(new Vector3(0, 110, 0), 1f));

    }

}
