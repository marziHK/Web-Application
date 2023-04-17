//بسم الله الرحمن الرحیم
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FSRM.Models.DataModel;
using FSRM.Models.ViewModels;

namespace FSRM.Models
{
    public class EmailData
    {
        #region Feild

        db_FSRMEntities DB = new db_FSRMEntities();

        #endregion

        public void SaveLog_Access(int EmailStatus, int AccessID, string eAdd, string eBody, string HDate, string eTime, DateTime MDate)
        {

            try
            {
                DB = new db_FSRMEntities();

                var q = new Tbl_AccessEmailsLog
                {
                    fld_EmailsStatus = EmailStatus,
                    fld_FK_AccessID = AccessID,
                    fld_EmailAddress = eAdd,
                    fld_EmailBody = eBody,
                    // fld_EmailSentLink = eLink,
                    fld_EmailSentHDate = HDate,
                    fld_EmailSentTime = eTime,
                    fld_EmailSentMDateTime = MDate
                };
                DB.Tbl_AccessEmailsLog.Add(q);

                DB.SaveChanges();
            }
            catch (Exception)
            {

            }


        }


        public void SaveLog_Folder(int EmailStatus, int FolderID, string eAdd, string eBody, string HDate, DateTime MDate)
        {

            try
            {
                DB = new db_FSRMEntities();

                var q = new Tbl_FoldersEmailsLog
                {
                    fld_EmailsStatus = EmailStatus,
                    fld_FK_FoldersID = FolderID,
                    fld_EmailAddress = eAdd,
                    fld_EmailBody = eBody,
                    // fld_EmailSentLink = eLink,
                    fld_EmailSentHDate = HDate,
                    fld_EmailSentMDateTime = MDate
                };
                DB.Tbl_FoldersEmailsLog.Add(q);

                DB.SaveChanges();
            }
            catch (Exception)
            {

            }


        }



    }
}