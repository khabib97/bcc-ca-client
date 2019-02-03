using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BCC_CA_App_Service.App
{
    class WindowsKeystoreHandler
    {
        // writes the private-key to container
        public  void writePrivateKey(Pki pki, long enrollmentID)
        {
            AsymmetricCipherKeyPair ackp= pki.asymmetricCipherKeyPair;
            var rsaPriv = Org.BouncyCastle.Security.DotNetUtilities.ToRSA(ackp.Private as RsaPrivateCrtKeyParameters);

            // Setup RSACryptoServiceProvider with "KeyContainerName" set to "KeyContainer"+ enrollmentID
            var csp = new CspParameters();
            csp.KeyContainerName = "KeyContainer"+ enrollmentID;
            csp.Flags |= CspProviderFlags.UseMachineKeyStore;

            var rsaPrivate = new RSACryptoServiceProvider(csp);
            rsaPrivate.PersistKeyInCsp = true; //persisting the key in container is important to retrieve the key later

            // Import private key to windows keystrore, from already generated BouncyCastle rsa privatekey
            rsaPrivate.ImportParameters(rsaPriv.ExportParameters(true));

            Console.Write("rsaprivate key:" + rsaPrivate.ToXmlString(true));
        }

        //// retrieves orresponding privatekey from windows keystore using the container-name
        public RSACryptoServiceProvider retrievePrivateKey(long enrollmentID)
        {
            // Finding the corresponding privatekey from windows keystore using the container-name
            var csp = new CspParameters();
            csp.KeyContainerName = "KeyContainer" + enrollmentID;
            csp.Flags |= CspProviderFlags.UseMachineKeyStore;
            var rsaPrivate = new RSACryptoServiceProvider(csp);
            rsaPrivate.PersistKeyInCsp = true;

            Console.Write("rsaprivate key:" + rsaPrivate.ToXmlString(true));
            return rsaPrivate;
        }
        public void writeCertificate(Org.BouncyCastle.X509.X509Certificate cert, long enrollmentID)
        {
            // converting from bouncycastle X509Certificate to  System.Security.Cryptography.X509Certificates.X509Certificate2
            System.Security.Cryptography.X509Certificates.X509Certificate2 certificate = new System.Security.Cryptography.X509Certificates.X509Certificate2();
            certificate.Import(cert.GetEncoded());

            // Finding the corresponding privatekey from windows keystore using the container-name
            RSACryptoServiceProvider rsaPrivate = retrievePrivateKey(enrollmentID);
           // linking the retrieved private key to the certificate
            certificate.PrivateKey = rsaPrivate;  

            // opening up the windows cert store because thats where I want to save it.
            System.Security.Cryptography.X509Certificates.X509Store store = new System.Security.Cryptography.X509Certificates.X509Store(System.Security.Cryptography.X509Certificates.StoreName.My, System.Security.Cryptography.X509Certificates.StoreLocation.CurrentUser);
            store.Open(System.Security.Cryptography.X509Certificates.OpenFlags.MaxAllowed);
            store.Add(certificate);
            store.Close();

        }
    }
}
