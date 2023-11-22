using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Revalsys.Properties
{
    public class ReasonSearchResponseListDTO
    {
        #region
        private string _ReasonName;
        [DataMember]
        public string ReasonName
        {
            get
            { return _ReasonName; }
            set
            { _ReasonName = value; }
        }
        #endregion

        #region
        private string _ReasonType;
        [DataMember]
        public string ReasonType
        {
            get
            { return _ReasonType; }
            set
            { _ReasonType = value; }
        }
        #endregion
        #region
        private bool _IsPublished;
        [DataMember]
        public bool IsPublished
        {
            get
            { return _IsPublished; }
            set
            { _IsPublished = value; }
        }
        #endregion

        #region
        private string _CreatedBy;
        [DataMember]
        public string CreatedBy
        {
            get
            { return _CreatedBy; }
            set
            { _CreatedBy = value; }
        }
        #endregion

        #region
        private DateTime _DateCreated;
        [DataMember]
        public DateTime DateCreated
        {
            get
            { return _DateCreated; }
            set
            { _DateCreated = value; }
        }
        #endregion

        #region
        private string _UpdatedBy;
        [DataMember]
        public string UpdatedBy
        {
            get
            { return _UpdatedBy; }
            set
            { _UpdatedBy = value; }
        }
        #endregion

        #region
        private DateTime? _LastUpdated;
        [DataMember]
        public DateTime? LastUpdated
        {
            get
            { return _LastUpdated; }
            set
            { _LastUpdated = value; }
        }
        #endregion
    }
}
