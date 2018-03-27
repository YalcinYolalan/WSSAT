using System;
using System.Collections.Generic;

namespace WSSAT.DataTypes
{
    public class ReportObject
    {
        public DateTime ScanStartDate { set; get; }
        public DateTime ScanEndDate { set; get; }
        public double TotalRequestCount { set; get; }
        public double Duration {
            get
            {
                return this.ScanEndDate.Subtract(this.ScanStartDate).TotalMinutes;
            }
        }

        public List<WSDescriberForReport> WsDescs { set; get; }
    }
}
