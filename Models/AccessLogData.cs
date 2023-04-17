//بسم الله الرحمن الرحیم

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FSRM.Models.DataModel;
using FSRM.Models.ViewModels;


namespace FSRM.Models
{
    public class AccessLogData
    {
        #region Feild

        db_FSRMEntities DB = new db_FSRMEntities();

        #endregion


        public IList<LogAccessViewModel> AccessLogByID(int AccID)
        {
            try
            {
                var AccessLog = new List<LogAccessViewModel>();

                int LogID = (int)DB.Tbl_AccessLog.OrderByDescending(x => x.fld_AccessLogHDate).Where(x => x.fld_FK_AccessID == AccID).Select(x => x.fld_AccessLogPreviousAccessID).FirstOrDefault();

                var q = (from aL in DB.Tbl_AccessLog
                         join aS in DB.Tbl_AccessStatus
                         on aL.fld_FK_AccessStatus equals aS.fld_AccessStatusID

                         where aL.fld_AccessLogPreviousAccessID == LogID
                         orderby aL.fld_AccessLogMDate
                         select new
                         {
                             aL.fld_FK_AccessID,
                             aL.fld_AccessLogID,
                             aS.fld_AccessStatusDesc,
                             aL.fld_AccessLogHDate,
                             aL.fld_AccessLogMDate
                         }).ToList();

                foreach (var x in q)
                {
                    string Hd = x.fld_AccessLogHDate.ToString();
                    Hd = Hd.Substring(0, 4) + "/" + Hd.Substring(4, 2) + "/" + Hd.Substring(6, 2);

                    AccessLog.Add(new LogAccessViewModel
                    {
                        AccessID = (int)x.fld_FK_AccessID,
                        AccessLogID = x.fld_AccessLogID,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        LogHDate = Hd,
                        LogMDate = x.fld_AccessLogMDate
                    });
                }


                return AccessLog;
            }
            catch (Exception ex)
            {

                throw;
            }

        }


        public IList<LogAccessViewModel> AccessLogByID_ForAdmin(int AccID)
        {
            try
            {
                var AccessLog = new List<LogAccessViewModel>();

                int LogID = (int)DB.Tbl_AccessLog.OrderByDescending(x => x.fld_AccessLogHDate).Where(x => x.fld_FK_AccessID == AccID).Select(x => x.fld_AccessLogPreviousAccessID).FirstOrDefault();

                var q = (from aL in DB.Tbl_AccessLog
                         join aS in DB.Tbl_AccessStatus
                         on aL.fld_FK_AccessStatus equals aS.fld_AccessStatusID

                         where aL.fld_AccessLogPreviousAccessID == LogID
                         orderby aL.fld_AccessLogMDate
                         select new
                         {
                             aL.fld_FK_AccessID,
                             aL.fld_AccessLogID,
                             aL.fld_AccessLogPersonName,
                             aS.fld_AccessStatusDesc,
                             aL.fld_AccessLogHDate,
                             aL.fld_AccessLogMDate
                         }).ToList();

                foreach (var x in q)
                {
                    string Hd = x.fld_AccessLogHDate.ToString();
                    Hd = Hd.Substring(0, 4) + "/" + Hd.Substring(4, 2) + "/" + Hd.Substring(6, 2);

                    AccessLog.Add(new LogAccessViewModel
                    {
                        AccessID = (int)x.fld_FK_AccessID,
                        AccessLogID = x.fld_AccessLogID,
                        AccessStatusDesc = x.fld_AccessStatusDesc,
                        PersonName = x.fld_AccessLogPersonName,
                        LogHDate = Hd,
                        LogMDate = x.fld_AccessLogMDate
                    });
                }


                return AccessLog;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
    }
}