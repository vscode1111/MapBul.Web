using System.Web;
using System.Web.Optimization;

namespace MapBul.Web.Helpers
{
    public class MyCssRewriteUrlTransform : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }

    }
}