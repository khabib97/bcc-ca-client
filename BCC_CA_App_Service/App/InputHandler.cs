using System;

namespace BCC_CA_App_Service.App
{
    public class InputHandler
    {
        public String GetUserPassPhase() {
            Console.WriteLine("Please input your Pass-phase :");
            return Console.ReadLine();
        }

        public String GetSmartCardPin()
        {
            Console.WriteLine("Please input smart card pin :");
            return Constants._PIN = Console.ReadLine();
        }
    }
}
