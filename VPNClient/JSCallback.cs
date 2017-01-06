using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using CefSharp;
using CefSharp.WinForms;

namespace VPNClient {
    public class JSCallback {
        private ChromiumWebBrowser chrome;
        private VPNConnector connector;

        public JSCallback(ChromiumWebBrowser chrome) {
            this.chrome = chrome;
        }

        public void openBrowser(String url) {
            SystemUtil.openSystemBrowser(url);
        }

        public void setVPNInfo(String ip, String username, String password) {
            connector = new VPNConnector(ip, "Q64 VPN INTERNAL", username, password, Protocol.IKEV2);
            connector.createOrUpdate();
        }

        public void connect() {
            Debug.WriteLine("Downloading cert");
            String cert = SystemUtil.downloadFile("https://q64.co/cert");
            if(!SystemUtil.isInstalled(cert)) {
                Debug.WriteLine("Installing cert now");
                SystemUtil.installCert(cert);
            } else {
                Debug.WriteLine("Cert is already installed");
            }

        }

        public void disconnect() {

        }
    }
}
