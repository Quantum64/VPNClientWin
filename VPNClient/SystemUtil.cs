using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using DotRas;
using System.Diagnostics;

namespace VPNClient {
    public class SystemUtil {
        public static bool isInstalled(String cert) {
            X509Certificate2 target = new X509Certificate2(X509Certificate2.CreateFromCertFile(cert));
            X509Store store = new X509Store(StoreName.My);
            foreach (X509Certificate2 itr in store.Certificates) {
                if (itr.GetHashCode() == target.GetHashCode()) {
                    return true;
                }
            }
            return false;
        }

        public static void installCert(String cert) {
            X509Store store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            store.Open(OpenFlags.ReadOnly);
            store.Add(new X509Certificate2(X509Certificate2.CreateFromCertFile(cert)));
            store.Close();
        }

        public static String downloadFile(String source) {
            String destination = Path.GetTempFileName();
            File.Delete(destination);
            using (var client = new WebClient()) {
                client.DownloadFile(source, destination);
            }
            return destination;
        }

        public static void openSystemBrowser(String url) {
            System.Diagnostics.Process.Start(url);
        }
    }
}
