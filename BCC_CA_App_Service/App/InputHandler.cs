using System;

namespace BCC_CA_App_Service.App
{
    public class InputHandler
    {
        public static String GetUserPassPhase() {
            Console.WriteLine("Please input your Pass-phase :");
            return Console.ReadLine();
        }

        public static String GetSmartCardPin()
        {
            Console.WriteLine("Please input smart card pin :");
            return Constants._PIN = Console.ReadLine();
        }

        public static String GetEnrollmentID()
        {
            Console.WriteLine("Please input enrollmentID :");
            return Console.ReadLine();
        }
        public static String GetGenerationMode()
        {
            Console.WriteLine("Please write 'key' or 'certificate' :");
            return Console.ReadLine();
        }
    }
}
