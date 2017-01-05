using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VPNClient {
    public class JSCallback {
        public void openBrowser(String url) {
            SystemUtil.openSystemBrowser(url);
        }
    }
}
