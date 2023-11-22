namespace Revalsys.Common
{
    public class RegularExpression
    {

        #region RegExSearchWord
        /// <summary>
        /// Gets the RegExSearchWord.
        /// </summary>
        //=======================================================
        //Version       Author          Date               Remark
        //=======================================================
        //1.0   Md Mujahed Ul Islam     09 Nov 2023       Creation 
        //=======================================================
        public string RegExSearchWord
        {
            get;
            set;
        }
        #endregion

        #region RegExForId
        /// <summary>
        /// Gets the RegExForId.
        /// </summary>
        //=======================================================
        //Version       Author          Date               Remark
        //=======================================================
        //1.0   Md Mujahed Ul Islam     09 Nov 2023       Creation 
        //=======================================================
        public string RegExForId
        {
            get;
            set;
        }
        #endregion

        #region RegExSort
        /// <summary>
        /// Gets the RegExSort.
        /// </summary>
        //=======================================================
        //Version       Author          Date               Remark
        //=======================================================
        //1.0    Md Mujahed Ul Islam    09 Nov 2023       Creation 
        //=======================================================
        public string RegExSort
        {
            get;
            set;
        }
        #endregion


        #region RegExNum
        /// <summary>
        /// Gets the RegExSort.
        /// </summary>
        //=======================================================
        //Version       Author          Date               Remark
        //=======================================================
        //1.0    Md Mujahed Ul Islam    09 Nov 2023       Creation 
        //=======================================================
        public string RegExNum
        {
            get;
            set;
        }
        #endregion

        #region RegExNums 
        public string RegExNums
        {
            get;
            set;
         }
        #endregion

      #region Contructor
       public RegularExpression()
        {

            RegExSearchWord = "^[a-zA-Z]*$";
            RegExForId = "^[^<>]*$";
            RegExNums = "^[0-9]+$";
            RegExNum = "^[0-8]+$";
        }
        #endregion
    }
}