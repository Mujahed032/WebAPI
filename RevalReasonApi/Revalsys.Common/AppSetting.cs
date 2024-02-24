using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Revalsys.Common
{
    public class AppSetting
    {
        public string? RegExSearchWord { get; set; } = string.Empty;
        public string? RegExSearchWords { get; set; } = string.Empty;
        public string? RegExForId { get; set; } = string.Empty;
        public string? RegExNums { get; set; } = string.Empty;
        public string? RegExNum { get; set; } = string.Empty;

        public int TimeOut { get; set; } = 0;

        private int _LogTypeId;
        [DataMember]
        public int LogTypeId
        {
            get
            { return _LogTypeId; }
            set
            { _LogTypeId = value; }
        }
    }
}
