using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;

public class GPTTasteEstimator : MonoBehaviour
{
    // OpenAIのAPIキーをここに貼り付けます
    public string apiKey = "";
    private string apiUrl = "https://api.openai.com/v1/chat/completions";
    // answer変数に，GPTからの返答を格納（塩味，甘味，酸味，旨味，苦味）
    public string answer;
    // public Text answerPanel;
    // public ChartDrawer chartPentagon;
    public TasteEqualizer tasteEqualizer;
    public float[] tasteData = new float[5];

    public float[] taste_substance_in_bottles = {0.492f, 2.624f, 1.312f, 1.64f, 0.656f};

    public float pump_ms_unit = 700f;

    public GameObject GO_Loading;

    // GPTに味を推定させる関数
    public void textTasteEstimation(string userMessage)
    {
        StartCoroutine(SendTextRequest(userMessage));
    }

    public void imageTasteEstimation(string base64Image)
    {
        StartCoroutine(SendImageRequest("Estimate the taste of the dish in this image and answer with five numbers.", base64Image));
    }

    // APIにTextのPOSTリクエストを送信するコルーチン
    private IEnumerator SendTextRequest(string userMessage)
    {
        // リクエストのペイロードを作成
        var requestBody = new
        {
            model = "gpt-4o", // 使用するGPTモデル
            messages = new[]
            {
                new { role = "system", content = "You are an excellent chef, and you are a professional who expresses the salt, sweetener, lactic acid, ajinomoto (umami), and potassium carbonate (bitterness)). The concentration of each solution is 9.0% salty, 41.7% sweet, 21% sour, 25% umami, and 6.3% bitter.  You are a professional who can recreate the taste of a dish by adding the five taste substances listed above to 1000 ml of water. Write the number of grams of each of taste substances, separated by commas. You can response by only 5 specific numbers. Do not use any other words. For example, the receipe of strawberry milk becomes 0,162.5,8,0,0 to the 1000ml of milk. The receipe of clam chowder becomes 6,10.75,0,20,6 to the 1000ml of milk. The receipe of bouillabaisse becomes 5,6.45,6.6,28.8,3.84 to the 1000ml of water." },
                new { role = "user", content = "The Current output is" + tasteEqualizer.tastes_output_substances_data[0] * 250 + "," + tasteEqualizer.tastes_output_substances_data[1] * 250 + "," + tasteEqualizer.tastes_output_substances_data[2] * 250 + "," + tasteEqualizer.tastes_output_substances_data[3] * 250 + "," + tasteEqualizer.tastes_output_substances_data[4] * 250 + "." + userMessage }
            }
        };

        string jsonBody = JsonConvert.SerializeObject(requestBody);

        // リクエストの準備
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            // リクエストを送信
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                GO_Loading.SetActive(false);
            }
            else
            {
                // レスポンスを受け取る
                string jsonResponse = request.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<ChatGPTResponse>(jsonResponse);

                // ChatGPTのレスポンスを処理する
                string reply = response.choices[0].message.content;

                answer = reply;

                //answerを5つの味の値に変換する
                // 回答を各味に分割
                string[] splittedText = answer.Split(',');
                for(int i=0; i<tasteData.Length; i++)
                {
                    //float型に変換
                    tasteData[i] = float.Parse(splittedText[i]);
                }
                tasteEqualizer.updateEqualizer_GPTEstimation();

                Debug.Log("ChatGPTの返信: " + reply);
                GO_Loading.SetActive(false);
            }
        }
    }

    // APIにImageのPOSTリクエストを送信するコルーチン
    private IEnumerator SendImageRequest(string userMessage, string base64Image)
    {
        // リクエストのペイロードを作成
        var requestBody = new
        {
            model = "gpt-4o", // 使用するGPTモデル
            messages = new[]
            {
                new {
                    role = "user",
                    content = new List<object>{
                        new { type = "text", text = "You are an excellent chef, and you are a professional who expresses the salt, sweetener, lactic acid, ajinomoto (umami), and potassium carbonate (bitterness)). You are a professional who can recreate the taste of a dish by adding the five taste substances listed above to 1000 ml of water. Write the number of grams of each of taste substances, separated by commas. You can response by only 5 specific numbers. Do not use any other words. For example, the receipe of strawberry becomes 0,107.5,16,0,0. The receipe of clam chowder becomes 6,10.75,0,20,4." + userMessage },
                        new { type = "image_url", image_url = new { url = $"data:image/jpeg;base64,{base64Image}"} } 
                    }
                }
            }
        };

        string jsonBody = JsonConvert.SerializeObject(requestBody);

        // リクエストの準備
        using (UnityWebRequest request = new UnityWebRequest(apiUrl, "POST"))
        {
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");
            request.SetRequestHeader("Authorization", "Bearer " + apiKey);

            // リクエストを送信
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                answer = request.error;
                // answerPanel.text = answer;
                GO_Loading.SetActive(false);
            }
            else
            {
                // レスポンスを受け取る
                string jsonResponse = request.downloadHandler.text;
                var response = JsonConvert.DeserializeObject<ChatGPTResponse>(jsonResponse);

                // ChatGPTのレスポンスを処理する
                string reply = response.choices[0].message.content;
                answer = reply;

                //answerを5つの味の値に変換する
                // 回答を各味に分割
                string[] splittedText = answer.Split(',');
                for(int i=0; i<tasteData.Length; i++)
                {
                    //float型に変換
                    tasteData[i] = float.Parse(splittedText[i]);
                }
                tasteEqualizer.updateEqualizer_GPTEstimation();

                Debug.Log("ChatGPTの返信: " + reply);
                GO_Loading.SetActive(false);
            }
        }
    }


    // ChatGPTのレスポンスをパースするためのクラス
    [System.Serializable]
    public class ChatGPTResponse
    {
        public Choice[] choices;

        [System.Serializable]
        public class Choice
        {
            public Message message;

            [System.Serializable]
            public class Message
            {
                public string content;
            }
        }
    }
}
