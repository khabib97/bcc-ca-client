using SuperWebSocket;
using System;
using System.Windows.Forms;

namespace BCC_CA_App_Service.App
{
    public class InputHandler
    {
        public static String GetUserPassPhase() {
            return PasswordPrompt.ShowDialog("Please provide your passphse:", "Passphase Window");
        }

        public static String GetSmartCardPin()
        {
            return PasswordPrompt.ShowDialog("Please provide your smart card pin", "Smart Card Pin Window");
        }

    }
}
