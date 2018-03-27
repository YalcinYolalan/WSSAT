using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSAPI2Test.Models
{
    public class UserModel2
    {
        public string Id { set; get; }
        public string Name { set; get; }
        public int Age { set; get; }
        public List<string> Cars { set; get; }
    }
}