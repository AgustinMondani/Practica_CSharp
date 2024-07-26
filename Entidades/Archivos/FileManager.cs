using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Entidades.Files
{
    
    public  static class FileManager
    {
        private static string path;

        static FileManager()
        {
            path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            path = Path.Combine(path, Path.GetFileName(path), "Final Laboratorio II Practica\\Recupertario SP1");

            FileManager.ValidarExistenciaDeDirectorio();
        }

        private static void ValidarExistenciaDeDirectorio()
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                }catch (Exception ex)
                {

                }
            }
        }


    }
}
