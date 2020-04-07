using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    CharacterController controller; // переменная для обращения к контроллеру
    float speedMove = 3f; // переменная для управления скоростью перемещения персонажа
    float speedTurn = 60f; // переменная для управления скорость поворота персонажа
    Animator anim;
    AudioSource source;

    private void Animate(int v)
    {
        bool walk = v != 0; // если v не равен 0, есть движение 
        anim.SetBool("walk", walk); // переключаем анимацию
    }


    private void Move(int v)
    {
        Vector3 movement = new Vector3(0f, -1f, v);
        movement = movement * speedMove * Time.deltaTime; // учитываем скорость и время
        controller.Move(transform.TransformDirection(movement));
    }


    private void Turn(int h)
    {
        float turn = h * speedTurn * Time.deltaTime;
        transform.Rotate(0f, turn, 0f);
    }


    void FixedUpdate()
    {
        int h = (int)Input.GetAxis("Horizontal");
        int v = (int)Input.GetAxis("Vertical");
        Move(v);
        Turn(h);
        Animate(v);
        AudioPlay(v);
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        { anim.SetBool("walk", true); }
        if (Input.GetKeyUp(KeyCode.W))
        { anim.SetBool("walk", false); }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        { anim.SetBool("run", true); }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        { anim.SetBool("run", false); }

        if (Input.GetKeyDown(KeyCode.V)) { anim.SetTrigger("Open door"); }
        //if (Input.GetKeyDown(KeyCode.V)) { anim.SetTrigger("victory 0"); }
        if (Input.GetKeyDown(KeyCode.P)) { anim.SetTrigger("pick up"); }

    }

    private void AudioPlay(int v)
    {
        if (v != 0 && !source.isPlaying) source.Play();
        else if (v == 0 && source.isPlaying) source.Stop();
    }

    
}