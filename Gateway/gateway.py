print("Hello MQTT SERVER")
import paho.mqtt.client as mqttclient
import time
import json
import random
import serial.tools.list_ports

# Define
BROKER_ADDRESS = "mqttserver.tk"
PORT = 1883
AIO_FEED_ID = ['/bkiot/1913446/led', '/bkiot/1913446/pump']


def subscribed(client, userdata, mid, granted_qos):
    print("Subscribed...")

def recv_message(client, userdata, message):
    print("Received: ", client, userdata, message.payload.decode("utf-8"))
    
def connected(client, usedata, flags, rc):
    if rc == 0:
        print("Server connected successfully!!")
        for feed in AIO_FEED_ID:
            client.subscribe(feed)
    else:
        print("Connection is failed")

client = mqttclient.Client()
client.username_pw_set("bkiot", '12345678') 
client.on_connect = connected
client.connect(BROKER_ADDRESS, 1883)
client.loop_start()
client.on_subscribe = subscribed
client.on_message = recv_message

while True:
    # random temperature and humidity
    temp = random.randrange(0, 100)
    humi = random.randrange(0, 100)
    # data json
    data = {'temperature': temp, 'humidity': humi} 
    print(data)
    x = client.publish('/bkiot/1913446/status', json.dumps(data), 1)
    time.sleep(10)