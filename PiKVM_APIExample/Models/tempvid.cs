//using PiKVM_APIExample.Models;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Net.WebSockets;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Windows.Threading;

//namespace PiKVM_APIExample.Modelsa
//{
//    using Models_No;
//    internal class Pikvm_ApiVideoStream
//    {
//        private const string USERNAME_PROMPT = "X-KVMD-User";
//        private const string PASSWORD_PROMPT = "X-KVMD-Passwd";
//        //private const string USERNAME = "admin";
//        private const string USERNAME = "admin";
//        private const string PASSWORD = "admin";
//        private const string URI_PATH = "wss://192.168.1.183/api/media/ws";
//        private const string STARTVIDEOSTREAM = "{\"event_type\": \"start\", \"event\": {\"type\": \"video\", \"format\": \"h264\"}}";
//        private const string FFMPEG_PATH = @"C:\ffmpeg\ffmpeg-7.1.1-full_build-shared\ffmpeg-7.1.1-full_build-shared\bin\ffmpeg.exe";
//        private const string FFMPEG_ARGS = "-f h264 -i pipe:0 -vf fps=10 -f image2pipe -vcodec mjpeg pipe:1";
//        //-f h264 -i pipe:0 -vf fps=10 -f image2pipe -vcodec mjpeg pipe:1
//        //-hwaccel d3d11va -f h264 -i pipe:0 -vf fps=10 -f image2pipe -vcodec mjpeg pipe:1
//        //-init_hw_device vulkan=vulkan0 -filter_hw_device vulkan0 -f h264 -i pipe:0 -vf "hwupload,format=vulkan,fps=10" -f image2pipe -vcodec mjpeg pipe:1

//        private Task<bool> _ConnectWss;
//        private CancellationTokenSource _Cts_ClientComm;

//        private ClientWebSocket _ClientWSS;
//        private Uri _WsUri;
//        private Process _FfmpegProcess;
//        private StreamWriter _FfmpegInput;
//        private StreamReader _FfmpegError;
//        private StreamReader _FfmpegOutput;

//        public event EventHandler<LogMessage> OnLogEvent;
//        public event EventHandler<byte[]> OnImageRecieved;

//        public Pikvm_ApiVideoStream()
//        {
//            _Cts_ClientComm = new CancellationTokenSource();
//            _ClientWSS = new ClientWebSocket();
//            _ClientWSS.Options.SetRequestHeader(USERNAME_PROMPT, USERNAME);
//            _ClientWSS.Options.SetRequestHeader(PASSWORD_PROMPT, PASSWORD);

//            _WsUri = new Uri(URI_PATH);

//            // Ignore SSL certificate validation errors
//            System.Net.ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, errors) => true;
//            //_ConnectWss = ConnectAsync();
//        }

//        public bool Connect()
//        {
//            Task<bool> connectWss = ConnectAsync();
//            connectWss.Wait();
//            return connectWss.Result;
//        }
//        public bool Disconnect()
//        {
//            Task<bool> disconnectWss = DisconnectAsync();
//            disconnectWss.Wait();
//            return disconnectWss.Result;
//        }
//        public bool Reconnect()
//        {
//            Task<bool> reconnectWss = ReconnectAsync();
//            reconnectWss.Wait();
//            return reconnectWss.Result;
//        }
//        public async Task<bool> ConnectAsync()
//        {
//            int cnt = 1;

//            PushLogEvent(new LogMessage() { LogLevel = LogLevel.Information, Message = $"Attempting connection to {_WsUri}", TimeStamp = DateTime.Now });
//            while (_ClientWSS.State != WebSocketState.Open && !_Cts_ClientComm.IsCancellationRequested)
//            {
//                try
//                {
//                    PushLogEvent(new LogMessage() { LogLevel = LogLevel.Debug, Message = $"Attempting connection count ({cnt}) to {_WsUri}", TimeStamp = DateTime.Now });

//                    await _ClientWSS.ConnectAsync(_WsUri, _Cts_ClientComm.Token);
//                }
//                catch (OperationCanceledException exCanceled)
//                {
//                    PushLogEvent(new LogMessage() { LogLevel = LogLevel.Error, Message = exCanceled.Message, TimeStamp = DateTime.Now });
//                    return false;
//                }
//                catch (Exception ex)
//                {
//                    PushLogEvent(new LogMessage() { LogLevel = LogLevel.Error, Message = ex.Message, TimeStamp = DateTime.Now });
//                    return false;
//                }
//                cnt++;
//            }

//            PushLogEvent(new LogMessage() { LogLevel = LogLevel.Information, Message = $"Successfully connected to {_WsUri}", TimeStamp = DateTime.Now });
//            return true;
//        }
//        public async Task<bool> DisconnectAsync()
//        {
//            await _ClientWSS.CloseAsync(WebSocketCloseStatus.NormalClosure, string.Empty, _Cts_ClientComm.Token);
//            return true;
//        }
//        public async Task<bool> ReconnectAsync()
//        {
//            bool disConnSuccess = await DisconnectAsync();
//            return disConnSuccess && await ConnectAsync();
//        }

        
//        private void PushLogEvent(LogMessage logMessage)
//        {
//            if (AppTypeDetector.IsWpfApp())
//                Dispatcher.CurrentDispatcher.Invoke(() => OnLogEvent?.Invoke(this, logMessage));
//            else
//                OnLogEvent?.Invoke(this, logMessage);
//        }
//    }
//}

