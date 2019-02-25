using System;
using Net.Pkcs11Interop.HighLevelAPI;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Utilities.IO.Pem;
using System.IO;
using System.Net;

namespace BCC_CA_App_Service.App
{
    public class Handler
    {
        public string KeyGeneratorCaller(EnrollementDTO enrollmentDTO,string pin,string passphase)
        {
            //confession of economic hitman 
            BigInteger enrollmentID = null;
            AsymmetricCipherKeyPair asymmetricCipherKeyPair;
            Pkcs10CertificationRequest certificationRequest;
            
            enrollmentID = BigInteger.ValueOf(enrollmentDTO.ID);

            IsPassphaseCorrect(passphase, enrollmentDTO.passPhase);

            //populate AsymmetricCipherKeyPair
            asymmetricCipherKeyPair = GetKeyPair();
            //populate Pkcs10CertificationRequest
            certificationRequest = GetCSR(asymmetricCipherKeyPair, enrollmentDTO);
            // save generated private key into windows or smart card store.
            SaveKeys(pin,asymmetricCipherKeyPair, enrollmentDTO);
            Console.WriteLine("Key Generation and Write: Done Successfully");

            //Generate .p7b in server
            return DotP7BGenerationUri(enrollmentDTO.ID, enrollmentDTO.keyStoreType, certificationRequest); 
        }

        private void IsPassphaseCorrect(String userInputedPassPhase, String passPhase)
        {
            try
            {
                SecurityHandler.CheckPassPhaseValidity(userInputedPassPhase, passPhase);
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex);
                //MessagePrompt.ShowDialog("Passphase mismatch. Please try again","Passphase Window");
                throw new Exception(ex.Message);
            }
        }

        private void GetEnrollmentDTO(out EnrollementDTO enrollmentDTO, long serverGeneratedEnrollmentID)
        {
            enrollmentDTO = new NetworkHandler().GetEnrollmentInfo(Constants.PartialUrlOfApi.ENROLLMENT_INFO, serverGeneratedEnrollmentID);
        }

        private string DotP7BGenerationUri(long iD, int keyStoreType, Pkcs10CertificationRequest certificationRequest)
        {
            PemObject pemObject = new PemObject("CERTIFICATE REQUEST", certificationRequest.GetEncoded());

            string csr = null;
            using (StringWriter stringWriter = new StringWriter())
            {
                PemWriter pemWriter = new PemWriter(stringWriter);
                pemWriter.WriteObject(pemObject);
                csr = stringWriter.ToString();
                stringWriter.Close();
            }

            String URI = Constants.PartialUrlOfApi.CRS + "?"
                + "csr=" + WebUtility.UrlEncode(csr) + "&serialNo=" + iD + "&keystoreType="
                + keyStoreType + "&mode=csr"; ;

            return URI;
        }

        private void SaveKeys(string pin,AsymmetricCipherKeyPair asymmetricCipherKeyPair, EnrollementDTO enrollmentDTO)
        {
            switch (enrollmentDTO.keyStoreType)
            {
                case Constants.KeyStore.SMART_CARD:
                    {
                        WritePrivateKeyToSmartCard(pin, asymmetricCipherKeyPair, enrollmentDTO);
                    }
                    break;
                case Constants.KeyStore.WINDOWS:
                    {
                        WritePrivateKeyToWindowsKeystore(asymmetricCipherKeyPair, enrollmentDTO);
                    }
                    break;
            }
        }

        private ObjectHandle WritePrivateKeyToSmartCard(string pin,AsymmetricCipherKeyPair asymmetricCipherKeyPair, EnrollementDTO enrollmentDTO)
        {
            Pkcs1xHandler pkcs1xHandler = new Pkcs1xHandler();
            SmartCardHandler smartCardHandler = new SmartCardHandler();
            ObjectHandle objectHandle;

            Session smartCardSession = null;
            {
                smartCardHandler.Start(pin,out smartCardSession);

                objectHandle = smartCardHandler.ImportPrivateKeyToSmartCard(smartCardSession, asymmetricCipherKeyPair, enrollmentDTO.ID);

                smartCardHandler.Destroy(smartCardSession);
            }

            return objectHandle;
        }

        private void WritePrivateKeyToWindowsKeystore(AsymmetricCipherKeyPair asymmetricCipherKeyPair, EnrollementDTO enrollmentDTO)
        {
            try
            {
                WindowsKeystoreHandler windowsKeystoreHandler = new WindowsKeystoreHandler();
                windowsKeystoreHandler.writePrivateKey(asymmetricCipherKeyPair, enrollmentDTO.ID);
            }catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Failed to write private key inside windows store",ex);
                throw new Exception("Failed to write private key inside windows store");
            }
        }


        public void CertificateGeneratorForSmartCard(string pin,X509Certificate x509certificate)
        {
            SmartCardHandler smartCardHandler = new SmartCardHandler();

            Session smartCardSession = null;
            {
                smartCardHandler.Start(pin,out smartCardSession);

                smartCardHandler.ImportCertificateToSmartCard(smartCardSession, x509certificate);

                smartCardHandler.Destroy(smartCardSession);
            }
        }

        public void CertificateGeneratorForWindowsKeystore(X509Certificate x509certificate, long enrollmentId)
        {
            try
            {
                WindowsKeystoreHandler windowsKeystoreHandler = new WindowsKeystoreHandler();
                windowsKeystoreHandler.writeCertificate(x509certificate, enrollmentId);
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("Failed to write Certificate in Win Store" + ex);
                throw new Exception("Failed to write Certificate inside windows store");
            }
        }

        public void CertificateGenerator(string pin,long serverGeneratedEnrollmentID, int KeyStore,string dotp7b)
        {
            //String stringifyCertificate = new NetworkHandler().GetCertificateByteArray(serverGeneratedEnrollmentID);
            String stringifyCertificate = dotp7b;
            X509Certificate x509certificate = new Pkcs1xHandler().GenerateCertificate(stringifyCertificate);

            switch (KeyStore)
            {
                case Constants.KeyStore.SMART_CARD:
                    {
                        CertificateGeneratorForSmartCard(pin,x509certificate);
                    }
                    break;
                case Constants.KeyStore.WINDOWS:
                    {
                        CertificateGeneratorForWindowsKeystore(x509certificate, serverGeneratedEnrollmentID);
                    }
                    break;
            }
            Console.WriteLine("Certificate Writing: Done Successfully");
        }

        private Pkcs10CertificationRequest GetCSR(AsymmetricCipherKeyPair asymmetricCipherKeyPair, EnrollementDTO enrollmentDTO)
        {
            return new Pkcs1xHandler().GenerateCertificateSigningRequest(asymmetricCipherKeyPair, enrollmentDTO);
        }

        private AsymmetricCipherKeyPair GetKeyPair()        
        {
            return new Pkcs1xHandler().GenerateKeyPair(Constants.RsaKeyLength.Length2048Bits);
        }

        [Obsolete("Not needed ")]
        private void GenerateKeyPair()
        {
            SmartCardHandler smartCardHandler = new SmartCardHandler();

            ObjectHandle publicKey = null;
            ObjectHandle privateKey = null;
            Session smartCardSession = null;
            {
                smartCardHandler.Start("",out smartCardSession);
                smartCardHandler.KeyPairGenerator(smartCardSession, out publicKey, out privateKey);
                smartCardHandler.Destroy(smartCardSession, publicKey, privateKey);
            }
        }
    }
}
