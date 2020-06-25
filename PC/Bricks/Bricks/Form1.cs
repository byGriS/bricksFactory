using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;

namespace Bricks {
  public partial class Form1 : Form {

    
    const string host = "http://c98744oh.beget.tech/";

    private float lastBrick = 0;
    private float pr = -1;
    private float rifey = -1;
    private float tenzo = -1;
    private int mercuriyA = -1;
    private int mercuriyR = -1;
    private int pulsar = -1;


    private List<SerialPort> ports = new List<SerialPort>();


    public Form1() {
      InitializeComponent();
      notifyIcon1.Visible = false;
      this.notifyIcon1.MouseDoubleClick += NotifyIcon1_MouseDoubleClick;
      this.Resize += Form1_Resize;
      this.Load += Form1_Load;
      
    }

    private void Form1_Load(object sender, EventArgs e) {
      this.WindowState = FormWindowState.Minimized;
      new Thread(new ThreadStart(Do)).Start();
    }

    private int timeout = 3;

    private string requestPR = "100302000002C6F2";
    private bool port0Busy = false;
    private string requestTenzo = "0903014C00020568";
    private bool port2Busy = false;
    private int indexDevice = 0;
    private string requestMercuriyA = "01031108000240F5";
    private string requestMercuriyR = "0103110C00020134";

    private string requestPulsar = "02049599010E010000009F7C1F02";


    private void Do() {
      try {
        ports.Add(new SerialPort("COM4"));
        ports.Add(new SerialPort("COM5"));
        ports.Add(new SerialPort("COM6") { BaudRate = 9600 }) ;
        RifeyWork();
        while (true) {

          /* READ PR */
          try {
            if (!ports[0].IsOpen) {
              ports[0].Open();
              ports[0].DataReceived += Port0_DataReceived;
            }
            byte[] prRequest = StringToByteArray(requestPR);
            ports[0].Write(prRequest, 0, prRequest.Length);
            port0Busy = true;
            while (port0Busy) {
              Thread.Sleep(500);
              timeout--;
              if (timeout <= 0) {
                timeout = 3;
                break;
              }
            }
          } catch(Exception ex) {
            label17.BeginInvoke(new InvokeDelegate(() => { label17.Text = ex.Message.ToString(); }));
          }

          timeout = 3;
          indexDevice = 0;
          try {
            if (!ports[2].IsOpen) {
              ports[2].Open();
              ports[2].DataReceived += Port2_DataReceived;
            }
            byte[] tenzoRequest = StringToByteArray(requestTenzo);
            ports[2].Write(tenzoRequest, 0, tenzoRequest.Length);
            port2Busy = true;
            while (port2Busy) {
              Thread.Sleep(500);
              timeout--;
              if (timeout <= 0) {
                timeout = 3;
                break;
              }
            }
          } catch (Exception ex) {
            label17.BeginInvoke(new InvokeDelegate(() => { label17.Text = ex.Message.ToString(); }));
          }


          timeout = 3;
          indexDevice = 1;
          try {
            if (!ports[2].IsOpen) {
              ports[2].Open();
              ports[2].DataReceived += Port2_DataReceived;
            }
            byte[] merARequest = StringToByteArray(requestMercuriyA);
            ports[2].Write(merARequest, 0, merARequest.Length);
            port2Busy = true;
            while (port2Busy) {
              Thread.Sleep(500);
              timeout--;
              if (timeout <= 0) {
                timeout = 3;
                break;
              }
            }
          } catch (Exception ex) {
            label17.BeginInvoke(new InvokeDelegate(() => { label17.Text = ex.Message.ToString(); }));
          }


          timeout = 3;
          indexDevice = 2;
          try {
            if (!ports[2].IsOpen) {
              ports[2].Open();
              ports[2].DataReceived += Port2_DataReceived;
            }
            byte[] merARequest = StringToByteArray(requestMercuriyR);
            ports[2].Write(merARequest, 0, merARequest.Length);
            port2Busy = true;
            while (port2Busy) {
              Thread.Sleep(500);
              timeout--;
              if (timeout <= 0) {
                timeout = 3;
                break;
              }
            }
          } catch (Exception ex) {
            label17.BeginInvoke(new InvokeDelegate(() => { label17.Text = ex.Message.ToString(); }));
          }

          timeout = 3;
          indexDevice = 3;
          try {
            if (!ports[2].IsOpen) {
              ports[2].Open();
              ports[2].DataReceived += Port2_DataReceived;
            }
            byte[] pulsarRequest = StringToByteArray(requestPulsar);
            ports[2].Write(pulsarRequest, 0, pulsarRequest.Length);
            port2Busy = true;
            while (port2Busy) {
              Thread.Sleep(500);
              timeout--;
              if (timeout <= 0) {
                timeout = 3;
                break;
              }
            }
          } catch (Exception ex) {
            label17.BeginInvoke(new InvokeDelegate(() => { label17.Text = ex.Message.ToString(); }));
          }

          Work();
          Thread.Sleep(1000);
        }
      } catch (Exception ex) {
        label17.BeginInvoke(new InvokeDelegate(() => { label17.Text = ex.Message.ToString(); }));
      }
    }

    private void Port0_DataReceived(object sender, SerialDataReceivedEventArgs e) {
      if (ports[0].BytesToRead >= 9) {
        byte[] response = new byte[ports[0].BytesToRead];
        ports[0].Read(response, 0, ports[0].BytesToRead);
        pr = BitConverter.ToSingle(new byte[]{
          response[4],response[3],response[6],response[5]
        }, 0);
        port0Busy = false;
      }
    }

    private void Port2_DataReceived(object sender, SerialDataReceivedEventArgs e) {
      switch (indexDevice) {
        case 0:
          if (ports[2].BytesToRead >= 9) {
            byte[] response = new byte[ports[2].BytesToRead];
            ports[2].Read(response, 0, ports[2].BytesToRead);
            tenzo = BitConverter.ToSingle(new byte[]{response[4],response[3],response[6],response[5]}, 0);
            port2Busy = false;
          }
          break;
        case 1:
          if (ports[2].BytesToRead >= 9) {
            byte[] response = new byte[ports[2].BytesToRead];
            ports[2].Read(response, 0, ports[2].BytesToRead);
            mercuriyA = BitConverter.ToInt32(new byte[] { response[4], response[3], response[6], response[5] }, 0);
            port2Busy = false;
          }
          break;
        case 2:
          if (ports[2].BytesToRead >= 9) {
            byte[] response = new byte[ports[2].BytesToRead];
            ports[2].Read(response, 0, ports[2].BytesToRead);
            mercuriyR = BitConverter.ToInt32(new byte[] { response[4], response[3], response[6], response[5] }, 0);
            port2Busy = false;
          }
          break;
        case 3:
          if (ports[2].BytesToRead >= 14) {
            byte[] response = new byte[ports[2].BytesToRead];
            ports[2].Read(response, 0, ports[2].BytesToRead);
            pulsar = BitConverter.ToInt32(new byte[] { response[6], response[7], response[8], response[9] }, 0);
            port2Busy = false;
          }
          break;
      }
    }

    private void RifeyWork() {
      try {
        if (!ports[1].IsOpen) {
          ports[1].Open();
          ports[1].DataReceived += Port1_DataReceived;
        }
      } catch (Exception ex) {
        label17.BeginInvoke(new InvokeDelegate(() => { label16.Text = ex.Message.ToString(); }));
      }
    }

    private void Port1_DataReceived(object sender, SerialDataReceivedEventArgs e) {
      if (ports[1].BytesToRead >= 22) {
        byte[] response = new byte[ports[1].BytesToRead];
        ports[1].Read(response, 0, ports[1].BytesToRead);
        rifey = 100 * response[14]  + 10* response[15] + response[16];
      }
    }

    private void Form1_Resize(object sender, EventArgs e) {
      if (WindowState == FormWindowState.Minimized) {
        this.ShowInTaskbar = false;
        notifyIcon1.Visible = true;
      }
    }

    private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e) {
      notifyIcon1.Visible = false;
      this.ShowInTaskbar = true;
      WindowState = FormWindowState.Normal;
    }

    public byte[] StringToByteArray(string hex) {
      return Enumerable.Range(0, hex.Length)
                       .Where(x => x % 2 == 0)
                       .Select(x => Convert.ToByte(hex.Substring(x, 2), 16))
                       .ToArray();
    }

    private bool rifeySend = false;
    private float rifeyMax = 1;
    private bool tenzoSend = false;
    private float tenzoMax = 1;

    private void Work() {
      Package package = new Package();
      package.dt = DateTime.Now;
      if (pr > lastBrick) {
        package.PR = pr;
        lastBrick = pr;
        if (mercuriyA > 1000)
          package.MercuriyA = mercuriyA;
        else
          package.MercuriyA = -1;

        if (mercuriyR > 1000)
          package.MercuriyR = mercuriyR;
        else
          package.MercuriyR = -1;

        if (pulsar > 1000)
          package.Pulsar = pulsar;
        else
          package.Pulsar = -1;
      } else {
        package.PR = -1;
        package.MercuriyA = -1;
        package.MercuriyR = -1;
        package.Pulsar = -1;
      }
      if (rifey > 10 && rifey < 1000) {
        if (rifey > 200) {
          if (rifey > rifeyMax) {
            rifeyMax = rifey;
          }
        } else {
          if (rifeyMax != 0)
            rifeySend = true;
        }
        if (rifeySend) {
          package.Rifey = rifeyMax;
          rifeyMax = 0;
          rifeySend = false;
        } else {
          package.Rifey = -1;
        }
      } else {
        package.Rifey = -1;
      }

      if (tenzo > 10 && tenzo < 200) {
        if (tenzo > 80) {
          if (tenzo > tenzoMax) {
            tenzoMax = tenzo;
          }
        } else {
          if (tenzoMax != 0)
            tenzoSend = true;
        }
        if (tenzoSend) {
          package.Tenzo = (tenzoMax - 63) * 2;
          tenzoMax = 0;
          tenzoSend = false;
        } else {
          package.Tenzo = -1;
        }
      } else {
        package.Tenzo = -1;
      }


      //package.Rifey = rifey;
      //package.Tenzo = tenzo;
      

      FillTable();
      label16.BeginInvoke(new InvokeDelegate(() => { label16.Text = package.ToString(); }));
      File.AppendAllText(DateTime.Now.Year + ".csv", package.ToString() + "\r\n");
      if (!SendToServer(package.ToString())) {
        File.AppendAllText("loss.csv", package.ToString() + "\r\n");
      }
    }

    public delegate void InvokeDelegate();
    private void FillTable() {
      label6.BeginInvoke(new InvokeDelegate(() => {
        label6.Text = pr.ToString();
        label7.Text = rifey.ToString();
        label8.Text = tenzo.ToString();
        label9.Text = mercuriyA.ToString();
        label10.Text = pulsar.ToString();
      }));
    }

    private bool SendToServer(string data) {
      try {
        if (SendData(data) == false)
          return false;
        return WorkWithLossBricks();
      } catch {
        return false;
      }
    }

    private bool WorkWithLossBricks() {
      bool result = true;
      if (File.Exists("loss.csv")) {
        string[] data = File.ReadAllLines("loss.csv");
        int i = 0;
        for (; i < data.Length; i++) {
          if (SendData(data[i]) == false) {
            result = false;
            break;
          }
        }
        string loss = "";
        for (; i < data.Length; i++) {
          loss += data[i] + "\r\n";
        }
        File.WriteAllText("loss.csv", loss);
      }
      return result;
    }

    private bool SendData(string data) {
      try {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(host + "add.php");
        request.ContentType = "application/json";
        request.Method = "POST";
        request.Timeout = 5000;
        request.UserAgent = "Mozilla/5.0";
        using (StreamWriter writer = new StreamWriter(request.GetRequestStream())) {
          writer.Write(data);
        }
        WebResponse response = request.GetResponse();
        using (Stream stream = response.GetResponseStream()) {
          using (StreamReader reader = new StreamReader(stream)) {
            string result = reader.ReadToEnd();
          }
        }
        response.Close();
        return true;
      } catch (Exception e) {
        return false;
      }
    }

  }
}
