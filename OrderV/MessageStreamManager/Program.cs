using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MessageStreamManager
{
    static class Program
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(typeof(Program));
        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            logger.Debug("Main thread start");
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            logger.Debug("create Mainform class");
            Application.Run(new Mainform());
        }
    }
}
