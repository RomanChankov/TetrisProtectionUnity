using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public struct MyHud //Вспомогательная структура
{
    public Text txt_score;
    private int my_int_score;

    public Text txt_line;
    private int my_int_line;

    public Text txt_level;
    public int my_int_level { get; private set; }

    public float my_speed;
    private int my_counter_line; //Счетчик линий

    public void AddScore(int _score)
    {
        my_int_score += _score;
        txt_score.text = my_int_score.ToString();
    }

    public void AddLine(int _line)
    {
        my_int_line += _line;
        txt_line.text = my_int_line.ToString();

        my_counter_line += _line;
        if (my_counter_line > 9)
            AddLevel(1); //Добавляем по одному уровню

        my_counter_line = my_counter_line % 10; //Если уровень достигнет 10 то после оновится и станет 0
    }
    public void AddLevel(int _level)
    {
        my_int_level += _level;
        txt_level.text = my_int_level.ToString();
        my_speed -= my_int_level > 5 ? 0.02f : 0.05f; //Нелинейное добавление скорости нашей фигуре
    }
}

public class my_main : MonoBehaviour
{
    private const int wid = 13, hei = 21; //размеры нашего поля
    private float my_step = 1; //Задаем шаг смещения фигуры
    private float my_curr_time; //Мое текущее время

    private GameObject pref_tetrino;  //Создаем GameObject как префаб(сделали префаб игрового объекта)
    private Object pref_tetrino_object; //есть префаб со ссылкой на игровой объект
    //сделаем ссылку на нашу фигуру, чтоб ею можно было управлять(скрипт на фигуру)
    private my_tetrino_figure my_figure;
    private TetrinoFigure my_figure_random;

    private my_tetrino_element[,] my_array;
    private MyHud my_hud; //Объявим структуру

    private GameObject my_3d_camera;//Сделаем игровой объект
    private GameObject my_main_camera;
    private GameObject my_animation_camera;

    private my_title_figure my_title; //Созд объект который создали си шарп скрипт пустой

    private AudioSource my_audio_sound;
    private AudioClip[] my_sounds; //Массив наших звуков которые переместили в папку

    private void Start()
    {
        my_curr_time = 0;
        my_array = new my_tetrino_element[wid, hei];
        //самост-но загрузим префаб чтоб у нас была ссылка на ту нашу фигуру которая будет падать вниз
        //и приведем к типу GameObject т к из папки Resource ,Load берется на сам объект
        //а нам нужен игровой объект
        pref_tetrino = Resources.Load("my_prefab/my_tetrino_figure") as GameObject; //загрузим в префаб ссылку
        pref_tetrino_object = Resources.Load("my_prefab/my_prefab_tetrino_o"); // загрузим ссылку на наш объект

        my_title = FindObjectOfType<my_title_figure>();

        my_sounds = Resources.LoadAll<AudioClip>("my_sound"); //Загружаем из ресурса звуки
        my_audio_sound = GetComponent<AudioSource>();
        MyRandomSound();

       my_main_camera = GameObject.FindGameObjectWithTag("MainCamera");
       my_3d_camera = GameObject.FindGameObjectWithTag("my_3d_camera");//ищем камеру на сцене по тегу
        my_animation_camera = GameObject.FindGameObjectWithTag("my_camera_animation");
        my_animation_camera.SetActive(false); //Анимационная камера изначально выключена
       MyOnSwitchCamera(false);//Говорим что 3D камеру нужно выключить.


        //Находим игровые объекты по тегу
        my_hud.txt_score = GameObject.FindGameObjectWithTag("my_score").GetComponent<Text>();
        my_hud.txt_line = GameObject.FindGameObjectWithTag("my_line").GetComponent<Text>();
        my_hud.txt_level = GameObject.FindGameObjectWithTag("my_level").GetComponent<Text>();
        my_hud.my_speed = 0.5f;
        my_hud.AddLevel(1); //Тут устанавливаем нашей структуре 1-ый уровень

        my_figure_random = CreateRandomFigure();
        CreateFigure(my_figure_random); //Создаем фигуру в наше игровое пространство
        my_figure_random = CreateRandomFigure(); //Инициализируем следующую рандомную фигуру
        my_title.GetComponentInChildren<my_tetrino_data>().MyInicialize(my_figure_random);
        for (int y = 0; y < hei; y++)
            for (int x = 0; x < wid; x++)
            { 
                GameObject go = Instantiate(pref_tetrino_object, new Vector3(x * my_step, y * my_step, 0), Quaternion.identity) as GameObject; //позициия , где будет появляться фигура
                my_array[x, y] = go.GetComponent<my_tetrino_element>(); //берем наш игровой объект который заспавнили получаем компонент который на него вешали чтоб определять
            }
    }

    private void MyRandomSound()
    {
        int index = Random.Range(0, my_sounds.Length);
        my_audio_sound.clip = my_sounds[index]; //Берем наш звук его свойство clip и присваиваем звук от индекса
    }

    //Создаю функцию возвр-щую TetrinoFigure без пар-ов
    private TetrinoFigure CreateRandomFigure()
    {
        return (TetrinoFigure)Random.Range(0, 5);
    }

    //фнкция создающая нашу фигуру
    private void CreateFigure(TetrinoFigure _figure) // передаем тип фигуры чтоб могли контролировать какую создать
    {
        //Установил позицию, где будет появляться наша фигура
        my_figure = Instantiate(pref_tetrino, new Vector3(my_step * 6, my_step * (hei - 2)), Quaternion.identity).GetComponent<my_tetrino_figure>();
        // кубы которые будут проинициализируются и сделают нашу фигуру
        my_figure.GetComponentInChildren<my_tetrino_data>().MyInicialize(_figure);

        if (my_hud.my_int_level < 1)
            my_figure.GetComponentInChildren<my_tetrino_data>().MySetColor(Random.ColorHSV(0.4f, 0.6f, 1, 1, 1, 1, 1, 1));
        else if (my_hud.my_int_level < 2)
            my_figure.GetComponentInChildren<my_tetrino_data>().MySetColor(Random.ColorHSV(0.5f, 0.7f, 1, 1, 1, 1, 1, 1));
        else if (my_hud.my_int_level < 3)
            my_figure.GetComponentInChildren<my_tetrino_data>().MySetColor(Random.ColorHSV(0.8f, 1.0f, 1, 1, 1, 1, 1, 1));
        else if (my_hud.my_int_level < 4)
            my_figure.GetComponentInChildren<my_tetrino_data>().MySetColor(Random.ColorHSV(0.2f, 0.4f, 1, 1, 1, 1, 1, 1));
        else if (my_hud.my_int_level >= 10)
            my_figure.GetComponentInChildren<my_tetrino_data>().MySetColor(Random.ColorHSV(0.3f, 0.5f, 1, 1, 1, 1, 1, 1));

        

        StartCoroutine(my_update(my_hud.my_speed)); //сделаем корутину
    }
    private IEnumerator my_update(float _time)
    {
        while (true) // Бесконеч цикл
        {
            yield return new WaitForSeconds(_time); //Возвращает задержку нашего времени
            my_figure.MyDropTetrino(true);
            if (CheckPreIntersect(my_figure)) //если произошло пересечение 
                break;     //остановим наш цикл
        }

        AddToArray();
        Destroy(my_figure.gameObject); //Удаляем фигуру которая упала и нам больше не нужна
        MyRemoveFullLine(); //Вызываем метод удаления линии

        if (!IsGameOver())  //Если я не проиграл
        {
            CreateFigure(my_figure_random); // то создам фигуру
            my_figure_random = CreateRandomFigure(); //рандомную
            my_title.GetComponentInChildren<my_tetrino_data>().MyInicialize(my_figure_random);

        }
        else
        {
            //Если проиграли выключаем все камеры и включим анимационную
            my_main_camera.SetActive(false);
            my_3d_camera.SetActive(false);
            my_animation_camera.SetActive(true);

            my_title.gameObject.SetActive(false); //Так же выключим след игровую фигуру
        }
    }

    private void AddToArray()  // когда фигура упала добавляем ее в двумерный массив
    {
        GameObject[] go = my_figure.GetComponentInChildren<my_tetrino_data>().GetTetrinoArray; //созд временный игровой объект то есть массив
        for (int i = 0; i < go.Length; i++)
        {
            //устан мировые координаты что можно было каждый квадратик вставить в двумер массив
            int x = (int)go[i].transform.position.x;
            int y = (int)go[i].transform.position.y;

            my_array[x, y].set_tetrino_active(true); //сделаем квадратики видимыми
            my_array[x, y].set_color(my_figure.GetComponentInChildren<my_tetrino_data>().my_color);
        }
    }

    private void MyRemoveFullLine()
    {
        int[] removeLine = MyCheckFullLine(); //вычисляем заполненные линии и если что то есть в нашем массиве заходим в цикл
        for (int i = 0; i < removeLine.Length; i++)
        {
            for (int x = 0; x < wid; x++)
                my_array[x, removeLine[i]].set_tetrino_active(false);// если массив заполнен просто выключим его
            my_hud.AddScore(removeLine.Length == 4 ? 750 : 350);
        }
        if (removeLine.Length != 0)
        {
            MyRandomSound();
            my_audio_sound.Play(); //если какая то строка убрана вызываем звук
            int[] empty_line = MyCheckEmptyLine();
            bool[,] arr_new_tetrino = new bool[wid, hei];

            int start_y = 0;
            my_hud.AddLine(removeLine.Length); //сколько линий убрали столько добавим в счетчик
            for (int y = 0; y < hei; y++)
            {
                if (MySkipTheLine(empty_line, y))
                    continue; //если линия пустая то пропускаем
                for (int x = 0; x < wid; x++)
                    arr_new_tetrino[x, start_y] = my_array[x, y].get_isActive_tetrino();
                start_y++;
            }
            MySetNewTetrinoArray(arr_new_tetrino); //передаем новый массив
        }
    }

    //Метод для события
    public void MyOnSwitchCamera(bool _is3D)
    {
        my_main_camera.SetActive(!_is3D);
        my_3d_camera.SetActive(_is3D);
    }


    public void MyOnStartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //Получаем активную сцену 
    }

    //Создадим метод делающий паузу
    public void MyOnPause(bool _isPause)
    {
        Time.timeScale = _isPause ? 0 : 1;
    }

    //Проиграли ли мы?
    private bool IsGameOver()
    {
        for (int i = 0; i < wid; i++)
        {
            if (my_array[i, hei - 3].get_isActive_tetrino()) //если есть хоть одна заполнен-я клетка мы проиграли
                return true;
        }
        return false;
    }

    private void MySetNewTetrinoArray(bool[,] _arr_new)
    {
        for (int y = 0; y < hei; y++)
        {
            for (int x = 0; x < wid; x++)
                my_array[x, y].set_tetrino_active(_arr_new[x, y]);
        }
    }
    private bool MySkipTheLine(int[] _empty_line, int _y)
    {
        for (int y = 0; y < _empty_line.Length; y++)
        {
            if (_empty_line[y] == _y)
                return true; //говорит о том что линия пустая 
        }
        return false;
    }

    private int[] MyCheckEmptyLine() //вычисляем не заполненные линии
    {
        List<int> arr = new List<int>();
        for (int i = 0; i < hei; i++)
        {
            int count_line_x = 0;
            for (int x = 0; x < wid; x++)
            {
                if (my_array[x, i].get_isActive_tetrino())
                    break;
                else
                    count_line_x++;
            }
            if (count_line_x == wid)
                arr.Add(i);
        }
        return arr.ToArray();
    }

    private int[] MyCheckFullLine() //вычисляем заполненные линии
    {
        List<int> arr = new List<int>(); //хранить все индексы по Y которые нужно удалить т к будут заполненными
        for (int i = 0; i < hei; i++) //цикл по высоте 
        {
            int count_line_x = 0;
            for (int x = 0; x < wid; x++) //цикл по ширине(смотрим заполнена линия или нет)
            {
                if (my_array[x, i].get_isActive_tetrino())
                    count_line_x++;
                else
                    break;
            }
            if (count_line_x == wid)
                arr.Add(i);
        }
        return arr.ToArray();
    }
    private void Update() //для обновления логики игры, вызывается каждый кадр
    {
        if (my_figure) // проверяем фигуру
        {
            if (Input.GetButtonDown("RotateTetrino")) // если нажали
            {
                my_figure.GetComponentInChildren<my_tetrino_data>().MyRotation(true); //у нашей фигуры ищем детей скрипт
                if (CheckIntersect(my_figure)) //проверка на пересечение
                    my_figure.GetComponentInChildren<my_tetrino_data>().MyRotation(false);// не даем фигуре поворачиваться если это невозможно


            }
            if (Input.GetButtonDown("LeftTetrino"))
            {
                my_curr_time = 0;
                my_figure.MySetDirecton(MyDirectionTetrino.LEFT); //если произошло пересечение с левым краем 
                if (CheckIntersect(my_figure))
                    my_figure.MySetDirecton(MyDirectionTetrino.RIGHT); // отодвигаем направо
            }
            else if (Input.GetButtonDown("RightTetrino"))
            {
                my_curr_time = 0;
                my_figure.MySetDirecton(MyDirectionTetrino.RIGHT);//если произошло пересечение с правым краем 
                if (CheckIntersect(my_figure))
                    my_figure.MySetDirecton(MyDirectionTetrino.LEFT);// отодвигаем влево
            }
            if (Input.GetButton("DownTetrino"))
                MyInputPress(MyDirectionTetrino.DOWN, 0.05f);
            if (Input.GetButton("RightTetrino"))
                MyInputPress(MyDirectionTetrino.RIGHT, 0.09f);
            else if (Input.GetButton("LeftTetrino"))
                MyInputPress(MyDirectionTetrino.LEFT, 0.09f);
            if (Input.GetButtonUp("LeftTetrino") || Input.GetButton("RightTetrino")) //если клавишу отжали
                //обнулим таймер и уже при более долгой зажатой клавише фигуры двигаться не будут
                my_curr_time = 0;
        }
    }

    private void MyInputPress(MyDirectionTetrino _dir, float _time)
    {
        my_curr_time += Time.deltaTime; //прибавляем некое время (прошедшее время с последнего кадра
        if (my_curr_time > _time) //если my_curr_time больше тоговремени которое мы передали сработает наш таймер
        {
            my_curr_time = 0;
            if (_dir == MyDirectionTetrino.LEFT)
            {
                my_figure.MySetDirecton(MyDirectionTetrino.LEFT);
                if (CheckIntersect(my_figure))
                    my_figure.MySetDirecton(MyDirectionTetrino.RIGHT);
            }
            else if (_dir == MyDirectionTetrino.RIGHT)
            {
                my_figure.MySetDirecton(MyDirectionTetrino.RIGHT);
                if (CheckIntersect(my_figure))
                    my_figure.MySetDirecton(MyDirectionTetrino.LEFT);
            }
            else if (_dir == MyDirectionTetrino.DOWN)
            {
                my_figure.MyDropTetrino(true);
                if (CheckIntersect(my_figure)) //если пересеклась с ниж частью 
                    my_figure.MyDropTetrino(false);//подн вверх
            }
        }
    }

    private bool CheckPreIntersect(my_tetrino_figure _figure) // Предварительное пересечение
    {
        for (int i = 0; i < _figure.GetSegments().Length; i++)
        {
            int x = (int)_figure.GetSegments()[i].transform.position.x;
            int y = (int)_figure.GetSegments()[i].transform.position.y;
            bool is_intersect = IsIntersect(x, y); //смотрим вышла ли за пределы массива на этом сегменте
            if (is_intersect) //если есть пересечение
            {
                _figure.MyDropTetrino(false); //поднимем ее вверх
                return is_intersect;
            }
        }
        return false;
    }
    private bool CheckIntersect(my_tetrino_figure _figure) 
    {
        for (int i = 0; i < _figure.GetSegments().Length; i++)
        {
            int x = (int)_figure.GetSegments()[i].transform.position.x;
            int y = (int)_figure.GetSegments()[i].transform.position.y;
            bool is_intersect = IsIntersect(x, y);
            if (is_intersect)
                return is_intersect;
        }
        return false;
    }
    //Проверяем установилась ли наша фигура на землю или нет (детекция пересечени)
    private bool IsIntersect(int _x, int _y)
    {
        try
        {
            if (my_array[_x, _y].get_isActive_tetrino()) //Обращ к двумер массиву и смотрм не вышли ли за пределы
                return true;
        }
        catch (System.Exception ex) { return true; } // Если нет у нас пересечения по позициям то вернем true т  к иначе будет возврвщать false а нам этого не надо

        return false;
    }
}
