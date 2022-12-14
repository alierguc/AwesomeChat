using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeChat.Domain.Common
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public bool isActive { get; set; }
    }
}
