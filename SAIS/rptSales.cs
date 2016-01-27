namespace SAIS
{
    using System;
    using System.ComponentModel;
    using CrystalDecisions.Shared;
    using CrystalDecisions.ReportSource;
    using CrystalDecisions.CrystalReports.Engine;


    public class rptSales : ReportClass
    {
        public rptSales()
        {
        }

        public override string ResourceName
        {
            get
            {
                return "rptSales.rpt";
            }
            set
            {
            }
        }

        public override bool NewGenerator
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        public override string FullResourceName
        {
            get
            {
                return "SAIS.rptSales.rpt";
            }
            set
            {
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public CrystalDecisions.CrystalReports.Engine.Section Section1
        {
            get
            {
                return ReportDefinition.Sections[0];
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public CrystalDecisions.CrystalReports.Engine.Section Section2
        {
            get
            {
                return ReportDefinition.Sections[1];
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public CrystalDecisions.CrystalReports.Engine.Section Section3
        {
            get
            {
                return ReportDefinition.Sections[2];
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public CrystalDecisions.CrystalReports.Engine.Section Section4
        {
            get
            {
                return ReportDefinition.Sections[3];
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public CrystalDecisions.CrystalReports.Engine.Section Section5
        {
            get
            {
                return ReportDefinition.Sections[4];
            }
        }
    }

    [System.Drawing.ToolboxBitmapAttribute(typeof(CrystalDecisions.Shared.ExportOptions), "report.bmp")]
    public class CachedrptSales : Component, ICachedReport
    {
        public CachedrptSales()
        {
        }

        [Browsable(false)]
        [DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public virtual bool IsCacheable
        {
            get
            {
                return true;
            }
            set
            {
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public virtual bool ShareDBLogonInfo
        {
            get
            {
                return false;
            }
            set
            {
            }
        }

        [Browsable(false)]
        [DesignerSerializationVisibilityAttribute(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
        public virtual System.TimeSpan CacheTimeOut
        {
            get
            {
                return CachedReportConstants.DEFAULT_TIMEOUT;
            }
            set
            {
            }
        }

        public virtual CrystalDecisions.CrystalReports.Engine.ReportDocument CreateReport()
        {
            var rpt = new rptSales();
            rpt.Site = Site;
            return rpt;
        }

        public virtual string GetCustomizedCacheKey(RequestContext request)
        {
            var key = (String )null;











            return key;
        }
    }
}
