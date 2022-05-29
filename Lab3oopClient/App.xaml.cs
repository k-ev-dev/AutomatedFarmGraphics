using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace Lab3oopClient {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {

        class JsonMethod {
            public string jsonrpc { get; set; }
            public string method { get; set; }
            public int[] _params { get; set; }
            public int id { get; set; }
        }
        class JsonResponce {
            public string jsonrpc { get; set; }
            public int result { get; set; }
            public int id { get; set; }
        }

        class Client {
            // отправитель запроса
            HttpClient requestSender;
            protected string url;

            protected Client(string url) {
                this.url = url;
                requestSender = new();
            }
            // возвращает полное содержимое ответа
            protected string callRemote(string json) {

                HttpMethod httpmethod = new HttpMethod("POST");
                // формирование запроса
                HttpRequestMessage request = new HttpRequestMessage(httpmethod, url);
                request.Content = new StringContent(json);
                request.Content.Headers.Remove("Content-Type");
                request.Content.Headers.Add("Content-Type", "application/json-rpc");
                // отправика запроса и получение результата
                HttpResponseMessage response = requestSender.Send(request);
                // чтение содержимого ответа
                Stream instream = response.Content.ReadAsStream();
                StreamReader reader = new StreamReader(instream);
                string text = reader.ReadToEnd();
                // если произошла ошибка
                if (!response.IsSuccessStatusCode) {
                    // консоль
                    Console.WriteLine(response.StatusCode.ToString());
                    Console.WriteLine("ERROR RESPONSE: " + text);
                    return "error";
                }
                // консоль
                Console.WriteLine("SUCCESSFULL RESPONSE: " + text);
                return text;
            }
            // создание содержимого запроса: настройка метода и параметров внутри запроса, сериализация в соотв. с JsonMethod
            protected string createMethodObject(string method, int[] args) {
                var jsonmethod = new JsonMethod {
                    jsonrpc = "example-version",
                    method = method,
                    _params = args,
                    id = 0
                };
                string json = JsonSerializer.Serialize<JsonMethod>(jsonmethod);
                // консоль
                Console.WriteLine("REQUEST SENDED: " + json);
                return json;
            }

            // десериализация результата ответ
            protected int parseResult(string json) {
                JsonResponce? jsonresponse = JsonSerializer.Deserialize<JsonResponce>(json);
                return jsonresponse.result;
            }

        }

        class JsonRPCClient : Client {
            public JsonRPCClient(string url) : base(url) { }

            // созадние метода запроса, отправка запроса, получение ответа, парсинг ответа
            // возвращает результат
            public int sum(int i, int j) {
                int[] args = { i, j };
                string json = createMethodObject("sum", args);
                json = callRemote(json);
                return parseResult(json);
            }

            // !!! СЮДА ДОБАВЛЯЕМ МЕТОДЫ ЗАПРОСА КЛИЕНТА
        }
    }
}
