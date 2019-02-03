using System;
using SuperWebSocket;
using System.Windows.Forms;

namespace BCC_CA_App_Service.App
{
    public partial class Installer : Form
    {
        public static WebSocketServer wsServer = new WebSocketServer();
        SocketCom socketCom = new SocketCom();
        public static int port = 8080;
        OpenFileDialog ofd = new OpenFileDialog();

        public Installer()
        {
            InitializeComponent();
          //  this.fileBtn.Click += new System.EventHandler(this.fileBtn_Click);
            this.submitBtn.Click += new System.EventHandler(this.submitBtn_Click);
        }

        //public void fileBtn_Click(object sender,EventArgs e)
        //{
        //    if (ofd.ShowDialog() == DialogResult.OK)
        //    {
        //        dllLoc.Text = ofd.FileName;
        //    }

        //}
      
        public void submitBtn_Click(object sender, EventArgs e)
        {
            Microsoft.Win32.RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            key.SetValue("Installer", Application.ExecutablePath.ToString());

            wsServer.Setup(port);
            wsServer.NewSessionConnected += socketCom.WsServer_NewSessionConnected;
            wsServer.NewMessageReceived += socketCom.WsServer_NewMessageReceived;
            wsServer.NewDataReceived += socketCom.WsServer_NewDataReceived;
            wsServer.SessionClosed += socketCom.WsServer_SessionClosed;
            wsServer.Start();
            //  Console.WriteLine("Server is running on port no : " + port + "  press any key to exit....");
            this.Close();
        }
    }
}
