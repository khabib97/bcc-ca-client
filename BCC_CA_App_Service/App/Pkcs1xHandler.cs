using System;
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
using Org.BouncyCastle.X509.Store;


namespace BCC_CA_App_Service.App
{
    public class Pkcs1xHandler
    {
         protected static X509Name GenerateRelativeDistinguishedName(EnrollementDTO enrollmentDTO) {

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

        internal AsymmetricCipherKeyPair GenerateKeyPair(Constants.RsaKeyLength rsaKeyLength) {

            AsymmetricCipherKeyPair asymmetricCipherKeyPair = null;
            try
            {
                SecureRandom secureRandom = new SecureRandom(new CryptoApiRandomGenerator());

                KeyGenerationParameters keyGenerationParameters = new KeyGenerationParameters(secureRandom, 2048);
                RsaKeyPairGenerator rsaKeyPairGenerator = new RsaKeyPairGenerator();

                rsaKeyPairGenerator.Init(keyGenerationParameters);

                asymmetricCipherKeyPair =  rsaKeyPairGenerator.GenerateKeyPair();
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("Asymmetric key pair generation failed :" + ex);
                throw new Exception("Asymmetric key pair generation failed");
            }

            return asymmetricCipherKeyPair;
        }

        internal Pkcs10CertificationRequest GenerateCertificateSigningRequest(AsymmetricCipherKeyPair asymmetricCipherKeyPair, EnrollementDTO enrollmentDTO){

            Pkcs10CertificationRequest pkcs10CertificationRequest = null;
            try
            {
                X509Name x509NameAsSubject = GenerateRelativeDistinguishedName(enrollmentDTO);

                Asn1SignatureFactory asn1SignatureFactory = new Asn1SignatureFactory(Constants.Algorithm.SIGNING, asymmetricCipherKeyPair.Private);

                pkcs10CertificationRequest = new Pkcs10CertificationRequest(asn1SignatureFactory, x509NameAsSubject, asymmetricCipherKeyPair.Public, null, asymmetricCipherKeyPair.Private);
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine("CSR generation failed : " + ex);
                throw new Exception("CSR generation failed");
            }
            return pkcs10CertificationRequest;
        }

        [Obsolete("We do not need to generate self signed certificate")]
        internal X509Certificate GenerateTemporarySelfSignedCertificate(AsymmetricCipherKeyPair asymmetricCipherKeyPair, Pkcs10CertificationRequest certificationRequest,BigInteger enrollmentID, int numberOfYear) { 

            X509Name x509NameSubject = certificationRequest.GetCertificationRequestInfo().Subject;
            X509V3CertificateGenerator x509V3CertificateGenerator = new X509V3CertificateGenerator();

            DateTime fromDate = DateTime.Today;
            DateTime toDate = fromDate.AddYears(numberOfYear);

            x509V3CertificateGenerator.SetIssuerDN(x509NameSubject);
            x509V3CertificateGenerator.SetNotBefore(fromDate);
            x509V3CertificateGenerator.SetNotAfter(toDate);
            x509V3CertificateGenerator.SetPublicKey(asymmetricCipherKeyPair.Public);
            x509V3CertificateGenerator.SetSerialNumber(enrollmentID);
            x509V3CertificateGenerator.SetSignatureAlgorithm(Constants.Algorithm.SIGNING);
            x509V3CertificateGenerator.SetSubjectDN(x509NameSubject);

            X509Certificate x509Certificate = x509V3CertificateGenerator.Generate(asymmetricCipherKeyPair.Private);

            return x509Certificate;

        }

        internal X509Certificate GenerateCertificate(String certificateString) {
            X509Certificate x509Certificate = null;
            try
            {
                byte[] bytes = Convert.FromBase64CharArray(certificateString.ToCharArray(), 0, certificateString.Length);
                CmsSignedData cmsSignedData = new CmsSignedData(bytes);

                IX509Store store = cmsSignedData.GetCertificates("Collection");
                ICollection allCertificates = store.GetMatches(null);

                IEnumerator enumerator = allCertificates.GetEnumerator();

                //we only need first certificate. so only one iteration
                enumerator.MoveNext();
                x509Certificate = (X509Certificate)enumerator.Current;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Certificate generation error: " + ex);
                throw new Exception("Certificate generation error");
            }
            return x509Certificate;
        }

    }
}
