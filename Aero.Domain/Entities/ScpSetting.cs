using System;

namespace Aero.Domain.Entities;

public sealed class ScpSetting
{
  public short nMsp1Port { get; set; }
  public int nTransaction { get; set; }
  public short nSio { get; set; }
  public short nMp { get; set; }
  public short nCp { get; set; }
  public short nAcr { get; set; }
  public short nAlvl { get; set; }
  public short nTrgr { get; set; }
  public short nProc { get; set; }
  public short gmtOffset { get; set; }
  public short nTz { get; set; }
  public short nHol { get; set; }
  public short nMpg { get; set; }
  public short nCard { get; set; }
  public short nArea { get; set; }

    public ScpSetting(short nMsp1Port,int nTransaction,short nSio,short nMp,short nCp,short nAcr,short nAlvl,short nTrgr,short nProc,short gmtOffset,short nTz,short nHol,short nMpg,short nCard,short nArea) 
    {
        this.nMsp1Port = nMsp1Port;
        this.nTransaction = nTransaction;
        this.nSio = nSio;
        this.nMp = nMp;
        this.nCp = nCp;
        this.nAcr = nAcr;
        this.nAlvl = nAlvl;
        this.nTrgr = nTrgr;
        this.nProc = nProc;
        this.gmtOffset = gmtOffset;
        this.nTz = nTz;
        this.nHol = nHol;
        this.nMpg = nMpg;
        this.nCard = nCard;
        this.nArea = nArea;
    }
}
