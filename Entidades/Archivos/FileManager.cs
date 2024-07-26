using Entidades.Exceptions;
using Entidades.Interfaces;
using Entidades.Modelos;
using System;
using System.Collections.Generic;
using System.Diagnostics.SymbolStore;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
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

            try
            {
                FileManager.ValidarExistenciaDeDirectorio();
            }
            catch(FileManagerException ex)
            {
                Console.WriteLine(ex.Message);
            }
            
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
                    throw new FileManagerException("Error al crear el directorio", ex);
                }
            }
        }
        /// <summary>
        /// método para poder generar archivos de texto. El mismo se podrá usar para agregar información a un archivo ya existente o sobre escribirlo.
        /// </summary>
        /// <param name="data">info guardar</param>
        /// <param name="nombreArchivo"> archivo donde se guarda</param>
        /// <param name="append"> false: sobreescribir  true: anexar </param>
        public static void Guardar(string data, string nombreArchivo, bool append)
        {
            string nuevoPath = Path.Combine(path, nombreArchivo);
            try
            {
                using (StreamWriter sw = new StreamWriter(nuevoPath, append))
                {
                    sw.WriteLine(data);
                }
            }
            catch (Exception)
            {
                Guardar("error al guardar", "logs.txt", true);
            }
        }

        public static bool Serializar<T>(T elemeto, string nombreArchivo) where T : class
        {
            try
            {
                string json = JsonSerializer.Serialize(elemeto);
                Guardar(json, nombreArchivo, true);
                return true;
            }
            catch(Exception)
            {
                Guardar("error al serializar", "logs.txt", true);
                return false;
            }
        }
    }
}
