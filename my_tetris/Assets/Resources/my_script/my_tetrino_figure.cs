using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MyDirectionTetrino:int { LEFT = -1,RIGHT = 1,DOWN}

public class my_tetrino_figure : MonoBehaviour
{
    //Функция сдвигает нашу фигуру вниз
    public void MyDropTetrino(bool _isPositive)
    {
        if (_isPositive)
            transform.Translate(0,-1,0); //если истина сдвигаем вниз
        else transform.Translate(0,1,0); //но если нужно, откатим фигуру вверх
    }

    public void MySetDirecton(MyDirectionTetrino _dir)
    {
        transform.Translate((int)_dir,0,0);
    }
    public my_tetrino_segment[] GetSegments()
    {
        return GetComponentsInChildren<my_tetrino_segment>(); //Возвращаем все сегменты которые есть в нашей фигуре
    }
}
