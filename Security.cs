using System;
using System.Security.Cryptography;
using System.Text;

namespace NewIspNL
{
    /// <summary>
    ///     Summary description for Security
    /// </summary>
    public static class Security{
        //static ISPDataContext DataContext = new ISPDataContext(System.Configuration.ConfigurationManager.AppSettings["ConnectionString"].ToString());
        public static string EncodePassword(string originalPassword){
            //Declarations
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();
            originalBytes = ASCIIEncoding.Default.GetBytes(originalPassword);
            encodedBytes = md5.ComputeHash(originalBytes);
           
            //Convert encoded bytes back to a 'readable' string
            //string pass = BitConverter.ToString(encodedBytes);
            return BitConverter.ToString(encodedBytes);
        }

       

    }


}
