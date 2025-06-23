//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Net.Http;
//using System.Net.Security;
//using System.Security.Cryptography.X509Certificates;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Threading;

//namespace PiKVM_APIExample.Models_No
//{
//    internal class Pikvm_HttpClient
//    {
//        private const string USERNAME_PROMPT = "X-KVMD-User";
//        private const string PASSWORD_PROMPT = "X-KVMD-Passwd";
//        private const string USERNAME = "admin";
//        private const string PASSWORD = "admin";
//        private const string URI_PATH = "https://192.168.1.183/";

//        private CancellationTokenSource _Cts_ReadingLog;

//        public event EventHandler<LogMessage> OnLogEvent;
//        public event EventHandler<LogMessage> OnHttpLogEvent;
//        private HttpClient _Client;

//        public Pikvm_HttpClient(string userPrompt = USERNAME_PROMPT, string userName = USERNAME, string passwordPrompt = PASSWORD_PROMPT, string passWord = PASSWORD)
//        {
//            _Cts_ReadingLog = new CancellationTokenSource();

//            HttpClientHandler handler = new HttpClientHandler();
//            handler.ServerCertificateCustomValidationCallback = ServerCertificateCustomValidationCallback_Handler;

//            _Client = new HttpClient(handler);
//            _Client.DefaultRequestHeaders.Add(userPrompt, userName);
//            _Client.DefaultRequestHeaders.Add(passwordPrompt, passWord);
//            _Client.BaseAddress = new Uri(URI_PATH);
//            //ReadLogStream();
//        }
//        private bool ServerCertificateCustomValidationCallback_Handler(HttpRequestMessage sender, X509Certificate2 cert, X509Chain chain, SslPolicyErrors sslPolicyErrors)
//        {
//            // Check specific certificate errors if necessary
//            if (sslPolicyErrors == SslPolicyErrors.None)
//                return true;

//            // Log the errors or handle specific issues here
//            if (sslPolicyErrors.HasFlag(SslPolicyErrors.RemoteCertificateNotAvailable))
//                return true; // Allow connections with no certificate

//            return true; // Only allow connections with no errors
//        }
//        public void EnableHttpLogStreamAsync()
//        {
//            ReadLogStream(_Cts_ReadingLog.Token);
//        }
//        public string GetResponse(string url)
//        {
//            try
//            {
//                PushLogEvent(new LogMessage() { LogLevel = LogLevel.Debug, Message = $"Initializing Get request to {url} ", TimeStamp = DateTime.Now });
//                HttpResponseMessage response = _Client.GetAsync(url).Result;
//                HttpResponseMessage success = response.EnsureSuccessStatusCode();
//                PushLogEvent(new LogMessage() { LogLevel = LogLevel.Debug, Message = $"Completed Get request to {url}", TimeStamp = DateTime.Now });
//                return response.Content.ReadAsStringAsync().Result;
//            }
//            catch (HttpRequestException e)
//            {
//                // Handle exception
//                return e.Message;
//            }
//        }
//        public async Task<string> PostRequest(string url, string jsonData)
//        {
//            try
//            {
//                if (jsonData == null)
//                    jsonData = string.Empty;

//                PushLogEvent(new LogMessage() { LogLevel = LogLevel.Debug, Message = $"Initializing POST request to {url} with data: {jsonData}", TimeStamp = DateTime.Now });

//                StringContent content = null;
//                if (!string.IsNullOrEmpty(jsonData))
//                    content = new StringContent(jsonData, Encoding.UTF8, "application/json");

                
//                HttpResponseMessage response = _Client.PostAsync(url, content).Result;
//                // Check if the response is successful
//                response.EnsureSuccessStatusCode();
//                PushLogEvent(new LogMessage() { LogLevel = LogLevel.Debug, Message = $"Completed POST request to {url} with data: {jsonData}", TimeStamp = DateTime.Now });
//                return response.Content.ReadAsStringAsync().Result;
//            }
//            catch (HttpRequestException e)
//            {
//                // Handle exception
//                return "Invalid command";
//            }
//        }
//        private async Task ReadLogStream(CancellationToken cancellationToken)
//        {

//            PushLogEvent(new LogMessage() { LogLevel = LogLevel.Information, Message = "Starting to read log stream...", TimeStamp = DateTime.Now });
//            Task<Stream> readStream = _Client.GetStreamAsync("/api/log?follow=1");

//            using (Stream stream = await readStream)
//            {
//                using (StreamReader reader = new StreamReader(stream))
//                {
//                    try
//                    {
//                        string line;
//                        while ((line = await reader.ReadLineAsync()) != null)
//                        {
//                            PushHttpLogEvent(new LogMessage() { LogLevel = LogLevel.Debug, Message = line, TimeStamp = DateTime.Now });
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        PushLogEvent(new LogMessage() { LogLevel = LogLevel.Error, Message = $"Error reading log stream: {ex.Message}", TimeStamp = DateTime.Now });
//                    }
//                }
//            }
//            PushLogEvent(new LogMessage() { LogLevel = LogLevel.Information, Message = "Finished reading log stream.", TimeStamp = DateTime.Now });
//        }

//        private void PushLogEvent(LogMessage logMessage)
//        {
//            if (AppTypeDetector.IsWpfApp())
//                Dispatcher.CurrentDispatcher.InvokeAsync(() => OnLogEvent?.Invoke(this, logMessage));
//            else
//                OnLogEvent?.BeginInvoke(this, logMessage, null, null);
//        }
//        private void PushHttpLogEvent(LogMessage logMessage)
//        {
//            if (AppTypeDetector.IsWpfApp())
//                Dispatcher.CurrentDispatcher.InvokeAsync(() => OnHttpLogEvent?.Invoke(this, logMessage));
//            else
//                OnHttpLogEvent?.BeginInvoke(this, logMessage, null, null);
//        }
//    }
//}
