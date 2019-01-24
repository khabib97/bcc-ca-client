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

        public String GetEnrollmentID()
        {
            Console.WriteLine("Please input enrollmentID :");
            return Console.ReadLine();
        }
        public String GetGenerationMode()
        {
            Console.WriteLine("Please write 'key' or 'certificate' :");
            return Console.ReadLine();
        }
    }
}
