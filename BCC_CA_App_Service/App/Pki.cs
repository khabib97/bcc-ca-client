using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.X509;
using Net.Pkcs11Interop.HighLevelAPI;
using System;

namespace BCC_CA_App_Service.App
{
    [Obsolete("We have removed pki from our project")]
    public class Pki
    {
        public AsymmetricCipherKeyPair asymmetricCipherKeyPair { get; set; }
        public Pkcs10CertificationRequest certificationRequest { get; set; }
        [Obsolete("currenly not used")]
        public X509Certificate temporaryX509Certificate{ get; set; }
        public X509Certificate x509certificate{ get; set; }
        public ObjectHandle publicKey { get; set; }
        public ObjectHandle privateKey { get; set; }
        [Obsolete("currenly not used")]
        public ObjectHandle temporaryCertificate { get; set; }
        public ObjectHandle certificate { get; set; }
    }
}
