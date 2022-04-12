using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace M2MqttUnity.Examples
{
    public class ChuongGaManager : MonoBehaviour
    {

        [SerializeField]
        private Text temperature;
        [SerializeField]
        private Text humidity;

        Window_Graph window_Graph;
        List<int> tempList = new List<int>() { 0 };
        List<int> humiList = new List<int>() { 0 };


        private void Start()
        {
            window_Graph = FindObjectOfType<Window_Graph>();
        }
        public void Update_Status(Status_Data _status_data)
        {
            temperature.text = _status_data.temperature + " °C";
            humidity.text = _status_data.humidity + " %";

            int temp = Int32.Parse(_status_data.temperature);
            int humi = Int32.Parse(_status_data.humidity);
            // Debug.Log(temp);
            tempList.Add(temp);
            humiList.Add(humi);

            // window_Graph.ShowGraph(tempList);
            window_Graph.ShowDoubleGraph(tempList, humiList);
        }

    }
}