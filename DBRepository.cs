using StringExtensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

        //private Task<IDBRepository> _loadingTask;
        //public Task<IDBRepository> LoadingTask { get { return _loadingTask; } set { _loadingTask = value; } }

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
        public void PopulateDataBase(Action<string> errorAction = null)
        {
            try
            {
                //AddArtist("August Burns Red", new DateTime(2003, 1, 1), null);//, DB);
                //AddArtist("Being As An Ocean", new DateTime(2011, 1, 1), null);//, DB);
                //AddArtist("Delain", new DateTime(2002, 1, 1), null);//, DB);
                //AddArtist("Fifth Dawn", new DateTime(2014, 1, 1), null);//, DB);
                //AddArtist("Saviour", new DateTime(2009, 1, 1), null);//, DB);

                //AddArtistGenre("August Burns Red", "metalcore");//, DB);
                //AddArtistGenre("Being As An Ocean", "melodic hardcore");//, DB);
                //AddArtistGenre("Being As An Ocean", "post-hardcore");//, DB);
                //AddArtistGenre("Delain", "symphonic metal");//, DB);
                //AddArtistGenre("Fifth Dawn", "alternative rock");//, DB);
                //AddArtistGenre("Saviour", "melodic hardcore");//, DB);

                //AddAlbum("August Burns Red", "Found In Far Away Places", 2015, "Fearless", "Studio");
                //AddAlbum("Being As An Ocean", "Waiting For Morning To Come (Deluxe Edition)", 2018, "SharpTone", "Studio");
                //AddAlbum("Being As An Ocean", "Waiting For Morning To Come", 2017, "SharpTone", "Studio");
                //AddAlbum("Delain", "April Rain", 2009, "Sensory", "Studio");
                //AddAlbum("Fifth Dawn", "Identity", 2018, "Dreambound", "Studio");
                //AddAlbum("Saviour", "Empty Skies", 2018, "Dreambound", "Studio");

                //AddComposition("August Burns Red", "Identity", "Found In Far Away Places", 259);
                //AddComposition("August Burns Red", "Marathon", "Found In Far Away Places", 286);
                //AddComposition("August Burns Red", "Martyr", "Found In Far Away Places", 275);
                //AddComposition("Being As An Ocean", "Alone", "Waiting For Morning To Come (Deluxe Edition)", 264);
                //AddComposition("Being As An Ocean", "Black & Blue", "Waiting For Morning To Come", 256);
                //AddComposition("Being As An Ocean", "Blacktop", "Waiting For Morning To Come", 296);
                //AddComposition("Being As An Ocean", "Dissolve", "Waiting For Morning To Come", 288);
                //AddComposition("Being As An Ocean", "Glow", "Waiting For Morning To Come", 314);
                //AddComposition("Being As An Ocean", "OK", "Waiting For Morning To Come", 256);
                //AddComposition("Being As An Ocean", "Thorns", "Waiting For Morning To Come", 230);
                //AddComposition("Being As An Ocean", "Waiting for Morning to Come", "Waiting For Morning To Come", 295);
                //AddComposition("Delain", "April Rain (Album Version)", "April Rain", 276);
                //AddComposition("Fifth Dawn", "Allure", "Identity", 288);
                //AddComposition("Fifth Dawn", "Element", "Identity", 288);
                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                if (errorAction != null) errorAction.Invoke(ex.Message);
            }
        }
    }
}
