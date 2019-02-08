using System;
using Net.Pkcs11Interop.HighLevelAPI;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Crypto;

namespace BCC_CA_App_Service.App
{
    public class Handler
    {
        public void KeyGeneratorCaller(long serverGeneratedEnrollmentID)
        {
            //confession of economic hitman 
            EnrollementDTO enrollmentDTO = null;
            BigInteger enrollmentID = null;
            AsymmetricCipherKeyPair asymmetricCipherKeyPair;
            Pkcs10CertificationRequest certificationRequest;

            GetEnrollmentDTO(out enrollmentDTO, serverGeneratedEnrollmentID);
            enrollmentID = BigInteger.ValueOf(enrollmentDTO.ID);

            IsPassphaseCorrect(InputHandler.GetUserPassPhase(), enrollmentDTO.passPhase);

            //populate AsymmetricCipherKeyPair
            asymmetricCipherKeyPair = GetKeyPair();
            //populate Pkcs10CertificationRequest
            certificationRequest = GetCSR(asymmetricCipherKeyPair, enrollmentDTO);
            // save generated private key into windows or smart card store.
            SaveKeys(asymmetricCipherKeyPair, enrollmentDTO);
            //Generate .p7b in server
            GenerationRequestToServerForDotP7B(enrollmentDTO.ID, enrollmentDTO.keyStoreType, certificationRequest);

            Console.WriteLine("Key Generation and Write: Done Successfully");
        }

        private void IsPassphaseCorrect(String userInputedPassPhase, String passPhase)
        {
            SecurityHandler.CheckPassPhaseValidity(userInputedPassPhase, passPhase);
        }

        private void GetEnrollmentDTO(out EnrollementDTO enrollmentDTO, long serverGeneratedEnrollmentID)
        {
            enrollmentDTO = new NetworkHandler().GetEnrollmentInfo(Constants.PartialUrlOfApi.ENROLLMENT_INFO, serverGeneratedEnrollmentID);
        }

        private void GenerationRequestToServerForDotP7B(long iD, int keyStoreType, Pkcs10CertificationRequest certificationRequest)
        {
            new NetworkHandler().PostCertificateGenerationRequest(iD, keyStoreType, certificationRequest);
        }

        private void SaveKeys(AsymmetricCipherKeyPair asymmetricCipherKeyPair, EnrollementDTO enrollmentDTO)
        {
            switch (enrollmentDTO.keyStoreType)
            {
                case Constants.KeyStore.SMART_CARD:
                    {
                        WritePrivateKeyToSmartCard(asymmetricCipherKeyPair, enrollmentDTO);
                    }
                    break;
                case Constants.KeyStore.WINDOWS:
                    {
                        WritePrivateKeyToWindowsKeystore(asymmetricCipherKeyPair, enrollmentDTO);
                    }
                    break;
            }
        }

        private ObjectHandle WritePrivateKeyToSmartCard(AsymmetricCipherKeyPair asymmetricCipherKeyPair, EnrollementDTO enrollmentDTO)
        {
            Pkcs1xHandler pkcs1xHandler = new Pkcs1xHandler();
            SmartCardHandler smartCardHandler = new SmartCardHandler();
            ObjectHandle objectHandle;

            Session smartCardSession = null;
            {
                smartCardHandler.Start(out smartCardSession);

                objectHandle = smartCardHandler.ImportPrivateKeyToSmartCard(smartCardSession, asymmetricCipherKeyPair, enrollmentDTO.ID);

                smartCardHandler.Destroy(smartCardSession);
            }

            return objectHandle;
        }

        private void WritePrivateKeyToWindowsKeystore(AsymmetricCipherKeyPair asymmetricCipherKeyPair, EnrollementDTO enrollmentDTO)
        {
            WindowsKeystoreHandler windowsKeystoreHandler = new WindowsKeystoreHandler();
            windowsKeystoreHandler.writePrivateKey(asymmetricCipherKeyPair, enrollmentDTO.ID);
        }


        public void CertificateGeneratorForSmartCard(X509Certificate x509certificate)
        {
            SmartCardHandler smartCardHandler = new SmartCardHandler();

            Session smartCardSession = null;
            {
                smartCardHandler.Start(out smartCardSession);

                smartCardHandler.ImportCertificateToSmartCard(smartCardSession, x509certificate);

                smartCardHandler.Destroy(smartCardSession);
            }
        }

        public void CertificateGeneratorForWindowsKeystore(X509Certificate x509certificate, long enrollmentId)
        {
            WindowsKeystoreHandler windowsKeystoreHandler = new WindowsKeystoreHandler();
            windowsKeystoreHandler.writeCertificate(x509certificate, enrollmentId);
        }

        public void CertificateGenerator(long serverGeneratedEnrollmentID, int KeyStore)
        {
            String stringifyCertificate = new NetworkHandler().GetCertificateByteArray(serverGeneratedEnrollmentID);
            X509Certificate x509certificate = new Pkcs1xHandler().GenerateCertificate(stringifyCertificate);

            switch (KeyStore)
            {
                case Constants.KeyStore.SMART_CARD:
                    {
                        CertificateGeneratorForSmartCard(x509certificate);
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
                smartCardHandler.Start(out smartCardSession);
                smartCardHandler.KeyPairGenerator(smartCardSession, out publicKey, out privateKey);
                smartCardHandler.Destroy(smartCardSession, publicKey, privateKey);
            }
        }
    }
}
