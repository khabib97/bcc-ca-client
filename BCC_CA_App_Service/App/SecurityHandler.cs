using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BCC_CA_App_Service.App
{
    class SecurityHandler
    {
        public static void CheckPassPhaseValidity(String userInputPassPhase, String apiFetchPassPhase) {
            if (!apiFetchPassPhase.Equals(Utility.SHA256(userInputPassPhase))) {
                throw new System.Exception("Pass-phase mismatch");
            }
        }
    }
}
