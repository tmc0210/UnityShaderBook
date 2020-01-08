using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamera : MonoBehaviour
{
    //旋转变量;
    private float m_deltX = 0f;
    private float m_deltY = 0f;
    //缩放变量;
    private float m_distance = 10f;
    private float m_mSpeed = 5f;
    //移动变量;
    private Vector3 m_mouseMovePos = Vector3.zero;

    void Start()
    {

    }

    void Update()
    {
        float move_speed = Input.GetKey(KeyCode.LeftShift) ? 4.0f : 2.0f;
        //w键前进
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, move_speed * Time.deltaTime));
        }
        //s键后退
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, -1 * move_speed * Time.deltaTime));
        }
        //a键后退
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-1 * move_speed * Time.deltaTime, 0, 0));
        }
        //d键后退
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(move_speed * Time.deltaTime, 0, 0));
        }

        //鼠标右键点下控制相机旋转;
        if (Input.GetMouseButton(1))
        {
            m_deltX += Input.GetAxis("Mouse X") * m_mSpeed;
            m_deltY -= Input.GetAxis("Mouse Y") * m_mSpeed;
            m_deltX = ClampAngle(m_deltX, -360, 360);
            m_deltY = ClampAngle(m_deltY, -70, 70);
            GetComponent<Camera>().transform.rotation = Quaternion.Euler(m_deltY, m_deltX, 0);
        }

        //鼠标中键点下场景缩放;
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            //自由缩放方式;
            m_distance = Input.GetAxis("Mouse ScrollWheel") * 10f;
            GetComponent<Camera>().transform.localPosition = GetComponent<Camera>().transform.position + GetComponent<Camera>().transform.forward * m_distance;
        }
        //鼠标点击场景移动;
        if (Input.GetMouseButtonDown(2))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                m_mouseMovePos = hitInfo.point;
            }
        }
        else if (Input.GetMouseButton(2))
        {
            Vector3 p = Vector3.zero;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                p = hitInfo.point - m_mouseMovePos;
                p.y = 0f;
            }
            GetComponent<Camera>().transform.localPosition = GetComponent<Camera>().transform.position - p * 0.05f; //在原有的位置上，加上偏移的位置量;
        }

        //相机复位远点;
        if (Input.GetKey(KeyCode.Space))
        {
            m_distance = 10.0f;
            GetComponent<Camera>().transform.localPosition = new Vector3(0, m_distance, 0);
        }
    }

    //规划角度;
    float ClampAngle(float angle, float minAngle, float maxAgnle)
    {
        if (angle <= -360)
            angle += 360;
        if (angle >= 360)
            angle -= 360;

        return Mathf.Clamp(angle, minAngle, maxAgnle);
    }
}

