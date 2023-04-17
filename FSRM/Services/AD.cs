//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Collections;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using System.Security.Principal;
using System.Configuration;
using System.Web.Security;

namespace FSRM.Services
{
    public class AD
    {

        public static string GetOU(string username)
        {

            string result = string.Empty;
            using (var context = new PrincipalContext(ContextType.Domain))
            {
                var principal = UserPrincipal.FindByIdentity(context, username);

                string str = System.Security.Principal.WindowsIdentity.GetCurrent().Name;

                string st = principal.DistinguishedName;

                string[] directoryEntryPath = st.Split(',');
                //Getting the each items of the array and spliting again with the "=" character
                foreach (var splitedPath in directoryEntryPath)
                {
                    string[] eleiments = splitedPath.Split('=');
                    //If the 1st element of the array is "OU" string then get the 2dn element
                    if (eleiments[0].Trim() == "OU")
                    {
                        result = principal.DisplayName + "-" + eleiments[1].Trim() + "-" + principal.EmailAddress;
                        break;
                    }
                }
            }
            return result;

        }


        public static void GetICTUsers()
        {
            string[] rolesArray;
            MembershipUserCollection users;
            string[] usersInRole;


            // Bind roles to ListBox.

            rolesArray = Roles.GetAllRoles();
            
            // Bind users to ListBox.

            users = Membership.GetAllUsers();

            string OU = "ICT Unit";

            usersInRole = Roles.GetUsersInRole(OU);

        }
    }

}
