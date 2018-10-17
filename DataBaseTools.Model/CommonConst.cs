using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBaseTools.Model
{
    public class CommonConst
    {
        public static List<SqlTypeModel> SqlTypeList = new List<SqlTypeModel>()
        {
            new SqlTypeModel(){Id = 1,SqlName = "Sql Server"},
            new SqlTypeModel(){Id = 2,SqlName = "Mysql"},
            new SqlTypeModel(){Id = 3,SqlName = "Orcale"},
            new SqlTypeModel(){Id = 4,SqlName = "Redis"}
        };
    }
}
