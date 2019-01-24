using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Utilities.IO.Pem;
using System;
using System.IO;
using System.Net;

namespace BCC_CA_App_Service.App
{
    class NetworkHandler
    {
        String requestProtocol = "http://";
        String certificateUriLocation = "storage";

        public EnrollementDTO GetDummyEnrollmentDTO(){
            return null;
        }

        public EnrollementDTO GetEnrollmentInfo( String apiEndpoint, long enrollmentID) {
            string html = string.Empty;

            String URL = requestProtocol + Constants.BASE_URL +"/"+ apiEndpoint + "?serialNo=" + enrollmentID + "&actionType=key"  ;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            EnrollementDTO enrollmentDTOForRemoteInit; 
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            using (Stream stream = response.GetResponseStream())
            using (StreamReader reader = new StreamReader(stream)){
                enrollmentDTOForRemoteInit = new EnrollementDTO();
                PopulateEnrollmentDTOFromApi(reader, enrollmentDTOForRemoteInit);
            }
            return enrollmentDTOForRemoteInit;
        }
     
        internal void PopulateEnrollmentDTOFromApi(StreamReader reader, EnrollementDTO enrollmentDTO){
            int index = 0;
            String line = String.Empty;

            Console.WriteLine("API Call For EnrollmentDTO");
            //depricated
            while ((line = reader.ReadLine()) != null){
                Console.WriteLine(line);
                LegacyEnrollementDTOInitialaization(index, line, enrollmentDTO);
                index++;   
            }
        }

        internal void LegacyEnrollementDTOInitialaization(int index, String line, EnrollementDTO enrollmentDTO)
        {
            switch (index) {
                case 0:
                    enrollmentDTO.ID = long.Parse(line);
                    break;
                case 1:
                    enrollmentDTO.firstName = line;
                    break;
                case 2:
                    enrollmentDTO.lastName = line;
                    break;
                case 3:
                    enrollmentDTO.email = line;
                    break;
                case 4:
                    enrollmentDTO.organization = line;
                    break;
                case 5:
                    enrollmentDTO.organizationUnit = line;
                    break;
                case 6:
                    enrollmentDTO.subjectAtlName = line; //email
                    break;
                case 7:
                    enrollmentDTO.area = line;
                    break;
                case 8:
                    enrollmentDTO.address = line;//blank
                    break;
                case 9:
                    enrollmentDTO.country = line;
                    break;
                case 10:
                    enrollmentDTO.state = line;
                    break;
                case 11:
                    enrollmentDTO.postalCode = line;
                    break;
                case 12:
                    enrollmentDTO.mobileNumber = line;//blank
                    break;
                case 13:
                    enrollmentDTO.serialNumber = line;//blank
                    break;
                case 14:
                    enrollmentDTO.passPhase = line;
                    break;
                case 15:
                    enrollmentDTO.identityType = line;
                    break;
                case 16:
                    enrollmentDTO.identityNo = line;
                    break;
                case 17:
                    enrollmentDTO.keyStoreType = Int32.Parse(line);
                    break;
                case 18:
                    enrollmentDTO.smartCardType = Int32.Parse(line);
                    break;
                default:
                    break;
            }

        }

        public String GetCertificateByteArray(long enrollementID){

            String URL = requestProtocol + Constants.BASE_URL + "/" + certificateUriLocation + "/" + enrollementID + Constants.FileExtension.CERTIFICATE;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream);
            String netStream = streamReader.ReadToEnd();
            Console.WriteLine(netStream);
            return netStream;   
              
            
        }

        public void PostCertificationRequest( long enrollmentID, int keyStoreType, Pkcs10CertificationRequest certificationSigningRequest)
        {
            PemObject pemObject = new PemObject("CERTIFICATE REQUEST", certificationSigningRequest.GetEncoded());
            StringWriter str = new StringWriter();
            PemWriter pemWriter = new PemWriter(str);
            pemWriter.WriteObject(pemObject);
            String csr = str.ToString();
            str.Close();
            String URL = requestProtocol + Constants.BASE_URL + "/" + Constants.PartialUrlOfApi.CRS + "?"
                + "csr=" + WebUtility.UrlEncode(csr) + "&serialNo=" + enrollmentID + "&keystoreType="
                + keyStoreType + "&mode=csr"; ;

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(URL);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded; charset=utf-8";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(stream);
            Console.WriteLine(streamReader.ReadToEnd());

            System.Threading.Thread.Sleep(3000);
        }

    }
}
