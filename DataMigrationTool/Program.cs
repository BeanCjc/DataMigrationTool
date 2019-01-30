using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoMapper;
using DataMigrationTool.New;
using DataMigrationTool.Old;

namespace DataMigrationTool
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Mapper.Initialize(config =>
            {
                config.CreateMap<CardOld, CardNew>();
            });
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new DataMigration());
        }
    }
}
