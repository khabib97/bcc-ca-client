using System;
using System.IO;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Operators;
using Org.BouncyCastle.X509;
using System.Collections;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.X509.Store;
using Org.BouncyCastle.Utilities.Encoders;


namespace BCC_CA_App_Service.App
{
    public class Pkcs1xHandler
    {
        public static X509Name GenerateRelativeDistinguishedName(EnrollementDTO enrollmentDTO) {

            IDictionary attributes = new Hashtable();
            IList ordering;

            attributes.Add(X509Name.CN, enrollmentDTO.getCommonName());
            attributes.Add(X509Name.O, enrollmentDTO.organization);
            attributes.Add(X509Name.OU, enrollmentDTO.organizationUnit);
            attributes.Add(X509Name.C, enrollmentDTO.country);
            attributes.Add(X509Name.ST, enrollmentDTO.state);
            attributes.Add(X509Name.L, enrollmentDTO.area);
            attributes.Add(X509Name.PostalCode, enrollmentDTO.postalCode);
            attributes.Add(X509Name.SerialNumber, enrollmentDTO.getSerialNumber());

            ordering = new ArrayList(attributes.Keys);
            return new X509Name(ordering , attributes);
        }

        public static AsymmetricCipherKeyPair GenerateKeyPair(Constants.RsaKeyLength rsaKeyLength) {

            SecureRandom secureRandom = new SecureRandom(new CryptoApiRandomGenerator());

            KeyGenerationParameters keyGenerationParameters = new KeyGenerationParameters(secureRandom, 2048);
            RsaKeyPairGenerator rsaKeyPairGenerator = new RsaKeyPairGenerator();

            rsaKeyPairGenerator.Init(keyGenerationParameters);

            return rsaKeyPairGenerator.GenerateKeyPair();
        }

        public static Pkcs10CertificationRequest GenerateCertificateSigningRequest(Pki pki,EnrollementDTO enrollmentDTO){

            X509Name x509NameAsSubject = Pkcs1xHandler.GenerateRelativeDistinguishedName(enrollmentDTO);  

            Asn1SignatureFactory asn1SignatureFactory = new Asn1SignatureFactory(Constants.Algorithm.SIGNING, pki.asymmetricCipherKeyPair.Private);

            return new Pkcs10CertificationRequest(asn1SignatureFactory, x509NameAsSubject, pki.asymmetricCipherKeyPair.Public, null, pki.asymmetricCipherKeyPair.Private);
        }

        [Obsolete("We do not need to generate self signed certificate")]
        public X509Certificate GenerateTemporarySelfSignedCertificate(Pki pki, BigInteger enrollmentID, int numberOfYear) { 

            X509Name x509NameSubject = pki.certificationRequest.GetCertificationRequestInfo().Subject;
            X509V3CertificateGenerator x509V3CertificateGenerator = new X509V3CertificateGenerator();

            DateTime fromDate = DateTime.Today;
            DateTime toDate = fromDate.AddYears(numberOfYear);

            x509V3CertificateGenerator.SetIssuerDN(x509NameSubject);
            x509V3CertificateGenerator.SetNotBefore(fromDate);
            x509V3CertificateGenerator.SetNotAfter(toDate);
            x509V3CertificateGenerator.SetPublicKey(pki.asymmetricCipherKeyPair.Public);
            x509V3CertificateGenerator.SetSerialNumber(enrollmentID);
            x509V3CertificateGenerator.SetSignatureAlgorithm(Constants.Algorithm.SIGNING);
            x509V3CertificateGenerator.SetSubjectDN(x509NameSubject);

            X509Certificate x509Certificate = x509V3CertificateGenerator.Generate(pki.asymmetricCipherKeyPair.Private);

            return x509Certificate;

        }

        public X509Certificate GenerateCertificate(String certificateString) {

            byte[] bytes = Convert.FromBase64CharArray(certificateString.ToCharArray(), 0, certificateString.Length);
            CmsSignedData cmsSignedData = new CmsSignedData(bytes);

            IX509Store store =  cmsSignedData.GetCertificates("Collection");
            ICollection allCertificates =  store.GetMatches(null);

            IEnumerator enumerator =  allCertificates.GetEnumerator();
            while (enumerator.MoveNext()) {
                X509Certificate x509 = (X509Certificate)enumerator.Current;
                Console.WriteLine("Server Generated Certificate: "+ x509.ToString());
                return x509;
            }
            throw new Exception("Certificate generation error");
        }

        public void SavePrivateKey() {
            //Pkcs12Store        
        }

        

    }
}
