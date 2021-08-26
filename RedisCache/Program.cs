using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using System.Configuration;
using System.Net.Sockets;
using System.Threading;
using System.Configuration;

namespace RedisCache
{
    public class ClaseMaestra {
        public ClaseA PropClaseA { get; set; }
        public ClaseB PropClaseB { get; set; }
    }
    public class ClaseA {
        public string Nombre { get; set; }
        public double Precio { get; set; }
    }
    public class ClaseB {
        public string InformaciónClase { get; set; }
        public ClaseC PropClaseC { get; set; }
    }
    public class ClaseC {
        public string DatoBasico { get; set; }
    }
    public class Program
    {
        private static Lazy<ConnectionMultiplexer> lazyConnection = CreateConnection();

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return lazyConnection.Value;
            }
        }
        private static Lazy<ConnectionMultiplexer> CreateConnection()
        {
            return new Lazy<ConnectionMultiplexer>(() =>
            {
                string cacheConnection = ConfigurationManager.AppSettings["CacheConnection"].ToString();
                return ConnectionMultiplexer.Connect(cacheConnection);
            });
        }

        static void Main(string[] args)
        {
            IDatabase redis = Connection.GetDatabase();
            string ping = redis.Execute("PING").ToString();

            Console.WriteLine(ping);

            string resultado = "";

            resultado = redis.StringGet("Usuario1");
            resultado = redis.StringGet("Usuario2");

            Console.WriteLine("Prueba 1:");
            Console.WriteLine(resultado);

            Console.WriteLine("Prueba 2:");
            Console.WriteLine(redis.StringSet("Usuario1", "Mi sesión usuario 1"));
            Console.WriteLine(redis.StringSet("Usuario2", "Mi sesión usuario 2"));

            ClaseMaestra master = new ClaseMaestra();
            master.PropClaseA = new ClaseA();
            master.PropClaseB = new ClaseB();
            master.PropClaseB.PropClaseC = new ClaseC();
            master.PropClaseA.Nombre = "Mi mega clase";
            master.PropClaseA.Precio = 2000;
            master.PropClaseB.InformaciónClase = "Mi mega info";
            master.PropClaseB.PropClaseC.DatoBasico = "Mega dato";


            resultado = redis.StringGet("Usuario1");

            Console.WriteLine("Prueba 3:");
            Console.WriteLine(resultado);

            Console.WriteLine("Prueba 4:");
            Console.WriteLine(redis.KeyDelete("prueba"));

            //Console.WriteLine("Prueba 5:");
            //Console.WriteLine(redis.StringGet("prueba"));

            //Console.WriteLine("Fin de la prueba");

            Console.Read();
        }
    }
}
