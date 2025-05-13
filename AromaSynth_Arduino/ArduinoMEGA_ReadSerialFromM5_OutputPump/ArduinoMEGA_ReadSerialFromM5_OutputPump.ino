// ポンプ制御用の配列
int motorNum[2][20];

const int PWM_motor = 3;

int data[30];
int UT[20];  //それぞれのポンプの停止単位時間

int NUM_PUMPS = 20;

void setup() {
  // PCデバッグ用のシリアル通信
  // Serial.begin(9600);
  // M5との通信用のシリアル通信
  Serial1.begin(9600);

  // ポンプの番号を定義
  setMotorNum();

  // ポンプの制御ピンをアウトプットに指定
  for (int i = 0; i < 2; i++) {
    for (int j = 0; j < 20; j++) {
      pinMode(motorNum[i][j], OUTPUT);
    }
  }
  // ポンプの停止単位時間を指定
  for (int i = 0; i < 20; i++) {
    UT[i] = 50;
  }
}

void setMotorNum() {
  //ポンプseat1、ブレボ上はArudino逆
  motorNum[0][0] = 22;//下段右から4
  motorNum[1][0] = 23;
  motorNum[0][1] = 24;//下段右から3
  motorNum[1][1] = 25;
  motorNum[0][2] = 26;//上段右から1
  motorNum[1][2] = 27;
  motorNum[0][3] = 28;//右上2
  motorNum[1][3] = 29;
  motorNum[0][4] = 30;//右上3
  motorNum[1][4] = 31;

  //ポンプseat2
  motorNum[0][5] = 32;//右上4
  motorNum[1][5] = 33;
  motorNum[0][6] = 34;//右上5
  motorNum[1][6] = 35;
  motorNum[0][7] = 36;//右上6
  motorNum[1][7] = 37;
  motorNum[0][8] = 38;//右上7
  motorNum[1][8] = 39;
  motorNum[0][9] = 40;//右上8
  motorNum[1][9] = 41;
  motorNum[0][10] = 42;
  motorNum[1][10] = 43;
  motorNum[0][11] = 44;
  motorNum[1][11] = 45;
  motorNum[0][12] = 46;
  motorNum[1][12] = 47;
  motorNum[0][13] = 48;
  motorNum[1][13] = 49;
  motorNum[0][14] = 50;//15 //下段右から2
  motorNum[1][14] = 51;
  motorNum[0][15] = 52;//16 //下段右から1
  motorNum[1][15] = 53;
  /*
    motorNum[0][16] = A8;
    motorNum[1][16] = A9;
    motorNum[0][17] = A10;
    motorNum[1][17] = A11;
    motorNum[0][18] = A12;
    motorNum[1][18] = A13;
    motorNum[0][19] = A14;
    motorNum[1][19] = A15;
  */
}

void loop() {
  //Output();
  if (Serial1.available() > 0) {
    String str = Serial1.readStringUntil('\n');  //受け取ったデータをstrにする
    // Serial.println("Received: " + str);          //デバッグ用にPCで表示
    stringToIntValues(str, data, ',');           //カンマ区切りのデータを分割してint型としてdataリストに保存
    //20個目のやつがデバッグ用か否かの判定
    if (data[20] == 0)  //通常の出力モード
    {
      Outputs_Kasahara();
    }
    else  //デバッグモードの時
    {
      setUT();
    }
  }
}

void setUT()  //単位時間を設定します
{
  for (int i = 0; i < 20; i++) {
    UT[i] = data[i];
  }
}

void Outputs()  //バッテリーが異なるものを同時に出すバージョン
{
  analogWrite(PWM_motor, 255);
  Outputs_block(0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19);
  // Serial.println((String) "Sending");
  // delay(100);
}

void Outputs_Kasahara()
{
  int splittedData[NUM_PUMPS][2] = {};    //0にはポンプ番号を入れて、1には駆動時間を入れる
  for(int i=0; i<NUM_PUMPS; i++){
    splittedData[i][0] = i;
    splittedData[i][1] = data[i];
  }

  //駆動時間の値を昇順にソート
  int pumpDriveTime_Array[NUM_PUMPS] = {};
  int pumpOrder_Array[NUM_PUMPS] = {};
  for(int i=0; i<NUM_PUMPS; i++){
    int maxPumpNum; //最大ポンプ駆動時間が必要なポンプ番号を格納する変数
    int maxPumpDriveTime = -1; //最大ポンプ駆動時間を格納する変数
    int maxID; //配列のIDを格納する変数
    //最大値を探索
    for(int j=0; j<NUM_PUMPS; j++){
      if(splittedData[j][1]>=maxPumpDriveTime){
        maxPumpNum = splittedData[j][0];
        maxPumpDriveTime = splittedData[j][1];
        maxID = j;
      }
    }
    //最大値とそのポンプ番号を格納して削除
    pumpDriveTime_Array[NUM_PUMPS-1-i] = maxPumpDriveTime;
    pumpOrder_Array[NUM_PUMPS-1-i] = maxPumpNum;
    splittedData[maxID][1] = -1; 
  }

  //各ポンプの停止までのdelayを計算
  int delay_Array[NUM_PUMPS] = {};
  delay_Array[0] = pumpDriveTime_Array[0];
  for(int i=1; i<NUM_PUMPS; i++){
    delay_Array[i] = pumpDriveTime_Array[i] - pumpDriveTime_Array[i-1];
  }

  //全ポンプをONにして，delay_Arrayに従って順番に停止
  for(int i=0; i<NUM_PUMPS; i++){
    pump_ON(i);
  }
  for(int i=0; i<NUM_PUMPS; i++){
    delay(delay_Array[i]);
    pump_OFF(pumpOrder_Array[i]);
  }
}

void Outputs_block(int a, int b, int c, int d, int e, int f, int g, int h, int i, int j, int k
                   , int l, int m, int n, int o, int p, int q, int r, int s, int t) {
  int datanum[20] = {a, b, c, d, e, f, g, h, i, j, k, l, m, n, o, p, q, r, s, t};
  int time[20];
  int maxTime = 0;

  // 出力ONと最大出力時間を取得
  for (int i = 0; i < 20; i++) {
    time[i] = data[datanum[i]];  // ミリ秒単位
    pump_ON(datanum[i]);
    // データの中で最大時間を保存
    if (maxTime < time[i])
    {
      maxTime = time[i];
    }
  }

  // 1ms単位でカウントアップしてOFFにする
  for (int t = 0; t <= maxTime; t++) {
    // delay(10);
    for (int i = 0; i < 20; i++) {
      if (t >= time[i]) pump_OFF(datanum[i]);
    }
  }
}

// ポンプをオンにする関数
void pump_ON(int num) {
  digitalWrite(motorNum[0][num], HIGH);
  digitalWrite(motorNum[1][num], LOW);
}
// ポンプをオフにする関数
void pump_OFF(int num) {
  digitalWrite(motorNum[0][num], LOW);
  digitalWrite(motorNum[1][num], LOW);
}

// String型のカンマ区切り文字列をint型配列に分解する関数
void stringToIntValues(String str, int value[], char delim) {
  int k = 0;
  int j = 0;
  char text[30];

  for (int i = 0; i <= str.length(); i++) {
    char c = str.charAt(i);
    if (c == delim || i == str.length()) {
      text[k] = '\0';
      value[j] = atoi(text);
      j++;
      k = 0;
    } else {
      text[k] = c;
      k++;
    }
  }
}
