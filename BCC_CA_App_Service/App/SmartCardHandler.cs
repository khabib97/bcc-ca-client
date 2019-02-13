using System;
using System.Collections.Generic;
using Net.Pkcs11Interop.HighLevelAPI;
using Net.Pkcs11Interop.Common;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Math;

namespace BCC_CA_App_Service.App
{
    class SmartCardHandler
    {  
        public void Start(out Session session) {
            try
            {
                String userPin = InputHandler.GetSmartCardPin();

                Pkcs11 pkcs11 = new Pkcs11(Constants.PKCS11_LIBRARY_PATH, AppType.SingleThreaded);

                List<Slot> slots = pkcs11.GetSlotList(SlotsType.WithTokenPresent);
                Slot matchingSlot = slots[0];
                session = matchingSlot.OpenSession(SessionType.ReadWrite);

                session.Login(CKU.CKU_USER, userPin);
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("Smart Card Access Problem "+ ex);
                MessagePrompt.ShowDialog("Can not access smart card. Please try again.", "Smart Card Window");
                throw new Exception("Can not access smart card");

            }
        }

        public void Destroy(Session session) {
            session.Logout();
        }

        public void Destroy(Session session,ObjectHandle publicKey, ObjectHandle privateKey){
            session.DestroyObject(publicKey);
            session.DestroyObject(privateKey);
            session.Logout();
        }

        //Write Private Key to Smard Card
        public ObjectHandle ImportPrivateKeyToSmartCard(Session session, AsymmetricCipherKeyPair asymmetricCipherKeyPair, long enrollmentID) {

            BigInteger id = BigInteger.ValueOf(enrollmentID);
            RsaPrivateCrtKeyParameters rsaPrivKey = (RsaPrivateCrtKeyParameters)asymmetricCipherKeyPair.Private;
            
            byte[] ckaId = id.ToByteArrayUnsigned();

            List<ObjectAttribute> objectAttributes = new List<ObjectAttribute>();
            //Common attribute
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_CLASS, CKO.CKO_PRIVATE_KEY));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_TOKEN, true));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_LABEL, Constants.APPLICATION_NAME));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_KEY_TYPE, CKK.CKK_RSA));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_ID, ckaId));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_SUBJECT, id.ToString()));

            //Must add this attribute to create objcet
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_MODULUS, rsaPrivKey.Modulus.ToByteArrayUnsigned()));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_PRIVATE_EXPONENT, rsaPrivKey.Exponent.ToByteArrayUnsigned()));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_PUBLIC_EXPONENT, rsaPrivKey.PublicExponent.ToByteArrayUnsigned()));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_PRIME_1, rsaPrivKey.P.ToByteArrayUnsigned()));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_PRIME_2, rsaPrivKey.Q.ToByteArrayUnsigned ()));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_COEFFICIENT, rsaPrivKey.QInv.ToByteArrayUnsigned()));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_EXPONENT_1, rsaPrivKey.DP.ToByteArrayUnsigned()));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_EXPONENT_2, rsaPrivKey.DQ.ToByteArrayUnsigned()));

            //write object/private key to smart card
            return session.CreateObject(objectAttributes);
        }

        //Write temporary certificate to smart card
        public void ImportCertificateToSmartCard(Session session, X509Certificate certificate)
        {
            // Get public key from certificate
            AsymmetricKeyParameter pubKeyParams = certificate.GetPublicKey();

            if (!(pubKeyParams is RsaKeyParameters))
                throw new NotSupportedException("Currently only RSA keys are supported");

            RsaKeyParameters rsaPubKeyParams = (RsaKeyParameters)pubKeyParams;

            // Find corresponding private key
            List<ObjectAttribute> privKeySearchTemplate = new List<ObjectAttribute>();
            privKeySearchTemplate.Add(new ObjectAttribute(CKA.CKA_CLASS, CKO.CKO_PRIVATE_KEY));
            privKeySearchTemplate.Add(new ObjectAttribute(CKA.CKA_KEY_TYPE, CKK.CKK_RSA));
            privKeySearchTemplate.Add(new ObjectAttribute(CKA.CKA_MODULUS, rsaPubKeyParams.Modulus.ToByteArrayUnsigned()));
            privKeySearchTemplate.Add(new ObjectAttribute(CKA.CKA_PUBLIC_EXPONENT, rsaPubKeyParams.Exponent.ToByteArrayUnsigned()));

            List<ObjectHandle> foundObjects = session.FindAllObjects(privKeySearchTemplate);
            if (foundObjects.Count != 1)
                throw new Exception("Corresponding RSA private key not found");
            else {
                Console.WriteLine("Corresponding private key found");

            }

            ObjectHandle privKeyObjectHandle = foundObjects[0];

            // Read CKA_LABEL and CKA_ID attributes of private key
            List<CKA> privKeyAttrsToRead = new List<CKA>();
            privKeyAttrsToRead.Add(CKA.CKA_LABEL);
            privKeyAttrsToRead.Add(CKA.CKA_ID);

            List<ObjectAttribute> privKeyAttributes = session.GetAttributeValue(privKeyObjectHandle, privKeyAttrsToRead);

            // Define attributes of new certificate object
            List<ObjectAttribute> certificateAttributes = new List<ObjectAttribute>();
            certificateAttributes.Add(new ObjectAttribute(CKA.CKA_CLASS, CKO.CKO_CERTIFICATE));
            certificateAttributes.Add(new ObjectAttribute(CKA.CKA_TOKEN, true));
            certificateAttributes.Add(new ObjectAttribute(CKA.CKA_PRIVATE, false));
            certificateAttributes.Add(new ObjectAttribute(CKA.CKA_MODIFIABLE, true));
            certificateAttributes.Add(new ObjectAttribute(CKA.CKA_LABEL, privKeyAttributes[0].GetValueAsString()));
            certificateAttributes.Add(new ObjectAttribute(CKA.CKA_CERTIFICATE_TYPE, CKC.CKC_X_509));
            certificateAttributes.Add(new ObjectAttribute(CKA.CKA_TRUSTED, false));
            certificateAttributes.Add(new ObjectAttribute(CKA.CKA_SUBJECT, certificate.SubjectDN.GetDerEncoded()));
            certificateAttributes.Add(new ObjectAttribute(CKA.CKA_ID, privKeyAttributes[1].GetValueAsByteArray()));
            certificateAttributes.Add(new ObjectAttribute(CKA.CKA_ISSUER, certificate.IssuerDN.GetDerEncoded()));
            certificateAttributes.Add(new ObjectAttribute(CKA.CKA_SERIAL_NUMBER, new DerInteger(certificate.SerialNumber).GetDerEncoded()));
            certificateAttributes.Add(new ObjectAttribute(CKA.CKA_VALUE, certificate.GetEncoded()));

            // Create certificate object
            session.CreateObject(certificateAttributes);
        }

        [Obsolete("Not needed")]
        public void KeyPairGenerator(Session session, out ObjectHandle publicKey, out ObjectHandle privateKey)
        {
            // The CKA_ID attribute is intended as a means of distinguishing multiple key pairs held by the same subject
            byte[] ckaId = session.GenerateRandom(20);

            // Prepare attribute template of new public key
            List<ObjectAttribute> publicKeyAttributes = new List<ObjectAttribute>();
            publicKeyAttributes.Add(new ObjectAttribute(CKA.CKA_TOKEN, true));
            publicKeyAttributes.Add(new ObjectAttribute(CKA.CKA_PRIVATE, false));
            publicKeyAttributes.Add(new ObjectAttribute(CKA.CKA_LABEL, Constants.APPLICATION_NAME));
            publicKeyAttributes.Add(new ObjectAttribute(CKA.CKA_ID, ckaId));
            publicKeyAttributes.Add(new ObjectAttribute(CKA.CKA_ENCRYPT, true));
            publicKeyAttributes.Add(new ObjectAttribute(CKA.CKA_VERIFY, true));
            publicKeyAttributes.Add(new ObjectAttribute(CKA.CKA_VERIFY_RECOVER, true));
            publicKeyAttributes.Add(new ObjectAttribute(CKA.CKA_WRAP, true));
            publicKeyAttributes.Add(new ObjectAttribute(CKA.CKA_MODULUS_BITS, 1024));
            publicKeyAttributes.Add(new ObjectAttribute(CKA.CKA_PUBLIC_EXPONENT, new byte[] { 0x01, 0x00, 0x01 }));

            // Prepare attribute template of new private key
            List<ObjectAttribute> privateKeyAttributes = new List<ObjectAttribute>();
            privateKeyAttributes.Add(new ObjectAttribute(CKA.CKA_TOKEN, true));
            privateKeyAttributes.Add(new ObjectAttribute(CKA.CKA_PRIVATE, true));
            privateKeyAttributes.Add(new ObjectAttribute(CKA.CKA_LABEL, Constants.APPLICATION_NAME));
            privateKeyAttributes.Add(new ObjectAttribute(CKA.CKA_ID, ckaId));
            privateKeyAttributes.Add(new ObjectAttribute(CKA.CKA_SENSITIVE, true));
            privateKeyAttributes.Add(new ObjectAttribute(CKA.CKA_DECRYPT, true));
            privateKeyAttributes.Add(new ObjectAttribute(CKA.CKA_SIGN, true));
            privateKeyAttributes.Add(new ObjectAttribute(CKA.CKA_SIGN_RECOVER, true));
            privateKeyAttributes.Add(new ObjectAttribute(CKA.CKA_UNWRAP, true));

            // Specify key generation mechanism
            Mechanism mechanism = new Mechanism(CKM.CKM_RSA_PKCS_KEY_PAIR_GEN);

            // Generate key pair
            session.GenerateKeyPair(mechanism, publicKeyAttributes, privateKeyAttributes, out publicKey, out privateKey);
        }

        [Obsolete("Not needed")]
        public ObjectHandle GenerateKey(Session session)
        {
            // Prepare attribute template of new key
            List<ObjectAttribute> objectAttributes = new List<ObjectAttribute>();
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_CLASS, CKO.CKO_SECRET_KEY));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_KEY_TYPE, CKK.CKK_DES3));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_ENCRYPT, true));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_DECRYPT, true));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_DERIVE, true));
            objectAttributes.Add(new ObjectAttribute(CKA.CKA_EXTRACTABLE, true));

            // Specify key generation mechanism
            Mechanism mechanism = new Mechanism(CKM.CKM_DES3_KEY_GEN);

            // Generate key
            return session.GenerateKey(mechanism, objectAttributes);
        }
    }
}
