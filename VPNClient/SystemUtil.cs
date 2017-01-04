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
        public static object Paths { get; private set; }

        public bool isInstalled(String cert) {
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

        public static void downloadFile(String source) {
            String destination = Path.GetTempFileName();
            File.Delete(destination);
            using (var client = new WebClient()) {
                client.DownloadFile(source, destination);
            }
        }

        public static void createVPNConnection(String name, String ip, String username, String password) {
            RasPhoneBook book = new RasPhoneBook();
            book.Open(RasPhoneBook.GetPhoneBookPath(RasPhoneBookType.User));
            foreach(RasEntry e in book.Entries) {
                if (name.Equals(e.Name)) {
                    Debug.WriteLine("Updating VPN user/password since it already exists");
                    e.UpdateCredentials(new NetworkCredential(username, password));
                    return;
                }                
            }

            RasDevice device = null;
            foreach (RasDevice d in RasDevice.GetDevices()) {
                if (d.Name.Contains("IKEv2")) {
                    device = d;
                }
            }
            RasEntry entry = RasEntry.CreateVpnEntry(name, ip, RasVpnStrategy.IkeV2Only, device);
            entry.Options.RemoteDefaultGateway = true;
            entry.Options.ShowDialingProgress = true;
            //entry.Options.RequireEap = true;

            book.Entries.Add(entry);
            entry.UpdateCredentials(new NetworkCredential(username, password));
        }
    }
}
