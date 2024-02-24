using System.Runtime.Serialization;

namespace Revalsys.Common
{
    public class Appsetting
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
