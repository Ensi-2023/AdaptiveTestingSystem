using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdaptiveTestingSystem.Data.NotEntityFramework
{
    public class StaticTSQLScript
    {
        static public string TSQL_StaffViewer { get; } =
                      "select " +
                            "UT.ID_User " +
                            ",(UT.SurnameUser + ' ' + UT.LastnameUser + ' ' + UT.MiddlenameUser) as NameUser " +
                            ",UT.DatebirchUser " +
                            ",UT.GenderUser " +
                            ",UT.RegistrationdatatimeUser " +
                            " from UserTable UT " +                      
                      " inner join Position P ON P.ID_User = UT.ID_User " +
                      " inner join Roly R ON R.ID_Roly = P.ID_Roly" +
                    "   where R.NameRoly = 'Учитель' ";

        static public string TSQL_UserViewer { get; } =
             "select " +
                          "UT.ID_User " +
                          ",(UT.SurnameUser + ' ' + UT.LastnameUser + ' ' + UT.MiddlenameUser) as NameUser " +
                          ",UT.DatebirchUser " +
                          ",UT.GenderUser " +
                          ",UT.RegistrationdatatimeUser " +
                          ",K.ID_Klass " +
                          ",KK.NameKlass from UserTable UT " +
                          "left join Klass_User K on K.ID_User = UT.ID_User " +
                          "left join Klass KK ON KK.ID_Klass = K.ID_Klass";


    }
}
