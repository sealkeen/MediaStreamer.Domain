using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MediaStreamer.Domain
{
    public partial class DBRepository : IPagedDBRepository
    {
        private static DBRepository _instance;
        public static async Task<IDBRepository> GetInstanceAsync(IPagedDMDBContext context)
        {
            if (_instance == null)
                _instance = new DBRepository();
            _instance.DB = context;
            return _instance;
        }

        public static IDBRepository GetInstance(IPagedDMDBContext context)
        {
            if (_instance == null)
                _instance = new DBRepository();
            _instance.DB = context;
            return _instance;
        }

        public IPagedDMDBContext DB { get; set; }

        public void OnStartup()
        {
            
        }

        public void EnsureCreated()
        {
            DB.EnsureCreated();
        }

        public void Update<TDBContext>() where TDBContext : IPagedDMDBContext, new()
        {
            if (DB == null)
            {
                //DB.Dispose();
                //DB = null;
                DB = new TDBContext();
            }
        }

        public string ToMD5(string source)
        {
            var buffer = Encoding.Default.GetBytes(source);

            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(buffer);
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }
            return sBuilder.ToString();
        }

        public Type GetDbContextType()
        {
            if (DB == null)
                return typeof(object);
            return DB.GetType();
        }
    }
}
