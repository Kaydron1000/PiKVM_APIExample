using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using PiKVM_APIExample.Models;
using PiKvmLibrary;
using PiKvmLibrary.Configuration;

namespace PiKVM_APIExample.ViewModels
{
    public class Testcls
    {

    }

    internal class MainVM :INotifyPropertyChanged
    {
        private string _GenericMessage;
        public string GenericMessage
        {
            get => _GenericMessage;
            set => SetField(ref _GenericMessage, value);
        }
        private Point _MouseAbsPoint;
        public Point MouseAbsPoint
        {
            get => _MouseAbsPoint;
            set => SetField(ref _MouseAbsPoint, value);
        }
        private int _MouseX;
        public int MouseX
        {
            get => _MouseX;
            set => SetField(ref _MouseX, value);
        }
        private int _MouseY;
        public int MouseY
        {
            get => _MouseY;
            set => SetField(ref _MouseY, value);
        }
        private Pikvm_HttpClient _PikvmHttpClient;
        public Pikvm_HttpClient PikvmHttpClient
        {
            get => _PikvmHttpClient;
            set => SetField(ref _PikvmHttpClient, value);
        }

        private Pikvm_Apiws _ApiWs;
        public Pikvm_Apiws ApiWs
        {
            get => _ApiWs;
            set => SetField(ref _ApiWs, value);
        }

        private Pikvm_Apiws _ApiWs2;
        public Pikvm_Apiws ApiWs2
        {
            get => _ApiWs2;
            set => SetField(ref _ApiWs2, value);
        }

        private Pikvm_ApiVideoStream _VideoStream;
        public Pikvm_ApiVideoStream VideoStream
        {
            get => _VideoStream;
            set => SetField(ref _VideoStream, value);
        }


        private WriteableBitmap _VideoBitmap;
        public WriteableBitmap VideoBitmap
        {
            get => _VideoBitmap;
            set => SetField(ref _VideoBitmap, value);
        }

        private ObservableCollection<LogMessage> _LogMessages;
        public ObservableCollection<LogMessage> LogMessages
        {
            get => _LogMessages;
            set => SetField(ref _LogMessages, value);
        }
        private ICommand _ConnectHttpReqCommand;
        public ICommand ConnectHttpReqCommand
        {
            get => _ConnectHttpReqCommand;
            set => SetField(ref _ConnectHttpReqCommand, value);
        }
        private ICommand _ConnectApiWsCommand;
        public ICommand ConnectApiWsCommand
        {
            get => _ConnectApiWsCommand;
            set => SetField(ref _ConnectApiWsCommand, value);
        }
        private ICommand _ConnectMediaWsCommand;
        public ICommand ConnectMediaWsCommand
        {
            get => _ConnectMediaWsCommand;
            set => SetField(ref _ConnectMediaWsCommand, value);
        }

        private ICommand _MouseMoveCommand;
        public ICommand MouseMoveCommand
        {
            get => _MouseMoveCommand;
            set => SetField(ref _MouseMoveCommand, value);
        }

        private ICommand _GenericCommand;
        public ICommand GenericCommand
        {
            get => _GenericCommand;
            set => SetField(ref _GenericCommand, value);
        }
        private ICommand _HttpPostCommand;
        public ICommand HttpPostCommand
        {
            get => _HttpPostCommand;
            set => SetField(ref _HttpPostCommand, value);
        }
        private ICommand _HttpGetCommand;
        public ICommand HttpGetCommand
        {
            get => _HttpGetCommand;
            set => SetField(ref _HttpGetCommand, value);
        }
        private ICommand _LoginCommand;
        public ICommand LoginCommand
        {
            get => _LoginCommand;
            set => SetField(ref _LoginCommand, value);
        }
        private ICommand _New1Command;
        public ICommand New1Command
        {
            get => _New1Command;
            set => SetField(ref _New1Command, value);
        }
        private ICommand _New2Command;
        public ICommand New2Command
        {
            get => _New2Command;
            set => SetField(ref _New2Command, value);
        }
        private string _HttpPostUri;
        public string HttpPostUri
        {
            get => _HttpPostUri;
            set => SetField(ref _HttpPostUri, value);
        }
        private string _HttpGetUri;
        public string HttpGetUri
        {
            get => _HttpGetUri;
            set => SetField(ref _HttpGetUri, value);
        }
        private string _UserName;
        public string UserName
        {
            get => _UserName;
            set => SetField(ref _UserName, value);
        }
        private string _PassWD;
        public string PassWD
        {
            get => _PassWD;
            set => SetField(ref _PassWD, value);
        }
        public MainVM()
        {
            ConnectHttpReqCommand = new AsyncSimpleCommand(ExecuteConnectHttpReqCommand);
            ConnectApiWsCommand = new AsyncSimpleCommand(ExecuteConnectApiWsCommand);
            ConnectMediaWsCommand = new AsyncSimpleCommand(ExecuteConnectMediaWsCommand);

            MouseMoveCommand = new AsyncSimpleCommand(ExecuteMouseMoveCommand);
            GenericCommand = new AsyncSimpleCommand(ExecuteGenericCommand);
            HttpPostCommand = new AsyncSimpleCommand(ExecuteHttpPostCommand);
            HttpGetCommand = new AsyncSimpleCommand(ExecuteHttpGetCommand);
            LoginCommand = new AsyncSimpleCommand(ExecuteLoginCommand);

            New1Command = new AsyncSimpleCommand(ExecuteNew1Command);
            New2Command = new AsyncSimpleCommand(ExecuteNew2Command);

            PikvmHttpClient = new Pikvm_HttpClient();
            ApiWs = new Pikvm_Apiws();
            ApiWs2 = new Pikvm_Apiws();
            VideoStream = new Pikvm_ApiVideoStream();

            LogMessages = new ObservableCollection<LogMessage>();

            PikvmHttpClient.OnLogEvent += Processes_OnLogEvent;
            PikvmHttpClient.OnHttpLogEvent += Processes_OnLogEvent;

            ApiWs.OnLogEvent += Processes_OnLogEvent;
            ApiWs.OnMessageRecieved += Processes_OnMessage;

            VideoStream.OnImageRecieved += OnVideoImageRecieved;
            VideoStream.OnLogEvent += Processes_OnLogEvent;
            

            PikvmInterface pikvmInterface = new PikvmInterface();
            pikvmInterface.OnLogEvent += (sender, logMessage) => Console.WriteLine(logMessage.TimeStamp);
            pikvmInterface.OnHttpMessageEvent += (sender, message) => Console.WriteLine(message);
            pikvmInterface.InitializeCommunication("https://192.168.1.183", "nefftest", "test");
            pikvmInterface.SetResolution(1920, 1080);
            pikvmInterface.SetMouseMode(MouseOutputType.usb_rel);
            //pikvmInterface.MoveMouse(MouseMode.Relative, -120, -120);
            //pikvmInterface.MoveMouse(MouseMode.Relative, 0, -120);
            //pikvmInterface.MouseClick(PiKvmLibrary.MouseButton.Right);
            pikvmInterface.OnLogEvent += (sender, e) => Debug.WriteLine($"{e.TimeStamp} - {e.LogLevel}: {e.Message}");
            pikvmInterface.OnHttpMessageEvent += (sender, e) => Debug.WriteLine($"{e}");
            try
            {
                pikvmInterface.SendPrintText("This\b\b is a test message from PiKVM API Example.");
                pikvmInterface.SendKeyboardKey('\n');
                pikvmInterface.SendPrintText("This is a test message from PiKVM API Example. 2", true);
                pikvmInterface.SendKeyboardKey('\n');
                pikvmInterface.SendPrintText("This is a test message from PiKVM API Example. 3", false, 10);
                pikvmInterface.SendKeyboardKey((char)8); // Backspace
                pikvmInterface.SendKeyboardKey((char)8); // Backspace
                pikvmInterface.SendKeyboardKey('\b'); // Backspace
                pikvmInterface.SendKeyboardKey('\b'); // Backspace
                pikvmInterface.SendKeyboardKey('\n');
                pikvmInterface.SendPrintText("This is a test message from PiKVM API Example. 4", false, 0, "da");
                pikvmInterface.SendKeyboardKey('\n');
                pikvmInterface.SendPrintText("This is a test message from PiKVM API Example. 5", true);
                pikvmInterface.SendKeyboardKey('\n');
                pikvmInterface.SendKeyboardMultiKey(new string[] { "ControlLeft", "ShiftLeft", "Escape" });
                //pikvmInterface.MouseClick(PiKvmLibrary.MouseButton.Left);
                //pikvmInterface.MouseClickState(PiKvmLibrary.MouseButton.Right, true);
                //pikvmInterface.MouseClickState(PiKvmLibrary.MouseButton.Right, false);
                //pikvmInterface.MouseClickState(PiKvmLibrary.MouseButton.Left, true);
                //pikvmInterface.MouseClickState(PiKvmLibrary.MouseButton.Left, false);
                //pikvmInterface.GenericRequest("MousMoveRelative", new object[] { "25", "25" });
                //pikvmInterface.GenericRequest("MouseButtonState", new object[] { "right", true });
                //pikvmInterface.GenericRequest("MouseButtonState", new object[] { "right", false });
                //pikvmInterface.GenericRequest("MouseButtonSt", new object[] { "right", true });
                //pikvmInterface.GenericRequest("MouseButtonSt", new object[] { "right", false });
                //pikvmInterface.GenericRequest("MousMoveRelative", new object[] { 25, 25 });
                //pikvmInterface.GenericRequest("MouseButtonState", new object[] { "right", true });
                //pikvmInterface.GenericRequest("MousMoveRelative", new object[] { 25, 25 });
            }
            catch
            (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            //ConfigurationData<PiKvmLibraryConfigurationType> config = new ConfigurationData<PiKvmLibraryConfigurationType>();
            //ConnectionType connection = config.ApplicationConfiguration.Connections.Connection.First();

            //connection.InitializeEndpoints();


            //EndpointType[] endpoints = connection.Endpoints;

            //EndpointType login = endpoints.First(o => o.Name == "Login_Endpoint");
            //login.SendEndpoint(new object[] { "nefftest", "test" });

            //connection.SetCredentials(login);

            //EndpointType mouseType = endpoints.First(o => o.Name == "MouseOutputType_Endpoint");
            //EndpointType mouseOutputRelMove = endpoints.First(o => o.Name == "MouseOutputRelative_Endpoint");


            //mouseType.SendEndpoint(new object[] { "usb_rel" });
            //mouseOutputRelMove.SendEndpoint(new object[] { "800", "800" });


            //VideoStream.OnImageRecieved += (sender, e) =>
            //{
            //    BitmapImage bitmapImage = LoadBitmapImagea(e);
            //    if (VideoBitmap == null)
            //    {
            //        VideoBitmap = new WriteableBitmap(bitmapImage);
            //    }
            //    else
            //    {
            //        // Lock the WriteableBitmap for updating
            //        VideoBitmap.Lock();

            //        // Copy pixels from BitmapImage into WriteableBitmap
            //        bitmapImage.CopyPixels(new Int32Rect(0, 0, bitmapImage.PixelWidth, bitmapImage.PixelHeight),
            //                               VideoBitmap.BackBuffer,
            //                               VideoBitmap.BackBufferStride * bitmapImage.PixelHeight,
            //                               VideoBitmap.BackBufferStride);

            //        // Mark WriteableBitmap as updated
            //        VideoBitmap.AddDirtyRect(new Int32Rect(0, 0, bitmapImage.PixelWidth, bitmapImage.PixelHeight));
            //        VideoBitmap.Unlock();
            //    }
            //    //if (sender is Models.Pikvm_ApiVideoStream vidStrm)
            //    //{
            //    //    if (VideoBitmap == null || VideoBitmap.PixelWidth != vidStrm.Width || VideoBitmap.PixelHeight != vidStrm.Height)
            //    //    {
            //    //        VideoBitmap = new WriteableBitmap(vidStrm.Width, vidStrm.Height, 96, 96, System.Windows.Media.PixelFormats.Bgra32, null);
            //    //    }
            //    //    VideoBitmap.Lock();
            //    //    VideoBitmap.WritePixels(new System.Windows.Int32Rect(0, 0, vidStrm.Width, vidStrm.Height), e, e.ImageData.Length, e.Width * 4);
            //    //    VideoBitmap.Unlock();
            //    //}
            //};
        }
        private async Task ExecuteNew1Command(object parameter)
        {
            string response = PikvmHttpClient.GetResponse("api/hid");
            //var nifo = HidInformationType.Deserialize(response);
        }
        private async Task ExecuteNew2Command(object parameter)
        {
        }
        private async Task ExecuteLoginCommand(object arg)
        {
            PikvmHttpClient.PostLoginCookie(UserName, PassWD);
            var cookies = PikvmHttpClient.GetAuthCookies();
            if (cookies != null && cookies.Count > 0)
            {
                ApiWs.SetCredentials(cookies);
                ApiWs2.SetCredentials(cookies);
                VideoStream.SetCredentials(cookies);
            }
        }

        private async Task ExecuteHttpGetCommand(object arg)
        {
            string response = PikvmHttpClient.GetResponse(HttpGetUri);
            Processes_OnLogEvent(this, new LogMessage() { LogLevel = LogLevel.Information, Message = $"GET request sent to {HttpGetUri}", TimeStamp = DateTime.Now });
            Processes_OnLogEvent(this, new LogMessage() { LogLevel = LogLevel.Information, Message = $"GET request response: {response}", TimeStamp = DateTime.Now });
        }

        private async Task ExecuteConnectHttpReqCommand(object parameter)
        {
            PikvmHttpClient.EnableHttpLogStreamAsync();
        }
        private async Task ExecuteConnectApiWsCommand(object parameter)
        {
            await ApiWs.ConnectAsync();
            await ApiWs2.ConnectAsync();
            //await VideoStream.ConnectAsync();
            await VideoStream.StartVideoStream();
        }
        private async Task ExecuteConnectMediaWsCommand(object parameter)
        {
            await VideoStream.ConnectAsync();
            await VideoStream.StartVideoStream();
        }
        private async Task ExecuteHttpPostCommand(object parameter)
        {
            if (parameter is object[] messageArr)
            {
                if (messageArr.Length < 2 || !(messageArr[0] is string) || !(messageArr[1] is string))
                    throw new ArgumentException("Invalid parameters for HttpPostCommand. Expected two strings: URL and message.");
                if (messageArr.Length == 1 && (messageArr[0] is string uri_1))
                {
                    string message = null;
                    await PikvmHttpClient.PostRequest(uri_1, null);
                }
            }
            if (parameter is string uri)
            {
                string a = await PikvmHttpClient.PostRequest(uri, null);
            }
        }
        private async Task ExecuteGenericCommand(object parameter)
        {
            if (parameter is string message)
                await ApiWs.GenericSendMessage(message);
        }

        private async Task ExecuteMouseMoveCommand(object parameter)
        {
            if (parameter is Point point)
            {
                await ApiWs.SendMouseRelativeAsync((int)point.X, (int)point.Y);
            }
        }

        private async Task ExecuteConnectCommand(object parameter)
        {
            await ApiWs.ConnectAsync();
            await VideoStream.ConnectAsync();
            await VideoStream.StartVideoStream();
        }
        private void Processes_OnMessage(object sender, string e)
        {
            if (sender is Pikvm_ApiVideoStream vidStrm || sender is Pikvm_Apiws apiws)
            {
                LogMessages.Add(new LogMessage() { LogLevel = LogLevel.Information, Message = e, TimeStamp = DateTime.Now });
            }
        }
        private void Processes_OnLogEvent(object sender, LogMessage e)
        {
            if (sender is Pikvm_ApiVideoStream vidStrm || sender is Pikvm_Apiws apiws || sender is Pikvm_HttpClient httpRequest)
            {
                LogMessages.Add(e);
            }
        }

        private void OnVideoImageRecieved(object sender, byte[] e)
        {
            if (sender is Pikvm_ApiVideoStream vidStrm)
            {
                BitmapImage bitmapImage = LoadBitmapImagea(e);
                if (VideoBitmap == null || VideoBitmap.PixelWidth != vidStrm.Width || VideoBitmap.PixelHeight != vidStrm.Height)
                {
                    VideoBitmap = new WriteableBitmap(bitmapImage);
                }
                else
                {
                    // Lock the WriteableBitmap for updating
                    VideoBitmap.Lock();

                    // Copy pixels from BitmapImage into WriteableBitmap
                    bitmapImage.CopyPixels(new Int32Rect(0, 0, bitmapImage.PixelWidth, bitmapImage.PixelHeight),
                                                               VideoBitmap.BackBuffer,
                                                               VideoBitmap.BackBufferStride * bitmapImage.PixelHeight,
                                                               VideoBitmap.BackBufferStride);

                    // Mark WriteableBitmap as updated
                    VideoBitmap.AddDirtyRect(new Int32Rect(0, 0, bitmapImage.PixelWidth, bitmapImage.PixelHeight));
                    VideoBitmap.Unlock();
                }
            }
        }
        private static BitmapImage LoadBitmapImagea(byte[] jpegData)
        {
            using (var stream = new MemoryStream(jpegData))
            {
                BitmapImage bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                return bitmapImage;
            }
        }

        #region Protected Methods
        /// <summary>
        /// Method that is used to invoke the PropertyChanged event.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Method that is used when a property value is changed to incoke the event PropertyChanged.
        /// </summary>
        /// <typeparam name="Ty">Type of the field that is being set.</typeparam>
        /// <param name="field">The field variable being set.</param>
        /// <param name="value">Value to set the field to.</param>
        /// <param name="propertyName">Property name assocaited with the field.</param>
        /// <returns>True, if OnPropertyChanged was called to invoke ProperyChanged event.</returns>
        protected virtual bool SetField<Ty>(ref Ty field, Ty value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<Ty>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }
        /// <summary>
        /// Method that is used to invoke the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The property name that was changed.</param>
        public virtual void OnPropertyChanged(string propertyName) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion
    }
}
