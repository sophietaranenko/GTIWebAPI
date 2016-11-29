using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GTIWebAPI.Models
{
    public interface IUserable
    {
        int Id { get; }
        string TableName { get; }
    }
}
