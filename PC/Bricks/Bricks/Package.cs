using System;

namespace Bricks {
  public class Package {
    public DateTime dt { get; set; }

    public float PR { get; set; } = -1;
    public float Tenzo { get; set; } = -1;
    public float MercuriyA { get; set; } = -1;
    public float MercuriyR { get; set; } = -1;
    public float Pulsar { get; set; } = -1;
    public float Rifey { get; set; } = -1;

    public bool shipped { get; set; }

    public override string ToString() {
      string result = "{\"dt\": \"" + dt.ToString("yyyy-MM-dd HH:mm:ss") + "\"";
      if (PR != -1)
        result += ",\"counter\": \"" + PR + "\"";
      if (Tenzo != -1)
        result += ",\"tenzo\": \"" + Tenzo + "\"";
      if (MercuriyA != -1)
        result += ",\"mercuriyA\": \"" + MercuriyA + "\"";
      if (MercuriyR != -1)
        result += ",\"mercuriyR\": \"" + MercuriyR + "\"";
      if (Pulsar != -1)
        result += ",\"pulsar\": \"" + Pulsar + "\"";
      if (Rifey != -1)
        result += ",\"rifey\": \"" + Rifey + "\"";
      result += "}";
      return result;
    }
  }
}