using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Lab3oopServer.BaseHandler {
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

    // начальная обработка запроса/формирование ответа
    class BaseHandler {
        public virtual bool processRequest(HttpListenerContext context) {

            HttpListenerResponse response = context.Response;
            // инициализация ответа
            string responseStr = "HELLO!!!";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseStr); // строка --> массив байтов
            response.ContentLength64 = buffer.Length; // устанавливаем длинну ответа

            Stream output = response.OutputStream;
            // output - ссылка на response.OutputStream, изменяя output, изменяем response.OutputStream
            output.Write(buffer, 0, buffer.Length); // запись данных в выходящий поток: response.OutputStream == output
            output.Close();
            return true;
        }
    }

    // продолжение обработки запроса/формирования ответа
    class JsonRpcHandler : BaseHandler {
        public override bool processRequest(HttpListenerContext context) {
            if (!testRequest(context.Request)) {
                return httpRequestError(context, "HTTP-REQUEST OR CONTENT-TYPE ERROR!!!!");
            }

            string json = callLocal(context.Request);
            HttpListenerResponse response = context.Response;
            string responseStr = json;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseStr);
            response.ContentLength64 = buffer.Length;
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            return true;
        }

        // проверка параметров воходящего запроса
        bool testRequest(HttpListenerRequest request) {
            //foreach (var key in request.Headers.Keys) {
            //    Console.WriteLine(key.ToString());
            //}
            //Console.WriteLine(request.HttpMethod);
            //Console.WriteLine(request.ContentType);
            return (request.HttpMethod == "POST" && request.ContentType == "application/json-rpc");
            // если метод - post, и содержимое - json-rpc, то true
        }

        // настройка ответа в случае ошибки: код ответа, выводящий поток
        bool httpRequestError(HttpListenerContext context, string message) {
            HttpListenerResponse response = context.Response;
            response.StatusCode = 405;
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(message);
            response.ContentLength64 = buffer.Length;
            // обновление ответа
            Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
            return true;
        }

        // извлечение тела запроса
        string getMethodInfo(HttpListenerRequest request) {
            Stream input = request.InputStream;
            StreamReader reader = new(input);
            string json = reader.ReadToEnd();
            return json;
        }

        // метод, возвращающий сереализованный ответ
        string callLocal(HttpListenerRequest request) {
            string json = getMethodInfo(request);
            // консоль
            Console.WriteLine(json);
            // десериализация в соответсвии с указанным классом - JsonMethod
            JsonMethod? jsonmethod = JsonSerializer.Deserialize<JsonMethod>(json);
            // формирование ответа в соответсвии с параметрами запроса: методом и параметрами
            JsonResponce jsonresponce = new JsonResponce {
                jsonrpc = "example-version",
                result = this.call(jsonmethod.method, jsonmethod._params),
                id = 0
            };
            json = JsonSerializer.Serialize<JsonResponce>(jsonresponce);
            return json;
        }
        // метод-недоделанный
        internal virtual int call(string name, int[] args) {
            return 0;
        }
    }

    class MethodHandler : JsonRpcHandler {
        // возможная реализация метода формирования ответа
        internal override int call(string name, int[] args) {
            if (name == "sum")
                return sum(args);
            return 0;
        }

        int sum(int[] args) {
            int s = 0;
            for (int i = 0; i < args.Length; i++) {
                s += args[i];
            }
            return s;
        }

        // !!! СЮДА ДОБАВЛЯЕМ МЕТОДЫ ОБРАБОТКИ ЗАПРОСА ОТ КЛИЕНТА
    }
}
