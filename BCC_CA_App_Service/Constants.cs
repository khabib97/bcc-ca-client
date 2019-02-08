using System;
using System.Collections.Generic;

namespace BCC_CA_App_Service
{
    public static class Constants
    {
        public class Token
        {
            public int value;
            public String name;
            public int slot;
            public String driver;

            public Token(int value, String name, int slot, String driver)
            {
                this.value = value;
                this.name = name;
                this.slot = slot;
                this.driver = driver;
            }
        }

        public enum SignatureAlgorithm
        {
            SHA1, SHA256, SHA512
        }

        public enum RsaKeyLength
        {
            Length2048Bits = 2048, Length3072Bits = 3072, Length4096Bits = 4096
        }


        //public const int VALUE = 1;
        //public const String NAME = "Gemlato";
        //public const int SLOT = 2;
        // public const String DRIVER = "gtop11dotnet64.dll";

        private const String FILE_SEPARATOR = "//";
        public static long ENROLLMENT_ID;

        public const int MAX_KEY_COUNT = 50;

        public static class Algorithm {
            public const String KEY_PAIR_GENERATION = "RSA";
            public const String SIGNING = "SHA256WithRSA";
        }

        public static class PartialUrlOfApi {
            public const String ENROLLMENT_INFO = "EnrollmentAction";
            public const String CRS = "EnrollmentAction";
        }

        public static class KeyStoreName {
            public const String DEFAULT_KSN = "pkcs12";
            public const String DEFAULT_WINDOWS_KSN = "Windows-MY";
            public const String ROOT_WINDOWS_KSN = "Windows-ROOT";
        }

        //obsulate : used for IE when using java platform
        public const String WINDOWS_PROVIDER_NAME = "SunMSCAPI";

        public const String GENERATED_PKCS_CONFIG_FILE = "pkcs11.cfg";

        public static class AliasName{
            public const String BCC_CA = "Bangladesh Computer Council CA 2016-G2";
            public const String ROOT_CA = "Bangladesh Computer Council CA 2016-G2";
        }

        public const String DIR_NAME = "C:" + "BCC_CA" ; // should be dynamic
        public const String CERTIFICATE_PATH = DIR_NAME + FILE_SEPARATOR;

        public static class Prifix {
            public const String SELF_CERTIFICATE = DIR_NAME + FILE_SEPARATOR + "SELF_SIGNED";
            public const String KEY = DIR_NAME + FILE_SEPARATOR + "KEY_";
        }

        public static class KeyStore {
            public const int WINDOWS = 1;
            public const int SMART_CARD = 2;
        }

        public static class TokenType {
            public const int GEMALTO = 1;
            public const int ETOKEN = 2;
        }

        //input: value, name, slot, driver
        static Token gemalto = new Token(1, "Gemalto",2, "gtop11dotnet64.dll");
        static Token eToken = new Token( 2, "eToken", 0, "eTPKCS11.dll");

        public static Dictionary<int, Token> TOKEN_DICTIONARY = new Dictionary<int, Token> { { TokenType.GEMALTO, gemalto }, { TokenType.ETOKEN, eToken } };

        public static class GeneratedTypeCertificateOrKey {
            public const String CERTIFICATE = "certificate";
            public const String KEY = "key";
        }

        public static class FileExtension {
            public const String CERTIFICATE = ".p7b";
            public const String STORE = ".p12";
        }

        public const int KEY_SIZE_KB = 2048;
        public const int DAY_COUNT_IN_YEAR = 365;
        public const int VALIDITY_DAY = 2 * DAY_COUNT_IN_YEAR;
        public const int NUMBER_OF_YEAR = 2;

        public static String _PIN;
        public const String APPLICATION_NAME = @"Pkcs11Interop";
        public static String PKCS11_LIBRARY_PATH ;

        public static String BASE_URL = "bcc-ca.gov.bd";
        public static String PIN;
        public static String PASSPHASE;

        public static String GLOBAL_RESPONSE_MSG;
        public static String GLOBAL_ERROR_MSG;

    }
}
