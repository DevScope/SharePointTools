using Microsoft.SharePoint.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DevScope.CascadeLookup.Framework.Loggers
{
    public class SharePointLogger : SPDiagnosticsServiceBase
    {
        // WebServices | WebPartes Logging Categories
        public static string DiagnosticAreaName = "DevScope CascadeLookup Diagnostic Area";
        public static string DiagnosticCategory = "DevScope CascadeLookup Site";

        // Jobs Logging Categories
        public static string DiagnosticJobsCategory = "Jobs";

        // EvenHanders Logging Categories
        public static string DiagnosticEventHandlersCategory = "EventHandlers";

        private static SharePointLogger _Current;
        public static SharePointLogger Current
        {
            get
            {
                if (_Current == null)
                {
                    _Current = new SharePointLogger();
                }

                return _Current;
            }
        }

        private SharePointLogger() :
            base("DevScope CascadeLookup Logging Service", SPFarm.Local)
        {

        }

        protected override IEnumerable<SPDiagnosticsArea> ProvideAreas()
        {
            List<SPDiagnosticsArea> areas = new List<SPDiagnosticsArea>
            {
                new SPDiagnosticsArea(DiagnosticAreaName, new List<SPDiagnosticsCategory>
                {
                    new SPDiagnosticsCategory(DiagnosticCategory, TraceSeverity.Unexpected, EventSeverity.Error),

                    new SPDiagnosticsCategory(DiagnosticJobsCategory, TraceSeverity.Medium, EventSeverity.Information),

                    new SPDiagnosticsCategory(DiagnosticEventHandlersCategory, TraceSeverity.Medium, EventSeverity.Information),
                }),
            };
            return areas;
        }

        public static void LogError(string errorMessage)
        {
            SPDiagnosticsCategory category = SharePointLogger.Current.Areas[DiagnosticAreaName].Categories[DiagnosticCategory];
            SharePointLogger.Current.WriteTrace(0, category, TraceSeverity.Unexpected, errorMessage);
        }

        public static void LogJobError(string errorMessage)
        {
            SPDiagnosticsCategory category = SharePointLogger.Current.Areas[DiagnosticAreaName].Categories[DiagnosticJobsCategory];
            SharePointLogger.Current.WriteTrace(0, category, TraceSeverity.Unexpected, errorMessage);
        }

        public static void LogJobMessage(string errorMessage)
        {
            SPDiagnosticsCategory category = SharePointLogger.Current.Areas[DiagnosticAreaName].Categories[DiagnosticJobsCategory];
            SharePointLogger.Current.WriteTrace(0, category, TraceSeverity.Medium, errorMessage);
        }

        public static void LogEventHandlerError(string errorMessage)
        {
            SPDiagnosticsCategory category = SharePointLogger.Current.Areas[DiagnosticAreaName].Categories[DiagnosticEventHandlersCategory];
            SharePointLogger.Current.WriteTrace(0, category, TraceSeverity.Unexpected, errorMessage);
        }

        public static void LogEventHandlerMessage(string errorMessage)
        {
            SPDiagnosticsCategory category = SharePointLogger.Current.Areas[DiagnosticAreaName].Categories[DiagnosticEventHandlersCategory];
            SharePointLogger.Current.WriteTrace(0, category, TraceSeverity.Medium, errorMessage);
        }

        public static void LogError(Exception e)
        {
            LogError(string.Format("Exception: {0} | StackTrace: {1}", e.Message, e.StackTrace));
        }

        public static void LogJobError(Exception ex)
        {
            LogJobError(string.Format("Exception: {0} | StackTrace: {1}", ex.Message, ex.StackTrace));
        }

        public static void LogEventHandlerError(Exception ex)
        {
            LogEventHandlerError(string.Format("Exception: {0} | StackTrace: {1}", ex.Message, ex.StackTrace));
        }
    }
}
