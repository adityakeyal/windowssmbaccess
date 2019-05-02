using System;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;

namespace WindowsSMBAccess
{ 

 class NetworkConnection : IDisposable
{
    string _networkName;

    public NetworkConnection(string networkName,
        NetworkCredential credentials)
    {
        _networkName = networkName;

        var netResource = new NetResource()
        {
            Scope = ResourceScope.GlobalNetwork,
            ResourceType = ResourceType.Disk,
            DisplayType = ResourceDisplaytype.Share,
            RemoteName = networkName
        };

        var userName = string.IsNullOrEmpty(credentials.Domain)
            ? credentials.UserName
            : string.Format(@"{0}\{1}", credentials.Domain, credentials.UserName);

        var result = WNetAddConnection2(
            netResource,
            credentials.Password,
            userName,
            0);

        if (result != 0)
        {
            throw new System.ComponentModel.Win32Exception(result, "Error connecting to remote share");
        }
    }

    ~NetworkConnection()
    {
        Dispose(false);
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        WNetCancelConnection2(_networkName, 0, true);
    }

    [DllImport("mpr.dll")]
    private static extern int WNetAddConnection2(NetResource netResource,
        string password, string username, int flags);

    [DllImport("mpr.dll")]
    private static extern int WNetCancelConnection2(string name, int flags,
        bool force);





    private static void NewMethod(string NwLocation)
    {


        var fileList = Directory.GetFiles(NwLocation);


        foreach (var item in fileList)
        {
            //if (item.Contains("{ClientDocument}")) { myNetworkPath = item; }

            var dateTime = File.GetLastWriteTime(item);
            DateTime now = DateTime.Now;

            double totalDays = now.Subtract(dateTime).TotalDays;
            //Console.WriteLine(totalDays);
            if (totalDays < 5)
            {

                try
                {
                    string destFileName = item.Substring(NwLocation.Length + 1);
                    File.Delete(destFileName);
                    File.Copy(item, destFileName);
                    Console.WriteLine(destFileName);
                }
                catch (Exception) { }






            }

        }

        var directories = Directory.GetDirectories(NwLocation);

        foreach (var directory in directories)
        {
            try
            {
                String FileName = directory.Substring(NwLocation.Length + 1);
                //Console.WriteLine(FileName);
                if (!FileName.Equals("devel"))
                {
                    NewMethod(directory);
                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e);

            }
        }


    }
}

[StructLayout(LayoutKind.Sequential)]
public class NetResource
{
    public ResourceScope Scope;
    public ResourceType ResourceType;
    public ResourceDisplaytype DisplayType;
    public int Usage;
    public string LocalName;
    public string RemoteName;
    public string Comment;
    public string Provider;
}

public enum ResourceScope : int
{
    Connected = 1,
    GlobalNetwork,
    Remembered,
    Recent,
    Context
};

public enum ResourceType : int
{
    Any = 0,
    Disk = 1,
    Print = 2,
    Reserved = 8,
}

public enum ResourceDisplaytype : int
{
    Generic = 0x0,
    Domain = 0x01,
    Server = 0x02,
    Share = 0x03,
    File = 0x04,
    Group = 0x05,
    Network = 0x06,
    Root = 0x07,
    Shareadmin = 0x08,
    Directory = 0x09,
    Tree = 0x0a,
    Ndscontainer = 0x0b
}

 class SMBAccess
{
    
        

    public void Fetch(Option option)
    {
           NetworkCredential nw = new NetworkCredential();
           nw.UserName = option.User;
           nw.Password = option.Password;

            using (new NetworkConnection(option.Directory, nw))
            {
                NewMethod(option.Directory,option.Day);
            }
    }




        private static void NewMethod(string NwLocation, int Day)
        {


            try
            {

                var fileList = Directory.GetFiles(NwLocation);


                foreach (var item in fileList)
                {
                    //if (item.Contains("{ClientDocument}")) { myNetworkPath = item; }

                    var dateTime = File.GetLastWriteTime(item);
                    DateTime now = DateTime.Now;

                    double totalDays = now.Subtract(dateTime).TotalDays;
                    //Console.WriteLine(totalDays);
                    if (totalDays < Day && item.IndexOf(".pst") < 0 && !item.StartsWith("~"))
                    {

                        try
                        {
                            string destFileName = item.Substring(NwLocation.Length + 1);
                            File.Delete(destFileName);
                            File.Copy(item, destFileName);
                            Console.WriteLine(destFileName);
                        }
                        catch (Exception) { }
                    }
                }

                var directories = Directory.GetDirectories(NwLocation);

                foreach (var directory in directories)
                {
                    try
                    {
                        String FileName = directory.Substring(NwLocation.Length + 1);
                        //Console.WriteLine(FileName);
                        if (!FileName.Equals("devel") )
                        {
                            NewMethod(directory, Day);
                        }

                    }
                    catch (Exception e)
                    {

                        Console.WriteLine(e);

                    }
                }

            }
            catch (Exception) { }
        }


    }









}