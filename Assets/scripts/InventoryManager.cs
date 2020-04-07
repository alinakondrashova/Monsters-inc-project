using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{

    [SerializeField]
    private GameObject Inventory; // ссылка на панель с инвентарём

    [SerializeField]
    UIObject[] objects; //массив элементов UI, отображающих предметы

    private void Start()
    {
        Inventory.SetActive(false); // скрываем инвентарь
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) // отслеживаем нажатие клавиши “I”
        {
            Inventory.SetActive(!Inventory.activeSelf); // инвертируем состояние инвентаря
            if (Inventory.activeSelf) UpdateUI();
            // обновляем предметы в инвентаре, если инвентарь открытый
        }
    }
    public void AddItem(GameObject objectInScene)
    // публичный метод для добавления объекта в инвентарь
    {
        foreach (UIObject obj in objects) // обходим массив UI объектов
        {
            if (objectInScene.Equals(obj.myObject()))
            // если имя подобранного объекта совпадаем с именем одного из объектов в массиве
            {
                obj.State = true; // отмечаем объект в массиве как активный (подобранный)
                //if (CheckItems()) MainManager.game.WinGame();
                break; // выходим из цикла, если нашли подходящий объект
            }
        }

        if (Inventory.activeSelf) UpdateUI();
        // если после добавления элемента инвентарь был открыт - обновляем его
    }

    void UpdateUI() // метод обновления инвентаря
    {
        foreach (UIObject obj in objects) // обходим массив объектов
        {
            obj.UpdateImage(); // обновляем каждый из объектов
        }
    }

    bool CheckItems() // проверка, все ли объекты собраны
    {
        foreach (UIObject obj in objects) // обходим массив объектов
        {
            if (!obj.State) return false; // если находим хоть один не активный - возвращаем false, т.е. что собраны еще не все предметы
        }
        return true; // если цикл прошел по всем предметам и не был выявлен ни один не активный - значит все предметы собраны
    }


}
