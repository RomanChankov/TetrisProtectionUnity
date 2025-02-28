using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public enum TetrinoFigure { L, Z, I, O, T } // перечисление

public class my_tetrino_data : MonoBehaviour
{
    private GameObject pref_cube; // Созд префаб фигуры

    private GameObject[] my_tetrino_array; //приватный игровой объект . массив из 4-х кубов будет формироваться в фигуры

     private int my_rotation; //поворот фигуры. какой поворот в данный момент будет иметь фигура

    private TetrinoFigure my_type; // Какой будет наш тип фигуры

    public Color my_color {  get; private set; }


    private void Awake() //Метод который выз-ся первый при создании объекта
    {
        my_rotation = 0; //поворот сейчас нулевой 
        my_tetrino_array = new GameObject[4]; //Выделим память под массив игровых кубов

        //загрузим ссылку на наш куб
        pref_cube = Resources.Load("my_prefab/my_prefab_cube") as GameObject;
    }

    public void MySetColor(Color _col)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject go = transform.GetChild(i).gameObject; //Созд временный игровой объект(берем каждый квадратик)
            Material mat = go.GetComponent<MeshRenderer>().material;//Получаем материал
            mat.color = _col; 
            my_color = _col;
        }
    }

    //чтоб могли получить приватный массив делаем свойство доступ только для чтения
    public GameObject[] GetTetrinoArray { get { return my_tetrino_array; } } 

    public void MyRotation(bool _isPositive)
    {
        if (_isPositive)
        {
            my_rotation++;
            my_rotation = my_rotation % 4; // если больше 4 то заново
        }
        else
        {
            my_rotation --; 
            if (my_rotation < 0)
                my_rotation = 3; // против час стрелки вращ
        }
        MyRotationType(my_type, my_rotation);
    }

    private void MyRotationType(TetrinoFigure _figure, int _rot) //Поворащиваем массив из 4х квадратиков нашей фигуры(передали фигуру и поворот)
    {
        switch (_rot)
        {
            case 0:
                if (_figure == TetrinoFigure.L)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(0, 1, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(0, -1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(1,-1, 0);
                }
                else if (_figure == TetrinoFigure.T)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(1, 0, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(-1, 0, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(0, 1, 0);
                }
                else if (_figure == TetrinoFigure.I)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(0, -1, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(0, 1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(0, -2, 0);
                }
                else if (_figure == TetrinoFigure.Z)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(1, 0, 0 );
                    my_tetrino_array[2].transform.localPosition = new Vector3(1, -1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(0, 1, 0);
                }
                else if (_figure == TetrinoFigure.O)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(-1, 0, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(-1, -1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(0, -1, 0);
                }
                break;
            case 1:
                if (_figure == TetrinoFigure.L)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(0, -1, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(-1,-1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(-2, -1, 0);
                }
                else if (_figure == TetrinoFigure.T)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(0, 1, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(0, -1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(-1, 0, 0);
                }
                else if (_figure == TetrinoFigure.I)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(1, 0, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(-1, 0, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(2, 0, 0);
                }
                else if (_figure == TetrinoFigure.Z)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(0, -1, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(-1, -1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(1, 0, 0);
                }
                else if (_figure == TetrinoFigure.O)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(-1, 0, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(-1, -1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(0, -1, 0);
                }
                break;
            case 2:
                if (_figure == TetrinoFigure.L)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(-1, 0, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(0, -1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(0, -2, 0);
                }
                else if (_figure == TetrinoFigure.T)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(1, 0, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(-1, 0, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(0, -1, 0);
                }
                else if (_figure == TetrinoFigure.I)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(0, -1, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(0, 1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(0, -2, 0);
                }
                else if (_figure == TetrinoFigure.Z)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(1, 0, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(1, 1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(0, -1, 0);
                }
                else if (_figure == TetrinoFigure.O)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(-1, 0, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(-1, -1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(0, -1, 0);
                }
                break;
            case 3:
                if (_figure == TetrinoFigure.L)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(0, -1, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(1, 0, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(2, 0, 0);
                }
                else if (_figure == TetrinoFigure.T)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(0, 1, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(0, -1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(1, 0, 0);
                }
                else if (_figure == TetrinoFigure.I)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(1, 0, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(-1, 0, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(2, 0, 0);
                }
                else if (_figure == TetrinoFigure.Z)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(0, -1, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(1, -1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(-1, 0, 0);
                }
                else if (_figure == TetrinoFigure.O)
                {
                    my_tetrino_array[0].transform.localPosition = new Vector3(0, 0, 0);
                    my_tetrino_array[1].transform.localPosition = new Vector3(-1, 0, 0);
                    my_tetrino_array[2].transform.localPosition = new Vector3(-1, -1, 0);
                    my_tetrino_array[3].transform.localPosition = new Vector3(0, -1, 0);
                }
                break;
            default:
                break;
        }
    }

    public void MyInicialize(TetrinoFigure _myType)

    {
        for (int i = 0; i < transform.childCount; i++) // от transform берем детей сколько есть
            Destroy(transform.GetChild(i).gameObject); //При замене типа фигуры на другой тип фигуры, то нужно удалять старые значения(детей которые нах-ся в my_tetrin0_data

            switch (_myType)
            {
                case TetrinoFigure.L:
                    my_type = _myType; //берем наш тип и передаем из метода
                                       //Создаем игровой объект, спавним префаб куда который загружали, делаем  в нулях и (Quaternion.identity)-поворот нулевой
                    GameObject obL = Instantiate(pref_cube, new Vector3(), Quaternion.identity);
                    obL.AddComponent<my_tetrino_segment>();
                //берем transform нашего объекта -куба и устанавливаем ему родителя и делаем ему не мировые координаты а локальные
                    obL.transform.SetParent(transform, false);

                    GameObject obL2 = Instantiate(pref_cube, new Vector3(0, 1, 0), Quaternion.identity);
                    obL2.AddComponent<my_tetrino_segment>();
                    obL2.transform.SetParent(transform, false);

                    GameObject obL3 = Instantiate(pref_cube, new Vector3(0, -1, 0), Quaternion.identity);
                    obL3.AddComponent<my_tetrino_segment>();
                    obL3.transform.SetParent(transform, false);

                    GameObject obL4 = Instantiate(pref_cube, new Vector3(1, -1, 0), Quaternion.identity);
                    obL4.AddComponent<my_tetrino_segment>();
                    obL4.transform.SetParent(transform, false);
                // тут пробегаемся по циклу где выделяли в памяти(4 игровых объекта) расставляем наши объекты которые создали
                    for (int ind = 0; ind < my_tetrino_array.Length; ind++)
                        my_tetrino_array[ind] = transform.GetChild(ind).gameObject;
                    break;

                case TetrinoFigure.Z:
                    my_type = _myType;
                    GameObject obZ = Instantiate(pref_cube, new Vector3(), Quaternion.identity);
                    obZ.AddComponent<my_tetrino_segment>();
                    obZ.transform.SetParent(transform, false);

                    GameObject obZ2 = Instantiate(pref_cube, new Vector3(1, 0, 0), Quaternion.identity);
                    obZ2.AddComponent<my_tetrino_segment>();
                    obZ2.transform.SetParent(transform, false);

                    GameObject obZ3 = Instantiate(pref_cube, new Vector3(1, -1, 0), Quaternion.identity);
                    obZ3.AddComponent<my_tetrino_segment>();
                    obZ3.transform.SetParent(transform, false);

                    GameObject obZ4 = Instantiate(pref_cube, new Vector3(0, 1, 0), Quaternion.identity);
                    obZ4.AddComponent<my_tetrino_segment>();
                    obZ4.transform.SetParent(transform, false);

                    for (int ind = 0; ind < my_tetrino_array.Length; ind++)
                        my_tetrino_array[ind] = transform.GetChild(ind).gameObject;
                    break;

                case TetrinoFigure.I:
                    my_type = _myType;
                    GameObject obI = Instantiate(pref_cube, new Vector3(), Quaternion.identity);
                    obI.AddComponent<my_tetrino_segment>();
                    obI.transform.SetParent(transform, false);

                    GameObject obI2 = Instantiate(pref_cube, new Vector3(0, -1, 0), Quaternion.identity);
                    obI2.AddComponent<my_tetrino_segment>();
                    obI2.transform.SetParent(transform, false);

                    GameObject obI3 = Instantiate(pref_cube, new Vector3(0, 1, 0), Quaternion.identity);
                    obI3.AddComponent<my_tetrino_segment>();
                    obI3.transform.SetParent(transform, false);

                    GameObject obI4 = Instantiate(pref_cube, new Vector3(0, 2, 0), Quaternion.identity);
                    obI4.AddComponent<my_tetrino_segment>();
                    obI4.transform.SetParent(transform, false);

                    for (int ind = 0; ind < my_tetrino_array.Length; ind++)
                        my_tetrino_array[ind] = transform.GetChild(ind).gameObject;
                    break;

                case TetrinoFigure.O:
                    my_type = _myType;
                    GameObject obO = Instantiate(pref_cube, new Vector3(), Quaternion.identity);
                    obO.AddComponent<my_tetrino_segment>();
                    obO.transform.SetParent(transform, false);

                    GameObject obO2 = Instantiate(pref_cube, new Vector3(-1, 0, 0), Quaternion.identity);
                    obO2.AddComponent<my_tetrino_segment>();
                    obO2.transform.SetParent(transform, false);

                    GameObject obO3 = Instantiate(pref_cube, new Vector3(-1, -1, 0), Quaternion.identity);
                    obO3.AddComponent<my_tetrino_segment>();
                    obO3.transform.SetParent(transform, false);

                    GameObject obO4 = Instantiate(pref_cube, new Vector3(0, -1, 0), Quaternion.identity);
                    obO4.AddComponent<my_tetrino_segment>();
                    obO4.transform.SetParent(transform, false);

                    for (int ind = 0; ind < my_tetrino_array.Length; ind++)
                        my_tetrino_array[ind] = transform.GetChild(ind).gameObject;
                    break;

                case TetrinoFigure.T:
                    my_type = _myType;
                    GameObject obT = Instantiate(pref_cube, new Vector3(), Quaternion.identity);
                    obT.AddComponent<my_tetrino_segment>();
                    obT.transform.SetParent(transform, false);

                    GameObject obT2 = Instantiate(pref_cube, new Vector3(1, 0, 0), Quaternion.identity);
                    obT2.AddComponent<my_tetrino_segment>();
                    obT2.transform.SetParent(transform, false);

                    GameObject obT3 = Instantiate(pref_cube, new Vector3(-1, 0, 0), Quaternion.identity);
                    obT3.AddComponent<my_tetrino_segment>();
                    obT3.transform.SetParent(transform, false);

                    GameObject obT4 = Instantiate(pref_cube, new Vector3(0, 1, 0), Quaternion.identity);
                    obT4.AddComponent<my_tetrino_segment>();
                    obT4.transform.SetParent(transform, false);
                    for (int ind = 0; ind < my_tetrino_array.Length; ind++)
                        my_tetrino_array[ind] = transform.GetChild(ind).gameObject;
                    break;
                default:
                    break;
            }
        }
    }
