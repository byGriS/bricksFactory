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


    const string host = "http://u0838029bricks.isp.regruhosting.ru/";

    private float lastBrick = -1;
    private float pr = -1;
    private int rifey = -1;

    private int expandedClay = -1;

    private float tenzo = -1;
    private int mercuriyA = -1;
    private int pulsar = -1;
    private int mercuriyAtemp = -1;
    private int pulsarTemp = -1;

    private bool init = false;
    private Thread work;


    private bool debug = false;


    private List<SerialPort> ports = new List<SerialPort>();


    public Form1() {
      InitializeComponent();
      notifyIcon1.Visible = false;
      this.notifyIcon1.MouseDoubleClick += NotifyIcon1_MouseDoubleClick;
      this.Resize += Form1_Resize;
      this.Load += Form1_Load;
      this.FormClosing += Form1_FormClosing;

    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
      try {
        work.Abort();
        ports[0].DataReceived += Port0_DataReceived;
        ports[1].DataReceived += Port1_DataReceived;
        ports[2].DataReceived += Port2_DataReceived;
        ports[0].Close();
        ports[1].Close();
        ports[2].Close();
      } catch { }
    }

    private void Form1_Load(object sender, EventArgs e) {
      //this.WindowState = FormWindowState.Minimized;
      work = new Thread(new ThreadStart(Do));
      work.Start();
    }

    private int timeout = 3;

    private string requestPR = "100302000002C6F2";
    private bool port0Busy = false;
    private string requestTenzo = "0903014C00020568";
    private bool port2Busy = false;
    private int indexDevice = 0;
    private string requestMercuriyA = "01031108000240F5";

    private string requestPulsar = "02049599010E010000009F7C1F02";


    private void Do() {
      try {
        if (debug) {
          ports.Add(new SerialPort("COM8"));
          ports.Add(new SerialPort("COM10"));
          ports.Add(new SerialPort("COM12") { BaudRate = 9600 });
        } else {
          ports.Add(new SerialPort("COM4"));
          ports.Add(new SerialPort("COM5"));
          ports.Add(new SerialPort("COM6") { BaudRate = 9600 });
        }
        /**/
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
          } catch (Exception ex) {
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
          try {
            Work();
          } catch { }
          Thread.Sleep(1000);
        }
      } catch (Exception ex) {
        label17.BeginInvoke(new InvokeDelegate(() => { label17.Text = ex.Message.ToString(); }));
      }
    }

    private void Port0_DataReceived(object sender, SerialDataReceivedEventArgs e) {
      try {
        if (ports[0].BytesToRead >= 9) {
          byte[] response = new byte[ports[0].BytesToRead];
          ports[0].Read(response, 0, ports[0].BytesToRead);
          if (response[0] == 16 && response[1] == 3) {
            pr = BitConverter.ToSingle(new byte[]{response[4],response[3],response[6],response[5]}, 0);
          }
          port0Busy = false;
        }
      } catch { }
    }

    private void Port2_DataReceived(object sender, SerialDataReceivedEventArgs e) {
      try {
          switch (indexDevice) {
          case 0:
            if (ports[2].BytesToRead >= 9) {
              byte[] response = new byte[ports[2].BytesToRead];
              ports[2].Read(response, 0, ports[2].BytesToRead);
              if (response[0] == 9 && response[1] == 3) {
                tenzo = BitConverter.ToSingle(new byte[] { response[4], response[3], response[6], response[5] }, 0);
              }
              port2Busy = false;
            } 
            break;
          case 1:
            if (ports[2].BytesToRead >= 9) {
              byte[] response = new byte[ports[2].BytesToRead];
              ports[2].Read(response, 0, ports[2].BytesToRead);
              if (response[0] == 1 && response[1] == 3) {
                mercuriyA = BitConverter.ToInt32(new byte[] { response[4], response[3], response[6], response[5] }, 0);
                if (mercuriyA < mercuriyAtemp)
                  mercuriyA = mercuriyAtemp;
                else
                  mercuriyAtemp = mercuriyA;
              }
              port2Busy = false;
            } 
            break;
          case 3:
            if (ports[2].BytesToRead >= 14) {
              byte[] response = new byte[ports[2].BytesToRead];
              ports[2].Read(response, 0, ports[2].BytesToRead);
              if (response[0] == 2 && response[1] == 4) {
                pulsar = BitConverter.ToInt32(new byte[] { response[6], response[7], response[8], response[9] }, 0);
                if (pulsar < pulsarTemp)
                  pulsar = pulsarTemp;
                else
                  pulsarTemp = pulsar;
              }
              port2Busy = false;
            } 
            break;
        }
      } catch { }
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

    
    private static string _text = "";
    private void Port1_DataReceived(object sender, SerialDataReceivedEventArgs e) {
      try {
        byte[] input = new byte[1024];
        int size = ports[1].BytesToRead;
        if (size < 1000) {
          ports[1].Read(input, 0, size);
          string text = BitConverter.ToString(input, 0, size);
          _text += text.Replace("-", "");

          if (_text.Contains("3A") && _text.Contains("A3")) {
            int startIndex = _text.IndexOf("3A");
            int endIndex = _text.LastIndexOf("A3");
            if (endIndex > startIndex + 33) {
              string result = _text.Substring(_text.IndexOf("3A"), endIndex - startIndex);

              int[] value = new int[3];
              value[0] = Convert.ToInt32(result[29].ToString(), 16);
              value[1] = Convert.ToInt32(result[31].ToString(), 16);
              value[2] = Convert.ToInt32(result[33].ToString(), 16);
              rifey = 100 * value[0] + 16 * value[1] + value[2];
              CalcRifey(rifey);
              _text = "";
            }
          }

        }
      } catch { }
    }

    private static bool up = false;
    private static int lastValue = 0;
    private static int diff = 0;
    private static List<int> diffArrayAll = new List<int>();
    private static int maxValue = 0;
    private static bool startFindDose1 = false;
    private static int preDose1 = 0;
    private static int dose1 = 0;
    private static int[] arrayMax = new int[3];
    const int valueDiff = 3;//значение ниже которого будем считать просто колебания весов
    const int countDiffArray = 6;//кол-во околонулевых точек
    const int valueMinForMax = 6;

    private void CalcRifey(int curValue) {
      if (lastValue == 0) {
        lastValue = curValue;
        maxValue = curValue;
        return;
      }
      if (curValue < 20)
        return;
      //идем вверх, сохраняем максимальное значение
      if (up && curValue > maxValue) maxValue = curValue;
      //если текущее значение меньше чем 80% от максимума, то начали идти вниз
      if (curValue < maxValue * 0.8) {
        up = false;
      }
      //если идем вниз и опустились ниже 50 кг, то начинаем идти вверх
      if (!up && curValue < 50) {
        up = true;
        maxValue = 0;
        dose1 = 0;
        preDose1 = 0;
        arrayMax = new int[3];
        diffArrayAll = new List<int>();
        startFindDose1 = false;
      }

      if (dose1 == 0 && up) {

        diff = Math.Abs(lastValue - curValue);
        if (diff > 250)
          return;
        //если больше 40 кг начинаем работу, если разница около нуля, то записываем разницу в массив, иначе массив чистим
        if (curValue > 130 && diff < valueDiff)
          diffArrayAll.Add(diff);
        else
          diffArrayAll = new List<int>();

        //если массив больше определенного значения, то закончилась дозировка грубо и начинаем искать дозу
        if (diffArrayAll.Count > countDiffArray)
          startFindDose1 = true;

        if (startFindDose1) {
          if (diff > valueMinForMax) {
            if (preDose1 == 0)
              preDose1 = lastValue;
            if (arrayMax[0] == 0)
              arrayMax[0] = diff;
            else {
              if (arrayMax[1] == 0) {
                if (diff >= arrayMax[0] || diff > 16)
                  arrayMax[1] = diff;
                else {
                  preDose1 = 0;
                  arrayMax = new int[3];
                }
              } else {
                if (diff >= arrayMax[1] || diff > 16) {
                  dose1 = preDose1;
                  expandedClay = dose1;
                  startFindDose1 = false;
                } else {
                  preDose1 = 0;
                  arrayMax = new int[3];
                }
              }
            }
          } else {
            if (diff > valueDiff) {
              preDose1 = 0;
              arrayMax = new int[3];
            }
          }
        }
      }

      lastValue = curValue;

      label12.BeginInvoke(new MethodInvoker(() => { label12.Text = dose1.ToString(); }));
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
      if (init && pr > lastBrick) {
        package.PR = pr;
        lastBrick = pr;
        if (mercuriyA > 1000)
          package.MercuriyA = mercuriyA;
        else
          package.MercuriyA = -1;

        if (pulsar > 1000)
          package.Pulsar = pulsar;
        else
          package.Pulsar = -1;
      } else {
        package.PR = -1;
        package.MercuriyA = -1;
        package.Pulsar = -1;
      }
      if (!init) {
        lastBrick = pr;
      }
      init = true;
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
          package.ExpandedClay = expandedClay;
          expandedClay = -1;
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


      FillTable();
      label16.BeginInvoke(new InvokeDelegate(() => { label16.Text = package.ToString(); }));
      File.AppendAllText(DateTime.Now.Year + ".csv", package.ToString() + "\r\n");
      if (!SendToServer(package.ToString())) {
        File.AppendAllText("loss.csv", package.ToString() + "\r\n");
      }
      Test();
    }

    public delegate void InvokeDelegate();
    private void FillTable() {
      label6.BeginInvoke(new InvokeDelegate(() => {
        label6.Text = pr.ToString();
        label7.Text = rifey.ToString();
        label8.Text = tenzo.ToString();
        label9.Text = mercuriyA.ToString();
        label10.Text = pulsar.ToString();

        label11.Text = lastBrick.ToString();
        label15.Text = watchdog.ToString();
      }));
    }

    private bool SendToServer(string data) {
      if (debug)
        return true;
      try {
        if (SendData(data) == false)
          return false;
        return WorkWithLossBricks();
      } catch (Exception ex) {
        label17.BeginInvoke(new InvokeDelegate(() => { label16.Text = ex.Message.ToString(); }));
        return false;
      }
    }

    private bool WorkWithLossBricks() {
      bool result = true;
      return result;
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
      } catch (Exception ex) {
        label17.BeginInvoke(new InvokeDelegate(() => { label17.Text = ex.Message.ToString(); }));
        return false;
      }
    }

    private int watchdog = 1;
    private void Test() {
      try {
        HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(host + "test.php");
        request.ContentType = "application/json";
        request.Method = "POST";
        request.Timeout = 5000;
        request.UserAgent = "Mozilla/5.0";
        using (StreamWriter writer = new StreamWriter(request.GetRequestStream())) {
          writer.Write("{\"test\":\"" + watchdog.ToString() + "\"}");
        }
        WebResponse response = request.GetResponse();
        using (Stream stream = response.GetResponseStream()) {
          using (StreamReader reader = new StreamReader(stream)) {
            string result = reader.ReadToEnd();
          }
        }
        response.Close();
        watchdog++;
        if (watchdog > 1000000)
          watchdog = 1;
      } catch (Exception ex) {
        label17.BeginInvoke(new InvokeDelegate(() => { label17.Text = ex.Message.ToString(); }));
      }
    }
  }
}