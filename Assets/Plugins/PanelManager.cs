using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace M2MqttUnity.Examples
{
    public class PanelManager : MonoBehaviour
    {
        public GameObject panel1;
        public GameObject panel2;
        // Start is called before the first frame update
        void Start()
        {
            setPanel1();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void setPanel1()
        {
            panel1.SetActive(true);
            panel2.SetActive(false);
            GetComponent<ChuongGaManager>().enabled = false;
            GetComponent<ChuongGaMqtt>().enabled = false;

        }

        public void setPanel2()
        {
            panel1.SetActive(false);
            panel2.SetActive(true);
            GetComponent<ChuongGaManager>().enabled = true;
            GetComponent<ChuongGaMqtt>().enabled = true;
            GetComponent<ChuongGaMqtt>().ManualStart();
        }
    }
}