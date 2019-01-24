using System;
using Net.Pkcs11Interop.HighLevelAPI;
using Org.BouncyCastle.Math;

namespace BCC_CA_App_Service.App
{
    public class Handler
    {
        public void Generator(string baseRemoteURL, string apiEndPoint, int serverGeneratedEnrollmentID, string generationMode)
        {
            NetworkHandler networkHandler = new NetworkHandler();
            SecurityHandler securityHandler = new SecurityHandler();
            InputHandler inputHandler = new InputHandler();
            Pkcs1xHandler pkcs1xHandler = new Pkcs1xHandler();
            EnrollementDTO enrollmentDTO = null;
            BigInteger enrollmentID = null;

            Pki pki = new Pki();

            enrollmentDTO = networkHandler.GetEnrollmentInfo(baseRemoteURL, Constants.PartialUrlOfApi.ENROLLMENT_INFO, serverGeneratedEnrollmentID, generationMode);
            enrollmentID = BigInteger.ValueOf(enrollmentDTO.ID);
            String userInputPassPhase = inputHandler.GetUserPassPhase();
            securityHandler.CheckPassPhaseValidity(userInputPassPhase, enrollmentDTO.passPhase);

            switch (generationMode)
            {
                case Constants.GeneratedTypeCertificateOrKey.CERTIFICATE:
                    throw new NotImplementedException();
                case Constants.GeneratedTypeCertificateOrKey.KEY:

                    pki.asymmetricCipherKeyPair = Pkcs1xHandler.GenerateKeyPair(Constants.RsaKeyLength.Length2048Bits);
                    pki.certificationRequest = Pkcs1xHandler.GenerateCertificateSigningRequest(enrollmentDTO);

                    pki.temporaryX509Certificate = pkcs1xHandler.GenerateTemporarySelfSignedCertificate(pki, enrollmentID, Constants.NUMBER_OF_YEAR);

                    Console.WriteLine("Self-signed Temp Certificate:" +pki.temporaryX509Certificate.ToString());

                    SaveKeysAndCertificate(pki, baseRemoteURL, enrollmentDTO.ID, enrollmentDTO.keyStoreType);
                    break;
            }

            Console.WriteLine("Success");
        }

        internal void SaveKeysAndCertificate(Pki pki, String baseUrl, long enrollmentID, int keyStoreType)
        {
            Pkcs1xHandler pkcs1xHandler = new Pkcs1xHandler();
            SmartCardHandler smartCardHandler = new SmartCardHandler();
            NetworkHandler networkHandler = new NetworkHandler();
            switch (keyStoreType)
            {
                case Constants.KeyStore.SMART_CARD:
                    {
                        Session smartCardSession = null;

                        smartCardHandler.StartSmartCardSession(out smartCardSession);

                        smartCardHandler.ImportPrivateKeyToSmartCard(smartCardSession, pki);
                        smartCardHandler.ImportCertificateToSmartCard(smartCardSession, pki.temporaryX509Certificate);

                        //Generate .p7b in server
                        networkHandler.PostCertificationRequest(baseUrl, enrollmentID, keyStoreType, pki.certificationRequest);

                        String cert = networkHandler.GetCertificateByteArray(enrollmentID, baseUrl);
                        pki.x509certificate = pkcs1xHandler.GenerateCertificate(cert)[0];
                        smartCardHandler.ImportCertificateToSmartCard(smartCardSession, pki.x509certificate);

                        smartCardHandler.DestroySmartCardSession(smartCardSession);
                    }
                    break;
                case Constants.KeyStore.WINDOWS:
                    break;
            }
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
