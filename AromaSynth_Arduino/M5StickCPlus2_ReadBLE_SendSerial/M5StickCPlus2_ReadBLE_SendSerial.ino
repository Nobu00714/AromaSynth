#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>
#include <BLE2902.h>
#include <M5StickCPlus2.h>

#define SERVICE_UUID        "00002220-0000-1000-8000-00805F9B34FB"
#define CHARACTERISTIC_UUID "00002221-0000-1000-8000-00805F9B34FB"
#define CHARACTERISTIC_UUID_RX  "00002222-0000-1000-8000-00805F9B34FB"

String BLEConnectName = "AromaSynthBLE";

BLEServer * pServer;
BLECharacteristic * pCharacteristic;
BLEAdvertising * pAdvertising;
BLE2902 *descriptor_2902 = NULL;

bool deviceConnected = false;
bool oldDeviceConnected = false;
uint32_t value = 0;

//サーバーのコールバック関数
class MyServerCallbacks : public BLEServerCallbacks 
{
  void onConnect(BLEServer *pServer) 
  {
    deviceConnected = true;
  };

  void onDisconnect(BLEServer *pServer) 
  {
    deviceConnected = false;
  }
};

//writeされたときのコールバック関数
class MyCallbacks: public BLECharacteristicCallbacks 
{
  //Writeを受け取る処理
  void onWrite(BLECharacteristic *pCharacteristic)
  {
    // BLEの値を受け取る
    std::string rxData = pCharacteristic->getValue();
    String receivedData = String(rxData.c_str());

    //最初の接続時の処理
    if(receivedData.equals("Bluetooth Connected"))
    {
      M5.Lcd.println("BLE Connected");
    }
    //接続以外の受信時の処理
    else
    {
      // BLEの文字列をそのままArduinoMegaへ送信
      Serial2.println(receivedData);
      M5.Lcd.println(receivedData);
      M5.Lcd.println("SendToArduinoMEGA");
    }
  }
};

void setup() {
  M5.begin();                 // 本体初期化
  M5.Lcd.begin();             // 画面初期化
  M5.Lcd.setRotation(0);      // 画面向き設定（0～3で設定、4～7は反転)※初期値は1
  M5.Lcd.setTextWrap(true);   // 画面端での改行の有無（true:有り[初期値], false:無し）※print関数のみ有効
  uint16_t background_color=0;
  background_color = M5.Lcd.color565(0,0,0);
  M5.Lcd.fillScreen(background_color);   // 画面の背景色をそれぞれの色に
  uint16_t text_color=0;
  text_color = M5.Lcd.color565(255,255,255);
  M5.Lcd.setTextColor(text_color, background_color);
  M5.Lcd.setTextSize(1);
  M5.Lcd.setCursor(0, 0);
  M5.Lcd.println(BLEConnectName);

  // Serial.begin(9600);
  Serial2.begin(9600, SERIAL_8N1, 0, 26);

  // Create the BLE Device
  BLEDevice::init(BLEConnectName.c_str());

  // Create the BLE Server
  pServer = BLEDevice::createServer();
  pServer->setCallbacks(new MyServerCallbacks());

  // Create the BLE Service
  BLEService *pService = pServer->createService(SERVICE_UUID);

  // Create a BLE Characteristic
  pCharacteristic = pService->createCharacteristic(
    CHARACTERISTIC_UUID,
    BLECharacteristic::PROPERTY_READ | BLECharacteristic::PROPERTY_WRITE | BLECharacteristic::PROPERTY_NOTIFY | BLECharacteristic::PROPERTY_INDICATE
  );
  
  // WriteされたときのコールバックをMyCallbacks（）に設定
  pCharacteristic->setCallbacks(new MyCallbacks());
  BLECharacteristic * pRxCharacteristic = pService->createCharacteristic(
    CHARACTERISTIC_UUID_RX,
    BLECharacteristic::PROPERTY_WRITE
  );
  pRxCharacteristic->setCallbacks(new MyCallbacks());
  
  // Creates BLE Descriptor 0x2902: Client Characteristic Configuration Descriptor (CCCD)
  pCharacteristic->addDescriptor(new BLE2902());

  // Start the service
  pService->start();

  // Start advertising
  pAdvertising = BLEDevice::getAdvertising();
  // BLEAdvertising *pAdvertising = BLEDevice::getAdvertising();
  pAdvertising->addServiceUUID(SERVICE_UUID);
  pAdvertising->setScanResponse(false);
  pAdvertising->setMinPreferred(0x0);  // set value to 0x00 to not advertise this parameter
  BLEDevice::startAdvertising();
  
  // Serial.println("Waiting a client connection to notify...");
}

void loop() {
// notify changed value 投げたい値をvalueに格納してnotify()するとBLEで送信できる
  if (deviceConnected) {
    value = M5.Power.getBatteryLevel();
    //valueをセットしてNotifyする
    pCharacteristic->setValue((uint8_t *)&value, 4);
    pCharacteristic->notify();
    delay(500);
  }
  
  // disconnecting
  if (!deviceConnected && oldDeviceConnected) {
    delay(500);                   // give the bluetooth stack the chance to get things ready
    pServer->startAdvertising();  // restart advertising
    // Serial.println("start advertising");
    oldDeviceConnected = deviceConnected;
  }
  
  // connecting
  if (deviceConnected && !oldDeviceConnected) {
    // do stuff here on connecting
    oldDeviceConnected = deviceConnected;
  }
}

