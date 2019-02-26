using System;



namespace BCC_CA_App_Service.App
{
    public class EnrollementDTO
    {
        public long ID { get; set; }//0
        public String firstName { get; set; }//1
        public String lastName { get; set; }//2
        public String email { get; set; }//3
        public String organization { get; set; }//4
        public String organizationUnit { get; set; }//5
        public String subjectAtlName { get; set; }//6 (email)
        public String area { get; set; }//7
        public String address { get; set; }//8 address : new added
        public String country { get; set; }//9
        public String state { get; set; }//10
        public String postalCode { get; set; } //11
        public String mobileNumber { get; set; }//12 mobile number : new added
        public String serialNumber { get; set; }//13 serial number : new added 
        public String passPhase { get; set; }//14
        public String identityType { get; set; }//15
        public String identityNo { get; set; } //16
                                               //windowsKeyStore = 1 , smartCardStore = 2
        public int keyStoreType { get; set; }//17 
                                             //gemalto = 1 , e-token = 2
        public int smartCardType { get; set; }//18 

        //not related to db
        public int type { get; set; }
        public int status { get; set; }
        public int verificationType { get; set; }
        public long accountID { get; set; }
        public long approvedByID { get; set; }
        public int certificateProfileID { get; set; }
        public long lastModificationTime { get; set; }
        public Boolean isDeleted { get; set; }

        //need for digital signature purpose
        public String getCommonName(){
            return firstName.Trim() + " " + lastName.Trim();
        }

        public String getSerialNumber() {
            return identityType + "" + Utility.SHA1(identityNo);
        }

        public string ToString() {
            return ("ID:" + ID + " First Name: " + firstName + " Last Name: " + lastName + " Email :" + email + " KeyStore: " + keyStoreType);
        }

    }
}
