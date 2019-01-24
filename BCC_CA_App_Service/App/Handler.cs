using System;
using Net.Pkcs11Interop.HighLevelAPI;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;

namespace BCC_CA_App_Service.App
{
    public class Handler
    {
        public void KeyGenerator(long serverGeneratedEnrollmentID)
        {
            EnrollementDTO enrollmentDTO = null;
            BigInteger enrollmentID = null;
            Pki pki = new Pki();

            GetEnrollmentDTO(out enrollmentDTO, serverGeneratedEnrollmentID);
            enrollmentID = BigInteger.ValueOf(enrollmentDTO.ID);

            IsPassphaseCorrect(enrollmentDTO.passPhase); 

            GetKeyPair(pki);
            GetCSR(pki, enrollmentDTO);
            SaveKeys(pki, enrollmentDTO);
            //Generate .p7b in server
            GenerationRequestToServerForDotP7B(enrollmentDTO.ID, enrollmentDTO.keyStoreType, pki.certificationRequest);
            
            Console.WriteLine("Key Generation and Write: Done Successfully");
        }

        private void IsPassphaseCorrect(string passPhase)
        {
            SecurityHandler securityHandler = new SecurityHandler();
            InputHandler inputHandler = new InputHandler();

            String userInputedPassPhase = inputHandler.GetUserPassPhase();
            securityHandler.CheckPassPhaseValidity(userInputedPassPhase, passPhase);
        }

        private void GetEnrollmentDTO(out EnrollementDTO enrollmentDTO, long serverGeneratedEnrollmentID)
        {
            NetworkHandler networkHandler = new NetworkHandler();
            enrollmentDTO = networkHandler.GetEnrollmentInfo(Constants.PartialUrlOfApi.ENROLLMENT_INFO, serverGeneratedEnrollmentID);
        }

        private void GenerationRequestToServerForDotP7B(long iD, int keyStoreType, Pkcs10CertificationRequest certificationRequest)
        {
            NetworkHandler networkHandler = new NetworkHandler();
            networkHandler.PostCertificationRequest(iD, keyStoreType, certificationRequest);
        }

        private void SaveKeys(Pki pki, EnrollementDTO enrollmentDTO)
        {
            
            switch (enrollmentDTO.keyStoreType)
            {
                case Constants.KeyStore.SMART_CARD:
                    {
                        WritePrivateKey(pki, enrollmentDTO);                       
                    }
                    break;
                case Constants.KeyStore.WINDOWS:
                    break;
            }
        }

        private void WritePrivateKey(Pki pki, EnrollementDTO enrollmentDTO)
        {
            Pkcs1xHandler pkcs1xHandler = new Pkcs1xHandler();
            SmartCardHandler smartCardHandler = new SmartCardHandler();

            Session smartCardSession = null;
            {
                smartCardHandler.StartSmartCardSession(out smartCardSession);

                smartCardHandler.ImportPrivateKeyToSmartCard(smartCardSession, pki, enrollmentDTO.ID);

                smartCardHandler.DestroySmartCardSession(smartCardSession);
            }
        }

        public void CertificateGenerator(long erollmentID)
        {
            NetworkHandler networkHandler = new NetworkHandler();
            Pkcs1xHandler pkcs1xHandler = new Pkcs1xHandler();
            SmartCardHandler smartCardHandler = new SmartCardHandler();

            Session smartCardSession = null;

            smartCardHandler.StartSmartCardSession(out smartCardSession);

            String stringifyCertificate = networkHandler.GetCertificateByteArray(erollmentID);
            X509Certificate x509certificate = pkcs1xHandler.GenerateCertificate(stringifyCertificate);
            smartCardHandler.ImportCertificateToSmartCard(smartCardSession, x509certificate);

            smartCardHandler.DestroySmartCardSession(smartCardSession);
        }

        private void GetCSR(Pki pki, EnrollementDTO enrollmentDTO)
        {
            pki.certificationRequest = Pkcs1xHandler.GenerateCertificateSigningRequest(pki, enrollmentDTO);
        }

        private void GetKeyPair(Pki pki)
        {
            pki.asymmetricCipherKeyPair = Pkcs1xHandler.GenerateKeyPair(Constants.RsaKeyLength.Length2048Bits);
        }

        [Obsolete("Not needed ")]
        private void GenerateKeyPair()
        {
            SmartCardHandler smartCardHandler = new SmartCardHandler();

            ObjectHandle publicKey = null;
            ObjectHandle privateKey = null;
            Session smartCardSession = null;
            {
                smartCardHandler.StartSmartCardSession(out smartCardSession);
                smartCardHandler.KeyPairGenerator(smartCardSession, out publicKey, out privateKey);
                smartCardHandler.DestroySmartCardSession(smartCardSession, publicKey, privateKey);
            }
        }
    } 
}
