using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Utility.Components
{
   public class SqlHelper
    {
        private static SqlHelper _single = new SqlHelper();
        public static SqlHelper Single
        {
            get { return _single; }
        }
    }
}
