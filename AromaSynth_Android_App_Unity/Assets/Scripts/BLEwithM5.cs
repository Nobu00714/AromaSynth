/* This is a simple example to show the steps and one possible way of
 * automatically scanning for and connecting to a device to receive
 * notification data from the device.
 *
 * It works with the esp32 sketch included at the bottom of this source file.
 */

using UnityEngine;
using UnityEngine.UI;

public class BLEwithM5 : MonoBehaviour
{
    public string beforeSceneName;
    public string DeviceName = "ledbtn";
    public string ServiceUUID = "A9E90000-194C-4523-A473-5FDF36AA4D20";
    public string SubscribeCharacteristic = "A9E90001-194C-4523-A473-5FDF36AA4D20";
    public string WriteCharacteristic = "A9E90002-194C-4523-A473-5FDF36AA4D20";

    public enum States
    {
        None,
        Scan,
        ScanRSSI,
        ReadRSSI,
        Connect,
        RequestMTU,
        Subscribe,
        Unsubscribe,
        Disconnect,
    }

    private bool _connected = false;
    private float _timeout = 0f;
    public States _state = States.None;
    private string _deviceAddress;
    private bool _foundWriteID = false;
    private bool _foundSubscribeID = false;
    public byte[] _dataBytes = null;
    private bool _rssiOnly = false;
    private int _rssi = 0;
    public int sendDataTest = 0;
    public string statusString;

    void Reset()
    {
        _connected = false;
        _timeout = 0f;
        _state = States.None;
        _deviceAddress = null;
        _foundWriteID = false;
        _foundSubscribeID = false;
        _dataBytes = null;
        _rssi = 0;
    }

    public void SetState(States newState, float timeout)
    {
        _state = newState;
        _timeout = timeout;
    }

    public void StartProcess()
    {
        Reset();
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {

            SetState(States.Scan, 0.1f);

        }, (error) =>
        {

            statusString = "Error during initialize: " + error;
        });
    }

    // Use this for initialization
    void Start()
    {
        StartProcess();
    }

    // private void ProcessButton(byte[] bytes)
    // {
    //     if (bytes[0] == 0x00)
    //         ButtonPositionText.text = "Not Pushed";
    //     else
    //         ButtonPositionText.text = "Pushed";
    // }

    // Update is called once per frame
    void Update()
    {
        if (_timeout > 0f)
        {
            _timeout -= Time.deltaTime;
            if (_timeout <= 0f)
            {
                _timeout = 0f;

                switch (_state)
                {
                    //今の状態がNoneならbreak
                    case States.None:
                        break;

                    //今の状態がScanならデバイスをスキャンしてConnectへ遷移（もしデバイスがRSSIOnlyならScanRSSIに遷移）
                    case States.Scan:
                        statusString = "Scanning for " + DeviceName;

                        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
                        {
                            // if your device does not advertise the rssi and manufacturer specific data
                            // then you must use this callback because the next callback only gets called
                            // if you have manufacturer specific data

                            if (!_rssiOnly)
                            {
                                if (name.Contains(DeviceName))
                                {
                                    statusString = "Found " + name;

                                    // found a device with the name we want
                                    // this example does not deal with finding more than one
                                    _deviceAddress = address;
                                    SetState(States.Connect, 0.5f);
                                }
                            }

                        }, (address, name, rssi, bytes) =>
                        {

                            // use this one if the device responses with manufacturer specific data and the rssi

                            if (name.Contains(DeviceName))
                            {
                                statusString = "Found " + name;

                                if (_rssiOnly)
                                {
                                    _rssi = rssi;
                                }
                                else
                                {
                                    // found a device with the name we want
                                    // this example does not deal with finding more than one
                                    _deviceAddress = address;
                                    SetState(States.Connect, 0.5f);
                                }
                            }

                        }, _rssiOnly); // this last setting allows RFduino to send RSSI without having manufacturer data

                        if (_rssiOnly)
                            SetState(States.ScanRSSI, 0.5f);
                        break;

                    //今の状態がScanRSSIならbreak（RSSI onlyには何もしない）
                    case States.ScanRSSI:
                        break;

                    //今の状態がReadRSSIならRSSIをreadして画面に表示する
                    case States.ReadRSSI:
                        statusString = $"Call Read RSSI";
                        BluetoothLEHardwareInterface.ReadRSSI(_deviceAddress, (address, rssi) =>
                        {
                            statusString = $"Read RSSI: {rssi}";
                        });

                        SetState(States.ReadRSSI, 2f);
                        break;

                    //今の状態がConnectならPeriheralに接続を試みて，接続したらデバイス名を取得してRequestMTUに遷移．もしDisconnectされたらScanに遷移して再接続を待つ．
                    case States.Connect:
                        // set these flags
                        _foundSubscribeID = false;
                        _foundWriteID = false;

                        // note that the first parameter is the address, not the name. I have not fixed this because
                        // of backwards compatiblity.
                        // also note that I am note using the first 2 callbacks. If you are not looking for specific characteristics you can use one of
                        // the first 2, but keep in mind that the device will enumerate everything and so you will want to have a timeout
                        // large enough that it will be finished enumerating before you try to subscribe or do any other operations.
                        //指定された周辺機器への接続を試みます。 
                        //接続が成功すると、connectActionは接続先の周辺機器の名前で呼び出されます。 
                        //接続されると、周辺機器がサポートする各サービスに対してserviceActionが呼び出されます。 
                        //各サービスは列挙され、各サービスによってサポートされている特性は、characterActionコールバックを呼び出すことによって示されます。
                        //  disconnectActionのデフォルト値は、下位互換性のためにnullです。 このパラメータにコールバックを指定した場合は、接続された機器が切断されるたびに呼び出されます。 
                        //以下のDisconnectPeripheralコマンドにもコールバックを指定すると、両方のコールバックが呼び出されることに注意してください。
                        BluetoothLEHardwareInterface.ConnectToPeripheral (_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) => {
                            if (IsEqual (serviceUUID, ServiceUUID))
                            {
                                _foundSubscribeID = _foundSubscribeID || IsEqual (characteristicUUID, SubscribeCharacteristic);
                                _foundWriteID = _foundWriteID ;
                                // if we have found both characteristics that we are waiting for
                                // set the state. make sure there is enough timeout that if the
                                // device is still enumerating other characteristics it finishes
                                // before we try to subscribe
                                //if (_foundSubscribeID && _foundWriteID)
                                if (_foundSubscribeID)
                                {
                                    _connected = true;
                                    SetState (States.RequestMTU, 2f);
                                }
                            }}, (disconnectDeviceAddress) =>
                            {
                                // if this is called, the device identitied with the passed in parameter
                                // just disconnected.
                                // You can start scanning again by setting the state to Scan like so:
                                _connected = false;
                                SetState(States.Scan, 0.1f);
                            });
                            break;

                    //今の状態がRequestMTUならMTUを指定（パケットサイズを変更）してSubscribeに遷移
                    case States.RequestMTU:
                        statusString = "Requesting MTU";

                        BluetoothLEHardwareInterface.RequestMtu(_deviceAddress, 185, (address, newMTU) =>
                        {
                            statusString = "MTU set to " + newMTU.ToString();
                            SendByte("Bluetooth Connected");

                            SetState(States.Subscribe, 0.1f);
                        });
                        break;

                    //今の状態がSubscribeならPeriheral（接続先）のNotify（値の変更）を受信して変更値を取得する
                    case States.Subscribe:
                        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress (_deviceAddress, ServiceUUID, SubscribeCharacteristic, null, (address, characteristicUUID, bytes) => {

                        // we don't have a great way to set the state other than waiting until we actually got
                        // some data back. For this demo with the rfduino that means pressing the button
                        // on the rfduino at least once before the GUI will update.
                        _state = States.None;

                        // we received some data from the device
                        _dataBytes = bytes;
                        });
                        break;

                    //今の状態がUnsubscribeならDisconnectに遷移
                    case States.Unsubscribe:
                        BluetoothLEHardwareInterface.UnSubscribeCharacteristic(_deviceAddress, ServiceUUID, WriteCharacteristic, null);
                        SetState(States.Disconnect, 4f);
                        break;

                    //今の状態がDisconnectならDeInitializeしてNoneに遷移
                    case States.Disconnect:
                        statusString = "Commanded disconnect.";

                        if (_connected)
                        {
                            BluetoothLEHardwareInterface.DisconnectPeripheral(_deviceAddress, (address) =>
                            {
                                statusString = "Device disconnected";
                                BluetoothLEHardwareInterface.DeInitialize(() =>
                                {
                                    _connected = false;
                                    _state = States.None;
                                });
                            });
                        }
                        else
                        {
                            BluetoothLEHardwareInterface.DeInitialize(() =>
                            {
                                _state = States.None;
                            });
                        }
                        break;
                }
            }
        }
    }


    string FullUUID(string uuid)
    {
        string fullUUID = uuid;
        if (fullUUID.Length == 4)
            fullUUID = "0000" + uuid + "-0000-1000-8000-00805f9b34fb";

        return fullUUID;
    }

    bool IsEqual(string uuid1, string uuid2)
    {
        if (uuid1.Length == 4)
            uuid1 = FullUUID(uuid1);
        if (uuid2.Length == 4)
            uuid2 = FullUUID(uuid2);

        return (uuid1.ToUpper().Equals(uuid2.ToUpper()));
    }

    public void SendByte(string value)
    {
        // byte[] data = new byte[] { value };
        byte[] data = System.Text.Encoding.ASCII.GetBytes(value);

        BluetoothLEHardwareInterface.WriteCharacteristic(_deviceAddress, ServiceUUID, SubscribeCharacteristic, data, data.Length, true, (characteristicUUID) =>
        {

            BluetoothLEHardwareInterface.Log("Write Succeeded");
        });
    }
}

/*

COPY FROM BELOW THIS LINE >>>
// This sketch is a companion to the Bluetooth LE for iOS, tvOS and Android plugin for Unity3D.
// It is the hardware side of the StartingExample.

// The URL to the asset on the asset store is:
// https://assetstore.unity.com/packages/tools/network/bluetooth-le-for-ios-tvos-and-android-26661

// This sketch simply advertises as ledbtn and has a single service with 2 characteristics.
// The SubscribeCharacteristic characteristic is used to turn the LED on and off. Writing 0 turns it off and 1 turns it on.
// The WriteCharacteristic characteristic can be read or subscribe to. When the button is down the characteristic
// value is 1. When the button is up the value is 0.

// This sketch was written for the Hiletgo ESP32 dev board found here:
// https://www.amazon.com/HiLetgo-ESP-WROOM-32-Development-Microcontroller-Integrated/dp/B0718T232Z/ref=sr_1_3?keywords=hiletgo&qid=1570209988&sr=8-3

// Many other ESP32 devices will work fine.

#include "BLEDevice.h"
#include "BLE2902.h"

// pin 2 on the Hiletgo
// (can be turned on/off from the iPhone app)
const uint32_t led = 2;

// pin 5 on the RGB shield is button 1
// (button press will be shown on the iPhone app)
const uint32_t button = 0;

static BLEUUID serviceUUID("A9E90000-194C-4523-A473-5FDF36AA4D20");
static BLEUUID SubscribeCharacteristic("A9E90001-194C-4523-A473-5FDF36AA4D20");
static BLEUUID WriteCharacteristic("A9E90002-194C-4523-A473-5FDF36AA4D20");

bool deviceConnected = false;
bool oldDeviceConnected = false;

bool lastButtonState = false;

BLEServer* pServer = 0;
BLECharacteristic* pCharacteristicCommand = 0;
BLECharacteristic* pCharacteristicData = 0;

class BTServerCallbacks : public BLEServerCallbacks
{
    void onConnect(BLEServer* pServer)
{
    Serial.println("Connected...");
    deviceConnected = true;
};

void onDisconnect(BLEServer* pServer)
{
    Serial.println("Disconnected...");
    deviceConnected = false;

    // don't leave the led on if they disconnect
    digitalWrite(led, LOW);
}
};


class BTCallbacks : public BLECharacteristicCallbacks
{
    void onRead(BLECharacteristic* pCharacteristic)
{
}

void onWrite(BLECharacteristic* pCharacteristic)
{
    uint8_t* data = pCharacteristic->getData();
    int len = pCharacteristic->getValue().empty() ? 0 : pCharacteristic->getValue().length();

    if (len > 0)
    {
        // if the first byte is 0x01 / on / true
        if (data[0] == 0x01)
            digitalWrite(led, HIGH);
        else
            digitalWrite(led, LOW);
    }
}
};

// debounce time (in ms)
int debounce_time = 10;

// maximum debounce timeout (in ms)
int debounce_timeout = 100;

void BluetoothStartAdvertising()
{
    if (pServer != 0)
    {
        BLEAdvertising* pAdvertising = pServer->getAdvertising();
        pAdvertising->start();
    }
}

void BluetoothStopAdvertising()
{
    if (pServer != 0)
    {
        BLEAdvertising* pAdvertising = pServer->getAdvertising();
        pAdvertising->stop();
    }
}

void setup()
{
    Serial.begin(115200);

    // led turned on/off from the iPhone app
    pinMode(led, OUTPUT);

    // button press will be shown on the iPhone app)
    pinMode(button, INPUT);

    BLEDevice::init("ledbtn");
    // BLEDevice::setCustomGattsHandler(my_gatts_event_handler);
    // BLEDevice::setCustomGattcHandler(my_gattc_event_handler);

    pServer = BLEDevice::createServer();
    BLEService* pService = pServer->createService(serviceUUID);
    pServer->setCallbacks(new BTServerCallbacks());

    pCharacteristicCommand = pService->createCharacteristic(
        WriteCharacteristic,
        BLECharacteristic::PROPERTY_READ |
            BLECharacteristic::PROPERTY_WRITE |
            BLECharacteristic::PROPERTY_NOTIFY);

    pCharacteristicCommand->setCallbacks(new BTCallbacks());
    pCharacteristicCommand->setValue("");
    pCharacteristicCommand->addDescriptor(new BLE2902());

    pCharacteristicData = pService->createCharacteristic(
        SubscribeCharacteristic,
        BLECharacteristic::PROPERTY_READ |
            BLECharacteristic::PROPERTY_WRITE |
            BLECharacteristic::PROPERTY_NOTIFY);

    pCharacteristicData->setCallbacks(new BTCallbacks());
    pCharacteristicData->setValue("");
    pCharacteristicData->addDescriptor(new BLE2902());

    pService->start();
    BluetoothStartAdvertising();
}

void loop()
{
    if (pServer != 0)
    {
        // disconnecting
        if (!deviceConnected && oldDeviceConnected)
        {
            delay(500);                  // give the bluetooth stack the chance to get things ready
            pServer->startAdvertising(); // restart advertising
            Serial.println("start advertising");
            oldDeviceConnected = deviceConnected;
        }

        // connecting
        if (deviceConnected && !oldDeviceConnected)
        {
            oldDeviceConnected = deviceConnected;
        }

        uint8_t buttonState = digitalRead(button);

        if (deviceConnected && pCharacteristicCommand != 0 && buttonState != lastButtonState)
        {
            lastButtonState = buttonState;

            uint8_t packet[1];
            packet[0] = buttonState == HIGH ? 0x00 : 0x01;
            pCharacteristicCommand->setValue(packet, 1);
            pCharacteristicCommand->notify();
        }
    }
}

<<< COPY TO ABOVE THIS LINE
*/
