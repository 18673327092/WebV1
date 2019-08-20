using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Model.Sys.Model
{
    public class Tree
    {
        public int id { get; set; }
        public int pId { get; set; }
        public string name { get; set; }
        public bool open { get; set; }

        public string code { get; set; }

        public List<Tree> children { get; set; }
    }
}
