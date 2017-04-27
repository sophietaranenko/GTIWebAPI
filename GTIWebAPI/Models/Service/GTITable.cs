using GTIWebAPI.Models.Context;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models.Service
{
    public abstract class GTITable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        protected virtual string TableName { get; }

        public int NewId(IGetNewTableId unitOfWork)
        {
            return unitOfWork.GetNewTableId(TableName);
        }
    }
}
