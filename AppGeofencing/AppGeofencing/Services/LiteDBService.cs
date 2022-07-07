using AppGeofencing.Interfaces;
using AppGeofencing.Models;
using LiteDB;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppGeofencing.Services
{
    public class LiteDBService
    {
        private static readonly object lockObject = new object();
        private static readonly string _CadenaConexion = DependencyService.Get<ILiteDB>().CadenaConexion();

        public LiteDBService()
        {
            _ = CreaTablas();
        }

        public void Inserta(Ubicacion objeto)
        {
            Monitor.Enter(lockObject);
#if DEBUG
            Console.WriteLine("LiteDB Open conexion: Inserta");
#endif
            try
            {
                using (var db = new LiteDatabase(_CadenaConexion))
                {
                    var col = db.GetCollection<Ubicacion>("ubicacion");

                    if (objeto != null)
                    {
                        _ = col.Insert(objeto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Monitor.Exit(lockObject);
            }
        }

        private Task<bool> CreaTablas()
        {
            return Task.Run(() =>
            {
                Monitor.Enter(lockObject);
                try
                {
#if DEBUG
                    Console.WriteLine("LiteDB Open conexion: CreaTablas");
#endif
                    using (var db = new LiteDatabase(_CadenaConexion))
                    {
                        var col = db.GetCollection<Ubicacion>("ubicacion");
                        col.EnsureIndex(x => x.DbTimeStamp, false);
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return false;
                }
                finally
                {
                    Monitor.Exit(lockObject);
                }
            });
        }
    }
}