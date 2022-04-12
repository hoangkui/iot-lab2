using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;
using M2MqttUnity;
using Newtonsoft.Json.Linq;
using System.Linq;
using Newtonsoft.Json;

namespace M2MqttUnity.Examples
{
    public class Status_Data
    {
        public string temperature { get; set; }
        public string humidity { get; set; }
    }

    public class Data_Control
    {
        public Data_Control(string a, string b)
        {
            this.device = a;
            this.status = b;
        }
        public string device { get; set; }
        public string status { get; set; }
    }


    public class ChuongGaMqtt : M2MqttUnityClient
    {
        public List<string> topics = new List<string>();


        public string msg_received_from_topic_status = "";
        public string msg_received_from_topic_control = "";


        private List<string> eventMessages = new List<string>();
        [SerializeField]
        public Status_Data _status_data;

        public InputField InputBrokenURI, InputUsername, InputPassword;

        public void SetEncrypted(bool isEncrypted)
        {
            this.isEncrypted = isEncrypted;
        }


        protected override void OnConnecting()
        {
            base.OnConnecting();
            //SetUiMessage("Connecting to broker on " + brokerAddress + ":" + brokerPort.ToString() + "...\n");
        }

        protected override void OnConnected()
        {
            base.OnConnected();

            SubscribeTopics();
        }

        protected override void SubscribeTopics()
        {

            foreach (string topic in topics)
            {
                if (topic != "")
                {
                    client.Subscribe(new string[] { topic }, new byte[] { MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE });

                }
            }
        }

        protected override void UnsubscribeTopics()
        {
            foreach (string topic in topics)
            {
                if (topic != "")
                {
                    client.Unsubscribe(new string[] { topic });
                }
            }

        }

        protected override void OnConnectionFailed(string errorMessage)
        {
            Debug.Log("CONNECTION FAILED! " + errorMessage);
            GetComponent<PanelManager>().setPanel1();
            this.Disconnect();
        }

     

        protected override void OnConnectionLost()
        {
            Debug.Log("CONNECTION LOST!");
        }



        protected override void Start()
        {
            return;
        }
        public void ManualStart()
        {
            this.brokerAddress = InputBrokenURI.text;
            this.mqttUserName = InputUsername.text;
            this.mqttPassword = InputPassword.text;
            base.Start();
        }
        protected override void DecodeMessage(string topic, byte[] message)
        {
            string msg = System.Text.Encoding.UTF8.GetString(message);
            Debug.Log("Received: " + msg);
            //StoreMessage(msg);
            if (topic == topics[0])
                ProcessMessageStatus(msg);

            /*if (topic == topics[2])
                ProcessMessageControl(msg);*/
        }

        public void ProcessMessageStatus(string msg)
        {
            var _status_data = JsonConvert.DeserializeObject<Status_Data>(msg);
            msg_received_from_topic_status = msg;
            // Debug.Log(_status_data.humidity);
            GetComponent<ChuongGaManager>().Update_Status(_status_data);
        }

        public void PublishDeviceControl(string name, string status)
        {
            var data = new Data_Control(name, status);
            string msg_config = JsonConvert.SerializeObject(data);
            if (name == "LED")
            {
                client.Publish(topics[1], System.Text.Encoding.UTF8.GetBytes(msg_config), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            }
            else if (name == "PUMP")
            {
                client.Publish(topics[2], System.Text.Encoding.UTF8.GetBytes(msg_config), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, true);
            }
            Debug.Log("publish " + name);
        }
        private void OnDestroy()
        {
            Disconnect();
        }

        private void OnValidate()
        {
            //if (autoTest)
            //{
            //    autoConnect = true;
            //}
        }
    }
}