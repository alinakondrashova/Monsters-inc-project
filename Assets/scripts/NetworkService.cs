using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


public class NetworkService : MonoBehaviour
{
    Material maintexture; // ссылка на текстуру экрана

    private void Start()
    {
        maintexture = GetComponent<Renderer>().material; // инициализируем ссылку на текстуру
        StartCoroutine(ShowImages()); // запускаем корутину, сменяющую изображения
    }
    //массив из 10 изображений для загрузки, замените ссылки на свои!
    private string[] webImages = {
        "https://images.unsplash.com/photo-1575015642299-5b92fcbd0ba4?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=1050&q=80",
        "https://images.unsplash.com/photo-1489183988443-b37b7e119ba6?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
        "https://images.unsplash.com/photo-1533422902779-aff35862e462?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=500&q=60",
        "https://images.unsplash.com/photo-1573867368999-5388ba407550?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=900&q=60",
        "https://images.unsplash.com/photo-1573743338941-39db12ef9b64?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=900&q=60",
        "https://images.unsplash.com/photo-1572295727871-7638149ea3d7?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=900&q=60",
        "https://images.unsplash.com/photo-1571217668979-f46db8864f75?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=900&q=60",
        "https://images.unsplash.com/photo-1507525428034-b723cf961d3e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=900&q=60",
        "https://images.unsplash.com/photo-1535479804851-93f60320e644?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=900&q=60",
        "https://images.unsplash.com/photo-1520190282873-afe1285c9a2a?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=crop&w=900&q=60"
    };
    private Texture[] Images = new Texture[10]; // массив из загруженных изображений
    int i = 0; // счетчик, чтобы знать какое изображение показывается
    IEnumerator ShowImages() // корутина смены изображений
    {
        while (true)
        {
            if (Images[i] == null) // если требуемой текстуры нет в массиве
            {
                using (WWW www = new WWW(webImages[i])) {
                    yield return www; // ждем когда изображение загрузится
                    Images[i] = www.texture; // записываем загруженную текстуру в массив

                } // загружаем изображение по ссылке       
            }
            maintexture.mainTexture = Images[i]; // устанавливаем текстуру из массива изображений
            i++; // увеличиваем счетчик
            if (i == 10) i = 0; // если загрузили уже 9, возвращаемся к первому
            yield return new WaitForSeconds(3f); // ждем 3 секунды между сменой изображений
        }
    }

}
