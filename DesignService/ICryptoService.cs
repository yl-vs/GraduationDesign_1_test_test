using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace DesignService
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码和配置文件中的接口名“ICryptoService”。
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface ICryptoService
    {
        [OperationContract]
        int Login(string username, string password);
        [OperationContract]
        bool AddUser(string name, string password);

        [OperationContract]
        List<string> showUsers();

        [OperationContract]
        void createFile(string FilePath, string userName, string downloadName);
        [OperationContract]
        string choseFile(string FilePath);

        [OperationContract]
        List<string> DisplayFiles(string userName);

        [OperationContract]
        List<string> FindFile(string file, string userName);

        [OperationContract]
        void DecryptFile(string filename, string username, string downloadPath);

    }
    
}
