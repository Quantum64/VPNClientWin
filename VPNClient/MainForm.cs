using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Diagnostics;

namespace VPNClient {
    public partial class MainForm : Form {
        private ChromiumWebBrowser chrome { get; }
        private double factor = 1.0f;
        private VPNConnector connector;

        public MainForm() {
            this.AutoScaleMode = AutoScaleMode.Font;
            this.InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
            this.AutoScaleMode = AutoScaleMode.Font;
            this.chrome = new ChromiumWebBrowser("https://q64.co/endpoint") {
                Dock = DockStyle.Fill,
            };
            chrome.FrameLoadEnd += browserOnFrameLoadEnd;
            chrome.RegisterJsObject("cefobj", new JSCallback());
            int currentDPI = 0;
            using (Graphics g = this.CreateGraphics()) {
                currentDPI = (int)g.DpiX;
            }
            this.factor = currentDPI / 96.0;
            this.Controls.Add(chrome);
            
            Debug.WriteLine("Running CEF at scale factor " + factor + " for DPI");
        }

        private void initBrowser() {

        }

        private void browserOnFrameLoadEnd(object sender, FrameLoadEndEventArgs frameLoadEndEventArgs) {
            ChromiumWebBrowser browser = (ChromiumWebBrowser)sender;
            browser.SetZoomLevel(factor * 2);
        }
    }
}
